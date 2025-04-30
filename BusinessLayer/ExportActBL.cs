using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using OnlineAppClassLibrary;
using DLPartner.PartnerDSTableAdapters;
using System.Web.Security;
using System.Security.Cryptography;
using DataLayer;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Xml;
using System.IO;

namespace BusinessLayer
{
    public class ExportActBL
    {
        private string DiscountPaid = "Monthly";
        private string ComplianceFee = "";
        PartnerLogBL LogData = new PartnerLogBL();
        //CALLED BY ExportAct.aspx
        public DataSet GetSummaryDataFromAct(string Email)
        {
            ACTDataDL ACT = new ACTDataDL();
            DataSet ds = ACT.GetSummaryData(Email);
            return ds;
        }//end function GetDataFromAct

        //This function generates a hash to encrypt the password
        protected string GenerateHash(string salt)
        {
            string strPassword = "Succeed1"; // Set default password for all online apps
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword.Trim() + salt, "sha1");
            return hash;
        }

        //This function gets a salt to encrypt the password which can be stored in the database
        protected string GetSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[5];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        #region EXPORT DATA

        //Check If LoginName Exists
        public bool CheckLoginNameExists(string LoginName)
        {
            bool bRetVal = false;
            OnlineAppDL NewApp = new OnlineAppDL();
            DataSet ds = NewApp.CheckLoginNameExists(LoginName);
            if (ds.Tables[0].Rows.Count > 0)
                bRetVal = true;
            return bRetVal;
        }//end function CheckIfLoginNameExists

        public DataSet GetACTEditDate(string ContactID, DateTime OnlineAppEditDate)
        {
            ACTDataDL ACTData = new ACTDataDL();
            DataSet ds = ACTData.GetACTEditDate(ContactID, OnlineAppEditDate);
            return ds;

        }

        //Used to Export a record from ACT to the Online App. CALLED BY ExportACT.aspx
        public string ExportData(string ContactID, string NewLoginID)
        {
            try
            {
                ACTDataDL ACT = new ACTDataDL();
                ACTOnlineAppFieldsTableAdapter ACTOnlineAppFieldsAdapter = new ACTOnlineAppFieldsTableAdapter();
                PartnerDS.ACTOnlineAppFieldsDataTable dt = ACTOnlineAppFieldsAdapter.GetData(new Guid(ContactID));
                 //Get the Data from the ACT record
               
                if (dt.Rows.Count > 0)
                {
                    string Email = Convert.ToString(dt[0].EMail).Trim();
                    string FirstName = Convert.ToString(dt[0].FIRSTNAME).Trim();
                    string LastName = Convert.ToString(dt[0].LASTNAME).Trim();
                    //string ContactFirstName = dt[0].ContactFIRSTNAME;
                    //string ContactLastName = dt[0].ContactLASTNAME;
                    string Phone = Convert.ToString(dt[0].Phone).Trim();
                    string OtherReferral = Convert.ToString(dt[0].OtherReferral).Trim();
                    int ReferralID = 0;
                    if (dt[0].ReferralID != "")
                        ReferralID = Convert.ToInt32(dt[0].ReferralID);

                    int PID = 0;

                    //Set up login ID
                    string strLogin = "";
                    bool retLoginExist = false;
                    if (NewLoginID == "0")
                    {
                        //Use email as loginID
                        if (Convert.ToString(dt[0].EMail).Trim() != "")
                            strLogin = Convert.ToString(dt[0].EMail).Trim(); //set Login to Email address
                    }
                    else
                    {
                        strLogin = NewLoginID.ToString().Trim();
                        retLoginExist = CheckLoginNameExists(strLogin);
                    }

                    if (retLoginExist)
                    {
                        return "This email address already exists. Please eneter another email address.";
                    }
                    else
                    {

                        string status = "INCOMPLETE";
                        string statusGW = "INCOMPLETE"; 
                        //Insert Data in OnlineAppNewApp  
                        //First get repnum based on Rep Name in the ACT Record
                        RepInfoDL Rep = new RepInfoDL();
                        string strRepNumber = Rep.ReturnMasterNum(dt[0].RepName);
                        OnlineAppDL OnlineApp = new OnlineAppDL();
                        if (!Convert.IsDBNull(dt[0].MerchantStatus))
                        {
                            if (Convert.ToString(dt[0].MerchantStatus) != "")
                            {
                                if (Convert.ToString(dt[0].MerchantStatus) != " ")
                                {
                                    status = Convert.ToString(dt[0].MerchantStatus);
                                }
                            }
                        }

                        if (!Convert.IsDBNull(dt[0].StatusGW))
                        {
                            if (Convert.ToString(dt[0].StatusGW) != "")
                            {
                                if (Convert.ToString(dt[0].StatusGW) != " ")
                                {
                                    statusGW = Convert.ToString(dt[0].StatusGW);
                                }
                            }
                        }

                        //Insert the new app data in the agent portal and return the new AppId
                        int NewAppId = OnlineApp.InsertNewAppInfoFromACT(PID, ReferralID, Convert.ToString(strRepNumber).Trim(), Convert.ToString(status).Trim(), Convert.ToString(statusGW).Trim(),
                            Convert.ToByte(dt[0].OnlineDebit), Convert.ToByte(dt[0].CheckServices),
                            Convert.ToByte(dt[0].Giftcard), Convert.ToByte(dt[0].EBT), Convert.ToByte(dt[0].Wireless),
                            Convert.ToByte(dt[0].Payroll), Convert.ToByte(dt[0].Lease), dt[0].OtherReferral);

                        //Insert Data in OnlineAppAccess

                        string strPassword = "";//password set in GenerateHash(salt);
                        string salt = GetSalt();
                        string hash = GenerateHash(salt);
                        OnlineAppDL Insert = new OnlineAppDL();
                        bool retValAccess = Insert.InsertLoginNamePassword(Convert.ToString(strLogin).Trim(), strPassword, hash, salt, NewAppId);
                        if (!retValAccess)
                            return "Could not insert data in OnlineAppAccess.";

                        bool retVal;

                        //Insert record in OnlineAppProfile
                        OnlineAppClassLibrary.OnlineAppProfile Profile = new OnlineAppClassLibrary.OnlineAppProfile(NewAppId);
                        if (!(Convert.IsDBNull(dt[0].AcctType)) && !(Convert.ToString(dt[0].AcctType) == ""))
                        {
                            retVal = Profile.IUProfile(Convert.ToString(FirstName).Trim(), Convert.ToString(LastName).Trim(), Convert.ToString(Email).Trim(), Convert.ToString(dt[0].ContactTitle).Trim(), Convert.ToString(Phone).Trim(), Convert.ToString(dt[0].BusinessPhoneExt).Trim(), "", Convert.ToString(dt[0].ContactMobilePhone).Trim(), Convert.ToInt32(dt[0].AcctType));
                            if (!retVal)
                                return "Could not insert data in OnlineAppProfile.";
                        }
                        else
                        {
                            retVal = Profile.IUProfile(Convert.ToString(FirstName).Trim(), Convert.ToString(LastName).Trim(), Convert.ToString(Email).Trim(), Convert.ToString(dt[0].P1Title).Trim(), Convert.ToString(Phone).Trim(), Convert.ToString(dt[0].BusinessPhoneExt).Trim(), "", Convert.ToString(dt[0].ContactMobilePhone).Trim(), 1);
                            if (!retVal)
                                return "Could not insert data in OnlineAppProfile.";
                        }
                        

                        //Insert data in CompanyInfo
                        OnlineAppClassLibrary.CompanyInfo Company = new OnlineAppClassLibrary.CompanyInfo(NewAppId);
                        retVal = Company.UpdateCompanyInfo(Convert.ToString(dt[0].Company).Trim(), Convert.ToString(dt[0].CustServPhone).Trim(),
                            Convert.ToString(dt[0].DBA).Trim(), Convert.ToString(dt[0].CompanyAddress).Trim(), Convert.ToString(dt[0].CompanyAddress2).Trim(),
                            Convert.ToString(dt[0].CompanyCity).Trim(), Convert.ToString(dt[0].CompanyState).Trim(), "",
                            Convert.ToString(dt[0].CompanyZip).Trim(), Convert.ToString(dt[0].CompanyCountry).Trim(), Convert.ToString(dt[0].YABL).Trim(), Convert.ToString(dt[0].MABL).Trim(),
                            Convert.ToString(dt[0].BusinessHours).Trim(), Convert.ToString(dt[0].BusinessPhone).Trim(), Convert.ToString(dt[0].BusinessPhoneExt).Trim(),
                            Convert.ToString(dt[0].BusinessFax).Trim(), Convert.ToString(dt[0].Website).Trim(), false);

                        if (!retVal)
                            return "Could not insert data in OnlineAppCompanyInfo.";

                        //Insert data in OnlineAppCardPCT
                        OnlineAppClassLibrary.CardPCT CardPCT = new OnlineAppClassLibrary.CardPCT(NewAppId);
                        retVal = CardPCT.UpdateCardPCT(Convert.ToString(dt[0].Retail).Trim(),
                            Convert.ToString(dt[0].Restaurant).Trim(), Convert.ToString(dt[0].MailOrder).Trim(), Convert.ToString(dt[0].Internet).Trim(),
                            Convert.ToString(dt[0].Swiped).Trim(), Convert.ToString(dt[0].KeyedWith).Trim(), Convert.ToString(dt[0].KeyedWithout).Trim(),
                            Convert.ToString(dt[0].Service).Trim(), Convert.ToString(dt[0].Others).Trim(), false);

                        if (!retVal)
                            return "Could not insert data in OnlineAppCardPCT.";

                        string processedBefore = Convert.ToString(dt[0].Processed);

                        if (!Convert.IsDBNull(dt[0].PrevProcessor))
                        { 
                            if (Convert.ToString(dt[0].PrevProcessor) != "")
                            {
                                processedBefore = "Yes";
                            }
                        }
                        //Insert data in OnlineAppBusinessInfo
                        OnlineAppClassLibrary.BusinessInfo Business = new OnlineAppClassLibrary.BusinessInfo(NewAppId);
                        Business.UpdateBusinessInfo(Convert.ToString(dt[0].BillingAddress).Trim(),
                            Convert.ToString(dt[0].BillingAddress2).Trim(), Convert.ToString(dt[0].BillingCity).Trim(), Convert.ToString(dt[0].BillingState).Trim(),
                            "", Convert.ToString(dt[0].BillingZip).Trim(), Convert.ToString(dt[0].BillingCountry).Trim(), Convert.ToString(dt[0].TaxID).Trim(),
                            Convert.ToString(dt[0].YearsInBusiness).Trim(), Convert.ToString(dt[0].MonthsinBusiness).Trim(), Convert.ToString(dt[0].NumberOfLocations).Trim(),
                            Convert.ToString(dt[0].TypeOwnership).Trim(), Convert.ToString(dt[0].TypeProduct).Trim(), Convert.ToString(dt[0].NumDaysDelivered).Trim(),
                            Convert.ToString(dt[0].AddlComments).Trim(), Convert.ToString(dt[0].RefundID).Trim(), Convert.ToString(dt[0].OtherRefund).Trim(),
                            Convert.ToString(dt[0].FiledBankruptcy).Trim(), Convert.ToString(processedBefore).Trim(), Convert.ToString(dt[0].PrevProcessor).Trim(), "",
                            Convert.ToString(dt[0].PrevMerchantAcctNo).Trim(), 0, Convert.ToString(dt[0].ReasonForLeaving).Trim(), Convert.ToString(dt[0].Terminated).Trim(), false);

                        if (!retVal)
                            return "Could not insert data in OnlineAppBusinessInfo.";

                        //Insert data in OnlineAppPrincipalInfo
                        OnlineAppClassLibrary.PrincipalInfo Principal1 = new OnlineAppClassLibrary.PrincipalInfo(NewAppId);
                        string hasSecondPrincipal = "No";
                        if (Convert.ToString(dt[0].P1OwnershipPercent).Trim() != "100")
                            hasSecondPrincipal = "Yes";
                        retVal = Principal1.UpdatePrincipal1Info(Convert.ToString(dt[0].P1FirstName).Trim(),
                            Convert.ToString(dt[0].P1LastName).Trim(), Convert.ToString(dt[0].P1MidName).Trim(), Convert.ToString(dt[0].P1Email).Trim(), Convert.ToString(dt[0].P1Title).Trim(),
                            Convert.ToString(dt[0].P1Address).Trim(), "", Convert.ToString(dt[0].P1State).Trim(), Convert.ToString(dt[0].P1City).Trim(),
                            Convert.ToString(dt[0].P1ZipCode).Trim(), "", Convert.ToString(dt[0].P1Country).Trim(), Convert.ToString(dt[0].P1YearsAtAddress).Trim(), "",
                            Convert.ToString(dt[0].P1PhoneNumber).Trim(), Convert.ToString(dt[0].P1MobilePhone).Trim(), Convert.ToString(dt[0].P1DriversLicenseNo).Trim(), Convert.ToString(dt[0].P1DriversLicenseState).Trim(),
                            Convert.ToString(dt[0].P1DriversLicenseExpiry).Trim(), Convert.ToString(dt[0].P1DOB).Trim(), Convert.ToString(dt[0].P1LivingStatus).Trim(), Convert.ToString(dt[0].P1OwnershipPercent).Trim(),
                            Convert.ToString(dt[0].P1SSN).Trim(), false, Convert.ToString(hasSecondPrincipal).Trim());
                        if (!retVal)
                            return "Could not insert data in OnlineAppPrincipalInfo.";

                        //Insert data in OnlineAppPrincipal2Info
                        if (Convert.ToString(dt[0].P1OwnershipPercent).Trim() != "100")
                        {
                            OnlineAppClassLibrary.Principal2Info Principal2 = new OnlineAppClassLibrary.Principal2Info(NewAppId);
                            retVal = Principal2.UpdatePrincipal2Info(Convert.ToString(dt[0].P2FirstName).Trim(),
                                Convert.ToString(dt[0].P2LastName).Trim(), "", Convert.ToString(dt[0].P2Email).Trim(), Convert.ToString(dt[0].P2Title).Trim(),
                                Convert.ToString(dt[0].P2Address).Trim(), "", Convert.ToString(dt[0].P2State).Trim(), Convert.ToString(dt[0].P2City).Trim(),
                                Convert.ToString(dt[0].P2ZipCode).Trim(), "", Convert.ToString(dt[0].P2Country).Trim(), Convert.ToString(dt[0].P2YearsAtAddress).Trim(), "",
                                Convert.ToString(dt[0].P2PhoneNumber).Trim(), Convert.ToString(dt[0].P2MobilePhone).Trim(), Convert.ToString(dt[0].P2DriversLicenseNo).Trim(), Convert.ToString(dt[0].P2DriversLicenseState).Trim(),
                                Convert.ToString(dt[0].P2DriversLicenseExpiry).Trim(), Convert.ToString(dt[0].P2DOB).Trim(), Convert.ToString(dt[0].P2LivingStatus).Trim(), Convert.ToString(dt[0].P2OwnershipPercent).Trim(),
                                Convert.ToString(dt[0].P2SSN).Trim(), false);

                            if (!retVal)
                                return "Could not insert data in OnlineAppPrincipal2Info.";
                        }

                        string CardPresent = "";
                        int SwipedPCT = 0;
                        if (dt[0].Swiped != "")
                            SwipedPCT = Convert.ToInt32(dt[0].Swiped);

                        if (SwipedPCT >= 70)
                            CardPresent = "CP";
                        else
                            CardPresent = "CNP";

                        if (!Convert.IsDBNull(dt[0].DiscountPaid))
                        {
                            if (Convert.ToString(dt[0].DiscountPaid).Trim() == "Monthly")
                            {
                                DiscountPaid = "Monthly";
                            }
                            else if (Convert.ToString(dt[0].DiscountPaid).Trim() == "Daily") 
                            {
                                DiscountPaid = "Daily";
                            }
                        }

                        ComplianceFee = Convert.ToString(dt[0].ComplianceFee).Trim();

                        //Insert data in OnlineAppProcessing only if a Processor is selected
                        if (Convert.ToString(dt[0].Processor).Trim() != "")
                        {
                            OnlineAppClassLibrary.ProcessingInfo Proc = new OnlineAppClassLibrary.ProcessingInfo(NewAppId);
                            retVal = Proc.UpdateProcessingInfo(Convert.ToString(dt[0].Processor).Trim(), Convert.ToString(CardPresent).Trim(),
                                Convert.ToString(dt[0].CustServFee).Trim(), Convert.ToString(dt[0].InternetStmt).Trim(), Convert.ToString(dt[0].TransactionFee).Trim(), Convert.ToString(dt[0].DRQualPres).Trim(),
                                Convert.ToString(dt[0].DRQualNP).Trim(), Convert.ToString(dt[0].DRMidQual).Trim(), Convert.ToString(dt[0].DRNonQual).Trim(),
                                Convert.ToString(dt[0].DRQualDebit).Trim(), Convert.ToString(dt[0].DebitQualNP).Trim(), Convert.ToString(dt[0].DebitMidQual).Trim(),
                    Convert.ToString(dt[0].DebitNonQual).Trim(), Convert.ToString(dt[0].AmexDiscQual).Trim(),Convert.ToString(dt[0].AmexDiscMidQual).Trim(), Convert.ToString(dt[0].AmexDiscNonQual).Trim(), Convert.ToString(dt[0].ChargebackFee).Trim(), Convert.ToString(dt[0].RetrievalFee).Trim(),
                                Convert.ToString(dt[0].VoiceAuth).Trim(), Convert.ToString(dt[0].BatchHeader).Trim(), Convert.ToString(dt[0].AVS).Trim(),
                                Convert.ToString(dt[0].MonMin).Trim(), Convert.ToString(dt[0].NBCTransFee).Trim(), Convert.ToString(dt[0].AnnualFee).Trim(),
                                Convert.ToString(dt[0].WirelessAccessFee).Trim(), Convert.ToString(dt[0].WirelessTransFee).Trim(),
                                "", "", Convert.ToString(dt[0].DebitMonFee).Trim(),
                                Convert.ToString(dt[0].DebitTransFee).Trim(), Convert.ToString(dt[0].CGMonFee).Trim(),
                                Convert.ToString(dt[0].CGTransFee).Trim(), Convert.ToString(dt[0].CGMonMin).Trim(),
                                Convert.ToString(dt[0].CGDiscRate).Trim(), Convert.ToString(dt[0].GCMonFee).Trim(),
                                Convert.ToString(dt[0].GCTransFee).Trim(), Convert.ToString(dt[0].EBTMonFee).Trim(),
                                Convert.ToString(dt[0].EBTTransFee).Trim(), Convert.ToString(DiscountPaid).Trim(), Convert.ToString(dt[0].ContractTerm).Trim(), Convert.ToString(ComplianceFee).Trim(), Convert.ToString(dt[0].PindebitDiscount).Trim());

                            NewAppTable newAppTable = new NewAppTable();
                            newAppTable.setRatesUpdatedBit(NewAppId, true);

                            if (!retVal)
                                return "Could not insert data in OnlineAppProcessing.";

                            retVal = Proc.InsertUpdateCheckServiceName(Convert.ToString(dt[0].CheckService).Trim());

                            if (!retVal)
                                return "Could not insert Check Service.";

                            OnlineAppProcessingBL Processing = new OnlineAppProcessingBL(NewAppId);
                            Processing.UpdateOtherProcessing(Convert.ToBoolean(dt[0].Interchange), Convert.ToBoolean(dt[0].BillingAssessments), Convert.ToString(dt[0].RollingReserve).Trim());
                            //Set the Last Sync Date since it is an exported App
                            Processing.UpdateLastSyncDate();
                            if (!retVal)
                                return "Could not insert other Processing data in OnlineAppProcessing.";
                        }

                        if (dt[0].Gateway.ToString().Trim() != "")
                        {
                            OnlineAppClassLibrary.Gateway Gateway = new OnlineAppClassLibrary.Gateway(NewAppId);
                            retVal = Gateway.UpdateGatewayInfo(
                                Convert.ToString(dt[0].Gateway).Trim(), Convert.ToString(dt[0].GatewayMonFee).Trim(),
                                "", Convert.ToString(dt[0].GatewayTransFee).Trim());

                            if (!retVal)
                                return "Could not insert data in OnlineAppGateway.";
                        }

                        //Insert data in OnlineAppBankingInfo
                        OnlineAppClassLibrary.BankingInfo Banking = new OnlineAppClassLibrary.BankingInfo(NewAppId);
                        retVal = Banking.UpdateBankingInfo(Convert.ToString(dt[0].BankName).Trim(), "",
                            Convert.ToString(dt[0].BankAddress).Trim(), Convert.ToString(dt[0].BankZip).Trim(), Convert.ToString(dt[0].BankCity).Trim(),
                            Convert.ToString(dt[0].BankState).Trim(), "", Convert.ToString(dt[0].CompanyCountry).Trim(), Convert.ToString(dt[0].Company).Trim(), Convert.ToString(dt[0].BankAccountNumber).Trim(),
                            Convert.ToString(dt[0].BankRoutingNumber).Trim(), Convert.ToString(dt[0].BankPhone).Trim(), false);
                        if (!retVal)
                            return "Could not insert data in OnlineAppBankingInfo.";

                        //Insert data in OnlineAppOtherInfo
                        string AmexApplied = "";
                        string AmexNum = "";
                        long result = 0;
                        if (Int64.TryParse(dt[0].AmexNum.ToString(), out result))
                        {
                            AmexNum = Convert.ToString(dt[0].AmexNum).Trim();
                            AmexApplied = "Yes - Existing";
                        }
                        else if ((Convert.ToString(dt[0].AmexNum).Trim().ToLower().Contains("yes")) || (Convert.ToString(dt[0].AmexNum).Trim().ToLower().Contains("submit")))
                        {
                            AmexApplied = "Yes";
                        }
                        else if ((Convert.ToString(dt[0].AmexNum).Trim().ToLower().Contains("opted")) || (Convert.ToString(dt[0].AmexNum).ToLower().Contains("declined")) || (Convert.ToString(dt[0].AmexNum).ToLower().Contains("cancelled")))
                        {
                            AmexApplied = "No";
                        }
                        else
                        {
                            AmexApplied = "";
                        }

                        string DiscApplied = "";
                        string DiscNum = "";
                        if (Int64.TryParse(Convert.ToString(dt[0].DiscoverNum), out result))
                        {
                            DiscNum = Convert.ToString(dt[0].DiscoverNum);
                            DiscApplied = "Yes - Existing";
                        }
                        else if ((Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("yes")) || (Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("submit")))
                            DiscApplied = "Yes";
                        else if ((Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("opted")) || (Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("declined")) || (Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("cancelled")) || (Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("International")) || (Convert.ToString(dt[0].DiscoverNum).ToLower().Contains("MAP")))
                            DiscApplied = "No";
                        else DiscApplied = "";

                        string JCBApplied = "";
                        string JCBNum = "";
                        if (Int64.TryParse(Convert.ToString(dt[0].JCBNum), out result))
                        {
                            JCBNum = Convert.ToString(dt[0].JCBNum);
                            JCBApplied = "Yes - Existing";
                        }
                        else if ((Convert.ToString(dt[0].JCBNum).ToLower().Contains("yes")) || (Convert.ToString(dt[0].JCBNum).ToLower().Contains("submit")))
                            JCBApplied = "Yes";
                        else if ((Convert.ToString(dt[0].JCBNum).ToLower().Contains("opted")) || (Convert.ToString(dt[0].JCBNum).ToLower().Contains("declined")) || (Convert.ToString(dt[0].JCBNum).ToLower().Contains("cancelled")))
                            JCBApplied = "No";
                        else JCBApplied = "";

                        OnlineAppClassLibrary.OtherInfo OtherInfo = new OnlineAppClassLibrary.OtherInfo(NewAppId);

                        if (!Convert.IsDBNull(dt[0].AvgTicket) && !Convert.IsDBNull(dt[0].MonVol) && !Convert.IsDBNull(dt[0].MaxTicket))
                        {
                            retVal = OtherInfo.UpdateOtherInfo(Convert.ToString(DiscApplied).Trim(), Convert.ToString(AmexApplied), Convert.ToString(JCBApplied), Convert.ToString(DiscNum).Trim(), Convert.ToString(AmexNum).Trim(), "", Convert.ToString(dt[0].MaxTicket).Trim(), Convert.ToString(dt[0].AvgTicket).Trim(), Convert.ToString(dt[0].MonVol).Trim(), false);
                        }
                        else if (Convert.IsDBNull(dt[0].AvgTicket) && !Convert.IsDBNull(dt[0].MonVol) && !Convert.IsDBNull(dt[0].MaxTicket))
                        {
                            retVal = OtherInfo.UpdateOtherInfo(Convert.ToString(DiscApplied), Convert.ToString(AmexApplied), Convert.ToString(JCBApplied), Convert.ToString(DiscNum).Trim(), Convert.ToString(AmexNum).Trim(), "", Convert.ToString(dt[0].MaxTicket).Trim(), "", Convert.ToString(dt[0].MonVol).Trim(), false);
                        }
                        else if (Convert.IsDBNull(dt[0].AvgTicket) && Convert.IsDBNull(dt[0].MonVol) && !Convert.IsDBNull(dt[0].MaxTicket))
                        {
                            retVal = OtherInfo.UpdateOtherInfo(Convert.ToString(DiscApplied), Convert.ToString(AmexApplied), Convert.ToString(JCBApplied), Convert.ToString(DiscNum).Trim(), Convert.ToString(AmexNum).Trim(), "", Convert.ToString(dt[0].MaxTicket).Trim(), "", "", false);
                        }
                        else if (!Convert.IsDBNull(dt[0].AvgTicket) && !Convert.IsDBNull(dt[0].MonVol) && Convert.IsDBNull(dt[0].MaxTicket))
                        {
                            retVal = OtherInfo.UpdateOtherInfo(Convert.ToString(DiscApplied), Convert.ToString(AmexApplied), Convert.ToString(JCBApplied), Convert.ToString(DiscNum).Trim(), Convert.ToString(AmexNum).Trim(), "", "", Convert.ToString(dt[0].AvgTicket).Trim(), Convert.ToString(dt[0].MonVol).Trim(), false);
                        }
                        else if (!Convert.IsDBNull(dt[0].AvgTicket) && Convert.IsDBNull(dt[0].MonVol) && !Convert.IsDBNull(dt[0].MaxTicket))
                        {
                            retVal = OtherInfo.UpdateOtherInfo(Convert.ToString(DiscApplied), Convert.ToString(AmexApplied), Convert.ToString(JCBApplied), Convert.ToString(DiscNum).Trim(), Convert.ToString(AmexNum).Trim(), "", Convert.ToString(dt[0].MaxTicket).Trim(), Convert.ToString(dt[0].AvgTicket).Trim(), "", false);
                        }
                        else if (Convert.IsDBNull(dt[0].AvgTicket) && Convert.IsDBNull(dt[0].MonVol) && Convert.IsDBNull(dt[0].MaxTicket))
                        {
                            retVal = OtherInfo.UpdateOtherInfo(Convert.ToString(DiscApplied).Trim(), Convert.ToString(AmexApplied).Trim(), Convert.ToString(JCBApplied).Trim(), Convert.ToString(DiscNum).Trim(), Convert.ToString(AmexNum).Trim(), "", "", "", "", false);
                        }



                        if (!retVal)
                            return "Could not insert data in OnlineAppOtherInfo.";

                        //Export Numbers to NBC Table
                        if ((dt[0].DiscoverNum != "") || (dt[0].AmexNum != "") || (dt[0].JCBNum != ""))
                        {
                            retVal = OnlineApp.InsertUpdateNBC(Convert.ToString(NewAppId).Trim(), Convert.ToString(dt[0].DiscoverNum).Trim(),
                                 Convert.ToString(dt[0].AmexNum).Trim(), Convert.ToString(dt[0].JCBNum).Trim());
                        }
                        if (!retVal)
                            return "Could not insert data in OnlineAppNonBankcard.";

                        //If Platform Info Exists in Act, update Reprogram table
                        if ((Convert.ToString(dt[0].Platform).Trim() != "") || (Convert.ToString(dt[0].VisaMasterNum).Trim() != ""))
                        {
                            OnlineAppClassLibrary.ReprogramInfo RPG = new OnlineAppClassLibrary.ReprogramInfo(NewAppId);
                            RPG.UpdateReprogramInfo(Convert.ToString(dt[0].Platform).Trim(),
                                "",//dt[0].VisaMasterNum - since two accounts will never have same merchant number
                                Convert.ToString(dt[0].MerchantID).Trim(),
                                "",
                                "", //PFLoginID
                                Convert.ToString(dt[0].BIN).Trim(),
                                Convert.ToString(dt[0].AgentBankNum).Trim(),
                                Convert.ToString(dt[0].AgentChainNum).Trim(),
                                Convert.ToString(dt[0].MCCCode).Trim(),
                                Convert.ToString(dt[0].StoreNum).Trim(),
                                false, true);
                        }

                        //set the Last Sync Date for the App again after all tables are created
                        OnlineApp.UpdateLastSyncDate(NewAppId);

                        //Get Data from SalesOpps in ACT

                        ACTOnlineAppSalesOppsTableAdapter ACTOnlineAppSalesOppsAdapter = new ACTOnlineAppSalesOppsTableAdapter();
                        PartnerDS.ACTOnlineAppSalesOppsDataTable dtSalesOpps = ACTOnlineAppSalesOppsAdapter.GetDataByContact(new Guid(ContactID));

                        DateTime lastModify = DateTime.Now.Add(new TimeSpan(7, 0, 0));

                        //Create a Sales Opp Object
                        SalesOppDL SalesOpp = new SalesOppDL();
                        if (dtSalesOpps.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtSalesOpps.Rows.Count; i++)
                            {
                                //Insert data in OnlineAppSalesOpps          
                                //Get RepNum Based on RepName
                                string strRepNum = Rep.ReturnMasterNum(dtSalesOpps[i].RepName);
                                retVal = SalesOpp.InsertUpdateSalesOpps(Convert.ToString(NewAppId), Convert.ToString(dtSalesOpps[i].CODE),
                                    Convert.ToString(dtSalesOpps[i].UNITPRICE), Convert.ToString(dtSalesOpps[i].UNITCOST), Convert.ToString(dtSalesOpps[i].QUANTITY),
                                    Convert.ToString(dtSalesOpps[i].TerminalID), Convert.ToString(dtSalesOpps[i].SerialNumber), Convert.ToString(dtSalesOpps[i].STATUS),
                                    "{" + dtSalesOpps[i].CREATEUSERID + "}", "{" + dtSalesOpps[i].ID + "}",
                                    Convert.ToString(strRepNum), Convert.ToString(dtSalesOpps[i].Stage), Convert.ToString(lastModify),
                                    Convert.ToString(dtSalesOpps[i].CREATEDATE), Convert.ToString(dtSalesOpps[i].Reprogram), Convert.ToString(dtSalesOpps[i].PaymentMethod)); 
                                if (!retVal)
                                    return "Could not insert data in OnlineAppSalesOpps.";
                            }
                        }//end if count not 0

                        //set the Last Sync Date for the App again after all tables are created
                        OnlineApp.UpdateLastSyncDate(NewAppId);
                    }
                }
                else
                    return "Record not found.";

                return "Record Exported.";
            }//end try
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region EXPORT ACT STATUS
        //This function is called when the Updates Status info and sales opps. CALLED BY OnlineAppMgmt/default.aspx
        public string ExportACTStatus(int AppId, int AcctType)
        {
            string strAppIdExists = "";
            string strAppIdNotExists = "";
            string strActUserID = "{11111111-1111-1111-1111-111111111111}";
            string currTime = System.DateTime.Now.ToString();

            ACTOnlineAppStatusFieldsTableAdapter ACTOnlineAppStatusFieldsAdapter = new ACTOnlineAppStatusFieldsTableAdapter(); 
            //Get the Online App Status Information
            PartnerDS.ACTOnlineAppStatusFieldsDataTable dtACTStatus = ACTOnlineAppStatusFieldsAdapter.GetData(Convert.ToInt16(AppId));

            if (dtACTStatus.Rows.Count > 0)
            {
                string ActStatus = dtACTStatus[0].MerchantStatus;
                string ActStatusGW = dtACTStatus[0].GatewayStatus;
                string ACTStatusOnlineDebit = dtACTStatus[0].StatusOnlineDebit;
                string ACTStatusGiftCard = dtACTStatus[0].StatusGiftCard;
                string ACTStatusCheckService = dtACTStatus[0].StatusCheckService;
                string ACTStatusEBT = dtACTStatus[0].StatusEBT;
                string ACTStatusMerchantFunding = dtACTStatus[0].StatusMerchantFunding;
                string ACTStatusLease = dtACTStatus[0].StatusLease;
                string ACTStatusPayroll = dtACTStatus[0].StatusPayroll;
                int StatusLength = 0;
                //Use StatusLength to determine is Status is NULL or blank
                if (ActStatus.Length > 0)
                    StatusLength = ActStatus.Length;

                //Get Online App Status from Agent Portal
                OnlineAppDL OnlineApp = new OnlineAppDL();
                OnlineAppStatusDL Status = new OnlineAppStatusDL();
                PartnerLogBL LogData = new PartnerLogBL();
                OnlineAppStatusFieldsTableAdapter OnlineAppStatusFieldsAdapter = new OnlineAppStatusFieldsTableAdapter();
                PartnerDS.OnlineAppStatusFieldsDataTable dtOnlineAppStatus = OnlineAppStatusFieldsAdapter.GetData(Convert.ToInt16(AppId));
                string OnlineAppStatus = string.Empty;
                string OnlineAppStatusGW = string.Empty;
                string StatusOnlineDebit = string.Empty;
                string StatusGiftCard = string.Empty;
                string StatusCheckService = string.Empty;
                string StatusEBT = string.Empty;
                string StatusMerchantFunding = string.Empty;
                string StatusLease = string.Empty;
                string StatusPayroll = string.Empty;

                string DiscoverNum = string.Empty;
                string AmexNum = string.Empty;
                string JCBNum = string.Empty;
                if (dtOnlineAppStatus.Rows.Count > 0)
                {
                    OnlineAppStatus = dtOnlineAppStatus[0].Status;
                    OnlineAppStatusGW = dtOnlineAppStatus[0].StatusGW;
                    StatusOnlineDebit = dtOnlineAppStatus[0].StatusOnlineDebit;
                    StatusGiftCard = dtOnlineAppStatus[0].StatusGiftCard;
                    StatusCheckService = dtOnlineAppStatus[0].StatusCheckService;
                    StatusEBT = dtOnlineAppStatus[0].StatusEBT;
                    StatusLease = dtOnlineAppStatus[0].StatusLease;
                    StatusPayroll = dtOnlineAppStatus[0].StatusPayroll;
                    StatusMerchantFunding = dtOnlineAppStatus[0].StatusMerchantFunding;
                    DiscoverNum = dtOnlineAppStatus[0].DiscoverNum;
                    AmexNum = dtOnlineAppStatus[0].AmexNum;
                    JCBNum = dtOnlineAppStatus[0].JCBNum;
                }
                
                //Merchant Status Or Gateway Status in ACT record is not beyond COMPLETED
                //This code is removed on 4/18/2013 per Jay's instruction that all records regardless or the current status should be updated 
                //and added back on 4/16/2014 per Jay's instruction that incomplete application status should not be written into online app when synching from ACT.
                //This code is removed on 5/12/2017 per Jay's instruction that all records regardless of the current status should be updated. 
                if ((AcctType == 4 && ((ActStatus.ToLower() == "incomplete" || ActStatus.ToLower() == "completed" || ActStatus.Trim() == "") && (OnlineAppStatus.ToLower() == "submitted for review" || OnlineAppStatus.ToLower() == "submitted for underwriting" || OnlineAppStatus.ToLower() == "pending" || OnlineAppStatus.ToLower().Contains("active")) &&
                    (ActStatusGW.ToLower() == "incomplete" || ActStatusGW.ToLower() == "completed" || ActStatusGW.Trim() == "") && (OnlineAppStatusGW.ToLower().Contains("awaiting") || OnlineAppStatusGW.ToLower().Contains("active")))) ||
                    (AcctType == 1 && (ActStatus.ToLower() == "incomplete" || ActStatus.ToLower() == "completed" || ActStatus.Trim() == "") && (OnlineAppStatus.ToLower() == "submitted for review" || OnlineAppStatus.ToLower() == "submitted for underwriting" || OnlineAppStatus.ToLower() == "pending" || OnlineAppStatus.ToLower().Contains("active"))) ||
                    (AcctType == 2 && (ActStatusGW.ToLower() == "incomplete" || ActStatusGW.ToLower() == "completed" || ActStatusGW.Trim() == "") && (OnlineAppStatusGW.ToLower().Contains("awaiting") || OnlineAppStatusGW.ToLower().Contains("active")))||
                    (AcctType == 4 && ((ActStatus.ToLower() == "incomplete" || ActStatus.Trim() == "") && (OnlineAppStatus.ToLower().Contains("complete") || OnlineAppStatus.ToLower() == "submitted for review" || OnlineAppStatus.ToLower() == "submitted for underwriting" || OnlineAppStatus.ToLower() == "pending" || OnlineAppStatus.ToLower().Contains("active")) &&
                    (ActStatusGW.ToLower() == "incomplete" || ActStatusGW.Trim() == "") && (OnlineAppStatusGW.ToLower().Contains("complete")||OnlineAppStatusGW.ToLower().Contains("awaiting") || OnlineAppStatusGW.ToLower().Contains("active")))) ||
                    (AcctType == 1 && (ActStatus.ToLower() == "incomplete" || ActStatus.Trim() == "") && (OnlineAppStatus.ToLower().Contains("complete") || OnlineAppStatus.ToLower() == "submitted for review" || OnlineAppStatus.ToLower() == "submitted for underwriting" || OnlineAppStatus.ToLower() == "pending" || OnlineAppStatus.ToLower().Contains("active"))) ||
                    (AcctType == 2 && (ActStatusGW.ToLower() == "incomplete" || ActStatusGW.ToLower() == "completed" || ActStatusGW.Trim() == "") && (OnlineAppStatusGW.ToLower().Contains("complete") || OnlineAppStatusGW.ToLower().Contains("awaiting") || OnlineAppStatusGW.ToLower().Contains("active")))
                    )
                { strAppIdExists = "+"; }
                else if((AcctType == 4 && (( ActStatus.Trim() == "") && (OnlineAppStatus.ToLower() == "submitted for review" || OnlineAppStatus.ToLower() == "submitted for underwriting" || OnlineAppStatus.ToLower() == "pending" || OnlineAppStatus.ToLower().Contains("active") || OnlineAppStatus.ToLower().Contains("completed") || OnlineAppStatus.ToLower().Contains("incomplete")) &&
                    (ActStatusGW.Trim() == "") && (OnlineAppStatusGW.ToLower().Contains("awaiting") || OnlineAppStatusGW.ToLower().Contains("active") || OnlineAppStatusGW.ToLower().Contains("complete") || OnlineAppStatusGW.ToLower().Contains("incomplete")))) ||
                    (AcctType == 1 && (ActStatus.Trim() == "") && (OnlineAppStatus.ToLower() == "submitted for review" || OnlineAppStatus.ToLower() == "submitted for underwriting" || OnlineAppStatus.ToLower() == "pending" || OnlineAppStatus.ToLower().Contains("active") || OnlineAppStatus.ToLower().Contains("incomplete") || OnlineAppStatus.ToLower().Contains("complete"))) ||
                    (AcctType == 2 && (ActStatusGW.Trim() == "") && (OnlineAppStatusGW.ToLower().Contains("awaiting") || OnlineAppStatusGW.ToLower().Contains("active") || OnlineAppStatusGW.ToLower().Contains("incomplete") || OnlineAppStatusGW.ToLower().Contains("complete"))))
                { strAppIdExists = ""; }
                /* Not sure why this was added, so removing the code.
                 * else if ((ActStatus == "DECLINED") && (OnlineAppStatus == "SUBMITTED FOR REVIEW"))
                    strAppIdExists = "#";*/
                else//Status info is OK to Copy over
                {
                    //Update OnlineAppNewApp                      
                    OnlineApp.UpdateNewAppStatus(AppId, ActStatus, ActStatusGW);

                    //if Merchant Status has changed, by comparing the ACT and Online App Records
                    if (OnlineAppStatus != ActStatus)
                        //Status.InsertNote(strActUserID, AppId, "Update from ACT: Merchant Status changed to " + ActStatus,  currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Merchant Status changed from " + OnlineAppStatus + " to " + ActStatus);

                    //if Gateway Status has changed, by comparing the ACT and Online App Records
                    if (OnlineAppStatusGW != ActStatusGW)
                        //Status.InsertNote(strActUserID, AppId, "Update from ACT: Gateway Status changed to " + ActStatusGW, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Gateway Status changed from " + OnlineAppStatusGW + " to " + ActStatusGW);

                    //Update Additional Services Status in OnlineAppNewApp table
                    OnlineApp.UpdateNewAppExportedAddlServicesStatus(AppId, dtACTStatus[0].StatusOnlineDebit,
                        dtACTStatus[0].StatusGiftCard, dtACTStatus[0].StatusCheckService,
                        dtACTStatus[0].StatusEBT, dtACTStatus[0].StatusMerchantFunding,
                        dtACTStatus[0].StatusLease, dtACTStatus[0].StatusPayroll);

                    if (AcctType == 2 || AcctType == 4)
                    {
                        //Insert or Update the Gateway ID and Password
                        OnlineApp.InsertUpdateGateway(AppId.ToString(), dtACTStatus[0].Gateway,
                            dtACTStatus[0].GatewayUserID);

                        OnlineApp.UpdateNewAppGWStatus(AppId, ActStatusGW);
                    }

                    if (AcctType == 1 || AcctType == 4)
                    {
                        //If Platform Info Exists in Act, update Platform table
                        //if (dtACTStatus[0].Platform != "")
                        {
                            OnlineApp.InsertUpdatePlatform(AppId, dtACTStatus[0].Platform,
                                 dtACTStatus[0].MerchantNum,
                                 dtACTStatus[0].MerchantID,
                                 "",
                                 "", //dtACTStatus[0].PFLoginID,
                                 dtACTStatus[0].BankIDNum,
                                 dtACTStatus[0].AgentBankIDNum,
                                 dtACTStatus[0].AgentChainNum,
                                 dtACTStatus[0].CategoryCode,
                                 dtACTStatus[0].StoreNum);
                        }
                    }//end if accttype = 1 or 4 

                    #region STATUS NOTES
                    //if OnlineDebit Status has changed, by comparing the ACT and Online App Records
                    if (StatusOnlineDebit != ACTStatusOnlineDebit)
                        //Status.InsertNote(strActUserID, AppId, "Online Debit Status changed to " + ACTStatusOnlineDebit, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Online Debit Status changed from " + StatusOnlineDebit + " to " + ACTStatusOnlineDebit);

                    //if GiftCard Status has changed, by comparing the ACT and Online App Records
                    if (StatusGiftCard != ACTStatusGiftCard)
                        //Status.InsertNote(strActUserID, AppId, "Gift Card Status changed to " + ACTStatusGiftCard, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Gift Card Status changed from " + StatusGiftCard + " to " + ACTStatusGiftCard);

                    //if CheckService Status has changed, by comparing the ACT and Online App Records
                    if (StatusCheckService != ACTStatusCheckService)
                        //Status.InsertNote(strActUserID, AppId, "Check Service Status changed to " + ACTStatusCheckService, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Check Service Status changed from " + StatusCheckService + " to " + ACTStatusCheckService);
                    //if EBT Status has changed, by comparing the ACT and Online App Records
                    if (StatusEBT != ACTStatusEBT)
                        //Status.InsertNote(strActUserID, AppId, "EBT Status changed to " + ACTStatusEBT, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: EBT Status changed from " + StatusEBT + " to " + ACTStatusEBT);

                    //if Merchant Funding Status has changed, by comparing the ACT and Online App Records
                    if (StatusMerchantFunding != ACTStatusMerchantFunding)
                        //Status.InsertNote(strActUserID, AppId, "Merchant Funding Status changed to " + ACTStatusMerchantFunding, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Merchant Funding Status from " + StatusEBT + " to " + ACTStatusEBT);

                    //if Lease Status has changed, by comparing the ACT and Online App Records
                    if (StatusLease != ACTStatusLease)
                        //Status.InsertNote(strActUserID, AppId, "Lease Status changed to " + ACTStatusLease, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Lease Status from " + StatusLease + " to " + ACTStatusLease);

                    //if Payroll Status has changed, by comparing the ACT and Online App Records
                    if (StatusPayroll != ACTStatusPayroll)
                        //Status.InsertNote(strActUserID, AppId, "Payroll Status changed to " + ACTStatusPayroll, currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Payroll Status from " + StatusPayroll + " to " + ACTStatusPayroll);

                    //if Discover Num has changed, by comparing the ACT and Online App Records
                    if (DiscoverNum != dtACTStatus[0].DiscoverNum)
                        //Status.InsertNote(strActUserID, AppId, "Discover Number Status Update", currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Discover Number Status Update from " + DiscoverNum + " to " + dtACTStatus[0].DiscoverNum);

                    //if Amex Num has changed, by comparing the ACT and Online App Records
                    if (AmexNum != dtACTStatus[0].AmexNum)
                        //Status.InsertNote(strActUserID, AppId, "Amex Number Status Update", currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: Amex Number Status Update from " + AmexNum + " to " + dtACTStatus[0].AmexNum);

                    //if JCB Num has changed, by comparing the ACT and Online App Records
                    if (JCBNum != dtACTStatus[0].JCBNum)
                        //Status.InsertNote(strActUserID, AppId, "JCB Number Status Update", currTime);
                        LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Update from ACT: JCB Number Status Update from " + JCBNum + " to " + dtACTStatus[0].AmexNum);
                    #endregion

                    //Export Numbers to NBC Table
                    if ((dtACTStatus[0].DiscoverNum != "") || (dtACTStatus[0].AmexNum != "") || (dtACTStatus[0].JCBNum != ""))
                    {
                        OnlineApp.InsertUpdateNBC(AppId.ToString(), dtACTStatus[0].DiscoverNum,
                            dtACTStatus[0].AmexNum, dtACTStatus[0].JCBNum);
                    }

                    //Insert or update Sales Opps
                    ACTOnlineAppSalesOppsTableAdapter ACTOnlineAppSalesOppsAdapter = new ACTOnlineAppSalesOppsTableAdapter();
                    PartnerDS.ACTOnlineAppSalesOppsDataTable dtACTSalesOpps = ACTOnlineAppSalesOppsAdapter.GetData(Convert.ToInt16(AppId));
                    if (dtACTSalesOpps.Rows.Count > 0)
                    {
                        SalesOppDL SalesOpp = new SalesOppDL();
                        RepInfoDL Rep = new RepInfoDL();
                        for (int i = 0; i < dtACTSalesOpps.Rows.Count; i++)
                        {
                            //Insert data in OnlineAppSalesOpps
                            //Get RepNum Based on RepName in the Sales Opp

                            string strRepNum = Rep.ReturnMasterNum(dtACTSalesOpps[i].RepName);
                            bool retVal = SalesOpp.InsertUpdateSalesOpps(AppId.ToString(), dtACTSalesOpps[i].CODE,
                                dtACTSalesOpps[i].UNITPRICE, dtACTSalesOpps[i].UNITCOST,
                                dtACTSalesOpps[i].QUANTITY,
                                dtACTSalesOpps[i].TerminalID, dtACTSalesOpps[i].SerialNumber,
                                dtACTSalesOpps[i].STATUS, "{" + dtACTSalesOpps[i].CREATEUSERID + "}", "{" + dtACTSalesOpps[i].ID + "}",
                                strRepNum, dtACTSalesOpps[i].Stage, dtACTSalesOpps[i].LastModified,
                                dtACTSalesOpps[i].CREATEDATE, dtACTSalesOpps[i].Reprogram, dtACTSalesOpps[i].PaymentMethod);
                        }//end for
                        //Set LastSynchDate to a default value for unlinked sales opps with LastSynchDate = null 
                        //and if IsACTRecord is true

                        bool RetVal = SalesOpp.UpdateLastSynchForUnlinkedOpps(AppId);
                    }//end if count not 0
                }//end else
            }//end if count not 0
            else
                strAppIdNotExists = "?";

            return strAppIdExists + System.Environment.NewLine + strAppIdNotExists;
        }//end function UpdateACTStatus
        #endregion

        #region ADD TO ACT
        //This function Adds online application information to ACT!
        //CALLED BY Edit.aspx
        public string AddInfoToACT(int AppId)
        {
            OnlineAppDL OnlineApp = new OnlineAppDL();
            OnlineAppACTFieldsTableAdapter OnlineAppACTFieldsAdapter = new OnlineAppACTFieldsTableAdapter();
            PartnerDS.OnlineAppACTFieldsDataTable dt = OnlineAppACTFieldsAdapter.GetData(Convert.ToInt16(AppId));

            bool retVal = false;

            //If Data found for this App
            if (dt.Rows.Count > 0)
            {
                retVal = true;
                //Update last sync date in OnlineAppNewApp
                OnlineApp.UpdateLastSyncDate(AppId);

                string strComplianceFee = "";

                OnlineAppBL Compliance = new OnlineAppBL(AppId);

                strComplianceFee = Compliance.GetComplianceFee();

                //Insert info in TBL_CONTACT in ACT
                string P1FullName = "";
                if (dt[0].P1MidName == "")
                    P1FullName = dt[0].P1FirstName + " " + dt[0].P1LastName;
                else
                    P1FullName = dt[0].P1FirstName + " " + dt[0].P1MidName + " " + dt[0].P1LastName;

                string FullName = "";
                //if (dt[0].P1MidName == "")
                    FullName = dt[0].FirstName + " " + dt[0].LastName;
                /*else
                    P1FullName = dt[0].P1FirstName + " " + dt[0].P1MidName + " " + dt[0].P1LastName;*/
                string CreateDate = DateTime.Now.ToString();
                
                ACTDataDL ACT = new ACTDataDL();
                retVal = ACT.AddDataContact(CreateDate, FullName, dt[0].LegalStatus,
                    dt[0].FirstName, dt[0].LastName,
                    dt[0].P1FirstName, dt[0].P1LastName,
                    dt[0].P1MidName, dt[0].P1Title, dt[0].Title,
                    dt[0].CompanyName, dt[0].DBA,
                    dt[0].COMPANYWEBADDRESS, dt[0].ReferredBy,
                    dt[0].AffiliateReferral, dt[0].SalesRep,
                    dt[0].Gateway, dt[0].Processor,
                    dt[0].YIB, dt[0].MIB,
                    dt[0].YABL, dt[0].MABL,
                    dt[0].BusHours, dt[0].NumOfLocs,
                    dt[0].NumOfDaysProdDel, dt[0].ProdServSold,
                    dt[0].AddlComments, dt[0].Bankruptcy,
                    dt[0].MAAddr1, dt[0].MAAddr2,
                    dt[0].MACity, dt[0].MAState,
                    dt[0].MAZip, dt[0].MACountry,
                    dt[0].FedTaxID, dt[0].Platform,
                    dt[0].AnnualFee, dt[0].P1SocialSecurity,
                    dt[0].P1OwnPct, dt[0].P1LivingStatus,
                    dt[0].P1LOR, dt[0].P1DLNum,
                    dt[0].P1DLState, dt[0].P1DLExpDate,
                    dt[0].P1DOB, dt[0].RefundPolicy,
                    dt[0].BankName, dt[0].BankCity,
                    dt[0].BankState, dt[0].BankZip,
                    dt[0].RoutingNum, dt[0].CheckingAcctNum,
                    dt[0].CustServFee, dt[0].MonMin, dt[0].InternetStmt,
                    dt[0].DiscQNP, dt[0].DiscMQ,
                    dt[0].DiscNQ, dt[0].AmexDiscRateQual, dt[0].AmexDiscRateMidQual, dt[0].AmexDiscRateNonQual, dt[0].DiscQD,
                    dt[0].DebitQualNP, dt[0].DebitMidQual, dt[0].DebitNonQual,
                    dt[0].DiscQP, dt[0].TransFee,
                    dt[0].RetrievalFee, dt[0].VoiceAuth,
                    dt[0].BatchHeader, dt[0].AVS,
                    dt[0].NBCTFee, dt[0].CBFee,
                    Convert.ToInt32(dt[0].AcctType),
                    dt[0].MonVol, dt[0].AvgTicket,
                    dt[0].JCBNum, dt[0].AmexNum,
                    dt[0].DiscoverNum,
                    dt[0].PctSwp, dt[0].PctKWI,
                    dt[0].PctKWOI, dt[0].PctRet,
                    dt[0].PctRest, dt[0].PctServ,
                    dt[0].PctMail, dt[0].PctInt,
                    dt[0].PctOth, dt[0].GWMonFee,
                    dt[0].GWTransFee, dt[0].GWSetupFee,
                    dt[0].ProcBCBefore, dt[0].CTMFMatch,
                    dt[0].MerchantStatus, dt[0].GatewayStatus,
                    dt[0].MerchantID, dt[0].MerchantNum,                    
                    dt[0].RepNum, dt[0].MCCCategoryCode,
                    dt[0].PayrollType,dt[0].Payroll, dt[0].PayrollStatus, dt[0].MCAType, dt[0].ContractTerm, AppId);

                if (!Convert.IsDBNull(dt[0].DiscountPaid))
                {
                    ACT.UpdateDiscountPaid(dt[0].DiscountPaid, AppId);
                }

                ACT.UpdateComplianceFee(strComplianceFee, AppId);

                if (!Convert.IsDBNull(dt[0].PinDebitDisount))
                {
                    ACT.UpdatePinDebitDiscount(dt[0].PinDebitDisount, AppId);
                }

                if (!retVal)
                    return "Contact Information cannnot be added.";

                //Insert info in secondary contact table in ACT
                //string FullName = FullName = dt[0].FirstName + " " + dt[0].LastName;

                //Get ContactID that was just inserted in TBL_CONTACT in ACT
                string ContactID = ACT.ReturnContactID(AppId);
                if (ContactID == "")
                    return "Contact ID not found";

                //INSERT Signup Person as a Secondary Contact in ACT (Principal is regarded as the Primary in ACT!)

                /*retVal = ACT.InsertUpdateSecContactInfo(ContactID, AppId, "Signup Contact",
                    dt[0].Title, FullName, dt[0].FirstName,
                    dt[0].LastName, dt[0].SignupEmail,
                    dt[0].Phone, dt[0].PhoneExt, dt[0].SecMobilePhone, dt[0].SecHomePhone);*/

                if (!retVal)
                    return "Secondary Contact cannot be added.";

                //INSERT p1's EMAIL IN THE EMAIL TABLE 	
                retVal = ACT.InsertUpdatep1Email(ContactID, dt[0].P1Email);

                //INSERT Contact's EMAIL IN THE EMAIL TABLE 	
                retVal = ACT.InsertUpdateContactEmail(ContactID, dt[0].SignupEmail);

                //INSERT BUSINESS ADDRESS
                retVal = ACT.InsertUpdateBusinessAddress(ContactID, dt[0].LINE1,
                    dt[0].LINE2, dt[0].City, dt[0].State,
                    dt[0].POSTALCODE, dt[0].COUNTRYNAME);


                //INSERT P1 HOME ADDRESS
                retVal = ACT.InsertUpdateP1HomeAddress(ContactID, dt[0].P1HomeLINE1,
                    dt[0].P1HomeLINE2, dt[0].P1HomeCITY, dt[0].P1HomeSTATE,
                    dt[0].P1HomePOSTALCODE, dt[0].P1HomeCOUNTRYNAME);

                //Insert (P1's) BUSINESS PHONE NUMBER
                retVal = ACT.InsertUpdateP1BusinessPhone(ContactID, dt[0].BUSINESS_PHONE,
                    dt[0].Business_PhoneEXT);
                if (!retVal)
                    return "Cannot insert P1 Business Phone Number.";

                //**************************** Insert all phone numbers ************************
                //Insert CUSTOMER SERVICE PHONE NUMBER
                int iRetVal = ACT.InsertUpdatePhone("CustServ", ContactID, dt[0].CustServPhone);

                //Insert Fax
                iRetVal = ACT.InsertUpdatePhone("Fax", ContactID, dt[0].Fax);

                //Insert HOME PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("Home", ContactID, dt[0].HOME_PHONE);

                //Insert P1 HOME PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("P1Home", ContactID, dt[0].HOME_PHONE);

                //Insert PRINCIPAL 2 HOME PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("P2Home", ContactID, dt[0].P2HomePhone);

                //Insert Mobile PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("Mobile", ContactID, dt[0].SecMobilePhone);

                //Insert P1 Mobile PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("P1Mobile", ContactID, dt[0].P1MobilePhone);

                //Insert P2 Mobile PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("P2Mobile", ContactID, dt[0].P2MobilePhone);

                //Insert BANK PHONE NUMBER
                iRetVal = ACT.InsertUpdatePhone("Bank", ContactID, dt[0].BankPhone);
                //************************************************************
                OnlineAppSalesOppsTableAdapter OnlineAppSalesOppsAdapter = new OnlineAppSalesOppsTableAdapter();
                PartnerDS.OnlineAppSalesOppsDataTable dtSalesOpps = OnlineAppSalesOppsAdapter.GetData(Convert.ToInt16(AppId));
                if (dt.Rows.Count > 0)
                {
                    SalesOppDL SalesOpps = new SalesOppDL();
                    for (int i = 0; i < dtSalesOpps.Rows.Count; i++)
                    {
                        ACT.InsertSalesOpp(dtSalesOpps[i].ID.ToString(),
                            ContactID, dtSalesOpps[i].ActUserID.ToString(), dtSalesOpps[i].Product,
                            dtSalesOpps[i].ProductCode, dtSalesOpps[i].Price, dtSalesOpps[i].CostOfGoods,
                            dtSalesOpps[i].Quantity, dtSalesOpps[i].Subtotal,
                            dtSalesOpps[i].RepName, dtSalesOpps[i].LastModified,
                            dtSalesOpps[i].CreateDate, dtSalesOpps[i].PaymentMethod, dtSalesOpps[i].Reprogram, dtSalesOpps[i].TerminalID);

                        //Update the isAddedtoAct Bit            
                        SalesOpps.UpdateIsAddedACTTrue(dtSalesOpps[i].ID.ToString());
                    }
                }

                //Add Notes Into ACT
                OnlineAppNotesTableAdapter OnlineAppNotesAdapter = new OnlineAppNotesTableAdapter();
                /*PartnerDS.OnlineAppNotesDataTable dtNotes = OnlineAppNotesAdapter.GetData(Convert.ToInt16(AppId));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNotes.Rows.Count; i++)
                    {
                        ACT.InsertNotes(dtNotes[i].NoteID.ToString(), ContactID,
                            dtNotes[i].ActUserID.ToString(), dtNotes[i].NoteText.Replace("'", ""), dtNotes[i].DateRecorded);
                    }
                }*/

                if (AppId != 1)
                {
                    //Add Reminder scheduled by Jay for Jay into ACT
                    string strRegarding = "New Online App.";
                    retVal = ACT.InsertReminder(ContactID, strRegarding);
                }
                
                retVal = ACT.InsertUpdateActCust(ContactID, dt[0].AddlComments,
                    dt[0].OtherRefund, dt[0].NameOfPrevProc,
                    dt[0].FormerMerchantNums, dt[0].ReasonLeavingProc,
                    dt[0].P2LastName, dt[0].P2FirstName,
                    dt[0].P2Title, dt[0].P2OwnPct,
                    dt[0].P2LOR, dt[0].P2HomeAddr1,
                    dt[0].P2HomeCity, dt[0].P2HomeState,
                    dt[0].P2HomeZip, dt[0].P2HomeCountry,
                    dt[0].P2SocialSecurity, dt[0].P2LivingStatus,
                    dt[0].P2DLNum, dt[0].P2DLState,
                    dt[0].P2DLExpDate, dt[0].P2DOB,
                    dt[0].BankAddr, dt[0].TerminalID,
                    dt[0].LoginID, dt[0].BankIDNum,
                    dt[0].AgentBankIDNum, dt[0].AgentChainNum,
                    dt[0].MCCCategoryCode, dt[0].StoreNum,
                    dt[0].MaxTicket,
                    Convert.ToBoolean(dt[0].OnlineDebit), Convert.ToBoolean(dt[0].GiftCard),
                    Convert.ToBoolean(dt[0].CheckService), dt[0].CheckServiceName,
                    Convert.ToBoolean(dt[0].EBT), Convert.ToBoolean(dt[0].MerchantFunding),
                    dt[0].USDANum, dt[0].DebitMonFee,
                    dt[0].DebitTransFee, dt[0].CGMonFee, dt[0].CGTransFee,
                    dt[0].CGMonMin, dt[0].CGDiscRate, 
                    dt[0].GiftCardType, dt[0].GCMonFee, dt[0].GCTransFee,
                    dt[0].EBTMonFee, dt[0].EBTTransFee, dt[0].WirelessAccess, 
                    dt[0].WirelessTransFee, Convert.ToBoolean(dt[0].Interchange),
                    Convert.ToBoolean(dt[0].Assessments), dt[0].RollingReserve, dt[0].Lease, dt[0].LeaseCompany,
                    dt[0].LeasePayment, dt[0].LeaseTerm, Convert.ToString(dt[0].MCAAmount), Convert.ToString(dt[0].MCAStatus));

                if (!retVal)
                    return "Cannot add information to ActCust table.";

                //INSERT P2's EMAIL IN THE EMAIL TABLE
                retVal = ACT.InsertUpdateP2Email(ContactID, dt[0].P2Email);

            }//end if row count not 0
            return "Add Successful";
        }//end function addinfotoact
        #endregion

        #region UPDATE ACT
        //This function Update online application (not including Card Number) information in ACT!
        //CALLED BY Edit.aspx




        public string UpdateAct(int AppId, int partnerID)
        {
            OnlineAppACTFieldsTableAdapter OnlineAppACTFieldsAdapter = new OnlineAppACTFieldsTableAdapter();
            PartnerDS.OnlineAppACTFieldsDataTable dt = OnlineAppACTFieldsAdapter.GetData(Convert.ToInt16(AppId));

            string strComplianceFee = "";

            OnlineAppBL Compliance = new OnlineAppBL(AppId);

            strComplianceFee = Compliance.GetComplianceFee();


            bool retVal = false;
            if (dt.Rows.Count > 0)
            {
                retVal = true;
                //Update last sync date in OnlineAppNewApp
                OnlineAppBL App = new OnlineAppBL(AppId);


                //Insert info in TBL_CONTACT in ACT
                string P1FullName = "";
                if (dt[0].P1MidName == "")
                    P1FullName = dt[0].P1FirstName + " " + dt[0].P1LastName;
                else
                    P1FullName = dt[0].P1FirstName + " " + dt[0].P1MidName + " " + dt[0].P1LastName;
                string CreateDate = DateTime.Now.ToString();


                //Get ContactID for the record to be updated
                ACTDataDL ACT = new ACTDataDL();
                //Get ContactID that was just inserted
                string ContactID = ACT.ReturnContactID(AppId);
                if (ContactID == "")
                    return "Contact ID not found";
                
                OnlineAppDL onlineAppData = new OnlineAppDL();
                string strOnlineAppEditDate = onlineAppData.GetOnlineAppEditDate(AppId);
                string strOnlineAppLastSynchDate = onlineAppData.GetOnlineAppLastSynchDate(AppId);
                DateTime onlineAppUpdateDate;

                DateTime OnlineAppEditDate = Convert.ToDateTime(strOnlineAppEditDate);
                DateTime OnlineAppLastSynchDate = Convert.ToDateTime(strOnlineAppLastSynchDate);

                if (OnlineAppLastSynchDate >= OnlineAppEditDate)
                {
                    onlineAppUpdateDate = OnlineAppLastSynchDate;
                }
                else {
                    onlineAppUpdateDate = OnlineAppEditDate;
                }

                DataSet dsACTEditDate = GetACTEditDate(ContactID, onlineAppUpdateDate);

                

                //if (dsACTEditDate.Tables[0].Rows.Count == 0)
                //if ((dsACTEditDate == null) || dsACTEditDate.Tables.Count == 0)
                {
                    //Insert information into Backup Table before updating
                    ACT.InsertActRecordBackup(ContactID);
                    retVal = ACT.AddDataContact(CreateDate, dt[0].FirstName + " " + dt[0].LastName, dt[0].LegalStatus,
                    dt[0].FirstName, dt[0].LastName, dt[0].P1FirstName, dt[0].P1LastName,
                    dt[0].P1MidName, dt[0].P1Title, dt[0].Title,
                    dt[0].CompanyName, dt[0].DBA,
                    dt[0].COMPANYWEBADDRESS, dt[0].ReferredBy,
                    dt[0].AffiliateReferral, dt[0].SalesRep,
                    dt[0].Gateway, dt[0].Processor,
                    dt[0].YIB, dt[0].MIB,
                    dt[0].YABL, dt[0].MABL,
                    dt[0].BusHours, dt[0].NumOfLocs,
                    dt[0].NumOfDaysProdDel, dt[0].ProdServSold,
                    dt[0].AddlComments, dt[0].Bankruptcy,
                    dt[0].MAAddr1, dt[0].MAAddr2,
                    dt[0].MACity, dt[0].MAState,
                    dt[0].MAZip, dt[0].MACountry,
                    dt[0].FedTaxID, dt[0].Platform,
                    dt[0].AnnualFee, dt[0].P1SocialSecurity,
                    dt[0].P1OwnPct, dt[0].P1LivingStatus,
                    dt[0].P1LOR, dt[0].P1DLNum,
                    dt[0].P1DLState, dt[0].P1DLExpDate,
                    dt[0].P1DOB, dt[0].RefundPolicy,
                    dt[0].BankName, dt[0].BankCity,
                    dt[0].BankState, dt[0].BankZip,
                    dt[0].RoutingNum, dt[0].CheckingAcctNum,
                    dt[0].CustServFee, dt[0].MonMin, dt[0].InternetStmt,
                    dt[0].DiscQNP, dt[0].DiscMQ,
                    dt[0].DiscNQ, dt[0].AmexDiscRateQual, dt[0].AmexDiscRateMidQual, dt[0].AmexDiscRateNonQual, dt[0].DiscQD,
                    dt[0].DebitQualNP, dt[0].DebitMidQual, dt[0].DebitNonQual,
                    dt[0].DiscQP, dt[0].TransFee,
                    dt[0].RetrievalFee, dt[0].VoiceAuth,
                    dt[0].BatchHeader, dt[0].AVS,
                    dt[0].NBCTFee, dt[0].CBFee,
                    Convert.ToInt32(dt[0].AcctType),
                    dt[0].MonVol, dt[0].AvgTicket,
                    dt[0].JCBNum, dt[0].AmexNum,
                    dt[0].DiscoverNum,
                    dt[0].PctSwp, dt[0].PctKWI,
                    dt[0].PctKWOI, dt[0].PctRet,
                    dt[0].PctRest, dt[0].PctServ,
                    dt[0].PctMail, dt[0].PctInt,
                    dt[0].PctOth, dt[0].GWMonFee,
                    dt[0].GWTransFee, dt[0].GWSetupFee,
                    dt[0].ProcBCBefore, dt[0].CTMFMatch,
                    dt[0].MerchantStatus, dt[0].GatewayStatus,
                    dt[0].MerchantID, dt[0].MerchantNum,
                    dt[0].RepNum, dt[0].MCCCategoryCode,
                    dt[0].PayrollType, dt[0].Payroll, dt[0].PayrollStatus, dt[0].MCAType, dt[0].ContractTerm, AppId);

                    if (!Convert.IsDBNull(dt[0].DiscountPaid))
                    {
                        ACT.UpdateDiscountPaid(dt[0].DiscountPaid, AppId);
                    }

                    ACT.UpdateComplianceFee(strComplianceFee, AppId);

                    if (!Convert.IsDBNull(dt[0].PinDebitDisount))
                    {
                        ACT.UpdatePinDebitDiscount(dt[0].PinDebitDisount, AppId);
                    }

                     /*if (!Convert.IsDBNull(dt[0].MCAAmount))
                     {
                         ACT.UpdateMCAAmount(Convert.ToInt32(dt[0].MCAAmount), AppId);
                     }*/
                    

                    //returns 1 if multiple records with same appid is found
                    if (!retVal)
                        return "Multiple records with the same App ID found in ACT! Update failed.";

                    //Insert info in secondary contact table in ACT
                    string FullName = FullName = dt[0].FirstName + " " + dt[0].LastName;

                    //Update Signup Person as a Secondary Contact in ACT (Principal is regarded as the Primary in ACT!)
                    /*retVal = ACT.InsertUpdateSecContactInfo(ContactID, AppId, "Signup Contact",
                        dt[0].Title, FullName, dt[0].FirstName,
                        dt[0].LastName, dt[0].SignupEmail,
                        dt[0].Phone, dt[0].PhoneExt, dt[0].SecMobilePhone, dt[0].SecHomePhone);*/

                    if (!retVal)
                        return "Secondary Contact cannot be added.";

                    //Insert or Update Contact's EMAIL IN THE EMAIL TABLE 	
                    retVal = ACT.InsertUpdateContactEmail(ContactID, dt[0].SignupEmail);

                    //INSERT p1's EMAIL IN THE EMAIL TABLE 	
                    retVal = ACT.InsertUpdatep1Email(ContactID, dt[0].P1Email);

                    //INSERT BUSINESS ADDRESS
                    retVal = ACT.InsertUpdateBusinessAddress(ContactID, dt[0].LINE1,
                        dt[0].LINE2, dt[0].City, dt[0].State,
                        dt[0].POSTALCODE, dt[0].COUNTRYNAME);

                    //INSERT P1 HOME ADDRESS
                    ACTDataDL AddP1HomeAddress = new ACTDataDL();
                    retVal = AddP1HomeAddress.InsertUpdateP1HomeAddress(ContactID, dt[0].P1HomeLINE1,
                        dt[0].P1HomeLINE2, dt[0].P1HomeCITY, dt[0].P1HomeSTATE,
                        dt[0].P1HomePOSTALCODE, dt[0].P1HomeCOUNTRYNAME);

                    //Insert (P1's) BUSINESS PHONE NUMBER
                    retVal = ACT.InsertUpdateP1BusinessPhone(ContactID, dt[0].BUSINESS_PHONE,
                        dt[0].Business_PhoneEXT);
                    if (!retVal)
                        return "Cannot insert P1 Business Phone Number.";

                    //**************************** Insert all phone numbers ************************
                    //Insert CUSTOMER SERVICE PHONE NUMBER
                    int iRetVal = ACT.InsertUpdatePhone("CustServ", ContactID, dt[0].CustServPhone);

                    //Insert Fax
                    iRetVal = ACT.InsertUpdatePhone("Fax", ContactID, dt[0].Fax);

                    //Insert HOME PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("Home", ContactID, dt[0].HOME_PHONE);

                    //Insert P1 HOME PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("P1Home", ContactID, dt[0].HOME_PHONE);

                    //Insert PRINCIPAL 2 HOME PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("P2Home", ContactID, dt[0].P2HomePhone);

                    //Insert Mobile PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("Mobile", ContactID, dt[0].SecMobilePhone);

                    //Insert P1 Mobile PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("P1Mobile", ContactID, dt[0].P1MobilePhone);

                    //Insert P2 Mobile PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("P2Mobile", ContactID, dt[0].P2MobilePhone);

                    //Insert BANK PHONE NUMBER
                    iRetVal = ACT.InsertUpdatePhone("Bank", ContactID, dt[0].BankPhone);
                    //************************************************************

                    //Add Notes Into ACT
                    /*OnlineAppNotesTableAdapter OnlineAppNotesAdapter = new OnlineAppNotesTableAdapter();
                    PartnerDS.OnlineAppNotesDataTable dtNotes = OnlineAppNotesAdapter.GetData(Convert.ToInt16(AppId));
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtNotes.Rows.Count; i++)
                        {
                            ACT.InsertNotes(dtNotes[i].NoteID.ToString(), ContactID,
                                dtNotes[i].ActUserID.ToString(), dtNotes[i].NoteText.Replace("'", ""), dtNotes[i].DateRecorded);
                        }
                    }*/

                    if (AppId != 1)
                    {
                        //Add Reminder scheduled by Jay for Jay into ACT
                        string strRegarding = "Online App Updated. Please follow up.";
                        retVal = ACT.InsertReminder(ContactID, strRegarding);
                    }

                    /*if (!Convert.IsDBNull(dt[0].MCAAmount))
                    {
                        MCAAmount = Convert.ToDecimal(dt[0].MCAAmount);
                    }
                    else {
                        MCAAmount = Convert.ToDecimal(dt[0].MCAAmount);
                    }*/

                    /*if (!(Convert.IsDBNull(dt[0].MCAAmount)))
                    {
                        MCAAmount = Convert.ToInt32(dt[0].MCAAmount);
                    }
                    else {
                        MCAAmount = dt[0].MCAAmount;
                    }*/

                    retVal = ACT.InsertUpdateActCust(ContactID, dt[0].AddlComments,
                        dt[0].OtherRefund, dt[0].NameOfPrevProc,
                        dt[0].FormerMerchantNums, dt[0].ReasonLeavingProc,
                        dt[0].P2LastName, dt[0].P2FirstName,
                        dt[0].P2Title, dt[0].P2OwnPct,
                        dt[0].P2LOR, dt[0].P2HomeAddr1,
                        dt[0].P2HomeCity, dt[0].P2HomeState,
                        dt[0].P2HomeZip, dt[0].P2HomeCountry,
                        dt[0].P2SocialSecurity, dt[0].P2LivingStatus,
                        dt[0].P2DLNum, dt[0].P2DLState,
                        dt[0].P2DLExpDate, dt[0].P2DOB,
                        dt[0].BankAddr, dt[0].TerminalID,
                        dt[0].LoginID, dt[0].BankIDNum,
                        dt[0].AgentBankIDNum, dt[0].AgentChainNum,
                        dt[0].MCCCategoryCode, dt[0].StoreNum,
                        dt[0].MaxTicket,
                        Convert.ToBoolean(dt[0].OnlineDebit), Convert.ToBoolean(dt[0].GiftCard),
                        Convert.ToBoolean(dt[0].CheckService), dt[0].CheckServiceName,
                        Convert.ToBoolean(dt[0].EBT), Convert.ToBoolean(dt[0].MerchantFunding),
                        dt[0].USDANum, dt[0].DebitMonFee,
                        dt[0].DebitTransFee, dt[0].CGMonFee, dt[0].CGTransFee,
                        dt[0].CGMonMin, dt[0].CGDiscRate, dt[0].GiftCardType, dt[0].GCMonFee, dt[0].GCTransFee,
                        dt[0].EBTMonFee, dt[0].EBTTransFee, dt[0].WirelessAccess,
                        dt[0].WirelessTransFee, Convert.ToBoolean(dt[0].Interchange),
                        Convert.ToBoolean(dt[0].Assessments), dt[0].RollingReserve, dt[0].Lease, dt[0].LeaseCompany,
                        dt[0].LeasePayment, dt[0].LeaseTerm, Convert.ToString(dt[0].MCAAmount), Convert.ToString(dt[0].MCAStatus));

                    if (!retVal)
                        return "Cannot add information to ActCust table.";

                    //INSERT P2's EMAIL IN THE EMAIL TABLE         
                    retVal = ACT.InsertUpdateP2Email(ContactID, dt[0].P2Email);

                    //Insert note stating record for updated
                    //ACT.AddUpdateNoteToACT(ContactID, "Merchant Information Updated from the Partner Portal");

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
                            //PartnerLogBL LogData = new PartnerLogBL();
                            //string PortalUserID = LogData.ReturnPortalUserID(Convert.ToInt32(Session["AffiliateID"]));

                            PrevValue = drPreUpdate[i].ToString();
                            NewValue = drPostUpdate[i].ToString();
                            //if the fields do not contain the same VALUE
                            if (PrevValue.ToLower().Replace(" ", "") != NewValue.ToLower().Replace(" ", ""))
                            {
                                //Record a Field Change History in ACT
                                ACT.InsertHistoryFieldChange(ContactID, ColNamePost, PrevValue, NewValue, partnerID);
                            }
                        }
                        //Delete the data for this Contact in the Backup Table created in the Update
                        ACT.DeleteActBackup(ContactID);
                    }

                    retVal = LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Application Updated in ACT!");

                    App.UpdateLastSyncDate();
                }/*
                else { 
                ACTOnlineAppFieldsTableAdapter ACTOnlineAppFieldsAdapter = new ACTOnlineAppFieldsTableAdapter();
                PartnerDS.ACTOnlineAppFieldsDataTable dtACT = ACTOnlineAppFieldsAdapter.GetData(new Guid(ContactID));
                 //Get the Data from the ACT record

                if (dtACT.Rows.Count > 0)
                {
                    string Email = dtACT[0].EMail;
                    string FirstName = dtACT[0].FIRSTNAME;
                    string LastName = dtACT[0].LASTNAME;
                    string Phone = dtACT[0].Phone;
                    string OtherReferral = dtACT[0].OtherReferral;
                    int ReferralID = 0;
                    int NewAppId = AppId;
                    if (dtACT[0].ReferralID != "")
                        ReferralID = Convert.ToInt32(dtACT[0].ReferralID);

                    int PID = 0;

                    NewAppInfo AppInfo = new NewAppInfo(AppId);
                    string Status = AppInfo.ReturnStatus();
                    string StatusGW = AppInfo.ReturnStatusGW();

                    CompanyInfo Company1 = new CompanyInfo(AppId);
                    int companyCom = Company1.CheckCompanyComplete();
                    bool boolCompanyCom = false;
                    if(companyCom == 1)
                    {
                        boolCompanyCom = true;
                    }

                    BusinessInfo Business1 = new BusinessInfo(AppId);
                    int busCom = Business1.CheckBusinessComplete("Merchant");
                    int busComGW = Business1.CheckBusinessComplete("Gateway");
                    bool boolBusCom = false;
                    bool boolBusComGW = false;

                    if(busCom == 1)
                    {
                        boolBusCom = true;
                    }

                    if (busComGW == 1)
                    {
                        boolBusComGW = true;
                    }

                    PrincipalInfo Principal11 = new PrincipalInfo(AppId);
                    int principalCom = Principal11.CheckPrincipalComplete("Merchant");
                    int principalComGW = Principal11.CheckPrincipalComplete("Gateway");
                    bool boolprincipalCom = false;
                    bool boolprincipalComGW = false;

                    if (principalCom == 1)
                    {
                        boolprincipalCom = true;
                    }

                    if (principalComGW == 1)
                    {
                        boolprincipalComGW = true;
                    }

                    Principal2Info Principal22 = new Principal2Info(AppId);
                    int principal2Com = Principal22.CheckPrincipal2Complete("Merchant");
                    bool boolprincipal2Com = false;

                    if (principal2Com == 1)
                    {
                        boolprincipal2Com = true;
                    }

                    CardPCT Card = new CardPCT(AppId);
                    int cardPCTCom = Card.CheckCardPCTComplete();
                    bool boolcardPCTCom = false;

                    if (cardPCTCom == 1)
                    {
                        boolcardPCTCom = true;
                    }


                    OtherInfo other = new OtherInfo(AppId);
                    int otherCom = other.CheckOtherInfoComplete("Merchant");
                    bool boolotherCom = false;

                    if (otherCom == 1)
                    {
                        boolotherCom = true;
                    }

                    BankingInfo Banking1 = new BankingInfo(AppId);
                    int bankCom = Banking1.CheckBankingComplete("Merchant");
                    int bankComGW = Banking1.CheckBankingComplete("Gateway");
                    bool boolbankCom = false;
                    bool boolbankComGW = false;

                    if (bankCom == 1)
                    {
                        boolbankCom = true;
                    }

                    if (bankComGW == 1)
                    {
                        boolbankComGW = true;
                    }


                    ReprogramInfo Reprogram = new ReprogramInfo(AppId);
                    int ReprogramComGW = Reprogram.CheckReprogramComplete();
                    bool boolReprogramComGW = false;

                    if (ReprogramComGW == 1)
                    {
                        boolReprogramComGW = true;
                    }

                    //Insert Data in OnlineAppNewApp  
                    //First get repnum based on Rep Name in the ACT Record
                    RepInfoDL Rep = new RepInfoDL();
                    string strRepNumber = Rep.ReturnMasterNum(dtACT[0].RepName);
                    OnlineAppDL OnlineApp = new OnlineAppDL();

                    OnlineAppClassLibrary.CompanyInfo Company = new OnlineAppClassLibrary.CompanyInfo(NewAppId);
                    retVal = Company.UpdateCompanyInfo(dtACT[0].Company, dtACT[0].CustServPhone,
                        dtACT[0].DBA, dtACT[0].CompanyAddress, dtACT[0].CompanyAddress2,
                        dtACT[0].CompanyCity, dtACT[0].CompanyState, "",
                        dtACT[0].CompanyZip, dtACT[0].CompanyCountry, dtACT[0].YABL, dtACT[0].MABL,
                        dtACT[0].BusinessHours, dtACT[0].BusinessPhone, dtACT[0].BusinessPhoneExt,
                        dtACT[0].BusinessFax, dtACT[0].Website, boolCompanyCom);

                    if (!retVal)
                        return "Could not insert data in OnlineAppCompanyInfo.";

                    //Insert data in OnlineAppCardPCT
                    OnlineAppClassLibrary.CardPCT CardPCT = new OnlineAppClassLibrary.CardPCT(NewAppId);
                    retVal = CardPCT.UpdateCardPCT(dtACT[0].Retail,
                        dtACT[0].Restaurant, dtACT[0].MailOrder, dtACT[0].Internet,
                        dtACT[0].Swiped, dtACT[0].KeyedWith, dtACT[0].KeyedWithout,
                        dtACT[0].Service, dtACT[0].Others, boolcardPCTCom);

                    if (!retVal)
                        return "Could not insert data in OnlineAppCardPCT.";

                    //Insert data in OnlineAppBusinessInfo
                    OnlineAppClassLibrary.BusinessInfo Business = new OnlineAppClassLibrary.BusinessInfo(NewAppId);
                    Business.UpdateBusinessInfo(dtACT[0].BillingAddress,
                        dtACT[0].BillingAddress2, dtACT[0].BillingCity, dtACT[0].BillingState,
                        "", dtACT[0].BillingZip, dtACT[0].BillingCountry, dtACT[0].TaxID,
                        dtACT[0].YearsInBusiness, dtACT[0].MonthsinBusiness, dtACT[0].NumberOfLocations,
                        dtACT[0].TypeOwnership, dtACT[0].TypeProduct, dtACT[0].NumDaysDelivered,
                        dtACT[0].AddlComments, dtACT[0].RefundID, dtACT[0].OtherRefund,
                        dtACT[0].FiledBankruptcy, dtACT[0].Processed, dtACT[0].PrevProcessor, "",
                        dtACT[0].PrevMerchantAcctNo, 0, dtACT[0].ReasonForLeaving, dtACT[0].Terminated, boolBusCom);

                    if (!retVal)
                        return "Could not insert data in OnlineAppBusinessInfo.";

                    //Insert data in OnlineAppPrincipalInfo
                    OnlineAppClassLibrary.PrincipalInfo Principal1 = new OnlineAppClassLibrary.PrincipalInfo(NewAppId);
                    string hasSecondPrincipal = "No";
                    if (dtACT[0].P1OwnershipPercent.ToString().Trim() != "100")
                        hasSecondPrincipal = "Yes";
                    retVal = Principal1.UpdatePrincipal1Info(dtACT[0].P1FirstName,
                        dtACT[0].P1LastName, dtACT[0].P1MidName, Email, dtACT[0].P1Title,
                        dtACT[0].P1Address, "", dtACT[0].P1State, dtACT[0].P1City,
                        dtACT[0].P1ZipCode, "", dtACT[0].P1Country, dtACT[0].P1YearsAtAddress, "",
                        dtACT[0].P1PhoneNumber, dtACT[0].P1MobilePhone, dtACT[0].P1DriversLicenseNo, dtACT[0].P1DriversLicenseState,
                        dtACT[0].P1DriversLicenseExpiry, dtACT[0].P1DOB, dtACT[0].P1LivingStatus, dtACT[0].P1OwnershipPercent,
                        dtACT[0].P1SSN, boolprincipalCom, hasSecondPrincipal);
                    if (!retVal)
                        return "Could not insert data in OnlineAppPrincipalInfo.";

                    //Insert data in OnlineAppPrincipal2Info
                    if (dtACT[0].P1OwnershipPercent.ToString().Trim() != "100")
                    {
                        OnlineAppClassLibrary.Principal2Info Principal2 = new OnlineAppClassLibrary.Principal2Info(NewAppId);
                        retVal = Principal2.UpdatePrincipal2Info(dtACT[0].P2FirstName,
                            dtACT[0].P2LastName, "", dtACT[0].P2Email, dtACT[0].P2Title,
                            dtACT[0].P2Address, "", dtACT[0].P2State, dtACT[0].P2City,
                            dtACT[0].P2ZipCode, "", dtACT[0].P2Country, dtACT[0].P2YearsAtAddress, "",
                            dtACT[0].P2PhoneNumber, dtACT[0].P2MobilePhone, dtACT[0].P2DriversLicenseNo, dtACT[0].P2DriversLicenseState,
                            dtACT[0].P2DriversLicenseExpiry, dtACT[0].P2DOB, dtACT[0].P2LivingStatus, dtACT[0].P2OwnershipPercent,
                            dtACT[0].P2SSN, boolprincipal2Com);

                        if (!retVal)
                            return "Could not insert data in OnlineAppPrincipal2Info.";
                    }

                    string CardPresent = "";
                    int SwipedPCT = 0;
                    if (dtACT[0].Swiped != "")
                        SwipedPCT = Convert.ToInt32(dtACT[0].Swiped);

                    if (SwipedPCT >= 70)
                        CardPresent = "CP";
                    else
                        CardPresent = "CNP";

                    if (!Convert.IsDBNull(dtACT[0].DiscountPaid))
                    {
                        if (Convert.ToString(dtACT[0].DiscountPaid) == "Monthly")
                        {
                            DiscountPaid = "Monthly";
                        }
                    }

                    //Insert data in OnlineAppProcessing only if a Processor is selected
                    if (dtACT[0].Processor.ToString().Trim() != "")
                    {
                        OnlineAppClassLibrary.ProcessingInfo Proc = new OnlineAppClassLibrary.ProcessingInfo(NewAppId);
                        retVal = Proc.UpdateProcessingInfo(dtACT[0].Processor, CardPresent,
                            dtACT[0].CustServFee, dtACT[0].InternetStmt, dtACT[0].TransactionFee, dtACT[0].DRQualPres,
                            dtACT[0].DRQualNP, dtACT[0].DRMidQual, dtACT[0].DRNonQual,
                            dtACT[0].DRQualDebit, dtACT[0].ChargebackFee, dtACT[0].RetrievalFee,
                            dtACT[0].VoiceAuth, dtACT[0].BatchHeader, dtACT[0].AVS,
                            dtACT[0].MonMin, dtACT[0].NBCTransFee, dtACT[0].AnnualFee,
                            dtACT[0].WirelessAccessFee, dtACT[0].WirelessTransFee,
                            "", "", dtACT[0].DebitMonFee,
                            dtACT[0].DebitTransFee, dtACT[0].CGMonFee,
                            dtACT[0].CGTransFee, dtACT[0].CGMonMin,
                            dtACT[0].CGDiscRate, dtACT[0].GCMonFee,
                            dtACT[0].GCTransFee, dtACT[0].EBTMonFee,
                            dtACT[0].EBTTransFee, DiscountPaid, ComplianceFee, "");

                        NewAppTable newAppTable = new NewAppTable();
                        newAppTable.setRatesUpdatedBit(NewAppId, true);

                        if (!retVal)
                            return "Could not insert data in OnlineAppProcessing.";

                        retVal = Proc.InsertUpdateCheckServiceName(dtACT[0].CheckService);

                        if (!retVal)
                            return "Could not insert Check Service.";

                        OnlineAppProcessingBL Processing = new OnlineAppProcessingBL(NewAppId);
                        Processing.UpdateOtherProcessing(Convert.ToBoolean(dtACT[0].Interchange), Convert.ToBoolean(dtACT[0].BillingAssessments), dtACT[0].RollingReserve.ToString());
                        //Set the Last Sync Date since it is an exported App
                        Processing.UpdateLastSyncDate();
                        if (!retVal)
                            return "Could not insert other Processing data in OnlineAppProcessing.";
                    }

                    if (dtACT[0].Gateway.ToString().Trim() != "")
                    {
                        OnlineAppClassLibrary.Gateway Gateway = new OnlineAppClassLibrary.Gateway(NewAppId);
                        retVal = Gateway.UpdateGatewayInfo(
                            dtACT[0].Gateway, dtACT[0].GatewayMonFee,
                            "", dtACT[0].GatewayTransFee);

                        if (!retVal)
                            return "Could not insert data in OnlineAppGateway.";
                    }

                    //Insert data in OnlineAppBankingInfo
                    OnlineAppClassLibrary.BankingInfo Banking = new OnlineAppClassLibrary.BankingInfo(NewAppId);
                    retVal = Banking.UpdateBankingInfo(dtACT[0].BankName, "",
                        dtACT[0].BankAddress, dtACT[0].BankZip, dtACT[0].BankCity,
                        dtACT[0].BankState, "", dtACT[0].CompanyCountry, dtACT[0].Company, dtACT[0].BankAccountNumber,
                        dtACT[0].BankRoutingNumber, dtACT[0].BankPhone, boolbankCom);
                    if (!retVal)
                        return "Could not insert data in OnlineAppBankingInfo.";

                    //Insert data in OnlineAppOtherInfo
                    string AmexApplied = "";
                    string AmexNum = "";
                    long result = 0;
                    if (Int64.TryParse(dtACT[0].AmexNum.ToString(), out result))
                    {
                        AmexNum = dtACT[0].AmexNum.ToString();
                        AmexApplied = "Yes - Existing";
                    }
                    else if ((dtACT[0].AmexNum.ToString().ToLower().Contains("yes")) || (dtACT[0].AmexNum.ToString().ToLower().Contains("submit")))
                    {
                        AmexApplied = "Yes";
                    }
                    else if ((dtACT[0].AmexNum.ToString().ToLower().Contains("opted")) || (dtACT[0].AmexNum.ToString().ToLower().Contains("declined")) || (dtACT[0].AmexNum.ToString().ToLower().Contains("cancelled")))
                    {
                        AmexApplied = "No";
                    }
                    else {
                        AmexApplied = "";
                    }

                    string DiscApplied = "";
                    string DiscNum = "";
                    if (Int64.TryParse(dtACT[0].DiscoverNum.ToString(), out result))
                    {
                        DiscNum = dtACT[0].DiscoverNum.ToString();
                        DiscApplied = "Yes - Existing";
                    }
                    else if ((dtACT[0].DiscoverNum.ToString().ToLower().Contains("yes")) || (dtACT[0].DiscoverNum.ToString().ToLower().Contains("submit")))
                        DiscApplied = "Yes";
                    else if ((dtACT[0].DiscoverNum.ToString().ToLower().Contains("opted")) || (dtACT[0].DiscoverNum.ToString().ToLower().Contains("declined")) || (dtACT[0].DiscoverNum.ToString().ToLower().Contains("cancelled")) || (dtACT[0].DiscoverNum.ToString().ToLower().Contains("International")) || (dtACT[0].DiscoverNum.ToString().ToLower().Contains("MAP")))
                        DiscApplied = "No";
                    else DiscApplied = "";

                    string JCBApplied = "";
                    string JCBNum = "";
                    if (Int64.TryParse(dtACT[0].JCBNum.ToString(), out result))
                    {
                        JCBNum = dtACT[0].JCBNum.ToString();
                        JCBApplied = "Yes - Existing";
                    }
                    else if ((dtACT[0].JCBNum.ToString().ToLower().Contains("yes")) || (dtACT[0].JCBNum.ToString().ToLower().Contains("submit")))
                        JCBApplied = "Yes";
                    else if ((dtACT[0].JCBNum.ToString().ToLower().Contains("opted")) || (dtACT[0].JCBNum.ToString().ToLower().Contains("declined")) || (dtACT[0].JCBNum.ToString().ToLower().Contains("cancelled")))
                        JCBApplied = "No";
                    else JCBApplied = "";

                    OnlineAppClassLibrary.OtherInfo OtherInfo = new OnlineAppClassLibrary.OtherInfo(NewAppId);
                    retVal = OtherInfo.UpdateOtherInfo(DiscApplied, AmexApplied, JCBApplied, DiscNum, AmexNum, "",
                        dtACT[0].MaxTicket, dtACT[0].AvgTicket, dtACT[0].MonVol, boolotherCom);

                    if (!retVal)
                        return "Could not insert data in OnlineAppOtherInfo.";

                    //Export Numbers to NBC Table
                    if ((dtACT[0].DiscoverNum != "") || (dtACT[0].AmexNum != "") || (dtACT[0].JCBNum != ""))
                    {
                        retVal = OnlineApp.InsertUpdateNBC(NewAppId.ToString(), dtACT[0].DiscoverNum,
                             dtACT[0].AmexNum, dtACT[0].JCBNum);
                    }
                    if (!retVal)
                        return "Could not insert data in OnlineAppNonBankcard.";

                    //If Platform Info Exists in Act, update Reprogram table
                    if ((dtACT[0].Platform != "") || (dtACT[0].VisaMasterNum != ""))
                    {
                        OnlineAppClassLibrary.ReprogramInfo RPG = new OnlineAppClassLibrary.ReprogramInfo(NewAppId);
                        RPG.UpdateReprogramInfo(dtACT[0].Platform,
                            "",//dt[0].VisaMasterNum - since two accounts will never have same merchant number
                            dtACT[0].MerchantID,
                            "", 
                            "", //PFLoginID
                            dtACT[0].BIN,
                            dtACT[0].AgentBankNum,
                            dtACT[0].AgentChainNum,
                            dtACT[0].MCCCode,
                            dtACT[0].StoreNum,
                            boolReprogramComGW, true);
                    }

                    //set the Last Sync Date for the App again after all tables are created
                    OnlineApp.UpdateLastSyncDate(NewAppId);

                    //Get Data from SalesOpps in ACT

                    ACTOnlineAppSalesOppsTableAdapter ACTOnlineAppSalesOppsAdapter = new ACTOnlineAppSalesOppsTableAdapter();
                    PartnerDS.ACTOnlineAppSalesOppsDataTable dtSalesOpps = ACTOnlineAppSalesOppsAdapter.GetDataByContact(new Guid(ContactID));
       
                    //Create a Sales Opp Object
                    SalesOppDL SalesOpp = new SalesOppDL();
                    if (dtSalesOpps.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSalesOpps.Rows.Count; i++)
                        {
                            //Insert data in OnlineAppSalesOpps          
                            //Get RepNum Based on RepName
                            string strRepNum = Rep.ReturnMasterNum(dtSalesOpps[i].RepName);
                            retVal = SalesOpp.InsertUpdateSalesOpps(NewAppId.ToString(), dtSalesOpps[i].CODE,
                                dtSalesOpps[i].UNITPRICE, dtSalesOpps[i].UNITCOST, dtSalesOpps[i].QUANTITY,
                                dtSalesOpps[i].TerminalID, dtSalesOpps[i].SerialNumber, dtSalesOpps[i].STATUS,
                                "{" + dtSalesOpps[i].CREATEUSERID + "}", "{" + dtSalesOpps[i].ID + "}",
                                strRepNum, dtSalesOpps[i].Stage, dtSalesOpps[i].LastModified,
                                dtSalesOpps[i].CREATEDATE, dtSalesOpps[i].Reprogram, dtSalesOpps[i].PaymentMethod);
                            if (!retVal)
                                return "Could not insert data in OnlineAppSalesOpps.";
                        }                        
                    }//end if count not 0

                    retVal = LogData.InsertLogData(AppId, Convert.ToInt32(HttpContext.Current.Session["AffiliateID"]), "Application Updated from ACT!");

                    string strretVal = ExportACTStatus(NewAppId, Convert.ToInt32(dt[0].AcctType));
                }
                else
                    return "Record not found.";

                }*/

                return "Update Successful";
            }//end if row count not 0
            return "Update Unsuccessful";
        }//end function UpdateAct
        #endregion

        #region UPDATE RATES
        //This function updates Rates only. CALLED BY Edit.aspx
        public bool UpdateRatesInACT(int AppId, int partnerID)
        {
            OnlineAppDL App = new OnlineAppDL();
            OnlineAppACTFieldsTableAdapter OnlineAppACTFieldsAdapter = new OnlineAppACTFieldsTableAdapter();
            PartnerDS.OnlineAppACTFieldsDataTable dt = OnlineAppACTFieldsAdapter.GetData(Convert.ToInt16(AppId));
            bool retVal = false;
            if (dt.Rows.Count > 0)
            {
                //Get ContactID for the record to be updated
                ACTDataDL ACT= new ACTDataDL();
                //Get ContactID that was just inserted in TBL_CONTACT in ACT
                string ContactID = ACT.ReturnContactID(AppId);
                if (ContactID == "")
                    return false;


                //Insert information into Backup Table before updating
                ACT.InsertActRecordBackup(ContactID);

                retVal = ACT.UpdateRatesInACT
                    (dt[0].Gateway,dt[0].Processor,
                     dt[0].RepNum, dt[0].AnnualFee,
                    dt[0].CustServFee, dt[0].MonMin, dt[0].InternetStmt,
                    dt[0].DiscQNP, dt[0].DiscMQ,
                    dt[0].DiscNQ, dt[0].DiscQD,
                    dt[0].DiscQP, dt[0].TransFee,
                    dt[0].RetrievalFee, dt[0].RollingReserve,
                    dt[0].VoiceAuth,
                    dt[0].BatchHeader, dt[0].AVS,
                    dt[0].NBCTFee, dt[0].CBFee,
                    Convert.ToInt32(dt[0].AcctType), dt[0].GWMonFee,
                    dt[0].GWTransFee, dt[0].GWSetupFee,
                    Convert.ToBoolean(dt[0].OnlineDebit), Convert.ToBoolean(dt[0].GiftCard),
                    Convert.ToBoolean(dt[0].CheckService), Convert.ToBoolean(dt[0].EBT),
                    dt[0].DebitMonFee, dt[0].DebitTransFee,
                    dt[0].CGMonFee, dt[0].CGTransFee,
                    dt[0].CGMonMin, dt[0].CGDiscRate,
                    dt[0].GCMonFee, dt[0].GCTransFee,
                    dt[0].EBTMonFee, dt[0].EBTTransFee,
                    dt[0].CheckServiceName, dt[0].WirelessAccess,
                    dt[0].WirelessTransFee, Convert.ToBoolean(dt[0].Interchange), 
                    Convert.ToBoolean(dt[0].Assessments), AppId);


                //Check to see which fields were changed to record histories
                //The Update Stored Procedures have created data in the Backup Tables
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
                        if (PrevValue != NewValue)
                        {
                            //Record a Field Change History in ACT
                            ACT.InsertHistoryFieldChange(ContactID, ColNamePost, PrevValue, NewValue, partnerID);
                        }
                    }
                }
                //Delete the data for this Contact in the Backup Table created in the Update
                ACT.DeleteActBackup(ContactID);

                //ACT.AddUpdateNoteToACT(ContactID, "Rates Updated from the Partner Portal");
            }
            return retVal;
        }//end function UpdateRatesInACT
        #endregion

        #region AddUpdateNoteToACT
        public void AddUpdateNoteToACT(int AppId, string NoteText)
        {
            ACTDataDL ACT = new ACTDataDL();
            string ContactID = ACT.ReturnContactID(AppId);
            ACT.AddUpdateNoteToACT(ContactID, NoteText);
        }

        #endregion

        #region CHECK RECORD EXISTS
        //Checks if a particular Online App Record Exists already in ACT via various fields
        //CALLED BY Edit.aspx
        public string CheckRecordExists(int AppId)
        {
            string strRetVal = "Error";
            OnlineAppBL OnlineApp = new OnlineAppBL(AppId);
            DataSet ds = OnlineApp.GetOnlineAppProfile();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ACTDataDL ACT = new ACTDataDL();
                //Checks if record already exists in agent portal DB and returns a string if match found.
                strRetVal = ACT.CheckRecordExists(
                    dr["FirstName"].ToString(),
                    dr["LastName"].ToString(),
                    dr["P1FirstName"].ToString(),
                    dr["P1LastName"].ToString(),
                    dr["SignupEmail"].ToString(),
                    dr["P1Email"].ToString(),
                    dr["DBA"].ToString(),
                    dr["CompanyName"].ToString(),
                    dr["BusinessPhone"].ToString());
            }//end if count not 0
            return strRetVal;
        }//end function CheckRecordExists
        #endregion
    }//end class ExportActBL
}
