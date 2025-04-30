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
using System.Xml;
using System.Text;
using AjaxControlToolkit;

public partial class Root : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)//Check if session variables have been set, else redirect to logout
                Response.Redirect("../logout.aspx");
            //This code changes the master file based on the access value. The master file has a menu and each
            //access type has a different menu
            bool menuDocsLogins = false;
            foreach (MenuItem item in mnuMain.Items)
            {
                if (Convert.ToString(item.Text).Contains("Documents And Logins"))
                {
                    menuDocsLogins = true;
                }
            }

            if (menuDocsLogins == false)
            {
                if (HttpContext.Current.User.IsInRole("Agent"))
                {
                    //Page.MasterPageFile = "~/AgentMaster.master";
                    mnuMain.Items.Add(new MenuItem
                        (
                            "Documents And Logins", "Documents And Logins", "", "DocsLoginsAgent.aspx"
                        ));
                }
                else if (HttpContext.Current.User.IsInRole("T1Agent"))
                {
                    mnuMain.Items.Add(new MenuItem
        (
           "Documents And Logins", "Documents And Logins", "", "DocsLoginsAgent.aspx"
        ));
                }
                else if (HttpContext.Current.User.IsInRole("Admin"))
                {


                            mnuMain.Items.Add(new MenuItem
                                (
                                    "Documents And Logins", "Documents And Logins", "", "DocsLogins.aspx"
                                ));

                }
                else if (HttpContext.Current.User.IsInRole("Employee"))
                {
                    mnuMain.Items.Add(new MenuItem
        (
            "Documents And Logins", "Documents And Logins", "", "DocsLogins.aspx"
        ));
                }
            }
        }
    }
}
