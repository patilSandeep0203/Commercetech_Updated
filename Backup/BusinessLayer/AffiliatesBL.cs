using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    /// <summary>
    /// This class gets data from the Affiliates table
    /// </summary>
    public class AffiliatesBL
    {        
        private int AffiliateID = 0;      

        public AffiliatesBL(int AffiliateID)
        {
            this.AffiliateID = AffiliateID;
        }

        //CALLED BY Login.aspx
        public string ReturnContactName()
        {
            string Contact = string.Empty;
            PartnerDS.AffiliatesDataTable dt = GetAffiliate();
            if (dt.Rows.Count > 0)
                Contact = dt[0].Contact.ToString().Trim();
            return Contact;
        }//end function GetAffiliateContactName

        //This function gets Confirmation Info based on affiliate id and month - CALLED by Residuals.aspx
        public DataSet GetConfirmationComm(string Month)
        {
            AffiliatesDL CCode = new AffiliatesDL();
            DataSet ds = CCode.GetConfirmationComm(AffiliateID, Month);
            return ds;
        }//end function    

        //This function gets Confirmation Info based on affiliate id and month period - CALLED by Residuals.aspx
        public DataSet GetConfirmationCommByRepMonPeriod(string Month, int Period)
        {
            AffiliatesDL CCode = new AffiliatesDL();
            DataSet ds = CCode.GetConfirmationCommByRepMonPeriod(AffiliateID, Month, Period);
            return ds;
        }//end function   

        //CALLED BY VerifyAppLogin.aspx
        public string CreateOnlineAppToken(string appName, string loginID, int AppID)
        {
            UsersDL User = new UsersDL();
            string strToken = User.CreateOnlineAppToken(appName, loginID, AppID);            
            return strToken;
        }

        //Called by ChangePWD.aspx, AddPartner.aspx, EditInfo.aspx
        public PartnerDS.AffiliatesDataTable GetAffiliate()
        {
            AffiliatesTableAdapter affiliatesAdapter = new AffiliatesTableAdapter();
            return affiliatesAdapter.GetData(Convert.ToInt16(AffiliateID));            
        }

        //Called by EditInfo.aspx
        public PartnerDS.AffiliateBankingDataTable GetAffiliateBanking()
        {
            AffiliateBankingTableAdapter AffiliateBankingAdapter = new AffiliateBankingTableAdapter();
            return AffiliateBankingAdapter.GetData(Convert.ToInt16(AffiliateID));
        }
          

        //Called by AddPartner.aspx
        public DataSet GetAffiliateAddPartner()
        {
            AffiliatesDL AffiliateLogin = new AffiliatesDL();
            DataSet dsAffiliate = AffiliateLogin.GetAffiliateAddPartner(AffiliateID);
            return dsAffiliate;
        }

        public DataSet GetAffiliateSalesOffice()
        {
            AffiliatesDL AffiliateLogin = new AffiliatesDL();
            DataSet dsAffiliate = AffiliateLogin.GetAffiliateSalesOffice(AffiliateID);
            return dsAffiliate;
        }

        //This function inserts/updates banking info for affiliate signups and edit profile
        //CALLED BY EditBanking.aspx
        public bool InsertUpdateBankingInfo(string BankName, string OtherBank, string Address, string ZipCode,
        string City, string State, string NameOnCheckingAcct, string AcctNum, string RoutingNum, string Phone)
        {
            AffiliatesDL BankingInfo = new AffiliatesDL();
            bool retVal = BankingInfo.InsertUpdateAffiliateBankingInfo(AffiliateID, BankName, OtherBank, Address, ZipCode, City, State,
                NameOnCheckingAcct, AcctNum, RoutingNum, Phone);
            return retVal;
        }

        //CALLED BY EditInfo.aspx
        public bool UpdateAffiliateWiz(string FirstName, string LastName, string PasswordPhrase, 
            string DBA, string CheckPayable, string Email, string TaxSSN, string TaxID, string SSN, string Address, 
            string City, string State, string Region, string Zip, string Country, string MailingAddress, 
            string MailingCity, string MailingState, string MailingRegion, string MailingZip, 
            string MailingCountry, string Phone, string HomePhone, string MobilePhone ,string Fax, string URL,  string Comments, bool Notify, 
            string LegalStatus, bool bDirectDeposit)
        {
            AffiliatesDL UpdateAffiliate = new AffiliatesDL();
            bool retVal = UpdateAffiliate.UpdateAffiliateInfo(AffiliateID, FirstName, LastName, PasswordPhrase, 
                DBA, CheckPayable, Email, TaxSSN, TaxID, SSN, Address, City, State, Region, Zip, Country, MailingAddress, 
                MailingCity, MailingState, MailingRegion, MailingZip, MailingCountry, Phone, HomePhone, 
                MobilePhone, Fax, URL, Comments, Notify, LegalStatus, bDirectDeposit);

            return retVal;
        }

        //Called by UpdatePartnerInfo.aspx - Editing PartnerInfo from Leads section
        public bool UpdateAffiliateLead(string FirstName, string LastName, string DBA,
            string CompanyName, string LegalStatus, string TaxSSN, string TaxID, string SSN,
            string Address, string City, string State, string Zip, string Country, 
            string MailingAddress, string MailingCity, string MailingState, string MailingZip,
            string MailingCountry, string Phone, string HomePhone, string MobilePhone, string Fax, 
            string URL, string Email, int ReferralID, string Specify)
        {
            AffiliatesDL UpdateAffiliate = new AffiliatesDL();
            bool retVal = UpdateAffiliate.UpdateAffiliateLead(AffiliateID, FirstName, LastName, DBA, CompanyName,
                LegalStatus, TaxSSN, TaxID, SSN, Address, City, State, Zip, Country, MailingAddress, MailingCity, MailingState, 
                MailingZip, MailingCountry, Phone, HomePhone, MobilePhone, Fax, URL, Email, ReferralID, Specify);
            return retVal;
        }

        public bool updateAffiliateOffice(string OfficeID)
        {
            AffiliatesDL UpdateAffiliate = new AffiliatesDL();
            bool retVal = UpdateAffiliate.updateAffiliateOffice(AffiliateID, OfficeID);
            return retVal;
        }

        //This function returns the ACTUserID for an Affiliate. CALLED BY Edit.aspx, SendReminder.aspx
        public string ReturnACTUserID()
        {
            //Set initial ID to ACT System User
            string strActUserID = "{11111111-1111-1111-1111-111111111111}";
            string returnID = "";
            AffiliatesDL Aff = new AffiliatesDL();
 
            returnID = Aff.GetACTUserID(AffiliateID);
            if (returnID == "")
                return strActUserID;
            else
                return returnID;
        }

        //This function returns rep list - CALLED BY CommUpdate.aspx
        public DataSet GetAffiliateList()
        {
            AffiliatesDL Aff = new AffiliatesDL();
            DataSet ds = Aff.GetAffiliateList();
            return ds;
        }

        public DataSet GetSalesOfficeList()
        {
            AffiliatesDL Aff = new AffiliatesDL();
            DataSet ds = Aff.GetSalesOfficeList();
            return ds;
        }

        //This function returns rep list - CALLED BY AddPartner.aspx
        public DataSet GetNonRepList(string MasterNum)
        {
            AffiliatesDL Aff = new AffiliatesDL();
            DataSet ds = Aff.GetNonRepList(MasterNum);
            return ds;
        }


        //This function gets commissions summary by AffiliateID - CALLED By CommSummary.aspx.cs
        public DataSet GetCommRefPaymentByAffiliateID(string Month)
        {
            AffiliatesDL Comm = new AffiliatesDL();
            DataSet ds = Comm.GetCommRefPaymentByAffiliateID(AffiliateID, Month);
            return ds;
        }//end function GetCommRefPaymentByAffiliateID

        //This function gets commissions summary by AffiliateID - CALLED By CommSummary.aspx.cs
        public DataSet GetResdCommPaymentByAffiliateID(string Month, int Period)
        {
            AffiliatesDL Comm = new AffiliatesDL();
            DataSet ds = Comm.GetResdCommPaymentByAffiliateID(AffiliateID, Month, Period);
            return ds;
        }//end function GetCommRefPaymentByAffiliateID

        //This function gets commissions summary by AffiliateID - CALLED By ResdSummary.aspx.cs
        public DataSet GetResidualPaymentByAffiliateID( string Month)
        {
            AffiliatesDL Resd = new AffiliatesDL();
            DataSet ds = Resd.GetResidualPaymentByAffiliateID(AffiliateID, Month);
            return ds;
        }//end function GetCommRefPaymentByAffiliateID

        //This function changes default package for agents and employees. CALLED BY Home.aspx
        public bool ChangeDefaultPackage(int PID, int CPPID)
        {
            AffiliatesDL Aff = new AffiliatesDL();
            bool retVal = Aff.UpdateDefaultPackage(AffiliateID, PID, CPPID);
            return retVal;
        }
        
        //This function returns the Master Rep Number based on Affiliate ID - CALLED BY AddPartner.aspx
        public string ReturnMasterNum()
        {
            AffiliatesDL Aff = new AffiliatesDL();
            return Aff.ReturnAffiliateRepNum(AffiliateID);
        }

        //This function returns the T1 Master Rep Number based on Affiliate ID 
        public string ReturnT1MasterNum()
        {
            AffiliatesDL Aff = new AffiliatesDL();
            return Aff.ReturnAffiliateT1MasterNum(AffiliateID);
        }
        

    }//end class AffiliatesBL
}
