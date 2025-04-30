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
using System.Web.Mail;
using DLPartner;

public partial class ChangePWD : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if ( Session.Keys.Count == 0 )
                Response.Redirect("~/logout.aspx?Authentication=False");
            if (User.IsInRole("Agent"))
                Page.MasterPageFile = "~/AgentMisc.master";
            else if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "~/T1Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "~/UserMisc.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "~/site.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "~/Employee.master";
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
        
        if (!IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/logout.aspx?Authentication=False");
        }
    }

    //This function handles submit button click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);
                UsersBL User = new UsersBL();
                int iRetVal = User.ResetAffiliatePassword(AffiliateID, txtPassword.Text.Trim());
                if (iRetVal > 0)
                {
                    //send email to accounts that password has been changed
                    SendEmail();
                    DisplayMessage("Password Updated Successfully.");
                }
                else
                    DisplayMessage("Error updating Password");
            }//end page isvalid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }//end submit button click

    //This function emails Admin
    public void SendEmail()
    {
        try
        {
            //Send Email to agent after successful registration before redirecting user
            string strSubject = "E-Commerce Exchange - Affiliate Password changed";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("information@ecenow.com", "accounting@ecenow.com");
            msg.Subject = strSubject;
            msg.Body = GetBody();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Send(msg);
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }//end function send email

    //This function creates email body
    public string GetBody()
    {
        int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);
        AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
        PartnerDS.AffiliatesDataTable dt = Aff.GetAffiliate();
        string AffiliateName = "";
        if (dt.Rows.Count > 0)
        {
            AffiliateName = dt[0].Contact.ToString().Trim();
        }
        string strBody = "Password for " + AffiliateName + " has been Changed" + System.Environment.NewLine;
        strBody = strBody + System.Environment.NewLine;
        strBody = strBody + "Thank You, " + System.Environment.NewLine;
        strBody = strBody + "E-Commerce Exchange" + System.Environment.NewLine;

        return strBody;
    }//end function GetBody

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("editinfo.aspx");
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
