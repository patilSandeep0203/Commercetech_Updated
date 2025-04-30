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

public partial class ResidualsAdmin : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
            Response.Redirect("~/logout.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                MonthBL Mon = new MonthBL();
                DataSet dsMon = Mon.GetMonthListForReports(1, "residuals");
                if (dsMon.Tables[0].Rows.Count > 0)
                {
                    lstMonth.DataSource = dsMon;
                    lstMonth.DataTextField = "Mon";
                    lstMonth.DataValueField = "Mon";
                    lstMonth.DataBind();

                    lstMonthDropped.DataSource = dsMon;
                    lstMonthDropped.DataTextField = "Mon";
                    lstMonthDropped.DataValueField = "Mon";
                    lstMonthDropped.DataBind();
                }
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }// not postback
    }//end page load

    //This function populates residuals

    public void PopulateDroppedReport(string Report, string Month)
    {
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        ResidualsAdminBL DroppedResd = new ResidualsAdminBL(Month);
        DataSet ds = DroppedResd.GetDroppedResiduals(Report);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            Label lblValue;

            #region Header Row

            tr = new TableRow();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                DataColumn dc = ds.Tables[0].Columns[i];
                td = new TableCell();
                td.Text = dc.ColumnName;
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "12px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }

            tblResiduals.Rows.Add(tr);

            #endregion

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                tr = new TableRow();

                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    lblValue = new Label();
                    lblValue.Text = dr[j].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }//end for
            pnlUpdateDroppedReport.Visible = true;
            pnlUpdateStatus.Visible = false;
        }//end if count not 0
        else
            DisplayMessage("No Records found for this month");
    }

    public void PopulateACTResidualStatus(string Status, string Service)
    {
        ResidualsAdminBL ACTResdStatus = new ResidualsAdminBL();
        DataSet ds = ACTResdStatus.GetACTResidualStatus(Status, Service);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            //pnlResetComm.Visible = true;

            Style ValueLabel = new Style();
            ValueLabel.ForeColor = System.Drawing.Color.Black;
            ValueLabel.Font.Size = FontUnit.Small;
            ValueLabel.Font.Name = "Arial";

            Style ValueLabelHeader = new Style();
            ValueLabelHeader.ForeColor = System.Drawing.Color.White;
            ValueLabelHeader.Font.Size = FontUnit.Small;
            ValueLabelHeader.Font.Bold = true;
            ValueLabelHeader.Font.Name = "Arial";

            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            string[] arrColumns;
            if (Service == "Merchant Account")
            {
                arrColumns = new string[] { "Company name", "DBA", "Processor", "Merchat Status", "Merchant number" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["Processor"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant Status
                    lblValue = new Label();
                    lblValue.Text = dr["MerchantStatus"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant number
                    lblValue = new Label();
                    lblValue.Text = dr["VisaMasterNum"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }//end for
            }
            else if (Service == "Gateway")
            {
                arrColumns = new string[] { "COMPANYNAME", "DBA", "Gateway", "GatewayStatus", "LoginID" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["Gateway"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant Status
                    lblValue = new Label();
                    lblValue.Text = dr["GatewayStatus"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant number
                    lblValue = new Label();
                    lblValue.Text = dr["LoginID"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }
                //end for
            }
            else if (Service == "Check Service")
            {
                arrColumns = new string[] { "COMPANYNAME", "DBA", "CrossCheck", "ActiveCheckGuarantee", "LoginID" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["CrossCheck"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant Status
                    lblValue = new Label();
                    lblValue.Text = dr["ActiveCheckGuarantee"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant number
                    lblValue = new Label();
                    lblValue.Text = dr["LoginID"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }
                //end for
            }
            else if (Service == "Gift Card")
            {
                arrColumns = new string[] { "COMPANYNAME", "DBA", "GiftType", "ActiveGift" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["GiftType"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant Status
                    lblValue = new Label();
                    lblValue.Text = dr["ActiveGift"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }
                //end for
            }
            else if (Service == "Cash Advance")
            {
                arrColumns = new string[] { "COMPANYNAME", "DBA", "MCACompany", "MerchantFundingStatus" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["MCACompany"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant Status
                    lblValue = new Label();
                    lblValue.Text = dr["MerchantFundingStatus"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }
                //end for
            }
            else if (Service == "Payroll")
            {
                arrColumns = new string[] { "COMPANYNAME", "DBA", "PayrollType", "PayrollStatus" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["PayrollType"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Merchant Status
                    lblValue = new Label();
                    lblValue.Text = dr["PayrollStatus"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }
                //end for
            }
            else if (Service == "Debit")
            {
                arrColumns = new string[] { "COMPANYNAME", "DBA", "ActiveDebit" };

                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[i].ToString();
                    td.Style["font-weight"] = "Bold";
                    td.CssClass = "MenuHeader";
                    tr.Cells.Add(td);
                }

                tblResiduals.Rows.Add(tr);
                double iTotal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Company name
                    lblValue = new Label();
                    lblValue.Text = dr["COMPANYNAME"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //DBA
                    lblValue = new Label();
                    lblValue.Text = dr["DBA"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Processor
                    lblValue = new Label();
                    lblValue.Text = dr["ActiveDebit"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblResiduals.Rows.Add(tr);
                }
                //end for
            }

            pnlUpdateStatus.Visible = true;
            pnlUpdateDroppedReport.Visible = false;

            /*
            tr = new TableRow();
            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            //Total Funded Header
            lblValue = new Label();
            lblValue.Text = "Total Funded";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            //Total
            lblValue = new Label();
            lblValue.Text = iTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);*/

            //tblCommSummary.Rows.Add(tr);
        }//end if count not 0
        else
            DisplayMessage("No Records found for this month");
    }

    public void PopulateResiduals(string Month)
    {
        //set the boundary date for the IMS , this report doesnot exist since OCT 2011
        string lastMonthStr = lstMonth.SelectedItem.Value;
        DateTime lastMonthDate = DateTime.Parse(lastMonthStr);
        DateTime dateBound = new DateTime(2011, 10, 1);

        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";        

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        Style sHyperLink = new Style();
        sHyperLink.Font.Bold = true;
        sHyperLink.CssClass = "LinkXSmall";
        sHyperLink.ForeColor = System.Drawing.Color.Teal;
        
        TableRow tr = new TableRow();
        TableCell td = new TableCell();
        
        bool ECXLegacyExists = false;
        bool ChaseExists = false;
        bool MerrickExists = false;
        bool OptimalCAExists = false;
        bool IPayment2Exists = false;
        bool IPayment3Exists = false;
        bool IMS2Exists = false;
        bool IMSExists = false;
        bool IPSExists = false;
        bool InovExists = false;
        bool InovGwyExists = false;
        bool DiscRapExists = false;
        bool SageExists = false;
        bool FBBHExists = false;
        bool WorldPayExists = false;
        //bool MiscExists = false;
        bool CSExists = false;
        bool GCExists = false;
        bool CTCartExists = false;
        bool MCAExists = false;
        bool PayrollExists = false;

        if ((lastMonthDate.CompareTo(dateBound)) < 0)
        {
            IMSExists = true;
        }
        else { IMSExists = false; }

        if ((lastMonthDate.CompareTo(dateBound)) < 0)
        {
            InovExists = true;
        }
        else { InovExists = false; }

        if ((lastMonthDate.CompareTo(dateBound)) < 0)
        {
            InovGwyExists = true;
        }
        else { InovGwyExists = false; }
                
        //Check which columns to generate based on month
        ResidualsAdminBL Resd = new ResidualsAdminBL(Month);

        DataSet dsCheck = Resd.CheckProcessorExists();
        if (dsCheck.Tables[0].Rows.Count > 0)
        {
            DataRow drCheck = dsCheck.Tables[0].Rows[0];
            ECXLegacyExists = Convert.ToBoolean(drCheck["ECXLegacy"]);
            ChaseExists = Convert.ToBoolean(drCheck["Chase"]);
            MerrickExists = Convert.ToBoolean(drCheck["Merrick"]);
            OptimalCAExists = Convert.ToBoolean(drCheck["OptimalCA"]);
            IPayment2Exists = Convert.ToBoolean(drCheck["IPayment2"]);
            IPayment3Exists = Convert.ToBoolean(drCheck["IPayment3"]);
            IMS2Exists = Convert.ToBoolean(drCheck["IMS2"]);
            IPSExists = Convert.ToBoolean(drCheck["IPS"]);
            DiscRapExists = Convert.ToBoolean(drCheck["DiscRap"]);
            SageExists = Convert.ToBoolean(drCheck["Sage"]);
            FBBHExists = Convert.ToBoolean(drCheck["ipayFBBH"]);
            WorldPayExists = Convert.ToBoolean(drCheck["WorldPay"]);
            //MiscExists = Convert.ToBoolean(drCheck["Misc"]);
            CSExists = Convert.ToBoolean(drCheck["CheckServices"]);
            GCExists = Convert.ToBoolean(drCheck["GiftCardServices"]);
            CTCartExists = Convert.ToBoolean(drCheck["CTCart"]);
            MCAExists = Convert.ToBoolean(drCheck["MerchantCashAdvance"]);
            PayrollExists = Convert.ToBoolean(drCheck["PayrollServices"]);
        }// check processor count not 0

        //Declare Arrays based on month selected and whether report exists for that month
        #region Array Declarations
        ArrayList arrList = new ArrayList();//Main Table Headers
        ArrayList arrListTotals = new ArrayList();//Totals Table Headers
        arrList.Add("Tier 1 Rep");
        arrList.Add("Tier 2 Rep"); //used to be Rep Name
        arrListTotals.Add("");
        arrList.Add("IPay1");
        arrListTotals.Add("IPay1");
        if (IPayment2Exists)
        {
            arrList.Add("IPay2");
            arrListTotals.Add("IPay2");
        }
        if (IPayment3Exists)
        {
            arrList.Add("IPay3");
            arrListTotals.Add("IPay3");
        }
        if (FBBHExists)
        {
            arrList.Add("IPay FBBH");
            arrListTotals.Add("IPay FBBH");
        }
        if (ECXLegacyExists)
        {
            arrList.Add("Ecx Legacy");
            arrListTotals.Add("Ecx Legacy");
        }
        if (IMSExists)
        {
            arrList.Add("IMS");
            arrListTotals.Add("IMS");
        }
        if (IMS2Exists)
        {
            arrList.Add("IMS(QB)");
            arrListTotals.Add("IMS(QB)");
        }
        if (IPSExists)
        {
            arrList.Add("IPS");
            arrListTotals.Add("IPS");
        }
        if (SageExists)
        {
            arrList.Add("Sage");
            arrListTotals.Add("Sage");
        }
        if (InovExists)
        {
            arrList.Add("Innov");
            arrListTotals.Add("Innov");
        }
        arrList.Add("CPS");
        arrListTotals.Add("CPS");
        if (DiscRapExists)
        {
            arrList.Add("Disc Rap");
            arrListTotals.Add("Disc Rap");
        }
        if (ChaseExists)
        {
            arrList.Add("Chase");
            arrListTotals.Add("Chase");
        }
        if (MerrickExists)
        {
            arrList.Add("Merrick");
            arrListTotals.Add("Merrick");
        }
        if (OptimalCAExists)
        {
            arrList.Add("Opt CA/Intl");
            arrListTotals.Add("Opt CA/Intl");
        }
        if (WorldPayExists)
        {
            arrList.Add("WPay");
            arrListTotals.Add("WPay");
        }
        arrList.Add("Auth Net");
        arrListTotals.Add("Auth Net");
        arrList.Add("IPay Gwy");
        arrListTotals.Add("IPay Gwy");
        if (InovGwyExists)
        {
            arrList.Add("Innov Gwy");
            arrListTotals.Add("Innov Gwy");
        }
        arrList.Add("Plug'n Pay");
        arrListTotals.Add("Plug'n Pay");

        if (CSExists)
        {
            arrList.Add("Check Services");
            arrListTotals.Add("Check Services");
        }

        if (GCExists)
        {
            arrList.Add("Gift Card");
            arrListTotals.Add("Gift Card");
        }

        if (CTCartExists)
        {
            arrList.Add("CT Cart");
            arrListTotals.Add("CT Cart");
        }

        if (MCAExists)
        {
            arrList.Add("Merchant Cash Advance");
            arrListTotals.Add("Merchant Cash Advance");
        }

        if (PayrollExists)
        {
            arrList.Add("Payroll");
            arrListTotals.Add("Payroll");
        }

        /*if (MiscExists)
        {
            arrList.Add("Misc");
            arrListTotals.Add("Misc");
        }*/
        arrListTotals.Add("TOTAL");
        
        arrList.Add("Company");
        arrList.Add("ECE Total");
        arrList.Add("Rep Split(%)");
        arrList.Add("Payment/ Rep Total");
        arrList.Add("2nd Tier Payment/ Total");
        arrList.Add("Total Payment (Tier 1 and Tier 2)");
        arrList.Add("Rep Cat");
        arrList.Add("Master Rep");
        arrList.Add("Funded/ Referred");
        arrList.Add("Comm");
        arrList.Add("Referral");

        #endregion

        #region MAIN TABLE HEADERS
        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        for (int i = 0; i < arrList.Count; i++)
        {
            td = new TableCell();
            td.Text = arrList[i].ToString();
            td.Style["font-weight"] = "Bold";
            td.CssClass = "MenuHeader";
            tr.Cells.Add(td);
        }
        tblResiduals.Rows.Add(tr);
        #endregion

        #region Variable Declarations
        double resdIPayGateTotal = 0;
        double resdIPayTotal = 0;
        double resdIMSTotal = 0;
        double resdIMS2Total = 0;
        double resdIPSTotal = 0;
        double resdSageTotal = 0;
        double resdWPayTotal = 0;
        double resdIPayFBBHTotal = 0;
        double resdCPSTotal = 0;
        double resdInnTotal = 0;
        double resdECXLegacyTotal = 0;
        double resdInnGateTotal = 0;
        double resdAuthnetTotal = 0;
        double resdDiscTotal = 0;
        double resdChaseTotal = 0;
        double resdMerrickTotal = 0;
        double resdOptimalCATotal = 0;
        double resdIPay2Total = 0;
        double resdIPay3Total = 0;
        double resdPlugTotal = 0;
        double resdCSTotal = 0;
        double resdGCTotal = 0;
        double resdCTCartTotal = 0;
        double resdMCATotal = 0;
        double resdPayrollTotal = 0;
        //double resdMiscTotal = 0;

        double eceInnTotal = 0;
        double eceIPayGateTotal = 0;
        double eceIPayTotal = 0;
        double eceIMSTotal = 0;
        double eceIMS2Total = 0;
        double eceIPSTotal = 0;
        double eceSageTotal = 0;
        double eceWPayTotal = 0;
        double eceIPayFBBHTotal = 0;
        double eceCPSTotal = 0;
        double eceECXLegacyTotal = 0;
        double eceInnGateTotal = 0;
        double eceAuthnetTotal = 0;
        double eceDiscTotal = 0;
        double eceChaseTotal = 0;
        double eceMerrickTotal = 0;
        double eceOptimalCATotal = 0;
        double eceIPay2Total = 0;
        double eceIPay3Total = 0;
        double ecePlugTotal = 0;
        double eceCSTotal = 0;
        double eceGCTotal = 0;
        double eceCTCartTotal = 0;
        double eceMCATotal = 0;
        double ecePayrollTotal = 0;
        //double eceMiscTotal = 0;

        double IPayGateSum = 0;
        double InnGateSum = 0;
        double IPayFBBHSum = 0;
        double DiscSum = 0;
        double CPSSum = 0;
        double InnSum = 0;
        double ECXLegacySum = 0;
        double IPaySum = 0;
        double IPay2Sum = 0;
        double IPay3Sum = 0;
        double IMSSum = 0;
        double IMS2Sum = 0;
        double IPSSum = 0;
        double SageSum = 0;
        double OptimalCASum = 0;
        double AuthnetSum = 0;
        double WpaySum = 0;
        double PlugSum = 0;
        double ChaseSum = 0;
        double MerrickSum = 0;
        double CSSum = 0;
        double GCSum = 0;
        double CTCartSum = 0;
        double MCASum = 0;
        double PayrollSum = 0;
        //double MiscSum = 0;

        double RepEceTotalSum = 0;

        double RepResidualTotal = 0;
        double RepResidualPayment = 0;

        double TRepResidualTotal = 0;
        double TRepResidualPayment = 0;

        //Tier1 and Tier2 totals
        double TotalRepResidualPayment = 0;
        double ECETotalResidualPaymentSum = 0;

        double eceTotal = 0;
        double ECEResidualSum = 0;
        double ECEResidualPaymentSum = 0;

        double ECEComm = 0;
        double RepCommision = 0;
        double ECEReferralTotal = 0;
        #endregion

        Label lblValue;
        HyperLink lnkExport;
        
        //Get residuals
        PartnerDS.ResidualsReportsDataTable dt = Resd.GetResidualsReport();
        if (dt.Rows.Count > 0)
        {
            for ( int i=0; i< dt.Rows.Count; i++)
            {
                //Each for loop iteration generates one row
                tr = new TableRow();                

                #region Totals
                resdIPayGateTotal = 0;
                resdIPayTotal = 0;
                resdIMSTotal = 0;
                resdIMS2Total = 0;
				resdIPSTotal = 0;
                resdSageTotal = 0;
                resdWPayTotal = 0;
                resdIPayFBBHTotal = 0;
                resdCPSTotal = 0;
                resdInnTotal = 0;
                resdECXLegacyTotal = 0;
                resdInnGateTotal = 0;
                resdAuthnetTotal = 0;
                resdDiscTotal = 0;
                resdChaseTotal = 0;
                resdMerrickTotal = 0;
                resdOptimalCATotal = 0;
                resdIPay2Total = 0;
                resdIPay3Total = 0;
                resdPlugTotal = 0;
                resdCSTotal = 0;
                resdGCTotal = 0;
                resdCTCartTotal = 0;
                resdMCATotal = 0;
                resdPayrollTotal = 0;
                //resdMiscTotal = 0;

                eceInnTotal = 0;
                eceIPayGateTotal = 0;
                eceIPayTotal = 0;
                eceIMSTotal = 0;
                eceIMS2Total = 0;
				eceIPSTotal = 0;
                eceSageTotal = 0;
                eceWPayTotal = 0;
                eceIPayFBBHTotal = 0;
                eceCPSTotal = 0;
                eceECXLegacyTotal = 0;
                eceInnGateTotal = 0;
                eceAuthnetTotal = 0;
                eceDiscTotal = 0;
                eceChaseTotal = 0;
                eceMerrickTotal = 0;
                eceOptimalCATotal = 0;
                eceIPay2Total = 0;
                eceIPay3Total = 0;
                ecePlugTotal = 0;
                eceCSTotal = 0;
                eceGCTotal = 0;
                eceCTCartTotal = 0;
                eceMCATotal = 0;
                ecePayrollTotal = 0;
                //eceMiscTotal = 0;

                if (dt[i].IpayGateECETotalRep.ToString().Trim() != "")
                {
                    eceIPayGateTotal = Convert.ToDouble(dt[i].IpayGateECETotalRep);
                    resdIPayGateTotal = Convert.ToDouble(dt[i].IpayGateRepTotalRep);
                }

                if (dt[i].InnGateECETotalRep.ToString().Trim() != "")
                {
                    eceInnGateTotal = Convert.ToDouble(dt[i].InnGateECETotalRep);
                    resdInnGateTotal = Convert.ToDouble(dt[i].InnGateRepTotalRep);
                }

                if (dt[i].CTCartECETotalRep.ToString().Trim() != "")
                {
                    eceCTCartTotal = Convert.ToDouble(dt[i].CTCartECETotalRep);
                    resdCTCartTotal = Convert.ToDouble(dt[i].CTCartRepTotalRep);
                }

                if (dt[i].DiscEceTotalRep.ToString().Trim() != "")
                {
                    eceDiscTotal = Convert.ToDouble(dt[i].DiscEceTotalRep);
                    resdDiscTotal = Convert.ToDouble(dt[i].DiscRepTotalRep);
                }

                if (dt[i].CPSECETotalRep.ToString().Trim() != "")
                {
                    eceCPSTotal = Convert.ToDouble(dt[i].CPSECETotalRep);
                    resdCPSTotal = Convert.ToDouble(dt[i].CPSRepTotalRep);
                }

                if (dt[i].InnECETotalRep.ToString().Trim() != "")
                {
                    eceInnTotal = Convert.ToDouble(dt[i].InnECETotalRep);
                    resdInnTotal = Convert.ToDouble(dt[i].InnRepTotalRep);
                }

                if (dt[i].ECXLegacyECETotalRep.ToString().Trim() != "")
                {
                    eceECXLegacyTotal = Convert.ToDouble(dt[i].ECXLegacyECETotalRep);
                    resdECXLegacyTotal = Convert.ToDouble(dt[i].ECXLegacyRepTotalRep);
                }

                if (dt[i].IPayECETotalRep.ToString().Trim() != "")
                {
                    eceIPayTotal = Convert.ToDouble(dt[i].IPayECETotalRep);
                    resdIPayTotal = Convert.ToDouble(dt[i].IPayRepTotalRep);
                }

                if (dt[i].IPayFBBHECETotalRep.ToString().Trim() != "")
                {
                    eceIPayFBBHTotal = Convert.ToDouble(dt[i].IPayFBBHECETotalRep);
                    resdIPayFBBHTotal = Convert.ToDouble(dt[i].IPayFBBHRepTotalRep);
                }

                if (dt[i].IPay2EceTotalRep.ToString().Trim() != "")
                {
                    eceIPay2Total = Convert.ToDouble(dt[i].IPay2EceTotalRep);
                    resdIPay2Total = Convert.ToDouble(dt[i].IPay2RepTotalRep);
                } 
                
                if (dt[i].IPay3EceTotalRep.ToString().Trim() != "")
                {
                    eceIPay3Total = Convert.ToDouble(dt[i].IPay3EceTotalRep);
                    resdIPay3Total = Convert.ToDouble(dt[i].IPay3RepTotalRep);
                }

                if (dt[i].IMSEceTotalRep.ToString().Trim() != "")
                {
                    eceIMSTotal = Convert.ToDouble(dt[i].IMSEceTotalRep);
                    resdIMSTotal = Convert.ToDouble(dt[i].IMSRepTotalRep);
                }

                if (dt[i].IMS2EceTotalRep.ToString().Trim() != "")
                {
                    eceIMS2Total = Convert.ToDouble(dt[i].IMS2EceTotalRep);
                    resdIMS2Total = Convert.ToDouble(dt[i].IMS2RepTotalRep);
                }
				
				if (dt[i].IPSEceTotalRep.ToString().Trim() != "")
                {
                    eceIPSTotal = Convert.ToDouble(dt[i].IPSEceTotalRep);
                    resdIPSTotal = Convert.ToDouble(dt[i].IPSRepTotalRep);
                }

                if (dt[i].SageEceTotalRep.ToString().Trim() != "")
                {
                    eceSageTotal = Convert.ToDouble(dt[i].SageEceTotalRep);
                    resdSageTotal = Convert.ToDouble(dt[i].SageRepTotalRep);
                }

                if (dt[i].OptimalCAECETotalRep.ToString().Trim() != "")
                {
                    eceOptimalCATotal = Convert.ToDouble(dt[i].OptimalCAECETotalRep);
                    resdOptimalCATotal = Convert.ToDouble(dt[i].OptimalCARepTotalRep);
                }

                if (dt[i].AuthnetECETotalRep.ToString().Trim() != "")
                {
                    eceAuthnetTotal = Convert.ToDouble(dt[i].AuthnetECETotalRep);
                    resdAuthnetTotal = Convert.ToDouble(dt[i].AuthnetRepTotalRep);
                }

                if (dt[i].WPayECETotalRep.ToString().Trim() != "")
                {
                    eceWPayTotal = Convert.ToDouble(dt[i].WPayECETotalRep);
                    resdWPayTotal = Convert.ToDouble(dt[i].WPayRepTotalRep);
                }

                if (dt[i].PlugnPayECETotalRep.ToString().Trim() != "")
                {
                    ecePlugTotal = Convert.ToDouble(dt[i].PlugnPayECETotalRep);
                    resdPlugTotal = Convert.ToDouble(dt[i].PlugnPayRepTotalRep);
                }

                if (dt[i].ChaseEceTotalRep.ToString().Trim() != "")
                {
                    eceChaseTotal = Convert.ToDouble(dt[i].ChaseEceTotalRep);
                    resdChaseTotal = Convert.ToDouble(dt[i].ChaseRepTotalRep);
                }

                if (dt[i].MerrickECETotalRep.ToString().Trim() != "")
                {
                    eceMerrickTotal = Convert.ToDouble(dt[i].MerrickECETotalRep);
                    resdMerrickTotal = Convert.ToDouble(dt[i].MerrickRepTotalRep);
                }

                if (dt[i].CSECETotalRep.ToString().Trim() != "")
                {
                    eceCSTotal = Convert.ToDouble(dt[i].CSECETotalRep);
                    resdCSTotal = Convert.ToDouble(dt[i].CSRepTotalRep);
                } 
                
                if (dt[i].GCECETotalRep.ToString().Trim() != "")
                {
                    eceGCTotal = Convert.ToDouble(dt[i].GCECETotalRep);
                    resdGCTotal = Convert.ToDouble(dt[i].GCRepTotalRep);
                }
                
                if (dt[i].MCAECETotalRep.ToString().Trim() != "")
                {
                    eceMCATotal = Convert.ToDouble(dt[i].MCAECETotalRep);
                    resdMCATotal = Convert.ToDouble(dt[i].MCARepTotalRep);
                }

                if (dt[i].PayrollECETotalRep.ToString().Trim() != "")
                {
                    ecePayrollTotal = Convert.ToDouble(dt[i].PayrollECETotalRep);
                    resdPayrollTotal = Convert.ToDouble(dt[i].PayrollRepTotalRep);
                }
                /*if (dt[i].MiscEceTotalRep.ToString().Trim() != "")
                {
                    eceMiscTotal = Convert.ToDouble(dt[i].MiscEceTotalRep);
                    resdMiscTotal = Convert.ToDouble(dt[i].MiscRepTotalRep);
                }*/

                #endregion

                #region Sum
                IPayGateSum = eceIPayGateTotal + IPayGateSum;
                InnGateSum = eceInnGateTotal + InnGateSum;
                IPayFBBHSum = eceIPayFBBHTotal + IPayFBBHSum;
                DiscSum = eceDiscTotal + DiscSum;
                CPSSum = eceCPSTotal + CPSSum;
                InnSum = eceInnTotal + InnSum;
                ECXLegacySum = eceECXLegacyTotal + ECXLegacySum;
                IPaySum = eceIPayTotal + IPaySum;
                IPay2Sum = eceIPay2Total + IPay2Sum;
                IPay3Sum = eceIPay3Total + IPay3Sum;
                IMSSum = eceIMSTotal + IMSSum;
                IMS2Sum = eceIMS2Total + IMS2Sum;
				IPSSum = eceIPSTotal + IPSSum;
                SageSum = eceSageTotal + SageSum;
                OptimalCASum = eceOptimalCATotal + OptimalCASum;
                AuthnetSum = eceAuthnetTotal + AuthnetSum;
                WpaySum = eceWPayTotal + WpaySum;
                PlugSum = ecePlugTotal + PlugSum;
                ChaseSum = eceChaseTotal + ChaseSum;
                MerrickSum = eceMerrickTotal + MerrickSum;
                CSSum = eceCSTotal + CSSum;
                GCSum = eceGCTotal + GCSum;
                CTCartSum = eceCTCartTotal + CTCartSum;
                MCASum = eceMCATotal + MCASum;
                PayrollSum = ecePayrollTotal + PayrollSum;
                //MiscSum = eceMiscTotal + MiscSum;

                #endregion

                //Calculate ece Total for Rep
                double RepECETotal = eceIPayGateTotal + eceIPayTotal + eceIMSTotal + eceIMS2Total + eceIPSTotal + eceSageTotal + eceWPayTotal + eceIPayFBBHTotal + eceCPSTotal + eceInnTotal + eceECXLegacyTotal + eceInnGateTotal + eceCTCartTotal + eceAuthnetTotal + eceDiscTotal + ecePlugTotal + eceChaseTotal + eceMerrickTotal + eceOptimalCATotal + eceIPay2Total + eceIPay3Total + eceCSTotal + eceGCTotal + eceMCATotal + ecePayrollTotal;//+ eceMiscTotal;

	            //Sum of all the individual's RepEceTotal (displayed)                
                RepEceTotalSum = RepEceTotalSum + RepECETotal;

                //===============CALCULATE Commissions FOR Agents==================
	            //Sum up Rep's Commissions for the month            	
	            //Rep has a Commission Total
	            double repCommTotal = 0;
	            double RefTotal = 0;
	            double BonusTotal = 0;
	            double MerchFundedCount = 0;
	            double ReferralCount =0;
            	
	            if (dt[i].CommTotal.ToString().Trim() !=  "")
                    repCommTotal = Convert.ToDouble(dt[i].CommTotal);	            
            	
	            if (dt[i].bonustotal.ToString().Trim() !=  "") 		
		            BonusTotal = Convert.ToDouble( dt[i].bonustotal);	            

	            if (dt[i].reftotal.ToString().Trim() !=  "" ) 
		            RefTotal = Convert.ToDouble( dt[i].reftotal);	            	

	            if (dt[i].MerchFundedCount.ToString().Trim() !=  "" ) 
		            MerchFundedCount = Convert.ToDouble( dt[i].MerchFundedCount);	            

	            if (dt[i].ReferralCount.ToString().Trim() !=  "" ) 
		            ReferralCount = Convert.ToDouble( dt[i].ReferralCount);
	            
	            RepCommision = repCommTotal + BonusTotal;                
                ECEComm = (ECEComm) + (RepCommision);                
                ECEReferralTotal = (ECEReferralTotal) + (RefTotal);
                //==================END OF Commissions===================

                #region Generate Table
                //*************************GENERATE TABLE*************************

                string MasterNum = dt[i].MasterNum.ToString().Trim();
                
                //Tier 1 RepName
                lnkExport = new HyperLink();
                lnkExport.Text = dt[i].T1RepName.ToString().Trim();
                lnkExport.NavigateUrl = "~/Reports/Residuals/Residuals.aspx?MasterNum=" + dt[i].T1MasterNum.ToString().Trim() + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Tier 2 RepName
                lnkExport = new HyperLink();
                lnkExport.Text = dt[i].RepName.ToString().Trim();
                lnkExport.NavigateUrl = "../Reports/Residuals/Residuals.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //IPay
                lnkExport = new HyperLink();
                lnkExport.Text = resdIPayTotal.ToString();
                lnkExport.NavigateUrl = "../Reports/Residuals/IPay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                if (IPayment2Exists)
                {
                    //IPay2
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdIPay2Total.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/IPay2.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (IPayment3Exists)
                {
                    //IPay3
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdIPay3Total.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/IPay3.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (FBBHExists)
                {
                    //IPayFBBH
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdIPayFBBHTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/IPayfbbh.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (ECXLegacyExists)
                {
                    //ECXLegacy
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdECXLegacyTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/ECXLegacy.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //IMS
                if (IMSExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdIMSTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/ims.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (IMS2Exists)
                {
                    //IMS2
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdIMS2Total.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/ims2.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }
				
				if (IPSExists)
                {
                    //IPS
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdIPSTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/ips.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (SageExists)
                {
                    //Sage
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdSageTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/Sage.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Innov
                if (InovExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdInnTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/Innovative.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //CPS
                lnkExport = new HyperLink();
                lnkExport.Text = resdCPSTotal.ToString();
                lnkExport.NavigateUrl = "../Reports/Residuals/cps.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Disc
                if (DiscRapExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdDiscTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/disc.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (ChaseExists)
                {
                    //Chase
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdChaseTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/chase.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (MerrickExists)
                {
                    //Merrick
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdMerrickTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/merrick.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                if (OptimalCAExists)
                {
                    //Opt CA
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdOptimalCATotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/optimalca.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //WPay
                if (WorldPayExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdWPayTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/wpay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Auth Net
                lnkExport = new HyperLink();
                lnkExport.Text = resdAuthnetTotal.ToString();
                lnkExport.NavigateUrl = "../Reports/Residuals/Authnet.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //IPayGwy
                lnkExport = new HyperLink();
                lnkExport.Text = resdIPayGateTotal.ToString();
                lnkExport.NavigateUrl = "../Reports/Residuals/IPaygate.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //InnovGwy
                if (InovGwyExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdInnGateTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/inngate.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Plug N Pay
                lnkExport = new HyperLink();
                lnkExport.Text = resdPlugTotal.ToString();
                lnkExport.NavigateUrl = "../Reports/Residuals/plugnpay.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Check Services
                if (CSExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdCSTotal.ToString();
                    lnkExport.NavigateUrl = "~/Reports/Residuals/CheckServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }
                //Gift Card Services
                if (GCExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdGCTotal.ToString();
                    lnkExport.NavigateUrl = "~/Reports/Residuals/GiftCardServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //CTCart
                if (CTCartExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdCTCartTotal.ToString();
                    lnkExport.NavigateUrl = "../Reports/Residuals/ctcart.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Merchant Cash Advance
                if (MCAExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdMCATotal.ToString();
                    lnkExport.NavigateUrl = "~/Reports/Residuals/MerchantCashAdvance.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Payroll
                if (PayrollExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdPayrollTotal.ToString();
                    lnkExport.NavigateUrl = "~/Reports/Residuals/PayrollServices.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }

                //Misc
                /*if (MiscExists)
                {
                    lnkExport = new HyperLink();
                    lnkExport.Text = resdMiscTotal.ToString();
                    lnkExport.NavigateUrl = "~/Reports/Residuals/Misc.aspx?MasterNum=" + MasterNum + "&Month=" + Month;
                    td = new TableCell();
                    lnkExport.ApplyStyle(sHyperLink);
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }*/

                //Company Name
                lnkExport = new HyperLink();
                lnkExport.Text = dt[i].CompanyName.ToString().Trim();
                lnkExport.NavigateUrl = "~/updatepartner.aspx?MasterNum=" + MasterNum;
                lnkExport.Target = "_blank";
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //ECE Total
                lblValue = new Label();
                lblValue.Text = RepECETotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);                
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Rep Split
                lblValue = new Label();
                lblValue.Text = dt[i].RepSplit.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);                
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                RepResidualTotal = Convert.ToDouble(dt[i].ResidualTotal);
                RepResidualPayment = Convert.ToDouble(dt[i].ResidualPayTotal);
                
                //Residual Payment
                lblValue = new Label();
                //If Rep's Residual Payment is the same as his Generated Residual Total, print once
                if (RepResidualPayment == RepResidualTotal)
                    lblValue.Text = dt[i].ResidualPayTotal.ToString();
                else
                    lblValue.Text = dt[i].ResidualPayTotal.ToString() + "[$" + dt[i].ResidualTotal.ToString() + "]";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.ForeColor = System.Drawing.Color.Red;
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                TRepResidualTotal = Convert.ToDouble(dt[i].TResidualTotal);
                TRepResidualPayment = Convert.ToDouble(dt[i].TResidualPayTotal);

                //Tier 2 Residual Payment
                lblValue = new Label();
                //If Rep's Residual Payment is the same as his Generated Residual Total, print once
                if (TRepResidualPayment == TRepResidualTotal)
                    lblValue.Text = dt[i].TResidualTotal.ToString();
                else
                    lblValue.Text = dt[i].TResidualPayTotal.ToString() + "[$" + dt[i].TResidualTotal.ToString() + "]";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.ForeColor = System.Drawing.Color.Red;
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                TotalRepResidualPayment = Convert.ToDouble(dt[i].Payment);                

                //Residual Payment = Tier 1 + Tier 2
                lblValue = new Label();
                lblValue.Text = dt[i].Payment.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.ForeColor = System.Drawing.Color.Red;
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Category
                lblValue = new Label();
                lblValue.Text = dt[i].RepCat.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //MasterNum
                lblValue = new Label();
                lblValue.Text = dt[i].MasterNum.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Merchant Funded Count
                lblValue = new Label();
                lblValue.Text = MerchFundedCount.ToString() + "/" + ReferralCount.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Rep Commission
                lnkExport = new HyperLink();
                lnkExport.Text = RepCommision.ToString();
                lnkExport.NavigateUrl = "CommAdmin.aspx?MasterNum=" + MasterNum + "&Month=" + lstMonth.SelectedItem.Value;
                td = new TableCell();
                lnkExport.ApplyStyle(sHyperLink);
                td.Controls.Add(lnkExport);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Ref Total
                lnkExport = new HyperLink();
                lnkExport.Text = RefTotal.ToString();
                lnkExport.NavigateUrl = "ReferralsAdmin.aspx?Month=" + Month;
                td = new TableCell();
   
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblResiduals.Rows.Add(tr);

                #endregion

                eceTotal = RepECETotal + eceTotal;
                //add Rep's Residual Total to the running sum for all reps
                ECEResidualSum = ECEResidualSum + RepResidualTotal;
                ECEResidualPaymentSum = ECEResidualPaymentSum + RepResidualPayment;

                ECETotalResidualPaymentSum = ECETotalResidualPaymentSum + TotalRepResidualPayment;

            }//end for
        }// count not 0

        //Generate Totals Table
        #region Totals Table

        #region Number of Accounts

        string GateCount = string.Empty;
        string InvgCount = string.Empty;
        string FbbhCount = string.Empty;
        string DiscCount = string.Empty;
        string CpsCount = string.Empty;
        string PlugCount = string.Empty;
        string InnvCount = string.Empty;
        string WpayCount = string.Empty;
        string AuthCount = string.Empty;
        string ImsCount = string.Empty;
        string Ims2Count = string.Empty;
		string IPSCount = string.Empty;
        string SageCount = string.Empty;
        string IPayCount = string.Empty;
        string IPay2Count = string.Empty;
        string IPay3Count = string.Empty;
        string ecxLegacyCount = string.Empty;
        string ChaseCount = string.Empty;
        string MerrickCount = string.Empty;
        string OptimalCACount = string.Empty;
        string CSCount = string.Empty;
        string GCCount = string.Empty;
        string CtcartCount = string.Empty;
        string MCACount = string.Empty;
        string PayrollCount = string.Empty;
        //string MiscCount = string.Empty;

        DataSet dsAcct = Resd.GetResidualsCount();
        if (dsAcct.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsAcct.Tables[0].Rows[0];
            GateCount = dr["IPayGateCount"].ToString().Trim();
            InvgCount = dr["InnGateCount"].ToString().Trim();
            FbbhCount = dr["IPayFBBHCount"].ToString().Trim();
            DiscCount = dr["DiscRapCount"].ToString().Trim();
            CpsCount = dr["CPSCount"].ToString().Trim();
            PlugCount = dr["PlugNPayCount"].ToString().Trim();
            InnvCount = dr["InnCount"].ToString().Trim();
            WpayCount = dr["WpayCount"].ToString().Trim();
            AuthCount = dr["AuthCount"].ToString().Trim();
            ImsCount = dr["ImsCount"].ToString().Trim();
            Ims2Count = dr["Ims2Count"].ToString().Trim();
			IPSCount = dr["IPSCount"].ToString().Trim();
            SageCount = dr["SageCount"].ToString().Trim();
            IPayCount = dr["IPayCount"].ToString().Trim();
            IPay2Count = dr["IPay2Count"].ToString().Trim();
            IPay3Count = dr["IPay3Count"].ToString().Trim();
            ecxLegacyCount = dr["ecxLegacyCount"].ToString().Trim();
            ChaseCount = dr["ChaseCount"].ToString().Trim();
            MerrickCount = dr["MerrickCount"].ToString().Trim();
            OptimalCACount = dr["OptimalCACount"].ToString().Trim();
            CSCount = dr["CSCount"].ToString().Trim();
            GCCount = dr["GCCount"].ToString().Trim();
            CtcartCount = dr["CtCartCount"].ToString().Trim();
            MCACount = dr["MCACount"].ToString().Trim();
            PayrollCount = dr["PayrollCount"].ToString().Trim();
            //MiscCount = dr["MiscReportCount"].ToString().Trim();
        }
        #endregion

        #region Calculate Totals
        //*******************************Calculate Totals*******************************

        #region Variables
        //Current Month Variables
        double CPSTotal = 0;
        double InnTotal = 0;
        double AuthnetTotal = 0;
        double IPayTotal = 0;
        double IPay2Total = 0;
        double IPay3Total = 0;
        double IMSTotal = 0;
        double IMS2Total = 0;
		double IPSTotal = 0;
        double SageTotal = 0;
        double WPayTotal = 0;
        double IPayGateTotal = 0;
        double InnGateTotal = 0;
        double IPayFBBHTotal = 0;
        double ChaseTotal = 0;
        double MerrickTotal = 0;
        double OptimalCATotal = 0;
        double DiscTotal = 0;
        double ECXLegacyTotal = 0;
        double PlugTotal = 0;
        double CSTotal = 0;
        double GCTotal = 0;
        double CTCartTotal = 0;
        double MCATotal = 0;
        double PayrollTotal = 0;
        //double MiscTotal = 0;
  
        //Previous Month Variables
        double CPSPrevTotal = 0;
        double InnPrevTotal = 0;
        double AuthnetPrevTotal = 0;
        double IPayPrevTotal = 0;
        double IPay2PrevTotal = 0;
        double IPay3PrevTotal = 0;
        double IMSPrevTotal = 0;
        double IMS2PrevTotal = 0;
		double IPSPrevTotal = 0;
        double SagePrevTotal = 0;
        double WPayPrevTotal = 0;
        double IPayGatePrevTotal = 0;
        double InnGatePrevTotal = 0;
        double IPayFBBHPrevTotal = 0;
        double ChasePrevTotal = 0;
        double MerrickPrevTotal = 0;
        double OptimalCAPrevTotal = 0;
        double DiscPrevTotal = 0;
        double ECXLegacyPrevTotal = 0;
        double PlugPrevTotal = 0;
        double CSPrevTotal = 0;
        double GCPrevTotal = 0;
        double CTCartPrevTotal = 0;
        double MCAPrevTotal = 0;
        double PayrollPrevTotal = 0;
        //double MiscPrevTotal = 0;
        #endregion


        #region Current Months Totals
        //Select the ECETotal for the current month for all CPS accounts
        CPSTotal = Resd.GetECETotal("CPS", "ALL");
        //Select the ECETotal for the current month for all Innovative accounts
        InnTotal = Resd.GetECETotal("Innovative", "ALL" );
        //Select the ECETotal for the current month for all Authnet accounts
        AuthnetTotal = Resd.GetECETotal("Authnet", "ALL");
        //Select the ECETotal for the current month for all IPay accounts
        IPayTotal = Resd.GetECETotal("IPay", "ALL");
        //Select the ECETotal for the current month for all IPay2 accounts
        IPay2Total = Resd.GetECETotal("IPay2", "ALL");
        //Select the ECETotal for the current month for all IPay3 accounts
        IPay3Total = Resd.GetECETotal("IPay3", "ALL");
        //Select the ECETotal for the current month for all IMS accounts
        IMSTotal = Resd.GetECETotal("IMS", "ALL");
        //Select the ECETotal for the current month for all IMS2 accounts
        IMS2Total = Resd.GetECETotal("IMS2", "ALL");
		//Select the ECETotal for the current month for all IPS accounts
		IPSTotal = Resd.GetECETotal("IPS", "ALL");
        //Select the ECETotal for the current month for all Sage accounts
        SageTotal = Resd.GetECETotal("Sage", "ALL");
        //Select the ECETotal for the current month for all WPay accounts
        WPayTotal = Resd.GetECETotal("WPay", "ALL");
        //Select the ECETotal for the current month for all IPayGate accounts
        IPayGateTotal = Resd.GetECETotal("IPayGate", "ALL");
        //Select the ECETotal for the current month for all InnGate accounts
        InnGateTotal = Resd.GetECETotal("InnGate", "ALL");
        //Select the ECETotal for the current month for all IPayFBBH accounts
        IPayFBBHTotal = Resd.GetECETotal("IPayFBBH", "ALL");
        //Select the ECETotal for the current month for all Chase accounts
        ChaseTotal = Resd.GetECETotal("Chase", "ALL");
        //Select the ECETotal for the current month for all Merrick accounts
        MerrickTotal = Resd.GetECETotal("Merrick", "ALL");
        //Select the ECETotal for the current month for all OptimalCA accounts
        OptimalCATotal = Resd.GetECETotal("OptimalCA", "ALL");
        //Select the ECETotal for the current month for all Disc accounts
        DiscTotal = Resd.GetECETotal("Disc", "ALL");
        //Select the ECETotal for the current month for all ECXLegacy accounts
        ECXLegacyTotal = Resd.GetECETotal("ECX", "ALL");
        //Select the ECETotal for the current month for all Plug accounts
        PlugTotal = Resd.GetECETotal("PlugNPay", "ALL");
        //Select the ECETotal for the current month for all Check Services accounts
        CSTotal = Resd.GetECETotal("CS", "ALL");
        //Select the ECETotal for the current month for all Gift Card accounts
        GCTotal = Resd.GetECETotal("GC", "ALL");
        //Select the ECETotal for the current month for all CTCart accounts
        CTCartTotal = Resd.GetECETotal("CTCart", "ALL");
        //Select the ECETotal for the current month for all Merchant Cash Advance accounts
        MCATotal = Resd.GetECETotal("MCA", "ALL");
        //Select the ECETotal for the current month for all Payroll accounts
        PayrollTotal = Resd.GetECETotal("Payroll", "ALL");
        //Select the ECETotal for the current month for all Misc accounts
        //MiscTotal = Resd.GetECETotal("Misc", "ALL");

        #endregion
        //---------------------------------------------------------------------------------------------

        //Get Prev Month
        MonthBL pMonth = new MonthBL();
        string strPrevMonth = pMonth.ReturnPrevMonth(lstMonth.SelectedItem.Value);
        ResidualsAdminBL PrevResd = new ResidualsAdminBL(strPrevMonth);

        #region Previous Months Totals

        //Select the ECETotal for the Previous month for all CPS accounts
        CPSPrevTotal = PrevResd.GetECETotal("CPS", "ALL");
        //Select the ECETotal for the Previous month for all Innovative accounts
        InnPrevTotal = PrevResd.GetECETotal("Innovative", "ALL");
        //Select the ECETotal for the Previous month for all Authnet accounts
        AuthnetPrevTotal = PrevResd.GetECETotal("Authnet", "ALL");
        //Select the ECETotal for the Previous month for all IPay accounts
        IPayPrevTotal = PrevResd.GetECETotal("IPay", "ALL");
        //Select the ECETotal for the Previous month for all IPay2 accounts
        IPay2PrevTotal = PrevResd.GetECETotal("IPay2", "ALL");
        //Select the ECETotal for the Previous month for all IPay3 accounts
        IPay3PrevTotal = PrevResd.GetECETotal("IPay3", "ALL");
        //Select the ECETotal for the Previous month for all IMS accounts
        IMSPrevTotal = PrevResd.GetECETotal("IMS", "ALL");
        //Select the ECETotal for the Previous month for all IMS2 accounts
        IMS2PrevTotal = PrevResd.GetECETotal("IMS2", "ALL");
		//Select the ECETotal for the Previous month for all IPS accounts
        IPSPrevTotal = PrevResd.GetECETotal("IPS", "ALL");
        //Select the ECETotal for the Previous month for all Sage accounts
        SagePrevTotal = PrevResd.GetECETotal("Sage", "ALL");
        //Select the ECETotal for the Previous month for all WPay accounts
        WPayPrevTotal = PrevResd.GetECETotal("WPay", "ALL");
        //Select the ECETotal for the Previous month for all IPayGate accounts
        IPayGatePrevTotal = PrevResd.GetECETotal("IPayGate", "ALL");
        //Select the ECETotal for the Previous month for all InnGate accounts
        InnGatePrevTotal = PrevResd.GetECETotal("InnGate", "ALL");
        //Select the ECETotal for the Previous month for all IPayFBBH accounts
        IPayFBBHPrevTotal = PrevResd.GetECETotal("IPayFBBH", "ALL");
        //Select the ECETotal for the Previous month for all Chase accounts
        ChasePrevTotal = PrevResd.GetECETotal("Chase", "ALL");
        //Select the ECETotal for the Previous month for all Merrick accounts
        MerrickPrevTotal = PrevResd.GetECETotal("Merrick", "ALL");
        //Select the ECETotal for the Previous month for all OptimalCA accounts
        OptimalCAPrevTotal = PrevResd.GetECETotal("OptimalCA", "ALL");
        //Select the ECETotal for the Previous month for all Disc accounts
        DiscPrevTotal = PrevResd.GetECETotal("Disc", "ALL");
        //Select the ECETotal for the Previous month for all ECXLegacy accounts
        ECXLegacyPrevTotal = PrevResd.GetECETotal("ECX", "ALL");
        //Select the ECETotal for the Previous month for all Plug accounts
        PlugPrevTotal = PrevResd.GetECETotal("PlugNPay", "ALL");
        //Select the ECETotal for the Previous month for all CS accounts
        CSPrevTotal = PrevResd.GetECETotal("CS", "ALL");
        //Select the ECETotal for the Previous month for all GC accounts
        GCPrevTotal = PrevResd.GetECETotal("GC", "ALL");
        //Select the ECETotal for the Previous month for all CTCart accounts
        CTCartPrevTotal = PrevResd.GetECETotal("CTCart", "ALL");
        //Select the ECETotal for the Previous month for all MCA accounts
        MCAPrevTotal = PrevResd.GetECETotal("MCA", "ALL");
        //Select the ECETotal for the Previous month for all Payroll accounts
        PayrollPrevTotal = PrevResd.GetECETotal("Payroll", "ALL");
        //Select the ECETotal for the Previous month for all Plug accounts
        //MiscPrevTotal = PrevResd.GetECETotal("Misc", "ALL");
              
        #endregion
        //---------------------------------------------------------------------------------------------
        
        //Sum of the ECETotals, as calculated by the DATABASE totals for each of the processors
        eceTotal = IPayGateTotal + IPayTotal + IMSTotal + IMS2Total + IPSTotal + SageTotal + WPayTotal + IPayFBBHTotal + CPSTotal + InnTotal + ECXLegacyTotal + InnGateTotal + CTCartTotal + AuthnetTotal + DiscTotal + PlugTotal + ChaseTotal + OptimalCATotal + MerrickTotal + IPay2Total + IPay3Total + CSTotal + GCTotal + MCATotal + PayrollTotal;//MiscTotal;

        double ECEPrevTotal = IPayGatePrevTotal + IPayPrevTotal + IMSPrevTotal + IMS2PrevTotal + IPSPrevTotal + SagePrevTotal + WPayPrevTotal + IPayFBBHPrevTotal + CPSPrevTotal + InnPrevTotal + ECXLegacyPrevTotal + InnGatePrevTotal + CTCartPrevTotal + AuthnetPrevTotal + DiscPrevTotal + PlugPrevTotal + ChasePrevTotal + OptimalCAPrevTotal + MerrickPrevTotal + IPay2PrevTotal + IPay3PrevTotal + CSPrevTotal + GCPrevTotal + MCAPrevTotal + PayrollPrevTotal;//MiscPrevTotal;

        //Sum of the ECE Sums, as calculated by summing up each of the ECETotals for each Rep
        double eceSum = IPayGateSum + IPaySum + IMSSum + IMS2Sum + IPSSum + SageSum + WpaySum + IPayFBBHSum + CPSSum + InnSum + ECXLegacySum + InnGateSum + CTCartSum + AuthnetSum + DiscSum + PlugSum + ChaseSum + MerrickSum + OptimalCASum + IPay2Sum + IPay3Sum + CSSum + GCSum + MCASum + PayrollSum;//MiscSum;


        //*******************************End Calculate Totals*******************************
        #endregion

        #region Header
        tr = new TableRow();
        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        td = new TableCell();
        td.Attributes.Add("colspan", "30");
        td.Text = "Totals For ECE by Vendors";
        td.Style["font-family"] = "Arial";
        td.Style["font-size"] = "15px";
        td.Style["font-weight"] = "Bold";
        td.Style["Color"] = "White";
        tr.Cells.Add(td);

        tr.Cells.Add(td);
        tblTotals.Rows.Add(tr);

        tr = new TableRow();
        td = new TableCell();        
        for (int i = 0; i < arrListTotals.Count; i++)
        {
            td = new TableCell();
            lblValue = new Label();
            lblValue.Text = arrListTotals[i].ToString();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(8);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
        }


        td = new TableCell();
        lblValue = new Label();
        lblValue.Text = RepEceTotalSum.ToString();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        
        td = new TableCell();
        /*lblValue = new Label();
        lblValue.Text = "$" + ECEResidualPaymentSum.ToString();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);*/
        tr.Cells.Add(td);


        td = new TableCell();
        lblValue = new Label();
        //if the Residual Payment and Residual Total are the same print just Once
        if (ECEResidualPaymentSum == ECEResidualSum)
            lblValue.Text = "$" + ECETotalResidualPaymentSum.ToString();
        else
            lblValue.Text = "$" + ECETotalResidualPaymentSum.ToString() + " / [$" + ECEResidualSum.ToString() + "]";
        lblValue.ApplyStyle(ValueLabel);
        lblValue.ForeColor = System.Drawing.Color.Red;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        
        td = new TableCell();
        tr.Cells.Add(td);
        td = new TableCell();
        tr.Cells.Add(td);
        td = new TableCell();
        tr.Cells.Add(td);

        td = new TableCell();
        lblValue = new Label();
        lblValue.Text = ECEComm.ToString();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        td = new TableCell();
        lblValue = new Label();
        lblValue.Text = ECEReferralTotal.ToString();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        tblTotals.Rows.Add(tr);
        #endregion

        #region FirstRow (ECE Total Curr)
        tr = new TableRow();
        //ECETotal Header
        lblValue = new Label();
        lblValue.Text = "ECE Total";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPay
        lblValue = new Label();
        lblValue.Text = IPayTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        if (IPayment2Exists)
        {
            //IPay2
            lblValue = new Label();
            lblValue.Text = IPay2Total.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPayment3Exists)
        {
            //IPay3
            lblValue = new Label();
            lblValue.Text = IPay3Total.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (FBBHExists)
        {
            //IPayFBBH
            lblValue = new Label();
            lblValue.Text = IPayFBBHTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        
        if (ECXLegacyExists)
        {
            //ECXLegacy
            lblValue = new Label();
            lblValue.Text = ECXLegacyTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //IMS
        if (IMSExists)
        {
            lblValue = new Label();
            lblValue.Text = IMSTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IMS2Exists)
        {
            //IMS2
            lblValue = new Label();
            lblValue.Text = IMS2Total.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
		
		if (IPSExists)
        {
            //IPS
            lblValue = new Label();
            lblValue.Text = IPSTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (SageExists)
        {
            //Sage
            lblValue = new Label();
            lblValue.Text = SageTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Innov
        if (InovExists)
        {
            lblValue = new Label();
            lblValue.Text = InnTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //CPS
        lblValue = new Label();
        lblValue.Text = CPSTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //Disc
        if (DiscRapExists)
        {
            lblValue = new Label();
            lblValue.Text = DiscTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ChaseExists)
        {
            //Chase
            lblValue = new Label();
            lblValue.Text = ChaseTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (MerrickExists)
        {
            //Merrick
            lblValue = new Label();
            lblValue.Text = MerrickTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (OptimalCAExists)
        {
            //Opt CA
            lblValue = new Label();
            lblValue.Text = OptimalCATotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //WPay
        if (WorldPayExists)
        {
            lblValue = new Label();
            lblValue.Text = WPayTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Auth Net
        lblValue = new Label();
        lblValue.Text = AuthnetTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPayGwy
        lblValue = new Label();
        lblValue.Text = IPayGateTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //InnovGwy
        if (InovGwyExists)
        {
            lblValue = new Label();
            lblValue.Text = InnGateTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Plug N Pay
        lblValue = new Label();
        lblValue.Text = PlugTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //CS
        if (CSExists)
        {
            lblValue = new Label();
            lblValue.Text = CSTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //GC
        if (GCExists)
        {
            lblValue = new Label();
            lblValue.Text = GCTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //CTCart
        if (CTCartExists)
        {
            lblValue = new Label();
            lblValue.Text = CTCartTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //MCA
        if (MCAExists)
        {
            lblValue = new Label();
            lblValue.Text = MCATotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Payroll
        if (PayrollExists)
        {
            lblValue = new Label();
            lblValue.Text = PayrollTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Misc
        /*if (MiscExists)
        {
            lblValue = new Label();
            lblValue.Text = MiscTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }*/

        //ECE Total, as calculated by the database
        lblValue = new Label();
        lblValue.Text = eceTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue); 
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Attributes.Add("colspan", "9");
        tr.Cells.Add(td);

        tblTotals.Rows.Add(tr);
        #endregion

        #region SecondRow (Number of Accounts)
        tr = new TableRow();
        //ECETotal Header
        lblValue = new Label();
        lblValue.Text = "# of Accounts";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPay
        lblValue = new Label();
        lblValue.Text = IPayCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        if (IPayment2Exists)
        {
            //IPay2
            lblValue = new Label();
            lblValue.Text = IPay2Count.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPayment3Exists)
        {
            //IPay3
            lblValue = new Label();
            lblValue.Text = IPay3Count.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (FBBHExists)
        {
            //IPayFBBH
            lblValue = new Label();
            lblValue.Text = FbbhCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ECXLegacyExists)
        {
            //ECXLegacy
            lblValue = new Label();
            lblValue.Text = ecxLegacyCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //IMS
        if (IMSExists)
        {
            lblValue = new Label();
            lblValue.Text = ImsCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IMS2Exists)
        {
            //IMS2
            lblValue = new Label();
            lblValue.Text = Ims2Count.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
		
		if (IPSExists)
        {
            //IPS
            lblValue = new Label();
            lblValue.Text = IPSCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (SageExists)
        {
            //Sage
            lblValue = new Label();
            lblValue.Text = SageCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Innov
        if (InovExists)
        {
            lblValue = new Label();
            lblValue.Text = InnvCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //CPS
        lblValue = new Label();
        lblValue.Text = CpsCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //Disc
        if (DiscRapExists)
        {
            lblValue = new Label();
            lblValue.Text = DiscCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ChaseExists)
        {
            //Chase
            lblValue = new Label();
            lblValue.Text = ChaseCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (MerrickExists)
        {
            //Merrick
            lblValue = new Label();
            lblValue.Text = MerrickCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (OptimalCAExists)
        {
            //Opt CA
            lblValue = new Label();
            lblValue.Text = OptimalCACount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //WPay
        if (WorldPayExists)
        {
            lblValue = new Label();
            lblValue.Text = WpayCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Auth Net
        lblValue = new Label();
        lblValue.Text = AuthCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPayGwy
        lblValue = new Label();
        lblValue.Text = GateCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //InnovGwy
        if (InovGwyExists)
        {
            lblValue = new Label();
            lblValue.Text = InvgCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Plug N Pay
        lblValue = new Label();
        lblValue.Text = PlugCount.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //CS
        if (CSExists)
        {
            lblValue = new Label();
            lblValue.Text = CSCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //GC
        if (GCExists)
        {
            lblValue = new Label();
            lblValue.Text = GCCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //CTCart
        if (CTCartExists)
        {
            lblValue = new Label();
            lblValue.Text = CtcartCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //MCA
        if (MCAExists)
        {
            lblValue = new Label();
            lblValue.Text = MCACount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Payroll
        if (PayrollExists)
        {
            lblValue = new Label();
            lblValue.Text = PayrollCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Misc
        /*if (MiscExists)
        {
            lblValue = new Label();
            lblValue.Text = MiscCount.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }*/

        td = new TableCell();
        td.Attributes.Add("colspan", "10");
        tr.Cells.Add(td);

        tblTotals.Rows.Add(tr);
        #endregion

        #region ThirdRow (Current Month)
        tr = new TableRow();
        //ECETotal Header
        lblValue = new Label();
        lblValue.Text = "ECE Total";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPay
        lblValue = new Label();
        lblValue.Text = IPaySum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        if (IPayment2Exists)
        {
            //IPay2
            lblValue = new Label();
            lblValue.Text = IPay2Sum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPayment3Exists)
        {
            //IPay3
            lblValue = new Label();
            lblValue.Text = IPay3Sum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (FBBHExists)
        {
            //IPayFBBH
            lblValue = new Label();
            lblValue.Text = IPayFBBHSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ECXLegacyExists)
        {
            //ECXLegacy
            lblValue = new Label();
            lblValue.Text = ECXLegacySum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //IMS
        if (IMSExists)
        {
            lblValue = new Label();
            lblValue.Text = IMSSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IMS2Exists)
        {
            //IMS2
            lblValue = new Label();
            lblValue.Text = IMS2Sum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
		
		if (IPSExists)
        {
            //IPS
            lblValue = new Label();
            lblValue.Text = IPSSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (SageExists)
        {
            //Sage
            lblValue = new Label();
            lblValue.Text = SageSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Innov
        if (InovExists)
        {
            lblValue = new Label();
            lblValue.Text = InnSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //CPS
        lblValue = new Label();
        lblValue.Text = CPSSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //Disc
        if (DiscRapExists)
        {
            lblValue = new Label();
            lblValue.Text = DiscSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ChaseExists)
        {
            //Chase
            lblValue = new Label();
            lblValue.Text = ChaseSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (MerrickExists)
        {
            //Merrick
            lblValue = new Label();
            lblValue.Text = MerrickSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (OptimalCAExists)
        {
            //Opt CA
            lblValue = new Label();
            lblValue.Text = OptimalCASum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //WPay
        if (WorldPayExists)
        {
            lblValue = new Label();
            lblValue.Text = WpaySum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Auth Net
        lblValue = new Label();
        lblValue.Text = AuthnetSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPayGwy
        lblValue = new Label();
        lblValue.Text = IPayGateSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //InnovGwy
        if (InovGwyExists)
        {
            lblValue = new Label();
            lblValue.Text = InnGateSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Plug N Pay
        lblValue = new Label();
        lblValue.Text = PlugSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //CS
        if (CSExists)
        {
            lblValue = new Label();
            lblValue.Text = CSSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //GC
        if (GCExists)
        {
            lblValue = new Label();
            lblValue.Text = GCSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //CTCart
        if (CTCartExists)
        {
            lblValue = new Label();
            lblValue.Text = CTCartSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //MCA
        if (MCAExists)
        {
            lblValue = new Label();
            lblValue.Text = MCASum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Payroll
        if (PayrollExists)
        {
            lblValue = new Label();
            lblValue.Text = PayrollSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Misc
        /*if (MiscExists)
        {
            lblValue = new Label();
            lblValue.Text = MiscSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }*/

        //ECE Sum
        lblValue = new Label();
        lblValue.Text = eceSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Attributes.Add("colspan", "9");
        tr.Cells.Add(td);

        tblTotals.Rows.Add(tr);
        
        #endregion

        #region Fourth Row (Previous Month)
        tr = new TableRow();
        //Prev Month Header
        lblValue = new Label();
        lblValue.Text = "Previous Month";
        lblValue.Font.Bold = true;
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPay
        lblValue = new Label();
        lblValue.Text = IPayPrevTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        if (IPayment2Exists)
        {
            //IPay2
            lblValue = new Label();
            lblValue.Text = IPay2PrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IPayment3Exists)
        {
            //IPay3
            lblValue = new Label();
            lblValue.Text = IPay3PrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (FBBHExists)
        {
            //IPayFBBH
            lblValue = new Label();
            lblValue.Text = IPayFBBHPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ECXLegacyExists)
        {
            //ECXLegacy
            lblValue = new Label();
            lblValue.Text = ECXLegacyPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //IMS
        if (IMSExists)
        {
            lblValue = new Label();
            lblValue.Text = IMSPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (IMS2Exists)
        {
            //IMS2
            lblValue = new Label();
            lblValue.Text = IMS2PrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
		
		if (IPSExists)
        {
            //IPS
            lblValue = new Label();
            lblValue.Text = IPSPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (SageExists)
        {
            //Sage
            lblValue = new Label();
            lblValue.Text = SagePrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Innov
        if (InovExists)
        {
            lblValue = new Label();
            lblValue.Text = InnPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //CPS
        lblValue = new Label();
        lblValue.Text = CPSPrevTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //Disc
        if (DiscRapExists)
        {
            lblValue = new Label();
            lblValue.Text = DiscPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (ChaseExists)
        {
            //Chase
            lblValue = new Label();
            lblValue.Text = ChasePrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (MerrickExists)
        {
            //Merrick
            lblValue = new Label();
            lblValue.Text = MerrickPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        if (OptimalCAExists)
        {
            //Opt CA
            lblValue = new Label();
            lblValue.Text = OptimalCAPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //WPay
        if (WorldPayExists)
        {
            lblValue = new Label();
            lblValue.Text = WPayPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Auth Net
        lblValue = new Label();
        lblValue.Text = AuthnetPrevTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //IPayGwy
        lblValue = new Label();
        lblValue.Text = IPayGatePrevTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //InnovGwy
        if (InovGwyExists)
        {
            lblValue = new Label();
            lblValue.Text = InnGatePrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }

        //Plug N Pay
        lblValue = new Label();
        lblValue.Text = PlugPrevTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        //CS
        if (CSExists)
        {
            lblValue = new Label();
            lblValue.Text = CSPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //GC
        if (GCExists)
        {
            lblValue = new Label();
            lblValue.Text = GCPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //CTCart
        if (CTCartExists)
        {
            lblValue = new Label();
            lblValue.Text = CTCartPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //MCA
        if (MCAExists)
        {
            lblValue = new Label();
            lblValue.Text = MCAPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Payroll
        if (PayrollExists)
        {
            lblValue = new Label();
            lblValue.Text = PayrollPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }
        //Misc
        /*if (MiscExists)
        {
            lblValue = new Label();
            lblValue.Text = MiscPrevTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Controls.Add(lblValue);
            td.Attributes.Add("align", "left");
            tr.Cells.Add(td);
        }*/

        //ECE Sum
        lblValue = new Label();
        lblValue.Text = ECEPrevTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);        
        td.Attributes.Add("align", "left");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Attributes.Add("colspan", "2");
        tr.Cells.Add(td);

        td = new TableCell();
        if (User.IsInRole("Admin"))
        {
            lnkExport = new HyperLink();
            lnkExport.Text = "PRINT";
            lnkExport.NavigateUrl = "ResidualsPrint.aspx?Month=" + Month;
            lnkExport.Target = "_blank";
            lnkExport.ApplyStyle(sHyperLink);
            lnkExport.Font.Size = FontUnit.Point(10);
            td.Controls.Add(lnkExport);
        }
        tr.Cells.Add(td);

        tblTotals.Rows.Add(tr);

        #endregion

        #endregion
    }//end function PopulateResiduals

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            //lblMonth.Text = "Residual Report for the Month of " + lstMonth.SelectedItem.Text;
            //PopulateResiduals(lstMonth.SelectedItem.Value.ToString().Trim());
            PopulateACTResidualStatus(lstStatus.SelectedItem.Value.ToString().Trim(), lstService.SelectedItem.Value.ToString().Trim());
            pnlUpdateDroppedReport.Visible = false;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            //DisplayMessage("Error Loading Residuals");
            DisplayMessage(err.Message);
        }
    }
     
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            //lblMonth.Text = "Residual Report for the Month of " + lstMonth.SelectedItem.Text;
            //PopulateResiduals(lstMonth.SelectedItem.Value.ToString().Trim());
            string Status = lstStatus.SelectedItem.Value.ToString().Trim();
            string Service = lstService.SelectedItem.Value.ToString().Trim();
            string Month = lstMonth.SelectedItem.Value.ToString().Trim();
            string CONTACTID = "";
            string strProcessor = "";
            string postStatus = "";
            ResidualsAdminBL ACTResdStatus = new ResidualsAdminBL();
            DataSet ds = ACTResdStatus.GetACTResidualStatus(Status, Service);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    CONTACTID = dr["CONTACTID"].ToString().Trim();
                    //string processor = dr["Processor"].ToString().Trim();
                    //string test = "iPayment, Inc.";
                    //strProcessor = dr["Processor"].ToString().Trim();
                    if (Service == "Merchant Account")
                    {
                        if (dr["Processor"].ToString().Trim()== "iPayment, Inc.")
                        {
                            strProcessor = "iPayment3";
                        }
                        else if (dr["Processor"].ToString().Trim() == "iPayment, Inc. (1503)")
                        {
                            strProcessor = "iPayment";
                        }
                        else if (dr["Processor"].ToString().Trim() == "iPayment, Inc. (40558)")
                        {
                            strProcessor = "iPayment2";
                        }
                        else if ((dr["Processor"].ToString().Trim() == "IMS") || (dr["Processor"].ToString().Trim() == "IMS (QuickBooks)"))
                        {
                            strProcessor = "IMS2";
                        }
                        else if (dr["Processor"].ToString().Trim().Contains("Intuit Payment Solutions")) 
                        {
                            strProcessor = "IPS";
                        }
                        else if (dr["Processor"].ToString().Contains("Sage Payment Solutions"))
                        {
                            strProcessor = "Sage";
                        }
                        else if (dr["Processor"].ToString().Trim().Contains("iPayment-CPS"))
                        {
                            strProcessor = "CPS";
                        }
                        else if (dr["Processor"].ToString().Trim().Contains("Optimal-Merrick"))
                        {
                            strProcessor = "Merrick";
                        }
                        else if (dr["Processor"].ToString().Trim().Contains("Optimal-Chase"))
                        {
                            strProcessor = "Chase";
                        }
                        else if (dr["Processor"].ToString().Trim().Contains("Optimal-Canada"))
                        {
                            strProcessor = "OptimalCA";
                        }
                    }

                    if (Service == "Gateway")
                    {
                        if (dr["Gateway"].ToString().Trim() == "Authorize.net")
                        {
                            strProcessor = "Authnet";
                        }
                        else if (dr["Gateway"].ToString().Trim().Contains("QuickCommerce"))
                        {
                            strProcessor = "iPayGate";
                        }
                        else if (dr["Gateway"].ToString().Trim().Contains("Plug'n Pay"))
                        {
                            strProcessor = "PlugNPay";
                        }
                        else if (dr["Gateway"].ToString().Trim().Contains("ROAMpay"))
                        {
                            strProcessor = "ROAMPay";
                        }
                        else if (dr["Gateway"].ToString().Trim().Contains("Optimal Gateway"))
                        {
                            strProcessor = "OptimalGateway";
                        }
                        else if (dr["Gateway"].ToString().Trim().Contains("Sage Gateway"))
                        {
                            strProcessor = "SageGateway";
                        }
                        else if (dr["Gateway"].ToString().Trim().Contains("QuickBooks"))
                        {
                            strProcessor = "QuickBooksGateway";
                        }

                    }

                    

                    string statusField = "";

                    if (Service == "Merchant Account")
                    {
                        statusField = "Merchant Status";
                    }
                    else if (Service == "Gateway")
                    {
                        statusField = "Gateway";
                    }
                    else if (Service == "Check Service")
                    {
                        statusField = "Check Service";
                    }
                    else if (Service == "Gift Card")
                    {
                        statusField = "Gift Card";
                    }
                    else if (Service == "Cash Advance")
                    {
                        statusField = "Cash Advance";
                    }
                    else if (Service == "Payroll")
                    {
                        statusField = "Payroll";
                    }

                    ACTResdStatus.UpdateACTResidualStatus(Service, strProcessor, Month, Status, CONTACTID);

                    ACTDataDL ACT = new ACTDataDL();

                    DataSet dsStatus = ACT.GetACTHistStatus(CONTACTID);

                    if (dsStatus.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        DataRow drStatus = dsStatus.Tables[0].Rows[0];
                        if (Service == "Merchant Account")
                        {
                            postStatus = drStatus["MerchantStatus"].ToString().Trim();
                        }
                        else if (Service == "Gateway")
                        {
                            postStatus = drStatus["GatewayStatus"].ToString().Trim();
                        }
                        else if (Service == "Check Service")
                        {
                            postStatus = drStatus["ActiveCheckGuarantee"].ToString().Trim();
                        }
                        else if (Service == "Gift Card")
                        {
                            postStatus = drStatus["ActiveGift"].ToString().Trim();
                        }
                        else if (Service == "Payroll")
                        {
                            postStatus = drStatus["PayrollStatus"].ToString().Trim();
                        }
                        else if (Service == "Cash Advance")
                        {
                            postStatus = drStatus["MerchantFundingStatus"].ToString().Trim();
                        }

                    }

                    string preStatus = Status;


                   
                    int partnerID = Convert.ToInt32(Session["AffiliateID"]);
                   //Record a Field Change History in ACT
                    if (!(preStatus == postStatus))
                    {
                        ACT.InsertHistoryFieldChange(CONTACTID, statusField, preStatus, postStatus, partnerID);
                    }

                    DisplayMessage("Records updated.");
                }
            }
            //PopulateACTResidualStatus(lstStatus.SelectedItem.Value.ToString().Trim(), lstService.SelectedItem.Value.ToString().Trim());
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            //DisplayMessage("Error Loading Residuals");
            DisplayMessage(err.Message);
        }
    }

    protected void btnUpdateACTRates_Click(object sender, EventArgs e)
    {
        try
        {
            ResidualsAdminBL Resd = new ResidualsAdminBL();
            //string ResidualReport = lstReport.SelectedItem.Value.ToString().Trim();
            DataSet ds = Resd.UploadACTRates();
            int retVal = Resd.UpdateACTRates();

            ResidualsDL ResdDL = new ResidualsDL();
            DataSet dsResdDL = ResdDL.GetACTRatesHistory();

            if (dsResdDL.Tables[0].Rows.Count > 0)
            {
                DataRow drResdDL = null;
                for (int i = 0; i < dsResdDL.Tables[0].Rows.Count; i++)
                {
                    drResdDL = dsResdDL.Tables[0].Rows[i];
                    ACTDataDL ACT = new ACTDataDL();

                    int partnerID = Convert.ToInt32(Session["AffiliateID"]);
                    //Record a Field Change History in ACT
                    ACT.InsertHistoryFieldChange(drResdDL["CONTACTID"].ToString().Trim(), drResdDL["FieldName"].ToString().Trim(), drResdDL["PrevValue"].ToString().Trim(), drResdDL["NewValue"].ToString().Trim(), partnerID);

                }
            }

            int deleteHist = ResdDL.DeleteACTRatesHistory();

            //Resc.UpdateACTRateHistory();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = null;
                string result = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = ds.Tables[0].Rows[i];
                    if (dr["output"].ToString().Trim().StartsWith("Error string"))
                    {
                        result += dr["output"].ToString().Trim() + "<br>";
                    }
                }
                if (!result.Contains("Error"))
                {
                    result = "ACT rates updated.";
                }
                DisplayMessage(result);
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void btnGetDropped_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            PopulateDroppedReport(lstReport.SelectedItem.Value, lstMonthDropped.SelectedItem.Value);
            pnlUpdateStatus.Visible = false;
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnUpdateDropped_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            //lblMonth.Text = "Residual Report for the Month of " + lstMonth.SelectedItem.Text;
            //PopulateResiduals(lstMonth.SelectedItem.Value.ToString().Trim());
            //string Status = lstStatus.SelectedItem.Value.ToString().Trim();
            string Report = lstReport.SelectedItem.Value.ToString().Trim();
            string Month = lstMonthDropped.SelectedItem.Value.ToString().Trim();
            string MerchantNum = "";
            string DBA = "";
            string CONTACTID = "";
            string strProcessor = "";
            string postStatus = "";
            string preStatus = "";
            //ResidualsAdminBL ACTResdStatus = new ResidualsAdminBL();
            //DataSet ds = ACTResdStatus.GetACTResidualStatus(Status, Service);


            ResidualsAdminBL DroppedResd = new ResidualsAdminBL(Month);
            DataSet ds = DroppedResd.GetDroppedResiduals(Report);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    DBA = dr["DBA"].ToString().Trim();
                    
                    //CONTACTID = dr["CONTACTID"].ToString().Trim();
                    //string processor = dr["Processor"].ToString().Trim();
                    //string test = "iPayment, Inc.";
                    //strProcessor = dr["Processor"].ToString().Trim();
                    if ((Report == "iPayment") || (Report == "iPayment2") || (Report == "iPayment3") || (Report == "IPS") || (Report == "IMS2") || (Report == "Sage") || (Report == "Merrick") || (Report == "OptimalCA") || (Report == "Chase"))
                    {
                        MerchantNum = dr["Merchant Number"].ToString().Trim();
                        DataSet dsContactID = DroppedResd.GetACTContactIDByMerchantID(MerchantNum);
                        for (int j = 0; j < dsContactID.Tables[0].Rows.Count; j++)
                        {
                            DataRow drContactID = dsContactID.Tables[0].Rows[j];
                            CONTACTID = drContactID["CONTACTID"].ToString().Trim();
                        }

                    }
                    else if (Report == "Authnet")
                    {
                        MerchantNum = dr["Gateway ID"].ToString().Trim();
                        DataSet dsContactID = DroppedResd.GetACTContactIDByGatewayID(MerchantNum);
                        for (int j = 0; j < dsContactID.Tables[0].Rows.Count; j++)
                        {
                            DataRow drContactID = dsContactID.Tables[0].Rows[j];
                            CONTACTID = drContactID["CONTACTID"].ToString().Trim();
                        }
                    }
                    else
                    {
                        DataSet dsContactID = DroppedResd.GetACTContactIDByDBA(DBA);
                        for (int j = 0; j < dsContactID.Tables[0].Rows.Count; j++)
                        {
                            DataRow drContactID = dsContactID.Tables[0].Rows[j];
                            CONTACTID = drContactID["CONTACTID"].ToString().Trim();
                        }
                    }

                    ACTDataDL ACT = new ACTDataDL();

                    DataSet dsPreStatus = ACT.GetACTHistStatus(CONTACTID);

                    if (dsPreStatus.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        DataRow drPreStatus = dsPreStatus.Tables[0].Rows[0];
                        if ((Report == "iPayment") || (Report == "iPayment2") || (Report == "iPayment3") || (Report == "IPS") || (Report == "IMS2") || (Report == "Sage") || (Report == "Merrick") || (Report == "OptimalCA") || (Report == "Chase") || (Report == "CPS"))
                        {
                            preStatus = drPreStatus["MerchantStatus"].ToString().Trim();

                        }
                        else if (Report == "Authnet")
                        {
                            preStatus = drPreStatus["GatewayStatus"].ToString().Trim();
                        }else if (Report == "iPayGate")
                        {
                            preStatus = drPreStatus["GatewayStatus"].ToString().Trim();
                        }
                        else if (Report == "PlugNPay")
                        {
                            preStatus = drPreStatus["GatewayStatus"].ToString().Trim();
                        }
                    }


                    string statusField = "";

                    DroppedResd.UpdateDroppedStatus(MerchantNum, DBA, Report);

                    DataSet dsStatus = ACT.GetACTHistStatus(CONTACTID);

                    if (dsStatus.Tables[0].Rows.Count > 0)
                    {
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        DataRow drStatus = dsStatus.Tables[0].Rows[0];
                        if ((Report == "iPayment") || (Report == "iPayment2") || (Report == "iPayment3") || (Report == "IPS") || (Report == "IMS2") || (Report == "Sage") || (Report == "Merrick") || (Report == "OptimalCA") || (Report == "Chase") || (Report == "CPS"))
                        {
                            postStatus = drStatus["MerchantStatus"].ToString().Trim();
                            statusField = "Merchant Status";
                        }
                        else if (Report == "Authnet")
                        {
                            postStatus = drStatus["GatewayStatus"].ToString().Trim();
                            statusField = "Gateway Status";
                        }
                        else if (Report == "iPayGate")
                        {
                            postStatus = drStatus["GatewayStatus"].ToString().Trim();
                            statusField = "Gateway Status";
                        }
                        else if (Report == "PlugNPay")
                        {
                            postStatus = drStatus["GatewayStatus"].ToString().Trim();
                            statusField = "Gateway Status";
                        }
                    }

                    

                    int partnerID = Convert.ToInt32(Session["AffiliateID"]);
                    //Record a Field Change History in ACT
                    if (!(preStatus == postStatus))
                    {
                        ACT.InsertHistoryFieldChange(CONTACTID, statusField, preStatus, postStatus, partnerID);
                    }

                    DisplayMessage("Records updated.");
                }
            }
            //PopulateACTResidualStatus(lstStatus.SelectedItem.Value.ToString().Trim(), lstService.SelectedItem.Value.ToString().Trim());
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            //DisplayMessage("Error Loading Residuals");
            DisplayMessage(err.Message);
        }
    }
     
    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
