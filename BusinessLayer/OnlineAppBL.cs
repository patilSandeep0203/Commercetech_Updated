using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class OnlineAppBL
    {
        private int AppId = 0;
        public OnlineAppBL(int AppId)
        {
            this.AppId = AppId;
        }

        private OnlineAppNotesTableAdapter _OnlineAppNotesAdapter = null;
        protected OnlineAppNotesTableAdapter OnlineAppNotesAdapter
        {
            get
            {
                if (_OnlineAppNotesAdapter == null)
                    _OnlineAppNotesAdapter = new OnlineAppNotesTableAdapter();

                return _OnlineAppNotesAdapter;
            }
        }

        public PartnerDS.OnlineAppNotesDataTable GetOnlineAppNotes()
        {
                return OnlineAppNotesAdapter.GetData(Convert.ToInt16(AppId));
        }

        //This function updates Last Sync date. Called by CommonFunctions.cs
        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool UpdateLastSyncDate()
        {
           OnlineAppDL NewApp = new OnlineAppDL();
            bool retVal = NewApp.UpdateLastSyncDate(AppId);
            return retVal;
        }//end function UpdateLastSyncDate

        //This function updates Referral Source
        public bool UpdateReferral(int ReferralID, string OtherReferral)
        {
            OnlineAppDL NewApp = new OnlineAppDL();
            bool retVal = NewApp.UpdateReferral(AppId, ReferralID, OtherReferral);
            return retVal;
        }

        //This function updates Merchant and Gateway Status. Called by *.aspx
        public bool UpdateStatus(string Status, string StatusGW)
        {
            OnlineAppDL NewApp = new OnlineAppDL();
            bool retVal = NewApp.UpdateNewAppStatus( AppId, Status, StatusGW);
            return retVal;
        }

        //This function updates status. Called by *.aspx
        public bool UpdateGWStatus(string StatusGW)
        {
             OnlineAppDL NewApp = new OnlineAppDL();

            bool retVal = NewApp.UpdateNewAppGWStatus(AppId, StatusGW);
            return retVal;
        }

        //This function Updates Repnum. CALLED BY Edit.aspx
        public int UpdateNewAppInfo(string RepNum)
        {
            OnlineAppDL NewApp = new OnlineAppDL();
            int iRetVal = NewApp.UpdateNewAppInfo(AppId, RepNum);
            return iRetVal;
        }//end function UpdateNewAppInfo

        public bool CheckAccess(string iUserID)
        {
    
            OnlineAppDL Access = new OnlineAppDL();
            bool bAccess = Access.CheckAccess( iUserID, AppId);
          
            return bAccess;
        }

        //This function returns General Information for an account
        public DataSet GetAddlServiceBits()
        {           
            OnlineAppDL GeneralInfo = new OnlineAppDL();
            DataSet ds = GeneralInfo.GetAddlServiceBits(AppId);
            return ds;
        }//end function GetAddlServices


        //
        public DataSet GetOnlineAppProfile()
        {
            OnlineAppDL GeneralInfo = new OnlineAppDL();
            DataSet ds = GeneralInfo.GetOnlineAppProfile(AppId);
            return ds;
        }//end function


        //This function updates status. Called by *.aspx
        public bool UpdateAddlServices(bool OnlineDebit, bool CheckServices, bool GiftCard, bool EBT, bool Lease, bool MerchantFunding,
            bool Payroll)
        {            
            OnlineAppDL NewApp = new OnlineAppDL();
            bool retVal = NewApp.UpdateAddlServices(AppId, OnlineDebit, CheckServices, GiftCard, EBT, Lease, MerchantFunding, Payroll);
            return retVal;
        }//end function OnlineAppDL

        public bool UpdateODEBT(bool OnlineDebit, bool EBT)
        {
            OnlineAppDL NewApp = new OnlineAppDL();
            bool retVal = NewApp.UpdateODEBT(AppId, OnlineDebit, EBT);
            return retVal;
        }//end function OnlineAppDL

        public DataSet ReturnAddlServices()
        {
            OnlineAppDL AddlServ = new OnlineAppDL();
            DataSet ds = AddlServ.GetAddlServices(AppId);
            return ds;
        }//end function GetCheckService

        //CALLED BY Edit.aspx
        public int DeleteAppInfo()
        {
            OnlineAppDL Delete = new OnlineAppDL();
            int iRetVal = Delete.DeleteApp(AppId);
            return iRetVal;
        }//end function DeleteAppInfo

        public int DelDocuSignEnv()
        {
            OnlineAppDL Delete = new OnlineAppDL();
            int iRetVal = Delete.DelDocuSignEnv(AppId);
            return iRetVal;
        }//end function DeleteAppInfo

        //This function updates loginname in OnlineAppAccess from the modify.aspx.cs page
        public int InsertUpdateLoginName(string LoginName)
        {
            OnlineAppDL Update = new OnlineAppDL();
            int iRetVal = Update.InsertUpdateLoginName(LoginName, AppId);
            return iRetVal;
        }//end function InsertUpdateLoginName

        //This function gets the login name from OnlineAppAccess. CALLED BY Edit.aspx
        public string ReturnLoginName()
        {
            string LoginName = string.Empty;
            OnlineAppDL Login = new OnlineAppDL();
            DataSet ds = Login.GetOnlineAppProfile(AppId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                LoginName = dr["LoginName"].ToString();
            }

            return LoginName;
        }//end function GetLoginName

        public string GetComplianceFee()
        {
            string strComplianceFee = string.Empty;
            OnlineAppDL ComplianceFee = new OnlineAppDL();
            DataSet ds = ComplianceFee.GetOnlineComplianceFee(AppId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (!Convert.IsDBNull(dr["NonComplianceFee"]))
                {
                    strComplianceFee = dr["NonComplianceFee"].ToString();
                }
                else
                {
                    strComplianceFee = "";
                }
            }

            return strComplianceFee;

        }

        //This function returns Online Application information - CALLED BY EDIT.ASPX.CS
        public DataSet GetEditInfo()
        {
            OnlineAppDL Summary = new OnlineAppDL();
            DataSet ds = Summary.GetEditInfo(AppId);
            return ds;
        }//end function GetEditInfo

        //This function resets login attempts count to 0. CALLED BY Edit.aspx
        public int ResetLoginAttemptCount()
        {
            OnlineAppDL Summ = new OnlineAppDL();
            int iRetVal = Summ.ResetLoginAttemptCount(AppId);
            return iRetVal;
        }//end function ResetLoginAttemptCount

        public bool GetEnMerchantNum(int AppID)
        {
            OnlineAppDL PlatformInfo = new OnlineAppDL();
            bool bRetVal = PlatformInfo.GetEnMerchantNum(AppId);
            return bRetVal;
        }//end function ResetLoginAttemptCount
    }//end class OnlineAppAppBL
}
