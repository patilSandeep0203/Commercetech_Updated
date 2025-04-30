using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class AdministrativeDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        //CALLED BY AdministrativeBL.GetAdministrativeInfo
        public int UpdateAdministrativeInfo(string UserName)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateAdministrativeInfo", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pUserName = cmd.Parameters.Add("@UserName", SqlDbType.VarChar);

                pUserName.Value = UserName;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return iRetVal;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function UpdateAdministrativeInfo


    }//end class AdministrativeDL
}
