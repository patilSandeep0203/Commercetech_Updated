using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;


namespace BusinessLayer
{
    public class FirstAffiliatesLeadsBL
    {
        //This function returns leads
        public DataSet GetReports(string LeadType, string SortBy)
        {
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();
            DataSet ds;
            switch (LeadType)
            {
                case "Free Report":
                    ds = Leads.GetFreeReport();
                    break;
                case "Free Consult":
                    ds = Leads.GetFreeConsult();
                    break;
                case "Free Apply":
                    ds = Leads.GetFreeApply();
                    break;
                case "Affiliate Signups":
                    ds = Leads.GetAffiliateSignups(SortBy);
                    break;
                default:
                    ds = null;
                    break;
            }//end switch
       
            return ds;
        }//end function GetReports

        //This function returns leads
        public DataSet GetAffiliatesByLookup(string ColName, string Value)
        {
            FirstAffiliatesLeadsDL GetLeads = new FirstAffiliatesLeadsDL();
            DataSet ds = GetLeads.GetAffiliatesByLookup(ColName, Value);
            return ds;
        }//end function GetAffiliateByLookup

        //This function returns leads for agents. CALLED BY ManageLeadsPartner.aspx
        public DataSet GetLeadsPartner(string LeadType, string AffiliateID)
        {
            FirstAffiliatesLeadsDL GetLeads = new FirstAffiliatesLeadsDL();
            DataSet ds = GetLeads.GetLeadsPartner(LeadType, AffiliateID);
            return ds;
        }//end function GetLeadsPartner


        //This function creates online app from Free Report and Free Consult Leads
        public bool CreateApp(int LeadID, string LeadType)
        {
            bool retVal = false;
            DataSet ds;
            FirstAffiliatesLeadsDL Lead = new FirstAffiliatesLeadsDL();           
            switch (LeadType)
            {
                case "Free Report":
                    ds = Lead.GetFreeReport(LeadID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        retVal = Lead.CreateLeadApp(dr["Email"].ToString().Trim(), dr["FirstName"].ToString().Trim(),
                            dr["LastName"].ToString().Trim(), Convert.ToInt32(dr["ReferralID"]),
                            "", "");
                    }
                    break;
                case "Free Consult":
                    ds = Lead.GetFreeConsult(LeadID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        retVal = Lead.CreateLeadApp(dr["Email"].ToString().Trim(), dr["FirstName"].ToString().Trim(),
                            dr["LastName"].ToString().Trim(), Convert.ToInt32(dr["ReferralID"]),
                            dr["HomePhone"].ToString().Trim(), dr["Phone"].ToString().Trim());
                    }
                    break;
            }//end switch
            return retVal;
        }//end function CreateApp

        //This function creates online app from Free Report and Free Consult Leads
        public bool CreateAppExt(int LeadID, string LeadType)
        {
            bool retVal = false;
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();  
            DataSet ds = Leads.GetFreeApply(LeadID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                FirstAffiliatesLeadsDL CreateOnlineApp = new FirstAffiliatesLeadsDL();
                retVal = CreateOnlineApp.CreateLeadAppExt(dr["Email"].ToString().Trim(), dr["FirstName"].ToString().Trim(),
                    dr["LastName"].ToString().Trim(), Convert.ToInt32(dr["ReferralID"]),
                    dr["HomePhone"].ToString().Trim(), dr["MobilePhone"].ToString().Trim(),
                    dr["Phone"].ToString().Trim(), dr["Company"].ToString().Trim(),
                    dr["Address"].ToString().Trim(), dr["City"].ToString().Trim(),
                    dr["State"].ToString().Trim(), dr["Zip"].ToString().Trim(),
                    dr["Country"].ToString().Trim(), dr["URL"].ToString().Trim());
            }
            return retVal;
        }//end function CreateAppExt

        //This function deletes lead
        public bool DeleteLead(int LeadID, string LeadType)
        {
            bool retVal = false;
            FirstAffiliatesLeadsDL DeleteInfo = new FirstAffiliatesLeadsDL();
            switch (LeadType)
            {
                case "Free Report":
                    retVal = DeleteInfo.DeleteLeadInfo(LeadID, "sp_DeleteFreeReport");
                    break;
                case "Free Consult":
                    retVal = DeleteInfo.DeleteLeadInfo(LeadID, "sp_DeleteFreeConsult");
                    break;
                case "Free Apply":
                    retVal = DeleteInfo.DeleteLeadInfo(LeadID, "sp_DeleteFreeApply");
                    break;
                case "Affiliate Signups":
                    retVal = DeleteInfo.DeleteAffiliateInfo(LeadID, "sp_DeleteAffiliate");
                    break;
            }//end switch
            return retVal;
        }//end function CreateAppExt

        //This function adds lead data to ACT
        public int AddLeadInfoToACT(int LeadID, string LeadType)
        {
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();
            ACTDataDL Act = new ACTDataDL();
            DataSet ds;
            DataSet dsExists;
            DataRow dr;
            switch (LeadType)  
            {
                case "Free Report":
                         ds = Leads.GetFreeReport(LeadID);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dr = ds.Tables[0].Rows[0];

                            //Check if name already exists in ACT
                            dsExists = Act.CheckLeadExists(dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim());
                            if (dsExists.Tables[0].Rows.Count > 0)                            
                                return 2; //Lead already exists in ACT, return
                            else
                            {
                                //Check if email exists in ACT
                                bool retEmail = CheckEmailACT(dr["Email"].ToString().Trim());
                                if (retEmail)
                                    return 3;
                                else
                                {
                                    //Add the record to ACT!                                  
                                    bool retVal = Act.InsertLeadFreeReport(dr["CreateDate"].ToString().Trim(),
                                        dr["Contact"].ToString().Trim(),
                                        dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(),
                                        dr["ReferralSource"].ToString().Trim(), dr["Email"].ToString().Trim());
                                    Leads.UpdateAddToACTDate(LeadID, "FreeReport");
                                    return 1; //return success
                                }
                      
                             
                            }//end if check record exists
                         }//end if Free Report Lead found              
                        break;
                    case "Free Consult":
                        //Check if application has already been added to ACT!
                        ds = Leads.GetFreeConsult(LeadID);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dr = ds.Tables[0].Rows[0];
                  
                            dsExists = Act.CheckLeadExists(dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim());
                            if (dsExists.Tables[0].Rows.Count > 0)
                                return 2;
                            else                                                            
                            {
                                //Check if email exists
                                bool retEmail = CheckEmailACT(dr["Email"].ToString().Trim());
                                if (retEmail)
                                    return 3;  //return email exists  
                                else
                                {
                                    bool retPhone = CheckPhoneACT(dr["Phone"].ToString().Trim());
                                    if (retPhone)
                                        return 4;  //return phone exists   
                                    else
                                    {
                                        //Add the record to ACT!                                        
                                        bool retVal = Act.InsertLeadFreeConsult(dr["CreateDate"].ToString().Trim(),
                                        dr["Contact"].ToString().Trim(),
                                        dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(),
                                        dr["ReferralSource"].ToString().Trim(), dr["Email"].ToString().Trim(),
                                        dr["CountryCode"].ToString().Trim(),
                                        dr["Phone"].ToString().Trim(), dr["HomePhone"].ToString().Trim());
                                       
                                        Leads.UpdateAddToACTDate(LeadID, "FreeConsult");
     
                                        return 1;//return success
                                     
                                    }//end if phone not exists                                  
                                }
                                   
                            }//end if check Lead Exists                                                  
                        }//end if Free Report Lead Found
                        break;                                                                     
                    case "Free Apply":                                            
                        ds = Leads.GetFreeApply(LeadID);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dr = ds.Tables[0].Rows[0];
                            //Check if application has already been added to ACT! 
                            dsExists = Act.CheckLeadExists(dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim());
                            if (dsExists.Tables[0].Rows.Count > 0)
                                return 2;//return name exists
                            else
                            {
                                //Check if email exists
                               bool retEmail = CheckEmailACT(dr["Email"].ToString().Trim());
                               if (retEmail)                            
                                    return 3;//Return email exists  
                               else 
                               {
                                    bool retPhone = CheckPhoneACT(dr["Phone"].ToString().Trim());
                                    if (retPhone)
                                        return 4; //Return Phone Exists
                                    else
                                    {
                                        //Add the record to ACT!
                                        bool retVal = Act.InsertLeadFreeApply(dr["CreateDate"].ToString().Trim(),
                                        dr["Contact"].ToString().Trim(),
                                        dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(),
                                        dr["ReferralSource"].ToString().Trim(), dr["Email"].ToString().Trim(),
                                        dr["Phone"].ToString().Trim(), dr["CountryCodeHome"].ToString().Trim(), 
                                        dr["HomePhone"].ToString().Trim(),
                                        dr["MobilePhone"].ToString().Trim(), dr["Company"].ToString().Trim(),
                                        dr["URL"].ToString().Trim(), dr["Address"].ToString().Trim(),
                                        dr["City"].ToString().Trim(), dr["State"].ToString().Trim(),
                                        dr["Zip"].ToString().Trim(), dr["Country"].ToString().Trim(),
                                        dr["Comments"].ToString().Trim(), dr["cart"].ToString().Trim());
                                        Leads.UpdateAddToACTDate(LeadID, "FreeApply");

                                        return 1; //return success
                                    }//end if phone not exists      
                                    
                                }//end if email exists               
                            }
                                                        
                       }//end if Lead is Found
                       break;
                }//end switch
            return 1;
        }//end function AddLeadInfoToACT

        public int CreateNewActRecordFromLeadReport(int LeadID, string LeadType)
        {
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();
            ACTDataDL Act = new ACTDataDL();
            DataSet ds = Leads.GetFreeReport(LeadID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                //Add the record to ACT!      
                bool retVal = Act.InsertLeadFreeReport(dr["CreateDate"].ToString().Trim(),
                    dr["Contact"].ToString().Trim(),
                    dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(),
                    dr["ReferralSource"].ToString().Trim(), dr["Email"].ToString().Trim());
                Leads.UpdateAddToACTDate(LeadID, "FreeReport");
                if (retVal)
                    return 1;
                else
                    return 0;
            }//end if count not 0
            return 1;
        }//end CreateNewActRecordFromLeadReport

        public int CreateNewActRecordFromLeadConsult(int LeadID, string LeadType)
        {
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();
            ACTDataDL Act = new ACTDataDL();
            DataSet ds = Leads.GetFreeConsult(LeadID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //Add the record to ACT!
                bool retVal = Act.InsertLeadFreeConsult(dr["CreateDate"].ToString().Trim(),
                    dr["Contact"].ToString().Trim(),
                    dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(),
                    dr["ReferralSource"].ToString().Trim(), dr["Email"].ToString().Trim(),
                    dr["CountryCode"].ToString().Trim(), dr["Phone"].ToString().Trim(), dr["HomePhone"].ToString().Trim());
                Leads.UpdateAddToACTDate(LeadID, "FreeConsult");
                if (retVal)
                    return 1;
                else
                    return 0;
            }//end if count not 0
            return 1;
        }//end CreateNewActRecordFromLeadConsult
      
        public int CreateNewActRecordFromLeadApply(int LeadID, string LeadType)
        {
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();
            ACTDataDL Act = new ACTDataDL();
            DataSet ds = Leads.GetFreeApply(LeadID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //Add the record to ACT!

                bool retVal = Act.InsertLeadFreeApply(dr["CreateDate"].ToString().Trim(),
                                         dr["Contact"].ToString().Trim(),
                                         dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(),
                                         dr["ReferralSource"].ToString().Trim(), dr["Email"].ToString().Trim(),
                                         dr["Phone"].ToString().Trim(), dr["CountryCodeHome"].ToString().Trim(), dr["HomePhone"].ToString().Trim(),
                                         dr["MobilePhone"].ToString().Trim(), dr["Company"].ToString().Trim(),
                                         dr["URL"].ToString().Trim(), dr["Address"].ToString().Trim(),
                                         dr["City"].ToString().Trim(), dr["State"].ToString().Trim(),
                                         dr["Zip"].ToString().Trim(), dr["Country"].ToString().Trim(),
                                         dr["Comments"].ToString().Trim(), dr["cart"].ToString().Trim());
                Leads.UpdateAddToACTDate(LeadID, "FreeApply");
                if (retVal)
                    return 1;
                else
                    return 0;
            }//end if count not 0
            return 1;
        }//end CreateNewActRecordFromLeadApply

        public int MergeAppFromLeadApply(int LeadID, int AppID)
        {
            FirstAffiliatesLeadsDL Leads = new FirstAffiliatesLeadsDL();
            ACTDataDL Act = new ACTDataDL();
            OnlineAppClassLibrary.NewAppInfo App = new OnlineAppClassLibrary.NewAppInfo(AppID);
            //Check if the App ID exists
            DataSet dsApp = App.GetNewAppData();
            if (dsApp.Tables[0].Rows.Count == 0)
                return 2;
            OnlineAppClassLibrary.CompanyInfo Company = new OnlineAppClassLibrary.CompanyInfo(AppID);
            DataSet ds = Leads.GetFreeApply(LeadID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //Update the App
                bool retVal = Company.UpdateCompanyInfo(dr["Company"].ToString().Trim(), "", dr["Company"].ToString().Trim(),
                                           dr["Address"].ToString().Trim(), "", dr["City"].ToString().Trim(),
                                           dr["State"].ToString().Trim(), "", dr["Zip"].ToString().Trim(),
                                           dr["Country"].ToString().Trim(), "", "", "", dr["Phone"].ToString().Trim(),
                                           "", "", dr["URL"].ToString().Trim(), false);
                if (!retVal)
                    return 0; //Update error occured
            }//end if count not 0
            return 1;
        }//end MergeAppFromLeadApply

        //This function checks if email exists in ACT before the record is added
        public bool CheckEmailACT(string Email)
        {
            bool retVal = false;
            ACTDataDL CheckEmail = new ACTDataDL();
            retVal = CheckEmail.CheckEmailExists(Email);
            return retVal;
        }//end CheckEmailACT

        //This function checks if email exists in ACT before the record is added
        public bool CheckPhoneACT(string Phone)
        {
            bool retVal = false;
            ACTDataDL CheckPhone = new ACTDataDL();
            retVal = CheckPhone.CheckPhoneExists(Phone);
            return retVal;
        }//end CheckPhoneACT

        //This function gets affiliate info including bank information for adding affiliate leads to ACT
        public DataSet GetAffiliateActInfo(int LeadID)
        {
            FirstAffiliatesLeadsDL GetAff = new FirstAffiliatesLeadsDL();
            DataSet ds = GetAff.GetAffiliateData(LeadID);
            return ds;            
        }//end function GetAffiliateActInfo

        //This function adds affiliate information from AffiliateWiz to ACT
        public int AddAffiliateInfoToACT(int LeadID)
        {
            DataSet ds = GetAffiliateActInfo(LeadID);
            int iRetVal = 5;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string BankName = string.Empty;
                if (dr["BankName"].ToString().Trim().ToLower().Contains("other"))
                    BankName = dr["OtherBank"].ToString().Trim();
                else
                    BankName = dr["BankName"].ToString().Trim();
                ACTDataDL ACT = new ACTDataDL();                
                iRetVal = ACT.InsertAffiliate(Convert.ToInt32(dr["AffiliateID"].ToString().Trim()), dr["CreateDate"].ToString().Trim(),
                    dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(), dr["AffiliateReferral"].ToString().Trim(),
                    dr["OtherReferral"].ToString().Trim(), dr["CompanyName"].ToString().Trim(), dr["DBA"].ToString().Trim(), dr["LegalStatus"].ToString().Trim(), dr["WebsiteURL"].ToString().Trim(),
                    dr["TaxID"].ToString().Trim(), dr["SocialSecurity"].ToString().Trim(), dr["Category"].ToString().Trim(),
                    dr["Email"].ToString().Trim(), dr["CompanyAddress"].ToString().Trim(), dr["City"].ToString().Trim(), dr["State"].ToString().Trim(),
                    dr["Zip"].ToString().Trim(), dr["Country"].ToString().Trim(),
                    dr["MailingAddress"].ToString().Trim(), dr["MailingCity"].ToString().Trim(),
                    dr["MailingState"].ToString().Trim(), dr["MailingZip"].ToString().Trim(), dr["MailingCountry"].ToString().Trim(), 
                    dr["Telephone"].ToString().Trim(),
                    dr["HomePhone"].ToString().Trim(), dr["MobilePhone"].ToString().Trim(), dr["Fax"].ToString().Trim(), dr["Comments"].ToString().Trim(), BankName,
                    dr["BankAddress"].ToString().Trim(), dr["BankCity"].ToString().Trim(), dr["BankState"].ToString().Trim(), 
                    dr["BankZip"].ToString().Trim(), dr["BankPhone"].ToString().Trim(), 
                    dr["BankRoutingNumber"].ToString().Trim(), dr["BankAccountNumber"].ToString().Trim());

                FirstAffiliatesLeadsDL Aff = new FirstAffiliatesLeadsDL();
                bool retVal = Aff.UpdateLastSync(LeadID);

            }//end if count not 0
            return iRetVal;
        }//end function AddAffiliateInfoToACT 

        //This function updates affiliate information to ACT
        public int UpdateAffiliateInfoInACT(int LeadID, int partnerID)
        {
            DataSet ds = GetAffiliateActInfo(LeadID);
            int iRetVal = 3;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string BankName = string.Empty;
                if (dr["BankName"].ToString().Trim().ToLower().Contains("other"))
                    BankName = dr["OtherBank"].ToString().Trim();
                else
                    BankName = dr["BankName"].ToString().Trim();                
                
                ACTDataDL ACT = new ACTDataDL();

                PartnerLogBL LogData = new PartnerLogBL();
                //string PortalUserID = LogData.ReturnPortalUserID(Convert.ToInt32(dr["AffiliateID"]));
                //int partnerID = Convert.ToInt32(dr["AffiliateID"]);
                
                string ContactID = ACT.ReturnContactID(dr["AffiliateID"].ToString().Trim());
                if (ContactID != "")
                    ACT.InsertActRecordBackup(ContactID);
                else
                    return 1;

                iRetVal = ACT.UpdateAffiliate(Convert.ToInt32(dr["AffiliateID"].ToString()), dr["CreateDate"].ToString(), dr["FirstName"].ToString().Trim(),
                     dr["LastName"].ToString().Trim(), dr["AffiliateReferral"].ToString().Trim(), dr["OtherReferral"].ToString().Trim(),                     
                    dr["CompanyName"].ToString().Trim(), dr["DBA"].ToString().Trim(), dr["LegalStatus"].ToString().Trim(), 
                    dr["WebsiteURL"].ToString().Trim(), dr["TaxID"].ToString().Trim(), dr["SocialSecurity"].ToString().Trim(),
                    dr["Email"].ToString().Trim(), dr["CompanyAddress"].ToString().Trim(), dr["City"].ToString().Trim(), dr["State"].ToString().Trim(),
                    dr["Zip"].ToString().Trim(), dr["Country"].ToString().Trim(), dr["Telephone"].ToString().Trim(),
                    dr["HomePhone"].ToString().Trim(), dr["MobilePhone"].ToString().Trim(), dr["Fax"].ToString().Trim(), dr["Comments"].ToString().Trim(),
                    dr["MailingAddress"].ToString().Trim(), dr["MailingCity"].ToString().Trim(),
                    dr["MailingState"].ToString().Trim(), dr["MailingZip"].ToString().Trim(), dr["MailingCountry"].ToString().Trim(), BankName ,
                    dr["BankAddress"].ToString().Trim(), dr["BankCity"].ToString().Trim(), dr["BankState"].ToString().Trim(), 
                    dr["BankZip"].ToString().Trim(), dr["BankPhone"].ToString().Trim(), 
                    dr["BankRoutingNumber"].ToString().Trim(), dr["BankAccountNumber"].ToString().Trim());

                //Check to see which fields were changed to record histories                
                //Compare the two entries in the tables (via a View) to see which fields will record histories

                //Get the Preupdated Contact Information
                DataSet dsPostUpdate = ACT.GetActRecord(ContactID);
                DataSet dsPreUpdate = ACT.GetActRecordBackup(ContactID);

                //number of columns in the Act Record View
                int colCount = dsPreUpdate.Tables[0].Columns.Count;

                DataRow drPreUpdate = dsPreUpdate.Tables[0].Rows[0];
                DataRow drPostUpdate = dsPostUpdate.Tables[0].Rows[0];
                String PrevValue, ColNamePre, NewValue, ColNamePost;

                //Loop through every column in the Pre-updated and Post Act Record
                for (int i = 0; i < colCount; i++)
                {
                    DataColumn dcPre = dsPreUpdate.Tables[0].Columns[i];
                    DataColumn dcPost = dsPostUpdate.Tables[0].Columns[i];
                    ColNamePre = dcPre.ColumnName;
                    ColNamePost = dcPost.ColumnName;

                    //If the column NAMES in the data sets are the same
                    if (ColNamePre == ColNamePost)
                    {
                        PrevValue = drPreUpdate[i].ToString();
                        NewValue = drPostUpdate[i].ToString();
                        //if the fields do not contain the same VALUE
                        if (PrevValue.ToLower() != NewValue.ToLower())
                        {
                            //Record a Field Change History in ACT
                            ACT.InsertHistoryFieldChange(ContactID, ColNamePost, PrevValue, NewValue, partnerID);
                        }
                    }
                    //Delete the data for this Contact in the Backup Table created in the Update
                    ACT.DeleteActBackup(ContactID);
                }//end for
            }//end if count not 0

            if (iRetVal == 0)
            {
                //update Rep Info dropdowns in Act Status tab
                RepInfoDL RepInfo = new RepInfoDL();
                DataSet dsRepInfo = RepInfo.GetRepInfoByAffiliateID(Convert.ToString(LeadID).ToString().Trim());
                if (dsRepInfo.Tables[0].Rows.Count > 0)
                {
                    DataRow drRepInfo = dsRepInfo.Tables[0].Rows[0];
                    //Add rep info to ACT dropdown
                    ACTDataDL Act = new ACTDataDL();
                    bool retVal = Act.InsertRepInfoInACT(drRepInfo["RepName"].ToString().Trim(), drRepInfo["SageNum"].ToString().Trim(), drRepInfo["IPSNum"].ToString().Trim(),
                        drRepInfo["IPay3Num"].ToString().Trim(), drRepInfo["IMS2Num"].ToString().Trim(), drRepInfo["ChaseNum"].ToString().Trim(),
                        drRepInfo["MasterNum"].ToString().Trim());
                }
            }
            return iRetVal;
        }//end function UpdateAffiliateInfoInACT 

        public bool UpdateLastSyncForAffiliates(int LeadID)
        {
            FirstAffiliatesLeadsDL Aff = new FirstAffiliatesLeadsDL();
            bool retVal = Aff.UpdateLastSync(LeadID);
            return retVal;
        }//end function UpdateLastSyncForAffiliates

        public int CreateNewActRecordFromAffiliate(int LeadID)
        {
            DataSet ds = GetAffiliateActInfo(LeadID);
            int iRetVal = 5;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string BankName = string.Empty;
                if (dr["BankName"].ToString().Trim().ToLower().Contains("other"))
                    BankName = dr["OtherBank"].ToString().Trim();
                else
                    BankName = dr["BankName"].ToString().Trim();
                ACTDataDL ACT = new ACTDataDL();
                iRetVal = ACT.InsertNewAffiliateOnConfirm(LeadID, dr["CreateDate"].ToString().Trim(),
                    dr["FirstName"].ToString().Trim(), dr["LastName"].ToString().Trim(), dr["AffiliateReferral"].ToString().Trim(), dr["OtherReferral"].ToString().Trim(),
                    dr["CompanyName"].ToString().Trim(), dr["DBA"].ToString().Trim(), dr["LegalStatus"].ToString().Trim(), dr["WebsiteURL"].ToString().Trim(),
                    dr["TaxID"].ToString().Trim(), dr["SocialSecurity"].ToString().Trim(), dr["Category"].ToString().Trim(),
                    dr["Email"].ToString().Trim(), dr["CompanyAddress"].ToString().Trim(), dr["City"].ToString().Trim(), dr["State"].ToString().Trim(),
                    dr["Zip"].ToString().Trim(), dr["Country"].ToString().Trim(), dr["MailingAddress"].ToString().Trim(), dr["MailingCity"].ToString().Trim(),
                    dr["MailingState"].ToString().Trim(), dr["MailingZip"].ToString().Trim(), dr["MailingCountry"].ToString().Trim(), dr["Telephone"].ToString().Trim(),
                    dr["HomePhone"].ToString().Trim(), dr["MobilePhone"].ToString().Trim(), dr["Fax"].ToString().Trim(), dr["Comments"].ToString().Trim(), BankName,
                    dr["BankAddress"].ToString().Trim(), dr["BankCity"].ToString().Trim(), dr["BankState"].ToString().Trim(),
                    dr["BankZip"].ToString().Trim(), dr["BankPhone"].ToString().Trim(),
                    dr["BankRoutingNumber"].ToString().Trim(), dr["BankAccountNumber"].ToString().Trim());

            }//end if count not 0
            return iRetVal;
        }//end function CreateNewActRecordFromAffiliate

    }//end class FirstAffiliatesLeadsBL
}
