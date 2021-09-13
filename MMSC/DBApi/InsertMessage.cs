using MMSC.API;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MMSC.DBApi
{
    public class InsertMessage
    {
        public static string Execute(MMSMessageModel message)
        {

            string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;
            string messageID = Tools.UniqueID;
            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("InsertMessage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MessageID", messageID);
                    cmd.Parameters.AddWithValue("@MessageType", message.MessageType);
                    cmd.Parameters.AddWithValue("@TransactionID", message.TransactionId);
                    cmd.Parameters.AddWithValue("@Version", message.Version);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Sender", message.Sender);
                    cmd.Parameters.AddWithValue("@From", message.From);
                    cmd.Parameters.AddWithValue("@To", string.Join(";", message.To));
                    cmd.Parameters.AddWithValue("@Subject", message.Subject);
                    cmd.Parameters.AddWithValue("@MessageClass", message.MessageClass);
                    cmd.Parameters.AddWithValue("@Priority", message.Priority);
                    cmd.Parameters.AddWithValue("@SenderVisibility", message.SenderVisibility);
                    cmd.Parameters.AddWithValue("@DeliveryReport", message.DeliveryReport);
                    cmd.Parameters.AddWithValue("@ReadReport", message.ReadReport);
                    cmd.Parameters.AddWithValue("@MessageSize", message.MessageSize);
                    cmd.Parameters.AddWithValue("@ContentType", message.ContentType);
                    cmd.Parameters.AddWithValue("@ContentTypeRaw", message.ContentTypeRaw);
                    cmd.Parameters.AddWithValue("@Data", message.Data);
                    cmd.Parameters.AddWithValue("@ResponseStatus", message.ResponseStatus);
                    cmd.Parameters.AddWithValue("@ResponseText", message.ResponseText);


                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return messageID;
        }
    }
}