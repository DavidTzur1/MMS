using MimeKit;
using MMSC.API;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MMSC.Decoders
{
    public class MM4Decoder
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       

        public static string IntToUIntString(int value)
        {
            string str = (value & 0x7f).ToString("x2");
            value = value >> 7;
            while (value > 0)
            {
                str += ((value & 0x7f) | 0x80).ToString("x2");
                value = value >> 7;
            }

            char[] ca = str.ToCharArray();
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i += 2)
            {
                sb.Insert(0, ca, i, 2);
            }
            return sb.ToString();
            //return null;
        }

        public static MMSMessageModel Parse(MimeMessage message)
        {
            MMSMessageModel res = new MMSMessageModel();
            try
            {
                foreach (Header item in message.Headers)
                {
                    switch (item.Field)
                    {
                        case "From":
                            {
                                //res.From = item.Value;
                                res.From = Decoder.DeviceAddress( item.Value);
                                break;
                            }
                        case "Sender":
                            {
                                int index = item.Value.IndexOf("@");
                                if(index > 0)res.Sender = item.Value.Substring(index+1);
                                break;
                            }
                        case "To":
                            {
                                //res.To.Add(item.Value);
                                 res.To.Add(Decoder.DeviceAddress(item.Value));
                                break;
                            }
                        case "Subject":
                            {
                                res.Subject = item.Value;
                                break;
                            }
                        case "X-Mms-Message-Type":
                            {
                                res.MessageType = item.Value;
                                break;
                            }
                        case "X-Mms-Originator-System":
                            {
                                res.OriginatorSystem = item.Value;
                                break;
                            }
                        case "X-Mms-Transaction-Id":
                        case "X-Mms-Transaction-ID":
                            {
                                res.TransactionId = item.Value;
                                break;
                            }
                        case "X-Mms-Message-ID":
                            {
                                res.MessageID = item.Value;
                                break;
                            }
                        case "X-Mms-Message-Class":
                            {
                                res.MessageClass = item.Value;
                                break;
                            }
                        case "X-Mms-Expiry":
                            {
                                //Expiry = item.Value;
                                break;
                            }
                        case "X-Mms-Priority":
                            {
                                res.Priority = item.Value;
                                break;
                            }
                        case "X-Mms-Delivery-Report":
                            {
                                res.DeliveryReport = (item.Value == "No") ? 0 : 1;
                                break;
                            }
                        case "X-Mms-Read-Reply":
                            {
                                res.ReadReport = (item.Value == "No") ? 0 : 1;
                                break;
                            }


                        default:
                            {
                               //log.Warn($"The parser ignore from key={item.Field} value= {item.Value}");
                                break;
                            }
                    }

                }

                //Data
                string hexData = message.BodyParts.Count().ToString("x2");
                string mediaType = "";
                foreach (var attachment in message.BodyParts)
                {
                    if (attachment.ContentType.MimeType.ToLower().Trim() != "application/smil")
                        mediaType = $"{mediaType};{attachment.ContentType.MimeType.ToLower().Trim()}";

                    string contentTypeEncode = "";
                    int contentTypeCode;
                    if (Decoder.ContentTypesByName.TryGetValue(attachment.ContentType.MimeType, out contentTypeCode))
                    {
                        contentTypeEncode = (contentTypeCode + 0x80).ToString("x2");
                    }
                    else
                    {
                        contentTypeEncode = Tools.ToHexString(attachment.ContentType.MimeType) + "00";
                    }
                    foreach (var parameter in attachment.ContentType.Parameters)
                    {
                        int parameterEncode;
                        //log.Debug($"Name={parameter.Name} Value={parameter.Value}");
                        if (Decoder.WellKnownParameters.TryGetValue(parameter.Name, out parameterEncode))
                        {
                            
                            switch (parameterEncode)
                            {
                                case 0x01: //Charset
                                    {
                                        
                                        int charsetCode;
                                        if (Decoder.CharacterSets.TryGetValue(parameter.Value, out charsetCode))
                                        {
                                            //log.Debug($"found charsetCode for {parameter.Value}");
                                            contentTypeEncode += (parameterEncode + 0x80).ToString("x2") + (charsetCode + 0x80).ToString("x2");
                                        }
                                        else
                                        {
                                            //log.Debug($"Not found charsetCode for {parameter.Value}");
                                        }
                                        break;

                                    }
                                default:
                                    {
                                        contentTypeEncode += (parameterEncode + 0x80).ToString("x2");
                                        contentTypeEncode += Tools.ToHexString(parameter.Value) + "00";
                                        break;
                                    }
                            }

                        }
                    }

                    contentTypeEncode = (contentTypeEncode.Length / 2).ToString("X2") + contentTypeEncode;

                    foreach (var header in attachment.Headers)
                    {
                        int headerCode;

                        if (Decoder.Headers.TryGetValue(header.Field, out headerCode))
                        {
                            switch (headerCode)
                            {
                                case 0x40: //Content-ID
                                    {
                                        contentTypeEncode += "c022" + Tools.ToHexString(header.Value) + "00";
                                        
                                        break;
                                    }
                                case 0x0E: //"Content-Location"
                                    {
                                        contentTypeEncode += "8e" + Tools.ToHexString(header.Value) + "00";
                                        break;
                                    }
                            }
                        }

                    }

                    int headersLen = contentTypeEncode.Length / 2;
                    hexData += IntToUIntString(headersLen);

                    byte[] contentData;
                    using (var memory = new MemoryStream())
                    {
                        if (attachment is MimePart)
                            ((MimePart)attachment).Content.DecodeTo(memory);
                        else
                            ((MessagePart)attachment).Message.WriteTo(memory);
                        contentData = memory.ToArray();
                    }

                    hexData += IntToUIntString(contentData.Length);
                    hexData += contentTypeEncode + Tools.GetHexString(contentData);

                }
                res.MediaType = mediaType.Trim(';');


                res.Data = hexData;
                res.MessageSize = hexData.Length / 2;
                return res;

            }
            catch (Exception ex)
            {
               // res.Status = -999;
               // res.
                log.Error(ex);
                return null;
            }
           
        }

        
    }
}
