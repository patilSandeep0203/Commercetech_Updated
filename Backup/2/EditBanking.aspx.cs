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
using OnlineAppClassLibrary;
using DLPartner;
using System.Web.Mail;

public partial class EditBanking : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
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
                Response.Redirect("login.aspx?Authentication=False");
            try
            {
                Populate();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end function Page Load

    public void Populate()
    {
        //Get states
        CommonListData States = new CommonListData();
        DataSet dsStates = States.GetCommonData("States");
        if (dsStates.Tables["States"].Rows.Count > 0)
        {
            lstBankState.DataSource = dsStates.Tables["States"];
            lstBankState.DataTextField = "StateID";
            lstBankState.DataValueField = "StateID";
            lstBankState.DataBind();
        }//end if count not 0

        //Get Banks
        CommonListData Banks = new CommonListData();
        DataSet dsBanks = Banks.GetCommonData("Banks");
        if (dsBanks.Tables[0].Rows.Count > 0)
        {
            lstBankName.DataSource = dsBanks.Tables[0];
            lstBankName.DataTextField = "BankName";
            lstBankName.DataValueField = "BankName";
            lstBankName.DataBind();
        }//end if count not 0

        int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);

        AffiliatesBL Affiliate = new AffiliatesBL(AffiliateID);
        //Get Banking Info For Affiliate
        PartnerDS.AffiliateBankingDataTable DTBanking = Affiliate.GetAffiliateBanking();

        if (DTBanking.Rows.Count > 0)
        {
            lstBankName.Text = Server.HtmlEncode(DTBanking[0].BankName.Trim());
            txtOtherBank.Text = Server.HtmlEncode(DTBanking[0].OtherBank.Trim());
            txtBankAddress.Text = Server.HtmlEncode(DTBanking[0].BankAddress.Trim());
            //lblBankAddress2.Text = DTBanking[0].Address2.Trim();
            txtZipCode.Text = Server.HtmlEncode(DTBanking[0].BankZip.Trim());
            txtPhone.Text = Server.HtmlEncode(DTBanking[0].BankPhone.Trim());
            lstBankState.Text = Server.HtmlEncode(DTBanking[0].BankState.Trim());
            txtBankCity.Text = Server.HtmlEncode(DTBanking[0].BankCity.Trim());
            txtNameOnChecking.Text = Server.HtmlEncode(DTBanking[0].NameonCheckingAcct.Trim());
            
            //Don't display Account Number and Password for security reasons.
            txtAcctNumber.Text = "";
            txtBankRoutingNumber.Text = "";
        }
        else
        {
            lstBankName.Text = "";
            txtOtherBank.Text = "";
            txtBankAddress.Text = "";
            txtZipCode.Text = "";
            txtPhone.Text = "";
            lstBankState.Text = "";
            txtBankCity.Text = "";
            txtNameOnChecking.Text = "";
            txtAcctNumber.Text = "";
            txtBankRoutingNumber.Text = "";
        }

    }//end function Populate

    //This function handles bank list selection changed event
    protected void lstBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstBankName.SelectedItem.Text.Trim() == "OTHER")
        {
            txtOtherBank.Enabled = true;
            txtOtherBank.BackColor = System.Drawing.Color.White;
            ValidateOtherBank.Enabled = true;
        }
        else
        {
            txtOtherBank.Enabled = false;
            txtOtherBank.BackColor = System.Drawing.Color.DarkGray;
            txtOtherBank.Text = "";
            ValidateOtherBank.Enabled = false;
        }
    }//end function Bank list selection changed event

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                //Validate data                
                if (ValidateData())
                {                    
                    if ((txtBankRoutingNumber.Text.Trim().Length < 9) || (txtBankRoutingNumber.Text.Trim().Length > 9))
                    {
                        DisplayMessage("Length of the Bank Routing Number must be 9 characters long.");
                    }
                    else
                    {
                        if ((txtBankRoutingNumber.Text.Trim().Length < 9) || (txtBankRoutingNumber.Text.Trim().Length > 9))
                            DisplayMessage("Routing number must be exactly 9 characters long");
                        else
                        {
                            int AffiliateID = Convert.ToInt16(Session["AffiliateID"].ToString());
                            string strReplace = "";

                            SendEmail();

                            AffiliatesBL Affiliate = new AffiliatesBL(AffiliateID);
                            Affiliate.InsertUpdateBankingInfo(lstBankName.SelectedItem.Text.Trim().Replace("'", strReplace),
                                txtOtherBank.Text.Trim().Replace("'", strReplace), txtBankAddress.Text.Trim().Replace("'", strReplace), txtZipCode.Text.Trim().Replace("'", strReplace), txtBankCity.Text.Trim().Replace("'", strReplace),
                                lstBankState.SelectedItem.Text.Trim().Replace("'", strReplace), txtNameOnChecking.Text.Trim().Replace("'", strReplace), txtAcctNumber.Text.Trim(),
                                txtBankRoutingNumber.Text.Trim(), txtPhone.Text.Trim().Replace("'", strReplace));  
                        }
                        Response.Redirect("EditInfo.aspx", false);                        
                    }                                        
                }//end if ValidateDate
            }//end if Page valid
        }//end try
        catch (Exception)
        {
            DisplayMessage("Error Processing Request. Please contact technical support");
        }        
    }

    //This function emails Admin
    public void SendEmail()
    {
        try
        {
            //Send Email to agent after successful registration before redirecting user
            string strSubject = "E-Commerce Exchange - Partner Banking Information Updated";
            //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("accounting@ecenow.com");
            MailMessage msg = new MailMessage();
            msg.To = "information@ecenow.com; accounting@ecenow.com";
            msg.From = "information@ecenow.com";
            msg.Subject = strSubject;
            msg.Body = GetBody();
            SmtpMail.Send(msg);
            //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            //smtp.Send(msg);
            
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
        string strBody = "Banking Information for " + Session["AffiliateName"].ToString() + " (PartnerID: " + Session["AffiliateID"].ToString() + 
                         ") has been updated. Please request Voided Check and Direct Deposit Authorization form for new Banking Information. Then set up Direct Deposit for partner at Bank of America.";
        strBody = strBody + System.Environment.NewLine;
        strBody = strBody + System.Environment.NewLine;
        strBody = strBody + "Thank You, " + System.Environment.NewLine;
        strBody = strBody + "E-Commerce Exchange" + System.Environment.NewLine;

        return strBody;
    }//end function GetBody

    //This function validates data in text fields
    protected bool ValidateData()
    {
        TextBox txtBox = new TextBox();
        for (int i = 0; i < pnlMainPage.Controls.Count; i++)
        {
            if (pnlMainPage.Controls[i].GetType() == txtBox.GetType())
            {
                txtBox = (TextBox)pnlMainPage.Controls[i];
                if ((txtBox.Text.Contains("--")) || (txtBox.Text.Contains("#")) || (txtBox.Text.Contains(";")) || (txtBox.Text.Contains("'")))
                {
                    DisplayMessage("You cannot use hyphens, apostrophe, # or semi-colons in any of the following fields.");
                    return false;
                }
            }
        }
        return true;
    }//end function validate data

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

    //This function masks numbers
    protected string MaskNumbers(string strValue)
    {
        string Number = Server.HtmlEncode(strValue);
        string Num = "";
        int j = 0;
        if (Number.Length >= 4)
        {
            for (int i = 0; i < Number.Length - 4; i++)
            {
                Num += "x";
                j++;
            }
        }
        Number = Number.Substring(j);
        Num += Number;
        return Num;
    }
}
