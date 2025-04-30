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

public partial class CreatePackage : System.Web.UI.Page
{
    private static string CardPresent = "CNP";
    private static string ProcessorAbbrev;

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if ((Session.IsNewSession) || (Session == null))
            Response.Redirect("~/login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                Populate();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), "CreatePackage Page Load - " + err.Message);
                DisplayMessage("Error retrieving rates");
            }
        }//end if not post back
    }//end page load

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    //This function populates lists
    public void Populate()
    {
        ListBL SalesRepList = new ListBL();
        DataSet ds = SalesRepList.GetSalesRepList();
        if (ds.Tables[0].Rows.Count > 0)
        {
            lstSalesRep.DataSource = ds.Tables[0];
            lstSalesRep.DataTextField = "RepName";
            lstSalesRep.DataValueField = "MasterNum";
            lstSalesRep.DataBind();
        }
        ListItem lstItem = new ListItem();
        lstItem.Text = "ALL";
        lstItem.Value = "ALL";
        lstSalesRep.Items.Add(lstItem);

        if (!User.IsInRole("Admin"))
        {
            lstSalesRep.Enabled = false;
            txtPackagePrefix.Enabled = false;
            //Rep has no Master Rep Number, show Demo package
            if (Session["MasterNum"].ToString() == "")
                lstSalesRep.SelectedValue = lstSalesRep.Items.FindByValue("1111").Value; 
            else
                lstSalesRep.SelectedValue = lstSalesRep.Items.FindByValue(Session["MasterNum"].ToString()).Value;
        }
        else
            lstSalesRep.SelectedValue = lstSalesRep.Items.FindByValue("ALL").Value;


        //Get Processor list
        OnlineAppProcessingBL Processors = new OnlineAppProcessingBL();
        DataSet dsProcessors = Processors.GetProcessorNames("");
        if (dsProcessors.Tables[0].Rows.Count > 0)
        {
            lstProcessorNames.DataSource = dsProcessors;
            lstProcessorNames.DataTextField = "Processor";
            lstProcessorNames.DataValueField = "Processor";
            lstProcessorNames.DataBind();
        }

        PopulateGatewayList();
        PopulateRates();
    }//end function Populate

    //This function loads rates for selected processor
    public void PopulateRates()
    {
        try
        {
            LoadUpperLowerBounds();
            DisableFees();
            txtPackagePrefix.Text = lstSalesRep.SelectedItem.Text + "-" + CardPresent + " " + ProcessorAbbrev + "-";
            dimFields();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating rates");
        }
    }//end function PopulateRates()

    //This function loads upper and lower bounds for a processor
    public void LoadUpperLowerBounds()
    {
        BoundsBL Bounds = new BoundsBL();
        PartnerDS.ProcessorBoundsDataTable dt = Bounds.GetProcessorBounds(lstProcessorNames.SelectedItem.Text, CardPresent);
        if (dt.Rows.Count == 0)
            dt = Bounds.GetProcessorBounds("Global", "CNP");

        if (dt.Rows.Count > 0)
        {
            CustServLow.Value = dt[0].CustServLow.ToString();
            InternetStmtLow.Value = dt[0].InternetStmtLow.ToString();
            MonMinLow.Value = dt[0].MonMinLow.ToString();
            TransFeeLow.Value = dt[0].TransFeeLow.ToString();
            DiscRateQualPresLow.Value = dt[0].DiscRateQualPresLow.ToString();
            DiscRateQualNPLow.Value = dt[0].DiscRateQualNPLow.ToString();
            DiscRateMidQualLow.Value = dt[0].DiscRateMidQualLow.ToString();
            DiscRateNonQualLow.Value = dt[0].DiscRateNonQualLow.ToString();
            DiscRateQualDebitLow.Value = dt[0].DiscRateQualDebitLow.ToString();
            ChargebackFeeLow.Value = dt[0].ChargebackFeeLow.ToString();
            RetrievalFeeLow.Value = dt[0].RetrievalFeeLow.ToString();
            VoiceAuthLow.Value = dt[0].VoiceAuthLow.ToString();
            BatchHeaderLow.Value = dt[0].BatchHeaderLow.ToString();
            AVSLow.Value = dt[0].AVSLow.ToString();
            AnnualFeeLow.Value = dt[0].AnnualFeeLow.ToString();
            NBCTransFeeLow.Value = dt[0].NBCTransFeeLow.ToString();
            WirelessAccessFeeLow.Value = dt[0].WirelessAccessFeeLow.ToString();
            WirelessTransFeeLow.Value = dt[0].WirelessTransFeeLow.ToString();

            DebitMonFeeLow.Value = dt[0].DebitMonFeeLow.ToString();
            DebitTransFeeLow.Value = dt[0].DebitTransFeeLow.ToString();
            CGDiscRateLow.Value = dt[0].CGDiscRateLow.ToString();
            CGMonFeeLow.Value = dt[0].CGMonFeeLow.ToString();
            CGMonMinLow.Value = dt[0].CGMonMinLow.ToString();
            CGTransFeeLow.Value = dt[0].CGTransFeeLow.ToString();
            GCMonFeeLow.Value = dt[0].GCMonFeeLow.ToString();
            GCTransFeeLow.Value = dt[0].GCTransFeeLow.ToString();
            EBTMonFeeLow.Value = dt[0].EBTMonFeeLow.ToString();
            EBTTransFeeLow.Value = dt[0].EBTTransFeeLow.ToString();

            DiscRateMidQualStep.Value = dt[0].DiscRateMidQualStep.ToString();
            DiscRateNonQualStep.Value = dt[0].DiscRateNonQualStep.ToString();

            txtCustomerService.Text = dt[0].CustServDef.ToString();
            txtInternetStmt.Text = dt[0].InternetStmtDef.ToString();
            txtMonMin.Text = dt[0].MonMinDef.ToString();
            txtTransFee.Text = dt[0].TransFeeDef.ToString();
            txtChargebackFee.Text = dt[0].ChargebackFeeDef.ToString();
            txtRetrievalFee.Text = dt[0].RetrievalFeeDef.ToString();
            txtVoiceAuth.Text = dt[0].VoiceAuthDef.ToString();
            txtBatchHeader.Text = dt[0].BatchHeaderDef.ToString();
            txtAVS.Text = dt[0].AVSDef.ToString();
            txtNBCTransFee.Text = dt[0].NBCTransFeeDef.ToString();

            txtDebitMonFee.Text = dt[0].DebitMonFeeDef.ToString();
            txtDebitTransFee.Text = dt[0].DebitTransFeeDef.ToString();
            txtCGDiscRate.Text = dt[0].CGDiscRateDef.ToString();
            txtCGMonFee.Text = dt[0].CGMonFeeDef.ToString();
            txtCGMonMin.Text = dt[0].CGMonMinDef.ToString();
            txtCGTransFee.Text = dt[0].CGTransFeeDef.ToString();
            txtGCMonFee.Text = dt[0].GCMonFeeDef.ToString();
            txtGCTransFee.Text = dt[0].GCTransFeeDef.ToString();
            txtEBTMonFee.Text = dt[0].EBTMonFeeDef.ToString();
            txtEBTTransFee.Text = dt[0].EBTTransFeeDef.ToString();

            ProcessorAbbrev = dt[0].Abbrev.ToString().Trim();

            LoadAnnualFees(CardPresent);
            if (lstAnnualFee.Items.FindByValue(dt[0].AnnualFeeDef.ToString()) != null )
               lstAnnualFee.SelectedValue = dt[0].AnnualFeeDef.ToString();
        }//end if count not 0             

 
    }//end function LoadUpperLowerBounds

    //This function disables fees textbox based on processor selected
    public void DisableFees()
    {
        if (AVSLow.Value != "")
            txtAVS.Enabled = true;
        else
        {
            txtAVS.Enabled = false;
            txtAVS.Text = "";
        }

        if (NBCTransFeeLow.Value != "")
            txtNBCTransFee.Enabled = true;
        else
        {
            txtNBCTransFee.Enabled = false;
            txtNBCTransFee.Text = "";
        }

        if (WirelessAccessFeeLow.Value != "")
            txtWirelessAccess.Enabled = true;
        else
        {
            txtWirelessAccess.Enabled = false;
            txtWirelessAccess.Text = "";
        }

        if (WirelessTransFeeLow.Value != "")
            txtWirelessTransFee.Enabled = true;
        else
        {
            txtWirelessTransFee.Enabled = false;
            txtWirelessTransFee.Text = "";
        }

        if (DiscRateMidQualLow.Value != "")
            txtDRMQ.Enabled = true;
        else
        {
            txtDRMQ.Enabled = false;
            txtDRMQ.Text = "";
        }

        if (CustServLow.Value != "")
            txtCustomerService.Enabled = true;
        else
        {
            txtCustomerService.Enabled = false;
            txtCustomerService.Text = "";
        }

        if (lstProcessorNames.SelectedItem.Text == "IMS")
        {
            txtDRQD.Attributes.Add("readonly", "readonly");
            txtChargebackFee.Attributes.Add("readonly", "readonly");
            txtRetrievalFee.Attributes.Add("readonly", "readonly");
            txtVoiceAuth.Attributes.Add("readonly", "readonly");
            txtBatchHeader.Attributes.Add("readonly", "readonly");
        }
        else //Remove IMS-Disabled Read only Attributes
        {
            txtDRQD.Attributes.Remove("readonly");
            txtChargebackFee.Attributes.Remove("readonly");
            txtRetrievalFee.Attributes.Remove("readonly");
            txtVoiceAuth.Attributes.Remove("readonly");
            txtBatchHeader.Attributes.Remove("readonly"); 
        }
        //end if processor is IMS
        
        if (DiscRateQualDebitLow.Value != "")
        {
            txtDRQD.Enabled = true;
            txtDRQD.ReadOnly = false;
        }
        else
            txtDRQD.Enabled = false;
    }//end function DisableFees

    //This function loads upper and lower bounds for gateway account
    public void LoadUpperLowerBoundsGW()
    {
        BoundsBL Bounds = new BoundsBL();
        PartnerDS.GatewayBoundsDataTable dt = Bounds.GetGatewayBounds(lstGatewayNames.SelectedItem.Text);
        if (dt.Rows.Count > 0)
        {
            GatewayMonFeeLow.Value = dt[0].GatewayMonFeeLow.ToString();
            GatewayTransFeeLow.Value = dt[0].GatewayTransFeeLow.ToString();
            GatewaySetupFeeLow.Value = dt[0].GatewaySetupFeeLow.ToString();
        }//end if count not 0
    }//end function LoadUpperLowerBoundsGW

    //This function disables fields based on CP or CNP
    public void dimFields()
    {
        //Card Present checked, dim Card Not Present Fields
        if (rdbCP.Checked)
        {
            txtDRQNP.Enabled = false;
            txtDRQNP.Text = "";
            //re-enable Card Present Fields
            txtDRQP.Enabled = true;
        }
        //Card Not Present checked, dim Card Present Fields
        else if (rdbCNP.Checked)
        {
            txtDRQP.Enabled = false;
            txtDRQP.Text = "";
            //renable Card Not Present Fields
            txtDRQNP.Enabled = true;
        }
    }//end function dimFields

    protected void lstProcessorNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckProcessor();
            /*if (lstProcessorNames.SelectedItem.Text.Contains("QuickBooks") || lstProcessorNames.SelectedItem.Text.Contains("WorldPay"))
            {
                //QuickBooks has only Check Guarantee, disable rest
                pnlOnlineDebit.Visible = false;
                pnlEBT.Visible = false;
                pnlGiftCard.Visible = false;
                txtDebitMonFee.Text = "";
                txtDebitTransFee.Text = "";
                txtGCMonFee.Text = "";
                txtGCTransFee.Text = "";
                txtEBTMonFee.Text = "";
                txtEBTTransFee.Text = "";
            }
            else
            {
                pnlOnlineDebit.Visible = true;
                pnlEBT.Visible = true;
                pnlGiftCard.Visible = true;
            }
            if (lstProcessorNames.SelectedItem.Text == "Optimal-Cal")
                rdbCP.Enabled = false;
            else
                rdbCP.Enabled = true;

            if (rdbCP.Checked)
            {
                CardPresent = "CP";
                pnlOnlineDebit.Visible = true;
                pnlEBT.Visible = true;
                pnlGiftCard.Visible = true;
            }
            else if (rdbCNP.Checked)
            {
                CardPresent = "CNP";
                pnlOnlineDebit.Visible = false;
                pnlEBT.Visible = false;
                pnlGiftCard.Visible = false;
            }*/
            PopulateRates();
            PopulatePlatformList();
            PopulateGatewayList();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error loading data");
        }
    }

    //Fill Batch Header and NBCTransaction Fee when Transaction Fee and Internet Statement are changed
    protected void txtTransFee_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //Sage and QuickBooks Processors don't have a Batch Fee so set it to 0
            if (lstProcessorNames.SelectedItem.Text.ToString().Trim().Contains("Sage")) // || lstProcessorNames.SelectedItem.Text.ToString().Trim().Contains("QuickBooks"))
            {
                txtBatchHeader.Text = "0.00";
                txtNBCTransFee.Text = txtTransFee.Text.ToString().Trim();
            }
            else
            {
                txtBatchHeader.Text = txtTransFee.Text.ToString().Trim();
                txtNBCTransFee.Text = txtTransFee.Text.ToString().Trim();
            }
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Error Updating Transaction Fee - " + err.Message);
            DisplayMessage("Error Updating Transaction Fee.");
        }
    }

    //checks the Processor selected and enables and disables Form Fields accordingly
    public void CheckProcessor()
    {

        //Enable or disable Interchange and Assessments chk box
        if (lstProcessorNames.SelectedItem.Text.Contains("Optimal-Canada")
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Cal")
            || lstProcessorNames.SelectedItem.Text.Contains("IMS (QuickBooks)")
            || lstProcessorNames.SelectedItem.Text.Contains("WorldPay")
            || lstProcessorNames.SelectedItem.Text.Contains("Kitts"))
        {
            chkApplyInterchange.Visible = false;
            chkBillAssessment.Visible = false;
        }
        else
        {
            chkApplyInterchange.Visible = true;
            //chkBillAssessment.Visible = true;
            chkApplyInterchange.Checked = false;
            chkBillAssessment.Checked = false;
        }

        //Enable or disable Card Present option based on Processor
        if (lstProcessorNames.SelectedItem.Text.Contains("QuickBooks"))
        {
            rdbCP.Checked = true;
            rdbCNP.Checked = false;
            rdbCNP.Enabled = false;
            rdbCP.Enabled = false;
        }
        else if
        (lstProcessorNames.SelectedItem.Text.Contains("Optimal-Cal")
            || (lstProcessorNames.SelectedItem.Text.Contains("WorldPay"))
            || lstProcessorNames.SelectedItem.Text.Contains("Kitts")
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Canada"))
        {
            rdbCP.Checked = false;
            rdbCNP.Checked = true;
            rdbCNP.Enabled = false;
            rdbCP.Enabled = false;
        }
        else
        {
            rdbCNP.Enabled = true;
            rdbCP.Enabled = true;
        }

        //If CNP account or Optimal International or Optimal Canada
        if (rdbCNP.Checked == true
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Canada")
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Cal")
            || (lstProcessorNames.SelectedItem.Text.Contains("WorldPay"))
            || lstProcessorNames.SelectedItem.Text.Contains("Kitts")
            || lstProcessorNames.SelectedItem.Text.Contains("IMS (QuickBooks)"))
        {

            //Disable Other Services except Check Guarantee, disable rest
            pnlOnlineDebit.Visible = false;
            pnlEBT.Visible = false;
            pnlGiftCard.Visible = false;
            txtDebitMonFee.Text = "";
            txtDebitTransFee.Text = "";
            txtGCMonFee.Text = "";
            txtGCTransFee.Text = "";
            txtEBTMonFee.Text = "";
            txtEBTTransFee.Text = "";
        }
        else
        {
            pnlOnlineDebit.Visible = true;
            pnlEBT.Visible = true;
            pnlGiftCard.Visible = true;
        }

        if (rdbCP.Checked)
        {
            CardPresent = "CP";
        }
        else if (rdbCNP.Checked)
        {
            CardPresent = "CNP";
        }
    }
    protected void lstGatewayNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUpperLowerBoundsGW();
    }

    public void PopulatePlatformList()
    {
        //Get list of platforms
    }//end function PopulatePlatformList

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string errMessage = ValidateRates();
            if (errMessage == "")
            {
                PackageBL InsertPack = new PackageBL();
                bool retVal = InsertPack.InsertPackage(txtPackagePrefix.Text.Trim(), lstProcessorNames.SelectedItem.Text,
                    txtPackageSuffix.Text.Trim(), CardPresent, lstSalesRep.SelectedItem.Value, txtCustomerService.Text.Trim(),
                    txtInternetStmt.Text.Trim(), txtTransFee.Text.Trim(), txtDRQP.Text.Trim(), txtDRQNP.Text.Trim(), txtDRMQ.Text.Trim(),
                    txtDRNQ.Text.Trim(), txtDRQD.Text.Trim(), txtChargebackFee.Text.Trim(), txtRetrievalFee.Text.Trim(),
                    txtVoiceAuth.Text.Trim(), txtBatchHeader.Text.Trim(), txtAVS.Text.Trim(), txtMonMin.Text.Trim(),
                    txtNBCTransFee.Text.Trim(), lstAnnualFee.SelectedItem.Value, txtWirelessAccess.Text.Trim(),
                    txtWirelessTransFee.Text.Trim(), txtApplicationFee.Text.Trim(), txtSetupFee.Text.Trim(),
                    lstGatewayNames.SelectedItem.Text, txtGWTransFee.Text.Trim(), txtGWMonthlyFee.Text.Trim(),
                    txtGWSetupFee.Text.Trim(), txtDebitMonFee.Text.Trim(), txtDebitTransFee.Text.Trim(),
                    txtCGDiscRate.Text.Trim(), txtCGMonFee.Text.Trim(), txtCGMonMin.Text.Trim(), txtCGTransFee.Text.Trim(),
                    txtGCMonFee.Text.Trim(), txtGCTransFee.Text.Trim(), txtEBTMonFee.Text.Trim(),
                    txtEBTTransFee.Text.Trim(), txtRollingReserve.Text.Trim(), chkApplyInterchange.Checked, chkBillAssessment.Checked);
                if (retVal)
                {
                    DisplayMessage("Package Created Successfully");
                    //Add log
                    PartnerLogBL LogData = new PartnerLogBL();
                    retVal = LogData.InsertLogRates(0,
                            Convert.ToInt32(Session["AffiliateID"]), "Package (" + txtPackagePrefix.Text.Trim() + txtPackageSuffix.Text.Trim() + ") Created.");
                }
                else
                    DisplayMessage("Error Creating Package");
                //Populate();
            }
            else
                DisplayMessage(errMessage);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error creating new package");
        }
    }

    public void LoadAnnualFees(string CardPresent)
    {
        //Get annual fees
        OnlineAppProcessingBL AnnualFees = new OnlineAppProcessingBL();
        DataSet dsAnnualFee = AnnualFees.GetAnnualFee(lstProcessorNames.SelectedItem.Text, CardPresent);
        if (dsAnnualFee.Tables[0].Rows.Count > 0)
        {
            lstAnnualFee.DataSource = dsAnnualFee;
            lstAnnualFee.DataTextField = "AnnualFee";
            lstAnnualFee.DataValueField = "AnnualFee";
            lstAnnualFee.DataBind();
        }
    }//end LoadAnnualFees

    protected void rdbCP_CheckedChanged(object sender, EventArgs e)
    {
        if (rdbCP.Checked)
        {
            CardPresent = "CP";
            pnlOnlineDebit.Visible = true;
            pnlEBT.Visible = true;
            pnlGiftCard.Visible = true;
        }
        else if (rdbCNP.Checked)
        {
            CardPresent = "CNP";
            pnlOnlineDebit.Visible = false;
            pnlEBT.Visible = false;
            pnlGiftCard.Visible = false;
        }
        if (lstProcessorNames.SelectedItem.Value.ToLower().Contains("quickbooks") || lstProcessorNames.SelectedItem.Text.Contains("WorldPay"))
        {
            pnlOnlineDebit.Visible = false;
            pnlEBT.Visible = false;
            pnlGiftCard.Visible = false;
        }
        PopulateRates();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {

    }

    protected void lstSalesRep_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPackagePrefix.Text = lstSalesRep.SelectedItem.Text + "-" + CardPresent + " " + ProcessorAbbrev + "-";
    }
    
    public void PopulateGatewayList()
    {
        //Get Gateway list
        OnlineAppProcessingBL Gateways = new OnlineAppProcessingBL();
        DataSet dsGateways = Gateways.GetGateways(lstProcessorNames.SelectedItem.Value);
        if (dsGateways.Tables[0].Rows.Count > 0)
        {
            lstGatewayNames.DataSource = dsGateways;
            lstGatewayNames.DataTextField = "Gateway";
            lstGatewayNames.DataValueField = "Gateway";
            lstGatewayNames.DataBind();
        }        
        ListItem lstItem = new ListItem();
        lstItem.Text = "None";
        lstItem.Value = "None";
        lstGatewayNames.Items.Add(lstItem);
        lstGatewayNames.SelectedValue = lstGatewayNames.Items.FindByValue("None").Value;
    }

    protected void chkApplyInterchange_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkApplyInterchange.Checked)
            {
                LoadInterchangeBounds();
                chkBillAssessment.Checked = true;
                if (rdbCP.Checked)
                {
                    txtDRMQ.Text = txtDRQP.Text.Trim();
                    txtDRNQ.Text = txtDRQP.Text.Trim();
                }
                else if (rdbCNP.Checked)
                {
                    txtDRMQ.Text = txtDRQNP.Text.Trim();
                    txtDRNQ.Text = txtDRQNP.Text.Trim();
                }
                txtDRMQ.Enabled = false;
                txtDRNQ.Enabled = false;

            }
            else if (!chkApplyInterchange.Checked)
            {
                chkBillAssessment.Checked = false;
                string strCardPresent = "CP";
                if (rdbCNP.Checked)
                    strCardPresent = "CNP";
                txtDRMQ.Enabled = true;
                txtDRNQ.Enabled = true;
                txtDRMQ.Text = "";
                txtDRNQ.Text = "";
                LoadUpperLowerBounds();
                LoadAnnualFees(strCardPresent);
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Apply Interchange Rates - " + err.Message);
            DisplayMessage("Error loading data.");
        }
    }

    public void LoadInterchangeBounds()
    {
        TransFeeLow.Value = "";
        DiscRateQualPresLow.Value = "";
        DiscRateQualNPLow.Value = "";
        DiscRateMidQualLow.Value = "";
        DiscRateNonQualLow.Value = "";
        DiscRateQualDebitLow.Value = "";
    }

    #region VALIDATE RATES
    public string ValidateRates()
    {
        string strError = "";
        //Customer Service
        if (CustServLow.Value != "")
        {
            if ((txtCustomerService.Text == "") || (Convert.ToDecimal(txtCustomerService.Text) < Convert.ToDecimal(CustServLow.Value)))
                strError += "Enter at least $" + CustServLow.Value + " for Customer Service Fee." + "<BR/>";
        }

        //Internet Statement
        if (InternetStmtLow.Value != "")
        {
            if ((txtCustomerService.Text == "") || (Convert.ToDecimal(txtCustomerService.Text) < Convert.ToDecimal(InternetStmtLow.Value)))
                strError += "Enter at least $" + InternetStmtLow.Value + " for Customer Service Fee." + "<BR/>";
        }
        //Monthly Minimum
        if (MonMinLow.Value != "")
        {
            if ((txtMonMin.Text == "") || (Convert.ToDecimal(txtMonMin.Text) < Convert.ToDecimal(MonMinLow.Value)))
            {
                strError += "Enter at least $" + MonMinLow.Value + " for Monthly Minimum." + "<BR/>";
            }
        }
        //Transaction Fee
        if (TransFeeLow.Value != "")
        {
            if ((txtTransFee.Text == "") || (Convert.ToDecimal(txtTransFee.Text) < Convert.ToDecimal(TransFeeLow.Value)))
                strError += "Enter at least $" + TransFeeLow.Value + " for Transaction Fee." + "<BR/>";
        }
        //DiscRateQualPres
        if (DiscRateQualPresLow.Value != "")
        {
            if ((txtDRQP.Text == "") || (Convert.ToDecimal(txtDRQP.Text) < Convert.ToDecimal(DiscRateQualPresLow.Value)))
            {
                strError += "Enter at least " + DiscRateQualPresLow.Value + "% for Disc Rate Qual Pres." + "<BR/>";
            }
        }
        //DiscRateQualNP
        if (DiscRateQualNPLow.Value != "")
        {
            if ((txtDRQNP.Text == "") || (Convert.ToDecimal(txtDRQNP.Text) < Convert.ToDecimal(DiscRateQualNPLow.Value)))
         
                strError += "Enter at least " + DiscRateQualNPLow.Value + "% for Disc Rate Qual NP." + "<BR/>";
            
        }

        //If IMS and Card Present overwrite the Mid Qual Low to be the Qual Pres PLUS the specified Mid Qual Step in the database 
        if ((rdbCP.Checked) && (lstProcessorNames.SelectedItem.Text == "IMS"))
            DiscRateMidQualLow.Value = Convert.ToString(Convert.ToDecimal(txtDRQP.Text) + Convert.ToDecimal(DiscRateMidQualStep.Value));

        //DiscRateMidQual
        if (DiscRateMidQualLow.Value != "")
        {
            if ((txtDRMQ.Text == "") || (Convert.ToDecimal(txtDRMQ.Text) < Convert.ToDecimal(DiscRateMidQualLow.Value)))
                strError += "Enter at least " + DiscRateMidQualLow.Value + "% for Disc Rate Mid Qual." + "<BR/>";
        }
        //DiscRateNonQual
        if (DiscRateNonQualLow.Value != "")
        {
            if ((txtDRNQ.Text == "") || (Convert.ToDecimal(txtDRNQ.Text) < Convert.ToDecimal(DiscRateNonQualLow.Value)))
                strError += "Enter at least " + DiscRateNonQualLow.Value + "% for Disc Rate Non Qual." + "<BR/>";
        }
        //DiscRateQualDebit        
        /*if (DiscRateQualDebitLow.Value != "")
        {
            if ((txtDRQD.Text == "") || (Convert.ToDecimal(txtDRQD.Text) < Convert.ToDecimal(DiscRateQualDebitLow.Value)))
            {
                strError += "Enter at least " + DiscRateQualDebitLow.Value + "% for Disc Rate Qual Debit." + "<BR/>";
            }
        }*/
        //ChargebackFee
        if (ChargebackFeeLow.Value != "")
        {
            if ((txtChargebackFee.Text == "") || (Convert.ToDecimal(txtChargebackFee.Text) < Convert.ToDecimal(ChargebackFeeLow.Value)))

                strError += "Enter at least $" + ChargebackFeeLow.Value + " for Chargeback Fee." + "<BR/>";
        }
        //RetrievalFee
        if (RetrievalFeeLow.Value != "")
        {
            if ((txtRetrievalFee.Text == "") || (Convert.ToDecimal(txtRetrievalFee.Text) < Convert.ToDecimal(RetrievalFeeLow.Value)))
                strError += "Enter at least $" + RetrievalFeeLow.Value + " for Retrieval Fee." + "<BR/>";
        }
        //VoiceAuth
        if (VoiceAuthLow.Value != "")
        {
            if ((txtVoiceAuth.Text == "") || (Convert.ToDecimal(txtVoiceAuth.Text) < Convert.ToDecimal(VoiceAuthLow.Value)))
                strError += "Enter at least $" + VoiceAuthLow.Value + " for Voice Authorization Fee." + "<BR/>";
        }
        //BatchHeader
        if (BatchHeaderLow.Value != "")
        {
            if ((txtBatchHeader.Text == "") || (Convert.ToDecimal(txtBatchHeader.Text) < Convert.ToDecimal(BatchHeaderLow.Value)))
            {
                strError += "Enter at least $" + BatchHeaderLow.Value + " for Batch Header Fee." + "<BR/>";
            }
        }
        //AVS
        if (AVSLow.Value != "")
        {
            if ((txtAVS.Text == "") || (Convert.ToDecimal(txtAVS.Text) < Convert.ToDecimal(AVSLow.Value)))
                strError += "Enter at least $" + AVSLow.Value + " for AVS Fee." + "<BR/>";
        }
        //WirelessAccessFee
        if ((WirelessAccessFeeLow.Value != "") && (txtWirelessAccess.Text != ""))
        {
            if ((Convert.ToDecimal(txtWirelessAccess.Text) < Convert.ToDecimal(WirelessAccessFeeLow.Value)))
                strError += "Enter at least $" + WirelessAccessFeeLow.Value + " for Wireless Access Fee or leave blank if it does not apply." + "<BR/>";
        }
        //WirelessTransFee
        if ((WirelessTransFeeLow.Value != "") && (txtWirelessTransFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtWirelessTransFee.Text) < Convert.ToDecimal(WirelessTransFeeLow.Value)))
                strError += "Enter at least $" + WirelessTransFeeLow.Value + " for Wireless Trans Fee or leave blank if it does not apply." + "<BR/>";
        }
        //NBCTransFee
        if (NBCTransFeeLow.Value != "")
        {
            if ((txtNBCTransFee.Text == "") || (Convert.ToDecimal(txtNBCTransFee.Text) < Convert.ToDecimal(NBCTransFeeLow.Value)))
                strError += "Enter at least $" + NBCTransFeeLow.Value + " for Non-Bankcard Trans Fee." + "<BR/>";
        }
        if (lstGatewayNames.SelectedItem.Text != "None")
        {
            //GatewayTransFee
            if (GatewayTransFeeLow.Value != "")
            {
                if ((txtGWTransFee.Text == "") || (Convert.ToDecimal(txtGWTransFee.Text) < Convert.ToDecimal(GatewayTransFeeLow.Value)))
                {
                    strError += "Enter at least $" + GatewayTransFeeLow.Value + " for Gateway Transaction Fee." + "<BR/>";
                }
            }
            //GatewayMonFee
            if (GatewayMonFeeLow.Value != "")
            {
                if ((txtGWMonthlyFee.Text == "") || (Convert.ToDecimal(txtGWMonthlyFee.Text) < Convert.ToDecimal(GatewayMonFeeLow.Value)))
                    strError += "Enter at least $" + GatewayMonFeeLow.Value + " for Gateway Monthly Fee." + "<BR/>";
            }
            //GatewaySetupFee
            if (GatewaySetupFeeLow.Value != "")
            {
                if ((txtGWSetupFee.Text == "") || (Convert.ToDecimal(txtGWSetupFee.Text) < Convert.ToDecimal(GatewaySetupFeeLow.Value)))
                    strError += "Enter at least " + GatewaySetupFeeLow.Value + " for Gateway Setup Fee" + "<BR/>";
            }
        }
        //Additional Services Checks
        //DebitMonFee
        if ((DebitMonFeeLow.Value != "") && (txtDebitMonFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtDebitMonFee.Text) < Convert.ToDecimal(DebitMonFeeLow.Value)))
                strError += "Enter at least $" + DebitMonFeeLow.Value + " for Debit Mon Fee or leave blank if it does not apply." + "<BR/>";
        }
        //DebitTransFee
        if ((DebitTransFeeLow.Value != "") && (txtDebitTransFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtDebitTransFee.Text) < Convert.ToDecimal(DebitTransFeeLow.Value)))
                strError += "Enter at least $" + DebitTransFeeLow.Value + " for Debit Trans Fee or leave blank if it does not apply." + "<BR/>";
        }
        //CGDiscRate
        if ((CGDiscRateLow.Value != "") && (txtCGDiscRate.Text != ""))
        {
            if ((Convert.ToDecimal(txtCGDiscRate.Text) < Convert.ToDecimal(CGDiscRateLow.Value)))
                strError += "Enter at least $" + CGDiscRateLow.Value + " for Check Guarantee Disc Rate or leave blank if it does not apply." + "<BR/>";
        }
        //CGMonFee
        if ((CGMonFeeLow.Value != "") && (txtCGMonFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtCGMonFee.Text) < Convert.ToDecimal(CGMonFeeLow.Value)))
            {
                strError += "Enter at least $" + CGMonFeeLow.Value + " for Check Guarantee Mon Fee or leave blank if it does not apply." + "<BR/>";
            }
        }
        //CGMonMin
        if ((CGMonMinLow.Value != "") && (txtCGMonMin.Text != ""))
        {
            if ((Convert.ToDecimal(txtCGMonMin.Text) < Convert.ToDecimal(CGMonMinLow.Value)))
                strError += "Enter at least $" + CGMonMinLow.Value + " for Check Guarantee Mon Min or leave blank if it does not apply." + "<BR/>";
        }
        //CGTransFee
        if ((CGTransFeeLow.Value != "") && (txtCGTransFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtCGTransFee.Text) < Convert.ToDecimal(CGTransFeeLow.Value)))
                strError += "Enter at least $" + CGTransFeeLow.Value + " for Check Guarantee Trans Fee or leave blank if it does not apply." + "<BR/>";
        }
        //GCMonMin
        if ((GCMonFeeLow.Value != "") && (txtGCMonFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtGCMonFee.Text) < Convert.ToDecimal(GCMonFeeLow.Value)))
            {
                strError += "Enter at least $" + GCMonFeeLow.Value + " for Gift Card Mon Min or leave blank if it does not apply." + "<BR/>";
            }
        }
        //GCTransFee
        if ((GCTransFeeLow.Value != "") && (txtGCTransFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtGCTransFee.Text) < Convert.ToDecimal(GCTransFeeLow.Value)))
                strError += "Enter at least $" + GCTransFeeLow.Value + " for Gift Card Trans Fee or leave blank if it does not apply." + "<BR/>";
        }
        //EBTMonMin
        if ((EBTMonFeeLow.Value != "") && (txtEBTMonFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtEBTMonFee.Text) < Convert.ToDecimal(EBTMonFeeLow.Value)))
            {
                strError += "Enter at least $" + EBTMonFeeLow.Value + " for EBT Mon Min or leave blank if it does not apply." + "<BR/>";
            }
        }
        //EBTTransFee
        if ((EBTTransFeeLow.Value != "") && (txtEBTTransFee.Text != ""))
        {
            if ((Convert.ToDecimal(txtEBTTransFee.Text) < Convert.ToDecimal(EBTTransFeeLow.Value)))
                strError += "Enter at least $" + EBTTransFeeLow.Value + " for EBT Trans Fee or leave blank if it does not apply." + "<BR/>";
        }
        //AnnualFee
        /*if (AnnualFeeLow.Value != "")
        {
            if ((txtAnnualFee.Text == "") || (Convert.ToDecimal(txtAnnualFee.Text) < Convert.ToDecimal(AnnualFeeLow.Value)))
            {
                strError += "Enter at least $" + AnnualFeeLow.Value + " for Annual Fee." + "<BR/>";
                strError += "Annual Fee Currently " + txtAnnualFee.Text;
            }
        }*/
        return strError;
    }//end function ValidateRates
    #endregion




}
