using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MMSC.API
{
    public class PPG
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static HttpClient client = new HttpClient();

        static PPG()
        {
            client = new HttpClient { BaseAddress = new Uri(AppSettings.PPG.URL), Timeout = TimeSpan.FromSeconds(AppSettings.PPG.Timeout) };
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AppSettings.PPG.AuthToken);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.ExpectContinue = false;
        }
        public static  async Task<int> PostNotificationAsync(PPGRequestModel notification)
        {
            try
            {
                var content = new MultipartContent("related");
                content.Headers.Add("Message-Id", notification.MessageID);
                content.Add(new StringContent(notification.PapXml, Encoding.UTF8, "application/xml"));
                
                var bc = new ByteArrayContent(notification.Content);
                bc.Headers.Add("Content-Type", notification.ContentType);
                bc.Headers.Add("X-Wap-Application-Id", "4");
                content.Add(bc);

                var response = await client.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    XElement resultXml = XElement.Parse(result);
                    int statusCode = 0;
                    if (int.TryParse(resultXml.Element("push-response").Element("response-result").Attribute("code").Value, out statusCode))
                    {
                        return statusCode;
                    }
                    else
                    {
                        return -998;
                    }
                }
                else
                {
                    return -997;
                }

                   
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return -999;
            }
           
        }
    }
}