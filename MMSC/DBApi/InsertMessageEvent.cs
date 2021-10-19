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
    public class InsertMessageEvent
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<int> Execute(MMSMessageEventModel message)
        {
            try
            {

                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertMessageEvent", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MessageType", message.MessageType);
                        cmd.Parameters.AddWithValue("@MediaType", message.MediaType);
                        cmd.Parameters.AddWithValue("@TransactionID", message.TransactionID);
                        cmd.Parameters.AddWithValue("@PushID", message.PushID);
                        cmd.Parameters.AddWithValue("@MessageID", message.MessageID);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@DomainSender", message.DomainSender);
                        cmd.Parameters.AddWithValue("@From", message.From);
                        cmd.Parameters.AddWithValue("@DomainRcpt", message.DomainRcpt);
                        cmd.Parameters.AddWithValue("@To", message.To);
                        cmd.Parameters.AddWithValue("@Status", message.Status);
                        con.Open();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        con.Close();
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 0;
            }
        }

       
    }
}