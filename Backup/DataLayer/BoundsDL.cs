using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class BoundsDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        
        //This function Gets processor list
        //CALLED BY BoundsBL.GetProcessorList
        public DataSet GetProcessorList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {               
                SqlCommand cmd = new SqlCommand("sp_GetProcessorList", Conn);               
                cmd.CommandType = CommandType.StoredProcedure;               
                cmd.Connection.Open();               
                SqlDataAdapter adapter = new SqlDataAdapter();               
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "ProcessorBounds");
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
        }//end function GetProcessorList

        //This function returns gateway list. CALLED BY BoundsBL.GetGatewayList
        public DataSet GetGatewayList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetGatewayList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "GatewayBounds");
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
        }//end function GetGatewayList

        public DataSet GetCheckServiceList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetCheckServiceList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "CheckServiceBounds");
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
        }//end function GetGatewayList

        public DataSet GetProcessorBoundComplianceFee(int ProcessorID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetProcessorBoundComplianceFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@ProcessorID", ProcessorID));
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
        }//end GetOnlineAppProfile

        public DataSet GetGiftCardList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetGiftCardList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "GiftCardBounds");
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
        }//end function GetGiftCardList

        //This function updates bounds for processor. CALLED BY BoundsBL.UpdateProcessorBounds
        public bool UpdateBounds(string LastModified, string CustServFeeLow, string InternetStmtLow, string TransFeeLow, string DRQPLow,
           string DRQNPLow, string DRMQLow, string DRNQLow, string DRQDLow, string ChargebackFeeLow,
            string RetrievalFeeLow, string VoiceAuthLow, string BatchHeaderLow, string AVSLow, string MonMinLow,
            string NBCTransFeeLow, string AnnualFeeLow, string WirelessAccessFeeLow, string WirelessTransFeeLow,
            string AppFeeLow, string AppSetupFeeLow, string ProcessorID, string DebitMonFee, string DebitTransFee,
            string EBTMonFee, string EBTTransFee)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateBounds", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pLastModified = cmdBounds.Parameters.Add("@LastModified", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmdBounds.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pInternetStmtLow  = cmdBounds.Parameters.Add("@InternetStmt", SqlDbType.VarChar);         
                SqlParameter pTransFeeLow = cmdBounds.Parameters.Add("@TransFee", SqlDbType.VarChar);
                SqlParameter pDRQPLow = cmdBounds.Parameters.Add("@DiscRateQualPres", SqlDbType.VarChar);
                SqlParameter pDRQNPLow = cmdBounds.Parameters.Add("@DiscRateQualNP", SqlDbType.VarChar);
                SqlParameter pDRMQLow = cmdBounds.Parameters.Add("@DiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pDRNQLow = cmdBounds.Parameters.Add("@DiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pDRQDLow = cmdBounds.Parameters.Add("@DiscRateQualDebit", SqlDbType.VarChar);
                SqlParameter pChargebackFeeLow = cmdBounds.Parameters.Add("@ChargebackFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFeeLow = cmdBounds.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuthLow = cmdBounds.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeaderLow = cmdBounds.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVSLow = cmdBounds.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pMonMinLow = cmdBounds.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pNBCTransFeeLow = cmdBounds.Parameters.Add("@NBCTransFee", SqlDbType.VarChar);
                SqlParameter pAnnualFeeLow = cmdBounds.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccessFeeLow = cmdBounds.Parameters.Add("@WirelessAccessFee", SqlDbType.VarChar);
                SqlParameter pWirelessTransFeeLow = cmdBounds.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pAppFeeLow = cmdBounds.Parameters.Add("@AppFee", SqlDbType.VarChar);
                SqlParameter pAppSetupFeeLow = cmdBounds.Parameters.Add("@AppSetupFee", SqlDbType.VarChar);
                SqlParameter pProcessorID = cmdBounds.Parameters.Add("@ProcID", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmdBounds.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmdBounds.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmdBounds.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmdBounds.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);

                pLastModified.Value = LastModified;
                pCustServFee.Value = CustServFeeLow;
                pInternetStmtLow.Value = InternetStmtLow;                
                pTransFeeLow.Value = TransFeeLow;
                pDRQPLow.Value = DRQPLow;
                pDRQNPLow.Value = DRQNPLow;
                pDRMQLow.Value = DRMQLow;
                pDRNQLow.Value = DRNQLow;
                pDRQDLow.Value = DRQDLow;
                pChargebackFeeLow.Value = ChargebackFeeLow;
                pRetrievalFeeLow.Value = RetrievalFeeLow;
                pVoiceAuthLow.Value = VoiceAuthLow;
                pBatchHeaderLow.Value = BatchHeaderLow;
                pAVSLow.Value = AVSLow;
                pMonMinLow.Value = MonMinLow;
                pNBCTransFeeLow.Value = NBCTransFeeLow;
                pAnnualFeeLow.Value = AnnualFeeLow;
                pWirelessAccessFeeLow.Value = WirelessAccessFeeLow;
                pWirelessTransFeeLow.Value = WirelessTransFeeLow;
                pAppFeeLow.Value = AppFeeLow;
                pAppSetupFeeLow.Value = AppSetupFeeLow;
                pProcessorID.Value = ProcessorID;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateBounds

        //This function updates bounds for processor. CALLED BY BoundsBL.UpdateProcessorDefaults
        public bool UpdateDefaults(string LastModified, string CustServFeeLow, string InternetStmtLow, string TransFeeLow, string DRQPLow,
            string DRQNPLow, string DRMQLow, string DRNQLow, string DRQDLow, string ChargebackFeeLow,
            string RetrievalFeeLow, string VoiceAuthLow, string BatchHeaderLow, string AVSLow, string MonMinLow,
            string NBCTransFeeLow, string AnnualFeeLow, string WirelessAccessFeeLow, string WirelessTransFeeLow,
            string AppFeeLow, string AppSetupFeeLow, string ProcessorID, string DebitMonFee, string DebitTransFee,
            string EBTMonFee, string EBTTransFee)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateDefaults", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pLastModified = cmdBounds.Parameters.Add("@LastModified", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmdBounds.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pInternetStmtLow = cmdBounds.Parameters.Add("@InternetStmt", SqlDbType.VarChar);               
                SqlParameter pTransFeeLow = cmdBounds.Parameters.Add("@TransFee", SqlDbType.VarChar);
                SqlParameter pDRQPLow = cmdBounds.Parameters.Add("@DiscRateQualPres", SqlDbType.VarChar);
                SqlParameter pDRQNPLow = cmdBounds.Parameters.Add("@DiscRateQualNP", SqlDbType.VarChar);
                SqlParameter pDRMQLow = cmdBounds.Parameters.Add("@DiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pDRNQLow = cmdBounds.Parameters.Add("@DiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pDRQDLow = cmdBounds.Parameters.Add("@DiscRateQualDebit", SqlDbType.VarChar);
                SqlParameter pChargebackFeeLow = cmdBounds.Parameters.Add("@ChargebackFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFeeLow = cmdBounds.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuthLow = cmdBounds.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeaderLow = cmdBounds.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVSLow = cmdBounds.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pMonMinLow = cmdBounds.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pNBCTransFeeLow = cmdBounds.Parameters.Add("@NBCTransFee", SqlDbType.VarChar);
                SqlParameter pAnnualFeeLow = cmdBounds.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccessFeeLow = cmdBounds.Parameters.Add("@WirelessAccessFee", SqlDbType.VarChar);
                SqlParameter pWirelessTransFeeLow = cmdBounds.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pAppFeeLow = cmdBounds.Parameters.Add("@AppFee", SqlDbType.VarChar);
                SqlParameter pAppSetupFeeLow = cmdBounds.Parameters.Add("@AppSetupFee", SqlDbType.VarChar);
                SqlParameter pProcessorID = cmdBounds.Parameters.Add("@ProcID", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmdBounds.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmdBounds.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmdBounds.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmdBounds.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);

                pLastModified.Value = LastModified;
                pCustServFee.Value = CustServFeeLow;
                pInternetStmtLow.Value = InternetStmtLow;
                pTransFeeLow.Value = TransFeeLow;
                pDRQPLow.Value = DRQPLow;
                pDRQNPLow.Value = DRQNPLow;
                pDRMQLow.Value = DRMQLow;
                pDRNQLow.Value = DRNQLow;
                pDRQDLow.Value = DRQDLow;
                pChargebackFeeLow.Value = ChargebackFeeLow;
                pRetrievalFeeLow.Value = RetrievalFeeLow;
                pVoiceAuthLow.Value = VoiceAuthLow;
                pBatchHeaderLow.Value = BatchHeaderLow;
                pAVSLow.Value = AVSLow;
                pMonMinLow.Value = MonMinLow;
                pNBCTransFeeLow.Value = NBCTransFeeLow;
                pAnnualFeeLow.Value = AnnualFeeLow;
                pWirelessAccessFeeLow.Value = WirelessAccessFeeLow;
                pWirelessTransFeeLow.Value = WirelessTransFeeLow;
                pAppFeeLow.Value = AppFeeLow;
                pAppSetupFeeLow.Value = AppSetupFeeLow;
                pProcessorID.Value = ProcessorID;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateDefaults


        public bool UpdateDefaults(string LastModified, string CustServFeeLow, string InternetStmtLow, string TransFeeLow, string DRQPLow,
            string DRQNPLow, string DRMQLow, string DRNQLow, string DRQDLow, string ChargebackFeeLow,
            string RetrievalFeeLow, string VoiceAuthLow, string BatchHeaderLow, string AVSLow, string MonMinLow,
            string NBCTransFeeLow, string AnnualFeeLow, string WirelessAccessFeeLow, string WirelessTransFeeLow,
            string AppFeeLow, string AppSetupFeeLow, string ProcessorID, string DebitMonFee, string DebitTransFee,
            string EBTMonFee, string EBTTransFee, string ComplianceFee)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateDefaultsProcessor", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pLastModified = cmdBounds.Parameters.Add("@LastModified", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmdBounds.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pInternetStmtLow = cmdBounds.Parameters.Add("@InternetStmt", SqlDbType.VarChar);
                SqlParameter pTransFeeLow = cmdBounds.Parameters.Add("@TransFee", SqlDbType.VarChar);
                SqlParameter pDRQPLow = cmdBounds.Parameters.Add("@DiscRateQualPres", SqlDbType.VarChar);
                SqlParameter pDRQNPLow = cmdBounds.Parameters.Add("@DiscRateQualNP", SqlDbType.VarChar);
                SqlParameter pDRMQLow = cmdBounds.Parameters.Add("@DiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pDRNQLow = cmdBounds.Parameters.Add("@DiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pDRQDLow = cmdBounds.Parameters.Add("@DiscRateQualDebit", SqlDbType.VarChar);
                SqlParameter pChargebackFeeLow = cmdBounds.Parameters.Add("@ChargebackFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFeeLow = cmdBounds.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuthLow = cmdBounds.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeaderLow = cmdBounds.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVSLow = cmdBounds.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pMonMinLow = cmdBounds.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pNBCTransFeeLow = cmdBounds.Parameters.Add("@NBCTransFee", SqlDbType.VarChar);
                SqlParameter pAnnualFeeLow = cmdBounds.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccessFeeLow = cmdBounds.Parameters.Add("@WirelessAccessFee", SqlDbType.VarChar);
                SqlParameter pWirelessTransFeeLow = cmdBounds.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pAppFeeLow = cmdBounds.Parameters.Add("@AppFee", SqlDbType.VarChar);
                SqlParameter pAppSetupFeeLow = cmdBounds.Parameters.Add("@AppSetupFee", SqlDbType.VarChar);
                SqlParameter pProcessorID = cmdBounds.Parameters.Add("@ProcID", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmdBounds.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmdBounds.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmdBounds.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmdBounds.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);
                SqlParameter pComplianceFee = cmdBounds.Parameters.Add("@ComplianceFee", SqlDbType.VarChar);

                pLastModified.Value = LastModified;
                pCustServFee.Value = CustServFeeLow;
                pInternetStmtLow.Value = InternetStmtLow;
                pTransFeeLow.Value = TransFeeLow;
                pDRQPLow.Value = DRQPLow;
                pDRQNPLow.Value = DRQNPLow;
                pDRMQLow.Value = DRMQLow;
                pDRNQLow.Value = DRNQLow;
                pDRQDLow.Value = DRQDLow;
                pChargebackFeeLow.Value = ChargebackFeeLow;
                pRetrievalFeeLow.Value = RetrievalFeeLow;
                pVoiceAuthLow.Value = VoiceAuthLow;
                pBatchHeaderLow.Value = BatchHeaderLow;
                pAVSLow.Value = AVSLow;
                pMonMinLow.Value = MonMinLow;
                pNBCTransFeeLow.Value = NBCTransFeeLow;
                pAnnualFeeLow.Value = AnnualFeeLow;
                pWirelessAccessFeeLow.Value = WirelessAccessFeeLow;
                pWirelessTransFeeLow.Value = WirelessTransFeeLow;
                pAppFeeLow.Value = AppFeeLow;
                pAppSetupFeeLow.Value = AppSetupFeeLow;
                pProcessorID.Value = ProcessorID;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;
                pComplianceFee.Value = ComplianceFee;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateDefaults

        //This function updates bounds for Gateways. CALLED BY BoundsBL.UpdateGatewayBounds
        public bool UpdateGatewayBounds(string GWSetupFee, string GWMonthlyFee, string GWTransFee, string GatewayID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateGatewayBounds", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pGWSetupFee = cmdBounds.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);
                SqlParameter pGWMonFee = cmdBounds.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGWTransFee = cmdBounds.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGatewayID = cmdBounds.Parameters.Add("@GatewayID", SqlDbType.VarChar);

                pGWSetupFee.Value = GWSetupFee;
                pGWMonFee.Value = GWMonthlyFee;
                pGWTransFee.Value = GWTransFee;
                pGatewayID.Value = GatewayID;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateGatewayBounds

        public bool UpdateCheckServiceBounds(string CheckServiceDiscRate, string CheckServiceMonFee, string CheckServiceMonMin, string CheckServiceTransFee, string CheckServiceID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateCheckServiceBounds", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pCheckServiceDiscRate = cmdBounds.Parameters.Add("@CheckServiceDiscRate", SqlDbType.VarChar);
                SqlParameter pCheckServiceMonFee = cmdBounds.Parameters.Add("@CheckServiceMonFee", SqlDbType.VarChar);
                SqlParameter pCheckServiceMonMin = cmdBounds.Parameters.Add("@CheckServiceMonMin", SqlDbType.VarChar);
                SqlParameter pCheckServiceTransFee = cmdBounds.Parameters.Add("@CheckServiceTransFee", SqlDbType.VarChar);
                SqlParameter pCheckServiceID = cmdBounds.Parameters.Add("@CheckServiceID", SqlDbType.VarChar);

                pCheckServiceDiscRate.Value = CheckServiceDiscRate;
                pCheckServiceMonFee.Value = CheckServiceMonFee;
                pCheckServiceMonMin.Value = CheckServiceMonMin;
                pCheckServiceTransFee.Value = CheckServiceTransFee;
                pCheckServiceID.Value = CheckServiceID;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateGatewayBounds

        public bool UpdateCheckServiceDefaults(string CheckServiceDiscRate, string CheckServiceMonFee, string CheckServiceMonMin, string CheckServiceTransFee, string CheckServiceID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateCheckServiceDefaults", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pCheckServiceDiscRate = cmdBounds.Parameters.Add("@CheckServiceDiscRate", SqlDbType.VarChar);
                SqlParameter pCheckServiceMonFee = cmdBounds.Parameters.Add("@CheckServiceMonFee", SqlDbType.VarChar);
                SqlParameter pCheckServiceMonMin = cmdBounds.Parameters.Add("@CheckServiceMonMin", SqlDbType.VarChar);
                SqlParameter pCheckServiceTransFee = cmdBounds.Parameters.Add("@CheckServiceTransFee", SqlDbType.VarChar);
                SqlParameter pCheckServiceID = cmdBounds.Parameters.Add("@CheckServiceID", SqlDbType.VarChar);

                pCheckServiceDiscRate.Value = CheckServiceDiscRate;
                pCheckServiceMonFee.Value = CheckServiceMonFee;
                pCheckServiceMonMin.Value = CheckServiceMonMin;
                pCheckServiceTransFee.Value = CheckServiceTransFee;
                pCheckServiceID.Value = CheckServiceID;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateGatewayBounds

        public bool UpdateGiftCardDefaults(string GiftCardMonFee, string GiftCardTransFee, string GiftCardID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateGiftCardDefaults", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pGiftCardMonFee = cmdBounds.Parameters.Add("@GiftCardMonFee", SqlDbType.VarChar);
                SqlParameter pGiftCardTransFee = cmdBounds.Parameters.Add("@GiftCardTransFee", SqlDbType.VarChar);
                SqlParameter pGiftCardID = cmdBounds.Parameters.Add("@GiftCardID", SqlDbType.VarChar);

                pGiftCardMonFee.Value = GiftCardMonFee;
                pGiftCardTransFee.Value = GiftCardTransFee;
                pGiftCardID.Value = GiftCardID;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateGatewayBounds

        public bool UpdateGiftCardBounds(string GiftCardMonFee, string GiftCardTransFee, string GiftCardID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateGiftCardBounds", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pGiftCardMonFee = cmdBounds.Parameters.Add("@GiftCardMonFee", SqlDbType.VarChar);
                SqlParameter pGiftCardTransFee = cmdBounds.Parameters.Add("@GiftCardTransFee", SqlDbType.VarChar);
                SqlParameter pGiftCardID = cmdBounds.Parameters.Add("@GiftCardID", SqlDbType.VarChar);

                pGiftCardMonFee.Value = GiftCardMonFee;
                pGiftCardTransFee.Value = GiftCardTransFee;
                pGiftCardID.Value = GiftCardID;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateGatewayBounds

        //This function updates bounds for Gateways. CALLED BY BoundsBL.UpdateGatewayBounds
        public bool UpdateGatewayDefaults(string GWSetupFee, string GWMonthlyFee, string GWTransFee, string GatewayID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdBounds = new SqlCommand("sp_UpdateGatewayDefaults", Conn);
                cmdBounds.CommandType = CommandType.StoredProcedure;
                SqlParameter pGWSetupFee = cmdBounds.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);
                SqlParameter pGWMonFee = cmdBounds.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGWTransFee = cmdBounds.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGatewayID = cmdBounds.Parameters.Add("@GatewayID", SqlDbType.VarChar);

                pGWSetupFee.Value = GWSetupFee;
                pGWMonFee.Value = GWMonthlyFee;
                pGWTransFee.Value = GWTransFee;
                pGatewayID.Value = GatewayID;

                cmdBounds.Connection.Open();
                cmdBounds.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end function UpdateGatewayDefaults
        
        public DataSet GetCSBounds(string CheckService)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetCSBounds", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@CheckService", CheckService));
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
        }//end GetCSBounds

        public DataSet GetGiftBoundsbyID(int GiftCardID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetGiftBoundsbyID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@GiftCardID", GiftCardID));
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
        }//end GetCSBounds

        public DataSet GetGiftBounds(string GiftCardID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetGiftBounds", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@GiftCardID", GiftCardID));
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
        }//end GetCSBounds
      
    }//end class SetBoundsDL
}
