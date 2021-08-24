using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MMSC.API
{
    public class GetSubscriptionOperator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static HttpClient httpClient;
        static GetSubscriptionOperator()
        {
            httpClient = new HttpClient();
        }

        public static async Task<IRResponseModel>  Execute(string subs)
        {
            try
            {
                IRResponseModel response = new IRResponseModel();
                string to = subs.Substring(0, subs.ToUpper().IndexOf("/TYPE") != -1 ? subs.ToUpper().IndexOf("/TYPE") : subs.Length);
                string uri = $"{AppSettings.IR.IP}/informationregistry.asmx/Get_Subs_Operator?PlatformName={AppSettings.IR.PlatformName}&PlatformUser={AppSettings.IR.PlatformUser}&PlatformPwd={AppSettings.IR.PlatformPwd}&MSISDN={to.TrimStart('+')}";
                var xmlStr = await httpClient.GetStringAsync(uri);
                XElement root = XElement.Parse(xmlStr);

                XNamespace ns = "http://orange.co.il/webservices/IR";
                response.Status = root.Element(ns + "Status").Value;
                response.Description = root.Element(ns + "Description").Value;

                foreach (var item in root.Element(ns + "Info").Elements(ns + "Param"))
                {
                    response.Info.Add(item.Attribute("Name").Value, item.Attribute("Value").Value);
                }

                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }
    }
}
