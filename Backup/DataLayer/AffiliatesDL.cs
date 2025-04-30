using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class AffiliatesDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        

        //This function gets the Master Rep Number for the Affiliate. CALLED BY AffiliatesBL.ReturnRepNumForSession
        public string ReturnAffiliateRepNum(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetMasterNumAffiliate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");
                string MasterNum = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    MasterNum = dr[0].ToString();                    
                }
                return MasterNum;
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
        }//end GetAffiliateRepNum

        //This function gets the Master Rep Number for the Affiliate. CALLED BY AffiliatesBL.ReturnRepNumForSession
        public string ReturnAffiliateT1MasterNum(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetT1MasterNumAffiliate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");
                string T1MasterNum = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    T1MasterNum = dr[0].ToString();
                }
                return T1MasterNum;
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
        }//end GetAffiliateRepNum


        //This function gets the Affiliate information - CALLED BY AffiliatesBL.GetAffiliateAddPartner
        public DataSet GetAffiliateAddPartner(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdAffiliate = new SqlCommand("sp_GetAffiliateAddPartner", Conn);
                cmdAffiliate.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmdAffiliate.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                pAffiliateID.Value = AffiliateID;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdAffiliate;
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
        }//end GetAffiliateAddPartner

        public void UploadPartnerLogo(int AffiliateID, bool DisplayLogo, string LogoFileName)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UploadPartnerLogo", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pDisplayLogo = cmd.Parameters.Add("@DisplayLogo", SqlDbType.VarChar);
                SqlParameter pLogoFileName = cmd.Parameters.Add("@LogoFileName", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;
                pDisplayLogo.Value = DisplayLogo;
                pLogoFileName.Value = LogoFileName;

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
        }


        //CALLED BY AffiliatesBL.InsertUpdateBankingInfo
        public bool InsertUpdateAffiliateBankingInfo(int AffiliateID, string BankName, string OtherBank, string Address, string ZipCode,
            string City, string State, string NameOnCheckingAcct, string AcctNum, string RoutingNum, string Phone)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdBankingInfo = new SqlCommand("sp_InsertUpdateBanking", Conn);
                cmdBankingInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pAddress = cmdBankingInfo.Parameters.Add("@BankAddress", SqlDbType.VarChar);
                SqlParameter pBankName = cmdBankingInfo.Parameters.Add("@BankName", SqlDbType.VarChar);
                SqlParameter pOtherBank = cmdBankingInfo.Parameters.Add("@OtherBank", SqlDbType.VarChar);                
                SqlParameter pZipCode = cmdBankingInfo.Parameters.Add("@BankZip", SqlDbType.VarChar);
                SqlParameter pBankCity = cmdBankingInfo.Parameters.Add("@BankCity", SqlDbType.VarChar);
                SqlParameter pBankState = cmdBankingInfo.Parameters.Add("@BankState", SqlDbType.VarChar);
                SqlParameter pBankAcctNum = cmdBankingInfo.Parameters.Add("@BankAccountNumber", SqlDbType.VarChar);
                SqlParameter pBankPhone = cmdBankingInfo.Parameters.Add("@BankPhone", SqlDbType.Char);
                SqlParameter pBankRoutingNumber = cmdBankingInfo.Parameters.Add("@BankRoutingNumber", SqlDbType.VarChar);
                SqlParameter pNameonCheckingAcct = cmdBankingInfo.Parameters.Add("@NameonCheckingAcct", SqlDbType.VarChar);
                SqlParameter pAffiliateID = cmdBankingInfo.Parameters.Add("@AffiliateID", SqlDbType.Int);

                pAffiliateID.Value = AffiliateID;
                pAddress.Value = Address;                
                pZipCode.Value = ZipCode;
                pBankName.Value = BankName;
                pOtherBank.Value = OtherBank;
                pBankCity.Value = City;
                pBankState.Value = State;                
                pBankAcctNum.Value = AcctNum;
                pBankRoutingNumber.Value = RoutingNum;
                pNameonCheckingAcct.Value = NameOnCheckingAcct;
                pBankPhone.Value = Phone;

                cmdBankingInfo.Connection.Open();
                cmdBankingInfo.ExecuteNonQuery();
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
        }//end function InsertUpdateAffiliateBankingInfo

       
        //This function updates Affilaite info in AffiliateWiz. CALLED BY AffiliatesBL.UpdateAffiliateWiz
        public bool UpdateAffiliateInfo(int AffiliateID, string FirstName, string LastName, 
            string PasswordPhrase, string DBA, string CheckPayable, string Email, string TaxSSN, string TaxID, string SSN, 
            string Address, string City, string State, string Region, string Zip, string Country, 
            string MailingAddress, string MailingCity, string MailingState, string MailingRegion, 
            string MailingZip, string MailingCountry, string Phone, string HomePhone, string MobilePhone,
            string Fax, string URL, string Comments, bool Notify, string LegalStatus, bool bDirectDeposit)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_UpdateAffiliate_2DBs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pPasswordPhrase = cmd.Parameters.Add("@PasswordPhrase", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pCheckPayable = cmd.Parameters.Add("@CheckPayable", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pTaxSSN = cmd.Parameters.Add("@TaxSSN", SqlDbType.VarChar);
                SqlParameter pTaxID = cmd.Parameters.Add("@FederalTaxID", SqlDbType.VarChar);
                SqlParameter pSSN = cmd.Parameters.Add("@SocialSecurity", SqlDbType.VarChar);
                SqlParameter pAddress = cmd.Parameters.Add("@CompanyAddress", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                //SqlParameter pRegion = cmd.Parameters.Add("@Region", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pMailingAddress = cmd.Parameters.Add("@MailingAddress", SqlDbType.VarChar);
                SqlParameter pMailingCity = cmd.Parameters.Add("@MailingCity", SqlDbType.VarChar);
                SqlParameter pMailingState = cmd.Parameters.Add("@MailingState", SqlDbType.VarChar);
                //SqlParameter pMailingRegion = cmd.Parameters.Add("@MailingRegion", SqlDbType.VarChar);
                SqlParameter pMailingZip = cmd.Parameters.Add("@MailingZipCode", SqlDbType.VarChar);
                SqlParameter pMailingCountry = cmd.Parameters.Add("@MailingCountry", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@BusinessPhone", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar);
                SqlParameter pFax = cmd.Parameters.Add("@Fax", SqlDbType.VarChar);
                SqlParameter pURL = cmd.Parameters.Add("@WebSiteURL", SqlDbType.VarChar);
                SqlParameter pComments = cmd.Parameters.Add("@Comments", SqlDbType.VarChar);
                SqlParameter pNotify = cmd.Parameters.Add("@Notify", SqlDbType.Bit);
                SqlParameter pLegalStatus = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar);
                SqlParameter pDirectDeposit = cmd.Parameters.Add("@DirectDeposit", SqlDbType.Bit);

                pAffiliateID.Value = AffiliateID;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pPasswordPhrase.Value = PasswordPhrase;
                pDBA.Value = DBA;
                pCheckPayable.Value = CheckPayable;
                pEmail.Value = Email;
                pTaxSSN.Value = TaxSSN;
                pTaxID.Value = TaxID;
                pSSN.Value = SSN;
                pAddress.Value = Address;
                pCity.Value = City;
                pState.Value = State;
                //pRegion.Value = Region;
                pZip.Value = Zip;
                pCountry.Value = Country;
                pMailingAddress.Value = MailingAddress;
                pMailingCity.Value = MailingCity;
                pMailingState.Value = MailingState;
                //pMailingRegion.Value = MailingRegion;
                pMailingZip.Value = MailingZip;
                pMailingCountry.Value = MailingCountry;
                pPhone.Value = Phone;
                pHomePhone.Value = HomePhone;
                pMobilePhone.Value = MobilePhone;
                pFax.Value = Fax;
                pURL.Value = URL;
                pComments.Value = Comments;
                pNotify.Value = Notify;
                pLegalStatus.Value = LegalStatus;
                pDirectDeposit.Value = bDirectDeposit;
                
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end function UpdateAffiliateWizInfo

        //This function returns noted information. CALLED BY AffiliatesBL.ReturnACTUserID
        public string GetACTUserID(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd= new SqlCommand("sp_GetActUserID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "tblaff_Affiliates");
                string ACTUserID = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ACTUserID = dr[0].ToString();
                }
                return ACTUserID;
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
        //end ReturnActUserID

        public DataSet GetConfirmationCommByRepMonPeriod(int AffiliateID, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmdRep = new SqlCommand("sp_GetConfirmationCommByRepMonPeriod", Conn);
                cmdRep.CommandTimeout = 600;
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                cmdRep.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdRep.Parameters.Add(new SqlParameter("@Period", Period));
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
        }//end GetConfirmationComm

        //CALLED BY commissions.aspx to display confirmation for Commissions
        public DataSet GetConfirmationComm(int AffiliateID, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmdRep = new SqlCommand("sp_GetConfirmationComm", Conn);
                cmdRep.CommandTimeout = 600;
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
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
        }//end GetConfirmationComm

        
        //This function gets the Rep List - CALLED By AffiliatesBL.GetAffiliateList
        public DataSet GetAffiliateList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAffiliateList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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
        }//end function ReturnPartnerList

        public DataSet GetSalesOfficeList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetSalesOfficeList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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
        }//end function ReturnPartnerList

        public DataSet GetAffiliateSalesOffice(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetAffiliateSalesOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
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
        }//end function ReturnPartnerList

        //This function gets the Rep List - CALLED By AffiliatesBL.GetNonRepList
        public DataSet GetNonRepList(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetNonRepList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                pMasterNum.Value = MasterNum;
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
        }//end function ReturnPartnerList

        public DataSet GetCommRefPaymentByAffiliateID(int AffiliateID, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_GetCommRefPaymentByAffID", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
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
        }//end GetCommRefPaymentByAffiliateID

        public DataSet GetResdCommPaymentByAffiliateID(int AffiliateID, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_GetResdCommPaymentByAffID", Conn);
                cmdRep.CommandTimeout = 200;
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                cmdRep.Parameters.Add(new SqlParameter("@Mon", Month));
                cmdRep.Parameters.Add(new SqlParameter("@Period", Period));
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
        }//end GetResdCommPaymentByAffiliateID


        public DataSet GetResidualPaymentByAffiliateID(int AffiliateID, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdRep = new SqlCommand("SP_GetResidualPaymentByAffID", Conn);
                cmdRep.CommandType = CommandType.StoredProcedure;
                cmdRep.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
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
        }//end GetResidualPaymentByAffiliateID

        public bool UpdateAffiliateLead(int AffiliateID, string FirstName, string LastName, string DBA,
            string CompanyName, string LegalStatus, string TaxSSN, string TaxID, string SSN, string Address,
            string City, string State, string Zip, string Country, string MailingAddress,
            string MailingCity, string MailingState, string MailingZip,
            string MailingCountry, string Phone, string HomePhone, string MobilePhone, string Fax,
            string URL, string Email, int ReferralID, string Specify)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateAffiliateLead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pDBA = cmd.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pCompanyName = cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar);
                SqlParameter pLegalStatus = cmd.Parameters.Add("@LegalStatus", SqlDbType.VarChar);
                SqlParameter pTaxSSN = cmd.Parameters.Add("@TaxSSN", SqlDbType.VarChar);
                SqlParameter pTaxID = cmd.Parameters.Add("@FederalTaxID", SqlDbType.VarChar);
                SqlParameter pSSN = cmd.Parameters.Add("@SocialSecurity", SqlDbType.VarChar);
                SqlParameter pAddress = cmd.Parameters.Add("@Address", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@Zip", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pMailingAddress = cmd.Parameters.Add("@MailingAddress", SqlDbType.VarChar);
                SqlParameter pMailingCity = cmd.Parameters.Add("@MailingCity", SqlDbType.VarChar);
                SqlParameter pMailingState = cmd.Parameters.Add("@MailingState", SqlDbType.VarChar);
                SqlParameter pMailingZip = cmd.Parameters.Add("@MailingZip", SqlDbType.VarChar);
                SqlParameter pMailingCountry = cmd.Parameters.Add("@MailingCountry", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@Phone", SqlDbType.VarChar);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar);
                SqlParameter pFax = cmd.Parameters.Add("@Fax", SqlDbType.VarChar);
                SqlParameter pURL = cmd.Parameters.Add("@URL", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pReferralID = cmd.Parameters.Add("@ReferralID", SqlDbType.SmallInt);
                SqlParameter pSpecify = cmd.Parameters.Add("@Specify", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;
                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pDBA.Value = DBA;
                pCompanyName.Value = CompanyName;
                pLegalStatus.Value = LegalStatus;
                pTaxSSN.Value = TaxSSN;
                pTaxID.Value = TaxID;
                pSSN.Value = SSN;
                pAddress.Value = Address;
                pCity.Value = City;
                pState.Value = State;
                pZip.Value = Zip;
                pCountry.Value = Country;
                pMailingAddress.Value = MailingAddress;
                pMailingCity.Value = MailingCity;
                pMailingState.Value = MailingState;
                pMailingZip.Value = MailingZip;
                pMailingCountry.Value = MailingCountry;
                pPhone.Value = Phone;
                pHomePhone.Value = HomePhone;
                pMobilePhone.Value = MobilePhone;
                pFax.Value = Fax;
                pURL.Value = URL;
                pEmail.Value = Email;
                pReferralID.Value = ReferralID;
                pSpecify.Value = Specify;

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
        }//end function UpdateAffiliateLead

        public bool updateAffiliateOffice(int AffiliateID, string OfficeID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateAffiliateOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);
                SqlParameter pOfficeID = cmd.Parameters.Add("@OfficeID", SqlDbType.SmallInt);

                //DBNull nullValue;

                pAffiliateID.Value = AffiliateID;
                if (OfficeID != "")
                {
                    pOfficeID.Value = OfficeID;
                }
                else {
                    pOfficeID.Value = DBNull.Value;
                }

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
        }//end function UpdateAffiliateLead

        //Updates the Default Rate Packages for an Affiliate. CALLED BY RepInfoBL.ChangeDefaultPackage, 
        //RepInfoBL.UpdatePartnerInfoCurrMon
        public bool UpdateDefaultPackage(int AffiliateID, int PID, int CPPID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateDefaultPackage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);
                SqlParameter pPID = cmd.Parameters.Add("@PackageID", SqlDbType.SmallInt);
                SqlParameter pCPPID = cmd.Parameters.Add("@CPPackageID", SqlDbType.SmallInt);

                pAffiliateID.Value = AffiliateID;
                pPID.Value = PID;
                pCPPID.Value = CPPID;

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
        }//end function UpdateDefaultPackage

     
    }   //END CLASS AFFILIATES
}
