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

public partial class Reports_Residuals_GC : System.Web.UI.Page
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
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                MonthBL mon = new MonthBL();
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {

                    DataSet dsMon = mon.GetMonthListForReports(1, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    ListBL RepList = new ListBL();
                    DataSet dsRep = RepList.GetRepListForVendor("giftcardservices");
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

                    if ((Month != "") && (MasterNum != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                    {
                        try
                        {
                            if (lstRepList.Items.FindByValue(MasterNum) != null)
                                lstRepList.SelectedValue = MasterNum;
                            lstMonth.SelectedValue = lstMonth.Items.FindByText(Month).Value;
                            lblError.Visible = false;
                            lblMonth.Text = "Gift Card Services Residuals for the month of: " + lstMonth.SelectedItem.Text;
                            Populate(MasterNum, Month, false, "");
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
                    Tabs.Tabs.Remove(TabPanelLookup);
                    DataSet dsMon = mon.GetMonthListForReports(2, "residuals");
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

        else
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Tabs.Tabs.Remove(TabPanelLookup);
        }
    }//end page load

    //This function populates ipayment residuals
    public void Populate(string MasterNum, string Month, bool bDBA, string DBA)
    {
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        double ECETotal = 0;
        double RepTotal = 0;
        PartnerDS.GiftCardServicesDataTable dt = null;

        if (!bDBA)
        {
            ResidualsBL GCResd = new ResidualsBL(Month, MasterNum);
            DataSet dsTotals = GCResd.GetGCTotals();

            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTotals.Tables[0].Rows[0];
                ECETotal = Convert.ToDouble(dr["gcecetotal"]);
                RepTotal = Convert.ToDouble(dr["gcreptotal"]);
            }//end if count not 0

            dt = GCResd.GetGCResiduals();
        }
        else
        {
            ResidualsAdminBL GC = new ResidualsAdminBL(Month);
            dt = GC.GetGCResidualsByDBA(DBA);
        }
        //Do not display the last six columns
        int numColsDisplay = dt.Columns.Count - 7;
        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            Label lblValue;

            #region Header Row

            tr = new TableRow();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td = new TableCell();
            td.Text = "";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "9px";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);

            for (int i = 0; i < numColsDisplay; i++)
            {
                td = new TableCell();
                td.Text = dt[0].Table.Columns[i].ColumnName;
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "9px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }

            tblResiduals.Rows.Add(tr);

            #endregion

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();

                //RepName
                lblValue = new Label();
                if (MasterNum == "ALL")
                    lblValue.Text = dt[i].RepName.ToString().Trim();
                else
                    lblValue.Text = "";
                if (bDBA)
                    lblValue.Text = dt[i].RepName.ToString().Trim() + " (" + dt[i].Mon.ToString().Trim() + ")";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                for (int j = 0; j < numColsDisplay; j++)
                {
                    lblValue = new Label();
                    lblValue.Text = dt[i][j].ToString().Trim();

                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }


                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }//end for

            if (!bDBA)
            {
                tr = new TableRow();

                lblValue = new Label();
                lblValue.Text = "Totals";
                td = new TableCell();
                td.Attributes.Add("colspan", "4");
                td.Attributes.Add("align", "left");
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
            }
        }//end if count not 0
        else
            DisplayMessage("No Records found.");
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
            lblMonth.Visible = true;
            lblMonth.Text = "Gift Card Services Residuals for the month of: " + lstMonth.SelectedItem.Text;

            Populate(Rep, lstMonth.SelectedItem.Value.ToString().Trim(), false, "");

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

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            lblMonth.Visible = false;
            Populate("", "", true, txtLookup.Text.Trim());
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
}
