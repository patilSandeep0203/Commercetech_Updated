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

public partial class ManagePartners : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            else if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "~/T1Agent.master";
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
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin") && !User.IsInRole("Employee") && !User.IsInRole("T1Agent"))
            Response.Redirect("~/logout.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                if ( User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    ListBL RepList = new ListBL();
                    DataSet ds = RepList.GetT1RepList();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = ds;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "MasterNum";
                        lstRepList.DataBind();
                    }
                    ListItem item = new ListItem();
                    item.Text = "ALL";
                    item.Value = "ALL";
                    lstRepList.Items.Add(item);
                    lstRepList.SelectedValue = "ALL";
                }
                else
                    pnlRepList.Visible = false;

                if (User.IsInRole("T1Agent"))
                {
                    lstPartnerCategory.Items.Remove(lstPartnerCategory.Items.FindByText("Past Employee"));
                    lstPartnerCategory.Items.Remove(lstPartnerCategory.Items.FindByText("Employee"));
                }

                if (User.IsInRole("OfficeManager"))
                {
                    lstPartnerCategory.Items.Remove(lstPartnerCategory.Items.FindByText("Past Employee"));
                    lstPartnerCategory.Items.Remove(lstPartnerCategory.Items.FindByText("Employee"));
                }

                MonthBL mon = new MonthBL();
                DataSet dsMon = mon.GetMonthList();
                if (dsMon.Tables[0].Rows.Count > 0)
                {
                    lstMonth.DataSource = dsMon;
                    lstMonth.DataTextField = "Mon";
                    lstMonth.DataValueField = "Mon";
                    lstMonth.DataBind();
                }                
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }

    //This function populates partner information
    public void Populate(string p_MasterNum)
    {
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Small;
        ValueLabel.Font.Name = "Arial";
                
        Style sHyperLink = new Style();
        //sHyperLink.Font.Bold = true;
        sHyperLink.Font.Size = FontUnit.Smaller;
        sHyperLink.Font.Name = "Arial";
        sHyperLink.CssClass = "One";

        //Get list of partners for selected month
        RepInfoBL PartnerInfo = new RepInfoBL();
        DataSet ds = PartnerInfo.GetPartners(lstMonth.SelectedItem.Value, lstPartnerCategory.SelectedItem.Value, p_MasterNum);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //Create headers
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            #region COLUMN HEADERS
            string[] arrColumns = { "Rep Name", "Tier 2 Rep Name", "Legal Name", "Cat", "Resd (%) *", "Comm (%) *", "Sage", "iPay1 (1503) Rep#", "iPay2 (40558) Rep#", "iPay3 (02733) Rep#", "IMS CTC 248 Rep#", "IMS2 CTC QB 487 Rep#", "IPS Rep#", "Chase Rep#", "Master Rep#", //"Active *", 
                                    "Default CNP Rate Package", "Default CP Rate Package"};
            /*if (User.IsInRole("OfficeManager"))
            {
                string[] arrColumns1 = { "Office Number", "Rep in the Office", "Legal Name", "Cat", "Resd (%) *", "Comm (%) *", "Sage", "iPay1 (1503) Rep#", "iPay2 (40558) Rep#", "iPay3 (02733) Rep#", "IMS CTC 248 Rep#", "IMS2 CTC QB 487 Rep#", "IPS Rep#", "Chase Rep#", "Master Rep#", //"Active *", 
                                    "Default CNP Rate Package", "Default CP Rate Package"};
            }
            else
            {
                string[] arrColumns = { "Rep Name", "Tier 2 Rep Name", "Legal Name", "Cat", "Resd (%) *", "Comm (%) *", "Sage", "iPay1 (1503) Rep#", "iPay2 (40558) Rep#", "iPay3 (02733) Rep#", "IMS CTC 248 Rep#", "IMS2 CTC QB 487 Rep#", "IPS Rep#", "Chase Rep#", "Master Rep#", //"Active *", 
                                    "Default CNP Rate Package", "Default CP Rate Package"};
            }*/
            tr.BackColor = System.Drawing.Color.FromArgb(93,123,157);
            for (int i = 0; i < arrColumns.Length; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i].ToString();
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "Small";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }
            tblSummary.Rows.Add(tr);
            #endregion

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                HyperLink lnkExport;
                tr = new TableRow();
                string MasterNum = string.Empty;
                MasterNum = dr["MasterNum"].ToString().Trim();
                //Create the RepName and Company links to updatepartner only if user is admin
                if (User.IsInRole("Admin"))
                {                    
                    //T1RepName
                    lnkExport = new HyperLink();
                    lnkExport.Text = dr["RepName"].ToString().Trim();
                    lnkExport.NavigateUrl = "UpdatePartner.aspx?MasterNum=" + MasterNum;
                    lnkExport.Target = "_Blank";
                    lnkExport.ApplyStyle(sHyperLink);
                    td = new TableCell();
                    td.Controls.Add(lnkExport);
                    tr.Cells.Add(td);

                    //T2RepName
                    lnkExport = new HyperLink();
                    lnkExport.Text = dr["T2RepName"].ToString().Trim();
                    lnkExport.NavigateUrl = "UpdatePartner.aspx?MasterNum=" + dr["T2MasterNum"].ToString().Trim();
                    lnkExport.Target = "_Blank";
                    lnkExport.ApplyStyle(sHyperLink);
                    td = new TableCell();
                    td.Controls.Add(lnkExport);
                    tr.Cells.Add(td);
                                        
                    lblValue = new Label();
                    lblValue.Text = dr["CompanyName"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }
                else
                {
                    //T1RepName
                    lblValue = new Label();
                    lblValue.Text = dr["RepName"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //T2RepName
                    lblValue = new Label();
                    lblValue.Text = dr["T2RepName"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Company
                    lblValue = new Label();
                    lblValue.Text = dr["CompanyName"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }
                //RepCat
                lblValue = new Label();
                lblValue.Text = dr["RepCat"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Rep Split
                lblValue = new Label();
                lblValue.Text = dr["RepSplit"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Commission
                lblValue = new Label();
                lblValue.Text = dr["Comm"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Sage               
                lblValue = new Label();
                lblValue.Text = dr["SageNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //iPayNum                
                lblValue = new Label();
                lblValue.Text = dr["IPayNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //iPay2Num
                lblValue = new Label();
                lblValue.Text = dr["IPay2Num"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //iPay3Num
                lblValue = new Label();
                lblValue.Text = dr["IPay3Num"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //IMSNum
                lblValue = new Label();
                lblValue.Text = dr["ImsNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Ims2Num
                lblValue = new Label();
                lblValue.Text = dr["Ims2Num"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = dr["IPSNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Chase Num
                lblValue = new Label();
                lblValue.Text = dr["ChaseNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Master Num
                lblValue = new Label();
                lblValue.Text = dr["T2MasterNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                /*string strActive = "No";
                if ((Convert.ToInt32(dr["MerchFundedCount"]) != 0) || (Convert.ToInt32(dr["referralCount"]) != 0))
                    strActive = "Yes";
                //Active
                lblValue = new Label();
                lblValue.Text = strActive;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);*/

                //CNP
                string strCNPPackageName = "Not An Affiliate";
                if (dr["PackageName"].ToString().Trim().Length > 1)
                    strCNPPackageName = dr["PackageName"].ToString().Trim();
                lnkExport = new HyperLink();
                lnkExport.Text = strCNPPackageName;
                lnkExport.NavigateUrl = "OnlineAppMgmt/ModifyPackage.aspx?PackageID=" + dr["PackageID"].ToString().Trim();
                lnkExport.Target = "_Blank";
                lnkExport.ApplyStyle(sHyperLink);
                td = new TableCell();
                td.Controls.Add(lnkExport);
                tr.Cells.Add(td);

                //CP
                string strCPPackageName = "Not An Affiliate";
                if (dr["CPPackageName"].ToString().Trim().Length > 1)
                    strCPPackageName = dr["CPPackageName"].ToString().Trim();
                lnkExport = new HyperLink();
                lnkExport.Text = strCPPackageName;
                lnkExport.NavigateUrl = "OnlineAppMgmt/ModifyPackage.aspx?PackageID=" + dr["CPPackageID"].ToString().Trim();
                lnkExport.Target = "_Blank";
                lnkExport.ApplyStyle(sHyperLink);
                td = new TableCell();
                td.Controls.Add(lnkExport);
                tr.Cells.Add(td);
                
                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblSummary.Rows.Add(tr);
            }//end for rows

        }//end if count not 0
    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            RepInfoBL Rep = new RepInfoBL();
            //Add the Rep Info Monthly Info for all Reps
            bool retVal = Rep.AddNewMonth(lstMonth.SelectedItem.Value);
            if (!retVal)
                DisplayMessage("Error adding information for new month");
            string MasterNum = string.Empty;
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                MasterNum = lstRepList.SelectedItem.Value;
            /*else if (User.IsInRole("OfficeManager"))
            { 
                RepInfoBL officeRep = new RepInfoBL();
                MasterNum = officeRep.(Session["MasterNum"].ToString().Trim());
            }*/
            else
                MasterNum = Session["MasterNum"].ToString().Trim();
            Populate(MasterNum);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating partner information.");
        }
    }//end submit button click

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
