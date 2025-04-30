using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class ResidualsDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringACT = ConfigurationManager.AppSettings["ConnectionStringACT"].ToString();

        public DataSet CheckExists(string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "SELECT * From ProcessorMonthly where mon = @Month";
                SqlCommand cmdResd = new SqlCommand(strQuery, Conn);
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@Month", Month));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "ProcessorMonthly");
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
        }//end CheckExists

        public DataSet UploadResiduals(string ResidualReport)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_RunDTSPackages", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;

                SqlParameter pReport = cmdComm.Parameters.Add("@ResidualReport", SqlDbType.VarChar);
                pReport.Value = ResidualReport;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdComm;
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
        }//end function ReturnCurrResidualMonth

        public DataSet GetACTRatesHistory()
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_GetRateHistoryBackup", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdComm;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                Conn.Close();
                Conn.Dispose();

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
        }//end function ReturnCurrResidualMonth

        public int DeleteACTRatesHistory()
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_DeleteRateHistoryBackup", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Connection.Open();

                int retVal = cmdComm.ExecuteNonQuery();

                Conn.Close();
                Conn.Dispose();

                return retVal;




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
        }//end function ReturnCurrResidualMonth
        
        public DataSet UploadACTRates()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_RunDTSPortalToACT", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;

                //SqlParameter pReport = cmdComm.Parameters.Add("@ResidualReport", SqlDbType.VarChar);
                //pReport.Value = ResidualReport;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdComm;
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
        }//end function ReturnCurrResidualMonth

        public int UpdateACTRates(string processor)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_UpdateACTResidualsNew", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Connection.Open();
                SqlParameter pProcessor = cmdComm.Parameters.Add("@Processor", SqlDbType.VarChar);
                pProcessor.Value = processor;

                int retVal = cmdComm.ExecuteNonQuery();
                
                return retVal;

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
        }//end function ReturnCurrResidualMonth

        public string ReturnCurrMonth()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_ReturnCurrResidualMonth", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;

                SqlParameter pCurrMonth = new SqlParameter("@CurrMon", SqlDbType.VarChar, 16);
                cmdComm.Parameters.Add(pCurrMonth);
                pCurrMonth.Direction = ParameterDirection.Output;
                cmdComm.Connection.Open();
                cmdComm.ExecuteNonQuery();

                string mon = Convert.ToString(pCurrMonth.Value);

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
        }//end function ReturnCurrResidualMonth

        //CALLED BY ResidualsBL.ReturnCount
        public DataSet GetResidualsCountByMon(string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetResidualsCountByMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetResidualsCountByMon


        //*******************************GET TOTALS*******************************

        //Gets ece total for current and prev months for all accounts
        //CALLED BY ResidualsBL.GetECETotal
        public double ReturnECETotalForMonth(string Vendor, string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                string strProcedure = string.Empty;
                string Num = "@MasterNum";
                switch (Vendor.ToLower())
                {

                    case "revel":
                        strProcedure = "SP_GetRevelTotalByRepMon";
                        break;
                    case "cps":
                        strProcedure = "SP_GetCPSTotalByRepMon";
                        break;
                    case "innovative":
                        strProcedure = "SP_GetInnTotalByRepMon";
                        break;
                    case "ctcart":
                        strProcedure = "SP_GetCTCartTotalByRepMon";
                        break;
                    case "authnet":
                        strProcedure = "SP_GetAuthnetTotalByRepMon";
                        break;
                    case "ipay":
                        strProcedure = "SP_GetIPayTotalByRepMon";
                        Num = "@IPayNum";
                        break;
                    case "ipay2":
                        strProcedure = "SP_GetIPay2TotalByRepMon";
                        Num = "@IPay2Num";
                        break;
                    case "ipay3":
                        strProcedure = "SP_GetIPay3TotalByRepMon";
                        Num = "@IPay3Num";
                        break;
                    case "ims":
                        strProcedure = "SP_GetIMSTotalByRepMon";
                        Num = "@IMSNum";
                        break;
                    case "ims2":
                        strProcedure = "SP_GetIMS2TotalByRepMon";
                        Num = "@IMS2Num";
                        break;
                    case "ips":
                        strProcedure = "SP_GetIPSTotalByRepMon";
                        Num = "@IPSNum";
                        break;
                    case "sage":
                        strProcedure = "SP_GetSageTotalByRepMon";
                        Num = "@SageNum";
                        break;
                    case "wpay":
                        strProcedure = "SP_GetWPayTotalByRepMon";
                        break;
                    case "ipaygate":
                        strProcedure = "SP_GetIPayGateTotalByRepMon";
                        break;
                    case "inngate":
                        strProcedure = "SP_GetInnGateTotalByRepMon";
                        break;
                    case "ipayfbbh":
                        strProcedure = "SP_GetIPayFBBHTotalByRepMon";
                        break;
                    case "chase":
                        strProcedure = "SP_GetChaseTotalByRepMon";
                        Num = "@ChaseNum";
                        break;
                    case "merrick":
                        strProcedure = "SP_GetMerrickTotalByRepMon";
                        break;
                    case "optimalca":
                        strProcedure = "SP_GetOptimalCATotalByRepMon";
                        break;
                    case "disc":
                        strProcedure = "SP_GetDiscTotalByRepMon";
                        break;
                    case "ecx":
                        strProcedure = "SP_GetECXLegacyTotalByRepMon";
                        break;
                    case "plugnpay":
                        strProcedure = "SP_GetPlugNPayTotalByRepMon";
                        break;
                    case "cs":
                        strProcedure = "SP_GetCSTotalByRepMon";
                        break;
                    case "gc":
                        strProcedure = "SP_GetGCTotalByRepMon";
                        break;
                    case "mca":
                        strProcedure = "SP_GetMCATotalByRepMon";
                        break;
                    case "payroll":
                        strProcedure = "SP_GetPayrollTotalByRepMon";
                        break;
                    //case "misc":
                        //strProcedure = "SP_GetMiscTotalByRepMon";
                        //sbreak;
                    case "pivotal":
                        strProcedure = "SP_GetPivotalTotalByRepMon";
                        Num = "@PivotalNum";
                        break;

                }//end switch

                SqlCommand cmd = new SqlCommand(strProcedure, Conn);
                cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter pRepNum = cmd.Parameters.Add(Num, SqlDbType.VarChar);
                SqlParameter pMonth = cmd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pRepNum.Value = RepNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //ECETotal will be the first element in the dataset regardless of which Stored Proc is executed
                double ECETotal = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ECETotal = Convert.ToDouble(Convert.ToString(dr[1]));
                }
                return ECETotal;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetECETotalForMonth



        //Gets rep list for Admins on all reports. CALLED BY ResidualsBL.GetRepListForVendor
        public DataSet GetRepListForVendor(string Vendor)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {

                SqlCommand cmdResd = new SqlCommand("sp_GetVendorRepList", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@Vendor", Vendor));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
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
        }//end function GetRepListForVendor

        //Gets the rep list for tier 1 reps on all reports. CALLED BY ResidualsBL.GetRepListForVendorByTier
        public DataSet GetRepListForVendorByTier(string Vendor, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetVendorRepListForTier", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdResd.Parameters.Add(new SqlParameter("@Vendor", Vendor));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
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
        }//end function GetRepListForVendorByTier

        public String ReturnMonthID(string Mon)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_ReturnMonthID", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Parameters.Add(new SqlParameter("@mon", Mon));
                SqlParameter pMonthID = new SqlParameter("@MonthID", SqlDbType.VarChar, 5);
                cmdResd.Parameters.Add(pMonthID);
                pMonthID.Direction = ParameterDirection.Output;

                cmdResd.Connection.Open();
                cmdResd.ExecuteNonQuery();

                string MonthID = Convert.ToString(pMonthID.Value);

                return MonthID;
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
        }//end function ReturnMonthID

        public String ReturnVendorNum(string Vendor, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_ReturnVendorRepNum", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdResd.Parameters.Add(new SqlParameter("@Vendor", Vendor));
                SqlParameter pRepNum = new SqlParameter("@RepNum", SqlDbType.VarChar, 16);
                cmdResd.Parameters.Add(pRepNum);
                pRepNum.Direction = ParameterDirection.Output;

                cmdResd.Connection.Open();
                cmdResd.ExecuteNonQuery();

                string RepNum = Convert.ToString(pRepNum.Value);

                return RepNum;
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
        }//end function ReturnVendorNum

        public bool CheckVendorExistsForRep(string Vendor, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmdResd = new SqlCommand("sp_CheckVendorExistsForRep", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdResd.Parameters.Add(new SqlParameter("@Vendor", Vendor));
                cmdResd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                //Query found at least one account for the given Rep's MasterNum
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
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
        }//end function ReturnVendorNum


        //****************************IPAY FUNCTIONS****************************

        public DataSet GetiPayResiduals(string iPayNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter piPayNum = cmdResd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                piPayNum.Value = iPayNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturniPayResiduals

        public DataSet GetiPayResidualsByDBA(string DBA)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayByDBA", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pDBA = cmdResd.Parameters.Add("@DBA", SqlDbType.VarChar);
                pDBA.Value = DBA;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturniPayResidualsByDBA

        public DataSet ReturniPayTotals(string iPayNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter piPayNum = cmdResd.Parameters.Add("@IPayNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                piPayNum.Value = iPayNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturniPayTotals

        public DataSet GetiPayTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetiPayTotalsT1

        //****************************IPAY2 FUNCTIONS****************************

        public DataSet ReturniPay2Totals(string iPay2Num, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPay2TotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter piPay2Num = cmdResd.Parameters.Add("@IPay2Num", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                piPay2Num.Value = iPay2Num;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturniPay2Totals

        public DataSet GetiPay2TotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPay2TotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetiPay2TotalsT1

        //****************************IPAY3 FUNCTIONS****************************

        public DataSet ReturniPay3Totals(string iPay2Num, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPay3TotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter piPay3Num = cmdResd.Parameters.Add("@IPay3Num", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                piPay3Num.Value = iPay2Num;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturniPay3Totals

        public DataSet GetiPay3TotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPay3TotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetiPay3TotalsT1


        public DataSet ReturnIMSTotals(string IMSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIMSTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIMSNum = cmdResd.Parameters.Add("@IMSNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIMSNum.Value = IMSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnIMSTotals

        

        public DataSet GetIMSTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIMSTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetIMSTotalsT1

        public DataSet GetIMSSubTotals(string Month, string MerchantID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetIMSByRep", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdResd.Parameters.Add(new SqlParameter("@MerchantID", MerchantID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "IMSByRep");
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
        }//end ReturnIMSSubTotals

        //****************************IPS FUNCTIONS****************************

        public DataSet ReturnIPSTotals(string IPSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPSTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIPSNum = cmdResd.Parameters.Add("@IPSNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIPSNum.Value = IPSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnIPSTotals

        //****************************IPS FUNCTIONS****************************

        public DataSet ReturnPivotalTotals(string PivotalNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetPivotalTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIPSNum = cmdResd.Parameters.Add("@PivotalNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIPSNum.Value = PivotalNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnIPSTotals

        public DataSet GetIPSTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPSTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetIPSTotalsT1

        public DataSet GetIPSSubTotals(string Month, string MerchantID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetIPSByRep", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdResd.Parameters.Add(new SqlParameter("@MerchantID", MerchantID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "IPSByRep");
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
        }//end ReturnIPSSubTotals

        //****************************IMS2 FUNCTIONS****************************

       
        public DataSet ReturnIMS2Totals(string IMS2Num, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIMS2TotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIMS2Num = cmdResd.Parameters.Add("@IMS2Num", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIMS2Num.Value = IMS2Num;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnIMS2Totals

        public DataSet GetIMS2TotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIMS2TotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetIMS2TotalsT1

        //****************************Sage FUNCTIONS****************************
        public DataSet ReturnSageTotals(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetSageTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@SageNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnSageTotals

        public DataSet GetSageTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetSageTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetSageTotalsT1

        public DataSet ReturnRevelTotals(string IMSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetRevelTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIMSNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIMSNum.Value = IMSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnAuthnetTotals

        public DataSet ReturnAuthnetTotals(string IMSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetAuthnetTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIMSNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIMSNum.Value = IMSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnAuthnetTotals

        public DataSet GetAuthnetTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetAuthnetTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetAuthnetTotalsT1

        //****************************DISCOVER FUNCTIONS****************************
      
        public DataSet ReturnDiscoverTotals(string IMSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetDiscTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pIMSNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pIMSNum.Value = IMSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnDiscoverTotals

        public DataSet GetDiscoverTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetDiscTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetDiscoverTotalsT1

        //****************************WPay FUNCTIONS****************************
        public DataSet ReturnWPayTotals(string WPayNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetWPayTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = WPayNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnWPayTotals

        public DataSet GetWPayTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetWPayTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetWPayTotalsT1

        //****************************IPayGate FUNCTIONS****************************

        public DataSet ReturnIPayGateTotals(string iPayGateNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayGateTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = iPayGateNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnIPayGateTotals

        public DataSet GetIPayGateTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayGateTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetIPayGateTotalsT1

        //****************************InnGate FUNCTIONS****************************
        public DataSet GetInnGateTotals(string InnGate, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetInnGateTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = InnGate;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnInnGateTotals

        public DataSet GetInnGateTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetInnGateTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetInnGateTotalsT1

        public DataSet GetInnGateSubTotals(string Month, int ID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                
                SqlCommand cmdResd = new SqlCommand("sp_GetInnGateSubTotals", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdResd.Parameters.Add(new SqlParameter("@GatewayID", ID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "InnGateByRep");
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
        }//end ReturnInnGateSubTotals

        //****************************IPayFBBH FUNCTIONS****************************

        public DataSet GetIPayFBBHTotals(string IPayFBBHNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayFBBHTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = IPayFBBHNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnIPayFBBHTotals

        public DataSet GetIPayFBBHTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetIPayFBBHTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetIPayFBBHTotalsT1

        //****************************Innovative FUNCTIONS****************************
        public DataSet GetInnovativeTotals(string InnovativeNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetInnTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = InnovativeNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnInnovativeTotals

        public DataSet GetInnovativeTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetInnTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetInnovativeTotalsT1

        //****************************CPS FUNCTIONS****************************

        public DataSet ReturnCPSTotals(string CPSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetCPSTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = CPSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnCPSTotals

        public DataSet GetCPSTotalsT1(string CPSNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetCPSTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = CPSNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetCPSTotalsT1

        //****************************Chase FUNCTIONS****************************

        
        public DataSet ReturnChaseTotals(string ChaseNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetChaseTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@ChaseNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = ChaseNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnChaseTotals

        public DataSet GetChaseTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetChaseTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetChaseTotalsT1

        //****************************Merrick FUNCTIONS****************************

        public DataSet ReturnMerrickTotals(string MerrickNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetMerrickTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MerrickNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMerrickTotals

        public DataSet GetMerrickTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetMerrickTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetMerrickTotalsT1


        //****************************OptimalCA FUNCTIONS****************************

        
        public DataSet GetOptimalCATotals(string OptimalCANum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetOptimalCATotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = OptimalCANum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnOptimalCATotals

        public DataSet GetOptimalCATotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetOptimalCATotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetOptimalCATotalsT1

        //****************************CTCart FUNCTIONS****************************
        
        public DataSet GetCTCartTotals(string CTCartNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetCTCartTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = CTCartNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnCTCartTotals

        public DataSet GetCTCartTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetCTCartTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetCTCartTotalsT1

        //****************************PlugNPay FUNCTIONS****************************
        public DataSet ReturnPlugNPayTotals(string PlugNPayNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetPlugNPayTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = PlugNPayNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPlugNPayTotals

        public DataSet GetPlugNPayTotalsT1(string PlugNPayNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetPlugNPayTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = PlugNPayNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetPlugNPayTotalsT1


        //****************************ECXLegacy FUNCTIONS****************************

        
        public DataSet ReturnECXLegacyTotals(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetECXLegacyTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnECXLegacyTotals

        public DataSet GetECXLegacyTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetECXLegacyTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetECXLegacyTotalsT1

        //****************************MISC FUNCTIONS****************************
        public DataSet GetMiscTotals(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetMiscTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = RepNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMiscTotals

        public DataSet GetMiscTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetMiscTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetMiscTotalsT1

        //****************************CS FUNCTIONS****************************
        public DataSet GetCSTotals(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetCSTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = RepNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnCSTotals

        public DataSet GetCSTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetCSTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetCSTotalsT1

        //****************************GC FUNCTIONS****************************
        public DataSet GetGCTotals(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetGCTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = RepNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnGCTotals

        public DataSet GetGCTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetGCTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetGCTotalsT1

        //****************************MCA FUNCTIONS****************************
        public DataSet GetMCATotals(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetMCATotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = RepNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMCATotals

        public DataSet GetMCATotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetMCATotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetMCATotalsT1

        //****************************Payroll FUNCTIONS****************************
        public DataSet GetPayrollTotals(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetPayrollTotalByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = RepNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPayrollTotals

        public DataSet GetPayrollTotalsT1(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetPayrollTotalByRepMonT1", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetPayrollTotalsT1

        //****************************AGENT RESIDUAL PAGE FUNCTIONS****************************
        
        public DataSet GetResidualPayByRepMon(string MasterNum, string mon)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetResidualPayByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMon = cmdResd.Parameters.Add("@mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMon.Value = mon;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetResidualPayByRepMon

        public DataSet GetMerchFundedCount(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetMerchFundedCountByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdResd.Parameters.Add(new SqlParameter("@Mon", Month));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
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
        }//end 

        public DataSet GetRefCount(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetRefCountByRepMon", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                cmdResd.Connection.Open();
                cmdResd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdResd.Parameters.Add(new SqlParameter("@Mon", Month));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
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
        }//end 


        //****************************DROPPED RESIDUAL PAGE FUNCTIONS****************************

        public DataSet GetDroppedResd(string Report, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_DroppedReports", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pReport = cmdResd.Parameters.Add("@Report", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pReport.Value = Report;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnDroppedResd

        //****************************DROPPED RESIDUAL PAGE FUNCTIONS****************************

        public DataSet GetDroppedResdOffice(string Report, string Month, string OfficeMasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_DroppedReportsOffice", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pReport = cmdResd.Parameters.Add("@Report", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pOfficeMasterNum = cmdResd.Parameters.Add("@OfficeMasterNum", SqlDbType.VarChar);
                pReport.Value = Report;
                pMonth.Value = Month;
                pOfficeMasterNum.Value = OfficeMasterNum;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnDroppedResd

        //****************************ACTIVATED RESIDUAL PAGE FUNCTIONS****************************

        public DataSet GetACTResidualStatus(string Status, string Service)
        {
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetServiceStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Status", Status));
                cmd.Parameters.Add(new SqlParameter("@Service", Service));
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;

            }
            catch (SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet GetACTContactIDByMerchantID(string MerchantNum)
        {
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetContactIDByMerchantID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MerchantNum", MerchantNum));

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;

            }
            catch (SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet GetACTContactIDByGatewayID(string GatewayID)
        {
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetContactIDByGatewayID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@GatewayID", GatewayID));

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;

            }
            catch (SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet GetACTContactIDByDBA(string DBA)
        {
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetContactIDByDBA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DBA", DBA));

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;

            }
            catch (SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public int UpdateACTResdStatus(string Service, string Processor, string Month, string Status, string CONTACTID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try{
                SqlCommand cmd = new SqlCommand("SP_UpdateACTResdStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Service", Service));
                cmd.Parameters.Add(new SqlParameter("@Processor",Processor));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Status",Status));
                cmd.Parameters.Add(new SqlParameter("@CONTACTID",CONTACTID));

                cmd.Connection.Open();
                int retVal = cmd.ExecuteNonQuery();
                return retVal;
            }
            catch(SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public int UpdateActivatedStatus(string MerchantNum, string DBA, string Report)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateActivatedReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MerchantID", MerchantNum));
                cmd.Parameters.Add(new SqlParameter("@DBA", DBA));
                cmd.Parameters.Add(new SqlParameter("@Report", Report));


                cmd.Connection.Open();
                int retVal = cmd.ExecuteNonQuery();
                return retVal;
            }
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public int UpdateDroppedStatus(string MerchantNum, string DBA, string Report)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateDroppedReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MerchantID", MerchantNum));
                cmd.Parameters.Add(new SqlParameter("@DBA", DBA));
                cmd.Parameters.Add(new SqlParameter("@Report", Report));


                cmd.Connection.Open();
                int retVal = cmd.ExecuteNonQuery();
                return retVal;
            }
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public DataSet GetActivatedResd(string Report, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_ActivatedReports", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pReport = cmdResd.Parameters.Add("@Report", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pReport.Value = Report;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnActivatedResd

        public DataSet GetActivatedResdOffice(string Report, string Month, string OfficeMasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_ActivatedReportsOffice", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pReport = cmdResd.Parameters.Add("@Report", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pOfficeMasterNum = cmdResd.Parameters.Add("@OfficeMasterNum", SqlDbType.VarChar);
                pReport.Value = Report;
                pMonth.Value = Month;
                pOfficeMasterNum.Value = OfficeMasterNum;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnActivatedResd

        public DataSet GetZeroNegativeResiduals(string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetZeroNegativeResiduals", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnActivatedResd

        public DataSet GetResidualPaymentSummaryByDD(int Employee, int DirectDeposit, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_GetResidualPaymentSummary", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@Employee", Employee));
                cmdRep.Parameters.Add(new SqlParameter("@DirectDeposit", DirectDeposit));
                cmdRep.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdRep.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
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
        }//end GetResidualsPaymentByDD

        public DataSet GetResidualPaymentCalcSummaryByDD(int Employee, int DirectDeposit, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_GetResidualPaymentCalcSummary", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@Employee", Employee));
                cmdRep.Parameters.Add(new SqlParameter("@DirectDeposit", DirectDeposit));
                cmdRep.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdRep.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
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
        }//end GetResidualsPaymentByDD
              
        public int InsertUpdateResdConfirm(string AffiliateID, string Month, string ConfirmNum, string Note, decimal CarryOver, decimal Payment, string DatePaid)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCode = new SqlCommand("SP_InsertUpdateResidualConfirm", Conn);
                cmdCode.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmdCode.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pMonth = cmdCode.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pConfirmNum = cmdCode.Parameters.Add("@ConfirmNum", SqlDbType.VarChar);
                SqlParameter pNote = cmdCode.Parameters.Add("@Note", SqlDbType.VarChar);
                SqlParameter pCarryOver = cmdCode.Parameters.Add("@CarryOver", SqlDbType.Decimal);
                SqlParameter pPayment = cmdCode.Parameters.Add("@Payment", SqlDbType.Decimal);
                SqlParameter pDatePaid = cmdCode.Parameters.Add("@DatePaid", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;
                pMonth.Value = Month;
                pConfirmNum.Value = ConfirmNum;
                pCarryOver.Value = CarryOver;
                pPayment.Value = Payment;
                pDatePaid.Value = DatePaid;
                pNote.Value = Note;

                cmdCode.Connection.Open();
                int iRetVal = cmdCode.ExecuteNonQuery();
                return iRetVal;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function InsertUpdateResdConfirmationCode

        //CALLED BY commissions.aspx to display confirmation for Commissions
        public DataSet GetConfirmationResd(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmdRep = new SqlCommand("sp_GetConfirmResd", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdRep.Parameters.Add(new SqlParameter("@Mon", Month));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
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
        }//end GetConfirmationResd

        public DataSet GetResdPayHistory(int PartnerID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetResdPayHistory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.Add(new SqlParameter("@PartnerID", PartnerID));
                cmd.Connection.Open();
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
        }//end GetResdPayHistory

        public DataSet GetT1Residuals(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetT1Residuals", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetPayrollTotalsT1

        public DataSet GetOfficeResiduals(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("SP_GetOfficeResiduals", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdResd.Parameters.Add("@Mon", SqlDbType.VarChar);
                pNum.Value = MasterNum;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetPayrollTotalsT1
    }//end class ResidualsDL
}
