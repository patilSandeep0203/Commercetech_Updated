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
using System.Xml;
using BusinessLayer;
using DLPartner;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Mail;

public partial class CreateACTPDF : System.Web.UI.Page
{
    //the selected Contact ID clicked by user
    private static string selContactID = "";
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
        if (!Session.IsNewSession)
        {
            //This page is accessible only by Admins and Employees
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
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
            if (e.CommandName == "CreatePDF")//If the CreatePDF button in the grid is clicked
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdPDF.Rows[index];

                System.Guid ContactID = new Guid(Server.HtmlDecode(grdRow.Cells[1].Text) );
                string strContactID = ContactID.ToString();               
                string Processor = Server.HtmlDecode(grdRow.Cells[7].Text);
                if (strContactID!= "")
                {
                    if (Processor.ToLower().Contains("sage"))
                    {
                        selContactID = strContactID;
                        pnlSagePDF.Visible = true;
                        lblDBASel2.Text = Server.HtmlDecode(grdRow.Cells[4].Text);
                        lblDBASel2.Font.Size = FontUnit.Point(10);

                        
                        //Show the Panel when Creating PDF for Sage
                        PDFBL SageData = new PDFBL();
                        PartnerDS.ACTSagePDFDataTable dt = SageData.GetSageDataFromACT(strContactID);
                                                
                        //Check to ensure correct BETs are being used
                        decimal midQualStep = Convert.ToDecimal(dt[0].DiscRateMidQual.ToString().Trim());
                        decimal nonQualStep = Convert.ToDecimal(dt[0].DiscRateNonQual.ToString().Trim());

                        if (dt[0].Interchange.ToString() != "True")
                        {
                            if ((midQualStep != 1m) || (nonQualStep != 2m))
                            {
                                if ((midQualStep != 0.8m) || (nonQualStep != 2.05m))
                                {
                                    if ((midQualStep != 1m) || (nonQualStep != 1.5m))
                                    {
                                        if ((midQualStep != 0.5m) || (nonQualStep != 1m))
                                        {
                                            //Error for all users except ADMINS if the Mid and NonQual Steps don't match standard BETs
                                            if (!User.IsInRole("Admin"))
                                            {
                                                DisplayMessage("Only the following combinations of MidQualSteps and NonQualSteps can be used: 0.80, 2.05; 1.00, 1.50; 0.50, 1.00. Please correct MidQual and NonQual rates in ACT!");
                                                pnlSagePDF.Visible = false;
                                            }
                                            else
                                            {
                                                DisplayMessage("The standard combinations of MidQualSteps and NonQualSteps are not being used: 0.80, 2.05; 1.00, 1.50; 0.50, 1.00. You will be able to create the PDF." +
                                                        System.Environment.NewLine + "But in order to submit the account to Sage, find a BET table that match the rates on UNO; or if none match, submit a request to RM for creation of a new BET table and forward to support@ecenow.com to add to the secondary BET list.");
                                                pnlSagePDF.Visible = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                                                
                        /*if (Convert.ToDecimal(dt[0].ProcessPctSwiped.ToString().Trim()) >= 70)
                            btnSageMOTO.Visible = false;
                        else
                            btnSageMOTO.Visible = true;*/
                    }
                    else if (Processor.Contains("Intuit"))
                        CreateIMSPDF(strContactID);
                    else if (Processor.ToLower().Contains("ipayment"))
                        CreateIPayPDF(strContactID);
                    else if (Processor.ToLower().Contains("optimal-merrick"))
                        CreateMerrickPDF(strContactID);
                    else if ((Processor.ToLower().Contains("cal")) || (Processor.ToLower().Contains("international")))
                        CreateInternationalPDF(strContactID);
                    else if (Processor.ToLower().Contains("barclays"))
                        CreateBarclaysPDF(strContactID);
                    else if (Processor.ToLower().Contains("canada"))
                        CreateCanadaPDF(strContactID);
                    else if (Processor.ToLower().Contains("chase"))
                    {
                        //Show the Panel when Creating PDF for Chase
                        selContactID = strContactID;                        
                        lblDBASel.Text = Server.HtmlDecode(grdRow.Cells[4].Text);
                        lblDBASel.Font.Size = FontUnit.Point(10);
                        PDFBL PDF = new PDFBL();
                        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(selContactID);
                        if ((dt[0].Interchange.ToString().Trim().ToLower().Contains("true")) || (dt[0].Assessments.ToString().Trim().ToLower().Contains("true")))
                        {
                            btnChaseFS3Tier.Visible = false;
                            btnChaseFSInterchangePlus.Visible = true;
                        }
                        else
                        {
                            btnChaseFS3Tier.Visible = true;
                            btnChaseFSInterchangePlus.Visible = false;
                        }
                        pnlChasePDF.Visible = true;
                    }
                    else if (Processor.ToLower().Contains("kitts"))
                        CreateStKittsPDF(strContactID);
                    else if (Processor.ToLower().Contains("payvision"))
                        CreatePayvisionPDF(strContactID);
                    else if (Processor != "")
                        DisplayMessage("Processor " + Processor + " is not a valid Processor for PDF creation.");
                    else
                        DisplayMessage("No Processor assigned to this ACT record. PDF cannot be created.");
                }
            }//end if command name            
            else if (e.CommandName == "CreateASPDF")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdPDF.Rows[index];

                //lblNoAddlServices.Visible = true;
                //btnNorthernLeasePDF.Visible = true;
                //pnlAddlServices.Visible = true;

                System.Guid ContactID = new Guid(Server.HtmlDecode(grdRow.Cells[1].Text));
                string strContactID = ContactID.ToString();
                selContactID = strContactID;

                string processor = Server.HtmlDecode(grdRow.Cells[7].Text);
                if (processor.ToLower().Contains("ipayment"))
                {
                    lblAddlServices.Visible = true;
                    btnRoamPayPDF.Visible = true;
                    lblNoAddlServices.Visible = false;
                }
                else
                {
                    lblAddlServices.Visible = false;
                    btnRoamPayPDF.Visible = false;
                    lblNoAddlServices.Visible = true;
                }

                //lblAddlServices.Visible = true;
                //lblNoAddlServices.Visible = false;

                    string lease = Server.HtmlDecode(grdRow.Cells[9].Text);
                    if (lease.ToLower().Contains("northern"))
                    {
                        lblAddlServices.Visible = true;
                        btnNorthernLeasePDF.Visible = true;
                        lblNoAddlServices.Visible = false;
                    }
                    else
                    {
                        //lblAddlServices.Visible = false;
                        btnNorthernLeasePDF.Visible = false;
                        //lblNoAddlServices.Visible = true;
                    }

                    string giftType = Server.HtmlDecode(grdRow.Cells[10].Text);
                    if (giftType.ToLower().Contains("global"))
                    {
                        lblAddlServices.Visible = true;
                        btnGETIGiftCardPDF.Visible = true;
                        lblNoAddlServices.Visible = false;
                    }
                    else
                    {
                        //lblAddlServices.Visible = false;
                        btnGETIGiftCardPDF.Visible = false;
                        //lblNoAddlServices.Visible = true;
                    }

                    btnAMIPDF.Visible = false;
                    btnRapidAdvancePDF.Visible = false;
                    btnBFSPDF.Visible = false;

                    string MCAType = Server.HtmlDecode(grdRow.Cells[11].Text);
                    if (MCAType.ToLower().Contains("advanceme"))
                    {
                        lblAddlServices.Visible = true;
                        btnAMIPDF.Visible = true;
                        lblNoAddlServices.Visible = false;
                    }
                    else if (MCAType.ToLower().Contains("rapid"))
                    {
                        lblAddlServices.Visible = true;
                        btnRapidAdvancePDF.Visible = true;
                        lblNoAddlServices.Visible = false;
                    }
                    else if (MCAType.ToLower().Contains("business"))
                    {
                        lblAddlServices.Visible = true;
                        btnBFSPDF.Visible = true;
                        lblNoAddlServices.Visible = false;
                    }
                    else {
                        lblAddlServices.Visible = false;
                    }

                    pnlAddlServices.Visible = true;

            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }//end function grid view button click

    //This function handles submit button click event
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            pnlChasePDF.Visible = false;
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

   /* #region IMS PDF
    //This function creates IMS PDF
    public bool CreateIMSPDF(string ContactID)
    {
        //Get data for IMS Application
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTIMSPDFDataTable dt = PDF.GetIMSDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Create PDFReader object by passing in the name of PDF to populate
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/IMS Application.pdf"));
            if ((dt[0].Interchange.ToString() == "True") || (dt[0].Assessments.ToString() == "True"))
                reader = new PdfReader(Server.MapPath("../PDF/IMS Application Interchange.pdf"));
            else
                reader = new PdfReader(Server.MapPath("../PDF/IMS Application.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "IMS_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");                
            }
            
            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            AcroFields acroFields = stamper.AcroFields;

            #region General Information
            acroFields.SetField("app.RepName", dt[0].RepName);
            acroFields.SetField("app.LegalName", dt[0].COMPANYNAME);
            acroFields.SetField("app.DBA", dt[0].DBA);
            acroFields.SetField("app.ApplicantDBA", dt[0].DBA);
            acroFields.SetField("app.Email", dt[0].Email);
            acroFields.SetField("app.ContactName", dt[0].ContactName);
            acroFields.SetField("app.Website", dt[0].Website);
            acroFields.SetField("app.MailingAddress", dt[0].BillingAddress);
            acroFields.SetField("app.MCity", dt[0].BillingCity);
            acroFields.SetField("app.MState", dt[0].BillingState);
            acroFields.SetField("app.MZip", dt[0].BillingZipCode);
            acroFields.SetField("app.LocationAddress", dt[0].Address);
            acroFields.SetField("app.LocationCity", dt[0].CITY);
            acroFields.SetField("app.LocationState", dt[0].STATE);
            acroFields.SetField("app.LZip", dt[0].ZipCode);
            acroFields.SetField("app.Years", dt[0].YearsInBusiness.ToString());
            acroFields.SetField("app.Months", dt[0].MonthsInBusiness.ToString());
            acroFields.SetField("app.FaxNumber", dt[0].Fax);
            acroFields.SetField("app.BusinessPhone", dt[0].BusinessPhone);
            acroFields.SetField("app.TaxID", dt[0].FederalTaxID);
            acroFields.SetField("app.ProductsSold", dt[0].ProductSold);
            if (dt[0].PrevIMSNum != "")
            {
                acroFields.SetField("app.chkIMSMerchant", "Yes");
                acroFields.SetField("app.chkNewMerchant", "No");
                acroFields.SetField("app.PrevIMSNum", dt[0].PrevIMSNum.ToString());
            }
            else
            {
                acroFields.SetField("app.chkIMSMerchant", "No");
                acroFields.SetField("app.chkNewMerchant", "Yes");
            }
            acroFields.SetField("app.IMSMerchantNum", dt[0].PrevIMSNum);
            acroFields.SetField("app.PrevProcessor", dt[0].PrevProcessor);
            acroFields.SetField("app.ReasonForLeaving", dt[0].ReasonForLeaving);
            acroFields.SetField("app.OtherRefund", dt[0].OtherRefund);
            if (dt[0].CTMF == "Yes")
            {
                acroFields.SetField("app.chkCTMFYes", "Yes");
                acroFields.SetField("app.chkCTMFNo", "Off");
            }
            else
            {
                acroFields.SetField("app.chkCTMFYes", "Off");
                acroFields.SetField("app.chkCTMFNo", "Yes");
            }

            if (dt[0].PrevProcessed.Contains("Yes"))
            {
                acroFields.SetField("app.chkPrevProcessedYes", "Yes");
                //acroFields.SetField("app.chkPrevProcessedNo", "Off");
            }
            else
            {
                //acroFields.SetField("app.chkPrevProcessedYes", "Off");
                acroFields.SetField("app.chkPrevProcessedNo", "Yes");
            }

            if ((dt[0].RefundPolicy == "Refund within 30 days") || (dt[0].RefundPolicy == "Refund Within 30 Days"))
                acroFields.SetField("app.chkRefund30Days", "Yes");

            if (dt[0].RefundPolicy == "Exchange Only")
                acroFields.SetField("app.chkExchangeOnly", "Yes");

            if (dt[0].RefundPolicy.Contains("Other"))
                acroFields.SetField("app.chkOtherRefund", "Yes");

            if (dt[0].RefundPolicy == "No Refund")
                acroFields.SetField("app.chkNoRefund", "Yes");

            if (dt[0].LegalStatus == "Sole Proprietorship")
                acroFields.SetField("app.chkSoleProp", "Yes");
            if (dt[0].LegalStatus == "Corporation")
                acroFields.SetField("app.chkCorporation", "Yes");
            if (dt[0].LegalStatus == "Partnership")
                acroFields.SetField("app.chkPartnership", "Yes");
            if (dt[0].LegalStatus == "Non-Profit")
                acroFields.SetField("app.chkNonProfit", "Yes");
            if (dt[0].LegalStatus == "LLC")
                acroFields.SetField("app.chkLLC", "Yes");
            #endregion

            #region CardPCT
            acroFields.SetField("app.Swiped", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("app.KeyedwImprint", dt[0].ProcessPctKeyedwImprint.ToString());
            acroFields.SetField("app.KeyedwoImprint", dt[0].ProcessPctKeyedwoImprint.ToString());
            acroFields.SetField("app.Retail", dt[0].BusinessPctRetail.ToString());
            acroFields.SetField("app.Restaurant", dt[0].BusinessPctRestaurant.ToString());
            acroFields.SetField("app.Service", dt[0].BusinessPctService.ToString());
            acroFields.SetField("app.MailPhone", dt[0].BusinessPctMailOrder.ToString().ToString());
            acroFields.SetField("app.Internet", dt[0].BusinessPctInternet.ToString().ToString());
            acroFields.SetField("app.Other", dt[0].BusinessPctOther.ToString());
            #endregion

            #region Principal #1
            //Principal #1
            acroFields.SetField("app.P1Zip", dt[0].P1ZipCode);
            acroFields.SetField("app.P1State", dt[0].P1State);
            acroFields.SetField("app.P1City", dt[0].P1City);
            acroFields.SetField("app.P1Address", dt[0].P1Address);
            acroFields.SetField("app.P1Title", dt[0].P1Title);
            acroFields.SetField("app.P1SSN", dt[0].P1SSN);
            acroFields.SetField("app.P1LastName", dt[0].P1LastName);
            acroFields.SetField("app.P1MiddleName", dt[0].P1MName);
            acroFields.SetField("app.P1FirstName", dt[0].P1FirstName);
            acroFields.SetField("app.P1FullName", dt[0].P1FirstName + " " + dt[0].P1LastName);
            acroFields.SetField("app.P1Ownership", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("app.P1DOB", dt[0].P1DOB);
            acroFields.SetField("app.P1DriversState", dt[0].P1DriversLicenseState);
            acroFields.SetField("app.P1DriversExp", dt[0].P1DriversLicenseExp);
            acroFields.SetField("app.P1DriversLicenseNo", dt[0].P1DriversLicenseNo);
            acroFields.SetField("app.P1Years", dt[0].P1YearsAtAddress);
            acroFields.SetField("app.P1Months", dt[0].P1MonthsAtAddress);
            acroFields.SetField("app.P1HomePhone", dt[0].P1PhoneNumber);
            if (dt[0].P1LivingStatus == "Rent")
                acroFields.SetField("app.chkP1Rent", "Yes");
            if (dt[0].P1LivingStatus == "Own")
                acroFields.SetField("app.chkP1Own", "Yes");
            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("app.P2Zip", dt[0].P2Zip);
            acroFields.SetField("app.P2State", dt[0].P2State);
            acroFields.SetField("app.P2City", dt[0].P2City);
            acroFields.SetField("app.P2Address", dt[0].p2Address);
            acroFields.SetField("app.P2Title", dt[0].P2Title);
            acroFields.SetField("app.P2SSN", dt[0].P2SSN);
            acroFields.SetField("app.P2LastName", dt[0].P2LastName);
            //acroFields.SetField("app.P2MiddleName", dt[0].P2MidName);
            acroFields.SetField("app.P2FirstName", dt[0].P2FirstName);
            acroFields.SetField("app.P2FullName", dt[0].P2FirstName + " " + dt[0].P2LastName);
            acroFields.SetField("app.P2Ownership", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("app.P2DOB", dt[0].P2DOB);
            acroFields.SetField("app.P2DriversState", dt[0].P2DriversLicenseState);
            acroFields.SetField("app.P2DriversExp", dt[0].P2DriversLicenseExp);
            acroFields.SetField("app.P2DriversLicenseNo", dt[0].P2DriversLicenseNo);
            acroFields.SetField("app.P2Years", dt[0].P2YearsAtAddress);
            acroFields.SetField("app.P2Months", dt[0].p2MonthsAtAddress);
            acroFields.SetField("app.P2HomePhone", dt[0].p2PhoneNumber);
            if (dt[0].P2LivingStatus == "Rent")
                acroFields.SetField("app.chkP2Rent", "Yes");
            if (dt[0].P2LivingStatus == "Own")
                acroFields.SetField("app.chkP2Own", "Yes");
            #endregion

            #region Rates
            //Rates
            acroFields.SetField("app.AvgTicket", dt[0].AverageTicket.ToString());
            acroFields.SetField("app.MonthlyVol", dt[0].MonthlyVolume.ToString());
            acroFields.SetField("app.DebitDiscRate", dt[0].DiscQD.ToString());
            acroFields.SetField("app.DiscRate", dt[0].DiscountRate.ToString());
            acroFields.SetField("app.TransFee", dt[0].TransactionFee.ToString());
            acroFields.SetField("app.CustServFee", dt[0].CustServFee.ToString());
            acroFields.SetField("app.MonMin", dt[0].MonMin.ToString());
            acroFields.SetField("app.SoftwareType", dt[0].Gateway);
            acroFields.SetField("app.TerminalType", dt[0].TerminalType);
            acroFields.SetField("app.TerminalModel", dt[0].TerminalModel);
            acroFields.SetField("app.CGDiscRate", dt[0].CGDiscRate.ToString());
            acroFields.SetField("app.CGTransFee", dt[0].CGTransFee.ToString());
            if ((dt[0].DebitTransFee.ToString().Trim() != "") || (dt[0].DebitMonFee.ToString().Trim() != ""))
            {
                acroFields.SetField("app.DebitTransFee", dt[0].DebitTransFee.ToString());
                acroFields.SetField("app.DebitMonFee", dt[0].DebitMonFee.ToString().Trim());
                acroFields.SetField("app.AcceptPinDebit", "Yes");
            }
            else
                acroFields.SetField("app.AcceptPinDebit", "No");

            if ((dt[0].Interchange.ToString() == "True") || (dt[0].Assessments.ToString() == "True"))
            {
                //acroFields.SetField("app.Interchange&Assessments", "Interchange + Assessments + ");
                //acroFields.SetField("app.Debit/Interchange+MQ", "Interchange + Assessments + " + dt[0].DiscMQ.ToString().Trim() + "%");
                //acroFields.SetField("app.Debit/Interchange+NQ", "Interchange + Assessments + " + dt[0].DiscNQ.ToString().Trim() + "%");
                acroFields.SetField("app.MidQualStep", dt[0].DiscMQ.ToString().Trim());
                acroFields.SetField("app.NonQualStep", dt[0].DiscNQ.ToString().Trim());
            }
            else
            {
                //acroFields.SetField("app.Debit/Interchange+MQ", "Debit/Credit Discount Rate + " + dt[0].DiscRateMidQualStep.ToString().Trim() + "% + Transaction Fee");
                //acroFields.SetField("app.Debit/Interchange+NQ", "Debit/Credit Discount Rate + " + dt[0].DiscRateNonQualStep.ToString().Trim() + "% + Transaction Fee");
                acroFields.SetField("app.MidQualStep", dt[0].DiscRateMidQualStep.ToString().Trim() + "%");
                acroFields.SetField("app.NonQualStep", dt[0].DiscRateNonQualStep.ToString().Trim() + "%");
            }

            if (dt[0].Gateway.ToString().Trim() == "Innovative Gateway")
            {
                acroFields.SetField("app.GatewayTransFee", dt[0].GatewayTransFee.ToString().Trim());
                //acroFields.SetField("app.GatewaySetupFee", dt[0].GatewaySetupFee.ToString().Trim());
                acroFields.SetField("app.GatewayMonFee", dt[0].GatewayMonFee.ToString().Trim());
            }

            if (dt[0].DiscoverAccepted == "1")
            {
                acroFields.SetField("app.DiscoverAcctNumbers", dt[0].PrevDiscoverNum);
                acroFields.SetField("app.chkDiscover", "Yes");
            }

            //Amex
            if (dt[0].PrevAmexNum.ToString() != "")
            {
                acroFields.SetField("app.AmexAcctNumbers", dt[0].PrevAmexNum);
                acroFields.SetField("app.chkAmex", "Yes");
            }
            else
                acroFields.SetField("app.chkAmex", "No");

            if (dt[0].jcbAccepted == "1")
            {
                acroFields.SetField("app.JCBAcctNumbers", dt[0].PrevJCBNum);
                acroFields.SetField("app.chkJCB", "Yes");
            }
            acroFields.SetField("app.NBCTransFee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("app.ApplicationFee", "0.00");

            if (dt[0].GiftCardType.ToString().Trim().ToLower().Contains("innovative"))
            {
                acroFields.SetField("app.chkGC", "Yes");
                acroFields.SetField("app.GCTransFee", dt[0].GCTransFee.ToString().Trim());
                acroFields.SetField("app.GCMonFee", dt[0].GCMonFee.ToString().Trim());
            }

            #endregion

            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("IMS Data not found for this record.");
            return false;
        }
    }//end function CreateIMSPDF
    #endregion*/

    #region IPS PDF
    //This function creates IMS PDF
    public bool CreateIMSPDF(string ContactID)
    {
        //Get data for IMS Application
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTIMSPDFDataTable dt = PDF.GetIMSDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Create PDFReader object by passing in the name of PDF to populate
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/IPS Application.pdf"));
            if ((dt[0].Interchange.ToString() == "True") || (dt[0].Assessments.ToString() == "True"))
                reader = new PdfReader(Server.MapPath("../PDF/IPS Application Interchange.pdf"));
            else
                reader = new PdfReader(Server.MapPath("../PDF/IPS Application.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "IPS_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            AcroFields acroFields = stamper.AcroFields;

            #region General Information
            acroFields.SetField("app.RepName", dt[0].RepName);
            acroFields.SetField("app.LegalName", dt[0].COMPANYNAME);
            acroFields.SetField("app.DBA", dt[0].DBA);
            acroFields.SetField("app.ApplicantDBA", dt[0].DBA);
            acroFields.SetField("app.Email", dt[0].Email);
            acroFields.SetField("app.ContactName", dt[0].ContactName);
            acroFields.SetField("app.Website", dt[0].Website);
            acroFields.SetField("app.MailingAddress", dt[0].BillingAddress);
            acroFields.SetField("app.MCity", dt[0].BillingCity);
            acroFields.SetField("app.MState", dt[0].BillingState);
            acroFields.SetField("app.MZip", dt[0].BillingZipCode);
            acroFields.SetField("app.LocationAddress", dt[0].Address);
            acroFields.SetField("app.LocationCity", dt[0].CITY);
            acroFields.SetField("app.LocationState", dt[0].STATE);
            acroFields.SetField("app.LZip", dt[0].ZipCode);
            acroFields.SetField("app.Years", dt[0].YearsInBusiness.ToString());
            acroFields.SetField("app.Months", dt[0].MonthsInBusiness.ToString());
            acroFields.SetField("app.FaxNumber", dt[0].Fax);
            acroFields.SetField("app.BusinessPhone", dt[0].BusinessPhone);
            acroFields.SetField("app.TaxID", dt[0].FederalTaxID);
            acroFields.SetField("app.ProductsSold", dt[0].ProductSold);
            if (dt[0].PrevIMSNum != "")
            {
                acroFields.SetField("app.chkIMSMerchant", "Yes");
                acroFields.SetField("app.chkNewMerchant", "No");
                acroFields.SetField("app.PrevIMSNum", dt[0].PrevIMSNum.ToString());
            }
            else
            {
                acroFields.SetField("app.chkIMSMerchant", "No");
                acroFields.SetField("app.chkNewMerchant", "Yes");
            }
            acroFields.SetField("app.IMSMerchantNum", dt[0].PrevIMSNum);
            acroFields.SetField("app.PrevProcessor", dt[0].PrevProcessor);
            acroFields.SetField("app.ReasonForLeaving", dt[0].ReasonForLeaving);
            acroFields.SetField("app.OtherRefund", dt[0].OtherRefund);
            if (dt[0].CTMF == "Yes")
            {
                acroFields.SetField("app.chkCTMFYes", "Yes");
                acroFields.SetField("app.chkCTMFNo", "Off");
            }
            else
            {
                acroFields.SetField("app.chkCTMFYes", "Off");
                acroFields.SetField("app.chkCTMFNo", "Yes");
            }

            if (dt[0].PrevProcessed.Contains("Yes"))
            {
                acroFields.SetField("app.chkPrevProcessedYes", "Yes");
                //acroFields.SetField("app.chkPrevProcessedNo", "Off");
            }
            else
            {
                //acroFields.SetField("app.chkPrevProcessedYes", "Off");
                acroFields.SetField("app.chkPrevProcessedNo", "Yes");
            }

            if ((dt[0].RefundPolicy == "Refund within 30 days") || (dt[0].RefundPolicy == "Refund Within 30 Days"))
                acroFields.SetField("app.chkRefund30Days", "Yes");

            if (dt[0].RefundPolicy == "Exchange Only")
                acroFields.SetField("app.chkExchangeOnly", "Yes");

            if (dt[0].RefundPolicy.Contains("Other"))
                acroFields.SetField("app.chkOtherRefund", "Yes");

            if (dt[0].RefundPolicy == "No Refund")
                acroFields.SetField("app.chkNoRefund", "Yes");

            if (dt[0].LegalStatus == "Sole Proprietorship")
                acroFields.SetField("app.chkSoleProp", "Yes");
            if (dt[0].LegalStatus == "Corporation")
                acroFields.SetField("app.chkCorporation", "Yes");
            if (dt[0].LegalStatus == "Partnership")
                acroFields.SetField("app.chkPartnership", "Yes");
            if (dt[0].LegalStatus == "Non-Profit")
                acroFields.SetField("app.chkNonProfit", "Yes");
            if (dt[0].LegalStatus == "LLC")
                acroFields.SetField("app.chkLLC", "Yes");
            #endregion

            #region CardPCT
            acroFields.SetField("app.Swiped", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("app.KeyedwImprint", dt[0].ProcessPctKeyedwImprint.ToString());
            acroFields.SetField("app.KeyedwoImprint", dt[0].ProcessPctKeyedwoImprint.ToString());
            acroFields.SetField("app.Retail", dt[0].BusinessPctRetail.ToString());
            acroFields.SetField("app.Restaurant", dt[0].BusinessPctRestaurant.ToString());
            acroFields.SetField("app.Service", dt[0].BusinessPctService.ToString());
            acroFields.SetField("app.MailPhone", dt[0].BusinessPctMailOrder.ToString().ToString());
            acroFields.SetField("app.Internet", dt[0].BusinessPctInternet.ToString().ToString());
            acroFields.SetField("app.Other", dt[0].BusinessPctOther.ToString());
            #endregion

            #region Principal #1
            //Principal #1
            acroFields.SetField("app.P1Zip", dt[0].P1ZipCode);
            acroFields.SetField("app.P1State", dt[0].P1State);
            acroFields.SetField("app.P1City", dt[0].P1City);
            acroFields.SetField("app.P1Address", dt[0].P1Address);
            acroFields.SetField("app.P1Title", dt[0].P1Title);
            acroFields.SetField("app.P1SSN", dt[0].P1SSN);
            acroFields.SetField("app.P1LastName", dt[0].P1LastName);
            acroFields.SetField("app.P1MiddleName", dt[0].P1MName);
            acroFields.SetField("app.P1FirstName", dt[0].P1FirstName);
            acroFields.SetField("app.P1FullName", dt[0].P1FirstName + " " + dt[0].P1LastName);
            acroFields.SetField("app.P1Ownership", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("app.P1DOB", dt[0].P1DOB);
            acroFields.SetField("app.P1DriversState", dt[0].P1DriversLicenseState);
            acroFields.SetField("app.P1DriversExp", dt[0].P1DriversLicenseExp);
            acroFields.SetField("app.P1DriversLicenseNo", dt[0].P1DriversLicenseNo);
            acroFields.SetField("app.P1Years", dt[0].P1YearsAtAddress);
            acroFields.SetField("app.P1Months", dt[0].P1MonthsAtAddress);
            acroFields.SetField("app.P1HomePhone", dt[0].P1PhoneNumber);
            if (dt[0].P1LivingStatus == "Rent")
                acroFields.SetField("app.chkP1Rent", "Yes");
            if (dt[0].P1LivingStatus == "Own")
                acroFields.SetField("app.chkP1Own", "Yes");
            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("app.P2Zip", dt[0].P2Zip);
            acroFields.SetField("app.P2State", dt[0].P2State);
            acroFields.SetField("app.P2City", dt[0].P2City);
            acroFields.SetField("app.P2Address", dt[0].p2Address);
            acroFields.SetField("app.P2Title", dt[0].P2Title);
            acroFields.SetField("app.P2SSN", dt[0].P2SSN);
            acroFields.SetField("app.P2LastName", dt[0].P2LastName);
            //acroFields.SetField("app.P2MiddleName", dt[0].P2MidName);
            acroFields.SetField("app.P2FirstName", dt[0].P2FirstName);
            acroFields.SetField("app.P2FullName", dt[0].P2FirstName + " " + dt[0].P2LastName);
            acroFields.SetField("app.P2Ownership", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("app.P2DOB", dt[0].P2DOB);
            acroFields.SetField("app.P2DriversState", dt[0].P2DriversLicenseState);
            acroFields.SetField("app.P2DriversExp", dt[0].P2DriversLicenseExp);
            acroFields.SetField("app.P2DriversLicenseNo", dt[0].P2DriversLicenseNo);
            acroFields.SetField("app.P2Years", dt[0].P2YearsAtAddress);
            acroFields.SetField("app.P2Months", dt[0].p2MonthsAtAddress);
            acroFields.SetField("app.P2HomePhone", dt[0].p2PhoneNumber);
            if (dt[0].P2LivingStatus == "Rent")
                acroFields.SetField("app.chkP2Rent", "Yes");
            if (dt[0].P2LivingStatus == "Own")
                acroFields.SetField("app.chkP2Own", "Yes");
            #endregion

            #region Rates
            //Rates
            acroFields.SetField("app.AvgTicket", dt[0].AverageTicket.ToString());
            acroFields.SetField("app.MonthlyVol", dt[0].MonthlyVolume.ToString());
            acroFields.SetField("app.DebitDiscRate", dt[0].DiscQD.ToString());
            acroFields.SetField("app.DiscRate", dt[0].DiscountRate.ToString());
            acroFields.SetField("app.TransFee", dt[0].TransactionFee.ToString());
            acroFields.SetField("app.CustServFee", dt[0].CustServFee.ToString());
            acroFields.SetField("app.MonMin", dt[0].MonMin.ToString());
            acroFields.SetField("app.SoftwareType", dt[0].Gateway);
            acroFields.SetField("app.TerminalType", dt[0].TerminalType);
            acroFields.SetField("app.TerminalModel", dt[0].TerminalModel);
            acroFields.SetField("app.CGDiscRate", dt[0].CGDiscRate.ToString());
            acroFields.SetField("app.CGTransFee", dt[0].CGTransFee.ToString());
            if ((dt[0].DebitTransFee.ToString().Trim() != "") || (dt[0].DebitMonFee.ToString().Trim() != ""))
            {
                acroFields.SetField("app.DebitTransFee", dt[0].DebitTransFee.ToString());
                acroFields.SetField("app.DebitMonFee", dt[0].DebitMonFee.ToString().Trim());
                acroFields.SetField("app.AcceptPinDebit", "Yes");
            }
            else
                acroFields.SetField("app.AcceptPinDebit", "No");

            if ((dt[0].Interchange.ToString() == "True") || (dt[0].Assessments.ToString() == "True"))
            {
                //acroFields.SetField("app.Interchange&Assessments", "Interchange + Assessments + ");
                //acroFields.SetField("app.Debit/Interchange+MQ", "Interchange + Assessments + " + dt[0].DiscMQ.ToString().Trim() + "%");
                //acroFields.SetField("app.Debit/Interchange+NQ", "Interchange + Assessments + " + dt[0].DiscNQ.ToString().Trim() + "%");
                acroFields.SetField("app.MidQualStep", dt[0].DiscMQ.ToString().Trim());
                acroFields.SetField("app.NonQualStep", dt[0].DiscNQ.ToString().Trim());
            }
            else
            {
                //acroFields.SetField("app.Debit/Interchange+MQ", "Debit/Credit Discount Rate + " + dt[0].DiscRateMidQualStep.ToString().Trim() + "% + Transaction Fee");
                //acroFields.SetField("app.Debit/Interchange+NQ", "Debit/Credit Discount Rate + " + dt[0].DiscRateNonQualStep.ToString().Trim() + "% + Transaction Fee");
                acroFields.SetField("app.MidQualStep", dt[0].DiscRateMidQualStep.ToString().Trim() + "%");
                acroFields.SetField("app.NonQualStep", dt[0].DiscRateNonQualStep.ToString().Trim() + "%");
            }

            if (dt[0].Gateway.ToString().Trim() == "Innovative Gateway")
            {
                acroFields.SetField("app.GatewayTransFee", dt[0].GatewayTransFee.ToString().Trim());
                //acroFields.SetField("app.GatewaySetupFee", dt[0].GatewaySetupFee.ToString().Trim());
                acroFields.SetField("app.GatewayMonFee", dt[0].GatewayMonFee.ToString().Trim());
            }

            if (dt[0].DiscoverAccepted == "1")
            {
                acroFields.SetField("app.DiscoverAcctNumbers", dt[0].PrevDiscoverNum);
                acroFields.SetField("app.chkDiscover", "Yes");
            }

            //Amex
            if (dt[0].PrevAmexNum.ToString() != "")
            {
                acroFields.SetField("app.AmexAcctNumbers", dt[0].PrevAmexNum);
                acroFields.SetField("app.chkAmex", "Yes");
            }
            else
                acroFields.SetField("app.chkAmex", "No");

            if (dt[0].jcbAccepted == "1")
            {
                acroFields.SetField("app.JCBAcctNumbers", dt[0].PrevJCBNum);
                acroFields.SetField("app.chkJCB", "Yes");
            }
            acroFields.SetField("app.NBCTransFee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("app.ApplicationFee", "0.00");

            if (dt[0].GiftCardType.ToString().Trim().ToLower().Contains("innovative"))
            {
                acroFields.SetField("app.chkGC", "Yes");
                acroFields.SetField("app.GCTransFee", dt[0].GCTransFee.ToString().Trim());
                acroFields.SetField("app.GCMonFee", dt[0].GCMonFee.ToString().Trim());
            }

            #endregion

            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("IPS Data not found for this record.");
            return false;
        }
    }//end function CreateIMSPDF
    #endregion

    #region SAGE PDF

    protected void btnSageApp_Click(object sender, EventArgs e)
    {
        try
        {
            CreateSagePDF(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
    protected void btnSageMOTO_Click(object sender, EventArgs e)
    {
        try
        {
            CreateSageMOTO(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnSageAgreement_Click(object sender, EventArgs e)
    {
        try
        {
            CreateSageAgreement(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
    //This function creates Sage PDF
    public bool CreateSagePDF(string ContactID)
    {
        //Get data for Sage Application
        PDFBL SageData = new PDFBL();
        PartnerDS.ACTSagePDFDataTable dt = SageData.GetSageDataFromACT(ContactID);

        if (dt.Rows.Count > 0)
        {
            string strSageAppDoc = "../PDF/Sage Application.pdf";
            /*
            if (!Convert.IsDBNull(dt[0].P2FirstName))
            {
                if (Convert.ToString(dt[0].P2FirstName) != "")
                {
                    strSageAppDoc = "../PDF/Sage Application 2 signers.pdf";
                }
            }*/
            //Populate data in PDF
            PdfReader reader = new PdfReader(Server.MapPath(strSageAppDoc));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "";
            if (FilePath != string.Empty)
            {

               
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                
                strPath = Server.MapPath(strHost + FilePath + "/" + "Sage_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
                //strPath = Server.MapPath("SageApp.pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            AcroFields acroFields = stamper.AcroFields;

            if (dt[0].Platform.ToString().Trim().Contains("Paymentech"))
                acroFields.SetField("Authorization Network", "Paymentech");
            else
                acroFields.SetField("Authorization Network", "Visanet/TSYS");

            #region General Information
            acroFields.SetField("Contractor Name", dt[0].RepName);


            acroFields.SetField("Merch Legal Business Name", dt[0].COMPANYNAME);
            acroFields.SetField("Merch MA_Address", dt[0].BillingAddress);
            acroFields.SetField("Merch MA_City", dt[0].BillingCity);
            acroFields.SetField("Merch MA_State", dt[0].BillingState);
            acroFields.SetField("Merch MA_Zip", dt[0].BillingZipCode);
            acroFields.SetField("Bus Info Legal Business Name", dt[0].COMPANYNAME);

            acroFields.SetField("Legal Business Name", dt[0].COMPANYNAME);
            acroFields.SetField("MA_Address", dt[0].BillingAddress);
            acroFields.SetField("MA_City", dt[0].BillingCity);
            acroFields.SetField("MA_State", dt[0].BillingState);
            acroFields.SetField("MA_Zip", dt[0].BillingZipCode);
            acroFields.SetField("Contact Name", dt[0].ContactName);
            //acroFields.SetField("Contact Title", dt[0].P1Title);
            acroFields.SetField("Phone", dt[0].BusinessPhone);
            acroFields.SetField("Email", dt[0].Email);

            if ((dt[0].MIB.ToString().Trim() != "") && (dt[0].YIB.ToString().Trim() != ""))
            {
                DateTime BusinessDate = DateTime.Now.AddMonths((Convert.ToInt32(dt[0].MIB.ToString().Trim())) * -1);
                DateTime BusinessOpenDate = BusinessDate.AddYears((Convert.ToInt32(dt[0].YIB.ToString().Trim())) * -1);
                string BusinessOpenMonth = BusinessOpenDate.Month.ToString().Trim();
                string BusinessOpenYear = BusinessOpenDate.Year.ToString().Trim();
                acroFields.SetField("Business Open Date", BusinessOpenMonth + "/" + BusinessOpenYear);

                string LengthOfOwnership = "";
                if (dt[0].YIB.ToString().Trim() != "")
                    LengthOfOwnership = dt[0].YIB.ToString().Trim() + " Years ";
                else
                    LengthOfOwnership = "0 Years ";

                if (dt[0].MIB.ToString().Trim() != "")
                    LengthOfOwnership += "and " + dt[0].MIB.ToString() + " Months";

                acroFields.SetField("Length Of Ownership", LengthOfOwnership);
            }

            if (dt[0].PrevProcessor.ToString().Contains("Sage"))
                acroFields.SetField("Existing Sage MID", dt[0].PrevMerchantAcctNo.ToString().Trim());

            acroFields.SetField("Business Name DBA", dt[0].DBA);
            acroFields.SetField("Address", dt[0].Address);
            acroFields.SetField("City", dt[0].CITY);
            acroFields.SetField("State", dt[0].STATE);
            acroFields.SetField("Zip", dt[0].ZipCode);
            acroFields.SetField("Phone_2", dt[0].BusinessPhone);
            acroFields.SetField("Fax_2", dt[0].Fax);
            acroFields.SetField("Web Site", dt[0].Website);
            acroFields.SetField("Customer Service Phone", dt[0].CustServPhone);
            acroFields.SetField("Number of Locations", dt[0].NumberOfLocations);

            acroFields.SetField("Fed Tax ID", dt[0].FederalTaxID);

                       
            //acroFields.SetField("app.HowLong", dt[0].TABL);
            if (dt[0].LegalStatus.ToString().ToLower().Contains("trust"))
                acroFields.SetField("OwnershipTypeAsso", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("llc"))
                acroFields.SetField("OwnershipTypellc", "Yes");
            else if ((dt[0].LegalStatus.ToString().ToLower().Contains("non-profit")) || (dt[0].LegalStatus.ToString().ToLower().Contains("tax exempt")))
                acroFields.SetField("OwnershipTypeTaxExmpt", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("corporation"))
                acroFields.SetField("OwnershipTypeCorp", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("medical"))
                acroFields.SetField("OwnershipTypemedical", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("government"))
                acroFields.SetField("OwnershipTypeGov", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("int'l"))
                acroFields.SetField("OwnershipTypeintl", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("partnership"))
                acroFields.SetField("OwnershipTypepartnership", "Yes");
            else if (dt[0].LegalStatus.ToString().ToLower().Contains("sole proprietorship"))
                acroFields.SetField("OwnershipTypeSol", "Yes");

            #endregion

            #region Owners/Officers
            //Principal #1
            acroFields.SetField("P1 Ownership%", dt[0].P1OwnershipPercent.ToString());
             acroFields.SetField("P1 Full Name", dt[0].P1FirstName + " " + dt[0].P1LastName);
                acroFields.SetField("P1 Full Name 2", dt[0].P1FirstName + " " + dt[0].P1LastName);
                acroFields.SetField("P1 Full Name 3", dt[0].P1FirstName + " " + dt[0].P1LastName);
                acroFields.SetField("P1 Full Name 4", dt[0].P1FirstName + " " + dt[0].P1LastName);
                acroFields.SetField("P1 Title", dt[0].P1Title);
                acroFields.SetField("P1 Title 2", dt[0].P1Title);
                acroFields.SetField("P1 Title 3", dt[0].P1Title);
            acroFields.SetField("P1 Full Name", dt[0].P1FirstName + " " + dt[0].P1LastName);
            acroFields.SetField("P1 Title", dt[0].P1Title);
            acroFields.SetField("P1 Address", dt[0].P1Address);
            acroFields.SetField("P1 City, State, Zip", dt[0].P1City + ", " + dt[0].P1State);
            acroFields.SetField("P1 Zip", dt[0].P1ZipCode);
            acroFields.SetField("P1 Phone", dt[0].P1PhoneNumber);
            acroFields.SetField("P1 Email", dt[0].Email.ToString().Trim());
            acroFields.SetField("P1 SSN", dt[0].P1SSN);
            acroFields.SetField("P1 DOB", dt[0].P1DOB);

            //Principal #2
            acroFields.SetField("P2 Ownership%", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("P2 Full Name", dt[0].P2FirstName + " " + dt[0].P2LastName);
            acroFields.SetField("P2 Full Name", dt[0].P2FirstName + " " + dt[0].P2LastName);
                acroFields.SetField("P2 Full Name 2", dt[0].P2FirstName + " " + dt[0].P2LastName);
                acroFields.SetField("P2 Full Name 3", dt[0].P2FirstName + " " + dt[0].P2LastName);
                acroFields.SetField("P2 Title", dt[0].P2Title);
                acroFields.SetField("P2 Title 2", dt[0].P2Title);
            acroFields.SetField("P2 Title", dt[0].P2Title);
            acroFields.SetField("P2 Address", dt[0].p2Address);
            acroFields.SetField("P2 City, State, Zip", dt[0].P2City + ", " + dt[0].P2State);
            acroFields.SetField("P2 Zip", dt[0].P2ZipCode);
            acroFields.SetField("P2 Phone", dt[0].p2PhoneNumber);
            acroFields.SetField("P2 Email", dt[0].P2Email.ToString().Trim());
            acroFields.SetField("P2 SSN", dt[0].P2SSN);
            acroFields.SetField("P2 DOB", dt[0].P2DOB);

            
            //Trade Reference
            acroFields.SetField("TR Name", dt[0].p1RelName);
            acroFields.SetField("TR Title", "");
            acroFields.SetField("TR Address", dt[0].p1RelAddr);
            acroFields.SetField("TR City", dt[0].P1RelCity);
            acroFields.SetField("TR State", dt[0].p1RelState);
            acroFields.SetField("TR Zip", dt[0].p1RelZip);
            acroFields.SetField("TR Phone", dt[0].p1RelPhone);

            #endregion 

            #region General Underwriting Profile

            if (dt[0].BusinessPctMailOrder.ToString().Trim() == "100")
                acroFields.SetField("BusinessType", "moto");
            else if (dt[0].BusinessPctInternet.ToString().Trim() == "100")
                acroFields.SetField("BusinessType", "internet");
            else if (dt[0].PctRet.ToString().Trim() == "100")
                acroFields.SetField("BusinessType", "retail");
            else if (dt[0].PctRest.ToString().Trim() == "100")
                acroFields.SetField("BusinessType", "restaurant");

            acroFields.SetField("Products Sold", dt[0].ProductSold);

            if (dt[0].RefundPolicy == "Refund within 30 days")
                acroFields.SetField("Return Policy", "30 Days Money Back Guarantee");
            else if (dt[0].RefundPolicy == "Exchange Only")
                acroFields.SetField("Return Policy", "30 Days Exchange Only");
            else if (dt[0].RefundPolicy == "No Refund")
                acroFields.SetField("Return Policy", "No Refund");
            else if (dt[0].RefundPolicy.Contains("Other"))
                acroFields.SetField("Return Policy", "Other");

            acroFields.SetField("Days Until Product Delivery", dt[0].NumDaysDelivered);
            #endregion

            #region Credit Card Underwriting Profile
            acroFields.SetField("Monthly Volume", dt[0].MonthlyVolume.ToString());
            acroFields.SetField("Average Ticket", dt[0].AverageTicket.ToString());
            acroFields.SetField("Highest Ticket", dt[0].MaxTicket.ToString());
            acroFields.SetField("Discount Paid", "monthly");
            acroFields.SetField("Current Processor", dt[0].PrevProcessor);

            acroFields.SetField("Card Present Swiped", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("Card Present Imprint", dt[0].ProcessPctKeyedWImprint.ToString());
            acroFields.SetField("Card Not Present", dt[0].ProcessPctKeyedWoImprint.ToString());
            #endregion

            #region MOTO/Internet Questionnaire
            acroFields.SetField("Days Until Product Delivery", dt[0].NumDaysDelivered);
            #endregion

            #region ACH Bank
            //Baking
            acroFields.SetField("ACH Bank Name", dt[0].BankName);
            acroFields.SetField("ACH Address 1", dt[0].BankAddress);
            acroFields.SetField("ACH City", dt[0].BankCity);
            acroFields.SetField("ACH State", dt[0].BankState);
            acroFields.SetField("ACH Zip", dt[0].BankZip);
            //acroFields.SetField("ACH Phone", dt[0].BankPhone);
            acroFields.SetField("ACH Routing Number", dt[0].BankRoutingNumber);
            acroFields.SetField("ACH Account Number", dt[0].BankAccountNumber);
            #endregion
            
            #region Rates
            acroFields.SetField("Visa Rate1", dt[0].DiscountRate.ToString().Trim());
            acroFields.SetField("MasterCard Rate1", dt[0].DiscountRate.ToString().Trim());
            acroFields.SetField("Disc Rate1", dt[0].DiscountRate.ToString().Trim());

            if ((dt[0].DebitStatus.ToString().ToLower().Contains("yes")) || (!Convert.IsDBNull(dt[0].DebitMonFee)) || (!Convert.IsDBNull(dt[0].DebitTransFee)))
            {
                if ((Convert.ToString(dt[0].DebitMonFee) != "") || (Convert.ToString(dt[0].DebitTransFee) != ""))
                {
                    acroFields.SetField("PinDebitY", "Yes");
                    acroFields.SetField("DPTY", "Yes");
                    acroFields.SetField("PinDebit Rate1", "0.00");
                }
            }
            else
                acroFields.SetField("PinDebitN", "No");


            if (Convert.ToString(dt[0].DiscountPaid).Trim() == "Daily")
            {
                acroFields.SetField("DailyDisc", "Yes");
            }
            else if (Convert.ToString(dt[0].DiscountPaid).Trim() == "Monthly")
            {
                acroFields.SetField("MonthlyDisc", "Yes");
            }
            

            acroFields.SetField("Visa Rate2", dt[0].DiscRateMidQual.ToString().Trim());
            acroFields.SetField("MasterCard Rate2", dt[0].DiscRateMidQual.ToString().Trim());
            acroFields.SetField("Disc Rate2", dt[0].DiscRateMidQual.ToString().Trim());

            acroFields.SetField("Visa Rate3", dt[0].DiscRateNonQual.ToString().Trim());
            acroFields.SetField("MasterCard Rate3", dt[0].DiscRateNonQual.ToString().Trim());
            acroFields.SetField("Disc Rate3", dt[0].DiscRateNonQual.ToString().Trim());

            acroFields.SetField("Visa INTL/NS", "1.15");
            acroFields.SetField("MC INTL/NS", "1.15");
            acroFields.SetField("Disc INTL/NS", "1.15");

            acroFields.SetField("Visa INTL/NS Surcharge", "0.20");
            acroFields.SetField("MC INTL/NS Surcharge", "0.20");
            acroFields.SetField("Disc INTL/NS Surcharge", "0.20");

            acroFields.SetField("Visa Business", dt[0].DiscRateNonQual.ToString().Trim());
            acroFields.SetField("MC Business", dt[0].DiscRateNonQual.ToString().Trim());
            acroFields.SetField("Disc Business", dt[0].DiscRateNonQual.ToString().Trim());
            
            if (dt[0].Interchange.ToString() == "True")
            {
                acroFields.SetField("VisaIPT", "Yes");
                acroFields.SetField("MCIPT", "Yes");
                acroFields.SetField("DiscIPT", "Yes");

                acroFields.SetField("Visa INTL/NS", "0.00");
                acroFields.SetField("MC INTL/NS", "0.00");
                acroFields.SetField("Disc INTL/NS", "0.00");

                acroFields.SetField("Visa INTL/NS Surcharge", "0.00");
                acroFields.SetField("MC INTL/NS Surcharge", "0.00");
                acroFields.SetField("Disc INTL/NS Surcharge", "0.00");
            }
            
            //American Express
            if (dt[0].PrevAmexNum == "Opted Out")
                acroFields.SetField("American Express Other", "None");
            else if ((dt[0].PrevAmexNum == "Submitted") || (dt[0].PrevAmexNum == "Yes"))
                acroFields.SetField("American Express Other", "New");
            else if (dt[0].PrevAmexNum != "")
            {
                acroFields.SetField("American Express Other", "Existing");
                acroFields.SetField("American Express Existing #", dt[0].PrevAmexNum);
            }

            //Discover
            /*if (dt[0].PrevDiscoverNum == "Opted Out")
                acroFields.SetField("Discover Other", "None");
            else if ((dt[0].PrevDiscoverNum == "Submitted") || (dt[0].PrevDiscoverNum == "Yes"))
                acroFields.SetField("Discover Other", "New");
            else if (dt[0].PrevDiscoverNum != "")
            {
                acroFields.SetField("Discover Other", "Existing");
                acroFields.SetField("Discover Existing #", dt[0].PrevDiscoverNum);
            }

            //JCB
            if (dt[0].PrevJCBNum == "Opted Out")
                acroFields.SetField("JCB Other", "None");
            else if ((dt[0].PrevJCBNum == "Submitted") || (dt[0].PrevJCBNum == "Yes"))
                acroFields.SetField("JCB Other", "New");
            else if (dt[0].PrevJCBNum != "")
            {
                acroFields.SetField("JCB Other", "Existing");
                acroFields.SetField("JCB Existing #", dt[0].PrevJCBNum);
            }*/
            
            acroFields.SetField("Visa MC Auth Fee", dt[0].TransactionFee.ToString().Trim());
            acroFields.SetField("Discover Auth Fee", dt[0].NBCTransFee.ToString().Trim());
            acroFields.SetField("American Express Auth Fee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("ARU Auth Fee", dt[0].VoiceAuth.ToString().ToString());
            acroFields.SetField("Carte Blanche Auth Fee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("Diners Club Auth Fee", dt[0].NBCTransFee.ToString().Trim());
            acroFields.SetField("EBT Auth Fee", dt[0].EBTTransFee.ToString().Trim());
            acroFields.SetField("JCB Auth Fee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("PIN Debit Auth Fee", dt[0].DebitTransFee.ToString());
            acroFields.SetField("Voice Authorization", dt[0].VoiceAuth.ToString().ToString());

            acroFields.SetField("Application Credit", dt[0].AppFee.ToString());
            if ((dt[0].WirelessAccessFee.ToString() != "") || (dt[0].WirelessTransFee.ToString() != ""))
                acroFields.SetField("Wireless Set Up", "35.00");

            if (dt[0].Gateway.ToString().ToLower().Contains("roampay"))
            {
                acroFields.SetField("Sage Mobile Payments Setup", "25.00");
                acroFields.SetField("Sage Mobile Payments Access", dt[0].GatewayMonFee.ToString());
                decimal TransFee = Convert.ToDecimal(dt[0].TransactionFee) + Convert.ToDecimal(dt[0].GatewayTransFee);
                string strTransFee = TransFee.ToString();
                acroFields.SetField("Visa MC Auth Fee", strTransFee);
                decimal NBCTransFee = Convert.ToDecimal(dt[0].NBCTransFee) + Convert.ToDecimal(dt[0].GatewayTransFee);
                string strNBCTransFee = NBCTransFee.ToString();
                acroFields.SetField("Discover Auth Fee", strNBCTransFee);
                acroFields.SetField("American Express Auth Fee", strNBCTransFee);
                acroFields.SetField("Carte Blanche Auth Fee", strNBCTransFee);
                acroFields.SetField("Diners Club Auth Fee", strNBCTransFee);
                acroFields.SetField("JCB Auth Fee", strNBCTransFee);
            }

            if (dt[0].Gateway.ToString().ToLower().Contains("sage gateway"))
            {
                acroFields.SetField("Sage Mobile Payments Setup", "25.00");
                //acroFields.SetField("Sage Mobile Payments Access", dt[0].GatewayMonFee.ToString());
                decimal TransFee = Convert.ToDecimal(dt[0].TransactionFee) + Convert.ToDecimal(dt[0].GatewayTransFee);
                string strTransFee = TransFee.ToString();
                acroFields.SetField("Visa MC Auth Fee", strTransFee);
                decimal NBCTransFee = Convert.ToDecimal(dt[0].NBCTransFee) + Convert.ToDecimal(dt[0].GatewayTransFee);
                string strNBCTransFee = NBCTransFee.ToString();
                acroFields.SetField("Discover Auth Fee", strNBCTransFee);
                acroFields.SetField("American Express Auth Fee", strNBCTransFee);
                acroFields.SetField("Carte Blanche Auth Fee", strNBCTransFee);
                acroFields.SetField("Diners Club Auth Fee", strNBCTransFee);
                acroFields.SetField("JCB Auth Fee", strNBCTransFee);
            }

            acroFields.SetField("Statement", "0.00");
            acroFields.SetField("Monthly Support", dt[0].CustServFee.ToString());
            acroFields.SetField("Monthly Minimum", dt[0].MonMin.ToString());
            if(dt[0].Gateway.ToString().Contains("Sage"))
                acroFields.SetField("Gateway Access", dt[0].GatewayMonFee.ToString());
            acroFields.SetField("Debit Access", dt[0].DebitMonFee.ToString());
            acroFields.SetField("Wireless Access", dt[0].WirelessAccessFee.ToString());

            if (dt[0].AnnualFeeCP.ToString() != "")
                acroFields.SetField("Annual Assessment", dt[0].AnnualFeeCP.ToString());
            else
                acroFields.SetField("Annual Assessment", dt[0].AnnualFeeCNP.ToString());
            acroFields.SetField("Chargeback", dt[0].ChargebackFee.ToString());
            acroFields.SetField("Signature Rate", dt[0].DiscRateQualDebit.ToString());            
            if (dt[0].Gateway.ToString().Trim() != "")
                acroFields.SetField("Terminal/Software Type", dt[0].Gateway.ToString().Trim());
            else
                acroFields.SetField("Terminal/Software Type", dt[0].Equipment.ToString().Trim());

            #endregion

            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("IPayment Data not found for this record.");
            return false;
        }
    }//end function CreateSagePDF

    public bool CreateSageMOTO(string ContactID)
    {
        //Get data for Sage Application
        PDFBL SageData = new PDFBL();
        PartnerDS.ACTSagePDFDataTable dt = SageData.GetSageDataFromACT(ContactID);

        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Sage MOTO-Internet Question.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "SageMOTO_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Contractor Name", dt[0].RepName.ToString().Trim() + " / " + dt[0].RepNum.ToString().Trim());
            acroFields.SetField("Business Name DBA", dt[0].DBA);
            acroFields.SetField("Contact Name", dt[0].ContactName);
            acroFields.SetField("Phone", dt[0].BusinessPhone);
            acroFields.SetField("Address Line1", dt[0].Address);
            acroFields.SetField("Address Line2", dt[0].CITY + ", " + dt[0].STATE + " " + dt[0].ZipCode);
            acroFields.SetField("Customer Service Phone", dt[0].CustServPhone);
            acroFields.SetField("Products Sold", dt[0].ProductSold);
            
            if (dt[0].RefundPolicy == "Refund within 30 days")
                acroFields.SetField("Return Policy", "30 Days Money Back Guarantee");
            else if (dt[0].RefundPolicy == "Exchange Only")
                acroFields.SetField("Return Policy", "30 Days Exchange Only");
            else if (dt[0].RefundPolicy == "No Refund")
                acroFields.SetField("Return Policy", "No Refund");
            else if (dt[0].RefundPolicy.Contains("Other"))
                acroFields.SetField("Return Policy", dt[0].OtherRefund.ToString());

            acroFields.SetField("Days Until Product Delivery", dt[0].NumDaysDelivered);
            acroFields.SetField("Web Site", dt[0].Website);

            acroFields.SetField("Principal Name", dt[0].P1FirstName + " " + dt[0].P1LastName);

            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("IPayment Data not found for this record.");
            return false;
        }
    }//end function CreateSageMOTO

    public bool CreateSageAgreement(string ContactID)
    {
        //Get data for Sage Application
        PDFBL SageData = new PDFBL();
        PartnerDS.ACTSagePDFDataTable dt = SageData.GetSageDataFromACT(ContactID);

        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Sage Merchant Agreement.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "Sage Merchant Agreement.pdf");
                //strPath = Server.MapPath("SageAgreement.pdf");
                FileStream fStream = null;
                fStream = new FileStream(strPath, FileMode.Create);
                PdfStamper stamper = new PdfStamper(reader, fStream);
                stamper.FormFlattening = true;
                stamper.Close();
            }
            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("IPayment Data not found for this record.");
            return false;
        }
    }
    #endregion

    #region IPAYMENT PDF
    //This function creates iPayment PDF
    public bool CreateIPayPDF(string ContactID)
    {
        //Get data for IPayment Application
        PDFBL IPayData = new PDFBL();
        PartnerDS.ACTiPayPDFDataTable dt = IPayData.GetIPayDataFromACT(ContactID);

        if (dt.Rows.Count > 0)
        {

            string striPayAppPath = "../PDF/ipayment application.pdf";
            /*
            if (!Convert.IsDBNull(dt[0].P2FirstName))
            { 
                if (Convert.ToString(dt[0].P2FirstName) != "")
                {
                    striPayAppPath = "../PDF/ipayment application two signers.pdf";
                }
            }*/
            //Populate data in PDF
            PdfReader reader = new PdfReader(Server.MapPath(striPayAppPath));
            
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "iPayment_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
                //strPath = Server.MapPath("iPayTest.pdf");
                //strPath = Server.MapPath( "iPayment_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
            }
            
            /*MemoryStream mStream = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, mStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            */
            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            AcroFields acroFields = stamper.AcroFields;
            //Code for Previous PDF Version
            #region Old App
            /*
            #region General Information
            acroFields.SetField("Representative Name", dt[0].RepName);
            acroFields.SetField("Rep #", dt[0].RepNum);
            acroFields.SetField("Office #", "248");
            acroFields.SetField("Rep's phone #", dt[0].RepPhone);
            acroFields.SetField("Legal Name", dt[0].COMPANYNAME);
            acroFields.SetField("DBA", dt[0].DBA);
            acroFields.SetField("Business Address", dt[0].Address);
            acroFields.SetField("City / State", dt[0].CITY + ", " + dt[0].STATE + ", " + dt[0].ZipCode);
            acroFields.SetField("How Long", dt[0].TABL);
            acroFields.SetField("Mailing Address", dt[0].BillingAddress);                      
            acroFields.SetField("M City / State", dt[0].BillingCity + ", " + dt[0].BillingState + ", " + dt[0].BillingZipCode);
            acroFields.SetField("Federal Tax ID", dt[0].FederalTaxID);
            acroFields.SetField("Business Phone", dt[0].BusinessPhone);
            acroFields.SetField("Customer Service Phone", dt[0].CustServPhone);
            acroFields.SetField("Fax #", dt[0].Fax);
            acroFields.SetField("Contact Name", dt[0].ContactName);
            acroFields.SetField("# of Locations", dt[0].NumberOfLocations);
            acroFields.SetField("Time in business", dt[0].YIB.ToString());
            acroFields.SetField("Time in Business", dt[0].MIB.ToString());
            acroFields.SetField("Business Hours", dt[0].BusinessHours);
            
            acroFields.SetField("Email Address", dt[0].Email);
            acroFields.SetField("Business Website", dt[0].Website);

            #region CardPCT
            acroFields.SetField("Retail Swipe", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("Retail Keyed.ToString()", dt[0].ProcessPctKeyed.ToString());
            acroFields.SetField("Mail Order.ToString()", dt[0].BusinessPctMailOrder.ToString());
            acroFields.SetField("Internet.ToString()", dt[0].BusinessPctInternet.ToString());
            #endregion

            acroFields.SetField("Product type/sold", dt[0].ProductSold);
            acroFields.SetField("Previous Processor", dt[0].PrevProcessor);
            acroFields.SetField("Former Merchant #", dt[0].PrevMerchantAcctNo);

            #region RefundPolicy
            if ((dt[0].RefundPolicy == "Refund within 30 days") || (dt[0].RefundPolicy == "Refund Within 30 Days"))
                acroFields.SetField("Check Box80", "Yes");

            if (dt[0].RefundPolicy == "Exchange Only")
                acroFields.SetField("Check Box81", "Yes");

            if (dt[0].RefundPolicy == "No Refund")
                acroFields.SetField("Check Box82", "Yes");

            if (dt[0].RefundPolicy.Contains("Other"))
                acroFields.SetField("Check Box83", "Yes");
            acroFields.SetField("Customer Return Policy", dt[0].OtherRefund);
            #endregion

            //acroFields.SetField("AddlComments", dt[0].AddlComments);
            //acroFields.SetField("BusinessPhoneExt", dt[0].BusinessPhoneExt);
            acroFields.SetField("# of days product delivered", dt[0].NumDaysDel);

            if (dt[0].CTMF == "Yes")
            {
                acroFields.SetField("Check Box94", "Yes");
                acroFields.SetField("Check Box95", "Off");
            }
            else
            {
                acroFields.SetField("Check Box94", "Off");
                acroFields.SetField("Check Box95", "Yes");
            }

            if (dt[0].PrevProcessed == "Yes")
            {
                acroFields.SetField("Check Box250", "Yes");
                //acroFields.SetField("Check Box91", "Off");
            }
            else
            {
                //acroFields.SetField("Check Box90", "Off");
                acroFields.SetField("Check Box251", "Yes");
            }

            //if (dt[0].Reprogram == "Yes")
            //acroFields.SetField("chkReprogram", "Yes");
         
            if (dt[0].LegalStatus == "Sole Proprietorship")
                acroFields.SetField("Check Box43", "Yes");
            if (dt[0].LegalStatus == "Corporation")
                acroFields.SetField("Check Box44", "Yes");
            if (dt[0].LegalStatus == "Partnership")
                acroFields.SetField("Check Box47", "Yes");
            if (dt[0].LegalStatus == "Non-Profit")
                acroFields.SetField("Check Box48", "Yes");
            if (dt[0].LegalStatus == "Legal/Medical Corp.")
                acroFields.SetField("Check Box52", "Yes");
            if (dt[0].LegalStatus == "Government")
                acroFields.SetField("Check Box49", "Yes");
            if (dt[0].LegalStatus == "Tax Exempt")
                acroFields.SetField("Check Box50", "Yes");
            if (dt[0].LegalStatus == "Other")
                acroFields.SetField("Check Box46", "Yes");
            if (dt[0].LegalStatus == "LLC")
                acroFields.SetField("Check Box45", "Yes");

            if (dt[0].Equipment != "")
            {
                string equipment = dt[0].Equipment;           
                if (equipment.Contains("Hypercom"))
                    acroFields.SetField("Check Box135", "Yes");            
                else if (equipment.Contains("Verifone"))
                    acroFields.SetField("Check Box136", "Yes");
                else if (equipment.Contains("Nurit"))
                    acroFields.SetField("Check Box137", "Yes");
                else
                {
                    acroFields.SetField("Check Box138", "Yes");
                    acroFields.SetField("Other manufacturer", equipment);
                }
                acroFields.SetField("Terminal Model", equipment);
            }

            #endregion          

            #region Principal #1
            //Principal #1
            acroFields.SetField("Principal 1 Name", dt[0].P1FirstName + " " + dt[0].P1LastName);
            acroFields.SetField("P1 SS Number", dt[0].P1SSN);
            acroFields.SetField("P1 ownership %", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("P1 Title", dt[0].P1Title);
            acroFields.SetField("P1 Address", dt[0].P1Address);
            acroFields.SetField("P1 City", dt[0].P1City);
            acroFields.SetField("P1 State", dt[0].P1State);          
            acroFields.SetField("P1 Zip", dt[0].P1ZipCode);
            acroFields.SetField("How long at address", dt[0].P1TimeAtAddress);
            acroFields.SetField("Home Telephone", dt[0].P1PhoneNumber);        
            acroFields.SetField("P1 Date of Birth", dt[0].P1DOB);
            acroFields.SetField("P1 License # and State",  dt[0].P1DriversLicenseNo + " " + dt[0].P1DriversLicenseState);
            
            if (dt[0].P1LivingStatus == "Rent")
                acroFields.SetField("Check Box104", "Yes");
            if (dt[0].P1LivingStatus == "Own")
                acroFields.SetField("Check Box105", "Yes");
            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("Principal 2 name", dt[0].P2FirstName + " " + dt[0].P2LastName);
            acroFields.SetField("P2 SSN", dt[0].P2SSN);
            acroFields.SetField("P2 ownership %", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("P2 Title", dt[0].P2Title);
            acroFields.SetField("P2 Address", dt[0].p2Address);
            acroFields.SetField("P2 City", dt[0].P2City);
            acroFields.SetField("P2 State", dt[0].P2State);          
            acroFields.SetField("P2 Zip", dt[0].P2ZipCode);
            //acroFields.SetField("P2 How long at address", dt[0].P2TimeAtAddress);
          
            acroFields.SetField("P2DOB", dt[0].P2DOB);
            acroFields.SetField("P2 Drivers License and State",dt[0].P2DriversLicenseNo + " " + dt[0].P2DriversLicenseState);
            acroFields.SetField("P2 Home Telephone", dt[0].p2PhoneNumber);
            
            if (dt[0].P2LivingStatus == "Rent")
                acroFields.SetField("Check Box118", "Yes");
            if (dt[0].P2LivingStatus == "Own")
                acroFields.SetField("Check Box119", "Yes");
            #endregion

            #region Rates
            //Rates
            //acroFields.SetField("MCC Code", dt[0].SICCode.ToString());
            acroFields.SetField("Average Ticket", dt[0].AverageTicket.ToString());
            acroFields.SetField("Monthly Sales Processing Limit", dt[0].MonthlyVolume.ToString());
            acroFields.SetField("V/MC Transaction Fee", dt[0].TransactionFee.ToString());
            acroFields.SetField("NBC Trans Fee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("Mon Min", dt[0].MonMin.ToString());
            acroFields.SetField("Annual Fee", dt[0].AnnualFeeCNP.ToString());
            
            acroFields.SetField("Customer Service Fee", dt[0].CustServFee.ToString());
            acroFields.SetField("RetrievalRequest", dt[0].RetrievalFee.ToString());
            acroFields.SetField("ChargeBacks", dt[0].ChargebackFee.ToString());
            acroFields.SetField("Application Fee", dt[0].AppFee.ToString());
            //acroFields.SetField("SetupFee", dt[0].AppSetupFee.ToString());
            acroFields.SetField("AVS", dt[0].AVS.ToString());
            //acroFields.SetField("BatchHeader", dt[0].BatchHeader);
            acroFields.SetField("VoiceAuth", dt[0].VoiceAuth.ToString());
            acroFields.SetField("BatchHeader", dt[0].BatchHeader.ToString());
            acroFields.SetField("Monthly Wireless Fee", dt[0].WirelessAccessFee.ToString());
            acroFields.SetField("Wireless Auth Fee", dt[0].WirelessTransFee.ToString());
            acroFields.SetField("PIN Debit Card Fee", dt[0].DebitMonFee.ToString());
            acroFields.SetField("PIN Debit Card Trans Fee", dt[0].DebitTransFee.ToString());
			
			acroFields.SetField("DebitMid-QualifiedFee", Convert.ToString((Convert.ToDouble(dt[0].DiscRateMidQual)) - (Convert.ToDouble(dt[0].DiscRateQualDebit))));
			acroFields.SetField("DebitNon-QualifiedFee", Convert.ToString((Convert.ToDouble(dt[0].DiscRateNonQual)) - (Convert.ToDouble(dt[0].DiscRateQualDebit))));
			
			acroFields.SetField("CreditMid-QualifiedFee", Convert.ToString((Convert.ToDouble(dt[0].DiscRateMidQual)) - (Convert.ToDouble(dt[0].DiscountRate))));
			acroFields.SetField("CreditNon-QualifiedFee", Convert.ToString((Convert.ToDouble(dt[0].DiscRateNonQual)) - (Convert.ToDouble(dt[0].DiscountRate))));
			
            //If Restaurant percentage is ZERO or BLANK
            /*if ((dt[0].PctRest.ToString() == "") || (Convert.ToDouble(dt[0].PctRest.ToString()) == 0))
            {
                acroFields.SetField("Credit Card Qualified Fee", dt[0].DiscountRate.ToString());
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateMidQual.ToString() != ""))
                    acroFields.SetField("Credit Card Mid Qualified Fee", Convert.ToString((Convert.ToDecimal(dt[0].DiscRateMidQual)) - (Convert.ToDecimal(dt[0].DiscountRate.ToString()))));
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateNonQual.ToString() != ""))
                    acroFields.SetField("Credit Card Non Qualified Fee", Convert.ToString((Convert.ToDecimal(dt[0].DiscRateNonQual)) - (Convert.ToDecimal(dt[0].DiscountRate.ToString()))));
         
            }
            else //Restaurant Pct is greater than ZERO              
            {
                acroFields.SetField("Restaurant/Lodging Rate", dt[0].DiscountRate.ToString());
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateMidQual.ToString() != ""))
                    acroFields.SetField("Restaurant/Lodging Mid Qualified", Convert.ToString((Convert.ToDecimal(dt[0].DiscRateMidQual)) - (Convert.ToDecimal(dt[0].DiscountRate.ToString()))));
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateNonQual.ToString() != ""))
                    acroFields.SetField("Restaurant/Lodging Non Qualified", Convert.ToString((Convert.ToDecimal(dt[0].DiscRateNonQual)) - (Convert.ToDecimal(dt[0].DiscountRate.ToString()))));            
            }
            if (dt[0].DiscRateQualDebit.ToString() != "") 
            {
                acroFields.SetField("Debit Card Qualified Rate", dt[0].DiscRateQualDebit.ToString());
                if (dt[0].DiscRateMidQual.ToString() != "")
                {
                    acroFields.SetField("Debit Card Qualified Mid Qual", Convert.ToString((Convert.ToDecimal(dt[0].DiscRateMidQual)) - (Convert.ToDecimal(dt[0].DiscRateQualDebit))));
                    acroFields.SetField("Debit Card Qualified Non Qual", Convert.ToString((Convert.ToDecimal(dt[0].DiscRateNonQual)) - (Convert.ToDecimal(dt[0].DiscRateQualDebit))));
                }
            }*/
            
            /*
            if (dt[0].Gateway != "")
            {
                acroFields.SetField("Check Box169", "Yes");
                acroFields.SetField("Gateway", dt[0].Gateway);
            }

            acroFields.SetField("Monthly Internet.ToString() Access Fee", dt[0].GatewayMonFee.ToString());
            acroFields.SetField("Internet.ToString() Gateway Per-Auth Fee", dt[0].GatewayTransFee.ToString());

            
            if ((dt[0].Interchange == "True") && (dt[0].DiscountRate.ToString() == "NULL"))
            {
                acroFields.SetField("Interchange Plus", dt[0].DiscRateQualDebit.ToString());
            }
            else
                acroFields.SetField("Interchange Plus", dt[0].DiscountRate.ToString());
             * */
            /*
                acroFields.SetField("Debit Card Qualified Rate", NULL);
                acroFields.SetField("DebitMid-QualifiedFee", NULL);
			    acroFields.SetField("DebitNon-QualifiedFee", NULL);
                acroFields.SetField("Credit Card Qualified Rate", NULL);
                acroFields.SetField("CreditMid-QualifiedFee", NULL);
			    acroFields.SetField("CreditNon-QualifiedFee", NULL);
            }*/

            /*
            if (dt[0].AmexNum.ToString().Trim() == "NULL")
            {
                acroFields.SetField("Check Box227", "Yes");
            }
            else
                acroFields.SetField("Check Box225", "Yes");

            #endregion

            #region Banking
            //Baking
            acroFields.SetField("Discover Card Existing Number", dt[0].PrevDiscoverNum);
            acroFields.SetField("Amex Existing Number", dt[0].PrevAmexNum);
            acroFields.SetField("JCB Existing Number", dt[0].PrevJCBNum);
            acroFields.SetField("Bank Name", dt[0].BankName);
            acroFields.SetField("Bank Address", dt[0].BankAddress);
            acroFields.SetField("City", dt[0].BankCity);
            acroFields.SetField("State", dt[0].BankState);
            acroFields.SetField("Zip Code", dt[0].BankZip);
            acroFields.SetField("Bank Telephone Number", dt[0].BankPhone);
            acroFields.SetField("Transit Routing #", dt[0].BankRoutingNumber);
            acroFields.SetField("Checking Account #", dt[0].BankAccountNumber);
            //acroFields.SetField("BankContactName", dt[0].NameOnCheckingAcct);
            #endregion

            #region Platform
            if (dt[0].Platform.ToString().Contains("Omaha") )
                acroFields.SetField("Check Box163", "Yes");
            else if (dt[0].Platform.ToString().Contains ("Nashville") )
                acroFields.SetField("Check Box165", "Yes");
            else if (dt[0].Platform.ToString().Contains ("Vital") )
                acroFields.SetField("Check Box166", "Yes");
            else if (dt[0].Platform.ToString().Contains ("North") )
                acroFields.SetField("Check Box164", "Yes");
            else if ((dt[0].Platform != "") && (dt[0].Platform.ToLower() != "none"))
            {
                acroFields.SetField("Check Box167", "Yes");
                acroFields.SetField("Other Platform", dt[0].Platform);
            }

            //if record has equipment and a gateway
           
            #endregion
            */
            #endregion
                        
            #region General Information
            acroFields.SetField("app.RepName", dt[0].RepName);
            acroFields.SetField("app.LegalName", dt[0].COMPANYNAME);
            acroFields.SetField("app.DBA", dt[0].DBA);
            acroFields.SetField("app.ApplicantDBA", dt[0].DBA);
            acroFields.SetField("app.EMail", dt[0].Email);
            acroFields.SetField("app.ContactName", dt[0].ContactName);
            acroFields.SetField("app.Website", dt[0].Website);
            acroFields.SetField("app.MailingAddress", dt[0].BillingAddress);
            acroFields.SetField("app.MCityState", dt[0].BillingCity + ", " + dt[0].BillingState + ", " + dt[0].BillingZipCode);
            acroFields.SetField("app.BusinessAddress", dt[0].Address);
            acroFields.SetField("app.CityState", dt[0].CITY + ", " + dt[0].STATE + ", " + dt[0].ZipCode);
            acroFields.SetField("app.Region", dt[0].Country);
            acroFields.SetField("app.HowLong", dt[0].TABL);
            acroFields.SetField("app.TIBYears", dt[0].YIB.ToString() );
            acroFields.SetField("app.TIBMonths", dt[0].MIB.ToString());
            acroFields.SetField("app.Fax", dt[0].Fax);
            acroFields.SetField("app.BusinessPhone", dt[0].BusinessPhone);
            acroFields.SetField("app.CustServPhone", dt[0].CustServPhone);
            acroFields.SetField("app.BusinessHours", dt[0].BusinessHours);
            
            acroFields.SetField("app.ProductsSold", dt[0].ProductSold);
            acroFields.SetField("app.PrevProcessor", dt[0].PrevProcessor);
            acroFields.SetField("app.PrevMerchantNum", dt[0].PrevMerchantAcctNo);
            acroFields.SetField("app.RepNum", dt[0].RepNum);
            acroFields.SetField("app.RepPhone", dt[0].RepPhone);
            acroFields.SetField("app.AddlComments", dt[0].AddlComments);
            acroFields.SetField("app.NumLocs", dt[0].NumberOfLocations);
            acroFields.SetField("app.BusinessPhoneExt", dt[0].BusinessPhoneExt);
            acroFields.SetField("app.NumDaysDel", dt[0].NumDaysDelivered);

            if ((dt[0].FederalTaxID.ToString().Trim() != "") && (dt[0].P1SSN.ToString().Trim() != ""))
            {
                if ((dt[0].FederalTaxID.ToString().Trim() == null) || (dt[0].FederalTaxID.ToString().Trim() == dt[0].P1SSN.ToString().Trim()))
                {
                    acroFields.SetField("app.SSNCheckbox", "Yes");
                    acroFields.SetField("app.SSNorTaxID", dt[0].P1SSN.ToString().Trim());
                }
                else
                {
                    acroFields.SetField("app.EINCheckbox", "Yes");
                    acroFields.SetField("app.SSNorTaxID", dt[0].FederalTaxID.ToString().Trim());
                }
            }
            if (dt[0].CTMF == "Yes")
            {
                acroFields.SetField("app.chkCTMFYes", "Yes");
                acroFields.SetField("app.chkCTMFNo", "Off");
            }
            else
            {
                acroFields.SetField("app.chkCTMFYes", "Off");
                acroFields.SetField("app.chkCTMFNo", "Yes");
            }

            if (dt[0].PrevProcessed == "Yes")
            {
                acroFields.SetField("app.chkPrevProcessedYes", "Yes");
                acroFields.SetField("app.chkPrevProcessedNo", "Off");
            }
            else
            {
                acroFields.SetField("app.chkPrevProcessedYes", "Off");
                acroFields.SetField("app.chkPrevProcessedNo", "Yes");
            }

            //if (dt[0].Reprogram == "Yes")
            //acroFields.SetField("app.chkReprogram", "Yes");

            if ((dt[0].RefundPolicy == "Refund within 30 days") || (dt[0].RefundPolicy == "Refund Within 30 Days"))
                acroFields.SetField("app.chkRefund30Days", "Yes");
            else if (dt[0].RefundPolicy == "Exchange Only")
                acroFields.SetField("app.chkExchangeOnly", "Yes");
            else if (dt[0].RefundPolicy == "No Refund")
            {
                acroFields.SetField("app.chkRefundOther", "Yes");
                acroFields.SetField("app.OtherRefund", "No Refund");       
            }
            else if (dt[0].RefundPolicy.Contains("Other"))
            {
                acroFields.SetField("app.chkRefundOther", "Yes");
                acroFields.SetField("app.OtherRefund", dt[0].OtherRefund);       
            }

         

            if (dt[0].LegalStatus == "Sole Proprietorship")
                acroFields.SetField("app.chkSole", "Yes");
            if (dt[0].LegalStatus == "Corporation")
                acroFields.SetField("app.chkCorp", "Yes");
            if (dt[0].LegalStatus == "Partnership")
                acroFields.SetField("app.chkPartnership", "Yes");
            if (dt[0].LegalStatus == "Non-Profit")
                acroFields.SetField("app.chkNonProfit", "Yes");
            if (dt[0].LegalStatus == "Legal/Medical Corp.")
                acroFields.SetField("app.chkLegaMedical", "Yes");
            if (dt[0].LegalStatus == "Government")
                acroFields.SetField("app.chkGovt", "Yes");
            if (dt[0].LegalStatus == "Tax Exempt")
                acroFields.SetField("app.chkTaxExempt", "Yes");
            if (dt[0].LegalStatus == "Others")
                acroFields.SetField("app.chkOwnershipOther", "Yes");
            if (dt[0].LegalStatus == "LLC")
                acroFields.SetField("app.chkLLC", "Yes");

            if (dt[0].Equipment != "")
            {
                string equipment = dt[0].Equipment;
                acroFields.SetField("app.EquipModel", equipment);
                if (equipment.Contains("Nurit"))
                    acroFields.SetField("app.chkNurit", "Yes");
                else if (equipment.Contains("Verifone"))
                    acroFields.SetField("app.chkVerifone", "Yes");
                else if (equipment.Contains("Hypercom"))
                    acroFields.SetField("app.chkHypercom", "Yes");
                else
                    acroFields.SetField("app.chkOther", "Yes");
            }

            #endregion

            #region CardPCT
            acroFields.SetField("app.Swiped", dt[0].ProcessPctSwiped.ToString().ToString());
            acroFields.SetField("app.Keyed", dt[0].ProcessPctKeyed.ToString().ToString());
            acroFields.SetField("app.MailOrder", dt[0].BusinessPctMailOrder.ToString().ToString());
            acroFields.SetField("app.Internet", dt[0].BusinessPctInternet.ToString().ToString());

            if (Convert.ToInt32(dt[0].BusinessPctInternet) >= 50)
            {
                acroFields.SetField("App.BusTypeInternet", "Yes");
            }
            else if (Convert.ToInt32(dt[0].BusinessPctService) >= 50)
            {
                acroFields.SetField("App.RetailTip", "Yes");
            }
            else if (Convert.ToInt32(dt[0].BusinessPctRetail) >= 50)
            {
                acroFields.SetField("App.Retail", "Yes");
            }
            else if (Convert.ToInt32(dt[0].BusinessPctMailOrder) >= 50)
            {
                acroFields.SetField("App.Moto", "Yes");
            }
            else if (Convert.ToInt32(dt[0].PctRest) >= 50)
            {
                acroFields.SetField("App.Restaurant", "Yes");
            }

            #endregion

            #region Principal #1
            //Principal #1
            acroFields.SetField("app.P1ZipCode", dt[0].P1ZipCode);
            acroFields.SetField("app.P1State", dt[0].P1State);
            acroFields.SetField("app.P1City", dt[0].P1City);
            acroFields.SetField("app.P1Address", dt[0].P1Address);
            acroFields.SetField("app.P1Title", dt[0].P1Title);
            acroFields.SetField("app.P1SSN", dt[0].P1SSN);
            acroFields.SetField("app.P1Name", dt[0].P1FirstName + " " + dt[0].P1LastName);
            acroFields.SetField("app.P1Ownership", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("app.P1DOB", dt[0].P1DOB);
            acroFields.SetField("app.P1DState", dt[0].P1DriversLicenseState);
            acroFields.SetField("app.P1DriversLicense", dt[0].P1DriversLicenseNo);
            acroFields.SetField("app.P1HomePhone", dt[0].P1PhoneNumber);
            acroFields.SetField("app.P1TimeAtAddress", dt[0].P1TimeAtAddress);
            if (dt[0].P1LivingStatus == "Rent")
                acroFields.SetField("app.chkP1Rent", "Yes");
            if (dt[0].P1LivingStatus == "Own")
                acroFields.SetField("app.chkP1Own", "Yes");
            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("app.P2ZipCode", dt[0].P2ZipCode);
            acroFields.SetField("app.P2State", dt[0].P2State);
            acroFields.SetField("app.P2City", dt[0].P2City);
            acroFields.SetField("app.P2Address", dt[0].p2Address);
            acroFields.SetField("app.P2Title", dt[0].P2Title);
            acroFields.SetField("app.P2SSN", dt[0].P2SSN);
            acroFields.SetField("app.P2Name", dt[0].P2FirstName + " " + dt[0].P2LastName);
            acroFields.SetField("app.P2Ownership", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("app.P2DOB", dt[0].P2DOB);
            acroFields.SetField("app.P2DState", dt[0].P2DriversLicenseState);
            acroFields.SetField("app.P2DriversLicense", dt[0].P2DriversLicenseNo);
            acroFields.SetField("app.P2HomePhone", dt[0].p2PhoneNumber);
            acroFields.SetField("app.P2TimeAtAddress", dt[0].P2TimeAtAddress);
            if (dt[0].P2LivingStatus == "Rent")
                acroFields.SetField("app.chkP2Rent", "Yes");
            if (dt[0].P2LivingStatus == "Own")
                acroFields.SetField("app.chkP2Own", "Yes");
            #endregion

            #region Rates
            //Rates
            acroFields.SetField("app.AvgTicket", dt[0].AverageTicket.ToString());
            acroFields.SetField("app.MonthlySalesProcessingLimit", dt[0].MonthlyVolume.ToString());

            if (dt[0].Interchange.ToString().Trim() != "True")
            {
                acroFields.SetField("app.QualifiedFee", dt[0].DiscountRate.ToString().Trim());
            }
            if (dt[0].Interchange.ToString().Trim() != "True")
            {
                acroFields.SetField("app.DebitQualifiedFee", dt[0].DiscRateQualDebit.ToString().Trim());
            }

            if ((dt[0].DiscountRate.ToString().Trim() != "") && (dt[0].DiscRateMidQual.ToString().Trim() != "") && (dt[0].Interchange.ToString().Trim() != "True"))
            {
                acroFields.SetField("app.MidQualifiedFee", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateMidQual) - Convert.ToDecimal(dt[0].DiscountRate)));
                acroFields.SetField("app.MidQualifiedFee1", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateMidQual) - Convert.ToDecimal(dt[0].DiscountRate)));
            }
            if ((dt[0].DiscountRate.ToString().Trim() != "") && (dt[0].DiscRateNonQual.ToString().Trim() != "") && (dt[0].Interchange.ToString().Trim() != "True"))
            {
                acroFields.SetField("app.NonQualifiedFee", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateNonQual) - Convert.ToDecimal(dt[0].DiscountRate)));
                acroFields.SetField("app.NonQualifiedFee1", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateNonQual) - Convert.ToDecimal(dt[0].DiscountRate)));
            }

            acroFields.SetField("app.TransactionFee", dt[0].TransactionFee.ToString());
            if (dt[0].AnnualFeeCP.ToString() != "")
                acroFields.SetField("app.AnnualFee", dt[0].AnnualFeeCP.ToString());
            else
                acroFields.SetField("app.AnnualFee", dt[0].AnnualFeeCNP.ToString());
            acroFields.SetField("app.CustServFee", dt[0].CustServFee.ToString());
            acroFields.SetField("app.MonthlyMinDiscFee", dt[0].MonMin.ToString());
            acroFields.SetField("app.RetrievalRequest", dt[0].RetrievalFee.ToString());
            acroFields.SetField("app.ChargeBacks", dt[0].ChargebackFee.ToString());
            acroFields.SetField("app.ApplicationFee", dt[0].AppFee.ToString());
            acroFields.SetField("app.SetupFee", dt[0].AppSetupFee.ToString());
            acroFields.SetField("app.AVS", dt[0].AVS.ToString());
            acroFields.SetField("app.BatchHeader", dt[0].BatchHeader.ToString());
            acroFields.SetField("app.VoiceAuth", dt[0].VoiceAuth.ToString().ToString());
            acroFields.SetField("app.BatchHeader", dt[0].BatchHeader.ToString());

            string strComment = "";

            strComment = "Discount billing type: " + Convert.ToString(dt[0].DiscountPaid).Trim() + " ; " + " Max ticket: " + Convert.ToString(dt[0].MaxTicket).Trim() + " ;";
            
            if ((dt[0].WirelessAccessFee.ToString().Trim() != "") && (dt[0].WirelessTransFee.ToString() != ""))
            {
                acroFields.SetField("app.WirelessMonthlyGatewayFee", dt[0].WirelessAccessFee.ToString());
                acroFields.SetField("app.WirelessPerAuthFee", dt[0].WirelessTransFee.ToString());
                acroFields.SetField("app.WirelessSetupfee", "35.00");
                acroFields.SetField("app.WirelessSetupQuantity", "1");
                acroFields.SetField("app.WirelessMonthlyAccessQuantity", "1");
            }

            acroFields.SetField("app.NBCTransactionFee", dt[0].NBCTransFee.ToString());
            acroFields.SetField("app.MCC", dt[0].MCC.ToString().Trim());

            if ((dt[0].DebitMonFee.ToString() != "") && (dt[0].DebitTransFee.ToString() != ""))
            {
                acroFields.SetField("app.DebitCardAccessFee", dt[0].DebitMonFee.ToString());
                acroFields.SetField("app.Debit", dt[0].DebitTransFee.ToString());
                acroFields.SetField("app.chkDebitCard", "Yes");
            }

            /*if (dt[0].EBTTransFee.ToString() != "")
            {
                acroFields.SetField("app.chkEBT", "Yes");
                acroFields.SetField("app.EBTTransFee", dt[0].EBTTransFee.ToString());
            }
            else {
                acroFields.SetField("app.chkEBT", "No");
            }*/
            //If Restaurant percentage is ZERO or BLANK
            if ((dt[0].PctRest.ToString() == "") || (Convert.ToInt16(dt[0].PctRest) == 0))
            {
                acroFields.SetField("app.QualifiedFee", dt[0].DiscountRate.ToString());
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateMidQual.ToString().ToString() != ""))
                    acroFields.SetField("app.MidQualifiedFee", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateMidQual) - Convert.ToDecimal(dt[0].DiscountRate.ToString() ) ) );
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateNonQual.ToString() != ""))
                    acroFields.SetField("app.NonQualifiedFee", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateNonQual) - Convert.ToDecimal(dt[0].DiscountRate.ToString())));

            }
            else //Restaurant Pct is greater than ZERO              
            {
                acroFields.SetField("app.QualFee", dt[0].DiscountRate.ToString());
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateMidQual.ToString() != ""))
                    acroFields.SetField("app.MidQual", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateMidQual) - Convert.ToDecimal(dt[0].DiscountRate.ToString())));
                if ((dt[0].DiscountRate.ToString() != "") && (dt[0].DiscRateNonQual.ToString() != ""))
                    acroFields.SetField("app.NonQual", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateNonQual) - Convert.ToDecimal(dt[0].DiscountRate.ToString())));
            }
            if (dt[0].DiscRateQualDebit.ToString() != "")
            {
                acroFields.SetField("app.QualFeeOD", dt[0].DiscRateQualDebit.ToString());
                if (dt[0].DiscRateMidQual.ToString() != "")
                {
                    acroFields.SetField("app.MidQualOD", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateMidQual) - Convert.ToDecimal(dt[0].DiscRateQualDebit)));
                    acroFields.SetField("app.NonQualOD", Convert.ToString(Convert.ToDecimal(dt[0].DiscRateNonQual) - Convert.ToDecimal(dt[0].DiscRateQualDebit)));
                }
            }
            
            if (dt[0].Gateway.ToString().Trim() != "")
            {
                acroFields.SetField("app.GatewayCheckbox", "Yes");
                acroFields.SetField("app.Gateway", dt[0].Gateway.ToString().Trim());
            }
            //acroFields.SetField("app.GatewaySetupFee", dt[0].GatewaySetupFee.ToString());
            acroFields.SetField("app.GatewayMonthlyAccess", dt[0].GatewayMonFee.ToString());
            acroFields.SetField("app.GatewayTransationFee", dt[0].GatewayTransFee.ToString());

            if (dt[0].InternetStmt.ToString().Trim() == "14.95")
                acroFields.SetField("app.iAccessSingle", "Yes");
            else if (dt[0].InternetStmt.ToString().Trim() == "30.00")
                acroFields.SetField("app.iAccessChain", "Yes");
            
            if (dt[0].Interchange.ToString().Trim() == "True")
            {
                acroFields.SetField("app.InterchangePlus", dt[0].DiscRateQualDebit.ToString());
            }

            if (!(dt[0].Interchange.ToString().Trim() == "True"))
            {
                strComment = strComment + " plus Dues & Assessments";
            }

            acroFields.SetField("Text220", strComment);

            #endregion

            #region Banking
            //Baking
            acroFields.SetField("app.DiscoverNum", dt[0].PrevDiscoverNum);
            if ((dt[0].AmexNum.ToString().Trim() != "") && (dt[0].AmexNum.ToString().Trim() != "Opted out") &&
                (dt[0].AmexNum.ToString().Trim() != "Cancelled") && (dt[0].AmexNum.ToString().Trim() != "Declined"))
            {
                if ((dt[0].AmexNum.ToString().Trim() == "Yes") || (dt[0].AmexNum.ToString().Trim() == "Submitted"))
                {
                    acroFields.SetField("CheckBox230", "Yes"); //Check AmEx OnePoint
                    acroFields.SetField("app.DiscRateCheckBox", "Yes");
                    if (Convert.ToDouble(dt[0].ProcessPctSwiped.ToString().Trim()) >= 60)
                    {
                        acroFields.SetField("app.AmexDiscountRate", "2.89");
                    }
                    else {
                        acroFields.SetField("app.AmexDiscountRate", "3.50");
                    }
                }
                else // Existing Amex number
                {
                    acroFields.SetField("app.AmexNum", dt[0].PrevAmexNum);
                    acroFields.SetField("Check Box229", "Yes"); //Check AmEx Direct
                    acroFields.SetField("app.MonthlyFFCheckBox", "Yes");
                }

                /*if (Convert.ToDouble(dt[0].ProcessPctSwiped.ToString().Trim()) >= 70)
                    acroFields.SetField("app.DiscRateCheckBox", "Yes");
                else
                    acroFields.SetField("app.MonthlyFFCheckBox", "Yes");*/
            }

            acroFields.SetField("app.JCBNum", dt[0].PrevJCBNum); 
            acroFields.SetField("app.BankName", dt[0].BankName);
            acroFields.SetField("app.BankAddress", dt[0].BankAddress);
            acroFields.SetField("app.BankCity", dt[0].BankCity);
            acroFields.SetField("app.BankState", dt[0].BankState);
            acroFields.SetField("app.BankZip", dt[0].BankZip);
            acroFields.SetField("app.BankPhone", dt[0].BankPhone);
            acroFields.SetField("app.RoutingNum", dt[0].BankRoutingNumber);
            acroFields.SetField("app.AcctNum", dt[0].BankAccountNumber);
            //acroFields.SetField("app.BankContactName", dt[0].NameOnCheckingAcct);
            #endregion

            #region Platform
            if (dt[0].Platform.ToString().Contains("Omaha"))
                acroFields.SetField("app.chkOmaha", "Yes");
            else if (dt[0].Platform.ToString().Contains("Nashville"))
                acroFields.SetField("app.chkNashville", "Yes");
            else if (dt[0].Platform.ToString().Contains("Buypass"))
                acroFields.SetField("app.chkBuypass", "Yes");
            else if (dt[0].Platform.ToString().Contains("North"))
                acroFields.SetField("app.chkNorth", "Yes");
            else if ((dt[0].Platform != "") && (dt[0].Platform.ToLower() != "none"))
            {
                acroFields.SetField("app.chkFrontEndOther", "Yes");
                acroFields.SetField("app.OtherPlatform", dt[0].Platform);
            }
            #endregion
            
            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("IPayment Data not found for this record.");
            return false;
        }
    }//end function CreateIPayPDF
    #endregion

    #region MERRICK PDF
    //This function creates Optimal-Merrick PDF
    public bool CreateMerrickPDF(string ContactID)
    {
        //Get data for Merrick Application
        PDFBL MerrickData = new PDFBL();
        PartnerDS.ACTMerrickPDFDataTable dt = MerrickData.GetMerrickDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {

            //Populate data in PDF
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/CNP_Merrick.pdf"));
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting | PdfWriter.AllowFillIn | PdfWriter.AllowModifyContents | PdfWriter.AllowAssembly);
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Merrick App_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "Merrick App_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            #region General Information

            acroFields.SetField("MerchantName", dt[0].DBA);

            //if Company Name different than DBA
            if (dt[0].COMPANYNAME != dt[0].DBA)
                acroFields.SetField("CorporateName", dt[0].COMPANYNAME);

            acroFields.SetField("MerchantAddress", dt[0].Address1 + dt[0].Address2);
            acroFields.SetField("MerchantCity", dt[0].CITY);
            acroFields.SetField("MerchantState", dt[0].STATE);
            acroFields.SetField("MerchantCountry", dt[0].billingCountry);
            acroFields.SetField("MerchantZip", dt[0].ZipCode);

            acroFields.SetField("CorporateAddress", dt[0].BillingAddress1 + dt[0].BillingAddress2);
            acroFields.SetField("CorporateCity", dt[0].BillingCity);
            acroFields.SetField("CorporateState", dt[0].BillingState);
            acroFields.SetField("CorporateCountry", dt[0].billingCountry);
            acroFields.SetField("CorporateZip", dt[0].BillingZipCode);

            acroFields.SetField("ContactEmail", dt[0].Email);
            acroFields.SetField("ContactName", dt[0].ContactName);
            acroFields.SetField("WebSite", dt[0].Website);
            acroFields.SetField("ContactPhone", dt[0].BusinessPhone);
            acroFields.SetField("ContactFax", dt[0].Fax);
            acroFields.SetField("ContactCS", dt[0].CustServPhone);
            acroFields.SetField("BusinessPhone", dt[0].CustServPhone);
            acroFields.SetField("TaxID", dt[0].FederalTaxID);
            acroFields.SetField("BusinessName", dt[0].DBA);

            acroFields.SetField("AverageTicket", dt[0].AverageTicket.ToString());
            acroFields.SetField("HighestTicket", dt[0].MaxTicket.ToString());
            acroFields.SetField("MonthlyVolume", dt[0].MonthlyVolume.ToString());

            acroFields.SetField("ProdDesc1", dt[0].ProductSold);
            acroFields.SetField("YearsBusiness", dt[0].YIB.ToString());

            if (dt[0].NewAmex.ToString().Trim() == "Yes")
                acroFields.SetField("ApplyAmex", "Yes");

            if (dt[0].LegalStatus == "Corporation")
                acroFields.SetField("Ownership", "2");
            else if (dt[0].LegalStatus == "Sole Proprietorship")
                acroFields.SetField("Ownership", "2");
            else if (dt[0].LegalStatus == "Partnership")
                acroFields.SetField("Ownership", "3");
            else if (dt[0].LegalStatus == "Government")
                acroFields.SetField("Ownership", "5");
            else if (dt[0].LegalStatus == "Non-Profit")
                acroFields.SetField("Ownership", "6");
            else if (dt[0].LegalStatus == "LLC")
                acroFields.SetField("Ownership", "7");
            else
                acroFields.SetField("StateOwnership", dt[0].LegalStatus);


            if (dt[0].PrevProcessed.Contains("Yes"))
                acroFields.SetField("PaymentCards", "2");
            else if (dt[0].PrevProcessed.Contains("No"))
                acroFields.SetField("PaymentCards", "1");
            acroFields.SetField("ReasonLeaving", dt[0].ReasonForLeaving);

            #endregion

            #region Principal #1
            //Principal #1
            acroFields.SetField("PrincipalFirst1", dt[0].P1FirstName);
            acroFields.SetField("PrincipalLast1", dt[0].P1LastName);
            acroFields.SetField("PrincipalMiddle1", dt[0].P1MName);
            acroFields.SetField("Principal%1", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("PrincipalSSN1", dt[0].P1SSN.ToString());
            acroFields.SetField("PrincipalDriver1", dt[0].P1DriversLicenseNo);
            acroFields.SetField("PrincipalTitle1", dt[0].P1Title);
            acroFields.SetField("PrincipalAddress1", dt[0].P1Address);
            acroFields.SetField("PrincipalCity1", dt[0].P1City);
            acroFields.SetField("PrincipalState1", dt[0].P1State);
            acroFields.SetField("PrincipalCountry1", dt[0].P1Country);
            acroFields.SetField("PrincipalZip1", dt[0].P1ZipCode);
            acroFields.SetField("PrincipalDOB1", dt[0].P1DOB);
            acroFields.SetField("PrincipalPhone1", dt[0].P1PhoneNumber.ToString());
            acroFields.SetField("PrincipalCell1", dt[0].P1MobilePhone.ToString());
            //acroFields.SetField("PrinicpalEmail1", dt[0].P1PhoneNumber);

            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("PrincipalFirst2", dt[0].P2FirstName);
            acroFields.SetField("PrincipalLast2", dt[0].P2LastName);
            //acroFields.SetField("PrincipalMiddle2", dt[0].P2MName);
            acroFields.SetField("Principal%2", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("PrinicpalSSN2", dt[0].P2SSN);
            acroFields.SetField("PrincipalDriver2", dt[0].P2DriversLicenseNo);
            acroFields.SetField("PrincipalTitle2", dt[0].P2Title);
            acroFields.SetField("PrincipalAddress2", dt[0].p2Address);
            acroFields.SetField("PrincipalCity2", dt[0].P2City);
            acroFields.SetField("PrincipalState2", dt[0].P2State);
            acroFields.SetField("PrincipalCountry2", dt[0].P2Country);
            acroFields.SetField("PrincipalZip2", dt[0].P2ZipCode);
            acroFields.SetField("PrincipalDOB2", dt[0].P2DOB);
            acroFields.SetField("PrinicpalPhone2", dt[0].p2PhoneNumber);
            //acroFields.SetField("PrincipalCell2", dt[0].P2MobilePhone);
            //acroFields.SetField("PrinicpalEmail2", dt[0].P2PhoneNumber);

            #endregion

            if (dt[0].CTMF.Contains("Yes"))
                acroFields.SetField("Terminated", "2");
            else if (dt[0].CTMF.Contains("No"))
                acroFields.SetField("Terminated", "1");

            #region Banking
            //Banking
            if (dt[0].BankRoutingNumber != "")
            {
                acroFields.SetField("AccountType", "1");
                acroFields.SetField("RoutNumber", dt[0].BankRoutingNumber);
                acroFields.SetField("AcctNumber", dt[0].BankAccountNumber);
            }

            //acroFields.SetField("BankContactName", dt[0].NameOnCheckingAcct);
            #endregion

            #region CardPCT
            int MailPhone = Convert.ToInt16(dt[0].BusinessPctMail) + Convert.ToInt16(dt[0].BusinessPctPhone);
            acroFields.SetField("Swipe%", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("Moto%", MailPhone.ToString());
            acroFields.SetField("Internet%", dt[0].BusinessPctInternet.ToString());
            if (!dt[0].Gateway.ToString().Contains("Optimal"))
                acroFields.SetField("GatewayName", dt[0].Gateway);

            #endregion

            acroFields.SetField("AmexApply", dt[0].NewAmex);
            int result = 0;
            if (Int32.TryParse(dt[0].PrevAmexNum, out result))
                acroFields.SetField("AcceptAmex", dt[0].PrevAmexNum);

            if(dt[0].Equipment.ToString().Trim() != "")
                acroFields.SetField("EquipType1", dt[0].Equipment.ToString());
            else
                acroFields.SetField("EquipType1", dt[0].Gateway.ToString());
            acroFields.SetField("Model1", dt[0].EquipmentModel);

            #region PrincipalSignatures
            acroFields.SetField("MerchantTitle1", dt[0].P1Title);
            acroFields.SetField("MerchantPrincipal1", dt[0].P1FullName);

            acroFields.SetField("MerchantTitle2", dt[0].P2Title);
            acroFields.SetField("MerchantPrincipal2", dt[0].P2FullName);

            acroFields.SetField("CorporateLegalName", dt[0].COMPANYNAME);
            acroFields.SetField("CorporateStatus", dt[0].LegalStatus.ToString().Trim());

            //acroFields.SetField("BankPrincipal1", dt[0].P1FullName);

            acroFields.SetField("GuarantorPrincipal1", dt[0].P1FullName);
            acroFields.SetField("GuarantorPrincipal2", dt[0].P2FullName);

            acroFields.SetField("CorporateSigner1", dt[0].P1FullName);
            acroFields.SetField("CorporateSignerTitle1", dt[0].P1Title);
            acroFields.SetField("CorporateSigner2", dt[0].P2FullName);
            acroFields.SetField("CorporateSignerTitle2", dt[0].P2Title);

            #endregion

            #region Platform
            if (dt[0].Platform.ToString().Contains("Vital"))
                acroFields.SetField("Processor", "1");
            else if (dt[0].Platform.ToString().Contains("CardSystems"))
                acroFields.SetField("Processor", "2");
            else if (dt[0].Platform.ToString().Contains("Global"))
                acroFields.SetField("Processor", "3");
            else if (dt[0].Platform.ToString().Contains("Paymentech"))
                acroFields.SetField("Processor", "4");
            else
            {
                acroFields.SetField("Processor", "5");
                acroFields.SetField("ProcessorNetwork", dt[0].Platform.ToString());
            }
            #endregion

            #region Rates
            //Rates

            //We never populate into these fields, so sending nulls

            acroFields.SetField("MotoTxnFeeMid", "");
            acroFields.SetField("MotoTxnFeeNon", "");

            acroFields.SetField("STAFM", dt[0].CustServFee.ToString());

            //if Internet.ToString() Account
            if (Convert.ToInt32(dt[0].ProcessPctSwiped.ToString()) <= 50)
            {
                if (dt[0].DiscQP.ToString() == "")
                {
                    acroFields.SetField("MotoQual", dt[0].DiscQNP.ToString());
                }
                else
                    acroFields.SetField("MotoQual", dt[0].DiscQP.ToString());

                if ((dt[0].Interchange.ToString() == "False") || (dt[0].Assessments.ToString() == "False"))
                {
                    acroFields.SetField("Footnote2", "Yes");
                    acroFields.SetField("Footnote4", "Yes");
                }

                acroFields.SetField("MotoMidQual", dt[0].DiscMQStep.ToString());
                acroFields.SetField("MotoNonQual", dt[0].DiscNQStep.ToString());
                acroFields.SetField("MotoBundled", "");

                acroFields.SetField("MotoSetupFeeApp", dt[0].AppFee.ToString());
                acroFields.SetField("MotoSetupFeeRB", "");
                acroFields.SetField("MotoSetupFeeAmex", "");

                acroFields.SetField("MotoMonthlyFeeMaint", dt[0].CustServFee.ToString());
                acroFields.SetField("MotoMonthlyFeeReport", dt[0].InternetStmt.ToString());
                acroFields.SetField("MotoMonthlyFeeMin", dt[0].MonMin.ToString());
                acroFields.SetField("MotoMonthlyFeeSecure", dt[0].GatewayMonFee.ToString());
                acroFields.SetField("MotoMonthlyFeeRB", "");

                acroFields.SetField("MotoTxnFee", dt[0].TransactionFee.ToString());
                acroFields.SetField("MotoTxnFeeMid", "0.00"); //populating 0.00
                acroFields.SetField("MotoTxnFeeNon", "0.00"); //populating 0.00
                acroFields.SetField("MotoTxnFeeAmex", dt[0].TransactionFee.ToString());
                acroFields.SetField("MotoTxnFeeTDS", "0.12"); //always default to 12 cents

                acroFields.SetField("MotoOtherFeeRR", dt[0].RetrievalFee.ToString());
                
                acroFields.SetField("MotoOtherFeeCB", dt[0].ChargebackFee.ToString());
                acroFields.SetField("MotoOtherFeeACH", dt[0].BatchHeader.ToString());
                acroFields.SetField("MotoOtherFeeFailed", "");
                acroFields.SetField("MotoOtherFeeAVS", dt[0].AVS.ToString());
                acroFields.SetField("MotoOtherFeeGateway", dt[0].GatewayTransFee.ToString());

                acroFields.SetField("MotoOtherFeeAnnual", dt[0].AnnualFee.ToString());
                acroFields.SetField("MotoOtherFeeOther", "");

                acroFields.SetField("ReserveAccount%CNP", dt[0].RollingReserve.ToString());
                acroFields.SetField("ReserveAccount%CP", "");

                //Set all Retail fields to blank
                acroFields.SetField("RetailMonthlyFeeMaint", "");
                acroFields.SetField("RetailMonthlyFeeStatement", "");
                acroFields.SetField("RetailDiscountMonthly", "");
                acroFields.SetField("RetailOtherFeeCB", "");
                acroFields.SetField("RetailOtherFeeBatch", "");
                acroFields.SetField("RetailOtherFeeAnnual", "");
            }
            else
            {
                acroFields.SetField("RetailQual", dt[0].DiscQP.ToString());
                acroFields.SetField("RetailMidQual", dt[0].DiscMQStep.ToString());
                acroFields.SetField("RetailNonQual", dt[0].DiscNQStep.ToString());
                //acroFields.SetField("RetailOffline", dt[0].DiscQDStep);
                acroFields.SetField("RetailBundled", "");

                acroFields.SetField("RetailSetupFeeApp", dt[0].AppFee.ToString());
                acroFields.SetField("RetailSetupFeeMobile", "");
                acroFields.SetField("RetailSetupFeeAmex", "");

                acroFields.SetField("RetailMonthlyFeeMaint", dt[0].CustServFee.ToString());
                acroFields.SetField("RetailMonthlyFeeReport", dt[0].InternetStmt.ToString());
                acroFields.SetField("RetailMonthlyFeeMin", dt[0].MonMin.ToString());
                acroFields.SetField("RetailMonthlyFeeSecure", dt[0].GatewayMonFee.ToString());
                acroFields.SetField("RetailMonthlyFeeMobile", "");
                acroFields.SetField("RetailMonthlyFeeClub", "");
                acroFields.SetField("RetailMonthlyFeeMonthly", "");
                acroFields.SetField("RetailMonthlyDiscountMonthly", "");

                acroFields.SetField("RetailOtherFeeRR", dt[0].RetrievalFee.ToString());

                acroFields.SetField("RetailTxnFee", dt[0].TransactionFee.ToString());
                acroFields.SetField("RetailTxnFeeMid", dt[0].TransactionFee.ToString());
                acroFields.SetField("RetailTxnFeeNon", dt[0].TransactionFee.ToString());
                acroFields.SetField("RetailTxnFeeAmex", dt[0].TransactionFee.ToString());
                acroFields.SetField("RetailTxnFeeDebit", dt[0].DebitTransFee.ToString());
                acroFields.SetField("RetailTxnFeeEBT", dt[0].EBTTransFee.ToString());

                acroFields.SetField("RetailOtherFeeCB", dt[0].ChargebackFee.ToString());
                acroFields.SetField("RetailOtherFeeAuth", "");
                acroFields.SetField("RetailOtherFeeVoice", dt[0].VoiceAuth.ToString());
                acroFields.SetField("RetailOtherFeeAVS", dt[0].AVS.ToString());
                acroFields.SetField("RetailOtherFeeMobile", "");
                acroFields.SetField("RetailOtherFeeBatch", dt[0].BatchHeader.ToString());
                acroFields.SetField("RetailOtherFeeAnnual", dt[0].AnnualFee.ToString());
                acroFields.SetField("RetailOtherFeeWarranty", "");

                acroFields.SetField("RetailOtherFeeOther", "");
                acroFields.SetField("ReserveAccount%CP", dt[0].RollingReserve.ToString());
                acroFields.SetField("ReserveAccount%CNP", "");

                //Set all MOTO fields to blank
                acroFields.SetField("MotoMidQual", "");
                acroFields.SetField("MotoNonQual", "");
                acroFields.SetField("MotoMonthlyFeeMaint", "");
                acroFields.SetField("MotoTxnFeeMid", "");
                acroFields.SetField("MotoTxnFeeNon", "");
                acroFields.SetField("MotoOtherFeeCB", "");
                acroFields.SetField("MotoOtherFeeACH", "");
                acroFields.SetField("MotoOtherFeeFailACH", "");
                acroFields.SetField("MotoOtherFeeAnnual", "");
            }
            #endregion


            //**********************INTERNET OR MOTO ACCOUNT Questionaire************************
            if (Convert.ToInt32(dt[0].ProcessPctSwiped.ToString()) <= 50)
            {
                //Only populate if its a CNP account
                if (dt[0].RefundPolicy.Contains("Refund within 30"))
                    acroFields.SetField("ReturnPolicy", "2");
                else if (dt[0].RefundPolicy == "No Refund")
                    acroFields.SetField("ReturnPolicy", "3");
                else if (dt[0].RefundPolicy == "Exchange Only")
                {
                    acroFields.SetField("ReturnPolicy", "4");
                    acroFields.SetField("SpecifyPolicy", "Exchange Only");
                }
                else if (dt[0].RefundPolicy == "Other")
                {
                    acroFields.SetField("ReturnPolicy", "4");
                    acroFields.SetField("SpecifyPolicy", dt[0].OtherRefund);
                }

                acroFields.SetField("ProdDescription1", dt[0].ProductSold);
                acroFields.SetField("Turnaround", dt[0].NumDaysDel.ToString());


            }//**********************END IF INTERNET OR MOTO ACCOUNT************************

            //Platform Check boxes            
            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if count > 0
        else
        {
            DisplayMessage("Optimal Merrick Data not found for this record.");
            return false;
        }
    }//end function CreateMerrickPDF

    #endregion

    #region CHASE PDF

    protected void btnChaseMPA_Click(object sender, EventArgs e)
    {
        try
        {
            CreateChaseMPAPDF(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnChaseFS3Tier_Click(object sender, EventArgs e)
    {
        try
        {
            CreateChaseFS3TierPDF(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnChaseFSInterchangePlus_Click(object sender, EventArgs e)
    {
        try
        {
            CreateChaseFSInterchangePlusPDF(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    public bool CreateChaseMPAPDF(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/ChaseMPA.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase MPA_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";
                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase MPA_" + P1FirstName + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("IU phone", "(800) 477-5363");
            acroFields.SetField("sales rep", dt[0].RepName);
            acroFields.SetField("sales ID", dt[0].RepNum);

            #region Merchant Information
            acroFields.SetField("business legal name", dt[0].COMPANYNAME);
            acroFields.SetField("mailing address", dt[0].BillingAddress);
            acroFields.SetField("city", dt[0].BillingCity);
            acroFields.SetField("State", dt[0].BillingState);
            acroFields.SetField("MI zip +4 = limit 10 char only", dt[0].BillingZipCode);
            acroFields.SetField("m phone", dt[0].BusinessPhone);
            acroFields.SetField("dba fax#", dt[0].Fax);
            acroFields.SetField("tax ID #", dt[0].FederalTaxID);
            acroFields.SetField("total # locations", dt[0].NumOfLocs);

            acroFields.SetField("merchant  doing business as", dt[0].DBA);
            acroFields.SetField("business start date", dt[0].StartYear.ToString());
            acroFields.SetField("how long at location", dt[0].YIB);
            acroFields.SetField("location address", dt[0].Address1 + " " + dt[0].Address2);
            acroFields.SetField("location city", dt[0].CITY);
            acroFields.SetField("location state", dt[0].STATE);
            acroFields.SetField("location zip", dt[0].ZipCode);
            acroFields.SetField("location phone", dt[0].CustServPhone);
            acroFields.SetField("primary merchant contact", dt[0].P1FullName);
            acroFields.SetField("m location email address", dt[0].Email);

            if (dt[0].LegalStatus.ToString().Trim() == "Sole Proprietorship")
                acroFields.SetField("type of ownership", "sole ownership");
            else if (dt[0].LegalStatus.ToString().Trim() == "Partnership")
                acroFields.SetField("type of ownership", "partnership");
            else if (dt[0].LegalStatus.ToString().Trim() == "LLC")
                acroFields.SetField("type of ownership", "llc");
            else if (dt[0].LegalStatus.ToString().Trim() == "Corporation")
                acroFields.SetField("type of ownership", "public corp");
            else if (dt[0].LegalStatus.ToString().Trim() == "Government")
                acroFields.SetField("type of ownership", "govn corp");
            else if (dt[0].LegalStatus.ToString().Trim() == "Non-Profit")
                acroFields.SetField("type of ownership", "non profit");
            else
                acroFields.SetField("type of ownership", "other");

            acroFields.SetField("m email address", dt[0].Website);
            acroFields.SetField("meerchandise sold", dt[0].ProductSold);

            if (dt[0].AmexAccept.ToString().Trim() == "Yes - Existing")
                acroFields.SetField("AMX #", dt[0].AmexAccept.ToString().Trim());
            if (dt[0].DiscoverAccept.ToString().Trim() == "Yes - Existing")
                acroFields.SetField("discover #", dt[0].DiscoverAccept.ToString().Trim());

            acroFields.SetField("software coding info", dt[0].Gateway.ToString().Trim());

            #endregion

            #region Sales Deposit & Refund Policy
            int iPctMOTO = Convert.ToInt32(dt[0].BusinessPctMailOrder) + Convert.ToInt32(dt[0].BusinessPctPhoneOrder);
            acroFields.SetField("% mail/phone", iPctMOTO.ToString().Trim());
            acroFields.SetField("% internet", dt[0].BusinessPctInternet);
            acroFields.SetField("% card swipe", dt[0].ProcessPctSwiped);
            acroFields.SetField("% hand keyed", dt[0].ProcessPctKeyed);

            int iNumDaysProdDel = Convert.ToInt32(dt[0].NumOfDaysProdDel);
            if (iNumDaysProdDel == 0)
                acroFields.SetField("% mail/phone  days 0", "100");
            else if ((iNumDaysProdDel >= 1) || (iNumDaysProdDel <= 7))
                acroFields.SetField("% mail/phone  days 1", "100");
            else if ((iNumDaysProdDel >= 8) || (iNumDaysProdDel <= 14))
                acroFields.SetField("% mail/phone  days 8", "100");
            else if ((iNumDaysProdDel >= 15) || (iNumDaysProdDel <= 30))
                acroFields.SetField("% mail/phone  days 15", "100");
            else if (iNumDaysProdDel > 30)
                acroFields.SetField("% mail/phone  days 30", "100");

            //Visa Master Refund Policy
            acroFields.SetField("refund", dt[0].VisaMasterRefund);

            //Refund Policy
            if (dt[0].RefundPolicy.ToString().Trim() == "Exchange Only")
                acroFields.SetField("SD refund policy", "exchange");
            else if (dt[0].RefundPolicy.ToString().Trim() == "Refund within 30 days")
                acroFields.SetField("SD refund policy", "MC/visacredit");
            else
                acroFields.SetField("SD refund policy", "other");
            #endregion

            #region Owners/Officers
            //Principal #1
            acroFields.SetField("o name", dt[0].P1FullName);
            acroFields.SetField("o title", dt[0].P1Title);
            acroFields.SetField("o %ownership", dt[0].P1OwnershipPercent);
            acroFields.SetField("o res address", dt[0].P1Address);
            acroFields.SetField("o city", dt[0].P1City);
            acroFields.SetField("o state", dt[0].P1State);
            acroFields.SetField("o zip", dt[0].P1ZipCode);
            acroFields.SetField("o home phone", dt[0].P1PhoneNumber);
            acroFields.SetField("o ss#", dt[0].P1SSN);
            acroFields.SetField("o date of birth", dt[0].P1DOB);
            acroFields.SetField("o drivers lic", dt[0].p1DLNum);
            acroFields.SetField("o state of drivers lic", dt[0].p1DLState);

            //Principal #2
            acroFields.SetField("o 2 name", dt[0].p2FullName);
            acroFields.SetField("o 2 title", dt[0].P2Title);
            acroFields.SetField("o 2 %ownership", dt[0].P2OwnershipPercent);
            acroFields.SetField("o 2 res address", dt[0].p2Address);
            acroFields.SetField("o 2 city", dt[0].P2City);
            acroFields.SetField("o 2 state", dt[0].P2State);
            acroFields.SetField("o 2 zip", dt[0].P2ZipCode);
            acroFields.SetField("o 2 home phone", dt[0].p2PhoneNumber);
            acroFields.SetField("o 2 ss#", dt[0].P2SSN);
            acroFields.SetField("o 2 date of birth", dt[0].P2DOB);
            acroFields.SetField("o 2 drivers lic", dt[0].P2DLNum);
            acroFields.SetField("o 2 state of drivers lic", dt[0].P2DLState);

            #endregion

            #region Credit Information
            acroFields.SetField("c annual visa/mastercard vol", dt[0].AnnualVol);
            acroFields.SetField("c aveage credit card ticket", dt[0].AvgTicket);

            #endregion

            #region Page 2
            acroFields.SetField("del time frame", dt[0].NumOfDaysProdDel);

            acroFields.SetField("bank name", dt[0].BankName);
            acroFields.SetField("transit routing", dt[0].BankRoutingNumber);
            acroFields.SetField("account #", dt[0].BankAccountNumber);
            acroFields.SetField("BR address", dt[0].BankAddress);
            acroFields.SetField("BR city", dt[0].BankCity);
            acroFields.SetField("BR state", dt[0].BankState);
            acroFields.SetField("BR zip", dt[0].BankZip);

            acroFields.SetField("processing bank", dt[0].PrevProcessor);
            acroFields.SetField("leaving", dt[0].ReasonForLeaving);

            if (dt[0].Bankruptcy.ToString().Trim().ToLower() == "yes")
                acroFields.SetField("bankruptcy", "Yes");
            else
            {
                acroFields.SetField("bankruptcy", "no");
                acroFields.SetField("bankruptcy 2nd", "no");
            }

            if (dt[0].PrevProcessed.ToString().Trim().ToLower() == "yes")
                acroFields.SetField("another business", "Yes");
            else
            {
                acroFields.SetField("another business", "no");
                acroFields.SetField("another business 2nd", "no");
            }

            acroFields.SetField("business legal name pg2", dt[0].COMPANYNAME);

            acroFields.SetField("M B title", dt[0].P1Title);
            acroFields.SetField("M B print name", dt[0].P1FullName);

            acroFields.SetField("M B title 2", dt[0].P2Title);
            acroFields.SetField("M B print name 2", dt[0].p2FullName);

            #endregion

            stamper.FormFlattening = true;
            stamper.Close();
            
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Chase MPA PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChasePDFMPA
    
    public bool CreateChaseFS3TierPDF(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase Fee schedule 3 tier.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase FS 3 Tier_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1LastName == "")
                    P1LastName = "Merchant";
                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase FS 3 Tier_" + P1FirstName + " " + P1LastName + " " + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Discount frequency", "Monthly");
            acroFields.SetField("Pricing Method", "Gross");
            acroFields.SetField("debit network Interchange", "Yes");

            #region Discount Fees
            acroFields.SetField("disc rate 1", dt[0].DiscountRate);
            acroFields.SetField("disc rate 2", dt[0].DiscountRateDebit);
            acroFields.SetField("disc rate 3", dt[0].DiscountRate);
            acroFields.SetField("disc rate 4", dt[0].DiscountRateDebit);
            acroFields.SetField("disc rate 5", dt[0].DiscountRate);
            acroFields.SetField("disc rate 6", dt[0].DiscountRateDebit);

            string DiscMQDebit = Convert.ToString(Convert.ToDouble(dt[0].DiscountRateDebit) + (Convert.ToDouble(dt[0].DiscMQ) - Convert.ToDouble(dt[0].DiscountRate)));
            acroFields.SetField("disc rate m1", dt[0].DiscMQ);
            acroFields.SetField("disc rate m2", DiscMQDebit);
            acroFields.SetField("disc rate m3", dt[0].DiscMQ);
            acroFields.SetField("disc rate m4", DiscMQDebit);
            acroFields.SetField("disc rate m5", dt[0].DiscMQ);
            acroFields.SetField("disc rate m6", DiscMQDebit);

            string DiscNQDebit = Convert.ToString(Convert.ToDouble(dt[0].DiscountRateDebit) + (Convert.ToDouble(dt[0].DiscNQ) - Convert.ToDouble(dt[0].DiscountRate)));
            acroFields.SetField("disc rate n1", dt[0].DiscNQ);
            acroFields.SetField("disc rate n2", DiscNQDebit);
            acroFields.SetField("disc rate n3", dt[0].DiscNQ);
            acroFields.SetField("disc rate n4", DiscNQDebit);
            acroFields.SetField("disc rate n5", dt[0].DiscNQ);
            acroFields.SetField("disc rate n6", DiscNQDebit);

            acroFields.SetField("trans fee 12", dt[0].DebitTransFee);
            acroFields.SetField("trans fee 13", dt[0].EBTTransFee);

            acroFields.SetField("auth fee 1", dt[0].TransactionFee);
            acroFields.SetField("auth fee 2", dt[0].TransactionFee);
            acroFields.SetField("auth fee 5", dt[0].NBCTransFee);
            acroFields.SetField("auth fee 7", dt[0].NBCTransFee);
            #endregion

            #region Other Service Fees
            //acroFields.SetField("OTHER 1 account fee", dr["AppSetupFee"].ToString());
            acroFields.SetField("OTHER 2 ACH fee", "10.00");
            acroFields.SetField("OTHER 3 annual mem fee", dt[0].AnnualFee);
            acroFields.SetField("OTHER  4 application fee", dt[0].BatchHeader);
            acroFields.SetField("OTHER 5 batch fee", dt[0].ChargebackFee);
            acroFields.SetField("OTHER 9 min monthly fee", "5.00");
            acroFields.SetField("OTHER 10 monthly maint fee", dt[0].MonMin);

            acroFields.SetField("O Resource 19fee", dt[0].InternetStmt);
            acroFields.SetField("OTHER 20retrieval fee", dt[0].RetrievalFee);
            acroFields.SetField("OTHER 22 stored value fee", dt[0].CustServFee);
            acroFields.SetField("OTHER 23voyager fee", "0.02");
            acroFields.SetField("OTHER 24 wireless monthly fee", "0.02");
            acroFields.SetField("OTHER 25 wireless set up fee.0", "0.10");
            acroFields.SetField("OTHER 25 wireless set up fee.1", "0.40");
            acroFields.SetField("OTHER 25 wireless set up fee.2xxx", dt[0].WirelessAccess);

            acroFields.SetField("O 2", dt[0].TransactionFee);
            acroFields.SetField("O 3", dt[0].VoiceAuth);
            acroFields.SetField("O 4", dt[0].AVS);
            acroFields.SetField("O 5", "2.00");
            acroFields.SetField("O 6", dt[0].WirelessTransFee);
            #endregion

            acroFields.SetField("merchant DBA name", dt[0].DBA);
            
            stamper.FormFlattening = true;
            stamper.Close();

            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Chase Fee Schedule PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChaseFS3TierPDF

    public bool CreateChaseFSInterchangePlusPDF(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase Fee schedule interchange plus.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase FS Interchange Plus_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1LastName == "")
                    P1LastName = "Merchant";
                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase FS Interchange Plus_" + P1FirstName + " " + P1LastName + " " + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Discount frequency", "Monthly");
            acroFields.SetField("Pricing Method", "Gross");
            acroFields.SetField("debit network Interchange", "Yes");

            #region Discount Fees
            acroFields.SetField("disc rate 1", dt[0].DiscountRate);
            acroFields.SetField("disc rate 2", dt[0].DiscountRateDebit);
            acroFields.SetField("disc rate 3", dt[0].DiscountRate);
            acroFields.SetField("disc rate 4", dt[0].DiscountRateDebit);
            acroFields.SetField("disc rate 5", dt[0].DiscountRate);
            acroFields.SetField("disc rate 6", dt[0].DiscountRateDebit);

            acroFields.SetField("trans fee 12", dt[0].DebitTransFee);
            acroFields.SetField("trans fee 13", dt[0].EBTTransFee);

            acroFields.SetField("auth fee 1", dt[0].TransactionFee);
            acroFields.SetField("auth fee 2", dt[0].TransactionFee);
            acroFields.SetField("auth fee 6", dt[0].NBCTransFee);
            acroFields.SetField("auth fee 7", dt[0].NBCTransFee);
            #endregion

            #region Other Service Fees
            //acroFields.SetField("OTHER account fee", dr["AppSetupFee"].ToString());
            acroFields.SetField("OTHER ACH 2fee", "10.00");
            acroFields.SetField("OTHER annual mem3 fee", dt[0].AnnualFee);
            acroFields.SetField("OTHER 4 batch settlement", dt[0].BatchHeader);
            acroFields.SetField("OTHER 5chargeback", dt[0].ChargebackFee);
            acroFields.SetField("OTHER 9", "5.00");
            acroFields.SetField("OTHER 10", dt[0].MonMin);

            acroFields.SetField("O Resource fee", dt[0].InternetStmt);
            acroFields.SetField("OTHER retrieval fee", dt[0].RetrievalFee);
            acroFields.SetField("OTHER statement fee", dt[0].CustServFee);
            acroFields.SetField("OTHER voyager fee", "0.02");
            acroFields.SetField("OTHER wireless monthly fee", "0.02");
            acroFields.SetField("OTHER wireless set up fee.0", "0.10");
            acroFields.SetField("OTHER wireless set up fee.1", "0.40");
            acroFields.SetField("OTHER wireless set up fee.2x", dt[0].WirelessAccess);

            acroFields.SetField("O A1", dt[0].TransactionFee);
            acroFields.SetField("O A3", dt[0].VoiceAuth);
            acroFields.SetField("O A4", dt[0].AVS);
            acroFields.SetField("O A5", "2.00");
            acroFields.SetField("O A6", dt[0].WirelessTransFee);
            #endregion

            acroFields.SetField("merchant DBA name", dt[0].DBA);

            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Chase Fee Schedule PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChaseFSInterchangePlusPDF
    #endregion 

    #region CHASE Old PDF
    //This function creates Chase About Merchant PDF
    public bool CreateChasePDFAbout(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL ChaseData = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = ChaseData.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase About Merchant.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase About_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase About_" + P1FirstName + " " + P1FirstName + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            #region Banking
            //Banking
            acroFields.SetField("Banking Info:First/Last Contact Name", "Manager");
            acroFields.SetField("Banking Info:Phone Number", dt[0].BankPhone);

            #endregion

            #region General Information
            acroFields.SetField("About Merchant's Bus.:DBA Name", dt[0].DBA);

            acroFields.SetField("Checklist Info:MCC", dt[0].MCC);
            acroFields.SetField("Checklist Info: Rep #", dt[0].RepNum);
            acroFields.SetField("Checklist Info: Print Sales Rep. Name", dt[0].RepName);

            //Refund Policy from View is 1,2, or 3
            acroFields.SetField("Client Visitation: 10. Return Policy", dt[0].RefundPolicy.ToString());

            //Visa Master Refund Policy is boolean 0 or 1
            acroFields.SetField("Client Visitation: 11. Do you have refund for MC/VISA", dt[0].VisaMasterRefund.ToString());


            if (dt[0].ThreeMonthsPrev.ToString() == "1")
                acroFields.SetField("Client Visitation: 11. Do you have Prev Processor MC/VISA Statements", "Yes");
            else
                acroFields.SetField("Client Visitation: 11. Do you have Prev Processor MC/VISA Statements", "No");

            if (dt[0].BillingSame.ToString() == "1")
            {
                acroFields.SetField("Mail Statements/Documents:  Bill To Name", dt[0].COMPANYNAME);
                acroFields.SetField("Mail Statements/Documents:  Contact Name", dt[0].P1FullName);
                acroFields.SetField("Mail Statements/Documents: Phone Number", dt[0].BusinessPhone);
                acroFields.SetField("Mail Statements/Documents:  Address", dt[0].BillingAddress);
                acroFields.SetField("Mail Statements/Documents:  City", dt[0].BillingCity);
                acroFields.SetField("Mail Statements/Documents:  State", dt[0].BillingState);
                acroFields.SetField("Mail Statements/Documents:: Zip", dt[0].BillingZipCode);
            }
            acroFields.SetField("Client Visitation: 13. Previous Processor", dt[0].PrevProcessor);
            acroFields.SetField("Client Visitation: 14. Previous Merchant Number", dt[0].PrevMerchantNum);

            acroFields.SetField("Client Visitation: 17. Email", dt[0].Email);

            acroFields.SetField("Client Visitation: 19.  Are customers required to have a deposit, time frame", dt[0].NumOfDaysProdDel);

            acroFields.SetField("Processing Information: 7.Debit Cash Back", dt[0].OnlineDebit.ToString());
            #endregion
            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Chase About PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChaseAboutPDF

    //This function creates Chase PDF Fee Schedule
    public bool CreateChasePDFFS(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase Fee Schedule.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase FS_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1LastName == "")
                    P1LastName = "Merchant";
                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase Fee Schedule_" + P1FirstName + " " + P1LastName + " " + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Service Fee Scgedule: DBA Name", dt[0].DBA);
            acroFields.SetField("Service Fee Schedule: Loc", "1");
            acroFields.SetField("Service Fee Schedule: Loc of", dt[0].NumOfLocs);
            
            #region Rates
            //Rates
            acroFields.SetField("Service Fee Schedule: Discount Fees: MC Quailified Credit Discount Rate", dt[0].DiscountRate.ToString());
            acroFields.SetField("Service Fee Schedule: Discount Fees: Visa Quailified Credit Discount Rate", dt[0].DiscountRate.ToString());
            acroFields.SetField("Service Fee Schedule: Discount Fees: MC Quailified Debit Discount Rate", dt[0].DiscountRateDebit.ToString());
            acroFields.SetField("Service Fee Schedule: Discount Fees: Visa Quailified Debit Discount Rate", dt[0].DiscountRateDebit.ToString());
            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Service Fee", dt[0].CustServFee.ToString());
            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Chargeback Fee", dt[0].ChargebackFee.ToString());
            //acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Chargeback Retrieval Fee", dt[0].RetrievalFee.ToString());
            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: ACH Fee Per Transfer", dt[0].BatchHeader.ToString());
            acroFields.SetField("Service Fee Schedule: Other Fees: Authorization: MC (Unbundled)", dt[0].TransactionFee.ToString());
            acroFields.SetField("Service Fee Schedule: Other Fees: Authorization: Visa Unbundled", dt[0].TransactionFee.ToString());

            //set Amex only if not Opted Out
            //if (dt[0].AmexAccept == "1")  
            acroFields.SetField("Service Fee Schedule: Other Fees: Authorization: American Express", dt[0].NBCTransFee.ToString());


            //set Amex only if not Opted Out
            //if (dt[0].DiscoverAccept == "1")
            acroFields.SetField("Service Fee Schedule: Other Fees: Authorization: Discover", dt[0].NBCTransFee.ToString());

            acroFields.SetField("Service Fee Schedule: Other Fees: Authorization: JCB", dt[0].NBCTransFee.ToString());
            //if (dt[0].JCBAccept == "1")
            acroFields.SetField("Service Fee Schedule: Other Fees: Authorization: Diners Club", dt[0].NBCTransFee.ToString());

            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Minimum Processing Fee", dt[0].MonMin.ToString());
            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Annual Membership Fee", dt[0].AnnualFee.ToString());
            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Internet Paper Statement Fee", dt[0].IPSFee.ToString());
            acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Wireless Access Fee", dt[0].WirelessAccess.ToString());
            acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: MC/Visa VRU/Voice", dt[0].VoiceAuth.ToString());
            acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: AVS-Auto", dt[0].AVS.ToString());
            //acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: AVS-Manual", dt[0].VoiceAuth.ToString());
            acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: EBT", dt[0].EBTTransFee.ToString());
            acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: Debit (PIN)", dt[0].DebitTransFee.ToString());

            if (Convert.ToBoolean(dt[0].Interchange))
            {
                if (Convert.ToBoolean(dt[0].Assessments))
                {
                    acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: Accessment Fees", ".095-.0925");//Mastercard and Visa                    
                }
                //acroFields.SetField("Service Fee Schedule: Other Fees: MC/VI Foreign: Other", "0");
                acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Other Code", "550-560");
                acroFields.SetField("Service Fee Schedule: Billed Monthly Fees: Other Desc", "I/C Pass through");
            }
            //Service Fee Schedule: Billed Monthly Fees: Other Desc
            //Service Fee Schedule: Other Fees: MC/VI Foreign: Accessment Fees
            #endregion

            acroFields.SetField("Service Fee Schedule: Printed Name of Signer", dt[0].P1FullName);
            acroFields.SetField("Service Fee Schedule: Title", dt[0].P1Title);
            stamper.FormFlattening = true;


            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Chase Fee Schedule PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChaseAboutPDF

    //This function creates Chase PDF
    public bool CreateChasePDFMP(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase MPA.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase MPA_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";
                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase MPA_" + P1FirstName + ".pdf");
            }
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            //stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "succeed", "succeed", PdfWriter.AllowCopy | PdfWriter.AllowPrinting);

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Tell Us About Your Business: Client", dt[0].COMPANYNAME);
            acroFields.SetField("Service Fee Scgedule: DBA Name", dt[0].DBA);
            acroFields.SetField("Merchant Processing Application: Of", dt[0].NumOfLocs);
            #region General Information

            if (dt[0].COMPANYNAME == dt[0].DBA)
                acroFields.SetField("Tell Us About Your Business: Same as Legal Name", "1");
                
            else
                acroFields.SetField("Tell Us About Your Business: DBA/Outlet Name", dt[0].DBA);

            acroFields.SetField("Tell Us About Your Business: Contact Name", dt[0].P1FullName);
            acroFields.SetField("Tell Us About Your Business: Address", dt[0].Address1);
            acroFields.SetField("Tell Us About Your Business: Suite", dt[0].Address2);

            acroFields.SetField("Tell Us About Your Business: City", dt[0].CITY);
            acroFields.SetField("Tell Us About Your Business: State", dt[0].STATE);
            acroFields.SetField("Tell Us About Your Business: Zipcode", dt[0].ZipCode);
            acroFields.SetField("Tell Us About Your Business: Fax Phone", dt[0].Fax);
            acroFields.SetField("Tell Us About Your Business: Business Phone", dt[0].BusinessPhone);

            if (dt[0].FederalTaxID != "")
            {
                acroFields.SetField("Provide More Business Data: Fed. Tax ID", dt[0].FederalTaxID);
                acroFields.SetField("Provide More Business Data: TIN Type", "1");
            }
            else if (dt[0].P1SSN != "")
            {
                acroFields.SetField("Provide More Business Data: Fed. Tax ID", dt[0].P1SSN);
                acroFields.SetField("Provide More Business Data: TIN Type", "2");
            }
            //acroFields.SetField("Describe Equipment Details: VAR/Internet.ToString()/Software", dt[0].Gateway);

            acroFields.SetField("Tell Us About Your Business: Same as Business Phone", dt[0].SameAsBusinessPhone.ToString());
            if (dt[0].BusinessPhone != dt[0].CustServPhone)
                acroFields.SetField("Tell Us About Your Business: Merchant's Customer Service Phone", dt[0].CustServPhone);

            //Business Type is 1 to 7
            acroFields.SetField("Provide More Business Data: Business Type", dt[0].LegalStatus.ToString());

            acroFields.SetField("Tell Us About Your Business:  Average Ticket/Sales Amount", dt[0].AvgTicket.ToString());
            acroFields.SetField("Tell Us About Your Business:  Annual MC/Visa Volume", dt[0].AnnualVol.ToString());
            acroFields.SetField("Provide More Business Data: Month/Yr Started", dt[0].StartYear.ToString());
            acroFields.SetField("Provide More Business Data: Products/Services You Sell", dt[0].ProductSold);
            acroFields.SetField("Other Entitlements: Non-Lic. JCB (existing account)", dt[0].JCBNum);
            acroFields.SetField("Other Entitlemen:Discover (EDC)", dt[0].DiscoverNum);
            acroFields.SetField("Other Entitlements:Amer. Exp", dt[0].AmexNum);
            acroFields.SetField("Other Entitlements", dt[0].JCBAccept);
            acroFields.SetField("Other Entitlements", dt[0].DiscoverAccept);
            acroFields.SetField("Other Entitlements:", dt[0].AmexAccept);

            acroFields.SetField("Describe Equipment Details: VAR/Internet/Software", dt[0].Gateway);
            acroFields.SetField("Describe Equipment:Equipment Type", dt[0].Equipment.ToString());
            acroFields.SetField("Describe Equip Details:Model Code and Name", dt[0].EquipmentModel.ToString());
            #endregion

            #region CardPCT
            acroFields.SetField("Provide More Business Data: Mag Swipe", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("Provide More Business Data: Keyed Manually", dt[0].ProcessPctKeyed.ToString());
            acroFields.SetField("Provide More Business Data: POS Cardswipe", dt[0].BusinessPctPOS.ToString());
            acroFields.SetField("Provide More Business Data: Mail Order", dt[0].BusinessPctMailOrder.ToString());
            acroFields.SetField("Provide More Business Data: Phone Order", dt[0].BusinessPctPhoneOrder.ToString());
            acroFields.SetField("Provide More Business Data: Tradeshows", dt[0].BusinessPctTradeShows.ToString());
            acroFields.SetField("Provide More Business Data: Internet", dt[0].BusinessPctInternet.ToString());
                       
            #endregion

            #region Principal #1
            //Principal #1
            acroFields.SetField("Provide Your Owner Info:ZIP", dt[0].P1ZipCode);
            acroFields.SetField("Provide Your Owner Info:State", dt[0].P1State);
            acroFields.SetField("Provide Your Owner Info:City", dt[0].P1City);
            acroFields.SetField("Provide Your Owner Info:Home Address", dt[0].P1Address);
            acroFields.SetField("Provide Your Owner Information:Title", dt[0].p1TitleID.ToString());
            if (dt[0].p1TitleID.ToString() == "6")
                acroFields.SetField("Provide Your Owner Information:Text", dt[0].P1Title);
            acroFields.SetField("Provide Your owner Info:Social Security", dt[0].P1SSN);
            acroFields.SetField("Provide Your owner Info:Owner/Partner", dt[0].P1FullName);
            
            //P1 Ownership field is correct, though labeled as 2 in the PDF
            acroFields.SetField("Provide Your owner Info:2.  % of owner", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("Provide Your owner Info:Phone Number", dt[0].P1PhoneNumber);

            acroFields.SetField("ProvideYour Owner Information: Print Name", dt[0].P1FullName);
            acroFields.SetField("ProvideYour Owner Info: Title", dt[0].P1Title);

            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("Provide Your Owner Info: 2. ZIP", dt[0].P2ZipCode);
            acroFields.SetField("Provide Your Owner Info:2. State", dt[0].P2State);
            acroFields.SetField("Provide Your Owner Info:2. City", dt[0].P2City);
            acroFields.SetField("Provide Your Owner Info: 2. Home Address", dt[0].p2Address);
            acroFields.SetField("Provide Your Owner Info: 2 Title", dt[0].P2TitleID.ToString());
            acroFields.SetField("Provide Your Owner Information: 2Text", dt[0].P2Title);
            acroFields.SetField("Provide Your owner Info:2. Social Security", dt[0].P2SSN);
            acroFields.SetField("Provide Your owner Info: 2 . Owner/Partner", dt[0].p2FullName);
            //P2 Ownership Pct is correct, though labeled as P1 in PDF
            acroFields.SetField("Provide Your owner Info:% of owner", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("Provide Your owner Info: 2. Phone Number", dt[0].p2PhoneNumber);
            
            #endregion

            if (dt[0].Platform.ToLower().Contains("nashville"))
                acroFields.SetField("Other Entitlements:Network", "2");
            else if (dt[0].Platform.ToLower().Contains("north"))
                acroFields.SetField("Other Entitlements:Network", "1");
            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Chase MPS PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChasePDFMP

    //This function creates Chase PDF for Multiple Locations
    public bool CreateChasePDFMPMultipleLocation(string ContactID, bool b3Locs)
    {
        //Get data for Chase PDF
        PDFBL ChaseData = new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = ChaseData.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase Multiple Locations.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase MPA_Multiple Locations_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase MPA_Multiple Locations_" + P1FirstName + " " + P1LastName + ".pdf");
            }
            FileStream fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Client: Business Legal Name", dt[0].COMPANYNAME);
            acroFields.SetField("Location_ of", dt[0].NumOfLocs);
            acroFields.SetField("Location", "2");
            #region General Information

            if (dt[0].COMPANYNAME == dt[0].DBA)
                acroFields.SetField("Same as Legal Name", "Yes");
            else
                acroFields.SetField("Your DBA/Outlet Name:", dt[0].DBA);

            acroFields.SetField("First Name Contact", dt[0].P1FullName);

            if (dt[0].FederalTaxID != "")
            {
                acroFields.SetField("Fed Tax ID", dt[0].FederalTaxID);
                acroFields.SetField("TIN Type", "1");
            }
            else if (dt[0].P1SSN != "")
            {
                acroFields.SetField("Fed Tax ID", dt[0].P1SSN);
                //acroFields.SetField("TIN Type", "2");
            }
            acroFields.SetField("EQUIPMENT: VAR/ Internet.ToString()/ Software: Name", dt[0].Gateway);

            //acroFields.SetField("Tell Us About Your Business: Same as Business Phone", dt[0].SameAsBusinessPhone);
            if (dt[0].BusinessPhone != dt[0].CustServPhone)
                acroFields.SetField("Merchant's Customer Service Phone", dt[0].CustServPhone);

            acroFields.SetField("Products ans Services", dt[0].ProductSold);
            acroFields.SetField("Other Entitlements: Non-Lic. JCB #", dt[0].JCBNum);
            acroFields.SetField("Other Entitlements: Discover #", dt[0].DiscoverNum);
            acroFields.SetField("Other Entitlements: Amer. Exp #", dt[0].AmexNum);
            if (dt[0].JCBAccept == "Yes")
                acroFields.SetField("Other Entitlements: JCB", "1");
            if (dt[0].DiscoverAccept == "Yes")
                acroFields.SetField("Other Entitlements:  Discover", "1");
            if (dt[0].AmexAccept == "Yes")
                acroFields.SetField("Other Entitlements:  Amer. Exp", "1");

            acroFields.SetField("Describe Equipment Details: VAR/Internet.ToString()/Software", dt[0].Gateway);
            #endregion

            #region CardPCT
            acroFields.SetField("Mag Swipe", dt[0].ProcessPctSwiped.ToString());
            acroFields.SetField("Keyed.ToString() Manually", dt[0].ProcessPctKeyed.ToString());
            acroFields.SetField("POS Carswipe/Manual Imprint", dt[0].BusinessPctPOS.ToString());
            acroFields.SetField("Mail Order.ToString()", dt[0].BusinessPctMailOrder.ToString());
            acroFields.SetField("Phone Order.ToString()", dt[0].BusinessPctPhoneOrder.ToString());
            acroFields.SetField("Internet.ToString()", dt[0].BusinessPctInternet.ToString());
            acroFields.SetField("Tradeshows", dt[0].BusinessPctTradeShows.ToString());
            #endregion

            if (dt[0].Platform.ToLower().Contains("nashville"))
                acroFields.SetField("Other Entitlements:  Network", "2");
            else if (dt[0].Platform.ToLower().Contains("north"))
                acroFields.SetField("Other Entitlements:  Network", "1");

            if (b3Locs)
            {
                //Populate the second half of the PDF for location 3
                acroFields.SetField("2Client: Business Legal Name", dt[0].COMPANYNAME);
                acroFields.SetField("2Location_ of", dt[0].NumOfLocs);
                acroFields.SetField("2Location", "3");
                #region General Information

                if (dt[0].COMPANYNAME == dt[0].DBA)
                    acroFields.SetField("2Same as Legal Name", "Yes");
                else
                    acroFields.SetField("2Your DBA/Outlet Name:", dt[0].DBA);

                acroFields.SetField("2First Name Contact", dt[0].P1FullName);

                if (dt[0].FederalTaxID != "")
                {
                    acroFields.SetField("2Fed Tax ID", dt[0].FederalTaxID);
                    acroFields.SetField("2TIN Type", "1");
                }
                else if (dt[0].P1SSN != "")
                {
                    acroFields.SetField("2Fed Tax ID", dt[0].P1SSN);
                    acroFields.SetField("2TIN Type", "2");
                }
                acroFields.SetField("2EQUIPMENT: VAR/ Internet.ToString()/ Software: Name", dt[0].Gateway);

                //acroFields.SetField("Tell Us About Your Business: Same as Business Phone", dt[0].SameAsBusinessPhone);
                if (dt[0].BusinessPhone != dt[0].CustServPhone)
                    acroFields.SetField("2Merchant's Customer Service Phone", dt[0].CustServPhone);

                acroFields.SetField("2Products ans Services", dt[0].ProductSold);
                acroFields.SetField("2Other Entitlements: Non-Lic. JCB #", dt[0].JCBNum);
                acroFields.SetField("2Other Entitlements: Discover #", dt[0].DiscoverNum);
                acroFields.SetField("2Other Entitlements: Amer. Exp #", dt[0].AmexNum);
                if (dt[0].JCBAccept == "Yes")
                    acroFields.SetField("2Other Entitlements: JCB", "1");
                if (dt[0].DiscoverAccept == "Yes")
                    acroFields.SetField("2Other Entitlements:  Discover", "1");
                if (dt[0].AmexAccept == "Yes")
                    acroFields.SetField("2Other Entitlements:  Amer. Exp", "1");

                acroFields.SetField("2Describe Equipment Details: VAR/Internet.ToString()/Software", dt[0].Gateway);
                #endregion

                #region CardPCT
                acroFields.SetField("2Mag Swipe", dt[0].ProcessPctSwiped.ToString().ToString());
                acroFields.SetField("2Keyed.ToString() Manually", dt[0].ProcessPctKeyed.ToString().ToString());
                acroFields.SetField("2POS Carswipe/Manual Imprint", dt[0].BusinessPctPOS.ToString());
                acroFields.SetField("2Mail Order.ToString()", dt[0].BusinessPctMailOrder.ToString().ToString());
                acroFields.SetField("2Phone Order.ToString()", dt[0].BusinessPctPhoneOrder.ToString().ToString());
                acroFields.SetField("2Internet.ToString()", dt[0].BusinessPctInternet.ToString().ToString());
                acroFields.SetField("2Tradeshows", dt[0].BusinessPctTradeShows.ToString().ToString());
                #endregion

                if (dt[0].Platform.ToLower().Contains("nashville"))
                    acroFields.SetField("2Other Entitlements:  Network", "2");
                else if (dt[0].Platform.ToLower().Contains("north"))
                    acroFields.SetField("2Other Entitlements:  Network", "1");
            }//end if 3 locations 

            stamper.FormFlattening = true;
            stamper.Close();           
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChasePDFMPMultipleLocation

    //This function creates Chase PDF Addendum
    public bool CreateChasePDFCreditAdd(string ContactID)
    {
        //Get data for Chase PDF
        PDFBL PDF= new PDFBL();
        PartnerDS.ACTChasePDFDataTable dt = PDF.GetChaseDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            //Populate data in PDF
            //Put the chase PDF in the PDF folder in Partner and name the PDF accordingly
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Chase Credit Addendum.pdf"));
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Chase Credit Addendum_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                string P1LastName = dt[0].P1LastName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "Chase Addendum_" + P1FirstName + " " +  P1LastName + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;
            acroFields.SetField("Merchant Processing Credit Addendum", dt[0].DBA);
            acroFields.SetField("Other Enclosures 4. Internet (required) list Web site address", dt[0].Website);
            if (dt[0].Website != "")
                acroFields.SetField("Other Enclosures 4. List Web Site Address", "Yes");

            //if ( ( Convert.ToInt32(dt[0].NumofDaysProdDel.ToString()  >= 0 ) && (Convert.ToInt32(dt[0].NumofDaysProdDel )  <= 7 )  )
            //  acroFields.SetField("Mail/Telephone Order.ToString() 3. 0-7 days", "100");

            stamper.FormFlattening = true;


            stamper.Close();

            DisplayMessage("Chase Addendum PDF created in the cusomter folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Chase Data not found for this record.");
            return false;
        }
    }//end function CreateChasePDFCreditAdd

    protected void btnChaseAbout_Click(object sender, EventArgs e)
    {
        try
        {
            CreateChasePDFAbout(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnChaseFee_Click(object sender, EventArgs e)
    {
        try
        {
            CreateChasePDFFS(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnChaseMP_Click(object sender, EventArgs e)
    {
        try
        {
            //Get data for Chase PDF
            PDFBL ChaseData = new PDFBL();
            PartnerDS.ACTChasePDFDataTable dt = ChaseData.GetChaseDataFromACT(selContactID);
            if (dt.Rows.Count > 0)
            {
                if (dt[0].NumOfLocs.Trim() != "")
                {
                    if (Convert.ToInt32(dt[0].NumOfLocs) >= 1)
                    {
                        CreateChasePDFMP(selContactID);//Create MPA PDF for first location
                        if (Convert.ToInt32(dt[0].NumOfLocs) == 2)
                        {
                            CreateChasePDFMPMultipleLocation(selContactID, false);//Create Multiple location PDF for location 2                            
                        }//end if num locs is 2
                        else if (Convert.ToInt32(dt[0].NumOfLocs) > 2)
                        {
                            CreateChasePDFMPMultipleLocation(selContactID, true);//Create Multiple location PDF for loc 3                            
                        }//end if number of locs is > 2
                    }//end if numlocs is >= 1
                }//end if numlocs is not blank
                else // If Num Locs not entered then default it to 1 of 1 and create the chase mpa PDF
                {
                    CreateChasePDFMP(selContactID);
                }
            }//end if count not 0            
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnChaseCreditAdd_Click(object sender, EventArgs e)
    {
        try
        {
            CreateChasePDFCreditAdd(selContactID);
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
    #endregion

    #region INTERNATIONAL PDF
    public int CreateInternationalPDF(string ContactID)
    {
        //This function creates International Cal App PDF                
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTOptimalIntlPDFDataTable dt = PDF.GetInternationalDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/CAL_Application_NA.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/CAL_Application_NA_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dt[0].P1FirstName;
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";
                strPath = Server.MapPath(strHost + FilePath + "/" + "CAL_Application_NA_" + P1FirstName.Substring(0, 1) + dt[0].P1LastName + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            #region Business Information
           
            acroFields.SetField("MerchantName", dt[0].DBA);
            if (dt[0].COMPANYNAME != dt[0].DBA)
                acroFields.SetField("CorpName", dt[0].COMPANYNAME);

            acroFields.SetField("MerchantAddress", dt[0].Address1 + ", " + dt[0].Address2);
            acroFields.SetField("MerchantCity", dt[0].CITY);
            acroFields.SetField("MerchantState", dt[0].STATE.ToString());
            acroFields.SetField("MerchantCountry", dt[0].Country);
            acroFields.SetField("MerchantZip", dt[0].ZipCode);

            if (dt[0].Address1 != dt[0].BillingAddress1)
            {
                acroFields.SetField("CorpAddress", dt[0].BillingAddress1 + ", " + dt[0].BillingAddress2);
                acroFields.SetField("CorpCity", dt[0].BillingCity);
                acroFields.SetField("CorpState", dt[0].BillingState.ToString());
                acroFields.SetField("CorpCountry", dt[0].billingCountry);
                acroFields.SetField("CorpZip", dt[0].BillingZipCode);
            }

            acroFields.SetField("ContactName", dt[0].FIRSTNAME + " " + dt[0].LASTNAME);
            acroFields.SetField("ContactEmail", dt[0].Email);
            acroFields.SetField("ContactPhone", dt[0].BusinessPhonePrefix + "-" + dt[0].BusinessPhonePostfix);
            acroFields.SetField("ContactFax", dt[0].FaxPhonePrefix + "-" + dt[0].FaxPhonePostfix);
            acroFields.SetField("CSPhone", dt[0].CustServPhonePrefix + "-" + dt[0].CustServPhonePostfix);

            acroFields.SetField("TaxNum", dt[0].FederalTaxID);
            acroFields.SetField("BusinessName", dt[0].DBA);
            acroFields.SetField("BusinessPhone", dt[0].BusinessPhonePrefix + "-" + dt[0].BusinessPhonePostfix);
            acroFields.SetField("BusinessURL", dt[0].Website);

            acroFields.SetField("MonthlyVolume", dt[0].MonthlyVolume.ToString());
            acroFields.SetField("AverageTicket", dt[0].AverageTicket.ToString());
            acroFields.SetField("HighTicket", dt[0].MaxTicket.ToString());
            
            acroFields.SetField("YearsBusiness", dt[0].YIB.ToString());
            acroFields.SetField("ProductDescription1", dt[0].ProductSold);

            if (dt[0].LegalStatus == "Corporation")
                acroFields.SetField("Ownership", "1");
            if (dt[0].LegalStatus == "Sole Proprietorship")
                acroFields.SetField("Ownership", "2");            
            if (dt[0].LegalStatus == "Partnership")
                acroFields.SetField("Ownership", "3");
            //if (dt[0].LegalStatus == "Publicly TRaded")
            //  acroFields.SetField("Ownership", "4");
            if (dt[0].LegalStatus == "Government")
                acroFields.SetField("Ownership", "5");
            if (dt[0].LegalStatus == "Non-Profit")
                acroFields.SetField("Ownership", "6");
            if (dt[0].LegalStatus == "LLC")
                acroFields.SetField("Ownership", "7");
            
            #endregion
        
            #region Principal/Owner Information
            //Principal 1
            acroFields.SetField("PrincipalFirst1", dt[0].P1FirstName);
            acroFields.SetField("PrincipalLast1", dt[0].P1LastName);
            acroFields.SetField("PrincipalOwnership1", dt[0].P1OwnershipPercent.ToString());
            acroFields.SetField("PrincipalSSN1", dt[0].P1SSN);
            acroFields.SetField("PrincipalLicense1", dt[0].P1DriversLicenseNo);
            acroFields.SetField("PrincipalTitle1", dt[0].P1Title);
            acroFields.SetField("PrincipalDOB1", dt[0].P1DOB);
            acroFields.SetField("PrincipalAddress1", dt[0].P1Address);
            acroFields.SetField("PrincipalCity1", dt[0].P1City);
            acroFields.SetField("PrincipalState1", dt[0].P1State);
            acroFields.SetField("PrincipalCountry1", dt[0].P1Country);
            acroFields.SetField("PrincipalZip1", dt[0].P1ZipCode);
            acroFields.SetField("Home Phone", dt[0].HomePhonePrefix + "-" + dt[0].HomePhonePostfix);
            acroFields.SetField("PrincipalEmail1", dt[0].Email);

            //Principal 2
            acroFields.SetField("PrincipalFirst2", dt[0].P2FirstName);
            acroFields.SetField("PrincipalLast2", dt[0].P2LastName);
            acroFields.SetField("PrincipalOwnership2", dt[0].P2OwnershipPercent.ToString());
            acroFields.SetField("PrincipalSSN2", dt[0].P2SSN);
            acroFields.SetField("PrincipalLicense2", dt[0].P2DriversLicenseNo);
            acroFields.SetField("PrincipalTitle2", dt[0].P2Title);
            acroFields.SetField("PrincipalDOB2", dt[0].P2DOB);
            acroFields.SetField("PrincipalAddress2", dt[0].p2Address);
            acroFields.SetField("PrincipalCity2", dt[0].P2City);
            acroFields.SetField("PrincipalState2", dt[0].P2State);
            acroFields.SetField("PrincipalCountry2", dt[0].P2Country);
            acroFields.SetField("PrincipalZip2", dt[0].P2ZipCode);
            acroFields.SetField("PrincipalPhone2", dt[0].p2PhoneNumber);  
            //acroFields.SetField("2OWNSOI", dt[0].p2email);                       

            if (dt[0].CTMF == "Yes")
                acroFields.SetField("Terminated", "2");
            else
                acroFields.SetField("Terminated", "1");

            acroFields.SetField("PrincipalName1", dt[0].P1FullName);
            acroFields.SetField("MerchantTitle1", dt[0].P1Title);

            acroFields.SetField("PrincipalName2", dt[0].P2FullName);
            acroFields.SetField("MerchantTitle2", dt[0].P2Title);

            acroFields.SetField("GuarantorName1", dt[0].P2FullName);
            acroFields.SetField("GuarantorCompany1", dt[0].COMPANYNAME + " / " + dt[0].P1Title);

            acroFields.SetField("GuarantorName2", dt[0].P2FullName);
            acroFields.SetField("GuarantorCompany2", dt[0].COMPANYNAME + " / " + dt[0].P2Title);

            acroFields.SetField("CorpCertTitle", dt[0].P1Title);
            acroFields.SetField("CorpCertName1", dt[0].P1FullName);
            acroFields.SetField("CorpCertTitle2", dt[0].P1Title);

            acroFields.SetField("CorpCertName2", dt[0].P2FullName);
            acroFields.SetField("CorpCertTitle3", dt[0].P2Title);   

            #endregion

            #region Schedule A - Pricing
            //Rates
            acroFields.SetField("CardPercentage", dt[0].PctSwp.ToString());
            double moto = Convert.ToDouble(dt[0].BusinessPctMail.ToString()) + Convert.ToDouble(dt[0].BusinessPctPhone.ToString());
            acroFields.SetField("MotoPercentage", moto.ToString().Trim());
            acroFields.SetField("InternetPercentage", dt[0].BusinessPctInternet.ToString());
            if (!dt[0].Gateway.ToString().Trim().Contains("Optimal"))
                acroFields.SetField("GatewayName", dt[0].Gateway.ToString().Trim());

            acroFields.SetField("BlendRateUSD", dt[0].DiscQNP.ToString());
            acroFields.SetField("TxnFeeUSD", dt[0].TransactionFee.ToString());
            acroFields.SetField("VolumeUSD", dt[0].MonthlyVolume.ToString());
            acroFields.SetField("ApplicationUSD", dt[0].AppFee.ToString());

            acroFields.SetField("MaintenanceUSD", dt[0].CustServFee.ToString());
            acroFields.SetField("ReportingUSD", dt[0].InternetStmt.ToString());
            acroFields.SetField("MinimumUSD", dt[0].MonMin.ToString());
            acroFields.SetField("GatewayUSD", dt[0].GatewayMonFee.ToString());

            acroFields.SetField("ChargebackUSD", dt[0].ChargebackFee.ToString());
            acroFields.SetField("WireUSD", dt[0].BatchHeader.ToString());
            acroFields.SetField("AVSUSD", dt[0].AVS.ToString());
            acroFields.SetField("GatewayTxnUSD", dt[0].GatewayTransFee.ToString());
            acroFields.SetField("MembershipUSD", dt[0].AnnualFee.ToString());
            acroFields.SetField("ReserveRate", dt[0].RollingReserve.ToString().Trim());

            #endregion

            #region Schedule B - Auhtorization For Funds Tranfer
            //Beneficiary Info
            acroFields.SetField("BenInfoName", dt[0].COMPANYNAME.ToString().Trim());
            acroFields.SetField("BenInfoAddress", dt[0].Address1.ToString().Trim() + ", " + dt[0].Address2.ToString().Trim());
            acroFields.SetField("BenInfoCity", dt[0].CITY.ToString().Trim());
            acroFields.SetField("BenInfoProv", dt[0].STATE.ToString().Trim());
            acroFields.SetField("BenInfoCountry", dt[0].Country.ToString().Trim());
            acroFields.SetField("BenInfoAccount", dt[0].CheckingAcctNum.ToString().Trim());

            //Beneficiary Bank
            acroFields.SetField("BenBankName", dt[0].BankName.ToString().Trim());
            acroFields.SetField("BenBankAddress", dt[0].BankAddr.ToString().Trim());
            acroFields.SetField("BenBankCity", dt[0].BankCity.ToString().Trim());
            acroFields.SetField("BenBankProv", dt[0].BankState.ToString().Trim());
            //acroFields.SetField("BenBankCountry", dr["Country"].ToString().Trim());
            acroFields.SetField("BenBankAba", dt[0].RoutingNum.ToString().Trim());

            #endregion

            #region Questionnaire
            acroFields.SetField("ProdDescription1", dt[0].ProductSold.ToString().Trim());

            if (dt[0].RefundPolicy.ToString().Trim().ToLower().Contains("within 30 days"))
                acroFields.SetField("ReturnPolicy", "2");
            else if (dt[0].RefundPolicy.ToString().Trim().ToLower().Contains("no refund"))
                acroFields.SetField("ReturnPolicy", "3");
            else if (dt[0].RefundPolicy.ToString().Trim().ToLower().Contains("other"))
            {
                acroFields.SetField("ReturnPolicy", "4");
                acroFields.SetField("SpecifyPolicy", dt[0].OtherRefund.ToString().Trim());
            }
            else
            {
                acroFields.SetField("ReturnPolicy", "4");
                acroFields.SetField("SpecifyPolicy", dt[0].RefundPolicy.ToString().Trim());
            }

            int numDaysDelivered = Convert.ToInt16(dt[0].NumDaysDel.ToString().Trim());
            acroFields.SetField("Turnaround", numDaysDelivered.ToString());
            #endregion

            #region AnnexB

            acroFields.SetField("AnnexBMerchant", dt[0].DBA);
            acroFields.SetField("AnnexBMerchantName", dt[0].DBA);
            acroFields.SetField("AnnexBMerchantAddress", dt[0].Address1 + ", " + dt[0].Address2 + ", " + dt[0].CITY + " " + dt[0].STATE.ToString() + " " + dt[0].ZipCode + " " + dt[0].Country);
            acroFields.SetField("AnnexBMerchantBank1", dt[0].CheckingAcctNum.ToString().Trim());
            acroFields.SetField("AnnexBMerchantBank3", dt[0].BankName.ToString().Trim());
            acroFields.SetField("AnnexBMerchantBank4", dt[0].BankCity.ToString().Trim());

            acroFields.SetField("AnnexBGuarantor1", dt[0].P1FullName);
            acroFields.SetField("AnnexBGuarantor2", dt[0].P2FullName);
            acroFields.SetField("AnnexBSigner1", dt[0].P1FullName);

            acroFields.SetField("AnnexBMerchant2", dt[0].DBA);

            #endregion

            stamper.FormFlattening = true;
            stamper.Close();
            DisplayMessage("Optimal International PDF created in the customer folder - " + FilePath);
            return 0;
        }//end if count not 0
        else
            DisplayMessage("CAL Data not found for this record");
            return 1;
    }//end function CreateInternationalPDF

    #endregion

    #region Payvision PDF
    public int CreatePayvisionPDF(string ContactID)
    {
        //This function creates International Cal App PDF                
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetPayvisionDataFromACT(ContactID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Payvision Application.pdf"));
            
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/Payvision Application_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dr["FIRSTNAME"].ToString().Trim();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";
                strPath = Server.MapPath(strHost + FilePath + "/" + "Payvision Application_" + P1FirstName.Substring(0, 1) + dr["LASTNAME"] + ".pdf");
            }
            /*
            MemoryStream mStream = new MemoryStream();
            PdfStamper stamper = new PdfStamper(reader, mStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Commerce1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            */
            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            
            AcroFields acroFields = stamper.AcroFields;

            #region Company Profile

            acroFields.SetField("DBA", dr["DBA"].ToString().Trim());
            acroFields.SetField("Corporate Legal Name", dr["CompanyName"].ToString().Trim());

            acroFields.SetField("Location Address", dr["Address"].ToString().Trim());
            acroFields.SetField("Location City,State", dr["City"].ToString().Trim() + ", " + dr["State"].ToString());
            acroFields.SetField("Location Zip Code", dr["ZipCode"].ToString().Trim());
            acroFields.SetField("Location Country", dr["Country"].ToString().Trim());

            acroFields.SetField("Corporate Billing Address", dr["BillingAddress"].ToString().Trim());
            acroFields.SetField("Billing City,State", dr["BillingCity"].ToString().Trim() + ", " + dr["BillingState"].ToString());
            acroFields.SetField("Billing Zip Code", dr["BillingZipCode"].ToString().Trim());
            acroFields.SetField("Billing Country", dr["BillingCountry"].ToString().Trim());

            acroFields.SetField("Contact Name & Relationship", dr["FirstName"].ToString().Trim() + " " + dr["LastName"].ToString().Trim());
            acroFields.SetField("Contact Email Address", dr["Email"].ToString().Trim());
            acroFields.SetField("Telephone Number", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("Fax Number", dr["Fax"].ToString().Trim());
            acroFields.SetField("Company Registration Number, Federal Tax ID", dr["FederalTaxID"].ToString().Trim());

            if (dr["Bankruptcy"].ToString().Trim() == "Yes")
                acroFields.SetField("FiledForBankruptcy", "YES");
            else
                acroFields.SetField("FiledForBankruptcy", "NO");

            if (dr["LegalStatus"].ToString().Trim() == "Corporation")
                acroFields.SetField("Corporation", "On");
            else if (dr["LegalStatus"].ToString().Trim() == "LLC")
                acroFields.SetField("Limited Liability Company", "On");
            else if (dr["LegalStatus"].ToString().Trim() == "Sole Proprietorship")
                acroFields.SetField("Sole Proprietor", "On");
            else if (dr["LegalStatus"].ToString().Trim() == "Partnership")
                acroFields.SetField("Partnership", "On");
            else if (dr["LegalStatus"].ToString().Trim() == "Non-Profit")
                acroFields.SetField("Not for Profit", "On");

            acroFields.SetField("Time in Business", dr["YIB"].ToString().Trim() + " yrs & " + dr["MIB"].ToString().Trim() + " mons");

            #endregion

            #region Ownership Profile
            //Principal 1
            acroFields.SetField("Principal1 Name", dr["P1FullName"].ToString().Trim());
            acroFields.SetField("Principal1 Title", dr["P1Title"].ToString().Trim());
            acroFields.SetField("Principal1 OwnershipPct", dr["P1OwnershipPercent"].ToString().Trim());
            acroFields.SetField("Principal1 Telephone Number", dr["P1PhoneNumber"].ToString().Trim());
            acroFields.SetField("Principal1 Email", dr["Email"].ToString().Trim());

            acroFields.SetField("Principal1 DOB", dr["P1DOB"].ToString().Trim());
            acroFields.SetField("Principal1 SSN", dr["P1SSN"].ToString().Trim());
            //acroFields.SetField("Principal1 Identification Type", "Drivers License");
            //acroFields.SetField("Principal1 StateCounty of ID", dr["P1DriversLicenseState"].ToString().Trim());                
            //acroFields.SetField("PrincipalLicense1", dr["P2DriversLicenseNo"].ToString().Trim());

            acroFields.SetField("Principal1 Address", dr["P1Address"].ToString().Trim());
            acroFields.SetField("Principal1 City,State", dr["P1City"].ToString().Trim() + ", " + dr["P1State"].ToString().Trim());
            acroFields.SetField("Principal1 Zip Code", dr["P1ZipCode"].ToString().Trim());
            acroFields.SetField("Principal1 Country", dr["P1Country"].ToString().Trim());

            //Principal 2
            acroFields.SetField("Principal2 Name", dr["P2FullName"].ToString().Trim());
            acroFields.SetField("Principal2 Title", dr["P2Title"].ToString().Trim());
            acroFields.SetField("Principal2 OwnershipPct", dr["P2OwnershipPercent"].ToString().Trim());
            acroFields.SetField("Principal2 Telephone Number", dr["P2PhoneNumber"].ToString().Trim());
            acroFields.SetField("Principal2 Email", dr["P2Email"].ToString().Trim());

            acroFields.SetField("Principal2 DOB", dr["P2DOB"].ToString().Trim());
            acroFields.SetField("Principal2 SSN", dr["P2SSN"].ToString().Trim());
            //acroFields.SetField("Principal2 Identification Type", "Drivers License");
            //acroFields.SetField("Principal2 StateCounty of ID", dr["P2DriversLicenseState"].ToString().Trim());
            //acroFields.SetField("PrincipalLicense2", dr["P2DriversLicenseNo"].ToString().Trim());

            acroFields.SetField("Principal2 Address", dr["P2Address"].ToString().Trim());
            acroFields.SetField("Principal2 City,State", dr["P2City"].ToString().Trim() + ", " + dr["P2State"].ToString().Trim());
            acroFields.SetField("Principal2 Country", dr["P2Country"].ToString().Trim());
            acroFields.SetField("Principal2 Zip Code", dr["P2ZipCode"].ToString().Trim());

            #endregion

            #region Business Profile

            acroFields.SetField("Current Acquirer", dr["PrevProcessor"].ToString().Trim());
            acroFields.SetField("Reason for leaving current acquirer", dr["ReasonforLeaving"].ToString().Trim());

            acroFields.SetField("Pct MOTO", dr["BusinessPctMailOrder"].ToString().Trim());
            acroFields.SetField("Pct Internet", dr["BusinessPctInternet"].ToString().Trim());
            acroFields.SetField("Pct Swipe", dr["ProcessPctSwiped"].ToString().Trim());

            acroFields.SetField("Monthly Volume", dr["MonthlyVolume"].ToString().Trim());
            acroFields.SetField("Average Ticket", dr["AverageTicket"].ToString().Trim());
            acroFields.SetField("Highest Ticket", dr["MaxTicket"].ToString().Trim());

            acroFields.SetField("URLs", dr["Website"].ToString().Trim());
            acroFields.SetField("Products services sold", dr["ProductSold"].ToString().Trim());

            //Always default Visa and MasterCard
            acroFields.SetField("Visa", "On");
            acroFields.SetField("MasterCard", "On");

            if (dr["AmexApplied"].ToString().Trim().Contains("Yes"))
                acroFields.SetField("American Express", "On");

            if ((dr["DiscoverApplied"].ToString().Trim().Contains("Yes")) || (dr["DiscoverApplied"].ToString().Trim().Contains("MAP")))
                acroFields.SetField("Discover", "On");

            #endregion

            #region Currency Requested
            acroFields.SetField("SWIFT Code", dr["BankRoutingNumber"].ToString().Trim());
            acroFields.SetField("Bank Name", dr["BankName"].ToString().Trim());
            acroFields.SetField("Bank Address", (dr["BankAddress"].ToString().Trim() + ", " + dr["BankCity"].ToString().Trim() + ", " +
                    dr["BankState"].ToString().Trim() + " " + dr["BankZip"].ToString().Trim()));
            acroFields.SetField("Bank Phone Number", dr["BankPhone"].ToString().Trim());
            //acroFields.SetField("BenBankCity", dr["BankCity"].ToString().Trim());
            //acroFields.SetField("BenBankProv", dr["BankState"].ToString().Trim());
            //acroFields.SetField("BenBankCountry", dr["Country"].ToString().Trim());
            acroFields.SetField("Account Number", dr["BankAccountNumber"].ToString().Trim());

            #endregion                              

            #region MOTO Questionnaire

            acroFields.SetField("Merchant Name", dr["CompanyName"].ToString().Trim());
            acroFields.SetField("Monthly Processing Volume", dr["MonthlyVolume"].ToString().Trim());
            acroFields.SetField("Average Ticket", dr["AverageTicket"].ToString().Trim());
            acroFields.SetField("Products Services Sold 1", dr["ProductSold"].ToString().Trim());
            acroFields.SetField("List all URLs", dr["Website"].ToString().Trim());
            if (dr["RefundPolicy"].ToString().Trim().ToLower().Contains("other"))
                acroFields.SetField("Describe your refund policy 1", dr["OtherRefund"].ToString().Trim());
            else
                acroFields.SetField("Describe your refund policy 1", dr["RefundPolicy"].ToString().Trim());

            #endregion

            stamper.FormFlattening = true;
            stamper.Close(); 
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("Payvision PDF created in the customer folder - " + FilePath);
            return 0;
        }//end if count not 0
        else
            DisplayMessage("Payvision Data not found for this record");
        return 1;
    }//end function CreateInternationalPDF

    #endregion

    #region Barclays PDF
    public int CreateBarclaysPDF(string ContactID)
    {
        //This function creates International Barclyas App PDF                
        PDFBL PDF = new PDFBL();
        PartnerDS.ACTOptimalIntlPDFDataTable dt = PDF.GetInternationalDataFromACT(ContactID);
        if (dt.Rows.Count > 0)
        {
            if (dt[0].Processor.ToString().ToLower().Contains("barclays"))
            {
                PdfReader reader = new PdfReader(Server.MapPath("../PDF/Barclays Bank Agreement.pdf"));

                ACTDataBL fp = new ACTDataBL();
                string FilePath = fp.ReturnCustomerFilePath(ContactID);
                string strPath = "../PDF/Barclays Bank Agreement_" + ContactID + ".pdf";
                if (FilePath != string.Empty)
                {
                    FilePath = FilePath.ToLower();
                    FilePath = FilePath.Replace("file://s:\\customers", "");
                    FilePath = FilePath.Replace("\\", "/");

                    string strHost = "../../Customers";
                    string P1FirstName = dt[0].P1FirstName;
                    //if the Principal's Name is empty, initalize to ECE Merchant
                    if (P1FirstName == "")
                        P1FirstName = "CTC";
                    strPath = Server.MapPath(strHost + FilePath + "/" + "Barclays Bank Agreement_" + P1FirstName.Substring(0, 1) + dt[0].P1LastName + ".pdf");
                }

                FileStream fStream = null;
                fStream = new FileStream(strPath, FileMode.Create);
                PdfStamper stamper = new PdfStamper(reader, fStream);
                stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

                AcroFields acroFields = stamper.AcroFields;

                #region Business Information

                acroFields.SetField("CompanyName", dt[0].COMPANYNAME);
                acroFields.SetField("DBA", dt[0].DBA);
                acroFields.SetField("P1Name", dt[0].P1FullName);
                acroFields.SetField("P1Title", dt[0].P1Title);

                #endregion

                #region Schedule 7A - Fees

                acroFields.SetField("SetupFee", "175.00");
                acroFields.SetField("MonthlyFee", dt[0].CustServFee.ToString());
                acroFields.SetField("DiscountFee", dt[0].DiscQNP.ToString());
                acroFields.SetField("TransactionFee", dt[0].TransactionFee.ToString());

                acroFields.SetField("ChargebackFee", dt[0].ChargebackFee.ToString());
                acroFields.SetField("WirelessFee", dt[0].BatchHeader.ToString());

                #endregion

                stamper.FormFlattening = true;
                stamper.Close();
                DisplayMessage("Optimal Barclays PDF created in the customer folder - " + FilePath);
                return 0;
            }//end if count not 0
            else
                DisplayMessage("Barclays Data not found for this record");
        }
        return 1;
    }//end function CreateBarclaysPDF

    #endregion

    #region St. Kitts PDF
    public int CreateStKittsPDF(string ContactID)
    {
        //This function creates St. Kitts App PDF                
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetStKittsDataFromACT(ContactID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/St_Kitts_Application.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "../PDF/St_Kitts_Application_" + ContactID + ".pdf";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dr["P1FirstName"].ToString().Trim();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";
                strPath = Server.MapPath(strHost + FilePath + "/" + "St_Kitts_Application_" + P1FirstName.Substring(0, 1) + dr["P1LastName"] + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;

            #region Business Information
            acroFields.SetField("MerchantName", dr["DBA"].ToString().Trim());
            if (dr["COMPANYNAME"].ToString().Trim() != dr["DBA"].ToString().Trim())
                acroFields.SetField("CorpName", dr["COMPANYNAME"].ToString().Trim());

            acroFields.SetField("MerchantAddress", dr["Address1"].ToString().Trim() + ", " + dr["Address2"].ToString().Trim());
            acroFields.SetField("MerchantCity", dr["CITY"].ToString().Trim());
            acroFields.SetField("MerchantState", dr["STATE"].ToString().Trim());
            acroFields.SetField("MerchantCountry", dr["Country"].ToString().Trim());
            acroFields.SetField("MerchantZip", dr["ZipCode"].ToString().Trim());
            if (dr["Address1"].ToString().Trim() != dr["BillingAddress1"].ToString().Trim())
            {
                acroFields.SetField("CorpAddress", dr["BillingAddress1"].ToString().Trim() + ", " + dr["BillingAddress2"].ToString().Trim());
                acroFields.SetField("CorpCity", dr["BillingCity"].ToString().Trim());
                acroFields.SetField("CorpState", dr["BillingState"].ToString().Trim());
                acroFields.SetField("CorpCountry", dr["billingCountry"].ToString().Trim());
                acroFields.SetField("CorpZip", dr["BillingZipCode"].ToString().Trim());
            }

            acroFields.SetField("ContactName", dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim());
            acroFields.SetField("ContactEmail", dr["DBA"].ToString().Trim());

            acroFields.SetField("ContactPhone", "(" + dr["BusinessPhonePrefix"].ToString().Trim() + ") " + dr["BusinessPhonePostfix"].ToString().Trim());
            acroFields.SetField("ContactFax", "(" + dr["FaxPhonePrefix"].ToString().Trim() + ") " + dr["FaxPhonePostfix"].ToString().Trim());
            acroFields.SetField("CSPhone", "(" + dr["CustServPhonePrefix"].ToString().Trim() + ") " + dr["CustServPhonePostfix"].ToString().Trim());

            acroFields.SetField("TaxNum", dr["FederalTaxID"].ToString().Trim());
            acroFields.SetField("BusinessName", dr["DBA"].ToString().Trim());
            acroFields.SetField("BusinessPhone", "(" + dr["BusinessPhonePrefix"].ToString().Trim() + ") " + dr["BusinessPhonePostfix"].ToString().Trim());
            acroFields.SetField("BusinessURL", dr["Website"].ToString().Trim());
            
            acroFields.SetField("MonthlyVolume", dr["MonthlyVolume"].ToString().Trim());
            acroFields.SetField("AverageTicket", dr["AverageTicket"].ToString().Trim());
            acroFields.SetField("HighTicket", dr["MaxTicket"].ToString().Trim());
            acroFields.SetField("LeaveProcessor1", dr["ReasonLeavingProc"].ToString().Trim());
            acroFields.SetField("ProductDescription1", dr["ProductSold"].ToString().Trim());
            acroFields.SetField("YearsBusiness", dr["YIB"].ToString().Trim());
            
            if (dr["LegalStatus"].ToString().Trim() == "Corporation")
                acroFields.SetField("Ownership", "1");
            if (dr["LegalStatus"].ToString().Trim() == "Sole Proprietorship")
                acroFields.SetField("Ownership", "2");
            if (dr["LegalStatus"].ToString().Trim() == "Partnership")
                acroFields.SetField("Ownership", "3");
            //if (dr["LegalStatus"].ToString().Trim() == "Publicly Traded")
            //  acroFields.SetField("Ownership", "4");
            if (dr["LegalStatus"].ToString().Trim() == "Government")
                acroFields.SetField("Ownership", "5");
            if (dr["LegalStatus"].ToString().Trim() == "Non-Profit")
                acroFields.SetField("Ownership", "6");
            if (dr["LegalStatus"].ToString().Trim() == "LLC")
                acroFields.SetField("Ownership", "7");
            
            #endregion
            
            #region Principal info
            //Principal 1
            acroFields.SetField("PrincipalFirst1", dr["P1FirstName"].ToString().Trim());
            acroFields.SetField("PrincipalMiddle1", dr["P1MName"].ToString().Trim()); 
            acroFields.SetField("PrincipalLast1", dr["P1LastName"].ToString().Trim());
            acroFields.SetField("PrincipalOwnership1", dr["P1OwnershipPercent"].ToString().Trim());
            acroFields.SetField("PrincipalSSN1", dr["P1SSN"].ToString().Trim());
            acroFields.SetField("PrincipalLicense1", dr["P1DriversLicenseNo"].ToString().Trim());
            acroFields.SetField("PrincipalTitle1", dr["P1Title"].ToString().Trim());
            acroFields.SetField("PrincipalDOB1", dr["P1DOB"].ToString().Trim());

            acroFields.SetField("PrincipalAddress1", dr["P1Address"].ToString().Trim());
            acroFields.SetField("PrincipalCity1", dr["P1City"].ToString().Trim());
            acroFields.SetField("PrincipalState1", dr["P1State"].ToString().Trim());
            acroFields.SetField("PrincipalCountry1", dr["P1Country"].ToString().Trim());
            acroFields.SetField("PrincipalZip1", dr["P1ZipCode"].ToString().Trim());

            acroFields.SetField("PrincipalPhone1", "(" + dr["HomePhonePrefix"].ToString().Trim() + ")" + dr["HomePhonePostfix"].ToString().Trim());
            acroFields.SetField("PrincipalEmail1", dr["Email"].ToString().Trim());
            acroFields.SetField("PrincipalCell1", "(" + dr["P1MobilePrefix"].ToString().Trim() + ")" + dr["P1MobilePostfix"].ToString().Trim());

            //Principal #2
            acroFields.SetField("PrincipalFirst2", dr["P2FirstName"].ToString().Trim());
            acroFields.SetField("PrincipalLast2", dr["P2LastName"].ToString().Trim());
            acroFields.SetField("PrincipalOwnership2", dr["P2OwnershipPercent"].ToString().Trim());
            acroFields.SetField("PrincipalSSN2", dr["P2SSN"].ToString().Trim());
            acroFields.SetField("PrincipalLicense2", dr["P2DriversLicenseNo"].ToString().Trim());
            acroFields.SetField("PrincipalTitle2", dr["P2Title"].ToString().Trim());
            acroFields.SetField("PrincipalDOB2", dr["P2DOB"].ToString().Trim());

            acroFields.SetField("PrincipalAddress2", dr["p2Address"].ToString().Trim());
            acroFields.SetField("PrincipalCity2", dr["P2City"].ToString().Trim());
            acroFields.SetField("PrincipalState2", dr["P2State"].ToString().Trim());
            acroFields.SetField("PrincipalCountry2", dr["P2Country"].ToString().Trim());
            acroFields.SetField("PrincipalZip2", dr["P2ZipCode"].ToString().Trim());                      

            #endregion

            #region CMTF AND CardPCT
            //CMTF
            if (dr["CTMF"].ToString().Trim() == "Yes")
                acroFields.SetField("Button Terminated", "2");
            else
                acroFields.SetField("Button Terminated", "1");

            //CardPCT 
            acroFields.SetField("CardPercentage", dr["PctSwp"].ToString().Trim());
            double MOTOPercent = Convert.ToDouble(dr["BusinessPctMail"]) + Convert.ToDouble(dr["BusinessPctPhone"]);
            acroFields.SetField("MotoPercentage", MOTOPercent.ToString());
            acroFields.SetField("InternetPercentage", dr["BusinessPctInternet"].ToString().Trim());
            acroFields.SetField("GatewayName", dr["Gateway"].ToString().Trim());

            #endregion

            #region Merchant Application Acceptance
            acroFields.SetField("PrincipalName1", dr["P1FirstName"].ToString().Trim() + " " + dr["P1LastName"].ToString().Trim());
            acroFields.SetField("MerchantTitle1", dr["P1Title"].ToString().Trim());
            acroFields.SetField("PrincipalName2", dr["P2FirstName"].ToString().Trim() + " " + dr["P2LastName"].ToString().Trim());
            acroFields.SetField("MerchantTitle2", dr["P2Title"].ToString().Trim());

            acroFields.SetField("GuarantorName", dr["P1FirstName"].ToString().Trim() + " " + dr["P1LastName"].ToString().Trim());
            acroFields.SetField("SpecificName", dr["P1FirstName"].ToString().Trim() + " " + dr["P1LastName"].ToString().Trim());

            #endregion

            #region Schedule A - Pricing/Rates
            //Rates
            acroFields.SetField("BlendRateUSD", dr["DiscQNP"].ToString().Trim());
            acroFields.SetField("TxnFeeUSD", dr["TransactionFee"].ToString().Trim());
            acroFields.SetField("ApplicationUSD", dr["AppFee"].ToString().Trim());
            acroFields.SetField("MaintenanceUSD", dr["CustServFee"].ToString().Trim());
            acroFields.SetField("MinimumUSD", dr["MonMin"].ToString().Trim());
            acroFields.SetField("GatewayUSD", dr["GatewayMonFee"].ToString().Trim());
            acroFields.SetField("ChargebackUSD", dr["ChargebackFee"].ToString().Trim());
            acroFields.SetField("WireUSD", dr["BatchHeader"].ToString().Trim());
            acroFields.SetField("AVSUSD", dr["AVS"].ToString().Trim());
            acroFields.SetField("GatewayTxnUSD", dr["GatewayTransFee"].ToString().Trim());
            acroFields.SetField("MembershipUSD", dr["AnnualFee"].ToString().Trim());
            acroFields.SetField("ReserveRate", dr["RollingReserve"].ToString().Trim());

            #endregion

            #region Schedule B - Auhtorization For Funds Tranfer
            //Beneficiary Info
            acroFields.SetField("BenInfoName", dr["COMPANYNAME"].ToString().Trim());
            acroFields.SetField("BenInfoAddress", dr["Address1"].ToString().Trim() + ", " + dr["Address2"].ToString().Trim());
            acroFields.SetField("BenInfoCity", dr["CITY"].ToString().Trim());
            acroFields.SetField("BenInfoProv", dr["STATE"].ToString().Trim());
            acroFields.SetField("BenInfoCountry", dr["Country"].ToString().Trim());
            acroFields.SetField("BenInfoAccount", dr["CheckingAcctNum"].ToString().Trim());

            //Beneficiary Bank
            acroFields.SetField("BenBankName", dr["BankName"].ToString().Trim());
            acroFields.SetField("BenBankAddress", dr["BankAddr"].ToString().Trim());
            acroFields.SetField("BankCity", dr["BankCity"].ToString().Trim());
            acroFields.SetField("BankProv", dr["BankState"].ToString().Trim());
            //acroFields.SetField("BenBankCountry", dr["Country"].ToString().Trim());
            acroFields.SetField("BenBankAba", dr["RoutingNum"].ToString().Trim());

            #endregion

            #region Questionnaire
            //acroFields.SetField("SalesMail", dt[0].BusinessPctMail.ToString());
            //acroFields.SetField("SalesRetail", dt[0].BusinessPctRetail.ToString());
            //acroFields.SetField("Trade Show", dt[0].BusinessPctTradeShows.ToString().ToString());
            //acroFields.SetField("SalesNet", dt[0].BusinessPctInternet.ToString().ToString());
            //acroFields.SetField("SalesPhone", dt[0].BusinessPctPhone.ToString());

            acroFields.SetField("ProdDescription1", dr["ProductSold"].ToString().Trim());

            //acroFields.SetField("Physical Address", dt[0].Address1 + " " + dt[0].Address2);
            //acroFields.SetField("Physical City", dt[0].CITY);
            //acroFields.SetField("PhysicalProv", dt[0].STATE + " / " + dt[0].ZipCode);

            if (dr["RefundPolicy"].ToString().Trim().ToLower().Contains("within 30 days"))
                acroFields.SetField("ReturnPolicy", "2");
            else if (dr["RefundPolicy"].ToString().Trim().ToLower().Contains("no refund"))
                acroFields.SetField("ReturnPolicy", "3");
            else if (dr["RefundPolicy"].ToString().Trim().ToLower().Contains("other"))
            {
                acroFields.SetField("ReturnPolicy", "4");
                acroFields.SetField("SpecifyPolicy", dr["OtherRefund"].ToString().Trim());
            }
            else
            {
                acroFields.SetField("ReturnPolicy", "4");
                acroFields.SetField("SpecifyPolicy", dr["RefundPolicy"].ToString().Trim());
            }

            int numDaysDelivered = Convert.ToInt16(dr["NumDaysDel"]);
            acroFields.SetField("Turnaround", numDaysDelivered.ToString());

            #endregion
            
            stamper.FormFlattening = true;
            stamper.Close();
            DisplayMessage("Optimal St. Kitts PDF created in the customer folder - " + FilePath);
            return 0;
        }//end if count not 0
        else
            DisplayMessage("St. Kitts Data not found for this record");
        return 1;
    }//end function CreateInternationalPDF

    #endregion

    #region Canada PDF
    public bool CreateCanadaPDF(string ContactID)
    {
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetCAPDFFromACT(ContactID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/Optimal_Canada_App.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dr["P1FirstName"].ToString().Trim();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                string P1LastName = dr["P1LastName"].ToString().Trim();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "Optimal_Canada_" + P1FirstName + " " + P1LastName + ".pdf");
                //strPath = Server.MapPath("testCan.pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);


            AcroFields acroFields = stamper.AcroFields;

            #region General Information
            acroFields.SetField("LEGNOB", dr["CompanyName"].ToString().Trim());
            acroFields.SetField("DBADBA", dr["DBA"].ToString().Trim());
            acroFields.SetField("WHADWYLT", dr["DBA"].ToString().Trim());
            if (dr["CompanyName"].ToString().Trim() == dr["DBA"].ToString().Trim())
                acroFields.SetField("SAMADOP", "Yes");
            acroFields.SetField("YOUEA", dr["Email"].ToString().Trim());
            acroFields.SetField("YOUWA", dr["Website"].ToString().Trim());
            acroFields.SetField("businessType", dr["LegalStatus"].ToString().Trim());

            if (dr["Address"].ToString().Trim() == dr["BillingAddress"].ToString().Trim())
                acroFields.SetField("HEAOMA.BILAD", "Yes");
            else
            {
                acroFields.SetField("MailingAddress", dr["BillingAddress"].ToString().Trim() + " " + dr["BillingAddress2"].ToString().Trim());
                acroFields.SetField("BillingCity", dr["BillingCity"].ToString().Trim());
                acroFields.SetField("BillingState", dr["BillingState"].ToString().Trim());
                acroFields.SetField("BillingZip", dr["BillingZip"].ToString().Trim());
                acroFields.SetField("BillingCountry", dr["BillingCountry"].ToString().Trim());
            }


            acroFields.SetField("BUSANPOB", dr["Address"].ToString().Trim());
            acroFields.SetField("BUSAC", dr["City"].ToString().Trim());
            acroFields.SetField("BUSASPR", dr["State"].ToString().Trim());
            acroFields.SetField("BUSAZ", dr["ZipCode"].ToString().Trim());
            acroFields.SetField("YEIAB", dr["YIB"].ToString().Trim());

            if (dr["RefundPolicy"].ToString().Trim() != "Other")
                acroFields.SetField("refundPolicy", dr["RefundPolicy"].ToString().Trim());
            else
                acroFields.SetField("refundPolicy", dr["OtherRefund"].ToString().Trim());

            string BusPhone = dr["BusinessPhone"].ToString().Trim();
            if (BusPhone != "")
            {
                acroFields.SetField("areaCode", BusPhone.Substring(0, 3));
                acroFields.SetField("phonePrefix", BusPhone.Substring(3, 3));
                acroFields.SetField("phonePostfix", BusPhone.Substring(6, 4));
            }

            string CustServPhone = dr["CustServPhone"].ToString().Trim();
            if (CustServPhone != "")
            {
                acroFields.SetField("areaCodeCS", CustServPhone.Substring(0, 3));
                acroFields.SetField("PhonePrefixCS", CustServPhone.Substring(3, 3));
                acroFields.SetField("PhonePostfixCS", CustServPhone.Substring(6, 4));
            }

            acroFields.SetField("PROSS", dr["ProductSold"].ToString().Trim());

            acroFields.SetField("previousProcessor", dr["PrevProcessor"].ToString().Trim());
            acroFields.SetField("leavingReason", dr["ReasonForLeaving"].ToString().Trim());
            #endregion

            #region Principal #1
            //Principal #1
            acroFields.SetField("1OWNPOFN", dr["P1FullName"].ToString().Trim());
            acroFields.SetField("1OWNZI", dr["P1ZipCode"].ToString().Trim());
            acroFields.SetField("1OWNST", dr["P1State"].ToString().Trim());
            acroFields.SetField("1OWNCI", dr["P1City"].ToString().Trim());
            acroFields.SetField("1OWNHA", dr["P1Address"].ToString().Trim());
            acroFields.SetField("1OWNTI", dr["P1Title"].ToString().Trim());
            acroFields.SetField("1OWNSSN", dr["P1SSN"].ToString().Trim());
            if (dr["P1Title"].ToString().Trim() == "Canada")
                acroFields.SetField("1OWNPCOR.CANAD", "Yes");
            else
                acroFields.SetField("1OWNPCOR.OTHER", "Yes");

            if (dr["CTMF"].ToString().Trim() == "Yes")
                acroFields.SetField("OWNPOHYE", "Yes");
            else if (dr["CTMF"].ToString().Trim() == "No")
                acroFields.SetField("OWNPOHYE", "NOQ");

            if (dr["Bankruptcy"].ToString().Trim() == "Yes")
                acroFields.SetField("filedBankruptcy", "Yes");
            else if (dr["CTMF"].ToString().Trim() == "No")
                acroFields.SetField("filedBankruptcy", "No");

            acroFields.SetField("1OWNPRISO", dr["P1OwnershipPercent"].ToString().Trim());
            acroFields.SetField("1OWNDOBMD", dr["P1DOB"].ToString().Trim());

            string P1Phone = dr["P1PhoneNumber"].ToString().Trim();
            if (P1Phone != "")
            {
                acroFields.SetField("ownerAreaCode", P1Phone.Substring(0, 3));
                acroFields.SetField("ownerPhonePrefix", P1Phone.Substring(3, 3));
                acroFields.SetField("ownerPhonePostfix", P1Phone.Substring(6, 4));
            }


            string P2Phone = dr["P2PhoneNumber"].ToString().Trim();
            if (P2Phone != "")
            {
                acroFields.SetField("partnerAreaCode", P2Phone.Substring(0, 3));
                acroFields.SetField("partnerPhonePrefix", P2Phone.Substring(3, 3));
                acroFields.SetField("partnerPhoneSuffix", P2Phone.Substring(6, 4));
            }

            #endregion

            #region Principal #2
            //Principal #2
            acroFields.SetField("2OWNZI", dr["P2ZipCode"].ToString().Trim());
            acroFields.SetField("2OWNST", dr["P2State"].ToString().Trim());
            acroFields.SetField("2OWNCI", dr["P2City"].ToString().Trim());
            acroFields.SetField("2OWNHA", dr["P2Address"].ToString().Trim());
            if (dr["P2Country"].ToString().Trim() == "Canada")
                acroFields.SetField("2OWNPCOR.CANAD", "Yes");
            else if (dr["P2Country"].ToString().Trim() != "")
                acroFields.SetField("2OWNPCOR.OTHER", "Yes");
            acroFields.SetField("2OWNTI", dr["P2Title"].ToString().Trim());
            acroFields.SetField("2OWNSSN", dr["P2SSN"].ToString().Trim());
            acroFields.SetField("2OWNPOFN", dr["P2FullName"].ToString().Trim());
            acroFields.SetField("2OWNPRISO", dr["P2OwnershipPercent"].ToString().Trim());
            acroFields.SetField("2OWNDOBMD", dr["P2DOB"].ToString().Trim());

            acroFields.SetField("2OWNHP", dr["P2PhoneNumber"].ToString().Trim());
            #endregion

            #region Rates
            acroFields.SetField("pctRetail", dr["BusinessPctRetail"].ToString().Trim());
            acroFields.SetField("pctInternet", dr["BusinessPctInternet"].ToString().Trim());
            acroFields.SetField("pctMail", dr["BusinessPctMailOrder"].ToString().Trim());

            //Rates

            if (dr["DiscQNP"].ToString().Trim() != "")
            {
                acroFields.SetField("DiscRate", dr["DiscQNP"].ToString().Trim());
                acroFields.SetField("DiscRateCD", dr["DiscQNP"].ToString().Trim());
                acroFields.SetField("setupFee", "60.00");
            }
            else if (dr["DiscQP"].ToString().Trim() != "")
            {
                acroFields.SetField("DiscRate", dr["DiscQP"].ToString().Trim());
                acroFields.SetField("DiscRateCD", dr["DiscQP"].ToString().Trim());
                acroFields.SetField("setupFee", "50.00");
            }

            acroFields.SetField("averageTicketUSD", dr["AverageTicket"].ToString().Trim());
            acroFields.SetField("maximumTicketUSD", dr["MaxTicket"].ToString().Trim());
            acroFields.SetField("monthlyVolumeUSD", dr["MonthlyVolume"].ToString().Trim());
            acroFields.SetField("monthlyMdrUSD", dr["MonMin"].ToString().Trim());

            acroFields.SetField("VTransFeeUSD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("VRefundFeeUSD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("VNQDiscRateUSD", dr["DiscQNP"].ToString().Trim());
            acroFields.SetField("MCTransFeeUSD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("MCRefundFeeUSD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("MCNQDiscRateUSD", dr["DiscQNP"].ToString().Trim());
            acroFields.SetField("NBCTransFeeUSD", dr["NBCTransFee"].ToString().Trim());
            acroFields.SetField("NBCRefundFeeUSD", dr["NBCTransFee"].ToString().Trim());

            //Canadian Rates, use same info as US (for now)
            acroFields.SetField("averageTicketCD", dr["AverageTicket"].ToString().Trim());
            acroFields.SetField("maximumTicketCD", dr["MaxTicket"].ToString().Trim());
            acroFields.SetField("monthlyVolumeCD", dr["MonthlyVolume"].ToString().Trim());
            acroFields.SetField("monthlyMdrCD", dr["MonMin"].ToString().Trim());

            acroFields.SetField("VTransFeeCAD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("VRefundFeeCAD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("VNQDiscRateCAD", dr["DiscQNP"].ToString().Trim());
            acroFields.SetField("MCTransFeeCAD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("MCRefundFeeCAD", dr["TransFee"].ToString().Trim());
            acroFields.SetField("MCNQDiscRateCAD", dr["DiscQNP"].ToString().Trim());
            acroFields.SetField("NBCTransFeeCAD", dr["NBCTransFee"].ToString().Trim());
            acroFields.SetField("NBCRefundFeeCAD", dr["NBCTransFee"].ToString().Trim());

            acroFields.SetField("custServFee", dr["CustServFee"].ToString().Trim());
            acroFields.SetField("chargebackFee", dr["ChargebackFee"].ToString().Trim());
            if (Convert.ToInt32(dr["BankRoutingNumber"].ToString().Trim().Substring(0, 0)) == 0)
            {
                acroFields.SetField("institutioinCND", dr["BankRoutingNumber"].ToString().Trim().Substring(1, 3));
            }
            else
            {
                acroFields.SetField("institutioinCND", dr["BankRoutingNumber"].ToString().Trim().Substring(0, 3));
            }
            acroFields.SetField("transitNumberCD", dr["BankRoutingNumber"].ToString().Trim());
            acroFields.SetField("accountNumberCD", dr["BankAccountNumber"].ToString().Trim());
            acroFields.SetField("americanExpress", dr["AmexApplied"].ToString().Trim());

            acroFields.SetField("merchantNumberOFI", dr["AmexNum"].ToString().Trim());

            if (Convert.ToString(dr["BankName"]).ToLower().Trim().Contains("monteal"))
            {
                acroFields.SetField("BankMontrealCan", "Yes");
            }
            else if (Convert.ToString(dr["BankName"]).ToLower().Trim().Contains("royal"))
            {
                acroFields.SetField("RoyalBankCan", "Yes");
            }
            else
            {
                acroFields.SetField("OtherBankCan", "Yes");
            }

            #endregion

            #region Questionnaire
            if (dr["RefundPolicy"].ToString().Contains("more than 30"))
                acroFields.SetField("ReturnPolicy", "1");
            else if (dr["RefundPolicy"].ToString().Contains("within 30 days"))
                acroFields.SetField("ReturnPolicy", "2");
            else if (dr["RefundPolicy"].ToString().Contains("No Refund"))
                acroFields.SetField("ReturnPolicy", "3");
            else if (dr["RefundPolicy"].ToString().Contains("Exchange Only"))
            {
                acroFields.SetField("ReturnPolicy", "4");
                acroFields.SetField("otherRefundPolicy", "Exchange Only");
            }
            else
            {
                acroFields.SetField("ReturnPolicy", "4");
                acroFields.SetField("otherRefundPolicy", dr["OtherRefund"].ToString().Trim());
            }
            acroFields.SetField("ProductsSold", dr["ProductSold"].ToString().Trim());

            #endregion

            stamper.FormFlattening = true;
            //stamper.Writer.CloseStream = false;
            stamper.Close();

            DisplayMessage("Optimal Canada PDF created in customer folder - " + FilePath);


            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Optimal Canada PDF could not be created. ");
            return false;
        }
    }//end function CreateCanadaPDF
    #endregion

    #region Northern Lease PDF

    public void btnNorthernLeasePDF_Click(object sender, EventArgs e)
    {
        try
        {
            CreateLeasePDF(selContactID);
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }


    //This function creates Lease PDF
    public bool CreateLeasePDF(string ContactID)
    {
        //Get data for Lease Application
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetLeasePDFACT(ContactID);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader;
            //Create PDFReader object by passing in the name of PDF to populate
            if (dr["State"].ToString().Trim().Contains("SD") || dr["State"].ToString().Trim().Contains("KS") ||
                dr["State"].ToString().Trim().Contains("TN") || dr["State"].ToString().Trim().Contains("PA") ||
                dr["State"].ToString().Trim().Contains("VT"))
            {
                reader = new PdfReader(Server.MapPath("../PDF/Northern Leasing Agreement - SD, KS, TN, PA & VT.pdf"));
            }
            else
                reader = new PdfReader(Server.MapPath("../PDF/Northern Leasing Agreement - Standard.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1FirstName = dr["P1FirstName"].ToString();
                string P1LastName = dr["P1LastName"].ToString();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1FirstName == "")
                    P1FirstName = "CTC";

                if (P1LastName == "")
                    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "NorthernLease_" + P1FirstName.Substring(0, 1) + P1LastName + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);

            AcroFields acroFields = stamper.AcroFields;
            
            acroFields.SetField("LeaseNumber", dr["LeaseNum"].ToString().Trim());

            string strP1HomePhone = dr["P1AreaCode"].ToString().Trim() + "-" + dr["P1HomePhone"].ToString().Trim();

            int curMon = DateTime.Now.Month;
            int curYear = DateTime.Now.Year;
            int yearStart = 0;
            int monStart = 1;
            if (curMon - Convert.ToInt32(dr["StartMonth"]) >= 0)
            {
                monStart = curMon - Convert.ToInt32(dr["StartMonth"]);
                yearStart = curYear - Convert.ToInt32(dr["StartYear"]);
            }
            else
            {
                monStart = 12 + curMon - Convert.ToInt32(dr["StartMonth"]);
                yearStart = curYear - Convert.ToInt32(dr["StartYear"]) - 1;
            }

            string strBusStartDate = monStart.ToString().Trim() + "/" + yearStart.ToString().Trim();

            #region About Your Business
            acroFields.SetField("CompanyName", dr["CompanyName"].ToString().Trim());
            acroFields.SetField("DBA", dr["DBA"].ToString().Trim());
            acroFields.SetField("BillingAddress", dr["MailingAddress"].ToString().Trim());
            acroFields.SetField("City", dr["MACity"].ToString().Trim());
            acroFields.SetField("State", dr["MAState"].ToString().Trim());
            acroFields.SetField("Zip", dr["MAZip"].ToString().Trim());
            acroFields.SetField("BusinessAreaCode", dr["AreaCode"].ToString().Trim());
            acroFields.SetField("BusinessPhone", dr["Phone"].ToString().Trim());
            string Phone = "(" + dr["AreaCode"].ToString().Trim() + ") " + dr["Phone"].ToString().Trim();
            acroFields.SetField("Phone", Phone);
            acroFields.SetField("TypeOfBusiness", dr["ProductSold"].ToString().Trim());
            if ((dr["LegalStatus"].ToString().ToLower().Trim().Contains("corporation") || dr["LegalStatus"].ToString().ToLower().Trim().Contains("llc")))
                acroFields.SetField("Corporation", "Yes");
            else if (dr["LegalStatus"].ToString().ToLower().Trim().Contains("proprietorship"))
                acroFields.SetField("Proprietorship", "Yes");
            else if (dr["LegalStatus"].ToString().ToLower().Trim().Contains("partnership"))
                acroFields.SetField("Partnership", "Yes");
            acroFields.SetField("Email", dr["Email"].ToString().Trim());
            acroFields.SetField("YearsInBusiness", dr["YIB"].ToString().Trim());
            acroFields.SetField("BusinessAddress", dr["BusinessAddress"].ToString().Trim());
            acroFields.SetField("BusinessAddressCity", dr["City"].ToString().Trim());
            acroFields.SetField("BusinessAddressState", dr["State"].ToString().Trim());
            acroFields.SetField("BusinessAddressZip", dr["Zip"].ToString().Trim());
            acroFields.SetField("BusStartDate", strBusStartDate);
            acroFields.SetField("P1Name", dr["FULLNAME"].ToString().Trim());
            acroFields.SetField("P1Title", dr["Title"].ToString().Trim());
            acroFields.SetField("P1Address", dr["P1Address"].ToString().Trim());
            acroFields.SetField("P1City", dr["P1City"].ToString().Trim());
            acroFields.SetField("P1State", dr["P1State"].ToString().Trim());
            acroFields.SetField("P1Zip", dr["P1Zip"].ToString().Trim());
            acroFields.SetField("P1AreaCode", dr["P1AreaCode"].ToString().Trim());
            acroFields.SetField("P1Phone", strP1HomePhone);
            acroFields.SetField("P1SSN", dr["P1SSN"].ToString().Trim());
            #endregion

            #region Equipment Information
            acroFields.SetField("Equipment", dr["Equipment"].ToString().Trim());
            #endregion

            #region Payment Info
            acroFields.SetField("MonthlyPayment", dr["LeasePayment"].ToString().Trim());
            acroFields.SetField("LeaseTerm", dr["LeaseTerm"].ToString().Trim());
            #endregion

            #region Bank Info
            acroFields.SetField("BankName", dr["BankName"].ToString().Trim());
            acroFields.SetField("RoutingNumber", dr["RoutingNum"].ToString().Trim());
            acroFields.SetField("AccountNumber", dr["CheckingAcctNum"].ToString().Trim());
            #endregion

            stamper.FormFlattening = true;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("Northern Lease Data not found for this record.");
            return false;
        }
    }//end function CreateLeasePDF
    #endregion

    #region Sage Payment Solutions EFT GiftCard PDF
    public void btnGETIGiftCardPDF_Click(object sender, EventArgs e)
    {
        try
        {
            CreateGETIPDF(selContactID);
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    //This function creates GETI PDF
    public bool CreateGETIPDF(string ContactID)
    {
        //Get data for Lease Application
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetGETIPDFACT(ContactID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/GETI_Gift_Merchant_App.pdf"));
            
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1Name = dr["P1Name"].ToString();
                //string P1LastName = dr["P1LastName"].ToString();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1Name == "")
                    P1Name = "CTC";

                //if (P1LastName == "")
                //    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "GETIGiftCard_" + P1Name + ".pdf");
            }
            
            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);
            
            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
                       

            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Sales Agent", dr["SalesRep"].ToString().Trim());

            #region Location Information
            acroFields.SetField("Legal Name", dr["COMPANYNAME"].ToString().Trim());
            acroFields.SetField("DBA Name", dr["DBA"].ToString().Trim());
            acroFields.SetField("Phone", dr["Phone"].ToString().Trim());
            acroFields.SetField("Fax", dr["Fax"].ToString().Trim());
            acroFields.SetField("DBA Address", dr["BusinessAddress"].ToString().Trim());
            acroFields.SetField("City", dr["CITY"].ToString().Trim());
            acroFields.SetField("State", dr["STATE"].ToString().Trim());
            acroFields.SetField("Zip", dr["Zip"].ToString().Trim());
            acroFields.SetField("Mail Address", dr["MailingAddress"].ToString().Trim());
            acroFields.SetField("Mail City", dr["MACity"].ToString().Trim());
            acroFields.SetField("Mail State", dr["MAState"].ToString().Trim());
            acroFields.SetField("Mail Zip", dr["MAZip"].ToString().Trim());
            #endregion

            #region Principal Information
            acroFields.SetField("Name Print", dr["P1Name"].ToString().Trim());
            acroFields.SetField("Title", dr["Title"].ToString().Trim());
            acroFields.SetField("Ownership", dr["P1Ownership"].ToString().Trim());
            acroFields.SetField("Email", dr["Email"].ToString().Trim());
            acroFields.SetField("Social Security", dr["P1SSN"].ToString().Trim());
            acroFields.SetField("Phone_2", dr["P1Phone"].ToString().Trim());
            acroFields.SetField("Principal Address", dr["P1Address"].ToString().Trim());
            acroFields.SetField("Principal City", dr["P1City"].ToString().Trim());
            acroFields.SetField("Principal State", dr["P1State"].ToString().Trim());
            acroFields.SetField("Principal Zip", dr["P1Zip"].ToString().Trim());
            #endregion

            #region Fees
            acroFields.SetField("Service Fee", dr["GCMonFee"].ToString().Trim());
            acroFields.SetField("Transaction Fee", dr["GCTransFee"].ToString().Trim());
            #endregion

            stamper.FormFlattening = true;
            //stamper.Writer.CloseStream = false;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("GETI GiftCard Data not found for this record.");
            return false;
        }
    }//end function CreateGETIPDF
    #endregion

    #region AdvanceMe, Inc. Merchant Cash Advance PDF
    public void btnAMIPDF_Click(object sender, EventArgs e)
    {
        try
        {
            CreateAMIPDF(selContactID);
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    public void btnRapidAdvancePDF_Click(object sender, EventArgs e)
    {
        try
        {
            CreateRapidAdvancePDF(selContactID);
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    public void btnBFSPDF_Click(object sender, EventArgs e)
    {
        try
        {
            CreateBFSPDF(selContactID);
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }



    //This function creates AdvanceMe PDF
    public bool CreateAMIPDF(string ContactID)
    {
        //Get data for Lease Application
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetAMIIPDFACT(ContactID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/CAN Pre_Qual_Form.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1Name = dr["P1Name"].ToString();
                //string P1LastName = dr["P1LastName"].ToString();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1Name == "")
                    P1Name = "CTC";

                //if (P1LastName == "")
                //    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "AdvanceMe_PreQual_" + P1Name + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);

            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);


            AcroFields acroFields = stamper.AcroFields;

            #region Form fields
            acroFields.SetField("txtLegalName", dr["COMPANYNAME"].ToString().Trim());
            acroFields.SetField("txtDBA", dr["DBA"].ToString().Trim());

            if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("corporation"))
                acroFields.SetField("Type of Business", "Corp");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("llc"))
                acroFields.SetField("Type of Business", "LLC");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("ltd partnership"))
                acroFields.SetField("Type of Business", "LimitedPartnership");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("partnership"))
                acroFields.SetField("Type of Business", "Partnership");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("sole proprietor"))
                acroFields.SetField("Type of Business", "SoleProp");

            acroFields.SetField("txtPhysicalAddress", dr["BusinessAddress"].ToString().Trim());
            acroFields.SetField("txtPhysicalCity", dr["CITY"].ToString().Trim());
            acroFields.SetField("txtPhysicalState", dr["STATE"].ToString().Trim());
            acroFields.SetField("txtPhysicalZipCode", dr["Zip"].ToString().Trim());
            acroFields.SetField("txtBillingAddress", dr["MailingAddress"].ToString().Trim());
            acroFields.SetField("txtBillingCity", dr["MACity"].ToString().Trim());
            acroFields.SetField("txtBillingState", dr["MAState"].ToString().Trim());
            acroFields.SetField("txtBillingZipCode", dr["MAZip"].ToString().Trim());

            acroFields.SetField("txtAverageMonthlyCCVolume", dr["MonthlyVolume"].ToString().Trim());

            acroFields.SetField("Owner/Officer", dr["P1Name"].ToString().Trim());
            acroFields.SetField("txtJobTitle", dr["Title"].ToString().Trim());

            acroFields.SetField("txtOwnerLastName", dr["P1LastName"].ToString().Trim());
            acroFields.SetField("txtOwnerFirstName", dr["P1FirstName"].ToString().Trim());
            acroFields.SetField("txtSocialSecurityNbr", dr["P1SSN"].ToString().Trim());
            acroFields.SetField("txtDateOfBirth", dr["P1DOB"].ToString().Trim());
            acroFields.SetField("txtHomePhoneNbr", dr["P1Phone"].ToString().Trim());

            acroFields.SetField("txtPhysicalPhoneNbr", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("txtBillingPhoneNbr", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("txtOwnerAddress", dr["P1Address"].ToString().Trim());
            acroFields.SetField("txtOwnerCity", dr["P1City"].ToString().Trim());
            acroFields.SetField("txtOwnerState", dr["P1State"].ToString().Trim());
            acroFields.SetField("txtOwnerZipCode", dr["P1Zip"].ToString().Trim());

            acroFields.SetField("RepName", dr["SalesRep"].ToString().Trim());

            acroFields.SetField("Email", dr["Email"].ToString().Trim());

            #endregion

            stamper.FormFlattening = true;
            //stamper.Writer.CloseStream = false;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("AdvanceMe Data not found for this record.");
            return false;
        }
    }//end function CreateAMIPDF

    public bool CreateRapidAdvancePDF(string ContactID)
    {
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetAMIIPDFACT(ContactID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/CAN Pre_Qual_Form.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1Name = dr["P1Name"].ToString();
                //string P1LastName = dr["P1LastName"].ToString();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1Name == "")
                    P1Name = "CTC";

                //if (P1LastName == "")
                //    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "AdvanceMe_PreQual_" + P1Name + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);

            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);


            AcroFields acroFields = stamper.AcroFields;

            #region Form fields
            acroFields.SetField("txtLegalName", dr["COMPANYNAME"].ToString().Trim());
            acroFields.SetField("txtDBA", dr["DBA"].ToString().Trim());

            if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("corporation"))
                acroFields.SetField("Type of Business", "Corp");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("llc"))
                acroFields.SetField("Type of Business", "LLC");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("ltd partnership"))
                acroFields.SetField("Type of Business", "LimitedPartnership");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("partnership"))
                acroFields.SetField("Type of Business", "Partnership");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("sole proprietor"))
                acroFields.SetField("Type of Business", "SoleProp");

            acroFields.SetField("txtPhysicalAddress", dr["BusinessAddress"].ToString().Trim());
            acroFields.SetField("txtPhysicalCity", dr["CITY"].ToString().Trim());
            acroFields.SetField("txtPhysicalState", dr["STATE"].ToString().Trim());
            acroFields.SetField("txtPhysicalZipCode", dr["Zip"].ToString().Trim());
            acroFields.SetField("txtBillingAddress", dr["MailingAddress"].ToString().Trim());
            acroFields.SetField("txtBillingCity", dr["MACity"].ToString().Trim());
            acroFields.SetField("txtBillingState", dr["MAState"].ToString().Trim());
            acroFields.SetField("txtBillingZipCode", dr["MAZip"].ToString().Trim());

            acroFields.SetField("txtAverageMonthlyCCVolume", dr["MonthlyVolume"].ToString().Trim());

            acroFields.SetField("Owner/Officer", dr["P1Name"].ToString().Trim());
            acroFields.SetField("txtJobTitle", dr["Title"].ToString().Trim());

            acroFields.SetField("txtOwnerLastName", dr["P1LastName"].ToString().Trim());
            acroFields.SetField("txtOwnerFirstName", dr["P1FirstName"].ToString().Trim());
            acroFields.SetField("txtSocialSecurityNbr", dr["P1SSN"].ToString().Trim());
            acroFields.SetField("txtDateOfBirth", dr["P1DOB"].ToString().Trim());
            acroFields.SetField("txtHomePhoneNbr", dr["P1Phone"].ToString().Trim());

            acroFields.SetField("txtPhysicalPhoneNbr", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("txtBillingPhoneNbr", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("txtOwnerAddress", dr["P1Address"].ToString().Trim());
            acroFields.SetField("txtOwnerCity", dr["P1City"].ToString().Trim());
            acroFields.SetField("txtOwnerState", dr["P1State"].ToString().Trim());
            acroFields.SetField("txtOwnerZipCode", dr["P1Zip"].ToString().Trim());

            acroFields.SetField("RepName", dr["SalesRep"].ToString().Trim());

            acroFields.SetField("Email", dr["Email"].ToString().Trim());

            #endregion

            stamper.FormFlattening = true;
            //stamper.Writer.CloseStream = false;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("AdvanceMe Data not found for this record.");
            return false;
        }
    }

    public bool CreateBFSPDF(string ContactID)
    {
        //Get data for Lease Application
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetAMIIPDFACT(ContactID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/CAN Pre_Qual_Form.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1Name = dr["P1Name"].ToString();
                //string P1LastName = dr["P1LastName"].ToString();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1Name == "")
                    P1Name = "CTC";

                //if (P1LastName == "")
                //    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "AdvanceMe_PreQual_" + P1Name + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);

            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);


            AcroFields acroFields = stamper.AcroFields;

            #region Form fields
            acroFields.SetField("txtLegalName", dr["COMPANYNAME"].ToString().Trim());
            acroFields.SetField("txtDBA", dr["DBA"].ToString().Trim());

            if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("corporation"))
                acroFields.SetField("Type of Business", "Corp");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("llc"))
                acroFields.SetField("Type of Business", "LLC");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("ltd partnership"))
                acroFields.SetField("Type of Business", "LimitedPartnership");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("partnership"))
                acroFields.SetField("Type of Business", "Partnership");
            else if (dr["LegalStatus"].ToString().Trim().ToLower().Contains("sole proprietor"))
                acroFields.SetField("Type of Business", "SoleProp");

            acroFields.SetField("txtPhysicalAddress", dr["BusinessAddress"].ToString().Trim());
            acroFields.SetField("txtPhysicalCity", dr["CITY"].ToString().Trim());
            acroFields.SetField("txtPhysicalState", dr["STATE"].ToString().Trim());
            acroFields.SetField("txtPhysicalZipCode", dr["Zip"].ToString().Trim());
            acroFields.SetField("txtBillingAddress", dr["MailingAddress"].ToString().Trim());
            acroFields.SetField("txtBillingCity", dr["MACity"].ToString().Trim());
            acroFields.SetField("txtBillingState", dr["MAState"].ToString().Trim());
            acroFields.SetField("txtBillingZipCode", dr["MAZip"].ToString().Trim());

            acroFields.SetField("txtAverageMonthlyCCVolume", dr["MonthlyVolume"].ToString().Trim());

            acroFields.SetField("Owner/Officer", dr["P1Name"].ToString().Trim());
            acroFields.SetField("txtJobTitle", dr["Title"].ToString().Trim());

            acroFields.SetField("txtOwnerLastName", dr["P1LastName"].ToString().Trim());
            acroFields.SetField("txtOwnerFirstName", dr["P1FirstName"].ToString().Trim());
            acroFields.SetField("txtSocialSecurityNbr", dr["P1SSN"].ToString().Trim());
            acroFields.SetField("txtDateOfBirth", dr["P1DOB"].ToString().Trim());
            acroFields.SetField("txtHomePhoneNbr", dr["P1Phone"].ToString().Trim());

            acroFields.SetField("txtPhysicalPhoneNbr", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("txtBillingPhoneNbr", dr["BusinessPhone"].ToString().Trim());
            acroFields.SetField("txtOwnerAddress", dr["P1Address"].ToString().Trim());
            acroFields.SetField("txtOwnerCity", dr["P1City"].ToString().Trim());
            acroFields.SetField("txtOwnerState", dr["P1State"].ToString().Trim());
            acroFields.SetField("txtOwnerZipCode", dr["P1Zip"].ToString().Trim());

            acroFields.SetField("RepName", dr["SalesRep"].ToString().Trim());

            acroFields.SetField("Email", dr["Email"].ToString().Trim());

            #endregion

            stamper.FormFlattening = true;
            //stamper.Writer.CloseStream = false;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("AdvanceMe Data not found for this record.");
            return false;
        }
    }

#endregion

    #region ROAMpay PDF
    public void btnRoamPayPDF_Click(object sender, EventArgs e)
    {
        try
        {
            CreateRoamPayPDF(selContactID);
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    //This function creates RoamPay PDF
    public bool CreateRoamPayPDF(string ContactID)
    {
        //Get data for Lease Application
        PDFBL PDF = new PDFBL();
        DataSet ds = PDF.GetRoamPayPDFACT(ContactID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            PdfReader reader = new PdfReader(Server.MapPath("../PDF/ROAMpay Application.pdf"));

            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(ContactID);//Get the customer path from ACT
            string strPath = "";
            if (FilePath != string.Empty)
            {
                FilePath = FilePath.ToLower();
                FilePath = FilePath.Replace("file://s:\\customers", "");
                FilePath = FilePath.Replace("\\", "/");

                string strHost = "../../Customers";
                string P1Name = dr["Contact"].ToString();
                //string P1LastName = dr["P1LastName"].ToString();
                //if the Principal's Name is empty, initalize to ECE Merchant
                if (P1Name == "")
                    P1Name = "CTC";

                //if (P1LastName == "")
                //    P1LastName = "Merchant";

                strPath = Server.MapPath(strHost + FilePath + "/" + "ROAMpay_" + P1Name + ".pdf");
            }

            FileStream fStream = null;
            fStream = new FileStream(strPath, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, fStream);
            //MemoryStream mStream = new MemoryStream();
            //PdfStamper stamper = new PdfStamper(reader, mStream);

            stamper.SetEncryption(PdfWriter.STRENGTH128BITS, "Succeed1", "Succeed1", PdfWriter.AllowPrinting | PdfWriter.AllowScreenReaders);
            
            AcroFields acroFields = stamper.AcroFields;

            acroFields.SetField("Agent Name", dr["SalesRep"].ToString().Trim());acroFields.SetField("Agent Name", dr["SalesRep"].ToString().Trim());
            acroFields.SetField("Rep Number", dr["RepNum"].ToString().Trim());

            #region Merchant Info
            acroFields.SetField("MID", dr["MID"].ToString().Trim());
            acroFields.SetField("DBA", dr["DBA"].ToString().Trim());
            acroFields.SetField("Contact", dr["Contact"].ToString().Trim());
            acroFields.SetField("ContactPhone1", dr["Phone1"].ToString().Trim());
            acroFields.SetField("ContactPhone2", dr["Phone2"].ToString().Trim());
            acroFields.SetField("ContactPhone3", dr["Phone3"].ToString().Trim());
            acroFields.SetField("Email", dr["Email"].ToString().Trim());

            acroFields.SetField("MonthlyAccessFee", dr["GatewayFee"].ToString().Trim());
            acroFields.SetField("TransactionFee", dr["GatewayTransFee"].ToString().Trim());
            acroFields.SetField("SetUpFee", dr["GatewaySetupFee"].ToString().Trim());

            acroFields.SetField("BankName", dr["BankName"].ToString().Trim());
            acroFields.SetField("RoutingNumber", dr["RoutingNum"].ToString().Trim());
            acroFields.SetField("AccountNumber", dr["CheckingAcctNum"].ToString().Trim());
            #endregion

            stamper.FormFlattening = true;
            //stamper.Writer.CloseStream = false;
            stamper.Close();
            //Response.OutputStream.Write(mStream.GetBuffer(), 0, mStream.GetBuffer().Length - 1);
            //Response.Flush();
            //Response.Close();

            DisplayMessage("PDF created in the customer folder - " + FilePath);
            return true;
        }//end if dataset count not 0
        else
        {
            DisplayMessage("ROAMpay Data not found for this record.");
            return false;
        }
    }//end function CreateRoamPayPDF
    #endregion

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}

