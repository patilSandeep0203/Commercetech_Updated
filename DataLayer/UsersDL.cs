using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class UsersDL
    {
        private static string ConnStringToken = ConfigurationManager.AppSettings["eSecurityConnectString"].ToString();


        //Checks the login ID against the password and returns a dataset if password 
        //matchs found
        public DataSet CheckLogin(string LoginID, string Password)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ChkLogin", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter psUserID = cmd.Parameters.Add("@sLoginID", SqlDbType.VarChar);
                SqlParameter pPassword = cmd.Parameters.Add("@sLoginPassword", SqlDbType.VarChar);

                psUserID.Value = LoginID;
                pPassword.Value = Password;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
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
        }

        //CALLED BY AffiliatesBL.ResetAffiliatePassword
        public int UpdatePassword(int iUserID, string Password)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateUserPassword", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pPassword = cmd.Parameters.Add("@Password", SqlDbType.VarChar);

                pAffiliateID.Value = iUserID;
                pPassword.Value = Password;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
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
        }//end ResetPassword


        public DataSet GetAppsByLoginID(string LoginID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetAppList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter psUserID = cmd.Parameters.Add("@sLoginID", SqlDbType.VarChar);

                psUserID.Value = LoginID;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
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
        }
        public string CreateLoginToken(string appName,
          string loginID, int userID, int appID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            SqlParameter param;
            SqlCommand cmd = new SqlCommand("sp_InsertToken", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            string token;

            // Generate a new Token
            token = System.Guid.NewGuid().ToString();
            try
            {

                param = new SqlParameter("@sToken", SqlDbType.Char);
                param.Value = token;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@sAppName", SqlDbType.Char);
                param.Value = appName;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@sLoginID", SqlDbType.Char);
                param.Value = loginID;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@iUserID", SqlDbType.SmallInt);
                param.Value = userID;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@iAppID", SqlDbType.Int);
                param.Value = appID;
                cmd.Parameters.Add(param);

                param = new SqlParameter("@dtCreated", SqlDbType.DateTime);
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
            }

            return token;
        }//end function CreateLoginToken

        //CALLED BY AffiliatesBL.CreateOnlineAppToken
        public string CreateOnlineAppToken(string sAppName, string sLoginID, int iOnlineAppID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ChkInsertOnlineAppToken", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter psAppName = cmd.Parameters.Add("@sAppName", SqlDbType.VarChar);
                //SqlParameter piUserID = cmd.Parameters.Add("@iUserID", SqlDbType.VarChar);
                SqlParameter psLoginID = cmd.Parameters.Add("@sLoginID", SqlDbType.VarChar);
                SqlParameter psToken = cmd.Parameters.Add("@sToken", SqlDbType.VarChar);
                SqlParameter piOnlineAppID = cmd.Parameters.Add("@iOnlineAppID", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.VarChar, 512);
                pRetVal.Direction = ParameterDirection.Output;
                string sToken = System.Guid.NewGuid().ToString();
                psAppName.Value = sAppName;
                piOnlineAppID.Value = iOnlineAppID;
                //piUserID.Value = iUserID;
                psLoginID.Value = sLoginID;
                psToken.Value = sToken;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                string strRetVal = Convert.ToString(pRetVal.Value);
                Conn.Close();
                Conn.Dispose();
                return strRetVal;
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
        }//end function CreateOnlineAppToken

        //CALLED BY AffiliatesBL.VerifyToken
        public DataSet VerifyLoginToken(string Token)
        {
            DataSet ds = new DataSet();
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            DataRow dr;
            SqlDataAdapter da;

            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckToken", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@sToken", SqlDbType.Char));
                cmd.Parameters["@sToken"].Value = Token;

                da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                //Delete Token after verifying it
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];

                    DeleteToken(Convert.ToInt32(dr["iAppTokenID"]));
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }//end function VerifyLoginToken

        public int GetAffiliateIDbyLoginID(string PortalUserID)
        {
            //string PortalUserID="";
            int AffiliateID;
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAffiliateIDbyLoginID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.Int);
                SqlParameter pPortalUserID = cmd.Parameters.Add("@PortalUserID", SqlDbType.VarChar);
                pPortalUserID.Size = 64;
                pPortalUserID.Value = PortalUserID;
                pAffiliateID.Direction = ParameterDirection.Output;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                AffiliateID = (int)cmd.Parameters["@AffiliateID"].Value;
                return AffiliateID;
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
        }//end InsertLog

        public void DeleteToken(int AppTokenID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = ("sp_DeleteToken");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@iAppTokenID", SqlDbType.Int));
                cmd.Parameters["@iAppTokenID"].Value = AppTokenID;
                cmd.Connection = new SqlConnection(ConnStringToken);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
            }
        }//end function DeleteToken


        //Checks whether the User ID exists
        public bool CheckIDExists(int iUserID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            int retVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CheckIDExists", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter piUserID = cmd.Parameters.Add("@iUserID", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.VarChar);
                pRetVal.Direction = ParameterDirection.ReturnValue;
                piUserID.Value = iUserID;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                retVal = Convert.ToInt32(pRetVal.Value);

                return Convert.ToBoolean(retVal);
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
        }//end GetAffiliateLogin


        public DataSet GetRoles(string UserName)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetRoles", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter psLoginID = cmd.Parameters.Add("@sLoginID", SqlDbType.VarChar);
                psLoginID.Value = UserName;

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
        }//end function GetRoles

        public DataSet GetRoleByApp(string UserName, int iAppID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetRoleByApp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter psLoginID = cmd.Parameters.Add("@sLoginID", SqlDbType.VarChar);
                SqlParameter piAppID = cmd.Parameters.Add("@iAppID", SqlDbType.VarChar);
                psLoginID.Value = UserName;
                piAppID.Value = iAppID;

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
        }//end function GetRolesByApp

        //This function gets the Login info by AffiliateID
        public DataSet GetLoginInfo(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetLoginInfo", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@iUserID", SqlDbType.VarChar);
                pAffiliateID.Value = AffiliateID;

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
        }//end GetUserInfo

        public DataSet GetLoginPassword(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetLoginPassword", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@iUserID", SqlDbType.VarChar);
                pAffiliateID.Value = AffiliateID;

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
        }//end GetUserInfo

        //This function gets the Login info By LoginID
        public DataSet GetLoginInfoByLoginID(string LoginID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetLoginInfoByLoginID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pLoginID = cmd.Parameters.Add("@LoginID", SqlDbType.VarChar);
                pLoginID.Value = LoginID;

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
        }//end GetUserInfo

        //This function gets the user info for editing. Called By UsersBL.GetAffiliate
        public DataSet GetUserInfo(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            try
            {

                SqlCommand cmd = new SqlCommand("sp_GetEditInfo", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "VW_EditInfo");
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
        }//end GetUserInfo

    }//end class ManageUsersDL
}
