using MMSC.API;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MMSC.Decoders
{
    public class MM1Decoder
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int BCC = 0x81;
        public const int CC = 0x82;
        public const int CONTENT_LOCATION = 0x83;
        public const int CONTENT_TYPE = 0x84;
        public const int DATE = 0x85;
        public const int DELIVERY_REPORT = 0x86;
        public const int DELIVERY_TIME = 0x87;
        public const int EXPIRY = 0x88;
        public const int FROM = 0x89;
        public const int MESSAGE_CLASS = 0x8A;
        public const int MESSAGE_ID = 0x8B;
        public const int MESSAGE_TYPE = 0x8C;
        public const int MMS_VERSION = 0x8D;
        public const int MESSAGE_SIZE = 0x8E;
        public const int PRIORITY = 0x8F;
        public const int READ_REPORT = 0x90;
        public const int REPORT_ALLOWED = 0x91;
        public const int RESPONSE_STATUS = 0x92;
        public const int RESPONSE_TEXT = 0x93;
        public const int SENDER_VISIBILITY = 0x94;
        public const int STATUS = 0x95;
        public const int SUBJECT = 0x96;
        public const int TO = 0x97;
        public const int TRANSACTION_ID = 0x98;

        public const int FROM_ADDRESS_PRESENT_TOKEN = 0x80;
        public const int FROM_INSERT_ADDRESS_TOKEN = 0x81;

        /*--------------------------*
       * Array of header contents *
       *--------------------------*/
        public static Dictionary<int, string> MMS_MESSAGE_TYPES = new Dictionary<int, string>()
            {
                {0x80, "m-send-req"},
                {0x81, "m-send-conf"},
                {0x82, "m-notification-ind"},
                {0x83, "m-notifyresp-ind"},
                {0x84, "m-retrieve-conf"},
                {0x85, "m-acknowledge-ind"},
                {0x86, "m-delivery-ind" }
            };

        private static Dictionary<int, int> MMS_YES_NO = new Dictionary<int, int>()
            {
                {0x80,1},
                {0x81,0}
            };
        private static Dictionary<int, string> MMS_PRIORITY = new Dictionary<int, string>()
            {
                {0x80, "Low" },
                {0x81, "Normal" },
                {0x82, "High" }
            };

        private static Dictionary<int, string> MMS_MESSAGE_CLASS = new Dictionary<int, string>()
            {
                {0x80, "Personnal" },
                {0x81, "Advertisement" },
                {0x82, "Informational" },
                {0x83, "Auto" }
            };

        // character set (mibenum numbers by IANA, ored with 0x80)
        private static Dictionary<int, string> MMS_CHARSET = new Dictionary<int, string>()

            {
                { 0xEA, "utf-8" },
                { 0x83, "ASCII" },
                { 0x84, "iso-8859-1" },
                { 0x85, "iso-8859-2" },
                { 0x86, "iso-8859-3" },
                { 0x87, "iso-8859-4" }
            };

        private static Dictionary<int, string> MMS_CONTENT_TYPES = new Dictionary<int, string>()

        {

                { 0x00, "*/*" },
                { 0x01, "text/*" },
                { 0x02, "text/html" },
                { 0x03, "text/plain"},
                { 0x04, "text/x-hdml" },
                { 0x05, "text/x-ttml" },
                { 0x06, "text/x-vCalendar" },
                { 0x07, "text/x-vCard" },
                { 0x08, "text/vnd.wap.wml" },
                { 0x09, "text/vnd.wap.wmlscript" },
                { 0x0A, "text/vnd.wap.wta-event" },
                { 0x0B, "multipart/*" },
                { 0x0C, "multipart/mixed" },
                { 0x0D, "multipart/form-data" },
                { 0x0E, "multipart/byterantes" },
                { 0x0F, "multipart/alternative" },
                { 0x10, "application/*" },
                { 0x11, "application/java-vm" },
                { 0x12, "application/x-www-form-urlencoded" },
                { 0x13, "application/x-hdmlc" },
                { 0x14, "application/vnd.wap.wmlc" },
                { 0x15, "application/vnd.wap.wmlscriptc" },
                { 0x16, "application/vnd.wap.wta-eventc" },
                { 0x17, "application/vnd.wap.uaprof" },
                { 0x18, "application/vnd.wap.wtls-ca-certificate" },
                { 0x19, "application/vnd.wap.wtls-user-certificate" },
                { 0x1A, "application/x-x509-ca-cert" },
                { 0x1B, "application/x-x509-user-cert" },
                { 0x1C, "image/*" },
                { 0x1D, "image/gif" },
                { 0x1E, "image/jpeg" },
                { 0x1F, "image/tiff" },
                { 0x20, "image/png" },
                { 0x21, "image/vnd.wap.wbmp" },
                { 0x22, "application/vnd.wap.multipart.*" },
                { 0x23, "application/vnd.wap.multipart.mixed" },
                { 0x24, "application/vnd.wap.multipart.form-data" },
                { 0x25, "application/vnd.wap.multipart.byteranges" },
                { 0x26, "application/vnd.wap.multipart.alternative" },
                { 0x27, "application/xml" },
                { 0x28, "text/xml" },
                { 0x29, "application/vnd.wap.wbxml" },
                { 0x2A, "application/x-x968-cross-cert" },
                { 0x2B, "application/x-x968-ca-cert" },
                { 0x2C, "application/x-x968-user-cert" },
                { 0x2D, "text/vnd.wap.si" },
                { 0x2E, "application/vnd.wap.sic" },
                { 0x2F, "text/vnd.wap.sl" },
                { 0x30, "application/vnd.wap.slc" },
                { 0x31, "text/vnd.wap.co" },
                { 0x32, "application/vnd.wap.coc" },
                { 0x33, "application/vnd.wap.multipart.related" },
                { 0x34, "application/vnd.wap.sia" },
                { 0x35, "text/vnd.wap.connectivity-xml" },
                { 0x36, "application/vnd.wap.connectivity-wbxml" },
                { 0x37, "application/pkcs7-mime" },
                { 0x38, "application/vnd.wap.hashed-certificate" },
                { 0x39, "application/vnd.wap.signed-certificate" },
                { 0x3A, "application/vnd.wap.cert-response" },
                { 0x3B, "application/xhtml+xml" },
                { 0x3C, "application/wml+xml" },
                { 0x3D, "text/css" },
                { 0x3E, "application/vnd.wap.mms-message" },
                { 0x3F, "application/vnd.wap.rollover-certificate" },
                { 0x40, "application/vnd.wap.locc+wbxml" },
                { 0x41, "application/vnd.wap.loc+xml" },
                { 0x42, "application/vnd.syncml.dm+wbxml" },
                { 0x43, "application/vnd.syncml.dm+xml" },
                { 0x44, "application/vnd.syncml.notification" },
                { 0x45, "application/vnd.wap.xhtml+xml" },
                { 0x46, "application/vnd.wv.csp.cir" },
                { 0x47, "application/vnd.oma.dd+xml" },
                { 0x48, "application/vnd.oma.drm.message" },
                { 0x49, "application/vnd.oma.drm.content" },
                { 0x4A, "application/vnd.oma.drm.rights+xml" },
                { 0x4B, "application/vnd.oma.drm.rights+wbxml" }
            };

        private byte[] PDU;
        int pos = 0;
        MMSMessageModel message = new MMSMessageModel();

        public MM1Decoder(Byte[] pdu)
        {
            PDU = pdu;
        }

        

        public MMSMessageModel Parse()
        {
            //MMSMessageModel message = new MMSMessageModel();
            pos = 0;
            try
            {
                if (pos >= PDU.Length) return null;
                while (pos < PDU.Length)
                {
                    switch (PDU[pos++])
                    {
                        case MESSAGE_TYPE: // OK
                            string messageType = string.Empty;
                            if (MMS_MESSAGE_TYPES.TryGetValue(PDU[pos++], out messageType))
                            {
                                message.MessageType = messageType;
                            };
                            break;
                        case TRANSACTION_ID: // OK
                            message.TransactionId = parseTextString();
                            break;
                        case MMS_VERSION: // OK
                            int vMaj = (PDU[pos] & (byte)0x70) >> 4;
                            int vMin = (PDU[pos++] & (byte)0x0F);
                            message.Version = vMaj + "." + vMin;
                            break;
                        case TO: // OK
                            message.To.Add(parseEncodedStringValue());
                            break;
                        case SUBJECT: // OK
                            message.Subject = this.parseEncodedStringValue();
                            break;
                        case FROM: // OK
                            message.From = this.parseFromValue();
                            break;
                        case MESSAGE_ID: // OK
                            message.MessageID = this.parseTextString();
                            break;
                        case DATE: // OK
                            message.Date = Tools.UnixTimeStampToDateTime(this.parseLongInteger());
                            break;
                        case DELIVERY_REPORT: // OK
                            message.DeliveryReport = MMS_YES_NO[PDU[pos++]];
                            break;
                        case BCC:
                            message.BCC = parseEncodedStringValue();
                            break;
                        case CC:
                            message.CC = parseEncodedStringValue();
                            break;
                        case CONTENT_LOCATION:
                            message.ContentLocation = parseTextString();
                            break;
                        case DELIVERY_TIME:
                            break;
                        case EXPIRY:
                            parseValueLength();
                            int token = PDU[pos++];                           
                            long timeValue;
                            timeValue = parseLongInteger();
                            if(token == 0x80)
                            {
                                DateTime date = Tools.UnixTimeStampToDateTime(timeValue);
                                timeValue = (long)(date - DateTime.Now).TotalSeconds;
                            }
                            message.Expiry = timeValue;
                            break;
                        case MESSAGE_CLASS:
                            message.MessageClass = parseMessageClassValue();
                            break;
                        case MESSAGE_SIZE:
                            message.MessageSize = this.parseLongInteger();
                            break;
                        case PRIORITY:
                            message.Priority = MMS_PRIORITY[PDU[this.pos++]];
                            break;
                        case READ_REPORT:
                            message.ReadReport = MMS_YES_NO[PDU[this.pos++]];
                            break;
                        case REPORT_ALLOWED:
                            message.ReportAllowed = MMS_YES_NO[PDU[this.pos++]];
                            break;
                        case RESPONSE_STATUS:
                            message.ResponseStatus = PDU[this.pos++];
                            break;
                        case RESPONSE_TEXT:
                            message.ResponseText = this.parseEncodedStringValue();
                            break;
                        case SENDER_VISIBILITY:
                            message.SenderVisibility = MMS_YES_NO[PDU[this.pos++]];
                            break;
                        case STATUS:
                            message.Status = PDU[this.pos++];
                            break;
                        case CONTENT_TYPE: // OK
                            int cntTypepos = pos;
                            if (PDU[pos] <= 31)
                            {
                                int len = this.parseValueLength();
                                int startPos = pos;

                                if (PDU[pos] > 31 && this.PDU[pos] < 128)
                                {
                                    message.ContentType = this.parseTextString();
                                }
                                else
                                {
                                    message.ContentType = MMS_CONTENT_TYPES[this.parseIntegerValue()];
                                }

                                bool noparams = false;
                                while (pos < (startPos + len) & !noparams)
                                {
                                    switch (PDU[pos])
                                    {
                                        case 0x89: //type
                                            pos++;
                                            message.ContentType += $"; type={parseTextString()}";
                                            break;
                                        case 0x8A: //start
                                            pos++;
                                            message.ContentType += $"; start={parseTextString()}";
                                            break;
                                        default:
                                            noparams = true;
                                            break;
                                    }
                                }

                            }
                            else if (PDU[this.pos] < 128)
                            {
                                message.ContentType = this.parseTextString();
                            }
                            else
                            {
                                message.ContentType = MMS_CONTENT_TYPES[this.parseShortInteger()];
                            }

                            byte[] data = new byte[PDU.Length - pos];
                            Array.Copy(PDU, pos, data, 0, PDU.Length - pos);
                            message.Data = Tools.GetHexString(data);
                            parseParts();
                            message.MediaType =GetMediaType();


                            pos = PDU.Length;

                            break;
                        //default:
                        //    if (PDU[this.pos - 1] > 127)
                        //    {
                        //        log.Warn($"Unknown field {PDU[this.pos - 1]} for pos { this.pos}");
                        //    }
                        //    else
                        //    {
                        //        this.pos--;
                                
                        //    }
                        //    break;


                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }

        /*---------------------------------------------------------------------*
       * Function called after header has been parsed. This function fetches *
       * the different parts in the MMS. Returns true until it encounter end *
       * of data. *
       *---------------------------------------------------------------------*/
        public bool parseParts()
        {
            if (this.pos >= PDU.Length) return false;
            int count = this.parseUint();
            for (int i = 0; i < count; i++)
            {
                MMSPartModel part = new MMSPartModel();
               // part.Headerlen  = this.parseUint();
                int headerlen = this.parseUint();
                //part.Datalen = this.parseUint();
                int datalen = this.parseUint();

                int startPos = pos;
                //Content Type                           
                if (PDU[this.pos] <= 31)
                {
                    int len = this.parseValueLength();
                    int pos1 = pos;
                    if (PDU[this.pos] > 31 && PDU[this.pos] < 128)
                    {
                        part.ContentType = this.parseTextString();
                    }
                    else
                    {
                        part.ContentType = MMS_CONTENT_TYPES[this.parseIntegerValue()];
                    }
                 
                }
                else if (PDU[this.pos] < 128)
                {
                   // contentType = this.parseTextString();
                   part.ContentType = parseTextString();
                }
                else
                {
                    part.ContentType = MMS_CONTENT_TYPES[this.parseShortInteger()];
                }

                //Header
                bool flag = true;
                while (pos < (headerlen + startPos) && flag)
                {
                    switch (PDU[pos++])
                    {
                        case 0x85: //name
                        case 0x97:

                            part.Name=parseTextString();
                            break;
                        case 0x81: //Charset

                            if (PDU[pos] > 127)
                            {
                                string value = "";
                                if (MMS_CHARSET.TryGetValue(PDU[pos], out value))
                                {
                                    ++pos;

                                    part.Charset = value;
                                }
                                else
                                {
                                    ++pos;
                                    log.Warn("not support this Charset, ignore it.");
                                }
                            }
                            else
                            {
                                
                                part.Charset = parseTextString();
                            }
                            break;
                        case 0x8E: //Content-Location

                            part.ContentLocation = parseTextString();
                            break;
                        case 0xc0://Content-ID

                            part.ContentId = parseTextString().Trim('"');
                            break;
                        case 0xae://Content-Disposition

                            int len = parseValueLength();
                            pos += len;
                            break;
                        default:
                            flag = false;
                            log.Warn($"Unknown Header :{PDU[pos - 1]}");

                            break;

                    }
                }

                pos = startPos + headerlen;

                //Content
                part.Content = new byte[datalen];
                for (int j = 0; j < datalen; j++)
                {
                    part.Content[j] = PDU[pos++];

                }
                

                message.Parts.Add(part);


               



            }


                return true;

        }


            /*-------------------------------------------------------------------*
           * Parse message-class *
           * message-class-value = Class-identifier | Token-text *
           * Class-idetifier = Personal | Advertisement | Informational | Auto *
           *-------------------------------------------------------------------*/
            private String parseMessageClassValue()
        {
            if (PDU[this.pos] > 127)
                return MMS_MESSAGE_CLASS[this.PDU[this.pos++]];
            else
                return this.parseTextString();
        }

        /*----------------------------------------------------------------*
       * Parse Text-string *
       * text-string = [Quote <Octet 127>] text [End-string <Octet 00>] *
       *----------------------------------------------------------------*/
        private String parseTextString()
        {
            StringBuilder sb = new StringBuilder();
            if (PDU[pos] == (byte)0x7F) pos++;

            char c;
            while (PDU[pos] != 0)
            {
                c = (char)PDU[pos++];
                sb.Append(c);
            }
            pos++;

            return sb.ToString();
        }

        /*------------------------------------------------------------------------*
        * Parse Encoded-string-value *
        * *
        * Encoded-string-value = Text-string | Value-length Char-set Text-string *
        * *
        *------------------------------------------------------------------------*/
        private string parseEncodedStringValue()
        {
            if (PDU[this.pos] <= 31)

            {
                this.parseValueLength();
                int mibnum = PDU[this.pos++];

                string charset;
                if (!MMS_CHARSET.TryGetValue(mibnum, out charset))
                {
                    charset = "";
                }

                string raw = this.parseTextString();
                if (charset.Equals("utf-8"))
                {
                    byte[] bytes = Encoding.Default.GetBytes(raw);
                    raw = Encoding.UTF8.GetString(bytes);
                }

                return raw;
            }


            return this.parseTextString();
        }

        /*--------------------------------------------------------------------------------*
        * Parse Value-length *
        * Value-length = Short-length<Octet 0-30> | Length-quote<Octet 31> Length<Uint> *
        * *
        * A list of content-types of a MMS message can be found here: *
        * http://www.wapforum.org/wina/wsp-content-type.htm *
        *--------------------------------------------------------------------------------*/
        private int parseValueLength()
        {
            if (PDU[this.pos] < 31)
            {
                return PDU[this.pos++];
            }
            else if (PDU[this.pos] == 31)
            {
                this.pos++;
                return this.parseUint();
            }
            else
            {
                log.Debug($"Short-length-octet {this.PDU[this.pos]} > 31 in Value-length at offset {this.pos} !");
                return 0;
            }
        }

        /*--------------------------------------------------------------------------*
      * Parse Long-integer *
      * Long-integer = Short-length<Octet 0-30> Multi-octet-integer<1*30 Octets> *
      *--------------------------------------------------------------------------*/
        private long parseLongInteger()
        {
            int octetcount = PDU[this.pos++];

            if (octetcount > 30)
            {
                log.Error($"Short-length-octet {PDU[this.pos - 1]} > 30 in Value-length at offset { this.pos - 1} !");
                return 0;
            }

            long longint = 0L;
            for (int i = 0; i < octetcount; i++)
            {
                longint = longint << 8;
                longint += PDU[this.pos++];
            }

            return longint;
        }

        /*------------------------------------------------------------------------*
       * Parse Short-integer *
       * Short-integer = OCTET *
       * Integers in range 0-127 shall be encoded as a one octet value with the *
       * most significant bit set to one, and the value in the remaining 7 bits *
       *------------------------------------------------------------------------*/
        private int parseShortInteger()
        {
            return PDU[pos++] & (byte)0x7F;
        }

        /*-------------------------------------------------------------*
	    * Parse Integer-value *
	    * Integer-value = short-integer | long-integer *
	    * *
	    * This function checks the value of the current byte and then *
	    * calls either parseLongInt() or parseShortInt() depending on *
	    * what value the current byte has *
	    *-------------------------------------------------------------*/
        private int parseIntegerValue()
        {
            if (PDU[pos] < 31)
            {
                return (int)parseLongInteger();
            }
            else if (PDU[pos] > 127)
            {
                return parseShortInteger();
            }
            else
            {
                log.Error($"Not a IntegerValue field at pos {pos}");
                pos++;
                return 0;
            }
        }

        /*------------------------------------------------------------------*
       * Parse Unsigned-integer *
       * *
       * The value is stored in the 7 last bits. If the first bit is set, *
       * then the value continues into the next byte. *
       * *
       * http://www.nowsms.com/discus/messages/12/522.html *
       *------------------------------------------------------------------*/
        private int parseUint()
        {
            int Uint = 0;

            while ((PDU[this.pos] & (byte)0x80) != 0)
            {
                Uint = Uint << 7;
                Uint |= PDU[this.pos++] & (byte)0x7F;
            }

            Uint = Uint << 7;
            Uint += PDU[this.pos++] & (byte)0x7F;

            return Uint;
        }

        /*-------------------------------------------------------------------------------------------------*
        * Parse From-value *
        * From-value = Value-length (Address-present-token Encoded-string-value | Insert-address-token ) *
        * *
        * Address-present-token = <Octet 128> *
        * Insert-address-token = <Octet 129> *
        *-------------------------------------------------------------------------------------------------*/
        private string parseFromValue()
        {

            int len = this.parseValueLength();

            if (PDU[this.pos] == FROM_ADDRESS_PRESENT_TOKEN)

            {
                // log.Debug("Address-present-token found");
                this.pos++;
                return this.parseEncodedStringValue();
            }
            else if (PDU[this.pos] == FROM_INSERT_ADDRESS_TOKEN)

            {
                //log.Debug("Insert-address-token found");
                this.pos++;
                return "";
            }
            else

            {
                log.Warn($"No from token found, trying to skip the value field by jumping {len} bytes");
                this.pos += len;
            }

            return "";
        }

        private string GetMediaType()
        {
            string mediaType = "";
            foreach (var item in message.Parts)
            {
                if (item.ContentType.ToLower().Trim() != "application/smil")
                {
                    string contentType = String.Concat(item.ContentType.Where(c => !Char.IsWhiteSpace(c)));
                    mediaType = $"{mediaType};{contentType}";
                }
            }
            return mediaType.Trim(';');
        }
    }
}
