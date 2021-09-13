using MMSC.API;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace MMSC.Decoders
{
    public class MM7Decoder
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

       

        public async static Task<MMSMessageModel> Parse(MultipartMemoryStreamProvider message)
        {
            MMSMessageModel res = new MMSMessageModel();
            try
            {
                foreach (var stream in message.Contents)
                {
                    // Getting of content as byte[], picture name and picture type
                    if (stream.IsMimeMultipartContent())
                    {                       
                        var parts = await stream.ReadAsMultipartAsync();
                        string hexData = parts.Contents.Count.ToString("x2");
                        foreach (var part in parts.Contents)
                        {
                            string contentTypeEncode = "";
                            int contentTypeCode;
                            if (Decoder.ContentTypesByName.TryGetValue(part.Headers.ContentType.MediaType, out contentTypeCode))
                            {
                                contentTypeEncode = (contentTypeCode + 0x80).ToString("x2");
                            }
                            else
                            {
                                contentTypeEncode = Tools.ToHexString(part.Headers.ContentType.MediaType) + "00";
                            }
                            foreach (var parameter in part.Headers.ContentType.Parameters)
                            {
                                int parameterEncode;
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

                            foreach (var header in part.Headers)
                            {
                                int headerCode;

                                if (Decoder.Headers.TryGetValue(header.Key, out headerCode))
                                {
                                    switch (headerCode)
                                    {
                                        case 0x40: //Content-ID
                                            {
                                                contentTypeEncode += "c022" + Tools.ToHexString(header.Value.First()) + "00";

                                                break;
                                            }
                                        case 0x0E: //"Content-Location"
                                            {
                                                contentTypeEncode += "8e" + Tools.ToHexString(header.Value.First()) + "00";
                                                break;
                                            }
                                    }
                                }

                            }

                            int headersLen = contentTypeEncode.Length / 2;
                            hexData += IntToUIntString(headersLen);

                            byte[] contentData;
                            IEnumerable<string> values;
                           
                            if (part.Headers.TryGetValues("Content-Transfer-Encoding", out values))
                            {
                                if (values.Contains("base64"))
                                {
                                    string base64String = await part.ReadAsStringAsync();
                                    contentData = System.Convert.FromBase64String(base64String);
                                }
                                else
                                {
                                    contentData = await part.ReadAsByteArrayAsync();
                                }
                            }
                            
                            else
                            {
                                contentData = await part.ReadAsByteArrayAsync();
                            }
                           
                            

                            hexData += IntToUIntString(contentData.Length);
                            hexData += contentTypeEncode + Tools.GetHexString(contentData);
                            
                        }
                        res.Data = hexData;
                        res.MessageSize = hexData.Length/2;
                    }
                    else
                    {
                        var content = await stream.ReadAsByteArrayAsync();                        
                        XElement xml = XElement.Parse(Encoding.UTF8.GetString(content));
                                             
                        XNamespace ab = "http://schemas.xmlsoap.org/soap/envelope/";
                        XNamespace ac = "urn:MM7Submit.Req";

                        res.TransactionId = xml.Element(ab + "Header").Element("MM7Header").Element("Transaction-ID").Value;
                        res.MessageType = xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "Message-Type").Value;
                        res.From = xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "From").Value;
                        res.To.Add(xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "Recipient").Value);
                        res.DeliveryReport = xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "Delivery-Report").Value == "False" ? 0 : 1;
                        res.ReadReport = xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "Read-Reply").Value == "False" ? 0 : 1;
                        res.Priority = xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "Priority").Value;
                        res.Subject = xml.Element(ab + "Body").Element(ac + "MultiMediaSubmit").Element(ac + "Subject").Value;
                        
                    }

                }
                return res;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }
    }
}