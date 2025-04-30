using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class ProductsDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();

        //CALLED BY ProductsBL.GetCOG
        public DataSet GetProductsByCategory(string SortBy, string Type, string Category, bool Active)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetProductsByCategory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SortBy", SortBy));
                cmd.Parameters.Add(new SqlParameter("@Type", Type));
                cmd.Parameters.Add(new SqlParameter("@CategoryID", Category));
                cmd.Parameters.Add(new SqlParameter("@ActiveFlag", Active));
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
        }//end GetCogInfo

      

        //CALLED BY ProductsBL.GetProductRep
        public DataSet GetProductRep(string SortBy, string Type, string Category, bool Active, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdCOG = new SqlCommand("SP_GetProductsRep", Conn);
                cmdCOG.CommandType = CommandType.StoredProcedure;
                cmdCOG.Parameters.Add(new SqlParameter("@SortBy", SortBy));
                cmdCOG.Parameters.Add(new SqlParameter("@Type", Type));
                cmdCOG.Parameters.Add(new SqlParameter("@CategoryID", Category));
                cmdCOG.Parameters.Add(new SqlParameter("@ActiveFlag", Active));
                cmdCOG.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdCOG;
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
        }//end GetProductRep

        //CALLED BY ProductsBL.GetProductCategories
        public DataSet GetProductCategories()
        {
            string sqlQuery = "Select * from ProductCategory";
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCOG = new SqlCommand(sqlQuery, Conn);
                cmdCOG.Connection.Open();                
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdCOG;
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
        }//end GetProductCategories
        
        //CALLED BY ProductsBL.DeleteProductInfo
        public bool DeleteProduct(int Code)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string sqlQuery = "DELETE FROM Product WHERE ProductCode= @Code";
                SqlCommand cmdCOG = new SqlCommand(sqlQuery, Conn);
                cmdCOG.Connection.Open();
                cmdCOG.Parameters.Add(new SqlParameter("@Code", Code));
                cmdCOG.ExecuteNonQuery();
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
        }//end DeleteProduct

        
        //CALLED BY ProductsBL.UpdateProductInfo
        public int UpdateProduct(int Code, string Product, string Description, decimal COG, decimal SellPrice, 
            int Active, string Type, string CategoryID, int WebsiteDisplay, int ProstoresCode, string ImageName,
            string SellDescription, bool Cascade)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCOG = new SqlCommand("SP_UpdateProduct", Conn);
                cmdCOG.CommandType = CommandType.StoredProcedure;
                SqlParameter pCode = cmdCOG.Parameters.Add("@ProductCode", SqlDbType.Int);
                SqlParameter pProduct = cmdCOG.Parameters.Add("@ProductName", SqlDbType.VarChar);
                SqlParameter pDescription = cmdCOG.Parameters.Add("@Description", SqlDbType.VarChar);
                SqlParameter pCOG = cmdCOG.Parameters.Add("@COG", SqlDbType.Decimal);
                SqlParameter pSellPrice = cmdCOG.Parameters.Add("@SellPrice", SqlDbType.Decimal);
                SqlParameter pActive = cmdCOG.Parameters.Add("@Active", SqlDbType.Bit);
                SqlParameter pWebsiteDisplay = cmdCOG.Parameters.Add("@WebsiteDisplay", SqlDbType.Bit);
                SqlParameter pProstoresCode = cmdCOG.Parameters.Add("@ProstoresCode", SqlDbType.Int);
                SqlParameter pImageName = cmdCOG.Parameters.Add("@ImageName", SqlDbType.VarChar);
                SqlParameter pSellDescription = cmdCOG.Parameters.Add("@SellDescription", SqlDbType.VarChar);
                SqlParameter pType = cmdCOG.Parameters.Add("@Type", SqlDbType.VarChar);
                SqlParameter pCategoryID = cmdCOG.Parameters.Add("@CategoryID", SqlDbType.VarChar);
                SqlParameter pCascade = cmdCOG.Parameters.Add("@Cascade", SqlDbType.Bit);

                pCode.Value = Code;
                pProduct.Value = Product;
                pDescription.Value = Description;
                pCOG.Value = COG;
                pSellPrice.Value = SellPrice;
                pActive.Value = Active;
                pWebsiteDisplay.Value = WebsiteDisplay;
                pProstoresCode.Value = ProstoresCode;
                pImageName.Value = ImageName;
                pSellDescription.Value = SellDescription;
                pType.Value = Type;
                pCategoryID.Value = CategoryID;
                pCascade.Value = Cascade;

                cmdCOG.Connection.Open();
                int iRetVal = cmdCOG.ExecuteNonQuery();
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
        }//end function UpdateProduct

        //CALLED BY ProductsBL.InsertUpdateProductInfoRep
        public int UpdateProductRep(string MasterNum, string ProductCode, string RepPrice, string SellDescription)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdCOG = new SqlCommand("SP_InsertUpdateProductRep", Conn);
                cmdCOG.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdCOG.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pProductCode = cmdCOG.Parameters.Add("@ProductCode", SqlDbType.VarChar);
                SqlParameter pRepPrice = cmdCOG.Parameters.Add("@RepPrice", SqlDbType.VarChar);
                SqlParameter pSellDescription = cmdCOG.Parameters.Add("@SellDescription", SqlDbType.VarChar);

                pMasterNum.Value = MasterNum;
                pProductCode.Value = ProductCode;                
                pRepPrice.Value = RepPrice;
                pSellDescription.Value = SellDescription;

                cmdCOG.Connection.Open();
                int iRetVal = cmdCOG.ExecuteNonQuery();
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
        }//end function UpdateProductRep

        //CALLED BY ProductsBL.InsertProductInfo
        public int InsertProduct(string Product, string Description, decimal COG, decimal SellPrice, 
            string Type, string CategoryID, int WebsiteDisplay, int ProstoresCode, string ImageName, string SellDescription )
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdCOG = new SqlCommand("SP_InsertProduct", Conn);
                cmdCOG.CommandType = CommandType.StoredProcedure;
                SqlParameter pProduct = cmdCOG.Parameters.Add("@ProductName", SqlDbType.VarChar);
                SqlParameter pDescription = cmdCOG.Parameters.Add("@Description", SqlDbType.VarChar);
                SqlParameter pCOG = cmdCOG.Parameters.Add("@COG", SqlDbType.Decimal);
                SqlParameter pSellPrice = cmdCOG.Parameters.Add("@SellPrice", SqlDbType.Decimal);
                SqlParameter pSellDescription = cmdCOG.Parameters.Add("@SellDescription", SqlDbType.VarChar);
                SqlParameter pType = cmdCOG.Parameters.Add("@Type", SqlDbType.VarChar);
                SqlParameter pCategoryID = cmdCOG.Parameters.Add("@CategoryID", SqlDbType.VarChar);
                SqlParameter pWebsiteDisplay = cmdCOG.Parameters.Add("@WebsiteDisplay", SqlDbType.Bit);
                SqlParameter pProstoresCode = cmdCOG.Parameters.Add("@ProstoresCode", SqlDbType.Int);
                SqlParameter pImageName = cmdCOG.Parameters.Add("@ImageName", SqlDbType.VarChar);

                pProduct.Value = Product;
                pDescription.Value = Description;
                pCOG.Value = COG;
                pSellPrice.Value = SellPrice;
                pSellDescription.Value = SellDescription;
                pType.Value = Type;
                pCategoryID.Value = CategoryID;
                pWebsiteDisplay.Value = WebsiteDisplay;
                pProstoresCode.Value = ProstoresCode;
                pImageName.Value = ImageName;

                cmdCOG.Connection.Open();
                int iRetVal = cmdCOG.ExecuteNonQuery();
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
        }//end function InsertProduct

        //CALLED BY ProductsBL.InsertProductInfo
        public int ReturnNewProductID()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string sqlQuery = "Select Max(ProductCode) From Product";
                SqlCommand cmdCOG = new SqlCommand(sqlQuery, Conn);
                cmdCOG.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdCOG;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int ProductCode = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ProductCode = Convert.ToInt32(dr[0]);
                }
                return ProductCode;
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
        }//end ReturnProductID

        
        //This function gets the list of active products. CALLED BY SalesOppsBL.GetProducts
        public DataSet GetProducts()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetActiveProducts", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
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
        }//end ReturnProducts

        
    }//end class ProductsDL
}
