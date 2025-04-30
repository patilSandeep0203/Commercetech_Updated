using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class OnlineAppSummaryBL
    {
        //This function returns Online Application information. CALLED BY OnlineAppMgmt/default.aspx, OnlineAppAgent.aspx
        public PartnerDS.OnlineAppSummaryDataTable GetSummary(string RepNum, string Status, string FromDate, string ToDate, 
            string SortBy, bool bDisplaySynched)
        {
            OnlineAppSummaryTableAdapter OnlineAppSummaryAdapter = new OnlineAppSummaryTableAdapter();
            return OnlineAppSummaryAdapter.GetData(RepNum, Status, FromDate, ToDate, SortBy, bDisplaySynched);
        }//end function GetSummary

        //CALLED BY OnlineAppMgmt/default.aspx
        public PartnerDS.OnlineAppSummaryDataTable GetSummaryLookup(string ColName, string Value)
        {
            OnlineAppSummaryTableAdapter OnlineAppSummaryAdapter = new OnlineAppSummaryTableAdapter();

            return OnlineAppSummaryAdapter.GetDataByLookup(ColName, Value);
      
        }//end function GetSummaryLookup

        //This function returns online applications for resellers and affiliates. CALLED BY OnlineAppReferrals.aspx
        public PartnerDS.OnlineAppSummaryDataTable GetSummaryAff(string AffiliateID, string Status, string FromDate, string ToDate)
        {
            OnlineAppSummaryTableAdapter OnlineAppSummaryAdapter = new OnlineAppSummaryTableAdapter();

            return OnlineAppSummaryAdapter.GetDataReferrals(AffiliateID, Status, FromDate, ToDate);

        }//end function GetSummaryAff

        //CALLED BY Home.aspx
        public DataSet GetNewAppIDs(string RepNum)
        {
            OnlineAppSummaryDL Summary = new OnlineAppSummaryDL();
            DataSet ds = Summary.GetNewAppIDs(RepNum);
            return ds;
        }//end function GetNewAppCount

       //CALLED BY Home.aspx
        public DataSet GetUnsyncAppbyRep(string RepNum)
        {
            OnlineAppSummaryDL Summary = new OnlineAppSummaryDL();
            DataSet ds = Summary.GetUnsyncAppbyRep(RepNum);
            return ds;
        }

        public DataSet GetNewUploadbyRep(string RepNum)
        {
            OnlineAppSummaryDL Summary = new OnlineAppSummaryDL();
            DataSet ds = Summary.GetNewUploadbyRep(RepNum);
            return ds;
        }

        //Called by OnlineAppManagement default page
        public DataSet GetRepInfoByRepName(string RepName)
        {
            OnlineAppSummaryDL Rep = new OnlineAppSummaryDL();
            DataSet ds = Rep.GetRepInfoByRepName(RepName);
            return ds;
        }
      
    }//end class OnlineAppSummaryBL
}
