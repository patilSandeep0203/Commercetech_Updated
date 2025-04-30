using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class PartnerLogDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string eSecurityConnString = ConfigurationManager.AppSettings["eSecurityConnectString"].ToString();
        //CALLED BY PartnerLogBL.GetLogData
        public DataSet GetLogData(string Action, int AppId)
        {
            string strQuery = "";
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            { 
                switch (Action)
                {
                    case "ALL":
                        strQuery = "Select * FROM VW_OnlineAppLog WHERE AppId=@AppID Order By RecordedDate DESC";
                        break;
                    case "Merchant":
                        strQuery = "Select * FROM VW_OnlineAppLog WHERE AppId=@AppID AND Portal = 0 Order By RecordedDate DESC";
                        break;
                    case "Partner":
                        strQuery = "Select * FROM VW_OnlineAppLog WHERE AppId=@AppID AND Portal = 1 Order By RecordedDate DESC";
                        break;
                }
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
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
        }//end GetLogData

        public DataSet GetLogForRates()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select * FROM VW_PackageLog";
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
        }//end GetLogForRates

        public bool DeleteLogInfo(int LogID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "DELETE FROM OnlineAppLog WHERE LogID = @LogID";
          
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@LogID", LogID));
                cmd.ExecuteNonQuery();
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
        }//end DeleteLogInfo

        public string ReturnPortalUserID(int AffiliateID)
        {
            //string PortalUserID="";
            SqlConnection Conn = new SqlConnection(eSecurityConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ReturnPortalUserIDbyAffiliateID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.Int);
                pAffiliateID.Value = AffiliateID;
                SqlParameter pPortalUserID = cmd.Parameters.Add("@PortalUserID", SqlDbType.VarChar);
                pPortalUserID.Size = 64; 
                pPortalUserID.Direction = ParameterDirection.Output; 

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                string PortalUserID = (string)cmd.Parameters["@PortalUserID"].Value;
                return PortalUserID;
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
        }//end InsertLog

        public bool InsertLog(int AppId, int AffiliateID, string Action)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertOnlineAppLog", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppID = cmd.Parameters.Add("@AppID", SqlDbType.VarChar);
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pAction = cmd.Parameters.Add("@Action", SqlDbType.VarChar);

                pAppID.Value = AppId;
                pAffiliateID.Value = AffiliateID;
                pAction.Value = Action;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end InsertLog

        public bool InsertPackageLog(int PackageID, int AffiliateID, string Action)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertPackageLog", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pPackageID = cmd.Parameters.Add("@PackageID", SqlDbType.VarChar);
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pAction = cmd.Parameters.Add("@Action", SqlDbType.VarChar);

                pPackageID.Value = PackageID;
                pAffiliateID.Value = AffiliateID;
                pAction.Value = Action;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end InsertPackageLog

        public bool InsertPartnerLog(int AffiliateID, string Action)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertPartnerLog", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAffiliateID = cmd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                SqlParameter pAction = cmd.Parameters.Add("@Action", SqlDbType.VarChar);

                pAffiliateID.Value = AffiliateID;
                pAction.Value = Action;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
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
        }//end InsertPackageLog

    }//end class PartnerLogDL
}
