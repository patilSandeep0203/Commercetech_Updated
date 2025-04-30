using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class ReferralsDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        
        //CALLED BY ReferralsBL.GetReferralsDetail
        public DataSet GetReferrals(string ReferralID, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdReferrals = new SqlCommand("SP_GetReferralsByRefMon", Conn);
                cmdReferrals.CommandType = CommandType.StoredProcedure;
                SqlParameter pReferralID = cmdReferrals.Parameters.Add("@ReferralID", SqlDbType.VarChar);
                SqlParameter pMonth = cmdReferrals.Parameters.Add("@Mon", SqlDbType.VarChar);
                pReferralID.Value = ReferralID;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdReferrals;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "CommissionsByRep");
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
        }//end function GetReferrals

        public DataSet GetReferralsbyMonPeriod(string ReferralID, string Month, int Period)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdReferrals = new SqlCommand("SP_GetReferralsByRefMonPeriod", Conn);
                cmdReferrals.CommandType = CommandType.StoredProcedure;
                SqlParameter pReferralID = cmdReferrals.Parameters.Add("@ReferralID", SqlDbType.VarChar);
                SqlParameter pMonth = cmdReferrals.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pPeriod = cmdReferrals.Parameters.Add("@Period", Period);
                pReferralID.Value = ReferralID;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdReferrals;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "CommissionsByRep");
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
        }//end function GetReferrals


        //CALLED BY ReferralsBL.GetReferralsDetail
        public DataSet GetT1Referrals(string ReferralID, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdReferrals = new SqlCommand("SP_GetT1ReferralsByRefMon", Conn);
                cmdReferrals.CommandType = CommandType.StoredProcedure;
                SqlParameter pReferralID = cmdReferrals.Parameters.Add("@T1ReferralID", SqlDbType.VarChar);
                SqlParameter pMonth = cmdReferrals.Parameters.Add("@Mon", SqlDbType.VarChar);
                pReferralID.Value = ReferralID;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdReferrals;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "CommissionsByRep");
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
        }//end function GetT1Referrals

        public DataSet ReturnReferralsByCompany(string Company, string Month)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdReferrals = new SqlCommand("SP_GetReferralsByCompany", Conn);
                cmdReferrals.CommandType = CommandType.StoredProcedure;
                SqlParameter pCompany = cmdReferrals.Parameters.Add("@Company", SqlDbType.VarChar);
                SqlParameter pMonth = cmdReferrals.Parameters.Add("@Mon", SqlDbType.VarChar);
                pCompany.Value = Company;
                pMonth.Value = Month;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdReferrals;
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
        }//end function ReturnReferralsByCompany

        //This function returns comm detail based on comm id
        public DataSet ReturnReferralDetailFromID(string CommissionID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetCommissionsByCommID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CommissionID", CommissionID));
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
        }//end function ReturnReferralDetailFromID

        public bool UpdateReferrals(string RefTotal, string CommissionID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdReferral = new SqlCommand("sp_UpdateReferralTotal", Conn);
                cmdReferral.CommandType = CommandType.StoredProcedure;
                cmdReferral.Parameters.Add(new SqlParameter("@RefTotal", RefTotal));
                cmdReferral.Parameters.Add(new SqlParameter("@CommissionID", CommissionID));
                cmdReferral.Connection.Open();
                cmdReferral.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
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
        }//end function UpdateReferrals

        //Get List of Referrals
        public DataSet GetReferralList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCommonInfo = new SqlCommand("sp_GetReferralList", Conn);
                cmdCommonInfo.CommandType = CommandType.StoredProcedure;
                cmdCommonInfo.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdCommonInfo;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
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
    }//end class ReferralsDL
}
