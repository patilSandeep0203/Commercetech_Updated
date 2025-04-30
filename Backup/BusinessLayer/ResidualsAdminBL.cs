using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class ResidualsAdminBL
    {
        private ResidualsReportsTableAdapter _ResidualsReportsAdapter = null;
        protected ResidualsReportsTableAdapter Adapter
        {
            get
            {
                if (_ResidualsReportsAdapter == null)
                    _ResidualsReportsAdapter = new ResidualsReportsTableAdapter();

                return _ResidualsReportsAdapter;
            }
        }
        public ResidualsAdminBL(string month)
        {
            this.Month = month;
        }

        public ResidualsAdminBL()
        {

        }

        private string Month = "";

        //********************Dropped Residuals Functions********************
        public DataSet GetDroppedResiduals(string Report)
        {
            ResidualsDL DroppedResd = new ResidualsDL();
            DataSet ds = DroppedResd.GetDroppedResd(Report, Month);
            return ds;
        }//end function GetDroppedResiduals


        //********************Activated Residuals Functions********************

        //This function returns count for all processors
        public DataSet GetActivatedResiduals(string Report)
        {
            ResidualsDL ActivatedResd = new ResidualsDL();
            DataSet ds = ActivatedResd.GetActivatedResd(Report, Month);
            return ds;
        }//end function GetActivatedResiduals

        public DataSet GetACTResidualStatus(string Status, string Service)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            DataSet ds = ACTResdStatus.GetACTResidualStatus(Status, Service);
            return ds;
        }

        public DataSet GetACTContactIDByMerchantID(string MerchantNum)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            DataSet ds = ACTResdStatus.GetACTContactIDByMerchantID(MerchantNum);
            return ds;
        }

        public DataSet GetACTContactIDByGatewayID(string GatewayID)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            DataSet ds = ACTResdStatus.GetACTContactIDByGatewayID(GatewayID);
            return ds;
        }

        public DataSet GetACTContactIDByDBA(string DBA)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            DataSet ds = ACTResdStatus.GetACTContactIDByDBA(DBA);
            return ds;
        }

        public int UpdateACTResidualStatus(string Service, string Processor, string Month, string Status, string CONTACTID)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            int retVal = ACTResdStatus.UpdateACTResdStatus(Service, Processor, Month, Status, CONTACTID);
            return retVal;
        }

        public int UpdateActivatedStatus(string MerchantNum, string DBA, string Report)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            int retVal = ACTResdStatus.UpdateActivatedStatus(MerchantNum, DBA, Report);
            return retVal;
        }

        public int UpdateDroppedStatus(string MerchantNum, string DBA, string Report)
        {
            ResidualsDL ACTResdStatus = new ResidualsDL();
            int retVal = ACTResdStatus.UpdateDroppedStatus(MerchantNum, DBA, Report);
            return retVal;
        }

        //This function returns zero and negative residuals for the selected month
        public DataSet GetZeroNegativeResiduals()
        {
            ResidualsDL ZeroNegative = new ResidualsDL();
            DataSet ds = ZeroNegative.GetZeroNegativeResiduals(Month);
            return ds;
        }//end function GetZeroNegativeResiduals

        //This function gets residuals summary ordered by Direct Deposit/BillPay - CALLED By ResdSummary.aspx.cs
        public DataSet GetResidualPaymentSummaryByDD(int Employee, int DirectDeposit)
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.GetResidualPaymentSummaryByDD(Employee, DirectDeposit, Month);
            return ds;
        }//end function GetResidualPaymentByDD

        //This function gets residuals summary ordered by Direct Deposit/BillPay - CALLED By ResdSummary.aspx.cs
        public DataSet GetResidualPaymentCalcSummaryByDD(int Employee, int DirectDeposit)
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.GetResidualPaymentCalcSummaryByDD(Employee, DirectDeposit, Month);
            return ds;
        }//end function 

        
        //This function inserts/updates confirmation code - CALLED BY ResdSummary.aspx.cs
        public int InsertUpdateResdConfirm(string AffiliateID, string Code, string Note, decimal CarryOver, decimal Payment, string DatePaid)
        {
            ResidualsDL CCode = new ResidualsDL();
            int iRetVal = CCode.InsertUpdateResdConfirm(AffiliateID, Month, Code, Note,CarryOver, Payment, DatePaid);
            return iRetVal;
        }//end function InsertUpdateResdConfirmationCode

        //This function checks if Residuals exists for a given Processor (Vendor) for a particular month. CALLED BY ResidualsAdmin.aspx, ResidualsPrint.aspx
        public DataSet CheckProcessorExists()
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.CheckExists(Month);
            return ds;
        }//end function CheckProcessorExists

        //
        //CALLED BY ResidualsAdmin.aspx, ResidualsPrint.aspx
        public PartnerDS.ResidualsReportsDataTable GetResidualsReport()
        {
            return Adapter.GetData(Month);
        }//end function Get

        //This function returns count for all processors. CALLED BY ResidualsAdmin.aspx, ResidualsPrint.aspx
        public DataSet GetResidualsCount()
        {
            ResidualsDL Count = new ResidualsDL();
            DataSet ds = Count.GetResidualsCountByMon(Month);
            return ds;
        }//end function ReturnCount

        //Get Totals for all accounts for current and previous months. 
        //CALLED BY ResidualsAdmin.aspx
        public double GetECETotal(string AccountName, string RepNum)
        {
            ResidualsDL Totals = new ResidualsDL();
            double ECETotal = Totals.ReturnECETotalForMonth(AccountName, RepNum, Month);
            return ECETotal;
        }//end function GetECETotal

        //This function returns iPayment residuals by DBA
        public PartnerDS.iPaymentDataTable GetiPayResidualsByDBA (string DBA)
        {
            iPaymentTableAdapter Adapter = new iPaymentTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetiPayResidualsByDBA

        //This function returns Chase residuals by DBA
        public PartnerDS.ChaseDataTable GetChaseResidualsByDBA(string DBA)
        {
            ChaseTableAdapter Adapter = new ChaseTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetChaseResidualsByDBA

        //This function returns ChaseNew residuals by DBA
        public PartnerDS.ChaseNewDataTable GetChaseNewResidualsByDBA(string DBA)
        {
            ChaseNewTableAdapter Adapter = new ChaseNewTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetChaseNewResidualsByDBA

        //This function returns WorldPay residuals by DBA
        public PartnerDS.WPayDataTable GetWPayResidualsByDBA(string DBA)
        {
            WPayTableAdapter Adapter = new WPayTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetWPayResidualsByDBA

        //This function returns Innovative residuals by DBA
        public PartnerDS.InnovativeDataTable GetInnResidualsByDBA(string DBA)
        {
            InnovativeTableAdapter Adapter = new InnovativeTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetInnResidualsByDBA

        //This function returns OptimalCA residuals by DBA
        public PartnerDS.OptimalCADataTable GetOptimalCAResidualsByDBA(string DBA)
        {
            OptimalCATableAdapter Adapter = new OptimalCATableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetOptimalCAResidualsByDBA

        //This function returns Merrick residuals by DBA
        public PartnerDS.MerrickDataTable GetMerrickResidualsByDBA(string DBA)
        {
            MerrickTableAdapter Adapter = new MerrickTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetMerrickResidualsByDBA

        //This function returns iPayment residuals by DBA
        public PartnerDS.iPaymentFBBHDataTable GetiPaymentFBBHResidualsByDBA(string DBA)
        {
            iPaymentFBBHTableAdapter Adapter = new iPaymentFBBHTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetiPaymentFBBHResidualsByDBA

        //This function returns iPayment2 residuals by DBA
        public PartnerDS.iPayment2DataTable GetiPay2ResidualsByDBA(string DBA)
        {
            iPayment2TableAdapter Adapter = new iPayment2TableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetiPay2ResidualsByDBA

        //This function returns iPayment3 residuals by DBA
        public PartnerDS.iPayment3DataTable GetiPay3ResidualsByDBA(string DBA)
        {
            iPayment3TableAdapter Adapter = new iPayment3TableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetiPay3ResidualsByDBA

        //This function returns IMS residuals by DBA
        public PartnerDS.IMSDataTable GetIMSResidualsByDBA(string DBA)
        {
            IMSTableAdapter Adapter = new IMSTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetIMSResidualsByDBA

        //This function returns IPS residuals by DBA
        public PartnerDS.IPSDataTable GetIPSResidualsByDBA(string DBA)
        {
            IPSTableAdapter IPSAdapter = new IPSTableAdapter();
            return IPSAdapter.GetDataByDBA(DBA);
        }//end function GetIMS2ResidualsByDBA

        //This function returns IMS2 residuals by DBA
        public PartnerDS.IMS2DataTable GetIMS2ResidualsByDBA(string DBA)
        {
            IMS2TableAdapter IMS2Adapter = new IMS2TableAdapter();
            return IMS2Adapter.GetDataByDBA(DBA);
        }//end function GetIMS2ResidualsByDBA

        //This function returns Sage residuals by DBA
        public PartnerDS.SageDataTable GetSageResidualsByDBA(string DBA)
        {
            SageTableAdapter SageAdapter = new SageTableAdapter();
            return SageAdapter.GetDataByDBA(DBA);
        }//end function GetSageResidualsByDBA

        //This function returns Authnet residuals by DBA
        public PartnerDS.AuthnetDataTable GetAuthnetResidualsByDBA(string DBA)
        {        
           AuthnetTableAdapter AuthnetAdapter = new AuthnetTableAdapter();
           return AuthnetAdapter.GetDataByDBA(DBA);
    
        }//end function GetAuthnetResidualsByDBA

        //This function returns PlugNPay residuals by DBA
        public PartnerDS.PlugNPayDataTable GetPlugNPayResidualsByDBA(string DBA)
        {
            PlugNPayTableAdapter PlugNPayAdapter = new PlugNPayTableAdapter();
            return PlugNPayAdapter.GetDataByDBA(DBA);

        }//end function GetPlugNPayResidualsByDBA

        //This function returns InnovativeGateway residuals by DBA
        public PartnerDS.InnovativeGatewayDataTable GetInnovativeGatewayResidualsByDBA(string DBA)
        {
            InnovativeGatewayTableAdapter InnovativeGatewayAdapter = new InnovativeGatewayTableAdapter();
            return InnovativeGatewayAdapter.GetDataByDBA(DBA);

        }//end function GetInnovativeGatewayResidualsByDBA

        //This function returns iPaymentGateway residuals by DBA
        public PartnerDS.iPaymentGatewayDataTable GetiPaymentGatewayResidualsByDBA(string DBA)
        {
            iPaymentGatewayTableAdapter iPaymentGatewayAdapter = new iPaymentGatewayTableAdapter();
            return iPaymentGatewayAdapter.GetDataByDBA(DBA);

        }//end function GetiPaymentGatewayResidualsByDBA

        //This function returns CTCart residuals by DBA
        public PartnerDS.CTCartDataTable GetCTCartResidualsByDBA(string DBA)
        {
            CTCartTableAdapter CTCartAdapter = new CTCartTableAdapter();
            return CTCartAdapter.GetDataByDBA(DBA);

        }//end function GetCTCartResidualsByDBA

        //This function returns MiscReport residuals by DBA
        public PartnerDS.MiscReportDataTable GetMiscReportResidualsByDBA(string DBA)
        {
            MiscReportTableAdapter MiscReportAdapter = new MiscReportTableAdapter();
            return MiscReportAdapter.GetDataByDBA(DBA);

        }//end function GetMiscReportResidualsByDBA

        //This function returns CS residuals by DBA
        public PartnerDS.CheckServicesDataTable GetCSResidualsByDBA(string DBA)
        {
            CheckServicesTableAdapter CSAdapter = new CheckServicesTableAdapter();
            return CSAdapter.GetDataByDBA(DBA);

        }//end function GetCSResidualsByDBA

        //This function returns GC residuals by DBA
        public PartnerDS.GiftCardServicesDataTable GetGCResidualsByDBA(string DBA)
        {
            GiftCardServicesTableAdapter GCAdapter = new GiftCardServicesTableAdapter();
            return GCAdapter.GetDataByDBA(DBA);

        }//end function GetGCResidualsByDBA

        //This function returns MCA residuals by DBA
        public PartnerDS.MerchantCashAdvanceDataTable GetMCAResidualsByDBA(string DBA)
        {
            MerchantCashAdvanceTableAdapter MCAAdapter = new MerchantCashAdvanceTableAdapter();
            return MCAAdapter.GetDataByDBA(DBA);

        }//end function GetMCAResidualsByDBA

        //This function returns Payroll residuals by DBA
        public PartnerDS.PayrollDataTable GetPayrollResidualsByDBA(string DBA)
        {
            PayrollTableAdapter PayrollAdapter = new PayrollTableAdapter();
            return PayrollAdapter.GetDataByDBA(DBA);

        }//end function GetPayrollResidualsByDBA

        //This function returns Discover residuals By DBA
        public PartnerDS.DiscoverDataTable GetDiscoverResidualsByDBA(string DBA)
        {
            DiscoverTableAdapter Adapter = new DiscoverTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetDiscoverResidualsByDBA

        public string ReturnCurrMonth()
        {
            ResidualsDL Resd = new ResidualsDL();
            return Resd.ReturnCurrMonth();

        }//end function ReturnCurrMonth


        public DataSet UploadResiduals(string ResidualReport)
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.UploadResiduals(ResidualReport);
            return ds;
        }

        public DataSet UploadACTRates()
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.UploadACTRates();
            return ds;
        }

        public int UpdateACTRates()
        {
            ResidualsDL Resd = new ResidualsDL();
            int retVal = Resd.UpdateACTRates("All");
            return retVal;
        }
    }
}
