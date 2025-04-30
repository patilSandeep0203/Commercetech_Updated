using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class MonthBL
    {
        private string Mon = "";

        public MonthBL(string Mon)
        {
            this.Mon = Mon;
        }

        public MonthBL()
        {
        }

        //gets the months available for either Residual, Commissions or Referral Reports
        public DataSet GetMonthListForReports(int iAccess, string ReportType)
        {
            MonthDL Months = new MonthDL();
            DataSet ds = Months.GetMonthListForReports(iAccess, ReportType);
            return ds;
        }//end function GetMonthListForReports

        //This function returns Month and year digits for month selected from the list. CALLED BY CTCSalesOpps.aspx
        public DataSet GetMonthYear(int MonthID)
        {
 
            MonthDL month = new MonthDL();
            DataSet dsMonth = month.GetMonthYear(MonthID);
            return dsMonth;
        }//end function GetMonthYear

        //This function returns residual and commission upload dates. CALLED BY Home.aspx
        public DataSet GetResdCommDates(string Month, string Year)
        {
          MonthDL CommDates = new MonthDL();
            DataSet ds = CommDates.GetResdCommDates(Month, Year);
            return ds;
        }//end function GetResdCommDates

        //This function returns residual and commission upload dates. CALLED BY Home.aspx
        public DataSet GetHolidays(string Month, string Year)
        {
            MonthDL HolidayDates = new MonthDL();
            DataSet ds = HolidayDates.GetHolidays(Month, Year);
            return ds;
        }//end function GetHolidays

        //This function returns Funded goals. CALLED BY Home.aspx
        public DataSet GetFundedGoals(string Month, string Year)
        {
            MonthDL FundedCount = new MonthDL();
            DataSet ds = FundedCount.GetFundedGoals(Month, Year);
            return ds;
        }//end function GetFundedGoals

        //This function returns Funded goals. CALLED BY Home.aspx
        public DataSet GetFundedPartnerGoals(string MasterNum, string Month, string Year)
        {
            MonthDL FundedCount = new MonthDL();
            DataSet ds = FundedCount.GetFundedPartnerGoals(MasterNum, Month, Year);
            return ds;
        }//end function GetFundedGoals

        //This function returns prev month. CALLED BY ResidualsAdmin.aspx, ResidualsPrint.aspx
        public string ReturnPrevMonth(string Month)
        {
            string PrevMonth = "";
            MonthDL rMonth = new MonthDL();
            PrevMonth = rMonth.ReturnPrevMonth(Month);
            return PrevMonth;
        }//end function GetPrevMonth

        //This function returns Funded goals for Rep
        public string GetFundedGoalsForRep(string MasterNum, string Month, string Year)
        {
            string strFundedGoalCount = string.Empty;
            MonthDL FundedCount = new MonthDL();
            DataSet ds = FundedCount.GetFundedGoalsForRep(Month, Year, MasterNum);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strFundedGoalCount = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            return strFundedGoalCount;
        }//end function GetFundedGoals

        //This function returns rep list from the RepMonFundings table. CALLED BY Home.aspx
        public DataSet GetRepListFromRepMonFundings(string Month)
        {
            MonthDL FundedCount = new MonthDL();
            DataSet ds = FundedCount.GetRepListFundings(Month);
            return ds;
        }//end function GetRepListFromRepMonFundings

        //This function returns month list
        public DataSet GetMonthList()
        {
            MonthDL Months = new MonthDL();
            DataSet ds = Months.GetMonthList();
            return ds;
        }//end function GetMonthList

        //CALLED BY Home.aspx
        public int InsertUpdateRepFundings(string MasterNum, string TargetGoal, string Month, string Year)
        {
            MonthDL Goals = new MonthDL();
            int iRetVal = Goals.InsertUpdateRepFundings(MasterNum, TargetGoal, Month, Year);
            return iRetVal;
        }//end function InsertUpdateRepFundings

        //CALLED BY Home.aspx
        public int DeleteRepFundings(string MasterNum, string Month, string Year)
        {
            MonthDL Goals = new MonthDL();
            int iRetVal = Goals.DeleteRepFundings(MasterNum, Month, Year);
            return iRetVal;
        }//end function DeleteRepFundings

    }//end class MonthBL
}
