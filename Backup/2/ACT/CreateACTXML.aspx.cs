using System;
using System.Data;
using System.Collections.Generic; 
using System.Configuration;
using System.Collections;
using System.Web;
//using System.Web.HttpContext.Current.Session;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using BusinessLayer;
using DLPartner;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Mail;
using DLPartner.net.eftsecure.uno;
using System.Xml.Serialization;
using System.Net;
using System.Windows.Forms;

public partial class CreateACTXML : System.Web.UI.Page
{
    //the selected Contact ID clicked by user
    //private static string selContactID = "";
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Employee"))
                Page.MasterPageFile = "Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "Admin.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        pnlAttachment.Visible = false;

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

    //This function handles grid view button click event
    protected void grdPDF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "CreateXML")//If the Create XML button in the grid is clicked
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdPDF.Rows[index];

                System.Guid ContactID = new Guid(Server.HtmlDecode(grdRow.Cells[1].Text));
                string Processor = Server.HtmlDecode(grdRow.Cells[7].Text);
                if (Processor.ToLower().Contains("ipayment"))
                    CreateiPayXMLNew(ContactID);
                    //btnAttachFile_Click();
                    //pnlAttachment.Visible = true;
                else if (Processor.ToLower().Contains("sage"))
                    CreateSageXML(ContactID);
                else if (Processor != "")
                    DisplayMessage("Processor " + Processor + " is not a valid Processor for XML creation.");
                else
                    DisplayMessage("No Processor assigned to this ACT record. XML cannot be created.");
            }//end if command name   
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Create ACT XML - " + err.Message);
            DisplayMessage(err.Message);
        }
    }//end function grid view button click

    protected void btnAttachFile_Click(object sender, EventArgs e)
    {
        Response.Redirect("AttachDocument.aspx", false);
    }

    public void CreateiPayXMLNew(System.Guid ContactID)
    {
        try
        {
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID.ToString());
            XMLBL IPay = new XMLBL();
            PartnerDS.ACTiPayXMLDataTable dt = IPay.GetIPayXML(ContactID);

            if (dt.Rows.Count > 0)
            {

                string FileName = "IPay_New.xml";
                XmlDocument doc = new XmlDocument();

                RepInfoDL repInfo = new RepInfoDL();
                string iPaySalesID = repInfo.ReturniPaySalesID(Convert.ToString(dt[0].RepNum));

                doc.Load(Server.MapPath(FileName));//Load the XML file

                #region ExtendedInfo

                XmlNodeList nodelist = doc.GetElementsByTagName("ExtendedInfo");
                XmlNode node = nodelist.Item(0);

                XmlAttribute xmlAttsalesNumber = doc.CreateAttribute("salesNumber");
                xmlAttsalesNumber.Value = Convert.ToString(dt[0].RepNum);
                node.Attributes.Append(xmlAttsalesNumber);

                XmlAttribute xmlAttsalesName = doc.CreateAttribute("salesName");
                xmlAttsalesName.Value = Convert.ToString(dt[0].RepName);
                node.Attributes.Append(xmlAttsalesName);

                XmlAttribute xmlAttsalesPhone = doc.CreateAttribute("salesPhone");
                xmlAttsalesPhone.Value = "800-477-5363";
                node.Attributes.Append(xmlAttsalesPhone);

                XmlAttribute xmlAttsalesID = doc.CreateAttribute("salesID");
                xmlAttsalesID.Value = Convert.ToString(iPaySalesID);
                node.Attributes.Append(xmlAttsalesID);

                XmlAttribute xmlAttFormID = doc.CreateAttribute("FormID");
                xmlAttFormID.Value = "1306";
                node.Attributes.Append(xmlAttFormID);

                XmlAttribute xmlAttVersion = doc.CreateAttribute("Version");
                xmlAttVersion.Value = "0212.2";
                node.Attributes.Append(xmlAttVersion);

                XmlAttribute xmlAttExtendedInfoID= doc.CreateAttribute("id");
                xmlAttExtendedInfoID.Value = "-1";
                node.Attributes.Append(xmlAttExtendedInfoID);

                #endregion

                #region BusinessInfo

                nodelist = doc.GetElementsByTagName("BusinessInfo");
                node = nodelist.Item(0);

                XmlAttribute xmlAttBusinessInfoID = doc.CreateAttribute("id");
                xmlAttBusinessInfoID.Value = "-1";
                //xmlAttlegalBusinessName.Value = "Test - E-Commerce Exchange";
                node.Attributes.Append(xmlAttBusinessInfoID);

                XmlAttribute xmlAttlegalBusinessName = doc.CreateAttribute("legalBusinessName");
                xmlAttlegalBusinessName.Value = Convert.ToString(dt[0].COMPANYNAME);
                //xmlAttlegalBusinessName.Value = "Test - E-Commerce Exchange";
                node.Attributes.Append(xmlAttlegalBusinessName);

                XmlAttribute xmlAttdoingBusinessAs = doc.CreateAttribute("doingBusinessAs");
                xmlAttdoingBusinessAs.Value = Convert.ToString(dt[0].DBA);
                node.Attributes.Append(xmlAttdoingBusinessAs);

                XmlAttribute xmlAttBusAddress = doc.CreateAttribute("address");
                xmlAttBusAddress.Value = Convert.ToString(dt[0].Address1);
                node.Attributes.Append(xmlAttBusAddress);

                XmlAttribute xmlAttBusAddress2 = doc.CreateAttribute("address2");
                xmlAttBusAddress2.Value = Convert.ToString(dt[0].Address2);
                node.Attributes.Append(xmlAttBusAddress2);

                XmlAttribute xmlAttBusCity = doc.CreateAttribute("city");
                xmlAttBusCity.Value = Convert.ToString(dt[0].CITY);
                node.Attributes.Append(xmlAttBusCity);

                XmlAttribute xmlAttBusState = doc.CreateAttribute("state");
                if (dt[0].STATE.Length != 2)
                {
                    DisplayMessage("Business State length must be 2 characters long.");
                }
                else
                {
                    xmlAttBusState.Value = Convert.ToString(dt[0].STATE);
                    node.Attributes.Append(xmlAttBusState);
                }

                XmlAttribute xmlAttBusZip = doc.CreateAttribute("zip");
                xmlAttBusZip.Value = Convert.ToString(dt[0].ZipCode);
                node.Attributes.Append(xmlAttBusZip);

                XmlAttribute xmlAttBusCountry = doc.CreateAttribute("county");
                xmlAttBusCountry.Value = Convert.ToString(dt[0].Country);
                node.Attributes.Append(xmlAttBusCountry);

                //[XmlAttribute(DataType = "string")]public string xmlAttBusmonthsAtAddress;
                XmlAttribute xmlAttBusmonthsAtAddress = doc.CreateAttribute("monthsAtAddress");
                //[XmlAttribute("monthsAtAddress", DataType = "string")] Public string PersID {Get;set;}
                xmlAttBusmonthsAtAddress.Value = "1";
                node.Attributes.Append(xmlAttBusmonthsAtAddress);

                XmlAttribute xmlAttBusyearsAtAddress = doc.CreateAttribute("yearsAtAddress");
                /*string strLOS = Convert.ToString(dt[0].LOS);
                if ((strLOS != "0") || (strLOS != ""))
                {
                    if (strLOS.Contains(" "))
                        strLOS = strLOS.Substring(0, strLOS.IndexOf(" "));
                }*/

                /*if (!dt[0].LOS.IsDBNull(0))
                {
                    xmlAttBusyearsAtAddress.Value = Convert.ToString(dt[0].LOS);
                }
                else
                { 
                    xmlAttBusyearsAtAddress.Value = "0";
                }*/
                xmlAttBusyearsAtAddress.Value = "1";
                //xmlAttBusyearsAtAddress.Value = Convert.ToString(dt[0].LOS);
                node.Attributes.Append(xmlAttBusyearsAtAddress);

                XmlAttribute xmlAttBusmainPhone = doc.CreateAttribute("mainPhone");
                xmlAttBusmainPhone.Value = Convert.ToString(dt[0].BusinessPhone);
                node.Attributes.Append(xmlAttBusmainPhone);

                XmlAttribute xmlAttcustomerServicePhone = doc.CreateAttribute("customerServicePhone");
                xmlAttcustomerServicePhone.Value = Convert.ToString(dt[0].CustServPhone);
                node.Attributes.Append(xmlAttcustomerServicePhone);

                XmlAttribute xmlAttBusmainFaxPhone = doc.CreateAttribute("mainFaxPhone");
                xmlAttBusmainFaxPhone.Value = Convert.ToString(dt[0].Fax);
                node.Attributes.Append(xmlAttBusmainFaxPhone);

                XmlAttribute xmlAttBuscontactFirstName = doc.CreateAttribute("contactFirstName");
                xmlAttBuscontactFirstName.Value = Convert.ToString(dt[0].P1FirstName);
                node.Attributes.Append(xmlAttBuscontactFirstName);

                XmlAttribute xmlAttBuscontactLastName = doc.CreateAttribute("contactLastName");
                xmlAttBuscontactLastName.Value = Convert.ToString(dt[0].P1LastName);
                node.Attributes.Append(xmlAttBuscontactLastName);

                XmlAttribute xmlAttBusnumberOfLocations = doc.CreateAttribute("numberOfLocations");
                xmlAttBusnumberOfLocations.Value = Convert.ToString(dt[0].NumberOfLocations);
                node.Attributes.Append(xmlAttBusnumberOfLocations);

                XmlAttribute xmlAttyearsInBusiness = doc.CreateAttribute("yearsInBusiness");
                xmlAttyearsInBusiness.Value = Convert.ToString(dt[0].YIB);
                node.Attributes.Append(xmlAttyearsInBusiness);

                XmlAttribute xmlAttmonthsInBusiness = doc.CreateAttribute("monthsInBusiness");
                xmlAttmonthsInBusiness.Value = Convert.ToString(dt[0].MIB);
                node.Attributes.Append(xmlAttmonthsInBusiness);

                XmlAttribute xmlAttbusinessHours = doc.CreateAttribute("businessHours");
                xmlAttbusinessHours.Value = Convert.ToString(dt[0].BusinessHours);
                node.Attributes.Append(xmlAttbusinessHours);

                XmlAttribute xmlAttmainEmailAddress = doc.CreateAttribute("mainEmailAddress");
                xmlAttmainEmailAddress.Value = Convert.ToString(dt[0].Email);
                node.Attributes.Append(xmlAttmainEmailAddress);

                XmlAttribute xmlAttmainWebSite = doc.CreateAttribute("mainWebSite");
                xmlAttmainWebSite.Value = Convert.ToString(dt[0].Website);
                node.Attributes.Append(xmlAttmainWebSite);

                XmlAttribute xmlAttHasWebsite = doc.CreateAttribute("HasWebsite");
                xmlAttHasWebsite.Value = "false";
                node.Attributes.Append(xmlAttHasWebsite);

                XmlAttribute xmlAttmailingAddress = doc.CreateAttribute("mailingAddress");
                xmlAttmailingAddress.Value = Convert.ToString(dt[0].BillingAddress);
                node.Attributes.Append(xmlAttmailingAddress);

                XmlAttribute xmlAttmailingAddress2 = doc.CreateAttribute("mailingAddress2");
                xmlAttmailingAddress2.Value = Convert.ToString(dt[0].BillingAddress2);
                node.Attributes.Append(xmlAttmailingAddress2);

                XmlAttribute xmlAttmailingAddressCity = doc.CreateAttribute("mailingAddressCity");
                xmlAttmailingAddressCity.Value = Convert.ToString(dt[0].BillingCity);
                node.Attributes.Append(xmlAttmailingAddressCity);

                XmlAttribute xmlAttmailingAddressState = doc.CreateAttribute("mailingAddressState");
                xmlAttmailingAddressState.Value = Convert.ToString(dt[0].BillingState).Trim();
                node.Attributes.Append(xmlAttmailingAddressState);

                XmlAttribute xmlAttmailingAddressZip = doc.CreateAttribute("mailingAddressZip");
                xmlAttmailingAddressZip.Value = Convert.ToString(dt[0].BillingZipCode);
                node.Attributes.Append(xmlAttmailingAddressZip);

                XmlAttribute xmlAttfederalTaxID = doc.CreateAttribute("federalTaxID");
                xmlAttfederalTaxID.Value = Convert.ToString(dt[0].FederalTaxID);
                node.Attributes.Append(xmlAttfederalTaxID);

                XmlAttribute xmlAttmerchantType = doc.CreateAttribute("merchantType");
                //xmlAttmerchantType.Value = "3";
                
                if (Convert.ToInt32(dt[0].BusinessPctInternet) >= 50)
                {
                    xmlAttmerchantType.Value = "3";
                }
                else if (Convert.ToInt32(dt[0].BusinessPctMailOrder) >= 50)
                {
                    xmlAttmerchantType.Value = "2";
                }
                else if (Convert.ToInt32(dt[0].BusinessPctRetail) >= 50)
                {
                    xmlAttmerchantType.Value = "0";
                }
                else if (Convert.ToInt32(dt[0].BusinessPctRestaurant) >= 50)
                {
                    xmlAttmerchantType.Value = "4";
                }
                else if (Convert.ToInt32(dt[0].BusinessPctService) >= 50)
                {
                    xmlAttmerchantType.Value = "1";
                }
                node.Attributes.Append(xmlAttmerchantType);

                XmlAttribute xmlAttownershipType = doc.CreateAttribute("ownershipType");
                xmlAttownershipType.Value = Convert.ToString(dt[0].LegalStatus);
                node.Attributes.Append(xmlAttownershipType);

                XmlAttribute xmlAttbusinessLocationType = doc.CreateAttribute("businessLocationType");
                xmlAttbusinessLocationType.Value = "3";
                node.Attributes.Append(xmlAttbusinessLocationType);

                XmlAttribute xmlAttbusinessLocationTypeComment = doc.CreateAttribute("businessLocationTypeComment");
                xmlAttbusinessLocationTypeComment.Value = "";
                node.Attributes.Append(xmlAttbusinessLocationTypeComment);

                XmlAttribute xmlAttproductsType = doc.CreateAttribute("productsType");
                xmlAttproductsType.Value = Convert.ToString(dt[0].ProductSold);
                node.Attributes.Append(xmlAttproductsType);

                XmlAttribute xmlAttmethodsOfMarketingType = doc.CreateAttribute("methodsOfMarketingType");
                xmlAttmethodsOfMarketingType.Value = "";
                node.Attributes.Append(xmlAttmethodsOfMarketingType);

                XmlAttribute xmlAttrefundPolicy = doc.CreateAttribute("refundPolicy");
                if (Convert.ToString(dt[0].RefundPolicy).Trim() == "Refund within 30 days")
                {
                    xmlAttrefundPolicy.Value = "0";
                }
                else if (Convert.ToString(dt[0].RefundPolicy).Trim() == "Exchange Only")
                {
                    xmlAttrefundPolicy.Value = "1";
                }
                else if (Convert.ToString(dt[0].RefundPolicy).Trim() == "No Refund")
                {
                    xmlAttrefundPolicy.Value = "2";
                }
                else if (Convert.ToString(dt[0].RefundPolicy).Trim() == "Other")
                {
                    xmlAttrefundPolicy.Value = "3";
                }
                //xmlAttrefundPolicy.Value = Convert.ToString(dt[0].RefundPolicy);
                node.Attributes.Append(xmlAttrefundPolicy);

                XmlAttribute xmlAttproductDeliveryInDays = doc.CreateAttribute("productDeliveryInDays");
                xmlAttproductDeliveryInDays.Value = Convert.ToString(dt[0].NumDaysDelivered).Trim();
                node.Attributes.Append(xmlAttproductDeliveryInDays);

                XmlAttribute xmlAttproductDeliveryComments = doc.CreateAttribute("productDeliveryComments");
                xmlAttproductDeliveryComments.Value = Convert.ToString(dt[0].AddlComments);
                node.Attributes.Append(xmlAttproductDeliveryComments);

                XmlAttribute xmlAttappliedBefore = doc.CreateAttribute("appliedBefore");
                XmlAttribute xmlAttappliedBeforePriorProcessor = doc.CreateAttribute("appliedBeforePriorProcessor");
                XmlAttribute xmlAttpreviousMerchantNumbers = doc.CreateAttribute("previousMerchantNumbers");
                XmlAttribute xmlAttcreditCardProcessorPriorClosure = doc.CreateAttribute("creditCardProcessorPriorClosure");
                XmlAttribute xmlAttcreditCardProcessorPriorClosureWhom = doc.CreateAttribute("creditCardProcessorPriorClosureWhom");
                if (dt[0].PrevProcessed.Trim() == "Yes")
                {
                    xmlAttappliedBefore.Value = "true";
                    xmlAttappliedBeforePriorProcessor.Value = Convert.ToString(dt[0].PrevProcessor);
                    xmlAttpreviousMerchantNumbers.Value = Convert.ToString(dt[0].PrevMerchantAcctNo);
                    xmlAttcreditCardProcessorPriorClosureWhom.Value = Convert.ToString(dt[0].PrevProcessor);
                    if (dt[0].CTMF == "Yes")
                    {
                        xmlAttcreditCardProcessorPriorClosure.Value = "true";
                    }
                    else
                    {
                        xmlAttcreditCardProcessorPriorClosure.Value = "false";
                    }

                }
                else
                {
                    xmlAttappliedBefore.Value = "false";
                    xmlAttappliedBeforePriorProcessor.Value = "";
                    xmlAttpreviousMerchantNumbers.Value = "";
                    xmlAttcreditCardProcessorPriorClosureWhom.Value = "";
                    xmlAttcreditCardProcessorPriorClosure.Value = "false";
                }
                node.Attributes.Append(xmlAttappliedBefore);
                node.Attributes.Append(xmlAttappliedBeforePriorProcessor);
                node.Attributes.Append(xmlAttpreviousMerchantNumbers);
                node.Attributes.Append(xmlAttcreditCardProcessorPriorClosureWhom);
                node.Attributes.Append(xmlAttcreditCardProcessorPriorClosure);

                XmlAttribute xmlAttcreditCardProcessorPriorClosureExplaination = doc.CreateAttribute("creditCardProcessorPriorClosureExplaination");
                xmlAttcreditCardProcessorPriorClosureExplaination.Value = Convert.ToString(dt[0].ReasonForLeaving);
                node.Attributes.Append(xmlAttcreditCardProcessorPriorClosureExplaination);


                int OpenYear = 0;
                int OpenMonth = 0;

                if ((!Convert.IsDBNull(dt[0].YIB)) && (!Convert.IsDBNull(dt[0].MIB)))
                {
                    int YearInBus = 0;
                    int MonInBus = 0;
                    YearInBus = Convert.ToInt32(dt[0].YIB);
                    MonInBus = Convert.ToInt32(dt[0].MIB);
                    DateTime CurrentDate = DateTime.Now;
                    int CurrentYear = CurrentDate.Year;
                    int CurrentMon = CurrentDate.Month;
                    if (YearInBus < CurrentYear)
                    {
                        if (MonInBus <= CurrentMon)
                        {
                            OpenYear = CurrentYear - YearInBus;
                            OpenMonth = CurrentMon - MonInBus;
                        }
                        else
                        {
                            OpenMonth = 12 - MonInBus + CurrentMon;
                            OpenYear = CurrentYear - YearInBus - 1;
                        }
                    }
                    else if (YearInBus == CurrentYear)
                    {
                        if (MonInBus <= CurrentMon)
                        {
                            OpenYear = CurrentYear - YearInBus;
                            OpenMonth = CurrentMon - MonInBus;
                        }

                    }
                }

                XmlAttribute xmlAttopenDateYear = doc.CreateAttribute("openDateYear");
                xmlAttopenDateYear.Value = Convert.ToString(OpenYear);
                node.Attributes.Append(xmlAttopenDateYear);

                XmlAttribute xmlAttopenDateMonth = doc.CreateAttribute("openDateMonth");
                xmlAttopenDateMonth.Value = Convert.ToString(OpenMonth);
                node.Attributes.Append(xmlAttopenDateMonth);

                XmlAttribute xmlAttlengthOfOwnership = doc.CreateAttribute("lengthOfOwnership");
                xmlAttlengthOfOwnership.Value = Convert.ToString(dt[0].YIB);
                node.Attributes.Append(xmlAttlengthOfOwnership);
                

                XmlAttribute xmlAttlocationNumber = doc.CreateAttribute("locationNumber");
                xmlAttlocationNumber.Value = Convert.ToString(dt[0].NumberOfLocations);
                node.Attributes.Append(xmlAttlocationNumber);

                
                XmlAttribute xmlAttb2BPercent = doc.CreateAttribute("b2BPercent");
                xmlAttb2BPercent.Value = "0";
                node.Attributes.Append(xmlAttb2BPercent);
                

                XmlAttribute xmlAttIsB2B = doc.CreateAttribute("IsB2B");
                xmlAttIsB2B.Value = "False";
                node.Attributes.Append(xmlAttIsB2B);

                XmlAttribute xmlAttIsVendorFulfillmentHouse = doc.CreateAttribute("IsVendorFulfillmentHouse");
                xmlAttIsVendorFulfillmentHouse.Value = "False";
                node.Attributes.Append(xmlAttIsVendorFulfillmentHouse);

                XmlAttribute xmlAttVFHName = doc.CreateAttribute("VFHName");
                xmlAttVFHName.Value = "";
                node.Attributes.Append(xmlAttVFHName);

                XmlAttribute xmlAttVFHAddress1 = doc.CreateAttribute("VFHAddress1");
                xmlAttVFHAddress1.Value = "";
                node.Attributes.Append(xmlAttVFHAddress1);

                XmlAttribute xmlAttVFHAddress2 = doc.CreateAttribute("VFHAddress2");
                xmlAttVFHAddress2.Value = "";
                node.Attributes.Append(xmlAttVFHAddress2);

                XmlAttribute xmlAttVFHCity = doc.CreateAttribute("VFHCity");
                xmlAttVFHCity.Value = "";
                node.Attributes.Append(xmlAttVFHCity);

                XmlAttribute xmlAttVFHState = doc.CreateAttribute("VFHState");
                xmlAttVFHState.Value = "";
                node.Attributes.Append(xmlAttVFHState);

                XmlAttribute xmlAttVFHZip = doc.CreateAttribute("VFHZip");
                xmlAttVFHZip.Value = "";
                node.Attributes.Append(xmlAttVFHZip);

                XmlAttribute xmlAttVFHPhone = doc.CreateAttribute("VFHPhone");
                xmlAttVFHPhone.Value = "";
                node.Attributes.Append(xmlAttVFHPhone);

                XmlAttribute xmlAttUsesThirdPartyData = doc.CreateAttribute("UsesThirdPartyData");
                xmlAttUsesThirdPartyData.Value = "false";
                node.Attributes.Append(xmlAttUsesThirdPartyData);

                XmlAttribute xmlAttTPDName = doc.CreateAttribute("TPDName");
                xmlAttTPDName.Value = "";
                node.Attributes.Append(xmlAttTPDName);

                XmlAttribute xmlAttTPDAddress1 = doc.CreateAttribute("TPDAddress1");
                xmlAttTPDAddress1.Value = "";
                node.Attributes.Append(xmlAttTPDAddress1);

                XmlAttribute xmlAttTPDAddress2 = doc.CreateAttribute("TPDAddress2");
                xmlAttTPDAddress2.Value = "";
                node.Attributes.Append(xmlAttTPDAddress2);

                XmlAttribute xmlAttTPDCity = doc.CreateAttribute("TPDCity");
                xmlAttTPDCity.Value = "";
                node.Attributes.Append(xmlAttTPDCity);

                XmlAttribute xmlAttTPDState = doc.CreateAttribute("TPDState");
                xmlAttTPDState.Value = "";
                node.Attributes.Append(xmlAttTPDState);

                XmlAttribute xmlAttTPDZip = doc.CreateAttribute("TPDZip");
                xmlAttTPDZip.Value = "";
                node.Attributes.Append(xmlAttTPDZip);

                XmlAttribute xmlAttTPDPhone = doc.CreateAttribute("TPDPhone");
                xmlAttTPDPhone.Value = "";
                node.Attributes.Append(xmlAttTPDPhone);

                XmlAttribute xmlAttTPDSoftware = doc.CreateAttribute("TPDSoftware");
                xmlAttTPDSoftware.Value = "";
                node.Attributes.Append(xmlAttTPDSoftware);

                
                XmlAttribute xmlAttActiveMonthsString = doc.CreateAttribute("ActiveMonthsString");
                xmlAttActiveMonthsString.Value = "111111111111";
                node.Attributes.Append(xmlAttActiveMonthsString);
                


                XmlAttribute xmlAttReturnPolicy = doc.CreateAttribute("ReturnPolicy");
                if (Convert.ToString(dt[0].RefundPolicy).Trim() == "Refund within 30 days")
                {
                    xmlAttReturnPolicy.Value = "0";
                }
                else if (Convert.ToString(dt[0].RefundPolicy).Trim() == "Exchange Only")
                {
                    xmlAttReturnPolicy.Value = "1";
                }
                else if (Convert.ToString(dt[0].RefundPolicy).Trim() == "No Refund")
                {
                    xmlAttReturnPolicy.Value = "2";
                }
                else if (Convert.ToString(dt[0].RefundPolicy).Trim() == "Other")
                {
                    xmlAttReturnPolicy.Value = "3";
                }
                node.Attributes.Append(xmlAttReturnPolicy);

                XmlAttribute xmlAttReturnPolicyComment = doc.CreateAttribute("ReturnPolicyComment");
                xmlAttReturnPolicyComment.Value = Convert.ToString(dt[0].AddlComments);
                node.Attributes.Append(xmlAttReturnPolicyComment);

                XmlAttribute xmlAttDays2Deliver = doc.CreateAttribute("Days2Deliver");
                xmlAttDays2Deliver.Value = Convert.ToString(dt[0].NumDaysDelivered);
                node.Attributes.Append(xmlAttDays2Deliver);

                XmlAttribute xmlAttDeliveryStartDate = doc.CreateAttribute("DeliveryStartDate");
                xmlAttDeliveryStartDate.Value = "";
                node.Attributes.Append(xmlAttDeliveryStartDate);

                XmlAttribute xmlAttDeliveryComments = doc.CreateAttribute("DeliveryComments");
                xmlAttDeliveryComments.Value = "";
                node.Attributes.Append(xmlAttDeliveryComments);

                XmlAttribute xmlAttRunCreditReport = doc.CreateAttribute("RunCreditReport");
                xmlAttRunCreditReport.Value = "";
                node.Attributes.Append(xmlAttRunCreditReport);

                XmlAttribute xmlAttcreditReportComments = doc.CreateAttribute("creditReportComments");
                xmlAttcreditReportComments.Value = "";
                node.Attributes.Append(xmlAttcreditReportComments);

                XmlAttribute xmlAttPortfolio = doc.CreateAttribute("Portfolio");
                xmlAttPortfolio.Value = "IPI";
                node.Attributes.Append(xmlAttPortfolio);

                XmlAttribute xmlAttAppVersion = doc.CreateAttribute("AppVersion");
                xmlAttAppVersion.Value = "0212.2";
                node.Attributes.Append(xmlAttAppVersion);

                if (!Convert.IsDBNull(dt[0].GatewayMonFee))
                {
                    if (Convert.ToString(dt[0].GatewayMonFee) != "")
                    {
                        XmlAttribute xmlAttPaymentGatewayMonthlyFee = doc.CreateAttribute("PaymentGatewayMonthlyFee");
                        xmlAttPaymentGatewayMonthlyFee.Value = Convert.ToString(dt[0].GatewayMonFee);
                        node.Attributes.Append(xmlAttPaymentGatewayMonthlyFee);
                    }
                    else
                    {
                        XmlAttribute xmlAttPaymentGatewayMonthlyFee = doc.CreateAttribute("PaymentGatewayMonthlyFee");
                        xmlAttPaymentGatewayMonthlyFee.Value = "0.00";
                        node.Attributes.Append(xmlAttPaymentGatewayMonthlyFee);
                    }
                }
                else {
                    XmlAttribute xmlAttPaymentGatewayMonthlyFee = doc.CreateAttribute("PaymentGatewayMonthlyFee");
                    xmlAttPaymentGatewayMonthlyFee.Value = "0.00";
                    node.Attributes.Append(xmlAttPaymentGatewayMonthlyFee);
                }

                if (!Convert.IsDBNull(dt[0].Gateway))
                {
                    if (Convert.ToString(dt[0].Gateway) != "")
                    {
                        XmlAttribute xmlAttpaymentGatewayName = doc.CreateAttribute("paymentGatewayName");
                        xmlAttpaymentGatewayName.Value = Convert.ToString(dt[0].Gateway);
                        node.Attributes.Append(xmlAttpaymentGatewayName);
                    }
                    else
                    {
                        XmlAttribute xmlAttpaymentGatewayName = doc.CreateAttribute("paymentGatewayName");
                        xmlAttpaymentGatewayName.Value = "";
                        node.Attributes.Append(xmlAttpaymentGatewayName);
                    }
                }
                else {
                    XmlAttribute xmlAttpaymentGatewayName = doc.CreateAttribute("paymentGatewayName");
                    xmlAttpaymentGatewayName.Value = "";
                    node.Attributes.Append(xmlAttpaymentGatewayName);
                }

                #endregion

                #region SalesBreakDown

                nodelist = doc.GetElementsByTagName("SalesBreakDown");
                node = nodelist.Item(0);

                XmlAttribute xmlAttSalesBreakDownID = doc.CreateAttribute("id");
                xmlAttSalesBreakDownID.Value = "-1";
                //xmlAttlegalBusinessName.Value = "Test - E-Commerce Exchange";
                node.Attributes.Append(xmlAttSalesBreakDownID);

                XmlAttribute XmlAttretailSwipePercentage = doc.CreateAttribute("retailSwipePercentage");
                XmlAttretailSwipePercentage.Value = Convert.ToString(dt[0].ProcessPctSwiped);
                node.Attributes.Append(XmlAttretailSwipePercentage);

                XmlAttribute XmlAttretailKeyedPercentage = doc.CreateAttribute("retailKeyedPercentage");
                XmlAttretailKeyedPercentage.Value = Convert.ToString(dt[0].ProcessPctKeyed);
                node.Attributes.Append(XmlAttretailKeyedPercentage);

                XmlAttribute XmlAttinternetPercentage = doc.CreateAttribute("internetPercentage");
                XmlAttinternetPercentage.Value = Convert.ToString(dt[0].BusinessPctInternet);
                node.Attributes.Append(XmlAttinternetPercentage);

                XmlAttribute XmlAttmailOrderPercentage = doc.CreateAttribute("mailOrderPercentage");
                XmlAttmailOrderPercentage.Value = Convert.ToString(dt[0].BusinessPctMailOrder);
                node.Attributes.Append(XmlAttmailOrderPercentage);

                #endregion

                #region PrincipalCollection

                nodelist = doc.GetElementsByTagName("PrincipalCollection");

                XmlNode parentNode = nodelist.Item(0);
                node = parentNode.ChildNodes.Item(0);

                #region Principal

                XmlAttribute xmlAttPrincipalID = doc.CreateAttribute("id");
                xmlAttPrincipalID.Value = "-1";
                //xmlAttlegalBusinessName.Value = "Test - E-Commerce Exchange";
                node.Attributes.Append(xmlAttPrincipalID);

                XmlAttribute XmlAttPrincipalnumber = doc.CreateAttribute("number");
                XmlAttPrincipalnumber.Value = "1";
                node.Attributes.Append(XmlAttPrincipalnumber);

                XmlAttribute XmlAttPrincipalfirstName = doc.CreateAttribute("firstName");
                XmlAttPrincipalfirstName.Value = Convert.ToString(dt[0].P1FirstName);
                node.Attributes.Append(XmlAttPrincipalfirstName);

                XmlAttribute XmlAttPrincipallastName = doc.CreateAttribute("lastName");
                XmlAttPrincipallastName.Value = Convert.ToString(dt[0].P1LastName);
                node.Attributes.Append(XmlAttPrincipallastName);

                XmlAttribute XmlAttPrincipalsocialSecurityNumber = doc.CreateAttribute("socialSecurityNumber");
                XmlAttPrincipalsocialSecurityNumber.Value = Convert.ToString(dt[0].P1SSN);
                node.Attributes.Append(XmlAttPrincipalsocialSecurityNumber);

                XmlAttribute XmlAttPrincipalownershipPercentage = doc.CreateAttribute("ownershipPercentage");
                XmlAttPrincipalownershipPercentage.Value = Convert.ToString(dt[0].P1OwnershipPercent);
                node.Attributes.Append(XmlAttPrincipalownershipPercentage);

                XmlAttribute XmlAttPrincipaltitle = doc.CreateAttribute("title");
                XmlAttPrincipaltitle.Value = Convert.ToString(dt[0].P1Title);
                node.Attributes.Append(XmlAttPrincipaltitle);

                XmlAttribute XmlAttPrincipalresidentialAddress = doc.CreateAttribute("residentialAddress");
                XmlAttPrincipalresidentialAddress.Value = Convert.ToString(dt[0].P1Address);
                node.Attributes.Append(XmlAttPrincipalresidentialAddress);

                XmlAttribute XmlAttPrincipalresidentialAddress2 = doc.CreateAttribute("residentialAddress2");
                XmlAttPrincipalresidentialAddress2.Value = Convert.ToString(dt[0].P1Address2);
                node.Attributes.Append(XmlAttPrincipalresidentialAddress2);



                XmlAttribute XmlAttPrincipalisRented = doc.CreateAttribute("isRented");
                XmlAttPrincipalisRented.Value = "";
                if (!Convert.IsDBNull(dt[0].P1LivingStatus))
                {
                    if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Rent")
                    {
                        XmlAttPrincipalisRented.Value = "true";
                    }
                    else if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Own")
                    {
                        XmlAttPrincipalisRented.Value = "false";
                    }
                }
                node.Attributes.Append(XmlAttPrincipalisRented);

                XmlAttribute XmlAttPrincipalcity = doc.CreateAttribute("city");
                XmlAttPrincipalcity.Value = Convert.ToString(dt[0].P1City);
                node.Attributes.Append(XmlAttPrincipalcity);

                XmlAttribute XmlAttPrincipalstate = doc.CreateAttribute("state");
                XmlAttPrincipalstate.Value = Convert.ToString(dt[0].P1State);
                node.Attributes.Append(XmlAttPrincipalstate);

                XmlAttribute XmlAttPrincipalzip = doc.CreateAttribute("zip");
                XmlAttPrincipalzip.Value = Convert.ToString(dt[0].P1ZipCode);
                node.Attributes.Append(XmlAttPrincipalzip);

                XmlAttribute XmlAttPrincipalemail = doc.CreateAttribute("email");
                XmlAttPrincipalemail.Value = Convert.ToString(dt[0].Email);
                node.Attributes.Append(XmlAttPrincipalemail);

                XmlAttribute XmlAttPrincipalhowLongAtAddress = doc.CreateAttribute("howLongAtAddress");
                string TimeAtAddress = "";
                if (!Convert.IsDBNull(dt[0].P1TimeAtAddress))
                {
                    TimeAtAddress = Convert.ToString(dt[0].P1TimeAtAddress).Trim();
                    if ((TimeAtAddress != "0") || (TimeAtAddress != ""))
                    {
                        if (TimeAtAddress.Contains(" "))
                            TimeAtAddress = TimeAtAddress.Substring(0, TimeAtAddress.IndexOf(" "));
                    }
                }
                XmlAttPrincipalhowLongAtAddress.Value = Convert.ToString(TimeAtAddress);
                node.Attributes.Append(XmlAttPrincipalhowLongAtAddress);

                XmlAttribute XmlAttPrincipalhomePhone = doc.CreateAttribute("homePhone");
                XmlAttPrincipalhomePhone.Value = Convert.ToString(dt[0].P1PhoneNumber);
                node.Attributes.Append(XmlAttPrincipalhomePhone);

                XmlAttribute XmlAttPrincipaldateOfBirth = doc.CreateAttribute("dateOfBirth");
                XmlAttPrincipaldateOfBirth.Value = Convert.ToString(dt[0].P1DOB);
                node.Attributes.Append(XmlAttPrincipaldateOfBirth);

                XmlAttribute XmlAttPrincipaldriversLicenseNumber = doc.CreateAttribute("driversLicenseNumber");
                XmlAttPrincipaldriversLicenseNumber.Value = Convert.ToString(dt[0].P1DriversLicenseNo);
                node.Attributes.Append(XmlAttPrincipaldriversLicenseNumber);

                XmlAttribute XmlAttPrincipaldriversLicenseState = doc.CreateAttribute("driversLicenseState");
                XmlAttPrincipaldriversLicenseState.Value = Convert.ToString(dt[0].P1DriversLicenseState).Trim();
                node.Attributes.Append(XmlAttPrincipaldriversLicenseState);

                XmlAttribute XmlAttPrincipalcheckCreditScore = doc.CreateAttribute("checkCreditScore");
                XmlAttPrincipalcheckCreditScore.Value = "";
                node.Attributes.Append(XmlAttPrincipalcheckCreditScore);

                XmlAttribute XmlAttPrincipalOwnershipType = doc.CreateAttribute("OwnershipType");
                if (!Convert.IsDBNull(dt[0].P1LivingStatus))
                {
                    if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Rent")
                    {
                        XmlAttPrincipalOwnershipType.Value = "Rent";
                    }
                    else if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Own")
                    {
                        XmlAttPrincipalOwnershipType.Value = "Own";
                    }
                }
                node.Attributes.Append(XmlAttPrincipalOwnershipType);

                #endregion

                #region Principal2

                string ns = "http://www.ipaymentinc.com";

                if (Convert.ToString(dt[0].P2FirstName) != "")
                {
                    //Get the root node in the XML file to which the new principal 2 tree will be added.
                    //In this XML, the root node is AppData.
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("PrincipalCollection");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Principal", ns);

                    //XmlElement childElement = doc.CreateElement("FirstName");
                    //childElement.InnerText = dt[0].P2FirstName;
                    //Append the child element to "element", which is the <principal> node created above
                    parentNode.AppendChild(childNode);

                    XmlAttribute xmlAttP2ID = doc.CreateAttribute("id");
                    xmlAttP2ID.Value = "2";
                    //xmlAttlegalBusinessName.Value = "Test - E-Commerce Exchange";
                    node.Attributes.Append(xmlAttP2ID);

                    node = parentNode.ChildNodes.Item(1);

                    XmlAttribute XmlAttP2number = doc.CreateAttribute("number");
                    XmlAttP2number.Value = "2";
                    node.Attributes.Append(XmlAttP2number);

                    XmlAttribute XmlAttP2firstName = doc.CreateAttribute("firstName");
                    XmlAttP2firstName.Value = Convert.ToString(dt[0].P2FirstName);
                    node.Attributes.Append(XmlAttP2firstName);

                    XmlAttribute XmlAttP2lastName = doc.CreateAttribute("lastName");
                    XmlAttP2lastName.Value = Convert.ToString(dt[0].P2LastName);
                    node.Attributes.Append(XmlAttP2lastName);

                    XmlAttribute XmlAttP2socialSecurityNumber = doc.CreateAttribute("socialSecurityNumber");
                    XmlAttP2socialSecurityNumber.Value = Convert.ToString(dt[0].P2SSN);
                    node.Attributes.Append(XmlAttP2socialSecurityNumber);

                    XmlAttribute XmlAttP2ownershipPercentage = doc.CreateAttribute("ownershipPercentage");
                    XmlAttP2ownershipPercentage.Value = Convert.ToString(dt[0].P2OwnershipPercent);
                    node.Attributes.Append(XmlAttP2ownershipPercentage);

                    XmlAttribute XmlAttP2title = doc.CreateAttribute("title");
                    XmlAttP2title.Value = Convert.ToString(dt[0].P2Title);
                    node.Attributes.Append(XmlAttP2title);

                    XmlAttribute XmlAttP2residentialAddress = doc.CreateAttribute("residentialAddress");
                    XmlAttP2residentialAddress.Value = Convert.ToString(dt[0].P2Address);
                    node.Attributes.Append(XmlAttP2residentialAddress);

                    XmlAttribute XmlAttP2residentialAddress2 = doc.CreateAttribute("residentialAddress2");
                    XmlAttP2residentialAddress2.Value = Convert.ToString(dt[0].P2Address2);
                    node.Attributes.Append(XmlAttP2residentialAddress2);

                    XmlAttribute XmlAttP2Rented = doc.CreateAttribute("isRented");
                    XmlAttPrincipalisRented.Value = "";
                    if (!Convert.IsDBNull(dt[0].P2LivingStatus))
                    {
                        if (Convert.ToString(dt[0].P2LivingStatus).Trim() == "Rent")
                        {
                            XmlAttP2Rented.Value = "true";
                        }
                        else if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Own")
                        {
                            XmlAttP2Rented.Value = "false";
                        }
                    }
                    node.Attributes.Append(XmlAttP2Rented);

                    XmlAttribute XmlAttP2city = doc.CreateAttribute("city");
                    XmlAttP2city.Value = Convert.ToString(dt[0].P2City);
                    node.Attributes.Append(XmlAttP2city);

                    XmlAttribute XmlAttP2state = doc.CreateAttribute("state");
                    XmlAttP2state.Value = Convert.ToString(dt[0].P2State);
                    node.Attributes.Append(XmlAttP2state);

                    XmlAttribute XmlAttP2zip = doc.CreateAttribute("zip");
                    XmlAttP2zip.Value = Convert.ToString(dt[0].P2ZipCode);
                    node.Attributes.Append(XmlAttP2zip);

                    XmlAttribute XmlAttP2email = doc.CreateAttribute("email");
                    XmlAttP2email.Value = "";
                    node.Attributes.Append(XmlAttP2email);

                    XmlAttribute XmlAttP2howLongAtAddress = doc.CreateAttribute("howLongAtAddress");
                    if (!Convert.IsDBNull(dt[0].P2TimeAtAddress))
                    {
                        TimeAtAddress = Convert.ToString(dt[0].P2TimeAtAddress).Trim();
                        if ((TimeAtAddress != "0") || (TimeAtAddress != ""))
                        {
                            if (TimeAtAddress.Contains(" "))
                                TimeAtAddress = TimeAtAddress.Substring(0, TimeAtAddress.IndexOf(" "));
                        }
                    }
                    XmlAttP2howLongAtAddress.Value = Convert.ToString(TimeAtAddress);
                    node.Attributes.Append(XmlAttP2howLongAtAddress);



                    XmlAttribute XmlAttP2homePhone = doc.CreateAttribute("homePhone");
                    XmlAttP2homePhone.Value = Convert.ToString(dt[0].P2PhoneNumber);
                    node.Attributes.Append(XmlAttP2homePhone);

                    XmlAttribute XmlAttP2dateOfBirth = doc.CreateAttribute("dateOfBirth");
                    XmlAttP2dateOfBirth.Value = Convert.ToString(dt[0].P2DOB);
                    node.Attributes.Append(XmlAttP2dateOfBirth);

                    XmlAttribute XmlAttP2driversLicenseNumber = doc.CreateAttribute("driversLicenseNumber");
                    XmlAttP2driversLicenseNumber.Value = Convert.ToString(dt[0].P2DriversLicenseNo);
                    node.Attributes.Append(XmlAttP2driversLicenseNumber);

                    XmlAttribute XmlAttP2driversLicenseState = doc.CreateAttribute("driversLicenseState");
                    XmlAttP2driversLicenseState.Value = Convert.ToString(dt[0].P2DriversLicenseState);
                    node.Attributes.Append(XmlAttP2driversLicenseState);

                    XmlAttribute XmlAttP2CreditScore = doc.CreateAttribute("checkCreditScore");
                    XmlAttP2CreditScore.Value = "";
                    node.Attributes.Append(XmlAttP2CreditScore);

                    XmlAttribute XmlAttP2OwnershipType = doc.CreateAttribute("OwnershipType");
                    if (!Convert.IsDBNull(dt[0].P1LivingStatus))
                    {
                        if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Rent")
                        {
                            XmlAttP2OwnershipType.Value = "Rent";
                        }
                        else if (Convert.ToString(dt[0].P1LivingStatus).Trim() == "Own")
                        {
                            XmlAttP2OwnershipType.Value = "Own";
                        }
                    }
                    node.Attributes.Append(XmlAttP2OwnershipType);
                }

                #endregion

                #endregion

                #region Fees

                nodelist = doc.GetElementsByTagName("Fees");
                node = nodelist.Item(0);

                XmlAttribute xmlAttFeesID = doc.CreateAttribute("id");
                xmlAttFeesID.Value = "-1";
                node.Attributes.Append(xmlAttFeesID);

                XmlAttribute XmlAttmonthlyProcessingLimit = doc.CreateAttribute("monthlyProcessingLimit");
                XmlAttmonthlyProcessingLimit.Value = Convert.ToString(dt[0].MonthlyVolume);
                node.Attributes.Append(XmlAttmonthlyProcessingLimit);

                XmlAttribute XmlAttmonthlyAverageTicket = doc.CreateAttribute("monthlyAverageTicket");
                XmlAttmonthlyAverageTicket.Value = Convert.ToString(dt[0].AverageTicket);
                node.Attributes.Append(XmlAttmonthlyAverageTicket);

                XmlAttribute XmlAttmonthlyMinimumDiscountFee = doc.CreateAttribute("monthlyMinimumDiscountFee");
                XmlAttmonthlyMinimumDiscountFee.Value = Convert.ToString(dt[0].MonMin);
                node.Attributes.Append(XmlAttmonthlyMinimumDiscountFee);

                XmlAttribute XmlAttmonthlyHighTicket = doc.CreateAttribute("monthlyHighTicket");
                XmlAttmonthlyHighTicket.Value = Convert.ToString(dt[0].MaxTicket);
                node.Attributes.Append(XmlAttmonthlyHighTicket);

                XmlAttribute XmlAttaVSFee = doc.CreateAttribute("aVSFee");
                XmlAttaVSFee.Value = Convert.ToString(dt[0].AVS);
                node.Attributes.Append(XmlAttaVSFee);

                XmlAttribute XmlAttaCHReturnItemFee = doc.CreateAttribute("aCHReturnItemFee");
                XmlAttaCHReturnItemFee.Value = "25.00";
                node.Attributes.Append(XmlAttaCHReturnItemFee);

                XmlAttribute XmlAttaCHChangeFee = doc.CreateAttribute("aCHChangeFee");
                XmlAttaCHChangeFee.Value = "25.00";
                node.Attributes.Append(XmlAttaCHChangeFee);

                XmlAttribute XmlAttretrievalRequestFee = doc.CreateAttribute("retrievalRequestFee");
                XmlAttretrievalRequestFee.Value = Convert.ToString(dt[0].RetrievalFee);
                node.Attributes.Append(XmlAttretrievalRequestFee);

                XmlAttribute XmlAttchargebackFee = doc.CreateAttribute("chargebackFee");
                XmlAttchargebackFee.Value = Convert.ToString(dt[0].ChargebackFee);
                node.Attributes.Append(XmlAttchargebackFee);

                XmlAttribute XmlAttpfcRecoveryFee = doc.CreateAttribute("pfcRecoveryFee");
                XmlAttpfcRecoveryFee.Value = "0.00";
                node.Attributes.Append(XmlAttpfcRecoveryFee);

                XmlAttribute XmlAttmtcFRPFee = doc.CreateAttribute("mtcFRPFee");
                XmlAttmtcFRPFee.Value = "0.00";
                node.Attributes.Append(XmlAttmtcFRPFee);

                XmlAttribute XmlAttoverLimitFee = doc.CreateAttribute("overLimitFee");
                XmlAttoverLimitFee.Value = "0.00";
                node.Attributes.Append(XmlAttoverLimitFee);

                XmlAttribute XmlAttuseDiscountInterchange = doc.CreateAttribute("useDiscountInterchange");
                XmlAttuseDiscountInterchange.Value = Convert.ToString(dt[0].Interchange);
                node.Attributes.Append(XmlAttuseDiscountInterchange);

                XmlAttribute XmlAttcreditCardDiscountQualifiedFee = doc.CreateAttribute("creditCardDiscountQualifiedFee");
                XmlAttcreditCardDiscountQualifiedFee.Value = Convert.ToString(dt[0].DiscountRate);
                node.Attributes.Append(XmlAttcreditCardDiscountQualifiedFee);

                XmlAttribute XmlAttcreditCardDiscountMidQualifiedFee = doc.CreateAttribute("creditCardDiscountMidQualifiedFee");
                XmlAttcreditCardDiscountMidQualifiedFee.Value = Convert.ToString(dt[0].DiscRateMidQual);
                node.Attributes.Append(XmlAttcreditCardDiscountMidQualifiedFee);

                XmlAttribute XmlAttcreditCardDiscountNonQualifiedFee = doc.CreateAttribute("creditCardDiscountNonQualifiedFee");
                XmlAttcreditCardDiscountNonQualifiedFee.Value = Convert.ToString(dt[0].DiscRateNonQual);
                node.Attributes.Append(XmlAttcreditCardDiscountNonQualifiedFee);

                XmlAttribute XmlAttdebitDiscountQualifiedFee = doc.CreateAttribute("debitDiscountQualifiedFee");
                XmlAttdebitDiscountQualifiedFee.Value = Convert.ToString(dt[0].DiscRateQualDebit).Trim();
                //XmlAttdebitDiscountQualifiedFee.Value = "";
                node.Attributes.Append(XmlAttdebitDiscountQualifiedFee);

                XmlAttribute XmlAttdebitDiscountMidQualifiedFee = doc.CreateAttribute("debitDiscountMidQualifiedFee");
                //if ((dt[0].DiscountRate.ToString().Trim() != "") && (dt[0].DiscRateMidQual.ToString().Trim() != ""))
                XmlAttdebitDiscountMidQualifiedFee.Value = Convert.ToString(Convert.ToDouble(dt[0].DiscRateQualDebit) + Convert.ToDouble(dt[0].DiscRateMidQual) - Convert.ToDouble(dt[0].DiscountRate));
                //XmlAttdebitDiscountMidQualifiedFee.Value = "";
                node.Attributes.Append(XmlAttdebitDiscountMidQualifiedFee);

                XmlAttribute XmlAttdebitDiscountNonQualifiedFee = doc.CreateAttribute("debitDiscountNonQualifiedFee");
                //if ((dt[0].DiscountRate.ToString().Trim() != "") && (dt[0].DiscRateNonQual.ToString().Trim() != ""))
                XmlAttdebitDiscountNonQualifiedFee.Value = Convert.ToString(Convert.ToDouble(dt[0].DiscRateQualDebit) + Convert.ToDouble(dt[0].DiscRateNonQual) - Convert.ToDouble(dt[0].DiscountRate));
                //XmlAttdebitDiscountNonQualifiedFee.Value = "";
                node.Attributes.Append(XmlAttdebitDiscountNonQualifiedFee);

                XmlAttribute XmlAttbatchHeaderFee = doc.CreateAttribute("batchHeaderFee");
                XmlAttbatchHeaderFee.Value = dt[0].BatchHeader.ToString().Trim();
                node.Attributes.Append(XmlAttbatchHeaderFee);

                XmlAttribute XmlAttvoiceAuthFee = doc.CreateAttribute("voiceAuthFee");
                XmlAttvoiceAuthFee.Value = Convert.ToString(dt[0].VoiceAuth);
                node.Attributes.Append(XmlAttvoiceAuthFee);


                if (!Convert.IsDBNull(dt[0].AnnualFeeCP))
                {
                    XmlAttribute XmlAttannualFee = doc.CreateAttribute("annualFee");
                    if (dt[0].AnnualFeeCP.ToString() != "")
                        XmlAttannualFee.Value = dt[0].AnnualFeeCP.ToString();
                    else
                        XmlAttannualFee.Value = dt[0].AnnualFeeCNP.ToString();
                    node.Attributes.Append(XmlAttannualFee);
                }
                else {
                    XmlAttribute XmlAttannualFee = doc.CreateAttribute("annualFee");
                    XmlAttannualFee.Value = "";
                    node.Attributes.Append(XmlAttannualFee);

                }

                XmlAttribute XmlAttreserveAccountFee = doc.CreateAttribute("reserveAccountFee");
                XmlAttreserveAccountFee.Value = "2.50";
                node.Attributes.Append(XmlAttreserveAccountFee);


                
                
                /*
                if ((!Convert.IsDBNull(dt[0].DebitMonFee)) && (!Convert.IsDBNull(dt[0].DebitTransFee)))
                {
                    if ((Convert.ToString(dt[0].DebitMonFee) != "") && (Convert.ToString(dt[0].DebitTransFee) != ""))
                    {
                        XmlAttribute XmlAttdebitCardAccessFee = doc.CreateAttribute("debitCardAccessFee");
                        XmlAttribute XmlAttdebitTransactionFee = doc.CreateAttribute("debitTransactionFee");

                        XmlAttdebitCardAccessFee.Value = Convert.ToString(dt[0].DebitMonFee).Trim();
                        XmlAttdebitTransactionFee.Value = Convert.ToString(dt[0].DebitTransFee).Trim();

                        node.Attributes.Append(XmlAttdebitCardAccessFee);
                        node.Attributes.Append(XmlAttdebitTransactionFee);
                    }
                }*/
                
                
                
                /*
                if ((!Convert.IsDBNull(dt[0].WirelessAccessFee)) && (!Convert.IsDBNull(dt[0].WirelessTransFee)))
                {
                    if ((dt[0].WirelessAccessFee.ToString().Trim() != "") && (dt[0].WirelessTransFee.ToString() != ""))
                    {
                        XmlAttribute XmlAttWirelessSetupFee = doc.CreateAttribute("WirelessSetupFee");
                        XmlAttribute XmlAttWirelessMonthlyFee = doc.CreateAttribute("WirelessMonthlyFee");
                        XmlAttribute XmlAttWirelessPerAuthFee = doc.CreateAttribute("WirelessPerAuthFee");

                        XmlAttWirelessMonthlyFee.Value = Convert.ToString(dt[0].WirelessAccessFee).Trim();
                        XmlAttWirelessPerAuthFee.Value = Convert.ToString(dt[0].WirelessTransFee).Trim();
                        XmlAttWirelessSetupFee.Value = "35.00";

                        node.Attributes.Append(XmlAttWirelessSetupFee);
                        node.Attributes.Append(XmlAttWirelessMonthlyFee);
                        node.Attributes.Append(XmlAttWirelessPerAuthFee);
                    }
                }*/

                

                if (!Convert.IsDBNull(dt[0].GatewayMonFee))
                {
                    if (Convert.ToString(dt[0].GatewayMonFee)!="")
                    {
                        XmlAttribute XmlAttPaymentGatewaySetupFee = doc.CreateAttribute("PaymentGatewaySetupFee");
                        XmlAttPaymentGatewaySetupFee.Value = "25.00";
                        node.Attributes.Append(XmlAttPaymentGatewaySetupFee);

                        XmlAttribute XmlAttPaymentGatewayMonthlyFee = doc.CreateAttribute("PaymentGatewayMonthlyFee");
                        XmlAttPaymentGatewayMonthlyFee.Value = Convert.ToString(dt[0].GatewayMonFee);
                        node.Attributes.Append(XmlAttPaymentGatewayMonthlyFee);

                        XmlAttribute XmlAttPaymentGatewayPerAuthFee = doc.CreateAttribute("PaymentGatewayPerAuthFee");
                        XmlAttPaymentGatewayPerAuthFee.Value = Convert.ToString(dt[0].GatewayTransFee);
                        node.Attributes.Append(XmlAttPaymentGatewayPerAuthFee);
                    }
                }

                XmlAttribute XmlAttcustomerService = doc.CreateAttribute("customerService");
                XmlAttcustomerService.Value = Convert.ToString(dt[0].CustServFee);
                node.Attributes.Append(XmlAttcustomerService);

                XmlAttribute XmlAtttAndEAuth = doc.CreateAttribute("tAndEAuth");
                XmlAtttAndEAuth.Value = "0.00";
                node.Attributes.Append(XmlAtttAndEAuth);

                XmlAttribute XmlAttmonthlyOnlineView = doc.CreateAttribute("monthlyOnlineView");
                XmlAttmonthlyOnlineView.Value = "0.00";
                node.Attributes.Append(XmlAttmonthlyOnlineView);

                XmlAttribute XmlAttbatchClosure = doc.CreateAttribute("batchClosure");
                XmlAttbatchClosure.Value = "0.00";
                node.Attributes.Append(XmlAttbatchClosure);

                XmlAttribute XmlAttdebitCardMonthlyNetworkGateway = doc.CreateAttribute("debitCardMonthlyNetworkGateway");
                XmlAttdebitCardMonthlyNetworkGateway.Value = "0.00";
                node.Attributes.Append(XmlAttdebitCardMonthlyNetworkGateway);

                XmlAttribute XmlAtteBTTransaction = doc.CreateAttribute("eBTTransaction");
                XmlAtteBTTransaction.Value = "0.00";
                node.Attributes.Append(XmlAtteBTTransaction);

                XmlAttribute XmlAtteCommerce = doc.CreateAttribute("e-Commerce");
                XmlAtteCommerce.Value = "0.00";
                node.Attributes.Append(XmlAtteCommerce);

                XmlAttribute XmlAttonlineAccessSingle = doc.CreateAttribute("onlineAccessSingle");
                XmlAttonlineAccessSingle.Value = "0.00";
                node.Attributes.Append(XmlAttonlineAccessSingle);

                XmlAttribute XmlAttonlineAccessChain = doc.CreateAttribute("onlineAccessChain");
                XmlAttonlineAccessChain.Value = "0.00";
                node.Attributes.Append(XmlAttonlineAccessChain);

                XmlAttribute XmlAttMCC_SIC = doc.CreateAttribute("MCC_SIC");
                XmlAttMCC_SIC.Value = "";
                node.Attributes.Append(XmlAttMCC_SIC);

                XmlAttribute XmlAttiAccess = doc.CreateAttribute("iAccess");
                if (!Convert.IsDBNull(dt[0].InternetStmt))
                {
                    if (Convert.ToString(dt[0].InternetStmt) != "")
                    {
                        XmlAttiAccess.Value = "true";
                    }
                    else
                    {
                        XmlAttiAccess.Value = "false";
                    }
                }
                node.Attributes.Append(XmlAttiAccess);

                XmlAttribute XmlAttiAccessFee = doc.CreateAttribute("iAccessFee");
                {
                    XmlAttiAccessFee.Value = dt[0].InternetStmt.ToString().Trim();
                }
                node.Attributes.Append(XmlAttiAccessFee);

                XmlAttribute XmlAttMerchantOrPremierClub = doc.CreateAttribute("MerchantOrPremierClub");
                XmlAttMerchantOrPremierClub.Value = "false";
                node.Attributes.Append(XmlAttMerchantOrPremierClub);

                
                XmlAttribute XmlAttMerchantOrPremierClubFee = doc.CreateAttribute("MerchantOrPremierClubFee");
                XmlAttMerchantOrPremierClubFee.Value = "0.00";
                node.Attributes.Append(XmlAttMerchantOrPremierClubFee);
                

                XmlAttribute XmlAttInterchangePlus = doc.CreateAttribute("InterchangePlus");
                //XmlAttInterchangePlus.Value = Convert.ToString(dt[0].DiscRateQualDebit);
                XmlAttInterchangePlus.Value = "";
                node.Attributes.Append(XmlAttInterchangePlus);

                XmlAttribute XmlAttVisaMCTransactionFee = doc.CreateAttribute("VisaMCTransactionFee");
                XmlAttVisaMCTransactionFee.Value = Convert.ToString(dt[0].TransactionFee).Trim();
                node.Attributes.Append(XmlAttVisaMCTransactionFee);

                XmlAttribute XmlAttTnETransactionFee = doc.CreateAttribute("TnETransactionFee");
                XmlAttTnETransactionFee.Value = Convert.ToString(dt[0].TransactionFee).Trim();
                node.Attributes.Append(XmlAttTnETransactionFee);

                
                XmlAttribute XmlAttEarlyTerminationFee = doc.CreateAttribute("EarlyTerminationFee");
                XmlAttEarlyTerminationFee.Value = "0.00";
                node.Attributes.Append(XmlAttEarlyTerminationFee);
                

                XmlAttribute XmlAttSetupFee = doc.CreateAttribute("SetupFee");
                XmlAttSetupFee.Value = "0.00";
                node.Attributes.Append(XmlAttSetupFee);

                XmlAttribute XmlAttMonthlyAccessFee = doc.CreateAttribute("MonthlyAccessFee");
                XmlAttMonthlyAccessFee.Value = "0.00";
                node.Attributes.Append(XmlAttMonthlyAccessFee);

                XmlAttribute XmlAttPerAuthFee = doc.CreateAttribute("PerAuthFee");
                XmlAttPerAuthFee.Value = Convert.ToString(dt[0].TransactionFee);
                node.Attributes.Append(XmlAttPerAuthFee);

                XmlAttribute XmlAttHighRiskFee = doc.CreateAttribute("HighRiskFee");
                XmlAttHighRiskFee.Value = "0.25";
                node.Attributes.Append(XmlAttHighRiskFee);

                XmlAttribute XmlAttRetailMerchantAnualFee = doc.CreateAttribute("RetailMerchantAnualFee");
                XmlAttRetailMerchantAnualFee.Value = Convert.ToString(dt[0].AnnualFeeCP);
                node.Attributes.Append(XmlAttRetailMerchantAnualFee);

                XmlAttribute XmlAttNonRetailMerchantAnualFee = doc.CreateAttribute("NonRetailMerchantAnualFee");
                XmlAttNonRetailMerchantAnualFee.Value = Convert.ToString(dt[0].AnnualFeeCNP);
                node.Attributes.Append(XmlAttNonRetailMerchantAnualFee);

                XmlAttribute XmlAttPlusDuesAndAssessments = doc.CreateAttribute("PlusDuesAndAssessments");
                XmlAttPlusDuesAndAssessments.Value = "";
                node.Attributes.Append(XmlAttPlusDuesAndAssessments);

                XmlAttribute XmlAttWexVoyagerTranFee = doc.CreateAttribute("WexVoyagerTranFee");
                XmlAttWexVoyagerTranFee.Value = "0.00";
                node.Attributes.Append(XmlAttWexVoyagerTranFee);

                XmlAttribute XmlAttErrRate = doc.CreateAttribute("ErrRate");
                XmlAttErrRate.Value = "0.00";
                node.Attributes.Append(XmlAttErrRate);

                XmlAttribute XmlAttERRTransactionFee = doc.CreateAttribute("ERRTransactionFee");
                XmlAttERRTransactionFee.Value = "0.00";
                node.Attributes.Append(XmlAttERRTransactionFee);

                #endregion
                
                #region Equipment
                nodelist = doc.GetElementsByTagName("Equipment");
                node = nodelist.Item(0);

                XmlAttribute XmlEquipmentid = doc.CreateAttribute("id");
                XmlEquipmentid.Value = "-1";
                node.Attributes.Append(XmlEquipmentid);

                        if (Convert.IsDBNull(dt[0].Gateway))
                        {
                            XmlAttribute XmlAttterminalType = doc.CreateAttribute("terminalType");
                            XmlAttterminalType.Value = "8";
                            node.Attributes.Append(XmlAttterminalType);

                            XmlAttribute XmlAttterminalModel = doc.CreateAttribute("terminalModel");
                            XmlAttterminalModel.Value = "";
                            node.Attributes.Append(XmlAttterminalModel);
                        }
                        else if (Convert.ToString(dt[0].Gateway) == "")
                        {
                            XmlAttribute XmlAttterminalType = doc.CreateAttribute("terminalType");
                            XmlAttterminalType.Value = "8";
                            node.Attributes.Append(XmlAttterminalType);

                            XmlAttribute XmlAttterminalModel = doc.CreateAttribute("terminalModel");
                            XmlAttterminalModel.Value = "";
                            node.Attributes.Append(XmlAttterminalModel);
                        }
                        else
                        {
                            XmlAttribute XmlAttterminalType = doc.CreateAttribute("terminalType");
                            XmlAttterminalType.Value = "";
                            node.Attributes.Append(XmlAttterminalType);

                            XmlAttribute XmlAttterminalModel = doc.CreateAttribute("terminalModel");
                            XmlAttterminalModel.Value = "";
                            node.Attributes.Append(XmlAttterminalModel);
                        }


                 

                

                XmlAttribute XmlAttwirelessType = doc.CreateAttribute("wirelessType");
                XmlAttwirelessType.Value = "4";
                node.Attributes.Append(XmlAttwirelessType);

                XmlAttribute XmlAttsoftwareRequirementNotes = doc.CreateAttribute("softwareRequirementNotes");
                XmlAttsoftwareRequirementNotes.Value = "";
                node.Attributes.Append(XmlAttsoftwareRequirementNotes);

                XmlAttribute XmlAttsoftwareRequirementNetworkVersion = doc.CreateAttribute("softwareRequirementNetworkVersion");
                XmlAttsoftwareRequirementNetworkVersion.Value = "";
                node.Attributes.Append(XmlAttsoftwareRequirementNetworkVersion);

                XmlAttribute XmlAttsoftwareRequirementConnectionType = doc.CreateAttribute("softwareRequirementConnectionType");
                XmlAttsoftwareRequirementConnectionType.Value = "";
                node.Attributes.Append(XmlAttsoftwareRequirementConnectionType);

                XmlAttribute XmlAttsoftwareRequirementConcurrentUsers = doc.CreateAttribute("softwareRequirementConcurrentUsers");
                XmlAttsoftwareRequirementConcurrentUsers.Value = "";
                node.Attributes.Append(XmlAttsoftwareRequirementConcurrentUsers);

                XmlAttribute XmlAttsoftwareRequirementAdditionalIDNumbers = doc.CreateAttribute("softwareRequirementAdditionalIDNumbers");
                XmlAttsoftwareRequirementAdditionalIDNumbers.Value = "";
                node.Attributes.Append(XmlAttsoftwareRequirementAdditionalIDNumbers);

                XmlAttribute XmlAttsoftware = doc.CreateAttribute("software");
                XmlAttsoftware.Value = "";
                node.Attributes.Append(XmlAttsoftware);

                XmlAttribute XmlAttreprogram = doc.CreateAttribute("reprogram");
                XmlAttreprogram.Value = "false";
                node.Attributes.Append(XmlAttreprogram);

                XmlAttribute XmlAttrentQuote = doc.CreateAttribute("rentQuote");
                XmlAttrentQuote.Value = "";
                node.Attributes.Append(XmlAttrentQuote);

                XmlAttribute XmlAtthasPrinter = doc.CreateAttribute("hasPrinter");
                XmlAtthasPrinter.Value = "false";
                node.Attributes.Append(XmlAtthasPrinter);

                XmlAttribute XmlAttprinterModel = doc.CreateAttribute("printerModel");
                XmlAttprinterModel.Value = "";
                node.Attributes.Append(XmlAttprinterModel);

                XmlAttribute XmlAttphoneSystemSplitterNeeded = doc.CreateAttribute("phoneSystemSplitterNeeded");
                XmlAttphoneSystemSplitterNeeded.Value = "false";
                node.Attributes.Append(XmlAttphoneSystemSplitterNeeded);


                XmlAttribute XmlAttphoneSystemDialOut = doc.CreateAttribute("phoneSystemDialOut");
                XmlAttphoneSystemDialOut.Value = "4";
                node.Attributes.Append(XmlAttphoneSystemDialOut);

                XmlAttribute XmlAttPhoneCodeForDialOutComment = doc.CreateAttribute("PhoneCodeForDialOutComment");
                XmlAttPhoneCodeForDialOutComment.Value = "";
                node.Attributes.Append(XmlAttPhoneCodeForDialOutComment);

                XmlAttribute XmlAttphoneSystemCallWaiting = doc.CreateAttribute("phoneSystemCallWaiting");
                XmlAttphoneSystemCallWaiting.Value = "false";
                node.Attributes.Append(XmlAttphoneSystemCallWaiting);

                XmlAttribute XmlAttpaymentGatewayName = doc.CreateAttribute("paymentGatewayName");
                XmlAttpaymentGatewayName.Value = "";
                if (!Convert.IsDBNull(dt[0].Gateway))
                {
                    if (Convert.ToString(dt[0].Gateway) != "")
                    {
                        
                        if (Convert.ToString(dt[0].Gateway).ToLower().Contains("authorize"))
                        {
                            XmlAttpaymentGatewayName.Value = "2";
                        }
                        else if (Convert.ToString(dt[0].Gateway).ToLower().Contains("plug"))
                        {
                            XmlAttpaymentGatewayName.Value = "27";
                        }
                        else if (Convert.ToString(dt[0].Gateway).ToLower().Contains("roam"))
                        {
                            XmlAttpaymentGatewayName.Value = "51";
                        }
                        else
                        {
                            XmlAttpaymentGatewayName.Value = "";
                        }
                    }
                    else
                    {

                        XmlAttpaymentGatewayName.Value = "";

                    }
                }
                else {
                    XmlAttpaymentGatewayName.Value = "";
                }

                node.Attributes.Append(XmlAttpaymentGatewayName);

                XmlAttribute XmlAtthasPinPad = doc.CreateAttribute("hasPinPad");
                XmlAtthasPinPad.Value = "false";
                node.Attributes.Append(XmlAtthasPinPad);

                XmlAttribute XmlAttpINPadModel = doc.CreateAttribute("pINPadModel");
                XmlAttpINPadModel.Value = "";
                node.Attributes.Append(XmlAttpINPadModel);

                XmlAttribute XmlAttfrontEndType = doc.CreateAttribute("frontEndType");
                XmlAttfrontEndType.Value = "1";
                node.Attributes.Append(XmlAttfrontEndType);

                XmlAttribute XmlAttfrontEndComment = doc.CreateAttribute("frontEndComment");
                XmlAttfrontEndComment.Value = "";
                node.Attributes.Append(XmlAttfrontEndComment);

                XmlAttribute XmlAttdebitAddendumAttached = doc.CreateAttribute("debitAddendumAttached");
                XmlAttdebitAddendumAttached.Value = "false";
                node.Attributes.Append(XmlAttdebitAddendumAttached);

                XmlAttribute XmlAttautomaticTimeZone = doc.CreateAttribute("automaticTimeZone");
                XmlAttautomaticTimeZone.Value = "";
                node.Attributes.Append(XmlAttautomaticTimeZone);

                XmlAttribute XmlAttautomaticTime = doc.CreateAttribute("automaticTime");
                XmlAttautomaticTime.Value = "";
                node.Attributes.Append(XmlAttautomaticTime);

                XmlAttribute XmlAttautomaticClose = doc.CreateAttribute("automaticClose");
                XmlAttautomaticClose.Value = "false";
                node.Attributes.Append(XmlAttautomaticClose);

                XmlAttribute XmlAttisEquipmentARU = doc.CreateAttribute("isEquipmentARU");
                XmlAttisEquipmentARU.Value = "false";
                node.Attributes.Append(XmlAttisEquipmentARU);

                XmlAttribute XmlAttNeedsStarterKit = doc.CreateAttribute("NeedsStarterKit");
                XmlAttNeedsStarterKit.Value = "false";
                node.Attributes.Append(XmlAttNeedsStarterKit);

                XmlAttribute XmlAttImprinterType = doc.CreateAttribute("ImprinterType");
                XmlAttImprinterType.Value = "-99";
                node.Attributes.Append(XmlAttImprinterType);

                XmlAttribute XmlAttPlate = doc.CreateAttribute("Plate");
                XmlAttPlate.Value = "false";
                node.Attributes.Append(XmlAttPlate);

                XmlAttribute XmlAttPlateQuality = doc.CreateAttribute("PlateQuality");
                XmlAttPlateQuality.Value = "0";
                node.Attributes.Append(XmlAttPlateQuality);

                XmlAttribute XmlAttToDoInstall = doc.CreateAttribute("ToDoInstall");
                XmlAttToDoInstall.Value = "false";
                node.Attributes.Append(XmlAttToDoInstall);

                XmlAttribute XmlAttToDoInstallComment = doc.CreateAttribute("ToDoInstallComment");
                XmlAttToDoInstallComment.Value = "";
                node.Attributes.Append(XmlAttToDoInstallComment);

                XmlAttribute XmlAttSuppliesEquipment = doc.CreateAttribute("SuppliesEquipment");
                XmlAttSuppliesEquipment.Value = "false";
                node.Attributes.Append(XmlAttSuppliesEquipment);

                XmlAttribute XmlAttSuppliesEquipmentComment = doc.CreateAttribute("SuppliesEquipmentComment");
                XmlAttSuppliesEquipmentComment.Value = "";
                node.Attributes.Append(XmlAttSuppliesEquipmentComment);

                XmlAttribute XmlAttShoppingCart = doc.CreateAttribute("ShoppingCart");
                XmlAttShoppingCart.Value = "";
                node.Attributes.Append(XmlAttShoppingCart);

                XmlAttribute XmlAttCellphoneCarrier = doc.CreateAttribute("CellphoneCarrier");
                XmlAttCellphoneCarrier.Value = "";
                node.Attributes.Append(XmlAttCellphoneCarrier);

                XmlAttribute XmlAttCellphoneManufacturer = doc.CreateAttribute("CellphoneManufacturer");
                XmlAttCellphoneManufacturer.Value = "";
                node.Attributes.Append(XmlAttCellphoneManufacturer);

                XmlAttribute XmlAttCellphoneModel = doc.CreateAttribute("CellphoneModel");
                XmlAttCellphoneModel.Value = "";
                node.Attributes.Append(XmlAttCellphoneModel);

                XmlAttribute XmlAttCellphoneNumber = doc.CreateAttribute("CellphoneNumber");
                XmlAttCellphoneNumber.Value = "";
                node.Attributes.Append(XmlAttCellphoneNumber);

                XmlAttribute XmlAttAccountType = doc.CreateAttribute("AccountType");
                XmlAttAccountType.Value = "";
                node.Attributes.Append(XmlAttAccountType);

                XmlAttribute XmlAttOrderType = doc.CreateAttribute("OrderType");
                XmlAttOrderType.Value = "";
                node.Attributes.Append(XmlAttOrderType);

                XmlAttribute XmlAttCheckService = doc.CreateAttribute("CheckService");
                XmlAttCheckService.Value = "0";
                node.Attributes.Append(XmlAttCheckService);

                XmlAttribute XmlAttCheckServiceAccountType = doc.CreateAttribute("CheckServiceAccountType");
                XmlAttCheckServiceAccountType.Value = "";
                node.Attributes.Append(XmlAttCheckServiceAccountType);

                XmlAttribute XmlAttCheckServiceProductType = doc.CreateAttribute("CheckServiceProductType");
                XmlAttCheckServiceProductType.Value = "";
                node.Attributes.Append(XmlAttCheckServiceProductType);

                XmlAttribute XmlAttCheckServiceDiscountRate = doc.CreateAttribute("CheckServiceDiscountRate");
                XmlAttCheckServiceDiscountRate.Value = "0";
                node.Attributes.Append(XmlAttCheckServiceDiscountRate);

                XmlAttribute XmlAttCheckServiceMonthlyMin = doc.CreateAttribute("CheckServiceMonthlyMin");
                XmlAttCheckServiceMonthlyMin.Value = "0";
                node.Attributes.Append(XmlAttCheckServiceMonthlyMin);

                XmlAttribute XmlAttCheckServiceStatementFee = doc.CreateAttribute("CheckServiceStatementFee");
                XmlAttCheckServiceStatementFee.Value = "0";
                node.Attributes.Append(XmlAttCheckServiceStatementFee);

                XmlAttribute XmlAttCheckServiceTranFee = doc.CreateAttribute("CheckServiceTranFee");
                XmlAttCheckServiceTranFee.Value = "0";
                node.Attributes.Append(XmlAttCheckServiceTranFee);

                #endregion

                #region LeaseInfo

                nodelist = doc.GetElementsByTagName("LeaseInfo");
                node = nodelist.Item(0);

                XmlAttribute XmlLeaseInfoid = doc.CreateAttribute("id");
                XmlLeaseInfoid.Value = "-1";
                node.Attributes.Append(XmlLeaseInfoid);

                XmlAttribute XmlAttleaseTerm = doc.CreateAttribute("leaseTerm");
                XmlAttleaseTerm.Value = "";
                node.Attributes.Append(XmlAttleaseTerm);

                XmlAttribute XmlAttEquipmentServiceProgram = doc.CreateAttribute("EquipmentServiceProgram");
                XmlAttEquipmentServiceProgram.Value = "true";
                node.Attributes.Append(XmlAttEquipmentServiceProgram);

                XmlAttribute XmlAtttotalMonthlyLeaseCharge = doc.CreateAttribute("totalMonthlyLeaseCharge");
                XmlAtttotalMonthlyLeaseCharge.Value = "0.00";
                node.Attributes.Append(XmlAtttotalMonthlyLeaseCharge);

                XmlAttribute XmlAttNonCancelableLease = doc.CreateAttribute("NonCancelableLease");
                XmlAttNonCancelableLease.Value = "false";
                node.Attributes.Append(XmlAttNonCancelableLease);

                XmlAttribute XmlAttAnnualTaxHandlingFee = doc.CreateAttribute("AnnualTaxHandlingFee");
                XmlAttAnnualTaxHandlingFee.Value = "0.00";
                node.Attributes.Append(XmlAttAnnualTaxHandlingFee);

                XmlAttribute XmlAttIsLeaseTermsAcknowledged = doc.CreateAttribute("IsLeaseTermsAcknowledged");
                XmlAttIsLeaseTermsAcknowledged.Value = "false";
                node.Attributes.Append(XmlAttIsLeaseTermsAcknowledged);

                XmlAttribute XmlAttIsLeasePaymentDeductionAcknowledged = doc.CreateAttribute("IsLeasePaymentDeductionAcknowledged");
                XmlAttIsLeasePaymentDeductionAcknowledged.Value = "false";
                node.Attributes.Append(XmlAttIsLeasePaymentDeductionAcknowledged);

                XmlAttribute XmlAttIsLeaseNonCancelableAcknowledged = doc.CreateAttribute("IsLeaseNonCancelableAcknowledged");
                XmlAttIsLeaseNonCancelableAcknowledged.Value = "false";
                node.Attributes.Append(XmlAttIsLeaseNonCancelableAcknowledged);

                XmlAttribute XmlAttIsLeaseEquipmentDeliveryAcknowledged = doc.CreateAttribute("IsLeaseEquipmentDeliveryAcknowledged");
                XmlAttIsLeaseEquipmentDeliveryAcknowledged.Value = "false";
                node.Attributes.Append(XmlAttIsLeaseEquipmentDeliveryAcknowledged);

                #endregion

                #region Terminals

                //nodelist = doc.GetElementsByTagName("Terminals");

                //parentNode = nodelist.Item(0);
                //node = parentNode.ChildNodes.Item(0);

                #region Terminal

                ns = "http://www.ipaymentinc.com";


                        if (Convert.IsDBNull(dt[0].Gateway))
                        {
                            //Get the root node in the XML file to which the new principal 2 tree will be added.
                            //In this XML, the root node is AppData.
                            XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminals");
                            //Create another Principal node <Principal> with the same tag names for principal 1.
                            parentNode = nodelistParent.Item(0);
                            XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Terminal", ns);

                            parentNode.AppendChild(childNode);

                            node = parentNode.ChildNodes.Item(0);

                            XmlAttribute XmlAttTerminalID = doc.CreateAttribute("TerminalID");
                            XmlAttTerminalID.Value = "8";
                            node.Attributes.Append(XmlAttTerminalID);

                            XmlAttribute XmlAttTerminalBrand = doc.CreateAttribute("TerminalBrand");
                            XmlAttTerminalBrand.Value = "Other";
                            node.Attributes.Append(XmlAttTerminalBrand);

                            XmlAttribute XmlAttTerminalModel = doc.CreateAttribute("TerminalModel");
                            XmlAttTerminalModel.Value = "See Notes";
                            node.Attributes.Append(XmlAttTerminalModel);

                            XmlAttribute XmlAttFrontEndName = doc.CreateAttribute("FrontEndName");
                            XmlAttFrontEndName.Value = "Omaha";
                            node.Attributes.Append(XmlAttFrontEndName);

                            XmlAttribute XmlAttModelID = doc.CreateAttribute("ModelID");
                            XmlAttModelID.Value = "705";
                            node.Attributes.Append(XmlAttModelID);

                            XmlAttribute XmlAttPrinterID = doc.CreateAttribute("PrinterID");
                            XmlAttPrinterID.Value = "-1";
                            node.Attributes.Append(XmlAttPrinterID);
                            /*
                            XmlAttribute XmlAttPrinterName = doc.CreateAttribute("PrinterName");
                            XmlAttPrinterName.Value = "";
                            node.Attributes.Append(XmlAttPrinterName);*/

                            XmlAttribute XmlAttPinPadID = doc.CreateAttribute("PinPadID");
                            XmlAttPinPadID.Value = "-1";
                            node.Attributes.Append(XmlAttPinPadID);

                            XmlAttribute XmlAttReProgram = doc.CreateAttribute("ReProgram");
                            XmlAttReProgram.Value = "false";
                            node.Attributes.Append(XmlAttReProgram);

                            XmlAttribute XmlAttNotes = doc.CreateAttribute("Notes");
                            XmlAttNotes.Value = "Other";
                            node.Attributes.Append(XmlAttNotes);

                            XmlAttribute XmlAttWirelessID = doc.CreateAttribute("WirelessID");
                            XmlAttWirelessID.Value = "4";
                            node.Attributes.Append(XmlAttWirelessID);

                            XmlAttribute XmlAttAutomaticClose = doc.CreateAttribute("AutomaticClose");
                            XmlAttAutomaticClose.Value = "false";
                            node.Attributes.Append(XmlAttAutomaticClose);

                            XmlAttribute XmlAttAutomaticCloseTime = doc.CreateAttribute("AutomaticCloseTime");
                            XmlAttAutomaticCloseTime.Value = "0001-01-01T00:00:00";
                            node.Attributes.Append(XmlAttAutomaticCloseTime);

                            XmlAttribute XmlAttTimeZoneID = doc.CreateAttribute("TimeZoneID");
                            XmlAttTimeZoneID.Value = "0";
                            node.Attributes.Append(XmlAttTimeZoneID);

                            XmlAttribute XmlAttPhoneDialOutCodeID = doc.CreateAttribute("PhoneDialOutCodeID");
                            XmlAttPhoneDialOutCodeID.Value = "4";
                            node.Attributes.Append(XmlAttPhoneDialOutCodeID);

                            XmlAttribute XmlAttFrontEndID = doc.CreateAttribute("FrontEndID");
                            XmlAttFrontEndID.Value = "1";
                            node.Attributes.Append(XmlAttFrontEndID);

                            XmlAttribute XmlAttIsLease = doc.CreateAttribute("IsLease");
                            XmlAttIsLease.Value = "false";
                            node.Attributes.Append(XmlAttIsLease);

                            XmlAttribute XmlAttIsCashSale = doc.CreateAttribute("IsCashSale");
                            XmlAttIsCashSale.Value = "false";
                            node.Attributes.Append(XmlAttIsCashSale);

                        }else if (Convert.ToString(dt[0].Gateway) == "")
                        {
                                                        //Get the root node in the XML file to which the new principal 2 tree will be added.
                            //In this XML, the root node is AppData.
                            XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminals");
                            //Create another Principal node <Principal> with the same tag names for principal 1.
                            parentNode = nodelistParent.Item(0);

                            XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Terminal", ns);

                            parentNode.AppendChild(childNode);

                            node = parentNode.ChildNodes.Item(0);

                            XmlAttribute XmlAttTerminalID = doc.CreateAttribute("TerminalID");
                            XmlAttTerminalID.Value = "8";
                            node.Attributes.Append(XmlAttTerminalID);

                            XmlAttribute XmlAttTerminalBrand = doc.CreateAttribute("TerminalBrand");
                            XmlAttTerminalBrand.Value = "Other";
                            node.Attributes.Append(XmlAttTerminalBrand);

                            XmlAttribute XmlAttTerminalModel = doc.CreateAttribute("TerminalModel");
                            XmlAttTerminalModel.Value = "See Notes";
                            node.Attributes.Append(XmlAttTerminalModel);

                            XmlAttribute XmlAttFrontEndName = doc.CreateAttribute("FrontEndName");
                            XmlAttFrontEndName.Value = "Omaha";
                            node.Attributes.Append(XmlAttFrontEndName);

                            XmlAttribute XmlAttModelID = doc.CreateAttribute("ModelID");
                            XmlAttModelID.Value = "705";
                            node.Attributes.Append(XmlAttModelID);

                            XmlAttribute XmlAttPrinterID = doc.CreateAttribute("PrinterID");
                            XmlAttPrinterID.Value = "-1";
                            node.Attributes.Append(XmlAttPrinterID);

                            XmlAttribute XmlAttPrinterName = doc.CreateAttribute("PrinterName");
                            XmlAttPrinterName.Value = "";
                            node.Attributes.Append(XmlAttPrinterName);

                            XmlAttribute XmlAttPinPadID = doc.CreateAttribute("PinPadID");
                            XmlAttPinPadID.Value = "-1";
                            node.Attributes.Append(XmlAttPinPadID);

                            XmlAttribute XmlAttReProgram = doc.CreateAttribute("ReProgram");
                            XmlAttReProgram.Value = "false";
                            node.Attributes.Append(XmlAttReProgram);

                            XmlAttribute XmlAttNotes = doc.CreateAttribute("Notes");
                            XmlAttNotes.Value = "";
                            node.Attributes.Append(XmlAttNotes);

                            XmlAttribute XmlAttWirelessID = doc.CreateAttribute("WirelessID");
                            XmlAttWirelessID.Value = "4";
                            node.Attributes.Append(XmlAttWirelessID);

                            XmlAttribute XmlAttEqpmautomaticClose = doc.CreateAttribute("automaticClose");
                            XmlAttEqpmautomaticClose.Value = "false";
                            node.Attributes.Append(XmlAttEqpmautomaticClose);

                            XmlAttribute XmlAttAutomaticCloseTime = doc.CreateAttribute("AutomaticCloseTime");
                            XmlAttAutomaticCloseTime.Value = "0001-01-01T00:00:00";
                            node.Attributes.Append(XmlAttAutomaticCloseTime);

                            XmlAttribute XmlAttTimeZoneID = doc.CreateAttribute("TimeZoneID");
                            XmlAttTimeZoneID.Value = "0";
                            node.Attributes.Append(XmlAttTimeZoneID);

                            XmlAttribute XmlAttPhoneDialOutCodeID = doc.CreateAttribute("PhoneDialOutCodeID");
                            XmlAttPhoneDialOutCodeID.Value = "4";
                            node.Attributes.Append(XmlAttPhoneDialOutCodeID);

                            XmlAttribute XmlAttFrontEndID = doc.CreateAttribute("FrontEndID");
                            XmlAttFrontEndID.Value = "1";
                            node.Attributes.Append(XmlAttFrontEndID);

                            XmlAttribute XmlAttIsLease = doc.CreateAttribute("IsLease");
                            XmlAttIsLease.Value = "false";
                            node.Attributes.Append(XmlAttIsLease);

                            XmlAttribute XmlAttIsCashSale = doc.CreateAttribute("IsCashSale");
                            XmlAttIsCashSale.Value = "false";
                            node.Attributes.Append(XmlAttIsCashSale);
                        }

                #endregion
                
                #region DownloadInformation

                ns = "http://www.ipaymentinc.com";


                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    /*
                    nodelist = doc.GetElementsByTagName("Terminal");

                    parentNode = nodelist.Item(0);
                    node = parentNode.ChildNodes.Item(0);*/

                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "DownloadInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(0);

                    XmlAttribute XmlAttApplication = doc.CreateAttribute("Application");
                    XmlAttApplication.Value = "";
                    node.Attributes.Append(XmlAttApplication);

                    XmlAttribute XmlAttDLAdditionalFeatures = doc.CreateAttribute("AdditionalFeatures");
                    XmlAttDLAdditionalFeatures.Value = "";
                    node.Attributes.Append(XmlAttDLAdditionalFeatures);

                    XmlAttribute XmlAttDLTiedMultiMerchantNumber = doc.CreateAttribute("TiedMultiMerchantNumber");
                    XmlAttDLTiedMultiMerchantNumber.Value = "";
                    node.Attributes.Append(XmlAttDLTiedMultiMerchantNumber);

                    XmlAttribute XmlAttDLSpecialPrompts = doc.CreateAttribute("SpecialPrompts");
                    XmlAttDLSpecialPrompts.Value = "";
                    node.Attributes.Append(XmlAttDLSpecialPrompts);

                    XmlAttribute XmlAttDLSpecialFeaturesProvider = doc.CreateAttribute("SpecialFeaturesProvider");
                    XmlAttDLSpecialFeaturesProvider.Value = "";
                    node.Attributes.Append(XmlAttDLSpecialFeaturesProvider);

                    XmlAttribute XmlAttApplicationDDLTID = doc.CreateAttribute("ApplicationDDLTID");
                    XmlAttApplicationDDLTID.Value = "";
                    node.Attributes.Append(XmlAttApplicationDDLTID);

                    XmlAttribute XmlAttApplicationDDLPhone = doc.CreateAttribute("ApplicationDDLPhone");
                    XmlAttApplicationDDLPhone.Value = "";
                    node.Attributes.Append(XmlAttApplicationDDLPhone);

                    XmlAttribute XmlAttDLDateFileBuilt = doc.CreateAttribute("DateFileBuilt");
                    XmlAttDLDateFileBuilt.Value = "0001-01-01T00:00:00";
                    node.Attributes.Append(XmlAttDLDateFileBuilt);
                    /*
                    XmlAttribute XmlAttLoadHolder = doc.CreateAttribute("LoadHolder");
                    XmlAttLoadHolder.Value = "";
                    node.Attributes.Append(XmlAttLoadHolder);*/

                    XmlAttribute XmlAttLoadInstructions = doc.CreateAttribute("LoadInstructions");
                    XmlAttLoadInstructions.Value = "";
                    node.Attributes.Append(XmlAttLoadInstructions);

                    XmlAttribute XmlAttDLEquipmentLocation = doc.CreateAttribute("EquipmentLocation");
                    XmlAttDLEquipmentLocation.Value = "";
                    node.Attributes.Append(XmlAttDLEquipmentLocation);

                    XmlAttribute XmlAttDLPaywareMobileDevice = doc.CreateAttribute("PaywareMobileDevice");
                    XmlAttDLPaywareMobileDevice.Value = "G3";
                    node.Attributes.Append(XmlAttDLPaywareMobileDevice);

                    XmlAttribute XmlAttDLPaywayreMobileDeviceVersion = doc.CreateAttribute("PaywayreMobileDeviceVersion");
                    XmlAttDLPaywayreMobileDeviceVersion.Value = "";
                    node.Attributes.Append(XmlAttDLPaywayreMobileDeviceVersion);
                }else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    /*
                    nodelist = doc.GetElementsByTagName("Terminal");

                    parentNode = nodelist.Item(0);
                    node = parentNode.ChildNodes.Item(0);*/

                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "DownloadInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(0);

                    XmlAttribute XmlAttApplication = doc.CreateAttribute("Application");
                    XmlAttApplication.Value = "";
                    node.Attributes.Append(XmlAttApplication);

                    XmlAttribute XmlAttDLAdditionalFeatures = doc.CreateAttribute("AdditionalFeatures");
                    XmlAttDLAdditionalFeatures.Value = "";
                    node.Attributes.Append(XmlAttDLAdditionalFeatures);

                    XmlAttribute XmlAttDLTiedMultiMerchantNumber = doc.CreateAttribute("TiedMultiMerchantNumber");
                    XmlAttDLTiedMultiMerchantNumber.Value = "";
                    node.Attributes.Append(XmlAttDLTiedMultiMerchantNumber);

                    XmlAttribute XmlAttDLSpecialPrompts = doc.CreateAttribute("SpecialPrompts");
                    XmlAttDLSpecialPrompts.Value = "";
                    node.Attributes.Append(XmlAttDLSpecialPrompts);

                    XmlAttribute XmlAttDLSpecialFeaturesProvider = doc.CreateAttribute("SpecialFeaturesProvider");
                    XmlAttDLSpecialFeaturesProvider.Value = "";
                    node.Attributes.Append(XmlAttDLSpecialFeaturesProvider);

                    XmlAttribute XmlAttApplicationDDLTID = doc.CreateAttribute("ApplicationDDLTID");
                    XmlAttApplicationDDLTID.Value = "";
                    node.Attributes.Append(XmlAttApplicationDDLTID);

                    XmlAttribute XmlAttApplicationDDLPhone = doc.CreateAttribute("ApplicationDDLPhone");
                    XmlAttApplicationDDLPhone.Value = "";
                    node.Attributes.Append(XmlAttApplicationDDLPhone);

                    XmlAttribute XmlAttDLDateFileBuilt = doc.CreateAttribute("DateFileBuilt");
                    XmlAttDLDateFileBuilt.Value = "0001-01-01T00:00:00";
                    node.Attributes.Append(XmlAttDLDateFileBuilt);
                    /*
                    XmlAttribute XmlAttLoadHolder = doc.CreateAttribute("LoadHolder");
                    XmlAttLoadHolder.Value = "";
                    node.Attributes.Append(XmlAttLoadHolder);*/

                    XmlAttribute XmlAttLoadInstructions = doc.CreateAttribute("LoadInstructions");
                    XmlAttLoadInstructions.Value = "";
                    node.Attributes.Append(XmlAttLoadInstructions);

                    XmlAttribute XmlAttDLEquipmentLocation = doc.CreateAttribute("EquipmentLocation");
                    XmlAttDLEquipmentLocation.Value = "";
                    node.Attributes.Append(XmlAttDLEquipmentLocation);

                    XmlAttribute XmlAttDLPaywareMobileDevice = doc.CreateAttribute("PaywareMobileDevice");
                    XmlAttDLPaywareMobileDevice.Value = "G3";
                    node.Attributes.Append(XmlAttDLPaywareMobileDevice);

                    XmlAttribute XmlAttDLPaywayreMobileDeviceVersion = doc.CreateAttribute("PaywayreMobileDeviceVersion");
                    XmlAttDLPaywayreMobileDeviceVersion.Value = "";
                    node.Attributes.Append(XmlAttDLPaywayreMobileDeviceVersion);
                }

                #endregion

                #region WirelessInformation

                /*
                nodelist = doc.GetElementsByTagName("WirelessInformation");

                node = nodelist.Item(0);*/

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "WirelessInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(1);

                    XmlAttribute XmlAttWirelessCommType = doc.CreateAttribute("WirelessCommType");
                    XmlAttWirelessCommType.Value = "DialUp";
                    node.Attributes.Append(XmlAttWirelessCommType);

                    XmlAttribute XmlAttWirelessCarrier = doc.CreateAttribute("WirelessCarrier");
                    XmlAttWirelessCarrier.Value = "";
                    node.Attributes.Append(XmlAttWirelessCarrier);

                    XmlAttribute XmlAttWirelessTcpIpGateway = doc.CreateAttribute("WirelessTcpIpGateway");
                    XmlAttWirelessTcpIpGateway.Value = "";
                    node.Attributes.Append(XmlAttWirelessTcpIpGateway);

                    XmlAttribute XmlAttDLWirelessDatawireDID = doc.CreateAttribute("WirelessDatawireDID");
                    XmlAttDLWirelessDatawireDID.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessDatawireDID);

                    XmlAttribute XmlAttDLWirelessSIMCard = doc.CreateAttribute("WirelessSIMCard");
                    XmlAttDLWirelessSIMCard.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessSIMCard);

                    XmlAttribute XmlAttDLWirelessESNNumber = doc.CreateAttribute("WirelessESNNumber");
                    XmlAttDLWirelessESNNumber.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessESNNumber);

                    XmlAttribute XmlAttDLWirelessMANNumber = doc.CreateAttribute("WirelessMANNumber");
                    XmlAttDLWirelessMANNumber.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessMANNumber);
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "WirelessInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(1);

                    XmlAttribute XmlAttWirelessCommType = doc.CreateAttribute("WirelessCommType");
                    XmlAttWirelessCommType.Value = "DialUp";
                    node.Attributes.Append(XmlAttWirelessCommType);

                    XmlAttribute XmlAttWirelessCarrier = doc.CreateAttribute("WirelessCarrier");
                    XmlAttWirelessCarrier.Value = "";
                    node.Attributes.Append(XmlAttWirelessCarrier);

                    XmlAttribute XmlAttWirelessTcpIpGateway = doc.CreateAttribute("WirelessTcpIpGateway");
                    XmlAttWirelessTcpIpGateway.Value = "";
                    node.Attributes.Append(XmlAttWirelessTcpIpGateway);

                    XmlAttribute XmlAttDLWirelessDatawireDID = doc.CreateAttribute("WirelessDatawireDID");
                    XmlAttDLWirelessDatawireDID.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessDatawireDID);

                    XmlAttribute XmlAttDLWirelessSIMCard = doc.CreateAttribute("WirelessSIMCard");
                    XmlAttDLWirelessSIMCard.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessSIMCard);

                    XmlAttribute XmlAttDLWirelessESNNumber = doc.CreateAttribute("WirelessESNNumber");
                    XmlAttDLWirelessESNNumber.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessESNNumber);

                    XmlAttribute XmlAttDLWirelessMANNumber = doc.CreateAttribute("WirelessMANNumber");
                    XmlAttDLWirelessMANNumber.Value = "";
                    node.Attributes.Append(XmlAttDLWirelessMANNumber);
                }

                #endregion

                #region ShippingInformation


                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "ShippingInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(2);
                    /*
                    nodelist = doc.GetElementsByTagName("ShippingInformation");

                    node = nodelist.Item(0);*/

                    XmlAttribute XmlAttShipTo = doc.CreateAttribute("ShipTo");
                    XmlAttShipTo.Value = "AgentOffice";
                    node.Attributes.Append(XmlAttShipTo);

                    XmlAttribute XmlAttShippingServiceType = doc.CreateAttribute("ShippingServiceType");
                    XmlAttShippingServiceType.Value = "Ground";
                    node.Attributes.Append(XmlAttShippingServiceType);

                    XmlAttribute XmlAttDLSerialNumber = doc.CreateAttribute("SerialNumber");
                    XmlAttDLSerialNumber.Value = "";
                    node.Attributes.Append(XmlAttDLSerialNumber);

                    XmlAttribute XmlAttDLDeploymentDate = doc.CreateAttribute("DeploymentDate");
                    XmlAttDLDeploymentDate.Value = "0001-01-01T00:00:00";
                    node.Attributes.Append(XmlAttDLDeploymentDate);

                    XmlAttribute XmlAttDLCondition = doc.CreateAttribute("Condition");
                    XmlAttDLCondition.Value = "New";
                    node.Attributes.Append(XmlAttDLCondition);

                    XmlAttribute XmlAttDLOrigin = doc.CreateAttribute("Origin");
                    XmlAttDLOrigin.Value = "0";
                    node.Attributes.Append(XmlAttDLOrigin);
                }else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "ShippingInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(2);
                    /*
                    nodelist = doc.GetElementsByTagName("ShippingInformation");

                    node = nodelist.Item(0);*/

                    XmlAttribute XmlAttShipTo = doc.CreateAttribute("ShipTo");
                    XmlAttShipTo.Value = "AgentOffice";
                    node.Attributes.Append(XmlAttShipTo);

                    XmlAttribute XmlAttShippingServiceType = doc.CreateAttribute("ShippingServiceType");
                    XmlAttShippingServiceType.Value = "Ground";
                    node.Attributes.Append(XmlAttShippingServiceType);

                    XmlAttribute XmlAttDLSerialNumber = doc.CreateAttribute("SerialNumber");
                    XmlAttDLSerialNumber.Value = "";
                    node.Attributes.Append(XmlAttDLSerialNumber);

                    XmlAttribute XmlAttDLDeploymentDate = doc.CreateAttribute("DeploymentDate");
                    XmlAttDLDeploymentDate.Value = "0001-01-01T00:00:00";
                    node.Attributes.Append(XmlAttDLDeploymentDate);

                    XmlAttribute XmlAttDLCondition = doc.CreateAttribute("Condition");
                    XmlAttDLCondition.Value = "New";
                    node.Attributes.Append(XmlAttDLCondition);

                    XmlAttribute XmlAttDLOrigin = doc.CreateAttribute("Origin");
                    XmlAttDLOrigin.Value = "0";
                    node.Attributes.Append(XmlAttDLOrigin);
                }

                #endregion

                #region PinPadInformation

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "PinPadInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(3);

                    /*
                nodelist = doc.GetElementsByTagName("PinPadInformation");

                node = nodelist.Item(0);*/

                    XmlAttribute XmlAttPinPadModelName = doc.CreateAttribute("PinPadModelName");
                    XmlAttPinPadModelName.Value = "";
                    node.Attributes.Append(XmlAttPinPadModelName);

                    XmlAttribute XmlAttPinPadCount = doc.CreateAttribute("PinPadCount");
                    XmlAttPinPadCount.Value = "1";
                    node.Attributes.Append(XmlAttPinPadCount);

                    XmlAttribute XmlAttDLCheckReaderCount = doc.CreateAttribute("CheckReaderCount");
                    XmlAttDLCheckReaderCount.Value = "0";
                    node.Attributes.Append(XmlAttDLCheckReaderCount);

                    XmlAttribute XmlAttDLCardReaderCount = doc.CreateAttribute("CardReaderCount");
                    XmlAttDLCardReaderCount.Value = "0";
                    node.Attributes.Append(XmlAttDLCardReaderCount);

                    XmlAttribute XmlAttDLFreeTerminalProgram = doc.CreateAttribute("FreeTerminalProgram");
                    XmlAttDLFreeTerminalProgram.Value = "false";
                    node.Attributes.Append(XmlAttDLFreeTerminalProgram);

                    XmlAttribute XmlAttDLEncryptionMethod = doc.CreateAttribute("EncryptionMethod");
                    XmlAttDLEncryptionMethod.Value = "Blank";
                    node.Attributes.Append(XmlAttDLEncryptionMethod);
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "PinPadInformation", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(3);

                    /*
                nodelist = doc.GetElementsByTagName("PinPadInformation");

                node = nodelist.Item(0);*/

                    XmlAttribute XmlAttPinPadModelName = doc.CreateAttribute("PinPadModelName");
                    XmlAttPinPadModelName.Value = "";
                    node.Attributes.Append(XmlAttPinPadModelName);

                    XmlAttribute XmlAttPinPadCount = doc.CreateAttribute("PinPadCount");
                    XmlAttPinPadCount.Value = "1";
                    node.Attributes.Append(XmlAttPinPadCount);

                    XmlAttribute XmlAttDLCheckReaderCount = doc.CreateAttribute("CheckReaderCount");
                    XmlAttDLCheckReaderCount.Value = "0";
                    node.Attributes.Append(XmlAttDLCheckReaderCount);

                    XmlAttribute XmlAttDLCardReaderCount = doc.CreateAttribute("CardReaderCount");
                    XmlAttDLCardReaderCount.Value = "0";
                    node.Attributes.Append(XmlAttDLCardReaderCount);

                    XmlAttribute XmlAttDLFreeTerminalProgram = doc.CreateAttribute("FreeTerminalProgram");
                    XmlAttDLFreeTerminalProgram.Value = "false";
                    node.Attributes.Append(XmlAttDLFreeTerminalProgram);

                    XmlAttribute XmlAttDLEncryptionMethod = doc.CreateAttribute("EncryptionMethod");
                    XmlAttDLEncryptionMethod.Value = "Blank";
                    node.Attributes.Append(XmlAttDLEncryptionMethod);
                }

                #endregion

                #region IsDeploy

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployPinPad", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(4);

                    node.InnerText = "false";
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployPinPad", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(4);

                    node.InnerText = "false";
                }

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployPrinter", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(5);

                    node.InnerText = "false";
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployPrinter", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(5);

                    node.InnerText = "false";
                }

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployCheckReader", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(6);

                    node.InnerText = "false";
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployCheckReader", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(6);

                    node.InnerText = "false";
                }

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployCardReader", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(7);

                    node.InnerText = "false";
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployCardReader", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(7);

                    node.InnerText = "false";
                }

                if (Convert.IsDBNull(dt[0].Gateway))
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployPinPadCable", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(8);

                    node.InnerText = "false";
                }
                else if (Convert.ToString(dt[0].Gateway) == "")
                {
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("Terminal");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    parentNode = nodelistParent.Item(0);
                    XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "IsDeployPinPadCable", ns);

                    parentNode.AppendChild(childNode);

                    node = parentNode.ChildNodes.Item(8);

                    node.InnerText = "false";
                }

                #endregion

                #endregion

                #region Gateways

                nodelist = doc.GetElementsByTagName("Gateways");

                //parentNode = nodelist.Item(0);
                //node = parentNode.ChildNodes.Item(0);

                #region Gateway

                if (!Convert.IsDBNull(dt[0].Gateway))
                {
                    if (Convert.ToString(dt[0].Gateway) != "")
                    {
                        //Get the root node in the XML file to which the new principal 2 tree will be added.
                        //In this XML, the root node is AppData.
                        XmlNodeList nodelistParent = doc.GetElementsByTagName("Gateways");
                        //Create another Principal node <Principal> with the same tag names for principal 1.
                        parentNode = nodelistParent.Item(0);
                        XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Gateway", ns);

                        parentNode.AppendChild(childNode);

                        node = parentNode.ChildNodes.Item(0);

                        XmlAttribute XmlAttGatewayID = doc.CreateAttribute("GatewayID");
                        if (Convert.ToString(dt[0].Gateway).ToLower().Contains("authorize"))
                        {
                            XmlAttGatewayID.Value = "2";
                        }else if (Convert.ToString(dt[0].Gateway).ToLower().Contains("plug"))
                        {
                            XmlAttGatewayID.Value = "27";
                        }else if (Convert.ToString(dt[0].Gateway).ToLower().Contains("roam"))
                        {
                            XmlAttGatewayID.Value = "51";
                        } else 
                        {
                            XmlAttGatewayID.Value = "";
                        }
                        node.Attributes.Append(XmlAttGatewayID);

                        XmlAttribute XmlAttGatewayShoppingCart = doc.CreateAttribute("ShoppingCart");
                        XmlAttGatewayShoppingCart.Value = "";
                        node.Attributes.Append(XmlAttGatewayShoppingCart);

                        XmlAttribute XmlAttDeviceType = doc.CreateAttribute("DeviceType");
                        XmlAttDeviceType.Value = "";
                        node.Attributes.Append(XmlAttDeviceType);

                        XmlAttribute XmlAttDeviceTypeVersion = doc.CreateAttribute("DeviceTypeVersion");
                        XmlAttDeviceTypeVersion.Value = "";
                        node.Attributes.Append(XmlAttDeviceTypeVersion);

                        XmlAttribute XmlAttGatewayNotes = doc.CreateAttribute("Notes");
                        XmlAttGatewayNotes.Value = "";
                        node.Attributes.Append(XmlAttGatewayNotes);

                        XmlAttribute XmlAttGatewayFrontEndID = doc.CreateAttribute("FrontEndID");
                        XmlAttGatewayFrontEndID.Value = "1";
                        node.Attributes.Append(XmlAttGatewayFrontEndID);

                        XmlAttribute XmlAttGatewayFrontEndName = doc.CreateAttribute("FrontEndName");
                        XmlAttGatewayFrontEndName.Value = "Omaha";
                        node.Attributes.Append(XmlAttGatewayFrontEndName);
                        
                        nodelistParent = doc.GetElementsByTagName("Gateway");
                        //Create another Principal node <Principal> with the same tag names for principal 1.
                        parentNode = nodelistParent.Item(0);
                        childNode = doc.CreateNode(XmlNodeType.Element, "DownloadInformation", ns);

                        parentNode.AppendChild(childNode);

                        node = parentNode.ChildNodes.Item(0);

                        XmlAttribute XmlAttAdditionalFeatures = doc.CreateAttribute("AdditionalFeatures");
                        XmlAttAdditionalFeatures.Value = "";
                        node.Attributes.Append(XmlAttAdditionalFeatures);

                        XmlAttribute XmlAttSpecialPrompts = doc.CreateAttribute("SpecialPrompts");
                        XmlAttSpecialPrompts.Value = "";
                        node.Attributes.Append(XmlAttSpecialPrompts);

                        XmlAttribute XmlAttSpecialFeaturesProvider = doc.CreateAttribute("SpecialFeaturesProvider");
                        XmlAttSpecialFeaturesProvider.Value = "";
                        node.Attributes.Append(XmlAttSpecialFeaturesProvider);

                        XmlAttribute XmlAttDateFileBuilt = doc.CreateAttribute("DateFileBuilt");
                        XmlAttDateFileBuilt.Value = "0001-01-01T00:00:00";
                        node.Attributes.Append(XmlAttDateFileBuilt);


                        XmlAttribute XmlAttPaywareMobileDevice = doc.CreateAttribute("PaywareMobileDevice");
                        XmlAttPaywareMobileDevice.Value = "";
                        node.Attributes.Append(XmlAttPaywareMobileDevice);

                        XmlAttribute XmlAttPaywayreMobileDeviceVersion = doc.CreateAttribute("PaywayreMobileDeviceVersion");
                        XmlAttPaywayreMobileDeviceVersion.Value = "";
                        node.Attributes.Append(XmlAttPaywayreMobileDeviceVersion);
                        
                    }
                }
                
                #region DownloadInformation

                /*
                if (!Convert.IsDBNull(dt[0].Gateway))
                {
                    if (Convert.ToString(dt[0].Gateway) != "")
                    {
                        nodelist = doc.GetElementsByTagName("Gateway");

                        parentNode = nodelist.Item(0);

                        XmlNode childNodeGwy = doc.CreateNode(XmlNodeType.Element, "DownloadInformation", null);

                        parentNode.AppendChild(childNodeGwy);

                        node = parentNode.ChildNodes.Item(0);


                        parentNode = nodelist.Item(0);
                        node = parentNode.ChildNodes.Item(0);

                        XmlAttribute XmlAttAdditionalFeatures = doc.CreateAttribute("AdditionalFeatures");
                        XmlAttAdditionalFeatures.Value = "";
                        node.Attributes.Append(XmlAttAdditionalFeatures);

                        XmlAttribute XmlAttSpecialPrompts = doc.CreateAttribute("SpecialPrompts");
                        XmlAttSpecialPrompts.Value = "";
                        node.Attributes.Append(XmlAttSpecialPrompts);

                        XmlAttribute XmlAttSpecialFeaturesProvider = doc.CreateAttribute("SpecialFeaturesProvider");
                        XmlAttSpecialFeaturesProvider.Value = "";
                        node.Attributes.Append(XmlAttSpecialFeaturesProvider);

                        XmlAttribute XmlAttDateFileBuilt = doc.CreateAttribute("DateFileBuilt");
                        XmlAttDateFileBuilt.Value = "";
                        node.Attributes.Append(XmlAttDateFileBuilt);


                        XmlAttribute XmlAttPaywareMobileDevice = doc.CreateAttribute("PaywareMobileDevice");
                        XmlAttPaywareMobileDevice.Value = "";
                        node.Attributes.Append(XmlAttPaywareMobileDevice);

                        XmlAttribute XmlAttPaywayreMobileDeviceVersion = doc.CreateAttribute("PaywayreMobileDeviceVersion");
                        XmlAttPaywayreMobileDeviceVersion.Value = "";
                        node.Attributes.Append(XmlAttPaywayreMobileDeviceVersion);
                    }
                }*/
                

                #endregion


                #endregion

                #endregion

                #region Softwares
                /*
                nodelist = doc.GetElementsByTagName("Softwares");

                parentNode = nodelist.Item(0);
                node = parentNode.ChildNodes.Item(0);*/

                /*
                #region Software

                if (!Convert.IsDBNull(dt[0].Gateway))
                {
                    if (Convert.ToString(dt[0].Gateway) != "")
                    {

                        XmlNodeList nodelistParent = doc.GetElementsByTagName("Softwares");
                        //Create another Principal node <Principal> with the same tag names for principal 1.
                        parentNode = nodelistParent.Item(0);
                        XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Software", ns);

                        parentNode.AppendChild(childNode);

                        node = parentNode.ChildNodes.Item(0);

                        string strGatewayType = "";
                        XmlAttribute XmlAttGatewayType = doc.CreateAttribute("GatewayType");
                        if (Convert.ToString(dt[0].Platform).ToLower().Contains("tsys"))
                        {
                            strGatewayType = "TSYS";
                        }
                        else if (Convert.ToString(dt[0].Platform).ToLower().Contains("global"))
                        {
                            strGatewayType = "Global";
                        }
                        else
                        {
                            strGatewayType = "Datawire";
                        }
                        XmlAttGatewayType.Value = strGatewayType;
                        node.Attributes.Append(XmlAttGatewayType);

                        XmlAttribute XmlAttSoftwareID = doc.CreateAttribute("SoftwareID");
                        XmlAttSoftwareID.Value = "";
                        node.Attributes.Append(XmlAttSoftwareID);

                        string strFrontEndID = "";
                        XmlAttribute XmlAttSoftwareFrontEndID = doc.CreateAttribute("FrontEndID");
                        if (Convert.ToString(dt[0].Platform).ToLower().Contains("omaha"))
                        {
                            strFrontEndID = "1";
                        }
                        else if (Convert.ToString(dt[0].Platform).ToLower().Contains("nashville"))
                        {
                            strFrontEndID = "2";
                        }
                        else if (Convert.ToString(dt[0].Platform).ToLower().Contains("north"))
                        {
                            strFrontEndID = "3";
                        }
                        else if (Convert.ToString(dt[0].Platform).ToLower().Contains("vital"))
                        {
                            strFrontEndID = "4";
                        }
                        else if (Convert.ToString(dt[0].Platform).ToLower().Contains("byPass"))
                        {
                            strFrontEndID = "6";
                        }
                        else {
                            strFrontEndID = "5";
                        }
                        XmlAttSoftwareFrontEndID.Value = strFrontEndID;
                        node.Attributes.Append(XmlAttSoftwareFrontEndID);

                        XmlAttribute XmlAttSoftwareVersion = doc.CreateAttribute("SoftwareVersion");
                        XmlAttSoftwareVersion.Value = "";
                        node.Attributes.Append(XmlAttSoftwareVersion);

                        XmlAttribute XmlAttTCPIP = doc.CreateAttribute("TCPIP");
                        XmlAttTCPIP.Value = "";
                        node.Attributes.Append(XmlAttTCPIP);

                        XmlAttribute XmlAttSoftwareNotes = doc.CreateAttribute("Notes");
                        XmlAttSoftwareNotes.Value = "";
                        node.Attributes.Append(XmlAttSoftwareNotes);

                        XmlAttribute XmlAttSoftwareFrontEndName = doc.CreateAttribute("FrontEndName");
                        XmlAttSoftwareFrontEndName.Value = "";
                        node.Attributes.Append(XmlAttSoftwareFrontEndName);
                    }
                }


                #endregion
                */
                #endregion

                #region ExistingMerchant

                nodelist = doc.GetElementsByTagName("ExistingMerchant");
                node = nodelist.Item(0);

                XmlAttribute XmlAttamericanExpress = doc.CreateAttribute("americanExpress");

                if (Convert.ToString(dt[0].PrevAmexNum) != "")
                {
                    XmlAttamericanExpress.Value = Convert.ToString(dt[0].PrevAmexNum);
                }
                else
                {
                    XmlAttamericanExpress.Value = "";
                }

                node.Attributes.Append(XmlAttamericanExpress);


                XmlAttribute XmlAttdiners = doc.CreateAttribute("diners");
                XmlAttdiners.Value = "";
                node.Attributes.Append(XmlAttdiners);

                XmlAttribute XmlAttdiscover = doc.CreateAttribute("discover");

                if (Convert.ToString(dt[0].PrevDiscoverNum) != "")
                {

                    XmlAttdiscover.Value = Convert.ToString(dt[0].PrevDiscoverNum);

                }
                else {
                    XmlAttdiscover.Value = "";
                }

                node.Attributes.Append(XmlAttdiscover);

                XmlAttribute XmlAttjCB = doc.CreateAttribute("jCB");

                if (Convert.ToString(dt[0].PrevDiscoverNum) != "")
                {

                    XmlAttjCB.Value = Convert.ToString(dt[0].PrevDiscoverNum);

                }
                else
                {
                    XmlAttjCB.Value = "";
                }

                node.Attributes.Append(XmlAttjCB);
                /*
                XmlElement xmlElemAmex = doc.CreateElement("Amex");
                XmlNodeList nodelistexistMerchant = doc.GetElementsByTagName("ExistingMerchant");
                XmlNode nodeExistMerchant = nodelistexistMerchant.Item(0);
                doc.InsertAfter(xmlElemAmex, nodeExistMerchant);*/

                //if (Convert.ToString(dt[0].PrevAmexNum) != "")
                //{
                    //Get the root node in the XML file to which the new Amex tree will be added.
                    //In this XML, the root node is AppData.
                    //XmlNodeList nodelistParent = doc.GetElementsByTagName("ExistingMerchant");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    //parentNode = nodelistParent.Item(0);
                    
                    //XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Amex", null);

                    //XmlElement childElement = doc.CreateElement("FirstName");
                    //childElement.InnerText = dt[0].P2FirstName;
                    //Append the child element to "element", which is the <principal> node created above
                    //parentNode.AppendChild(childNode);
                    nodelist = doc.GetElementsByTagName("Amex");
                    node = nodelist.Item(0);

                    XmlAttribute XmlAttAmexdiscountRate = doc.CreateAttribute("discountRate");
                    XmlAttAmexdiscountRate.Value = "";
                    node.Attributes.Append(XmlAttAmexdiscountRate);

                    XmlAttribute XmlAttAmexmonthlyFlatFee = doc.CreateAttribute("monthlyFlatFee");
                    XmlAttAmexmonthlyFlatFee.Value = "";
                    node.Attributes.Append(XmlAttAmexmonthlyFlatFee);

                    XmlAttribute XmlAttAmexgrossPayType = doc.CreateAttribute("grossPayType");
                    XmlAttAmexgrossPayType.Value = "";
                    node.Attributes.Append(XmlAttAmexgrossPayType);

                    XmlAttribute XmlAttAmexestimateAnnualVolume = doc.CreateAttribute("estimateAnnualVolume");
                    XmlAttAmexestimateAnnualVolume.Value = "";
                    node.Attributes.Append(XmlAttAmexestimateAnnualVolume);

                    XmlAttribute XmlAttAmexestimateAverageTicket = doc.CreateAttribute("estimateAverageTicket");
                    XmlAttAmexestimateAverageTicket.Value = "";
                    node.Attributes.Append(XmlAttAmexestimateAverageTicket);

                    XmlAttribute XmlAttAmexpayFrequency = doc.CreateAttribute("payFrequency");
                    XmlAttAmexpayFrequency.Value = "";
                    node.Attributes.Append(XmlAttAmexpayFrequency);

                    XmlAttribute XmlAttAmexmerchantInitials = doc.CreateAttribute("merchantInitials");
                    XmlAttAmexmerchantInitials.Value = "";
                    node.Attributes.Append(XmlAttAmexmerchantInitials);

                //}

                //if (Convert.ToString(dt[0].PrevDiscoverNum) != "")
                //{
                    //Get the root node in the XML file to which the new principal 2 tree will be added.
                    //In this XML, the root node is AppData.
                    //XmlNodeList nodelistParent = doc.GetElementsByTagName("ExistingMerchant");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    //parentNode = nodelistParent.Item(0);
                    //XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "Discover", null);

                    //XmlElement childElement = doc.CreateElement("FirstName");
                    //childElement.InnerText = dt[0].P2FirstName;
                    //Append the child element to "element", which is the <principal> node created above
                    //parentNode.AppendChild(childNode);

                    //node = parentNode.ChildNodes.Item(1);
                /*
                    XmlElement xmlElemDiscover = doc.CreateElement("Discover");

                    XmlNodeList nodelistAmex = doc.GetElementsByTagName("Amex");
                    XmlNode nodeAmex = nodelistAmex.Item(0);
                    doc.InsertAfter(xmlElemDiscover, nodeAmex);
                    nodelist = doc.GetElementsByTagName("Discover");
                    node = nodelist.Item(0);*/

                    /*if (Convert.ToString(dt[0].PrevAmexNum) != "")
                    {
                        XmlNodeList nodelistAmex = doc.GetElementsByTagName("Amex");
                        XmlNode nodeAmex = nodelistAmex.Item(0);
                        doc.InsertAfter(xmlElemDiscover, nodeAmex);
                        nodelist = doc.GetElementsByTagName("Discover");
                        node = nodelist.Item(0);
                    }

                    else
                    {

                        XmlNodeList nodelistexistMerchant = doc.GetElementsByTagName("ExistingMerchant");
                        XmlNode nodeExistMerchant = nodelistexistMerchant.Item(0);
                        doc.InsertAfter(xmlElemDiscover, nodeExistMerchant);
                        nodelist = doc.GetElementsByTagName("Discover");
                        node = nodelist.Item(0);
                    }*/
                    nodelist = doc.GetElementsByTagName("Discover");
                    node = nodelist.Item(0);

                    XmlAttribute XmlAttDiscdiscountRate = doc.CreateAttribute("discountRate");
                    XmlAttDiscdiscountRate.Value = "";
                    node.Attributes.Append(XmlAttDiscdiscountRate);

                    XmlAttribute XmlAttDisctransactionFee = doc.CreateAttribute("transactionFee");
                    XmlAttDisctransactionFee.Value = "";
                    node.Attributes.Append(XmlAttDisctransactionFee);

                    XmlAttribute XmlAttDiscmembershipFee = doc.CreateAttribute("membershipFee");
                    XmlAttDiscmembershipFee.Value = "";
                    node.Attributes.Append(XmlAttDiscmembershipFee);

                //}

                #endregion

                #region BankInformation

                nodelist = doc.GetElementsByTagName("BankInformation");
                node = nodelist.Item(0);

                XmlAttribute XmlAttBankInfoname = doc.CreateAttribute("name");
                XmlAttBankInfoname.Value = Convert.ToString(dt[0].BankName).Trim();
                node.Attributes.Append(XmlAttBankInfoname);

                XmlAttribute XmlAttBankInfoaddress = doc.CreateAttribute("address");
                XmlAttBankInfoaddress.Value = Convert.ToString(dt[0].BankAddress).Trim();
                node.Attributes.Append(XmlAttBankInfoaddress);

                XmlAttribute XmlAttBankInfoaddress2 = doc.CreateAttribute("address2");
                XmlAttBankInfoaddress2.Value = "";
                node.Attributes.Append(XmlAttBankInfoaddress2);

                XmlAttribute XmlAttBankInfocity = doc.CreateAttribute("city");
                XmlAttBankInfocity.Value = Convert.ToString(dt[0].BankCity).Trim();
                node.Attributes.Append(XmlAttBankInfocity);

                XmlAttribute XmlAttBankInfostate = doc.CreateAttribute("state");
                XmlAttBankInfostate.Value = Convert.ToString(dt[0].BankState).Trim();
                node.Attributes.Append(XmlAttBankInfostate);

                XmlAttribute XmlAttBankInfozip = doc.CreateAttribute("zip");
                XmlAttBankInfozip.Value = Convert.ToString(dt[0].BankZip).Trim();
                node.Attributes.Append(XmlAttBankInfozip);

                XmlAttribute XmlAttBankInfobranch = doc.CreateAttribute("branch");
                XmlAttBankInfobranch.Value = "";
                node.Attributes.Append(XmlAttBankInfobranch);

                XmlAttribute XmlAttBankInfobranchPhone = doc.CreateAttribute("branchPhone");
                XmlAttBankInfobranchPhone.Value = Convert.ToString(dt[0].BankPhone).Trim();
                node.Attributes.Append(XmlAttBankInfobranchPhone);

                XmlAttribute XmlAttBankInfocontactName = doc.CreateAttribute("contactName");
                XmlAttBankInfocontactName.Value = "";
                node.Attributes.Append(XmlAttBankInfocontactName);

                XmlAttribute XmlAttBankInfotransRoutingNumber = doc.CreateAttribute("transRoutingNumber");
                XmlAttBankInfotransRoutingNumber.Value = Convert.ToString(dt[0].BankRoutingNumber).Trim();
                node.Attributes.Append(XmlAttBankInfotransRoutingNumber);

                XmlAttribute XmlAttBankInfodDA = doc.CreateAttribute("dDA");
                XmlAttBankInfodDA.Value = Convert.ToString(dt[0].BankAccountNumber).Trim();
                node.Attributes.Append(XmlAttBankInfodDA);

                #endregion

                #region SiteSurvey


                nodelist = doc.GetElementsByTagName("SiteSurvey");
                node = nodelist.Item(0);

                XmlAttribute XmlAttSurveydate = doc.CreateAttribute("date");
                XmlAttSurveydate.Value = "";
                node.Attributes.Append(XmlAttSurveydate);

                XmlAttribute XmlAttSurveytypeOfBuilding = doc.CreateAttribute("typeOfBuilding");
                XmlAttSurveytypeOfBuilding.Value = "";
                node.Attributes.Append(XmlAttSurveytypeOfBuilding);

                XmlAttribute XmlAttSurveysquareFootage = doc.CreateAttribute("squareFootage");
                XmlAttSurveysquareFootage.Value = "-1";
                node.Attributes.Append(XmlAttSurveysquareFootage);

                XmlAttribute XmlAttSurveyinspectorComments = doc.CreateAttribute("inspectorComments");
                XmlAttSurveyinspectorComments.Value = "";
                node.Attributes.Append(XmlAttSurveyinspectorComments);




                #endregion

                #region ServiceEnrollment

                nodelist = doc.GetElementsByTagName("ServiceEnrollment");
                node = nodelist.Item(0);

                XmlAttribute XmlAttServiceApplicationID = doc.CreateAttribute("ApplicationID");
                XmlAttServiceApplicationID.Value = "-1";
                node.Attributes.Append(XmlAttServiceApplicationID);

                XmlAttribute XmlAttDebitCardServices = doc.CreateAttribute("DebitCardServices");
                XmlAttDebitCardServices.Value = "false";
                node.Attributes.Append(XmlAttDebitCardServices);

                XmlAttribute XmlAttETB_FNS = doc.CreateAttribute("ETB_FNS");
                XmlAttETB_FNS.Value = "false";
                node.Attributes.Append(XmlAttETB_FNS);

                XmlAttribute XmlAttETB_FNS_Number = doc.CreateAttribute("ETB_FNS_Number");
                XmlAttETB_FNS_Number.Value = "";
                node.Attributes.Append(XmlAttETB_FNS_Number);

                XmlAttribute XmlAttCheckServices = doc.CreateAttribute("CheckServices");
                XmlAttCheckServices.Value = "false";
                node.Attributes.Append(XmlAttCheckServices);

                XmlAttribute XmlAttLeaseServices = doc.CreateAttribute("LeaseServices");
                XmlAttLeaseServices.Value = "false";
                node.Attributes.Append(XmlAttLeaseServices);

                XmlAttribute XmlAttGiftCardServices = doc.CreateAttribute("GiftCardServices");
                XmlAttGiftCardServices.Value = "false";
                node.Attributes.Append(XmlAttGiftCardServices);

                XmlAttribute XmlAttWexVoyagerServices = doc.CreateAttribute("WexVoyagerServices");
                XmlAttWexVoyagerServices.Value = "false";
                node.Attributes.Append(XmlAttWexVoyagerServices);

                XmlAttribute XmlAttWirelessServices = doc.CreateAttribute("WirelessServices");
                XmlAttWirelessServices.Value = "false";
                node.Attributes.Append(XmlAttWirelessServices);

                XmlAttribute XmlAttGatewayServices = doc.CreateAttribute("GatewayServices");
                XmlAttGatewayServices.Value = "false";
                node.Attributes.Append(XmlAttGatewayServices);

                #endregion

                #region CardAcceptance

                nodelist = doc.GetElementsByTagName("CardAcceptance");
                node = nodelist.Item(0);

                XmlAttribute XmlAttCardAcceptanceApplicationID = doc.CreateAttribute("ApplicationID");
                XmlAttCardAcceptanceApplicationID.Value = "-1";
                node.Attributes.Append(XmlAttCardAcceptanceApplicationID);

                XmlAttribute XmlAttCardAcceptanceMCCredit = doc.CreateAttribute("MCCredit");
                XmlAttCardAcceptanceMCCredit.Value = "false";
                node.Attributes.Append(XmlAttCardAcceptanceMCCredit);

                XmlAttribute XmlAttCardAcceptanceMCNonPinDebit = doc.CreateAttribute("MCNonPinDebit");
                XmlAttCardAcceptanceMCNonPinDebit.Value = "false";
                node.Attributes.Append(XmlAttCardAcceptanceMCNonPinDebit);

                XmlAttribute XmlAttCardAcceptanceVisaCredit = doc.CreateAttribute("VisaCredit");
                XmlAttCardAcceptanceVisaCredit.Value = "false";
                node.Attributes.Append(XmlAttCardAcceptanceVisaCredit);

                XmlAttribute XmlAttCardAcceptanceVisaNonPinDebit = doc.CreateAttribute("VisaNonPinDebit");
                XmlAttCardAcceptanceVisaNonPinDebit.Value = "false";
                node.Attributes.Append(XmlAttCardAcceptanceVisaNonPinDebit);

                XmlAttribute XmlAttCardAcceptanceDiscoverCredit = doc.CreateAttribute("DiscoverCredit");
                XmlAttCardAcceptanceDiscoverCredit.Value = "false";
                node.Attributes.Append(XmlAttCardAcceptanceDiscoverCredit);

                XmlAttribute XmlAttCardAcceptanceDiscoverNonPinDebit = doc.CreateAttribute("DiscoverNonPinDebit");
                XmlAttCardAcceptanceDiscoverNonPinDebit.Value = "false";
                node.Attributes.Append(XmlAttCardAcceptanceDiscoverNonPinDebit);

                #endregion
                
                #region SalesRepInfo

                nodelist = doc.GetElementsByTagName("SalesRepInfo");
                node = nodelist.Item(0);

                XmlAttribute XmlAttSalesRepInfoApplicationID = doc.CreateAttribute("ApplicationID");
                XmlAttSalesRepInfoApplicationID.Value = "-1";
                node.Attributes.Append(XmlAttSalesRepInfoApplicationID);

                XmlAttribute XmlAttSalesRepInfoAuth = doc.CreateAttribute("Auth");
                XmlAttSalesRepInfoAuth.Value = "";
                node.Attributes.Append(XmlAttSalesRepInfoAuth);

                XmlAttribute XmlAttSalesRepInfoInerchangeTable = doc.CreateAttribute("InerchangeTable");
                XmlAttSalesRepInfoInerchangeTable.Value = "";
                node.Attributes.Append(XmlAttSalesRepInfoInerchangeTable);

                XmlAttribute XmlAttSalesRepInfoAssoc = doc.CreateAttribute("Assoc");
                XmlAttSalesRepInfoAssoc.Value = "";
                node.Attributes.Append(XmlAttSalesRepInfoAssoc);

                XmlAttribute XmlAttSalesRepInfoGrid = doc.CreateAttribute("Grid");
                XmlAttSalesRepInfoGrid.Value = "";
                node.Attributes.Append(XmlAttSalesRepInfoGrid);

                #endregion

                //string strTestPath = Server.MapPath("test.xml");
                //doc.Save(strTestPath);


                //FileName = "TestAppXML.xml";
                //doc.Load(Server.MapPath(FileName));

                

                //break;

                string strFileName = "iPaySave.xml";
                XmlDocument outputDoc = new XmlDocument();

                outputDoc.Load(Server.MapPath(strFileName));

                string strXml = doc.OuterXml;

                XmlNode cdata = outputDoc.CreateCDataSection(strXml);

                nodelist = outputDoc.GetElementsByTagName("ApplicationXml");
                node = nodelist.Item(0);

                node.AppendChild(cdata);

                //validate the document

                nodelist = outputDoc.GetElementsByTagName("Action");
                node = nodelist.Item(0);

                node.InnerText = "Validate";

                //Validate the xml before saving, this step will return error message if the xml is not validate.


                string xmlRequest = outputDoc.OuterXml;

                string result = ValidateSaveiPay(xmlRequest);

                string strTestPath = Server.MapPath("test.xml");
                doc.Save(strTestPath);     

                /*
                string result = "";

                string URLString = "http://api.ipaymentinc.com/merchantapplications?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(URLString);
                httpReq.Method = "POST";

                httpReq.ContentType = "application/xml";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postData = System.Text.UTF8Encoding.UTF8.GetBytes(xmlRequest);
                httpReq.ContentLength = postData.LongLength;

                Stream writer = httpReq.GetRequestStream();
                writer.Write(postData, 0, Convert.ToInt32(httpReq.ContentLength));

                HttpWebResponse r = null;
                StreamReader re = null;

                System.Xml.XmlDocument xmlResult = new System.Xml.XmlDocument(); 

                r = (HttpWebResponse)httpReq.GetResponse();

                re = new StreamReader(r.GetResponseStream());
                */


                //try
                //{

                /*
                    result = re.ReadToEnd();

                    DisplayMessage(result);

                    r.Close();*/

                   //using (Stream responseStream = r.GetResponseStream())
                    //{

                    //System.Xml.XmlDocument xmlResult = new System.Xml.XmlDocument();

                    //xmlResult.LoadXml(result);

                    //}
                    
                        System.Xml.XmlDocument xmlResult = new System.Xml.XmlDocument();

                        xmlResult.LoadXml(result);

                        string strResultPath = Server.MapPath("iPayResponse.xml");
                        xmlResult.Save(strResultPath);

                        XmlNodeList resultNodelist = xmlResult.GetElementsByTagName("ErrorCount");
                        XmlNode resultNode = resultNodelist.Item(0);

                        string strErrorCount = resultNode.InnerText;

                        if (Convert.ToInt32(strErrorCount) != 0)
                        {
                            DisplayMessage(result);
                        }
                        else
                        {
                            // Save the application and return application id for submission.

                            //DisplayMessage("Total Error Count: " + strErrorCount);
                            nodelist = outputDoc.GetElementsByTagName("Action");
                            node = nodelist.Item(0);

                            node.InnerText = "Save";
                            xmlRequest = outputDoc.OuterXml;

                            result = ValidateSaveiPay(xmlRequest);

                            xmlResult.LoadXml(result);

                            strResultPath = Server.MapPath("iPayResponse.xml");
                            xmlResult.Save(strResultPath);

                            resultNodelist = xmlResult.GetElementsByTagName("ApplicationID");
                            resultNode = resultNodelist.Item(0);

                            string strAppID = resultNode.InnerText;

                            DisplayMessage("AppID: " + strAppID);

                            resultNodelist = xmlResult.GetElementsByTagName("ErrorCount");
                            resultNode = resultNodelist.Item(0);

                            strErrorCount = resultNode.InnerText;
                            
                            if ((Convert.ToInt32(strAppID) != 0) && (Convert.ToInt32(strErrorCount) == 0))
                            {
                                //string submitUrl = "https://api.ipaymentinc.com/merchantapplications/" + strAppID + "/SubmitApplication?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";
                                //string submitUrl = "http://localhost/datafeeds/Services/v1_0/MerchantApplication.svc/xml/" + strAppID + "/SubmitApplication?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";

                                try
                                {
                                    string subresult = SubmitIpayXml(strAppID);

                                    xmlResult.Load(subresult);
                                    strResultPath = Server.MapPath("iPayResponse1.xml");
                                    xmlResult.Save(strResultPath);

                                    resultNodelist = xmlResult.GetElementsByTagName("ErrorCount");
                                    resultNode = resultNodelist.Item(0);

                                    strErrorCount = resultNode.InnerText;



                                    if (Convert.ToInt32(strErrorCount) == 0)
                                    {
                                        DisplayMessage("Xml created. AppID: " + strAppID);
                                        pnlAttachment.Visible = true;
                                        Session["iPayAppID"] = strAppID;
                                    }
                                    else
                                    {
                                        DisplayMessage(result);
                                        DisplayMessage("Xml created. AppID: " + strAppID);
                                        pnlAttachment.Visible = true;
                                        Session["iPayAppID"] = strAppID;
                                    }
                                }
                                catch (Exception err)
                                {

                                    //DisplayMessage(subresult);

                                    CreateLog Log1 = new CreateLog();
                                    Log1.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXML - " + result);
                                }
                            }
                        }



                    //DisplayMessage(result);

                //}*/
                /*catch (Exception err)
                {
                    
                    CreateLog Log = new CreateLog();
                    Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXML - " + err.Message);
                    DisplayMessage(err.Message);
                }*/


                /*
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXML - " + result);*/
                //DisplayMessage(err.Message);


                //doc.Save(Server.MapPath("../IPayment/IPay_" + dt[0].ContactName.Replace(" ", "") + "_" + ContactID + ".xml"));
                //string strMachineName = "www.firstaffiliates.com/PartnerPortal";
                //lblDownload.Visible = true;
                //lblDownload.Font.Size = FontUnit.Medium;
                //lblDownload.Text = "<a class=\"One\" href=\"http://" + strMachineName + "/IPayment/" + strFile + "\">Download File</a>";

                //Converts the XMLdocument to a string.
                /*StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                doc.WriteTo(xw);
                return sw.ToString();*/

                //DisplayMessage("XML Created.");
            }//end if count not 0
            else
                DisplayMessage("XML Creation Failed. Add this application to ACT! and then click on the Load button.");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXML - " + err.Message);
            DisplayMessage(err.Message);
        }
    }

    public string SubmitIpayXml(string strAppID)
    {
        string result = "";
        try
        {
            string submitUrl = "http://api.ipaymentinc.com/merchantapplications/" + strAppID + "/SubmitApplication?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";

            StringBuilder SubmitResults = new StringBuilder();

            SubmitResults.Append("<ApplicationSubmitRequest xmlns=\"http://ipaymentinc.com/MerchantApplications/20110105\">");
            SubmitResults.Append("<LeaseSignature></LeaseSignature>");
            SubmitResults.Append("<Signature1></Signature1>");
            SubmitResults.Append("<Signature2></Signature2>");
            SubmitResults.Append("</ApplicationSubmitRequest>");


            //string submitUrl = "http://localhost/datafeeds/Services/v1_0/MerchantApplication.svc/xml/{}/SubmitApplication?apitoken={}&usertoken={}";

            HttpWebRequest httpReqSub = (HttpWebRequest)WebRequest.Create(submitUrl);
            httpReqSub.Method = "POST";
            httpReqSub.KeepAlive = false;
            httpReqSub.Timeout = 60000;
            httpReqSub.ContentType = "application/xml";

            byte[] postDataSubmit = UTF8Encoding.UTF8.GetBytes(SubmitResults.ToString());
            httpReqSub.ContentLength = postDataSubmit.LongLength;

            Stream writeSubmit = httpReqSub.GetRequestStream();
            writeSubmit.Write(postDataSubmit, 0, Convert.ToInt32(httpReqSub.ContentLength));

            DisplayMessage("Xml created. AppID: " + strAppID);
            pnlAttachment.Visible = true;
            Session["iPayAppID"] = strAppID;

            HttpWebResponse submitResponse = (HttpWebResponse)httpReqSub.GetResponse();

            StreamReader submitReader = new StreamReader(submitResponse.GetResponseStream());

            result = submitReader.ReadToEnd();

            submitResponse.Close();
            
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXMLSubmit - " + err.Message);
            DisplayMessage(err.Message);
        }

        return result;
    }

    public string ValidateSaveiPay(string xmlRequest)
    {
        string result = "";

        try
        {
            string URLString = "http://api.ipaymentinc.com/merchantapplications?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(URLString);
            httpReq.Method = "POST";

            //httpReq.ContentType = "application/x-www-form-urlencoded";
            httpReq.ContentType = "application/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postData = System.Text.UTF8Encoding.UTF8.GetBytes(xmlRequest);
            httpReq.ContentLength = postData.LongLength;

            Stream writer = httpReq.GetRequestStream();
            writer.Write(postData, 0, Convert.ToInt32(httpReq.ContentLength));

            HttpWebResponse r = null;
            StreamReader re = null;

            System.Xml.XmlDocument xmlResult = new System.Xml.XmlDocument();

            r = (HttpWebResponse)httpReq.GetResponse();

            re = new StreamReader(r.GetResponseStream());

            result = re.ReadToEnd();

            r.Close();
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXMLSave - " + err.Message);
            DisplayMessage(err.Message);
        }

        return result;

    }

    //This function creates the ipayment XML file in the customer folder
    public void CreateiPayXML(System.Guid ContactID)
    {
        try
        {
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID.ToString());
            XMLBL IPay = new XMLBL();
            PartnerDS.ACTiPayXMLDataTable dt = IPay.GetIPayXML(ContactID);

            if (dt.Rows.Count > 0)
            {

                string FileName = "IPay.xml";
                XmlDocument doc = new XmlDocument();

                doc.Load(Server.MapPath(FileName));//Load the XML file

                #region General Information

                //Get the nodes with the tag AttachedFileName
                //GetElementsByTagName always returns a list of nodes that match the name specified.
                //Even if there is only one node with the tag name, it will return a node list with one item.                
                XmlNodeList nodelist = doc.GetElementsByTagName("AttachedFileName");
                //To access that item, define XMLNode object
                XmlNode node = nodelist.Item(0);
                //This is the text inside the node returned.
                node.InnerText = "";

                nodelist = doc.GetElementsByTagName("BusName");
                node = nodelist.Item(0);
                node.InnerText = dt[0].COMPANYNAME;

                nodelist = doc.GetElementsByTagName("DBA");
                node = nodelist.Item(0);
                node.InnerText = dt[0].DBA;

                #region Business Address
                //Business Address
                nodelist = doc.GetElementsByTagName("BusAddr");
                node = nodelist.Item(0);
                XmlNode childNode = node.ChildNodes.Item(0);
                childNode.InnerText = dt[0].Address1;
                childNode = node.ChildNodes.Item(1);
                childNode.InnerText = dt[0].CITY;
                childNode = node.ChildNodes.Item(2);
                childNode.InnerText = dt[0].STATE;
                childNode = node.ChildNodes.Item(3);
                childNode.InnerText = dt[0].Country;
                if (dt[0].ZipCode.Length != 5)
                {
                    DisplayMessage("Business Zip code length must be 5 characters long.");
                    return;
                }
                childNode = node.ChildNodes.Item(4);
                childNode.InnerText = dt[0].ZipCode;
                #endregion

                #region Mail Address
                //Mail Address
                nodelist = doc.GetElementsByTagName("MailAddr");
                node = nodelist.Item(0);
                childNode = node.ChildNodes.Item(0);
                childNode.InnerText = dt[0].BillingAddress;
                childNode = node.ChildNodes.Item(1);
                childNode.InnerText = dt[0].BillingCity;
                childNode = node.ChildNodes.Item(2);
                childNode.InnerText = dt[0].BillingState;
                childNode = node.ChildNodes.Item(3);
                childNode.InnerText = dt[0].billingCountry;
                if (dt[0].BillingZipCode.Length != 5)
                {
                    DisplayMessage("Billing Zip code length must be 5 characters long.");
                    return;
                }
                childNode = node.ChildNodes.Item(4);
                childNode.InnerText = dt[0].BillingZipCode;
                #endregion

                string strLOS = dt[0].LOS;
                if ((strLOS != "0") || (strLOS != ""))
                {
                    if (strLOS.Contains(" "))
                        strLOS = strLOS.Substring(0, strLOS.IndexOf(" "));
                }

                nodelist = doc.GetElementsByTagName("LOS");
                node = nodelist.Item(0);
                node.InnerText = strLOS;

                nodelist = doc.GetElementsByTagName("FedTaxId");
                node = nodelist.Item(0);
                node.InnerText = dt[0].FederalTaxID;

                nodelist = doc.GetElementsByTagName("BusPhone");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BusinessPhone;

                nodelist = doc.GetElementsByTagName("BusFax");
                node = nodelist.Item(0);
                node.InnerText = dt[0].Fax;

                nodelist = doc.GetElementsByTagName("CustPhone");
                node = nodelist.Item(0);
                node.InnerText = dt[0].CustServPhone;

                nodelist = doc.GetElementsByTagName("ContactName");
                node = nodelist.Item(0);
                node.InnerText = dt[0].ContactName;

                nodelist = doc.GetElementsByTagName("NumLocs");
                node = nodelist.Item(0);
                node.InnerText = dt[0].NumberOfLocations;

                nodelist = doc.GetElementsByTagName("LOB");
                node = nodelist.Item(0);
                node.InnerText = dt[0].LOB;

                nodelist = doc.GetElementsByTagName("BusHrs");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BusinessHours;

                nodelist = doc.GetElementsByTagName("email");
                node = nodelist.Item(0);
                node.InnerText = dt[0].Email;

                nodelist = doc.GetElementsByTagName("url");
                node = nodelist.Item(0);
                node.InnerText = dt[0].Website;

                #endregion

                #region Business Information

                nodelist = doc.GetElementsByTagName("RetailCardSwipe");
                node = nodelist.Item(0);
                node.InnerText = dt[0].ProcessPctSwiped.ToString();

                nodelist = doc.GetElementsByTagName("RetailManuallyKeyed");
                node = nodelist.Item(0);
                node.InnerText = dt[0].ProcessPctKeyed.ToString();

                nodelist = doc.GetElementsByTagName("Internet");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BusinessPctInternet.ToString();

                nodelist = doc.GetElementsByTagName("Moto");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BusinessPctMailOrder.ToString();

                nodelist = doc.GetElementsByTagName("OwnershipType");
                node = nodelist.Item(0);
                node.InnerText = dt[0].LegalStatus;

                nodelist = doc.GetElementsByTagName("CustRefundPol");
                node = nodelist.Item(0);
                node.InnerText = dt[0].RefundPolicy;

                nodelist = doc.GetElementsByTagName("ProdType");
                node = nodelist.Item(0);
                node.InnerText = dt[0].ProductSold;

                nodelist = doc.GetElementsByTagName("DelTime");
                node = nodelist.Item(0);
                node.InnerText = dt[0].NumDaysDelivered;

                nodelist = doc.GetElementsByTagName("Comments");
                node = nodelist.Item(0);
                node.InnerText = dt[0].AddlComments;

                #endregion

                #region Proc History

                nodelist = doc.GetElementsByTagName("EverProcessed");
                node = nodelist.Item(0);
                if (dt[0].PrevProcessed == "Yes")
                {
                    node.InnerText = "1";

                    nodelist = doc.GetElementsByTagName("WhoProcessed");
                    node = nodelist.Item(0);
                    node.InnerText = dt[0].PrevProcessor;

                    nodelist = doc.GetElementsByTagName("FormerMerchNum");
                    node = nodelist.Item(0);
                    node.InnerText = dt[0].PrevMerchantAcctNo;

                    nodelist = doc.GetElementsByTagName("TerminationReason");
                    node = nodelist.Item(0);
                    node.InnerText = dt[0].ReasonForLeaving;
                }
                else
                {
                    node.InnerText = "0";

                    nodelist = doc.GetElementsByTagName("ProcHistory");
                    XmlNodeList childnodelist = doc.GetElementsByTagName("WhoProcessed");
                    node = childnodelist.Item(0);
                    nodelist.Item(0).RemoveChild(node);

                    childnodelist = doc.GetElementsByTagName("FormerMerchNum");
                    node = childnodelist.Item(0);
                    nodelist.Item(0).RemoveChild(node);

                    childnodelist = doc.GetElementsByTagName("TerminationReason");
                    node = childnodelist.Item(0);
                    nodelist.Item(0).RemoveChild(node);

                }

                nodelist = doc.GetElementsByTagName("EverTerminated");
                node = nodelist.Item(0);
                if (dt[0].CTMF == "Yes")
                {
                    node.InnerText = "1";

                    nodelist = doc.GetElementsByTagName("WhoTerminated");
                    node = nodelist.Item(0);
                    node.InnerText = "Processor Name";
                }
                else
                {
                    node.InnerText = "0";
                    nodelist = doc.GetElementsByTagName("ProcHistory");
                    XmlNodeList childnodelist = doc.GetElementsByTagName("WhoTerminated");
                    node = childnodelist.Item(0);
                    nodelist.Item(0).RemoveChild(node);
                }

                #endregion

                #region Principal

                //Principal Information
                nodelist = doc.GetElementsByTagName("Principal");

                #region Principal 1

                //First Principal
                node = nodelist.Item(0);
                childNode = node.ChildNodes.Item(0);
                childNode.InnerText = dt[0].P1FirstName;
                childNode = node.ChildNodes.Item(1);
                childNode.InnerText = dt[0].P1LastName;
                childNode = node.ChildNodes.Item(2);
                childNode.InnerText = dt[0].P1SSN;
                childNode = node.ChildNodes.Item(3);
                childNode.InnerText = dt[0].P1OwnershipPercent;
                childNode = node.ChildNodes.Item(4);
                childNode.InnerText = dt[0].P1Title;

                //P1 Residence Address
                childNode = node.ChildNodes.Item(5);
                XmlNode P1Node = childNode.ChildNodes.Item(0);
                P1Node.InnerText = dt[0].P1Address;
                P1Node = childNode.ChildNodes.Item(1);
                P1Node.InnerText = dt[0].P1City;
                P1Node = childNode.ChildNodes.Item(2);
                P1Node.InnerText = dt[0].P1State;
                P1Node = childNode.ChildNodes.Item(3);
                P1Node.InnerText = dt[0].P1Country;
                if (dt[0].P1ZipCode.Length != 5)
                {
                    DisplayMessage("Principal #1 Zip code length must be 5 characters long.");
                    return;
                }
                P1Node = childNode.ChildNodes.Item(4);
                P1Node.InnerText = dt[0].P1ZipCode;

                childNode = node.ChildNodes.Item(6);
                childNode.InnerText = dt[0].P1LivingStatus;

                string TimeAtAddress = dt[0].P1TimeAtAddress;
                if ((TimeAtAddress != "0") || (TimeAtAddress != ""))
                {
                    if (TimeAtAddress.Contains(" "))
                        TimeAtAddress = TimeAtAddress.Substring(0, TimeAtAddress.IndexOf(" "));
                }

                childNode = node.ChildNodes.Item(7);
                childNode.InnerText = TimeAtAddress;
                childNode = node.ChildNodes.Item(8);
                childNode.InnerText = dt[0].P1PhoneNumber;
                childNode = node.ChildNodes.Item(9);
                childNode.InnerText = dt[0].P1DOB;
                childNode = node.ChildNodes.Item(10);
                childNode.InnerText = dt[0].P1DriversLicenseNo;
                childNode = node.ChildNodes.Item(11);
                childNode.InnerText = dt[0].P1DriversLicenseState;

                #endregion

                #region Principal 2
                //Second Principal
                //Check if second principal name in ACT is not blank, and then create Principal subtree.
                if (dt[0].P2FirstName != "")
                {
                    //Get the root node in the XML file to which the new principal 2 tree will be added.
                    //In this XML, the root node is AppData.
                    XmlNodeList nodelistParent = doc.GetElementsByTagName("AppData");
                    //Create another Principal node <Principal> with the same tag names for principal 1.
                    XmlElement element = doc.CreateElement("Principal");

                    XmlElement childElement = doc.CreateElement("FirstName");
                    childElement.InnerText = dt[0].P2FirstName;
                    //Append the child element to "element", which is the <principal> node created above
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("LastName");
                    childElement.InnerText = dt[0].P2LastName;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("SSN");
                    childElement.InnerText = dt[0].P2SSN;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("PercentOwnership");
                    childElement.InnerText = dt[0].P2OwnershipPercent;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("Title");
                    childElement.InnerText = dt[0].P2Title;
                    element.AppendChild(childElement);

                    #region P2Address
                    //Address
                    XmlElement childElementAddress = doc.CreateElement("Address");

                    childElement = doc.CreateElement("st_name");
                    childElement.InnerText = dt[0].P2Address;
                    childElementAddress.AppendChild(childElement);

                    childElement = doc.CreateElement("city");
                    childElement.InnerText = dt[0].P2City;
                    childElementAddress.AppendChild(childElement);

                    childElement = doc.CreateElement("state");
                    childElement.InnerText = dt[0].P2State;
                    childElementAddress.AppendChild(childElement);

                    childElement = doc.CreateElement("country");
                    childElement.InnerText = dt[0].P2Country;
                    childElementAddress.AppendChild(childElement);

                    if (dt[0].P2ZipCode.Length != 5)
                    {
                        DisplayMessage("Principal #2 Zip code length must be 5 characters long.");
                        return;
                    }

                    childElement = doc.CreateElement("zip");
                    childElement.InnerText = dt[0].P2ZipCode;
                    //Append the ChildElement which holds the stname, city, zip etc to the <Address> node.
                    /*
                     <Address>
                        <St_Name/>
                        <Zip/>
                        <state/>
                        <Country/>
                     </Address>*/
                    childElementAddress.AppendChild(childElement);
                    //Append the <Address> node to the <Principal> node.
                    element.AppendChild(childElementAddress);
                    //End Address
                    #endregion

                    childElement = doc.CreateElement("AddrType");
                    childElement.InnerText = dt[0].P2LivingStatus;
                    element.AppendChild(childElement);

                    TimeAtAddress = dt[0].P2TimeAtAddress;
                    if ((TimeAtAddress != "0") || (TimeAtAddress != ""))
                    {
                        if (TimeAtAddress.Contains(" "))
                            TimeAtAddress = TimeAtAddress.Substring(0, TimeAtAddress.IndexOf(" "));
                    }
                    childElement = doc.CreateElement("LOS");
                    childElement.InnerText = TimeAtAddress;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("Phone");
                    childElement.InnerText = dt[0].P2PhoneNumber;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("DOB");
                    childElement.InnerText = dt[0].P2DOB;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("LicNum");
                    childElement.InnerText = dt[0].P2DriversLicenseNo;
                    element.AppendChild(childElement);

                    childElement = doc.CreateElement("LicState");
                    childElement.InnerText = dt[0].P2DriversLicenseState;
                    element.AppendChild(childElement);

                    node = nodelist.Item(0);//Principal 1 node
                    XmlNode parentNode = nodelistParent.Item(0);//AppData node
                    parentNode.InsertAfter(element, node);//Insert after principal 1 node                    
                }
                #endregion

                #endregion

                #region RepInfo

                nodelist = doc.GetElementsByTagName("RepName");
                node = nodelist.Item(0);
                node.InnerText = dt[0].RepName;

                nodelist = doc.GetElementsByTagName("RepNum");
                node = nodelist.Item(0);
                node.InnerText = dt[0].RepNum;

                #endregion

                #region CardBasedFees

                nodelist = doc.GetElementsByTagName("CardType");
                node = nodelist.Item(0);
                /*if ( dt[0].CardType.ToLower().Contains("visa") )
                    node.InnerText = "4";
                else if (dt[0].CardType.ToLower().Contains("master"))*/
                node.InnerText = "1";

                nodelist = doc.GetElementsByTagName("DiscFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].DiscountRate;

                nodelist = doc.GetElementsByTagName("MidQualFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].DiscRateMidQual;

                nodelist = doc.GetElementsByTagName("NonQualFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].DiscRateNonQual;

                nodelist = doc.GetElementsByTagName("TransFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].TransactionFee;

                nodelist = doc.GetElementsByTagName("AVS");
                node = nodelist.Item(0);
                node.InnerText = dt[0].AVS;

                nodelist = doc.GetElementsByTagName("VoiceAuth");
                node = nodelist.Item(0);
                node.InnerText = dt[0].VoiceAuth;

                nodelist = doc.GetElementsByTagName("ARU");
                node = nodelist.Item(0);
                node.InnerText = "0";//dt[0].ARU;

                #endregion

                #region AgentValues

                nodelist = doc.GetElementsByTagName("MonthlyStatementFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].CustServFee;

                nodelist = doc.GetElementsByTagName("MonthlyMinDiscFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].MonMin;

                nodelist = doc.GetElementsByTagName("ApplicationFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].AppFee;

                nodelist = doc.GetElementsByTagName("SetupFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].AppSetupFee;

                nodelist = doc.GetElementsByTagName("BatchHeaders");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BatchHeader;

                nodelist = doc.GetElementsByTagName("ExternalGWFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].GatewayMonFee;

                nodelist = doc.GetElementsByTagName("WirelessFee");
                node = nodelist.Item(0);
                node.InnerText = dt[0].WirelessAccess;

                #endregion

                #region MiscAgentValues

                nodelist = doc.GetElementsByTagName("MonthlyProcLimit");
                node = nodelist.Item(0);
                node.InnerText = dt[0].MonthlyVolume;

                nodelist = doc.GetElementsByTagName("AvgTicket");
                node = nodelist.Item(0);
                node.InnerText = dt[0].AverageTicket;

                nodelist = doc.GetElementsByTagName("NeedAmex");
                node = nodelist.Item(0);
                node.InnerText = dt[0].AmexApplied;

                nodelist = doc.GetElementsByTagName("NeedDiscover");
                node = nodelist.Item(0);
                node.InnerText = dt[0].DiscoverApplied;

                #endregion

                #region Equipment

                nodelist = doc.GetElementsByTagName("EquipType");
                node = nodelist.Item(0);
                node.InnerText = "G";

                nodelist = doc.GetElementsByTagName("EquipModel");
                node = nodelist.Item(0);
                node.InnerText = "Others";// dt[0].Equipment;

                nodelist = doc.GetElementsByTagName("EquipID");
                node = nodelist.Item(0);
                node.InnerText = "";//dt[0].TerminalID;
                /*}
                else
                {
                    nodelist = doc.GetElementsByTagName("AppData");
                    XmlNodeList childnodelist = doc.GetElementsByTagName("Equipment");
                    node = childnodelist.Item(0);
                    nodelist.Item(0).RemoveChild(node);
                }*/
                #endregion

                #region CardNums

                nodelist = doc.GetElementsByTagName("AMEX");
                //if (dt[0].PrevAmexNum == "")
                //{
                //XmlNodeList childnodelist = doc.GetElementsByTagName("CardNums");
                //childnodelist.Item(0).RemoveChild(nodelist.Item(0));
                //}
                //else
                //{                
                node = nodelist.Item(0);
                node.InnerText = dt[0].PrevAmexNum;
                //}

                if ((dt[0].PrevDiscoverNum.Length != 15) && (dt[0].PrevDiscoverNum != ""))
                    Response.Write("Discover number length not 15. Please check in ACT.");
                nodelist = doc.GetElementsByTagName("Discover");
                node = nodelist.Item(0);
                node.InnerText = dt[0].PrevDiscoverNum;

                nodelist = doc.GetElementsByTagName("Jcb");
                node = nodelist.Item(0);
                node.InnerText = dt[0].PrevJCBNum;

                #endregion

                #region BankInfo

                nodelist = doc.GetElementsByTagName("BankName");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BankName;

                nodelist = doc.GetElementsByTagName("BankAddr");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BankAddress;

                nodelist = doc.GetElementsByTagName("TransroutingNum");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BankRoutingNumber;

                nodelist = doc.GetElementsByTagName("DDA");
                node = nodelist.Item(0);
                node.InnerText = dt[0].BankAccountNumber;

                #endregion

                //doc.Save(Server.MapPath("IPay_" + dt[0].ContactName.Replace(" ", "") + ".xml"));
                //string strFile = "IPay_" + dt[0].ContactName.Replace(" ", "") + "_" + ContactID + ".xml";

                
                if (FilePath != string.Empty)
                {
                    FilePath = FilePath.ToLower();
                    FilePath = FilePath.Replace("file://s:\\customers", "");
                    FilePath = FilePath.Replace("\\", "/");

                    string strHost = "../../Customers";
                    string strPath = Server.MapPath(strHost + FilePath + "/" + dt[0].P1FirstName.Substring(0, 1) + dt[0].P1LastName + ".xml");
                    doc.Save(strPath);
                }

                //doc.Save(Server.MapPath("../IPayment/IPay_" + dt[0].ContactName.Replace(" ", "") + "_" + ContactID + ".xml"));
                //string strMachineName = "www.firstaffiliates.com/PartnerPortal";
                //lblDownload.Visible = true;
                //lblDownload.Font.Size = FontUnit.Medium;
                //lblDownload.Text = "<a class=\"One\" href=\"http://" + strMachineName + "/IPayment/" + strFile + "\">Download File</a>";

                //Converts the XMLdocument to a string.
                /*StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                doc.WriteTo(xw);
                return sw.ToString();*/

                DisplayMessage("XML Created.");
            }//end if count not 0
            else
                DisplayMessage("XML Creation Failed. Add this application to ACT! and then click on the Load button.");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPaymentXML - " + err.Message);
            DisplayMessage(err.Message);
        }
    }//end function CreateXML

    public void CreateSageXML(System.Guid ContactID)
    {
        try
        {
            //ACTDataBL fp = new ACTDataBL();
            //string FilePath = fp.ReturnCustomerFilePath(ContactID.ToString());
            XMLBL Sage = new XMLBL();
            string retVal = UploadSageXML(ContactID);
            DisplayMessage(retVal);
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "SageXML - " + err.Message);
            DisplayMessage(err.Message);
        }
    }

    public string UploadSageXML(System.Guid ContactID)
    {
        XMLBL Sage = new XMLBL();
        PartnerDS.ACTSageXMLDataTable dt = Sage.GetSageXML(ContactID);

        if (dt.Rows.Count > 0)
        {
            wsSalesCenterBoarding ws = new wsSalesCenterBoarding();

            ws.clsAuthenticationHeaderValue = XMLBL.CreateSageAuthentication(ContactID);

            clsMerchantApplicationV1 app = new clsMerchantApplicationV1();

            #region General Information
            clsAppInfoV1 appInfo = new clsAppInfoV1();
            appInfo.OfficeName = "Commerce Technologies Corporation";
            appInfo.AssociationID = 3849; //AssociationID 
            //appInfo.ContractorID = Convert.ToInt32(dt[0].RepNum.ToString().Trim());
            //appInfo.ContractorName = dt[0].RepName.ToString().Trim();
            appInfo.LeadSourceID = 0; // 2 = internet; 0 = None

            //Merchant info starts here
            if (dt[0].LegalStatus.ToString().Trim().Contains("Corporation"))
                appInfo.CorpTypeID = 3;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("Government"))
                appInfo.CorpTypeID = 7;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("Int'l"))
                appInfo.CorpTypeID = 8;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("Legal/Medical"))
                appInfo.CorpTypeID = 4;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("LLC"))
                appInfo.CorpTypeID = 9;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("Partnership"))
                appInfo.CorpTypeID = 2;
            else if ((dt[0].LegalStatus.ToString().Trim().Contains("Non-Profit")) || (dt[0].LegalStatus.ToString().Trim().Contains("Tax Exempt")))
                appInfo.CorpTypeID = 6;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("Trust"))
                appInfo.CorpTypeID = 5;
            else if (dt[0].LegalStatus.ToString().Trim().Contains("Proprietorship"))
                appInfo.CorpTypeID = 1;

            appInfo.CorpName = dt[0].LegalName.ToString().Trim();
            appInfo.CorpAddress1 = dt[0].BillingAddressLine1.ToString().Trim() + ", " + dt[0].BillingAddressLine2.ToString().Trim();
            appInfo.CorpAddress2 = dt[0].BillingAddressLine2.ToString().Trim();
            appInfo.CorpCity = dt[0].BillingCity.ToString().Trim();
            appInfo.CorpState = dt[0].BillingState.ToString().Trim();
            appInfo.CorpZip = dt[0].BillingZipCode.ToString().Trim();
            appInfo.CorpCountryID = Convert.ToInt32(dt[0].BillingCountryID.ToString().Trim());
            appInfo.CorpPhone = dt[0].BusinessPhone.ToString().Trim();
            if (dt[0].Fax.ToString().Trim() != "")
                appInfo.CorpFax = dt[0].Fax.ToString().Trim();

            appInfo.CorpContactFirstName = dt[0].P1FirstName.ToString().Trim();
            appInfo.CorpContactLastName = dt[0].P1LastName.ToString().Trim();
            appInfo.CorpEMail = dt[0].Email.ToString().Trim();
            appInfo.LocEMail = dt[0].Email.ToString().Trim();
            appInfo.LocURL = dt[0].Website.ToString().Trim();
            appInfo.DBA = dt[0].DBA.ToString().Trim();
            appInfo.DBAAddress1 = dt[0].AddressLine1.ToString().Trim() + ", " + dt[0].AddressLine2.ToString().Trim();
            appInfo.DBAAddress2 = dt[0].AddressLine2.ToString().Trim();
            appInfo.DBACity = dt[0].CITY.ToString().Trim();
            appInfo.DBAState = dt[0].STATE.ToString().Trim();
            appInfo.DBAZip = dt[0].ZipCode.ToString().Trim();
            appInfo.DBACountryID = Convert.ToInt32(dt[0].CountryID.ToString().Trim());
            appInfo.DBAPhone = dt[0].BusinessPhone.ToString().Trim();
            if (dt[0].Fax.ToString().Trim() != "")
                appInfo.DBAFax = dt[0].Fax.ToString().Trim();

            appInfo.CustomerServicePhone = dt[0].CustServPhone.ToString().Trim();

            DateTime BusinessDate = DateTime.Now.AddMonths((Convert.ToInt32(dt[0].MIB.ToString().Trim())) * -1);
            DateTime BusinessOpenDate = BusinessDate.AddYears((Convert.ToInt32(dt[0].YIB.ToString().Trim())) * -1);
            string BusinessOpenMonth = BusinessOpenDate.Month.ToString().Trim();
            string BusinessOpenYear = BusinessOpenDate.Year.ToString().Trim();
            appInfo.BusOpenDate = BusinessOpenMonth + "/" + BusinessOpenYear;

            

            appInfo.FedTaxID = dt[0].FederalTaxID.ToString().Trim();

           // appInfo.LegalName = dt[0].LegalName.ToString().Trim();
            //appInfo.DunnBradNum = ""; //we don't track this
            if ((dt[0].PrevProcessor.ToString().Trim().Contains("Sage")) && (dt[0].PrevMerchantAcctNo.ToString().Trim() != ""))
                appInfo.MerchID = dt[0].PrevMerchantAcctNo.ToString().Trim();
            else
                appInfo.MerchID = "";
            appInfo.GeneralComment = ""; //no comment
            if (Convert.ToDecimal(dt[0].CPSwiped.ToString().Trim()) >= 70)
                appInfo.BusinessLevelID = 1; //Retail
            else
                appInfo.BusinessLevelID = 2; //MOTO

            if (dt[0].NumDaysDelivered.ToString().Trim() != "")
                appInfo.DaysUntilProductDelivery = Convert.ToInt32(dt[0].NumDaysDelivered);
            else
                return "Please enter a value in # of days until product delivered field.";

            if (dt[0].RefundPolicy == "Refund within 30 days")
                appInfo.ReturnPolicyID = 1; //30 Days Money Back Guarantee; 
            else if (dt[0].RefundPolicy == "Exchange Only")
                appInfo.ReturnPolicyID = 2; //30 Days Exchange Only; 
            //else if (dt[0].RefundPolicy == "No Refund")
            //    acroFields.SetField("Return Policy", "No Refund");
            else //if (dt[0].RefundPolicy.Contains("Other"))
                appInfo.ReturnPolicyID = 7; //Other

            appInfo.Product = dt[0].ProductSold.ToString().Trim();
            appInfo.Seasonal = false;
            appInfo.HighVolMonths = "";
            #endregion

            #region Principal Info
            appInfo.Owner1Equity = dt[0].P1OwnershipPercent.ToString().Trim();
            appInfo.LengthOwner1 = "0M"; //we don't track this
            appInfo.Owner1Title = dt[0].P1Title.ToString().Trim();
            appInfo.Owner1FullName = dt[0].P1FullName.ToString().Trim();
            appInfo.Owner1FName = dt[0].P1FirstName.ToString().Trim();
            appInfo.Owner1LName = dt[0].P1LastName.ToString().Trim();
            appInfo.Owner1Address1 = dt[0].P1AddressLine1.ToString().Trim();
            appInfo.Owner1Address2 = dt[0].P1AddressLine2.ToString().Trim();
            appInfo.Owner1City = dt[0].P1City.ToString().Trim();
            appInfo.Owner1State = dt[0].P1State.ToString().Trim();
            appInfo.Owner1Zip = dt[0].P1ZipCode.ToString().Trim();
            appInfo.Owner1CountryID = Convert.ToInt32(dt[0].P1Country.ToString().Trim());
            appInfo.Owner1PhoneNumber = dt[0].P1PhoneNumber.ToString().Trim();
            appInfo.Owner1SSN = dt[0].P1SSN.ToString().Trim();
            appInfo.Owner1EMail = dt[0].Email.ToString().Trim();
            appInfo.Owner1DOB = dt[0].P1DOB.ToString().Trim();

            if (Convert.ToString(dt[0].P2OwnershipPercent).Trim() != "")
            {
                appInfo.Owner2Equity = dt[0].P2OwnershipPercent.ToString().Trim();
                appInfo.LengthOwner2 = "0M"; //we don't track this
                appInfo.Owner2Title = dt[0].P2Title.ToString().Trim();
                appInfo.Owner2FullName = dt[0].P2FirstName.ToString().Trim() + " " + dt[0].P2LastName.ToString().Trim();
                appInfo.Owner2FName = dt[0].P2FirstName.ToString().Trim();
                appInfo.Owner2LName = dt[0].P2LastName.ToString().Trim();
                appInfo.Owner2Address1 = dt[0].p2AddressLine1.ToString().Trim();
                appInfo.Owner2Address2 = dt[0].p2AddressLine2.ToString().Trim();
                appInfo.Owner2City = dt[0].P2City.ToString().Trim();
                appInfo.Owner2State = dt[0].P2State.ToString().Trim();
                appInfo.Owner2Zip = dt[0].P2ZipCode.ToString().Trim();
                appInfo.Owner2CountryID = Convert.ToInt32(dt[0].P2Country.ToString().Trim());
                appInfo.Owner2PhoneNumber = dt[0].p2PhoneNumber.ToString().Trim();
                appInfo.Owner2SSN = dt[0].P2SSN.ToString().Trim();
                appInfo.Owner2EMail = dt[0].P2Email.ToString().Trim();
                appInfo.Owner2DOB = dt[0].P2DOB.ToString().Trim();
            }

            clsReferenceV1 reference = new clsReferenceV1();
            reference.ReferenceName = "";
            reference.ReferenceTitle = "";
            reference.ReferenceAddress = "";
            reference.ReferenceCity = "";
            reference.ReferenceState = "";
            reference.ReferenceZip = "";
            reference.ReferenceCountryID = 1;// us
            reference.ReferenceContact = "";
            reference.ReferencePhone = "";
            reference.ReferenceEmail = "";
            app.Reference = reference;

            app.AppInfo = appInfo;
            #endregion

            #region Banking Info
            clsBankInfoV1 bank = new clsBankInfoV1();
            bank.BankName = dt[0].BankName.ToString().Trim();
            bank.Address1 = dt[0].BankAddress.ToString().Trim(); //"Not Provided";
            bank.City = dt[0].BankCity.ToString().Trim();
            bank.State = dt[0].BankState.ToString().Trim();
            bank.Zip = dt[0].BankZip.ToString().Trim();
            bank.CountryID = Convert.ToInt32(dt[0].BankCountryID.ToString().Trim()); //1=us
            bank.PhoneNumber = dt[0].BankPhone.ToString().Trim();
            bank.CreditRoutingNumber = dt[0].BankRoutingNumber.ToString().Trim();
            bank.CreditAccountNumber = dt[0].BankAccountNumber.ToString().Trim();
            bank.DebitRoutingNumber = dt[0].BankRoutingNumber.ToString().Trim();
            bank.DebitAccountNumber = dt[0].BankAccountNumber.ToString().Trim();
            app.BankInfo = bank;
            #endregion

            #region Product Info
            clsMerchProductV1[] product = new clsMerchProductV1[3];
            product[0] = new clsMerchProductV1();
            product[0].ProductID = 1; //1=credit card

            product[1] = new clsMerchProductV1();
            product[2] = new clsMerchProductV1();
            if (!Convert.IsDBNull(dt[0].DebTransFee))
            {
                if (Convert.ToString(dt[0].DebTransFee) != "")
                {
                    if (!Convert.ToString(dt[0].DebTransFee).Trim().Contains("0.00"))
                        product[1].ProductID = 2; //2=debit card
                }
            }

            if (!dt[0].EBTTransFee.ToString().Trim().Contains("0.00"))
                product[2].ProductID = 3; //3=EBT

            app.MerchProduct = product;

            clsMerchCreditV1 credit = new clsMerchCreditV1();
            credit.UserBankID = 1; //value = 1 description = 3948
            if (dt[0].Platform.ToString().Trim().Contains("Vital"))
                credit.FrontEndProcessorID = 4; //Visanet/TSYS/Vital
            else if (dt[0].Platform.ToString().Trim() == "Paymentech")
                credit.FrontEndProcessorID = 1; //Paymentech; 

            credit.BackEndProcessorID = 10; //Vital
            credit.SettlementBankID = 1; //Harris Bank N.A.;
            credit.MonthlyVolume = Convert.ToDouble(dt[0].MonthlyVolume.ToString().Trim());
            credit.AverageTicket = Convert.ToDouble(dt[0].AverageTicket.ToString().Trim());
            credit.MaxSalesAmt = Convert.ToDouble(dt[0].MaxTicket.ToString().Trim());
            if (((Convert.ToInt32(dt[0].CPSwiped) % 5) == 0) && ((Convert.ToInt32(dt[0].CPKeyed) % 5) == 0) && ((Convert.ToInt32(dt[0].CNPKeyed) %5) == 0))
            {
                credit.CardPresentSwiped = dt[0].CPSwiped.ToString().Trim();
                credit.CardPresentImprint = dt[0].CPKeyed.ToString().Trim();
                credit.CardNotPresent = dt[0].CNPKeyed.ToString().Trim();
            }else 
            { 
                return "Card Swiped, Key Imprint and Key without Imprint percentage can only be increment of 5. Please check the percentage."; 
            }
            credit.DiscountPaidID = 1; //1 = MONTHLY always?
            credit.SalesToConsumer = "100"; //we don't track this
            credit.BusToBus = "0"; //we don't track this
            credit.SalesToGov = "0"; //we don't track this
            if (dt[0].PrevProcessor.ToString().Trim() != "")
                credit.CurrentProcessor = dt[0].PrevProcessor.ToString().Trim();
            else
                credit.CurrentProcessor = "";

            if (dt[0].Interchange.ToString().Trim().ToLower() == "true")
                credit.BetInterchangeTypeID = 1; //Interchange At Pass Through
            else
                credit.BetInterchangeTypeID = 0; //Tiered

            app.MerchCredit = credit;

            clsMerchTEV1[] te = new clsMerchTEV1[2];
            te[0] = new clsMerchTEV1();
            te[0].CardTypeID = 3;  // amex 
            switch (dt[0].AmexApplied.ToString().ToUpper())
            {
                case "NEW":
                    te[0].MerchTEStatusID = 2;
                    break;
                case "NONE":
                    te[0].MerchTEStatusID = 1;
                    break;
                case "EXISTING":
                    te[0].MerchTEStatusID = 3;
                    break;
            }
            te[0].AccountNum = dt[0].PrevAmexNum.ToString().Trim();

            /*te[1] = new clsMerchTEV1();
            te[1].CardTypeID = 4; // discover
            switch (dt[0].DiscoverApplied.ToString().ToUpper())
            {
                case "NEW":
                    te[1].MerchTEStatusID = 2;
                    break;
                case "NONE":
                    te[1].MerchTEStatusID = 1;
                    break;
                case "EXISTING":
                    te[1].MerchTEStatusID = 3;
                    break;
            }
            te[1].AccountNum = dt[0].PrevDiscoverNum.ToString().Trim();*/

            te[1] = new clsMerchTEV1();
            te[1].CardTypeID = 5; // diners 
            switch (dt[0].DinersApplied.ToString().ToUpper())
            {
                case "NEW":
                    te[1].MerchTEStatusID = 2;
                    break;
                case "NONE":
                    te[1].MerchTEStatusID = 1;
                    break;
                case "EXISTING":
                    te[1].MerchTEStatusID = 3;
                    break;
            }
            te[1].AccountNum = dt[0].PrevDinersNum.ToString().Trim();

            app.MerchTE = te;
            #endregion

            #region Rates

            clsMerchBETInterchangeV1[] bi = new clsMerchBETInterchangeV1[3];
            bi[0] = new clsMerchBETInterchangeV1();
            bi[1] = new clsMerchBETInterchangeV1();
            bi[2] = new clsMerchBETInterchangeV1();

            //Interchange BET
            if (dt[0].Interchange.ToString().Trim().ToLower() == "true")
            {
                bi[0].CardTypeID = 1;   // visa 
                bi[0].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[0].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[0].BetInterchangeID = "8099"; //BET#

                bi[1].CardTypeID = 2;  // mastercard
                bi[1].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[1].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[1].BetInterchangeID = "6099"; //BET#

                bi[2].CardTypeID = 4;  // discover
                bi[2].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[2].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[2].BetInterchangeID = "3299"; //BET#
            }

            //Retail
            else if (Convert.ToDecimal(dt[0].CPSwiped.ToString().Trim()) >= 70)
            {
                bi[0].CardTypeID = 1;   // visa 
                bi[0].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[0].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());

                bi[1].CardTypeID = 2;  // mastercard
                bi[1].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[1].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());

                bi[2].CardTypeID = 4;  // discover
                bi[2].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[2].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());

                decimal midQualStep = Convert.ToDecimal(dt[0].DiscRateMidQual.ToString().Trim()) - Convert.ToDecimal(dt[0].DiscountRate.ToString().Trim());
                decimal nonQualStep = Convert.ToDecimal(dt[0].DiscRateNonQual.ToString().Trim()) - Convert.ToDecimal(dt[0].DiscountRate.ToString().Trim());
                
                if ((midQualStep == 1m) && (nonQualStep == 2m))
                {   
                    bi[0].BetInterchangeID = "7965"; 
                    bi[1].BetInterchangeID = "5965"; 
                    bi[2].BetInterchangeID = "3427"; 
                } else if ((midQualStep == 0.8m) && (nonQualStep == 2.05m))
                {
                    bi[0].BetInterchangeID = "7263"; //"3297" - old bets 0.8/1.9 //BET# MidQualStep = 0.80, NonQualStep = 2.05
                    bi[1].BetInterchangeID = "5263"; //"5232" - old bets 0.8/1.9 //BET#
                    bi[2].BetInterchangeID = "3297"; //"3291" - old bets 0.8/1.9 //BET#
                }

                else if ((midQualStep == 1m) && (nonQualStep == 1.5m))
                {
                    bi[0].BetInterchangeID = "7035"; //BET# MidQualStep = 1.00, NonQualStep = 1.5
                    bi[1].BetInterchangeID = "5035"; //BET#
                    bi[2].BetInterchangeID = "3201"; //BET#
                }

                else if ((midQualStep == 0.5m) && (nonQualStep == 1m))
                {
                    bi[0].BetInterchangeID = "7148"; //BET# MidQualStep = 0.50, NonQualStep = 1.00
                    bi[1].BetInterchangeID = "5148"; //BET#
                    bi[2].BetInterchangeID = "3235"; //BET#
                }
                else if ((midQualStep == 0.72m) && (nonQualStep == 1.67m))
                {
                    bi[0].BetInterchangeID = "7076"; //BET# MidQualStep = 0.72, NonQualStep = 1.67
                    bi[1].BetInterchangeID = "5076"; //BET#
                    bi[2].BetInterchangeID = "3289"; //BET#
                }
                else
                    return "Non standard BETs have been used. Please resubmit with one of the following combinations of MidQualSteps and NonQualSteps: 0.80, 2.05; 1.00, 1.50; 0.50, 1.00;" + 
                        System.Environment.NewLine + "find a BET table that match the rates on the application; or if none match, submit a request to RM for creation of a new BET table and forward to support@ecenow.com to add to the secondary BET list.";
                //"Invalid BETs have been used. Only the following combinations of MidQualSteps and NonQualSteps can be used: 1.00, 2.00; 0.80, 1.90; 1.00, 1.50; 0.50, 1.00." + System.Environment.NewLine + "Please correct MidQual and NonQual rates.";
            }

            //ecommerce and moto
            else
            {
                bi[0].CardTypeID = 1;   // visa 
                bi[0].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[0].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());

                bi[1].CardTypeID = 2;  // mastercard
                bi[1].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[1].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());

                bi[2].CardTypeID = 4;  // discover
                bi[2].DiscountQualifiedRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());
                bi[2].DiscountCheckcardRate = Convert.ToDouble(dt[0].DiscountRate.ToString().Trim());

                decimal midQualStep = Convert.ToDecimal(dt[0].DiscRateMidQual.ToString().Trim()) - Convert.ToDecimal(dt[0].DiscountRate.ToString().Trim());
                decimal nonQualStep = Convert.ToDecimal(dt[0].DiscRateNonQual.ToString().Trim()) - Convert.ToDecimal(dt[0].DiscountRate.ToString().Trim());

                if ((midQualStep == 1m) && (nonQualStep == 2m))
                {
                    bi[0].BetInterchangeID = "8205"; //
                    bi[1].BetInterchangeID = "6205"; //
                    bi[2].BetInterchangeID = "3418"; //
                }
                else if ((midQualStep == 0.8m) && (nonQualStep == 2.05m))
                {
                    bi[0].BetInterchangeID = "8202"; //"8212" - old bets 0.8/1.9 //BET# MidQualStep = 0.80, NonQualStep = 2.05
                    bi[1].BetInterchangeID = "6202"; //"6212" - old bets 0.8/1.9 //BET#
                    bi[2].BetInterchangeID = "3274"; //"3262" - old bets 0.8/1.9 //BET#
                }

                else if ((midQualStep == 1m) && (nonQualStep == 1.5m))
                {
                    bi[0].BetInterchangeID = "8013"; //BET# MidQualStep = 1.00, NonQualStep = 1.5
                    bi[1].BetInterchangeID = "6013"; //BET#
                    bi[2].BetInterchangeID = "3204"; //BET#
                }

                else if ((midQualStep == 0.5m) && (nonQualStep == 1m))
                {
                    bi[0].BetInterchangeID = "8061"; //BET# MidQualStep = 0.50, NonQualStep = 1.00
                    bi[1].BetInterchangeID = "6061"; //BET#
                    bi[2].BetInterchangeID = "3232"; //BET#
                }
                else if ((midQualStep == 0.72m) && (nonQualStep == 1.67m))
                {
                    bi[0].BetInterchangeID = "8085"; //BET# MidQualStep = 0.72, NonQualStep = 1.67
                    bi[1].BetInterchangeID = "6085"; //BET#
                    bi[2].BetInterchangeID = "3264"; //BET#
                }
                else
                    return "Non standard BETs have been used. Please resubmit with one of the following combinations of MidQualSteps and NonQualSteps: 0.80, 2.05; 1.00, 1.50; 0.50, 1.00; 0.72, 1.67;" + 
                        System.Environment.NewLine + "find a BET table that match the rates on the application; or if none match, submit a request to RM for creation of a new BET table and forward to support@ecenow.com to add to the secondary BET list.";
                //"Invalid BETs have been used. Only the following combinations of MidQualSteps and NonQualSteps can be used: 0.80, 1.90; 1.00, 1.50; 0.50, 1.00." + System.Environment.NewLine + "Please correct MidQual and NonQual rates.";
            }

            app.MerchBETInterchange = bi;

            // per item fees 
            clsMerchBETPlanTableV1[] bpt = new clsMerchBETPlanTableV1[11];
            bpt[0] = new clsMerchBETPlanTableV1();
            bpt[0].CardTypeID = 10;   // visa/mastercard/overall 
            bpt[0].ExpenseTypeID = 2;
            bpt[0].BetPlanTableTypeID = 1;
            bpt[0].PerItemFee = Convert.ToDouble(dt[0].TransactionFee.ToString().Trim());
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                bpt[0].PerItemFee = Convert.ToDouble(dt[0].TransactionFee.ToString().Trim()) + Convert.ToDouble(dt[0].GatewayTransFee.ToString().Trim());

            bpt[1] = new clsMerchBETPlanTableV1();
            bpt[1].CardTypeID = 3;    // amex 
            bpt[1].ExpenseTypeID = 2;
            bpt[1].BetPlanTableTypeID = 1;
            bpt[1].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim());
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                bpt[1].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim()) + Convert.ToDouble(dt[0].GatewayTransFee.ToString().Trim());

            bpt[2] = new clsMerchBETPlanTableV1();
            bpt[2].CardTypeID = 4;    // discover
            bpt[2].ExpenseTypeID = 2;
            bpt[2].BetPlanTableTypeID = 1;
            bpt[2].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim());
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                bpt[2].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim()) + Convert.ToDouble(dt[0].GatewayTransFee.ToString().Trim());

            bpt[3] = new clsMerchBETPlanTableV1();
            bpt[3].CardTypeID = 5;   // diners 
            bpt[3].ExpenseTypeID = 2;
            bpt[3].BetPlanTableTypeID = 1;
            bpt[3].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim());
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                bpt[3].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim()) + Convert.ToDouble(dt[0].GatewayTransFee.ToString().Trim());

            bpt[4] = new clsMerchBETPlanTableV1();
            bpt[4].CardTypeID = 7;   // jcb 
            bpt[4].ExpenseTypeID = 2;
            bpt[4].BetPlanTableTypeID = 1;
            bpt[4].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim());
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                bpt[4].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim()) + Convert.ToDouble(dt[0].GatewayTransFee.ToString().Trim());

            bpt[5] = new clsMerchBETPlanTableV1();
            bpt[5].CardTypeID = 6;   // Carte Blanche
            bpt[5].ExpenseTypeID = 2;
            bpt[5].BetPlanTableTypeID = 1;
            bpt[5].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim());
            //bpt[5].PerItemFee = 0.25;
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                bpt[5].PerItemFee = Convert.ToDouble(dt[0].NBCTransFee.ToString().Trim()) + Convert.ToDouble(dt[0].GatewayTransFee.ToString().Trim());

            bpt[6] = new clsMerchBETPlanTableV1();
            bpt[6].CardTypeID = 8;    // pin debit 
            bpt[6].ExpenseTypeID = 2;
            bpt[6].BetPlanTableTypeID = 1;
            
            if(!Convert.IsDBNull (dt[0].DebTransFee))
            {
                if (Convert.ToString(dt[0].DebTransFee) != "")
                {
                    if (!Convert.ToString(dt[0].DebTransFee).Trim().Contains("0.00"))
                        bpt[6].PerItemFee = Convert.ToDouble(dt[0].DebTransFee.ToString().Trim());
                    else
                        bpt[6].PerItemFee = 0.00;
                }
            }

            bpt[7] = new clsMerchBETPlanTableV1();
            bpt[7].CardTypeID = 9;    // ebt - we don't do EBT apps
            bpt[7].ExpenseTypeID = 2;
            bpt[7].BetPlanTableTypeID = 1;
            if (!dt[0].EBTTransFee.ToString().Trim().Contains("0.00"))
                bpt[7].PerItemFee = Convert.ToDouble(dt[0].EBTTransFee.ToString().Trim());
            else
                bpt[7].PerItemFee = 0.00;

            bpt[8] = new clsMerchBETPlanTableV1();
            bpt[8].CardTypeID = 0;
            bpt[8].ExpenseTypeID = 5; // aru
            bpt[8].BetPlanTableTypeID = 1;
            bpt[8].PerItemFee = Convert.ToDouble(dt[0].VoiceAuth.ToString().Trim());

            bpt[9] = new clsMerchBETPlanTableV1();
            bpt[9].CardTypeID = 0;
            bpt[9].ExpenseTypeID = 6; // vioce auth
            bpt[9].BetPlanTableTypeID = 1;
            bpt[9].PerItemFee = Convert.ToDouble(dt[0].VoiceAuth.ToString().Trim());

            bpt[10] = new clsMerchBETPlanTableV1();
            bpt[10].CardTypeID = 0;
            bpt[10].ExpenseTypeID = 0;
            bpt[10].BetPlanTableTypeID = 4; // chargeback 
            bpt[10].PerItemFee = Convert.ToDouble(dt[0].ChargebackFee.ToString().Trim());

            app.MerchBETPlanTable = bpt;

            clsMerchFeeV1[] mf = new clsMerchFeeV1[12];
            mf[0] = new clsMerchFeeV1();
            mf[0].MerchFeeItemID = 22;  // monthly min 
            mf[0].Amount = Convert.ToDouble(dt[0].MonMin.ToString().Trim());

            mf[1] = new clsMerchFeeV1();
            mf[1].MerchFeeItemID = 18; // statement 
            mf[1].Amount = Convert.ToDouble(dt[0].CustServFee.ToString().Trim());

            mf[2] = new clsMerchFeeV1();
            mf[2].MerchFeeItemID = 20;// support 
            mf[2].Amount = 0.00; //we don't charge a separate support fee

            mf[3] = new clsMerchFeeV1();
            mf[3].MerchFeeItemID = 23; // gateway access 
            if (dt[0].Gateway.ToString().Trim().ToLower().Contains("sage"))
                mf[3].Amount = Convert.ToDouble(dt[0].GatewayMonFee.ToString().Trim());
            else
                mf[3].Amount = 0;

            mf[4] = new clsMerchFeeV1();
            mf[4].MerchFeeItemID = 63; // online reporting 
            mf[4].Amount = 0; // ??

            mf[5] = new clsMerchFeeV1();
            mf[5].MerchFeeItemID = 12; // annual 
            mf[5].Amount = Convert.ToDouble(dt[0].AnnualFee.ToString().Trim());

            mf[6] = new clsMerchFeeV1();
            mf[6].MerchFeeItemID = 26; // Wireless Access 
            if (dt[0].WirelessAccess.ToString().Trim() != "")
                mf[6].Amount = Convert.ToDouble(dt[0].WirelessAccess.ToString().Trim());

            mf[7] = new clsMerchFeeV1();
            mf[7].MerchFeeItemID = 24; // Pin Debit Access 
            
            if (!Convert.IsDBNull(dt[0].DebMonFee))
            {
            if (dt[0].DebMonFee.ToString().Trim() != "")
                mf[7].Amount = Convert.ToDouble(dt[0].DebMonFee.ToString().Trim());
            }


            mf[8] = new clsMerchFeeV1();
            mf[8].MerchFeeItemID = 86; // IRS Reporting Fee - Monthly
            mf[8].Amount = 2.10;

            mf[9] = new clsMerchFeeV1();
            mf[9].MerchFeeItemID = 87; // IRS Verification - Annual
            mf[9].Amount = 9.25;

            mf[10] = new clsMerchFeeV1();
            mf[10].MerchFeeItemID = 83; // Mobile Payments Monthly Access
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                mf[10].Amount = Convert.ToDouble(dt[0].GatewayMonFee.ToString().Trim());

            /*mf[11] = new clsMerchFeeV1();
            mf[11].MerchFeeItemID = 85; // Mobile Payments Set up
            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
                mf[11].Amount = 25.00;*/

            app.MerchFee = mf;

            clsMerchStartupFeeV1[] sf = new clsMerchStartupFeeV1[6];
            sf[0] = new clsMerchStartupFeeV1();
            sf[0].FeeID = 1;      // credit application fee - we don't track this
            sf[0].ChargeAmount = 0.00;

            sf[1] = new clsMerchStartupFeeV1();
            sf[1].FeeID = 5;      // wireless setup 
            if ((dt[0].WirelessAccess.ToString().Trim() == "") || (dt[0].WirelessAccess.ToString() == "0.00"))
                sf[1].ChargeAmount = 0.00; 
            else
                sf[1].ChargeAmount = 35.00; // default on the pdf 

            sf[2] = new clsMerchStartupFeeV1();
            sf[2].FeeID = 65;   // expedite 
            sf[2].ChargeAmount = 0;

            sf[3] = new clsMerchStartupFeeV1();
            sf[3].FeeID = 7;    // training pys 
            sf[3].ChargeAmount = 0;

            sf[4] = new clsMerchStartupFeeV1();
            sf[4].FeeID = 64;  // training vir 
            sf[4].ChargeAmount = 0;

            sf[5] = new clsMerchStartupFeeV1();
            sf[5].FeeID = 60;   // lease rental
            sf[5].ChargeAmount = 0;

            app.MerchStartupFee = sf;
            #endregion

            if (dt[0].Gateway.ToString().Trim().ToLower().Contains("sage"))
            {
                clsMerchProductServiceV1[] ps = new clsMerchProductServiceV1[1];
                ps[0] = new clsMerchProductServiceV1();
                ps[0].ProductID = 1;
                ps[0].ProductServiceID = 5;   // sage gateway ( credit card ) 
                app.MerchProductService = ps;
            }

            string msg = "";
            string appID = "";
            //bool retVal = ws.CreateMerchantApplication(app, ref appID, ref msg);
            //return "Successfull";
            if (ws.CreateMerchantApplication(app, ref appID, ref msg))
            {
                return "App submitted successfully. AppID: " + appID.ToString().Trim();
            }
            else
            {
                return "App was not submitted. Error Msg: " + msg;
            }
        }
        else
        {
            return "Sage data not found";
        }

    }

    //This function handles submit button click event
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            if (Page.IsValid)
            {
                grdPDF.Visible = true;
                PDFBL ActRecords = new PDFBL();
                DataSet ds = ActRecords.GetPDFSummaryACT(lstLookup.SelectedItem.Text, txtLookup.Text.Trim());
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

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}

