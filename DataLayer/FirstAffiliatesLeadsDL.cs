using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class FirstAffiliatesLeadsDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        public DataSet GetAffiliateSignups(string SortBy)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetAffiliateSignups", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                cmdLeads.Parameters.Add(new SqlParameter("@SortBy", SortBy));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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
        }//end GetLeadsInfo

        public DataSet GetAffiliatesByLookup(string ColName, string ColValue)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetAffiliateByLookup",Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                cmdLeads.Parameters.Add(new SqlParameter("@ColName", ColName));
                cmdLeads.Parameters.Add(new SqlParameter("@ColValue", ColValue));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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

        public DataSet GetAffiliatesByEmail(string ColValue)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("SP_GetAffiliateByEmail", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                cmdLeads.Parameters.Add(new SqlParameter("@ColValue", ColValue));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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

        //Gets all the Free Report Leads (for Admin Only)
        public DataSet GetFreeReport()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetFreeReport", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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


        //Gets the Free Report Lead for a particular Lead
        public DataSet GetFreeReport(int LeadID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetFreeReportByID", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                cmdLeads.Parameters.Add(new SqlParameter("@LeadID", LeadID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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

        //Gets all the Free Report Leads (for Admin Only)
        public DataSet GetFreeApply()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetFreeApply", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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


        //Gets the Free Report Lead for a particular Lead
        public DataSet GetFreeApply(int LeadID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetFreeApplyByID", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                cmdLeads.Parameters.Add(new SqlParameter("@LeadID", LeadID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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

        //Gets all the Free Consult Leads (for Admin Only)
        public DataSet GetFreeConsult()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetFreeConsult", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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


        //Gets the Free Consult Lead for a particular Lead
        public DataSet GetFreeConsult(int LeadID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetFreeConsultByID", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Connection.Open();
                cmdLeads.Parameters.Add(new SqlParameter("@LeadID", LeadID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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

        //This function gets leads for agents. CALLED BY FirstAffiliatesLeadsBL.GetReportsAgent
        public DataSet GetLeadsPartner(string LeadType, string AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetLeadsPartner", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Parameters.Add(new SqlParameter("@LeadType", LeadType));
                cmdLeads.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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
        }//end GetLeadsInfoAgent

        
        //This function returns affiliate info including decrypted banking info. This data is used to insert/update act records from leads page
        public DataSet GetAffiliateData(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdLeads = new SqlCommand("sp_GetAffiliateActInfo", Conn);
                cmdLeads.CommandType = CommandType.StoredProcedure;
                cmdLeads.Parameters.Add(new SqlParameter("@AffiliateID", AffiliateID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdLeads;
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
        }//end GetAffiliateData

        //This function Creates online app for Free Report and Free Consult Lead Types
        public bool CreateLeadApp(string Email, string FirstName, string LastName, int ReferralID, string HomePhone,
            string Phone)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_CreateNewApp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pReferralID = cmd.Parameters.Add("@ReferralID", SqlDbType.SmallInt);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@BusPhone", SqlDbType.VarChar);

                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pEmail.Value = Email;
                pHomePhone.Value = HomePhone;
                pPhone.Value = Phone;
                pReferralID.Value = ReferralID;

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
        }//end function CreateLeadApp

        //This function Creates online app for Free Report and Free Consult Lead Types
        public bool CreateLeadAppExt(string Email, string FirstName, string LastName, int ReferralID,
            string HomePhone, string MobilePhone, string Phone, string Company, string Address, string City,
            string State, string Zip, string Country, string URL)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_CreateExtApp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pFirstName = cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
                SqlParameter pLastName = cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
                SqlParameter pEmail = cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                SqlParameter pReferralID = cmd.Parameters.Add("@ReferralID", SqlDbType.SmallInt);
                SqlParameter pHomePhone = cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar);
                SqlParameter pMobilePhone = cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar);
                SqlParameter pPhone = cmd.Parameters.Add("@BusPhone", SqlDbType.VarChar);
                SqlParameter pCompany = cmd.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pAddress = cmd.Parameters.Add("@Address", SqlDbType.VarChar);
                SqlParameter pCity = cmd.Parameters.Add("@City", SqlDbType.VarChar);
                SqlParameter pState = cmd.Parameters.Add("@State", SqlDbType.VarChar);
                SqlParameter pZip = cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar);
                SqlParameter pCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
                SqlParameter pURL = cmd.Parameters.Add("@website", SqlDbType.VarChar);

                pFirstName.Value = FirstName;
                pLastName.Value = LastName;
                pEmail.Value = Email;
                pHomePhone.Value = HomePhone;
                pPhone.Value = Phone;
                pReferralID.Value = ReferralID;
                pCompany.Value = Company;
                pCity.Value = City;
                pCountry.Value = Country;
                pState.Value = State;
                pZip.Value = Zip;
                pAddress.Value = Address;
                pURL.Value = URL;
                pMobilePhone.Value = MobilePhone;

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
        }//end function CreateLeadAppExt

        //This function deletes leads
        public bool DeleteLeadInfo(int LeadID, string ProcedureName)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {                
                SqlCommand cmd = new SqlCommand(ProcedureName, Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pLeadID = cmd.Parameters.Add("@LeadID", SqlDbType.SmallInt);

                pLeadID.Value = LeadID;
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
        }//end function DeleteLeadInfo

        //This function deletes leads
        public bool DeleteAffiliateInfo(int LeadID, string ProcedureName)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {                
                SqlCommand cmd = new SqlCommand(ProcedureName, Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pLeadID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);

                pLeadID.Value = LeadID;
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
        }//end function DeleteAffiliateInfo

       
        //This function updates the last sync date for the Affiliate whenever the affiliate lead is inserted or updated        
        public bool UpdateLastSync(int AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {                
                SqlCommand cmd = new SqlCommand("sp_UpdateAffiliateSync", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.SmallInt);

                pAffiliateID.Value = AffiliateID;

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
        }//end function UpdateLastSync

        //This function updates the Add To ACT date based on lead type
        public int UpdateAddToACTDate(int LeadID, string LeadType)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateLeadActDate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pLeadType = cmd.Parameters.Add("@LeadType", SqlDbType.VarChar);
                SqlParameter pLeadID = cmd.Parameters.Add("@LeadID", SqlDbType.SmallInt);

                pLeadID.Value = LeadID;
                pLeadType.Value = LeadType;

                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();

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
        }//end function UpdateAddToACTDate
        
    }//end class FirstAffiliatesLeadsDL
}
