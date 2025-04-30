using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;


namespace BusinessLayer
{
    public class ListBL
    {
        //CALLED BY Home.aspx, CreatePackage.aspx, OnlineAppMgmt/default.aspx, Edit.aspx, CTCSalesOpps.aspx
        public DataSet GetSalesRepList()
        {
            RepInfoDL SalesRepList = new RepInfoDL();
            DataSet ds = SalesRepList.GetSalesRepList();
            return ds;
        }//end function GetSalesRepList

        //CALLED BY ManagePartners.aspx
        public DataSet GetT1RepList()
        {
            RepInfoDL SalesRepList = new RepInfoDL();
            DataSet ds = SalesRepList.GetT1RepList();
            return ds;
        }//end function GetT1RepList

        //This function returns rep list
        public DataSet GetPartnerList()
        {
            RepInfoDL AgentInfo = new RepInfoDL();
            DataSet ds = AgentInfo.GetPartnerList();

            return ds;
        }

        //This function returns Rep Category list - CALLED BY AddPartner.aspx, UpdatePartner.aspx
        public DataSet GetRepCatList()
        {

            RepInfoDL RepCat = new RepInfoDL();
            DataSet ds = RepCat.GetRepCatList();
            return ds;
        }//end function GetRepCatList

        //Gets Rep List for Admins. CALLED BY ALL RESIDUAL REPORTS
        public DataSet GetRepListForVendor(string Vendor)
        {
            ResidualsDL RepList = new ResidualsDL();
            DataSet ds = RepList.GetRepListForVendor(Vendor);
            return ds;
        }//end function GetRepListForVendor

        //Gets Rep List for Tier 1 Agents. CALLED BY ALL RESIDUAL REPORTS
        public DataSet GetRepListForVendorByTier(string Vendor, string MasterNum)
        {
            ResidualsDL RepList = new ResidualsDL();
            DataSet ds = RepList.GetRepListForVendorByTier(Vendor, MasterNum);
            return ds;
        }//end function GetRepListForVendorByTier

    }
}
