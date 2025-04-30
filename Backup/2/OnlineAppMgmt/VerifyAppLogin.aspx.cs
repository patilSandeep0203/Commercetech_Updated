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

public partial class VerifyAppLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin") && !User.IsInRole("Employee") && !User.IsInRole("Agent") && !User.IsInRole("T1Agent"))
            Response.Redirect("~/login.aspx?Authentication=False");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                if (Request.Params.Get("AppID") != null)
                {
                    int AppID = Convert.ToInt32(Request.Params.Get("AppID"));
                    Verify(AppID);
                }
                else
                    DisplayMessage("Invalid Request.");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage(err.Message);
            }
        }
    }

    public void Verify(int AppID)
    {
        int AffiliateID = Convert.ToInt16(Session["AffiliateID"]);
        AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
        bool redirect = false;
        string token = String.Empty;
        try
        {
            // Create a Token for this user/app
            token = Aff.CreateOnlineAppToken("OnlineApp", User.Identity.Name, AppID);            
            if (token != "")
                redirect = true;
            else
                DisplayMessage("You are not authorized to view this resource.");            
        }//end try
        catch (Exception err)
        {
            redirect = false;
            DisplayMessage(err.Message);
        }

        if (redirect)
        {
            // Redirect to web application 
            // passing in the generated token
            OnlineAppDL newOnlineAppDL = new OnlineAppDL();

            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
            {
                newOnlineAppDL.UpdateDocusignAccess(AppID, "Agent");
            }
            else { newOnlineAppDL.UpdateDocusignAccess(AppID, "Admin"); }
            Response.Redirect("../../OnlineApplication/login.aspx" + "?Token=" + token + "&AppId=" + AppID, false);
        }
    }//end function Verify

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
