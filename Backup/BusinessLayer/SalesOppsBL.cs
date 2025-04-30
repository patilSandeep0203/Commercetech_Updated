using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class SalesOppsBL
    {
        private OnlineAppSalesOppsTableAdapter _OnlineAppSalesOppsAdapter = null;
        protected OnlineAppSalesOppsTableAdapter OnlineAppSalesOppsAdapter
        {
            get
            {
                if (_OnlineAppSalesOppsAdapter == null)
                    _OnlineAppSalesOppsAdapter = new OnlineAppSalesOppsTableAdapter();

                return _OnlineAppSalesOppsAdapter;
            }
        }
      
        //This function returns sales opps. CALLED BY Edit.aspx
        public PartnerDS.OnlineAppSalesOppsDataTable GetSalesOpps(int AppId)
        {
            return OnlineAppSalesOppsAdapter.GetData(Convert.ToInt16(AppId));          
        }//end function GetSalesOpps


        //This function deletes Sales Opp. CALLED BY Edit.aspx
        public bool DeleteSalesOpp(string ID)
        {
            SalesOppDL DeleteOpp = new SalesOppDL();
            bool retVal = DeleteOpp.DeleteSalesOpp(ID);
            return retVal;
        }//end delete sales opp

        //This function returns list of products. CALLED BY Edit.aspx
        public DataSet GetProducts()
        {
            ProductsDL Products = new ProductsDL();
            DataSet ds = Products.GetProducts();
            return ds;
        }

        //This function adds sales opp. CALLED BY Edit.aspx
        public bool AddSalesOpp(string Code, string SellPrice, string COG, string Quantity, 
            string ACTUserID, int AppId, string SubTotal, string RepNum, 
            string RepName, string PaymentMethod, string Reprogram)
        {
            SalesOppDL AddOpp = new SalesOppDL();
            bool retVal = AddOpp.AddSalesOppInfo(Code, SellPrice, COG, Quantity, ACTUserID,
                AppId, RepNum, PaymentMethod, Reprogram);
            return retVal;
        }//end AddSalesOpp

        //This function edits sales opp. CALLED BY Edit.aspx
        public bool EditSalesOpp(string productCode, string SellPrice, string COG, string Quantity, string PaymentMethod, string Reprogram,
            string RepNum, string ID)
        {
            SalesOppDL SalesOpp = new SalesOppDL();
            bool retVal = SalesOpp.EditSalesOpp(Convert.ToInt32(productCode), SellPrice, COG, Quantity, PaymentMethod, Reprogram, RepNum, ID);
            if (retVal)
            {
                retVal = SalesOpp.UpdateIsAddedACTFalse(ID);
            }
            return retVal;
        }//end EditSalesOpp

        //This function adds sales opps to ACT from the partner portal.
        //CALLED BY Edit.aspx
        public bool InsertSalesOppsInACT(int AppID, string ID)
        {
            //Get ContactID that was inserted in TBL_CONTACT in ACT
            ACTDataDL ACT = new ACTDataDL();
            string ContactID = ACT.ReturnContactID(AppID);
            
            if (ContactID == "")
                return false;

            //Add Sales Opps Into ACT
            SalesOppDL SalesOpp = new SalesOppDL();

            PartnerDS.OnlineAppSalesOppsDataTable dt = OnlineAppSalesOppsAdapter.GetDataByOppID(new Guid(ID));
            if (dt.Rows.Count > 0)
            {         
                ACT.InsertSalesOpp(dt[0].ID.ToString().Trim(),
                    ContactID, dt[0].ActUserID.ToString().Trim(), dt[0].Product.ToString().Trim(),
                    dt[0].ProductCode.ToString().Trim(), dt[0].Price.ToString().Trim(), dt[0].CostOfGoods.ToString().Trim(),
                    dt[0].Quantity.ToString().Trim(), dt[0].Subtotal.ToString().Trim(),
                    dt[0].RepName.ToString().Trim(), dt[0].LastModified.ToString().Trim(),
                    dt[0].CreateDate.ToString().Trim(), dt[0].PaymentMethod.ToString().Trim(), dt[0].Reprogram.ToString().Trim());
                
                SalesOpp.UpdateIsAddedACTTrue(dt[0].ID.ToString().Trim());

                //Insert note stating record for updated
                //ACTDataDL InsertNotes = new ACTDataDL();
                //InsertNotes.AddUpdateNoteToACT(ContactID, "Sales Opportunity (" + dt[0].Product.ToString().Trim() + ") added from the Partner Portal");
                

                return true;
            }//end if count not 0
            return false;
        }//end function InsertSalesOppsInACT


    }//end class SalesOppsInfo
}
