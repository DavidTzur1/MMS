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
    public class InsertNotification
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async  Task<int> Execute(MMSNotificationModel notif)
        {
            try
            {

                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertNotification", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MessageType", notif.MessageType);
                        cmd.Parameters.AddWithValue("@TransactionID", notif.TransactionID);
                        cmd.Parameters.AddWithValue("@PushID", notif.PushID);
                        cmd.Parameters.AddWithValue("@MessageID", notif.MessageID);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Domain", notif.Domain);
                        cmd.Parameters.AddWithValue("@To", notif.To);
                        cmd.Parameters.AddWithValue("@Status", notif.Status);
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

        public static async Task<int> Execute(MMSMessageModel message)
        {
            try
            {

                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertNotification", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MessageType", message.MessageType);
                        cmd.Parameters.AddWithValue("@TransactionID", message.TransactionId);
                        cmd.Parameters.AddWithValue("@PushID", message.PushID);
                        cmd.Parameters.AddWithValue("@MessageID", message.MessageID);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@To", message.To.First());
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