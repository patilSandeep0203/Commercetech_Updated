using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class ACTDataBL
    {
        private ACTAuthnetExcelTableAdapter _AuthnetExcelAdapter = null;
        protected ACTAuthnetExcelTableAdapter AuthnetExcelAdapter
        {
            get
            {
                if (_AuthnetExcelAdapter == null)
                    _AuthnetExcelAdapter = new ACTAuthnetExcelTableAdapter();

                return _AuthnetExcelAdapter;
            }
        }

        private ACTAuthnetPlatformTableAdapter _AuthnetPlatformAdapter = null;
        protected ACTAuthnetPlatformTableAdapter AuthnetPlatformAdapter
        {
            get
            {
                if (_AuthnetPlatformAdapter == null)
                    _AuthnetPlatformAdapter = new ACTAuthnetPlatformTableAdapter();

                return _AuthnetPlatformAdapter;
            }
        }


        //CALLED BY UploadGateway.aspx
        public PartnerDS.ACTAuthnetExcelDataTable GetAuthnetExcel(System.Guid ContactID)
        {
            return AuthnetExcelAdapter.GetData(ContactID);
        }//end function GetAuthnetExcel

        //CALLED BY UploadGateway.aspx
        public PartnerDS.ACTAuthnetPlatformDataTable  GetAuthnetPlatform(System.Guid ContactID)
        {
            return AuthnetPlatformAdapter.GetData(ContactID);
        }//end function GetAuthnetPlatform

        //CALLED BY OnlineAppMgmt/default.aspx
        public string ReturnActEditDate(int AppId)
        {
            ACTDataDL ACTRecord = new ACTDataDL();
            string ActEditDate = "";
            DataSet ds = ACTRecord.GetACTLastEditDate(AppId);
            if (ds.Tables[0].Rows.Count == 1)
                ActEditDate = ds.Tables[0].Rows[0][0].ToString();
            else if (ds.Tables[0].Rows.Count > 1)
                ActEditDate = "Multiple";            
            return ActEditDate;
        }//end function GetActEditDate

        //CALLED BY UploadGateway.aspx
        public string ReturnCustomerFilePath(string ContactID)
        {
            string FilePath = string.Empty;
            string strQuery = "SELECT FilePath FROM tbl_Contact WHERE ContactID=@ContactID";
            ACTDataDL ACTRecord = new ACTDataDL();
            DataSet ds = ACTRecord.GetACTInfoSQL(ContactID, strQuery);
            if (ds.Tables[0].Rows.Count > 0)
                FilePath = ds.Tables[0].Rows[0][0].ToString().Trim();
            return FilePath;
        }//end function ReturnCustomerFilePath


        //CALLED BY Edit.aspx
        public bool CheckAppIDExists(int AppId)
        {
            ACTDataDL Check = new ACTDataDL();
            DataSet dsAccess = Check.CheckAppIDExists(AppId);
            bool retVal = false;
            if (dsAccess.Tables[0].Rows.Count > 0)
            {
                retVal = true;
            }
            return retVal;
        }//end function CheckAppID

        //CALLED BY Edit.aspx
        public DataSet GetOtherReferralList()
        {
            string strQuery = "SELECT * FROM VW_NonAffiliateList";
            ACTDataDL ACTRecord = new ACTDataDL();
            DataSet ds = ACTRecord.GetACTInfoSQL(strQuery);
            return ds;
        }//end function GetOtherReferralList


        //This function Uploads commissions info from ACT to Partner Portal
        public string UploadCommissions(string Month)
        {

            int ReferralID = 0;
            //Delete Existing commissions data for the current month
            CommissionsDL Comm = new CommissionsDL();
            Comm.DeleteCommissions(Month);

            string Processor;

            //Get Commissions data from ACT
            ACTDataDL ACT = new ACTDataDL();
            DataSet ds = ACT.GetCommissionsFromACT();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = null;
                int iRetval = 0;
                int i = 0;

                

                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //Insert Commissions data in Partner Portal DB
                    dr = ds.Tables[0].Rows[i];

                    Processor = Convert.ToString(dr["Processor"]);

                    if (!Convert.IsDBNull(dr["ReferralID"]))
                    {
                        if (Convert.ToString(dr["ReferralID"]) != "")
                        {
                            ReferralID = Convert.ToInt32(dr["ReferralID"]);
                        }
                    }
                    //Get partner Category
                    string strReferalCategory = Comm.GetPartnerType(ReferralID);


                    // check to ensure that the total = price * quantity since Adjusted Price in ACT Sales Opp
                    // might be changed by Sales Rep
                    double price = Convert.ToDouble(dr["Price"].ToString().Trim());
                    double quantity = Convert.ToDouble(dr["Units"].ToString().Trim());
                    double subtotal = Math.Round((price * quantity), 2);
                    double total = Math.Round(Convert.ToDouble(dr["Total"].ToString().Trim()), 2);
                    if (subtotal != total)
                    {
                        //Delete commissions uploaded
                        Comm.DeleteCommissions(Month);
                        return "Error Uploading Commissions - Recheck " + dr["Product"].ToString().Trim()
                            + " Sales Opportunity for " + dr["Company"].ToString().Trim() + ". Total should equal Price x Quantity.";
                    }
                    else
                    {
                        iRetval = Comm.InsertCommissions(
                            Month,
                            dr["RepName"].ToString().Trim(),
                            dr["Company"].ToString().Trim(),
                            dr["DBA"].ToString().Trim(),
                            dr["MerchantID"].ToString().Trim(),
                            dr["ReferralID"].ToString().Trim(),
                            dr["NonAffiliateReferral"].ToString().Trim(),
                            dr["Product"].ToString().Trim(),
                            dr["ProductCode"].ToString().Trim(),
                            dr["COG"].ToString().Trim(),
                            dr["Price"].ToString().Trim(),
                            dr["Units"].ToString().Trim(), 
                            dr["ActualCloseDate"].ToString().Trim(),
                            strReferalCategory,
                            Processor);

                        int iRetAgRef = Comm.UpdateAgentReferal(Month,strReferalCategory);
                        if (iRetval == 0)
                            return "Error Uploading Commission - " + dr["Product"].ToString().Trim()
                            + " Sales Opportunity for " + dr["Company"].ToString().Trim() + ".";
                    }                    
                }//end for
                
                //Reset Commission %
                CommissionsDL Reset = new CommissionsDL();
                Reset.ResetCommissions("ALL", Month);

                //Encrypt Merchant ID
                CommissionsDL EncrID = new CommissionsDL();
                int iRetVal = EncrID.EncryptCommissions(Month);
                if (iRetVal <= 0)
                    return "Error Encrypting MerchantIDs";

                //Update Funded Values  


                
                    CommissionsDL Funded = new CommissionsDL();
                    Funded.UpdateCommissionValues(Month);
                    Funded.UpdateCommBeforeJan2012();
                /*
                CommissionsBL Comm = new CommissionsBL();
                PartnerDS.CommissionsDataTable dt = new PartnerDS.CommissionsDataTable();


                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {
                        
                    }
                }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drIntuit = null;
                        int iRetval1 = 0;
                        int i1 = 0;

                        

                        for (i1 = 0; i1 < ds.Tables[0].Rows.Count; i++)
                        {
                            string Processor;

                            drIntuit = ds.Tables[0].Rows[i];

                            Processor = Convert.ToString(dr["Processor"]);

                            if (Processor.ToLower().Contains("intuit"))
                            { 
                                UpdateCommissionIntuitValues
                            }
                        }
                    }*/
                
                if (iRetVal <= 0)
                    return "Error Updating Commissions Funds";

                //Return Success Message
                return "Successfully Uploaded " + i.ToString() + " Records";
            }//end if count not 0
            return "Cannot find Commissions data in ACT for the Month of " + Month;
        }//end function InsertCommissions

        //This function returns sales opps from ACT! CALLED BY CTCSalesOpps.aspx
        public DataSet GetACTSalesOpps(string RepNum, string Month, string Year, string Status)        
        {
            ACTDataDL Act = new ACTDataDL();
            DataSet dsSalesOpps = Act.GetACTSalesOpps(RepNum, Month, Year, Status);
            return dsSalesOpps;
        }//end function GetSalesOpps

        public DataSet GetList(string ListName)
        {
            ACTDataDL CommonInfo = new ACTDataDL();
            DataSet dsCommon = CommonInfo.GetList(ListName);
            return dsCommon;
        }

    }//end class ACTDataBL
}
