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
    public class InsertNotificationResp
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<int> Execute(MMSMessageModel message)
        {
            try
            {

                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertNotificationResp", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TransactionID", message.TransactionId);
                        cmd.Parameters.AddWithValue("@MessageType", message.MessageType);
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