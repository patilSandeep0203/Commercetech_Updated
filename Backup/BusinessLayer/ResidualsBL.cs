using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;
namespace BusinessLayer
{
    public class ResidualsBL
    {
        private string Month = "";
        private string MasterNum = "";
        private string IMSNum = "";
        private string IPSNum = "";
        private string IMS2Num = "";
        private string SageNum = "";
        private string IPayNum = "";
        private string IPay2Num = "";
        private string IPay3Num = "";
        private string ChaseNum = "";

        //Constructor
        public ResidualsBL(string Month, string MasterNum)
        {
            this.Month = Month;
            this.MasterNum = MasterNum;

            if (MasterNum == "ALL")
            {
                this.IPayNum = "ALL";
                this.IPay2Num = "ALL";
                this.IPay3Num = "ALL";
                this.IMSNum = "ALL";
                this.IPSNum = "ALL";
                this.IMS2Num = "ALL";
                this.SageNum = "ALL";
                this.ChaseNum = "ALL";
            }
            else
            {
                RepInfoDL Rep = new RepInfoDL();
                DataSet ds = Rep.GetRepInfo(MasterNum);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    this.IPayNum = dr["IpayNum"].ToString().Trim();
                    this.IPay2Num = dr["IPay2Num"].ToString().Trim();
                    this.IPay3Num = dr["IPay3Num"].ToString().Trim();
                    this.IMSNum = dr["ImsNum"].ToString().Trim();
                    this.IPSNum = dr["IPSNum"].ToString().Trim();
                    this.IMS2Num = dr["IMS2Num"].ToString().Trim();
                    this.SageNum = dr["SageNum"].ToString().Trim();
                    this.ChaseNum = dr["ChaseNum"].ToString().Trim();
                }
            }


        }
              
        
        public string ReturnIMSNum()
        {
            return this.IMSNum;
        }

        public string ReturnIPSNum()
        {
            return this.IPSNum;
        }

        public string ReturnIMS2Num()
        {
            return this.IMS2Num;
        }

        public string ReturnSageNum()
        {
            return this.SageNum;
        }

        public string ReturnIPayNum()
        {
            return this.IPayNum;
        }

        public string ReturnIPay2Num()
        {
            return this.IPay2Num;
        }

        public string ReturnIPay3Num()
        {
            return this.IPay3Num;
        }

        public string ReturnChaseNum()
        {
            return this.ChaseNum;
        }
          

        //Gets Rep Vendor num for Admins and Employees
        public String ReturnVendorNumForRep(string Vendor, string MasterNum)
        {
            ResidualsDL Resd = new ResidualsDL();
            String RepNum = Resd.ReturnVendorNum(Vendor, MasterNum);
            return RepNum;
        }//end function GetVendorNumForRep

        //Gets MonthID
        public String ReturnMonthID(string Mon)
        {
            ResidualsDL Resd = new ResidualsDL();
            String MonthID = Resd.ReturnMonthID(Mon);
            return MonthID;
        }//end function ReturnMonthID

        //********************iPayment Functions********************

        public PartnerDS.iPaymentDataTable GetIPayResiduals()
        {
            iPaymentTableAdapter Adapter = new iPaymentTableAdapter();
            return Adapter.GetData(IPayNum, Month);
        }
        //This function returns iPayment Totals
        public DataSet GetiPayTotals()
        {
            ResidualsDL iPayResd = new ResidualsDL();
            DataSet ds = iPayResd.ReturniPayTotals(MasterNum, Month);
            return ds;
        }//end function GetiPayTotals

        //This function returns iPayment Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetiPayTotalsT1()
        {
            ResidualsDL iPayResd = new ResidualsDL();
            DataSet ds = iPayResd.GetiPayTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetiPayTotalsT1

        //********************iPayment2 Functions********************

        //This function returns iPayment2 residuals
        public PartnerDS.iPayment2DataTable GetiPay2Residuals()
        {
            iPayment2TableAdapter Adapter = new iPayment2TableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetiPay2Residuals

        //This function returns iPayment2 Totals
        public DataSet GetiPay2Totals()
        {
            ResidualsDL iPay2Resd = new ResidualsDL();
            DataSet ds = iPay2Resd.ReturniPay2Totals(MasterNum, Month);
            return ds;
        }//end function GetiPay2Totals

        //This function returns iPayment2 Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetiPay2TotalsT1()
        {
            ResidualsDL iPay2Resd = new ResidualsDL();
            DataSet ds = iPay2Resd.GetiPay2TotalsT1(MasterNum, Month);
            return ds;
        }//end function GetiPay2TotalsT1

        //********************iPayment3 Functions********************

        //This function returns iPayment3 residuals
        public PartnerDS.iPayment3DataTable GetiPay3Residuals()
        {
            iPayment3TableAdapter Adapter = new iPayment3TableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetiPay3Residuals

        //This function returns iPayment3 Totals
        public DataSet GetiPay3Totals()
        {
            ResidualsDL iPay3Resd = new ResidualsDL();
            DataSet ds = iPay3Resd.ReturniPay3Totals(MasterNum, Month);
            return ds;
        }//end function GetiPay3Totals

        //This function returns iPayment3 Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetiPay3TotalsT1()
        {
            ResidualsDL iPay3Resd = new ResidualsDL();
            DataSet ds = iPay3Resd.GetiPay3TotalsT1(MasterNum, Month);
            return ds;
        }//end function GetiPay3TotalsT1

        //********************IMS Functions********************

        //This function returns IMS residuals
        public PartnerDS.IMSDataTable GetIMSResiduals()
        {
            IMSTableAdapter Adapter = new IMSTableAdapter();
            return Adapter.GetData(MasterNum,Month);
        }//end function GetIMSResiduals



        //This function returns IMS Totals
        public DataSet GetIMSTotals()
        {
            ResidualsDL IMSResd = new ResidualsDL();
            DataSet ds = IMSResd.ReturnIMSTotals(MasterNum, Month);
            return ds;
        }//end function GetIMSTotals

        //This function returns IMS Totals for Tier 1. CALLED BY TierResiduals.aspx
        public DataSet GetIMSTotalsT1()
        {
            ResidualsDL IMSResd = new ResidualsDL();
            DataSet ds = IMSResd.GetIMSTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetIMSTotals

        //This function returns sub totals for IMS
        public DataSet GetIMSSubTotals( string MerchantID)
        {
            ResidualsDL SubTotals = new ResidualsDL();
            DataSet ds = SubTotals.GetIMSSubTotals( Month, MerchantID);
            return ds;
        }

        //********************IPS Functions********************

        //This function returns IPS residuals
        public PartnerDS.IPSDataTable GetIPSResiduals()
        {
            IPSTableAdapter Adapter = new IPSTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetIPSResiduals

        //This function gets IPS totals
        public DataSet GetIPSTotals()
        {
            ResidualsDL IPSResd = new ResidualsDL();
            DataSet ds = IPSResd.ReturnIPSTotals(MasterNum, Month);
            return ds;
        }//end function GetIPSTotals

        public DataSet GetIPSTotalsT1()
        {
            ResidualsDL IPSResd = new ResidualsDL();
            DataSet ds = IPSResd.GetIPSTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetIMSTotals

        //This function returns sub totals for IMS
        public DataSet GetIPSSubTotals(string MerchantID)
        {
            ResidualsDL SubTotals = new ResidualsDL();
            DataSet ds = SubTotals.GetIPSSubTotals(Month, MerchantID);
            return ds;
        }

        //********************IMS2 Functions********************

        //This function returns IMS2 residuals
        public PartnerDS.IMS2DataTable GetIMS2Residuals()
        {
            IMS2TableAdapter IMS2Adapter = new IMS2TableAdapter();
            return IMS2Adapter.GetData(MasterNum, Month);
        }//end function GetIMS2Residuals

        //This function returns IMS2 Totals
        public DataSet GetIMS2Totals()
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.ReturnIMS2Totals(MasterNum, Month);
            return ds;
        }//end function GetIMS2Totals

        //This function returns IMS2 Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetIMS2TotalsT1()
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.GetIMS2TotalsT1(MasterNum, Month);
            return ds;
        }//end function GetIMS2TotalsT1

        //********************Sage Functions********************

        //This function returns Sage residuals
        public PartnerDS.SageDataTable GetSageResiduals()
        {
            SageTableAdapter SageAdapter = new SageTableAdapter();
            return SageAdapter.GetData(MasterNum, Month);
        }//end function GetSageResiduals

        //This function returns Sage Totals
        public DataSet GetSageTotals()
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.ReturnSageTotals(MasterNum, Month);
            return ds;
        }//end function GetSageTotals

        //This function returns Sage Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetSageTotalsT1()
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.GetSageTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetSageTotalsT1
        //********************AUTHORIZE.NET Functions********************

        //This function returns Authnet residuals
        public PartnerDS.AuthnetDataTable GetAuthnetResiduals()
        {
            AuthnetTableAdapter AuthnetAdapter = new AuthnetTableAdapter();
            return AuthnetAdapter.GetData(MasterNum, Month);
        }//end function GetAuthnetResiduals


        //This function returns Authnet Totals
        public DataSet GetAuthnetTotals()
        {
            ResidualsDL AuthnetResd = new ResidualsDL();
            DataSet ds = AuthnetResd.ReturnAuthnetTotals(MasterNum, Month);
            return ds;
        }//end function GetAuthnetTotals

        //This function returns Authnet Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetAuthnetTotalsT1()
        {
            ResidualsDL AuthnetResd = new ResidualsDL();
            DataSet ds = AuthnetResd.GetAuthnetTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetAuthnetTotalsT1

        //********************DISCOVER Functions********************

        //This function returns Discover residuals
        public PartnerDS.DiscoverDataTable GetDiscoverResiduals()
        {
            DiscoverTableAdapter Adapter = new DiscoverTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetDiscoverResiduals



        //This function returns Discover Totals
        public DataSet GetDiscoverTotals()
        {
            ResidualsDL DiscoverResd = new ResidualsDL();
            DataSet ds = DiscoverResd.ReturnDiscoverTotals(MasterNum, Month);
            return ds;
        }//end function GetDiscoverTotals

        //This function returns Discover Totals for Tier 1. CALLED BY TierResiduals.aspx
        public DataSet GetDiscoverTotalsT1()
        {
            ResidualsDL DiscoverResd = new ResidualsDL();
            DataSet ds = DiscoverResd.GetDiscoverTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetDiscoverTotalsT1

        //********************WPay Functions********************

        //This function returns WPay residuals
        public PartnerDS.WPayDataTable GetWPayResiduals()
        {
            WPayTableAdapter Adapter = new WPayTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetWPayResiduals

        //This function returns WPay Totals
        public DataSet GetWPayTotals()
        {
            ResidualsDL WPayResd = new ResidualsDL();
            DataSet ds = WPayResd.ReturnWPayTotals(MasterNum, Month);
            return ds;
        }//end function GetWPayTotals

        //This function returns WPay Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetWPayTotalsT1()
        {
            ResidualsDL WPayResd = new ResidualsDL();
            DataSet ds = WPayResd.GetWPayTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetWPayTotalsT1

        //********************IPayGate Functions********************

        //This function returns IPayGate residuals
        public PartnerDS.iPaymentGatewayDataTable GetIPayGateResiduals()
        {
            iPaymentGatewayTableAdapter Adapter = new iPaymentGatewayTableAdapter();
            return Adapter.GetData(MasterNum,Month);
        }//end function GetIPayGateResiduals

        //This function returns IPayGate Totals
        public DataSet GetIPayGateTotals()
        {
            ResidualsDL IPayGateResd = new ResidualsDL();
            DataSet ds = IPayGateResd.ReturnIPayGateTotals(MasterNum, Month);
            return ds;
        }//end function GetIPayGateTotals

        //This function returns IPayGate Resd. CALLED BY iPayGate.aspx
        public DataSet GetIPayGateTotalsT1()
        {
            ResidualsDL IPayGateResd = new ResidualsDL();
            DataSet ds = IPayGateResd.GetIPayGateTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetIPayGateTotalsT1

        //********************InnGate Functions********************

        //This function returns InnGate residuals
        public PartnerDS.InnovativeGatewayDataTable GetInnGateResiduals()
        {
            InnovativeGatewayTableAdapter Adapter = new InnovativeGatewayTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetInnGateResiduals

        //This function returns InnGate Totals
        public DataSet GetInnGateTotals()
        {
            ResidualsDL InnGateResd = new ResidualsDL();
            DataSet ds = InnGateResd.GetInnGateTotals(MasterNum, Month);
            return ds;
        }//end function GetInnGateTotals

        //This function returns InnGate Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetInnGateTotalsT1()
        {
            ResidualsDL InnGateResd = new ResidualsDL();
            DataSet ds = InnGateResd.GetInnGateTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetInnGateTotalsT1

        //This function returns sub totals for InnGate
        public DataSet GetInnGateSubTotals( int ID)
        {
            ResidualsDL SubTotals = new ResidualsDL();
            DataSet ds = SubTotals.GetInnGateSubTotals(Month, ID);
            return ds;
        }

        //********************IPayFBBH Functions********************

        //This function returns IPayFBBH residuals
        public PartnerDS.iPaymentFBBHDataTable GetIPayFBBHResiduals()
        {
            iPaymentFBBHTableAdapter Adapter = new iPaymentFBBHTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetIPayFBBHResiduals

        //This function returns IPayFBBH Totals
        public DataSet GetIPayFBBHTotals()
        {
            ResidualsDL IPayFBBHResd = new ResidualsDL();
            DataSet ds = IPayFBBHResd.GetIPayFBBHTotals(MasterNum, Month);
            return ds;
        }//end function GetIPayFBBHTotals

        //This function returns IPayFBBH Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetIPayFBBHTotalsT1()
        {
            ResidualsDL IPayFBBHResd = new ResidualsDL();
            DataSet ds = IPayFBBHResd.GetIPayFBBHTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetIPayFBBHTotalsT1

        //********************Innovative Functions********************

        //This function returns Innovative residuals
        public PartnerDS.InnovativeDataTable GetInnovativeResiduals()
        {
           InnovativeTableAdapter Adapter = new InnovativeTableAdapter();
           return Adapter.GetData(MasterNum, Month);
        }//end function GetInnovativeResiduals

        //This function returns Innovative Totals
        public DataSet GetInnovativeTotals()
        {
            ResidualsDL InnovativeResd = new ResidualsDL();
            DataSet ds = InnovativeResd.GetInnovativeTotals(MasterNum, Month);
            return ds;
        }//end function GetInnovativeTotals

        //This function returns Innovative Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetInnovativeTotalsT1()
        {
            ResidualsDL InnovativeResd = new ResidualsDL();
            DataSet ds = InnovativeResd.GetInnovativeTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetInnovativeTotalsT1

        //********************CPS Functions********************

        //This function gets CPS residuals
        public PartnerDS.CPSDataTable GetCPSResiduals()
        {
            CPSTableAdapter Adapter = new CPSTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetCPSResiduals

        //This function returns CPS residuals By DBA
        public PartnerDS.CPSDataTable GetCPSResidualsByDBA(string DBA)
        {
            CPSTableAdapter Adapter = new CPSTableAdapter();
            return Adapter.GetDataByDBA(DBA);
        }//end function GetCPSResidualsByDBA

        //This function returns CPS Totals
        public DataSet GetCPSTotals()
        {
            ResidualsDL CPSResd = new ResidualsDL();
            DataSet ds = CPSResd.ReturnCPSTotals(MasterNum, Month);
            return ds;
        }//end function GetCPSTotals

        //This function returns CPS Totals for Tier 1. CALLED BY T1Residuals.aspx
        public DataSet GetCPSTotalsT1()
        {
            ResidualsDL CPSResd = new ResidualsDL();
            DataSet ds = CPSResd.GetCPSTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetCPSTotalsT1

        //********************Chase Functions********************

        //This function returns Chase residuals
        public PartnerDS.ChaseDataTable GetChaseResiduals()
        {
            ChaseTableAdapter Adapter = new ChaseTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetChaseResiduals

        //This function returns ChaseNew residuals
        public PartnerDS.ChaseNewDataTable GetChaseNewResiduals()
        {
            ChaseNewTableAdapter Adapter = new ChaseNewTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetChaseResiduals

        //This function returns Chase Totals
        public DataSet GetChaseTotals()
        {
            ResidualsDL ChaseResd = new ResidualsDL();
            DataSet ds = ChaseResd.ReturnChaseTotals(MasterNum, Month);
            return ds;
        }//end function GetChaseTotals

        //This function returns Chase Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetChaseTotalsT1()
        {
            ResidualsDL ChaseResd = new ResidualsDL();
            DataSet ds = ChaseResd.GetChaseTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetChaseTotalsT1

        //********************Merrick Functions********************

        //This function returns Merrick residuals
        public PartnerDS.MerrickDataTable GetMerrickResiduals()
        {
            MerrickTableAdapter Adapter = new MerrickTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetMerrickResiduals

        //This function returns Merrick Totals
        public DataSet GetMerrickTotals()
        {
            ResidualsDL MerrickResd = new ResidualsDL();
            DataSet ds = MerrickResd.ReturnMerrickTotals(MasterNum, Month);
            return ds;
        }//end function GetMerrickTotals

        //This function returns Merrick Totals
        public DataSet GetMerrickTotalsT1()
        {
            ResidualsDL MerrickResd = new ResidualsDL();
            DataSet ds = MerrickResd.GetMerrickTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetMerrickTotalsT1

        //********************OptimalCA Functions********************

        //This function returns OptimalCA residuals
        public PartnerDS.OptimalCADataTable GetOptimalCAResiduals()
        {
            OptimalCATableAdapter Adapter = new OptimalCATableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetOptimalCAResiduals

        //This function returns OptimalCA Totals
        public DataSet GetOptimalCATotals()
        {
            ResidualsDL OptimalCAResd = new ResidualsDL();
            DataSet ds = OptimalCAResd.GetOptimalCATotals(MasterNum, Month);
            return ds;
        }//end function GetOptimalCATotals

        //This function returns OptimalCA Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetOptimalCATotalsT1()
        {
            ResidualsDL OptimalCAResd = new ResidualsDL();
            DataSet ds = OptimalCAResd.GetOptimalCATotalsT1(MasterNum, Month);
            return ds;
        }//end function GetOptimalCATotalsT1

        //********************CTCart Functions********************

        //This function returns CTCart residuals
        public PartnerDS.CTCartDataTable GetCTCartResiduals()
        {
            CTCartTableAdapter Adapter = new CTCartTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetCTCartResiduals

        //This function returns CTCart Totals
        public DataSet GetCTCartTotals()
        {
            ResidualsDL CTCartResd = new ResidualsDL();
            DataSet ds = CTCartResd.GetCTCartTotals(MasterNum, Month);
            return ds;
        }//end function GetCTCartTotals

        //This function returns CTCart Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetCTCartTotalsT1()
        {
            ResidualsDL CTCartResd = new ResidualsDL();
            DataSet ds = CTCartResd.GetCTCartTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetCTCartTotalsT1

        //********************PlugNPay Functions********************

        //This function returns PlugNPay residuals
        public PartnerDS.PlugNPayDataTable GetPlugNPayResiduals()
        {            
            PlugNPayTableAdapter Adapter = new PlugNPayTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function GetPlugNPayResiduals

        //This function returns PlugNPay Totals
        public DataSet GetPlugNPayTotals()
        {
            ResidualsDL PlugNPayResd = new ResidualsDL();
            DataSet ds = PlugNPayResd.ReturnPlugNPayTotals(MasterNum, Month);
            return ds;
        }//end function GetPlugNPayTotals

        //This function returns PlugNPay Totals fot Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetPlugNPayTotalsT1()
        {
            ResidualsDL PlugNPayResd = new ResidualsDL();
            DataSet ds = PlugNPayResd.GetPlugNPayTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetPlugNPayTotalsT1

        //********************ECXLegacy Functions********************

        //This function returns ECXLegacy residuals
        public PartnerDS.ECXLegacyDataTable GetECXLegacyResiduals()
        {
            ECXLegacyTableAdapter Adapter = new ECXLegacyTableAdapter();           
            return Adapter.GetData(MasterNum,Month);
        }//end function GetECXLegacyResiduals

        //This function returns ECXLegacy Totals
        public DataSet GetECXLegacyTotals()
        {
            ResidualsDL ECXLegacyResd = new ResidualsDL();
            DataSet ds = ECXLegacyResd.ReturnECXLegacyTotals(MasterNum, Month);
            return ds;
        }//end function GetECXLegacyTotals

        //This function returns ECXLegacy Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetECXLegacyTotalsT1()
        {
            ResidualsDL ECXLegacyResd = new ResidualsDL();
            DataSet ds = ECXLegacyResd.GetECXLegacyTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetECXLegacyTotalsT1

        //********************Misc Functions********************

        //This function returns Misc Report residuals
        public PartnerDS.MiscReportDataTable GetMiscResiduals()
        {
            MiscReportTableAdapter Adapter = new MiscReportTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function Get

        //This function returns Misc Report Totals
        public DataSet GetMiscTotals()
        {
            ResidualsDL MiscResd = new ResidualsDL();
            DataSet ds = MiscResd.GetMiscTotals(MasterNum, Month);
            return ds;
        }//end function GetMiscTotals

        //This function returns Misc Report Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetMiscTotalsT1()
        {
            ResidualsDL MiscResd = new ResidualsDL();
            DataSet ds = MiscResd.GetMiscTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetMiscTotalsT1

        //This function returns CS Report residuals
        public PartnerDS.CheckServicesDataTable GetCSResiduals()
        {
            CheckServicesTableAdapter Adapter = new CheckServicesTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function Get

        //This function returns CS Report Totals
        public DataSet GetCSTotals()
        {
            ResidualsDL CSResd = new ResidualsDL();
            DataSet ds = CSResd.GetCSTotals(MasterNum, Month);
            return ds;
        }//end function GetCSTotals

        //This function returns CS Report Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetCSTotalsT1()
        {
            ResidualsDL CSResd = new ResidualsDL();
            DataSet ds = CSResd.GetCSTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetCSTotalsT1

        //This function returns GC Report residuals
        public PartnerDS.GiftCardServicesDataTable GetGCResiduals()
        {
            GiftCardServicesTableAdapter Adapter = new GiftCardServicesTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function Get

        //This function returns GC Report Totals
        public DataSet GetGCTotals()
        {
            ResidualsDL GCResd = new ResidualsDL();
            DataSet ds = GCResd.GetGCTotals(MasterNum, Month);
            return ds;
        }//end function GetGCTotals

        //This function returns GC Report Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetGCTotalsT1()
        {
            ResidualsDL GCResd = new ResidualsDL();
            DataSet ds = GCResd.GetGCTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetGCTotalsT1

        //This function returns GC Report residuals
        public PartnerDS.MerchantCashAdvanceDataTable GetMCAResiduals()
        {
            MerchantCashAdvanceTableAdapter Adapter = new MerchantCashAdvanceTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function Get

        //This function returns MCA Report Totals
        public DataSet GetMCATotals()
        {
            ResidualsDL MCAResd = new ResidualsDL();
            DataSet ds = MCAResd.GetMCATotals(MasterNum, Month);
            return ds;
        }//end function GetMCATotals

        //This function returns MCA Report Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetMCATotalsT1()
        {
            ResidualsDL MCAResd = new ResidualsDL();
            DataSet ds = MCAResd.GetMCATotalsT1(MasterNum, Month);
            return ds;
        }//end function GetMCATotalsT1

        //This function returns GC Report residuals
        public PartnerDS.PayrollDataTable GetPayrollResiduals()
        {
            PayrollTableAdapter Adapter = new PayrollTableAdapter();
            return Adapter.GetData(MasterNum, Month);
        }//end function Get

        //This function returns Payroll Report Totals
        public DataSet GetPayrollTotals()
        {
            ResidualsDL PayrollResd = new ResidualsDL();
            DataSet ds = PayrollResd.GetPayrollTotals(MasterNum, Month);
            return ds;
        }//end function GetPayrollTotals

        //This function returns Payroll Report Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetPayrollTotalsT1()
        {
            ResidualsDL PayrollResd = new ResidualsDL();
            DataSet ds = PayrollResd.GetPayrollTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetPayrollTotalsT1

        //********************Agent Residuals Functions*******************

        //Gets Rep Vendor num for Admins and Employees
        public bool CheckVendorExistsForRep(string Vendor)
        {            
            ResidualsDL Resd = new ResidualsDL();
            string RepNum = MasterNum;
            if (Vendor == "chase")
                RepNum = ChaseNum;
            else if (Vendor == "ipay")
                RepNum = IPayNum;
            else if (Vendor == "ipay2")
                RepNum = IPay2Num;
            else if (Vendor == "ims")
                RepNum = IMSNum;
            else if (Vendor == "ims2")
                RepNum = IMS2Num;

            bool Exists = Resd.CheckVendorExistsForRep(Vendor, RepNum);
            return Exists;
        }//end function CheckVendorExistsForRep

        //This function returns the Merchant Funded Count for Reps
        public double ReturnMerchFundedCount()
        {
            double Count = 0;
            ResidualsDL Rep = new ResidualsDL();
            DataSet ds = Rep.GetMerchFundedCount(MasterNum, Month);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Count = Convert.ToDouble(dr["MerchFundedCount"]);
            }//end if count not 0
            return Count;
        }//end function GetMerchFundedCount

        //This function returns ReferralCount for Reps
        public double ReturnReferralCount()
        {
            double Count = 0;
            ResidualsDL Rep = new ResidualsDL();
            DataSet ds = Rep.GetRefCount(MasterNum, Month);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Count = Convert.ToDouble(dr["ReferralCount"]);
            }//end if count not 0
            return Count;
        }//end function GetReferralCount


        public DataSet GetResidualPayByRepMon()
        {
           ResidualsDL Rep = new ResidualsDL();
           DataSet ds = Rep.GetResidualPayByRepMon(MasterNum, Month);
           return ds;
        }//end function GetResidualPayByRepMon

        //This function gets Confirmation Info based on affiliate id and month - CALLED by Residuals.aspx
        public DataSet GetConfirmationResd()
        {
            ResidualsDL Rep = new ResidualsDL();
            DataSet ds = Rep.GetConfirmationResd(MasterNum, Month);
            return ds;
        }//end function GetResdConfirmationResd

        public DataSet GetResdPayHistory(int PartnerID)
        {
            ResidualsDL Resd = new ResidualsDL();
            DataSet ds = Resd.GetResdPayHistory(PartnerID);
            return ds;
        }

        //This function returns Payroll Report Totals for Tier 1 Agents. CALLED BY TierResiduals.aspx
        public DataSet GetT1Residuals()
        {
            ResidualsDL T1Resd = new ResidualsDL();
            DataSet ds = T1Resd.GetT1Residuals(MasterNum, Month);
            return ds;
        }

        public DataSet GetOfficeResiduals()
        {
            ResidualsDL T1Resd = new ResidualsDL();
            DataSet ds = T1Resd.GetOfficeResiduals(MasterNum, Month);
            return ds;
        }
        
        //end function GetT1Residuals

     }//end class ResidualsBL
}
