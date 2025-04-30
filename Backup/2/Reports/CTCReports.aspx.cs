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

public partial class Reports_CTCReports : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)//Check if session variables have been set, else redirect to logout
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Affiliate"))
                Page.MasterPageFile = "Affiliates.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Page.MasterPageFile = "Agent.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "Admin.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
