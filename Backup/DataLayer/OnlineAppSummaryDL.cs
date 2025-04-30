using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class OnlineAppSummaryDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
         

        //CALLED BY OnlineAppSummaryBL.GetSummaryLookupSQL, OnlineAppSummaryBL.GetNewAppCount
        public DataSet GetNewAppIDs(string RepNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetNewApps", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add (new SqlParameter ("@RepNum", RepNum));
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
        }//end GetNewAppIDs

        public DataSet GetUnsyncAppbyRep(string RepNum) {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetOnlineAppUnsyncbyRep", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RepNum", RepNum));
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public DataSet GetNewUploadbyRep(string RepNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetNewUploadbyRep", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RepNum", RepNum));
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public int DeleteACTSalesOppID(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand Comm = new SqlCommand("sp_DeleteACTSalesOppID", Conn);
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Connection.Open();
                Comm.Parameters.Add(new SqlParameter("@AppId", AppId));
                int iRet = Comm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return iRet;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public DataSet GetOnlineAppSalesOppID (int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand Comm = new SqlCommand("sp_GetOnlineAppSalesOppID", Conn);
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Connection.Open();
                Comm.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter Adapter = new SqlDataAdapter();
                Adapter.SelectCommand = Comm;
                DataSet ds = new DataSet();
                Adapter.Fill(ds);
                return ds;
                
            }catch(SqlException sqlErr)
            {
                throw sqlErr;
            }finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public int updateUnlinkedSalesOpp(int AppId, string ID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand Comm = new SqlCommand("SP_INSERTUPDATEOnlineAppACTSalesOppID", Conn);
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Connection.Open();
                Comm.Parameters.Add(new SqlParameter("@AppID", AppId));
                Comm.Parameters.Add(new SqlParameter("@ID", ID));
                int iRet = Comm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return iRet;
            }
            catch (Exception sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public int unlinkedSalesOpps(int AppId, string ID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand Comm = new SqlCommand("SP_GetUnlinkedSalesOpps", Conn);
                Conn.Open();
                Comm.CommandType = CommandType.StoredProcedure;
                Comm.Parameters.Add(new SqlParameter("@AppId", AppId));
                Comm.Parameters.Add(new SqlParameter("@ID", ID));
                int retVal = Comm.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return retVal;
            }
            catch (Exception sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public DataSet GetRepInfoByRepName(string RepName)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetRepInfoByRepName", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepName = cmdResd.Parameters.Add("@RepName", SqlDbType.VarChar);
                pRepName.Value = RepName;

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
        }//end function GetRepInfo

    }//end class SummaryView
}
