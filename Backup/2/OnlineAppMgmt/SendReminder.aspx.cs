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
//using System.Web.Mail;
using System.Net.Mail;
using BusinessLayer;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

public partial class SendReminder : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    private static int AppId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if ((Request.Params.Get("AppId") != null))
        {
            if (Int32.TryParse(Request.Params.Get("AppId").ToString(), out AppId))
                AppId = Convert.ToInt32(Request.Params.Get("AppId"));
            else
                DisplayMessage("Invalid request");
        }

        if (!IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                //********************************CHECK ACCESS********************************
                //Check if the partner logged in has access to the App. If not redirect to login.aspx
                bool bAccess = false;
                if (User.IsInRole("Employee") || User.IsInRole("Admin"))
                    bAccess = true;
                else
                {
                    OnlineAppBL CheckRepAccess = new OnlineAppBL(AppId);
                    bAccess = CheckRepAccess.CheckAccess(Session["AffiliateID"].ToString());
                }
                if (!bAccess)
                    Response.Redirect("~/login.aspx?Authentication=False");

                //********************************END CHECK ACCESS********************************

                Style TextArea = new Style();
                TextArea.Width = new Unit(600);
                TextArea.Height = new Unit(250);
                TextArea.Font.Size = FontUnit.Point(9);
                TextArea.Font.Name = "Arial";
                txtBody.ApplyStyle(TextArea);

                PopulateEmail();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Loading Email Information");
            }
        }//end ispostback
    }

    //This function Populates Email Information
    public void PopulateEmail()
    {
        try
        {
            //Get application information and create email message
            Reminder Reminder = new Reminder(AppId);
            PartnerDS.OnlineAppReminderDataTable dt = Reminder.GetReminderSummary();
            if (dt.Rows.Count > 0)
            {
                string AcctType = dt[0].AcctTypeDesc.ToString();

                /*int RepNum = 1503;
                if (dt[0].RepNum.ToString().Trim() != "")
                    RepNum = Convert.ToInt32(dt[0].RepNum);*/

                string SalesRepEmail = "information@ecenow.com";
                if (dt[0].SalesRepEmail.ToString().Trim() != "")
                    SalesRepEmail = dt[0].SalesRepEmail.ToString().Trim();
                txtTo.Text = dt[0].Email.ToString().Trim();
                txtFrom.Text = SalesRepEmail;
                txtSubject.Text = "E-Commerce Exchange Online Application Reminder";

                string RepName = "Jay Scott";
                if (dt[0].RepName.ToString().Trim() != "")
                    RepName = dt[0].RepName.ToString().Trim();

                string strMessage = "Dear " + dt[0].FirstName.ToString().Trim() + " " + dt[0].LastName.ToString().Trim() + ", " + System.Environment.NewLine + System.Environment.NewLine;
                strMessage = strMessage + "Thank you for signing up for a " + AcctType + " Account with E-Commerce Exchange." + System.Environment.NewLine;
                strMessage = strMessage + "Your Online Application information is not yet complete." + System.Environment.NewLine;
                strMessage = strMessage + "Our records show that you last logged in to the Online Application at " + dt[0].LastModifiedDate.ToString().Trim() + "." + System.Environment.NewLine;
                strMessage = strMessage + "Please log in to your application by clicking on the following link: " + System.Environment.NewLine;
                strMessage = strMessage + "https://www.firstaffiliates.com/OnlineApplication/default.aspx?AppId=" + AppId + System.Environment.NewLine + System.Environment.NewLine;
                strMessage = strMessage + "Thank you, " + System.Environment.NewLine + System.Environment.NewLine;
                strMessage = strMessage + RepName;
                txtBody.Text = strMessage;
            }
            //else { DisplayMessage("Not Able to load Email Information"); }//end if count not 0
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Showing Email Information");
        }
    }//end PopulateEmail

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> { self.close() }</script>");
    }

    //This function handles send email button click
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        try
        {
            bool retVal = false;
            SendEmail();
            int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);
            string partnerID = Convert.ToString(Session["AffiliateID"]);
            //Insert Note
            AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
            PartnerLogBL LogData = new PartnerLogBL();
            //string PortalUserID = LogData.ReturnPortalUserID((Convert.ToInt32(Session["AffiliateID"])));
            //string strActUserID = Aff.ReturnACTUserID();
            Reminder InsertNoteInfo = new Reminder(AppId);
            string strNote = "Reminder Email Sent." + System.Environment.NewLine + txtBody.Text;            
            //Insert Note in Online App and ACT
            //InsertNoteInfo.InsertNoteReminder(partnerID, strNote);
            retVal = LogData.InsertLogData(AppId, AffiliateID, "Reminder Email Sent.");

            /* CODE for adding email to history
            string strTo = txtTo.Text.Trim();
            string strCc = txtCC.Text.Trim();
            //string strFrom = txtFrom.Text.Trim();
            string strSubject = txtSubject.Text.Trim();
            string strBody = txtBody.Text;
            InsertNoteInfo.InsertACTHistory(strActUserID, strTo, strCc, strSubject, strBody);
             */
            DisplayMessage("Reminder Email Sent");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Sending Reminder Email");
        }
    }

    //This function emails Admin
    public void SendEmail()
    {
        try
        {
            //Send Email to agent after successful registration before redirecting user
            MailMessage msg = new MailMessage();
            //msg.To = txtTo.Text.Trim();
            msg.To.Add(new MailAddress(txtTo.Text.Trim()));
            //msg.Cc = txtCC.Text.Trim();
            if (txtCC.Text.Trim() != "")
                msg.CC.Add(new MailAddress(txtCC.Text.Trim()));
            //msg.From = txtFrom.Text.Trim();
            msg.From = new MailAddress(txtFrom.Text.Trim());
            msg.Subject = txtSubject.Text.Trim();
            msg.Body = txtBody.Text;
            //SmtpMail.Send(msg);
            SmtpClient mSmtpClient = new SmtpClient();
            mSmtpClient.Send(msg);
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }//end function send email

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
