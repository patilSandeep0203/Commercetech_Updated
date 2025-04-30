using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class OnlineAppDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnStringToken = ConfigurationManager.AppSettings["eSecurityConnectString"].ToString();

        public DataSet CheckLoginNameExists(string LoginName)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckLoginNameExists", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@LoginName", LoginName));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                Conn.Close();
                Conn.Dispose();
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
        }//end function CheckIfLoginNameExists

        public int InsertUpdateLoginName(string LoginName, int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateLoginName", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pLoginName = cmd.Parameters.Add("@LoginName", SqlDbType.VarChar);                
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);

                pLoginName.Value = LoginName;
                pAppId.Value = AppId;

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
        }//end function insertupdatelogin

        public bool InsertLoginNamePassword(string LoginName, string Password, string hash, string salt, int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateAccess", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pLoginName = cmd.Parameters.Add("@LoginName", SqlDbType.VarChar);
                SqlParameter pPassword = cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                SqlParameter pPasswordString = cmd.Parameters.Add("@PasswordString", SqlDbType.VarChar);
                SqlParameter pSalt = cmd.Parameters.Add("@salt", SqlDbType.VarChar);
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);

                pLoginName.Value = LoginName;
                pPassword.Value = hash;
                pPasswordString.Value = Password;
                pSalt.Value = salt;
                pAppId.Value = AppId;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function insertupdatepassword

        //Called BY ExportActBL.AddInfoToACT, ExportActBL.UpdateAct, OnlineAppBL.UpdateLastSyncDate

        public bool UpdateDocusignAccess(int AppId, string Access)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOnlineAppDocusignAcc", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pDocuSignAccess = cmd.Parameters.Add("@DocuSignAccess", SqlDbType.VarChar);
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);

                pDocuSignAccess.Value = Access;
                pAppId.Value = AppId;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end UpdateDocuSign
        
        public bool UpdateLastSyncDate(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "UPDATE OnlineAppNewApp SET LastSynchDate = GetDate() WHERE AppId = @AppId ";
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppID", AppId));
                cmd.ExecuteNonQuery();
                return true;
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
        }//end UpdateSyncDate

        public DataSet GetOnlineAppStatus( int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_GetOnlineAppStatusFields", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end GetOnlineAppStatus


        public DataSet GetOnlineAppProfile(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetOnlineAppProfile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end GetOnlineAppProfile

        public DataSet GetOnlineComplianceFee(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetOnlineAppComplianceFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end GetOnlineAppProfile

        //CALLED BY OnlineAppBL.DeleteApp
        public int DeleteApp(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteApp_2DBs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppID = cmd.Parameters.Add("@AppId", SqlDbType.VarChar);
                pAppID.Value = AppId;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
        }//end function DeleteApp

        public int DelDocuSignEnv(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DelDocuSignEnv", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppID = cmd.Parameters.Add("@AppId", SqlDbType.VarChar);
                pAppID.Value = AppId;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
        }//end function DeleteApp

      
        //CALLED BY OnlineAppStatusBL.UpdateStatusInformation, OnlineAppStatusBL.ExportACTStatus, 
        //OnlineAppBL.UpdateStatus
        public bool UpdateNewAppStatus(int AppId, string Status, string StatusGW)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
       
                SqlCommand cmd = new SqlCommand("sp_UpdateOnlineAppStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Status", Status));
                cmd.Parameters.Add(new SqlParameter("@StatusGW", StatusGW));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
            return true;
        }//end function UpdateNewAppStatus

        //CALLED BY OnlineAppStatusBL.UpdateStatusInformation
        public bool UpdateOnlineAppPlatform(int AppId, string Platform)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOnlineAppPlatform", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Platform", Platform));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
            return true;
        }//end function UpdateOnlineAppPlatform

        public string GetOnlineAppEditDate(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                string onlineAppEditDate = string.Empty;
                SqlCommand cmdData = new SqlCommand("SP_GetOnlineAppEditDate", Conn);
                cmdData.CommandType = CommandType.StoredProcedure;
                cmdData.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdData;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds.Tables[0].Rows.Count == 1)
                    onlineAppEditDate = ds.Tables[0].Rows[0][0].ToString();
                else if (ds.Tables[0].Rows.Count > 1)
                    onlineAppEditDate = "Multiple";    
                return onlineAppEditDate;
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
        }//end GetACTLastEditDate

        public string GetOnlineAppLastSynchDate(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                string onlineAppLastSynchDate = string.Empty;
                SqlCommand cmdData = new SqlCommand("SP_GetOnlineAppLastSyncDate", Conn);
                cmdData.CommandType = CommandType.StoredProcedure;
                cmdData.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdData;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds.Tables[0].Rows.Count == 1)
                    onlineAppLastSynchDate = ds.Tables[0].Rows[0][0].ToString();
                else if (ds.Tables[0].Rows.Count > 1)
                    onlineAppLastSynchDate = "Multiple";
                return onlineAppLastSynchDate;
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
        }//end GetACTLastEditDate

        //
        public bool UpdateReferral (int AppId, int ReferralID, string OtherReferral)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOnlineAppReferral", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ReferralID", ReferralID) );
                cmd.Parameters.Add(new SqlParameter("@OtherReferral", OtherReferral) );
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
            return true;
        }//end function UpdateNewAppStatus

        //CALLED BY ExportActBL.ExportActStatus
        public int UpdateNewAppExportedAddlServicesStatus(int AppId, string StatusOnlineDebit, string StatusGiftCard,
            string StatusCheckService, string StatusEBT, string StatusMerchantFunding, string StatusLease,
            string StatusPayroll)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStatusAddlServices", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StatusOnlineDebit", StatusOnlineDebit));
                cmd.Parameters.Add(new SqlParameter("@StatusGiftCard", StatusGiftCard));
                cmd.Parameters.Add(new SqlParameter("@StatusCheckService", StatusCheckService));
                cmd.Parameters.Add(new SqlParameter("@StatusEBT", StatusEBT));
                cmd.Parameters.Add(new SqlParameter("@StatusMerchantFunding", StatusMerchantFunding));
                cmd.Parameters.Add(new SqlParameter("@StatusLease", StatusLease));
                cmd.Parameters.Add(new SqlParameter("@StatusPayroll", StatusPayroll));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end function UpdateNewAppExportedAddlServicesStatus

        //CALLED BY ExportActBL.ExportActStatus
        public int UpdateNewAppExportedAddlServicesStatus(int AppId, string StatusGiftCard,
            string StatusCheckService, string StatusMerchantFunding, string StatusLease,
            string StatusPayroll)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStatusAddlServicesEdit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StatusGiftCard", StatusGiftCard));
                cmd.Parameters.Add(new SqlParameter("@StatusCheckService", StatusCheckService));
                cmd.Parameters.Add(new SqlParameter("@StatusMerchantFunding", StatusMerchantFunding));
                cmd.Parameters.Add(new SqlParameter("@StatusLease", StatusLease));
                cmd.Parameters.Add(new SqlParameter("@StatusPayroll", StatusPayroll));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end function UpdateNewAppExportedAddlServicesStatus

        public bool UpdateODEBT(int AppId, bool OnlineDebit, bool EBT)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOnlineAppUpdateODEBT", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OnlineDebit", OnlineDebit));
                cmd.Parameters.Add(new SqlParameter("@EBT", EBT));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
            return true;
        }//end function UpdateAddlServices


        public bool UpdateAddlServices(int AppId, bool OnlineDebit, bool CheckServices, bool GiftCard, bool EBT, bool Lease, 
            bool MerchantFunding, bool Payroll)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);

            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOnlineAppAddlServices", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OnlineDebit", OnlineDebit));
                cmd.Parameters.Add(new SqlParameter("@CheckServices", CheckServices));
                cmd.Parameters.Add(new SqlParameter("@GiftCard", GiftCard));
                cmd.Parameters.Add(new SqlParameter("@EBT", EBT));
                cmd.Parameters.Add(new SqlParameter("@Lease", Lease));
                cmd.Parameters.Add(new SqlParameter("@MerchantFunding", MerchantFunding));
                cmd.Parameters.Add(new SqlParameter("@Payroll", Payroll));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
            return true;
        }//end function UpdateAddlServices

        public bool UpdateNewAppGWStatus( int AppId, string StatusGW)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_UpdateGWStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StatusGW", StatusGW));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
            return true;
        }//end function UpdateNewAppGWStatus

        //CALLED BY OnlineAppBL.UpdateNewAppInfo
        public int UpdateNewAppInfo(int AppId, string RepNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateNewApp_2DBs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RepNum", RepNum));
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end function UpdateNewAppInfo

        //Creates a New Online App from a record in ACT. CALLED BY ExportActBL.ExportData
        public int InsertNewAppInfoFromACT(int PID, int ReferralID, string RepNum,
            string Status, string StatusGW, byte OnlineDebit, byte CheckServices,
            byte GiftCard, byte EBT, byte Wireless, byte Payroll, byte Lease, string OtherReferral)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertNewAppFromACT_2DBs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PID", PID));
                cmd.Parameters.Add(new SqlParameter("@ReferralID", ReferralID));
                cmd.Parameters.Add(new SqlParameter("@RepNum", RepNum));
                cmd.Parameters.Add(new SqlParameter("@Status", Status));
                cmd.Parameters.Add(new SqlParameter("@StatusGW", StatusGW));
                cmd.Parameters.Add(new SqlParameter("@OnlineDebit", OnlineDebit));
                cmd.Parameters.Add(new SqlParameter("@CheckServices", CheckServices));
                cmd.Parameters.Add(new SqlParameter("@GiftCard", GiftCard));
                cmd.Parameters.Add(new SqlParameter("@EBT", EBT));
                cmd.Parameters.Add(new SqlParameter("@Wireless", Wireless));
                cmd.Parameters.Add(new SqlParameter("@Payroll", Payroll)); 
                cmd.Parameters.Add(new SqlParameter("@Lease", Lease));
                cmd.Parameters.Add(new SqlParameter("@OtherReferral", OtherReferral));
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.SmallInt);
                pAppId.Direction = ParameterDirection.ReturnValue;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                int AppId = Convert.ToInt32(pAppId.Value);
                return AppId;
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
        }//end function InsertNewAppInfoFromACT

        public bool CheckAccess(string iUserID, int OnlineAppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringToken);
            int RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckOnlineAppAccess", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@iUserID", iUserID));
                cmd.Parameters.Add(new SqlParameter("@OnlineAppId", OnlineAppId));
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.SmallInt);
                pRetVal.Direction = ParameterDirection.ReturnValue;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                RetVal = Convert.ToInt32(pRetVal.Value);

                return Convert.ToBoolean(RetVal);
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
        }//end function GetAccess

      
        //This function gets status bits based on App ID
        public DataSet GetAddlServiceBits(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetAddlServiceBits", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);
                pAppId.Value = AppId;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "OnlineAppNewApp");

                Conn.Close();
                Conn.Dispose();
                return ds;

            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function 

     

        //CALLED BY ExportActBL.ExportData
        public bool InsertUpdateNBC(string AppId, string DiscoverNum, string AmexNum, string JCBNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdNBCInfo = new SqlCommand("sp_InsertUpdateNBC", Conn);
                cmdNBCInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pDiscoverNum = cmdNBCInfo.Parameters.Add("@DiscoverNum", SqlDbType.VarChar);
                SqlParameter pAmexNum = cmdNBCInfo.Parameters.Add("@AmexNum", SqlDbType.VarChar);
                SqlParameter pJCBNum = cmdNBCInfo.Parameters.Add("@JCBNum", SqlDbType.VarChar);
                SqlParameter pAppId = cmdNBCInfo.Parameters.Add("@AppId", SqlDbType.Int);

                pDiscoverNum.Value = DiscoverNum;
                pAmexNum.Value = AmexNum;
                pJCBNum.Value = JCBNum;
                pAppId.Value = AppId;


                cmdNBCInfo.Connection.Open();
                cmdNBCInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function InsertUpdateNBC

        //CALLED BY ExportActBL.ExportActStatus
        public bool InsertUpdatePlatform(int AppId, string Platform, string MerchantNum, string MerchantID, string TerminalID, string LoginID,
               string BankIDNum, string AgentBankIDNum, string AgentChainNum, string MCCCategoryCode, string StoreNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdNBCInfo = new SqlCommand("SP_InsertUpdatePlatform", Conn);
                cmdNBCInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pPlatform = cmdNBCInfo.Parameters.Add("@Platform", SqlDbType.VarChar);
                SqlParameter pMerchantNum = cmdNBCInfo.Parameters.Add("@MerchantNum", SqlDbType.VarChar);
                SqlParameter pMerchantID = cmdNBCInfo.Parameters.Add("@MerchantID", SqlDbType.VarChar);
                SqlParameter pTerminalID = cmdNBCInfo.Parameters.Add("@TerminalID", SqlDbType.VarChar);
                SqlParameter pLoginID = cmdNBCInfo.Parameters.Add("@LoginID", SqlDbType.VarChar);
                SqlParameter pBankIDNum = cmdNBCInfo.Parameters.Add("@BankIDNum", SqlDbType.VarChar);
                SqlParameter pAgentBankIDNum = cmdNBCInfo.Parameters.Add("@AgentBankIDNum", SqlDbType.VarChar);
                SqlParameter pAgentChainNum = cmdNBCInfo.Parameters.Add("@AgentChainNum", SqlDbType.VarChar);
                SqlParameter pMCCCategoryCode = cmdNBCInfo.Parameters.Add("@MCCCategoryCode", SqlDbType.VarChar);
                SqlParameter pStoreNum = cmdNBCInfo.Parameters.Add("@StoreNum", SqlDbType.VarChar);
                SqlParameter pAppId = cmdNBCInfo.Parameters.Add("@AppId", SqlDbType.Int);

                pPlatform.Value = Platform;
                pMerchantNum.Value = MerchantNum;
                pMerchantID.Value = MerchantID;
                pTerminalID.Value = TerminalID;
                pLoginID.Value = LoginID;
                pBankIDNum.Value = BankIDNum;
                pAgentBankIDNum.Value = AgentBankIDNum;
                pAgentChainNum.Value = AgentChainNum;
                pMCCCategoryCode.Value = MCCCategoryCode;
                pStoreNum.Value = StoreNum;
                pAppId.Value = AppId;


                cmdNBCInfo.Connection.Open();
                cmdNBCInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function InsertUpdatePlatform

        //CALLED BY ExportACTBL.ExportData
        public bool InsertUpdateReprogram(int AppId, string Platform, string MerchantNum, string MerchantID, string TerminalID, string LoginID,
               string BankIDNum, string AgentBankIDNum, string AgentChainNum, string MCCCategoryCode, string StoreNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            SqlCommand cmdNBCInfo = new SqlCommand("SP_InsertUpdateRPG", Conn);
            cmdNBCInfo.CommandType = CommandType.StoredProcedure;
            SqlParameter pPlatform = cmdNBCInfo.Parameters.Add("@Platform", SqlDbType.VarChar);
            SqlParameter pMerchantNum = cmdNBCInfo.Parameters.Add("@MerchantNum", SqlDbType.VarChar);
            SqlParameter pMerchantID = cmdNBCInfo.Parameters.Add("@MerchantID", SqlDbType.VarChar);
            SqlParameter pTerminalID = cmdNBCInfo.Parameters.Add("@TerminalID", SqlDbType.VarChar);
            SqlParameter pLoginID = cmdNBCInfo.Parameters.Add("@LoginID", SqlDbType.VarChar);
            SqlParameter pBankIDNum = cmdNBCInfo.Parameters.Add("@BankIDNum", SqlDbType.VarChar);
            SqlParameter pAgentBankIDNum = cmdNBCInfo.Parameters.Add("@AgentBankIDNum", SqlDbType.VarChar);
            SqlParameter pAgentChainNum = cmdNBCInfo.Parameters.Add("@AgentChainNum", SqlDbType.VarChar);
            SqlParameter pMCCCategoryCode = cmdNBCInfo.Parameters.Add("@MCCCategoryCode", SqlDbType.VarChar);
            SqlParameter pStoreNum = cmdNBCInfo.Parameters.Add("@StoreNum", SqlDbType.VarChar);
            SqlParameter pAppId = cmdNBCInfo.Parameters.Add("@AppId", SqlDbType.Int);

            pPlatform.Value = Platform;
            pMerchantNum.Value = MerchantNum;
            pMerchantID.Value = MerchantID;
            pTerminalID.Value = TerminalID;
            pLoginID.Value = LoginID;
            pBankIDNum.Value = BankIDNum;
            pAgentBankIDNum.Value = AgentBankIDNum;
            pAgentChainNum.Value = AgentChainNum;
            pMCCCategoryCode.Value = MCCCategoryCode;
            pStoreNum.Value = StoreNum;
            pAppId.Value = AppId;


            cmdNBCInfo.Connection.Open();
            cmdNBCInfo.ExecuteNonQuery();
            Conn.Close();
            Conn.Dispose();
            return true;
        }//end function InsertUpdateReprogram

        //CALLED BY ExportActBL.ExportActStatus
        public bool InsertUpdateGateway(string AppId, string GatewayName, string GatewayLogin)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdGateway = new SqlCommand("sp_InsertUpdateGateway", Conn);
                cmdGateway.CommandType = CommandType.StoredProcedure;
                SqlParameter pGateway = cmdGateway.Parameters.Add("@Gateway", SqlDbType.VarChar);
                SqlParameter pGatewayLogin = cmdGateway.Parameters.Add("@GatewayUserID", SqlDbType.VarChar);
                SqlParameter pAppId = cmdGateway.Parameters.Add("@AppId", SqlDbType.Int);

                pGateway.Value = GatewayName;
                pGatewayLogin.Value = GatewayLogin;
                pAppId.Value = AppId;


                cmdGateway.Connection.Open();
                cmdGateway.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function InsertUpdateGateway

        //CALLED BY OnlineAppSummaryBL.GetEditInfo
        public DataSet GetEditInfo(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetOnlineAppEdit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.SmallInt);
                pAppId.Value = AppId;

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
        }//end function GetEditInfo
        //CALLED BY OnlineAppSummaryBL.ResetLoginAttemptCount
        public int ResetLoginAttemptCount(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ResetLoginCount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppID = cmd.Parameters.Add("@AppId", SqlDbType.SmallInt);
                pAppID.Value = AppId;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
        }//end function ResetLoginAttemptCount

        //This function returns encrypted Merchant Number
        public bool GetEnMerchantNum(int AppID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetEnMerchantNum", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppID = cmd.Parameters.Add("@AppID", SqlDbType.SmallInt);
                pAppID.Value = AppID;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function 

        public DataSet GetAddlServices(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetOnlineAppAddlServices", Conn);
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                Conn.Close();
                Conn.Dispose();
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

    }//end class OnlineAppDL
}
