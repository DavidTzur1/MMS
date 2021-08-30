using MMSC.Encoders;
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
    public class GetMessageByPushID
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<MMSMessageModel> Execute(string pushID)
        {
            try
            {
                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;

               // RetrieveConfEncoder res = new RetrieveConfEncoder();
                MMSMessageModel res = new MMSMessageModel();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("GetMessageByPushID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PushID", pushID);

                        await con.OpenAsync();


                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                res.MessageID = reader["MessageID"].ToString();
                                res.TransactionId = reader["TransactionID"].ToString();
                                res.From = reader["From"].ToString();
                                res.To.Add(reader["To"].ToString());
                                res.ContentType = reader["ContentType"].ToString();
                                res.Data = reader["Data"].ToString();
                                // res.ContentType = Tools.FromHexString(reader["ContentType"].ToString());

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