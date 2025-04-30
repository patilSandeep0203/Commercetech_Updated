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
using DLPartner.PartnerDSTableAdapters;
using DLPartner;

public partial class SetDefaults : System.Web.UI.Page
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

                    lstProcessor.SelectedIndex = lstProcessor.Items.IndexOf(ProcessorItem);
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
        BoundsBL Bounds= new BoundsBL();
        string strComplianceFee = "";
        PartnerDS.ProcessorBoundsDataTable dt = Bounds.GetProcessorBounds(Convert.ToInt32(lstProcessor.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            strComplianceFee = Bounds.GetComplianceFee(Convert.ToInt32(lstProcessor.SelectedItem.Value));
            //Merchant Account Rates
            CardPresent = Server.HtmlEncode(dt[0].CardPresent.ToString().Trim());
            if ((CardPresent == "CNP") || (lstProcessor.SelectedItem.Value.ToLower().Contains("quickbooks")))
                pnlAdditionalServices.Visible = false;                
            else
            {
                pnlAdditionalServices.Visible = true;
                txtDebitMonFee.Text = Server.HtmlEncode(dt[0].DebitMonFeeDef.ToString());
                txtDebitTransFee.Text = Server.HtmlEncode(dt[0].DebitTransFeeDef.ToString());
                txtEBTMonFee.Text = Server.HtmlEncode(dt[0].EBTMonFeeDef.ToString());
                txtEBTTransFee.Text = Server.HtmlEncode(dt[0].EBTTransFeeDef.ToString());
            }
            Processor = Server.HtmlEncode(dt[0].Processor.ToString().Trim());
            lblProcessor.Text = Server.HtmlEncode(dt[0].Processor.ToString().Trim()) + "(" + CardPresent + ")";
            lblLastModified.Text = Server.HtmlEncode(dt[0].LastModified.ToString().Trim());

            txtDRQP.Text = Server.HtmlEncode(dt[0].DiscRateQualPresDef.ToString());
            txtDRQNP.Text = Server.HtmlEncode(dt[0].DiscRateQualNPDef.ToString());
            txtDRMQ.Text = Server.HtmlEncode(dt[0].DiscRateMidQualDef.ToString());
            txtDRNQ.Text = Server.HtmlEncode(dt[0].DiscRateNonQualDef.ToString());
            txtDRQD.Text = Server.HtmlEncode(dt[0].DiscRateQualDebitDef.ToString());
            txtCustomerService.Text = Server.HtmlEncode(dt[0].CustServDef.ToString());
            txtInternetStmt.Text = Server.HtmlEncode(dt[0].InternetStmtDef.ToString());
            txtMonMin.Text = Server.HtmlEncode(dt[0].MonMinDef.ToString());
            txtTransFee.Text = Server.HtmlEncode(dt[0].TransFeeDef.ToString());
            txtWirelessAccess.Text = Server.HtmlEncode(dt[0].WirelessAccessFeeDef.ToString());
            txtWirelessTransFee.Text = Server.HtmlEncode(dt[0].WirelessTransFeeDef.ToString());
            txtChargebackFee.Text = Server.HtmlEncode(dt[0].ChargebackFeeDef.ToString());
            txtRetrievalFee.Text = Server.HtmlEncode(dt[0].RetrievalFeeDef.ToString());
            txtVoiceAuth.Text = Server.HtmlEncode(dt[0].VoiceAuthDef.ToString());
            txtBatchHeader.Text = Server.HtmlEncode(dt[0].BatchHeaderDef.ToString());
            txtAVS.Text = Server.HtmlEncode(dt[0].AVSDef.ToString());
            txtNBCTransFee.Text = Server.HtmlEncode(dt[0].NBCTransFeeDef.ToString());
            txtApplicationFee.Text = Server.HtmlEncode(dt[0].AppFeeDef.ToString());
            txtComplianceFee.Text = Server.HtmlEncode(strComplianceFee);
            LoadAnnualFees();
            if (lstAnnualFee.Items.FindByValue(dt[0].AnnualFeeDef.ToString()) != null)
                lstAnnualFee.SelectedValue = dt[0].AnnualFeeDef.ToString();

            DiscRateQualPresLow.Value = Server.HtmlEncode(dt[0].DiscRateQualPresLow.ToString());
            DiscRateQualNPLow.Value = Server.HtmlEncode(dt[0].DiscRateQualNPLow.ToString());
            DiscRateMidQualLow.Value = Server.HtmlEncode(dt[0].DiscRateMidQualLow.ToString());
            DiscRateNonQualLow.Value = Server.HtmlEncode(dt[0].DiscRateNonQualLow.ToString());
            DiscRateQualDebitLow.Value = Server.HtmlEncode(dt[0].DiscRateQualDebitLow.ToString());
            
            CustServLow.Value = Server.HtmlEncode(dt[0].CustServLow.ToString());
            MonMinLow.Value = Server.HtmlEncode(dt[0].MonMinLow.ToString());
            TransFeeLow.Value = Server.HtmlEncode(dt[0].TransFeeLow.ToString());
            WirelessAccessFeeLow.Value = Server.HtmlEncode(dt[0].WirelessAccessFeeLow.ToString());
            WirelessTransFeeLow.Value = Server.HtmlEncode(dt[0].WirelessTransFeeLow.ToString());
            ChargebackFeeLow.Value = Server.HtmlEncode(dt[0].ChargebackFeeLow.ToString());
            RetrievalFeeLow.Value = Server.HtmlEncode(dt[0].RetrievalFeeLow.ToString());
            VoiceAuthLow.Value = Server.HtmlEncode(dt[0].VoiceAuthLow.ToString());
            BatchHeaderLow.Value = Server.HtmlEncode(dt[0].BatchHeaderLow.ToString());
            AVSLow.Value = Server.HtmlEncode(dt[0].AVSLow.ToString());
            NBCTransFeeLow.Value = Server.HtmlEncode(dt[0].NBCTransFeeLow.ToString());
            AnnualFeeLow.Value = Server.HtmlEncode(dt[0].AnnualFeeLow.ToString());
            AppFeeLow.Value = Server.HtmlEncode(dt[0].AppFeeLow.ToString());
            //SetupFeeLow.Value = Server.HtmlEncode(dt[0].AppSetupFeeDef.ToString());

          
    
        }//end if count not 0
    }//end function Populate

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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string strErr = ValidateLowerBounds();
            if (strErr == "")
            {
                string strAnnualFee = "";
                if (lstAnnualFee.Items.Count != 0)
                    strAnnualFee = lstAnnualFee.SelectedItem.Text.Trim();
                BoundsBL UpdateDefaults = new BoundsBL();
                bool retVal = UpdateDefaults.UpdateProcessorDefaults(DateTime.Now.ToString(), txtCustomerService.Text.Trim(),
                    txtInternetStmt.Text.Trim(), txtTransFee.Text.Trim(), txtDRQP.Text.Trim(), txtDRQNP.Text.Trim(), txtDRMQ.Text.Trim(),
                    txtDRNQ.Text.Trim(), txtDRQD.Text.Trim(), txtChargebackFee.Text.Trim(), txtRetrievalFee.Text.Trim(),
                    txtVoiceAuth.Text.Trim(), txtBatchHeader.Text.Trim(), txtAVS.Text.Trim(), txtMonMin.Text.Trim(),
                    txtNBCTransFee.Text.Trim(), strAnnualFee, txtWirelessAccess.Text.Trim(),
                    txtWirelessTransFee.Text.Trim(), txtApplicationFee.Text.Trim(), txtSetupFee.Text.Trim(),
                    lstProcessor.SelectedItem.Value.ToString(), txtDebitMonFee.Text.Trim(),
                    txtDebitTransFee.Text.Trim(), txtEBTMonFee.Text.Trim(), txtEBTTransFee.Text.Trim(), txtComplianceFee.Text.Trim());

                if (retVal)
                {
                    DisplayMessage("Defaults Updated Successfully.");
                    PartnerLogBL LogData = new PartnerLogBL();
                    retVal = LogData.InsertPartnerLog(Convert.ToInt32(Session["AffiliateID"]), "Defaults Updated for Processor - " + lstProcessor.SelectedItem.Text.ToString().Trim() + ".");
                }
                else
                    DisplayMessage("Error Updating Defaults.");
            }//end if rates validated
            else
                DisplayMessage(strErr);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating defaults");
        }
    }
       
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

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
    }

    //This function validates lower bounds before setting fees
    public string ValidateLowerBounds()
    {
        string strError = "";
        //Customer Service
        if (CustServLow.Value != "")
        {
            if ((txtCustomerService.Text == "") || (Convert.ToDecimal(txtCustomerService.Text) < Convert.ToDecimal(CustServLow.Value)))
                strError += "Enter a value greater than or equal to $" + CustServLow.Value + " for Customer Service Fee." + System.Environment.NewLine;
        }
        //Monthly Minimum
        if (MonMinLow.Value != "")
        {
            if ((txtMonMin.Text == "") || (Convert.ToDecimal(txtMonMin.Text) < Convert.ToDecimal(MonMinLow.Value)))
                strError += "Enter a value greater than or equal to $" + MonMinLow.Value + " for Monthly Minimum." + System.Environment.NewLine;
        }
        //Transaction Fee
        /*if (TransFeeLow.Value != "")
        {
            if ((txtTransFee.Text == "") || (Convert.ToDecimal(txtTransFee.Text) < Convert.ToDecimal(TransFeeLow.Value)))
            {
                strError += "Enter a value greater than or equal to $" + TransFeeLow.Value + " for Transaction Fee." + System.Environment.NewLine;
            }
        }*/
        //DiscRateQualPres
        if (DiscRateQualPresLow.Value != "")
        {
            if ((txtDRQP.Text == "") || (Convert.ToDecimal(txtDRQP.Text) < Convert.ToDecimal(DiscRateQualPresLow.Value)))            
                strError += "Enter a value greater than or equal to " + DiscRateQualPresLow.Value + "% for Disc Rate Qual Pres." + System.Environment.NewLine;            
        }
        //DiscRateQualNP
        if (DiscRateQualNPLow.Value != "")
        {
            if ((txtDRQNP.Text == "") || (Convert.ToDecimal(txtDRQNP.Text) < Convert.ToDecimal(DiscRateQualNPLow.Value)))       
                strError += "Enter a value greater than or equal to " + DiscRateQualNPLow.Value + "% for Disc Rate Qual NP." + System.Environment.NewLine;
        }

        //If IMS and Card Present overwrite the Mid Qual Low to be the Qual Pres PLUS the specified Mid Qual Step in the database 
        if ((CardPresent == "CP") && (lstProcessor.SelectedItem.Text == "Intuit payment Solutions"))
        {
            DiscRateMidQualLow.Value = Convert.ToString(Convert.ToDecimal(txtDRQP.Text) + Convert.ToDecimal(DiscRateMidQualStep.Value));
        }
        //DiscRateMidQual
        if (DiscRateMidQualLow.Value != "")
        {
            if ((txtDRMQ.Text == "") || (Convert.ToDecimal(txtDRMQ.Text) < Convert.ToDecimal(DiscRateMidQualLow.Value)))
                strError += "Enter a value greater than or equal to " + DiscRateMidQualLow.Value + "% for Disc Rate Mid Qual." + System.Environment.NewLine;
        }
        //DiscRateNonQual
        if (DiscRateNonQualLow.Value != "")
        {
            if ((txtDRNQ.Text == "") || (Convert.ToDecimal(txtDRNQ.Text) < Convert.ToDecimal(DiscRateNonQualLow.Value)))
                strError += "Enter a value greater than or equal to " + DiscRateNonQualLow.Value + "% for Disc Rate Non Qual." + System.Environment.NewLine;
        }
        //DiscRateQualDebit        
        if (DiscRateQualDebitLow.Value != "")
        {
            if ((txtDRQD.Text == "") || (Convert.ToDecimal(txtDRQD.Text) < Convert.ToDecimal(DiscRateQualDebitLow.Value)))
                strError += "Enter a value greater than or equal to " + DiscRateQualDebitLow.Value + "% for Disc Rate Qual Debit." + System.Environment.NewLine;
        }
        //ChargebackFee
        if (ChargebackFeeLow.Value != "")
        {
            if ((txtChargebackFee.Text == "") || (Convert.ToDecimal(txtChargebackFee.Text) < Convert.ToDecimal(ChargebackFeeLow.Value)))
                strError += "Enter a value greater than or equal to $" + ChargebackFeeLow.Value + " for Chargeback Fee." + System.Environment.NewLine;
        }
        //RetrievalFee
        if (RetrievalFeeLow.Value != "")
        {
            if ((txtRetrievalFee.Text == "") || (Convert.ToDecimal(txtRetrievalFee.Text) < Convert.ToDecimal(RetrievalFeeLow.Value)))
                strError += "Enter a value greater than or equal to $" + RetrievalFeeLow.Value + " for Retrieval Fee." + System.Environment.NewLine;
        }
        //VoiceAuth
        if (VoiceAuthLow.Value != "")
        {
            if ((txtVoiceAuth.Text == "") || (Convert.ToDecimal(txtVoiceAuth.Text) < Convert.ToDecimal(VoiceAuthLow.Value)))
                strError += "Enter a value greater than or equal to $" + VoiceAuthLow.Value + " for Voice Authorization Fee." + System.Environment.NewLine;
        }
        //BatchHeader
        if (BatchHeaderLow.Value != "")
        {
            if ((txtBatchHeader.Text == "") || (Convert.ToDecimal(txtBatchHeader.Text) < Convert.ToDecimal(BatchHeaderLow.Value)))
                strError += "Enter a value greater than or equal to $" + BatchHeaderLow.Value + " for Batch Header Fee." + System.Environment.NewLine;
        }
        //AVS
        if (AVSLow.Value != "")
        {
            if ((txtAVS.Text == "") || (Convert.ToDecimal(txtAVS.Text) < Convert.ToDecimal(AVSLow.Value)))
            {
                strError += "Enter a value greater than or equal to $" + AVSLow.Value + " for AVS Fee." + System.Environment.NewLine;
            }
        }
        //WirelessAccessFee
        if ((WirelessAccessFeeLow.Value != "") && (txtWirelessAccess.Text != ""))
        {
            if ((Convert.ToDecimal(txtWirelessAccess.Text) < Convert.ToDecimal(WirelessAccessFeeLow.Value)))
                strError += "Enter a value greater than or equal to $" + WirelessAccessFeeLow.Value + " for Wireless Access Fee or leave blank if it does not apply." + System.Environment.NewLine;
      
        }
        //WirelessTransFee
        if ((WirelessTransFeeLow.Value != "") && (txtWirelessTransFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtWirelessTransFee.Text) < Convert.ToDecimal(WirelessTransFeeLow.Value)))
                strError += "Enter a value greater than or equal to $" + WirelessTransFeeLow.Value + " for Wireless Trans Fee or leave blank if it does not apply." + System.Environment.NewLine;
        }
        //NBCTransFee
        /*if (NBCTransFeeLow.Value != "")
        {
            if ((txtNBCTransFee.Text == "") || (Convert.ToDecimal(txtNBCTransFee.Text) < Convert.ToDecimal(NBCTransFeeLow.Value)))
            {
                strError += "Enter a value greater than or equal to $" + NBCTransFeeLow.Value + " for Non-Bankcard Trans Fee." + System.Environment.NewLine;
            }
        }*/
        //AnnualFee
        /*if (AnnualFeeLow.Value != "")
        {
            if ((txtAnnualFee.Text == "") || (Convert.ToDecimal(txtAnnualFee.Text) < Convert.ToDecimal(AnnualFeeLow.Value)))
            {
                strError += "Enter a value greater than or equal to $" + AnnualFeeLow.Value + " for Annual Fee." + System.Environment.NewLine;
                strError += "Annual Fee Currently " + txtAnnualFee.Text;
            }
        }*/
        return strError;
    }//end function ValidateRates

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
        lstProcessor.SelectedValue = lstProcessor.Items.FindByText("").Value;
    }

    public void PopulateGatewayRates()
    {
        //Get rates
        BoundsBL Rates = new BoundsBL();
        PartnerDS.GatewayBoundsDataTable dt = Rates.GetGatewayBounds(Convert.ToInt32(lstGatewayNames.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            //Gateway Rates
            txtGWSetupFee.Text = Server.HtmlEncode(dt[0].GatewaySetupFeeDef.ToString());
            txtGWMonthlyFee.Text = Server.HtmlEncode(dt[0].GatewayMonFeeDef.ToString());
            txtGWTransFee.Text = Server.HtmlEncode(dt[0].GatewayTransFeeDef.ToString());
        }//end if count not 0
    }//end function Populate

    public void PopulateCheckServiceRates()
    {
        //Get rates
        BoundsBL Rates = new BoundsBL();
        PartnerDS.CheckServiceBoundsDataTable dt = Rates.CheckServiceBounds(Convert.ToInt32(lstCheckService.SelectedItem.Value));
        if (dt.Rows.Count > 0)
        {
            //Gateway Rates
            txtCheckServiceDiscRate.Text = Server.HtmlEncode(dt[0].CGDiscRateDef.ToString());
            txtCheckServiceMonFee.Text = Server.HtmlEncode(dt[0].CGMonFeeDef.ToString());
            txtCheckServiceMonMin.Text = Server.HtmlEncode(dt[0].CGMonMinDef.ToString());
            txtCheckServiceTransFee.Text = Server.HtmlEncode(dt[0].CGTransFeeDef.ToString());
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
            txtGifrCardMonFee.Text = dr["GiftCardMonFeeDef"].ToString().Trim();
            txtGifrCardTransFee.Text = dr["GiftCardTransFeeDef"].ToString().Trim();
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

    protected void btnResetGiftCard_Click(object sender, EventArgs e)
    {
        ResetControlsCheckService();
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
            if (lstGiftCard.SelectedItem.Value != "")
                PopulateGiftCardRates();
            else
                ResetControlsGiftCard();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            SetErrorMessageGW("Error retrieving rates");
        }
    }

    protected void btnUpdateGateway_Click(object sender, EventArgs e)
    {
        try
        {
            BoundsBL SetGWBounds = new BoundsBL();
            //SetGWBounds.UpdateGatewayBounds(txtGWSetupFee.Text.Trim(), txtGWMonthlyFee.Text.Trim(),
             //   txtGWTransFee.Text.Trim(), lstGatewayNames.SelectedItem.Value);
            SetGWBounds.UpdateGatewayDefaults(txtGWSetupFee.Text.Trim(), txtGWMonthlyFee.Text.Trim(),
               txtGWTransFee.Text.Trim(), lstGatewayNames.SelectedItem.Value);
            SetErrorMessageGW("Gateway Defaults set successfully");
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
            SetGWBounds.UpdateCheckServiceDefaults(txtCheckServiceDiscRate.Text.Trim(), txtCheckServiceMonFee.Text.Trim(),
                txtCheckServiceMonMin.Text.Trim(), txtCheckServiceTransFee.Text.Trim(), lstCheckService.SelectedItem.Value);
            SetErrorMessageCS("Check Service defaults set successfully");
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
            SetGiftCardBounds.UpdateGiftCardDefaults(txtGifrCardMonFee.Text.Trim(),
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

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

 
}
