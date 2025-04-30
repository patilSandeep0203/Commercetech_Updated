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

public partial class OnlineAppMgmt_main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/logout.aspx?Authentication=False", false);

            if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Response.Redirect("OnlineAppAgent.aspx?OnlineApp=True", false);
            else if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                Response.Redirect("default.aspx?OnlineApp=True", false);
            else if (User.IsInRole("Affiliate") || User.IsInRole("Reseller"))
                Response.Redirect("OnlineAppReferrals.aspx", false);
            else
                Response.Redirect("~/Misc.aspx", false);
        }
    }
}
