using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;
using DLPartner.net.eftsecure.uno;
using AnetMerchantBoarding;
using AnetMerchantBoarding.ANetAPI;


namespace BusinessLayer
{
    public class XMLBL
    {

        public PartnerDS.ACTiPayXMLDataTable GetIPayXML(System.Guid ContactID)
        {
            ACTiPayXMLTableAdapter iPayXMLAdapter = new ACTiPayXMLTableAdapter();
            return iPayXMLAdapter.GetData(ContactID);
        }

        #region CreateSageXML
        public PartnerDS.ACTSageXMLDataTable GetSageXML(System.Guid ContactID)
        {
            ACTSageXMLTableAdapter SageXMLAdapter = new ACTSageXMLTableAdapter();
            return SageXMLAdapter.GetData(ContactID);
        }

        public static clsAuthenticationHeader CreateSageAuthentication(System.Guid ContactID)
        {
            XMLBL Sage = new XMLBL();
            PartnerDS.ACTSageXMLDataTable dt = Sage.GetSageXML(ContactID);
            wsSalesCenterBoarding ws = new wsSalesCenterBoarding();

            //Contractor Authentication Username and password
            clsAuthenticationHeader header = new clsAuthenticationHeader();
            header.userName = "Comtech1";
            header.password = "1Success11";

            string SageRepNum = dt[0].RepNum.ToString().Trim();
            RepInfoBL RepInfo = new RepInfoBL();
            string MasterNum = RepInfo.ReturnMasterNum(dt[0].RepName.ToString().Trim());
            DataSet ds = RepInfo.GetRepInfoUnoLogin(MasterNum);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow drRepInfo = ds.Tables[0].Rows[0];
                header.userName = drRepInfo["UnoUsername"].ToString().Trim();
                header.password = drRepInfo["UnoPassword"].ToString().Trim();
            }
            return header;
        }
                
        #endregion

        #region CreateAnetXML

        public PartnerDS.ACTAuthnetXMLDataTable GetAuthnetXML(System.Guid ContactID)
        {
            ACTAuthnetXMLTableAdapter AuthnetXMLAdapter = new ACTAuthnetXMLTableAdapter();
            return AuthnetXMLAdapter.GetData(ContactID);
        }
                
        #endregion

    }//end class XMLBL
}
