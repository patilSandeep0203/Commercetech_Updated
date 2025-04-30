using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLayer;
using DLPartner;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AnetMerchantBoarding.ANetAPI;
using AnetMerchantBoarding;

public partial class UploadAnetGatewayXML : System.Web.UI.Page
{
    
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Employee"))
                Page.MasterPageFile = "../Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "../Admin.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            //This page is accessible only by Admins
            if (!User.IsInRole("Admin"))
                Response.Redirect("~/logout.aspx");
        }

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
        
            txtLookup.Focus();
        }
    }//end page load

    //This function handles submit button click event
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblDownload.Visible = false;
            lblError.Visible = false;
            pnlPlatform.Visible = false;
            if (Page.IsValid)
            {
                grdPDF.Visible = true;
                PDFBL ActPDF = new PDFBL();
                DataSet ds = ActPDF.GetAuthnetSummaryACT(lstLookup.SelectedItem.Text, txtLookup.Text.Trim());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grdPDF.DataSource = ds;
                    grdPDF.DataBind();
                }//end if count not 0
                else
                {
                    DisplayMessage("No records found.");
                    grdPDF.Visible = false;
                }
            }//end if page is valid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error retrieving data from ACT!");
        }
    }//end submit button click

    //This function handles grid view button click event
    protected void grdPDF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "UploadGateway")
            {
                lblError.Visible = false;
                lblError.Text = "";
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdPDF.Rows[index];
                System.Guid ContactID = new Guid(Server.HtmlDecode(grdRow.Cells[1].Text));

                if (ContactID.ToString() != "")
                {
                    pnlPlatform.Visible = true;
                    XMLBL Authnet = new XMLBL();
                    PartnerDS.ACTAuthnetXMLDataTable dt_XML = Authnet.GetAuthnetXML(ContactID);

                    //new code
                    string dat = dt_XML[0].ProcessorID.ToString();
                    string dat1 = dt_XML[0].GatewayFee.ToString();
                    DisplayMessage(dat);
                    DisplayMessage(dat1);
                    //
                    if (dt_XML.Rows.Count > 0)
                    {
                        //if ((dt_XML[0].DiscountRatePres.ToString().Trim() != "") && (Convert.ToInt32(dt_XML[0].PctInt.ToString().Trim()) > 0))
                            //DisplayMessage("Error - Cannot be both Retail and Internet account. Please check Discount Rate Present and Internet Percentage.");
                        //else 
                        if ((dt_XML[0].DiscountRatePres.ToString().Trim() == "") || (dt_XML[0].DiscountRatePres.ToString().Trim() == "0.00"))
                        {
                            lblDeviceList.Visible = false;
                            lstDeviceList.Visible = false;
                            lblRgstState.Visible = false;
                            txtRgstState.Visible = false;
                            lblRgstZip.Visible = false;
                            txtRgstZip.Visible = false;
                            lblRgstCountry.Visible = false;
                            txtRgstCountry.Visible = false;
                            pnlCNPWaive.Visible = true;
                            pnlCPWaive.Visible = false;
                        }
                        else
                        {
                            lblDeviceList.Visible = true;
                            lstDeviceList.Visible = true;
                            lblRgstState.Visible = true;
                            txtRgstState.Visible = true;
                            lblRgstZip.Visible = true;
                            txtRgstZip.Visible = true;
                            lblRgstCountry.Visible = true;
                            txtRgstCountry.Visible = true;
                            pnlCNPWaive.Visible = false;
                            pnlCPWaive.Visible = true;
                        }
                    }

                    string platform = Server.HtmlDecode(grdRow.Cells[6].Text).ToString();
                    if ((platform.Contains("1")) || (platform.Contains("2")) ||
                        (platform.Contains("4")) || (platform.Contains("7")) ||
                        (platform.Contains("15")) || (platform.Contains("11")))
                    {
                        lstPlatform.SelectedValue = lstPlatform.Items.FindByValue(Server.HtmlDecode(grdRow.Cells[6].Text)).Value;
                        ACTDataBL PlatformInfo = new ACTDataBL();
                        PartnerDS.ACTAuthnetPlatformDataTable dt = PlatformInfo.GetAuthnetPlatform(ContactID);
                        if (dt.Rows.Count > 0)
                        {
                            txtAgentBankNumber.Text = dt[0].AgentBankIDNumber.ToString().Trim();
                            txtAgentChainNumber.Text = dt[0].AgentChainNumber.ToString().Trim();
                            txtMCCCode.Text = dt[0].MCCCategoryCode.ToString().Trim();
                            //txtLoginID.Text = "";// dt[0].LoginID.ToString().Trim();
                            txtMerchantID.Text = dt[0].MerchantID.ToString().Trim();
                            txtStoreNumber.Text = dt[0].StoreNumber.ToString().Trim();
                            txtVisaMasterNumber.Text = dt[0].MerchantNumber.ToString().Trim();
                            txtBINNumber.Text = dt[0].BankIDNumber.ToString().Trim();
                        }//end if count not 0
                        DisablePlatformFields();
                        lblContactID.Text = ContactID.ToString();

                        //XMLBL Authnet = new XMLBL();
                        //string result = CreateAnetXML(ContactID);
                        //DisplayMessage(result);
                    }
                    else
                        DisplayMessage("Invalid Platform for Authorize.net XML upload.");
                }
             }//end if command name
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "RowCommand - " + err.Message);
            DisplayMessage(err.Message);
        }
    }//end function grid view button click

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected resellerCreateMerchantRequest ResellerCreateMerchantRequestObj(System.Guid ContactID)
    {
        //try
        //{
        XMLBL Authnet = new XMLBL();
        PartnerDS.ACTAuthnetXMLDataTable dt = Authnet.GetAuthnetXML(ContactID);
        //if (dt.Rows.Count > 0)
        //{
        InputParams inputParams = new InputParams();
        resellerCreateMerchantRequest request = new resellerCreateMerchantRequest();
        request.resellerAuthentication = Utils.CreateResellerAuthentication(inputParams);

        merchantType merchant = new merchantType();

        #region Business Info
        merchant.name = dt[0].COMPANYNAME.ToString().Trim();
        merchant.referenceId = dt[0].ReferenceID.ToString().Trim();
        merchant.phone = dt[0].Phone.ToString().Trim();
        if (dt[0].Fax.ToString().Trim() != "")
            merchant.fax = dt[0].Fax.ToString().Trim();
        merchant.email = dt[0].Email.ToString().Trim();

        addressType businessAddress = new addressType();
        businessAddress.streetAddress = dt[0].Address.ToString().Trim();
        businessAddress.city = dt[0].CITY.ToString().Trim();
        businessAddress.state = dt[0].STATE.ToString().Trim();
        businessAddress.zip = dt[0].Zip.ToString().Trim();
        businessAddress.country = dt[0].COUNTRYNAME.ToString().Trim();
        merchant.businessAddress = businessAddress;

        businessInfoType businessInfo = new businessInfoType();
        if (dt[0].BusinessType.ToString().Trim() == "SoleProprietorShip")
            businessInfo.businessType = businessTypeEnum.SoleProprietorShip;
        else if (dt[0].BusinessType.ToString().Trim() == "PartnerShip")
            businessInfo.businessType = businessTypeEnum.PartnerShip;
        else if (dt[0].BusinessType.ToString().Trim() == "Corporation")
            businessInfo.businessType = businessTypeEnum.Corporation;
        else if (dt[0].BusinessType.ToString().Trim() == "NonProfit")
            businessInfo.businessType = businessTypeEnum.NonProfit;
        else if (dt[0].BusinessType.ToString().Trim() == "Trust")
            businessInfo.businessType = businessTypeEnum.Trust;
        businessInfo.taxId = dt[0].TaxID.ToString().Trim();
        businessInfo.ageOfBusiness = Convert.ToInt32(dt[0].AgeOfBusiness.ToString().Trim());
        businessInfo.productsSold = dt[0].ProductsSold.ToString().Trim();
        businessInfo.sicCode = dt[0].SICCode.ToString().Trim();

        //if ((dt[0].DiscountRatePres.ToString().Trim() != "") && (Convert.ToInt32(dt[0].PctInt.ToString().Trim()) > 0))
        //    return "Error - Cannot be both Retail and Internet account. Please check Discount Rate Present and Internet Percentage.";
        //else 
        if (dt[0].DiscountRatePres.ToString().Trim() != "")
            businessInfo.marketTypeId = 2; //Retail
        else if (Convert.ToInt32(dt[0].PctInt.ToString().Trim()) > 0)
            businessInfo.marketTypeId = 0; //ecommerce
        else
            businessInfo.marketTypeId = 1; //moto

        merchant.businessInfo = businessInfo;

        ownerInfoType ownerInfo = new ownerInfoType();
        ownerInfo.name = dt[0].OwnerName.ToString().Trim();
        ownerInfo.title = dt[0].Title;
        if (dt[0].P1Phone.ToString().Trim() != "")
            ownerInfo.phone = dt[0].P1Phone.ToString().Trim();

        if (dt[0].BusinessType.ToString().Trim() == "NonProfit")
            ownerInfo.ssn = dt[0].TaxID.ToString().Trim();
        else
            ownerInfo.ssn = dt[0].P1SSN.ToString().Trim();

        //if principal information is not available for example when business is non-profit, do not submit Address info
        if (dt[0].P1Address.ToString().Trim() != "")
        {
            addressType ownerAddress = new addressType();
            ownerAddress.streetAddress = dt[0].P1Address.ToString().Trim();
            ownerAddress.city = dt[0].P1City.ToString().Trim();
            ownerAddress.state = dt[0].P1State.ToString().Trim();
            ownerAddress.zip = dt[0].P1Zip.ToString().Trim();
            ownerAddress.country = dt[0].P1Country.ToString().Trim();
            ownerInfo.address = ownerAddress;
        }
        
        merchant.ownerInfo = ownerInfo;

        billingInfoType billingInfo = new billingInfoType();
        billingInfo.nameOnBankAccount = dt[0].OwnerName.ToString().Trim();
        billingInfo.bankAccountType = resellerBankAccountTypeEnum.Checking;
        billingInfo.bankAccountOwnerType = bankAccountOwnerTypeEnum.Business;
        billingInfo.bankABACode = dt[0].BankABARoutingNumber.ToString().Trim();
        billingInfo.bankAccountNumber = dt[0].BankAccountNumber.ToString().Trim();
        billingInfo.bankName = dt[0].BankName.ToString().Trim();
        billingInfo.bankCity = dt[0].BankCity.ToString().Trim();
        billingInfo.bankState = dt[0].BankState.ToString().Trim();
        billingInfo.bankZip = dt[0].BankZip.ToString().Trim();
        merchant.billingInfo = billingInfo;

        #endregion

        #region Processor and Payment Info

        paymentGroupingType paygroup = new paymentGroupingType();

        paygroup.paymentTypes = new string[7];
        int i = 0;
        if (dt[0].Visa.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "V";
            i++;
        }
        if (dt[0].Mastercard.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "M";
            i++;
        }
        if (dt[0].AMEX.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "A";
            i++;
        }
        if (dt[0].Discover.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "D";
            i++;
        }
        if (dt[0].Diners.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "C";
            i++;
        }
        if (dt[0].Enroute.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "E";
            i++;
        }
        if (dt[0].JCB.ToString().Trim().ToLower() == "yes")
        {
            paygroup.paymentTypes[i] = "J";
            i++;
        }

        processorType processor = new processorType();
        processor.id = Convert.ToInt32(dt[0].ProcessorID.ToString().Trim());

        //FDC Nashville or FDMS Nashville
        if (processor.id == 2)
        {
            processor.procConfig = new fieldConfigType[2];
            processor.displayName = "FDC";
            processor.procConfig[0] = new fieldConfigType();
            processor.procConfig[0].fieldName = "MerchID";
            processor.procConfig[0].fieldValue = txtMerchantID.Text;

            processor.procConfig[1] = new fieldConfigType();
            processor.procConfig[1].fieldName = "TermID";
            processor.procConfig[1].fieldValue = txtTerminalID.Text;//add terminal id input
        }

        //FDCO or FDMS Omaha
        if (processor.id == 15)
        {
            processor.procConfig = new fieldConfigType[1];
            processor.displayName = "FDCO";
            processor.procConfig[0] = new fieldConfigType();
            processor.procConfig[0].fieldName = "FDCOMerchantID";
            processor.procConfig[0].fieldValue = txtVisaMasterNumber.Text;
        }

        //Global Payments or Global Payments East Platform
        if (processor.id == 7)
        {
            processor.procConfig = new fieldConfigType[2];
            processor.displayName = "Global Payments";
            processor.procConfig[0] = new fieldConfigType();
            processor.procConfig[0].fieldName = "AcquirerInstitutionID";
            processor.procConfig[0].fieldValue = txtBINNumber.Text;

            processor.procConfig[1] = new fieldConfigType();
            processor.procConfig[1].fieldName = "CardAcceptorID";
            processor.procConfig[1].fieldValue = txtMerchantID.Text;
        }

        //Nova or Elavon
        if (processor.id == 1)
        {
            processor.procConfig = new fieldConfigType[2];
            processor.displayName = "Nova";
            processor.procConfig[0] = new fieldConfigType();
            processor.procConfig[0].fieldName = "BankNumber";
            processor.procConfig[0].fieldValue = txtBINNumber.Text;

            processor.procConfig[1] = new fieldConfigType();
            processor.procConfig[1].fieldName = "TermID";
            processor.procConfig[1].fieldValue = txtTerminalID.Text;//add terminal id input
        }

        //Paymentech (Terminal Capture) or Chase Paymentech Tampa Platform
        if (processor.id == 11)
        {
            processor.procConfig = new fieldConfigType[3];
            processor.displayName = "Paymentech (Terminal Capture)";
            processor.procConfig[0] = new fieldConfigType();
            processor.procConfig[0].fieldName = "ClientNumber";
            processor.procConfig[0].fieldValue = txtStoreNumber.Text;

            processor.procConfig[1] = new fieldConfigType();
            processor.procConfig[1].fieldName = "MerchantNumber";
            processor.procConfig[1].fieldValue = dt[0].MerchantID.ToString().Trim();

            processor.procConfig[2] = new fieldConfigType();
            processor.procConfig[2].fieldName = "TerminalNumber";
            processor.procConfig[2].fieldValue = txtTerminalID.Text;//add terminal id input
        }

        //Vital or TSYS Acquiring Solutions
        if (processor.id == 4)
        {
            processor.procConfig = new fieldConfigType[7];
            processor.displayName = "Vital";
            processor.procConfig[0] = new fieldConfigType();
            processor.procConfig[0].fieldName = "AcquirerBIN";
            processor.procConfig[0].fieldValue = txtBINNumber.Text;

            processor.procConfig[1] = new fieldConfigType();
            processor.procConfig[1].fieldName = "AgentBankNumber";
            processor.procConfig[1].fieldValue = txtAgentBankNumber.Text;

            processor.procConfig[2] = new fieldConfigType();
            processor.procConfig[2].fieldName = "AgentChainNumber";
            processor.procConfig[2].fieldValue = txtAgentChainNumber.Text;

            processor.procConfig[3] = new fieldConfigType();
            processor.procConfig[3].fieldName = "CategoryCode";
            processor.procConfig[3].fieldValue = txtMCCCode.Text;

            processor.procConfig[4] = new fieldConfigType();
            processor.procConfig[4].fieldName = "MerchantNumber";
            processor.procConfig[4].fieldValue = txtMerchantID.Text;

            processor.procConfig[5] = new fieldConfigType();
            processor.procConfig[5].fieldName = "StoreNumber";
            processor.procConfig[5].fieldValue = txtStoreNumber.Text;

            processor.procConfig[6] = new fieldConfigType();
            processor.procConfig[6].fieldName = "TerminalNumber";
            processor.procConfig[6].fieldValue = txtTerminalID.Text;//add terminal id input
        }

        processor.acquirerId = Convert.ToInt32(dt[0].Acquirer.ToString().Trim());
        processor.acquirerIdSpecified = true;
        paygroup.processor = processor;
        merchant.paymentGrouping = paygroup;

        #endregion

        #region SalesRep Info

        salesRepType salesRep = new salesRepType();
        salesRep.salesRepName = dt[0].SalesRepName.ToString().Trim();
        salesRep.salesRepId = dt[0].SalesRepID.ToString().Trim();
        salesRep.salesRepCommission = 0.00M;
        merchant.salesRep = salesRep;

        #endregion

        #region Service Type

        List<serviceType> services = new List<serviceType>();

        #region Service Type - Gateway
        serviceType svc1 = new serviceType();
        svc1.id = 8; //gateway
        List<serviceBuyRateProgramType> buyratePrograms = new List<serviceBuyRateProgramType>();
        serviceBuyRateProgramsType brps = new serviceBuyRateProgramsType();

        serviceBuyRateProgramType brp = new serviceBuyRateProgramType();
        if ((businessInfo.marketTypeId == 1) || (businessInfo.marketTypeId == 0))
        {
            if (lstCNPWaive.SelectedItem.Text.ToString().Trim() == "No")
                brp.id = 6949; // moto and ecommerce buyrateprogram; SetupFee $0.00 - GWFee $5.00 - TransFee $0.05 - Threshold 1
            else
                brp.id = 6967; // moto and ecommerce Alternate Buy Rates - SetupFee $0.00 - GWFee $5.00 - TransFee $0.05 - Threshold 251
        }
        else
        {
            if (lstCPWaive.SelectedItem.Text.ToString().Trim() == "No")
                brp.id = 7229; // retail buyrateprogram; SetupFee $0.00 - GWFee $8.00 - TransFee $0.04 - Threshold 100
            else
                brp.id = 7252; // retail buyrateprogram; SetupFee $0.00 - GWFee $20.00 - TransFee $0.02 - Threshold 1001
        }
        brp.fees = new feeType[3];
        brp.fees[0] = new feeType();
        brp.fees[0].id = 11; //Gateway Monthly Fee
        brp.fees[0].singleTiered = true;
        brp.fees[0].tiers = new feeTierType[1];
        brp.fees[0].tiers[0] = new feeTierType();
        brp.fees[0].tiers[0].sellRate = Convert.ToDecimal(dt[0].GatewayFee.ToString().Trim());

        brp.fees[1] = new feeType();
        brp.fees[1].id = 19; //Credit Card Per-Transaction Fee
        brp.fees[1].singleTiered = true;
        brp.fees[1].tiers = new feeTierType[1];
        brp.fees[1].tiers[0] = new feeTierType();
        brp.fees[1].tiers[0].sellRate = Convert.ToDecimal(dt[0].CreditCardTransFee.ToString().Trim());

        brp.fees[2] = new feeType();
        brp.fees[2].id = 21; //Batch Fee - Optional (Same as GW Transaction fee)
        brp.fees[2].singleTiered = true;
        brp.fees[2].tiers = new feeTierType[1];
        brp.fees[2].tiers[0] = new feeTierType();
        brp.fees[2].tiers[0].sellRate = Convert.ToDecimal(dt[0].CreditCardTransFee.ToString().Trim());

        buyratePrograms.Add(brp);
        brps.serviceBuyRateProgram = buyratePrograms.ToArray();
        svc1.Item = brps;
        services.Add(svc1);
        #endregion

        #region Service Type - eCheck.Net
        //The eCheck service can only be used for market types of 0 (eCommerce) or 1 (MOTO). 
        //These two market types always have the same buy rate packages.
        //Two buy rate packages must be specified, Standard rates and preferred rates. 
        //UW determine which rate package will be used based off of merchant's market type and service.
        if ((businessInfo.marketTypeId == 1) || (businessInfo.marketTypeId == 0))
        {
            serviceType svc2 = new serviceType();
            svc2.id = 4; //eCheck.Net
            List<serviceBuyRateProgramType> eCheckBuyratePrograms = new List<serviceBuyRateProgramType>();
            serviceBuyRateProgramsType eCheckBrps = new serviceBuyRateProgramsType();

            serviceBuyRateProgramType eCheckBrp1 = new serviceBuyRateProgramType();
            eCheckBrp1.id = 23857; //Standard eCheck.Net Buy Rates                   
            eCheckBrp1.fees = new feeType[7];
            eCheckBrp1.fees[0] = new feeType();
            eCheckBrp1.fees[0].id = 3; //eCheck.Net Setup Fee
            eCheckBrp1.fees[0].singleTiered = true;
            eCheckBrp1.fees[0].tiers = new feeTierType[1];
            eCheckBrp1.fees[0].tiers[0] = new feeTierType();
            eCheckBrp1.fees[0].tiers[0].sellRate = 0.00M;

            eCheckBrp1.fees[1] = new feeType();
            eCheckBrp1.fees[1].id = 8; //eCheck.Net Chargeback Fee
            eCheckBrp1.fees[1].singleTiered = true;
            eCheckBrp1.fees[1].tiers = new feeTierType[1];
            eCheckBrp1.fees[1].tiers[0] = new feeTierType();
            eCheckBrp1.fees[1].tiers[0].sellRate = 25.00M;

            eCheckBrp1.fees[2] = new feeType();
            eCheckBrp1.fees[2].id = 7; //eCheck.Net Returned Item Fee
            eCheckBrp1.fees[2].singleTiered = true;
            eCheckBrp1.fees[2].tiers = new feeTierType[1];
            eCheckBrp1.fees[2].tiers[0] = new feeTierType();
            eCheckBrp1.fees[2].tiers[0].sellRate = 3.00M;

            eCheckBrp1.fees[3] = new feeType();
            eCheckBrp1.fees[3].id = 9; //eCheck.Net Batch Fee
            eCheckBrp1.fees[3].singleTiered = true;
            eCheckBrp1.fees[3].tiers = new feeTierType[1];
            eCheckBrp1.fees[3].tiers[0] = new feeTierType();
            eCheckBrp1.fees[3].tiers[0].sellRate = 0.35M;

            eCheckBrp1.fees[4] = new feeType();
            eCheckBrp1.fees[4].id = 6; //eCheck.Net Minimum Monthly Fee
            eCheckBrp1.fees[4].singleTiered = true;
            eCheckBrp1.fees[4].tiers = new feeTierType[1];
            eCheckBrp1.fees[4].tiers[0] = new feeTierType();
            eCheckBrp1.fees[4].tiers[0].sellRate = 10.00M;

            eCheckBrp1.fees[5] = new feeType();
            eCheckBrp1.fees[5].id = 5; //eCheck.Net Per-Transaction Fee
            eCheckBrp1.fees[5].singleTiered = false;
            eCheckBrp1.fees[5].tiers = new feeTierType[1];
            eCheckBrp1.fees[5].tiers[0] = new feeTierType();
            //eCheckBrp1.fees[5].tiers[0].idSpecified = true;
            //eCheckBrp1.fees[5].tiers[0].id = 93210;
            eCheckBrp1.fees[5].tiers[0].sellRate = 0.35M;
            /*eCheckBrp1.fees[5].tiers[1] = new feeTierType();
            eCheckBrp1.fees[5].tiers[1].idSpecified = true;
            eCheckBrp1.fees[5].tiers[1].id = 93211;
            eCheckBrp1.fees[5].tiers[1].sellRate = 0.30M;
            eCheckBrp1.fees[5].tiers[2] = new feeTierType();
            eCheckBrp1.fees[5].tiers[2].idSpecified = true;
            eCheckBrp1.fees[5].tiers[2].id = 93212;
            eCheckBrp1.fees[5].tiers[2].sellRate = 0.30M;
            eCheckBrp1.fees[5].tiers[3] = new feeTierType();
            eCheckBrp1.fees[5].tiers[3].idSpecified = true;
            eCheckBrp1.fees[5].tiers[3].id = 93213;
            eCheckBrp1.fees[5].tiers[3].sellRate = 0.30M;*/

            eCheckBrp1.fees[6] = new feeType();
            eCheckBrp1.fees[6].id = 4; //eCheck.Net Discount Rate
            eCheckBrp1.fees[6].singleTiered = false;
            eCheckBrp1.fees[6].tiers = new feeTierType[4];
            eCheckBrp1.fees[6].tiers[0] = new feeTierType();
            eCheckBrp1.fees[6].tiers[0].idSpecified = true;
            eCheckBrp1.fees[6].tiers[0].id = 801907;
            eCheckBrp1.fees[6].tiers[0].sellRate = 0.0075M;
            eCheckBrp1.fees[6].tiers[1] = new feeTierType();
            eCheckBrp1.fees[6].tiers[1].idSpecified = true;
            eCheckBrp1.fees[6].tiers[1].id = 801908;
            eCheckBrp1.fees[6].tiers[1].sellRate = 0.0070M;
            eCheckBrp1.fees[6].tiers[2] = new feeTierType();
            eCheckBrp1.fees[6].tiers[2].idSpecified = true;
            eCheckBrp1.fees[6].tiers[2].id = 801909;
            eCheckBrp1.fees[6].tiers[2].sellRate = 0.0060M;
            eCheckBrp1.fees[6].tiers[3] = new feeTierType();
            eCheckBrp1.fees[6].tiers[3].idSpecified = true;
            eCheckBrp1.fees[6].tiers[3].id = 801910;
            eCheckBrp1.fees[6].tiers[3].sellRate = 0.0055M;

            eCheckBuyratePrograms.Add(eCheckBrp1);

            serviceBuyRateProgramType eCheckBrp2 = new serviceBuyRateProgramType();
            eCheckBrp2.id = 23858; //Preferred eCheck.Net Buy Rates                   
            eCheckBrp2.fees = new feeType[7];
            eCheckBrp2.fees[0] = new feeType();
            eCheckBrp2.fees[0].id = 3; //eCheck.Net Setup Fee
            eCheckBrp2.fees[0].singleTiered = true;
            eCheckBrp2.fees[0].tiers = new feeTierType[1];
            eCheckBrp2.fees[0].tiers[0] = new feeTierType();
            eCheckBrp2.fees[0].tiers[0].sellRate = 0.00M;

            eCheckBrp2.fees[1] = new feeType();
            eCheckBrp2.fees[1].id = 8; //eCheck.Net Chargeback Fee
            eCheckBrp2.fees[1].singleTiered = true;
            eCheckBrp2.fees[1].tiers = new feeTierType[1];
            eCheckBrp2.fees[1].tiers[0] = new feeTierType();
            eCheckBrp2.fees[1].tiers[0].sellRate = 25.00M;

            eCheckBrp2.fees[2] = new feeType();
            eCheckBrp2.fees[2].id = 7; //eCheck.Net Returned Item Fee
            eCheckBrp2.fees[2].singleTiered = true;
            eCheckBrp2.fees[2].tiers = new feeTierType[1];
            eCheckBrp2.fees[2].tiers[0] = new feeTierType();
            eCheckBrp2.fees[2].tiers[0].sellRate = 3.00M;

            eCheckBrp2.fees[3] = new feeType();
            eCheckBrp2.fees[3].id = 9; //eCheck.Net Batch Fee
            eCheckBrp2.fees[3].singleTiered = true;
            eCheckBrp2.fees[3].tiers = new feeTierType[1];
            eCheckBrp2.fees[3].tiers[0] = new feeTierType();
            eCheckBrp2.fees[3].tiers[0].sellRate = 0.30M;

            eCheckBrp2.fees[4] = new feeType();
            eCheckBrp2.fees[4].id = 6; //eCheck.Net Minimum Monthly Fee
            eCheckBrp2.fees[4].singleTiered = true;
            eCheckBrp2.fees[4].tiers = new feeTierType[1];
            eCheckBrp2.fees[4].tiers[0] = new feeTierType();
            eCheckBrp2.fees[4].tiers[0].sellRate = 10.00M;

            eCheckBrp2.fees[5] = new feeType();
            eCheckBrp2.fees[5].id = 5; //eCheck.Net Per-Transaction Fee
            eCheckBrp2.fees[5].singleTiered = true;
            eCheckBrp2.fees[5].tiers = new feeTierType[1];
            eCheckBrp2.fees[5].tiers[0] = new feeTierType();
            eCheckBrp2.fees[5].tiers[0].sellRate = 0.5M;

            eCheckBrp2.fees[6] = new feeType();
            eCheckBrp2.fees[6].id = 4; //eCheck.Net Discount Rate
            eCheckBrp2.fees[6].singleTiered = true;
            eCheckBrp2.fees[6].tiers = new feeTierType[1];
            eCheckBrp2.fees[6].tiers[0] = new feeTierType();
            eCheckBrp2.fees[6].tiers[0].sellRate = 0.00M;

            eCheckBuyratePrograms.Add(eCheckBrp2);

            eCheckBrps.serviceBuyRateProgram = eCheckBuyratePrograms.ToArray();
            svc2.Item = eCheckBrps;
            services.Add(svc2);
        }
        #endregion

        merchant.services = services.ToArray();

        #endregion

        #region Device Info for CP merchants
        //if retail account add device info
        if (businessInfo.marketTypeId == 2)
        {
            List<deviceInfoType> deviceList = new List<deviceInfoType>();
            deviceInfoType device = new deviceInfoType();
            if (lstDeviceList.SelectedItem.Text == "Unconfigured")
                device.deviceType = deviceTypeEnum.Unconfigured;
            else if (lstDeviceList.SelectedItem.Text == "UnattendedTerminal")
                device.deviceType = deviceTypeEnum.UnattendedTerminal;
            else if (lstDeviceList.SelectedItem.Text == "SelfServiceTerminal")
                device.deviceType = deviceTypeEnum.SelfServiceTerminal;
            else if (lstDeviceList.SelectedItem.Text == "ElectronicCashRegister")
                device.deviceType = deviceTypeEnum.ElectronicCashRegister;
            else if (lstDeviceList.SelectedItem.Text == "PCBasedTerminal")
                device.deviceType = deviceTypeEnum.PCBasedTerminal;
            else if (lstDeviceList.SelectedItem.Text == "Airpay")
                device.deviceType = deviceTypeEnum.Airpay;
            else if (lstDeviceList.SelectedItem.Text == "WirelessPOS")
                device.deviceType = deviceTypeEnum.WirelessPOS;
            else if (lstDeviceList.SelectedItem.Text == "WebSite")
                device.deviceType = deviceTypeEnum.WebSite;
            else if (lstDeviceList.SelectedItem.Text == "DialTerminal")
                device.deviceType = deviceTypeEnum.DialTerminal;
            else if (lstDeviceList.SelectedItem.Text == "VirtualTerminal")
                device.deviceType = deviceTypeEnum.VirtualTerminal;
            else if (lstDeviceList.SelectedItem.Text == "StoreController")
                device.deviceType = deviceTypeEnum.StoreController;

            device.state = txtRgstState.Text.ToString().Trim();
            device.zip = txtRgstZip.Text.ToString().Trim();
            device.country = txtRgstCountry.Text.ToString().Trim();
            deviceList.Add(device);
            merchant.deviceList = deviceList.ToArray();
        }

        #endregion

        request.merchant = merchant;

        return request;
        //}
        /*else
        {
            return "Authnet data not found";
        }*/
    }        /*catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "CreateAnetXML - " + err.Message);
            //return err.Message;
        }*/

    public string CreateAnetXML(System.Guid ContactID)
    {
        try
        {
            object request = null;
            request = ResellerCreateMerchantRequestObj(ContactID);
            InputParams inputParams = new InputParams();
            ANetApiResponse apiResponse = (ANetApiResponse)Utils.CallAPIServer(inputParams, request);
            ANetApiResponse baseResponse = (ANetApiResponse)apiResponse;
            string result = "Result: " + baseResponse.messages.resultCode.ToString() + " - ";
            if (baseResponse.messages.resultCode != messageTypeEnum.Ok)
            {
                // Write error messages
                for (int a = 0; a < baseResponse.messages.message.Length; a++)
                {
                    result += "[" + baseResponse.messages.message[a].code + "] " + baseResponse.messages.message[a].text;
                }
            }
            else
            {
                // resellerCreateMerchantResponse is the only API call that returns data other than messages.
                if (apiResponse.GetType() == typeof(resellerCreateMerchantResponse))
                {
                    result = "Gateway uploaed. " + "Merchant Id: " + ((resellerCreateMerchantResponse)apiResponse).merchantId.ToString();
                }
            }
            return result;
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "CreateAnetXML - " + err.Message);
            return err.Message;
        }
    }

    protected void btnCancelPlatform_Click(object sender, EventArgs e)
    {
        pnlPlatform.Visible = false;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string result = CreateAnetXML(new Guid(lblContactID.Text.ToString().Trim()));
            DisplayMessage(result);
            pnlPlatform.Visible = false;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.ToString());
        }
    }

    //This function disables fields based on reprogram platform selected
    public void DisablePlatformFields()
    {
        lstPlatform.Enabled = false;
        if (lstPlatform.SelectedItem.Text == "Nashville")
        {
            //Merchant ID
            txtMerchantID.Enabled = true;
            txtMerchantID.MaxLength = 11;
            txtTerminalID.Enabled = true;
            txtTerminalID.MaxLength = 11;

            if ((txtMerchantID.Text.Length < 6) || (txtMerchantID.Text.Length > 11))
            {
                lblError.Visible = true;
                lblError.Text += "Merchant ID is invalid. Please correct in ACT! and retry<br/>";
            }

            if ((txtTerminalID.Text.Length < 6) || (txtTerminalID.Text.Length > 11))
            {
                lblError.Visible = true;
                lblError.Text += "Enter the Terminal ID from Sales Opportunity in ACT!<br/>";
            }

            txtVisaMasterNumber.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMCCCode.Enabled = false;
            txtBINNumber.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            //txtLoginID.Enabled = false;
            
            txtVisaMasterNumber.BackColor = System.Drawing.Color.DarkGray;
            txtStoreNumber.BackColor = System.Drawing.Color.DarkGray;
            txtMCCCode.BackColor = System.Drawing.Color.DarkGray;
            txtBINNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentChainNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentBankNumber.BackColor = System.Drawing.Color.DarkGray;
            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;

            txtVisaMasterNumber.Text = "";
            txtStoreNumber.Text = "";
            txtMCCCode.Text = "";
            txtBINNumber.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            //txtLoginID.Text = "";
        }//end if nashville
        else if (lstPlatform.SelectedItem.Text == "Vital")
        {
            txtBINNumber.Enabled = true;
            txtBINNumber.MaxLength = 6;
            txtAgentBankNumber.Enabled = true;
            txtAgentBankNumber.MaxLength = 6;
            txtAgentChainNumber.Enabled = true;
            txtAgentChainNumber.MaxLength = 6;
            txtMCCCode.Enabled = true;
            txtMCCCode.MaxLength = 4;
            txtMerchantID.Enabled = true;
            txtMerchantID.MaxLength = 12;
            txtStoreNumber.Enabled = true;
            txtStoreNumber.MaxLength = 4;
            txtTerminalID.Enabled = true;
            txtTerminalID.MaxLength = 4;

            if ((txtBINNumber.Text.Length < 6) || (txtBINNumber.Text.Length > 6))
            {
                lblError.Visible = true;
                lblError.Text += "BIN Number length must be 6 characters long. Please correct this in ACT!<br/>";
            }

            if ((txtAgentBankNumber.Text.Length < 6) || (txtAgentBankNumber.Text.Length > 6))
            {
                lblError.Visible = true;
                lblError.Text += "Agent Bank Number length must be 6 characters long. Please correct this in ACT!<br/>";
            }

            if ((txtMCCCode.Text.Length < 4) || (txtMCCCode.Text.Length > 4))
            {
                lblError.Visible = true;
                lblError.Text += "MCC Code length must be 4 characters long. Please correct in ACT!<br/>";
            }

            if ((txtAgentChainNumber.Text.Length < 6) || (txtAgentChainNumber.Text.Length > 6))
            {
                lblError.Visible = true;
                lblError.Text += "Agent Chain Number length must be 6 characters long. Please correc in ACT!<br/>";
            }

            if ((txtMerchantID.Text.Length < 12) || (txtMerchantID.Text.Length > 12))
            {
                lblError.Visible = true;
                lblError.Text += "Merchant ID length must be 12 characters long. Please correct in ACT!<br/>";
            }

            if ((txtStoreNumber.Text.Length < 4) || (txtStoreNumber.Text.Length > 4))
            {
                lblError.Visible = true;
                lblError.Text += "Store Number length must be 4 characters long. Please correct in ACT!<br/>";
            }

            if ((txtTerminalID.Text.Length < 4) || (txtTerminalID.Text.Length > 4))
            {
                lblError.Visible = true;
                lblError.Text += "Terminal ID length must be 4 characters long.<br/>";
            }

            //txtLoginID.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;
            txtVisaMasterNumber.BackColor = System.Drawing.Color.DarkGray;

            //txtLoginID.Text = "";
            txtVisaMasterNumber.Text = "";
        }//end if vital
        else if (lstPlatform.SelectedItem.Text == "Nova")
        {
            txtBINNumber.Enabled = true;
            txtBINNumber.MaxLength = 6;
            txtTerminalID.Enabled = true;
            txtTerminalID.MaxLength = 16;

            if ((txtBINNumber.Text.Length < 6) || (txtBINNumber.Text.Length > 6))
            {
                lblError.Visible = true;
                lblError.Text += "BIN Number length must be 6 characters long. Please correct in ACT!<br/>";
            }

            if ((txtTerminalID.Text.Length < 6) || (txtTerminalID.Text.Length > 16))
            {
                lblError.Visible = true;
                lblError.Text += "Terminal ID length must be 16 characters long. Please correct in ACT!<br/>";
            }

            //txtLoginID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMerchantID.Enabled = false;
            txtMCCCode.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;
            txtStoreNumber.BackColor = System.Drawing.Color.DarkGray;
            txtMerchantID.BackColor = System.Drawing.Color.DarkGray;
            txtMCCCode.BackColor = System.Drawing.Color.DarkGray;
            txtAgentChainNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentBankNumber.BackColor = System.Drawing.Color.DarkGray;
            txtVisaMasterNumber.BackColor = System.Drawing.Color.DarkGray;

            //txtLoginID.Text = "";
            txtStoreNumber.Text = "";
            txtMerchantID.Text = "";
            txtMCCCode.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            txtVisaMasterNumber.Text = "";
        }//end if nova
        else if (lstPlatform.SelectedItem.Text == "Omaha")
        {
            txtVisaMasterNumber.MaxLength = 16;
            txtVisaMasterNumber.Enabled = true;
            txtMCCCode.Enabled = true;
            txtMCCCode.MaxLength = 4;

            if ((txtVisaMasterNumber.Text.Length < 7) || (txtVisaMasterNumber.Text.Length > 16))
            {
                lblError.Visible = true;
                lblError.Text += "Visa Master Number length must be between 7 and 16 characters long. Please correct in ACT!<br/>";
            }
            if ((txtMCCCode.Text.Length < 4) || (txtMCCCode.Text.Length > 4))
            {
                lblError.Visible = true;
                lblError.Text += "MCC Code length must be 4 characters long. Please correct in ACT!<br/>";
            }

            txtTerminalID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMerchantID.Enabled = false;
            //txtLoginID.Enabled = false;
            txtBINNumber.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;

            txtTerminalID.BackColor = System.Drawing.Color.DarkGray;
            txtStoreNumber.BackColor = System.Drawing.Color.DarkGray;
            txtMerchantID.BackColor = System.Drawing.Color.DarkGray;
            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;
            txtBINNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentChainNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentBankNumber.BackColor = System.Drawing.Color.DarkGray;

            txtTerminalID.Text = "";
            txtStoreNumber.Text = "";
            txtMerchantID.Text = "";
            //txtLoginID.Text = "";
            txtBINNumber.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
        }
        else if (lstPlatform.SelectedItem.Text == "Global Payments")
        {
            txtMerchantID.Enabled = true;
            txtMerchantID.MaxLength = 15;
            txtBINNumber.Enabled = true;
            txtBINNumber.MaxLength = 6;

            if ((txtBINNumber.Text.Length < 4) || (txtBINNumber.Text.Length > 6))
            {
                lblError.Visible = true;
                lblError.Text += "BIN Number length must be between 4 and 6 characters long. Please correct in ACT!<br/>";
            }

            if ((txtMerchantID.Text.Length < 3) || (txtMerchantID.Text.Length > 15))
            {
                lblError.Visible = true;
                lblError.Text += "Merchant ID length must be between 3 and 15 characters long. Please correct in ACT!<br/>";
            }

            //txtLoginID.Enabled = false;
            txtTerminalID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMCCCode.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;
            txtTerminalID.BackColor = System.Drawing.Color.DarkGray;
            txtStoreNumber.BackColor = System.Drawing.Color.DarkGray;
            txtMCCCode.BackColor = System.Drawing.Color.DarkGray;
            txtAgentChainNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentBankNumber.BackColor = System.Drawing.Color.DarkGray;
            txtVisaMasterNumber.BackColor = System.Drawing.Color.DarkGray;

            //txtLoginID.Text = "";
            txtTerminalID.Text = "";
            txtStoreNumber.Text = "";
            txtMCCCode.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            txtVisaMasterNumber.Text = "";
        }//end if Global Payments
        else if (lstPlatform.SelectedItem.Text == "Paymentech")
        {
            txtMerchantID.Enabled = true;
            txtMerchantID.MaxLength = 12;
            txtTerminalID.Enabled = true;
            txtTerminalID.MaxLength = 3;
            txtStoreNumber.Enabled = true;
            txtStoreNumber.MaxLength = 4;

            if ((txtMerchantID.Text.Length < 11) || (txtMerchantID.Text.Length > 12))
            {
                lblError.Visible = true;
                lblError.Text += "Merchant ID Number length must be 11 or 12 characters long. Please correct in ACT!<br/>";
            }

            if ((txtTerminalID.Text.Length < 2) || (txtTerminalID.Text.Length > 3))
            {
                lblError.Visible = true;
                lblError.Text += "Terminal ID length must be 2 or 3 characters long. <br/>";
            }

            if ((txtStoreNumber.Text.Length < 3) || (txtStoreNumber.Text.Length > 4))
            {
                lblError.Visible = true;
                lblError.Text += "Client Number length must be 3 or 4 characters long. Please correct in ACT!<br/>";
            }

            txtBINNumber.Enabled = false;
            //txtLoginID.Enabled = false;
            txtMCCCode.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            txtBINNumber.BackColor = System.Drawing.Color.DarkGray;
            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;
            txtMCCCode.BackColor = System.Drawing.Color.DarkGray;
            txtAgentChainNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentBankNumber.BackColor = System.Drawing.Color.DarkGray;
            txtVisaMasterNumber.BackColor = System.Drawing.Color.DarkGray;

            txtBINNumber.Text = "";
            //txtLoginID.Text = "";
            txtMCCCode.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            txtVisaMasterNumber.Text = "";
        }
        else
        {
            txtMerchantID.Enabled = false;
            txtBINNumber.Enabled = false;
            //txtLoginID.Enabled = false;
            txtTerminalID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMCCCode.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            txtMerchantID.BackColor = System.Drawing.Color.DarkGray;
            txtBINNumber.BackColor = System.Drawing.Color.DarkGray;
            //txtLoginID.BackColor = System.Drawing.Color.DarkGray;
            txtTerminalID.BackColor = System.Drawing.Color.DarkGray;
            txtStoreNumber.BackColor = System.Drawing.Color.DarkGray;
            txtMCCCode.BackColor = System.Drawing.Color.DarkGray;
            txtAgentChainNumber.BackColor = System.Drawing.Color.DarkGray;
            txtAgentBankNumber.BackColor = System.Drawing.Color.DarkGray;
            txtVisaMasterNumber.BackColor = System.Drawing.Color.DarkGray;

            txtMerchantID.Text = "";
            txtBINNumber.Text = "";
            //txtLoginID.Text = "";
            txtTerminalID.Text = "";
            txtStoreNumber.Text = "";
            txtMCCCode.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            txtVisaMasterNumber.Text = "";
        }
    }//end function DisablePlatformFields

    protected void lstPlatform_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetPlatformControls();
        DisablePlatformFields();
    }

    //This function resets Platform controls
    public void ResetPlatformControls()
    {
        System.Web.UI.WebControls.TextBox txtBox = new System.Web.UI.WebControls.TextBox();
        for (int i = 0; i < pnlPlatform.Controls.Count; i++)
        {
            if (pnlPlatform.Controls[i].GetType() == txtBox.GetType())
            {
                txtBox = (System.Web.UI.WebControls.TextBox)pnlPlatform.Controls[i];
                txtBox.Text = "";
            }
        }//end for        
    }
}
