using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.DBApi
{
    public class GetMessageEventInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<MMSMessageEventModel> Execute(string transactionID)
        {
            try
            {
                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;

                // RetrieveConfEncoder res = new RetrieveConfEncoder();
                MMSMessageEventModel res = new MMSMessageEventModel();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("GetMessageEventInfo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TransactionID", transactionID);

                        await con.OpenAsync();


                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                res.MessageID = reader["MessageID"].ToString();
                                res.PushID = reader["PushID"].ToString();                             
                                res.DomainSender = reader["DomainSender"].ToString();
                                res.From = reader["From"].ToString();
                                res.DomainRcpt = reader["DomainRcpt"].ToString();
                                res.To= reader["To"].ToString();
                                
                            }
                        }


                        con.Close();
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