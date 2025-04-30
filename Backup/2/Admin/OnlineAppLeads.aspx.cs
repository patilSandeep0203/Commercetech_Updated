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
using BusinessLayer;
using AjaxControlToolkit;

public partial class OnlineAppLeads : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Employee"))
                Page.MasterPageFile = "Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "AdminMaster.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    private static int iLeadID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!Session.IsNewSession)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Response.Redirect("~/login.aspx");
        }

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
        }//end if not post back
    }//end page load

    #region POPULATE REPORTS

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            string strLeadType = lstLeadReport.SelectedValue;
            switch (strLeadType)
            {
                case "Free Report":
                    grdAffiliateSignups.Visible = false;
                    grdFreeApply.Visible = false;
                    grdFreeConsult.Visible = false;
                    grdFreeReport.Visible = true;
                    PopulateFreeReport();
                    break;
                case "Free Consult":
                    grdAffiliateSignups.Visible = false;
                    grdFreeApply.Visible = false;
                    grdFreeConsult.Visible = true;
                    grdFreeReport.Visible = false;
                    PopulateFreeConsult();
                    break;
                case "Free Apply":
                    grdAffiliateSignups.Visible = false;
                    grdFreeApply.Visible = true;
                    grdFreeConsult.Visible = false;
                    grdFreeReport.Visible = false;
                    PopulateFreeApply();
                    break;
                case "Partner Signups":
                    grdAffiliateSignups.Visible = true;
                    grdFreeApply.Visible = false;
                    grdFreeConsult.Visible = false;
                    grdFreeReport.Visible = false;
                    PopulateAffiliateSignups("AffiliateID");
                    break;
            }//end switch
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Data");
        }
    }//end button submit click

    //This function populates table with Free report signup information
    public void PopulateFreeReport()
    {
        lnkbtnLookup.Visible = false;
        pnlSortBy.Visible = false;
        FirstAffiliatesLeadsBL FreeReport = new FirstAffiliatesLeadsBL();
        DataSet ds = FreeReport.GetReports("Free Report", "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdFreeReport.DataSource = ds;
            grdFreeReport.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }//end function PopulateFreeReport

    //This function populates table with Free consult signup information
    public void PopulateFreeConsult()
    {
        lnkbtnLookup.Visible = false;
        pnlSortBy.Visible = false;
        FirstAffiliatesLeadsBL FreeConsult = new FirstAffiliatesLeadsBL();
        DataSet ds = FreeConsult.GetReports("Free Consult", "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdFreeConsult.DataSource = ds;
            grdFreeConsult.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }//end function PopulateFreeConsult

    //This function populates table with Free Apply signup information
    public void PopulateFreeApply()
    {
        lnkbtnLookup.Visible = false;
        pnlSortBy.Visible = false;
        FirstAffiliatesLeadsBL FreeApply = new FirstAffiliatesLeadsBL();
        DataSet ds = FreeApply.GetReports("Free Apply", "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdFreeApply.DataSource = ds;
            grdFreeApply.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }//end function PopulateFreeApply

    //This function populates table with Affiliate Signup information
    public void PopulateAffiliateSignups(string SortBy)
    {
        try
        {
            lnkbtnLookup.Visible = true;
            pnlSortBy.Visible = true;
            FirstAffiliatesLeadsBL AffiliateSignups = new FirstAffiliatesLeadsBL();
            DataSet ds = AffiliateSignups.GetReports("Affiliate Signups", SortBy);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grdAffiliateSignups.DataSource = ds;
                grdAffiliateSignups.DataBind();
            }//end if count not 0
            else
                DisplayMessage("No Records Found");
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }

    }//end function PopulateAffiliateSignups
    #endregion

    #region FREE REPORT FUNCTIONS
    protected void grdFreeReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CreateApp")
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdFreeReport.Rows[index];
                string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
                if (strLeadID != "")
                {
                    FirstAffiliatesLeadsBL CreateOnlineApp = new FirstAffiliatesLeadsBL();
                    bool retVal = CreateOnlineApp.CreateApp(Convert.ToInt16(strLeadID), "Free Report");
                    if (retVal)
                        DisplayMessage("Online App Created for Lead " + strLeadID);
                    else
                        DisplayMessage("Error Creating Online App");
                }
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Creating Online App");
            }
        }//end if command name
        else if (e.CommandName == "AddToACT")
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow grdRow = grdFreeReport.Rows[index];
                    string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
                    if (strLeadID != "")
                        AddLeadToACT(Convert.ToInt32(strLeadID), "Free Report");
                }//end if access
                else
                    DisplayMessage("You cannot add this application to ACT!");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Adding Record to ACT!");
            }
        }//end if command name is Add to ACT!
    }

    protected void grdFreeReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow grdRow = grdFreeReport.Rows[e.RowIndex];
            string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
            if (strLeadID != "")
            {
                if (User.IsInRole("Admin"))
                {
                    pnlDeleteConfirm.Visible = true;
                    lblstrLeadID.Text = strLeadID;
                    lblDeleteLeadType.Text = "FreeReport";
                }//end if access
                else
                    DisplayMessage("You cannot delete this application");
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Deleting Lead");
        }
    }

    protected void grdFreeReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (User.IsInRole("Employee"))
            {
                grdFreeReport.Columns[8].Visible = false;
                grdFreeReport.Columns[9].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[9].Text.Trim() == "&nbsp;")
                {
                    e.Row.BackColor = System.Drawing.Color.Gold;
                    e.Row.ToolTip = "This record has not been added to ACT!";
                }
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
    #endregion

    #region FREE CONSULT FUNCTIONS
    protected void grdFreeConsult_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow grdRow = grdFreeConsult.Rows[index];
        string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
        if (e.CommandName == "CreateApp")
        {
            try
            {
                if (strLeadID != "")
                {
                    FirstAffiliatesLeadsBL CreateOnlineApp = new FirstAffiliatesLeadsBL();
                    bool retVal = CreateOnlineApp.CreateApp(Convert.ToInt32(strLeadID), "Free Consult");
                    if (retVal)
                        DisplayMessage("Online App Created for Lead " + strLeadID);
                    else
                        DisplayMessage("Error Creating Online App");
                }
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Creating Online App");
            }
        }//end if command name is CreateApp
        else if (e.CommandName == "MergeApp")
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    pnlMerge.Visible = true;
                    iLeadID = Convert.ToInt16(strLeadID);
                }
                //end if access
                else
                    DisplayMessage("You do not have access to merge lead to an Application");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error merging lead to app");
            }
        }//end if command name is Merge to ACT!            
        else if (e.CommandName == "AddToACT")
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    if (strLeadID != "")
                        AddLeadToACT(Convert.ToInt32(strLeadID), "Free Consult");
                }//end if access
                else
                    DisplayMessage("You cannot add this application to ACT!");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Adding Record to ACT!");
            }
        }//end if command name is Add to ACT!

    }

    protected void grdFreeConsult_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow grdRow = grdFreeConsult.Rows[e.RowIndex];
            string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
            if (strLeadID != "")
            {
                if (User.IsInRole("Admin"))
                {
                    pnlDeleteConfirm.Visible = true;
                    lblstrLeadID.Text = strLeadID;
                    lblDeleteLeadType.Text = "FreeConsult";
                }//end if access
                else
                    DisplayMessage("You cannot delete this application");
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Deleting Lead");
        }
    }

    protected void grdFreeConsult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (User.IsInRole("Employee"))
            {
                grdFreeConsult.Columns[10].Visible = false;
                grdFreeConsult.Columns[11].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[12].Text.Trim() == "&nbsp;")
                {
                    e.Row.BackColor = System.Drawing.Color.Gold;
                    e.Row.ToolTip = "This record has not been added to ACT!";
                }
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    #endregion

    #region FREE APPLY FUNCTIONS

    protected void grdFreeApply_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow grdRow = grdFreeApply.Rows[index];
        string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);

        if (e.CommandName == "CreateApp")
        {
            try
            {
                if (strLeadID != "")
                {
                    FirstAffiliatesLeadsBL CreateOnlineApp = new FirstAffiliatesLeadsBL();
                    bool retVal = CreateOnlineApp.CreateAppExt(Convert.ToInt32(strLeadID), "Free Apply");
                    if (retVal)
                        DisplayMessage("Online App Created for Lead " + strLeadID);
                    else
                        DisplayMessage("Error Creating Online App");
                }
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Creating Online App");
            }
        }//end if command name is CreateApp
        else if (e.CommandName == "MergeApp")
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    pnlMerge.Visible = true;
                    iLeadID = Convert.ToInt16(strLeadID);
                }
                //end if access
                else
                    DisplayMessage("You do not have access to merge lead to an Application");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error merging lead to app");
            }
        }//end if command name is Add to ACT!      
        else if (e.CommandName == "AddToACT")
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    if (strLeadID != "")
                        AddLeadToACT(Convert.ToInt32(strLeadID), "Free Apply");
                }//end if access
                else
                    DisplayMessage("You cannot add this application to ACT!");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error adding Record to ACT!");
            }
        }//end if command name is Add to ACT!  
    }

    protected void grdFreeApply_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow grdRow = grdFreeApply.Rows[e.RowIndex];
            string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
            if (strLeadID != "")
            {
                if (User.IsInRole("Admin"))
                {
                    pnlDeleteConfirm.Visible = true;
                    lblstrLeadID.Text = strLeadID;
                    lblDeleteLeadType.Text = "FreeApply";
                }//end if access
                else
                    DisplayMessage("You cannot delete this application");
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Deleting Lead");
        }
    }

    protected void grdFreeApply_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (User.IsInRole("Employee"))
            {
                grdFreeApply.Columns[11].Visible = false;
                grdFreeApply.Columns[12].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[13].Text.Trim() == "&nbsp;")
                {
                    e.Row.BackColor = System.Drawing.Color.Gold;
                    e.Row.ToolTip = "This record has not been added to ACT!";
                }
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
    #endregion

    #region AFFILIATE FUNCTIONS
    protected void grdAffiliateSignups_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CreateApp")
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdAffiliateSignups.Rows[index];
                string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
                if (strLeadID != "")
                {
                    FirstAffiliatesLeadsBL CreateOnlineApp = new FirstAffiliatesLeadsBL();
                    bool retVal = CreateOnlineApp.CreateAppExt(Convert.ToInt32(strLeadID), "Affiliate Signups");
                    if (retVal)
                        DisplayMessage("Online App Created for Lead " + strLeadID);
                    else
                        DisplayMessage("Error Creating Online App");
                }
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Creating Online App");
            }
        }//end if command name is CreateApp
        else if (e.CommandName == "AddToACT")
        {
            try
            {
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow grdRow = grdAffiliateSignups.Rows[index];
                    string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
                    if (strLeadID != "")
                    {
                        AddAffiliateToACT(Convert.ToInt32(strLeadID));
                    }
                }//end if access
                else
                    DisplayMessage("You cannot add this application to ACT!");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error adding Record to ACT!");
            }
        }//end if command name is Add to ACT! 
        else if (e.CommandName == "UpdateInACT")
        {
            try
            {
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow grdRow = grdAffiliateSignups.Rows[index];
                    string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
                    if (strLeadID != "")
                    {
                        UpdateAffiliateInACT(Convert.ToInt32(strLeadID));
                    }
                }//end if access
                else
                    DisplayMessage("You cannot update this application in ACT!");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error updating Record to ACT!");
            }
        }//end if command name is Update In ACT! 
    }

    protected void grdAffiliateSignups_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow grdRow = grdAffiliateSignups.Rows[e.RowIndex];
            string strLeadID = Server.HtmlDecode(grdRow.Cells[0].Text);
            if (strLeadID != "")
            {
                if (User.IsInRole("Admin"))
                {
                    pnlDeleteConfirm.Visible = true;
                    lblstrLeadID.Text = strLeadID;
                    lblDeleteLeadType.Text = "AffiliateSignup";
                }//end if access
                else
                    DisplayMessage("You cannot delete this application");
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Deleting Lead");
        }
    }

    protected void grdAffiliateSignups_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (User.IsInRole("Employee"))
            {
                grdAffiliateSignups.Columns[1].Visible = false;
                grdAffiliateSignups.Columns[12].Visible = false;
                grdAffiliateSignups.Columns[13].Visible = false;
                grdAffiliateSignups.Columns[14].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.Cells[9].Text != "&nbsp;") && (e.Row.Cells[10].Text.Trim() != "&nbsp;"))
                {
                    DataRowView drView = (DataRowView)e.Row.DataItem;
                    //Gets the PartnerID from table
                    string AffiliateId = drView[0].ToString();

                    //color codes each row based on sync dates
                    if (Convert.ToDateTime(e.Row.Cells[9].Text) > Convert.ToDateTime(e.Row.Cells[10].Text))
                    {
                        e.Row.BackColor = System.Drawing.Color.Salmon;
                        e.Row.ToolTip = "This record was modified but has not been synchronized with ACT!";
                    }
                }
                if (e.Row.Cells[10].Text.Trim() == "&nbsp;")
                {
                    e.Row.BackColor = System.Drawing.Color.Gold;
                    e.Row.ToolTip = "This record has not been added to ACT!";
                }
            }//end if
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    //This function adds Affiliate information to ACT
    public void AddAffiliateToACT(int LeadID)
    {
        try
        {
            FirstAffiliatesLeadsBL Affiliate = new FirstAffiliatesLeadsBL();
            int iRetVal = Affiliate.AddAffiliateInfoToACT(LeadID);
            iLeadID = LeadID;
            if (iRetVal == 1)
                DisplayMessage("Partner information added to ACT!");
            else if (iRetVal == 0)
            {
                lblErrorMessage.Text = "This Partner ID already exists in ACT!";
                pnlConfirm.Visible = true;
            }
            else if (iRetVal == 2)
            {
                lblErrorMessage.Text = "This contact name already exists in ACT!";
                pnlConfirm.Visible = true;
            }
            else if (iRetVal == 3)
            {
                lblErrorMessage.Text = "This email address already exists in ACT!";
                pnlConfirm.Visible = true;
            }
            else if (iRetVal == 4)
            {
                lblErrorMessage.Text = "This phone number already exists in ACT!";
                pnlConfirm.Visible = true;
            }
            else if (iRetVal == 5)
                DisplayMessage("Error adding partner information to ACT!");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Adding Partner to ACT!");
        }

    }//end function AddAffiliateToACT

    //This function adds Affiliate information to ACT
    public void UpdateAffiliateInACT(int LeadID)
    {
        try
        {
            int partnerID = Convert.ToInt32(Session["AffiliateID"]);
            FirstAffiliatesLeadsBL Aff = new FirstAffiliatesLeadsBL();
            int iRetVal = Aff.UpdateAffiliateInfoInACT(LeadID, partnerID);
            iLeadID = LeadID;
            if (iRetVal == 1)
                DisplayMessage("Partner Record not found in ACT! Please add the record to ACT! first.");
            else if (iRetVal == 2)
                DisplayMessage("More than one records for this Partner ID found in ACT! Please correct in ACT! before updating.");
            else if (iRetVal == 3)
                DisplayMessage("The Partner ID was found but the company name for this record has changed. Record will not be updated.");
            else if (iRetVal == 0)
            {
                DisplayMessage("Partner information updated in ACT!");
                Aff.UpdateLastSyncForAffiliates(LeadID);
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating Partner information in ACT!");
        }

    }//end function UpdateAffiliateToACT
    #endregion

    #region COMMON LEAD FUNCTIONS
    //This function adds the lead information to ACT
    public void AddLeadToACT(int LeadID, string LeadType)
    {
        try
        {
            FirstAffiliatesLeadsBL Lead = new FirstAffiliatesLeadsBL();
            int iRetVal = Lead.AddLeadInfoToACT(LeadID, LeadType);
            iLeadID = LeadID;
            if (iRetVal == 2)
            {
                lblErrorMessage.Text = "This contact name already exists in ACT!";
                pnlConfirm.Visible = true;
            }
            else if (iRetVal == 2)
                DisplayMessage("Error adding Lead to ACT!");
            else if (iRetVal == 3)
            {
                lblErrorMessage.Text = "This email address already exists in ACT!";
                pnlConfirm.Visible = true;
            }
            else if (iRetVal == 4)
            {
                lblErrorMessage.Text = "This phone number already exists in ACT!";
                pnlConfirm.Visible = true;
            }

            else
                DisplayMessage("Lead Added to ACT!");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Adding Lead to ACT!");
        }
    }//end function AddLeadToACT

    protected void btnCreateRecordYes_Click(object sender, EventArgs e)
    {
        try
        {
            FirstAffiliatesLeadsBL Lead = new FirstAffiliatesLeadsBL();
            int iRetVal = 0;
            if (lstLeadReport.SelectedItem.Text == "Free Report")
                iRetVal = Lead.CreateNewActRecordFromLeadReport(iLeadID, "Free Report");
            if (lstLeadReport.SelectedItem.Text == "Free Consult")
                iRetVal = Lead.CreateNewActRecordFromLeadConsult(iLeadID, "Free Consult");
            else if (lstLeadReport.SelectedItem.Text == "Free Apply")
                iRetVal = Lead.CreateNewActRecordFromLeadApply(iLeadID, "Free Apply");
            else if (lstLeadReport.SelectedItem.Text == "Partner Signups")
                iRetVal = Lead.CreateNewActRecordFromAffiliate(iLeadID);
            if (iRetVal == 1)
                DisplayMessage("New record created");
            pnlConfirm.Visible = false;
            iLeadID = 0;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error creating new ACT! record");
        }
    }

    protected void btnCreateRecordNo_Click(object sender, EventArgs e)
    {
        pnlConfirm.Visible = false;
        iLeadID = 0;
    }

    protected void btnDeleteYes_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                if (lblstrLeadID.Text != "")
                {
                    if (lblDeleteLeadType.Text == "FreeReport")
                    {
                        FirstAffiliatesLeadsBL DeleteLeadData = new FirstAffiliatesLeadsBL();
                        bool retVal = DeleteLeadData.DeleteLead(Convert.ToInt32(lblstrLeadID.Text.Trim()), "Free Report");
                        if (retVal)
                            DisplayMessage("Free Report Lead " + lblstrLeadID.Text.Trim() + " deleted");
                        lblstrLeadID.Text = "";
                        lblDeleteLeadType.Text = "";
                        PopulateFreeReport();
                    }
                    else if (lblDeleteLeadType.Text == "FreeConsult")
                    {
                        FirstAffiliatesLeadsBL DeleteLeadData = new FirstAffiliatesLeadsBL();
                        bool retVal = DeleteLeadData.DeleteLead(Convert.ToInt32(lblstrLeadID.Text.Trim()), "Free Consult");
                        if (retVal)
                            DisplayMessage("Free Consult Lead " + lblstrLeadID.Text.Trim() + " deleted");
                        lblstrLeadID.Text = "";
                        lblDeleteLeadType.Text = "";
                        PopulateFreeConsult();
                    }
                    else if (lblDeleteLeadType.Text == "FreeApply")
                    {
                        FirstAffiliatesLeadsBL DeleteLeadData = new FirstAffiliatesLeadsBL();
                        bool retVal = DeleteLeadData.DeleteLead(Convert.ToInt32(lblstrLeadID.Text.Trim()), "Free Apply");
                        if (retVal)
                            DisplayMessage("Free Apply Lead " + lblstrLeadID.Text.Trim() + " deleted");
                        lblstrLeadID.Text = "";
                        lblDeleteLeadType.Text = "";
                        PopulateFreeApply();
                    }
                    else if (lblDeleteLeadType.Text == "AffiliateSignup")
                    {
                        FirstAffiliatesLeadsBL DeleteLeadData = new FirstAffiliatesLeadsBL();
                        bool retVal = DeleteLeadData.DeleteLead(Convert.ToInt32(lblstrLeadID.Text.Trim()), "Affiliate Signups");
                        if (retVal)
                            DisplayMessage("Partner Signup Lead " + lblstrLeadID.Text.Trim() + " deleted");
                        lblstrLeadID.Text = "";
                        lblDeleteLeadType.Text = "";
                        PopulateAffiliateSignups("AffiliateID");
                    }
                }//end if leadid not blank
            }//end if user is admin
            else
                DisplayMessage("You cannot delete leads");
            pnlDeleteConfirm.Visible = false;
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void btnDeleteNo_Click(object sender, EventArgs e)
    {
        pnlDeleteConfirm.Visible = false;
    }

    #endregion

    #region SORT BY BUTTON CLICKS
    protected void lnkBtnSortByFirst_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateAffiliateSignups("FirstName");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Affiliates");
        }
    }
    protected void lnkbtnSortByAffiliateID_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateAffiliateSignups("AffiliateID");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Affiliates");
        }
    }
    protected void lnkBtnSortByCompany_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateAffiliateSignups("CompanyName");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Affiliates");
        }
    }
    protected void lnkbtnSortByDBA_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateAffiliateSignups("DBA");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Affiliates");
        }
    }

    protected void lnkBtnSortByLast_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateAffiliateSignups("LastName");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Affiliates");
        }
    }

    #endregion

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        pnlSortBy.Visible = true;
        FirstAffiliatesLeadsBL Leads = new FirstAffiliatesLeadsBL();
        DataSet ds = Leads.GetAffiliatesByLookup(lstLookup.SelectedItem.Value, txtLookup.Text.Trim());
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdAffiliateSignups.DataSource = ds;
            grdAffiliateSignups.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            FirstAffiliatesLeadsBL Lead = new FirstAffiliatesLeadsBL();
            int iRetVal = 0;
            if (lstLeadReport.SelectedItem.Text == "Free Report")
                iRetVal = Lead.CreateNewActRecordFromLeadReport(iLeadID, "Free Report");
            if (lstLeadReport.SelectedItem.Text == "Free Consult")
                iRetVal = Lead.CreateNewActRecordFromLeadConsult(iLeadID, "Free Consult");
            else if (lstLeadReport.SelectedItem.Text == "Free Apply")
                iRetVal = Lead.CreateNewActRecordFromLeadApply(iLeadID, "Free Apply");
            else if (lstLeadReport.SelectedItem.Text == "Partner Signups")
                iRetVal = Lead.CreateNewActRecordFromAffiliate(iLeadID);
            if (iRetVal == 1)
                DisplayMessage("New record created");
            pnlConfirm.Visible = false;
            iLeadID = 0;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error creating new ACT! record");
        }

    }
    protected void btnMerge_Click(object sender, EventArgs e)
    {
        try
        {
            FirstAffiliatesLeadsBL Lead = new FirstAffiliatesLeadsBL();
            int iRetVal = 0;
            int AppID = Convert.ToInt16(txtMerge.Text.Trim());

            iRetVal = Lead.MergeAppFromLeadApply(iLeadID, AppID);

            if (iRetVal == 1)
                DisplayMessage("New record created");
            else if (iRetVal == 2)
                DisplayMessage("App ID does not exist");

            pnlConfirm.Visible = false;
            iLeadID = 0;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error creating new ACT! record");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlConfirm.Visible = false;
        iLeadID = 0;
    }

    protected void btnCancelMerge_Click(object sender, EventArgs e)
    {
        pnlMerge.Visible = false;
        iLeadID = 0;
    }
}
