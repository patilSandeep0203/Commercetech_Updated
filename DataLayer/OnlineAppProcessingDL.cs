using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class OnlineAppProcessingDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
             
        public DataSet GetCPLow(string Processor)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetCPSwipedLow", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Processor", Processor));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "ProcessorBounds");
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
        }//end GetCPLow

        //CALLED BY OnlineappProcessing.GetAnnualFee        
        public DataSet GetAnnualFees( string Processor, string CardPresent)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAnnualFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@Processor", Processor));
                cmd.Parameters.Add(new SqlParameter("@CardPresent", CardPresent));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "ProcessorAnnualFee");
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
        }//end GetAnnualFees

        public DataSet GetSwipedPCT(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetOnlineAppSwipedPCT",Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "OnlineAppCardPCT");
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
        }//end GetSwipedPCT

        //CALLED BY OnlineAppProcessingBL.GetProcessorNames     
        public DataSet GetProcessorNames(string Swiped)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetProcessorNames", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SwipedPCT", Swiped));
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
        }//end GetProcessorList

        //CALLED BY OnlineAppProcessingBL.GetGateways
        public DataSet GetGatewayList(string Processor)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetGatewayNames", Conn);
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

        //CALLED BY OnlineAppProcessing.UpdateLastSyncDate
        public bool UpdateLastSyncDate(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "UPDATE OnlineAppProcessing SET LastSynchDate = GetDate() WHERE AppId = @AppId ";
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppID", AppId));
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
        }//end UpdateSyncDate

        //This function inserts/updates eCheck rates - CALLED BY setrates.aspx.cs
        public int ApplyeCheckRates(int AppID, string eSIRMonMin, string eSIRTransFee1,
                            string eSIRTransFee2, string eSIRTransFee3,
                            string eSIRTransFee4, string eSIRDiscountRate1,
                            string eSIRDiscountRate2, string eSIRDiscountRate3,
                            string eSIRDiscountRate4, string ePIRMonMin,
                            string ePIRTransFee1, string ePIRDiscountRate1)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUpdateECheckRates", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AppID", AppID));
                cmd.Parameters.Add(new SqlParameter("@eSIRMonMin", eSIRMonMin));
                cmd.Parameters.Add(new SqlParameter("@eSIRTransFee1", eSIRTransFee1));
                cmd.Parameters.Add(new SqlParameter("@eSIRTransFee2", eSIRTransFee2));
                cmd.Parameters.Add(new SqlParameter("@eSIRTransFee3", eSIRTransFee3));
                cmd.Parameters.Add(new SqlParameter("@eSIRTransFee4", eSIRTransFee4));
                cmd.Parameters.Add(new SqlParameter("@eSIRDiscountRate1", eSIRDiscountRate1));
                cmd.Parameters.Add(new SqlParameter("@eSIRDiscountRate2", eSIRDiscountRate2));
                cmd.Parameters.Add(new SqlParameter("@eSIRDiscountRate3", eSIRDiscountRate3));
                cmd.Parameters.Add(new SqlParameter("@eSIRDiscountRate4", eSIRDiscountRate4));
                cmd.Parameters.Add(new SqlParameter("@ePIRMonMin", ePIRMonMin));
                cmd.Parameters.Add(new SqlParameter("@ePIRTransFee1", ePIRTransFee1));
                cmd.Parameters.Add(new SqlParameter("@ePIRDiscountRate1", ePIRDiscountRate1));
                
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
        }//end function ApplyeCheckRates

        //This function Removes the Echeck Rates
        public bool RemoveECheckRates(int AppID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_RemoveOnlineAppECheckRates", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AppID", AppID));
          
                cmd.Connection.Open();
                bool RetVal = Convert.ToBoolean(cmd.ExecuteNonQuery());
                Conn.Close();
                Conn.Dispose();
                return RetVal;
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
        }//end function RemoveECheckRates


        public int UpdateOtherProcessing(int AppId, bool bInterchange, bool bAssessments, string RollingReserve)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOtherProcessing", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Parameters.Add(new SqlParameter("@Interchange", bInterchange));
                cmd.Parameters.Add(new SqlParameter("@Assessments", bAssessments));
                cmd.Parameters.Add(new SqlParameter("@RollingReserve", RollingReserve));
                cmd.Connection.Open();
                int iRetVal = cmd.ExecuteNonQuery();
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
        }//end UpdateInterchange

    }//end class OnlineAppProcessingDL
}
