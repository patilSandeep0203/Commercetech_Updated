using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{   
    public class PDFDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        private static string ConnStringACT = ConfigurationManager.AppSettings["ConnectionStringACT"].ToString();
        //This function returns Data based on AppId to populate PDF
        public DataSet GetACTDataForAppSQL(string sqlQuery, int AppId)
        {
            SqlConnection Conn = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdPDF = new SqlCommand(sqlQuery, Conn);
                cmdPDF.Connection.Open();
                cmdPDF.Parameters.Add(new SqlParameter("@AppId", AppId));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdPDF;
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
        }//end function ReturnData
        

        //This function gets data based on contact id for ACT via a search query
        public DataSet GetACTDataSQL(string sqlQuery, string ColumnName, string strValue)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                //cmdACT.Parameters.Add(new SqlParameter("@ColumnName", ColumnName));
                //cmdACT.Parameters.Add(new SqlParameter("@Value", strValue));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetACTDataSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetACTDataPDFSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetDataForACTPDF

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetCAPDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetCAPDFACTSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetPayvisionPDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetPayvisionPDFACTSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetStKittsPDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetStKittsPDFACTSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetLeasePDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetLeasePDFACTSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetGETIPDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetGETIPDFACTSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetAMIPDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetAMIPDFACTSQL

        //This function gets data based on contact id for creating pdfs from ACT
        public DataSet GetRoamPayPDFACTSQL(string sqlQuery, string ContactID)
        {
            SqlConnection ConnACT = new SqlConnection(ConnStringACT);
            try
            {
                SqlCommand cmdACT = new SqlCommand(sqlQuery, ConnACT);
                cmdACT.Connection.Open();
                cmdACT.Parameters.Add(new SqlParameter("@ContactID", ContactID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdACT;
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
                ConnACT.Close();
                ConnACT.Dispose();
            }
        }//end function GetRoamPayPDFACTSQL

    }//end class PDFDL
}


