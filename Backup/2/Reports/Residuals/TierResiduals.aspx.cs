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

public partial class Reports_Residuals_TierResidual : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "../Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "../Agent.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "../Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "../Admin.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        //if (User.IsInRole("Admin") && (Request.Params.Get("MasterNum") == null))
        //  Response.Redirect("../../PayRoll/ResidualsAdmin.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                MonthBL Mon = new MonthBL();
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    DataSet dsMon = Mon.GetMonthListForReports(1, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    string Month = string.Empty;
                    string MasterNum = string.Empty;
                    if (Request.Params.Get("Month") != null)
                        Month = Request.Params.Get("Month");

                    ResidualsBL Residuals = new ResidualsBL(Month, MasterNum);

                    if (Request.Params.Get("MasterNum") != null)
                        MasterNum = Request.Params.Get("MasterNum");

                    if ((Month != "") && (MasterNum != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                    {
                        try
                        {
                            lblError.Visible = false;
                            lstMonth.SelectedValue = lstMonth.Items.FindByText(Month).Value;
                            lblError.Visible = false;
                            lblMonth.Text = "Residuals for the month of: " + lstMonth.SelectedItem.Text;
                            Populate(MasterNum, Month);
                        }
                        catch (Exception err)
                        {
                            CreateLog Log = new CreateLog();
                            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                            DisplayMessage("Error Processing Request. Please contact technical support");
                        }
                    }//end if month not null
                }//end if
                else
                {
                    DataSet dsMon = Mon.GetMonthListForReports(2, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }
                }
            }//end try
            catch (Exception)
            {
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }// not postback
    }//end page load

    //This function populates ipayment residuals
    public void Populate(string MasterNum, string Month)
    {
        ResidualsBL Residuals = new ResidualsBL(Month, MasterNum);

        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        Style sHyperLink = new Style();
        sHyperLink.Font.Bold = true;
        sHyperLink.Font.Size = FontUnit.Point(8);
        sHyperLink.Font.Name = "Arial";
        sHyperLink.CssClass = "One";

        lstMonth.SelectedValue = lstMonth.Items.FindByText(Month).Value;
        lblMonth.Text = "Residuals for the month of: " + Month + " for MasterNum " + MasterNum;

        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
        {
            lblRepName.Visible = true;
            lblRepName.Text = "";
        }

         TableRow tr = new TableRow();
         TableCell td = new TableCell();

         Label lblValue;
         HyperLink lnkExport;

            #region Header Row

            string[] arrColumns = { "Processors/Banks", "ECE Total", "Tier Total" };
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < arrColumns.Length; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i];
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "12px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }
            tblResiduals.Rows.Add(tr);

            #endregion

            #region IPay

            //******************************IPay******************************
            double ECEIPayTotal = 0;
            double ResdIPayTTotal = 0;

            //Check if IPay exists
            //If Rep has at least ONE IPay Account
            if (Residuals.ReturnIPayNum() != "")
            {
                //Rep has a IPay ECE Total (and a IPay Number)
                DataSet dsTotals = Residuals.GetiPayTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPayTotal = Convert.ToDouble(drTotal["IPayEceTotal"]);
                    ResdIPayTTotal = Convert.ToDouble(drTotal["IPayTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "iPayment, Inc.(1503 Portfolio)";
                //lnkExport.NavigateUrl = "IPay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPayTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region iPay2

            //******************************iPay2******************************
            double ECEiPay2Total = 0;
            double ResdiPay2TTotal = 0;

            //Check if iPay2 exists
            //If Rep has at least ONE iPay2 Account
            if (Residuals.ReturnIPay2Num() != "")
            {
                //Rep has a iPay2 ECE Total (and a iPay2 Number)
                DataSet dsTotals = Residuals.GetiPay2TotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEiPay2Total = Convert.ToDouble(drTotal["iPay2EceTotal"]);
                    ResdiPay2TTotal = Convert.ToDouble(drTotal["iPay2TTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "iPayment, Inc.(40558 Portfolio)";
                //lnkExport.NavigateUrl = "iPay2.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEiPay2Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdiPay2TTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region iPay3

            //******************************iPay3******************************
            double ECEiPay3Total = 0;
            double ResdiPay3TTotal = 0;

            //Check if iPay3 exists
            //If Rep has at least ONE iPay2 Account
            if (Residuals.ReturnIPay3Num() != "")
            {
                //Rep has a iPay2 ECE Total (and a iPay2 Number)
                DataSet dsTotals = Residuals.GetiPay3TotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEiPay3Total = Convert.ToDouble(drTotal["iPay3EceTotal"]);
                    ResdiPay3TTotal = Convert.ToDouble(drTotal["iPay3TTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "iPayment, Inc.";
                //lnkExport.NavigateUrl = "iPay3.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEiPay3Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdiPay3TTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region IPayFBBH
            //******************************IPayFBBH******************************
            double ECEIPayFBBHTotal = 0;
            double ResdIPayFBBHTTotal = 0;

            //Check if IPayFBBH exists
            bool FBBHExists = Residuals.CheckVendorExistsForRep("ipayfbbh");
            //If Rep has at least ONE IPayFBBH Account
            if (FBBHExists)
            {
                //Rep has a IPayFBBH ECE Total (and a IPayFBBH Number)
                DataSet dsTotals = Residuals.GetIPayFBBHTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPayFBBHTotal = Convert.ToDouble(drTotal["IPayFBBHEceTotal"]);
                    ResdIPayFBBHTTotal = Convert.ToDouble(drTotal["IPayFBBHTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "iPayment FBBH";
                //lnkExport.NavigateUrl = "IPayFBBH.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPayFBBHTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPayFBBHTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region ECXLegacy

            //******************************ECXLegacy******************************
            double ECEECXLegacyTotal = 0;
            double ResdECXLegacyTTotal = 0;
            if (Month.Contains("2005"))
            {
                //Check if ECXLegacy exists
                bool ECXLegExists = Residuals.CheckVendorExistsForRep("ECXLegacy");
                //If Rep has at least ONE ECX Legacy Account
                if (ECXLegExists)
                {
                    //Rep has a ECXLegacy ECE Total (and a ECXLegacy Number)
                    DataSet dsTotals = Residuals.GetECXLegacyTotalsT1();
                    if (dsTotals.Tables[0].Rows.Count > 0)
                    {
                        DataRow drTotal = dsTotals.Tables[0].Rows[0];
                        ECEECXLegacyTotal = Convert.ToDouble(drTotal["ECXLegacyEceTotal"]);
                        ResdECXLegacyTTotal = Convert.ToDouble(drTotal["ECXLegacyTTotal"]);
                    }//end if count not 0

                    tr = new TableRow();
                    lblValue = new Label();
                    lblValue.Text = "ECXLegacy";
                    //lnkExport.NavigateUrl = "ECXLegacy.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = ECEECXLegacyTotal.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = ResdECXLegacyTTotal.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    tblResiduals.Rows.Add(tr);
                }//end if count not 0
            }//end if month contains 2005
         #endregion

            #region IMS

            //******************************IMS******************************
            double ECEIMSTotal = 0;
            double ResdIMSTTotal = 0;

            //Check if IMS exists
            //If Rep has at least ONE IMS Account
            if (Residuals.ReturnIPay2Num() != "")
            {
                //Rep has a IMS ECE Total (and a IMS Number)
                DataSet dsTotals = Residuals.GetIMSTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIMSTotal = Convert.ToDouble(drTotal["IMSEceTotal"]);
                    ResdIMSTTotal = Convert.ToDouble(drTotal["IMSTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "IMS";
                //lnkExport.NavigateUrl = "IMS.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIMSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIMSTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if num exists

            #endregion

            #region IMS2

            //******************************IMS2******************************
            double ECEIMS2Total = 0;
            double ResdIMS2TTotal = 0;

            //Check if IMS2 exists
            //If Rep has at least ONE IMS2 Account
            if (Residuals.ReturnIMS2Num() != "")
            {
                //Rep has a IMS2 ECE Total (and a IMS2 Number)
                DataSet dsTotals = Residuals.GetIMS2TotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIMS2Total = Convert.ToDouble(drTotal["IMS2EceTotal"]);
                    ResdIMS2TTotal = Convert.ToDouble(drTotal["IMS2TTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "IMS(QB)";
                //lnkExport.NavigateUrl = "IMS2.aspx?MasterNum=" + Residuals.ReturnIMS2Num() + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIMS2Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIMS2TTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Sage

            //******************************Sage******************************
            double ECESageTotal = 0;
            double ResdSageTTotal = 0;

            //Check if Sage exists
            //If Rep has at least ONE Sage Account
            if (Residuals.ReturnSageNum() != "")
            {
                //Rep has a Sage ECE Total (and a Sage Number)
                DataSet dsTotals = Residuals.GetSageTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECESageTotal = Convert.ToDouble(drTotal["SageEceTotal"]);
                    ResdSageTTotal = Convert.ToDouble(drTotal["SageTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Sage";
                //lnkExport.NavigateUrl = "Sage.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECESageTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdSageTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion
        
            #region Innovative
            //******************************Innovative******************************
            double ECEInnovativeTotal = 0;
            double ResdInnovativeTTotal = 0;

            //Check if Innovative exists
            bool InnExists = Residuals.CheckVendorExistsForRep("innovative");
            //If Rep has at least ONE Innovative Account
            if (InnExists)
            {
                //Rep has a Innovative ECE Total (and a Innovative Number)
                DataSet dsTotals = Residuals.GetInnovativeTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEInnovativeTotal = Convert.ToDouble(drTotal["InnEceTotal"]);
                    ResdInnovativeTTotal = Convert.ToDouble(drTotal["InnTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Innovative Merchant Solutions";
                //lnkExport.NavigateUrl = "Innovative.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEInnovativeTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdInnovativeTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region CPS
            //******************************CPS******************************
            double ECECPSTotal = 0;
            double ResdCPSTTotal = 0;

            //Check if CPS exists
            bool CPSExists = Residuals.CheckVendorExistsForRep( "cps");
            //If Rep has at least ONE CPS Account
            if (CPSExists)
            {
                //Rep has a CPS ECE Total (and a CPS Number)
                DataSet dsTotals = Residuals.GetCPSTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECECPSTotal = Convert.ToDouble(drTotal["CPSEceTotal"]);
                    ResdCPSTTotal = Convert.ToDouble(drTotal["CPSTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "CPS";
                //lnkExport.NavigateUrl = "cps.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECECPSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdCPSTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0
            #endregion
        
            #region Chase

            //******************************Chase******************************
            double ECEChaseTotal = 0;
            double ResdChaseTTotal = 0;
        
            //If Rep has at least a Chase Rep Number
            if (Residuals.ReturnChaseNum() != "")
            {
                //Rep has a Chase ECE Total (and a Chase Number)
                DataSet dsTotals = Residuals.GetChaseTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEChaseTotal = Convert.ToDouble(drTotal["ChaseEceTotal"]);
                    ResdChaseTTotal = Convert.ToDouble(drTotal["ChaseTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Chase";
                //lnkExport.NavigateUrl = "Chase.aspx?MasterNum==" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEChaseTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdChaseTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Merrick

            //******************************Merrick******************************
            double ECEMerrickTotal = 0;
            double ResdMerrickTTotal = 0;

            //Check if Merrick exists
            bool MerrickExists = Residuals.CheckVendorExistsForRep( "Merrick");
            //If Rep has at least ONE WPay Account
            if (MerrickExists)
            {
                //Rep has a Merrick ECE Total (and a Merrick Number)
                DataSet dsTotals = Residuals.GetMerrickTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEMerrickTotal = Convert.ToDouble(drTotal["MerrickEceTotal"]);
                    ResdMerrickTTotal = Convert.ToDouble(drTotal["MerrickTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Merrick";
                //lnkExport.NavigateUrl = "Merrick.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEMerrickTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdMerrickTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region OptimalCA

            //******************************OptimalCA******************************
            double ECEOptimalCATotal = 0;
            double ResdOptimalCATTotal = 0;

            //Check if OptimalCA exists
            bool OptCAExists = Residuals.CheckVendorExistsForRep( "OptimalCA");
            //If Rep has at least ONE WPay Account
            if (OptCAExists)
            {
                //Rep has a OptimalCA ECE Total (and a OptimalCA Number)
                DataSet dsTotals = Residuals.GetOptimalCATotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEOptimalCATotal = Convert.ToDouble(drTotal["OptimalCAEceTotal"]);
                    ResdOptimalCATTotal = Convert.ToDouble(drTotal["OptimalCATTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Optimal CA";
                //lnkExport.NavigateUrl = "OptimalCA.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEOptimalCATotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdOptimalCATTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }

            #endregion

            #region WPay

            //******************************WPay******************************
            double ECEWPayTotal = 0;
            double ResdWPayTTotal = 0;

            //Check if WPay exists
            bool WPayExists = Residuals.CheckVendorExistsForRep("WPay");
            //If Rep has at least ONE WPay Account
            if (WPayExists)
            {
                //Rep has a WPay ECE Total (and a WPay Number)
                DataSet dsTotals = Residuals.GetWPayTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEWPayTotal = Convert.ToDouble(drTotal["WPayEceTotal"]);
                    ResdWPayTTotal = Convert.ToDouble(drTotal["WPayTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "WorldPay";
                //lnkExport.NavigateUrl = "WPay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEWPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdWPayTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Header Row Payment Gateways
            tr = new TableRow();
            string[] arrColumns2 = { "Payment Gateways", "ECE Total", "Tier Total" };
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < arrColumns.Length; i++)
            {
                td = new TableCell();
                td.Text = arrColumns2[i];
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "12px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }
            tblResiduals.Rows.Add(tr);

            #endregion

            #region Authnet
            //******************************Authnet******************************
            double ECEAuthnetTotal = 0;
            double ResdAuthnetTTotal = 0;

            //Check if Authnet exists
            bool AuthExists = Residuals.CheckVendorExistsForRep( "Authnet");
            //If Rep has at least ONE Authnet Account
            if (AuthExists)
            {
                //Rep has a Authnet ECE Total (and a Authnet Number)
                DataSet dsTotals = Residuals.GetAuthnetTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEAuthnetTotal = Convert.ToDouble(drTotal["AuthnetEceTotal"]);
                    ResdAuthnetTTotal = Convert.ToDouble(drTotal["AuthnetTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Authorize.Net";
                //lnkExport.NavigateUrl = "Authnet.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEAuthnetTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdAuthnetTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region IPayGate
            //******************************IPayGate******************************
            double ECEIPayGateTotal = 0;
            double ResdIPayGateTTotal = 0;

            //Check if IPayGate exists
            bool IPayGateExists = Residuals.CheckVendorExistsForRep( "ipaygate");
            //If Rep has at least ONE IPayGate Account
            if (IPayGateExists)
            {
                //Rep has a IPayGate ECE Total (and a IPayGate Number)
                DataSet dsTotals = Residuals.GetIPayGateTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPayGateTotal = Convert.ToDouble(drTotal["IPayGateEceTotal"]);
                    ResdIPayGateTTotal = Convert.ToDouble(drTotal["IPayGateTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "iPayment Gateway";
                //lnkExport.NavigateUrl = "IPayGate.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPayGateTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPayGateTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region InnGate
            //******************************InnGate******************************
            double ECEInnGateTotal = 0;
            double ResdInnGateTTotal = 0;

            //Check if InnGate exists
            bool InnGateExists = Residuals.CheckVendorExistsForRep( "inngate");
            //If Rep has at least ONE InnGate Account
            if (InnGateExists)
            {
                //Rep has a InnGate ECE Total (and a InnGate Number)
                DataSet dsTotals = Residuals.GetInnGateTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEInnGateTotal = Convert.ToDouble(drTotal["InnGateEceTotal"]);
                    ResdInnGateTTotal = Convert.ToDouble(drTotal["InnGateTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Innovative Gateway";
                //lnkExport.NavigateUrl = "InnGate.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEInnGateTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdInnGateTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region PlugNPay
            //******************************PlugNPay******************************
            double ECEPlugNPayTotal = 0;
            double ResdPlugNPayTTotal = 0;

            //Check if PlugNPay exists

            bool PlugExists = Residuals.CheckVendorExistsForRep( "plugnpay");
            //If Rep has at least ONE PlugNPay Account
            if (PlugExists)
            {
                //Rep has a PlugNPay ECE Total (and a PlugNPay Number)
                DataSet dsTotals = Residuals.GetPlugNPayTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEPlugNPayTotal = Convert.ToDouble(drTotal["PlugNPayECETotal"]);
                    ResdPlugNPayTTotal = Convert.ToDouble(drTotal["PlugNPayTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Plug N Pay Gateway";
                //lnkExport.NavigateUrl = "PlugNPay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEPlugNPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdPlugNPayTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Header Row Other
            tr = new TableRow();
            string[] arrColumns3 = { "Other", "ECE Total", "Tier Total" };
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < arrColumns.Length; i++)
            {
                td = new TableCell();
                td.Text = arrColumns3[i];
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "12px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }
            tblResiduals.Rows.Add(tr);

            #endregion

            #region Discover
            tr = new TableRow();
            //******************************Discover******************************
            double ECEDiscoverTotal = 0;
            double ResdDiscoverTTotal = 0;

            //Check if Discover exists            
            bool DiscExists = Residuals.CheckVendorExistsForRep( "disc");
            //If Rep has at least ONE Discover Account
            if (DiscExists)
            {
                //Rep has a Discover ECE Total (and a Discover Number)
                DataSet dsTotals = Residuals.GetDiscoverTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEDiscoverTotal = Convert.ToDouble(drTotal["DiscEceTotal"]);
                    ResdDiscoverTTotal = Convert.ToDouble(drTotal["DiscTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Discover RAP";
                //lnkExport.NavigateUrl = "Disc.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEDiscoverTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdDiscoverTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Check Services
            tr = new TableRow();
            //******************************Check Services******************************
            double ECECSTotal = 0;
            double ResdCSTTotal = 0;

            //Check if CS exists            
            bool CSExists = Residuals.CheckVendorExistsForRep("checkservices");
            //If Rep has at least ONE Misc Account
            if (CSExists)
            {
                //Rep has a CS ECE Total
                DataSet dsTotals = Residuals.GetCSTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECECSTotal = Convert.ToDouble(drTotal["CSEceTotal"]);
                    ResdCSTTotal = Convert.ToDouble(drTotal["CSRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Check Services";
                //lnkExport.NavigateUrl = "CheckServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECECSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdCSTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Gift Card
            tr = new TableRow();
            //******************************Gift Card******************************
            double ECEGCTotal = 0;
            double ResdGCTTotal = 0;

            //Check if Misc exists            
            bool GCExists = Residuals.CheckVendorExistsForRep("giftcardservices");
            //If Rep has at least ONE GC Account
            if (GCExists)
            {
                //Rep has a Misc ECE Total
                DataSet dsTotals = Residuals.GetGCTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEGCTotal = Convert.ToDouble(drTotal["GCEceTotal"]);
                    ResdGCTTotal = Convert.ToDouble(drTotal["GCRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Gift Card Services";
                //lnkExport.NavigateUrl = "GiftCardServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEGCTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdGCTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0
            #endregion

            #region CTCart
            //******************************CTCart******************************
            double ECECTCartTotal = 0;
            double ResdCTCartTTotal = 0;

            //Check if CTCart exists      
            bool CTCartExists = Residuals.CheckVendorExistsForRep( "ctcart");
            //If Rep has at least ONE CTCart Account
            if (CTCartExists)
            {
                //Rep has a CTCart ECE Total (and a CTCart Number)
                DataSet dsTotals = Residuals.GetCTCartTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECECTCartTotal = Convert.ToDouble(drTotal["CTCartEceTotal"]);
                    ResdCTCartTTotal = Convert.ToDouble(drTotal["CTCartTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "CT Cart";
                //lnkExport.NavigateUrl = "CTCart.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECECTCartTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdCTCartTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Merchant Cash Advance
            tr = new TableRow();
            //******************************Merchant Cash Advance******************************
            double ECEMCATotal = 0;
            double ResdMCATTotal = 0;

            //Check if MCA exists            
            bool MCAExists = Residuals.CheckVendorExistsForRep("merchantcashadvance");
            //If Rep has at least ONE McA Account
            if (MCAExists)
            {
                //Rep has a MCA ECE Total
                DataSet dsTotals = Residuals.GetMCATotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEMCATotal = Convert.ToDouble(drTotal["MCAReportEceTotal"]);
                    ResdMCATTotal = Convert.ToDouble(drTotal["MCAReportRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Merchant Cash Advance";
                //lnkExport.NavigateUrl = "MerchantCashAdvance.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEMCATotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdMCATTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Payroll
            tr = new TableRow();
            //******************************Payroll******************************
            double ECEPayrollTotal = 0;
            double ResdPayrollTTotal = 0;

            //Check if Payroll exists            
            bool PayrollExists = Residuals.CheckVendorExistsForRep("payroll");
            //If Rep has at least ONE Payroll Account
            if (PayrollExists)
            {
                //Rep has a Payroll ECE Total
                DataSet dsTotals = Residuals.GetPayrollTotalsT1();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEPayrollTotal = Convert.ToDouble(drTotal["PayrollEceTotal"]);
                    ResdPayrollTTotal = Convert.ToDouble(drTotal["PayrollRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "Payroll";
                //lnkExport.NavigateUrl = "PayrollServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEPayrollTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdPayrollTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Misc - old
            /*tr = new TableRow();
            //******************************Misc******************************
            double ECEMiscTotal = 0;
            double ResdMiscTTotal = 0;

            //Check if Misc exists            
            bool MiscExists = Residuals.CheckVendorExistsForRep( "misc");
            //If Rep has at least ONE Misc Account
            if (MiscExists)
            {
                //Rep has a Misc ECE Total
                DataSet dsTotals = Residuals.GetMiscTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEMiscTotal = Convert.ToDouble(drTotal["MiscReportEceTotal"]);
                    ResdMiscTTotal = Convert.ToDouble(drTotal["MiscReportTTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Misc";
                lnkExport.NavigateUrl = "Misc.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEMiscTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdMiscTTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0
            */
            #endregion

            #region Totals
            //Calculate ECE Total for Rep
            double RepECETotal = ECEIPayGateTotal + ECEIPayTotal + ECEIMSTotal + ECEIMS2Total + ECEWPayTotal + ECEIPayFBBHTotal + ECECPSTotal + ECEInnovativeTotal + ECEECXLegacyTotal + ECEInnGateTotal + ECECTCartTotal + ECEAuthnetTotal + ECEDiscoverTotal + ECEPlugNPayTotal + ECEChaseTotal + ECEOptimalCATotal + ECEMerrickTotal + ECEiPay2Total + ECEiPay3Total + ECECSTotal + ECEGCTotal + ECEMCATotal + ECEPayrollTotal;//+ ECEMiscTotal;

            //Calculate Residual Total for Rep
            double RepResidualTotal = ResdIPayGateTTotal + ResdIPayTTotal + ResdIMSTTotal + ResdIMS2TTotal + ResdWPayTTotal + ResdIPayFBBHTTotal + ResdCPSTTotal + ResdInnovativeTTotal + ResdECXLegacyTTotal + ResdInnGateTTotal + ResdCTCartTTotal + ResdAuthnetTTotal + ResdDiscoverTTotal + ResdPlugNPayTTotal + ResdChaseTTotal + ResdOptimalCATTotal + ResdMerrickTTotal + ResdiPay2TTotal + ResdiPay3TTotal + ResdCSTTotal + ResdGCTTotal + ResdMCATTotal + ResdPayrollTTotal;//+ ResdMiscTTotal;

            tr = new TableRow();

            lblValue = new Label();
            lblValue.Text = "TOTAL";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + RepECETotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + RepResidualTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.WhiteSmoke;
            tblResiduals.Rows.Add(tr);

            #endregion

    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string MasterNum = string.Empty;
            if (Request.Params.Get("MasterNum") != null)
                MasterNum = Request.Params.Get("MasterNum");
            if (MasterNum != "")
                Populate(MasterNum, lstMonth.SelectedItem.Value);
            else
                Populate(Session["MasterNum"].ToString(), lstMonth.SelectedItem.Value);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }//end submit button click

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
