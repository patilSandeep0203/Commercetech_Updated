using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class OnlineAppStatusBL
    {
        private int AppId = 0;
        public OnlineAppStatusBL(int AppId)
        {
            this.AppId = AppId;
        }
        public OnlineAppStatusBL()
        {
        }

        //This function checks whether the application is locked
        public string ReturnLocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetLocked();
        
            return strLocked;
        }

        public string ReturnGatewayLocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetGatewayLocked();

            return strLocked;
        }

        public string ReturnCheckServiceLocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetCheckServiceLocked();

            return strLocked;
        }

        public string ReturnGiftLocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetGiftLocked();

            return strLocked;
        }

        public string ReturnLeaseLocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetLeaseLocked();

            return strLocked;
        }

        public string ReturnMCALocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetMCALocked();

            return strLocked;
        }

        public string ReturnPayrollLocked()
        {
            OnlineAppClassLibrary.OnlineAppStatus Status = new OnlineAppClassLibrary.OnlineAppStatus(AppId);
            string strLocked = Status.GetPayrollLocked();

            return strLocked;
        }

        //This function returns status list based on the User's Role and the Account Type for the Online App
        //CALLED BY OnlineAppMgmt/default.aspx
        public DataSet GetStatusList(string Role, string AcctTypeDesc)
        {
            OnlineAppStatusDL Status = new OnlineAppStatusDL();
            DataSet ds = null;
            if (Role == "Agent")
            {
                if (AcctTypeDesc == "Merchant")
                    ds = Status.GetStatusListAgent();
                else if (AcctTypeDesc == "Gateway")
                    ds = Status.GetStatusListGWAgent();
                else if (AcctTypeDesc == "CheckServ")
                    ds = Status.GetStatusListCheckServAgent();
                else if (AcctTypeDesc == "Gift")
                    ds = Status.GetStatusListGiftAgent();
                else if (AcctTypeDesc == "Lease")
                    ds = Status.GetStatusListLeaseAgent();
                else if (AcctTypeDesc == "MerchFund")
                    ds = Status.GetStatusListMerchFundAgent();
                else if (AcctTypeDesc == "Payroll")
                    ds = Status.GetStatusListPayrollAgent();
            }
            else if (Role == "Admin")
            {
                if (AcctTypeDesc == "Merchant")
                    ds = Status.GetStatusList();
                else if (AcctTypeDesc == "Gateway")
                    ds = Status.GetStatusListGW();
                else if (AcctTypeDesc == "CheckServ")
                    ds = Status.GetStatusListCheckServ();
                else if (AcctTypeDesc == "Gift")
                    ds = Status.GetStatusListGift();
                else if (AcctTypeDesc == "Lease")
                    ds = Status.GetStatusListLease();
                else if (AcctTypeDesc == "MerchFund")
                    ds = Status.GetStatusListMerchFund();
                else if (AcctTypeDesc == "Payroll")
                    ds = Status.GetStatusListPayroll();
            }


            return ds;
           
        }//end function GetStatusList

        //This function gets Status info for AppId. CALLED BY Edit.aspx
        public DataSet GetStatusSearchList()
        {
            OnlineAppStatusDL Status = new OnlineAppStatusDL();
            DataSet ds = Status.GetStatusSearchList();
            return ds;
        }//end function GetStatusFields

        //This function gets Status info for AppId. CALLED BY Edit.aspx
        public PartnerDS.OnlineAppStatusFieldsDataTable GetStatusFields()
        {
            OnlineAppStatusFieldsTableAdapter OnlineAppStatusFieldsAdapter = new OnlineAppStatusFieldsTableAdapter();
            return OnlineAppStatusFieldsAdapter.GetData(Convert.ToInt16(AppId));
        }//end function GetStatusFields

        //This function checks whether account has reprogram info. CALLED BY Edit.aspx
        public bool CheckReprogram()
        {
            OnlineAppStatusDL Check = new OnlineAppStatusDL();
            DataSet ds = Check.CheckReprogramExists(AppId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["Reprogram"].ToString() == "True")
                    return true;
                else
                    return false;
            }
            else
                return false;

        }//end function CheckReprogram

        //This function returns platform information. CALLED BY Edit.aspx
        public DataSet GetPlatformInfo()
        {
            OnlineAppStatusDL PlatformInfo = new OnlineAppStatusDL();
            DataSet ds = PlatformInfo.GetPlatformData(AppId);
            return ds;
        }//end function GetPlatformInfo

        //This function returns platform information. CALLED BY Edit.aspx
        public DataSet GetReprogramInfo()
        {
            OnlineAppStatusDL ReprogramInfo = new OnlineAppStatusDL();
            DataSet ds = ReprogramInfo.GetReprogramData(AppId);
            return ds;
        }//end function GetReprogramInfo


        //This function Deletes Notes. CALLED BY Edit.aspx
        public int DeleteNote(string NoteID)
        {
            OnlineAppStatusDL Notes = new OnlineAppStatusDL();
            int iRetVal = Notes.DeleteNote( NoteID);
            return iRetVal;
        }//end function GetNotes

        //This function updates status information. CALLED BY Edit.aspx
        public string UpdateStatusInformation(string Status, string StatusGW, string Platform, int AffiliateID)
        {
            //Update status in OnlineAppNewApp
            OnlineAppDL OnlineApp = new OnlineAppDL();
            bool retVal = OnlineApp.UpdateNewAppStatus(AppId, Status, StatusGW);
            if (retVal)
            {
                retVal = OnlineApp.UpdateOnlineAppPlatform(AppId, Platform);
                if (retVal)
                    return "Status Information Updated Successfully.";
                else
                    return "Platform Information was not Updated.";
            }
            else
                return "Status Information was not Updated.";
        }//end UpdateStatusInformation        

        //This function deletes info from Platform Table
        public bool DeletePlatform()
        {
            OnlineAppStatusDL DelPlatform = new OnlineAppStatusDL();
            bool retVal = DelPlatform.DeletePlatform(AppId);
            return retVal;
        }//end if DeletePlatform

        //This function deletes info from Reprogram table
        public bool DeleteReprogram()
        {
            OnlineAppStatusDL DelReprogram = new OnlineAppStatusDL();
            bool retVal = DelReprogram.DeleteReprogramInfo( AppId);
            return retVal;
        }//end if DeleteReprogram

        //This function Inserts/Updates NBC information
        public bool InsertUpdateNBC(string DiscoverNum, string AmexNum, string JCBNum, string strModify)
        {
            OnlineAppStatusDL InsertUpdateGW = new OnlineAppStatusDL();
            bool retVal = InsertUpdateGW.InsertUpdateNBCInfo(DiscoverNum, AmexNum, JCBNum, "", AppId);
            return retVal;
        }// InsertUpdateNBC

        //This function Inserts/Updates Gateway information
        public bool InsertUpdateGateway(string GatewayUserID, string GatewayPassword)
        {
            OnlineAppStatusDL App = new OnlineAppStatusDL();
            bool retVal = App.InsertUpdateGatewayInfo(GatewayUserID, GatewayPassword, AppId);
            return retVal;
        }//end InsertUpdateGateway

        //This function checks if gateway info exists for appid
        public bool CheckGatewayExists()
        {
            bool retVal = false;
            OnlineAppStatusDL Check = new OnlineAppStatusDL();
            DataSet ds = Check.CheckGateway(AppId);
            if (ds.Tables[0].Rows.Count > 0)
                retVal = true;
            return retVal;
        }//end CheckGatewayExists

        //This function checks if gateway info exists for appid
        public bool CheckNBCExists()
        {
            bool retVal = false;
            OnlineAppStatusDL Check = new OnlineAppStatusDL();
            DataSet ds = Check.CheckNBC(AppId);
            if (ds.Tables[0].Rows.Count > 0)
                retVal = true;
            return retVal;
        }//end CheckGatewayExists

        //This function inserts note. CALLED BY Edit.aspx
        public bool InsertNote(string ActUserID, string NoteText)
        {
            OnlineAppStatusDL Note = new OnlineAppStatusDL();
            DataSet ds = Note.InsertNote(ActUserID, AppId, NoteText, DateTime.Now.ToString());

            //Add the same note to ACT
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Get ContactID from ACT based on AppID
                ACTDataDL ACT = new ACTDataDL();
                string ContactID = ACT.ReturnContactID(AppId);
                if (ContactID == "")
                    return false;

                DataRow drNotes = ds.Tables[0].Rows[0];

                ACT.InsertNotes(drNotes["NoteID"].ToString().Trim(), ContactID,
                                ActUserID, NoteText, DateTime.Now.ToString());
                return true;
            }
            return false;
        }//end function InsertNote

        public bool InsertTrackingNumber(string ActUserID, string NoteText)
        {
            OnlineAppStatusDL Note = new OnlineAppStatusDL();
            DataSet ds = Note.InsertTrackingNumber(ActUserID, AppId, NoteText, DateTime.Now.ToString());

            //Add the same note to ACT
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Get ContactID from ACT based on AppID
                ACTDataDL ACT = new ACTDataDL();
                string ContactID = ACT.ReturnContactID(AppId);
                if (ContactID == "")
                    return false;

                DataRow drNotes = ds.Tables[0].Rows[0];

                ACT.InsertNotes(drNotes["NoteID"].ToString().Trim(), ContactID,
                                ActUserID, NoteText, DateTime.Now.ToString());
                return true;
            }
            return false;
        }//end function InsertNote


        public bool InsertPendingReason(string ActUserID, string NoteText)
        {
            OnlineAppStatusDL Note = new OnlineAppStatusDL();
            DataSet ds = Note.InsertPendingReason(ActUserID, AppId, NoteText, DateTime.Now.ToString());

            //Add the same note to ACT
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Get ContactID from ACT based on AppID
                ACTDataDL ACT = new ACTDataDL();
                string ContactID = ACT.ReturnContactID(AppId);
                if (ContactID == "")
                    return false;

                DataRow drNotes = ds.Tables[0].Rows[0];

                ACT.InsertNotes(drNotes["NoteID"].ToString().Trim(), ContactID,
                                ActUserID, NoteText, DateTime.Now.ToString());
                return true;
            }
            return false;
        }//end function InsertNote

        //This function returns Platform List for the selected Processor
        //CALLED BY OnlineAppMgmt/Edit.aspx Status Tab
        public DataSet GetPlatforms(string Processor)
        {
            OnlineAppStatusDL Status = new OnlineAppStatusDL();
            DataSet ds = Status.GetPlatformList(Processor);
            return ds;
        }//end function GetPackages

        }//end class OnlineAppStatusInfo
}
