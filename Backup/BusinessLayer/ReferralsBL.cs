using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class ReferralsBL
    {
        //This function updates Referrals
        public bool UpdateReferralsInfo(string RefTotal, string CommissionID)
        {
            ReferralsDL Update = new ReferralsDL();
            bool retVal = Update.UpdateReferrals(RefTotal, CommissionID);
            return retVal;
        }//end function UpdateReferralsInfo

        //Get List of Referrals
        public DataSet GetReferralList()
        {
            ReferralsDL Referrals = new ReferralsDL();
            DataSet ds = Referrals.GetReferralList();
            return ds;
        }

        //This function gets Referrals details for selected rep and month. 
        //CALLED BY Referrals.aspx, ReferralsAdmin.aspx, ReferralsPrint.aspx
        public DataSet GetReferralsDetail(string ReferralID, string Month)
        {
            ReferralsDL Reff = new ReferralsDL();
            DataSet ds = Reff.GetReferrals(ReferralID, Month);
            return ds;
        }

        public DataSet GetReferralsDetailbyMonPeriod(string ReferralID, string Month, int Period)
        {
            ReferralsDL Reff = new ReferralsDL();
            DataSet ds = Reff.GetReferralsbyMonPeriod(ReferralID, Month, Period);
            return ds;
        }
        //end function GetReferralsDetail

        //This function gets Teir 1 Referrals details for selected rep and month. 
        //CALLED BY Referrals.aspx, ReferralsAdmin.aspx, ReferralsPrint.aspx
        public DataSet GetT1Referrals(string ReferralID, string Month)
        {
            ReferralsDL Reff = new ReferralsDL();
            DataSet ds = Reff.GetT1Referrals(ReferralID, Month);
            return ds;
        }//end function GetT1Referrals

        //This function gets Commission detail for specific commission id
        public DataSet GetReferralDetailFromID(string CommissionID)
        {
            ReferralsDL ReffDetail = new ReferralsDL();
            DataSet ds = ReffDetail.ReturnReferralDetailFromID(CommissionID);
            return ds;
        }//end function GetReferralDetailFromID

        //This function gets Commission detail for By Company for verification
        public DataSet GetReferralsByCompany(string Company, string Month)
        {            
            ReferralsDL ReffDetail = new ReferralsDL();
            DataSet ds = ReffDetail.ReturnReferralsByCompany(Company, Month);
            return ds;
        }//end function GetReferralsByCompany
    }//end class ReferralsBL
}
