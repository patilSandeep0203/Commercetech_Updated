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

public partial class Referrals : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Affiliate"))
                Page.MasterPageFile = "Affiliates.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Page.MasterPageFile = "Agent.master";
            else if (User.IsInRole("Employee"))
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
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");


        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                MonthBL mon = new MonthBL();
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    DataSet dsMon = mon.GetMonthListForReports(1, "referrals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }
                }
                else
                {
         
                    DataSet dsMon = mon.GetMonthListForReports(2, "referrals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
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
        sHyperLink.Font.Size = FontUnit.Point(10);
        sHyperLink.Font.Name = "Arial";

        //confirmation code
        double CarryOver = 0;
        int AffiliateID = Convert.ToInt16(Session["AffiliateID"]);
        AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
        DataSet dsCode = new DataSet();

        if (lstPeriod.SelectedValue.ToString().Trim() == "0")
        {
            dsCode = Aff.GetConfirmationCommByRepMonPeriod(lstMonth.SelectedItem.Text.ToString().Trim(), 1);
            //dsCode = Aff.GetConfirmationComm(lstMonth.SelectedItem.Text.ToString().Trim());
        }
        else
        {
           dsCode = Aff.GetConfirmationCommByRepMonPeriod(lstMonth.SelectedItem.Text.ToString().Trim(),Convert.ToInt32(lstPeriod.SelectedItem.Value));
        }
        //dsCode = Aff.GetConfirmationComm(lstMonth.SelectedItem.Text.ToString().Trim());

        if (dsCode.Tables[0].Rows.Count > 0)
        {
            if (dsCode.Tables[0].Rows[0]["CarryOverBalance"].ToString() != "")
                CarryOver = Convert.ToDouble(dsCode.Tables[0].Rows[0]["CarryOverBalance"]);
        }
        else
        {
            pnlConfirmation.Visible = false;
        }

        if (dsCode.Tables[0].Rows.Count > 0)
        {
            //int i = 0;
            for (int i = 0; i < dsCode.Tables[0].Rows.Count ; i++)
            {

                if (dsCode.Tables[0].Rows[i]["ConfirmNum"].ToString() != "")
                {
                    pnlConfirmation.Visible = true;
                    lblConfirmation.Text = "Commission paid on " + dsCode.Tables[0].Rows[i]["ConfirmDate"].ToString().Trim() + " with Confirmation Code: " + dsCode.Tables[0].Rows[i]["ConfirmNum"].ToString().Trim() + "\n";
                }

            }
        }
        else
        {
            pnlConfirmation.Visible = false;
        }

        //referral detail
        ReferralsBL ReferralsDetail = new ReferralsBL();
        DataSet ds = new DataSet();
        if (lstPeriod.SelectedValue.ToString().Trim() == "0")
        {
             ds= ReferralsDetail.GetReferralsDetail(MasterNum, Month);
        }
        else {
            ds = ReferralsDetail.GetReferralsDetailbyMonPeriod(MasterNum, Month, Convert.ToInt32(lstPeriod.SelectedItem.Value));
        }
        double fRefTotalSum = 0;
        TableRow tr = new TableRow();
        TableCell td = new TableCell();
        Label lblValue;
        Label lblStatusSTR;
        int iCount = 0;
        string[] arrColumns = { "Account Referred", "Sales Rep", "Product", "Qty", "Amount", "Referral Paid" };
        iCount = arrColumns.Length;
        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
        for (int i = 0; i < iCount; i++)
        {
            td = new TableCell();
            td.Text = arrColumns[i].ToString();
            td.Style["font-weight"] = "Bold";
            td.CssClass = "MenuHeader";
            tr.Cells.Add(td);
            tblReferrals.Rows.Add(tr);
        }
        if (ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];

                tr = new TableRow();
                lblStatusSTR = new Label();

                //Account Referred
                lblValue = new Label();
                lblValue.Text = dr["DBA"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblStatusSTR);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Sales Rep
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
                lblValue.Text = Convert.ToInt32(dr["Units"]).ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Amount
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

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblReferrals.Rows.Add(tr);

                //carry over reduction

                tr = new TableRow();
                //Monthly Total With Bonus Header
                lblValue = new Label();
                lblValue.Text = "Adjustments: ";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);


            }//end for rows

            //carryover deduction
            lblValue = new Label();
            lblValue.Text = "$" + CarryOver.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
            tblBonus.Rows.Add(tr);
            //tblReferrals.Rows.Add(tr);

            tr = new TableRow();
            //Monthly Total With Bonus Header
            lblValue = new Label();
            lblValue.Text = "Commission Total with Bonuses/Adjustments: ";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            //Final total with carryover deduction
            lblValue = new Label();
            lblValue.Text = "$" + Convert.ToString(fRefTotalSum + CarryOver);
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblBonus.Rows.Add(tr);
            //tblReferrals.Rows.Add(tr);
        }//end if count not 0
        else if (CarryOver != 0)
        {
            /*TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;*/

            tr = new TableRow();
            //Monthly Total With Bonus Header
            lblValue = new Label();
            lblValue.Text = "Adjustments: ";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            //carryover deduction
            lblValue = new Label();
            lblValue.Text = "$" + CarryOver.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
            tblBonus.Rows.Add(tr);
            //tblReferrals.Rows.Add(tr);

            tr = new TableRow();
            //Monthly Total With Bonus Header
            lblValue = new Label();
            lblValue.Text = "Commission Total with Bonuses/Adjustments: ";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            //Final total with carryover deduction
            lblValue = new Label();
            lblValue.Text = "$" + Convert.ToString(fRefTotalSum + CarryOver);
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblBonus.Rows.Add(tr);
            //tblReferrals.Rows.Add(tr);
        }
        else
        {
            DisplayMessage("No Records found for this month");
        }

        #region TIER 2 REFERRALS
        //Get the tier 2 referrals
        ReferralsDetail = new ReferralsBL();
        DataSet dsT1 = ReferralsDetail.GetT1Referrals(MasterNum, Month);
        if (dsT1.Tables[0].Rows.Count > 0)
        {
            tr = new TableRow();

            td = new TableCell();
            td.Attributes.Add("ColSpan", "5");
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
                lblValue.Text = dr["DBA"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //ReferralID
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
        td.Attributes.Add("ColSpan", "3");
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
            double fTotal = 0.0;
            if ((User.IsInRole("Admin") || User.IsInRole("Employee")) && (Session["MasterNum"].ToString() == ""))
                fTotal = PopulateReferrals("ALL", lstMonth.SelectedItem.Text);
            else
                fTotal = PopulateReferrals(Session["AffiliateID"].ToString(), lstMonth.SelectedItem.Text);
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }
    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

}
