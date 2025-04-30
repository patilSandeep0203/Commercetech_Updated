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
using OnlineAppClassLibrary;
using DLPartner;
using BusinessLayer;

public partial class UpdatePartnerInfo : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    private static int PartnerID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if ((Session.IsNewSession) || (Session == null))
            Response.Redirect("../login.aspx");

        if (!User.IsInRole("Admin"))
        {
            Response.Redirect("../login.aspx");
        }

        if ((Request.Params.Get("PartnerID") != null))
            PartnerID = Convert.ToInt32(Request.Params.Get("PartnerID"));

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("../login.aspx?Authentication=False");
            if (Convert.ToString(PartnerID) == "")
                Response.Redirect("../login.aspx");

            try
            {
                Populate();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error retrieving Partner Infomation");
            }
        }//end if not post back
    }

    //This function populates partner information
    public void Populate()
    {
        try
        {
            //Get Referral
            /*ReferralsBL Referrals = new ReferralsBL();
            DataSet dsReferrals = Referrals.GetReferralList();
            if (dsReferrals.Tables[0].Rows.Count > 0)
            {
                lstReferral.DataSource = dsReferrals.Tables[0];
                lstReferral.DataTextField = "CompanyName";
                lstReferral.DataValueField = "AffiliateID";
                lstReferral.DataBind();
            }//end if count not 0

            System.Web.UI.WebControls.ListItem ReferralItem = new System.Web.UI.WebControls.ListItem();
            ReferralItem.Text = "Other";
            ReferralItem.Value = "0";
            lstReferral.Items.Add(ReferralItem);*/

            AffiliatesBL Aff = new AffiliatesBL(PartnerID);
            DataSet dsAffSalesOff = Aff.GetAffiliateSalesOffice();

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

            AffiliatesBL Affiliates = new AffiliatesBL(11);
            DataSet dsAffiliates = Affiliates.GetAffiliateList();
            DataSet dsSalesOffice = Affiliates.GetSalesOfficeList();
            if (dsAffiliates.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i < dsAffiliates.Tables[0].Rows.Count; i++)
                {
                    dr = dsAffiliates.Tables[0].Rows[i];
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    item.Text = dr["DBA"].ToString().Trim() + " - " + dr["CompanyName"].ToString().Trim() + " - (" + dr["AffiliateID"].ToString().Trim() + ")";
                    item.Value = dr["AffiliateID"].ToString().Trim();
                    lstReferral.Items.Add(item);
                }
                System.Web.UI.WebControls.ListItem ReferralItem = new System.Web.UI.WebControls.ListItem();
                ReferralItem.Text = "OTHER";
                ReferralItem.Value = "0";
                lstReferral.Items.Add(ReferralItem);


            }

            /*if (dsAffSalesOff.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i < dsAffSalesOff.Tables[0].Rows.Count; i++)
                {
                    dr = dsAffSalesOff.Tables[0].Rows[i];

                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    item.Text = dr["OfficeDBA"].ToString().Trim() + " - " + dr["OfficeCompanyName"].ToString().Trim() + " - (" + dr["SalesOfficeID"].ToString().Trim() + ")";
                    item.Value = dr["SalesOfficeID"].ToString().Trim();
                    lstOffice.Items.Add(item);
                }

            }*/


            if (dsSalesOffice.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i < dsSalesOffice.Tables[0].Rows.Count; i++)
                {
                    dr = dsSalesOffice.Tables[0].Rows[i];
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    item.Text = dr["DBA"].ToString().Trim() + " - " + dr["CompanyName"].ToString().Trim() + " - (" + dr["AffiliateID"].ToString().Trim() + ")";
                    item.Value = dr["AffiliateID"].ToString().Trim();
                    lstOffice.Items.Add(item);
                }
                System.Web.UI.WebControls.ListItem OfficeItem = new System.Web.UI.WebControls.ListItem();
                OfficeItem.Text = "";
                OfficeItem.Value = "";
                lstOffice.Items.Add(OfficeItem);
            }

            

            //end if count not 0

            //Populate other referral list
            ACTDataBL OtherRefList = new ACTDataBL();
            DataSet ds = OtherRefList.GetOtherReferralList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lstOtherReferral.DataSource = ds;
                lstOtherReferral.DataTextField = "DBA";
                lstOtherReferral.DataValueField = "DBA";
                lstOtherReferral.DataBind();
            }

            System.Web.UI.WebControls.ListItem lstItem = new System.Web.UI.WebControls.ListItem();
            lstItem.Text = "";
            lstItem.Value = "";
            lstOtherReferral.Items.Add(lstItem);

            lblPartnerID.Text = PartnerID.ToString().Trim();

            //Get Partner Info
            
            PartnerDS.AffiliatesDataTable affDT = Aff.GetAffiliate();
            
            if (affDT.Rows.Count > 0)
            {
                txtFirstName.Text = affDT[0].FirstName.ToString().Trim();
                txtLastName.Text = affDT[0].LastName.ToString().Trim();
                if (!Convert.IsDBNull(affDT[0].CompanyName))
                {
                    if (Convert.ToString(affDT[0].CompanyName) != "")
                    {
                        txtCompanyName.Text = affDT[0].CompanyName.ToString().Trim();
                        txtCompanyName.Enabled = true;
                    }
                }
                txtDBA.Text = affDT[0].DBA.ToString().Trim();
                lstLegalStatus.SelectedValue = lstLegalStatus.Items.FindByValue(affDT[0].LegalStatus.ToString().Trim()).Value;

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

                txtTaxSSN.Enabled = false;
                rdbSSN.Enabled = false;
                rdbTaxID.Enabled = false;

                txtAddress.Text = affDT[0].CompanyAddress.ToString().Trim();
                txtCity.Text = affDT[0].City.ToString().Trim();
                lstState.SelectedValue = lstState.Items.FindByValue(affDT[0].State.ToString()).Value;
                txtZip.Text = affDT[0].Zip.ToString().Trim(); 
                lstCountry.SelectedValue = lstCountry.Items.FindByValue(affDT[0].Country.ToString().Trim()).Value;
                txtMailingAddress.Text = affDT[0].MailingAddress.ToString().Trim();
                txtMailingCity.Text = affDT[0].MailingCity.ToString().Trim();
                lstMailingState.SelectedValue = lstMailingState.Items.FindByValue(affDT[0].MailingState.ToString()).Value;
                txtMailingZip.Text = affDT[0].MailingZip.ToString().Trim();
                lstMailingCountry.SelectedValue = lstMailingCountry.Items.FindByValue(affDT[0].MailingCountry.ToString().Trim()).Value;

                txtPhone.Text = affDT[0].Telephone.Trim();
                txtHomePhone.Text = affDT[0].HomePhone.Trim();
                txtMobilePhone.Text = affDT[0].MobilePhone.Trim();
                txtFax.Text = affDT[0].Fax.Trim();
                txtURL.Text = affDT[0].WebSiteURL.Trim();
                txtEmail.Text = affDT[0].Email.ToString().Trim();

                lstReferral.SelectedValue = lstReferral.Items.FindByValue(affDT[0].ReferralID.ToString().Trim()).Value;
                if (lstReferral.SelectedValue.ToString().Trim() == "0")
                {
                    lstOtherReferral.Enabled = true;
                    lstOtherReferral.SelectedValue = lstOtherReferral.Items.FindByValue(affDT[0].OtherReferral.ToString().Trim()).Value;
                    //txtSpecify.Enabled = true;
                    //txtSpecify.Text = affDT[0].OtherReferral.ToString().Trim();
                }
                else
                    lstOtherReferral.Text = "";

            }

            if (dsAffSalesOff.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i < dsAffSalesOff.Tables[0].Rows.Count; i++)
                {
                    dr = dsAffSalesOff.Tables[0].Rows[i];
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    //item.Text = dr["DBA"].ToString().Trim() + " - " + dr["CompanyName"].ToString().Trim() + " - (" + dr["AffiliateID"].ToString().Trim() + ")";
                    //item.Value = dr["AffiliateID"].ToString().Trim();
                    lstOffice.SelectedValue = lstOffice.Items.FindByValue(dr["SalesOfficeID"].ToString().Trim()).Value;
                    //lstOffice.Value = dr["AffiliateID"].ToString().Trim();
                }
                /*
                System.Web.UI.WebControls.ListItem OfficeItem = new System.Web.UI.WebControls.ListItem();
                OfficeItem.Text = "";
                OfficeItem.Value = "";
                lstOffice.Items.Add(OfficeItem);*/
            }
            else {
                lstOffice.SelectedValue = "";
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Partner Information");
        }
    }//end function Populate

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> { self.close() }</script>");
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        rdbSSN.Enabled = true;
        rdbTaxID.Enabled = true;
        txtTaxSSN.Enabled = true;
        txtTaxSSN.Text = "";
        lnkEdit.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                AffiliatesBL Partner = new AffiliatesBL(PartnerID);
                PartnerDS.AffiliatesDataTable affDT = Partner.GetAffiliate();

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

                    bool retVal = Partner.UpdateAffiliateLead(txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtDBA.Text.Trim(),
                                    txtCompanyName.Text.Trim(), lstLegalStatus.Text.Trim(), TaxSSN, FederalTaxID, SocialSecurity, 
                                    txtAddress.Text.Trim(), txtCity.Text.Trim(), lstState.Text.Trim(), txtZip.Text.Trim(), 
                                    lstCountry.Text.Trim(), txtMailingAddress.Text.Trim(), txtMailingCity.Text.Trim(), 
                                    lstMailingState.Text.Trim(), txtMailingZip.Text.Trim(), lstMailingCountry.Text.Trim(), 
                                    txtPhone.Text.Trim(), txtHomePhone.Text.Trim(), txtMobilePhone.Text.Trim(), txtFax.Text.Trim(), 
                                    txtURL.Text.Trim(), txtEmail.Text.Trim(), Convert.ToInt32(lstReferral.SelectedItem.Value),
                                    lstOtherReferral.Text);
                    //if (Conver.ToString(lstOffice.SelectedItem.Value) != "")
                    //{
                        bool retOfficeVal = Partner.updateAffiliateOffice(Convert.ToString(lstOffice.SelectedItem.Value));
                    //}
                    //else
                    //{
                        //bool retOfficeVal = Partner.updateAffiliateOffice(Convert.ToInt32(lstOffice.SelectedItem.Value));
                    //}

                    if (retVal)
                    {
                        DisplayMessage("Partner Updated Successfully");
                        PartnerLogBL LogData = new PartnerLogBL();
                        retVal = LogData.InsertPartnerLog(Convert.ToInt32(Session["AffiliateID"]), "Partner Info Updated for Partner - " + txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim() + ".");
                    }
                }
            }//end if page is valid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating Partner Information");
        }
    }//end submit button click

    //This function handles Referral change event and allows user to enter text in Specify field
    protected void lstReferral_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstReferral.SelectedItem.ToString().Trim() == "Other")
            {
                lstOtherReferral.Enabled = true;
                //txtSpecify.Enabled = true;
            }
            else
            {
                lstOtherReferral.Enabled = false;
                lstOtherReferral.Text = "";
                //txtSpecify.Text = ""; 
                //txtSpecify.Enabled = false;
            }

        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error retrieving Rep Info Information");
        }
    }//end referral changed event

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void chkMailingSame_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMailingSame.Checked)
        {
            txtMailingAddress.Text = txtAddress.Text;
            txtMailingCity.Text = txtCity.Text;
            lstMailingState.SelectedIndex = lstState.SelectedIndex;
            txtMailingZip.Text = txtZip.Text;
            lstMailingCountry.SelectedIndex = lstCountry.SelectedIndex;
        }
        else
        {
            txtMailingAddress.Text = "";
            txtMailingCity.Text = "";
            lstMailingState.SelectedIndex = 0;
            txtMailingZip.Text = "";
            lstMailingCountry.SelectedIndex = 0;
        }
    }//end if chkMailingSame changed
}
