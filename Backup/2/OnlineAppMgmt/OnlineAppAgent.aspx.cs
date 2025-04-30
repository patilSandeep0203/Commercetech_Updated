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

public partial class OnlineAppAgent : System.Web.UI.Page
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

        if (!User.IsInRole("Agent") && !User.IsInRole("T1Agent"))
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

                PopulateSummary(lstSortBy.SelectedItem.Text);
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
    public void PopulateSummary(string SortBy)
    {        
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.XSmall;
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

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

        OnlineAppSummaryBL Summary= new OnlineAppSummaryBL();
        PartnerDS.OnlineAppSummaryDataTable dt = Summary.GetSummary(Convert.ToString(Session["MasterNum"]), lstStatus.SelectedItem.Value.ToString().Trim(), strFromDate, strToDate, SortBy, false);
        lblNumberOfRecords.Text = "Number of records with status " + lstStatus.SelectedItem.Text + ": " + dt.Rows.Count;
        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            Label lblAsterisk;

            #region Header
            string[] arrColumns = { "AppId", "Edit", "Contact (Click to send reminder to customer)", "DBA", "Start Date", "Merchant Status/Pgs Complete", "Gateway Status/Pgs Complete", "Additional Services", "AppId" };
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
            #endregion

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                HyperLink lnkExport;
                tr = new TableRow();
                string AppID = "";
                
                lblAsterisk = new Label();
                if (dt[i].QuickBooks.ToString().Trim() != "")
                {
                    if (Convert.ToBoolean(dt[i].QuickBooks))
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

                #region AppId
                //AppId
                AppID = dt[i].AppId.ToString().Trim();
                lnkExport = new HyperLink();
                lnkExport.Text = AppID;
                lnkExport.NavigateUrl = "VerifyAppLogin.aspx?AppID=" + AppID;
                lnkExport.Target = "_Blank";
                lnkExport.ApplyStyle(sHyperLink);
                lnkExport.ToolTip = "Click to login to the application using your Partner Portal Login name and Password";
                td = new TableCell();
                if (lblAsterisk.Text == "*")
                    td.ToolTip = "QuickBooks Signup";
                td.Controls.Add(lblAsterisk);
                td.Controls.Add(lnkExport);
                tr.Cells.Add(td);
                #endregion

                #region Edit
                //Edit
                lnkExport = new HyperLink();
                lnkExport.Text = "Edit";
                lnkExport.NavigateUrl = "edit.aspx?AppId=" + AppID;
                lnkExport.ApplyStyle(sHyperLink);
                lnkExport.Target = "_Blank";
                lnkExport.ToolTip = "Click to Modify information, Set Rates, Modify Status, ACT! features, Modify Sales Opps or Delete Application";
                td = new TableCell();
                td.Controls.Add(lnkExport);
                tr.Cells.Add(td);
                #endregion

                #region Contact/Send Reminder
                //FirstName
                lnkExport = new HyperLink();
                lnkExport.Text = dt[i].FirstName.ToString().Trim() + " " + dt[i].LastName.ToString().Trim();
                string strURL = "SendReminder.aspx?AppId=" + AppID;
                string strURL2 = "window.open('" + strURL + "', 'reminder','width=800,height=500,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes');";
                lnkExport.Attributes.Add("onClick", strURL2);
                lnkExport.CssClass = "One";
                lnkExport.ForeColor = System.Drawing.Color.Blue;
                lnkExport.Attributes.Add("style", "cursor:pointer");

                //lnkExport.NavigateUrl = "SendReminder.aspx?AppID=" + AppID;
                lnkExport.Target = "_Blank";
                lnkExport.ApplyStyle(sHyperLink);
                lnkExport.ToolTip = dt[i].Email.ToString().Trim();
                td = new TableCell();
                td.Controls.Add(lnkExport);
                tr.Cells.Add(td);
                #endregion

                #region DBA
                //DBA
                lblValue = new Label();
                lblValue.Text = dt[i].DBA.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion

                #region StartDate
                //StartDate
                lblValue = new Label();
                lblValue.Text = dt[i].StartDate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion

                #region Merchant Status
                //Merchant Status + Pages Complete
                lblValue = new Label();
                lblValue.ApplyStyle(ValueLabel);
                if ((Convert.ToInt32(dt[i].AcctType) == 1) || (Convert.ToInt32(dt[i].AcctType) == 4))
                {
                    if (dt[i].Status.ToString().Trim().Contains("INCOMPLETE") || dt[i].Status.ToString().Trim().Contains("Incomplete") || dt[i].Status.ToString().Trim().Equals(""))
                    {
                        lblValue.Text = dt[i].Status.ToString().Trim() + "(" + dt[i].PgsComplete.ToString().Trim() + ")";
                        lblValue.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblValue.Text = dt[i].Status.ToString().Trim();
                        lblValue.ForeColor = System.Drawing.Color.MidnightBlue;
                    }
                }
                else
                    lblValue.Text = "";
                td = new TableCell();
                if (dt[i].Status.ToString().Trim().ToLower().Contains("active"))
                    lblValue.ForeColor = System.Drawing.Color.DarkGreen;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion
                /*
                #region Account Type
                //Account Type
                lblValue = new Label();
                lblValue.Text = dt[i].AcctTypeDesc.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion
                */
                #region Gateway Status
                //Gateway Status + Pages Complete
                lblValue = new Label();
                lblValue.ApplyStyle(ValueLabel);
                if ((Convert.ToInt32(dt[i].AcctType) == 2) || (Convert.ToInt32(dt[i].AcctType) == 4))
                {
                    if (dt[i].StatusGW.ToString().Trim().Contains("INCOMPLETE") || dt[i].StatusGW.ToString().Trim().Contains("Incomplete") || dt[i].StatusGW.ToString().Trim().Equals(""))
                    {
                        lblValue.Text = dt[i].StatusGW.ToString().Trim() + "(" + dt[i].PgsCompleteGW.ToString().Trim() + ")";
                        lblValue.ForeColor = System.Drawing.Color.DarkRed;
                    }
                    else
                    {
                        lblValue.Text = dt[i].StatusGW.ToString().Trim();
                        lblValue.ForeColor = System.Drawing.Color.MidnightBlue;
                    }
                }
                else
                    lblValue.Text = "";
                if (dt[i].StatusGW.ToString().Trim().ToLower().Contains("active"))
                    lblValue.ForeColor = System.Drawing.Color.DarkGreen;
                td = new TableCell();                
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion

                #region Check Services
                lblValue = new Label();
                lblValue.Text = dt[i].AddlServices.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion
                
                #region AppId
                //AppId
                lnkExport = new HyperLink();
                lnkExport.Text = AppID;
                lnkExport.NavigateUrl = "https://www.firstaffiliates.com/OnlineApplication/default.aspx?AppID=" + AppID;
                lnkExport.ApplyStyle(sHyperLink);
                lnkExport.ToolTip = "Send this link to the customer to login using the customer's Email and Password";
                lnkExport.Target = "_Blank";
                td = new TableCell();
                td.Controls.Add(lnkExport);
                #endregion

                tr.Cells.Add(td);

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
            PopulateSummary(lstSortBy.SelectedItem.Text);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

}
