using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class OnlineAppStatusDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();

        //Gets the Merchant Status List
        public DataSet GetStatusSearchList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetStatusSearchList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
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
        }//end 

        //Gets the list of all Merchant Statuses 
        public DataSet GetStatusList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd= new SqlCommand("sp_GetStatusList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
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
        }//end         

        //Gets the Gateway status lists
        public DataSet GetStatusListGW()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd= new SqlCommand("sp_GetStatusListGW", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
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
        }//end 

        //Gets the Merchant status for the Agent
        public DataSet GetStatusListAgent()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd= new SqlCommand("sp_GetStatusListAgent", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
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
        }//end 

        //Gets the Gift services Status for Agent
        public DataSet GetStatusListGiftAgent()
        {
            SqlConnection conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListGiftAgent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;
            }
            catch(SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Gets the Gift Services Status
        public DataSet GetStatusListGift()
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListGift", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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

        //Gets the Lease Status
        public DataSet GetStatusListLease()
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListLease", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;
            }
            catch (Exception sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Get Lease status for agents
        public DataSet GetStatusListLeaseAgent()
        {
            SqlConnection conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListLeaseAgent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;

            }catch(SqlException sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        
        //Gets the Merchant status for the Agent
        public DataSet GetStatusListGWAgent()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd= new SqlCommand("sp_GetStatusListGWAgent", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
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
        }//end 

        //Gets Cash Advance status
        public DataSet GetStatusListMerchFund()
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListMerchFund", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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

        //Get Cash Advance status for agents
        public DataSet GetStatusListMerchFundAgent()
        {
            SqlConnection conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListMerchFundAgent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                conn.Dispose();
                return ds;
            }
            catch (Exception sqlErr)
            {
                throw sqlErr;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Get payroll status
        public DataSet GetStatusListPayroll()
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListPayroll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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
      
        //Get payroll status for agents
        public DataSet GetStatusListPayrollAgent()
        {
            SqlConnection conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListPayrollAgent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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

        //Get check services status
        public DataSet GetStatusListCheckServ()
        {
            SqlConnection conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListCheckServ", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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

        //Get check services status for agents
        public DataSet GetStatusListCheckServAgent()
        {
            SqlConnection conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetStatusListCheckServAgent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
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

        //This function checks if regprogram info exists for AppId. CALLED BY OnlineAppStatusBL.CheckReprogram
        public DataSet CheckReprogramExists(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckReprogramSignup", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "OnlineAppReprogram");
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
        }//end CheckReprogramExists

        //This function returns platform information. CALLED BY OnlineAppStatusBL.GetPlatformInfo
        public DataSet GetPlatformData(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetPlatform", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
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
        }//end GetPlatformData

        //This function returns reprogram information. CALLED BY OnlineAppStatusBL.GetReprogramInfo
        public DataSet GetReprogramData(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetReprogram", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
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
        }//end GetReprogramData
        //Inserts a Note and returns the dataset containing the NoteID. 
        //CALLED BY ReminderBL.InsertNoteReminder, ExportActBL.ExportACTStatus, OnlineAppStatusBL.InsertNote
        public DataSet InsertNote(string ActUserID, int AppId, string NoteText, string CurrentTime)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdNotes = new SqlCommand("SP_InsertNote", Conn);
                cmdNotes.CommandType = CommandType.StoredProcedure;
                SqlParameter pActUserID = cmdNotes.Parameters.Add("@ActUserID", SqlDbType.VarChar);
                SqlParameter pNote = cmdNotes.Parameters.Add("@NoteText", SqlDbType.VarChar);
                SqlParameter pCurrentTime = cmdNotes.Parameters.Add("@DateRecorded", SqlDbType.DateTime);
                SqlParameter pAppId = cmdNotes.Parameters.Add("@AppId", SqlDbType.Int);

                pActUserID.Value = ActUserID;
                pNote.Value = NoteText;
                pCurrentTime.Value = CurrentTime;
                pAppId.Value = AppId;

                //cmdNotes.Connection.Open();
                //cmdNotes.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdNotes;
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
        }//end function InsertNoteInfo

        //Selects the Gateway ID of an Online App if the Gateway Signup exists
        public DataSet CheckGateway ( int AppId)
        {
   
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckGatewaySignup", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "OnlineAppGateway");
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
        }//end CheckGateway

        //This function returns noted information
        public DataSet CheckNBC(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckPrevNBCExists", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "OnlineAppNonBankCard");
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
        }//end CheckNBC

        //This function Inserts/Updates gateway info
        public bool InsertUpdateGatewayInfo( string GatewayUserID, string GatewayPassword, int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {               
                SqlCommand cmdGateway = new SqlCommand("sp_InsertUpdateGatewayInfo", Conn);
                cmdGateway.CommandType = CommandType.StoredProcedure;
                cmdGateway.Connection.Open();
                cmdGateway.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmdGateway.Parameters.Add(new SqlParameter("@GatewayUserID", GatewayUserID));
                cmdGateway.Parameters.Add(new SqlParameter("@GatewayPassword", GatewayPassword));

                cmdGateway.ExecuteNonQuery();
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
        }//end InsertUpdateGatewayInfo

        //This function Inserts/Updates gateway info
        public bool InsertUpdateNBCInfo( string DiscoverNum, string AmexNum, string JCBNum, string DinersNum, int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdNBC = new SqlCommand("sp_InsertUpdateNBCNumbers", Conn);
                cmdNBC.CommandType = CommandType.StoredProcedure;
                cmdNBC.Connection.Open();
                cmdNBC.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmdNBC.Parameters.Add(new SqlParameter("@DiscoverNum", DiscoverNum));
                cmdNBC.Parameters.Add(new SqlParameter("@AmexNum", AmexNum));
                cmdNBC.Parameters.Add(new SqlParameter("@JCBNum", JCBNum));
                cmdNBC.Parameters.Add(new SqlParameter("@DinersNum", DinersNum));

                cmdNBC.ExecuteNonQuery();
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
        }//end InsertUpdateNBCInfo

        public bool DeleteReprogramInfo( int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            string sqlQuery = "Delete from OnlineAppReprogram where AppID = @AppId";
            try
            {
                SqlCommand cmdReprogram = new SqlCommand(sqlQuery, Conn);
                cmdReprogram.Connection.Open();
                cmdReprogram.Parameters.Add(new SqlParameter("@AppId", AppId));

                cmdReprogram.ExecuteNonQuery();
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
        }//end DeleteReprogramInfo

        //CALLED BY OnlineAppStatusBL.DeleteNote
        public int DeleteNote(string NoteID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            string sqlQuery = "Delete FROM onlineappnotes WHERE NoteID = @NoteID";
            try
            {
                SqlCommand cmdNotes = new SqlCommand(sqlQuery, Conn);
                cmdNotes.Connection.Open();
                cmdNotes.Parameters.Add(new SqlParameter("@NoteID", NoteID));

                int iRetVal = cmdNotes.ExecuteNonQuery();
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
        }//end DeleteNote

        //CALLED BY OnlineAppStatusB
        public bool DeletePlatform(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            string sqlQuery = "Delete FROM OnlineAppPlatform WHERE AppID = @AppId";
            try
            {
                SqlCommand cmdNotes = new SqlCommand(sqlQuery, Conn);
                cmdNotes.Connection.Open();
                cmdNotes.Parameters.Add(new SqlParameter("@AppID", AppId));

                cmdNotes.ExecuteNonQuery();
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
        }//end DeleteNote

        //CALLED BY OnlineAppMgmt/Edit.aspx.cs 
        public DataSet GetPlatformList(string Processor)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetProcessorPlatform", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Processor", Processor));
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
        }//end GetGatewayList
    
    }//end class OnlineAppStatusDL
}
