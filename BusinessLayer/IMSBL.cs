using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    class IMSBL
    {
        private string IMSNum = "";
        private string Month = "";
        public IMSBL(string IMSNum, string Month)
        {
            this.IMSNum = IMSNum;
            this.Month = Month;
            if (Month == "ALL")
                this.Month = "ALL";
        }

        //This function returns IMS residuals
        public DataSet GetIMSResiduals()
        {
            ResidualsDL IMSResd = new ResidualsDL();
            DataSet ds = IMSResd.GetIMSResiduals(IMSNum, Month);
            return ds;
        }//end function GetIMSResiduals



        //This function returns IMS Totals
        public DataSet GetIMSTotals()
        {
            ResidualsDL IMSResd = new ResidualsDL();
            DataSet ds = IMSResd.ReturnIMSTotals(IMSNum, Month);
            return ds;
        }//end function GetIMSTotals

        /*
        //This function returns IMS Totals for Tier 1. CALLED BY TierResiduals.aspx
        public DataSet GetIMSTotalsT1()
        {
            ResidualsDL IMSResd = new ResidualsDL();
            DataSet ds = IMSResd.GetIMSTotalsT1(MasterNum, Month);
            return ds;
        }//end function GetIMSTotals
         */

        //This function returns sub totals for IMS
        public DataSet GetIMSSubTotals(string MerchantID)
        {
            ResidualsDL SubTotals = new ResidualsDL();
            DataSet ds = SubTotals.GetIMSSubTotals(Month, MerchantID);
            return ds;
        }

    }
}
