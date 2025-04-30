using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class BoundsBL
    {
        private ProcessorBoundsTableAdapter _ProcessorBoundsAdapter = null;
        protected ProcessorBoundsTableAdapter ProcessorBoundsAdapter
        {
            get
            {
                if (_ProcessorBoundsAdapter == null)
                    _ProcessorBoundsAdapter = new ProcessorBoundsTableAdapter();

                return _ProcessorBoundsAdapter;
            }
        }
        private GatewayBoundsTableAdapter _GatewayBoundsAdapter = null;
        protected GatewayBoundsTableAdapter GatewayBoundsAdapter
        {
            get
            {
                if (_GatewayBoundsAdapter == null)
                    _GatewayBoundsAdapter = new GatewayBoundsTableAdapter();

                return _GatewayBoundsAdapter;
            }
        }
        private CheckServiceBoundsTableAdapter _CheckServiceBoundsAdapter = null;
        protected CheckServiceBoundsTableAdapter CheckServiceBoundsAdapter
        {
            get
            {
                if (_CheckServiceBoundsAdapter == null)
                    _CheckServiceBoundsAdapter = new CheckServiceBoundsTableAdapter();

                return _CheckServiceBoundsAdapter;
            }
        }


        //This function returns processor list. CALLED BY SetBounds.aspx, SetDefaults.aspx
        public DataSet GetProcessorList()
        {
            BoundsDL ProcessorList = new BoundsDL();
            DataSet ds = ProcessorList.GetProcessorList();
            return ds;
        }//end GetProcessorList


        //Gets the Bounds for a processor. CALLED BY SetBounds.aspx, SetDefaults.aspx
        public PartnerDS.ProcessorBoundsDataTable GetProcessorBounds(string Processor, string CardPresent)
        {
            return ProcessorBoundsAdapter.GetData(Processor, CardPresent);
        }//end function GetProcessorRates

        //Gets the Bounds for a processor based on a Processor ID. CALLED BY SetBounds.aspx, SetDefaults.aspx
        public PartnerDS.ProcessorBoundsDataTable GetProcessorBounds(int ProcessorID)
        {
            return ProcessorBoundsAdapter.GetDataByProcID(Convert.ToInt16(ProcessorID));
        }//end function GetProcessorRates

        public string GetComplianceFee(int ProcessorID)
        {
            string strComplianceFee = string.Empty;
            BoundsDL ComplianceFee = new BoundsDL();
            DataSet ds = ComplianceFee.GetProcessorBoundComplianceFee(ProcessorID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (!Convert.IsDBNull(dr["NonComplianceFeeDef"]))
                {
                    strComplianceFee = dr["NonComplianceFeeDef"].ToString();
                }
                else
                {
                    strComplianceFee = "";
                }
            }

            return strComplianceFee;

        }


        //This function returns Gateway List. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public DataSet GetGatewayList()
        {
            BoundsDL GetList = new BoundsDL();
            DataSet ds = GetList.GetGatewayList();
            return ds;
        }//end function GetGatewayList

        public DataSet GetCheckServiceList()
        {
            BoundsDL GetList = new BoundsDL();
            DataSet ds = GetList.GetCheckServiceList();
            return ds;
        }//end function GetGatewayList

        public DataSet GetGiftCardList()
        {
            BoundsDL GetList = new BoundsDL();
            DataSet ds = GetList.GetGiftCardList();
            return ds;
        }//end function GetGatewayList

        //This function returns rates for selected processor. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public PartnerDS.GatewayBoundsDataTable GetGatewayBounds(int GatewayID)
        {
            return GatewayBoundsAdapter.GetDataByGatewayID(Convert.ToInt16(GatewayID));
        }//end function GetGatewayBounds

        //This function returns rates for selected processor. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public PartnerDS.CheckServiceBoundsDataTable CheckServiceBounds(int CheckServiceID)
        {
            return CheckServiceBoundsAdapter.GetDataByCheckServiceID(Convert.ToInt16(CheckServiceID));
        }//end function GetGatewayBounds

        public DataSet GiftCardBounds(string GiftCardID)
        {
            BoundsDL GiftBounds = new BoundsDL();
            DataSet ds = GiftBounds.GetGiftBounds(GiftCardID);
            return ds;
        }//end function GetGatewayBounds

        public DataSet GiftCardBoundsbyID(int GiftCardID)
        {
            BoundsDL GiftBounds = new BoundsDL();
            DataSet ds = GiftBounds.GetGiftBoundsbyID(GiftCardID);
            return ds;
        }//end function GetGatewayBounds

        //This function returns rates for selected processor. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public PartnerDS.GatewayBoundsDataTable GetGatewayBounds(string Gateway)
        {
            return GatewayBoundsAdapter.GetData(Gateway);
        }//end function GetGatewayBounds

        //This function returns rates for selected checkservice. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public DataSet GetCSBounds(string CheckService)
        {
            BoundsDL CS = new BoundsDL();
            DataSet ds = CS.GetCSBounds(CheckService);
            return ds;
        }//end function GetCSBounds

        //This function Updates Bounds. CALLED BY SetBounds.aspx
        public bool UpdateProcessorBounds(string LastModified, string CustServFeeLow,
             string InternetStmtLow, string TransFeeLow, string DRQPLow,
            string DRQNPLow, string DRMQLow, string DRNQLow, string DRQDLow, string ChargebackFeeLow,
            string RetrievalFeeLow, string VoiceAuthLow, string BatchHeaderLow, string AVSLow, string MonMinLow,
            string NBCTransFeeLow, string AnnualFeeLow, string WirelessAccessFeeLow, string WirelessTransFeeLow,
            string AppFeeLow, string AppSetupFeeLow, string ProcessorID, string DebitMonFee, string DebitTransFee, 
            string EBTMonFee, string EBTTransFee)
        {
            BoundsDL UpdatePBounds = new BoundsDL();
            bool retVal = UpdatePBounds.UpdateBounds(LastModified, CustServFeeLow, InternetStmtLow, TransFeeLow, DRQPLow,
            DRQNPLow, DRMQLow, DRNQLow, DRQDLow, ChargebackFeeLow,
            RetrievalFeeLow, VoiceAuthLow, BatchHeaderLow, AVSLow, MonMinLow,
            NBCTransFeeLow, AnnualFeeLow, WirelessAccessFeeLow, WirelessTransFeeLow,
            AppFeeLow, AppSetupFeeLow, ProcessorID, DebitMonFee, DebitTransFee, EBTMonFee, EBTTransFee);
            return retVal;
        }//end function UpdateProcessorBounds

        //This function Updates Processor Defaults. CALLED BY SetDefaults.aspx
        public bool UpdateProcessorDefaults(string LastModified, string CustServFeeLow, string InternetStmtLow, string TransFeeLow, string DRQPLow,
            string DRQNPLow, string DRMQLow, string DRNQLow, string DRQDLow, string ChargebackFeeLow,
            string RetrievalFeeLow, string VoiceAuthLow, string BatchHeaderLow, string AVSLow, string MonMinLow,
            string NBCTransFeeLow, string AnnualFeeLow, string WirelessAccessFeeLow, string WirelessTransFeeLow,
            string AppFeeLow, string AppSetupFeeLow, string ProcessorID, string DebitMonFee, string DebitTransFee,
            string EBTMonFee, string EBTTransFee)
        {
            BoundsDL UpdatePDefaults = new BoundsDL();
            bool retVal = UpdatePDefaults.UpdateDefaults(LastModified, CustServFeeLow, InternetStmtLow, TransFeeLow,
            DRQPLow, DRQNPLow, DRMQLow, DRNQLow, DRQDLow, ChargebackFeeLow,
            RetrievalFeeLow, VoiceAuthLow, BatchHeaderLow, AVSLow, MonMinLow,
            NBCTransFeeLow, AnnualFeeLow, WirelessAccessFeeLow, WirelessTransFeeLow,
            AppFeeLow, AppSetupFeeLow, ProcessorID, DebitMonFee, DebitTransFee, EBTMonFee, EBTTransFee);
            return retVal;
        }//end function UpdateProcessorDefaults

        public bool UpdateProcessorDefaults(string LastModified, string CustServFeeLow, string InternetStmtLow, string TransFeeLow, string DRQPLow,
            string DRQNPLow, string DRMQLow, string DRNQLow, string DRQDLow, string ChargebackFeeLow,
            string RetrievalFeeLow, string VoiceAuthLow, string BatchHeaderLow, string AVSLow, string MonMinLow,
            string NBCTransFeeLow, string AnnualFeeLow, string WirelessAccessFeeLow, string WirelessTransFeeLow,
            string AppFeeLow, string AppSetupFeeLow, string ProcessorID, string DebitMonFee, string DebitTransFee,
            string EBTMonFee, string EBTTransFee, string ComplianceFee)
        {
            BoundsDL UpdatePDefaults = new BoundsDL();
            bool retVal = UpdatePDefaults.UpdateDefaults(LastModified, CustServFeeLow, InternetStmtLow, TransFeeLow,
            DRQPLow, DRQNPLow, DRMQLow, DRNQLow, DRQDLow, ChargebackFeeLow,
            RetrievalFeeLow, VoiceAuthLow, BatchHeaderLow, AVSLow, MonMinLow,
            NBCTransFeeLow, AnnualFeeLow, WirelessAccessFeeLow, WirelessTransFeeLow,
            AppFeeLow, AppSetupFeeLow, ProcessorID, DebitMonFee, DebitTransFee, EBTMonFee, EBTTransFee, ComplianceFee);
            return retVal;
        }//end function UpdateProcessorDefaults

        //This function Updates Bounds. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public bool UpdateGatewayBounds(string GWSetupFee, string GWMonthlyFee, string GWTransFee, string GatewayID)
        {
            BoundsDL UpdateGWBounds = new BoundsDL();
            bool retVal = UpdateGWBounds.UpdateGatewayBounds(GWSetupFee, GWMonthlyFee, GWTransFee, GatewayID);
            return retVal;
        }//end function UpdateGatewayBounds

        //This function Updates Bounds. CALLED BY SetBounds.aspx, SetMinimums.aspx
        public bool UpdateGatewayDefaults(string GWSetupFee, string GWMonthlyFee, string GWTransFee, string GatewayID)
        {
            BoundsDL UpdateGWBounds = new BoundsDL();
            bool retVal = UpdateGWBounds.UpdateGatewayDefaults(GWSetupFee, GWMonthlyFee, GWTransFee, GatewayID);
            return retVal;
        }//end function UpdateGatewayBounds

        public bool UpdateCheckServiceBounds(string CheckServiceDiscRate, string CheckServiceMonFee, string CheckServiceMonMin, string CheckServiceTransFee, string CheckServiceID)
        {
            BoundsDL UpdateCSBounds = new BoundsDL();
            bool retVal = UpdateCSBounds.UpdateCheckServiceBounds(CheckServiceDiscRate, CheckServiceMonFee, CheckServiceMonMin, CheckServiceTransFee, CheckServiceID);
            return retVal;
        }//end function UpdateGatewayBounds

        public bool UpdateCheckServiceDefaults(string CheckServiceDiscRate, string CheckServiceMonFee, string CheckServiceMonMin, string CheckServiceTransFee, string CheckServiceID)
        {
            BoundsDL UpdateCSBounds = new BoundsDL();
            bool retVal = UpdateCSBounds.UpdateCheckServiceDefaults(CheckServiceDiscRate, CheckServiceMonFee, CheckServiceMonMin, CheckServiceTransFee, CheckServiceID);
            return retVal;
        }//end function UpdateGatewayBounds

        public bool UpdateGiftCardDefaults(string GiftCardMonFee, string GiftCardTransFee, string GiftCardID)
        {
            BoundsDL UpdateGiftCardBounds = new BoundsDL();
            bool retVal = UpdateGiftCardBounds.UpdateGiftCardDefaults(GiftCardMonFee, GiftCardTransFee, GiftCardID);
            return retVal;
        }//end function UpdateGatewayBounds

        public bool UpdateGiftCardBounds(string GiftCardMonFee, string GiftCardTransFee, string GiftCardID)
        {
            BoundsDL UpdateCSBounds = new BoundsDL();
            bool retVal = UpdateCSBounds.UpdateGiftCardBounds(GiftCardMonFee, GiftCardTransFee, GiftCardID);
            return retVal;
        }//end function UpdateGatewayBounds


    }//end class BoundsBL
}
