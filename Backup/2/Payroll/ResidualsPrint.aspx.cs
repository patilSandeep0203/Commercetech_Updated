using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLayer;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

public partial class Residuals_ResidualsPrint : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin"))
            Response.Redirect("~/login.aspx?Authentication=False");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                string Month = "";
                if ( Request.Params.Get("Month") != null )
                    Month = Request.Params.Get("Month");
                lblMonth.Text = "Residual Report for the Month of " + Month;
                PopulateResiduals(Month);
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                Response.Write("Error Processing Request. Please contact technical support");
            }
        }// not postback
    }//end page load

    //This function populates residuals
    public void PopulateResiduals(string Month)
    {
        //set the boundary date for the IMS , this report doesnot exist since OCT 2011
        string lastMonthStr = Month;
        DateTime lastMonthDate = DateTime.Parse(lastMonthStr);
        DateTime dateBound = new DateTime(2011, 10, 1);

        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(7);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";
        
        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        bool ECXLegacyExists = false;
        bool ChaseExists = false;
        bool MerrickExists = false;
        bool OptimalCAExists = false;
        bool IPayment2Exists = false;
        bool IPayment3Exists = false;
        bool IMSExists = false;
        bool IMS2Exists = false;
        bool IPSExists = false;
        bool InovExists = false;
        bool InovGwyExists = false;
        bool SageExists = false;
        bool DiscRapExists = false;
        bool FBBHExists = false;
        bool WorldPayExists = false;
        //bool MiscExists = false;
        bool CSExists = false;
        bool GCExists = false;
        bool CTCartExists = false;
        bool MCAExists = false;
        bool PayrollExists = false;

        if ((lastMonthDate.CompareTo(dateBound)) < 0)
        {
            IMSExists = true;
        }
        else { IMSExists = false; }

        if ((lastMonthDate.CompareTo(dateBound)) < 0)
        {
            InovExists = true;
        }
        else { InovExists = false; }

        if ((lastMonthDate.CompareTo(dateBound)) < 0)
        {
            InovGwyExists = true;
        }
        else { InovGwyExists = false; }

        //Check which columns to generate based on month
        ResidualsAdminBL Resd = new ResidualsAdminBL(Month);
        DataSet dsCheck = Resd.CheckProcessorExists();
        if (dsCheck.Tables[0].Rows.Count > 0)
        {
            DataRow drCheck = dsCheck.Tables[0].Rows[0];
            ECXLegacyExists = Convert.ToBoolean(drCheck["ECXLegacy"]);
            ChaseExists = Convert.ToBoolean(drCheck["Chase"]);
            MerrickExists = Convert.ToBoolean(drCheck["Merrick"]);
            OptimalCAExists = Convert.ToBoolean(drCheck["OptimalCA"]);
            IPayment2Exists = Convert.ToBoolean(drCheck["IPayment2"]);
            IPayment3Exists = Convert.ToBoolean(drCheck["IPayment3"]);
            IMS2Exists = Convert.ToBoolean(drCheck["IMS2"]);
            IPSExists = Convert.ToBoolean(drCheck["IPS"]);
            SageExists = Convert.ToBoolean(drCheck["Sage"]);
            DiscRapExists = Convert.ToBoolean(drCheck["DiscRap"]);
            FBBHExists = Convert.ToBoolean(drCheck["ipayFBBH"]);
            WorldPayExists = Convert.ToBoolean(drCheck["WorldPay"]);
            //MiscExists = Convert.ToBoolean(drCheck["Misc"]);
            CSExists = Convert.ToBoolean(drCheck["CheckServices"]);
            GCExists = Convert.ToBoolean(drCheck["GiftCardServices"]);
            CTCartExists = Convert.ToBoolean(drCheck["CTCart"]);
            MCAExists = Convert.ToBoolean(drCheck["MerchantCashAdvance"]);
            PayrollExists = Convert.ToBoolean(drCheck["PayrollServices"]);
        }// check processor count not 0

        #region Array Declarations
        ArrayList arrList = new ArrayList();
        ArrayList arrListTotals = new ArrayList();
        arrList.Add("Tier 1 Rep");
        arrList.Add("Tier 2 Rep"); //used to be Sub Rep
        arrListTotals.Add("");
        arrListTotals.Add("");
        arrList.Add("IPay1");
        arrListTotals.Add("IPay1");

        if (IPayment2Exists)
        {
            arrList.Add("IPay2");
            arrListTotals.Add("IPay2");
        }
        if (IPayment3Exists)
        {
            arrList.Add("IPay3");
            arrListTotals.Add("IPay3");
        }
        if (FBBHExists)
        {
            arrList.Add("IPay FBBH");
            arrListTotals.Add("IPay FBBH");
        }
        if (ECXLegacyExists)
        {
            arrList.Add("Ecx Legacy");
            arrListTotals.Add("Ecx Legacy");
        }

        if (IMSExists)
        {
            arrList.Add("IMS");
            arrListTotals.Add("IMS");
        }
        if (IPSExists)
        {
            arrList.Add("IPS");
            arrListTotals.Add("IPS");
        }
        if (IMS2Exists)
        {
            arrList.Add("IMS(QB)");
            arrListTotals.Add("IMS(QB)");
        }
        if (SageExists)
        {
            arrList.Add("Sage");
            arrListTotals.Add("Sage");
        }
        if (InovExists)
        {
            arrList.Add("Innov");
            arrListTotals.Add("Innov");
        }
        arrList.Add("CPS");
        arrListTotals.Add("CPS");
        if (DiscRapExists)
        {
            arrList.Add("Disc Rap");
            arrListTotals.Add("Disc Rap");
        }
        if (ChaseExists)
        {
            arrList.Add("Chase");
            arrListTotals.Add("Chase");
        }
        if (MerrickExists)
        {
            arrList.Add("Merrick");
            arrListTotals.Add("Merrick");
        }
        if (OptimalCAExists)
        {
            arrList.Add("Opt CA");
            arrListTotals.Add("Opt CA");
        }
        if (WorldPayExists)
        {
            arrList.Add("WPay");
            arrListTotals.Add("WPay");
        }
        arrList.Add("Auth Net");
        arrListTotals.Add("Auth Net");
        arrList.Add("IPay Gwy");
        arrListTotals.Add("IPay Gwy");
        if (InovGwyExists)
        {
            arrList.Add("Innov Gwy");
            arrListTotals.Add("Innov Gwy");
        }
        arrList.Add("Plug'n Pay");
        arrListTotals.Add("Plug'n Pay");
        if (CSExists)
        {
            arrList.Add("Check Services");
            arrListTotals.Add("Check Services");
        }

        if (GCExists)
        {
            arrList.Add("Gift Card");
            arrListTotals.Add("Gift Card");
        }

        if (CTCartExists)
        {
            arrList.Add("CT Cart");
            arrListTotals.Add("CT Cart");
        }

        if (MCAExists)
        {
            arrList.Add("Merchant Cash Advance");
            arrListTotals.Add("Merchant Cash Advance");
        }

        if (PayrollExists)
        {
            arrList.Add("Payroll");
            arrListTotals.Add("Payroll");
        }

        /*if (MiscExists)
        {
            arrList.Add("Misc");
            arrListTotals.Add("Misc");
        }*/

        arrListTotals.Add("Ece Total");        
        arrListTotals.Add("Rep Total");
        arrListTotals.Add("Sub Tier Total");
        arrListTotals.Add("Payment");
        arrListTotals.Add("");//Tier Total Column not displayed

        arrList.Add("ECE Total");        
        arrList.Add("Rep Total");
        arrList.Add("Sub Tier Total");        
        arrList.Add("Payment");
        arrList.Add("Rep Cat");
        arrList.Add("Funded/ Referred");
        arrList.Add("Rep Split(%)");
        
        #endregion

        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        for (int i = 0; i < arrList.Count; i++)
        {
            td = new TableCell();
            td.Text = arrList[i].ToString();
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "8px";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);
        }
        tblPrintResd.Rows.Add(tr);

        #region Variable Declarations

        double resdIPayGateTotal = 0;
        double resdIPayTotal = 0;
        double resdIMSTotal = 0;
        double resdIPSTotal = 0;
        double resdIMS2Total = 0;
        double resdSageTotal = 0;
        double resdWPayTotal = 0;
        double resdIPayFBBHTotal = 0;
        double resdCPSTotal = 0;
        double resdInnTotal = 0;
        double resdECXLegacyTotal = 0;
        double resdInnGateTotal = 0;
        double resdAuthnetTotal = 0;
        double resdDiscTotal = 0;
        double resdChaseTotal = 0;
        double resdMerrickTotal = 0;
        double resdOptimalCATotal = 0;
        double resdIPay2Total = 0;
        double resdIPay3Total = 0;
        double resdPlugTotal = 0;
        double resdCSTotal = 0;
        double resdGCTotal = 0;
        double resdCTCartTotal = 0;
        double resdMCATotal = 0;
        double resdPayrollTotal = 0;
        //double resdMiscTotal = 0;

        double eceInnTotal = 0;
        double eceIPayGateTotal = 0;
        double eceIPayTotal = 0;
        double eceIMSTotal = 0;
        double eceIPSTotal = 0;
        double eceIMS2Total = 0;
        double eceSageTotal = 0;
        double eceWPayTotal = 0;
        double eceIPayFBBHTotal = 0;
        double eceCPSTotal = 0;
        double eceECXLegacyTotal = 0;
        double eceInnGateTotal = 0;
        double eceAuthnetTotal = 0;
        double eceDiscTotal = 0;
        double eceChaseTotal = 0;
        double eceMerrickTotal = 0;
        double eceOptimalCATotal = 0;
        double eceIPay2Total = 0;
        double eceIPay3Total = 0;
        double ecePlugTotal = 0;
        double eceCSTotal = 0;
        double eceGCTotal = 0;
        double eceCTCartTotal = 0;
        double eceMCATotal = 0;
        double ecePayrollTotal = 0;
        //double eceMiscTotal = 0;

        double IPayGateSum = 0;
        double InnGateSum = 0;
        double IPayFBBHSum = 0;
        double DiscSum = 0;
        double CPSSum = 0;
        double InnSum = 0;
        double ECXLegacySum = 0;
        double IPaySum = 0;
        double IPay2Sum = 0;
        double IPay3Sum = 0;
        double IMSSum = 0;
        double IPSSum = 0;
        double IMS2Sum = 0;
        double SageSum = 0;
        double OptimalCASum = 0;
        double AuthnetSum = 0;
        double WPaySum = 0;
        double PlugSum = 0;
        double ChaseSum = 0;
        double MerrickSum = 0;
        double CSSum = 0;
        double GCSum = 0;
        double CTCartSum = 0;
        double MCASum = 0;
        double PayrollSum = 0;
        //double MiscSum = 0;

        double RepEceTotalSum = 0;

        double resdInnTotalCurr = 0;
        double resdIPayGateTotalCurr = 0;
        double resdIPayTotalCurr = 0;
        double resdIMSTotalCurr = 0;
        double resdIPSTotalCurr = 0;
        double resdIMS2TotalCurr = 0;
        double resdSageTotalCurr = 0;
        double resdWPayTotalCurr = 0;
        double resdIPayFBBHTotalCurr = 0;
        double resdCPSTotalCurr = 0;
        double resdECXLegacyTotalCurr = 0;
        double resdInnGateTotalCurr = 0;
        double resdAuthnetTotalCurr = 0;
        double resdDiscTotalCurr = 0;
        double resdChaseTotalCurr = 0;
        double resdMerrickTotalCurr = 0;
        double resdOptimalCATotalCurr = 0;
        double resdIPay2TotalCurr = 0;
        double resdIPay3TotalCurr = 0;
        double resdPlugTotalCurr = 0;
        double resdCSTotalCurr = 0;
        double resdGCTotalCurr = 0;
        double resdCTCartTotalCurr = 0;
        double resdMCATotalCurr = 0;
        double resdPayrollTotalCurr = 0;
        //double resdMiscTotalCurr = 0;

        double RepResidualTotal = 0;
        double RepResidualPayment = 0;
        
        double RepT1ResidualTotal = 0;
        double RepT1ResidualPayment = 0;
        
        double eceTotal = 0;
        double ECEResidualSum = 0;
        double ECET1ResidualSum = 0;
 
        double ECEPaymentSum = 0;

        double ECEComm = 0;
        double RepCommision = 0;
        double ECEReferralTotal = 0;
        #endregion

        Label lblValue;

        PartnerDS.ResidualsReportsDataTable dt  = Resd.GetResidualsReport();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Each for loop iteration generates one row

                tr = new TableRow();                

                #region Totals
                resdIPayGateTotal = 0;
                resdIPayTotal = 0;
                resdIMSTotal = 0;
                resdIPSTotal = 0;
                resdIMS2Total = 0;
                resdSageTotal = 0;
                resdWPayTotal = 0;
                resdIPayFBBHTotal = 0;
                resdCPSTotal = 0;
                resdInnTotal = 0;
                resdECXLegacyTotal = 0;
                resdInnGateTotal = 0;
                resdAuthnetTotal = 0;
                resdDiscTotal = 0;
                resdChaseTotal = 0;
                resdMerrickTotal = 0;
                resdOptimalCATotal = 0;
                resdIPay2Total = 0;
                resdIPay3Total = 0;
                resdPlugTotal = 0;
                resdCSTotal = 0;
                resdGCTotal = 0;
                resdCTCartTotal = 0;
                resdMCATotal = 0;
                resdPayrollTotal = 0;
                //resdMiscTotal = 0;

                eceInnTotal = 0;
                eceIPayGateTotal = 0;
                eceIPayTotal = 0;
                eceIMSTotal = 0;
                eceIPSTotal = 0;
                eceIMS2Total = 0;
                eceSageTotal = 0;
                eceWPayTotal = 0;
                eceIPayFBBHTotal = 0;
                eceCPSTotal = 0;
                eceECXLegacyTotal = 0;
                eceInnGateTotal = 0;
                eceAuthnetTotal = 0;
                eceDiscTotal = 0;
                eceChaseTotal = 0;
                eceMerrickTotal = 0;
                eceOptimalCATotal = 0;
                eceIPay2Total = 0;
                eceIPay3Total = 0;
                ecePlugTotal = 0;
                eceCSTotal = 0;
                eceGCTotal = 0;
                eceCTCartTotal = 0;
                eceMCATotal = 0;
                ecePayrollTotal = 0;
                //eceMiscTotal = 0;

                if (dt[i].IpayGateECETotalRep.ToString().Trim() != "")
                {
                    eceIPayGateTotal = Convert.ToDouble(dt[i].IpayGateECETotalRep);
                    resdIPayGateTotal = Convert.ToDouble(dt[i].IpayGateRepTotalRep);
                }

                if (dt[i].InnGateECETotalRep.ToString().Trim() != "")
                {
                    eceInnGateTotal = Convert.ToDouble(dt[i].InnGateECETotalRep);
                    resdInnGateTotal = Convert.ToDouble(dt[i].InnGateRepTotalRep);
                }

                if (dt[i].CTCartECETotalRep.ToString().Trim() != "")
                {
                    eceCTCartTotal = Convert.ToDouble(dt[i].CTCartECETotalRep);
                    resdCTCartTotal = Convert.ToDouble(dt[i].CTCartRepTotalRep);
                }

                if (dt[i].DiscEceTotalRep.ToString().Trim() != "")
                {
                    eceDiscTotal = Convert.ToDouble(dt[i].DiscEceTotalRep);
                    resdDiscTotal = Convert.ToDouble(dt[i].DiscRepTotalRep);
                }

                if (dt[i].CPSECETotalRep.ToString().Trim() != "")
                {
                    eceCPSTotal = Convert.ToDouble(dt[i].CPSECETotalRep);
                    resdCPSTotal = Convert.ToDouble(dt[i].CPSRepTotalRep);
                }

                if (dt[i].InnECETotalRep.ToString().Trim() != "")
                {
                    eceInnTotal = Convert.ToDouble(dt[i].InnECETotalRep);
                    resdInnTotal = Convert.ToDouble(dt[i].InnRepTotalRep);
                }

                if (dt[i].ECXLegacyECETotalRep.ToString().Trim() != "")
                {
                    eceECXLegacyTotal = Convert.ToDouble(dt[i].ECXLegacyECETotalRep);
                    resdECXLegacyTotal = Convert.ToDouble(dt[i].ECXLegacyRepTotalRep);
                }

                if (dt[i].IPayECETotalRep.ToString().Trim() != "")
                {
                    eceIPayTotal = Convert.ToDouble(dt[i].IPayECETotalRep);
                    resdIPayTotal = Convert.ToDouble(dt[i].IPayRepTotalRep);
                }

                if (dt[i].IPayFBBHECETotalRep.ToString().Trim() != "")
                {
                    eceIPayFBBHTotal = Convert.ToDouble(dt[i].IPayFBBHECETotalRep);
                    resdIPayFBBHTotal = Convert.ToDouble(dt[i].IPayFBBHRepTotalRep);
                }

                if (dt[i].IPay2EceTotalRep.ToString().Trim() != "")
                {
                    eceIPay2Total = Convert.ToDouble(dt[i].IPay2EceTotalRep);
                    resdIPay2Total = Convert.ToDouble(dt[i].IPay2RepTotalRep);
                }

                if (dt[i].IPay3EceTotalRep.ToString().Trim() != "")
                {
                    eceIPay3Total = Convert.ToDouble(dt[i].IPay3EceTotalRep);
                    resdIPay3Total = Convert.ToDouble(dt[i].IPay3RepTotalRep);
                }

                if (dt[i].IMSEceTotalRep.ToString().Trim() != "")
                {
                    eceIMSTotal = Convert.ToDouble(dt[i].IMSEceTotalRep);
                    resdIMSTotal = Convert.ToDouble(dt[i].IMSRepTotalRep);
                }

                if (dt[i].IPSEceTotalRep.ToString().Trim() != "")
                {
                    eceIPSTotal = Convert.ToDouble(dt[i].IPSEceTotalRep);
                    resdIPSTotal = Convert.ToDouble(dt[i].IPSRepTotalRep);
                }

                if (dt[i].IMS2EceTotalRep.ToString().Trim() != "")
                {
                    eceIMS2Total = Convert.ToDouble(dt[i].IMS2EceTotalRep);
                    resdIMS2Total = Convert.ToDouble(dt[i].IMS2RepTotalRep);
                }

                if (dt[i].SageEceTotalRep.ToString().Trim() != "")
                {
                    eceSageTotal = Convert.ToDouble(dt[i].SageEceTotalRep);
                    resdSageTotal = Convert.ToDouble(dt[i].SageRepTotalRep);
                }

                if (dt[i].OptimalCAECETotalRep.ToString().Trim() != "")
                {
                    eceOptimalCATotal = Convert.ToDouble(dt[i].OptimalCAECETotalRep);
                    resdOptimalCATotal = Convert.ToDouble(dt[i].OptimalCARepTotalRep);
                }

                if (dt[i].AuthnetECETotalRep.ToString().Trim() != "")
                {
                    eceAuthnetTotal = Convert.ToDouble(dt[i].AuthnetECETotalRep);
                    resdAuthnetTotal = Convert.ToDouble(dt[i].AuthnetRepTotalRep);
                }

                if (dt[i].WPayECETotalRep.ToString().Trim() != "")
                {
                    eceWPayTotal = Convert.ToDouble(dt[i].WPayECETotalRep);
                    resdWPayTotal = Convert.ToDouble(dt[i].WPayRepTotalRep);
                }

                if (dt[i].PlugnPayECETotalRep.ToString().Trim() != "")
                {
                    ecePlugTotal = Convert.ToDouble(dt[i].PlugnPayECETotalRep);
                    resdPlugTotal = Convert.ToDouble(dt[i].PlugnPayRepTotalRep);
                }

                if (dt[i].ChaseEceTotalRep.ToString().Trim() != "")
                {
                    eceChaseTotal = Convert.ToDouble(dt[i].ChaseEceTotalRep);
                    resdChaseTotal = Convert.ToDouble(dt[i].ChaseRepTotalRep);
                }

                if (dt[i].MerrickECETotalRep.ToString().Trim() != "")
                {
                    eceMerrickTotal = Convert.ToDouble(dt[i].MerrickECETotalRep);
                    resdMerrickTotal = Convert.ToDouble(dt[i].MerrickRepTotalRep);
                }

                if (dt[i].CSECETotalRep.ToString().Trim() != "")
                {
                    eceCSTotal = Convert.ToDouble(dt[i].CSECETotalRep);
                    resdCSTotal = Convert.ToDouble(dt[i].CSRepTotalRep);
                }

                if (dt[i].GCECETotalRep.ToString().Trim() != "")
                {
                    eceGCTotal = Convert.ToDouble(dt[i].GCECETotalRep);
                    resdGCTotal = Convert.ToDouble(dt[i].GCRepTotalRep);
                }

                if (dt[i].MCAECETotalRep.ToString().Trim() != "")
                {
                    eceMCATotal = Convert.ToDouble(dt[i].MCAECETotalRep);
                    resdMCATotal = Convert.ToDouble(dt[i].MCARepTotalRep);
                }

                if (dt[i].PayrollECETotalRep.ToString().Trim() != "")
                {
                    ecePayrollTotal = Convert.ToDouble(dt[i].PayrollECETotalRep);
                    resdPayrollTotal = Convert.ToDouble(dt[i].PayrollRepTotalRep);
                }
                /*if (dt[i].MiscEceTotalRep.ToString().Trim() != "")
                {
                    eceMiscTotal = Convert.ToDouble(dt[i].MiscEceTotalRep);
                    resdMiscTotal = Convert.ToDouble(dt[i].MiscRepTotalRep);
                }*/

                #endregion

                #region Sum
                IPayGateSum = eceIPayGateTotal + IPayGateSum;
                InnGateSum = eceInnGateTotal + InnGateSum;
                IPayFBBHSum = eceIPayFBBHTotal + IPayFBBHSum;
                DiscSum = eceDiscTotal + DiscSum;
                CPSSum = eceCPSTotal + CPSSum;
                InnSum = eceInnTotal + InnSum;
                ECXLegacySum = eceECXLegacyTotal + ECXLegacySum;
                IPaySum = eceIPayTotal + IPaySum;
                IPay2Sum = eceIPay2Total + IPay2Sum;
                IPay3Sum = eceIPay3Total + IPay3Sum;
                IMSSum = eceIMSTotal + IMSSum;
                IPSSum = eceIPSTotal + IPSSum;
                IMS2Sum = eceIMS2Total + IMS2Sum;
                SageSum = eceSageTotal + SageSum;
                OptimalCASum = eceOptimalCATotal + OptimalCASum;
                AuthnetSum = eceAuthnetTotal + AuthnetSum;
                WPaySum = eceWPayTotal + WPaySum;
                PlugSum = ecePlugTotal + PlugSum;
                ChaseSum = eceChaseTotal + ChaseSum;
                MerrickSum = eceMerrickTotal + MerrickSum; 
                CSSum = eceCSTotal + CSSum;
                GCSum = eceGCTotal + GCSum;
                CTCartSum = eceCTCartTotal + CTCartSum;
                MCASum = eceMCATotal + MCASum;
                PayrollSum = ecePayrollTotal + PayrollSum;
                //MiscSum = eceMiscTotal + MiscSum;
                #endregion

                //Calculate ece Total for Rep
                double RepECETotal = eceIPayGateTotal + eceIPayTotal + eceIMSTotal + eceIPSTotal + eceIMS2Total + eceSageTotal + eceWPayTotal + eceIPayFBBHTotal + eceCPSTotal + eceInnTotal + eceECXLegacyTotal + eceInnGateTotal + eceCTCartTotal + eceAuthnetTotal + eceDiscTotal + ecePlugTotal + eceChaseTotal + eceMerrickTotal + eceOptimalCATotal + eceIPay2Total + eceIPay3Total + eceCSTotal + eceGCTotal + eceMCATotal + ecePayrollTotal;//+ eceMiscTotal;

                double RepECETotalCurr = RepECETotal;
                //Sum of all the individual's RepEceTotal (displayed)                
                RepEceTotalSum = RepEceTotalSum + RepECETotal;

                //Calculate ECE Total for Rep
                resdInnTotalCurr = resdInnTotal;
                resdIPayGateTotalCurr = resdIPayGateTotal;
                resdIPayTotalCurr = resdIPayTotal;
                resdIMSTotalCurr = resdIMSTotal;
                resdIPSTotalCurr = resdIPSTotal;
                resdIMS2TotalCurr = resdIMS2Total;
                resdSageTotalCurr = resdSageTotal;
                resdWPayTotalCurr = resdWPayTotal;
                resdIPayFBBHTotalCurr = resdIPayFBBHTotal;
                resdCPSTotalCurr = resdCPSTotal;
                resdInnTotalCurr = resdInnTotal;
                resdECXLegacyTotalCurr = resdECXLegacyTotal;
                resdInnGateTotalCurr = resdInnGateTotal;
                resdAuthnetTotalCurr = resdAuthnetTotal;
                resdDiscTotalCurr = resdDiscTotal;
                resdChaseTotalCurr = resdChaseTotal;
                resdMerrickTotalCurr = resdMerrickTotal;
                resdOptimalCATotalCurr = resdOptimalCATotal;
                resdIPay2TotalCurr = resdIPay2Total;
                resdIPay3TotalCurr = resdIPay3Total;
                resdPlugTotalCurr = resdPlugTotal;
                resdCSTotalCurr = resdCSTotal;
                resdGCTotalCurr = resdGCTotal;
                resdCTCartTotalCurr = resdCTCartTotal;
                resdMCATotalCurr = resdMCATotal;
                resdPayrollTotalCurr = resdPayrollTotal;
                //resdMiscTotalCurr = resdMiscTotal;

                //===============CALCULATE Commissions FOR Agents==================
                //Sum up Rep's Commissions for the month            	
                //Rep has a Commission Total
                double repCommTotal = 0;
                double RefTotal = 0;
                double BonusTotal = 0;
                double MerchFundedCount = 0;
                double ReferralCount = 0;

                if (dt[i].CommTotal.ToString().Trim() != "")
                    repCommTotal = Convert.ToDouble(dt[i].CommTotal);

                if (dt[i].bonustotal.ToString().Trim() != "")
                    BonusTotal = Convert.ToDouble(dt[i].bonustotal);

                if (dt[i].reftotal.ToString().Trim() != "")
                    RefTotal = Convert.ToDouble(dt[i].reftotal);

                if (dt[i].MerchFundedCount.ToString().Trim() != "")
                    MerchFundedCount = Convert.ToDouble(dt[i].MerchFundedCount);

                if (dt[i].ReferralCount.ToString().Trim() != "")
                    ReferralCount = Convert.ToDouble(dt[i].ReferralCount);

                RepCommision = repCommTotal + BonusTotal;
                ECEComm = (ECEComm) + (RepCommision);
                ECEReferralTotal = (ECEReferralTotal) + (RefTotal);
                //==================END OF Commissions===================

                #region Generate Table
                //*************************GENERATE TABLE*************************

                string MasterNum = dt[i].MasterNum.ToString().Trim();

                //RepName
                lblValue = new Label();
                lblValue.Text = dt[i].T1RepName.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Tier 2 RepName
                lblValue = new Label();
                lblValue.Text = dt[i].RepName.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //IPay
                lblValue = new Label();
                lblValue.Text = resdIPayTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                if (IPayment2Exists)
                {
                    //IPay2
                    lblValue = new Label();
                    lblValue.Text = resdIPay2Total.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (IPayment3Exists)
                {
                    //IPay3
                    lblValue = new Label();
                    lblValue.Text = resdIPay3Total.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (FBBHExists)
                {
                    //IPayFBBH
                    lblValue = new Label();
                    lblValue.Text = resdIPayFBBHTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (ECXLegacyExists)
                {
                    //ECXLegacy
                    lblValue = new Label();
                    lblValue.Text = resdECXLegacyTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //IMS
                if (IMSExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdIMSTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (IPSExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdIPSTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (IMS2Exists)
                {
                    //IMS2
                    lblValue = new Label();
                    lblValue.Text = resdIMS2Total.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (SageExists)
                {
                    //Sage
                    lblValue = new Label();
                    lblValue.Text = resdSageTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Innov
                if (InovExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdInnTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //CPS
                lblValue = new Label();
                lblValue.Text = resdCPSTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Disc
                if (DiscRapExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdDiscTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (ChaseExists)
                {
                    //Chase
                    lblValue = new Label();
                    lblValue.Text = resdChaseTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (MerrickExists)
                {
                    //Merrick
                    lblValue = new Label();
                    lblValue.Text = resdMerrickTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (OptimalCAExists)
                {
                    //Opt CA
                    lblValue = new Label();
                    lblValue.Text = resdOptimalCATotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //WPay
                if (WorldPayExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdWPayTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Auth Net
                lblValue = new Label();
                lblValue.Text = resdAuthnetTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //IPayGwy
                lblValue = new Label();
                lblValue.Text = resdIPayGateTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //InnovGwy
                if (InovGwyExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdInnGateTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Plug N Pay
                lblValue = new Label();
                lblValue.Text = resdPlugTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Check Services
                if (CSExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdCSTotal.ToString();                    
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }
                //Gift Card Services
                if (GCExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdGCTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //CTCart
                if (CTCartExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdCTCartTotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Merchant Cash Advance
                if (MCAExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdMCATotal.ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Payroll
                if (PayrollExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdPayrollTotal.ToString();                    
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Misc
                /*if (MiscExists)
                {
                    lblValue = new Label();
                    lblValue.Text = resdMiscTotal.ToString();                    
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }*/

                //ECE Total
                lblValue = new Label();
                lblValue.Text = RepECETotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                RepResidualTotal = Convert.ToDouble(dt[i].ResidualTotal.ToString());
                RepResidualPayment = Convert.ToDouble(dt[i].Payment);
                RepT1ResidualTotal = Convert.ToDouble(dt[i].TResidualTotal.ToString());
                //RepT1ResidualPayment = Convert.ToDouble(dt[i].RepT1ResidualPayment.ToString());
     
                //Residual Total
                lblValue = new Label();
                lblValue.Text = dt[i].ResidualTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);
                                
                //Residual Total
                lblValue = new Label();
                lblValue.Text = dt[i].TResidualTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Payment
                lblValue = new Label();
                lblValue.Text = dt[i].Payment.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Category
                lblValue = new Label();
                lblValue.Text = dt[i].RepCat.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Merchant Funded Count
                lblValue = new Label();
                lblValue.Text = MerchFundedCount.ToString() + "/" + ReferralCount.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Rep Split
                lblValue = new Label();
                lblValue.Text = dt[i].RepSplit.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);
               
                tblPrintResd.Rows.Add(tr);

                #endregion

                eceTotal = RepECETotal + eceTotal;
                //add Rep's Residual Total to the running sum for all reps
                ECEResidualSum = ECEResidualSum + RepResidualTotal;
                //Add to Tier total only when Rep is Tier 1 Rep
                if (dt[i].T1MasterNum.ToString().Trim() == dt[i].MasterNum.ToString().Trim())
                    ECET1ResidualSum = ECET1ResidualSum + RepT1ResidualTotal;
                ECEPaymentSum = ECEPaymentSum + RepResidualPayment;
                //ECEPaymentSum = ECEPaymentSum + Payment;

            }//end for
        }// count not 0

        //Generate Totals
        #region Totals

        #region Number of Accounts

        string GateCount = "";
        string InvgCount = "";
        string FbbhCount = "";
        string DiscCount = "";
        string CpsCount = "";
        string PlugCount = "";
        string InnvCount = "";
        string WpayCount = "";
        string AuthCount = "";
        string ImsCount = "";
        string IPSCount = "";
        string Ims2Count = "";
        string SageCount = "";
        string IPayCount = "";
        string IPay2Count = "";
        string IPay3Count = "";
        string ecxLegacyCount = "";
        string ChaseCount = "";
        string MerrickCount = "";
        string OptimalCACount = "";
        string CSCount = "";
        string GCCount = "";
        string CtcartCount = "";
        string MCACount = "";
        string PayrollCount = "";
        //string MiscCount = "";

        DataSet dsAcct = Resd.GetResidualsCount();
        if (dsAcct.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsAcct.Tables[0].Rows[0];
            GateCount = dr["IPayGateCount"].ToString().Trim();
            InvgCount = dr["InnGateCount"].ToString().Trim();
            FbbhCount = dr["IPayFBBHCount"].ToString().Trim();
            DiscCount = dr["DiscRapCount"].ToString().Trim();
            CpsCount = dr["CPSCount"].ToString().Trim();
            PlugCount = dr["PlugNPayCount"].ToString().Trim();
            InnvCount = dr["InnCount"].ToString().Trim();
            WpayCount = dr["WpayCount"].ToString().Trim();
            AuthCount = dr["AuthCount"].ToString().Trim();
            ImsCount = dr["ImsCount"].ToString().Trim();
            IPSCount = dr["IPSCount"].ToString().Trim();
            Ims2Count = dr["Ims2Count"].ToString().Trim();
            SageCount = dr["SageCount"].ToString().Trim();
            IPayCount = dr["IPayCount"].ToString().Trim();
            IPay2Count = dr["IPay2Count"].ToString().Trim();
            IPay3Count = dr["IPay3Count"].ToString().Trim();
            ecxLegacyCount = dr["ecxLegacyCount"].ToString().Trim();
            ChaseCount = dr["ChaseCount"].ToString().Trim();
            MerrickCount = dr["MerrickCount"].ToString().Trim();
            OptimalCACount = dr["OptimalCACount"].ToString().Trim(); 
            CSCount = dr["CSCount"].ToString().Trim();
            GCCount = dr["GCCount"].ToString().Trim();
            CtcartCount = dr["CtCartCount"].ToString().Trim();
            MCACount = dr["MCACount"].ToString().Trim();
            PayrollCount = dr["PayrollCount"].ToString().Trim();
            //MiscCount = dr["MiscReportCount"].ToString().Trim();
        }
        #endregion

        #region Calculate Totals
        //*******************************Calculate Totals*******************************

        #region Variables
     
        //Previous Month Variables
        double CPSPrevTotal = 0;
        double InnPrevTotal = 0;
        double AuthnetPrevTotal = 0;
        double IPayPrevTotal = 0;
        double IPay2PrevTotal = 0;
        double IPay3PrevTotal = 0;
        double IMSPrevTotal = 0;
        double IPSPrevTotal = 0;
        double IMS2PrevTotal = 0;
        double SagePrevTotal = 0;
        double WPayPrevTotal = 0;
        double IPayGatePrevTotal = 0;
        double InnGatePrevTotal = 0;
        double IPayFBBHPrevTotal = 0;
        double ChasePrevTotal = 0;
        double MerrickPrevTotal = 0;
        double OptimalCAPrevTotal = 0;
        double DiscPrevTotal = 0;
        double ECXLegacyPrevTotal = 0;
        double PlugPrevTotal = 0;
        double CSPrevTotal = 0;
        double GCPrevTotal = 0;
        double CTCartPrevTotal = 0;
        double MCAPrevTotal = 0;
        double PayrollPrevTotal = 0;
        //double MiscPrevTotal = 0;

        #endregion

        #region Current Months Totals
        //Select the ECETotal for the current month for all CPS accounts
        /*
        CPSTotal = Resd.GetECETotal("CPS", "ALL");
        InnTotal = Resd.GetECETotal("Innovative", "ALL");
        CTCartTotal = Resd.GetECETotal("CTCart", "ALL");
        AuthnetTotal = Resd.GetECETotal("Authnet", "ALL");
        IPayTotal = Resd.GetECETotal("IPay", "ALL");
        IPay2Total = Resd.GetECETotal("IPay2", "ALL");
        IMSTotal = Resd.GetECETotal("IMS", "ALL");
        IMS2Total = Resd.GetECETotal("IMS2", "ALL");
        WPayTotal = Resd.GetECETotal("WPay", "ALL");
        IPayGateTotal = Resd.GetECETotal("IPayGate", "ALL");
        InnGateTotal = Resd.GetECETotal("InnGate", "ALL");
        IPayFBBHTotal = Resd.GetECETotal("IPayFBBH", "ALL");
        ChaseTotal = Resd.GetECETotal("Chase", "ALL");
        MerrickTotal = Resd.GetECETotal("Merrick", "ALL");
        OptimalCATotal = Resd.GetECETotal("OptimalCA", "ALL");
        DiscTotal = Resd.GetECETotal("Disc", "ALL");
        ECXLegacyTotal = Resd.GetECETotal("ECX", "ALL");
        PlugTotal = Resd.GetECETotal("PlugNPay", "ALL");
        MiscTotal = Resd.GetECETotal("Misc", "ALL");
        */

        #endregion
        //---------------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------------

        //eceTotal = IPayGateTotal + IPayTotal + IMSTotal + IMS2Total + WPayTotal + IPayFBBHTotal + CPSTotal + InnTotal + ECXLegacyTotal + InnGateTotal + CTCartTotal + AuthnetTotal + DiscTotal + PlugTotal + ChaseTotal + OptimalCATotal + MerrickTotal + IPay2Total + MiscTotal;

        double ECEPrevTotal = IPayGatePrevTotal + IPayPrevTotal + IMSPrevTotal + IPSPrevTotal + IMS2PrevTotal + SagePrevTotal + WPayPrevTotal + IPayFBBHPrevTotal + CPSPrevTotal + InnPrevTotal + ECXLegacyPrevTotal + InnGatePrevTotal + CTCartPrevTotal + AuthnetPrevTotal + DiscPrevTotal + PlugPrevTotal + ChasePrevTotal + OptimalCAPrevTotal + MerrickPrevTotal + IPay2PrevTotal + IPay3PrevTotal + CSPrevTotal + GCPrevTotal + MCAPrevTotal + PayrollPrevTotal;//MiscPrevTotal;

        double eceSum = IPayGateSum + IPaySum + IMSSum + IPSSum + IMS2Sum + SageSum + WPaySum + IPayFBBHSum + CPSSum + InnSum + ECXLegacySum + InnGateSum + CTCartSum + AuthnetSum + DiscSum + PlugSum + ChaseSum + MerrickSum + OptimalCASum + IPay2Sum + IPay3Sum + CSSum + GCSum + MCASum + PayrollSum;//MiscSum;


        //*******************************End Calculate Totals*******************************
        #endregion

        #region Header
        /*tr = new TableRow();
        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        td = new TableCell();
        td.Attributes.Add("colspan", "28");
        td.Text = "Totals For ECE by Vendors";
        td.Style["font-family"] = "Arial";
        td.Style["font-size"] = "15px";
        td.Style["font-weight"] = "Bold";
        td.Style["Color"] = "White";
        tr.Cells.Add(td);

        tr.Cells.Add(td);
        tblPrintResd.Rows.Add(tr);*/

        tr = new TableRow();
        td = new TableCell();
        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        for (int i = 0; i < arrListTotals.Count; i++)
        {
            td = new TableCell();
            td.Text = arrListTotals[i].ToString();
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "8px";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);
        }

        /*td = new TableCell();
        lblValue = new Label();
        lblValue.Text = RepEceTotalSum.ToString();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);*/

        td = new TableCell();
        td.Attributes.Add("colspan", "7");
        tr.Cells.Add(td);
        
        tblPrintResd.Rows.Add(tr);
        #endregion

        #region FirstRow (ECE Total Curr)
        tr = new TableRow();
        //ECETotal Header
        lblValue = new Label();
        lblValue.Text = "ECE Total";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        td = new TableCell();
        tr.Cells.Add(td);

        //IPay
        lblValue = new Label();
        lblValue.Text = IPaySum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        if (IPayment2Exists)
        {
            //IPay2
            lblValue = new Label();
            lblValue.Text = IPay2Sum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPayment3Exists)
        {
            //IPay3
            lblValue = new Label();
            lblValue.Text = IPay3Sum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (FBBHExists)
        {
            //IPayFBBH
            lblValue = new Label();
            lblValue.Text = IPayFBBHSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ECXLegacyExists)
        {
            //ECXLegacy
            lblValue = new Label();
            lblValue.Text = ECXLegacySum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //IMS
        if (IMSExists)
        {
            lblValue = new Label();
            lblValue.Text = IMSSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPSExists)
        {
            lblValue = new Label();
            lblValue.Text = IPSSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IMS2Exists)
        {
            //IMS2
            lblValue = new Label();
            lblValue.Text = IMS2Sum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (SageExists)
        {
            //Sage
            lblValue = new Label();
            lblValue.Text = SageSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Innov
        if (InovExists)
        {
            lblValue = new Label();
            lblValue.Text = InnSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //CPS
        lblValue = new Label();
        lblValue.Text = CPSSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //Disc
        if (DiscRapExists)
        {
            lblValue = new Label();
            lblValue.Text = DiscSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ChaseExists)
        {
            //Chase
            lblValue = new Label();
            lblValue.Text = ChaseSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (MerrickExists)
        {
            //Merrick
            lblValue = new Label();
            lblValue.Text = MerrickSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (OptimalCAExists)
        {
            //Opt CA
            lblValue = new Label();
            lblValue.Text = OptimalCASum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //WPay
        if (WorldPayExists)
        {
            lblValue = new Label();
            lblValue.Text = WPaySum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Auth Net
        lblValue = new Label();
        lblValue.Text = AuthnetSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);
        
        //IPayGwy
        lblValue = new Label();
        lblValue.Text = IPayGateSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //InnovGwy
        if (InovGwyExists)
        {
            lblValue = new Label();
            lblValue.Text = InnGateSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Plug N Pay
        lblValue = new Label();
        lblValue.Text = PlugSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //CS
        if (CSExists)
        {
            lblValue = new Label();
            lblValue.Text = CSSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //GC
        if (GCExists)
        {
            lblValue = new Label();
            lblValue.Text = GCSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //CTCart
        if (CTCartExists)
        {
            lblValue = new Label();
            lblValue.Text = CTCartSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //MCA
        if (MCAExists)
        {
            lblValue = new Label();
            lblValue.Text = MCASum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Payroll
        if (PayrollExists)
        {
            lblValue = new Label();
            lblValue.Text = PayrollSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Misc
        /*if (MiscExists)
        {
            lblValue = new Label();
            lblValue.Text = MiscTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }*/

        //ECE Total, as calculated by summing up the ECETotals in the database
        lblValue = new Label();
        lblValue.Text = eceSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);
        
        td = new TableCell();
        lblValue = new Label();
        lblValue.Text = "$" + ECEResidualSum.ToString();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        td = new TableCell();
        lblValue = new Label();
        lblValue.Text = "$" + ECET1ResidualSum.ToString();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        td = new TableCell();
        lblValue = new Label();
        lblValue.Text = "$" + ECEPaymentSum.ToString();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);


        td = new TableCell();
        td.Attributes.Add("colspan", "5");
        tr.Cells.Add(td);

        tblPrintResd.Rows.Add(tr);
        #endregion

        #region SecondRow (Number of Accounts)
        tr = new TableRow();
        //ECETotal Header
        lblValue = new Label();
        lblValue.Text = "# of Accounts";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        td = new TableCell();
        tr.Cells.Add(td);

        //IPay
        lblValue = new Label();
        lblValue.Text = IPayCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        if (IPayment2Exists)
        {
            //IPay2
            lblValue = new Label();
            lblValue.Text = IPay2Count.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPayment3Exists)
        {
            //IPay3
            lblValue = new Label();
            lblValue.Text = IPay3Count.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (FBBHExists)
        {
            //IPayFBBH
            lblValue = new Label();
            lblValue.Text = FbbhCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ECXLegacyExists)
        {
            //ECXLegacy
            lblValue = new Label();
            lblValue.Text = ecxLegacyCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //IMS
        if (IMSExists)
        {
            lblValue = new Label();
            lblValue.Text = ImsCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPSExists)
        {
            lblValue = new Label();
            lblValue.Text = IPSCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IMS2Exists)
        {
            //IMS2
            lblValue = new Label();
            lblValue.Text = Ims2Count.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (SageExists)
        {
            //Sage
            lblValue = new Label();
            lblValue.Text = SageCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Innov
        if (InovExists)
        {
            lblValue = new Label();
            lblValue.Text = InnvCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //CPS
        lblValue = new Label();
        lblValue.Text = CpsCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //Disc
        if (DiscRapExists)
        {

            lblValue = new Label();
            lblValue.Text = DiscCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ChaseExists)
        {
            //Chase
            lblValue = new Label();
            lblValue.Text = ChaseCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (MerrickExists)
        {
            //Merrick
            lblValue = new Label();
            lblValue.Text = MerrickCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (OptimalCAExists)
        {
            //Opt CA
            lblValue = new Label();
            lblValue.Text = OptimalCACount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //WPay
        if (WorldPayExists)
        {
            lblValue = new Label();
            lblValue.Text = WpayCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Auth Net
        lblValue = new Label();
        lblValue.Text = AuthCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPayGwy
        lblValue = new Label();
        lblValue.Text = GateCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //InnovGwy
        if (InovGwyExists)
        {
            lblValue = new Label();
            lblValue.Text = InvgCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        
        //Plug N Pay
        lblValue = new Label();
        lblValue.Text = PlugCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //CS
        if (CSExists)
        {
            lblValue = new Label();
            lblValue.Text = CSCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //GC
        if (GCExists)
        {
            lblValue = new Label();
            lblValue.Text = GCCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //CTCart
        if (CTCartExists)
        {
            lblValue = new Label();
            lblValue.Text = CtcartCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //MCA
        if (MCAExists)
        {
            lblValue = new Label();
            lblValue.Text = MCACount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Payroll
        if (PayrollExists)
        {
            lblValue = new Label();
            lblValue.Text = PayrollCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Misc
        /*if (MiscExists)
        {
            lblValue = new Label();
            lblValue.Text = MiscCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }*/

        td = new TableCell();
        td.Attributes.Add("colspan", "7");
        tr.Cells.Add(td);

        tblPrintResd.Rows.Add(tr);
        #endregion
       
        #endregion
    }//end function PopulateResiduals
}
