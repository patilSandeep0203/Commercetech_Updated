using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;
using OnlineAppClassLibrary;
//using DLPartner.OnlineAppProcessingDL;


namespace BusinessLayer
{
    public class OnlineAppProcessingBL
    {
        private int AppId = 0;
        private OnlineAppECheckRatesTableAdapter _OnlineAppECheckRatesAdapter = null;
        protected OnlineAppECheckRatesTableAdapter ECheckAdapter
        {
            get
            {
                if (_OnlineAppECheckRatesAdapter == null)
                    _OnlineAppECheckRatesAdapter = new OnlineAppECheckRatesTableAdapter();

                return _OnlineAppECheckRatesAdapter;
            }
        }
        private OnlineAppRatesTableAdapter _OnlineAppRatesAdapter = null;
        protected OnlineAppRatesTableAdapter RatesAdapter
        {
            get
            {
                if (_OnlineAppRatesAdapter == null)
                    _OnlineAppRatesAdapter = new OnlineAppRatesTableAdapter();

                return _OnlineAppRatesAdapter;
            }
        }

        //Constructor
        public OnlineAppProcessingBL()
        {
        }

        public OnlineAppProcessingBL(int AppId)
        {
            this.AppId = AppId;
        }


        public string GetPackageComplianceFee(int PID)
        {
            string strComplianceFee = string.Empty;
            //bool retVal = false;
            //First get package information for PID
            OnlineAppClassLibrary.PackageInfo PackInfo = new OnlineAppClassLibrary.PackageInfo();
            DataSet dsPackInfo = PackInfo.GetPackageInfo(PID);
            if (dsPackInfo.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsPackInfo.Tables[0].Rows[0];
                strComplianceFee = dr["ComplianceFee"].ToString().Trim();
            }

            return strComplianceFee;

        }

        public string ApplyPackageCheckOnlineDebitEBT(int PID, bool chkEBT, bool chkOnlineDebit)
        {
            bool retVal = false;
            //First get package information for PID
            OnlineAppClassLibrary.PackageInfo PackInfo = new OnlineAppClassLibrary.PackageInfo();
            DataSet dsPackInfo = PackInfo.GetPackageInfo(PID);

            

            string OnlineDebitMonthFee = "";
            string OnlineDebitTransFee = "";
            string EBTMonthFee = "";
            string EBTTransFee = "";
            string DiscountPaid = "Monthly";
            string ComplianceFee = "";


            if (dsPackInfo.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsPackInfo.Tables[0].Rows[0];
                if (chkOnlineDebit)
                {
                    OnlineDebitMonthFee = dr["DebitMonFee"].ToString().Trim();
                    OnlineDebitTransFee = dr["DebitTransFee"].ToString().Trim();
                }

                if (chkEBT)
                {
                    EBTMonthFee = dr["EBTMonFee"].ToString().Trim();
                    EBTTransFee = dr["EBTTransFee"].ToString().Trim();
                }
                string CardPresent = dr["CardPresent"].ToString().Trim();
                if (CardPresent == "CP")
                {
                    //Check the Processing Pct on the current App
                    //Get the Card Present Swiped Minimum allowed for a Retail Account by that Processor
                    //Get SwipedPCT from OnlineAppCardPCT
                    string SwipedPCT = ReturnSwipedPercent();
                    if (SwipedPCT != "")
                    {
                        if (Convert.ToInt32(SwipedPCT) < GetProcessingPCT(dr["Processor"].ToString().Trim()))
                            //return "error";
                        return "Rate Package could not be applied. Card Swiped Percentage may be set too low for a Card Present account specified in the Package.";
                    }
                }//end if card present is CP

                ComplianceFee = GetPackageComplianceFee(PID);

                if (Convert.ToString(dr["Processor"]).Contains("Intuit"))
                {
                    DiscountPaid = "Daily";
                }

                //Update Processing Table
                OnlineAppClassLibrary.ProcessingInfo Processing = new OnlineAppClassLibrary.ProcessingInfo(AppId);
                retVal = Processing.UpdateProcessingInfo(dr["Processor"].ToString().Trim(),
                    dr["CardPresent"].ToString().Trim(), dr["CustServFee"].ToString().Trim(),
                    dr["InternetStmt"].ToString().Trim(), dr["TransactionFee"].ToString().Trim(),
                    dr["DiscRateQualPres"].ToString().Trim(), dr["DiscRateQualNP"].ToString().Trim(),
                    dr["DiscRateMidQual"].ToString().Trim(), dr["DiscRateNonQual"].ToString().Trim(),
                    dr["DiscRateQualDebit"].ToString().Trim(),
                    dr["DebitQualNP"].ToString().Trim(), dr["DebitMidQual"].ToString().Trim(),
                    dr["DebitNonQual"].ToString().Trim(),
                    dr["AmexDiscRateQual"].ToString().Trim(), dr["AmexDiscRateMidQual"].ToString().Trim(), dr["AmexDiscRateNonQual"].ToString().Trim(),  dr["ChargebackFee"].ToString().Trim(),
                    dr["RetrievalFee"].ToString().Trim(), dr["VoiceAuth"].ToString().Trim(),
                    dr["BatchHeader"].ToString().Trim(), dr["AVS"].ToString().Trim(),
                    dr["MonMin"].ToString().Trim(), dr["NBCTransFee"].ToString().Trim(),
                    dr["AnnualFee"].ToString().Trim(), dr["WirelessAccessFee"].ToString().Trim(),
                    dr["WirelessTransFee"].ToString().Trim(), dr["AppSetupFee"].ToString().Trim(),
                    dr["AppFee"].ToString().Trim(),
                    OnlineDebitMonthFee,
                    OnlineDebitTransFee, dr["CGMonFee"].ToString().Trim(),
                    dr["CGTransFee"].ToString().Trim(), dr["CGMonMin"].ToString().Trim(),
                    dr["CGDiscRate"].ToString().Trim(), dr["GCMonFee"].ToString().Trim(),
                    dr["GCTransFee"].ToString().Trim(), EBTMonthFee,
                    EBTTransFee, DiscountPaid, Convert.ToString(dr["ContractTerm"]).Trim(), ComplianceFee, "");

                OnlineAppClassLibrary.Gateway GatewayRates = new OnlineAppClassLibrary.Gateway(AppId);
                retVal = GatewayRates.UpdateGatewayInfo(
                    dr["Gateway"].ToString().Trim(), dr["GatewayMonFee"].ToString().Trim(),
                    dr["GatewaySetupFee"].ToString().Trim(), dr["GatewayTransFee"].ToString().Trim());

                OnlineAppProcessingDL Proc = new OnlineAppProcessingDL();
                int iRetVal = Proc.UpdateOtherProcessing(AppId, Convert.ToBoolean(dr["Interchange"]), Convert.ToBoolean(dr["Assessments"]),
                    dr["RollingReserve"].ToString().Trim());

                //Update PID in OnlineAppNewApp
                OnlineAppClassLibrary.NewAppInfo App = new OnlineAppClassLibrary.NewAppInfo(AppId);
                retVal = App.UpdatePID(PID);

                if (retVal)
                    return "Package applied successfully";
                else
                    return "Package could not be applied";
            }//end if count not 0   
            return "Package could not be applied";
        }

        //This function Applied selected package
        public string ApplyPackage(int PID)
        {
            bool retVal = false;
            //First get package information for PID
            OnlineAppClassLibrary.PackageInfo PackInfo = new OnlineAppClassLibrary.PackageInfo();
            DataSet dsPackInfo = PackInfo.GetPackageInfo(PID);

            string DiscountPaid = "Monthly";
            string ComplianceFee = "";

            string OnlineDebitMonthFee = "";
            string OnlineDebitTransFee = "";
            string EBTMonthFee = "";
            string EBTTransFee = "";


            if (dsPackInfo.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsPackInfo.Tables[0].Rows[0];
                string CardPresent = dr["CardPresent"].ToString().Trim();
                if (CardPresent == "CP")
                {
                    //Check the Processing Pct on the current App
                    //Get the Card Present Swiped Minimum allowed for a Retail Account by that Processor
                    //Get SwipedPCT from OnlineAppCardPCT
                    string SwipedPCT = ReturnSwipedPercent();
                    if (SwipedPCT != "")
                    {
                        if (Convert.ToInt32(SwipedPCT) < GetProcessingPCT(dr["Processor"].ToString().Trim()))
                            return "error";
                            //return "Rate Package could not be applied. Card Swiped Percentage may be set too low for a Card Present account specified in the Package.";
                    }
                }//end if card present is CP

                if (Convert.ToString(dr["Processor"]).Contains("Intuit"))
                {
                    DiscountPaid = "Daily";
                }

                //Update Processing Table
                OnlineAppClassLibrary.ProcessingInfo Processing = new OnlineAppClassLibrary.ProcessingInfo(AppId);
                retVal = Processing.UpdateProcessingInfo( dr["Processor"].ToString().Trim(),
                    dr["CardPresent"].ToString().Trim(), dr["CustServFee"].ToString().Trim(),
                    dr["InternetStmt"].ToString().Trim(), dr["TransactionFee"].ToString().Trim(),
                    dr["DiscRateQualPres"].ToString().Trim(), dr["DiscRateQualNP"].ToString().Trim(),
                    dr["DiscRateMidQual"].ToString().Trim(), dr["DiscRateNonQual"].ToString().Trim(),
                    dr["DiscRateQualDebit"].ToString().Trim(),
                    dr["DebitQualNP"].ToString().Trim(), dr["DebitMidQual"].ToString().Trim(),
                    dr["DebitNonQual"].ToString().Trim(),
                    dr["AmexDiscRateQual"].ToString().Trim(), dr["AmexDiscRateMidQual"].ToString().Trim(), dr["AmexDiscRateNonQual"].ToString().Trim(), 
                    dr["ChargebackFee"].ToString().Trim(),
                    dr["RetrievalFee"].ToString().Trim(), dr["VoiceAuth"].ToString().Trim(),
                    dr["BatchHeader"].ToString().Trim(), dr["AVS"].ToString().Trim(),
                    dr["MonMin"].ToString().Trim(), dr["NBCTransFee"].ToString().Trim(),
                    dr["AnnualFee"].ToString().Trim(), dr["WirelessAccessFee"].ToString().Trim(),
                    dr["WirelessTransFee"].ToString().Trim(), dr["AppSetupFee"].ToString().Trim(),
                    dr["AppFee"].ToString().Trim(),
                    dr["DebitMonFee"].ToString().Trim(),
                    dr["DebitTransFee"].ToString().Trim(), dr["CGMonFee"].ToString().Trim(),
                    dr["CGTransFee"].ToString().Trim(), dr["CGMonMin"].ToString().Trim(),
                    dr["CGDiscRate"].ToString().Trim(), dr["GCMonFee"].ToString().Trim(),
                    dr["GCTransFee"].ToString().Trim(), dr["EBTMonFee"].ToString().Trim(),
                    dr["EBTTransFee"].ToString().Trim(), DiscountPaid, Convert.ToString(dr["ContractTerm"]).Trim(), ComplianceFee, "");

                OnlineAppClassLibrary.Gateway GatewayRates = new OnlineAppClassLibrary.Gateway(AppId);
                retVal = GatewayRates.UpdateGatewayInfo(
                    dr["Gateway"].ToString().Trim(), dr["GatewayMonFee"].ToString().Trim(),
                    dr["GatewaySetupFee"].ToString().Trim(), dr["GatewayTransFee"].ToString().Trim());

                OnlineAppProcessingDL Proc = new OnlineAppProcessingDL();
                int iRetVal = Proc.UpdateOtherProcessing(AppId, Convert.ToBoolean(dr["Interchange"]), Convert.ToBoolean(dr["Assessments"]), 
                    dr["RollingReserve"].ToString().Trim());

                //Update PID in OnlineAppNewApp
                OnlineAppClassLibrary.NewAppInfo App = new OnlineAppClassLibrary.NewAppInfo(AppId);
                retVal = App.UpdatePID(PID);

                if (retVal)
                    return "Package applied successfully";
                else
                    return "Package could not be applied";
            }//end if count not 0   
            return "Package could not be applied";
        }//end function ApplyPackage

        //This function applies rates to application
        public bool ApplyRates(string Processor,
                string CardPresent, string CustServFee, 
                string InternetStmt, string TransactionFee,
                string DiscRateQualPres, string DiscRateQualNP,
                string DiscRateMidQual, string DiscRateNonQual,
                string DiscRateQualDebit, string QualNPDebit, string MidQualDebit, string NonQualDebit, string AmexDiscRateQual, string AmexDiscRateMidQual, string AmexDiscRateNonQual, string ChargebackFee,
                string RetrievalFee, string VoiceAuth,
                string BatchHeader, string AVS,
                string MonMin, string NBCTransFee,
                string AnnualFee, string WirelessAccessFee,
                string WirelessTransFee, string AppSetupFee, string AppFee, 
                string RollingReserve, string GatewayMonFee, string GatewayTransFee,
                string GatewaySetupFee, string Gateway, string DebitMonFee, string DebitTransFee,
                string CheckService, string CGDiscRate, string CGMonFee, string CGMonMin, string CGTransFee,
                string GCType, string GCMonFee, string GCTransFee, string EBTMonFee,
                string EBTTransFee, string LeaseCompany, string LeasePayment, string LeaseTerm,
                string PayrollType, string MCAType, string DiscountPaid, string ContractTerm, string ComplianceFee, string PindebitDiscount)
        {
            //Update Processing Table
            OnlineAppClassLibrary.ProcessingInfo Processing = new OnlineAppClassLibrary.ProcessingInfo(AppId);
            bool retVal = Processing.UpdateProcessingInfo(Processor,
                CardPresent, CustServFee, InternetStmt, TransactionFee,
                DiscRateQualPres, DiscRateQualNP,
                DiscRateMidQual, DiscRateNonQual,
                DiscRateQualDebit, QualNPDebit, MidQualDebit, NonQualDebit, AmexDiscRateQual, AmexDiscRateMidQual, AmexDiscRateNonQual,  ChargebackFee,
                RetrievalFee, VoiceAuth,
                BatchHeader, AVS,
                MonMin, NBCTransFee,
                AnnualFee, WirelessAccessFee,
                WirelessTransFee, AppSetupFee, 
                AppFee, DebitMonFee, DebitTransFee, CGMonFee, 
                CGTransFee, CGMonMin, CGDiscRate,
                GCMonFee, GCTransFee, EBTMonFee, EBTTransFee, DiscountPaid, ContractTerm, ComplianceFee, PindebitDiscount);

            OnlineAppClassLibrary.Gateway GatewayInfo = new OnlineAppClassLibrary.Gateway(AppId);
            retVal = GatewayInfo.UpdateGatewayInfo(Gateway, GatewayMonFee, GatewaySetupFee, GatewayTransFee);
                        
            OnlineAppClassLibrary.NewAppInfo AddlServ = new OnlineAppClassLibrary.NewAppInfo(AppId);
            //retVal = AddlServ.UpdateCheckService(CheckService, Convert.ToDecimal(CGDiscRate), Convert.ToDecimal(CGMonFee), 
                //Convert.ToDecimal(CGTransFee), Convert.ToDecimal(CGMonMin) );

            retVal = AddlServ.UpdateGiftCardType(GCType);

            //retVal = AddlServ.UpdatePayrollType(PayrollType);

            retVal = AddlServ.UpdateMCAType(MCAType);

            retVal = AddlServ.InsertUpdateLeaseInfo(LeaseCompany, LeasePayment, LeaseTerm);//update lease info

            return retVal;
        }//end function ApplyRates

        public int UpdateOtherProcessing(bool bInterchange, bool bAssessments, string RollingReserve)
        {
            OnlineAppProcessingDL Proc = new OnlineAppProcessingDL();
            int iRetVal = Proc.UpdateOtherProcessing(AppId, bInterchange, bAssessments, RollingReserve);
            return iRetVal;
        }//end function UpdateInterchange

        public int ApplyeCheckRates(string eSIRMonMin, string eSIRTransFee1,
                            string eSIRTransFee2, string eSIRTransFee3,
                            string eSIRTransFee4, string eSIRDiscountRate1,
                            string eSIRDiscountRate2, string eSIRDiscountRate3,
                            string eSIRDiscountRate4, string ePIRMonMin,
                            string ePIRTransFee1, string ePIRDiscountRate1)
        {
            OnlineAppProcessingDL eCheckRates = new OnlineAppProcessingDL();
            int iRetVal = eCheckRates.ApplyeCheckRates(AppId, eSIRMonMin, eSIRTransFee1,
                            eSIRTransFee2, eSIRTransFee3,
                            eSIRTransFee4, eSIRDiscountRate1,
                            eSIRDiscountRate2, eSIRDiscountRate3,
                            eSIRDiscountRate4, ePIRMonMin,
                            ePIRTransFee1, ePIRDiscountRate1);
            return iRetVal;
        }//end function ApplyeCheckRates

        public bool RemoveECheckRates()
        {
            OnlineAppProcessingDL Processing  = new OnlineAppProcessingDL();
            bool RetVal = Processing.RemoveECheckRates(AppId);
            return RetVal;
        }//end function ApplyeCheckRates


        public int GetProcessingPCT(string Processor)
        {
            OnlineAppProcessingDL GetCPCNP = new OnlineAppProcessingDL();
            DataSet dsCPCNP = GetCPCNP.GetCPLow(Processor);
            int CPPctSwipedLow = 50;
            if (dsCPCNP.Tables[0].Rows.Count > 0)
            {
                DataRow drCPCNP = dsCPCNP.Tables[0].Rows[0];
                CPPctSwipedLow = Convert.ToInt32(drCPCNP["CPPctSwipedLow"]);
            }//end if count not 0
            return CPPctSwipedLow;
        }//end function GetProcessingPCT

        //This function returns swiped PCT from OnlineAppCardPCT
        public string ReturnSwipedPercent()
        {
            OnlineAppProcessingDL Swiped = new OnlineAppProcessingDL();
            string SwipedPCT = string.Empty;
            DataSet dsSwiped = Swiped.GetSwipedPCT(AppId);
            if (dsSwiped.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsSwiped.Tables[0].Rows[0];
                SwipedPCT = dr["ProcessPctSwiped"].ToString();
            }
            return SwipedPCT;
        }//end function GetSwipedPercent

        //This function returns processor list - CALLED BY SetRates.aspx, CreatePackage.aspx, ModifyPackage.aspx
        public DataSet GetProcessorNames(string Swiped)
        {
            OnlineAppProcessingDL Processing = new OnlineAppProcessingDL();
            DataSet ds = Processing.GetProcessorNames(Swiped);
            return ds;
        }//end function GetProcessorNames

        //This function returns Gateway List
        //CALLED BY SetRates.aspx, ModifyPackage.aspx, CreatePackage.aspx
        public DataSet GetGateways(string Processor)
        {
            OnlineAppProcessingDL Processing = new OnlineAppProcessingDL();
            DataSet ds = Processing.GetGatewayList(Processor);
            return ds;
        }//end function GetPackages

        //This function Gets Rates
        public PartnerDS.OnlineAppRatesDataTable GetRates()
        {
            OnlineAppDL App = new OnlineAppDL();
            return RatesAdapter.GetData(Convert.ToInt16(AppId) );        
           
        }//end function GetRates

        //This function Gets Rates
        public PartnerDS.OnlineAppECheckRatesDataTable GetECheckRates()
        {
            OnlineAppDL App = new OnlineAppDL();
            return ECheckAdapter.GetData(Convert.ToInt16(AppId));     
        }//end function GetECheckRates

        //CALLED BY CreatePackage.aspx, ModifyPackage.aspx, SetRates.aspx, SetBounds.aspx, SetDefaults.aspx
        public DataSet GetAnnualFee(string Processor, string CardPresent)
        {
            OnlineAppProcessingDL Proc = new OnlineAppProcessingDL();
            DataSet ds = Proc.GetAnnualFees(Processor, CardPresent);
            return ds;
        }//end function GetRates

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public string ReturnProcessor()
        {
            string Processor = "";
            OnlineAppClassLibrary.ProcessingInfo Processing = new OnlineAppClassLibrary.ProcessingInfo(AppId);
            DataSet ds = Processing.GetProcessingInfo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Processor = dr["Processor"].ToString().Trim();
            }
            return Processor;
        }//end function GetProcessor 

        //This function updates Last Sync date
        //CALLED BY Edit.aspx
        public bool UpdateLastSyncDate()
        {
            OnlineAppProcessingDL NewApp = new OnlineAppProcessingDL();
            bool retVal = NewApp.UpdateLastSyncDate(AppId);
            return retVal;
        }//end function UpdateLastSyncDate
    }
     //end class RatesBL
}
