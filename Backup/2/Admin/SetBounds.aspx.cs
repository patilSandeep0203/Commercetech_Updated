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
using DLPartner.PartnerDSTableAdapters;

public partial class SetBounds : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    private static string CardPresent;
    private static string Processor;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;
       
        if ((Session.IsNewSession) || (Session == null))
            Response.Redirect("~/login.aspx");

        //This page is accessible only by Admins
        if (!User.IsInRole("Admin"))
        {
            //Response.Redirect("login.aspx?Authentication=False");
            DisplayMessage("You are not authorized to view this resource.");
            btnUpdate.Enabled = false;
        }

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                //Get Processor list
                BoundsBL ProcessorsList = new BoundsBL();
                DataSet dsProcessors = ProcessorsList.GetProcessorList();
                if (dsProcessors.Tables[0].Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i < dsProcessors.Tables[0].Rows.Count; i++)
                    {
                        dr = dsProcessors.Tables[0].Rows[i];
                        ListItem item = new ListItem();
                        item.Text = dr["Processor"].ToString().Trim() + "(" + dr["CardPresent"].ToString().Trim() + ")";
                        item.Value = dr["ProcessorID"].ToString();
                        lstProcessor.Items.Add(item);
                    }
                    ListItem ProcessorItem = new ListItem();
                    ProcessorItem.Text = "";
                    ProcessorItem.Value = "";
                    lstProcessor.Items.Add(ProcessorItem);

                    lstProcessor.SelectedValue = lstProcessor.Items.FindByValue("").Value;
                }

                //Get Gateway list
                BoundsBL Gateways = new BoundsBL();
                DataSet dsGateways = Gateways.GetGatewayList();
                if (dsGateways.Tables[0].Rows.Count > 0)
                {
                    lstGatewayNames.DataSource = dsGateways;
                    lstGatewayNames.DataTextField = "Gateway";
                    lstGatewayNames.DataValueField = "GatewayID";
                    lstGatewayNames.DataBind();
                }
                PopulateGatewayRates();

                //Get Check Service list
                BoundsBL CheckService = new BoundsBL();
                DataSet dsCheckService = CheckService.GetCheckServiceList();
                if (dsCheckService.Tables[0].Rows.Count > 0)
                {
                    lstCheckService.DataSource = dsCheckService;
                    lstCheckService.DataTextField = "CheckService";
                    lstCheckService.DataValueField = "CheckServiceID";
                    lstCheckService.DataBind();
                }
                PopulateCheckServiceRates();

                BoundsBL GiftCard = new BoundsBL();
                DataSet dsGiftCard = CheckService.GetGiftCardList();
                if (dsGiftCard.Tables[0].Rows.Count > 0)
                {
                    lstGiftCard.DataSource = dsGiftCard;
                    lstGiftCard.DataTextField = "GiftCard";
                    lstGiftCard.DataValueField = "GiftCardID";
                    lstGiftCard.DataBind();
                }

                PopulateGiftCardRates();

            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error getting Processor list");
            }
        }//end if not post back
    }//end page load

    //This function Populates Rates
    public void Populate()
    {
        //Get rates
        BoundsBL Bounds = new BoundsBL();
        PartnerDS.ProcessorBoundsDataTable dt = Bounds.GetProcessorBounds(Convert.ToInt32(lstProcessor.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            //Merchant Account Rates
            CardPresent = Server.HtmlEncode(dt[0].CardPresent.ToString().Trim());

            CardPresent = Server.HtmlEncode(dt[0].CardPresent.ToString().Trim());
            if ((CardPresent == "CNP") || (lstProcessor.SelectedItem.Value.ToLower().Contains("quickbooks")))
            {
                pnlAdditionalServices.Visible = false;
            }
            else
            {
                pnlAdditionalServices.Visible = true;
                txtDebitMonFee.Text = Server.HtmlEncode(dt[0].DebitMonFeeLow.ToString());
                txtDebitTransFee.Text = Server.HtmlEncode(dt[0].DebitTransFeeLow.ToString());
                txtEBTMonFee.Text = Server.HtmlEncode(dt[0].EBTMonFeeLow.ToString());
                txtEBTTransFee.Text = Server.HtmlEncode(dt[0].EBTTransFeeLow.ToString());
            }

            txtCustomerService.Text = Server.HtmlEncode(dt[0].CustServLow.ToString());
            txtInternetStmt.Text = Server.HtmlEncode(dt[0].InternetStmtLow.ToString());
            txtMonMin.Text = Server.HtmlEncode(dt[0].MonMinLow.ToString());
            txtTransFee.Text = Server.HtmlEncode(dt[0].TransFeeLow.ToString());
            txtWirelessAccess.Text = Server.HtmlEncode(dt[0].WirelessAccessFeeLow.ToString());
            txtWirelessTransFee.Text = Server.HtmlEncode(dt[0].WirelessTransFeeLow.ToString());
            txtDRQP.Text = Server.HtmlEncode(dt[0].DiscRateQualPresLow.ToString());
            txtDRQNP.Text = Server.HtmlEncode(dt[0].DiscRateQualNPLow.ToString());
            txtDRMQ.Text = Server.HtmlEncode(dt[0].DiscRateMidQualLow.ToString());
            txtDRNQ.Text = Server.HtmlEncode(dt[0].DiscRateNonQualLow.ToString());
            txtDRQD.Text = Server.HtmlEncode(dt[0].DiscRateQualDebitLow.ToString());
            txtChargebackFee.Text = Server.HtmlEncode(dt[0].ChargebackFeeLow.ToString());
            txtRetrievalFee.Text = Server.HtmlEncode(dt[0].RetrievalFeeLow.ToString());
            txtVoiceAuth.Text = Server.HtmlEncode(dt[0].VoiceAuthLow.ToString());
            txtBatchHeader.Text = Server.HtmlEncode(dt[0].BatchHeaderLow.ToString());
            txtAVS.Text = Server.HtmlEncode(dt[0].AVSLow.ToString());
            txtNBCTransFee.Text = Server.HtmlEncode(dt[0].NBCTransFeeLow.ToString());
            txtApplicationFee.Text = Server.HtmlEncode(dt[0].AppFeeLow.ToString());
            txtSetupFee.Text = Server.HtmlEncode(dt[0].AppSetupFeeLow.ToString());

            Processor = Server.HtmlEncode(dt[0].Processor.ToString().Trim());
            lblProcessor.Text = Server.HtmlEncode(dt[0].Processor.ToString().Trim()) + "(" + CardPresent + ")";
            lblLastModified.Text = Server.HtmlEncode(dt[0].LastModified.ToString().Trim());
            
            LoadAnnualFees();
            //if current Annual Fee loaded is not found in the database, add to dropdown
            if (lstAnnualFee.Items.FindByValue(dt[0].AnnualFeeLow.ToString()) == null)
                lstAnnualFee.Items.Add(dt[0].AnnualFeeLow.ToString());
            lstAnnualFee.SelectedValue = dt[0].AnnualFeeLow.ToString();
        }//end if count not 0
    }//end function Populate

    //This function loads annual fees list based on processor and CP/CNP
    public void LoadAnnualFees()
    {
        //Get annual fees
        OnlineAppProcessingBL AnnualFees = new OnlineAppProcessingBL();
        DataSet dsAnnualFee = AnnualFees.GetAnnualFee(Processor, CardPresent);
        if (dsAnnualFee.Tables[0].Rows.Count > 0)
        {
            lstAnnualFee.DataSource = dsAnnualFee;
            lstAnnualFee.DataTextField = "AnnualFee";
            lstAnnualFee.DataValueField = "AnnualFee";
            lstAnnualFee.DataBind();
        }
    }//end function AnnualFees

    //This function handles update button click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string strAnnualFee = "";
            if ( lstAnnualFee.Items.Count != 0 )
                strAnnualFee = lstAnnualFee.SelectedItem.Text.Trim();
            BoundsBL UpdateBounds = new BoundsBL();
            bool retVal = UpdateBounds.UpdateProcessorBounds(DateTime.Now.ToString(), txtCustomerService.Text.Trim(),
                txtInternetStmt.Text.Trim(), txtTransFee.Text.Trim(), txtDRQP.Text.Trim(), txtDRQNP.Text.Trim(), txtDRMQ.Text.Trim(),
                txtDRNQ.Text.Trim(), txtDRQD.Text.Trim(), txtChargebackFee.Text.Trim(), txtRetrievalFee.Text.Trim(),
                txtVoiceAuth.Text.Trim(), txtBatchHeader.Text.Trim(), txtAVS.Text.Trim(), txtMonMin.Text.Trim(),
                txtNBCTransFee.Text.Trim(), strAnnualFee , txtWirelessAccess.Text.Trim(),
                txtWirelessTransFee.Text.Trim(), txtApplicationFee.Text.Trim(), txtSetupFee.Text.Trim(),
                lstProcessor.SelectedItem.Value.ToString(), txtDebitMonFee.Text.Trim(), 
                txtDebitTransFee.Text.Trim(), txtEBTMonFee.Text.Trim(), txtEBTTransFee.Text.Trim());
            if (retVal)
            {
                DisplayMessage("Bounds Updated Successfully.");
                PartnerLogBL LogData = new PartnerLogBL();
                retVal = LogData.InsertPartnerLog(Convert.ToInt32(Session["AffiliateID"]), "Minimums Updated for Processor - " + lstProcessor.SelectedItem.Text.ToString().Trim() + ".");
            }
            else
                DisplayMessage("Error Updating Bounds.");        
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating Bounds");
        }
    }//end update button click
        
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    //This function populates rates based on processor selected
    protected void lstProcessor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstProcessor.SelectedItem.Value != "")
                Populate();
            else            
                ResetControls();            
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error retrieving rates");
        }
    }//end processor name changed

    
    //This function resets all controls on form
    public void ResetControls()
    {
        TextBox txtBox = new TextBox();
        for (int i = 0; i < pnlMerchantRates.Controls.Count; i++)
        {
            if (pnlMerchantRates.Controls[i].GetType() == txtBox.GetType())
            {
                txtBox = (TextBox)pnlMerchantRates.Controls[i];
                txtBox.Text = "";
            }
        }//end for
        lstProcessor.SelectedValue = lstProcessor.Items.FindByValue("").Value;
    }//end function reset controls

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    //This function Populates Rates
    public void PopulateCheckServiceRates()
    {
        //Get rates
        BoundsBL Rates = new BoundsBL();
        PartnerDS.CheckServiceBoundsDataTable dt = Rates.CheckServiceBounds(Convert.ToInt32(lstCheckService.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            //Gateway Rates
            txtCheckServiceDiscRate.Text = Server.HtmlEncode(dt[0].CGDiscRateLow.ToString());
            txtCheckServiceMonFee.Text = Server.HtmlEncode(dt[0].CGMonFeeLow.ToString());
            txtCheckServiceMonMin.Text = Server.HtmlEncode(dt[0].CGMonMinLow.ToString());
            txtCheckServiceTransFee.Text = Server.HtmlEncode(dt[0].CGTransFeeLow.ToString());
        }//end if count not 0
    }//end function Populate

    public void PopulateGatewayRates()
    {
        //Get rates
        BoundsBL Rates = new BoundsBL();
        PartnerDS.GatewayBoundsDataTable dt = Rates.GetGatewayBounds(Convert.ToInt32(lstGatewayNames.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            //Gateway Rates
            txtGWSetupFee.Text = Server.HtmlEncode(dt[0].GatewaySetupFeeLow.ToString());
            txtGWMonthlyFee.Text = Server.HtmlEncode(dt[0].GatewayMonFeeLow.ToString());
            txtGWTransFee.Text = Server.HtmlEncode(dt[0].GatewayTransFeeLow.ToString());
        }//end if count not 0
    }//end function Populate

    public void PopulateGiftCardRates()
    {
        //Get rates
        BoundsBL Rates = new BoundsBL();
        DataSet ds = Rates.GiftCardBoundsbyID(Convert.ToInt32(lstGiftCard.SelectedItem.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            //Gift Card Rates
            txtGifrCardMonFee.Text = dr["GiftCardMonFeeLow"].ToString().Trim();
            txtGifrCardTransFee.Text = dr["GiftCardTransFeeLow"].ToString().Trim();
        }//end if count not 0
    }//end function Populate

    protected void btnResetGateway_Click(object sender, EventArgs e)
    {
        ResetControlsGateway();
    }

    //This function resets all controls on form
    public void ResetControlsGateway()
    {
        TextBox txtBox = new TextBox();
        for (int i = 0; i < pnlGatewayRates.Controls.Count; i++)
        {
            if (pnlGatewayRates.Controls[i].GetType() == txtBox.GetType())
            {
                txtBox = (TextBox)pnlGatewayRates.Controls[i];
                txtBox.Text = "";
            }
        }//end for
        lstGatewayNames.SelectedValue = lstGatewayNames.Items.FindByValue("None").Value;
    }

    protected void btnResetCheckService_Click(object sender, EventArgs e)
    {
        ResetControlsCheckService();
    }

    //This function resets all controls on form
    public void ResetControlsCheckService()
    {
        TextBox txtBox = new TextBox();
        for (int i = 0; i < pnlCheckService.Controls.Count; i++)
        {
            if (pnlCheckService.Controls[i].GetType() == txtBox.GetType())
            {
                txtBox = (TextBox)pnlCheckService.Controls[i];
                txtBox.Text = "";
            }
        }//end for
        lstCheckService.SelectedValue = lstCheckService.Items.FindByValue("None").Value;
    }

    public void ResetControlsGiftCard()
    {
        TextBox txtBox = new TextBox();
        for (int i = 0; i < pnlGiftCard.Controls.Count; i++)
        {
            if (pnlGiftCard.Controls[i].GetType() == txtBox.GetType())
            {
                txtBox = (TextBox)pnlGiftCard.Controls[i];
                txtBox.Text = "";
            }
        }//end for
        lstGiftCard.SelectedValue = lstGiftCard.Items.FindByValue("None").Value;
    }

    protected void lstGatewayNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstGatewayNames.SelectedItem.Value != "")
                PopulateGatewayRates();
            else
                ResetControlsGateway();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageGW("Error retrieving rates");
        }
    }

    protected void lstCheckService_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstCheckService.SelectedItem.Value != "")
                PopulateCheckServiceRates();
            else
                ResetControlsCheckService();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageGW("Error retrieving rates");
        }
    }

    protected void lstGiftCard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstCheckService.SelectedItem.Value != "")
                PopulateGiftCardRates();
            else
                ResetControlsGiftCard();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageGiftCard("Error retrieving rates");
        }
    }

    protected void btnUpdateGateway_Click(object sender, EventArgs e)
    {
        try
        {
            BoundsBL SetGWBounds = new BoundsBL();
            SetGWBounds.UpdateGatewayBounds(txtGWSetupFee.Text.Trim(), txtGWMonthlyFee.Text.Trim(),
                txtGWTransFee.Text.Trim(), lstGatewayNames.SelectedItem.Value);
            SetErrorMessageGW("Gateway Bounds set successfully");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageGW("Error Updateing rates");
        }
    }

    protected void btnUpdateCheckService_Click(object sender, EventArgs e)
    {
        try
        {
            BoundsBL SetGWBounds = new BoundsBL();
            SetGWBounds.UpdateCheckServiceBounds(txtCheckServiceDiscRate.Text.Trim(), txtCheckServiceMonFee.Text.Trim(),
                txtCheckServiceMonMin.Text.Trim(), txtCheckServiceTransFee.Text.Trim(), lstCheckService.SelectedItem.Value);
            SetErrorMessageCS("Check Service Bounds set successfully");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageCS("Error Updateing rates");
        }
    }

    protected void btnUpdateGiftCard_Click(object sender, EventArgs e)
    {
        try
        {
            BoundsBL SetGiftCardBounds = new BoundsBL();
            SetGiftCardBounds.UpdateGiftCardBounds(txtGifrCardMonFee.Text.Trim(),
                txtGifrCardTransFee.Text.Trim(), lstGiftCard.SelectedItem.Value);
            SetErrorMessageGiftCard("Gift Royalty defaults set successfully");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageGiftCard("Error Updateing rates");
        }
    }

    //This function displays error message on a label
    protected void SetErrorMessageGW(string errText)
    {
        lblErrorGateway.Visible = true;
        lblErrorGateway.Text = errText;
    }//end function set error message

    protected void SetErrorMessageCS(string errText)
    {
        lblErrorCheckService.Visible = true;
        lblErrorCheckService.Text = errText;
    }//end function set error message

    protected void SetErrorMessageGiftCard(string errText)
    {
        lblErrorGiftCard.Visible = true;
        lblErrorGiftCard.Text = errText;
    }//end function set error message
}
