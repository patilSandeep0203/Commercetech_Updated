using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class UserRolesDL
    {
        private static string ConnStringTokenAdmin = ConfigurationManager.AppSettings["eSecurityConnectStringAdmin"].ToString();
          
        //CALLED BY UserRolesBL.GetRolesByAffiliateID
        public DataSet GetRolesByAffiliateID(int iUserID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringTokenAdmin);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetRolesByUserID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@iUserID", iUserID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetRolesByAffiliateID

        //This function returns roles description for each iAppId
        //CALLED BY UserRolesBL.GetRolesDescription
        public DataSet GetRolesDescription()
        {
            SqlConnection Conn = new SqlConnection(ConnStringTokenAdmin);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetAppRolesDesc", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetRolesDescription

        //This function updates user roles - CALLED BY MANAGEUSERS.ASPX.CS
        public int UpdateUserRoles(string iUserID, string iAppId, string Access)
        {
            SqlConnection Conn = new SqlConnection(ConnStringTokenAdmin);
            try
            {
                SqlCommand cmdRoles = new SqlCommand("sp_UpdateUserRoles", Conn);
                cmdRoles.CommandType = CommandType.StoredProcedure;
                cmdRoles.Parameters.Add(new SqlParameter("@iUserID", iUserID));
                cmdRoles.Parameters.Add(new SqlParameter("@iAppId", iAppId));
                cmdRoles.Parameters.Add(new SqlParameter("@Access", Access));

                cmdRoles.Connection.Open();
                int iRetVal = cmdRoles.ExecuteNonQuery();
                //Conn.Close();
                //Conn.Dispose();
                return iRetVal;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function UpdateUserRoles

        //This function gets the user list based on specified Access. CALLED BY UserBL.GetUsersByAccess
        public DataSet GetUsersByAccess(string Role, string iAppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringTokenAdmin);
            try
            {
                SqlCommand cmd= new SqlCommand("sp_GetUsersByRole", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@SRole", Role));
                cmd.Parameters.Add(new SqlParameter("@iAppId", iAppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "VW_ManageUsers");
                return ds;
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
        }//end ReturnUser


    }//end class UserRolesDL
}
