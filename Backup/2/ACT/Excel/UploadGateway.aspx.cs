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
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text;

public partial class UploadGateway : System.Web.UI.Page
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

    //This function handles grid view button click event
    protected void grdPDF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "CreateFile")
            {
                lblError.Visible = false;
                lblError.Text = "";
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdPDF.Rows[index];
                System.Guid ContactID = new Guid(Server.HtmlDecode(grdRow.Cells[1].Text));
                if (ContactID.ToString() != "")
                {
                    pnlPlatform.Visible = true;
                    lstPlatform.SelectedValue = lstPlatform.Items.FindByValue(Server.HtmlDecode(grdRow.Cells[6].Text)).Value;
                    ACTDataBL PlatformInfo = new ACTDataBL();
                    PartnerDS.ACTAuthnetPlatformDataTable dt = PlatformInfo.GetAuthnetPlatform(ContactID);
                    if (dt.Rows.Count > 0)
                    {
                        txtAgentBankNumber.Text = dt[0].AgentBankIDNumber.ToString();
                        txtAgentChainNumber.Text = dt[0].AgentChainNumber.ToString();
                        txtMCCCode.Text = dt[0].MCCCategoryCode.ToString();
                        txtLoginID.Text = "";// dt[0].LoginID.ToString();
                        txtMerchantID.Text = dt[0].MerchantID.ToString();
                        txtStoreNumber.Text = dt[0].StoreNumber.ToString();
                        txtVisaMasterNumber.Text = dt[0].MerchantNumber.ToString();
                        txtBINNumber.Text = dt[0].BankIDNumber.ToString();
                    }//end if count not 0
                    DisablePlatformFields();
                    lblContactID.Text = ContactID.ToString();
                }
            }//end if command name
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

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    public void GenerateExcelFile()
    {
        try
        {
            string strCurrentDir = Server.MapPath(".") + "\\";
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel.Workbook oWB;
            Microsoft.Office.Interop.Excel.Worksheet oSheet;

            GC.Collect();// clean up any other excel objects
            object missing = Type.Missing;
            oXL = new Microsoft.Office.Interop.Excel.Application();
            oXL.Visible = false;
            //Get a new workbook.
            //oWB = oXL.Workbooks.Open(Server.MapPath("RINT BU Template.xls"), missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing); //(Excel._Workbook)(oXL.Workbooks.Add(Server.MapPath("RINT BU Template.xls")));//System.Reflection.Missing.Value));
            oWB = oXL.Workbooks.Add(System.Reflection.Missing.Value);
            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.ActiveSheet;

            ACTDataBL ACTData = new ACTDataBL();
            PartnerDS.ACTAuthnetExcelDataTable dt = ACTData.GetAuthnetExcel(new Guid(lblContactID.Text));
            string CompanyName = string.Empty;
            if (dt.Rows.Count > 0)
            {
                //Login ID will contain the Company name
                CompanyName = dt[0].Login_ID.ToString();
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                    oSheet.Cells[1, i + 1] = dt[0][i].ToString();

                if (lstPlatform.SelectedItem.Text == "Nashville")
                {
                    oSheet.Cells[1, 50] = "";
                    oSheet.Cells[1, 51] = "";
                    oSheet.Cells[1, 52] = "";
                    oSheet.Cells[1, 53] = "";
                    oSheet.Cells[1, 54] = "";
                    oSheet.Cells[1, 55] = "";
                    oSheet.Cells[1, 56] = "";

                    oSheet.Cells[1, 50] = "'" + txtMerchantID.Text;
                    oSheet.Cells[1, 51] = "'" + txtTerminalID.Text;
                }//end if nashville
                else if (lstPlatform.SelectedItem.Text == "Vital")
                {
                    oSheet.Cells[1, 50] = "";
                    oSheet.Cells[1, 51] = "";
                    oSheet.Cells[1, 52] = "";
                    oSheet.Cells[1, 53] = "";
                    oSheet.Cells[1, 54] = "";
                    oSheet.Cells[1, 55] = "";
                    oSheet.Cells[1, 56] = "";

                    oSheet.Cells[1, 50] = "'" + txtBINNumber.Text;
                    oSheet.Cells[1, 51] = "'" + txtAgentBankNumber.Text;
                    oSheet.Cells[1, 52] = "'" + txtAgentChainNumber.Text;
                    oSheet.Cells[1, 53] = "'" + txtMCCCode.Text;
                    oSheet.Cells[1, 54] = "'" + txtMerchantID.Text;
                    oSheet.Cells[1, 55] = "'" + txtStoreNumber.Text;
                    oSheet.Cells[1, 56] = "'" + txtTerminalID.Text;
                }//end if vital
                else if (lstPlatform.SelectedItem.Text == "Nova")
                {
                    oSheet.Cells[1, 50] = "";
                    oSheet.Cells[1, 51] = "";
                    oSheet.Cells[1, 52] = "";
                    oSheet.Cells[1, 53] = "";
                    oSheet.Cells[1, 54] = "";
                    oSheet.Cells[1, 55] = "";
                    oSheet.Cells[1, 56] = "";

                    oSheet.Cells[1, 50] = "'" + txtBINNumber.Text;
                    oSheet.Cells[1, 51] = "'" + txtTerminalID.Text;
                }//end if nova
                else if (lstPlatform.SelectedItem.Text == "Omaha")
                {
                    oSheet.Cells[1, 50] = "";
                    oSheet.Cells[1, 51] = "";
                    oSheet.Cells[1, 52] = "";
                    oSheet.Cells[1, 53] = "";
                    oSheet.Cells[1, 54] = "";
                    oSheet.Cells[1, 55] = "";
                    oSheet.Cells[1, 56] = "";

                    oSheet.Cells[1, 50] = "'" + txtVisaMasterNumber.Text;
                }
                else if (lstPlatform.SelectedItem.Text == "Global Payments")
                {
                    oSheet.Cells[1, 50] = "";
                    oSheet.Cells[1, 51] = "";
                    oSheet.Cells[1, 52] = "";
                    oSheet.Cells[1, 53] = "";
                    oSheet.Cells[1, 54] = "";
                    oSheet.Cells[1, 55] = "";
                    oSheet.Cells[1, 56] = "";

                    oSheet.Cells[1, 50] = "'" + txtBINNumber.Text;
                    oSheet.Cells[1, 51] = "'" + txtMerchantID.Text;
                }

                lstRecurringBilling.SelectedValue = lstRecurringBilling.Items.FindByValue(dt[0].Recurring_Billing.ToString()).Value;
                lstShippedGoods.SelectedValue = lstShippedGoods.Items.FindByValue(dt[0].Shipped_Goods.ToString()).Value;
                lstSubsSales.SelectedValue = lstSubsSales.Items.FindByValue(dt[0].Subscription_Sales.ToString()).Value;

                oSheet.Cells[1, 13] = lstRecurringBilling.SelectedItem.Text;
                oSheet.Cells[1, 14] = lstShippedGoods.SelectedItem.Text;
                oSheet.Cells[1, 15] = lstSubsSales.SelectedItem.Text;
                oSheet.Cells[1, 31] = "Yes";
            }//end if count

            oXL.Visible = false;
            oXL.UserControl = false;
            //string strFile = DateTime.Now.Ticks.ToString() + ".xls";
            //oWB.SaveAs(strCurrentDir + strFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);
            ACTDataBL fp = new ACTDataBL();
            string FilePath = fp.ReturnCustomerFilePath(lblContactID.Text);

            FilePath = FilePath.ToLower();
            FilePath = FilePath.Replace("file://s:\\customers", "");
            FilePath = FilePath.Replace("\\", "/");

            string strHost = "../../../Customers";
            string strPath = Server.MapPath(strHost + FilePath + "/" + CompanyName + ".txt");
            DisplayMessage(strPath);
            oWB.SaveAs(strPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlUnicodeText, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, false, false, null, null, null);


            DisplayMessage("File created in the customer folder. To upload, you can select the file from the customer folder.");

            string strFile = DateTime.Now.Ticks.ToString() + ".xls";
            oWB.SaveAs(strCurrentDir + strFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null, null, null);


            // Need all following code to clean up and extingush all references!!!
            oWB.Close(null, null, null);
            oXL.Workbooks.Close();
            oXL.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
            oSheet = null;
            oWB = null;
            oXL = null;
            GC.Collect();  // force final cleanup!        
        }
        catch(Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.ToString());
        }
    }//end function GenerateExcelFile

    private void RemoveFiles(string strPath)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(strPath);
        FileInfo[] fiArr = di.GetFiles();
        foreach (FileInfo fri in fiArr)
        {

            if (fri.Extension.ToString() == ".xls")
            {
                TimeSpan min = new TimeSpan(0, 0, 60, 0, 0);
                if (fri.CreationTime < DateTime.Now.Subtract(min))
                {
                    fri.Delete();
                }
            }
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
            GenerateExcelFile();
            //DisplayMessage("Excel file created.");
            pnlPlatform.Visible = false;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.ToString());
        }
    }

    protected void lstPlatform_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetPlatformControls();
        DisablePlatformFields();
    }

    //This function disables fields based on reprogram platform selected
    public void DisablePlatformFields()
    {
        if (lstPlatform.SelectedItem.Text == "Nashville")
        {
            //Merchant ID
            txtMerchantID.Enabled = true;
            txtMerchantID.MaxLength = 11;
            txtTerminalID.Enabled = true;
            txtTerminalID.MaxLength = 11;

            if ((txtMerchantID.Text.Length < 1))
            {
                lblError.Visible = true;
                lblError.Text += "Merchant ID is invalid. Please correct in ACT! and retry<br/>";
            }

            if ((txtTerminalID.Text.Length < 1) )
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
            txtLoginID.Enabled = false;

            txtVisaMasterNumber.Text = "";
            txtStoreNumber.Text = "";
            txtMCCCode.Text = "";
            txtBINNumber.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            txtLoginID.Text = "";
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

            txtLoginID.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            txtLoginID.Text = "";
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

            if ((txtTerminalID.Text.Length < 16) || (txtTerminalID.Text.Length > 16))
            {
                lblError.Visible = true;
                lblError.Text += "Terminal ID length must be 16 characters long. Please correct in ACT!<br/>";
            }

            txtLoginID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMerchantID.Enabled = false;
            txtMCCCode.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            txtLoginID.Text = "";
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

            if ((txtVisaMasterNumber.Text.Length < 16) || (txtVisaMasterNumber.Text.Length > 16))
            {
                lblError.Visible = true;
                lblError.Text += "Visa Master Number length must be 16 characters long. Please correct in ACT!<br/>";
            }
            if ((txtMCCCode.Text.Length < 4) || (txtMCCCode.Text.Length > 4))
            {
                lblError.Visible = true;
                lblError.Text += "MCC Code length must be 4 characters long. Please correct in ACT!<br/>";
            }

            txtTerminalID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMerchantID.Enabled = false;
            txtLoginID.Enabled = false;
            txtBINNumber.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;

            txtTerminalID.Text = "";
            txtStoreNumber.Text = "";
            txtMerchantID.Text = "";
            txtLoginID.Text = "";
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

            if ((txtBINNumber.Text.Length < 6) || (txtBINNumber.Text.Length > 6))
            {
                lblError.Visible = true;
                lblError.Text += "BIN Number length must be 6 characters long. Please correct in ACT!<br/>";
            }

            if ((txtMerchantID.Text.Length < 15) || (txtMerchantID.Text.Length > 15))
            {
                lblError.Visible = true;
                lblError.Text += "Merchant ID length must be 15 characters long. Please correct in ACT!<br/>";
            }

            txtLoginID.Enabled = false;
            txtTerminalID.Enabled = false;
            txtStoreNumber.Enabled = false;
            txtMCCCode.Enabled = false;
            txtAgentChainNumber.Enabled = false;
            txtAgentBankNumber.Enabled = false;
            txtVisaMasterNumber.Enabled = false;

            txtLoginID.Text = "";
            txtTerminalID.Text = "";
            txtStoreNumber.Text = "";
            txtMCCCode.Text = "";
            txtAgentChainNumber.Text = "";
            txtAgentBankNumber.Text = "";
            txtVisaMasterNumber.Text = "";
        }//end if nova
    }//end function DisablePlatformFields

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
