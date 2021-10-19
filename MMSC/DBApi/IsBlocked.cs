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
    public class IsBlocked
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<bool> Execute(string msisdn,int serviceCode)
        {
            try
            {
                string constring = ConfigurationManager.ConnectionStrings["MMS"].ConnectionString;
                bool res = false;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand("IsBlocked", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MSISDN", msisdn);
                        cmd.Parameters.AddWithValue("@ServiceCode", serviceCode);

                        await con.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                               
                                res = int.Parse(reader["IsBlocked"].ToString()) > 0;
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
                return true;
            }
        }
    }
}