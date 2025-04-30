using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class MonthDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        //This function returns the residual and commission upload dates, Holidays and Funded counts based on 
        //sqlQuery passed. CALLED BY MonthBL.GetResdCommDates, MonthBL.GetHolidays, MonthBL.GetFundedGoals
        public DataSet GetResdCommDates(string Month, string Year)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetResdCommDates", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Year", Year));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function 

        public DataSet GetFundedGoals(string Month, string Year)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetRepMonFundings", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Year", Year));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetDataSQL

        public DataSet GetFundedPartnerGoals(string MasterNum, string Month, string Year)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetRepMonFundingsByRep", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Year", Year));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetDataSQL

        public DataSet GetHolidays(string Month, string Year)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetMonHolidays", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Year", Year));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function 

        public DataSet GetMonthListForReports(int iAccess, string ReportType)
        {

            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmds = new SqlCommand("sp_GetMonthListForReports", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection.Open();
                cmds.Parameters.Add(new SqlParameter("@iAccess", iAccess));
                cmds.Parameters.Add(new SqlParameter("@ReportType", ReportType));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Mon");
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMonthListForReports

     

        //CALLED BY MonthBL.GetRepListFromRepMonFundings
        public DataSet GetRepListFundings(string Mon)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetRepListFundings",Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Mon", Mon));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetDeleteGoalsList

        public DataSet GetFundedGoalsForRep(string Month, string Year, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            string sqlQuery = "Select FundedGoal from VW_RepMonFundings where Month=@Month AND Year=@Year AND MasterNum=@MasterNum";
         
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Year", Year));
                cmd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetFundedGoalsForRep

        //Called By RepInfoBL.AddNewMonth
        public bool CheckRepMonExists(string Mon)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select DISTINCT mon from RepInfoMonthly WHERE mon=@Mon";

                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Mon", Mon));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //count is 1 if the month is found in Rep Info Monthly
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                    return true;
                else
                    return false;
           
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetDataSQL
        
        public DataSet GetMonthList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmds = new SqlCommand("sp_GetDisplayedMonthList",Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Mon");
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMonthList


        //This function returns month and year. CALLED BY SalesOppsBL.GetMonthYear
        public DataSet GetMonthYear(int MonthID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetMonthYear", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@MonthID", MonthID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end GetMonthYear



        //Returns the Current Month for the Rep Information - CALLED BY RepInfoBL.GetPartnerInfoCurrMon, RepInfoBL.AddPartnerInfo
        public string ReturnCurrMon()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
      
            try
            {
                string strCurrMon = "";
                SqlCommand cmds = new SqlCommand("sp_GetCurrMon", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Mon");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    strCurrMon = dr["mon"].ToString();
                }
                return strCurrMon;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnCurrMon

        //CALLED BY MonthBL.InsertUpdateRepFundings
        public int InsertUpdateRepFundings(string MasterNum, string TargetGoal, string Mon, string Year)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_InsertUpdateRepFundings", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdRep.Parameters.Add(new SqlParameter("@FundedGoal", TargetGoal));
                cmdRep.Parameters.Add(new SqlParameter("@Month", Mon));
                cmdRep.Parameters.Add(new SqlParameter("@Year", Year));

                cmdRep.Connection.Open();
                int iRetVal = cmdRep.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return iRetVal;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end InsertUpdateRepFundings

        //CALLED BY MonthBL.DeleteRepFundings
        public int DeleteRepFundings(string MasterNum, string Mon, string Year)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_DeleteRepFundings", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdRep.Parameters.Add(new SqlParameter("@Month", Mon));
                cmdRep.Parameters.Add(new SqlParameter("@Year", Year));

                cmdRep.Connection.Open();
                int iRetVal = cmdRep.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return iRetVal;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end DeleteRepFundings

        public string ReturnPrevMonth(string CurrMon)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_ReturnPrevMonth", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                SqlParameter pCurrMonth = new SqlParameter("@CurrMon", SqlDbType.VarChar, 16);
                SqlParameter pPrevMonth = new SqlParameter("@PrevMon", SqlDbType.VarChar, 16);     
                cmdComm.Parameters.Add(pCurrMonth);
                cmdComm.Parameters.Add(pPrevMonth);
                pCurrMonth.Value = CurrMon;

                pPrevMonth.Direction = ParameterDirection.Output;

                cmdComm.Connection.Open();
                cmdComm.ExecuteNonQuery();

                string mon = Convert.ToString(pPrevMonth.Value);

                Conn.Close();
                Conn.Dispose();
                return mon;

            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPrevMonth

     
    }//end class HomeDL
}
