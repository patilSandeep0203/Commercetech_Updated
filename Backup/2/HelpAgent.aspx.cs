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

public partial class HelpAgent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session.IsNewSession)
       //     Response.Redirect("~/logout.aspx?SessionExpired=True", false);

        if (!IsPostBack)
        {
          //  if ( !User.Identity.IsAuthenticated )
          //      Response.Redirect("~/login.aspx?Authentication=False");

            if (User.IsInRole("Admin"))
            {
                pnlAdminQs.Visible = true;
                pnlAdminAns.Visible = true;
                pnlEmployeeQs.Visible = true;
                pnlEmployeeAns.Visible = true;
            }
            else if (User.IsInRole("Employee"))
            {
                pnlEmployeeQs.Visible = true;
                pnlEmployeeAns.Visible = true;
                pnlAdminQs.Visible = false;
                pnlAdminAns.Visible = false;
            }
            else
            {
                pnlAdminQs.Visible = false;
                pnlAdminAns.Visible = false;
                pnlEmployeeQs.Visible = false;
                pnlEmployeeAns.Visible = false;
            }


        }//end if not postback
    }
}
