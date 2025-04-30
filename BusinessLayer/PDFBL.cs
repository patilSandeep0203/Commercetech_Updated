using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class PDFBL
    {
        //Called by CreateActPDF.aspx.cs
        public PartnerDS.ACTIMSPDFDataTable GetIMSDataFromACT(string ContactID)
        {
            ACTIMSPDFTableAdapter adapter = new ACTIMSPDFTableAdapter();
            return adapter.GetData(new System.Guid (ContactID));
        }//end function GetIMSDataFromACT


        //Called by CreateActPDF.aspx.cs
        public PartnerDS.ACTiPayPDFDataTable GetIPayDataFromACT(string ContactID)
        {
            ACTiPayPDFTableAdapter adapter = new ACTiPayPDFTableAdapter();
            return adapter.GetData(new System.Guid(ContactID));
        }//end function GetIPayDataFromACT

        //Called by CreateActPDF.aspx.cs
        public PartnerDS.ACTSagePDFDataTable GetSageDataFromACT(string ContactID)
        {
            ACTSagePDFTableAdapter adapter = new ACTSagePDFTableAdapter();
            return adapter.GetData(new System.Guid(ContactID));
        }//end function GetSageDataFromACT

        //Called by CreateActPDF.aspx.cs
        public PartnerDS.ACTMerrickPDFDataTable GetMerrickDataFromACT(string ContactID)
        {
            ACTMerrickPDFTableAdapter adapter = new ACTMerrickPDFTableAdapter();
            return adapter.GetData(new System.Guid(ContactID));
        }//end function GetMerrickDataFromACT

        //Called by CreateActPDF.aspx.cs
	    public PartnerDS.ACTChasePDFDataTable GetChaseDataFromACT(string ContactID)
        {
            ACTChasePDFTableAdapter adapter = new ACTChasePDFTableAdapter();
            return adapter.GetData(new System.Guid(ContactID));
        }//end function GetChaseDataFromACT


        //Called by CreateActPDF.aspx.cs
        public DataSet GetPDFSummaryACT(string ColumnName, string Value)
        {
            string strQuery = "Select * from AppPDFSummary Where " + ColumnName + " like '%" + Value + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetACTDataSQL(strQuery, ColumnName, Value);
            return ds;
        }//end function GetDataFromAct

        public DataSet GetAuthnetSummaryACT(string ColumnName, string Value)
        {
            string strQuery = "Select * from VW_AuthnetSummary Where " + ColumnName + " like '%" + Value + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetACTDataSQL(strQuery, ColumnName, Value);
            return ds;
        }//end function GetDataFromAc

        //Called by CreateActPDF.aspx.cs
        public PartnerDS.ACTOptimalIntlPDFDataTable GetInternationalDataFromACT(string ContactID)
        {
            ACTOptimalIntlPDFTableAdapter adapter = new ACTOptimalIntlPDFTableAdapter();
            return adapter.GetData(new System.Guid(ContactID));
        }//end function 

        //Called by CreateActPDF.aspx.cs
        public DataSet GetPayvisionDataFromACT(string ContactID)
        {
            string strQuery = "Select * from VW_PayvisionACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetPayvisionPDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function 

        public DataSet GetStKittsDataFromACT(string ContactID)
        {
            string strQuery = "Select * from VW_OptimalStKittsACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetStKittsPDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function GetDataFromAct

        //Called by CreateActPDF.aspx.cs
        public PartnerDS.ACTOptimalCAPDFDataTable GetCanadaPDFFromACT(string ContactID)
        {
           ACTOptimalCAPDFTableAdapter adapter = new ACTOptimalCAPDFTableAdapter();
           return adapter.GetData(new System.Guid(ContactID));
        }//end function GetChaseDataFromACT

        public DataSet GetCAPDFFromACT(string ContactID)
        {
            string strQuery = "Select * from VW_OptimalCAACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetCAPDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function GetDataFromAct

        public DataSet GetLeasePDFACT(string ContactID)
        {
            string strQuery = "Select * from VW_NorthernLeaseACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetLeasePDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function GetDataFromAct

        public DataSet GetGETIPDFACT(string ContactID)
        {
            string strQuery = "Select * from VW_GETIGiftCardACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetGETIPDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function GetGETIPDFACT

        public DataSet GetAMIIPDFACT(string ContactID)
        {
            string strQuery = "Select * from VW_AMIACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetAMIPDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function GetGETIPDFACT

        public DataSet GetRoamPayPDFACT(string ContactID)
        {
            string strQuery = "Select * from VW_ROAMpayACTPDF Where CONTACTID like '%" + ContactID + "%' ";
            PDFDL PDF = new PDFDL();
            DataSet ds = PDF.GetRoamPayPDFACTSQL(strQuery, ContactID);
            return ds;
        }//end function GetRoamPayPDFACT

    }//end class PDFBL

}
