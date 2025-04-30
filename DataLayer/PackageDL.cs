using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class PackageDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
              
        public int DeletePackageInfo(int PackageID)
        {
            int RetVal = 0;
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_DeletePackage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@PackageID", PackageID);
                SqlParameter pRetVal = new SqlParameter("@RetVal", SqlDbType.Int);
                pRetVal.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(pRetVal);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                //returns 0 if package cannot be deleted, 1 if successful
                RetVal = Convert.ToInt16(pRetVal.Value);    
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
            return RetVal;
        }//end ReturnPackageInfo

        //CALLED BY PackageBL.InsertPackage
        public bool InsertPackageInfo(string PackagePrefix, string Processor, string PackageSuffix, string CardPresent,
            string RepNum, string CustServFee, string InternetStmt, string TransactionFee, string DiscRateQualPres, string DiscRateQualNP,
            string DiscRateMidQual, string DiscRateNonQual,
            string AmexDiscRateQual,
            string AmexDiscRateMidQual, string AmexDiscRateNonQual,
            string DiscRateQualDebit, string ChargebackFee,
            string RetrievalFee, string VoiceAuth, string BatchHeader, string AVS, string MonMin,
            string NBCTransFee, string AnnualFee, string WirelessAccessFee, string WirelessTransFee,
            string AppFee, string AppSetupFee, string Gateway, string GatewayTransFee, string GatewayMonFee,
            string GatewaySetupFee, string DebitMonFee, string DebitTransFee, 
            string CGDiscRate, string CGMonFee, string CGMonMin, string CGTransFee, 
            string GCMonFee, string GCTransFee, string EBTMonFee, 
            string EBTTransFee, string RollingReserve, bool bInterchange, bool bAssessments)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_InsertPackage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pPackagePrefix = cmd.Parameters.Add("@PackagePrefix", SqlDbType.VarChar);
                SqlParameter pPackageSuffix = cmd.Parameters.Add("@PackageSuffix", SqlDbType.VarChar);
                SqlParameter pProcessor = cmd.Parameters.Add("@Processor", SqlDbType.VarChar);
                SqlParameter pCardPresent = cmd.Parameters.Add("@CardPresent", SqlDbType.VarChar);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmd.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pInternetStmt = cmd.Parameters.Add("@InternetStmt", SqlDbType.VarChar);             
                SqlParameter pTransFee = cmd.Parameters.Add("@TransactionFee", SqlDbType.VarChar);
                SqlParameter pDiscRateQualPres = cmd.Parameters.Add("@DiscRateQualPres", SqlDbType.VarChar);
                SqlParameter pDiscRateQualNP = cmd.Parameters.Add("@DiscRateQualNP", SqlDbType.VarChar);
                SqlParameter pDiscRateMidQual = cmd.Parameters.Add("@DiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pDiscRateNonQual = cmd.Parameters.Add("@DiscRateNonQual", SqlDbType.VarChar);

                SqlParameter pAmexDiscRateQual = cmd.Parameters.Add("@AmexDiscRateQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateMidQual = cmd.Parameters.Add("@AmexDiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateNonQual = cmd.Parameters.Add("@AmexDiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pDiscRateQualDebit = cmd.Parameters.Add("@DiscRateQualDebit", SqlDbType.VarChar);
                SqlParameter pChargebackFee = cmd.Parameters.Add("@ChargebackFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFee = cmd.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuth = cmd.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeader = cmd.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVS = cmd.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pMonMin = cmd.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pNBCTransFee = cmd.Parameters.Add("@NBCTransFee", SqlDbType.VarChar);
                SqlParameter pAnnualFee = cmd.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccessFee = cmd.Parameters.Add("@WirelessAccessFee", SqlDbType.VarChar);
                SqlParameter pWirelessTransFee = cmd.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pAppFee = cmd.Parameters.Add("@AppFee", SqlDbType.VarChar);
                SqlParameter pAppSetupFee = cmd.Parameters.Add("@AppSetupFee", SqlDbType.VarChar);
                SqlParameter pGateway = cmd.Parameters.Add("@Gateway", SqlDbType.VarChar);
                SqlParameter pGatewayTransFee = cmd.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGatewayMonFee = cmd.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGatewaySetupFee = cmd.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmd.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmd.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonFee = cmd.Parameters.Add("@CGMonFee", SqlDbType.VarChar);
                SqlParameter pCGTransFee = cmd.Parameters.Add("@CGTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonMin = cmd.Parameters.Add("@CGMonMin", SqlDbType.VarChar);
                SqlParameter pCGDiscRate = cmd.Parameters.Add("@CGDiscRate", SqlDbType.VarChar);
                SqlParameter pGCMonFee = cmd.Parameters.Add("@GCMonFee", SqlDbType.VarChar);
                SqlParameter pGCTransFee = cmd.Parameters.Add("@GCTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmd.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmd.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);
                SqlParameter pRollingReserve = cmd.Parameters.Add("@RollingReserve", SqlDbType.VarChar);
                SqlParameter pAssessments = cmd.Parameters.Add("@Assessments", SqlDbType.VarChar);
                SqlParameter pInterchange = cmd.Parameters.Add("@Interchange", SqlDbType.VarChar);

                pPackagePrefix.Value = PackagePrefix;
                pProcessor.Value = Processor;
                pPackageSuffix.Value = PackageSuffix;
                pCardPresent.Value = CardPresent;
                pRepNum.Value = RepNum;
                pCustServFee.Value = CustServFee;
                pInternetStmt.Value = InternetStmt;
                pTransFee.Value = TransactionFee;
                pDiscRateQualPres.Value = DiscRateQualPres;
                pDiscRateQualNP.Value = DiscRateQualNP;
                pDiscRateMidQual.Value = DiscRateMidQual;
                pDiscRateNonQual.Value = DiscRateNonQual;
                pAmexDiscRateQual.Value = AmexDiscRateQual;
                pAmexDiscRateMidQual.Value = AmexDiscRateMidQual;
                pAmexDiscRateNonQual.Value = AmexDiscRateNonQual;
                pDiscRateQualDebit.Value = DiscRateQualDebit;
                pChargebackFee.Value = ChargebackFee;
                pRetrievalFee.Value = RetrievalFee;
                pVoiceAuth.Value = VoiceAuth;
                pBatchHeader.Value = BatchHeader;
                pAVS.Value = AVS;
                pMonMin.Value = MonMin;
                pNBCTransFee.Value = NBCTransFee;
                pAnnualFee.Value = AnnualFee;
                pWirelessAccessFee.Value = WirelessAccessFee;
                pWirelessTransFee.Value = WirelessTransFee;
                pAppFee.Value = AppFee;
                pAppSetupFee.Value = AppSetupFee;
                pGateway.Value = Gateway;
                pGatewayTransFee.Value = GatewayTransFee;
                pGatewayMonFee.Value = GatewayMonFee;
                pGatewaySetupFee.Value = GatewaySetupFee;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pCGMonFee.Value = CGMonFee;
                pCGTransFee.Value = CGTransFee;
                pCGMonMin.Value = CGMonMin;
                pCGDiscRate.Value = CGDiscRate;
                pGCMonFee.Value = GCMonFee;
                pGCTransFee.Value = GCTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;
                pRollingReserve.Value = RollingReserve;
                pInterchange.Value = bInterchange;
                pAssessments.Value = bAssessments;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end class InsertPackageInfo

        public bool UpdatePackageInfo(int PackageID, string PackagePrefix, string PackageSuffix,
            string CustServFee, string InternetStmt, string TransactionFee, string DiscRateQualPres, string DiscRateQualNP,
            string DiscRateMidQual, string DiscRateNonQual, string AmexDiscRateQual, string AmexDiscRateMidQual, string AmexDiscRateNonQual, string DiscRateQualDebit, string QualNPDebit, string MidQualDebit, string NonQualDebit, string ChargebackFee,
            string RetrievalFee, string VoiceAuth, string BatchHeader, string AVS, string MonMin,
            string NBCTransFee, string AnnualFee, string WirelessAccessFee, string WirelessTransFee,
            string AppFee, string AppSetupFee, string Gateway, string GatewayTransFee, string GatewayMonFee,
            string GatewaySetupFee, string DebitMonFee, string DebitTransFee,
            string CGDiscRate, string CGMonFee, string CGMonMin, string CGTransFee,
            string GCMonFee, string GCTransFee, string EBTMonFee,
            string EBTTransFee, string PinDebitDiscount, string ComplianceFee)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdPackage = new SqlCommand("sp_UpdatePackage", Conn);
                cmdPackage.CommandType = CommandType.StoredProcedure;
                SqlParameter pPackageID = cmdPackage.Parameters.Add("@PID", SqlDbType.SmallInt);
                SqlParameter pPackagePrefix = cmdPackage.Parameters.Add("@PackagePrefix", SqlDbType.VarChar);
                SqlParameter pPackageSuffix = cmdPackage.Parameters.Add("@PackageSuffix", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmdPackage.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pInternetStmt = cmdPackage.Parameters.Add("@InternetStmt", SqlDbType.VarChar);               
                SqlParameter pTransFee = cmdPackage.Parameters.Add("@TransactionFee", SqlDbType.VarChar);
                SqlParameter pDiscRateQualPres = cmdPackage.Parameters.Add("@DiscRateQualPres", SqlDbType.VarChar);
                SqlParameter pDiscRateQualNP = cmdPackage.Parameters.Add("@DiscRateQualNP", SqlDbType.VarChar);
                SqlParameter pDiscRateMidQual = cmdPackage.Parameters.Add("@DiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pDiscRateNonQual = cmdPackage.Parameters.Add("@DiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateQual = cmdPackage.Parameters.Add("@AmexDiscRateQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateMidQual = cmdPackage.Parameters.Add("@AmexDiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateNonQual = cmdPackage.Parameters.Add("@AmexDiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pDiscRateQualDebit = cmdPackage.Parameters.Add("@DiscRateQualDebit", SqlDbType.VarChar);

                SqlParameter pQualNPDebit = cmdPackage.Parameters.Add("@QualNPDebit", SqlDbType.VarChar);
                SqlParameter pMidQualDebit = cmdPackage.Parameters.Add("@MidQualDebit", SqlDbType.VarChar);
                SqlParameter pNonQualDebit = cmdPackage.Parameters.Add("@NonQualDebit", SqlDbType.VarChar);

                SqlParameter pChargebackFee = cmdPackage.Parameters.Add("@ChargebackFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFee = cmdPackage.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuth = cmdPackage.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeader = cmdPackage.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVS = cmdPackage.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pMonMin = cmdPackage.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pNBCTransFee = cmdPackage.Parameters.Add("@NBCTransFee", SqlDbType.VarChar);
                SqlParameter pAnnualFee = cmdPackage.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccessFee = cmdPackage.Parameters.Add("@WirelessAccessFee", SqlDbType.VarChar);
                SqlParameter pWirelessTransFee = cmdPackage.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pAppFee = cmdPackage.Parameters.Add("@AppFee", SqlDbType.VarChar);
                SqlParameter pAppSetupFee = cmdPackage.Parameters.Add("@AppSetupFee", SqlDbType.VarChar);
                SqlParameter pGateway = cmdPackage.Parameters.Add("@Gateway", SqlDbType.VarChar);
                SqlParameter pGatewayTransFee = cmdPackage.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGatewayMonFee = cmdPackage.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGatewaySetupFee = cmdPackage.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmdPackage.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmdPackage.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pPinDebitDiscount = cmdPackage.Parameters.Add("@PinDebitDiscount", SqlDbType.VarChar);
                SqlParameter pCGMonFee = cmdPackage.Parameters.Add("@CGMonFee", SqlDbType.VarChar);
                SqlParameter pCGTransFee = cmdPackage.Parameters.Add("@CGTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonMin = cmdPackage.Parameters.Add("@CGMonMin", SqlDbType.VarChar);
                SqlParameter pCGDiscRate = cmdPackage.Parameters.Add("@CGDiscRate", SqlDbType.VarChar);
                SqlParameter pGCMonFee = cmdPackage.Parameters.Add("@GCMonFee", SqlDbType.VarChar);
                SqlParameter pGCTransFee = cmdPackage.Parameters.Add("@GCTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmdPackage.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmdPackage.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);
                SqlParameter pComplianceFee = cmdPackage.Parameters.Add("@ComplianceFee", SqlDbType.VarChar);

                pPackageID.Value = PackageID;
                pPackagePrefix.Value = PackagePrefix;
                pPackageSuffix.Value = PackageSuffix;
                pCustServFee.Value = CustServFee;
                pInternetStmt.Value = InternetStmt;
                pTransFee.Value = TransactionFee;
                pDiscRateQualPres.Value = DiscRateQualPres;
                pDiscRateQualNP.Value = DiscRateQualNP;
                pDiscRateMidQual.Value = DiscRateMidQual;
                pDiscRateNonQual.Value = DiscRateNonQual;
                pAmexDiscRateQual.Value = AmexDiscRateQual;
                pAmexDiscRateMidQual.Value = AmexDiscRateMidQual;
                pAmexDiscRateNonQual.Value = AmexDiscRateNonQual;
                pDiscRateQualDebit.Value = DiscRateQualDebit;

                pQualNPDebit.Value = QualNPDebit;
                pMidQualDebit.Value = MidQualDebit;
                pNonQualDebit.Value = NonQualDebit;

                pChargebackFee.Value = ChargebackFee;
                pRetrievalFee.Value = RetrievalFee;
                pVoiceAuth.Value = VoiceAuth;
                pBatchHeader.Value = BatchHeader;
                pAVS.Value = AVS;
                pMonMin.Value = MonMin;
                pNBCTransFee.Value = NBCTransFee;
                pAnnualFee.Value = AnnualFee;
                pWirelessAccessFee.Value = WirelessAccessFee;
                pWirelessTransFee.Value = WirelessTransFee;
                pAppFee.Value = AppFee;
                pAppSetupFee.Value = AppSetupFee;
                pGateway.Value = Gateway;
                pGatewayTransFee.Value = GatewayTransFee;
                pGatewayMonFee.Value = GatewayMonFee;
                pGatewaySetupFee.Value = GatewaySetupFee;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pPinDebitDiscount.Value = PinDebitDiscount;
                pCGMonFee.Value = CGMonFee;
                pCGTransFee.Value = CGTransFee;
                pCGMonMin.Value = CGMonMin;
                pCGDiscRate.Value = CGDiscRate;
                pGCMonFee.Value = GCMonFee;
                pGCTransFee.Value = GCTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;
                pComplianceFee.Value = ComplianceFee;

                cmdPackage.Connection.Open();
                cmdPackage.ExecuteNonQuery();
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
        }//end class UpdatePackageInfo

        public bool UpdatePackageInfoBillingMethod(int PackageID, string PackagePrefix, string PackageSuffix,
    string CustServFee, string InternetStmt, string TransactionFee, string DiscRateQualPres, string DiscRateQualNP,
    string DiscRateMidQual, string DiscRateNonQual, string AmexDiscRateQual, string AmexDiscRateMidQual, string AmexDiscRateNonQual, string DiscRateQualDebit, string QualNPDebit, string MidQualDebit, string NonQualDebit, string ChargebackFee,
    string RetrievalFee, string VoiceAuth, string BatchHeader, string AVS, string MonMin,
    string NBCTransFee, string AnnualFee, string WirelessAccessFee, string WirelessTransFee,
    string AppFee, string AppSetupFee, string Gateway, string GatewayTransFee, string GatewayMonFee,
    string GatewaySetupFee, string DebitMonFee, string DebitTransFee,
    string CGDiscRate, string CGMonFee, string CGMonMin, string CGTransFee,
    string GCMonFee, string GCTransFee, string EBTMonFee,
    string EBTTransFee, string PinDebitDiscount, string ComplianceFee, string BillingMethod)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdPackage = new SqlCommand("sp_UpdatePackageBillingMethod", Conn);
                cmdPackage.CommandType = CommandType.StoredProcedure;
                SqlParameter pPackageID = cmdPackage.Parameters.Add("@PID", SqlDbType.SmallInt);
                SqlParameter pPackagePrefix = cmdPackage.Parameters.Add("@PackagePrefix", SqlDbType.VarChar);
                SqlParameter pPackageSuffix = cmdPackage.Parameters.Add("@PackageSuffix", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmdPackage.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pInternetStmt = cmdPackage.Parameters.Add("@InternetStmt", SqlDbType.VarChar);
                SqlParameter pTransFee = cmdPackage.Parameters.Add("@TransactionFee", SqlDbType.VarChar);
                SqlParameter pDiscRateQualPres = cmdPackage.Parameters.Add("@DiscRateQualPres", SqlDbType.VarChar);
                SqlParameter pDiscRateQualNP = cmdPackage.Parameters.Add("@DiscRateQualNP", SqlDbType.VarChar);
                SqlParameter pDiscRateMidQual = cmdPackage.Parameters.Add("@DiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pDiscRateNonQual = cmdPackage.Parameters.Add("@DiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateQual = cmdPackage.Parameters.Add("@AmexDiscRateQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateMidQual = cmdPackage.Parameters.Add("@AmexDiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateNonQual = cmdPackage.Parameters.Add("@AmexDiscRateNonQual", SqlDbType.VarChar);
                SqlParameter pDiscRateQualDebit = cmdPackage.Parameters.Add("@DiscRateQualDebit", SqlDbType.VarChar);

                SqlParameter pQualNPDebit = cmdPackage.Parameters.Add("@QualNPDebit", SqlDbType.VarChar);
                SqlParameter pMidQualDebit = cmdPackage.Parameters.Add("@MidQualDebit", SqlDbType.VarChar);
                SqlParameter pNonQualDebit = cmdPackage.Parameters.Add("@NonQualDebit", SqlDbType.VarChar);

                SqlParameter pChargebackFee = cmdPackage.Parameters.Add("@ChargebackFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFee = cmdPackage.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuth = cmdPackage.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeader = cmdPackage.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVS = cmdPackage.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pMonMin = cmdPackage.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pNBCTransFee = cmdPackage.Parameters.Add("@NBCTransFee", SqlDbType.VarChar);
                SqlParameter pAnnualFee = cmdPackage.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccessFee = cmdPackage.Parameters.Add("@WirelessAccessFee", SqlDbType.VarChar);
                SqlParameter pWirelessTransFee = cmdPackage.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pAppFee = cmdPackage.Parameters.Add("@AppFee", SqlDbType.VarChar);
                SqlParameter pAppSetupFee = cmdPackage.Parameters.Add("@AppSetupFee", SqlDbType.VarChar);
                SqlParameter pGateway = cmdPackage.Parameters.Add("@Gateway", SqlDbType.VarChar);
                SqlParameter pGatewayTransFee = cmdPackage.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGatewayMonFee = cmdPackage.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGatewaySetupFee = cmdPackage.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmdPackage.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmdPackage.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pPinDebitDiscount = cmdPackage.Parameters.Add("@PinDebitDiscount", SqlDbType.VarChar);
                SqlParameter pCGMonFee = cmdPackage.Parameters.Add("@CGMonFee", SqlDbType.VarChar);
                SqlParameter pCGTransFee = cmdPackage.Parameters.Add("@CGTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonMin = cmdPackage.Parameters.Add("@CGMonMin", SqlDbType.VarChar);
                SqlParameter pCGDiscRate = cmdPackage.Parameters.Add("@CGDiscRate", SqlDbType.VarChar);
                SqlParameter pGCMonFee = cmdPackage.Parameters.Add("@GCMonFee", SqlDbType.VarChar);
                SqlParameter pGCTransFee = cmdPackage.Parameters.Add("@GCTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmdPackage.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmdPackage.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);
                SqlParameter pComplianceFee = cmdPackage.Parameters.Add("@ComplianceFee", SqlDbType.VarChar);
                SqlParameter pBillingMethod = cmdPackage.Parameters.Add("@BillingMethod", SqlDbType.VarChar);

                pPackageID.Value = PackageID;
                pPackagePrefix.Value = PackagePrefix;
                pPackageSuffix.Value = PackageSuffix;
                pCustServFee.Value = CustServFee;
                pInternetStmt.Value = InternetStmt;
                pTransFee.Value = TransactionFee;
                pDiscRateQualPres.Value = DiscRateQualPres;
                pDiscRateQualNP.Value = DiscRateQualNP;
                pDiscRateMidQual.Value = DiscRateMidQual;
                pDiscRateNonQual.Value = DiscRateNonQual;
                pAmexDiscRateQual.Value = AmexDiscRateQual;
                pAmexDiscRateMidQual.Value = AmexDiscRateMidQual;
                pAmexDiscRateNonQual.Value = AmexDiscRateNonQual;
                pDiscRateQualDebit.Value = DiscRateQualDebit;

                pQualNPDebit.Value = QualNPDebit;
                pMidQualDebit.Value = MidQualDebit;
                pNonQualDebit.Value = NonQualDebit;

                pChargebackFee.Value = ChargebackFee;
                pRetrievalFee.Value = RetrievalFee;
                pVoiceAuth.Value = VoiceAuth;
                pBatchHeader.Value = BatchHeader;
                pAVS.Value = AVS;
                pMonMin.Value = MonMin;
                pNBCTransFee.Value = NBCTransFee;
                pAnnualFee.Value = AnnualFee;
                pWirelessAccessFee.Value = WirelessAccessFee;
                pWirelessTransFee.Value = WirelessTransFee;
                pAppFee.Value = AppFee;
                pAppSetupFee.Value = AppSetupFee;
                pGateway.Value = Gateway;
                pGatewayTransFee.Value = GatewayTransFee;
                pGatewayMonFee.Value = GatewayMonFee;
                pGatewaySetupFee.Value = GatewaySetupFee;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pPinDebitDiscount.Value = PinDebitDiscount;
                pCGMonFee.Value = CGMonFee;
                pCGTransFee.Value = CGTransFee;
                pCGMonMin.Value = CGMonMin;
                pCGDiscRate.Value = CGDiscRate;
                pGCMonFee.Value = GCMonFee;
                pGCTransFee.Value = GCTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;
                pComplianceFee.Value = ComplianceFee;
                pBillingMethod.Value = BillingMethod;

                cmdPackage.Connection.Open();
                cmdPackage.ExecuteNonQuery();
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
        }//end class UpdatePackageInfo

        public bool UpdateOtherProcessingPackage(int PackageID, bool bInterchange, bool bAssessments, string RollingReserve)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOtherProcessingPackage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PackageID", PackageID));
                cmd.Parameters.Add(new SqlParameter("@Interchange", bInterchange));
                cmd.Parameters.Add(new SqlParameter("@Assessments", bAssessments));
                cmd.Parameters.Add(new SqlParameter("@RollingReserve", RollingReserve));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
            return true;
        }//end UpdateOtherProcessingPackage

        public DataSet GetPackageList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select * from PackageInfo ORDER BY PackageName";
                //string strQuery = "Select * from PackageInfo where PackageName not like '%Chase%' ORDER BY PackageName";
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
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
        }//end GetPackageList

        public DataSet GetPackageList(string CardPresent)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select * from PackageInfo Where CardPresent = @CardPresent ORDER BY PackageName";

                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@CardPresent", CardPresent));
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
        }//end GetPackageList

        //CALLED BY PackageBL.GetPackagesForRep, GetPackageListForRep.GetPackagesForRep
        public DataSet GetPackageListForRep(string RepNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetRepPackageList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@RepNum", RepNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "PackageInfo");
                
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
        }//end GetPackageListForRep

        public DataSet GetAffiliateIDs (int PID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAffiliateIDsForPackage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@PID", PID);
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
        }//end

        public DataSet GetAppIDs(int PID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAppIDsForPackage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@PID", PID);
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
        }//end


    }//end class ModifyPackageDL
}
