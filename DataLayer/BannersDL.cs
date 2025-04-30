using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class BannersDL
    {
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();
        //CALLED BY BannersBL.GetGroups
        public DataSet ReturnGroups()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdBanners = new SqlCommand("SP_GetGroups", Conn);
                cmdBanners.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdBanners;
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
        }//end function ReturnGroups

        //CALLED BY BannersBL.GetBannersByGroupID
        public DataSet ReturnBannersByGroupID(int GroupID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdBanners = new SqlCommand("SP_GetBannersByGroupID", Conn);
                cmdBanners.CommandType = CommandType.StoredProcedure;
                cmdBanners.Parameters.Add(new SqlParameter("@GroupID", GroupID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdBanners;
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
        }//end function ReturnBannersByGroupID

        //CALLED BY BannersBL.GetGroupsByGroupID
        public DataSet ReturnTextLinksByGroupID(int GroupID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdBanners = new SqlCommand("SP_GetGroupsByGroupID", Conn);
                cmdBanners.CommandType = CommandType.StoredProcedure;
                cmdBanners.Parameters.Add(new SqlParameter("@GroupID", GroupID));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdBanners;
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
        }//end function ReturnTextLinksByGroupID
    }//End Class BannersDL
}//end NameSpace DLPartner
