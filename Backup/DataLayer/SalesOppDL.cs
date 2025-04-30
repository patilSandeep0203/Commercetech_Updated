using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class SalesOppDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringACT = ConfigurationManager.AppSettings["ConnectionStringACT"].ToString();
        //This function adds sales opp info. CALLED BY SalesOppsBL.AddSalesOpp
        public bool AddSalesOppInfo(string Code, string SellPrice, string COG, string Quantity, string ACTUserID,
            int AppId, string RepNum, string PaymentMethod, string Reprogram)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_InsertSalesOpp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);
                SqlParameter pPrice = cmd.Parameters.Add("@Price", SqlDbType.SmallMoney);
                SqlParameter pQuantity = cmd.Parameters.Add("@Quantity", SqlDbType.TinyInt);
                SqlParameter pActUserID = cmd.Parameters.Add("@ActUserID", SqlDbType.VarChar);
                SqlParameter pCode = cmd.Parameters.Add("@Code", SqlDbType.SmallInt);
                SqlParameter pCOG = cmd.Parameters.Add("@COG", SqlDbType.SmallMoney);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pPaymentMethod = cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar);
                SqlParameter pReprogram = cmd.Parameters.Add("@Reprogram", SqlDbType.VarChar);

                pAppId.Value = AppId;
                pPrice.Value = SellPrice;
                pCOG.Value = COG;
                pCode.Value = Code;
                pActUserID.Value = ACTUserID;
                pQuantity.Value = Quantity;
                pRepNum.Value = RepNum;
                pPaymentMethod.Value = PaymentMethod;
                pReprogram.Value = Reprogram;

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
        }//end AddSalesOppInfo

        public int SalesOppAddedToACT(string ID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_CheckSalesOppExists", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
                //SqlParameter retval = cmd.Parameters.Add("@Ret", SqlDbType.VarChar);
                //retval.Direction = ParameterDirection.ReturnValue;

                //int retunvalue = 1;
                  //int retunvalue = (int)cmd.Parameters["@Ret"].Value;
                  
                cmd.Connection.Open();
                //cmd.ExecuteNonQuery();
                int retunvalue = (int)cmd.ExecuteScalar();
                return retunvalue;
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
        //This function edits sales opp info. CALLED BY SalesOppsBL.EditSalesOpp
        public bool EditSalesOpp(int productCode, string SellPrice, string COG, string Quantity, string PaymentMethod, string Reprogram,
            string RepNum, string ID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateSalesOpp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pproductCode = cmd.Parameters.Add("@productCode", SqlDbType.SmallInt);
                SqlParameter pCOG = cmd.Parameters.Add("@COG", SqlDbType.SmallMoney);
                SqlParameter pPrice = cmd.Parameters.Add("@Price", SqlDbType.SmallMoney);
                SqlParameter pQuantity = cmd.Parameters.Add("@Quantity", SqlDbType.TinyInt);
                SqlParameter pPaymentMethod = cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar);
                SqlParameter pReprogram = cmd.Parameters.Add("@Reprogram", SqlDbType.VarChar);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pID = cmd.Parameters.Add("@ID", SqlDbType.VarChar);

                pproductCode.Value = productCode;
                pCOG.Value = COG;
                pPrice.Value = SellPrice;
                pQuantity.Value = Quantity;
                pPaymentMethod.Value = PaymentMethod;
                pReprogram.Value = Reprogram;
                pRepNum.Value = RepNum;
                pID.Value = ID;

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
        }//end EditSalesOppInfo

        public bool UpdateIsAddedACTFalse(string ID)
        {
            string sqlQuery = "Update OnlineAppSalesOpps SET IsAddedACT = 0 WHERE ID=@ID";
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
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
        }

        public bool UpdateIsAddedACTTrue(string ID)
        {
            string sqlQuery = "Update OnlineAppSalesOpps SET IsAddedACT = 1 WHERE ID=@ID";
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
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
        }

        //This function Deletes Sales Opp. CALLED BY SalesOppsBL.DeleteSalesOpp
        public bool DeleteSalesOpp(string ID)
        {
            string strQuery = "DELETE FROM OnlineAppSalesOpps WHERE ID = @ID";
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand(strQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@ID", ID));

                cmd.ExecuteNonQuery();
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
        }//end DeleteSalesOpp

        //This function updates last synch date for unlinked sales opps with sync date = null
        //CALLED BY ExportActBL.ExportActStatus
        public bool UpdateLastSynchForUnlinkedOpps(int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateUnlinkedSalesOpps", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);

                pAppId.Value = AppId;

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
        }//end UpdateLastSynchForUnlinkedOpps

       
        public DataSet GetSalesOppInfo (int AppId, string ID)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            string sqlQuery = "Select * from VW_OnlineAppSalesOpps where AppID = @AppID AND ID = @ID"; 
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@AppId", AppId));
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
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
                Conn.Close();
                Conn.Dispose();
            }
        }//end GetEquipmentInfo

        //Updates Sales Opp info for a given ID via a specified SQL query
        public bool UpdateSalesOppInfo(string sqlQuery, string ID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.Connection.Open();
                cmd.Parameters.Add(new SqlParameter("@ID", ID));
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
        }//end UpdateSalesOppInfo

        //CALLED BY ExportActBL.ExportData, ExportActBL.ExportActStatus
        public bool InsertUpdateSalesOpps(string AppId, string Code, string UnitPrice, string UnitCost,
            string Quantity, string TerminalID, string SerialNo, string Status, string CreateUserID,
            string ID, string RepNum, string Stage, string LastModified, string CreateDate, string Reprogram, string PaymentMethod)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {                
                SqlCommand cmd = new SqlCommand("SP_InsertUpdateExportedSalesOpp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pCode = cmd.Parameters.Add("@Code", SqlDbType.VarChar);
                SqlParameter pUnitPrice = cmd.Parameters.Add("@Price", SqlDbType.SmallMoney);
                SqlParameter pUnitCost = cmd.Parameters.Add("@COG", SqlDbType.SmallMoney);
                SqlParameter pQuantity = cmd.Parameters.Add("@Quantity", SqlDbType.VarChar);
                SqlParameter pTerminalID = cmd.Parameters.Add("@TerminalID", SqlDbType.VarChar);
                SqlParameter pSerialNo = cmd.Parameters.Add("@SerialNumber", SqlDbType.VarChar);
                SqlParameter pStatus = cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                SqlParameter pCreateUserID = cmd.Parameters.Add("@ActUserID", SqlDbType.VarChar);
                SqlParameter pID = cmd.Parameters.Add("@ID", SqlDbType.VarChar);
                SqlParameter pAppId = cmd.Parameters.Add("@AppId", SqlDbType.Int);
                SqlParameter pRepNum = cmd.Parameters.Add("@RepNum", SqlDbType.VarChar);
                SqlParameter pStage = cmd.Parameters.Add("@Stage", SqlDbType.VarChar);
                SqlParameter pLastModified = cmd.Parameters.Add("@LastModified", SqlDbType.DateTime);
                SqlParameter pCreateDate = cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime);
                SqlParameter pReprogram = cmd.Parameters.Add("@Reprogram", SqlDbType.VarChar);
                SqlParameter pPaymentMethod = cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar);

                pCode.Value = Code;
                pUnitPrice.Value = UnitPrice;
                pUnitCost.Value = UnitCost;
                pQuantity.Value = Quantity;
                pTerminalID.Value = TerminalID;
                pSerialNo.Value = SerialNo;
                pStatus.Value = Status;
                pCreateUserID.Value = CreateUserID;
                pID.Value = ID;
                pAppId.Value = AppId;
                pRepNum.Value = RepNum;
                pStage.Value = Stage;
                pLastModified.Value = LastModified;
                pCreateDate.Value = CreateDate;
                pReprogram.Value = Reprogram;
                pPaymentMethod.Value = PaymentMethod;

                cmd.Connection.Open();
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
            
        }//end function InsertUpdateSalesOpps

    }//end class SalesOppDL
}
