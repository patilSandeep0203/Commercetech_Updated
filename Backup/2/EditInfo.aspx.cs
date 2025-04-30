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
using OnlineAppClassLibrary;
using DLPartner;
//using System.Web.Mail;
using System.Net.Mail;

public partial class EditInfo : System.Web.UI.Page
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

        if (Session.IsNewSession)
            Response.Redirect("~/logout.aspx?SessionExpired=True", false);

        if (!IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("login.aspx?Authentication=False");
            try
            {
                Style TextArea = new Style();
                TextArea.Width = new Unit(140);
                TextArea.Height = new Unit(50);
                TextArea.Font.Size = FontUnit.Point(8);
                TextArea.Font.Name = "Arial";
                txtComments.ApplyStyle(TextArea);

                Populate();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function populates the form fields
    public void Populate()
    {
        //Get states
        CommonListData States = new CommonListData();
        DataSet dsStates = States.GetCommonData("States");
        if (dsStates.Tables["States"].Rows.Count > 0)
        {
            lstState.DataSource = dsStates.Tables["States"];
            lstState.DataTextField = "StateID";
            lstState.DataValueField = "StateID";
            lstState.DataBind();

            lstMailingState.DataSource = dsStates.Tables["States"];
            lstMailingState.DataTextField = "StateID";
            lstMailingState.DataValueField = "StateID";
            lstMailingState.DataBind();
        }//end if count not 0

        //Get Countries
        CommonListData Countries = new CommonListData();
        DataSet dsCountry = Countries.GetCommonData("Countries");
        if (dsCountry.Tables["Countries"].Rows.Count > 0)
        {
            lstCountry.DataSource = dsCountry.Tables["Countries"];
            lstCountry.DataTextField = "Country";
            lstCountry.DataValueField = "Country";
            lstCountry.DataBind();

            lstMailingCountry.DataSource = dsCountry.Tables["Countries"];
            lstMailingCountry.DataTextField = "Country";
            lstMailingCountry.DataValueField = "Country";
            lstMailingCountry.DataBind();

        }//end if count not 0
        lstCountry.SelectedItem.Text = "United States";
        lstCountry.SelectedItem.Value = "United States";

        lstMailingCountry.SelectedItem.Text = "United States";
        lstMailingCountry.SelectedItem.Value = "United States";
                
        int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);

        AffiliatesBL Affiliate = new AffiliatesBL(AffiliateID);
        PartnerDS.AffiliatesDataTable affDT = Affiliate.GetAffiliate();

        if (affDT.Rows.Count > 0)
        {     
            //lblUserName.Text = Server.HtmlEncode(dr["sLoginID"].Trim());
            lblUserName.Text = affDT[0].sLoginID.ToString();
            
            txtFirstName.Text = affDT[0].FirstName.ToString();
            txtLastName.Text = affDT[0].LastName.Trim();
            txtPasswordPhrase.Text = affDT[0].Phrase.Trim();
            lblCompanyName.Text = affDT[0].CompanyName.Trim();
            txtDBAName.Text = affDT[0].DBA.Trim();
            lstLegalStatus.SelectedValue = lstLegalStatus.Items.FindByText(affDT[0].LegalStatus.ToString()).Value;
            if (lstLegalStatus.Text != "")
                lstLegalStatus.Enabled = false;
            else
                lstLegalStatus.Enabled = true;

            txtEmail.Text = affDT[0].Email.Trim();
            txtAddress.Text = affDT[0].CompanyAddress.Trim();
            txtCity.Text = affDT[0].City.Trim();
            if (affDT[0].State.ToString() != "")
                lstState.SelectedValue= lstState.Items.FindByText(affDT[0].State.ToString()).Value;
  
            lstCountry.SelectedValue = lstCountry.Items.FindByText(affDT[0].Country.ToString()).Value;

            //txtBusRegion.Text = affDT[0].Region.Trim();
            txtZip.Text = affDT[0].Zip.Trim();
            txtMailingAddress.Text = affDT[0].MailingAddress.Trim();
            txtMailingCity.Text = affDT[0].MailingCity.Trim();
            txtMailingZip.Text = affDT[0].MailingZip.Trim();
            //txtMailingRegion.Text = affDT[0].MailingRegion.Trim();
            if (affDT[0].MailingState.ToString() != "")
                lstMailingState.SelectedValue =lstMailingState.Items.FindByText(affDT[0].MailingState.ToString()).Value;
            
            if (affDT[0].MailingCountry.ToString() != "")
            lstMailingCountry.SelectedValue = lstMailingCountry.Items.FindByValue(affDT[0].MailingCountry.ToString()).Value;
                
            if (affDT[0].DirectDeposit.ToString() != "")
            {
                if (Convert.ToBoolean(affDT[0].DirectDeposit))
                {
                    pnlBanking.Visible = true;

                    rdbDDYes.Checked = true;
                    rdbDDNo.Checked = false;

                    //Get Banking Info For Affiliate
                    PartnerDS.AffiliateBankingDataTable DTBanking = Affiliate.GetAffiliateBanking();
                    if (DTBanking.Rows.Count > 0)
                    {
       
                        lblBankName.Text = Server.HtmlEncode(DTBanking[0].BankName.Trim());
                        lblOtherBank.Text = Server.HtmlEncode(DTBanking[0].OtherBank.Trim());
                        lblBankAddress.Text = Server.HtmlEncode(DTBanking[0].BankAddress.Trim());
                        //lblBankAddress2.Text = DTBanking[0].Address2.Trim();
                        lblZipCode.Text = Server.HtmlEncode(DTBanking[0].BankZip.Trim());
                        lblBankPhone.Text = Server.HtmlEncode(DTBanking[0].BankPhone.Trim());
                        lblBankState.Text = Server.HtmlEncode(DTBanking[0].BankState.Trim());
                        lblBankCity.Text = Server.HtmlEncode(DTBanking[0].BankCity.Trim());
                        lblNameOnChecking.Text = Server.HtmlEncode(DTBanking[0].NameonCheckingAcct.Trim());

                        //string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(lblPassword.Text.Trim() + GetSalt(), "sha1");
                        //This code displays only the last 4 digits of the account number            
                        lblAcctNumber.Text = "xxxxxx" + DTBanking[0].BankAccountNumber.Trim();

                        //This code displays only the last 4 digits of the routing number                        
                        lblBankRoutingNumber.Text = "xxxxxx" + DTBanking[0].BankRoutingNumber.Trim();
                    }//end if count not 0
                }//end if directdeposit is true
                else
                {
                    rdbDDYes.Checked = false;
                    rdbDDNo.Checked = true;
                    pnlBanking.Visible = false;
                }
            }
            else
            {
                rdbDDYes.Checked = false;
                rdbDDNo.Checked = true;
                pnlBanking.Visible = false;
            }

            txtPhone.Text = affDT[0].Telephone.Trim();
            txtHomePhone.Text = affDT[0].HomePhone.Trim();
            txtMobilePhone.Text = affDT[0].MobilePhone.Trim();
            txtFax.Text = affDT[0].Fax.Trim();
            txtURL.Text = affDT[0].WebSiteURL.Trim();
            txtComments.Text = affDT[0].Comments.Trim();
             if (affDT[0].SendEmailNotification)
                lstNotify.SelectedIndex = 0;
             else
                lstNotify.SelectedIndex = 1;
            
            if (affDT[0].CheckPayable.ToString() == "DBA")
                rdbDBA.Checked = true;
            else
                rdbLegalName.Checked = true;

            if ((affDT[0].SocialSecurity.Trim() == "") && (affDT[0].TaxID.Trim() == ""))
            {
                txtTaxSSN.Enabled = true;
                rdbSSN.Enabled = true;
                rdbTaxID.Enabled = true;
                txtTaxSSN.Text = "";
            }
            else
            {
                txtTaxSSN.Enabled = false;
			    rdbSSN.Enabled = false;
			    rdbTaxID.Enabled = false;
            }
                        
            if (affDT[0].TaxSSN.Trim() == "TaxID")
            {
                rdbTaxID.Checked = true;
                txtTaxSSN.Text = "xxxxxx" + affDT[0].TaxID.Trim();
            }
            else if (affDT[0].TaxSSN.Trim() == "SSN")
            {
                rdbSSN.Checked = true;
                txtTaxSSN.Text = "xxxxxx" + affDT[0].SocialSecurity.Trim();
            }                                   
	                    
        }//end if count not 0
    }//end function populate

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
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        rdbSSN.Enabled = true;
        rdbTaxID.Enabled = true;
        txtTaxSSN.Enabled = true;
        txtTaxSSN.Text = "";
        lnkEdit.Visible = false;
    }

    protected void lnkEditBankInfo_Click(object sender, EventArgs e)
    {
        btnSubmit_Click(sender, e);
        Response.Redirect("EditBanking.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                //Check which fields have been modified
                int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);
                AffiliatesBL Affiliate = new AffiliatesBL(AffiliateID);
                PartnerDS.AffiliatesDataTable affDT = Affiliate.GetAffiliate();
                
                if (affDT.Rows.Count > 0)
                {
                    //initialize the Tax or SSN to denote to the database to NOT Update the field                    
                    string TaxSSN = "-";
                    string FederalTaxID = "-";
                    string SocialSecurity = "-";

                    //if Tax or SSN has been for editing
                    if (txtTaxSSN.Enabled)
                    {
                        if (rdbTaxID.Checked)
                        {
                            TaxSSN = "TaxID";
                            FederalTaxID = txtTaxSSN.Text.Trim();
                            //SocialSecurity = "";
                        }
                        else
                        {
                            TaxSSN = "SSN";
                            SocialSecurity = txtTaxSSN.Text.Trim();
                            //FederalTaxID = "";
                        }
                    }

                    string strCheckPayable = "";
                    if (rdbDBA.Checked)
                        strCheckPayable = "DBA";
                    else
                        strCheckPayable = "LegalName";

                    bool bNotify = false;
                    if (affDT[0].SendEmailNotification)
                        bNotify = true;
                    else
                        bNotify = false;

                    SendEmail();

                    //Update Information in database
                    AffiliatesBL UpdateAffiliate = new AffiliatesBL(AffiliateID);
                    bool retVal = UpdateAffiliate.UpdateAffiliateWiz(txtFirstName.Text.Trim(),
                            txtLastName.Text.Trim(), txtPasswordPhrase.Text.Trim(), txtDBAName.Text.Trim(),
                            strCheckPayable, txtEmail.Text.Trim(), TaxSSN, FederalTaxID, SocialSecurity,
                            txtAddress.Text.Trim(), txtCity.Text.Trim(), lstState.SelectedItem.Value.Trim(),
                            txtBusRegion.Text.Trim(), txtZip.Text.Trim(), lstCountry.SelectedItem.Value.Trim(),
                            txtMailingAddress.Text.Trim(), txtMailingCity.Text.Trim(), lstMailingState.SelectedItem.Value.Trim(),
                            txtMailingRegion.Text.Trim(), txtMailingZip.Text.Trim(),
                            lstMailingCountry.SelectedItem.Value.Trim(), txtPhone.Text.Trim(),
                            txtHomePhone.Text.Trim(), txtMobilePhone.Text.Trim(),
                            txtFax.Text.Trim(), txtURL.Text.Trim(), txtComments.Text.Trim(),
                            bNotify, lstLegalStatus.SelectedItem.Text.Trim(), rdbDDYes.Checked);                                       

                }//end if count not 0
                DisplayMessage("Information Updated");
            }//end if page valid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }//end button click


    //This function emails Admin
    public void SendEmail()
    {
        try
        {
            //Send Email to agent after successful registration before redirecting user
            string strSubject = "E-Commerce Exchange - Partner Information Updated";
            MailMessage msg = new MailMessage();
            msg.Subject = strSubject;
            //msg.To = "information@ecenow.com; accounting@ecenow.com";
            //msg.From = "information@ecenow.com";
            msg.To.Add(new MailAddress("information@ecenow.com"));
            msg.To.Add(new MailAddress("accounting@ecenow.com"));
            msg.From = new MailAddress("information@ecenow.com"); 
            msg.Body = GetBody();
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

    //This function creates email body
    public string GetBody()
    {
        string strBody = "Information for " + txtFirstName.Text.Trim() + " (PartnerID: " + Session["AffiliateID"].ToString() + ") has been updated. The updated information is as follows: " + System.Environment.NewLine;
        strBody = strBody + System.Environment.NewLine;

        //Check which fields have been modified
        int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);
        AffiliatesBL Affiliate = new AffiliatesBL(AffiliateID);
        PartnerDS.AffiliatesDataTable affDT = Affiliate.GetAffiliate();
        if (affDT.Rows.Count > 0)
        {
            //string strMail = "PartnerID: " + AffiliateID + System.Environment.NewLine;
            if (affDT[0].FirstName.Trim() != txtFirstName.Text.Trim())
                strBody += "First Name: " + txtFirstName.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].LastName.Trim() != txtLastName.Text.Trim())
                strBody += "Last Name: " + txtLastName.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].Phrase.Trim() != txtPasswordPhrase.Text.Trim())
                strBody += "Password Phrase: " + txtPasswordPhrase.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].DBA.Trim() != txtDBAName.Text.Trim())
                strBody += "DBA: " + txtDBAName.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].LegalStatus.Trim() != lstLegalStatus.SelectedItem.Text.Trim())
                strBody += "Legal Status: " + lstLegalStatus.SelectedItem.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].Email.Trim() != txtEmail.Text.Trim())
                strBody += "Email: " + txtEmail.Text.Trim() + System.Environment.NewLine;
            //Business Address Check
            if (affDT[0].CompanyAddress.Trim() != txtAddress.Text.Trim())
                strBody += "Address: " + txtAddress.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].City.Trim() != txtCity.Text.Trim())
                strBody += "City: " + txtCity.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].State.Trim() != lstState.SelectedItem.Value.Trim())
                strBody += "State: " + lstState.SelectedItem.Value.Trim() + System.Environment.NewLine;
            //if (affDT[0].Region.Trim() != txtBusRegion.Text.Trim())
            //strMail += "Region: " + txtBusRegion.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].Zip.Trim() != txtZip.Text.Trim())
                strBody += "Zip: " + txtZip.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].Country.Trim() != lstCountry.SelectedItem.Value.Trim())
                strBody += "Country: " + lstCountry.SelectedItem.Value.Trim() + System.Environment.NewLine;
            //Mailing Address Check
            if (affDT[0].MailingAddress.Trim() != txtMailingAddress.Text.Trim())
                strBody += "Mailing Address: " + txtMailingAddress.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].MailingCity.Trim() != txtMailingCity.Text.Trim())
                strBody += "Mailing City: " + txtMailingCity.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].MailingState.Trim() != lstMailingState.SelectedItem.Value.Trim())
                strBody += "Mailing State: " + lstMailingState.SelectedItem.Value.Trim() + System.Environment.NewLine;
            //if (affDT[0].MailingRegion.Trim() != txtMailingRegion.Text.Trim())
            //strMail += "Mailing Region: " + txtMailingRegion.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].MailingZip.Trim() != txtMailingZip.Text.Trim())
                strBody += "Mailing Zip: " + txtMailingZip.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].MailingCountry.Trim() != lstMailingCountry.SelectedItem.Value.Trim())
                strBody += "Mailing Country: " + lstMailingCountry.SelectedItem.Value.Trim() + System.Environment.NewLine;
            if (affDT[0].Telephone.Trim() != txtPhone.Text.Trim())
                strBody += "Business Phone: " + txtPhone.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].HomePhone.Trim() != txtHomePhone.Text.Trim())
                strBody += "Home Phone: " + txtHomePhone.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].MobilePhone.Trim() != txtMobilePhone.Text.Trim())
                strBody += "Mobile Phone: " + txtMobilePhone.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].Fax.Trim() != txtFax.Text.Trim())
                strBody += "Fax: " + txtFax.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].WebSiteURL.Trim() != txtURL.Text.Trim())
                strBody += "Website URL: " + txtURL.Text.Trim() + System.Environment.NewLine;
            if (affDT[0].Comments.Trim() != txtComments.Text.Trim())
                strBody += "Comments: " + txtComments.Text.Trim() + System.Environment.NewLine;
            string strCheckPayable = "";
            if (rdbDBA.Checked)
                strCheckPayable = "DBA";
            else
                strCheckPayable = "LegalName";
            if (affDT[0].CheckPayable.Trim() != strCheckPayable)
                strBody += "Check Payable: " + strCheckPayable + System.Environment.NewLine;

            /*SSN or Federal TaxID cannot be changed by partner

            //initialize the Tax or SSN to denote to the database to NOT Update the field                    
            string TaxSSN = "-";
            string FederalTaxID = "-";
            string SocialSecurity = "-";

            //if Tax or SSN has been for editing
            if (txtTaxSSN.Enabled)
            {
                if (rdbTaxID.Checked)
                {
                    TaxSSN = "TaxID";
                    FederalTaxID = txtTaxSSN.Text.Trim();
                    //SocialSecurity = "";
                }
                else
                {
                    TaxSSN = "SSN";
                    SocialSecurity = txtTaxSSN.Text.Trim();
                    //FederalTaxID = "";
                }
            }

            if (rdbTaxID.Checked)
            {
                if (affDT[0].SocialSecurity.Trim() != "")
                {
                    strBody += "Social Security: xxxxxx" + System.Environment.NewLine;
                    //strMail += "Tax ID: " + MaskNumbers(FederalTaxID) + System.Environment.NewLine;
                }
            }
            if (rdbSSN.Checked)
            {
                if (affDT[0].TaxID.Trim() != "")
                {
                    strBody += "Tax ID: xxxxxx" + System.Environment.NewLine;
                    //strMail += "Social Security: " + MaskNumbers(SocialSecurity) + System.Environment.NewLine;
                }
            }
            */

            //Check if Direct Deposit information has been modified and add to email
            if ((Convert.ToBoolean(affDT[0].DirectDeposit)) != (rdbDDYes.Checked))
            {
                if (Convert.ToBoolean(affDT[0].DirectDeposit))
                    strBody += "Direct Deposit: No" + System.Environment.NewLine;
                else
                    strBody += "Direct Deposit: Yes" + System.Environment.NewLine;
            }

            string strNotify = "";
            if (affDT[0].SendEmailNotification)
                strNotify = "Yes";
            else
                strNotify = "No";

            if (lstNotify.SelectedItem.Text != strNotify)
                strBody += "Notify by Email: " + lstNotify.SelectedValue.ToString() + System.Environment.NewLine;
        }

        strBody = strBody + System.Environment.NewLine;
        strBody = strBody + System.Environment.NewLine;
        strBody = strBody + "Thank You, " + System.Environment.NewLine;
        strBody = strBody + "E-Commerce Exchange" + System.Environment.NewLine;

        return strBody;
    }//end function GetBody

    protected void chkMailingSame_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMailingSame.Checked)
        {
            txtMailingAddress.Text = txtAddress.Text;
            txtMailingCity.Text = txtCity.Text;
            lstMailingState.SelectedIndex = lstState.SelectedIndex;
            txtMailingZip.Text = txtZip.Text;
            lstMailingCountry.SelectedIndex = lstCountry.SelectedIndex;
            txtMailingRegion.Text = txtBusRegion.Text;
        }
        else
        {
            txtMailingAddress.Text = "";
            txtMailingCity.Text = "";
            lstMailingState.SelectedIndex = 0;
            txtMailingZip.Text = "";
            lstMailingCountry.SelectedIndex = 0;
            txtBusRegion.Text = "";
        }
    }//end if check changed

    protected void rdbDDYes_CheckedChanged(object sender, EventArgs e)
    {
        if (rdbDDNo.Checked)
            pnlBanking.Visible = false;
        else if (rdbDDYes.Checked)
        {
            pnlBanking.Visible = true;
            int AffiliateID = Convert.ToInt32(Session["AffiliateID"]);
            AffiliatesBL Affiliate = new AffiliatesBL(AffiliateID);
            PartnerDS.AffiliateBankingDataTable DTBanking = Affiliate.GetAffiliateBanking();
            if (DTBanking.Rows.Count > 0)
            {
                lblBankName.Text = Server.HtmlEncode(DTBanking[0].BankName.Trim());
                lblOtherBank.Text = Server.HtmlEncode(DTBanking[0].OtherBank.Trim());
                lblBankAddress.Text = Server.HtmlEncode(DTBanking[0].BankAddress.Trim());
                //lblBankAddress2.Text = DTBanking[0].Address2.Trim();
                lblZipCode.Text = Server.HtmlEncode(DTBanking[0].BankZip.Trim());
                lblBankPhone.Text = Server.HtmlEncode(DTBanking[0].BankPhone.Trim());
                lblBankState.Text = Server.HtmlEncode(DTBanking[0].BankState.Trim());
                lblBankCity.Text = Server.HtmlEncode(DTBanking[0].BankCity.Trim());
                lblNameOnChecking.Text = Server.HtmlEncode(DTBanking[0].NameonCheckingAcct.Trim());

                //string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(lblPassword.Text.Trim() + GetSalt(), "sha1");
                //This code displays only the last 4 digits of the account number            
                lblAcctNumber.Text = "xxxxxx" + DTBanking[0].BankAccountNumber.Trim();

                //This code displays only the last 4 digits of the routing number                        
                lblBankRoutingNumber.Text = "xxxxxx" + DTBanking[0].BankRoutingNumber.Trim();
            }//end if count not 0
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
