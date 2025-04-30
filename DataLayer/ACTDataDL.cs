using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class ACTDataDL
    {
        private static string ConnStringACT = ConfigurationManager.AppSettings["ConnectionStringACT"].ToString();
        //CALLED BY ACTDataBL.ReturnCustomerFilePath
        //ACTDataBL.GetOtherReferralList
        public DataSet GetACTInfoSQL(string ContactID, string sqlQuery)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdData = new SqlCommand(sqlQuery, Conn);
                cmdData.Connection.Open();
                cmdData.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdData;
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
        }//end GetActInfoSQL

        public DataSet GetACTSaleOppsID(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand Comm = new SqlCommand("SP_GetACTSaleOppID", Conn);
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Connection.Open();
                Comm.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter Adapter = new SqlDataAdapter();
                Adapter.SelectCommand = Comm;
                DataSet dsACTSalesOppID = new DataSet();
                Adapter.Fill(dsACTSalesOppID);
                Conn.Close();
                Conn.Dispose();
                return dsACTSalesOppID;
            }
            catch (Exception err)
            {
                throw err;
            }finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            
        }

        public DataSet GetList(string ListName)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdCommonInfo = new SqlCommand("sp_GetList", Conn);
                cmdCommonInfo.CommandType = CommandType.StoredProcedure;
                cmdCommonInfo.Connection.Open();
                cmdCommonInfo.Parameters.Add(new SqlParameter("@ListName", ListName));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdCommonInfo;
                DataSet ds = new DataSet();
                adapter.Fill(ds, ListName);
                Conn.Close();
                Conn.Dispose();
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
        }//end function GetCommonInfo

        //This function Inserts rep table - CALLED BY RepInfoBL.AddPartnerInfo, RepInfoBL.UpdatePartnerInfoCurrMon
        public bool InsertRepInfoInACT(string RepName, string SageNum, string IPSNum, string IPay3Num, string IMS2Num, string ChaseNum,
            string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("SP_InsertRepDropdowns", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepName = cmdRepInfo.Parameters.Add("@RepName", SqlDbType.VarChar);
                SqlParameter pIPSNum = cmdRepInfo.Parameters.Add("@IPSNum", SqlDbType.VarChar);
                SqlParameter pSageNum = cmdRepInfo.Parameters.Add("@SageNum", SqlDbType.VarChar);
                SqlParameter pIPay3Num = cmdRepInfo.Parameters.Add("@IPay3Num", SqlDbType.VarChar);
                SqlParameter pIMS2Num = cmdRepInfo.Parameters.Add("@IMS2Num", SqlDbType.VarChar);
                SqlParameter pChaseNum = cmdRepInfo.Parameters.Add("@ChaseNum", SqlDbType.VarChar);
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);

                pRepName.Value = RepName;
                pSageNum.Value = SageNum;
                pIPSNum.Value = IPSNum;
                pIPay3Num.Value = IPay3Num;
                pIMS2Num.Value = IMS2Num;
                pChaseNum.Value = ChaseNum;
                pMasterNum.Value = MasterNum;

                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
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
        }//end function InsertRepInfoInACT

       

        //CALLED BY ACTDataBL.GetOtherReferralList, ACTDataBL.GetAuthnetExcel, ACTDataBL.ReturnCustomerFilePath
        //ACTDataBL.GetAuthnetPlatform
        public DataSet GetACTInfoSQL(string sqlQuery)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdData = new SqlCommand(sqlQuery, Conn);
                cmdData.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdData;
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
        }//end GetActInfoSQL

        //CALLED BY ACTDataBL.GetActEditDate
        public DataSet GetACTLastEditDate(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdData = new SqlCommand("SP_GetACTLastEditDate", Conn);
                cmdData.CommandType = CommandType.StoredProcedure;
                cmdData.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdData;
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
        }//end GetACTLastEditDate

        //CALLED BY ExportActBL.GetSummaryDataFromAct
        public DataSet GetSummaryData(string Email)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                string strQuery = "Select * from AppSummary Where Email like '%" + Email + "%' ";
            
                SqlCommand cmdData = new SqlCommand(strQuery, Conn);
                cmdData.Connection.Open();
                cmdData.Parameters.Add(new SqlParameter("@Email", Email));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdData;
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
        }//end ReturnACTRecord
        
        //CALLED BY ExportActBL.UpdateRatesInACT, ExportActBL.UpdateAct, FirstAffiliateLeadsBL.AddAffiliateInfoToACT, 
        public DataSet GetActRecord(string ContactID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetActRecord", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ContactId", ContactID));
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
        }//end GetTBL_CONTACT

        //CALLED BY ExportActBL.UpdateRatesInACT, ExportActBL.UpdateAct, FirstAffiliateLeadsBL.UpdateAffiliateInfoInACT
        public DataSet GetActRecordBackup(string ContactId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetActRecordBackup", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ContactId", ContactId));
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
        }//end GetTBL_CONTACT

        public DataSet GetACTHistStatus(string ContactId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ContactId", ContactId));
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
        }//end GetTBL_CONTACT

        /*public int InsertUpdateOnlineAppACTNotes()
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand()
            }
            catch (SqlException sqlErr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }*/

        //CALLED BY ExportACTBL.UpdateRatesInACT, ExportACTBL.UpdateACT, FirstAffiliateLeadsBL.UpdateAffiliateInfoInACT
        public int InsertHistoryFieldChange(string ContactID, string FieldName, string PrevValue, string NewValue, int partnerID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdInsert = new SqlCommand("SP_InsertHistoryFieldChange", Conn);
                cmdInsert.CommandType = CommandType.StoredProcedure;
                cmdInsert.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                cmdInsert.Parameters.Add(new SqlParameter("@FieldName", FieldName));
                cmdInsert.Parameters.Add(new SqlParameter("@PrevValue", PrevValue));
                cmdInsert.Parameters.Add(new SqlParameter("@NewValue", NewValue));
                cmdInsert.Parameters.Add(new SqlParameter("@partnerID", partnerID));

                cmdInsert.Connection.Open();
                int iRetVal = cmdInsert.ExecuteNonQuery();
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
        }//end 

        //CALLED BY ExportActBL.UpdateRatesInACT, ExportActBL.UpdateAct
        public int InsertActRecordBackup(string ContactID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdInsert = new SqlCommand("SP_InsertActRecordBackup", Conn);
                cmdInsert.CommandType = CommandType.StoredProcedure;
                cmdInsert.Parameters.Add(new SqlParameter("@ContactID", ContactID));

                cmdInsert.Connection.Open();
                int iRetVal = cmdInsert.ExecuteNonQuery();
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
        }//end GetTBL_CONTACT

        //CALLED BY SalesOppsBL.GetACTSalesOpps
        public DataSet GetACTSalesOpps(string RepNum, string Month, string Year, string Status)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetSalesOpps", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pMonth = cmd.Parameters.Add("@Month", SqlDbType.VarChar);
                SqlParameter pYear = cmd.Parameters.Add("@Year", SqlDbType.VarChar);
                SqlParameter pStatus = cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                pRepNum.Value = RepNum;
                pMonth.Value = Month;
                pYear.Value = Year;
                pStatus.Value = Status;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
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
        }//end function GetACTSalesOpps
        
        //Deletes the Backup Tables created when Update in ACT is clicked
        //CALLED BY ExportActBL.UpdateRatesInACT, ExportActBL.UpdateAct, FirstAffiliateLeadsBL.UpdateAffiliateInfoInACT
        public int DeleteActBackup(string ContactID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdDelContact = new SqlCommand("sp_DeleteActRecordBackup", Conn);
                cmdDelContact.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmdDelContact.Parameters.Add("@ContactID", SqlDbType.VarChar);
                pContactID.Value = ContactID;

                cmdDelContact.Connection.Open();
                int iRetVal = cmdDelContact.ExecuteNonQuery();
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
        }//end function DeleteBackup

        //This function checks if appid is present in ACT. CALLED BY ACTDataBL.CheckAppIDExists
        public DataSet CheckAppIDExists(int AppId)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                string strQuery = "Select * FROM TBL_CONTACT WHERE AppID=@AppId AND TYPENUM <> 2";
          
                SqlCommand cmd = new SqlCommand(strQuery, ConnACT);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "TBL_CONTACT");
                return ds;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end check AppId exists


        //This function gets the ContactID from ACT based on the AppID. CALLED BY ExportActBL.UpdateRatesInACT, 
        //ExportActBL.AddInfoToACT, ExportActBL.UpdateAct, SalesOppsBL.InsertSalesOppsInACT, ReminderBL.InsertNoteReminder
        public String ReturnContactID(int AppId)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            string sqlQuery = "Select ContactID from TBL_CONTACT where AppID = @AppID AND TYPENUM <> 2";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, ConnACT);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "TBL_CONTACT");
      
                String ContactID = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drContact = ds.Tables[0].Rows[0];
                    ContactID = drContact["ContactID"].ToString().Trim();
                }
                return ContactID;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end GetNewContactID

        //This function returns contact id from affiliateid
        public String ReturnContactID(string AffiliateID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            string sqlQuery = "Select ContactID from TBL_CONTACT where InstantMSGID = @AffiliateID AND TYPENUM <> 2";
            //string sqlQuery = "Select * from TBL_CONTACT where InstantMSGID = @AffiliateID AND TYPENUM <> 2";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, ConnACT);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "TBL_CONTACT");
                String ContactID = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drContact = ds.Tables[0].Rows[0];
                    ContactID = drContact["ContactID"].ToString().Trim();
                }
                return ContactID;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end GetNewContactID

        public DataSet GetACTEditDate(string ContactID, DateTime OnlineAppEditDate)
        {
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetACTEditDate",conn);

                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@CONTACTID", ContactID));
                cmd.Parameters.Add(new SqlParameter("@OnlineAppEditDate", OnlineAppEditDate));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;
            }
            catch(SqlException err)
            {
                throw err;
            }
            finally 
            {
                conn.Close();
                conn.Dispose();
            }

            
        }

       //CALLED BY ExportActBL.AddInfoToACT
        public bool AddDataContact(string CREATEDATE, string P1fullname, string LegalStatus, string FirstName,
            string LastName, string P1FirstName, 
            string P1LastName, string P1MidName, string P1Title, string JobTitle, string CompanyName, string DBA, 
            string COMPANYWEBADDRESS, string ReferredBy, string AffiliateReferral, string SalesRep,
            string Gateway, string Processor, string YIB, string MIB, string YABL, string MABL, string BusHours, 
            string NumOfLocs, string NumOfDaysProdDel, string ProdServSold, string AddlComments, 
            string Bankruptcy, string MAAddr1, string MAAddr2, string MACity, string MAState, 
            string MAZip, string MACountry, string FedTaxID, string Platform, string AnnualFee, 
            string P1SocialSecurity, string P1OwnPct, string P1LivingStatus, string P1LOR, 
            string P1DLNum, string P1DLState, string P1DLExpDate, string P1DOB, string RefundPolicy, 
            string BankName, string BankCity, string BankState, string BankZip, string RoutingNum, 
            string CheckingAcctNum, string CustServFee, string MonMin, string InternetStmt, string DiscQNP, string DiscMQ, 
            string DiscNQ, string AmexDiscRateQual, string AmexDiscRateMidQual, string AmexDiscRateNonQual, string DiscQD,

            string DebitQualNP, string DebitMidQual, string DebitNonQual, 

            string DiscQP, string TransFee, string RetrievalFee, 
            string VoiceAuth, string BatchHeader, string AVS, string NBCTFee, string CBFee, 
            int AcctType, string MonVol, string AvgTicket, string JCBNum, 
            string AmexNum, string DiscoverNum, string PctSwp, string PctKWI, 
            string PctKWOI, string PctRet, string PCTRest, string PCTServ, string PctMail, 
            string PctInt, string PctOth, string GWMonFee, string GWTransFee, 
            string GWSetupFee, string ProcBCBefore, string CTMFMatch, string MerchantStatus,
            string GatewayStatus, string MerchantID, string MerchantNum, string RepNum, string MCCCategoryCode,
            string PayrollType, bool Payroll, string PayrollStatus, string MCAType, string ContractTerm, int AppId)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertActTBLContact", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add(new SqlParameter("@AppID", SqlDbType.Int));
                SqlParameter pCREATEDATE = cmd.Parameters.Add("@CREATEDATE", SqlDbType.VarChar);
                SqlParameter pP1fullname = cmd.Parameters.Add("@FULLNAME", SqlDbType.VarChar);
                SqlParameter pLegalStatus = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar);
                SqlParameter pP1FirstName = cmd.Parameters.Add("@P1FIRSTNAME", SqlDbType.VarChar);
                SqlParameter pP1MidName = cmd.Parameters.Add("@P1MIDNAME", SqlDbType.VarChar);
                SqlParameter pP1LastName = cmd.Parameters.Add("@P1LASTNAME", SqlDbType.VarChar);
                SqlParameter pP1Title = cmd.Parameters.Add("@P1Title", SqlDbType.VarChar);

                SqlParameter pFirstName = cmd.Parameters.Add("@FirstNAME", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LASTNAME", SqlDbType.VarChar);

                SqlParameter pJobTitle = cmd.Parameters.Add("@JOBTITLE", SqlDbType.VarChar);
                SqlParameter pCompanyName = cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pCOMPANYWEBADDRESS = cmd.Parameters.Add("@COMPANYWEBADDRESS", SqlDbType.VarChar);
                SqlParameter pReferredBy = cmd.Parameters.Add("@ReferredBy", SqlDbType.VarChar);
                SqlParameter pAffiliateReferral = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pSalesRep = cmd.Parameters.Add("@SalesRep", SqlDbType.VarChar);
                SqlParameter pGateway = cmd.Parameters.Add("@Gateway", SqlDbType.VarChar);
                SqlParameter pProcessor = cmd.Parameters.Add("@Processor", SqlDbType.VarChar);
                SqlParameter pYIB = cmd.Parameters.Add("@YIB", SqlDbType.VarChar);
                SqlParameter pMIB = cmd.Parameters.Add("@MIB", SqlDbType.VarChar);
                SqlParameter pYABL = cmd.Parameters.Add("@YABL", SqlDbType.VarChar);
                SqlParameter pMABL = cmd.Parameters.Add("@MABL", SqlDbType.VarChar);
                SqlParameter pBusHours = cmd.Parameters.Add("@BusHours", SqlDbType.VarChar);
                SqlParameter pNumOfLocs = cmd.Parameters.Add("@NumOfLocs", SqlDbType.VarChar);
                SqlParameter pNumOfDaysProdDel = cmd.Parameters.Add("@NumOfDaysProdDel", SqlDbType.VarChar);
                SqlParameter pProdServSold = cmd.Parameters.Add("@ProdServSold", SqlDbType.VarChar);
                SqlParameter pAddlComments = cmd.Parameters.Add("@AddlComments", SqlDbType.VarChar);
                SqlParameter pBankruptcy = cmd.Parameters.Add("@Bankruptcy", SqlDbType.VarChar);
                SqlParameter pMAAddr1 = cmd.Parameters.Add("@MAAddr1", SqlDbType.VarChar);
                SqlParameter pMAAddr2 = cmd.Parameters.Add("@MAAddr2", SqlDbType.VarChar);
                SqlParameter pMACity = cmd.Parameters.Add("@MACity", SqlDbType.VarChar);
                SqlParameter pMAState = cmd.Parameters.Add("@MAState", SqlDbType.VarChar);
                SqlParameter pMAZip = cmd.Parameters.Add("@MAZip", SqlDbType.VarChar);
                SqlParameter pMACountry = cmd.Parameters.Add("@MACountry", SqlDbType.VarChar);
                SqlParameter pFedTaxID = cmd.Parameters.Add("@FedTaxID", SqlDbType.VarChar);
                SqlParameter pPlatform = cmd.Parameters.Add("@Platform", SqlDbType.VarChar);
                SqlParameter pAnnualFee = cmd.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pP1SocialSecurity = cmd.Parameters.Add("@P1SocialSecurity", SqlDbType.VarChar);
                SqlParameter pP1OwnPct = cmd.Parameters.Add("@P1OwnPct", SqlDbType.VarChar);
                SqlParameter pP1LivingStatus = cmd.Parameters.Add("@P1LivingStatus", SqlDbType.VarChar);
                SqlParameter pP1LOR = cmd.Parameters.Add("@P1LOR", SqlDbType.VarChar);
                SqlParameter pP1DLNum = cmd.Parameters.Add("@P1DLNum", SqlDbType.VarChar);
                SqlParameter pP1DLState = cmd.Parameters.Add("@P1DLState", SqlDbType.VarChar);
                SqlParameter pP1DLExpDate = cmd.Parameters.Add("@P1DLExpDate", SqlDbType.VarChar);
                SqlParameter pP1DOB = cmd.Parameters.Add("@P1DOB", SqlDbType.VarChar);
                SqlParameter pRefundPolicy = cmd.Parameters.Add("@RefundPolicy", SqlDbType.VarChar);
                SqlParameter pBankName = cmd.Parameters.Add("@BankName", SqlDbType.VarChar);
                SqlParameter pBankCity = cmd.Parameters.Add("@BankCity", SqlDbType.VarChar);
                SqlParameter pBankState = cmd.Parameters.Add("@BankState", SqlDbType.VarChar);
                SqlParameter pBankZip = cmd.Parameters.Add("@BankZip", SqlDbType.VarChar);
                SqlParameter pRoutingNum = cmd.Parameters.Add("@RoutingNum", SqlDbType.VarChar);
                SqlParameter pCheckingAcctNum = cmd.Parameters.Add("@CheckingAcctNum", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmd.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pMonMin = cmd.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pInternetStmt = cmd.Parameters.Add("@InternetStmt", SqlDbType.VarChar);
                SqlParameter pDiscQNP = cmd.Parameters.Add("@DiscQNP", SqlDbType.VarChar);
                SqlParameter pDiscMQ = cmd.Parameters.Add("@DiscMQ", SqlDbType.VarChar);
                SqlParameter pDiscNQ = cmd.Parameters.Add("@DiscNQ", SqlDbType.VarChar);
                SqlParameter pDiscQD = cmd.Parameters.Add("@DiscQD", SqlDbType.VarChar);

                SqlParameter pAmexDiscRateQual = cmd.Parameters.Add("@AmexDiscRateQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateMidQual = cmd.Parameters.Add("@AmexDiscRateMidQual", SqlDbType.VarChar);
                SqlParameter pAmexDiscRateNonQual = cmd.Parameters.Add("@AmexDiscRateNonQual", SqlDbType.VarChar);

                SqlParameter pDebitQualNP = cmd.Parameters.Add("@DebitQualNP", SqlDbType.VarChar);
                SqlParameter pDebitMidQual = cmd.Parameters.Add("@DebitMidQual", SqlDbType.VarChar);
                SqlParameter pDebitNonQual = cmd.Parameters.Add("@DebitNonQual", SqlDbType.VarChar);

                SqlParameter pDiscQP = cmd.Parameters.Add("@DiscQP", SqlDbType.VarChar);
                SqlParameter pTransFee = cmd.Parameters.Add("@TransFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFee = cmd.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pVoiceAuth = cmd.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeader = cmd.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVS = cmd.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pNBCTFee = cmd.Parameters.Add("@NBCTFee", SqlDbType.VarChar);
                SqlParameter pCBFee = cmd.Parameters.Add("@CBFee", SqlDbType.VarChar);
                SqlParameter pAcctType = cmd.Parameters.Add("@AcctType", SqlDbType.SmallInt);
                SqlParameter pMonVol = cmd.Parameters.Add("@MonVol", SqlDbType.VarChar);
                SqlParameter pAvgTicket = cmd.Parameters.Add("@AvgTicket", SqlDbType.VarChar);
                SqlParameter pJCBNum = cmd.Parameters.Add("@JCBNum", SqlDbType.VarChar);
                SqlParameter pAmexNum = cmd.Parameters.Add("@AmexNum", SqlDbType.VarChar);
                SqlParameter pDiscoverNum = cmd.Parameters.Add("@DiscoverNum", SqlDbType.VarChar);
                SqlParameter pPctSwp = cmd.Parameters.Add("@PctSwp", SqlDbType.VarChar);
                SqlParameter pPctKWI = cmd.Parameters.Add("@PctKWI", SqlDbType.VarChar);
                SqlParameter pPctKWOI = cmd.Parameters.Add("@PctKWOI", SqlDbType.VarChar);
                SqlParameter pPctRet = cmd.Parameters.Add("@PctRet", SqlDbType.VarChar);
                SqlParameter pPCTRest = cmd.Parameters.Add("@PCTRest", SqlDbType.VarChar);
                SqlParameter pPCTServ = cmd.Parameters.Add("@PCTServ", SqlDbType.VarChar);
                SqlParameter pPctMail = cmd.Parameters.Add("@PctMail", SqlDbType.VarChar);
                SqlParameter pPctInt = cmd.Parameters.Add("@PctInt", SqlDbType.VarChar);
                SqlParameter pPctOth = cmd.Parameters.Add("@PctOth", SqlDbType.VarChar);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pGWMonFee = cmd.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGWTransFee = cmd.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGWSetupFee = cmd.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);
                SqlParameter pProcBCBefore = cmd.Parameters.Add("@ProcBCBefore", SqlDbType.VarChar);
                SqlParameter pCTMFMatch = cmd.Parameters.Add("@CTMFMatch", SqlDbType.VarChar);
                SqlParameter pMerchantStatus = cmd.Parameters.Add("@MerchantStatus", SqlDbType.VarChar);
                SqlParameter pGatewayStatus = cmd.Parameters.Add("@GatewayStatus", SqlDbType.VarChar);
                SqlParameter pMerchantID = cmd.Parameters.Add("@MerchantID", SqlDbType.VarChar);
                SqlParameter pMerchantNum = cmd.Parameters.Add("@MerchantNum", SqlDbType.VarChar);
                SqlParameter pMCCCategoryCode = cmd.Parameters.Add("@MCCCategoryCode", SqlDbType.VarChar);
                SqlParameter pPayrollType = cmd.Parameters.Add("@PayrollType", SqlDbType.VarChar);

                SqlParameter pPayrollStatus = cmd.Parameters.Add("@PayrollStatus", SqlDbType.VarChar);

                SqlParameter pPayroll = cmd.Parameters.Add("@Payroll", SqlDbType.Bit);
                SqlParameter pMCAType = cmd.Parameters.Add("@MCAType", SqlDbType.VarChar);

                SqlParameter pContractTerm = cmd.Parameters.Add("@ContractTerm", SqlDbType.VarChar);

                pAppId.Value = AppId;
                pCREATEDATE.Value = CREATEDATE;
                pP1fullname.Value = P1fullname;
                pLegalStatus.Value = LegalStatus;
                pP1FirstName.Value = P1FirstName;
                pP1MidName.Value = P1MidName;
                pP1LastName.Value = P1LastName;

                pP1Title.Value = P1Title;

                pFirstName.Value = FirstName;
                pLastName.Value = LastName;

                pJobTitle.Value = JobTitle;
                pCompanyName.Value = CompanyName;
                pDBA.Value = DBA;
                pCOMPANYWEBADDRESS.Value = COMPANYWEBADDRESS;
                pReferredBy.Value = ReferredBy;
                pAffiliateReferral.Value = AffiliateReferral;
                pSalesRep.Value = SalesRep;
                pGateway.Value = Gateway;
                pProcessor.Value = Processor;
                pYIB.Value = YIB;
                pMIB.Value = MIB;
                pYABL.Value = YABL;
                pMABL.Value = MABL;
                pBusHours.Value = BusHours;
                pNumOfLocs.Value = NumOfLocs;
                pNumOfDaysProdDel.Value = NumOfDaysProdDel;
                pProdServSold.Value = ProdServSold;
                pAddlComments.Value = AddlComments;
                pBankruptcy.Value = Bankruptcy;
                pMAAddr1.Value = MAAddr1;
                pMAAddr2.Value = MAAddr2;
                pMACity.Value = MACity;
                pMAState.Value = MAState;
                pMAZip.Value = MAZip;
                pMACountry.Value = MACountry;
                pFedTaxID.Value = FedTaxID;
                pPlatform.Value = Platform;
                pAnnualFee.Value = AnnualFee;
                pP1SocialSecurity.Value = P1SocialSecurity;
                pP1OwnPct.Value = P1OwnPct;
                pP1LivingStatus.Value = P1LivingStatus;
                pP1LOR.Value = P1LOR;
                pP1DLNum.Value = P1DLNum;
                pP1DLState.Value = P1DLState;
                pP1DLExpDate.Value = P1DLExpDate;
                pP1DOB.Value = P1DOB;
                pRefundPolicy.Value = RefundPolicy;
                pBankName.Value = BankName;
                pBankCity.Value = BankCity;
                pBankState.Value = BankState;
                pBankZip.Value = BankZip;
                pRoutingNum.Value = RoutingNum;
                pCheckingAcctNum.Value = CheckingAcctNum;
                pCustServFee.Value = CustServFee;
                pMonMin.Value = MonMin;
                pInternetStmt.Value = InternetStmt;
                pDiscQNP.Value = DiscQNP;
                pDiscMQ.Value = DiscMQ;
                pDiscNQ.Value = DiscNQ;

                pAmexDiscRateQual.Value = AmexDiscRateQual;
                pAmexDiscRateMidQual.Value = AmexDiscRateMidQual;
                pAmexDiscRateNonQual.Value = AmexDiscRateNonQual;

                pDebitQualNP.Value = DebitQualNP;
                pDebitMidQual.Value = DebitMidQual;
                pDebitNonQual.Value = DebitNonQual;

                pDiscQD.Value = DiscQD;
                pDiscQP.Value = DiscQP;
                pTransFee.Value = TransFee;
                pRetrievalFee.Value = RetrievalFee;
                pVoiceAuth.Value = VoiceAuth;
                pBatchHeader.Value = BatchHeader;
                pAVS.Value = AVS;
                pNBCTFee.Value = NBCTFee;
                pCBFee.Value = CBFee;
                pAcctType.Value = AcctType;
                pMonVol.Value = MonVol;
                pAvgTicket.Value = AvgTicket;
                pJCBNum.Value = JCBNum;
                pAmexNum.Value = AmexNum;
                pDiscoverNum.Value = DiscoverNum;
                pPctSwp.Value = PctSwp;
                pPctKWI.Value = PctKWI;
                pPctKWOI.Value = PctKWOI;
                pPctRet.Value = PctRet;
                pPCTRest.Value = PCTRest;
                pPCTServ.Value = PCTServ;
                pPctMail.Value = PctMail;
                pPctInt.Value = PctInt;
                pPctOth.Value = PctOth;
                pRepNum.Value = RepNum;
                pGWMonFee.Value = GWMonFee;
                pGWTransFee.Value = GWTransFee;
                pGWSetupFee.Value = GWSetupFee;
                pProcBCBefore.Value = ProcBCBefore;
                pCTMFMatch.Value = CTMFMatch;
                pMerchantStatus.Value = MerchantStatus;
                pGatewayStatus.Value = GatewayStatus;
                pMerchantID.Value = MerchantID;
                pMerchantNum.Value = MerchantNum;
                pMCCCategoryCode.Value = MCCCategoryCode;
                pPayrollType.Value = PayrollType;

                pPayrollStatus.Value = PayrollStatus;
                pPayroll.Value = Payroll;
                pMCAType.Value = MCAType;

                pContractTerm.Value = ContractTerm;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function add info

        public bool UpdateDiscountPaid(string DiscountPaid, int AppId) 
        {
            string CONTACTID = ReturnContactID(AppId);
            Guid CONTACTIDGuid = new Guid(CONTACTID);
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateDiscountPaid", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pDiscountPaid = cmd.Parameters.Add("@DiscountPaid",SqlDbType.VarChar);
                SqlParameter pCONTACTID= cmd.Parameters.Add("@CONTACTID", SqlDbType.UniqueIdentifier);
                pDiscountPaid.Value = DiscountPaid;
                pCONTACTID.Value = CONTACTIDGuid;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();

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
            return true;
        }

        public bool UpdateMCAAmount(int MCAAmount, int AppId)
        {
            string CONTACTID = ReturnContactID(AppId);
            Guid CONTACTIDGuid = new Guid(CONTACTID);
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateMCAAmount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMCAAmount = cmd.Parameters.Add("@MCAAmount", SqlDbType.Int);
                SqlParameter pAppID = cmd.Parameters.Add("@AppID", SqlDbType.SmallInt);
                pMCAAmount.Value = MCAAmount;
                pAppID.Value = AppId;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();

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
            return true;
        }

        public bool UpdatePinDebitDiscount(string PinDebitDisount, int AppId)
        {
            string CONTACTID = ReturnContactID(AppId);
            Guid CONTACTIDGuid = new Guid(CONTACTID);
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdatePinDebitDisount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pPinDebitDisount = cmd.Parameters.Add("@PinDebitDisount", SqlDbType.VarChar);
                SqlParameter pCONTACTID = cmd.Parameters.Add("@CONTACTID", SqlDbType.UniqueIdentifier);
                pPinDebitDisount.Value = PinDebitDisount;
                pCONTACTID.Value = CONTACTIDGuid;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();

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
            return true;
        }

        public bool UpdateComplianceFee(string strComplianceFee, int AppId)
        {
            string CONTACTID = ReturnContactID(AppId);
            Guid CONTACTIDGuid = new Guid(CONTACTID);
            SqlConnection conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateComplianceFee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pDiscountPaid = cmd.Parameters.Add("@strComplianceFee", SqlDbType.VarChar);
                SqlParameter pCONTACTID = cmd.Parameters.Add("@CONTACTID", SqlDbType.UniqueIdentifier);
                pDiscountPaid.Value = strComplianceFee;
                pCONTACTID.Value = CONTACTIDGuid;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();

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
            return true;
        }

        //CALLED BY ExportActBL.UpdateAct
        public int UpdateActContact(string CREATEDATE, string P1fullname, string LegalStatus, string P1FirstName, 
            string P1LastName, string P1MidName, string JobTitle, string CompanyName, string DBA, 
            string COMPANYWEBADDRESS, string ReferredBy, string AffiliateReferral, string SalesRep,
            string YIB, string MIB, string YABL, string MABL, string BusHours, string NumOfLocs, string NumOfDaysProdDel, 
            string ProdServSold, string AddlComments, string Bankruptcy, string MAAddr1, string MAAddr2, 
            string MACity, string MAState, string MAZip, string MACountry, string FedTaxID, string Platform, 
            string P1SocialSecurity, string P1OwnPct, string P1LivingStatus, string P1LOR, string P1DLNum, 
            string P1DLState, string P1DLExpDate, string P1DOB, string RefundPolicy, string BankName, 
            string BankCity, string BankState, string BankZip, string RoutingNum, string CheckingAcctNum, 
            int AcctType, string MonVol, string AvgTicket, string JCBNum, string AmexNum, 
            string DiscoverNum, string PctSwp, string PctKWI, string PctKWOI, 
            string PctRet, string PCTRest, string PCTServ, string PctMail, string PctInt, string PctOth, 
            string ProcBCBefore, string CTMFMatch, string MerchantStatus, string GatewayStatus, 
            string MerchantID, string MerchantNum, string RepNum, string MCCategoryCode, int AppId)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateActContact", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add(new SqlParameter("@AppID", SqlDbType.Int));
                SqlParameter pCREATEDATE = cmd.Parameters.Add("@CREATEDATE", SqlDbType.VarChar);
                SqlParameter pP1fullname = cmd.Parameters.Add("@FULLNAME", SqlDbType.VarChar);
                SqlParameter pLegalStatus = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar);
                SqlParameter pP1FirstName = cmd.Parameters.Add("@P1FIRSTNAME", SqlDbType.VarChar);
                SqlParameter pP1MidName = cmd.Parameters.Add("@P1MIDNAME", SqlDbType.VarChar);
                SqlParameter pP1LastName = cmd.Parameters.Add("@P1LASTNAME", SqlDbType.VarChar);
                SqlParameter pJobTitle = cmd.Parameters.Add("@JOBTITLE", SqlDbType.VarChar);
                SqlParameter pCompanyName = cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pCOMPANYWEBADDRESS = cmd.Parameters.Add("@COMPANYWEBADDRESS", SqlDbType.VarChar);
                SqlParameter pReferredBy = cmd.Parameters.Add("@ReferredBy", SqlDbType.VarChar);
                SqlParameter pAffiliateReferral = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pSalesRep = cmd.Parameters.Add("@SalesRep", SqlDbType.VarChar);
                SqlParameter pYIB = cmd.Parameters.Add("@YIB", SqlDbType.VarChar);
                SqlParameter pMIB = cmd.Parameters.Add("@MIB", SqlDbType.VarChar);
                SqlParameter pYABL = cmd.Parameters.Add("@YABL", SqlDbType.VarChar);
                SqlParameter pMABL = cmd.Parameters.Add("@MABL", SqlDbType.VarChar);
                SqlParameter pBusHours = cmd.Parameters.Add("@BusHours", SqlDbType.VarChar);
                SqlParameter pNumOfLocs = cmd.Parameters.Add("@NumOfLocs", SqlDbType.VarChar);
                SqlParameter pNumOfDaysProdDel = cmd.Parameters.Add("@NumOfDaysProdDel", SqlDbType.VarChar);
                SqlParameter pProdServSold = cmd.Parameters.Add("@ProdServSold", SqlDbType.VarChar);
                SqlParameter pAddlComments = cmd.Parameters.Add("@AddlComments", SqlDbType.VarChar);
                SqlParameter pBankruptcy = cmd.Parameters.Add("@Bankruptcy", SqlDbType.VarChar);
                SqlParameter pMAAddr1 = cmd.Parameters.Add("@MAAddr1", SqlDbType.VarChar);
                SqlParameter pMAAddr2 = cmd.Parameters.Add("@MAAddr2", SqlDbType.VarChar);
                SqlParameter pMACity = cmd.Parameters.Add("@MACity", SqlDbType.VarChar);
                SqlParameter pMAState = cmd.Parameters.Add("@MAState", SqlDbType.VarChar);
                SqlParameter pMAZip = cmd.Parameters.Add("@MAZip", SqlDbType.VarChar);
                SqlParameter pMACountry = cmd.Parameters.Add("@MACountry", SqlDbType.VarChar);
                SqlParameter pFedTaxID = cmd.Parameters.Add("@FedTaxID", SqlDbType.VarChar);
                SqlParameter pPlatform = cmd.Parameters.Add("@Platform", SqlDbType.VarChar);
                SqlParameter pP1SocialSecurity = cmd.Parameters.Add("@P1SocialSecurity", SqlDbType.VarChar);
                SqlParameter pP1OwnPct = cmd.Parameters.Add("@P1OwnPct", SqlDbType.VarChar);
                SqlParameter pP1LivingStatus = cmd.Parameters.Add("@P1LivingStatus", SqlDbType.VarChar);
                SqlParameter pP1LOR = cmd.Parameters.Add("@P1LOR", SqlDbType.VarChar);
                SqlParameter pP1DLNum = cmd.Parameters.Add("@P1DLNum", SqlDbType.VarChar);
                SqlParameter pP1DLState = cmd.Parameters.Add("@P1DLState", SqlDbType.VarChar);
                SqlParameter pP1DLExpDate = cmd.Parameters.Add("@P1DLExpDate", SqlDbType.VarChar);
                SqlParameter pP1DOB = cmd.Parameters.Add("@P1DOB", SqlDbType.VarChar);
                SqlParameter pRefundPolicy = cmd.Parameters.Add("@RefundPolicy", SqlDbType.VarChar);
                SqlParameter pBankName = cmd.Parameters.Add("@BankName", SqlDbType.VarChar);
                SqlParameter pBankCity = cmd.Parameters.Add("@BankCity", SqlDbType.VarChar);
                SqlParameter pBankState = cmd.Parameters.Add("@BankState", SqlDbType.VarChar);
                SqlParameter pBankZip = cmd.Parameters.Add("@BankZip", SqlDbType.VarChar);
                SqlParameter pRoutingNum = cmd.Parameters.Add("@RoutingNum", SqlDbType.VarChar);
                SqlParameter pCheckingAcctNum = cmd.Parameters.Add("@CheckingAcctNum", SqlDbType.VarChar);
                SqlParameter pAcctType = cmd.Parameters.Add("@AcctType", SqlDbType.SmallInt);
                SqlParameter pMonVol = cmd.Parameters.Add("@MonVol", SqlDbType.VarChar);
                SqlParameter pAvgTicket = cmd.Parameters.Add("@AvgTicket", SqlDbType.VarChar);
                SqlParameter pJCBNum = cmd.Parameters.Add("@JCBNum", SqlDbType.VarChar);
                SqlParameter pAmexNum = cmd.Parameters.Add("@AmexNum", SqlDbType.VarChar);
                SqlParameter pDiscoverNum = cmd.Parameters.Add("@DiscoverNum", SqlDbType.VarChar);
                SqlParameter pPctSwp = cmd.Parameters.Add("@PctSwp", SqlDbType.VarChar);
                SqlParameter pPctKWI = cmd.Parameters.Add("@PctKWI", SqlDbType.VarChar);
                SqlParameter pPctKWOI = cmd.Parameters.Add("@PctKWOI", SqlDbType.VarChar);
                SqlParameter pPctRet = cmd.Parameters.Add("@PctRet", SqlDbType.VarChar);
                SqlParameter pPCTRest = cmd.Parameters.Add("@PCTRest", SqlDbType.VarChar);
                SqlParameter pPCTServ = cmd.Parameters.Add("@PCTServ", SqlDbType.VarChar);
                SqlParameter pPctMail = cmd.Parameters.Add("@PctMail", SqlDbType.VarChar);
                SqlParameter pPctInt = cmd.Parameters.Add("@PctInt", SqlDbType.VarChar);
                SqlParameter pPctOth = cmd.Parameters.Add("@PctOth", SqlDbType.VarChar);     
                SqlParameter pProcBCBefore = cmd.Parameters.Add("@ProcBCBefore", SqlDbType.VarChar);
                SqlParameter pCTMFMatch = cmd.Parameters.Add("@CTMFMatch", SqlDbType.VarChar);
                SqlParameter pMerchantStatus = cmd.Parameters.Add("@MerchantStatus", SqlDbType.VarChar);
                SqlParameter pGatewayStatus = cmd.Parameters.Add("@GatewayStatus", SqlDbType.VarChar);
                SqlParameter pMerchantID = cmd.Parameters.Add("@MerchantID", SqlDbType.VarChar);
                SqlParameter pMerchantNum = cmd.Parameters.Add("@MerchantNum", SqlDbType.VarChar);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pMCCategoryCode = cmd.Parameters.Add("@MCCategoryCode", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.SmallInt);
                
                pRetVal.Direction = ParameterDirection.ReturnValue;

                pAppId.Value = AppId;
                pCREATEDATE.Value = CREATEDATE;
                pP1fullname.Value = P1fullname;
                pLegalStatus.Value = LegalStatus;
                pP1FirstName.Value = P1FirstName;
                pP1MidName.Value = P1MidName;
                pP1LastName.Value = P1LastName;
                pJobTitle.Value = JobTitle;
                pCompanyName.Value = CompanyName;
                pDBA.Value = DBA;
                pCOMPANYWEBADDRESS.Value = COMPANYWEBADDRESS;
                pReferredBy.Value = ReferredBy;
                pAffiliateReferral.Value = AffiliateReferral;
                pSalesRep.Value = SalesRep;
                pYIB.Value = YIB;
                pMIB.Value = MIB;
                pYABL.Value = YABL;
                pMABL.Value = MABL;
                pBusHours.Value = BusHours;
                pNumOfLocs.Value = NumOfLocs;
                pNumOfDaysProdDel.Value = NumOfDaysProdDel;
                pProdServSold.Value = ProdServSold;
                pAddlComments.Value = AddlComments;
                pBankruptcy.Value = Bankruptcy;
                pMAAddr1.Value = MAAddr1;
                pMAAddr2.Value = MAAddr2;
                pMACity.Value = MACity;
                pMAState.Value = MAState;
                pMAZip.Value = MAZip;
                pMACountry.Value = MACountry;
                pFedTaxID.Value = FedTaxID;
                pPlatform.Value = Platform;
                pP1SocialSecurity.Value = P1SocialSecurity;
                pP1OwnPct.Value = P1OwnPct;
                pP1LivingStatus.Value = P1LivingStatus;
                pP1LOR.Value = P1LOR;
                pP1DLNum.Value = P1DLNum;
                pP1DLState.Value = P1DLState;
                pP1DLExpDate.Value = P1DLExpDate;
                pP1DOB.Value = P1DOB;
                pRefundPolicy.Value = RefundPolicy;
                pBankName.Value = BankName;
                pBankCity.Value = BankCity;
                pBankState.Value = BankState;
                pBankZip.Value = BankZip;
                pRoutingNum.Value = RoutingNum;
                pCheckingAcctNum.Value = CheckingAcctNum;
                pAcctType.Value = AcctType;
                pMonVol.Value = MonVol;
                pAvgTicket.Value = AvgTicket;
                pJCBNum.Value = JCBNum;
                pAmexNum.Value = AmexNum;
                pDiscoverNum.Value = DiscoverNum;
                pPctSwp.Value = PctSwp;
                pPctKWI.Value = PctKWI;
                pPctKWOI.Value = PctKWOI;
                pPctRet.Value = PctRet;
                pPCTRest.Value = PCTRest;
                pPCTServ.Value = PCTServ;
                pPctMail.Value = PctMail;
                pPctInt.Value = PctInt;
                pPctOth.Value = PctOth;
                pRepNum.Value = RepNum;
                pProcBCBefore.Value = ProcBCBefore;
                pCTMFMatch.Value = CTMFMatch;
                pMerchantStatus.Value = MerchantStatus;
                pGatewayStatus.Value = GatewayStatus;
                pMerchantID.Value = MerchantID;
                pMerchantNum.Value = MerchantNum;
                pMCCategoryCode.Value = MCCategoryCode;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                int iRetVal = Convert.ToInt32(pRetVal.Value);
                ConnACT.Close();
                ConnACT.Dispose();
                return iRetVal;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end UpdateActContact

        //CALLED BY ExportActBL.UpdateRatesInACT
        public bool UpdateRatesInACT(string Gateway, string Processor, string RepNum, string AnnualFee,
            string CustServFee, string MonMin, string InternetStmt, string DiscQNP, string DiscMQ, string DiscNQ, string DiscQD, 
            string DiscQP, string TransFee, string RetrievalFee, string RollingReserve, string VoiceAuth, 
            string BatchHeader, string AVS, string NBCTFee, string CBFee, int AcctType,
            string GWMonFee, string GWTransFee, string GWSetupFee, 
            bool OnlineDebit, bool GiftCard, bool CheckService, bool EBT, string DebitMonFee, 
            string DebitTransFee, string CGMonFee, string CGTransFee, string CGMonMin, 
            string CGDiscRate, string GCMonFee, string GCTransFee, string EBTMonFee, 
            string EBTTransFee, string CheckServiceName, string WirelessAccess, string WirelessTransFee, 
            bool Interchange, bool Assessments, int AppId)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateACTRates", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add(new SqlParameter("@AppID", SqlDbType.Int));
                SqlParameter pGateway = cmd.Parameters.Add("@Gateway", SqlDbType.VarChar);
                SqlParameter pProcessor = cmd.Parameters.Add("@Processor", SqlDbType.VarChar);
                SqlParameter pAnnualFee = cmd.Parameters.Add("@AnnualFee", SqlDbType.VarChar);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pCustServFee = cmd.Parameters.Add("@CustServFee", SqlDbType.VarChar);
                SqlParameter pMonMin = cmd.Parameters.Add("@MonMin", SqlDbType.VarChar);
                SqlParameter pInternetStmt = cmd.Parameters.Add("@InternetStmt", SqlDbType.VarChar);
                SqlParameter pDiscQNP = cmd.Parameters.Add("@DiscQNP", SqlDbType.VarChar);
                SqlParameter pDiscMQ = cmd.Parameters.Add("@DiscMQ", SqlDbType.VarChar);
                SqlParameter pDiscNQ = cmd.Parameters.Add("@DiscNQ", SqlDbType.VarChar);
                SqlParameter pDiscQD = cmd.Parameters.Add("@DiscQD", SqlDbType.VarChar);
                SqlParameter pDiscQP = cmd.Parameters.Add("@DiscQP", SqlDbType.VarChar);
                SqlParameter pTransFee = cmd.Parameters.Add("@TransFee", SqlDbType.VarChar);
                SqlParameter pRetrievalFee = cmd.Parameters.Add("@RetrievalFee", SqlDbType.VarChar);
                SqlParameter pRollingReserve = cmd.Parameters.Add("@RollingReserve", SqlDbType.VarChar);
                SqlParameter pVoiceAuth = cmd.Parameters.Add("@VoiceAuth", SqlDbType.VarChar);
                SqlParameter pBatchHeader = cmd.Parameters.Add("@BatchHeader", SqlDbType.VarChar);
                SqlParameter pAVS = cmd.Parameters.Add("@AVS", SqlDbType.VarChar);
                SqlParameter pNBCTFee = cmd.Parameters.Add("@NBCTFee", SqlDbType.VarChar);
                SqlParameter pCBFee = cmd.Parameters.Add("@CBFee", SqlDbType.VarChar);
                SqlParameter pAcctType = cmd.Parameters.Add("@AcctType", SqlDbType.SmallInt);
                SqlParameter pGWMonFee = cmd.Parameters.Add("@GWMonFee", SqlDbType.VarChar);
                SqlParameter pGWTransFee = cmd.Parameters.Add("@GWTransFee", SqlDbType.VarChar);
                SqlParameter pGWSetupFee = cmd.Parameters.Add("@GWSetupFee", SqlDbType.VarChar);

                //Additional Services Info
                SqlParameter pCGMonFee = cmd.Parameters.Add("@CGMonFee", SqlDbType.VarChar);
                SqlParameter pCGTransFee = cmd.Parameters.Add("@CGTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonMin = cmd.Parameters.Add("@CGMonMin", SqlDbType.VarChar);
                SqlParameter pCGDiscRate = cmd.Parameters.Add("@CGDiscRate", SqlDbType.VarChar);
                SqlParameter pDebitMonFee = cmd.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmd.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pGCMonFee = cmd.Parameters.Add("@GCMonFee", SqlDbType.VarChar);
                SqlParameter pGCTransFee = cmd.Parameters.Add("@GCTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmd.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmd.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);
                SqlParameter pOnlineDebit = cmd.Parameters.Add("@OnlineDebit", SqlDbType.Bit);
                SqlParameter pGiftCard = cmd.Parameters.Add("@GiftCard", SqlDbType.Bit);
                SqlParameter pCheckService = cmd.Parameters.Add("@CheckService", SqlDbType.Bit);
                SqlParameter pEBT = cmd.Parameters.Add("@EBT", SqlDbType.Bit);
                SqlParameter pCheckServiceName = cmd.Parameters.Add("@CheckServiceName", SqlDbType.VarChar);
                SqlParameter pWirelessAccess = cmd.Parameters.Add("@WirelessAccess", SqlDbType.VarChar);
                SqlParameter pWirelessTransFee = cmd.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pInterchange = cmd.Parameters.Add("@Interchange", SqlDbType.Bit);
                SqlParameter pAssessments = cmd.Parameters.Add("@Assessments", SqlDbType.Bit);

                pAppId.Value = AppId;
                pGateway.Value = Gateway;
                pProcessor.Value = Processor;
                pAnnualFee.Value = AnnualFee;
                pRepNum.Value = RepNum;
                pCustServFee.Value = CustServFee;
                pMonMin.Value = MonMin;
                pInternetStmt.Value = InternetStmt;
                pDiscQNP.Value = DiscQNP;
                pDiscMQ.Value = DiscMQ;
                pDiscNQ.Value = DiscNQ;
                pDiscQD.Value = DiscQD;
                pDiscQP.Value = DiscQP;
                pTransFee.Value = TransFee;
                pRetrievalFee.Value = RetrievalFee;
                pRollingReserve.Value = RollingReserve;
                pVoiceAuth.Value = VoiceAuth;
                pBatchHeader.Value = BatchHeader;
                pAVS.Value = AVS;
                pNBCTFee.Value = NBCTFee;
                pCBFee.Value = CBFee;
                pAcctType.Value = AcctType;
                pGWMonFee.Value = GWMonFee;
                pGWTransFee.Value = GWTransFee;
                pGWSetupFee.Value = GWSetupFee;
                pCGMonFee.Value = CGMonFee;
                pCGTransFee.Value = CGTransFee;
                pCGMonMin.Value = CGMonMin;
                pCGDiscRate.Value = CGDiscRate;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pGCMonFee.Value = GCMonFee;
                pGCTransFee.Value = GCTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;
                pOnlineDebit.Value = OnlineDebit;
                pGiftCard.Value = GiftCard;
                pCheckService.Value = CheckService;
                pCheckServiceName.Value = CheckServiceName;
                pWirelessAccess.Value = WirelessAccess;
                pWirelessTransFee.Value = WirelessTransFee;
                pEBT.Value = EBT;
                pAssessments.Value = Assessments;
                pInterchange.Value = Interchange;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function UpdateRatesInACT

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateSecContactInfo(string ContactID, int AppId, string PrimaryContact, string Title,
            string FullName, string FirstName, string LastName, string SignupEmail, string Phone, string PhoneExt,
            string SecMobilePhone, string HomePhone)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateSecContact", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add(new SqlParameter("@AppID", SqlDbType.Int));
                SqlParameter pContactID = cmd.Parameters.Add("@PrimaryContactID", SqlDbType.VarChar);
                SqlParameter pFullName = cmd.Parameters.Add("@FullName", SqlDbType.VarChar);
                SqlParameter pPrimaryContact = cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LASTNAME", SqlDbType.VarChar);
                SqlParameter pTitle = cmd.Parameters.Add("@Title", SqlDbType.VarChar);
                SqlParameter pSignupEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar);
                SqlParameter pPhoneExt = cmd.Parameters.Add("@PhoneExt", SqlDbType.VarChar);
                SqlParameter pSecMobilePhone = cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar);
                SqlParameter pSecHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);

                pAppId.Value = AppId;
                pContactID.Value = ContactID;
                pFullName.Value = FullName;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pTitle.Value = Title;
                pSignupEmail.Value = SignupEmail;
                pPhone.Value = Phone;
                pPhoneExt.Value = PhoneExt;
                pSecMobilePhone.Value = SecMobilePhone;
                pPrimaryContact.Value = PrimaryContact;
                pSecHomePhone.Value = HomePhone;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function add secondary contact info

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateActCust(string ContactID, string AddlComments, string OtherRefund,
            string NameOfPrevProc, string FormerMerchantNums, string ReasonLeavingProc, string P2LastName,
            string P2FirstName, string P2Title, string P2OwnPct, string P2LOR, string P2HomeAddr1,
            string P2HomeCity, string P2HomeState, string P2HomeZip, string P2HomeCountry, string P2SocialSecurity,
            string P2LivingStatus, string P2DLNum, string P2DLState, string P2DLExpDate, string P2DOB,
            string BankAddress, string TerminalID, string LoginID, string BankIDNum, string AgentBankIDNum, 
            string AgentChainNum, string MCCCategoryCode, string StoreNum,           
            string MaxTicket, bool OnlineDebit, bool GiftCard, bool CheckService, string CheckServiceName, 
            bool EBT, bool MerchantFunding, string USDANum, string DebitMonFee, string DebitTransFee, string CGMonFee, string CGTransFee, 
            string CGMonMin, string CGDiscRate, string GiftCardType, string GCMonFee, string GCTransFee, string EBTMonFee, 
            string EBTTransFee, string WirelessAccess, string WirelessTransFee, 
            bool Interchange, bool Assessments, string RollingReserve, string Lease, string LeaseCompany, string LeasePayment, 
            string LeaseTerm, string MCAAmount, string MCAStatus)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateActCUST", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pAddlComments = cmd.Parameters.Add("@AddlComments", SqlDbType.VarChar);
                SqlParameter pOtherRefund = cmd.Parameters.Add("@OtherRefund", SqlDbType.VarChar);
                SqlParameter pNameOfPrevProc = cmd.Parameters.Add("@NameOfPrevProc", SqlDbType.VarChar);
                SqlParameter pFormerMerchantNums = cmd.Parameters.Add("@FormerMerchantNums", SqlDbType.VarChar);
                SqlParameter pReasonLeavingProc = cmd.Parameters.Add("@ReasonLeavingProc", SqlDbType.VarChar);
                SqlParameter pP2LastName = cmd.Parameters.Add("@P2LastName", SqlDbType.VarChar);
                SqlParameter pP2FirstName = cmd.Parameters.Add("@P2FirstName", SqlDbType.VarChar);
                SqlParameter pP2Title = cmd.Parameters.Add("@P2Title", SqlDbType.VarChar);
                SqlParameter pP2OwnPct = cmd.Parameters.Add("@P2OwnPct", SqlDbType.VarChar);
                SqlParameter pP2LOR = cmd.Parameters.Add("@P2LOR", SqlDbType.VarChar);
                SqlParameter pP2HomeAddr1 = cmd.Parameters.Add("@P2HomeAddr1", SqlDbType.VarChar);
                SqlParameter pP2HomeCity = cmd.Parameters.Add("@P2HomeCity", SqlDbType.VarChar);
                SqlParameter pP2HomeState = cmd.Parameters.Add("@P2HomeState", SqlDbType.VarChar);
                SqlParameter pP2HomeZip = cmd.Parameters.Add("@P2HomeZip", SqlDbType.VarChar);
                SqlParameter pP2HomeCountry = cmd.Parameters.Add("@P2HomeCountry", SqlDbType.VarChar);
                SqlParameter pP2SocialSecurity = cmd.Parameters.Add("@P2SocialSecurity", SqlDbType.VarChar);
                SqlParameter pP2LivingStatus = cmd.Parameters.Add("@P2LivingStatus", SqlDbType.VarChar);
                SqlParameter pP2DLNum = cmd.Parameters.Add("@P2DLNum", SqlDbType.VarChar);
                SqlParameter pP2DLState = cmd.Parameters.Add("@P2DLState", SqlDbType.VarChar);
                SqlParameter pP2DLExpDate = cmd.Parameters.Add("@P2DLExpDate", SqlDbType.VarChar);
                SqlParameter pP2DOB = cmd.Parameters.Add("@P2DOB", SqlDbType.VarChar);
                SqlParameter pBankAddr = cmd.Parameters.Add("@BankAddr", SqlDbType.VarChar);
                SqlParameter pTerminalID = cmd.Parameters.Add("@TerminalID", SqlDbType.VarChar);
                SqlParameter pLoginID = cmd.Parameters.Add("@LoginID", SqlDbType.VarChar);
                SqlParameter pBankIDNum = cmd.Parameters.Add("@BankIDNum", SqlDbType.VarChar);
                SqlParameter pAgentBankIDNum = cmd.Parameters.Add("@AgentBankIDNum", SqlDbType.VarChar);
                SqlParameter pAgentChainNum = cmd.Parameters.Add("@AgentChainNum", SqlDbType.VarChar);
                SqlParameter pMCCCategoryCode = cmd.Parameters.Add("@MCCCategoryCode", SqlDbType.VarChar);
                SqlParameter pStoreNum = cmd.Parameters.Add("@StoreNum", SqlDbType.VarChar);
                SqlParameter pMaxTicket = cmd.Parameters.Add("@MaxTicket", SqlDbType.VarChar);
                SqlParameter pOnlineDebit = cmd.Parameters.Add("@OnlineDebit", SqlDbType.Bit);
                SqlParameter pGiftCard = cmd.Parameters.Add("@GiftCard", SqlDbType.Bit);
                SqlParameter pCheckService = cmd.Parameters.Add("@CheckService", SqlDbType.Bit);
                SqlParameter pCheckServiceName = cmd.Parameters.Add("@CheckServiceName", SqlDbType.VarChar);
                SqlParameter pEBT = cmd.Parameters.Add("@EBT", SqlDbType.Bit);
                SqlParameter pMerchantFunding = cmd.Parameters.Add("@MerchantFunding", SqlDbType.Bit);
                SqlParameter pUSDANum = cmd.Parameters.Add("@USDANum", SqlDbType.VarChar);

                SqlParameter pDebitMonFee = cmd.Parameters.Add("@DebitMonFee", SqlDbType.VarChar);
                SqlParameter pDebitTransFee = cmd.Parameters.Add("@DebitTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonFee = cmd.Parameters.Add("@CGMonFee", SqlDbType.VarChar);
                SqlParameter pCGTransFee = cmd.Parameters.Add("@CGTransFee", SqlDbType.VarChar);
                SqlParameter pCGMonMin = cmd.Parameters.Add("@CGMonMin", SqlDbType.VarChar);
                SqlParameter pCGDiscRate = cmd.Parameters.Add("@CGDiscRate", SqlDbType.VarChar);
                SqlParameter pGiftCardType = cmd.Parameters.Add("@GiftType", SqlDbType.VarChar); 
                SqlParameter pGCMonFee = cmd.Parameters.Add("@GCMonFee", SqlDbType.VarChar);
                SqlParameter pGCTransFee = cmd.Parameters.Add("@GCTransFee", SqlDbType.VarChar);
                SqlParameter pEBTMonFee = cmd.Parameters.Add("@EBTMonFee", SqlDbType.VarChar);
                SqlParameter pEBTTransFee = cmd.Parameters.Add("@EBTTransFee", SqlDbType.VarChar);
                SqlParameter pWirelessAccess = cmd.Parameters.Add("@WirelessAccess", SqlDbType.VarChar);
                SqlParameter pWirelessTransFee = cmd.Parameters.Add("@WirelessTransFee", SqlDbType.VarChar);
                SqlParameter pInterchange = cmd.Parameters.Add("@Interchange", SqlDbType.Bit);
                SqlParameter pAssessments = cmd.Parameters.Add("@Assessments", SqlDbType.Bit);
                SqlParameter pRollingReserve = cmd.Parameters.Add("@RollingReserve", SqlDbType.VarChar);
                SqlParameter pLease = cmd.Parameters.Add("@Lease", SqlDbType.Bit);
                SqlParameter pLeaseCompany = cmd.Parameters.Add("@LeaseCompany", SqlDbType.VarChar);
                SqlParameter pLeasePayment = cmd.Parameters.Add("@LeasePayment", SqlDbType.VarChar);
                SqlParameter pLeaseTerm = cmd.Parameters.Add("@LeaseTerm", SqlDbType.VarChar);
                SqlParameter pMCAAmount = cmd.Parameters.Add("@MCAAmount", SqlDbType.VarChar);
                SqlParameter pMCAStatus = cmd.Parameters.Add("@MCAStatus", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pAddlComments.Value = AddlComments;
                pOtherRefund.Value = OtherRefund;
                pNameOfPrevProc.Value = NameOfPrevProc;
                pFormerMerchantNums.Value = FormerMerchantNums;
                pReasonLeavingProc.Value = ReasonLeavingProc;
                pP2LastName.Value = P2LastName;
                pP2FirstName.Value = P2FirstName;
                pP2Title.Value = P2Title;
                pP2OwnPct.Value = P2OwnPct;
                pP2LOR.Value = P2LOR;
                pP2HomeAddr1.Value = P2HomeAddr1;
                pP2HomeCity.Value = P2HomeCity;
                pP2HomeState.Value = P2HomeState;
                pP2HomeZip.Value = P2HomeZip;
                pP2HomeCountry.Value = P2HomeCountry;
                pP2SocialSecurity.Value = P2SocialSecurity;
                pP2LivingStatus.Value = P2LivingStatus;
                pP2DLNum.Value = P2DLNum;
                pP2DLState.Value = P2DLState;
                pP2DLExpDate.Value = P2DLExpDate;
                pP2DOB.Value = P2DOB;
                pBankAddr.Value = BankAddress;
                pTerminalID.Value = TerminalID;
                pLoginID.Value = LoginID;
                pBankIDNum.Value = BankIDNum;
                pAgentBankIDNum.Value = AgentBankIDNum;
                pAgentChainNum.Value = AgentChainNum;
                pMCCCategoryCode.Value = MCCCategoryCode;
                pStoreNum.Value = StoreNum;
                pMaxTicket.Value = MaxTicket;
                pOnlineDebit.Value = OnlineDebit;
                pGiftCard.Value = GiftCard;
                pCheckService.Value = CheckService;
                pCheckServiceName.Value = CheckServiceName;
                pEBT.Value = EBT;
                pMerchantFunding.Value = MerchantFunding;
                pUSDANum.Value = USDANum;
                pDebitMonFee.Value = DebitMonFee;
                pDebitTransFee.Value = DebitTransFee;
                pCGMonFee.Value = CGMonFee;
                pCGTransFee.Value = CGTransFee;
                pCGMonMin.Value = CGMonMin;
                pCGDiscRate.Value = CGDiscRate;
                pGiftCardType.Value = GiftCardType; 
                pGCMonFee.Value = GCMonFee;
                pGCTransFee.Value = GCTransFee;
                pEBTMonFee.Value = EBTMonFee;
                pEBTTransFee.Value = EBTTransFee;
                pWirelessAccess.Value = WirelessAccess;
                pWirelessTransFee.Value = WirelessTransFee;
                pInterchange.Value = Interchange;
                pAssessments.Value = Assessments;
                pRollingReserve.Value = RollingReserve;
                pLease.Value = Lease;
                pLeaseCompany.Value = LeaseCompany;
                pLeasePayment.Value = LeasePayment;
                pLeaseTerm.Value = LeaseTerm;
                pMCAAmount.Value = MCAAmount;
                pMCAStatus.Value = MCAStatus; 

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateActCust

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdatep1Email(string ContactID, string Email)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateP1ContactEmail", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pEmail.Value = Email;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateContactEmail

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateContactEmail(string ContactID, string Email)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateContactEmail", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pEmail.Value = Email;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateContactEmail

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateP2Email(string ContactID, string Email)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdAddInfoP2Email = new SqlCommand("sp_InsertUpdateP2Email", ConnACT);
                cmdAddInfoP2Email.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmdAddInfoP2Email.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pEmail = cmdAddInfoP2Email.Parameters.Add("@Email", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pEmail.Value = Email;

                cmdAddInfoP2Email.Connection.Open();
                cmdAddInfoP2Email.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateP2Email

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdatePricingStructure(string ContactID, string billingMethod)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdAddInfoPricingStructure = new SqlCommand("sp_InsertUpdatebillingMethod", ConnACT);
                cmdAddInfoPricingStructure.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmdAddInfoPricingStructure.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pBillingMethod = cmdAddInfoPricingStructure.Parameters.Add("@billing", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pBillingMethod.Value = billingMethod;

                cmdAddInfoPricingStructure.Connection.Open();
                cmdAddInfoPricingStructure.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateBillingMethod



        public bool InsertGroupContact(string ContactID, string GroupName)
        {
            SqlConnection ConnAct = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdGroupContact = new SqlCommand("sp_InsertGroupContact", ConnAct);
                cmdGroupContact.CommandType = CommandType.StoredProcedure;
                cmdGroupContact.Parameters.Add(new SqlParameter("@GroupName", GroupName));
                cmdGroupContact.Parameters.Add(new SqlParameter("@CONTACTID", ContactID));

                cmdGroupContact.Connection.Open();
                cmdGroupContact.ExecuteNonQuery();
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnAct.Close();
                ConnAct.Dispose();
            }
            return true;
        }//end function InsertGroupContact

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateBusinessAddress(string ContactID, string Line1, string Line2, string City,
            string State, string PostalCode, string Country)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateBusinessAddress", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pLine1 = cmd.Parameters.Add("@Address1", SqlDbType.VarChar);
                SqlParameter pLine2 = cmd.Parameters.Add("@Address2", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pPostalCode = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pLine1.Value = Line1;
                pLine2.Value = Line2;
                pCity.Value = City;
                pState.Value = State;
                pPostalCode.Value = PostalCode;
                pCountry.Value = Country;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateBusinessAddress

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateP1HomeAddress(string ContactID, string Line1, string Line2, string City,
            string State, string PostalCode, string Country)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateP1HomeAddress", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pLine1 = cmd.Parameters.Add("@Line1", SqlDbType.VarChar);
                SqlParameter pLine2 = cmd.Parameters.Add("@Line2", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pPostalCode = cmd.Parameters.Add("@PostalCode", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pLine1.Value = Line1;
                pLine2.Value = Line2;
                pCity.Value = City;
                pState.Value = State;
                pPostalCode.Value = PostalCode;
                pCountry.Value = Country;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateP1HomeAddress

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertUpdateP1BusinessPhone(string ContactID, string Phone, string PhoneExt)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateBusinessPhone", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@NumberDisplay", SqlDbType.VarChar);
                SqlParameter pPhoneExt = cmd.Parameters.Add("@Ext", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pPhone.Value = Phone;
                pPhoneExt.Value = PhoneExt;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertUpdateP1BusinessPhone

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public int InsertUpdatePhone(string PhoneType, string ContactID, string Phone)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdatePhone", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pPhoneType = cmd.Parameters.Add("@PhoneType", SqlDbType.VarChar);
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@NumberDisplay", SqlDbType.VarChar);

                pPhoneType.Value = PhoneType;
                pContactID.Value = ContactID;
                pPhone.Value = Phone;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
                return iRetVal;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function InsertUpdatePhone

        //CALLED BY SalesOppsBL.InsertSalesOppsInACT, ExportACTBL.AddInfoToACT
        public bool InsertSalesOpp(string ID, string ContactID, string OppUserID, string Product,
            string ProductCode, string Price, string COG, string Quantity, string SubTotal, string RepName,
            string LastModified, string CreateDate, string PaymentMethod, string Reprogram, string TerminalID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateSalesOpp", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pID = cmd.Parameters.Add("@ID", SqlDbType.VarChar);
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pOppUserID = cmd.Parameters.Add("@ActUserID", SqlDbType.VarChar);
                SqlParameter pProduct = cmd.Parameters.Add("@Product", SqlDbType.VarChar);
                SqlParameter pProductCode = cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar);
                SqlParameter pPrice = cmd.Parameters.Add("@Price", SqlDbType.VarChar);
                SqlParameter pCOG = cmd.Parameters.Add("@COG", SqlDbType.VarChar);
                SqlParameter pQuantity = cmd.Parameters.Add("@Quantity", SqlDbType.VarChar);
                SqlParameter pSubTotal = cmd.Parameters.Add("@SubTotal", SqlDbType.VarChar);
                SqlParameter pRepName = cmd.Parameters.Add("@RepName", SqlDbType.VarChar);
                SqlParameter pLastModified = cmd.Parameters.Add("@LastModified", SqlDbType.VarChar);
                SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.VarChar);
                SqlParameter pPaymentMethod = cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar);
                SqlParameter pReprogram = cmd.Parameters.Add("@Reprogram", SqlDbType.VarChar);
                SqlParameter pTerminalID = cmd.Parameters.Add("@TerminalID", SqlDbType.VarChar);

                pID.Value = ID;
                pContactID.Value = ContactID;
                pOppUserID.Value = OppUserID;
                pProduct.Value = Product;
                pProductCode.Value = ProductCode;
                pPrice.Value = Price;
                pCOG.Value = COG;
                pQuantity.Value = Quantity;
                pSubTotal.Value = SubTotal;
                pRepName.Value = RepName;
                pLastModified.Value = LastModified;
                pCreateDate.Value = CreateDate;
                pPaymentMethod.Value = PaymentMethod;
                pReprogram.Value = Reprogram;
                pTerminalID.Value = TerminalID;

                cmd.Connection.Open();
                int int1 = cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertSalesOpp

        //CALLED BY ReminderBL.InsertNoteReminder, OnlineAppStatusBL.InsertNote, ExportActBL.AddInfoToACT, ExportActBL.UpdateRatesInACT
        public bool InsertNotes(string NoteID, string ContactID, string NoteUserID, string NoteText, string DateReported)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertNote", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNoteID = cmd.Parameters.Add("@NoteID", SqlDbType.VarChar);
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pNoteUserID = cmd.Parameters.Add("@ActUserID", SqlDbType.VarChar);
                SqlParameter pNoteText = cmd.Parameters.Add("@Note", SqlDbType.VarChar);
                SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime);

                pNoteID.Value = NoteID;
                pContactID.Value = ContactID;
                pNoteUserID.Value = NoteUserID;
                pNoteText.Value = NoteText;
                pCreateDate.Value = DateReported;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertNotes

        //CALLED BY ExportActBL.UpdateRatesInACT, ExportActBL.UpdateAct, SalesOppsBL.InsertSalesOppsInACT
        public bool AddUpdateNoteToACT(string ContactID, string NoteText)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertOnlineAppNote", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmd.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pNoteText = cmd.Parameters.Add("@Note", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pNoteText.Value = NoteText;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function AddUpdateNoteToACT

        //CALLED BY ExportActBL.AddIntoToACT, ExportActBL.UpdateAct
        public bool InsertReminder(string ContactID, string Regarding)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdReminder = new SqlCommand("sp_InsertReminder", ConnACT);
                cmdReminder.CommandType = CommandType.StoredProcedure;
                SqlParameter pContactID = cmdReminder.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pRegarding = cmdReminder.Parameters.Add("@Regarding", SqlDbType.VarChar);

                pContactID.Value = ContactID;
                pRegarding.Value = Regarding;

                cmdReminder.Connection.Open();
                cmdReminder.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertReminder

        //This function checks if record exists in ACT!
        public string CheckRecordExists(string FirstName, string LastName, string P1FirstName, string P1LastName,
            string Email, string P1Email, string DBA, string Company, string Phone)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckAppDuplicates", ConnACT);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pPhone = cmd.Parameters.Add("@Business_Phone", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@SignupEmail", SqlDbType.VarChar);
                SqlParameter pP1Email = cmd.Parameters.Add("@P1Email", SqlDbType.VarChar);
                SqlParameter pP1FirstName = cmd.Parameters.Add("@P1FirstName", SqlDbType.VarChar);
                SqlParameter pP1LastName = cmd.Parameters.Add("@P1LastName", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pCompany = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.VarChar, 512);
                pRetVal.Direction = ParameterDirection.Output;

                pPhone.Value = Phone;
                pDBA.Value = DBA;
                pCompany.Value = Company;
                pEmail.Value = Email;
                pP1Email.Value = P1Email;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pP1FirstName.Value = P1FirstName;
                pP1LastName.Value = P1LastName;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                string strRetVal = Convert.ToString(pRetVal.Value);
                ConnACT.Close();
                ConnACT.Dispose();
                return strRetVal;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function CheckRecordExistsInACT
       
        /*
        //CALLED BY ExportActBL.ExportData
        public DataSet GetSalesOppsFromACT( string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetOnlineAppContact", ConnACT);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "VW_OnlineAppSalesOpps");
                return ds;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end GetSalesOppsFromACT
         */
        /*
        //CALLED BY ExportActBL.ExportActStatus
        public DataSet GetSalesOppsByAppID(int AppId)
        {
            string sqlQuery = "Select * from VW_OnlineAppSalesOpps where AppId = @AppId";

            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, ConnACT);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "VW_OnlineAppSalesOpps");
                return ds;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end GetSalesOppsByAppID
        */

        public DataSet GetAuthnetSummary(string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                String sqlQuery = "Select * from VW_OnlineAppFields";
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "VW_OnlineAppFields");
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetOnlineAppData


        //This function inserts Affiliate in ACT dropdown list
        public bool InsertAffiliateDropdown(string Company, string DBA)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertAffiliateDropDown", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pCompany = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);

                pCompany.Value = Company;
                pDBA.Value = DBA;

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
        }//end function InsertAffiliateGroup


        //This function inserts affiliate email info in ACT
        public bool CheckEmailExists(string Email)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckEmailExists", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pReturnVal = cmd.Parameters.Add("@Ret", SqlDbType.Bit);
                pReturnVal.Direction = ParameterDirection.ReturnValue;
                pEmail.Value = Email;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                bool retVal = Convert.ToBoolean(pReturnVal.Value);
                Conn.Close();
                Conn.Dispose();
                return retVal;
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
        }//end function InsertAffiliateEmail

        public bool CheckPhoneExists(string Phone)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckBusPhoneExists", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pEmail = cmd.Parameters.Add("@BusPhoneDisplay", SqlDbType.VarChar);
                SqlParameter pReturnVal = cmd.Parameters.Add("@Ret", SqlDbType.Bit);
                pReturnVal.Direction = ParameterDirection.ReturnValue;

                pEmail.Value = Phone;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                bool retVal = Convert.ToBoolean(pReturnVal.Value);
                Conn.Close();
                Conn.Dispose();
                return retVal;
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
        }//end function CheckPhoneExistsInACT

   

        //This function inserts affiliate info in ACT
        public int InsertAffiliate(int AffiliateID, string CreateDate,
                    string FirstName, string LastName, string AffiliateReferral, string OtherReferral,
                    string CompanyName, string DBA, string LegalStatus, string WebsiteURL,
                    string TaxID, string SocialSecurity, string Category, string Email,
                    string CompanyAddress, string City, string State,
                    string Zip, string Country,
                    string MailingAddress,
                    string MailingCity, string MailingState,
                    string MailingZip, string MailingCountry, 
                    string Telephone, string HomePhone, string MobilePhone, 
                    string Fax, string Comments, string BankName,
                    string BankAddress, string BankCity, string BankState, 
                    string BankZip, string BankPhone, 
                    string BankRoutingNumber, string BankAccountNumber)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_InsertActAffiliate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pAffiliateReferral = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pOtherReferral = cmd.Parameters.Add("@OtherReferral", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pLegalStatus  = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar);
                SqlParameter pURL = cmd.Parameters.Add("@Websiteurl", SqlDbType.VarChar);
                SqlParameter pTaxID = cmd.Parameters.Add("@FederalTaxID", SqlDbType.VarChar);
                SqlParameter pSocialSecurity = cmd.Parameters.Add("@SocialSecurity", SqlDbType.VarChar);
                SqlParameter pCategory = cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pCompanyName = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pCompanyAddress = cmd.Parameters.Add("@Address", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pMailingAddress = cmd.Parameters.Add("@MailingAddress", SqlDbType.VarChar);
                SqlParameter pMailingCity = cmd.Parameters.Add("@MailingCity", SqlDbType.VarChar);
                SqlParameter pMailingState = cmd.Parameters.Add("@MailingState", SqlDbType.VarChar);
                //SqlParameter pMailingRegion = cmd.Parameters.Add("@MailingRegion", SqlDbType.VarChar);
                SqlParameter pMailingZip = cmd.Parameters.Add("@MailingZipCode", SqlDbType.VarChar);
                SqlParameter pMailingCountry = cmd.Parameters.Add("@MailingCountry", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@PhoneNumberDisplay", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhoneDisplay", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhoneDisplay", SqlDbType.VarChar);
                SqlParameter pFax = cmd.Parameters.Add("@FaxNumberDisplay", SqlDbType.VarChar);
                SqlParameter pComments = cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                SqlParameter pBankName = cmd.Parameters.Add("@BankName", SqlDbType.VarChar);
                SqlParameter pBankAddress = cmd.Parameters.Add("@BankAddress", SqlDbType.VarChar);
                SqlParameter pBankCity = cmd.Parameters.Add("@BankCity", SqlDbType.VarChar);
                SqlParameter pBankState = cmd.Parameters.Add("@BankState", SqlDbType.VarChar);
                SqlParameter pBankZip = cmd.Parameters.Add("@BankZip", SqlDbType.VarChar);                
                SqlParameter pBankPhone = cmd.Parameters.Add("@BankPhone", SqlDbType.VarChar);                
                SqlParameter pBankRoutingNumber = cmd.Parameters.Add("@BankRoutingNumber", SqlDbType.VarChar);
                SqlParameter pBankAccountNumber = cmd.Parameters.Add("@BankAccountNumber", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.SmallInt);
                pRetVal.Direction = ParameterDirection.ReturnValue;

                pAffiliateID.Value = AffiliateID;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pAffiliateReferral.Value = AffiliateReferral;
                pOtherReferral.Value = OtherReferral;
                pCompanyName.Value = CompanyName;
                pDBA.Value = DBA;
                pLegalStatus.Value = LegalStatus;
                pURL.Value = WebsiteURL;
                pTaxID.Value = TaxID;
                pSocialSecurity.Value = SocialSecurity;
                pCategory.Value = Category;
                pEmail.Value = Email;
                pCompanyAddress.Value = CompanyAddress;
                pCity.Value = City;
                pState.Value = State;
                pZip.Value = Zip;
                pCountry.Value = Country;
                pMailingAddress.Value = MailingAddress;
                pMailingCity.Value = MailingCity;
                pMailingState.Value = MailingState;
                pMailingZip.Value = MailingZip;
                pMailingCountry.Value = MailingCountry;
                pPhone.Value = Telephone;
                pHomePhone.Value = HomePhone;
                pMobilePhone.Value = MobilePhone;
                pFax.Value = Fax;
                pComments.Value = Comments;
                pBankName.Value = BankName;
                pBankAddress.Value = BankAddress;
                pBankCity.Value = BankCity;
                pBankState.Value = BankState;
                pBankZip.Value = BankZip;
                pBankPhone.Value = BankPhone;                
                pBankRoutingNumber.Value = BankRoutingNumber;
                pBankAccountNumber.Value = BankAccountNumber;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                int iRetVal = Convert.ToInt32(pRetVal.Value);
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
        }//end function InsertAffiliate

        //This function inserts affiliate info in ACT after the user confirms new record creation upon prompt
        public int InsertNewAffiliateOnConfirm(int AffiliateID, string CreateDate,
                    string FirstName, string LastName, string AffiliateReferral, string OtherReferral,
                    string CompanyName, string DBA, string LegalStatus, string WebsiteURL,
                    string TaxID, string SocialSecurity, string Category, string Email,
                    string CompanyAddress, string City, string State,
                    string Zip, string Country,
                    string MailingAddress,
                    string MailingCity, string MailingState,
                    string MailingZip, string MailingCountry, 
                    string Telephone,
                    string HomePhone, string MobilePhone, 
                    string Fax, string Comments, string BankName,
                    string BankAddress, string BankCity, string BankState,
                    string BankZip, string BankPhone, 
                    string BankRoutingNumber, string BankAccountNumber)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_InsertActAffiliateOnConfirm", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);
                //SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pAffiliateReferral = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pOtherReferral = cmd.Parameters.Add("@OtherReferral", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pLegalStatus = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar);
                SqlParameter pURL = cmd.Parameters.Add("@Websiteurl", SqlDbType.VarChar);
                SqlParameter pTaxID = cmd.Parameters.Add("@FederalTaxID", SqlDbType.VarChar);
                SqlParameter pSocialSecurity = cmd.Parameters.Add("@SocialSecurity", SqlDbType.VarChar);
                SqlParameter pCategory = cmd.Parameters.Add("@Category", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pCompanyName = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pCompanyAddress = cmd.Parameters.Add("@Address", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pMailingAddress = cmd.Parameters.Add("@MailingAddress", SqlDbType.VarChar);
                SqlParameter pMailingCity = cmd.Parameters.Add("@MailingCity", SqlDbType.VarChar);
                SqlParameter pMailingState = cmd.Parameters.Add("@MailingState", SqlDbType.VarChar);
                //SqlParameter pMailingRegion = cmd.Parameters.Add("@MailingRegion", SqlDbType.VarChar);
                SqlParameter pMailingZip = cmd.Parameters.Add("@MailingZipCode", SqlDbType.VarChar);
                SqlParameter pMailingCountry = cmd.Parameters.Add("@MailingCountry", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@PhoneNumberDisplay", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhoneDisplay", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhoneDisplay", SqlDbType.VarChar);
                SqlParameter pFax = cmd.Parameters.Add("@FaxNumberDisplay", SqlDbType.VarChar);
                SqlParameter pComments = cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                SqlParameter pBankName = cmd.Parameters.Add("@BankName", SqlDbType.VarChar);
                SqlParameter pBankAddress = cmd.Parameters.Add("@BankAddress", SqlDbType.VarChar);
                SqlParameter pBankCity = cmd.Parameters.Add("@BankCity", SqlDbType.VarChar);
                SqlParameter pBankState = cmd.Parameters.Add("@BankState", SqlDbType.VarChar);
                SqlParameter pBankZip = cmd.Parameters.Add("@BankZip", SqlDbType.VarChar);
                SqlParameter pBankPhone = cmd.Parameters.Add("@BankPhone", SqlDbType.VarChar);
                SqlParameter pBankRoutingNumber = cmd.Parameters.Add("@BankRoutingNumber", SqlDbType.VarChar);
                SqlParameter pBankAccountNumber = cmd.Parameters.Add("@BankAccountNumber", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.SmallInt);
                pRetVal.Direction = ParameterDirection.ReturnValue;

                pAffiliateID.Value = AffiliateID;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pAffiliateReferral.Value = AffiliateReferral;
                pOtherReferral.Value = OtherReferral;
                pCompanyName.Value = CompanyName;
                pDBA.Value = DBA;
                pLegalStatus.Value = LegalStatus;
                pURL.Value = WebsiteURL;
                pTaxID.Value = TaxID;
                pSocialSecurity.Value = SocialSecurity;
                pCategory.Value = Category;
                pEmail.Value = Email;
                pCompanyAddress.Value = CompanyAddress;
                pCity.Value = City;
                pState.Value = State;
                pZip.Value = Zip;
                pCountry.Value = Country;
                pMailingAddress.Value = MailingAddress;
                pMailingCity.Value = MailingCity;
                pMailingState.Value = MailingState;
                pMailingZip.Value = MailingZip;
                pMailingCountry.Value = MailingCountry;
                pPhone.Value = Telephone;
                pHomePhone.Value = HomePhone;
                pMobilePhone.Value = MobilePhone;
                pFax.Value = Fax;
                pComments.Value = Comments;
                pBankName.Value = BankName;
                pBankAddress.Value = BankAddress;
                pBankCity.Value = BankCity;
                pBankState.Value = BankState;
                pBankZip.Value = BankZip;
                pBankPhone.Value = BankPhone;
                pBankRoutingNumber.Value = BankRoutingNumber;
                pBankAccountNumber.Value = BankAccountNumber;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                int iRetVal = Convert.ToInt32(pRetVal.Value);
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
        }//end function InsertNewAffiliateOnConfirm


        //This function updates affiliate info in ACT
        public int UpdateAffiliate(int AffiliateID, string CreateDate,
                    string FirstName, string LastName, string AffiliateReferral, string OtherReferral,
                    string CompanyName, string DBA, string LegalStatus, string WebsiteURL,
                    string TaxID, string SocialSecurity, string Email,
                    string CompanyAddress, string City, string State,
                    string Zip, string Country, string Telephone,
                    string HomePhone, string MobilePhone, 
                    string Fax, string Comments, string MailingAddress,
                    string MailingCity, string MailingState,
                    string MailingZip, string MailingCountry, string BankName,
                    string BankAddress, string BankCity, string BankState,
                    string BankZip, string BankPhone, 
                    string BankRoutingNumber, string BankAccountNumber)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_UpdateActAffiliate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);
                //SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pAffiliateReferral = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pOtherReferral = cmd.Parameters.Add("@OtherReferral", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pLegalStatus = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar); 
                SqlParameter pURL = cmd.Parameters.Add("@Websiteurl", SqlDbType.VarChar);
                SqlParameter pTaxID = cmd.Parameters.Add("@FederalTaxID", SqlDbType.VarChar);
                SqlParameter pSocialSecurity = cmd.Parameters.Add("@SocialSecurity", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pCompanyName = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pCompanyAddress = cmd.Parameters.Add("@Address", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pMailingAddress = cmd.Parameters.Add("@MailingAddress", SqlDbType.VarChar);
                SqlParameter pMailingCity = cmd.Parameters.Add("@MailingCity", SqlDbType.VarChar);
                SqlParameter pMailingState = cmd.Parameters.Add("@MailingState", SqlDbType.VarChar);
                SqlParameter pMailingZip = cmd.Parameters.Add("@MailingZipCode", SqlDbType.VarChar);
                SqlParameter pMailingCountry = cmd.Parameters.Add("@MailingCountry", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@PhoneNumberDisplay", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhoneDisplay", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhoneDisplay", SqlDbType.VarChar);
                SqlParameter pFax = cmd.Parameters.Add("@FaxNumberDisplay", SqlDbType.VarChar);
                SqlParameter pComments = cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                SqlParameter pBankName = cmd.Parameters.Add("@BankName", SqlDbType.VarChar);
                SqlParameter pBankAddress = cmd.Parameters.Add("@BankAddress", SqlDbType.VarChar);
                SqlParameter pBankCity = cmd.Parameters.Add("@BankCity", SqlDbType.VarChar);
                SqlParameter pBankState = cmd.Parameters.Add("@BankState", SqlDbType.VarChar);
                SqlParameter pBankZip = cmd.Parameters.Add("@BankZip", SqlDbType.VarChar);
                SqlParameter pBankPhone = cmd.Parameters.Add("@BankPhone", SqlDbType.VarChar);                
                SqlParameter pBankRoutingNumber = cmd.Parameters.Add("@BankRoutingNumber", SqlDbType.VarChar);
                SqlParameter pBankAccountNumber = cmd.Parameters.Add("@BankAccountNumber", SqlDbType.VarChar);
                SqlParameter pRetVal = cmd.Parameters.Add("@RetVal", SqlDbType.SmallInt);
                pRetVal.Direction = ParameterDirection.ReturnValue;

                pAffiliateID.Value = AffiliateID;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pAffiliateReferral.Value = AffiliateReferral;
                pOtherReferral.Value = OtherReferral;
                pCompanyName.Value = CompanyName;
                pDBA.Value = DBA;
                pLegalStatus.Value = LegalStatus;
                pURL.Value = WebsiteURL;
                pTaxID.Value = TaxID;
                pSocialSecurity.Value = SocialSecurity;
                pEmail.Value = Email;
                pCompanyAddress.Value = CompanyAddress;
                pCity.Value = City;
                pState.Value = State;
                pZip.Value = Zip;
                pCountry.Value = Country;
                pMailingAddress.Value = MailingAddress;
                pMailingCity.Value = MailingCity;
                pMailingState.Value = MailingState;
                pMailingZip.Value = MailingZip;
                pMailingCountry.Value = MailingCountry;
                pPhone.Value = Telephone;
                pHomePhone.Value = HomePhone;
                pMobilePhone.Value = MobilePhone;
                pFax.Value = Fax;
                pComments.Value = Comments;
                pBankName.Value = BankName;
                pBankAddress.Value = BankAddress;
                pBankCity.Value = BankCity;
                pBankState.Value = BankState;
                pBankZip.Value = BankZip;
                pBankPhone.Value = BankPhone;                
                pBankRoutingNumber.Value = BankRoutingNumber;
                pBankAccountNumber.Value = BankAccountNumber;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                int iRetVal = Convert.ToInt32(pRetVal.Value);
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
        }//end function UpdateAffiliate        

        //CALLED BY ExportActBL.ExportData
        public DataSet GetOnlineAppFields(string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                string sqlQuery = "Select * from VW_OnlineAppFields where CONTACTID = @ContactID";
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "VW_OnlineAppFields");
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetDataFromContactID        

        //CALLED BY ProductsBL.DeleteProductInfo
        public bool DeleteProductACT(int Code)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                string sqlQuery = "DELETE FROM tbl_Product WHERE Code = @Code";
                SqlCommand cmdCOG = new SqlCommand(sqlQuery, Conn);
                cmdCOG.Connection.Open();
                cmdCOG.Parameters.Add(new SqlParameter("@Code", Code));
                cmdCOG.ExecuteNonQuery();
                return true;
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
        }//end DeleteProductACT

        //CALLED BY ProductsBL.InsertUpdateProductInfoACT, ProductsBL.UpdateProductInfo
        public int InsertUpdateProductACT(int ProductCode, string Product, string Desc, decimal COG, decimal Price)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdCOG = new SqlCommand("SP_InsertUpdateCOG", Conn);
                cmdCOG.CommandType = CommandType.StoredProcedure;
                SqlParameter pProductCode = cmdCOG.Parameters.Add("@ProductCode", SqlDbType.SmallInt);
                SqlParameter pProduct = cmdCOG.Parameters.Add("@ProductName", SqlDbType.VarChar);
                SqlParameter pDesc = cmdCOG.Parameters.Add("@Description", SqlDbType.VarChar);
                SqlParameter pCOG = cmdCOG.Parameters.Add("@COG", SqlDbType.Decimal);
                SqlParameter pPrice = cmdCOG.Parameters.Add("@Price", SqlDbType.Decimal);

                pProductCode.Value = ProductCode;
                pProduct.Value = Product;
                pDesc.Value = Desc;
                pCOG.Value = COG;
                pPrice.Value = Price;

                cmdCOG.Connection.Open();
                int iRetVal = cmdCOG.ExecuteNonQuery();
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
        }//end function InsertUpdateProductACT

        /*public DataSet GetCommissionsFromACT1(string strMonth)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {

                string sqlQuery = "SELECT  * from  VW_SalesOppsCurrMonth WHERE (VW_SalesOppsCurrMonth.ACTUALCLOSEDATE >= CONVERT(DATETIME, CAST(DATEPART(year, CONVERT(DATETIME, @Month)) AS varchar(4)) + '-' + CAST(DATEPART(month, CONVERT(DATETIME, @Month)) AS varchar(2)) + '-1', 102)) AND ( VW_SalesOppsCurrMonth.ACTUALCLOSEDATE< (CASE WHEN DATEPART(month, GETDATE()) = 12 THEN CONVERT(DATETIME, CAST(DATEPART(year, CONVERT(DATETIME, @Month)) + 1 AS varchar(4)) + '-01-01', 102) ELSE CONVERT(DATETIME, CAST(DATEPART(year, CONVERT(DATETIME, @Month)) AS varchar(4))  + '-' + CAST(DATEPART(month, CONVERT(DATETIME, @Month)) + 1 AS varchar(2)) + '-1', 102) END)) AND (VW_SalesOppsCurrMonth.STATUSNUM = 1)";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Month", strMonth));

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
        }*///end function ReturnCommissionsFromACT


        //This function returns commissions from ACT
        public DataSet GetCommissionsFromACT(string strMonth)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                /*
                string sqlQuery = "SELECT  * from  VW_SalesOppsCurrMonth WHERE (VW_SalesOppsCurrMonth.ACTUALCLOSEDATE >= CONVERT(DATETIME, CAST(DATEPART(year, CONVERT(DATETIME, @Month)) AS varchar(4)) + '-' + CAST(DATEPART(month, CONVERT(DATETIME, @Month)) AS varchar(2)) + '-1', 102)) AND ( VW_SalesOppsCurrMonth.ACTUALCLOSEDATE< (CASE WHEN DATEPART(month, GETDATE()) = 12 THEN CONVERT(DATETIME, CAST(DATEPART(year, CONVERT(DATETIME, @Month)) + 1 AS varchar(4)) + '-01-01', 102) ELSE CONVERT(DATETIME, CAST(DATEPART(year, CONVERT(DATETIME, @Month)) AS varchar(4))  + '-' + CAST(DATEPART(month, CONVERT(DATETIME, @Month)) + 1 AS varchar(2)) + '-1', 102) END)) AND (VW_SalesOppsCurrMonth.STATUSNUM = 1)";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Month", strMonth));

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
                */

                SqlCommand cmdComm = new SqlCommand("SP_GetSalesOppsCurrMonth", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;

                //cmdComm.Parameters.AddWithValue("@Month", strMonth);
                cmdComm.Parameters["@Month"].Value = strMonth;
                //cmdComm.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(cmdComm);
                //adapter.SelectCommand = cmdComm;
                //DataSet ds = cmdComm.ExecuteNonQuery();

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
        }//end function ReturnCommissionsFromACT

        //This function checks if the lead has already been added to ACT!
        public DataSet CheckLeadExists(string FirstName, string LastName)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                string sqlQuery = "Select * FROM TBL_CONTACT WHERE FIRSTNAME = @FirstName AND LASTNAME = @LastName";
                SqlCommand cmd = new SqlCommand(sqlQuery, ConnACT);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@FirstName", FirstName));
                cmd.Parameters.Add(new SqlParameter("@LastName", LastName));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "TBL_CONTACT");
                return ds;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end check CheckLeadExists

        //This function inserts lead info into ACT
        public bool InsertLeadFreeApply(string CreateDate, string Contact, string FirstName, string LastName, string ReferralSource,
            string Email, string Phone, string CountryCodeHome, string HomePhone, string MobilePhone, string Company, string Website,
            string Address, string City, string State, string ZipCode, string Country, string Comments, string Cart)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertActFreeApply", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.VarChar);
                SqlParameter pContact = cmd.Parameters.Add("@Contact", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pReferralSource = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@Phone", SqlDbType.VarChar);
                SqlParameter pCountryCodeHome = cmd.Parameters.Add("@CountryCodeHome", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar);
                SqlParameter pCompany = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pURL = cmd.Parameters.Add("@Websiteurl", SqlDbType.VarChar);
                SqlParameter pAddress = cmd.Parameters.Add("@Address", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pComments = cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                SqlParameter pCart = cmd.Parameters.Add("@Cart", SqlDbType.VarChar);

                pCreateDate.Value = CreateDate;
                pContact.Value = Contact;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pReferralSource.Value = ReferralSource;
                pEmail.Value = Email;
                pPhone.Value = Phone;
                pCountryCodeHome.Value = CountryCodeHome;
                pHomePhone.Value = HomePhone;
                pMobilePhone.Value = MobilePhone;
                pCompany.Value = Company;
                pURL.Value = Website;
                pAddress.Value = Address;
                pCity.Value = City;
                pState.Value = State;
                pZip.Value = ZipCode;
                pCountry.Value = Country;
                pComments.Value = Comments;
                pCart.Value = Cart;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end function InsertLeadFreeApply


        //This function inserts lead info into ACT
        public bool InsertLeadFreeConsult(string CreateDate, string Contact, string FirstName, string LastName, string ReferralSource,
            string Email, string CountryCode, string Phone, string HomePhone)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertActFreeConsult", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.VarChar);
                SqlParameter pContact = cmd.Parameters.Add("@Contact", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pReferralSource = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pCountryCode = cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@Phone", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);

                pCreateDate.Value = CreateDate;
                pContact.Value = Contact;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pReferralSource.Value = ReferralSource;
                pEmail.Value = Email;
                pCountryCode.Value = CountryCode;
                pPhone.Value = Phone;
                pHomePhone.Value = HomePhone;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end function InsertLeadFreeConsult

        //This function inserts lead info into ACT
        public bool InsertLeadFreeReport(string CreateDate, string Contact, string FirstName, string LastName, string ReferralSource,
            string Email)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertActFreeReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.VarChar);
                SqlParameter pContact = cmd.Parameters.Add("@Contact", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pReferralSource = cmd.Parameters.Add("@AffiliateReferral", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);

                pCreateDate.Value = CreateDate;
                pContact.Value = Contact;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pReferralSource.Value = ReferralSource;
                pEmail.Value = Email;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end function InsertLeadFreeReport


        //Inserts a History and returns the dataset containing the HistoryID. 
        //CALLED BY ReminderBL.InsertACTHistory
        public bool InsertHistory(string HistoryID, string ContactID, string ActUserID, string DateReported, string Details)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdHistory = new SqlCommand("SP_InsertHistory", ConnACT);
                cmdHistory.CommandType = CommandType.StoredProcedure;
                SqlParameter pHistoryID = cmdHistory.Parameters.Add("@HistoryID", SqlDbType.VarChar);
                SqlParameter pContactID = cmdHistory.Parameters.Add("@ContactID", SqlDbType.VarChar);
                SqlParameter pActUserID = cmdHistory.Parameters.Add("@ActUserID", SqlDbType.VarChar);
                SqlParameter pDateReported = cmdHistory.Parameters.Add("@CreateDate", SqlDbType.DateTime);
                SqlParameter pDetails = cmdHistory.Parameters.Add("@Details", SqlDbType.VarChar);
                //SqlParameter pFileName = cmdHistory.Parameters.Add("@FileName", SqlDbType.Int);
                //SqlParameter pDisplayName = cmdHistory.Parameters.Add("@DisplayName", SqlDbType.Int);

                pHistoryID.Value = HistoryID;
                pContactID.Value = ContactID;
                pActUserID.Value = ActUserID;
                pDateReported.Value = DateReported;
                pDetails.Value = Details;
                //pFileName.Value = FileName;
                //pDisplayName.Value = DisplayName;

                cmdHistory.Connection.Open();
                cmdHistory.ExecuteNonQuery();
                ConnACT.Close();
                ConnACT.Dispose();

            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                ConnACT.Close();
                ConnACT.Dispose();
            }
            return true;
        }//end function InsertHistory
    }//end class ACTDataDL
}
