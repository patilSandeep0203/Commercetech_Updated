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


public partial class OnlineAppReferrals : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Reseller") && !User.IsInRole("Affiliate"))
            Response.Redirect("~/login.aspx?Authentication=False");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                OnlineAppStatusBL Status = new OnlineAppStatusBL();
                DataSet dsStatus = Status.GetStatusSearchList();
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    lstStatus.DataSource = dsStatus;
                    lstStatus.DataTextField = "StatusSearch";
                    lstStatus.DataValueField = "StatusSearch";
                    lstStatus.DataBind();
                }
                ListItem itemAll = new ListItem();
                itemAll.Text = "ALL";
                itemAll.Value = "ALL";
                lstStatus.Items.Add(itemAll);
                lstStatus.SelectedIndex = lstStatus.Items.IndexOf(itemAll);

                PopulateSummary();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }
    }

    //This function displays the online app summary
    public void PopulateSummary()
    {
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.XSmall;
        ValueLabel.Font.Name = "Arial";

        Style sHyperLink = new Style();
        sHyperLink.Font.Bold = true;
        sHyperLink.Font.Size = FontUnit.XSmall;
        sHyperLink.Font.Name = "Arial";
        sHyperLink.CssClass = "One";

        string strFromDate = txtFromDate.Text.Trim();
        string strToDate = txtToDate.Text.Trim();
        if (strFromDate == "")
            strFromDate = "01/01/2005";
        if (strToDate == "")
            strToDate = DateTime.Today.Date.Month + "/" + DateTime.Today.Date.Day + "/" + DateTime.Today.Date.Year;

        lnkCreateOnlineApp.NavigateUrl = "https://www.firstaffiliates.com/onlineapplication/start.aspx?Referral=" + Session["AffiliateID"].ToString();

        OnlineAppSummaryBL Summary = new OnlineAppSummaryBL();
        PartnerDS.OnlineAppSummaryDataTable dt = Summary.GetSummaryAff(Convert.ToString(Session["AffiliateID"]), lstStatus.SelectedItem.Value.ToString().Trim(), strFromDate, strToDate);
        lblNumberOfRecords.Text = "Number of records with status " + lstStatus.SelectedItem.Text + ": " + dt.Rows.Count;
        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            Label lblAsterisk;
            string[] arrColumns = { "AppId", "Contact", "DBA", "Email", "Start Date", "Sales Rep", "Merchant Status/Pgs Complete", "Account Type", "Gateway Status/Pgs Complete", "Additonal Services" };
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < arrColumns.Length; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i].ToString();
                td.Style["font-weight"] = "Bold";
                td.CssClass = "MenuHeader";
                tr.Cells.Add(td);
            }
            tblSummary.Rows.Add(tr);            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();
                string AppID = "";
                string strAcctType = dt[0].AcctTypeDesc.ToString().Trim();

                lblAsterisk = new Label();
                if (dt[0].QuickBooks.ToString().Trim() != "")
                {                    
                    if (Convert.ToBoolean(dt[0].QuickBooks))
                    {
                        Style AsteriskLabel = new Style();
                        AsteriskLabel.ForeColor = System.Drawing.Color.Red;
                        AsteriskLabel.Font.Size = FontUnit.Medium;
                        AsteriskLabel.Font.Bold = true;
                        AsteriskLabel.Font.Name = "Arial";
                        lblAsterisk.Text = "*";
                        lblAsterisk.ApplyStyle(AsteriskLabel);
                    }
                }
                else
                {
                    lblAsterisk.Text = "";
                }

                //AppId
                AppID = dt[i].AppId.ToString().Trim();
                lblValue = new Label();
                lblValue.Text = AppID;
                lblValue.ApplyStyle(ValueLabel);
                td = new TableCell();
                if (lblAsterisk.Text == "*")
                    td.ToolTip = "QuickBooks Signup";
                td.Controls.Add(lblAsterisk);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Contact
                lblValue = new Label();
                lblValue.Text = dt[i].FirstName.ToString().Trim() + " " + dt[i].LastName.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //DBA
                lblValue = new Label();
                lblValue.Text = dt[i].DBA.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Email                
                lblValue = new Label();
                lblValue.Text = dt[i].Email.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //StartDate
                lblValue = new Label();
                lblValue.Text = dt[i].StartDate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Sales Rep
                lblValue = new Label();
                lblValue.Text = dt[i].RepName.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Merchant Status + Pages Complete
                lblValue = new Label();
                if ((Convert.ToInt32(dt[i].AcctType) == 1) || (Convert.ToInt32(dt[i].AcctType) == 4))
                {
                    if (dt[i].Status.ToString().Trim().Contains("INCOMPLETE") || dt[i].Status.ToString().Trim().Contains("Incomplete") || dt[i].Status.ToString().Trim().Equals(""))
                        lblValue.Text = dt[i].Status.ToString().Trim() + "(" + dt[i].PgsComplete.ToString().Trim() + ")";
                    else
                        lblValue.Text = dt[i].Status.ToString().Trim();
                }
                else
                    lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Account Type
                lblValue = new Label();
                lblValue.Text = dt[i].AcctTypeDesc.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Gateway Status + Pages Complete
                lblValue = new Label();
                if ((Convert.ToInt32(dt[i].AcctType) == 2) || (Convert.ToInt32(dt[i].AcctType) == 4))
                {
                    if (dt[i].StatusGW.ToString().Trim().Contains("INCOMPLETE") || dt[i].StatusGW.ToString().Trim().Contains("Incomplete") || dt[i].StatusGW.ToString().Trim().Equals(""))
                        lblValue.Text = dt[i].StatusGW.ToString().Trim() + "(" + dt[i].PgsCompleteGW.ToString().Trim() + ")";
                    else
                        lblValue.Text = dt[i].StatusGW.ToString().Trim();
                }
                else
                    lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                #region Check Services
                lblValue = new Label();
                lblValue.Text = dt[0].AddlServices.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblSummary.Rows.Add(tr);
            }//end for rows
        }//end if count not 0
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateSummary();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

}
