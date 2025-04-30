using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class CommissionsBL
    {
        private CommissionsTableAdapter _CommissionsAdapter = null;
        protected CommissionsTableAdapter Adapter
        {
            get
            {
                if (_CommissionsAdapter == null)
                    _CommissionsAdapter = new CommissionsTableAdapter();

                return _CommissionsAdapter;
            }
        }

        //This function returns rep list. CALLED BY CommAdmin.aspx, CommUpdate.aspx
        public DataSet GetRepList()
        {
            CommissionsDL RepList = new CommissionsDL();
            DataSet ds = RepList.GetRepList();
            return ds;
        }//end function GetRepList

        //Called By CommAdmin.aspx
        public string ReturnCurrMonth()
        {
            CommissionsDL Months = new CommissionsDL();
            string Month = Months.ReturnCurrMonth();
            return Month;
        }//end function GetCurrMonth

        //This function gets the Funding Count(s) for selected Rep or all Reps. CALLED BY Commissions.aspx, CommAdmin.aspx
        public DataSet GetFundedCount(string RepNum, string Month)
        {
            CommissionsDL CommSummary = new CommissionsDL();
            return CommSummary.GetFundedCount(RepNum, Month);

        }//end function GetCommissionsSummary

        //This function gets the Funding Count(s) for selected Rep or all Reps. CALLED BY Commissions.aspx, CommAdmin.aspx
        public DataSet GetFundedCountPeriod(string RepNum, string Month, string Period)
        {
            CommissionsDL CommSummary = new CommissionsDL();
            return CommSummary.GetFundedCountPeriod(RepNum, Month, Period);

        }//end function GetFundedCountPeriod

        public double ReturnFundedCount(string RepNum, string Month)
        {
            //CommissionsDL Comm = new CommissionsDL();
            //return Comm.ReturnFundedCount(RepNum, Month);
            double Count = 0;
            ResidualsDL Rep = new ResidualsDL();
            DataSet ds = Rep.GetMerchFundedCount(RepNum, Month);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Count = Convert.ToDouble(dr["MerchFundedCount"]);
            }//end if count not 0
            return Count;

        }//end function ReturnFundedCount

        //This function gets commissions summary. CALLED BY Commissions.aspx, CommAdmin.aspx, CommPrint.aspx
        public DataSet GetBonusInfo(string RepNum, string Month, int Period)
        {
            CommissionsDL CommBonus = new CommissionsDL();
            DataSet ds = CommBonus.GetBonus(RepNum, Month, Period);
            return ds;
        }//end function GetBonusInfo

        //This function gets commissions details for selected rep and month. 
        //CALLED BY Commissions.aspx, CommAdmin.aspx, CommPrint.aspx
        public PartnerDS.CommissionsDataTable GetCommissionsByRepMon(string RepNum, string Month)
        {
            return Adapter.GetDataByRepMon(RepNum, Month);
        }//end function GetCommissionsByRepMon

        public PartnerDS.CommissionsDataTable GetCommissionsByRepMonPeriod(string RepNum, string Month, int Period)
        {
            return Adapter.GetDataByRepMonPeriod(RepNum, Month, Period);
        }//end function GetCommissionsByRepMon

        /*
        //This function gets commissions totals for the selected month. CALLED BY CommAdmin.aspx, CommPrint.aspx
        public DataSet GetCommissionTotals(string MasterNum, string Month)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetCommissionTotals(MasterNum, Month);
            return ds;
        }//end function GetCommissionsTotals
        */

        //This function gets Commission detail for specific commission id
        //CALLED BY CommUpdate.aspx
        public PartnerDS.CommissionsDataTable GetCommDetailFromID(Int32 CommissionID)
        {
            return Adapter.GetDataByCommID(CommissionID);
        }

        //This function updates commissions. CALLED BY CommUpdate.aspx
        public bool UpdateCommissionInfo(string Product, string Referral, string Price, string COG, string Units,
            string Comm, string FundedValue, int CommID)
        {
            CommissionsDL Update = new CommissionsDL();
            bool retVal = Update.UpdateCommissions(Product, Referral, Price, COG, Units, Comm, FundedValue, CommID);
            return retVal;
        }

        public bool UpdateCommissionInfo(string Product, string Referral, string Price, string COG, string Units,
            string Comm, string FundedValue, string ReferralPaid, int CommID)
        {
            CommissionsDL Update = new CommissionsDL();
            bool retVal = Update.UpdateCommissions(Product, Referral, Price, COG, Units, Comm, FundedValue, ReferralPaid, CommID);
            return retVal;
        }

        //This function Inserts Bonus Information. CALLED BY CommUpdate.aspx
        public int InsertBonus(string MasterNum, string BonusDesc, string Reason, string RepTotal, string Mon, int Period)
        {
            CommissionsDL Bonus = new CommissionsDL();
            int iRetVal = Bonus.InsertBonus(MasterNum, BonusDesc, Reason, RepTotal, Mon, Period);
            return iRetVal;
        }//end function InsertBonus

        //This function resets commissions. CALLED BY CommAdmin.aspx
        public bool ResetComm(string RepNum, string Month)
        {
            CommissionsDL Reset = new CommissionsDL();
            bool retVal = Reset.ResetCommissions(RepNum, Month);
            return retVal;
        }

        //This function gets commissions by DBA for verification
        public PartnerDS.CommissionsDataTable GetCommissionsByDBA(string DBA, string MonthExclude)
        {
            return Adapter.GetDataByDBA(DBA, MonthExclude);
        }//end function GetCommissionsByDBA

        //This function gets commissions payment summary for the selected month
        public DataSet GetCommPayment(string MasterNum, string Month)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetCommPayment(MasterNum, Month);
            return ds;
        }//end function GetCommPayment

        //This function gets commissions summary ordered by Direct Deposit/BillPay
        //CALLED By CommSummary.aspx.cs
        public DataSet GetCommRefPaymentByDD(int Employee, int DirectDeposit, string Month)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetCommRefPaymentByDD(Employee, DirectDeposit, Month);
            return ds;
        }//end function GetCommRefPaymentByDD

        //This function gets commissions summary ordered by Direct Deposit/BillPay and Month Period
        //CALLED By CommSummary.aspx.cs
        public DataSet GetCommRefPaymentByMonPeriod(int Employee, int DirectDeposit, string Month, int Period)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetCommRefPaymentByMonPeriod(Employee, DirectDeposit, Month, Period);
            return ds;
        }//end function GetCommRefPaymentByDD

        //This function gets residuals and commissions payment summary ordered by Direct Deposit/BillPay - CALLED By Payment.aspx.cs
        public DataSet GetResdCommPaymentByMonPeriod(int Employee, int DirectDeposit, string Month, int Period)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetResdCommPaymentByMonPeriod(Employee, DirectDeposit, Month, Period);
            return ds;
        }//end function

        public DataSet GetResdCommPayHistory(int PartnerID)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetResdCommPayHistory(PartnerID);
            return ds;
        }

        public DataSet GetCommPayHistory(int PartnerID)
        {
            CommissionsDL Comm = new CommissionsDL();
            DataSet ds = Comm.GetCommPayHistory(PartnerID);
            return ds;
        }

        //This function inserts/updates confirmation code
        public int InsertUpdateConfirmationCode(string AffiliateID, string Month, string Code, string Note, decimal Carryover, decimal Payment, string DatePaid)
        {
            CommissionsDL CCode = new CommissionsDL();
            int iRetVal = CCode.InsertUpdateConfirmationCode(AffiliateID, Month, Code, Note, Carryover, Payment, DatePaid);
            return iRetVal;
        }//end function InsertUpdateConfirmationCode

        //This function inserts/updates confirmation code
        public int InsertUpdatePaymentInfo(string AffiliateID, string Month, string Code, string Note, decimal Carryover, decimal Payment, string DatePaid, string Period)
        {
            CommissionsDL Comm = new CommissionsDL();
            int iRetVal = Comm.InsertUpdatePaymentInfo(AffiliateID, Month, Code, Note, Carryover, Payment, DatePaid, Period);
            return iRetVal;
        }//end function InsertUpdateConfirmationCode

    }//end class CommissionsBL
}
