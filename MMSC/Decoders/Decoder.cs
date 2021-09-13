﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Decoders
{
    public class Decoder
    {
        public static Dictionary<string, int> Headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)

        {
            ["Accept"] = 0x00,
            ["Accept-Charset"] = 0x01,
            ["Accept-Encoding"] = 0x02,
            ["Accept-Language"] = 0x03,
            ["Accept-Ranges"] = 0x04,
            ["Age"] = 0x05,
            ["Allow"] = 0x06,
            ["Authorization"] = 0x07,
            ["Cache-Control"] = 0x08,
            ["Connection"] = 0x09,
            ["Content-Base"] = 0x0A,
            ["Content-Encoding"] = 0x0B,
            ["Content-Language"] = 0x0C,
            ["Content-Length"] = 0x0D,
            ["Content-Location"] = 0x0E,
            ["Content-MD5"] = 0x0F,
            ["Content-Range"] = 0x10,
            ["Content-Type"] = 0x11,
            ["Date"] = 0x12,
            ["Etag"] = 0x13,
            ["Expires"] = 0x14,
            ["From"] = 0x15,
            ["Host"] = 0x16,
            ["If-Modified-Since"] = 0x17,
            ["If-Match"] = 0x18,
            ["If-None-Match"] = 0x19,
            ["If-Range"] = 0x1A,
            ["If-Unmodified-Since"] = 0x1B,
            ["Location"] = 0x1C,
            ["Last-Modified"] = 0x1D,
            ["Max-Forwards"] = 0x1E,
            ["Pragma"] = 0x1F,
            ["Proxy-Authenticate"] = 0x20,
            ["Proxy-Authorization"] = 0x21,
            ["Public"] = 0x22,
            ["Range"] = 0x23,
            ["Referer"] = 0x24,
            ["Retry-After"] = 0x25,
            ["Server"] = 0x26,
            ["Transfer-Encoding"] = 0x27,
            ["Upgrade"] = 0x28,
            ["User-Agent"] = 0x29,
            ["Vary"] = 0x2A,
            ["Via"] = 0x2B,
            ["Warning"] = 0x2C,
            ["WWW-Authenticate"] = 0x2D,
            ["Content-Disposition"] = 0x2E,
            ["X-Wap-Application-Id"] = 0x2F,
            ["X-Wap-Content-URI"] = 0x30,
            ["XX-Wap-Initiator-URI"] = 0x31,
            ["Accept-Application"] = 0x32,
            ["Bearer-Indication"] = 0x33,
            ["Push-Flag"] = 0x34,
            ["Profile"] = 0x35,
            ["Profile-Diff"] = 0x36,
            ["Profile-Warning"] = 0x37,
            ["Expect"] = 0x38,
            ["TE"] = 0x39,
            ["Trailer"] = 0x3A,
            ["Accept-Charset"] = 0x3B,
            ["Accept-Encoding"] = 0x3C,
            ["Cache-Control"] = 0x3D,
            ["Content-Range"] = 0x3E,
            ["X-Wap-Tod"] = 0x3F,
            ["Content-ID"] = 0x40,
            ["Set-Cookie"] = 0x41,
            ["Cookie"] = 0x42,
            ["Encoding-Version"] = 0x43,
            ["Profile-Warning"] = 0x44,
            ["Content-Disposition"] = 0x45,
            ["X-WAP-Security"] = 0x46,
            ["Cache-Control"] = 0x47,
            ["Expect"] = 0x48,
            ["X-Wap-Loc-Invocation"] = 0x49,
            ["X-Wap-Loc-Delivery"] = 0x4A
        };
        public static Dictionary<string, int> CharacterSets = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)

        {
            ["US-ASCII"] = 3,
            ["ISO_8859-1:1987"] = 4,
            ["ISO_8859-2:1987"] = 5,
            ["ISO_8859-3:1988"] = 6,
            ["ISO_8859-4:1988"] = 7,
            ["ISO_8859-5:1988"] = 8,
            ["ISO_8859-6:1987"] = 9,
            ["ISO_8859-7:1987"] = 10,
            ["ISO_8859-8:1988"] = 11,
            ["ISO_8859-9:1989"] = 12,
            ["ISO-8859-10"] = 13,
            ["ISO_6937-2-add"] = 14,
            ["JIS_X0201"] = 15,
            ["JIS_Encoding"] = 16,
            ["Shift_JIS"] = 17,
            ["Extended_UNIX_Code_Packed_Format_for_Japanese"] = 18,
            ["Extended_UNIX_Code_Fixed_Width_for_Japanese"] = 19,
            ["BS_4730"] = 20,
            ["SEN_850200_C"] = 21,
            ["IT"] = 22,
            ["ES"] = 23,
            ["DIN_66003"] = 24,
            ["NS_4551-1"] = 25,
            ["NF_Z_62-010"] = 26,
            ["ISO-10646-UTF-1"] = 27,
            ["ISO_646.basic:1983"] = 28,
            ["INVARIANT"] = 29,
            ["ISO_646.irv:1983"] = 30,
            ["NATS-SEFI"] = 31,
            ["NATS-SEFI-ADD"] = 32,
            ["NATS-DANO"] = 33,
            ["NATS-DANO-ADD"] = 34,
            ["SEN_850200_B"] = 35,
            ["KS_C_5601-1987"] = 36,
            ["ISO-2022-KR"] = 37,
            ["EUC-KR"] = 38,
            ["ISO-2022-JP"] = 39,
            ["ISO-2022-JP-2"] = 40,
            ["JIS_C6220-1969-jp"] = 41,
            ["JIS_C6220-1969-ro"] = 42,
            ["PT"] = 43,
            ["greek7-old"] = 44,
            ["latin-greek"] = 45,
            ["NF_Z_62-010_(1973)"] = 46,
            ["Latin-greek-1"] = 47,
            ["ISO_5427"] = 48,
            ["JIS_C6226-1978"] = 49,
            ["BS_viewdata"] = 50,
            ["INIS"] = 51,
            ["INIS-8"] = 52,
            ["INIS-cyrillic"] = 53,
            ["ISO_5427:1981"] = 54,
            ["ISO_5428:1980"] = 55,
            ["GB_1988-80"] = 56,
            ["GB_2312-80"] = 57,
            ["NS_4551-2"] = 58,
            ["videotex-suppl"] = 59,
            ["PT2"] = 60,
            ["ES2"] = 61,
            ["MSZ_7795.3"] = 62,
            ["JIS_C6226-1983"] = 63,
            ["greek7"] = 64,
            ["ASMO_449"] = 65,
            ["iso-ir-90"] = 66,
            ["JIS_C6229-1984-a"] = 67,
            ["JIS_C6229-1984-b"] = 68,
            ["JIS_C6229-1984-b-add"] = 69,
            ["JIS_C6229-1984-hand"] = 70,
            ["JIS_C6229-1984-hand-add"] = 71,
            ["JIS_C6229-1984-kana"] = 72,
            ["ISO_2033-1983"] = 73,
            ["ANSI_X3.110-1983"] = 74,
            ["T.61-7bit"] = 75,
            ["T.61-8bit"] = 76,
            ["ECMA-cyrillic"] = 77,
            ["CSA_Z243.4-1985-1"] = 78,
            ["CSA_Z243.4-1985-2"] = 79,
            ["CSA_Z243.4-1985-gr"] = 80,
            ["ISO_8859-6-E"] = 81,
            ["ISO_8859-6-I"] = 82,
            ["T.101-G2"] = 83,
            ["ISO_8859-8-E"] = 84,
            ["ISO_8859-8-I"] = 85,
            ["CSN_369103"] = 86,
            ["JUS_I.B1.002"] = 87,
            ["IEC_P27-1"] = 88,
            ["JUS_I.B1.003-serb"] = 89,
            ["JUS_I.B1.003-mac"] = 90,
            ["greek-ccitt"] = 91,
            ["NC_NC00-10:81"] = 92,
            ["ISO_6937-2-25"] = 93,
            ["GOST_19768-74"] = 94,
            ["ISO_8859-supp"] = 95,
            ["ISO_10367-box"] = 96,
            ["latin-lap"] = 97,
            ["JIS_X0212-1990"] = 98,
            ["DS_2089"] = 99,
            ["us-dk"] = 100,
            ["dk-us"] = 101,
            ["KSC5636"] = 102,
            ["UNICODE-1-1-UTF-7"] = 103,
            ["ISO-2022-CN"] = 104,
            ["ISO-2022-CN-EXT"] = 105,
            ["UTF-8"] = 106,
            ["ISO-8859-13"] = 109,
            ["ISO-8859-14"] = 110,
            ["ISO-8859-15"] = 111,
            ["ISO-8859-16"] = 112,
            ["GBK"] = 113,
            ["GB18030"] = 114,
            ["OSD_EBCDIC_DF04_15"] = 115,
            ["OSD_EBCDIC_DF03_IRV"] = 116,
            ["OSD_EBCDIC_DF04_1"] = 117,
            ["ISO-11548-1"] = 118,
            ["KZ-1048"] = 119,
            ["ISO-10646-UCS-2"] = 1000,
            ["ISO-10646-UCS-4"] = 1001,
            ["ISO-10646-UCS-Basic"] = 1002,
            ["ISO-10646-Unicode-Latin1"] = 1003,
            ["ISO-10646-J-1"] = 1004,
            ["ISO-Unicode-IBM-1261"] = 1005,
            ["ISO-Unicode-IBM-1268"] = 1006,
            ["ISO-Unicode-IBM-1276"] = 1007,
            ["ISO-Unicode-IBM-1264"] = 1008,
            ["ISO-Unicode-IBM-1265"] = 1009,
            ["UNICODE-1-1"] = 1010,
            ["SCSU"] = 1011,
            ["UTF-7"] = 1012,
            ["UTF-16BE"] = 1013,
            ["UTF-16LE"] = 1014,
            ["UTF-16"] = 1015,
            ["CESU-8"] = 1016,
            ["UTF-32"] = 1017,
            ["UTF-32BE"] = 1018,
            ["UTF-32LE"] = 1019,
            ["BOCU-1"] = 1020,
            ["UTF-7-IMAP"] = 1021,
            ["ISO-8859-1-Windows-3.0-Latin-1"] = 2000,
            ["ISO-8859-1-Windows-3.1-Latin-1"] = 2001,
            ["ISO-8859-2-Windows-Latin-2"] = 2002,
            ["ISO-8859-9-Windows-Latin-5"] = 2003,
            ["hp-roman8"] = 2004,
            ["Adobe-Standard-Encoding"] = 2005,
            ["Ventura-US"] = 2006,
            ["Ventura-International"] = 2007,
            ["DEC-MCS"] = 2008,
            ["IBM850"] = 2009,
            ["PC8-Danish-Norwegian"] = 2012,
            ["IBM862"] = 2013,
            ["PC8-Turkish"] = 2014,
            ["IBM-Symbols"] = 2015,
            ["IBM-Thai"] = 2016,
            ["HP-Legal"] = 2017,
            ["HP-Pi-font"] = 2018,
            ["HP-Math8"] = 2019,
            ["Adobe-Symbol-Encoding"] = 2020,
            ["HP-DeskTop"] = 2021,
            ["Ventura-Math"] = 2022,
            ["Microsoft-Publishing"] = 2023,
            ["Windows-31J"] = 2024,
            ["GB2312"] = 2025,
            ["Big5"] = 2026,
            ["macintosh"] = 2027,
            ["IBM037"] = 2028,
            ["IBM038"] = 2029,
            ["IBM273"] = 2030,
            ["IBM274"] = 2031,
            ["IBM275"] = 2032,
            ["IBM277"] = 2033,
            ["IBM278"] = 2034,
            ["IBM280"] = 2035,
            ["IBM281"] = 2036,
            ["IBM284"] = 2037,
            ["IBM285"] = 2038,
            ["IBM290"] = 2039,
            ["IBM297"] = 2040,
            ["IBM420"] = 2041,
            ["IBM423"] = 2042,
            ["IBM424"] = 2043,
            ["IBM437"] = 2011,
            ["IBM500"] = 2044,
            ["IBM851"] = 2045,
            ["IBM852"] = 2010,
            ["IBM855"] = 2046,
            ["IBM857"] = 2047,
            ["IBM860"] = 2048,
            ["IBM861"] = 2049,
            ["IBM863"] = 2050,
            ["IBM864"] = 2051,
            ["IBM865"] = 2052,
            ["IBM868"] = 2053,
            ["IBM869"] = 2054,
            ["IBM870"] = 2055,
            ["IBM871"] = 2056,
            ["IBM880"] = 2057,
            ["IBM891"] = 2058,
            ["IBM903"] = 2059,
            ["IBM904"] = 2060,
            ["IBM905"] = 2061,
            ["IBM918"] = 2062,
            ["IBM1026"] = 2063,
            ["EBCDIC-AT-DE"] = 2064,
            ["EBCDIC-AT-DE-A"] = 2065,
            ["EBCDIC-CA-FR"] = 2066,
            ["EBCDIC-DK-NO"] = 2067,
            ["EBCDIC-DK-NO-A"] = 2068,
            ["EBCDIC-FI-SE"] = 2069,
            ["EBCDIC-FI-SE-A"] = 2070,
            ["EBCDIC-FR"] = 2071,
            ["EBCDIC-IT"] = 2072,
            ["EBCDIC-PT"] = 2073,
            ["EBCDIC-ES"] = 2074,
            ["EBCDIC-ES-A"] = 2075,
            ["EBCDIC-ES-S"] = 2076,
            ["EBCDIC-UK"] = 2077,
            ["EBCDIC-US"] = 2078,
            ["UNKNOWN-8BIT"] = 2079,
            ["MNEMONIC"] = 2080,
            ["MNEM"] = 2081,
            ["VISCII"] = 2082,
            ["VIQR"] = 2083,
            ["KOI8-R"] = 2084,
            ["HZ-GB-2312"] = 2085,
            ["IBM866"] = 2086,
            ["IBM775"] = 2087,
            ["KOI8-U"] = 2088,
            ["IBM00858"] = 2089,
            ["IBM00924"] = 2090,
            ["IBM01140"] = 2091,
            ["IBM01141"] = 2092,
            ["IBM01142"] = 2093,
            ["IBM01143"] = 2094,
            ["IBM01144"] = 2095,
            ["IBM01145"] = 2096,
            ["IBM01146"] = 2097,
            ["IBM01147"] = 2098,
            ["IBM01148"] = 2099,
            ["IBM01149"] = 2100,
            ["Big5-HKSCS"] = 2101,
            ["IBM1047"] = 2102,
            ["PTCP154"] = 2103,
            ["Amiga-1251"] = 2104,
            ["KOI7-switched"] = 2105,
            ["BRF"] = 2106,
            ["TSCII"] = 2107,
            ["CP51932"] = 2108,
            ["windows-874"] = 2109,
            ["windows-1250"] = 2250,
            ["windows-1251"] = 2251,
            ["windows-1252"] = 2252,
            ["windows-1253"] = 2253,
            ["windows-1254"] = 2254,
            ["windows-1255"] = 2255,
            ["windows-1256"] = 2256,
            ["windows-1257"] = 2257,
            ["windows-1258"] = 2258,
            ["TIS-620"] = 2259,
            ["CP50220"] = 2260
        };
        public static Dictionary<string, int> WellKnownParameters = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)

        {
            ["Q"] = 0x00,
            ["Charset"] = 0x01,
            ["Level"] = 0x02,
            ["Type"] = 0x03,
            ["TBD"] = 0x04,
            ["Name"] = 0x05,
            ["Filename"] = 0x06,
            ["Differences"] = 0x07,
            ["Fadding"] = 0x08,
            ["Type"] = 0x09,
            ["Start"] = 0x0A,
            ["Start-info"] = 0x0B,
            ["Comment"] = 0x0C,
            ["Domain"] = 0x0D,
            ["Max-Age"] = 0x0E,
            ["Path"] = 0x0F,
            ["Secure"] = 0x10
        };



        public static Dictionary<string, int> ContentTypesByName = new Dictionary<string, int>()

        {
            [" */*"] = 0x00,
            ["text/*"] = 0x01,
            ["text/html"] = 0x02,
            ["text/plain"] = 0x03,
            ["text/x-hdml"] = 0x04,
            ["text/x-ttml"] = 0x05,
            ["text/x-vCalendar"] = 0x06,
            ["text/x-vCard"] = 0x07,
            ["text/vnd.wap.wml"] = 0x08,
            ["text/vnd.wap.wmlscript"] = 0x09,
            ["text/vnd.wap.wta-event"] = 0x0A,
            ["multipart/mixed"] = 0x0C,
            ["multipart/form-data"] = 0x0D,
            ["multipart/byterantes"] = 0x0E,
            ["multipart/alternative"] = 0x0F,
            ["application/*"] = 0x10,
            ["application/java-vm"] = 0x11,
            ["application/x-www-form-urlencoded"] = 0x12,
            ["application/x-hdmlc"] = 0x13,
            ["application/vnd.wap.wmlc"] = 0x14,
            ["application/vnd.wap.wmlscriptc"] = 0x15,
            ["application/vnd.wap.wta-eventc"] = 0x16,
            ["application/vnd.wap.uaprof"] = 0x17,
            ["application/vnd.wap.wtls-ca-certificate"] = 0x18,
            ["application/vnd.wap.wtls-user-certificate"] = 0x19,
            ["application/x-x509-ca-cert"] = 0x1A,
            ["application/x-x509-user-cert"] = 0x1B,
            ["image/*"] = 0x1C,
            ["image/gif"] = 0x1D,
            ["image/jpeg"] = 0x1E,
            ["image/tiff"] = 0x1F,
            ["image/png"] = 0x20,
            ["image/vnd.wap.wbmp"] = 0x21,
            ["application/vnd.wap.multipart.*"] = 0x22,
            ["application/vnd.wap.multipart.mixed"] = 0x23,
            ["application/vnd.wap.multipart.form-data"] = 0x24,
            ["application/vnd.wap.multipart.byteranges"] = 0x25,
            ["application/vnd.wap.multipart.alternative"] = 0x26,
            ["application/xml"] = 0x27,
            ["text/xml"] = 0x28,
            ["application/vnd.wap.wbxml"] = 0x29,
            ["application/x-x968-cross-cert"] = 0x2A,
            ["application/x-x968-ca-cert"] = 0x2B,
            ["application/x-x968-user-cert"] = 0x2C,
            ["text/vnd.wap.si"] = 0x2D,
            ["application/vnd.wap.sic"] = 0x2E,
            ["text/vnd.wap.sl"] = 0x2F,
            ["application/vnd.wap.slc"] = 0x30,
            ["text/vnd.wap.co"] = 0x31,
            ["application/vnd.wap.coc"] = 0x32,
            ["application/vnd.wap.multipart.related"] = 0x33,
            ["application/vnd.wap.sia"] = 0x34,
            ["text/vnd.wap.connectivity-xml"] = 0x35,
            ["application/vnd.wap.connectivity-wbxml"] = 0x36,
            ["application/pkcs7-mime"] = 0x37,
            ["application/vnd.wap.hashed-certificate"] = 0x38,
            ["application/vnd.wap.cert-response"] = 0x3A,
            ["application/xhtml+xml"] = 0x3B,
            ["application/wml+xml"] = 0x3C,
            ["text/css"] = 0x3D,
            ["application/vnd.wap.mms-message"] = 0x3E,
            ["application/vnd.wap.rollover-certificate"] = 0x3F,
            ["application/vnd.wap.locc+wbxml"] = 0x40,
            ["application/vnd.wap.loc+xml"] = 0x41,
            ["application/vnd.syncml.dm+wbxml"] = 0x42,
            ["application/vnd.syncml.dm+xml"] = 0x43,
            ["application/vnd.syncml.notification"] = 0x44,
            ["application/vnd.wap.xhtml+xml"] = 0x45,
            ["application/vnd.wv.csp.cir"] = 0x46,
            ["application/vnd.oma.dd+xml"] = 0x47,
            ["application/vnd.oma.drm.message"] = 0x48,
            ["application/vnd.oma.drm.content"] = 0x49,
            ["application/vnd.oma.drm.rights+xml"] = 0x4A,
            ["application/vnd.oma.drm.rights+wbxml"] = 0x4B,
            ["application/vnd.wv.csp+xml"] = 0x4C,
            ["application/vnd.wv.csp+wbxml"] = 0x4D,
            ["application/vnd.syncml.ds.notification"] = 0x4E,
            ["audio/*"] = 0x4F,
            ["video/*"] = 0x50,
            ["application/vnd.oma.dd2+xml"] = 0x51,
            ["application/mikey"] = 0x52,
            ["application/vnd.oma.dcd"] = 0x53,
            ["application/vnd.oma.dcdc"] = 0x54,
            ["text/x-vMessage"] = 0x55,
            ["application/vnd.omads-email+wbxml"] = 0x56,
            ["text/x-vBookmark"] = 0x57,
            ["application/vnd.syncml.dm.notification"] = 0x58,
            ["application/octet-stream"] = 0x5A,
            ["application/json"] = 0x5B
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
    }
}