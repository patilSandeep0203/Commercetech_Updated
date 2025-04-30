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

public partial class ReferralsAdmin : System.Web.UI.Page
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
                MonthBL mon = new MonthBL();
                DataSet dsMon = mon.GetMonthListForReports(1, "referrals");
                if (dsMon.Tables[0].Rows.Count > 0)
                {
                    lstMonth.DataSource = dsMon;
                    lstMonth.DataTextField = "Mon";
                    lstMonth.DataValueField = "Mon";
                    lstMonth.DataBind();
                }

                string MasterNum = "";
                string Month = "";
                if (Request.Params.Get("Month") != null)
                    Month = Request.Params.Get("Month");

                if (Request.Params.Get("MasterNum") != null)
                    MasterNum = Request.Params.Get("MasterNum");

                if ((Month != "") && (MasterNum != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                {
                    try
                    {
                        lblError.Visible = false;
                        lblMonth.Text = "Referrals for the month of: " + Month;
                        lstMonth.SelectedValue = lstMonth.Items.FindByValue(Month).Value;
                        double fTotal = PopulateReferrals(MasterNum, Month);
                    }//end try
                    catch (Exception err)
                    {
                        CreateLog Log = new CreateLog();
                        Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                        DisplayMessage("Error Processing Request. Please contact technical support");
                    }
                }

            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function populates the Referrals detail
    public double PopulateReferrals(string MasterNum, string Month)
    {
        double fRepTotalSum = 0;

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
        sHyperLink.Font.Size = FontUnit.Point(8);
        sHyperLink.Font.Name = "Arial";
        sHyperLink.CssClass = "One";

        ReferralsBL ReferralsDetail = new ReferralsBL();
        DataSet ds = ReferralsDetail.GetReferralsDetail(MasterNum, Month);

        TableRow tr = new TableRow();
        TableCell td = new TableCell();
        Label lblValue;
        Label lblStatusSTR;
        int iCount = 0;

        string[] arrColumns = { "Referred By", "Referral ID", "Company", "DBA", "Merchant Num", "Rep Name", "Product", "Qty", "Total", "Referral Paid", "Update" };
        if (User.IsInRole("Admin"))
            iCount = arrColumns.Length;
        else
            iCount = arrColumns.Length - 1;

        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        for (int i = 0; i < iCount; i++)
        {
            td = new TableCell();
            td.Text = arrColumns[i].ToString();
            td.Style["font-weight"] = "Bold";
            td.CssClass = "MenuHeader";
            tr.Cells.Add(td);
        }
        double fRefTotalSum = 0;
        tblReferrals.Rows.Add(tr);
        if (ds.Tables[0].Rows.Count > 0)
        {   
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                HyperLink lnkExport;
                tr = new TableRow();
                lblStatusSTR = new Label();

                //Referred By
                lblValue = new Label();
                lblValue.Text = dr["ReferredBy"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //ReferralID
                lblValue = new Label();
                lblValue.Text = dr["ReferralID"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                if (dr["OtherReferrals"].ToString().Trim() != "")
                {
                    //Company
                    lnkExport = new HyperLink();
                    lnkExport.Text = dr["Company"].ToString().Trim();
                    string strURL = "referralverify.aspx?Company=" + dr["Company"].ToString().Trim() + "&Month=" + Month;
                    string strURL2 = "window.open('" + strURL + "', 'rates','width=720,height=300,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes');";
                    lnkExport.Attributes.Add("onClick", strURL2);
                    //lnkExport.NavigateUrl = "commverify.aspx?DBA=" + dr["DBA"].ToString().Trim() + "&Month=" + Month;
                    lnkExport.Target = "_blank";
                    lnkExport.ApplyStyle(sHyperLink);
                    td = new TableCell();
                    td.Controls.Add(lblStatusSTR);
                    lnkExport.ForeColor = System.Drawing.Color.Blue;
                    td.Controls.Add(lnkExport);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }
                else
                {
                    //Company
                    lblValue = new Label();
                    lblValue.Text = dr["Company"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblStatusSTR);
                    td.Controls.Add(lblValue);
                    td.Attributes.Add("align", "left");
                    tr.Cells.Add(td);
                }
                //DBA
                lblValue = new Label();
                lblValue.Text = dr["DBA"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblStatusSTR);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Merchant Number
                lblValue = new Label();
                lblValue.Text = dr["MerchantID"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepName
                lblValue = new Label();
                lblValue.Text = dr["RepName"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Product
                lblValue = new Label();
                lblValue.Text = dr["Product"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Units
                lblValue = new Label();
                lblValue.Text = Convert.ToInt32(dr["Units"]).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Total
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble(dr["Total"]).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RefTotal
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble(dr["RefTotal"]).ToString().Trim();
                fRefTotalSum = fRefTotalSum + Convert.ToDouble(dr["RefTotal"]);
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                if (User.IsInRole("Admin"))
                {
                    //Update
                    lnkExport = new HyperLink();
                    lnkExport.Text = "Update";
                    string CommissionID = dr["CommissionID"].ToString().Trim();
                    lnkExport.NavigateUrl = "ReferralsUpdate.aspx?CommID=" + CommissionID + "&Month=" + lstMonth.SelectedItem.Text;
                    lnkExport.ApplyStyle(sHyperLink);
                    td = new TableCell();
                    td.Controls.Add(lnkExport);
                    tr.Cells.Add(td);
                }

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblReferrals.Rows.Add(tr);
            }//end for rows
        }//end if count not 0
        #region TIER 2 REFERRALS
        //Get the tier 2 referrals            

        ReferralsDetail = new ReferralsBL();
        DataSet dsT1 = ReferralsDetail.GetT1Referrals(MasterNum, Month);
        if (dsT1.Tables[0].Rows.Count > 0)
        {
            tr = new TableRow();

            td = new TableCell();
            td.Attributes.Add("ColSpan", "11");
            td.Text = "<span class=\"Labels\"><b>Tier 2 Referrals</b></span>";
            td.Attributes.Add("align", "center");
            td.CssClass = "DivGreen";
            tr.Cells.Add(td);
            tblReferrals.Rows.Add(tr);

            for (int i = 0; i < dsT1.Tables[0].Rows.Count; i++)
            {
                DataRow dr = dsT1.Tables[0].Rows[i];
                HyperLink lnkExport;
                tr = new TableRow();
                lblStatusSTR = new Label();

                //Referred By
                lblValue = new Label();
                lblValue.Text = dr["ReferredBy"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //ReferralID
                lblValue = new Label();
                lblValue.Text = dr["ReferralID"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Company
                lblValue = new Label();
                lblValue.Text = dr["Company"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblStatusSTR);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //DBA
                lblValue = new Label();
                lblValue.Text = dr["DBA"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblStatusSTR);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Merchant Number
                lblValue = new Label();
                lblValue.Text = dr["MerchantID"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepName
                lblValue = new Label();
                lblValue.Text = dr["RepName"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Product
                lblValue = new Label();
                lblValue.Text = dr["Product"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Units
                lblValue = new Label();
                lblValue.Text = Convert.ToInt32(dr["Units"]).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Total
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble(dr["Total"]).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RefTotal
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble(dr["RefTotal"]).ToString().Trim();
                fRefTotalSum = fRefTotalSum + Convert.ToDouble(dr["RefTotal"]);
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                if (User.IsInRole("Admin"))
                {
                    //Update
                    lnkExport = new HyperLink();
                    lnkExport.Text = "Update";
                    string CommissionID = dr["CommissionID"].ToString().Trim();
                    lnkExport.NavigateUrl = "ReferralsUpdate.aspx?CommID=" + CommissionID + "&Month=" + lstMonth.SelectedItem.Text;
                    lnkExport.ApplyStyle(sHyperLink);
                    td = new TableCell();
                    td.Controls.Add(lnkExport);
                    tr.Cells.Add(td);
                }

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblReferrals.Rows.Add(tr);
            }//end for rows
        }//end if T1 count not 0

        #endregion

        tr = new TableRow();

        td = new TableCell();
        td.Attributes.Add("ColSpan", "7");
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Sub Total: ";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "2");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + fRefTotalSum.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        if (User.IsInRole("Admin"))
        {
            HyperLink lnkPrint = new HyperLink();
            td = new TableCell();
            lnkPrint.Text = "PRINT";
            lnkPrint.NavigateUrl = "referralsprint.aspx?Month=" + lstMonth.SelectedItem.Value;
            lnkPrint.ApplyStyle(sHyperLink);
            lnkPrint.Target = "_Blank";
            lnkPrint.Font.Size = FontUnit.Point(10);
            td.Controls.Add(lnkPrint);
            tr.Cells.Add(td);
        }

        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
        tblReferrals.Rows.Add(tr);

        return fRepTotalSum;
    }//end populate Referrals

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            lblMonth.Text = "Referrals for the month of: " + lstMonth.SelectedItem.Text;
            double fTotal = PopulateReferrals("ALL", lstMonth.SelectedItem.Value);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Referrals");
        }
    }
    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

}
