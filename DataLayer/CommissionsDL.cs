using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class CommissionsDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        
        //CALLED BY CommissionsBL.GetRepList
        public DataSet GetRepList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "SELECT DISTINCT RepName, RepNum FROM CommissionsByRep ORDER BY RepName";
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "CommissionsByRep");
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
        }//end GetRepList

        /*
        //CALLED BY CommissionsBL.GetCommissionsTotals
        public DataSet GetCommissionTotals(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCommTotals", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmd.Parameters.Add(new SqlParameter("@Mon", Month));
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
        }//end GetCommissionsTotals
        */

        //Gets the Funding Information for a specified Rep or All Reps
        //CALLED BY CommissionsBL.GetFundedCount
        public DataSet GetFundedCount(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {

                SqlCommand cmd = new SqlCommand("sp_GetCommissionsFundedCount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@MasterNum", RepNum));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "FundedCountByRepMon");
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

        //Gets the Funding Information for a specified Rep or All Reps
        //CALLED BY CommissionsBL.GetFundedCount
        public DataSet GetFundedCountPeriod(string RepNum, string Month, string Period)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetCommissionsFundedCountPeriod", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@MasterNum", RepNum));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Period", Period));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "FundedCountByRepMonPeriod");
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

        //Returns the number of fundings for a specified Rep and month
        //CALLED BY
        public int ReturnFundedCount(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmd = new SqlCommand("sp_ReturnFundedCount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@MasterNum", RepNum));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                SqlParameter pCount = new SqlParameter("Count", SqlDbType.TinyInt);
                pCount.Direction = ParameterDirection.ReturnValue;

                SqlDataAdapter adapter = new SqlDataAdapter();
                cmd.ExecuteNonQuery(); 
               
                return Convert.ToInt32(pCount.Value);
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
    
        //CALLED BY CommissionsBL.GetBonusInfo
        public DataSet GetBonus(string MasterNum, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetRepBonus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmd.Parameters.Add(new SqlParameter("@Month", Month));
                cmd.Parameters.Add(new SqlParameter("@Period", Period));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Bonus");
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
        }//end GetBonus

        public DataSet GetReferalCagegory(string ReferalID)
        {
            SqlConnection Conn = new SqlConnection("ConnString");
            try
            {
                SqlCommand cmdGetRefList = new SqlCommand("SP_GetReferalCagegory", Conn);
                cmdGetRefList.CommandType = CommandType.StoredProcedure;
                cmdGetRefList.Parameters.Add(new SqlParameter("@ReferalID", ReferalID));
                SqlDataAdapter Adapter = new SqlDataAdapter();
                Adapter.SelectCommand = cmdGetRefList;
                DataSet ds = new DataSet();
                Adapter.Fill(ds);
                Conn.Close();
                Conn.Dispose();
                return ds;
            }
            catch (SqlException SqlErr)
            {
                throw SqlErr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        
        }
       
        //CALLED BY CommissionsBL.UpdateCommissionInfo
        public bool UpdateCommissions(string Product, string Referral, string Price, string COG, string Units, 
            string Comm, string FundedValue, string  ReferralPaid, int CommID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_UpdateCommRefPaid", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                SqlParameter pProduct = cmdComm.Parameters.Add("@Product", SqlDbType.VarChar);
                SqlParameter pReferral = cmdComm.Parameters.Add("@Referral", SqlDbType.VarChar);
                SqlParameter pPrice = cmdComm.Parameters.Add("@Price", SqlDbType.VarChar);
                SqlParameter pCOG = cmdComm.Parameters.Add("@COG", SqlDbType.VarChar);
                SqlParameter pUnits = cmdComm.Parameters.Add("@Units", SqlDbType.VarChar);
                SqlParameter pComm = cmdComm.Parameters.Add("@Comm", SqlDbType.VarChar);
                SqlParameter pFundedValue = cmdComm.Parameters.Add("@FundedValue", SqlDbType.VarChar);
                SqlParameter pReferralPaid = cmdComm.Parameters.Add("@ReferralPaid", SqlDbType.VarChar);
                SqlParameter pCommID = cmdComm.Parameters.Add("@CommID", SqlDbType.Int);
                pProduct.Value = Product;
                pReferral.Value = Referral;
                pPrice.Value = Price;
                pCOG.Value = COG;
                pUnits.Value = Units;
                pComm.Value = Comm;
                pFundedValue.Value = FundedValue;
                pReferralPaid.Value = ReferralPaid;
                pCommID.Value = CommID;

                cmdComm.Connection.Open();
                cmdComm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function UpdateCommissions

        public bool UpdateCommissions(string Product, string Referral, string Price, string COG, string Units,
            string Comm, string FundedValue, int CommID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_UpdateComm", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                SqlParameter pProduct = cmdComm.Parameters.Add("@Product", SqlDbType.VarChar);
                SqlParameter pReferral = cmdComm.Parameters.Add("@Referral", SqlDbType.VarChar);
                SqlParameter pPrice = cmdComm.Parameters.Add("@Price", SqlDbType.VarChar);
                SqlParameter pCOG = cmdComm.Parameters.Add("@COG", SqlDbType.VarChar);
                SqlParameter pUnits = cmdComm.Parameters.Add("@Units", SqlDbType.VarChar);
                SqlParameter pComm = cmdComm.Parameters.Add("@Comm", SqlDbType.VarChar);
                SqlParameter pFundedValue = cmdComm.Parameters.Add("@FundedValue", SqlDbType.VarChar);
                SqlParameter pCommID = cmdComm.Parameters.Add("@CommID", SqlDbType.SmallInt);
                pProduct.Value = Product;
                pReferral.Value = Referral;
                pPrice.Value = Price;
                pCOG.Value = COG;
                pUnits.Value = Units;
                pComm.Value = Comm;
                pFundedValue.Value = FundedValue;
                pCommID.Value = CommID;

                cmdComm.Connection.Open();
                cmdComm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function UpdateCommissions

        //This function inserts bonus info -- CALLED BY CommissionsBL.InsertBonus
        public int InsertBonus(string MasterNum, string BonusDesc, string Reason, string RepTotal, string Mon, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_InsertBonus", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdComm.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pBonusDesc = cmdComm.Parameters.Add("@BonusDesc", SqlDbType.VarChar);
                SqlParameter pMon = cmdComm.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pPeriod = cmdComm.Parameters.Add(new SqlParameter("@Period", SqlDbType.Int));
                SqlParameter pReason = cmdComm.Parameters.Add("@Reason", SqlDbType.VarChar);
                SqlParameter pRepTotal = cmdComm.Parameters.Add("@RepTotal", SqlDbType.VarChar);

                pMasterNum.Value = MasterNum;
                pBonusDesc.Value = BonusDesc;
                pReason.Value = Reason;
                pRepTotal.Value = RepTotal;
                pMon.Value = Mon;
                pPeriod.Value = Period;

                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
        }//end function InsertBonus

        //CALLED BY CommissionsBL.ResetComm, CommissionsBL.UploadCommissions
        public bool ResetCommissions(string RepNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_ResetCommPct", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepNum = cmdComm.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmdComm.Parameters.Add("@Mon", SqlDbType.VarChar);
                pRepNum.Value = RepNum;
                pMonth.Value = Month;

                cmdComm.Connection.Open();
                cmdComm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
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
        }//end function ResetCommissions

        
        public int InsertCommissions(string Month, string RepName, string Company, string DBA, string MerchantID, 
                                    string ReferralID, string NonAffiliateReferral, 
                                    string Product, string ProductCode, string COG, string Price,
                                    string Units, string CloseDate, string ReferalCategory, string Processor)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {      
                SqlCommand cmdComm = new SqlCommand("sp_InsertCommissions", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdComm.Parameters.Add(new SqlParameter("@RepName", RepName));
                cmdComm.Parameters.Add(new SqlParameter("@Company", Company));
                cmdComm.Parameters.Add(new SqlParameter("@DBA", DBA));
                cmdComm.Parameters.Add(new SqlParameter("@MerchantID", MerchantID));                
                cmdComm.Parameters.Add(new SqlParameter("@ReferralID", ReferralID));
                cmdComm.Parameters.Add(new SqlParameter("@NonAffiliateReferral", NonAffiliateReferral));
                cmdComm.Parameters.Add(new SqlParameter("@Product", Product));
                cmdComm.Parameters.Add(new SqlParameter("@ProductCode", ProductCode));
                cmdComm.Parameters.Add(new SqlParameter("@COG", COG));
                cmdComm.Parameters.Add(new SqlParameter("@Price", Price));                
                cmdComm.Parameters.Add(new SqlParameter("@Units", Units));
                cmdComm.Parameters.Add(new SqlParameter("@CloseDate", CloseDate));
                cmdComm.Parameters.Add(new SqlParameter("@ReferralCategory", ReferalCategory));
                cmdComm.Parameters.Add(new SqlParameter("@Processor", Processor));

                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
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
        }//end function InsertCommissions

        //This function encrypts merchant num
        public int EncryptCommissions(string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_EncryptCommissions", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
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
        }//end function EncryptCommissions

        //This function updates funded value and Referral totals in commission table
        //CALLED BY CommissionsBL.UploadCommissions

        public int UpdateCommBeforeJan2012()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_UpdateCommValuesBeforeJan2012", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                //cmdComm.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
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
        }

        public int UpdateAgentReferal(string Month, string strReferalCategory)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdAgentRef = new SqlCommand("SP_UpdateCommissionAgentReferal", Conn);
                cmdAgentRef.CommandType = CommandType.StoredProcedure;
                cmdAgentRef.Parameters.Add(new SqlParameter("@mon", Month));
                cmdAgentRef.Parameters.Add(new SqlParameter("@Category", strReferalCategory));
                cmdAgentRef.Connection.Open();
                int iRetVal = cmdAgentRef.ExecuteNonQuery();
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
        }

        public string GetPartnerType(int AffiliateID)
        {
            string retVal = "";
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetPartnerType",Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AffiliateId",AffiliateID));
                Conn.Open();
                /*
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();
                adapter.Fill(ds);*/
                object val = cmd.ExecuteScalar();
                retVal = Convert.ToString(val);
                Conn.Close();
                Conn.Dispose();
                return retVal;
            }
            catch(SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public int UpdateCommissionValues(string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_UpdateCommissionValues", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Parameters.Add(new SqlParameter("@Mon", Month));
                //cmdComm.Parameters.Add(new SqlParameter("@Processor", Processor));
                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
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
        }//end function UpdateCommissionFunds

        public int UpdateCommissionIntuitValues(int CommissionID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_UpdateCommissionIntuitValues", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Parameters.Add(new SqlParameter("@CommissionID", CommissionID));
                //cmdComm.Parameters.Add(new SqlParameter("@Processor", Processor));
                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
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
        }//end function UpdateCommissionFunds

        //This function deletes commissions based on month
        public int DeleteCommissions(string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_DeleteCommissions", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdComm.Connection.Open();
                int iRetVal = cmdComm.ExecuteNonQuery();
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
        }//end function UpdateCommissionFunds


        public string ReturnCurrMonth()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_ReturnCurrCommMonth", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                SqlParameter pCurrMonth = new SqlParameter("@CurrMon",SqlDbType.VarChar, 16);
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
        }//end function ReturnCurrMonth


        public DataSet GetCommPayment(string MasterNum, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCommPayment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmd.Parameters.Add(new SqlParameter("@Mon", Month));
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
        }//end GetCommPayment

        public DataSet GetCommRefPaymentByDD(int Employee, int DirectDeposit, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCommRefPayment", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Employee", Employee));
                cmd.Parameters.Add(new SqlParameter("@DirectDeposit", DirectDeposit));
                cmd.Parameters.Add(new SqlParameter("@Mon", Month));
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
        }//end GetCommRefPaymentByDD

        public DataSet GetCommRefPaymentByMonPeriod(int Employee, int DirectDeposit, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCommRefPaymentByMonPeriod", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Employee", Employee));
                cmd.Parameters.Add(new SqlParameter("@DirectDeposit", DirectDeposit));
                cmd.Parameters.Add(new SqlParameter("@Mon", Month));
                cmd.Parameters.Add(new SqlParameter("@Period", Period));
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
        }//end GetCommRefPaymentByDD

        public DataSet GetResdCommPayHistory(int PartnerID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetResdCommPayHistory", Conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
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
        }//end GetResdCommPayHistory

        public DataSet GetCommPayHistory(int PartnerID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCommPayHistory", Conn);
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
        }//end GetCommPayHistory

        public DataSet GetResdCommPaymentByMonPeriod(int Employee, int DirectDeposit, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                //SqlCommand cmd = new SqlCommand("SP_GetResdCommPaymentByMonPeriod_New", Conn);
                SqlCommand cmd = new SqlCommand("SP_GetResdCommPaymentByMonPeriod", Conn);
                cmd.CommandTimeout = 6000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Employee", Employee));
                cmd.Parameters.Add(new SqlParameter("@DirectDeposit", DirectDeposit));
                cmd.Parameters.Add(new SqlParameter("@Mon", Month));
                cmd.Parameters.Add(new SqlParameter("@Period", Period));
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
        }//end GetResdCommPaymentByMonPeriod


        public DataSet GetResdCommPaymentByMonPeriodOffice(int DirectDeposit, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                //SqlCommand cmd = new SqlCommand("SP_GetResdCommPaymentByMonPeriod_New", Conn);
                SqlCommand cmd = new SqlCommand("SP_GetResdCommPaymentOfficeByMonPeriod", Conn);
                cmd.CommandTimeout = 6000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@DirectDeposit", DirectDeposit));
                cmd.Parameters.Add(new SqlParameter("@Mon", Month));
                cmd.Parameters.Add(new SqlParameter("@Period", Period));
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
        }//end GetResdCommPaymentByMonPeriod

        public int InsertUpdateConfirmationCode(string AffiliateID, string Month, string ConfirmNum, string Note, decimal Carryover, 
            decimal Payment, string DatePaid)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCode = new SqlCommand("SP_InsertUpdateCommRefConfirmNum", Conn);
                cmdCode.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmdCode.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pMonth = cmdCode.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pConfirmNum = cmdCode.Parameters.Add("@CommRefConfirmNum", SqlDbType.VarChar);
                SqlParameter pNote= cmdCode.Parameters.Add("@CommRefNote", SqlDbType.VarChar);
                SqlParameter pCarryover = cmdCode.Parameters.Add("@Carryover", SqlDbType.Decimal);
                SqlParameter pPayment = cmdCode.Parameters.Add("@Payment", SqlDbType.Decimal);
                SqlParameter pDatePaid = cmdCode.Parameters.Add("@DatePaid", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;
                pMonth.Value = Month;
                pConfirmNum.Value = ConfirmNum;
                pDatePaid.Value = DatePaid;
                pNote.Value = Note;
                pCarryover.Value = Carryover;
                pPayment.Value = Payment;

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
        }//end function InsertUpdateConfirmationCode

        public int InsertUpdatePaymentInfo(string AffiliateID, string Month, string ConfirmNum, string Note, decimal Carryover,
            decimal Payment, string DatePaid, string Period)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCode = new SqlCommand("SP_InsertUpdatePaymentInfo", Conn);
                cmdCode.CommandType = CommandType.StoredProcedure;
                cmdCode.CommandTimeout = 200;
                SqlParameter pAffiliateID = cmdCode.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pMonth = cmdCode.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pConfirmNum = cmdCode.Parameters.Add("@ConfirmNum", SqlDbType.VarChar);
                SqlParameter pNote = cmdCode.Parameters.Add("@Note", SqlDbType.VarChar);
                SqlParameter pCarryover = cmdCode.Parameters.Add("@Carryover", SqlDbType.Decimal);
                SqlParameter pPayment = cmdCode.Parameters.Add("@Payment", SqlDbType.Decimal);
                SqlParameter pDatePaid = cmdCode.Parameters.Add("@DatePaid", SqlDbType.VarChar);
                SqlParameter pPeriod = cmdCode.Parameters.Add("@Period", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;
                pMonth.Value = Month;
                pConfirmNum.Value = ConfirmNum;
                pDatePaid.Value = DatePaid;
                pNote.Value = Note;
                pCarryover.Value = Carryover;
                pPayment.Value = Payment;
                pPeriod.Value = Period;

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
        }//end function InsertUpdateConfirmationCode


        }//end class CommissionsDL
}
