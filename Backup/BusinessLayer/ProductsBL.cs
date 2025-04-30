using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class ProductsBL
    {
        private ProductInfoTableAdapter _ProductInfoAdapter = null;
        protected ProductInfoTableAdapter ProductInfoAdapter
        {
            get
            {
                if (_ProductInfoAdapter == null)
                    _ProductInfoAdapter = new ProductInfoTableAdapter();

                return _ProductInfoAdapter;
            }
        }

        //This function returns list of equipment. CALLED BY Cog.aspx
        public DataSet GetProductsByCategory(string SortBy, string Type, string Category, bool Active)
        {
            ProductsDL EquipmentList = new ProductsDL();
            DataSet ds = EquipmentList.GetProductsByCategory(SortBy, Type, Category, Active);
            return ds;
        }//end function GetCOG


        //This function gets the Products based on MasterNum. CALLED BY Cog.aspx
        public DataSet GetProductsRep(string SortBy, string Type, string Category, bool Active, string MasterNum)
        {
            ProductsDL EquipmentList = new ProductsDL();
            DataSet ds = EquipmentList.GetProductRep(SortBy, Type, Category, Active, MasterNum);
            return ds;
        }//end function GetProductRep

        //This function returns Product Information. CALLED BY cog.aspx
        public PartnerDS.ProductInfoDataTable GetProductInfo(int ProductCode)
        {
            return ProductInfoAdapter.GetData(Convert.ToInt16(ProductCode) );
        }//end function GetProductInfo


        //This function inserts product in Agent Portal database - CALLED BY cog.aspx
        public int InsertProductInfo(string Product, string Description, decimal COG, decimal SellPrice,
            string Type, string CategoryID, int WebsiteDisplay, int ProstoresCode, string ImageName, string SellDescription)
        {
            ProductsDL EquipmentList = new ProductsDL();
            int iRetVal = EquipmentList.InsertProduct(Product, Description, COG, SellPrice, Type, CategoryID, 
                WebsiteDisplay, ProstoresCode, ImageName, SellDescription);

            ProductsDL ProdID = new ProductsDL();
            int ProductCode = ProdID.ReturnNewProductID();
         
             if (ProductCode > 1)
                    iRetVal = InsertUpdateProductInfoACT(ProductCode, Product, Description, COG, SellPrice);
            
            return iRetVal;
        }//end function InsertProductInfo

        //This function inserts product in ACT! CALLED BY ProductsBL.InsertProductInfo
        public int InsertUpdateProductInfoACT(int ProductCode, string Product, string Desc, decimal COG, decimal Price)
        {
            ACTDataDL Equipment = new ACTDataDL();
            int iRetVal = Equipment.InsertUpdateProductACT(ProductCode, Product, Desc, COG, Price);
            return iRetVal;
        }//end function InsertProductInfoACT

        //This function returns updates product info. CALLED BY Cog.aspx
        public int UpdateProductInfo(int Code, string ProductName, string Description, decimal COG, decimal SellPrice, 
            int Active, string Type, string CategoryID, int WebsiteDisplay, int ProstoresCode, string ImageName,
            string SellDescription, bool Cascade)
        {
            ProductsDL Prod = new ProductsDL();
            ACTDataDL Act = new ACTDataDL();
            int iRetVal = Prod.UpdateProduct(Code, ProductName, Description, COG, SellPrice, 
                Active, Type, CategoryID, WebsiteDisplay, ProstoresCode, ImageName, SellDescription, Cascade);
            //if product is to be de-activated
            /*if (Active == 0)            
                 Act.DeleteProductACT(Code);            
            else //also Update Product in ACT*/
                iRetVal = Act.InsertUpdateProductACT(Code, ProductName, Description, COG, SellPrice);
          
            return iRetVal;
        }//end function UpdateProductInfo

        //This function inserts/updates product info for reps. CALLED BY Cog.aspx
        public int InsertUpdateProductInfoRep(string MasterNum, string ProductCode, string RepPrice, string SellDescription)
        {
            ProductsDL EquipmentList = new ProductsDL();
            int iRetVal = EquipmentList.UpdateProductRep(MasterNum, ProductCode, RepPrice, SellDescription);            
            return iRetVal;
        }//end function InsertUpdateProductInfoRep

        //This function gets product categories - CALLED BY cog.aspx
        public DataSet GetProductCategories()
        {
            ProductsDL Cat = new ProductsDL();
            DataSet ds = Cat.GetProductCategories();
            return ds;
        }//end function GetProductCategories

        //Deletes a Product from Product Table and In ACT
        public bool DeleteProduct(int Code)
        {
            //First delete in ACT
         
            ACTDataDL ACT = new ACTDataDL();
            bool retVal = ACT.DeleteProductACT(Code);

            //if Product deleted from ACT
            if (retVal)
            { 
                ProductsDL Equipment = new ProductsDL();
                retVal = Equipment.DeleteProduct(Code);
            }
            return retVal;
        }//end function DeleteProductInfo
    }//end class ProductsBL
}
