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

public partial class Residuals_iPayFBBH : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
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
            MonthBL Mon = new MonthBL();

            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
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
                    ListBL RepList = new ListBL();
                    DataSet dsRep = RepList.GetRepListForVendor("ipayfbbh");
                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = dsRep;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "MasterNum";
                        lstRepList.DataBind();
                    }
                    ListItem item = new ListItem();
                    item.Text = "ALL";
                    item.Value = "ALL";
                    lstRepList.Items.Add(item);
                    lstRepList.SelectedIndex = lstRepList.Items.IndexOf(item);

                    string MasterNum = "";
                    string Month = "";
                    if (Request.Params.Get("Month") != null)
                        Month = Request.Params.Get("Month");

                    if (Request.Params.Get("MasterNum") != null)
                        MasterNum = Request.Params.Get("MasterNum");

                    if (User.IsInRole("Admin") || User.IsInRole("Employee") )
                    {
                        try
                        {
                            lblError.Visible = false;
                            lstMonth.SelectedValue = lstMonth.Items.FindByText(Month).Value;
                            lblError.Visible = false;
                            lblMonth.Text = "IPayment FBBH Residuals for the month of: " + lstMonth.SelectedItem.Text;
                            Populate (MasterNum, Month);
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

                    lstRepList.Visible = false;
                    lblSelectRepName.Visible = false;
                }
            }//end try
            catch (Exception)
            {
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function populates ipayment residuals
    public void Populate(string MasterNum, string Month)
    {
        ResidualsBL Resd = new ResidualsBL(Month, MasterNum);

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

        DataSet dsTotals = Resd.GetIPayFBBHTotals();
        double ECETotal = 0;
        double RepTotal = 0;
        if (dsTotals.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsTotals.Tables[0].Rows[0];
            ECETotal = Convert.ToDouble(dr["IPayFBBHecetotal"]);
            RepTotal = Convert.ToDouble(dr["IPayFBBHreptotal"]);
        }//end if count not 0

        PartnerDS.iPaymentFBBHDataTable dt = Resd.GetIPayFBBHResiduals();
        if (dt.Rows.Count == 0)
            DisplayMessage("No Records found for this month");
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            Label lblValue;

            #region Header Row

            string[] arrColumns = { "", "DBA", "Disc Rate", "Trans Rate", "Volume", "VIA", "MQV", "MQIA", "NQV", 
                "NQIA", "Tran", "TIA", "MonMin Chg", "MonMin Due", "SC", "SIA", "CB", "CBIA", "Rtvl", "RIA", 
                "UCF", "ECE Total", "Agent Split", "Agent Residual", };
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < 24; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i];
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "9px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }

            tblResiduals.Rows.Add(tr);

            #endregion
            string[] arrValues = { "DBA", "MerchDiscRate", "MerchTransFee", "Volume", "VIA", "MQV", "MQIA", 
                "NQV", "NQIA", "Trans", "TIA", "MonthlyMinCharged", "MonthlyMinDueAgent", "SC", "SIA", "CB", 
                "CBIA", "Rtvl", "RIA", "UCF", "ECETotal", "RepSplit", "RepTotal", };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();

                //RepName
                lblValue = new Label();
                if (MasterNum == "ALL")
                    lblValue.Text = dt[i].RepName.ToString().Trim();
                else
                    lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                for (int j = 0; j < arrValues.Length; j++)
                {
                    lblValue = new Label();                    
                
                    lblValue.Text = dt[i][j].ToString();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }


                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }//end for

            tr = new TableRow();
            td = new TableCell();
            td.Attributes.Add("colspan", "20");
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "Total";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + ECETotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            td = new TableCell();
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + RepTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.WhiteSmoke;
            tblResiduals.Rows.Add(tr);
        }//end if count not 0

    }//end function Populate



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string Rep = Session["MasterNum"].ToString();
            //if the Rep List is visible, set the Rep to be searched
            if (lstRepList.Visible == true)
                Rep = lstRepList.SelectedValue;
                   
            lblError.Visible = false;
            lblMonth.Text = "iPayment FBBH Residuals for the month of: " + lstMonth.SelectedItem.Text;
            Populate(Rep, lstMonth.SelectedItem.Value.ToString().Trim() );
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

}
