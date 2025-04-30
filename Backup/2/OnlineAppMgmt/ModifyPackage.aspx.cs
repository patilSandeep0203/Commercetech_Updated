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

public partial class ModifyPackage : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    private static string ProcessorAbbrev;
    private static string SalesRep;
    //private static int iPackageID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;
                
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
            Response.Redirect("~/login.aspx?Authentication=False");
        
        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                Style PIDLabel = new Style();
                PIDLabel.ForeColor = System.Drawing.Color.White;
                PIDLabel.Font.Size = FontUnit.Small;
                PIDLabel.Font.Bold = true;
                lblPID.ApplyStyle(PIDLabel);                                               
                Populate();
                PopulateRates(Convert.ToInt16(lstPackageNames.SelectedValue.ToString() ) );
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), "ModifyPackage Page Load- " + err.Message);
                DisplayMessage("Error retrieving rates");
            }
        }//end if not post back
    }//end page load

    //This function disables fields based on CP or CNP
    public void dimFields()
    {
         //If Quickbooks account, enable both Disc Rate fields
		if ( lstProcessorNames.SelectedItem.Text.Contains("QuickBooks") )
		{
		    txtDRQP.Enabled = true;
		    txtDRQNP.Enabled = true;
		}
        //Card Present checked, dim Card Not Present Fields (except for Quickbooks)
        else if (rdbCP.Checked )
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
            //re-enable Card Not Present Fields
            txtDRQNP.Enabled = true;
        }

        if (lblSalesRep.Text.ToString().Trim().Contains("ALL"))
        {
            if (!User.IsInRole("Admin"))
            {
                btnDelete.Enabled = false;
                btnSubmit.Enabled = false;
            }
        }
        else
        {
            btnSubmit.Enabled = true;
        }
    }//end function dimFields

    public void PopulateRates(int SelValue)
    {
        lblPID.Text = SelValue.ToString();
        lblAppIDsMore.Visible = false;
        lblAffiliateIDsMore.Visible = false;
        lblInactive.Visible = false;

        string CardPresent;
        //Get rates
        OnlineAppClassLibrary.PackageInfo Package = new OnlineAppClassLibrary.PackageInfo();
        PackageBL PackageMgr = new PackageBL();
        DataSet ds = Package.GetPackageInfo(SelValue);

        DataSet dsAppIDs = PackageMgr.GetAppIDs(SelValue);
        DataSet dsAffIDs = PackageMgr.GetAffiliateIDs(SelValue);
        string strAppIDs = "";
        string strAffIDs = "";
        string strAffIDsMore = "";
        string strAppIDsMore = "";
        DataRow dr;
        //Set the label to hold all the App IDs 
        for (int i = 0; i < dsAppIDs.Tables[0].Rows.Count; i++)
        {
            string strAppID = dsAppIDs.Tables[0].Rows[i][0].ToString();
            //do not print the first comma
            if (i == 0)
                strAppIDs = strAppID;
            else if (i < 10)
                strAppIDs = strAppIDs + " " + strAppID;
            else
            {
                lblAppIDsMore.Visible = true;
                strAppIDsMore = strAppIDsMore + " " + strAppID;
            }
        }
        lblAppIDs.Text = strAppIDs;
        lblAppIDsMore.ToolTip = strAppIDsMore;

        //set label to hold Affiliate IDs
        for (int i = 0; i < dsAffIDs.Tables[0].Rows.Count; i++)
        {
            string strAffID = dsAffIDs.Tables[0].Rows[i][0].ToString();

            //do not print the first comma
            if (i == 0)
                strAffIDs = strAffID;
            else if (i < 10)
                strAffIDs = strAffIDs + " " + strAffID;
            else
            {
                lblAffiliateIDsMore.Visible = true;
                strAffIDsMore = strAffIDsMore + " " + strAffID;
            }
        }
        lblAffiliateIDs.Text = strAffIDs;
        lblAffiliateIDsMore.ToolTip = strAffIDsMore;
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            dr = ds.Tables[0].Rows[0];
            if (!Convert.ToBoolean(dr["Active"].ToString()))
                lblInactive.Visible = true;

            //Merchant Account Rates
            CardPresent = dr["CardPresent"].ToString().Trim();
            if (CardPresent == "CP")
            {
                pnlOnlineDebit.Visible = true;
                pnlGiftCard.Visible = true;
                pnlEBT.Visible = true;
            }
            else
            {   
                pnlOnlineDebit.Visible = false;
                pnlGiftCard.Visible = false;
                pnlEBT.Visible = false;
            }

            //lstPackageNames.SelectedItem.Text = Server.HtmlEncode(dr["PackageName"].ToString());
            txtCustomerService.Text = Server.HtmlEncode(dr["CustServFee"].ToString());
            txtInternetStmt.Text = Server.HtmlEncode(dr["InternetStmt"].ToString());            
            txtMonMin.Text = Server.HtmlEncode(dr["MonMin"].ToString());
            txtTransFee.Text = Server.HtmlEncode(dr["TransactionFee"].ToString());
            txtWirelessAccess.Text = Server.HtmlEncode(dr["WirelessAccessFee"].ToString());
            txtWirelessTransFee.Text = dr["WirelessTransFee"].ToString();
            txtDRQP.Text = dr["DiscRateQualPres"].ToString();
            txtDRQNP.Text = dr["DiscRateQualNP"].ToString();
            txtDRMQ.Text = dr["DiscRateMidQual"].ToString();
            txtDRNQ.Text = dr["DiscRateNonQual"].ToString();
            txtDRQD.Text = dr["DiscRateQualDebit"].ToString();
            txtChargebackFee.Text = Server.HtmlEncode(dr["ChargebackFee"].ToString());
            txtRetrievalFee.Text = Server.HtmlEncode(dr["RetrievalFee"].ToString());
            txtVoiceAuth.Text = Server.HtmlEncode(dr["VoiceAuth"].ToString());
            txtBatchHeader.Text = Server.HtmlEncode(dr["BatchHeader"].ToString());
            txtAVS.Text = Server.HtmlEncode(dr["AVS"].ToString());
            txtNBCTransFee.Text = Server.HtmlEncode(dr["NBCTransFee"].ToString());
            txtApplicationFee.Text = Server.HtmlEncode(dr["AppFee"].ToString());
            txtSetupFee.Text = Server.HtmlEncode(dr["AppSetupFee"].ToString());
            txtRollingReserve.Text = Server.HtmlEncode(dr["RollingReserve"].ToString());
            //Gateway Rates
            txtGWSetupFee.Text = Server.HtmlEncode(dr["GatewaySetupFee"].ToString());
            txtGWMonthlyFee.Text = Server.HtmlEncode(dr["GatewayMonFee"].ToString());
            txtGWTransFee.Text = Server.HtmlEncode(dr["GatewayTransFee"].ToString());

            //Additional Services Rates
            //Online Debit
            txtDebitMonFee.Text = Server.HtmlEncode(dr["DebitMonFee"].ToString());
            txtDebitTransFee.Text = Server.HtmlEncode(dr["DebitTransFee"].ToString());
            //Check Guarantee
            txtCGDiscRate.Text = Server.HtmlEncode(dr["CGDiscRate"].ToString());
            txtCGMonFee.Text = Server.HtmlEncode(dr["CGMonFee"].ToString());
            txtCGMonMin.Text = Server.HtmlEncode(dr["CGMonMin"].ToString());
            txtCGTransFee.Text = Server.HtmlEncode(dr["CGTransFee"].ToString());
            txtGCMonFee.Text = Server.HtmlEncode(dr["GCMonFee"].ToString());
            txtGCTransFee.Text = Server.HtmlEncode(dr["GCTransFee"].ToString());
            txtEBTMonFee.Text = Server.HtmlEncode(dr["EBTMonFee"].ToString());
            txtEBTTransFee.Text = Server.HtmlEncode(dr["EBTTransFee"].ToString());

            if (lstProcessorNames.Items.FindByValue(dr["Processor"].ToString()) == null)
                lstProcessorNames.Items.Add(dr["Processor"].ToString());

            lstProcessorNames.SelectedValue = lstProcessorNames.Items.FindByValue(dr["Processor"].ToString()).Value;
            
            if ((SelValue == 253) || (SelValue == 254))
            {
                pnlGatewayRates.Enabled = false;
            }
            else
            {
                pnlGatewayRates.Enabled = true;
                PopulateGatewayList();
            }

            /*if ((SelValue != 253) && (SelValue != 254))
            {
                pnlGatewayRates.Enabled = true;
                PopulateGatewayList();

            }
            else
            {
                pnlGatewayRates.Enabled = false;
            }*/

            /*if ((lstPackageNames.SelectedItem.Text == "ALL-CP IPS-QuickBooks POS") || (lstPackageNames.SelectedItem.Value == "ALL-CP IPS-GoPayment"))
            {
                lblGateway.Enabled = false;

            }
            else
            {
                lblGateway.Enabled = true;
                //PopulateGatewayList();
            }*/

            //if ((SelValue == 253) && (SelValue == 253))

            if (lstGatewayNames.Items.FindByValue(dr["Gateway"].ToString()) == null)
                lstGatewayNames.Items.Add(dr["Gateway"].ToString());
            lstGatewayNames.SelectedValue = lstGatewayNames.Items.FindByValue(dr["Gateway"].ToString()).Value;
            
            if (dr["RepNum"].ToString().Trim() == "ALL")
                SalesRep = "ALL";
            else
                SalesRep = dr["RepName"].ToString().Trim();

            if (CardPresent == "CP")
            {
                rdbCP.Checked = true;
                rdbCNP.Checked = false;
            }
            else
            {
                rdbCNP.Checked = true;
                rdbCP.Checked = false;
            }
                                    
            LoadUpperLowerBounds(lstProcessorNames.SelectedItem.Text.Trim(), CardPresent);
            LoadUpperLowerBoundsGW(lstGatewayNames.SelectedItem.Text.Trim());
            DisableFees();

            if (Convert.ToBoolean(dr["Interchange"].ToString()))
                chkApplyInterchange.Checked = true;
            else
                chkApplyInterchange.Checked = false;

            if (chkApplyInterchange.Checked)
                LoadInterchangeBounds();

            if (Convert.ToBoolean(dr["Assessments"].ToString()))
                chkBillAssessment.Checked = true;
            else
                chkBillAssessment.Checked = false;

            //if Annual Fee not already present in dropdown, add it
            if (lstAnnualFee.Items.FindByValue(dr["AnnualFee"].ToString()) == null)
                lstAnnualFee.Items.Add(dr["AnnualFee"].ToString());
                
            lstAnnualFee.SelectedValue = dr["AnnualFee"].ToString();
            txtPackageSuffix.Text = dr["PackageSuffix"].ToString().Trim();
            txtPackagePrefix.Text = SalesRep + "-" + CardPresent + " " + ProcessorAbbrev + "-";
            
            lblSalesRep.Text = Server.HtmlEncode(dr["RepName"].ToString().Trim());
            if (dr["RepNum"].ToString().Trim() == "ALL")
                lblSalesRep.Text = "ALL";

            CheckProcessor();
            /*
            if (lstProcessorNames.SelectedItem.Text.Contains("QuickBooks") || lstProcessorNames.SelectedItem.Text.Contains("WorldPay")
                || (CardPresent == "CNP"))
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
            }*/
            dimFields();
        }//end if count not 0
    }//end function PopulateRates

    public void CheckProcessor()
    {

        //Enable or disable Interchange and Assessments chk box
        if (lstProcessorNames.SelectedItem.Text.Contains("Optimal-Canada")
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Cal")
            || lstProcessorNames.SelectedItem.Text.Contains("Intuit Payment Solutions (QuickBooks)")
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
        }

        //If CNP account or Optimal International or Optimal Canada
        if (rdbCNP.Checked == true
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Canada")
            || lstProcessorNames.SelectedItem.Text.Contains("Optimal-Cal")
            || (lstProcessorNames.SelectedItem.Text.Contains("WorldPay"))
            || lstProcessorNames.SelectedItem.Text.Contains("Kitts")
            || lstProcessorNames.SelectedItem.Text.Contains("Intuit Payment Solutions (QuickBooks)"))
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
    }
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

        if (CustServLow.Value != "")
            txtCustomerService.Enabled = true;
        else
        {
            txtCustomerService.Enabled = false;
            txtCustomerService.Text = "";
        }

        if (DiscRateMidQualLow.Value != "")
            txtDRMQ.Enabled = true;
        else
        {
            txtDRMQ.Enabled = false;
            txtDRMQ.Text = "";
        }

        if (lstProcessorNames.SelectedItem.Text == "Intuit Payment Solutions")
        {
            txtDRQD.Attributes.Add("readonly", "readonly");
            txtChargebackFee.Attributes.Add("readonly", "readonly");
            txtRetrievalFee.Attributes.Add("readonly", "readonly");
            txtVoiceAuth.Attributes.Add("readonly", "readonly");
            txtBatchHeader.Attributes.Add("readonly", "readonly");
        }
        else
        {
            txtDRQD.Attributes.Remove("readonly");
            txtChargebackFee.Attributes.Remove("readonly");
            txtRetrievalFee.Attributes.Remove("readonly");
            txtVoiceAuth.Attributes.Remove("readonly");
            txtBatchHeader.Attributes.Remove("readonly"); 
        }//end if processor is IMS

        if (DiscRateQualDebitLow.Value != "")
        {
            txtDRQD.Enabled = true;
            txtDRQD.ReadOnly = false;
        }
        else
            txtDRQD.Enabled = false;
    }//end function DisableFees

    //This function loads upper and lower bounds for a processor
    public void LoadUpperLowerBounds(string Processor, string CardPresent)
    {
        BoundsBL Bounds = new BoundsBL();
        PartnerDS.ProcessorBoundsDataTable dt = Bounds.GetProcessorBounds(Processor, CardPresent);
        
        //If No Bounds defined, explictly search for the Global structure
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
            ProcessorAbbrev = dt[0].Abbrev.ToString().Trim();
        }//end if count not 0     
        LoadAnnualFees(CardPresent);
    }//end function LoadUpperLowerBounds

    //This function loads upper and lower bounds for gateway account
    public void LoadUpperLowerBoundsGW(string Gateway)
    {
        BoundsBL Bounds = new BoundsBL();
        PartnerDS.GatewayBoundsDataTable dt = Bounds.GetGatewayBounds(Gateway);
        if (dt.Rows.Count > 0)
        {
            GatewayMonFeeLow.Value = dt[0].GatewayMonFeeLow.ToString();
            GatewayTransFeeLow.Value = dt[0].GatewayTransFeeLow.ToString();
            GatewaySetupFeeLow.Value = dt[0].GatewaySetupFeeLow.ToString();
        }//end if count not 0
    }//end function LoadUpperLowerBoundsGW

    //This function Populates Rates
    public void Populate()
    {        
        PackageBL Packages = new PackageBL();
        DataSet dsPackages;
        if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
        {
            dsPackages = Packages.GetPackagesForRep(Convert.ToString(Session["MasterNum"]));
            pnlDetail.Visible = false;
        }
        else
            dsPackages = Packages.GetAllPackages();        
        
        if (dsPackages.Tables[0].Rows.Count > 0)
        {
            lstPackageNames.DataSource = dsPackages;
            lstPackageNames.DataTextField = "PackageName";
            lstPackageNames.DataValueField = "PackageID";
            lstPackageNames.DataBind();
            lstPackageNames.SelectedIndex = 0;
        }
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

        /*if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
        {
            ListItem lstItemPackage = new ListItem();
            lstItemPackage.Text = "None";
            lstItemPackage.Value = "None";
            lstPackageNames.Items.Add(lstItemPackage);
        }*/
        /*if ((lstPackageNames.SelectedItem.Text == "ALL-CP IPS-QuickBooks POS") || (lstPackageNames.SelectedItem.Value == "ALL-CP IPS-GoPayment"))
        {
            lblGateway.Enabled = false;
            
        }
        else {
            lblGateway.Enabled = true;
            //PopulateGatewayList();
        }*/

        if ((lstPackageNames.SelectedItem.Value == "253") || (lstPackageNames.SelectedItem.Value == "254"))
        {
            pnlGatewayRates.Enabled = false;

        }
        else
        {
            pnlGatewayRates.Enabled = true;
            PopulateGatewayList();
        }
               
        if (!User.IsInRole("Admin"))
            txtPackagePrefix.Enabled = false;
            
    }//end function Populate

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        pnlDeletePackage.Visible = true;
    }

    protected void lstPackageNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            PopulateRates(Convert.ToInt32(lstPackageNames.SelectedItem.Value));
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error loading package information.");
        }
    }    

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try       
        {
			string errMessage = "";
			if ( lstPackageNames.SelectedItem.Value != "0")
				 errMessage = ValidateRates();
			else errMessage = "This package is used for clearing rates and cannot be modified";
			
            if (errMessage == "")
            {
                PackageBL UpdatePack = new PackageBL();
                bool retVal = UpdatePack.UpdatePackage(Convert.ToInt32(lstPackageNames.SelectedItem.Value),
                    txtPackagePrefix.Text.Trim(), txtPackageSuffix.Text.Trim(), txtCustomerService.Text.Trim(),
                    txtInternetStmt.Text.Trim(),
                    txtTransFee.Text.Trim(), txtDRQP.Text.Trim(), txtDRQNP.Text.Trim(), txtDRMQ.Text.Trim(),
                    txtDRNQ.Text.Trim(), txtDRQD.Text.Trim(), txtChargebackFee.Text.Trim(), txtRetrievalFee.Text.Trim(),
                    txtVoiceAuth.Text.Trim(), txtBatchHeader.Text.Trim(), txtAVS.Text.Trim(), txtMonMin.Text.Trim(),
                    txtNBCTransFee.Text.Trim(), lstAnnualFee.SelectedItem.Value, txtWirelessAccess.Text.Trim(),
                    txtWirelessTransFee.Text.Trim(), txtApplicationFee.Text.Trim(), txtSetupFee.Text.Trim(),
                    lstGatewayNames.SelectedItem.Text, txtGWTransFee.Text.Trim(), txtGWMonthlyFee.Text.Trim(),
                    txtGWSetupFee.Text.Trim(), txtDebitMonFee.Text.Trim(), txtDebitTransFee.Text.Trim(), 
                    txtCGDiscRate.Text.Trim(), txtCGMonFee.Text.Trim(), txtCGMonMin.Text.Trim(), 
                    txtCGTransFee.Text.Trim(), txtGCMonFee.Text.Trim(), txtGCTransFee.Text.Trim(), 
                    txtEBTMonFee.Text.Trim(), txtEBTTransFee.Text.Trim());
                bool bretval = UpdatePack.UpdateOtherProcessingPackage(Convert.ToInt32(lstPackageNames.SelectedItem.Value), 
                    chkApplyInterchange.Checked, chkBillAssessment.Checked, txtRollingReserve.Text.Trim());
                if ((retVal) && (bretval))
                {
                    DisplayMessage("Package Updated Successfully");
                    //Add log
                    PartnerLogBL LogData = new PartnerLogBL();
                    if (lstPackageNames.SelectedItem.Text == txtPackagePrefix.Text.Trim() + txtPackageSuffix.Text.Trim())
                        retVal = LogData.InsertLogRates(Convert.ToInt32(lstPackageNames.SelectedItem.Value),
                            Convert.ToInt32(Session["AffiliateID"]), "Package (" + lstPackageNames.SelectedItem.Text + ") Modified.");
                    else if (lstPackageNames.SelectedItem.Text != txtPackagePrefix.Text.Trim() + txtPackageSuffix.Text.Trim())
                        retVal = LogData.InsertLogRates(Convert.ToInt32(lstPackageNames.SelectedItem.Value),
                            Convert.ToInt32(Session["AffiliateID"]), "Package (" + lstPackageNames.SelectedItem.Text + ") modified and renamed to (" + txtPackagePrefix.Text.Trim() + txtPackageSuffix.Text.Trim() + ").");
                }
                else
                    DisplayMessage("Error Updating Package");

            }
            else
                DisplayMessage(errMessage);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error updating package information.");
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

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnDelYes_Click(object sender, EventArgs e)
    {
        try
        {
			int retVal = 0;
            if (lstPackageNames.SelectedItem.Value == "0")
				DisplayMessage("This package is used for clearing rates and cannot be deleted.");
			else
			{
				PackageBL delPackage = new PackageBL();
				retVal = delPackage.DeletePackage(Convert.ToInt32(lstPackageNames.SelectedItem.Value));

                if (retVal == 1) //Package was deleted
                {
                    DisplayMessage("Package deleted successfully");
                    //Add log
                    PartnerLogBL LogData = new PartnerLogBL();
                    LogData.InsertLogRates(Convert.ToInt32(lstPackageNames.SelectedItem.Value),
                            Convert.ToInt32(Session["AffiliateID"]), "Package (" + lstPackageNames.SelectedItem.Text + ") deleted.");
                }
                else //package not deleted but marked for deletetion
                {
                    DisplayMessage("Package will be marked inactive but cannot be deleted because it is a Default Package or assigned to an App ID.");
                    //Add log
                    PartnerLogBL LogData = new PartnerLogBL();
                    LogData.InsertLogRates(Convert.ToInt32(lstPackageNames.SelectedItem.Value),
                            Convert.ToInt32(Session["AffiliateID"]), "Package (" + lstPackageNames.SelectedItem.Text + ") marked for deletion.");
                }

			}

            
            pnlDeletePackage.Visible = false;
            Populate();
      
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error deleting package. Note: This package has been assigned as a default package.");
        }
    }

    protected void btnDelNo_Click(object sender, EventArgs e)
    {
        pnlDeletePackage.Visible = false;
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

    protected void lstGatewayNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadUpperLowerBoundsGW(lstGatewayNames.SelectedItem.Text);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error loading gateway bounds");
        }
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
                //LoadUpperLowerBounds(lstProcessorNames.SelectedItem.Value.ToString().Trim(), strCardPresent);
                LoadUpperLowerBounds(lstProcessorNames.SelectedItem.Value.ToString().Trim(), strCardPresent);
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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Populate();
    }

    public string ValidateRates()
    {
        string strError = "";
        //Customer Service
        if (CustServLow.Value != "")
        {
            if ((txtCustomerService.Text == "") || (Convert.ToDecimal(txtCustomerService.Text) < Convert.ToDecimal(CustServLow.Value)))           
                strError += "Enter at least $" + CustServLow.Value + " for Customer Service Fee." + "<BR/>";           
        }

        if (InternetStmtLow.Value != "")
        {
            if ((txtCustomerService.Text == "") || (Convert.ToDecimal(txtCustomerService.Text) < Convert.ToDecimal(InternetStmtLow.Value)))
                strError += "Enter at least $" + InternetStmtLow.Value + " for Customer Service Fee." + "<BR/>";
        }

        //Monthly Minimum
        if (MonMinLow.Value != "")
        {
            if ((txtMonMin.Text == "") || (Convert.ToDecimal(txtMonMin.Text) < Convert.ToDecimal(MonMinLow.Value)))
                strError += "Enter at least $" + MonMinLow.Value + " for Monthly Minimum." + "<BR/>";      
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
                strError += "Enter at least " + DiscRateQualPresLow.Value + "% for Disc Rate Qual Pres." + "<BR/>";            
        }
        //DiscRateQualNP
        if (DiscRateQualNPLow.Value != "")
        {
            if ((txtDRQNP.Text == "") || (Convert.ToDecimal(txtDRQNP.Text) < Convert.ToDecimal(DiscRateQualNPLow.Value)))
                strError += "Enter at least " + DiscRateQualNPLow.Value + "% for Disc Rate Qual NP." + "<BR/>";
        }

        //If IMS and Card Present overwrite the Mid Qual Low to be the Qual Pres PLUS the specified Mid Qual Step in the database 
        if ((rdbCP.Checked) && (lstProcessorNames.SelectedItem.Text == "Intuit Payment Solutions"))
        {
            if ((txtDRQP.Text != "") && (DiscRateMidQualStep.Value != ""))
                DiscRateMidQualLow.Value = Convert.ToString(Convert.ToDecimal(txtDRQP.Text) + Convert.ToDecimal(DiscRateMidQualStep.Value));
        }
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
            {
                strError += "Enter at least " + DiscRateNonQualLow.Value + "% for Disc Rate Non Qual." + "<BR/>";
            }
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
            {
                strError += "Enter at least $" + ChargebackFeeLow.Value + " for Chargeback Fee." + "<BR/>";
            }
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
                strError += "Enter at least $" + BatchHeaderLow.Value + " for Batch Header Fee." + "<BR/>";            
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
            {
                strError += "Enter at least $" + WirelessTransFeeLow.Value + " for Wireless Trans Fee or leave blank if it does not apply." + "<BR/>";
            }
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
                    strError += "Enter at least $" + GatewayTransFeeLow.Value + " for Gateway Transaction Fee." + "<BR/>";                
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
            {
                strError += "Enter at least $" + DebitTransFeeLow.Value + " for Debit Trans Fee or leave blank if it does not apply." + "<BR/>";
            }
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
                strError += "Enter at least $" + CGMonFeeLow.Value + " for Check Guarantee Mon Fee or leave blank if it does not apply." + "<BR/>";            
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
                strError += "Enter at least $" + GCMonFeeLow.Value + " for Gift Card Mon Min or leave blank if it does not apply." + "<BR/>";         
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
                strError += "Enter at least $" + EBTMonFeeLow.Value + " for EBT Mon Min or leave blank if it does not apply." + "<BR/>";            
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
        //lstGatewayNames.SelectedValue = lstGatewayNames.Items.FindByText("None").Value;
    }


  
}
