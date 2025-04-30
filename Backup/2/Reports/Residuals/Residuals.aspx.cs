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

public partial class Residuals_Residuals : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Agent") || User.IsInRole("T1Agent") || User.IsInRole("Office"))
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


        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                /*if (User.IsInRole("T1Agent"))
                    lnkTierResiduals.Visible = true;
                else
                    lnkTierResiduals.Visible = false;*/
                MonthBL Mon = new MonthBL();
                ListBL List = new ListBL();

                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    lstRepList.Visible = true;
                    lblRepName.Visible = true;
                    DataSet dsMon = Mon.GetMonthListForReports(1, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    DataSet dsRep = List.GetSalesRepList();
                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = dsRep;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "MasterNum";
                        lstRepList.DataBind();
                        //lstRepList.SelectedValue = "";
                    }

                    //the month and Master Number specified in URL
                    string Month = string.Empty;
                    string MasterNum = string.Empty;
                    if (Request.Params.Get("Month") != null)
                        Month = Request.Params.Get("Month");

                    if (Request.Params.Get("MasterNum") != null)
                        MasterNum = Request.Params.Get("MasterNum");

                    //if Month and MasterNum is specified 
                    if ((Month != "") && (MasterNum != "") )
                    {
                        try
                        {
                            lblError.Visible = false;
                            //Auto select the dropdown values to match URL
                            if (lstRepList.Items.FindByValue(MasterNum) != null)
                                lstRepList.SelectedValue = MasterNum;
                            if (lstMonth.Items.FindByValue(Month) != null)
                                lstMonth.SelectedValue = Month;

                            lblMonth.Text = lstRepList.SelectedValue.ToString() + ": Residuals for the month of: " + lstMonth.SelectedItem.Text;
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
        //set the boundary date for the IMS , this report doesnot exist since OCT 2011

        RepInfoBL repInfo = new RepInfoBL();

        string lastMonthStr = lstMonth.SelectedItem.Value;
        DateTime lastMonthDate = DateTime.Parse(lastMonthStr);
        DateTime dateBound = new DateTime(2011, 10, 1);

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

        lstMonth.SelectedValue = lstMonth.Items.FindByValue(Month).Value;
        lblMonth.Text =  "Residuals for the month of: " + lstMonth.SelectedItem.Text;
                      

        //Display Confirmation Code
        ResidualsBL Residual = new ResidualsBL(Month, MasterNum);
        DataSet dsCode = new DataSet();

        dsCode = Residual.GetConfirmationResd();

        double CarryOver = 0;
        if (dsCode.Tables[0].Rows.Count > 0)
        {
            pnlConfirmation.Visible = true;
            CarryOver = Convert.ToDouble(dsCode.Tables[0].Rows[0]["CarryOverBalance"]);
            string strConfirmDate = dsCode.Tables[0].Rows[0]["ConfirmDate"].ToString().Trim();
            string strConfirmCode =  dsCode.Tables[0].Rows[0]["ConfirmNum"].ToString().Trim();
            string strResdNote = dsCode.Tables[0].Rows[0]["Note"].ToString().Trim();
            string strText = "";
            //if a Confirmation Date exists
            if (strConfirmCode != "")
                strText = "Residual paid on " + strConfirmDate
                                    + " with Confirmation Code: " + strConfirmCode + "<br>";
            //if a Residual Note Exists
            if (strResdNote != "")
                strText = strText + "Note: " + strResdNote;

            lblConfirmation.Text = strText;

        }
        else
            pnlConfirmation.Visible = false;

        ResidualsBL Residuals = new ResidualsBL(Month, MasterNum);

        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
        {
            lblRepName.Visible = true;
            lblRepName.Text = "";
            lstRepList.Visible = true;
        }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        Label lblValue;
        HyperLink lnkExport;

        #region Header Row

        string[] arrColumns = { "Processors / Banks", "ECE Total", "Partner Residual" };
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

        #region Processors

        double ECEIPayTotal = 0;
        double ResdIPayTotal = 0;

        double ECEiPay2Total = 0;
        double ResdiPay2Total = 0;

        double ECEiPay3Total = 0;
        double ResdiPay3Total = 0;

        double ECEIPayFBBHTotal = 0;
        double ResdIPayFBBHTotal = 0;

        double ECEECXLegacyTotal = 0;
        double ResdECXLegacyTotal = 0;

        double ECEIMSTotal = 0;
        double ResdIMSTotal = 0;

        double ECEIPSTotal = 0;
        double ResdIPSTotal = 0;

        double ECEIMS2Total = 0;
        double ResdIMS2Total = 0;

        double ECESageTotal = 0;
        double ResdSageTotal = 0;

        double ECEInnovativeTotal = 0;
        double ResdInnovativeTotal = 0;

        double ECECPSTotal = 0;
        double ResdCPSTotal = 0;

        double ECEChaseTotal = 0;
        double ResdChaseTotal = 0;

        double ECEMerrickTotal = 0;
        double ResdMerrickTotal = 0;

        double ECEOptimalCATotal = 0;
        double ResdOptimalCATotal = 0;

        double ECEWPayTotal = 0;
        double ResdWPayTotal = 0;

        //Check if User is in Office Role
        //If Rep has an Ipayment Number

        if (User.IsInRole("Office"))
        {
            DataSet dsOfficeAgentnum = repInfo.ReturnOfficeAgentMasterNum(MasterNum);
            if (dsOfficeAgentnum.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsOfficeAgentnum.Tables[0].Rows.Count; i++)
                {
                    DataRow drOfficeAgentnum = dsOfficeAgentnum.Tables[0].Rows[i];
                    ResidualsBL Residuals1 = new ResidualsBL(Month, Convert.ToString(drOfficeAgentnum["MasterNum"]));

                    DataSet dsTotals;

                    #region iPay
                    if (Residuals1.ReturnIPayNum() != "")
                    {
                        ECEIPayTotal = 0;
                        ResdIPayTotal = 0;

                        dsTotals = Residuals1.GetiPayTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEIPayTotal = Convert.ToDouble(drTotal["IPayEceTotal"]);
                            ResdIPayTotal = Convert.ToDouble(drTotal["IPayRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "iPayment, Inc.(1503 Portfolio) - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "IPay.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEIPayTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdIPayTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }
                    #endregion

                    #region iPay2
                    //Rep has a iPay2 ECE Total (and a iPay2 Number)
                    if (Residuals1.ReturnIPay2Num() != "")
                    {
                        ECEiPay2Total = 0;
                        ResdiPay2Total = 0;

                        dsTotals = Residuals1.GetiPay2Totals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEiPay2Total = Convert.ToDouble(drTotal["iPay2EceTotal"]);
                            ResdiPay2Total = Convert.ToDouble(drTotal["iPay2RepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "iPayment, Inc.(40558 Portfolio) - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "iPay2.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEiPay2Total.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdiPay2Total.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }
                    #endregion

                    #region iPay3
                    //Rep has a iPay3 ECE Total (and a iPay3 Number)
                    if (Residuals1.ReturnIPay3Num() != "")
                    {

                        ECEiPay3Total = 0 ;
                        ResdiPay3Total = 0 ;
                        dsTotals = Residuals1.GetiPay3Totals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEiPay3Total = Convert.ToDouble(drTotal["iPay3EceTotal"]);
                            ResdiPay3Total = Convert.ToDouble(drTotal["iPay3RepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "iPayment, Inc. - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "iPay3.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEiPay3Total.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdiPay3Total.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }

                    bool FBBHExists = Residuals1.CheckVendorExistsForRep("ipayfbbh");
                    //If Rep has at least ONE IPayFBBH Account
                    if (FBBHExists)
                    {
                        //Rep has a IPayFBBH ECE Total (and a IPayFBBH Number)
                        dsTotals = Residuals1.GetIPayFBBHTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEIPayFBBHTotal = Convert.ToDouble(drTotal["IPayFBBHEceTotal"]);
                            ResdIPayFBBHTotal = Convert.ToDouble(drTotal["IPayFBBHRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "iPayment FBBH";
                        lnkExport.NavigateUrl = "IPayFBBH.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEIPayFBBHTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdIPayFBBHTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region ECXLegacy
                    if (Month.Contains("2005"))
                    {
                        //Check if ECXLegacy exists
                        bool ECXLegExists = Residuals1.CheckVendorExistsForRep("ECXLegacy");
                        //If Rep has at least ONE ECX Legacy Account
                        if (ECXLegExists)
                        {

                            ECEECXLegacyTotal = 0;
                            ResdECXLegacyTotal = 0;
                            //Rep has a ECXLegacy ECE Total (and a ECXLegacy Number)
                            dsTotals = Residuals.GetECXLegacyTotals();
                            if (dsTotals.Tables[0].Rows.Count > 0)
                            {
                                DataRow drTotal = dsTotals.Tables[0].Rows[0];
                                ECEECXLegacyTotal = Convert.ToDouble(drTotal["ECXLegacyEceTotal"]);
                                ResdECXLegacyTotal = Convert.ToDouble(drTotal["ECXLegacyRepTotal"]);
                            }//end if count not 0

                            tr = new TableRow();
                            lnkExport = new HyperLink();
                            lnkExport.Text = "ECXLegacy - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                            lnkExport.NavigateUrl = "ECXLegacy.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                            td = new TableCell();
                            lnkExport.ApplyStyle(sHyperLink);
                            td.Controls.Add(lnkExport);
                            td.Attributes.Add("align", "left");
                            tr.Cells.Add(td);

                            lblValue = new Label();
                            lblValue.Text = ECEECXLegacyTotal.ToString().Trim();
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);

                            lblValue = new Label();
                            lblValue.Text = ResdECXLegacyTotal.ToString().Trim();
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);

                            tblResiduals.Rows.Add(tr);
                        }//end if count not 0
                    }//end if month contains 2005
                    #endregion

                    #region IMS
                    if ((Residuals1.ReturnIMSNum() != "") && ((lastMonthDate.CompareTo(dateBound)) < 0))
                    {
                        ECEIMSTotal = 0;
                        ResdIMSTotal = 0;
                        //Rep has a IMS ECE Total (and a IMS Number)
                        dsTotals = Residuals1.GetIMSTotals();
                        if ((dsTotals.Tables[0].Rows.Count > 0) && ((lastMonthDate.CompareTo(dateBound)) < 0))
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEIMSTotal = Convert.ToDouble(drTotal["IMSEceTotal"]);
                            ResdIMSTotal = Convert.ToDouble(drTotal["IMSRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "IMS - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "IMS.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEIMSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdIMSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if num exists
                    #endregion

                    #region IPS
                    //Check if IPS exists
                    //If Rep has at least ONE IPS Account
                    if (Residuals1.ReturnIPSNum() != "")
                    {
                        ECEIPSTotal = 0;
                        ResdIPSTotal = 0;
                        //Rep has a IPS ECE Total (and a IPS Number)
                        dsTotals = Residuals1.GetIPSTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEIPSTotal = Convert.ToDouble(drTotal["IPSEceTotal"]);
                            ResdIPSTotal = Convert.ToDouble(drTotal["IPSRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "IPS - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "IPS.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEIPSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdIPSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if num exists
                    #endregion

                    #region IMS2
                    //Check if IMS2 exists
                    //If Rep has at least an IMS Number
                    if (Residuals1.ReturnIMS2Num() != "")
                    {
                        ECEIMS2Total = 0;
                        ResdIMS2Total = 0;
                        //Rep has a IMS2 ECE Total (and a IMS2 Number)
                        dsTotals = Residuals1.GetIMS2Totals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEIMS2Total = Convert.ToDouble(drTotal["IMS2EceTotal"]);
                            ResdIMS2Total = Convert.ToDouble(drTotal["IMS2RepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "IMS(QB) - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "IMS2.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEIMS2Total.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdIMS2Total.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region Sage
                    //Check if Sage exists
                    //If Rep has at least an Sage Number
                    if (Residuals1.ReturnSageNum() != "")
                    {
                        ECESageTotal = 0;
                        ResdSageTotal = 0;
                        //Rep has a Sage ECE Total (and a Sage Number)
                        dsTotals = Residuals1.GetSageTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECESageTotal = Convert.ToDouble(drTotal["SageEceTotal"]);
                            ResdSageTotal = Convert.ToDouble(drTotal["SageRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Sage - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "Sage.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECESageTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdSageTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region innovative
                    //Check if Innovative exists
                    bool InnExists = Residuals1.CheckVendorExistsForRep("innovative");
                    //If Rep has at least ONE Innovative Account
                    if (InnExists)
                    {
                        ECEInnovativeTotal = 0;
                        ResdInnovativeTotal = 0;
                        //Rep has a Innovative ECE Total (and a Innovative Number)
                        dsTotals = Residuals1.GetInnovativeTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEInnovativeTotal = Convert.ToDouble(drTotal["InnEceTotal"]);
                            ResdInnovativeTotal = Convert.ToDouble(drTotal["InnRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Innovative Merchant Solutions - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "Innovative.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEInnovativeTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdInnovativeTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region CPS
                    bool CPSExists = Residuals1.CheckVendorExistsForRep("cps");
                    //If Rep has at least ONE CPS Account
                    if (CPSExists)
                    {
                        ECECPSTotal = 0;
                        ResdCPSTotal = 0;
                        //Rep has a CPS ECE Total (and a CPS Number)
                        dsTotals = Residuals1.GetCPSTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECECPSTotal = Convert.ToDouble(drTotal["CPSEceTotal"]);
                            ResdCPSTotal = Convert.ToDouble(drTotal["CPSRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "CPS - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "cps.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECECPSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdCPSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region Chase
                    //Check if Chase exists
                    bool ChaseExists = Residuals1.CheckVendorExistsForRep("Chase");
                    //If Rep has at least ONE WPay Account
                    if (ChaseExists)
                    {
                        ECEChaseTotal = 0;
                        ResdChaseTotal = 0;
                        //Rep has a Chase ECE Total (and a Chase Number)
                        dsTotals = Residuals1.GetChaseTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEChaseTotal = Convert.ToDouble(drTotal["ChaseEceTotal"]);
                            ResdChaseTotal = Convert.ToDouble(drTotal["ChaseRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "CardConnect - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "Chase.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEChaseTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdChaseTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region Merrick
                    //Check if Merrick exists
                    bool MerrickExists = Residuals1.CheckVendorExistsForRep("Merrick");
                    //If Rep has at least ONE WPay Account
                    if (MerrickExists)
                    {
                        ECEMerrickTotal = 0;
                        ResdMerrickTotal = 0;
                        //Rep has a Merrick ECE Total (and a Merrick Number)
                        dsTotals = Residuals1.GetMerrickTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEMerrickTotal = Convert.ToDouble(drTotal["MerrickEceTotal"]);
                            ResdMerrickTotal = Convert.ToDouble(drTotal["MerrickRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Merrick - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "Merrick.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEMerrickTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdMerrickTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region Optimal
                    //Check if OptimalCA exists
                    bool OptCAExists = Residuals1.CheckVendorExistsForRep("OptimalCA");
                    //If Rep has at least ONE WPay Account
                    if (OptCAExists)
                    {
                        ECEOptimalCATotal = 0;
                        ResdOptimalCATotal = 0;
                        //Rep has a OptimalCA ECE Total (and a OptimalCA Number)
                        dsTotals = Residuals1.GetOptimalCATotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEOptimalCATotal = Convert.ToDouble(drTotal["OptimalCAEceTotal"]);
                            ResdOptimalCATotal = Convert.ToDouble(drTotal["OptimalCARepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Optimal CA - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "OptimalCA.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEOptimalCATotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdOptimalCATotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion

                    #region WPay
                    //Check if WPay exists
                    bool WPayExists = Residuals1.CheckVendorExistsForRep("WPay");
                    //If Rep has at least ONE WPay Account
                    if (WPayExists)
                    {
                        ECEWPayTotal = 0;
                        ResdWPayTotal = 0;
                        //Rep has a WPay ECE Total (and a WPay Number)
                        dsTotals = Residuals1.GetWPayTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEWPayTotal = Convert.ToDouble(drTotal["WPayEceTotal"]);
                            ResdWPayTotal = Convert.ToDouble(drTotal["WPayRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "WorldPay - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "WPay.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEWPayTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdWPayTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0
                    #endregion


                }
            }
        }
        else
        {
            if (Residuals.ReturnIPayNum() != "")
            {
                //Rep has a IPay ECE Total (and a IPay Number)
                DataSet dsTotals = Residuals.GetiPayTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPayTotal = Convert.ToDouble(drTotal["IPayEceTotal"]);
                    ResdIPayTotal = Convert.ToDouble(drTotal["IPayRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "iPayment, Inc.(1503 Portfolio)";
                lnkExport.NavigateUrl = "IPay.aspx?MasterNum=" + Residuals.ReturnIPayNum() + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            if (Residuals.ReturnIPay2Num() != "")
            {
                //Rep has a iPay2 ECE Total (and a iPay2 Number)
                DataSet dsTotals = Residuals.GetiPay2Totals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEiPay2Total = Convert.ToDouble(drTotal["iPay2EceTotal"]);
                    ResdiPay2Total = Convert.ToDouble(drTotal["iPay2RepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "iPayment, Inc.(40558 Portfolio)";
                lnkExport.NavigateUrl = "iPay2.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEiPay2Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdiPay2Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            if (Residuals.ReturnIPay3Num() != "")
            {
                //Rep has a iPay3 ECE Total (and a iPay3 Number)
                DataSet dsTotals = Residuals.GetiPay3Totals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEiPay3Total = Convert.ToDouble(drTotal["iPay3EceTotal"]);
                    ResdiPay3Total = Convert.ToDouble(drTotal["iPay3RepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "iPayment, Inc.";
                lnkExport.NavigateUrl = "iPay3.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEiPay3Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdiPay3Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            bool FBBHExists = Residuals.CheckVendorExistsForRep("ipayfbbh");
            //If Rep has at least ONE IPayFBBH Account
            if (FBBHExists)
            {
                //Rep has a IPayFBBH ECE Total (and a IPayFBBH Number)
                DataSet dsTotals = Residuals.GetIPayFBBHTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPayFBBHTotal = Convert.ToDouble(drTotal["IPayFBBHEceTotal"]);
                    ResdIPayFBBHTotal = Convert.ToDouble(drTotal["IPayFBBHRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "iPayment FBBH";
                lnkExport.NavigateUrl = "IPayFBBH.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPayFBBHTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPayFBBHTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            if (Month.Contains("2005"))
            {
                //Check if ECXLegacy exists
                bool ECXLegExists = Residuals.CheckVendorExistsForRep("ECXLegacy");
                //If Rep has at least ONE ECX Legacy Account
                if (ECXLegExists)
                {
                    //Rep has a ECXLegacy ECE Total (and a ECXLegacy Number)
                    DataSet dsTotals = Residuals.GetECXLegacyTotals();
                    if (dsTotals.Tables[0].Rows.Count > 0)
                    {
                        DataRow drTotal = dsTotals.Tables[0].Rows[0];
                        ECEECXLegacyTotal = Convert.ToDouble(drTotal["ECXLegacyEceTotal"]);
                        ResdECXLegacyTotal = Convert.ToDouble(drTotal["ECXLegacyRepTotal"]);
                    }//end if count not 0

                    tr = new TableRow();
                    lnkExport = new HyperLink();
                    lnkExport.Text = "ECXLegacy";
                    lnkExport.NavigateUrl = "ECXLegacy.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = ECEECXLegacyTotal.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = ResdECXLegacyTotal.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    tblResiduals.Rows.Add(tr);
                }//end if count not 0
            }//end if month contains 2005


            if ((Residuals.ReturnIMSNum() != "") && ((lastMonthDate.CompareTo(dateBound)) < 0))
            {
                //Rep has a IMS ECE Total (and a IMS Number)
                DataSet dsTotals = Residuals.GetIMSTotals();
                if ((dsTotals.Tables[0].Rows.Count > 0) && ((lastMonthDate.CompareTo(dateBound)) < 0))
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIMSTotal = Convert.ToDouble(drTotal["IMSEceTotal"]);
                    ResdIMSTotal = Convert.ToDouble(drTotal["IMSRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "IMS";
                lnkExport.NavigateUrl = "IMS.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIMSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIMSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if num exists

            //Check if IPS exists
            //If Rep has at least ONE IPS Account
            if (Residuals.ReturnIPSNum() != "")
            {
                //Rep has a IPS ECE Total (and a IPS Number)
                DataSet dsTotals = Residuals.GetIPSTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPSTotal = Convert.ToDouble(drTotal["IPSEceTotal"]);
                    ResdIPSTotal = Convert.ToDouble(drTotal["IPSRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "IPS";
                lnkExport.NavigateUrl = "IPS.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if num exists

            //Check if IMS2 exists
            //If Rep has at least an IMS Number
            if (Residuals.ReturnIMS2Num() != "")
            {
                //Rep has a IMS2 ECE Total (and a IMS2 Number)
                DataSet dsTotals = Residuals.GetIMS2Totals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIMS2Total = Convert.ToDouble(drTotal["IMS2EceTotal"]);
                    ResdIMS2Total = Convert.ToDouble(drTotal["IMS2RepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "IMS(QB)";
                lnkExport.NavigateUrl = "IMS2.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIMS2Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIMS2Total.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            //Check if Sage exists
            //If Rep has at least an Sage Number
            if (Residuals.ReturnSageNum() != "")
            {
                //Rep has a Sage ECE Total (and a Sage Number)
                DataSet dsTotals = Residuals.GetSageTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECESageTotal = Convert.ToDouble(drTotal["SageEceTotal"]);
                    ResdSageTotal = Convert.ToDouble(drTotal["SageRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Sage";
                lnkExport.NavigateUrl = "Sage.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECESageTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdSageTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0


            //Check if Innovative exists
            bool InnExists = Residuals.CheckVendorExistsForRep("innovative");
            //If Rep has at least ONE Innovative Account
            if (InnExists)
            {
                //Rep has a Innovative ECE Total (and a Innovative Number)
                DataSet dsTotals = Residuals.GetInnovativeTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEInnovativeTotal = Convert.ToDouble(drTotal["InnEceTotal"]);
                    ResdInnovativeTotal = Convert.ToDouble(drTotal["InnRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Innovative Merchant Solutions";
                lnkExport.NavigateUrl = "Innovative.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEInnovativeTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdInnovativeTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            //Check if CPS exists

            bool CPSExists = Residuals.CheckVendorExistsForRep("cps");
            //If Rep has at least ONE CPS Account
            if (CPSExists)
            {
                //Rep has a CPS ECE Total (and a CPS Number)
                DataSet dsTotals = Residuals.GetCPSTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECECPSTotal = Convert.ToDouble(drTotal["CPSEceTotal"]);
                    ResdCPSTotal = Convert.ToDouble(drTotal["CPSRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "CPS";
                lnkExport.NavigateUrl = "cps.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECECPSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdCPSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            //Check if Chase exists
            bool ChaseExists = Residuals.CheckVendorExistsForRep("Chase");
            //If Rep has at least ONE WPay Account
            if (ChaseExists)
            {
                //Rep has a Chase ECE Total (and a Chase Number)
                DataSet dsTotals = Residuals.GetChaseTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEChaseTotal = Convert.ToDouble(drTotal["ChaseEceTotal"]);
                    ResdChaseTotal = Convert.ToDouble(drTotal["ChaseRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Chase";
                lnkExport.NavigateUrl = "Chase.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEChaseTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdChaseTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0


            //Check if Merrick exists
            bool MerrickExists = Residuals.CheckVendorExistsForRep("Merrick");
            //If Rep has at least ONE WPay Account
            if (MerrickExists)
            {
                //Rep has a Merrick ECE Total (and a Merrick Number)
                DataSet dsTotals = Residuals.GetMerrickTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEMerrickTotal = Convert.ToDouble(drTotal["MerrickEceTotal"]);
                    ResdMerrickTotal = Convert.ToDouble(drTotal["MerrickRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Merrick";
                lnkExport.NavigateUrl = "Merrick.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEMerrickTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdMerrickTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            //Check if OptimalCA exists
            bool OptCAExists = Residuals.CheckVendorExistsForRep("OptimalCA");
            //If Rep has at least ONE WPay Account
            if (OptCAExists)
            {
                //Rep has a OptimalCA ECE Total (and a OptimalCA Number)
                DataSet dsTotals = Residuals.GetOptimalCATotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEOptimalCATotal = Convert.ToDouble(drTotal["OptimalCAEceTotal"]);
                    ResdOptimalCATotal = Convert.ToDouble(drTotal["OptimalCARepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Optimal CA";
                lnkExport.NavigateUrl = "OptimalCA.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEOptimalCATotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdOptimalCATotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            //Check if WPay exists
            bool WPayExists = Residuals.CheckVendorExistsForRep("WPay");
            //If Rep has at least ONE WPay Account
            if (WPayExists)
            {
                //Rep has a WPay ECE Total (and a WPay Number)
                DataSet dsTotals = Residuals.GetWPayTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEWPayTotal = Convert.ToDouble(drTotal["WPayEceTotal"]);
                    ResdWPayTotal = Convert.ToDouble(drTotal["WPayRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "WorldPay";
                lnkExport.NavigateUrl = "WPay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEWPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdWPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0


        }

        #endregion

        #region iPay2

        //******************************iPay2******************************
        

        //Check if iPay2 exists
        //If Rep has at least ONE iPay2 Account
        

        #endregion

        #region iPay3

        //******************************iPay3******************************
        

        //Check if iPay3 exists
        //If Rep has at least ONE iPay3 Account
        

        #endregion

        #region IPayFBBH
        //******************************IPayFBBH******************************
       

        //Check if IPayFBBH exists

        
        #endregion

        #region ECXLegacy

        #endregion        

        #region IMS

        //******************************IMS******************************
        

        //Check if IMS exists
        //If Rep has at least ONE IMS Account
        

        #endregion

        #region IPS

        //******************************IPS******************************
        

        

        #endregion

        #region IMS2

        //******************************IMS2******************************
        

        

        #endregion

        #region Sage

        //******************************Sage******************************
       

       

        #endregion

        #region Innovative
        //******************************Innovative******************************
        

       

        #endregion

        #region CPS
        //******************************CPS******************************
        

        
        #endregion
        
        #region Chase

        //******************************Chase******************************
        

       

        #endregion

        #region Merrick

        //******************************Merrick******************************
       

        
        #endregion

        #region OptimalCA

        //******************************OptimalCA******************************
        

        

        #endregion        
                        
        #region WPay

        //******************************WPay******************************
        

        

        #endregion

        #region Header Row Payment Gateways
        tr = new TableRow();
        string[] arrColumns2 = { "Payment Gateways", "ECE Total", "Partner Residual" };
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

        double ECEAuthnetTotal = 0;
        double ResdAuthnetTotal = 0;

        double ECEIPayGateTotal = 0;
        double ResdIPayGateTotal = 0;

        double ECEInnGateTotal = 0;
        double ResdInnGateTotal = 0;

        double ECEPlugNPayTotal = 0;
        double ResdPlugNPayTotal = 0;

        if (User.IsInRole("Office"))
        {
            DataSet dsOfficeAgentnum = repInfo.ReturnOfficeAgentMasterNum(MasterNum);
            if (dsOfficeAgentnum.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsOfficeAgentnum.Tables[0].Rows.Count; i++)
                {
                    DataRow drOfficeAgentnum = dsOfficeAgentnum.Tables[0].Rows[i];
                    ResidualsBL Residuals1 = new ResidualsBL(Month, Convert.ToString(drOfficeAgentnum["MasterNum"]));


                    #region Authnet
                    //******************************Authnet******************************
                    

                    //Check if Authnet exists
                    bool AuthExists = Residuals1.CheckVendorExistsForRep("Authnet");
                    //If Rep has at least ONE Authnet Account
                    if (AuthExists)
                    {
                        ECEAuthnetTotal = 0;
                        ResdAuthnetTotal = 0;
                        //Rep has a Authnet ECE Total (and a Authnet Number)
                        DataSet dsTotals = Residuals1.GetAuthnetTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEAuthnetTotal = Convert.ToDouble(drTotal["AuthnetEceTotal"]);
                            ResdAuthnetTotal = Convert.ToDouble(drTotal["AuthnetRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Authorize.Net - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "Authnet.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEAuthnetTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdAuthnetTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0

                    #endregion

                    #region IPayGate
                    //******************************IPayGate******************************
                    

                    //Check if IPayGate exists
                    bool IPayGateExists = Residuals1.CheckVendorExistsForRep("ipaygate");
                    //If Rep has at least ONE IPayGate Account
                    if (IPayGateExists)
                    {
                        ECEIPayGateTotal = 0;
                        ResdIPayGateTotal = 0;
                        //Rep has a IPayGate ECE Total (and a IPayGate Number)
                        DataSet dsTotals = Residuals1.GetIPayGateTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEIPayGateTotal = Convert.ToDouble(drTotal["IPayGateEceTotal"]);
                            ResdIPayGateTotal = Convert.ToDouble(drTotal["IPayGateRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "iPayment Gateway - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "IPayGate.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEIPayGateTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdIPayGateTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0

                    #endregion

                    #region InnGate
                    //******************************InnGate******************************
                    

                    //Check if InnGate exists

                    bool InnGateExists = Residuals.CheckVendorExistsForRep("inngate");
                    //If Rep has at least ONE InnGate Account
                    if (InnGateExists)
                    {
                        ECEInnGateTotal = 0;
                        ResdInnGateTotal = 0;
                        //Rep has a InnGate ECE Total (and a InnGate Number)
                        DataSet dsTotals = Residuals.GetInnGateTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEInnGateTotal = Convert.ToDouble(drTotal["InnGateEceTotal"]);
                            ResdInnGateTotal = Convert.ToDouble(drTotal["InnGateRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Innovative Gateway - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "InnGate.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEInnGateTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdInnGateTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0

                    #endregion

                    #region PlugNPay
                    //******************************PlugNPay******************************
                    

                    //Check if PlugNPay exists

                    bool PlugExists = Residuals.CheckVendorExistsForRep("plugnpay");
                    //If Rep has at least ONE PlugNPay Account
                    if (PlugExists)
                    {
                        ECEPlugNPayTotal = 0;
                        ResdPlugNPayTotal = 0;
                        //Rep has a PlugNPay ECE Total (and a PlugNPay Number)
                        DataSet dsTotals = Residuals.GetPlugNPayTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEPlugNPayTotal = Convert.ToDouble(drTotal["PlugNPayECETotal"]);
                            ResdPlugNPayTotal = Convert.ToDouble(drTotal["PlugNPayRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Plug N Pay Gateway - " + Convert.ToString(drOfficeAgentnum["RepName"]);
                        lnkExport.NavigateUrl = "PlugNPay.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEPlugNPayTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdPlugNPayTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0

                    #endregion
                }
            }
        }
        else {
            #region Authnet
            //******************************Authnet******************************
           

            //Check if Authnet exists
            bool AuthExists = Residuals.CheckVendorExistsForRep("Authnet");
            //If Rep has at least ONE Authnet Account
            if (AuthExists)
            {
                //Rep has a Authnet ECE Total (and a Authnet Number)
                DataSet dsTotals = Residuals.GetAuthnetTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEAuthnetTotal = Convert.ToDouble(drTotal["AuthnetEceTotal"]);
                    ResdAuthnetTotal = Convert.ToDouble(drTotal["AuthnetRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Authorize.Net";
                lnkExport.NavigateUrl = "Authnet.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEAuthnetTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdAuthnetTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region IPayGate
            //******************************IPayGate******************************
            

            //Check if IPayGate exists
            bool IPayGateExists = Residuals.CheckVendorExistsForRep("ipaygate");
            //If Rep has at least ONE IPayGate Account
            if (IPayGateExists)
            {
                //Rep has a IPayGate ECE Total (and a IPayGate Number)
                DataSet dsTotals = Residuals.GetIPayGateTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEIPayGateTotal = Convert.ToDouble(drTotal["IPayGateEceTotal"]);
                    ResdIPayGateTotal = Convert.ToDouble(drTotal["IPayGateRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "iPayment Gateway";
                lnkExport.NavigateUrl = "IPayGate.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEIPayGateTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdIPayGateTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region InnGate
            //******************************InnGate******************************
            

            //Check if InnGate exists

            bool InnGateExists = Residuals.CheckVendorExistsForRep("inngate");
            //If Rep has at least ONE InnGate Account
            if (InnGateExists)
            {
                //Rep has a InnGate ECE Total (and a InnGate Number)
                DataSet dsTotals = Residuals.GetInnGateTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEInnGateTotal = Convert.ToDouble(drTotal["InnGateEceTotal"]);
                    ResdInnGateTotal = Convert.ToDouble(drTotal["InnGateRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Innovative Gateway";
                lnkExport.NavigateUrl = "InnGate.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEInnGateTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdInnGateTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region PlugNPay
            //******************************PlugNPay******************************
            //Check if PlugNPay exists

            bool PlugExists = Residuals.CheckVendorExistsForRep("plugnpay");
            //If Rep has at least ONE PlugNPay Account
            if (PlugExists)
            {
                //Rep has a PlugNPay ECE Total (and a PlugNPay Number)
                DataSet dsTotals = Residuals.GetPlugNPayTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEPlugNPayTotal = Convert.ToDouble(drTotal["PlugNPayECETotal"]);
                    ResdPlugNPayTotal = Convert.ToDouble(drTotal["PlugNPayRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Plug N Pay Gateway";
                lnkExport.NavigateUrl = "PlugNPay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEPlugNPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdPlugNPayTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion
        }


        double ECEDiscoverTotal = 0;
        double ResdDiscoverTotal = 0;

        double ECECSTotal = 0;
        double ResdCSTotal = 0;

        double ECEGCTotal = 0;
        double ResdGCTotal = 0;

        double ECECTCartTotal = 0;
        double ResdCTCartTotal = 0;

        double ECEMCATotal = 0;
        double ResdMCATotal = 0;

        double ECEPayrollTotal = 0;
        double ResdPayrollTotal = 0;


        #region Header Row Other
        tr = new TableRow();
        string[] arrColumns3 = { "Other", "ECE Total", "Partner Residual" };
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

        if (User.IsInRole("Office"))
        {
            DataSet dsOfficeAgentnum = repInfo.ReturnOfficeAgentMasterNum(MasterNum);
            if (dsOfficeAgentnum.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsOfficeAgentnum.Tables[0].Rows.Count; i++)
                {
                    DataRow drOfficeAgentnum = dsOfficeAgentnum.Tables[0].Rows[i];
                    ResidualsBL Residuals1 = new ResidualsBL(Month, Convert.ToString(drOfficeAgentnum["MasterNum"]));

                    #region Discover
                    tr = new TableRow();
                    //******************************Discover******************************


                    //Check if Discover exists            
                    bool DiscExists = Residuals1.CheckVendorExistsForRep("disc");
                    //If Rep has at least ONE Discover Account
                    if (DiscExists)
                    {
                        ECEDiscoverTotal = 0;
                        ResdDiscoverTotal = 0;
                        //Rep has a Discover ECE Total (and a Discover Number)
                        DataSet dsTotals = Residuals1.GetDiscoverTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEDiscoverTotal = Convert.ToDouble(drTotal["DiscEceTotal"]);
                            ResdDiscoverTotal = Convert.ToDouble(drTotal["DiscRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Discover RAP";
                        lnkExport.NavigateUrl = "Disc.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEDiscoverTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdDiscoverTotal.ToString().Trim();
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


                    //Check if Misc exists            
                    bool CSExists = Residuals1.CheckVendorExistsForRep("checkservices");
                    //If Rep has at least ONE Misc Account
                    if (CSExists)
                    {
                        ECECSTotal = 0;
                        ResdCSTotal = 0;
                        //Rep has a CS ECE Total
                        DataSet dsTotals = Residuals1.GetCSTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECECSTotal = Convert.ToDouble(drTotal["CSEceTotal"]);
                            ResdCSTotal = Convert.ToDouble(drTotal["CSRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Check Services";
                        lnkExport.NavigateUrl = "CheckServices.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECECSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdCSTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0

                    #endregion

                    #region Gift Card
                    tr = new TableRow();
                    //******************************GC******************************


                    //Check if GC exists            
                    bool GCExists = Residuals1.CheckVendorExistsForRep("giftcardservices");
                    //If Rep has at least ONE GC Account
                    if (GCExists)
                    {
                        ECEGCTotal = 0;
                        ResdGCTotal = 0;
                        //Rep has a GC ECE Total
                        DataSet dsTotals = Residuals1.GetGCTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEGCTotal = Convert.ToDouble(drTotal["GCEceTotal"]);
                            ResdGCTotal = Convert.ToDouble(drTotal["GCRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Gift Card Services";
                        lnkExport.NavigateUrl = "GiftCardServices.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEGCTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdGCTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        tblResiduals.Rows.Add(tr);
                    }//end if count not 0

                    #endregion

                    #region CTCart
                    //******************************CTCart******************************


                    //Check if CTCart exists      
                    bool CTCartExists = Residuals1.CheckVendorExistsForRep("ctcart");
                    //If Rep has at least ONE CTCart Account
                    if (CTCartExists)
                    {
                        ECECTCartTotal = 0;
                        ResdCTCartTotal = 0;
                        //Rep has a CTCart ECE Total (and a CTCart Number)
                        DataSet dsTotals = Residuals1.GetCTCartTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECECTCartTotal = Convert.ToDouble(drTotal["CTCartEceTotal"]);
                            ResdCTCartTotal = Convert.ToDouble(drTotal["CTCartRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "CT Cart";
                        lnkExport.NavigateUrl = "CTCart.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECECTCartTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdCTCartTotal.ToString().Trim();
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


                    //Check if MCA exists            
                    bool MCAExists = Residuals1.CheckVendorExistsForRep("merchantcashadvance");
                    //If Rep has at least ONE MCA Account
                    if (MCAExists)
                    {
                        ECEMCATotal = 0;
                        ResdMCATotal = 0;
                        //Rep has a MCA ECE Total
                        DataSet dsTotals = Residuals1.GetMCATotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEMCATotal = Convert.ToDouble(drTotal["MCAReportEceTotal"]);
                            ResdMCATotal = Convert.ToDouble(drTotal["MCAReportRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Merchant Cash Advance";
                        lnkExport.NavigateUrl = "MerchantCashAdvance.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEMCATotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdMCATotal.ToString().Trim();
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


                    //Check if Payroll exists            
                    bool PayrollExists = Residuals1.CheckVendorExistsForRep("payroll");
                    //If Rep has at least ONE Misc Account
                    if (PayrollExists)
                    {
                        ECEPayrollTotal = 0;
                        ResdPayrollTotal = 0;
                        //Rep has a Misc ECE Total
                        DataSet dsTotals = Residuals1.GetPayrollTotals();
                        if (dsTotals.Tables[0].Rows.Count > 0)
                        {
                            DataRow drTotal = dsTotals.Tables[0].Rows[0];
                            ECEPayrollTotal = Convert.ToDouble(drTotal["PayrollEceTotal"]);
                            ResdPayrollTotal = Convert.ToDouble(drTotal["PayrollRepTotal"]);
                        }//end if count not 0

                        tr = new TableRow();
                        lnkExport = new HyperLink();
                        lnkExport.Text = "Payroll";
                        lnkExport.NavigateUrl = "PayrollServices.aspx?MasterNum=" + Convert.ToString(drOfficeAgentnum["MasterNum"]) + "&Month=" + Month;
                        td = new TableCell();
                        lnkExport.ApplyStyle(sHyperLink);
                        td.Controls.Add(lnkExport);
                        td.Attributes.Add("align", "left");
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ECEPayrollTotal.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = ResdPayrollTotal.ToString().Trim();
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
        double ResdMiscTotal = 0;

        //Check if Misc exists            
        bool MiscExists = Residuals.CheckVendorExistsForRep("misc");
        //If Rep has at least ONE Misc Account
        if (MiscExists)
        {
            //Rep has a Misc ECE Total
            DataSet dsTotals = Residuals.GetMiscTotals();
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow drTotal = dsTotals.Tables[0].Rows[0];
                ECEMiscTotal = Convert.ToDouble(drTotal["MiscReportEceTotal"]);
                ResdMiscTotal = Convert.ToDouble(drTotal["MiscReportRepTotal"]);
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
            lblValue.Text = ResdMiscTotal.ToString().Trim();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tblResiduals.Rows.Add(tr);
        }//end if count not 0
        */
                    #endregion


                }
            }
        }
        else {
            #region Discover
            tr = new TableRow();
            //******************************Discover******************************

            //Check if Discover exists            
            bool DiscExists = Residuals.CheckVendorExistsForRep("disc");
            //If Rep has at least ONE Discover Account
            if (DiscExists)
            {
                //Rep has a Discover ECE Total (and a Discover Number)
                DataSet dsTotals = Residuals.GetDiscoverTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEDiscoverTotal = Convert.ToDouble(drTotal["DiscEceTotal"]);
                    ResdDiscoverTotal = Convert.ToDouble(drTotal["DiscRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Discover RAP";
                lnkExport.NavigateUrl = "Disc.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEDiscoverTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdDiscoverTotal.ToString().Trim();
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

            //Check if Misc exists            
            bool CSExists = Residuals.CheckVendorExistsForRep("checkservices");
            //If Rep has at least ONE Misc Account
            if (CSExists)
            {
                //Rep has a CS ECE Total
                DataSet dsTotals = Residuals.GetCSTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECECSTotal = Convert.ToDouble(drTotal["CSEceTotal"]);
                    ResdCSTotal = Convert.ToDouble(drTotal["CSRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Check Services";
                lnkExport.NavigateUrl = "CheckServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECECSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdCSTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region Gift Card
            tr = new TableRow();
            //******************************GC******************************

            //Check if GC exists            
            bool GCExists = Residuals.CheckVendorExistsForRep("giftcardservices");
            //If Rep has at least ONE GC Account
            if (GCExists)
            {
                //Rep has a GC ECE Total
                DataSet dsTotals = Residuals.GetGCTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEGCTotal = Convert.ToDouble(drTotal["GCEceTotal"]);
                    ResdGCTotal = Convert.ToDouble(drTotal["GCRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Gift Card Services";
                lnkExport.NavigateUrl = "GiftCardServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEGCTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdGCTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }//end if count not 0

            #endregion

            #region CTCart
            //******************************CTCart******************************

            //Check if CTCart exists      
            bool CTCartExists = Residuals.CheckVendorExistsForRep("ctcart");
            //If Rep has at least ONE CTCart Account
            if (CTCartExists)
            {
                //Rep has a CTCart ECE Total (and a CTCart Number)
                DataSet dsTotals = Residuals.GetCTCartTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECECTCartTotal = Convert.ToDouble(drTotal["CTCartEceTotal"]);
                    ResdCTCartTotal = Convert.ToDouble(drTotal["CTCartRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "CT Cart";
                lnkExport.NavigateUrl = "CTCart.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECECTCartTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdCTCartTotal.ToString().Trim();
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

            //Check if MCA exists            
            bool MCAExists = Residuals.CheckVendorExistsForRep("merchantcashadvance");
            //If Rep has at least ONE MCA Account
            if (MCAExists)
            {
                //Rep has a MCA ECE Total
                DataSet dsTotals = Residuals.GetMCATotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEMCATotal = Convert.ToDouble(drTotal["MCAReportEceTotal"]);
                    ResdMCATotal = Convert.ToDouble(drTotal["MCAReportRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Merchant Cash Advance";
                lnkExport.NavigateUrl = "MerchantCashAdvance.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEMCATotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdMCATotal.ToString().Trim();
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

            //Check if Payroll exists            
            bool PayrollExists = Residuals.CheckVendorExistsForRep("payroll");
            //If Rep has at least ONE Misc Account
            if (PayrollExists)
            {
                //Rep has a Misc ECE Total
                DataSet dsTotals = Residuals.GetPayrollTotals();
                if (dsTotals.Tables[0].Rows.Count > 0)
                {
                    DataRow drTotal = dsTotals.Tables[0].Rows[0];
                    ECEPayrollTotal = Convert.ToDouble(drTotal["PayrollEceTotal"]);
                    ResdPayrollTotal = Convert.ToDouble(drTotal["PayrollRepTotal"]);
                }//end if count not 0

                tr = new TableRow();
                lnkExport = new HyperLink();
                lnkExport.Text = "Payroll";
                lnkExport.NavigateUrl = "PayrollServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ECEPayrollTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = ResdPayrollTotal.ToString().Trim();
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
        double ResdMiscTotal = 0;

        //Check if Misc exists            
        bool MiscExists = Residuals.CheckVendorExistsForRep("misc");
        //If Rep has at least ONE Misc Account
        if (MiscExists)
        {
            //Rep has a Misc ECE Total
            DataSet dsTotals = Residuals.GetMiscTotals();
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow drTotal = dsTotals.Tables[0].Rows[0];
                ECEMiscTotal = Convert.ToDouble(drTotal["MiscReportEceTotal"]);
                ResdMiscTotal = Convert.ToDouble(drTotal["MiscReportRepTotal"]);
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
            lblValue.Text = ResdMiscTotal.ToString().Trim();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tblResiduals.Rows.Add(tr);
        }//end if count not 0
        */
            #endregion
            
        }

        #region T1Totals
        double T1ECETotal = 0;
        double T1ResdTotal = 0;

        if (User.IsInRole("T1Agent")) 
        {
            //Rep has a IPay ECE Total (and a IPay Number)
            DataSet dsTotals = Residuals.GetT1Residuals();
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow drTotal = dsTotals.Tables[0].Rows[0];
                T1ECETotal = Convert.ToDouble(drTotal["TEceTotal"]);
                T1ResdTotal = Convert.ToDouble(drTotal["TResidualTotal"]);
            }//end if count not 0

            tr = new TableRow();
            //Add row for Residual Total and Ece Total
            lnkExport = new HyperLink();
            lnkExport.Text = "T1 Residuals";
            lnkExport.NavigateUrl = "TierResiduals.aspx";
            td = new TableCell();
            lnkExport.ApplyStyle(sHyperLink);
            lnkExport.Font.Bold = true;
            td.Controls.Add(lnkExport);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + T1ECETotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + T1ResdTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
            tr.BackColor = System.Drawing.Color.WhiteSmoke;
            tblResiduals.Rows.Add(tr);
        }
        else if (User.IsInRole("Office"))
        {
            //Rep has a IPay ECE Total (and a IPay Number)
            DataSet dsTotals = Residuals.GetOfficeResiduals();
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow drTotal = dsTotals.Tables[0].Rows[0];
                T1ECETotal = Convert.ToDouble(drTotal["TEceTotal"]);
                T1ResdTotal = Convert.ToDouble(drTotal["TResidualTotal"]);
            }//end if count not 0

            tr = new TableRow();
            //Add row for Residual Total and Ece Total
            lnkExport = new HyperLink();
            lnkExport.Text = "Office Residuals";
            //lnkExport.NavigateUrl = "OfficeResiduals.aspx";
            td = new TableCell();
            lnkExport.ApplyStyle(sHyperLink);
            lnkExport.Font.Bold = true;
            td.Controls.Add(lnkExport);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + T1ECETotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + T1ResdTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
            tr.BackColor = System.Drawing.Color.WhiteSmoke;
            tblResiduals.Rows.Add(tr);
        }

        #endregion

        #region Totals
        //Calculate ECE Total for Rep
        if (!(User.IsInRole("Office")))
        {

            double RepECETotal = ECEIPayGateTotal + ECEIPayTotal + ECEIMSTotal + ECEIMS2Total + ECEIPSTotal 
                + ECESageTotal 
                + ECEWPayTotal + ECEIPayFBBHTotal + ECECPSTotal + ECEInnovativeTotal + ECEECXLegacyTotal + ECEInnGateTotal + ECECTCartTotal + ECEAuthnetTotal + ECEDiscoverTotal + ECEPlugNPayTotal + ECEChaseTotal + ECEOptimalCATotal + ECEMerrickTotal + ECEiPay2Total + ECEiPay3Total + ECECSTotal + ECEGCTotal + ECEMCATotal + ECEPayrollTotal + T1ECETotal; //+ ECEMiscTotal;

            //Calculate Residual Total for Rep
            double RepResidualTotal = ResdIPayGateTotal + ResdIPayTotal + ResdIMSTotal + ResdIMS2Total + ResdIPSTotal + ResdSageTotal + ResdWPayTotal + ResdIPayFBBHTotal + ResdCPSTotal + ResdInnovativeTotal + ResdECXLegacyTotal + ResdInnGateTotal + ResdCTCartTotal + ResdAuthnetTotal + ResdDiscoverTotal + ResdPlugNPayTotal + ResdChaseTotal + ResdOptimalCATotal + ResdMerrickTotal + ResdiPay2Total + ResdiPay3Total + ResdCSTotal + ResdGCTotal + ResdMCATotal + ResdPayrollTotal + T1ResdTotal;//+ ResdMiscTotal;

            tr = new TableRow();

            //Add row for Residual Total and Ece Total
            lblValue = new Label();
            lblValue.Text = "Residual Total";
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

        //if Carryover exists, add two more rows
            if (CarryOver != 0)
            {
                //Row for displaying the Carryover
                tr = new TableRow();

                lblValue = new Label();
                lblValue.Text = "Carryover";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                td = new TableCell();
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + CarryOver.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);

                //Row for the Payment Total
                tr = new TableRow();

                lblValue = new Label();
                lblValue.Text = "Total";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                td = new TableCell();
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + Convert.ToString(RepResidualTotal + CarryOver);
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tblResiduals.Rows.Add(tr);
            }
        }

        #endregion

        #region Counts

        double MerchantFundedCount = Residuals.ReturnMerchFundedCount();

        double ReferralCount = Residuals.ReturnReferralCount();

        tr = new TableRow();
        lblValue = new Label();
        lblValue.Text = "Number of Funded Merchant Accounts in " + Month + ": " + MerchantFundedCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.ForeColor = System.Drawing.Color.Green;
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("colspan", "3");
        tr.Cells.Add(td);
        tblResiduals.Rows.Add(tr);

        tr = new TableRow();
        lblValue = new Label();
        lblValue.Text = "Number of Funded Referral Accounts in " + Month + ": " + ReferralCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.ForeColor = System.Drawing.Color.Green;
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("colspan", "3");
        tr.Cells.Add(td);
        tblResiduals.Rows.Add(tr);

        DataSet dsPayTotals = Residuals.GetResidualPayByRepMon();
        if (dsPayTotals.Tables[0].Rows.Count > 0)
        {
            DataRow drResd = dsPayTotals.Tables[0].Rows[0];

            if (Convert.ToDouble(drResd["ResidualPayTotal"]) != Convert.ToDouble(drResd["ResidualTotal"]))
            {
                tr = new TableRow();
                lblValue = new Label();
                lblValue.Text = "You did not receive payment for your residuals this month due to not meeting the minimum terms of your agreement which are: <br/> Having at least " + drResd["FundingMin"].ToString().Trim() + " fundings or " + drResd["RefMin"].ToString().Trim() + " referrals or generating at least " + drResd["ResidualMin"].ToString().Trim() + " in residual payments.(Merchant account count as 1 funding and any Additional Services count as half funding)";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.ForeColor = System.Drawing.Color.Red;
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                td.Attributes.Add("colspan", "3");
                td.Attributes.Add("align", "left");
                td.CssClass = "DivNewsHighlighted";
                tr.Cells.Add(td);
                tblResiduals.Rows.Add(tr);
            }
        }//end if count not 0

        #endregion

        
    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstRepList.Visible)
                Populate(lstRepList.SelectedValue, lstMonth.SelectedItem.Value);
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
