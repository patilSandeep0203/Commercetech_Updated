using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class PartnerLogBL
    {
        //CALLED BY Edit.aspx
        public DataSet GetLogData(int AppId, string Action)
        {
            PartnerLogDL LogData = new PartnerLogDL();
            DataSet dsLog = LogData.GetLogData(Action, AppId);
            return dsLog;
        }//end function GetLogData

        public DataSet GetLogForRates()
        {
            PartnerLogDL LogData = new PartnerLogDL();
            DataSet dsLog = LogData.GetLogForRates();
            return dsLog;
        }//end function GetLogData

        public bool DeleteLogData(int LogID)
        {
        PartnerLogDL LogData = new PartnerLogDL();
            bool retVal = LogData.DeleteLogInfo(LogID);
            return retVal;
        }//end function DeleteLogData

        
        // This function returns the partner portal user id
        public string ReturnPortalUserID(int AffiliateID)
        { 
            PartnerLogDL LogData = new PartnerLogDL();
            string PortalUserID = LogData.ReturnPortalUserID(AffiliateID);
            return PortalUserID;
        }

        //This function inserts log into log table
        public bool InsertLogData(int AppID, int AffiliateID, string Action)
        {
            PartnerLogDL LogData = new PartnerLogDL();
            bool retVal = LogData.InsertLog(AppID, AffiliateID, Action);
            return retVal;
        }//end function InsertLogData

        //This function inserts rates changes into package log
        public bool InsertLogRates(int PackageID, int AffiliateID, string Action)
        {
            PartnerLogDL LogData = new PartnerLogDL();
            bool retVal = LogData.InsertPackageLog(PackageID, AffiliateID, Action);
            return retVal;
        }//end function InsertLogRates

        //This function inserts log for partner updates to Admin module
        public bool InsertPartnerLog(int AffiliateID, string Action)
        {
            PartnerLogDL LogData = new PartnerLogDL();
            bool retVal = LogData.InsertPartnerLog(AffiliateID, Action);
            return retVal;
        }//end function InsertLogRates

    }//end class PartnerLogBL
}
