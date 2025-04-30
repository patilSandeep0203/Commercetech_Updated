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
using System.Data.SqlClient;
using OnlineAppClassLibrary;
using BusinessLayer;
using DLPartner;
using AjaxControlToolkit;

public partial class OnlineAppAdmin : System.Web.UI.Page
{
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

            if (User.IsInRole("Agent"))
                Response.Redirect("onlineappagent.aspx");

            if (User.IsInRole("Affiliate") || User.IsInRole("Reseller"))
                Response.Redirect("onlineappreferrals.aspx");

            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Response.Redirect("~/logout.aspx?Authentication=False");

            try
            {
                DataSet dsRep = (DataSet)Cache["OARepList"];
                if (dsRep == null)
                {
                    //Get rep list
                    ListBL List = new ListBL();
                    dsRep = List.GetSalesRepList();
                    Cache.Insert("OARepList", dsRep, null);
                }

                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    lstRepName.DataSource = dsRep;
                    lstRepName.DataTextField = "RepName";
                    lstRepName.DataValueField = "MasterNum";
                    lstRepName.DataBind();
                }
                ListItem item = new ListItem();
                item.Text = "ALL";
                item.Value = "ALL";
                lstRepName.Items.Add(item);                

                if (Session["MasterNum"].ToString() != "" && User.IsInRole("Agent"))//Set selected index for rep list based on masternum
                    lstRepName.SelectedValue = lstRepName.Items.FindByValue(Session["MasterNum"].ToString()).Value;
                else
                    lstRepName.SelectedIndex = lstRepName.Items.IndexOf(item);


                DataSet dsStatus = (DataSet)Cache["OAStatusList"];
                if (dsStatus == null)
                {
                    //Get rep list
                    OnlineAppStatusBL Status = new OnlineAppStatusBL();
                    dsStatus = Status.GetStatusSearchList();
                    Cache.Insert("OAStatusList", dsStatus, null);
                }
                //Get Status list
                
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
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support.");
            }
        }//end if not postback
    }

    //This function displays the online app summary
    public void PopulateSummary(bool bUpdateStatus, bool bLookup)
    {
        try
        {
            #region Set Styles
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

            Style eHyperLink = new Style();
            eHyperLink.Font.Bold = true;
            eHyperLink.Font.Size = FontUnit.Point(8);
            eHyperLink.Font.Name = "Arial";
            eHyperLink.CssClass = "One";
            #endregion

            

            //Panel pnlEditApp; //This is the Yellow panel with menu options to Add to ACT, Update, History etc.
            HyperLink lnkEdit;//Hyperlinks in the menu panel
            /*Table tblPanel;//Table in the menu panel
            TableRow trPanel;//Table Rows
            TableCell tdPanel;//Table Cells
            HoverMenuExtender mnuExt;//This is the AJAX hover menu which basically displays the menu panel when the 
            //mouse is moved over the Edit link
            */
            //Set the default from and to dates to populate the online app list
            DateTime UpdateStatusDate = DateTime.Now;
            string ACTEditDate = string.Empty;
            string strFromDate = txtFromDate.Text.Trim();
            string strToDate = txtToDate.Text.Trim();
            string onlineAppEditDate = string.Empty;
            //Set From and To dates
            if (strFromDate == "")
                strFromDate = DateTime.Today.Date.Month + "/1/" + (DateTime.Today.Date.Year - 1);

            if (lstRepName.SelectedItem.Value.ToString().Trim().ToLower() != "all")
            {
                strFromDate = "1/1/1999";
            }
            if (bUpdateStatus)
            {
                strFromDate = "1/1/1999";
            }

            if (strToDate == "")
                strToDate = DateTime.Today.Date.Month + "/" + DateTime.Today.Date.Day + "/" + DateTime.Today.Date.Year;

            OnlineAppSummaryBL Summary = new OnlineAppSummaryBL();
            PartnerDS.OnlineAppSummaryDataTable dt;
            AdministrativeBL Admin = new AdministrativeBL();
            //Display last status update date
            PartnerDS.AdministrativeInfoDataTable dtAdminInfo = Admin.GetAdministrativeInfo();



            if (dtAdminInfo[0].Table.Rows.Count > 0)
            {
                lblUpdateStatusDate.Text = "Update Status last run on " + dtAdminInfo[0].LastUpdateStatus.ToString() + " by " + dtAdminInfo[0].LastUpdatedBy.ToString();
                UpdateStatusDate = Convert.ToDateTime(dtAdminInfo[0].LastUpdateStatus);
            }//end if count not 0 for Admin Info

            bool bDisplaySynched = false;
            if (chkDisplayUnsynched.Checked)
                bDisplaySynched = true;
            //If submit button is clicked
            if (bLookup == false)
                dt = Summary.GetSummary(lstRepName.SelectedItem.Value.ToString().Trim(), lstStatus.SelectedItem.Value.ToString().Trim(), strFromDate, strToDate, lstSortBy.SelectedItem.Value, bDisplaySynched);
            else //lookup button is clicked
                dt = Summary.GetSummaryLookup(lstLookup.SelectedItem.Value.ToString().Trim(), txtLookup.Text.Trim());

            lblNumberOfRecords.Text = "Number of records found: " + dt.Rows.Count;

            if (dt.Rows.Count > 0)
            {
                Label lblValue;
                Label lblAsterisk;
                Label lblStatusSTR;
                //Create Header Row
                string[] arrColumns = { "AppId", "Edit", "Contact (Click to send reminder to customer)", "DBA", "Sales Rep", "Start Date", "Last Modified", "Last Sync Date", "Referral Source (DBA)", "Merchant Status", "Gateway Status", "Addl Services", "AppId" };

                #region Header Row
                //*********************CREATE HEADER ROW******************
                TableRow tr = new TableRow();//Table Row declation
                TableCell td = new TableCell();//Table Cell declaration
                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();//Create a new cell for each column
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);//Add the cell created to the row
                }
                tblSummary.Rows.Add(tr);//Add the Header row to the table
                //*********************END HEADER ROW**********************
                #endregion

                //**********************GENERATE ROWS****************************
                for (int i = 0; i < dt.Rows.Count; i++)
                //for (int i = 0; i < 1; i++)
                {
                    //if (dt[i].RepName.ToString().Trim() = 

                    HyperLink lnkExport;
                    tr = new TableRow();
                    string AppID = "";
                    string strretVal = "";
                    string strAcctType = dt[0].AcctTypeDesc.ToString().Trim();
                    lblStatusSTR = new Label();

                    #region Update Status
                    //This is called only when the Update Status button is clicked
                    if (bUpdateStatus)
                    {
                        strFromDate = "1/1/2005";
                        ACTEditDate = "";
                        //Get ACT Edit Date
                        ACTDataBL ACT = new ACTDataBL();
                        ACTEditDate = ACT.ReturnActEditDate(Convert.ToInt32(dt[i].AppId));

                        OnlineAppDL onlineAppEdit = new OnlineAppDL();
                        //onlineAppEditDate = onlineAppEdit.GetOnlineAppEditDate(Convert.ToInt32(dt[i].AppId));
                        onlineAppEditDate = Convert.ToString(dt[i].LastModifiedDate);
                        //If App ID not found in ACT
                        if (ACTEditDate == "")
                        {
                            Style UpdateStatusLabel = new Style();
                            UpdateStatusLabel.ForeColor = System.Drawing.Color.Red;
                            UpdateStatusLabel.Font.Size = FontUnit.Small;
                            UpdateStatusLabel.Font.Bold = true;
                            UpdateStatusLabel.Font.Name = "Arial";
                            lblStatusSTR.Text = "?";
                            lblStatusSTR.ApplyStyle(UpdateStatusLabel);
                        }
                        //if Multiple App IDs found in ACT                  
                        else if (ACTEditDate == "Multiple")
                        {
                            DisplayMessage("More than one ACT record found for App ID " + dt[i].AppId);
                            lblStatusSTR.Text = "";
                        }
                        //If Act Record has an Edit Date (implying that it exists in ACT)
                        else
                        {
                            if (Convert.ToDateTime(ACTEditDate) > UpdateStatusDate)
                            //if (Convert.ToDateTime(ACTEditDate) > Convert.ToDateTime(onlineAppEditDate))
                            {
                                //Update status one App at a time within For Loop
                                ExportActBL ExportActStatus = new ExportActBL();
                                strretVal = ExportActStatus.ExportACTStatus(Convert.ToInt32(dt[i].AppId), Convert.ToInt32(dt[i].AcctType));
                                //strretVal = ExportActStatus.ExportACTStatus(4913, Convert.ToInt32(dt[i].AcctType));
                                if (strretVal.Contains("+"))
                                {
                                    //denotes Merchant Status or Gateway Status in Act is not
                                    //beyond COMPLETED. Record will NOT be updated. 
                                    Style UpdateStatusLabel = new Style();
                                    UpdateStatusLabel.ForeColor = System.Drawing.Color.DarkGreen;
                                    UpdateStatusLabel.Font.Size = FontUnit.Small;
                                    UpdateStatusLabel.Font.Bold = true;
                                    UpdateStatusLabel.Font.Name = "Arial";
                                    lblStatusSTR.Text = "+";
                                    lblStatusSTR.ApplyStyle(UpdateStatusLabel);
                                }
                                /* Not sure why this was added, so removing the code.
                                 * else if (strretVal.Contains("#"))
                                {
                                    //denotes Online App Status is locked. Updating to 
                                    //DECLINED status will unlock the application
                                    Style UpdateStatusLabel = new Style();
                                    UpdateStatusLabel.ForeColor = System.Drawing.Color.DarkBlue;
                                    UpdateStatusLabel.Font.Size = FontUnit.Small;
                                    UpdateStatusLabel.Font.Bold = true;
                                    UpdateStatusLabel.Font.Name = "Arial";
                                    lblStatusSTR.Text = "#";
                                    lblStatusSTR.ApplyStyle(UpdateStatusLabel);
                                }*/
                            }//end if edit date
                            //Record was Updated
                            else
                            {
                                Style UpdateStatusLabel = new Style();
                                UpdateStatusLabel.ForeColor = System.Drawing.Color.Orange;
                                UpdateStatusLabel.Font.Size = FontUnit.Small;
                                UpdateStatusLabel.Font.Bold = true;
                                UpdateStatusLabel.Font.Name = "Arial";
                                lblStatusSTR.Text = "$";
                                lblStatusSTR.ApplyStyle(UpdateStatusLabel);
                            }
                        }//End if Act Record found for App ID

                    }//end if UpdateStatus
                    else
                        lblStatusSTR.Text = "";
                    #endregion

                    #region Redirect
                    lblAsterisk = new Label();
                    lblAsterisk.Text = "";
                    OnlineAppBL App = new OnlineAppBL(Convert.ToInt32(dt[i].AppId));
                    DataSet ds1 = App.GetEditInfo();
                    string pid="";
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow drNewApp = ds1.Tables[0].Rows[0];
                        pid = Server.HtmlEncode(drNewApp["PID"].ToString());

                        if(pid=="255" || pid=="253" || pid=="254" || pid=="252")
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
                   
                    //Check if quickbooks is true
                    if (dt[i].Redirect.ToString().Trim() == "QuickBooks")
                    {
                        Style AsteriskLabel = new Style();
                        AsteriskLabel.ForeColor = System.Drawing.Color.Red;
                        AsteriskLabel.Font.Size = FontUnit.Medium;
                        AsteriskLabel.Font.Bold = true;
                        AsteriskLabel.Font.Name = "Arial";
                        lblAsterisk.Text = "*";
                        lblAsterisk.ApplyStyle(AsteriskLabel);
                    }//end if quickbooks
                    else if (dt[i].Redirect.ToString().Trim() == "WorldPay")
                    {
                        Style AsteriskLabel = new Style();
                        AsteriskLabel.ForeColor = System.Drawing.Color.Red;
                        AsteriskLabel.Font.Size = FontUnit.Medium;
                        AsteriskLabel.Font.Bold = true;
                        AsteriskLabel.Font.Name = "Arial";
                        lblAsterisk.Text = "**";
                        lblAsterisk.ApplyStyle(AsteriskLabel);
                    }//end if WorldPay
                    
                    #endregion

                    #region AppId
                    //AppId
                    AppID = dt[i].AppId.ToString().Trim();//Set the appid variable with the value from the datarow
                    lnkExport = new HyperLink();
                    lnkExport.Text = AppID;
                    //int OnlineAppID = Convert.ToInt32(AppID);
                    //lnkExport.NavigateUrl = "http://www.firstaffiliates.com/OnlineApplication/login.aspx?AppId=" + AppID;
                    //lnkExport.NavigateUrl = "VerifyAppLogin.aspx?AppID=" + AppID;



                    lnkExport.Attributes.Add("onClick", "window.open('VerifyAppLogin.aspx?AppID=" + AppID + "', 'OnlineApp');");
                    lnkExport.CssClass = "One";
                    lnkExport.ForeColor = System.Drawing.Color.Blue;
                    lnkExport.Attributes.Add("style", "cursor:pointer");

                    lnkExport.Target = "_Blank";
                    lnkExport.ApplyStyle(sHyperLink);
                    lnkExport.ToolTip = "Click to log in to the application";

                    td = new TableCell();
                    if (lblAsterisk.Text == "*")
                    td.ToolTip = "QuickBooks Signup";
                    td.Controls.Add(lblStatusSTR);//This is the label for update status
                    td.Controls.Add(lblAsterisk);//This is the label for Quickbooks signup
                    td.Controls.Add(lnkExport);//This is the actual hyper link with appid as the text
                    tr.Cells.Add(td);//Add this cell to the table row
                    #endregion
                    
                    #region Edit Menu

                    //Edit
                    /*pnlEditApp = new Panel();
                    pnlEditApp.ID = "pnlEditApp" + AppID;
                    pnlEditApp.Width = Unit.Pixel(100);
                    pnlEditApp.Attributes.Add("style", "border-right: silver 1px solid; border-top: silver 1px solid; display: none; z-index: 1; border-left: silver 1px solid; border-bottom: silver 1px solid; background-color: #fbfbfb;");
                    tblPanel = new Table();
                
                    //Add to ACT link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    HyperLink lnkACT = new HyperLink();
                    lnkACT.Text = "Add To ACT!";
                    lnkACT.NavigateUrl = "edit.aspx?AppId=" + AppID + "&Task=AddToACT";
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT); // ADD Link to CELL
                    trPanel.Cells.Add(tdPanel); // Add Cell to ROW
                    tblPanel.Rows.Add(trPanel); // Add Row to TABLE
                 
                
                    //Update in ACT link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "Update In ACT!";
                    lnkACT.NavigateUrl = "edit.aspx?AppId=" + AppID + "&Task=UpdateInACT";
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);
                

                    //Create PDF Link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "Create PDF";
                    lnkACT.NavigateUrl = "edit.aspx?AppId=" + AppID + "&Task=CreatePDF";
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);

                    //Modify Link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "Modify";
                    lnkACT.NavigateUrl = "Modify.aspx?AppId=" + AppID;
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);

                    //Set Rates Link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "Set Rates";
                    lnkACT.NavigateUrl = "SetRates.aspx?AppId=" + AppID;
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);

                    //Sales Opps Link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "Sales Opps";
                    lnkACT.NavigateUrl = "SalesOpps.aspx?AppId=" + AppID;
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);
                
                    //Status Link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "Status";
                    lnkACT.NavigateUrl = "OnlineAppStatus.aspx?AppId=" + AppID;
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);

                    //History link
                    trPanel = new TableRow();
                    tdPanel = new TableCell();
                    lnkACT = new HyperLink();
                    lnkACT.Text = "History";
                    lnkACT.NavigateUrl = "history.aspx?AppId=" + AppID;
                    lnkACT.Target = "_blank";
                    lnkACT.ApplyStyle(eHyperLink);
                    tdPanel.Controls.Add(lnkACT);
                    trPanel.Cells.Add(tdPanel);
                    tblPanel.Rows.Add(trPanel);

                    pnlEditApp.Controls.Add(tblPanel); // Add table to panel
                    */
                    lnkEdit = new HyperLink();
                    lnkEdit.ID = "lnkEdit" + AppID;
                    lnkEdit.Visible = true;
                    lnkEdit.Text = "Edit";
                    //lnkEdit.NavigateUrl = "edit.aspx?AppId=" + AppID;

                    lnkEdit.Attributes.Add("onClick", "window.open('edit.aspx?AppId=" + AppID + "', 'OnlineAppMgmt" + AppID + "');");
                    lnkEdit.CssClass = "One";
                    lnkEdit.ForeColor = System.Drawing.Color.Blue;
                    lnkEdit.Attributes.Add("style", "cursor:pointer");

                    lnkEdit.Target = "_Blank";
                    lnkEdit.ApplyStyle(sHyperLink);                    

                    td = new TableCell();
                    td.Controls.Add(lnkEdit);
                    /*td.Controls.Add(pnlEditApp);
                                
                    mnuExt = new HoverMenuExtender();       
                    mnuExt.PopupControlID = "pnlEditApp" + AppID;
                    mnuExt.TargetControlID = "lnkEdit" + AppID;
                
                    mnuExt.PopupPosition = HoverMenuPopupPosition.Right;
              
                    //mnuExt.Visible = false;
                    //mnuExt.Dispose();
                

                    //******************END MENU*****************
                    //this.Page.Controls.Add(mnuExt);

                    td.Controls.Add(mnuExt);*/

                    tr.Cells.Add(td);
                    #endregion
                    
                    #region Contact/Send Reminder
                    //FirstName
                    lnkExport = new HyperLink();
                    lnkExport.Text = dt[i].FirstName.ToString().Trim() + " " + dt[i].LastName.ToString().Trim();
                    string strURL1 = "SendReminder.aspx?AppId=" + AppID;
                    string strURL2 = "window.open('" + strURL1 + "', 'reminder','width=720,height=500,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes');";
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

                    #region SalesRep
                    //SalesRep
                    lblValue = new Label();
                    lblValue.Text = dt[i].RepName.ToString().Trim();
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

                    #region LastModifiedDate
                    if ((dt[i].LastModifiedDate.ToString() != "") && (dt[i].LastSynchDate.ToString() != ""))
                    {
                        //If Last Modified Date is more recent than Sync Date (on the Merchant App)
                        if (dt[i].Unsynched.ToString() == "Yes")
                        {
                            //Last Modified Date
                            lblValue = new Label();
                            lblValue.Text = dt[i].LastModifiedDate.ToString().Trim();
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            lblValue.ForeColor = System.Drawing.Color.Red;
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);
                        }
                        //If Last Modified Date of the Rates is more recent than the Sync Date 
                        else if (dt[i].UnsynchedRates.ToString() == "Yes")
                        {
                            //Last Modified Date
                            lblValue = new Label();
                            lblValue.Text = dt[i].LastModifiedDate.ToString().Trim() + System.Environment.NewLine;
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            lblValue.ForeColor = System.Drawing.Color.Red;
                            td.Controls.Add(lblValue);
                            Style RatesLabel = new Style();
                            RatesLabel.ForeColor = System.Drawing.Color.Red;
                            RatesLabel.Font.Size = FontUnit.Point(7);
                            RatesLabel.Font.Bold = true;
                            RatesLabel.Font.Name = "Arial";

                            lblValue = new Label();
                            lblValue.Text = "**Rates Changed Only**";
                            lblValue.ApplyStyle(RatesLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);
                        }
                        //If Last Modified Date of the Profile is more recent than the Sync Date 
                        else if (dt[i].UnsynchedProfile.ToString() == "Yes")
                        {
                            //Last Modified Date
                            lblValue = new Label();
                            lblValue.Text = dt[i].LastModifiedDate.ToString().Trim() + System.Environment.NewLine;
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            lblValue.ForeColor = System.Drawing.Color.Red;
                            td.Controls.Add(lblValue);
                            Style RatesLabel = new Style();
                            RatesLabel.ForeColor = System.Drawing.Color.Red;
                            RatesLabel.Font.Size = FontUnit.Point(7);
                            RatesLabel.Font.Bold = true;
                            RatesLabel.Font.Name = "Arial";

                            lblValue = new Label();
                            lblValue.Text = "**Profile Changed Only**";
                            lblValue.ApplyStyle(RatesLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);
                        }
                        //else if (!Convert.ToBoolean (dt[i].IsAddedSalesOpps.ToString()))
                        else if (dt[i].IsAddedSalesOpps == "0")
                        {
                            //This checks if a new sales opp was added in the partner portal.
                            //The IsAddedSalesOpps is set to 1 if any of the sales opps for this appid
                            //has at least one IsAddedACT bit set to 1.
                            lblValue = new Label();
                            lblValue.Text = dt[i].LastModifiedDate.ToString().Trim() + System.Environment.NewLine;
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            lblValue.ForeColor = System.Drawing.Color.Red;
                            td.Controls.Add(lblValue);
                            Style RatesLabel = new Style();
                            RatesLabel.ForeColor = System.Drawing.Color.Red;
                            RatesLabel.Font.Size = FontUnit.Point(7);
                            RatesLabel.Font.Bold = true;
                            RatesLabel.Font.Name = "Arial";
                            lblValue = new Label();
                            lblValue.Text = "**New Sales Opp(s) Added**";
                            lblValue.ApplyStyle(RatesLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);
                        }
                        else if (dt[i].UnlinkedSalesOpps.ToString() == "Yes")
                        {
                            //Check if any of the sync dates is less than the sync dates for the other sales opps and set
                            //UnlinkedSalesOpps to true.
                            //ISSUE: If a sales opp was added in the partner portal and then added to ACT, but deleted
                            //before clicking the update status button, the sync date for this sales opp will be null.
                            //This still must be displayed as an unlinked sales opp.

                            lblValue = new Label();
                            lblValue.Text = dt[i].LastModifiedDate.ToString().Trim() + System.Environment.NewLine;
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            lblValue.ForeColor = System.Drawing.Color.Red;
                            td.Controls.Add(lblValue);
                            Style RatesLabel = new Style();
                            RatesLabel.ForeColor = System.Drawing.Color.Red;
                            RatesLabel.Font.Size = FontUnit.Point(7);
                            RatesLabel.Font.Bold = true;
                            RatesLabel.Font.Name = "Arial";
                            lblValue = new Label();
                            lblValue.Text = "**Sales Opp(s) Unlinked**";
                            lblValue.ApplyStyle(RatesLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);
                        }
                        else
                        {
                            //Last Modified Date
                            lblValue = new Label();
                            lblValue.Text = dt[i].LastModifiedDate.ToString().Trim();
                            td = new TableCell();
                            lblValue.ApplyStyle(ValueLabel);
                            td.Controls.Add(lblValue);
                            tr.Cells.Add(td);
                        }
                    }
                    else if ((dt[i].LastModifiedDate.ToString() != "") && (dt[i].LastSynchDate.ToString() == ""))
                    {
                        //Last Modified Date
                        lblValue = new Label();
                        lblValue.Text = dt[i].LastModifiedDate.ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        lblValue.ForeColor = System.Drawing.Color.Red;
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);
                    }
                    #endregion
                    
                    #region LastSyncDate
                    //Last Sync Date
                    lblValue = new Label();
                    lblValue.Text = dt[i].LastSynchDate.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                    #endregion

                    #region ReferredBy
                    //Referred By
                    lblValue = new Label();
                    lblValue.Text = dt[i].ReferralName.ToString().Trim();
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
                            lblValue.Text = dt[i].Status.ToString().Trim() + " (" + dt[i].PgsComplete.ToString().Trim() + ")";
                            lblValue.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblValue.Text = dt[i].Status.ToString().Trim() + "<br/>";
                            lblValue.ForeColor = System.Drawing.Color.MidnightBlue;
                        }
                    }
                    else
                        lblValue.Text = "";
                    td = new TableCell();
                    //lblValue.ApplyStyle(ValueLabel);
                    if (dt[i].Status.ToString().Trim().ToLower().Contains("active"))
                        lblValue.ForeColor = System.Drawing.Color.DarkGreen;
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                    #endregion

                    #region Gateway Status
                    lblValue = new Label();
                    lblValue.ApplyStyle(ValueLabel);
                    int PackageID = 0;
                    int appid = Convert.ToInt32(AppID);
                    NewAppInfo ReturnApp = new NewAppInfo(appid);
                    PackageID = ReturnApp.ReturnPID();
                    if ((Convert.ToInt32(dt[i].AcctType) == 2) || (Convert.ToInt32(dt[i].AcctType) == 4))
                    {
                        /*if (PackageID == 253 || PackageID == 254)
                        {
                            lblValue.Text = "";
                        }*/
                        if (dt[i].StatusGW.ToString().Trim().Contains("INCOMPLETE") || dt[i].StatusGW.ToString().Trim().Contains("Incomplete") || dt[i].StatusGW.ToString().Trim().Equals(""))
                        {
                            lblValue.Text = dt[i].StatusGW.ToString() + " (" + dt[i].PgsCompleteGW.ToString() + ")";
                            lblValue.ForeColor = System.Drawing.Color.DarkRed;
                        }
                        else
                        {
                            lblValue.Text = dt[i].StatusGW.ToString();
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

                    #region Additional Services
                    lblValue = new Label();
                    string AddlServices = dt[i].AddlServices.ToString().Trim();
                    lblValue.Text = AddlServices;
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                    #endregion

                    /*#region Online Debit
                lblValue = new Label();
                lblValue.Text = dt[i].OnlineDebit.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion

                #region EBT
                lblValue = new Label();
                lblValue.Text = dt[i].EBT.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion

                #region Merchant Funding
                lblValue = new Label();
                lblValue.Text = dt[i].MerchantFunding.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                #endregion*/

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
                    tr.Cells.Add(td);

                    //Check if application locked because of too many login attempts
                    if (Convert.ToInt32(dt[i].LoginAttempts) == 5)
                    {
                        td.BackColor = System.Drawing.Color.Salmon;
                        td.ToolTip = "This application is locked because of too many login attempts. To unlock, click on Edit";
                    }
                    #endregion
                   
                    //if (i % 2 == 0)
                    //    tr.BackColor = System.Drawing.Color.WhiteSmoke;//.FromArgb(230,237,245);

                    //sets the background color based on Rep Category
                    DataSet ds = Summary.GetRepInfoByRepName(dt[i].RepName.ToString().Trim());
                    DataRow drRepCat = ds.Tables[0].Rows[0];
                    string RepCategory = drRepCat["RepCat"].ToString().Trim();

                    if (RepCategory == "A")
                    {
                        tr.BackColor = System.Drawing.Color.PaleTurquoise;
                        tr.ToolTip = "Agent Application";
                    }

                    else if (RepCategory == "E")
                    {
                        tr.BackColor = System.Drawing.Color.LemonChiffon;
                        tr.ToolTip = "Employee Application";
                    }

                    else if (RepCategory == "R")
                    {
                        tr.BackColor = System.Drawing.Color.Pink;
                        tr.ToolTip = "Reseller Application";
                    }
                    else if (RepCategory == "PE")
                    {
                        tr.BackColor = System.Drawing.Color.WhiteSmoke;
                        tr.ToolTip = "Previous Employee Application";
                    }

                    tblSummary.Rows.Add(tr);
                }//end for rows

                //bUpdateStatus = true;

                //if Update Status was called and finished successfully
                if (bUpdateStatus)
                {
                    pnlUpdateStatus.Visible = true;
                    //Store Update Status Date and Name in Administrative Table
                    Admin.UpdateAdministrativeInfo(Session["AffiliateName"].ToString().Trim());
                }
            }//end if count not 0
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }

    }//end function PopulateSummary

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
            lblError.Visible = false;
            pnlUpdateStatus.Visible = false;
            PopulateSummary(false, false);
            pnlUpdateStatusDate.Visible = true;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateSummary(true, false);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support.");
        }
    }//end update status click

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            PopulateSummary(false, true);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void chkDisplayUnsynched_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDisplayUnsynched.Checked)
            {
                lstRepName.Enabled = false;
                lstStatus.Enabled = false;
                TabAdvanced.Enabled = false;
                TabLookup.Enabled = false;
                TabUpdate.Enabled = false;
                PopulateSummary(false, false);
            }
            else
            {
                lstRepName.Enabled = true;
                lstStatus.Enabled = true;
                TabAdvanced.Enabled = true;
                TabLookup.Enabled = true;
                TabUpdate.Enabled = true;
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Apply Interchange Rates - " + err.Message);
            DisplayMessage("Error loading data.");
        }
    }
}
