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


public partial class ManageLeadsPartner : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Page.MasterPageFile = "Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "User.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "Admin.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "Employee.master";
        }
    }

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
            Response.Redirect("login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("login.aspx?Authentication=False");

        }//end if not post back
    }//end page load
        
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            string strLeadType = lstLeadReport.SelectedValue;
            switch (strLeadType)
            {
                case "Leads":
                    grdAffiliateSignups.Visible = false;
                    grdAllLeads.Visible = true;
                    PopulateLeadReport();
                    break;
                case "Affiliate Signups":
                    grdAffiliateSignups.Visible = true;
                    grdAllLeads.Visible = false;
                    PopulateAffiliateSignups();
                    break;
            }//end switch
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Leads Information. Please try again later");
        }
    }//end button submit click

    //This function populates table with leads information
    public void PopulateLeadReport()
    {
        FirstAffiliatesLeadsBL LeadsReport = new FirstAffiliatesLeadsBL();
        DataSet ds = LeadsReport.GetLeadsPartner("Leads", Session["AffiliateID"].ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdAllLeads.DataSource = ds;
            grdAllLeads.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }//end function PopulateFreeReport

    //This function populates table with Affiliate Signup information
    public void PopulateAffiliateSignups()
    {
        FirstAffiliatesLeadsBL AffiliateSignups = new FirstAffiliatesLeadsBL();
        DataSet ds = AffiliateSignups.GetLeadsPartner("Affiliate Signups", Session["AffiliateID"].ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdAffiliateSignups.DataSource = ds;
            grdAffiliateSignups.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }//end function PopulateAffiliateSignups

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

}
