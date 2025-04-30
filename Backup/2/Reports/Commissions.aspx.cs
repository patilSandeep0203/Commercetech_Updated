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

public partial class Commissions : System.Web.UI.Page
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
                    DataSet dsMon = mon.GetMonthListForReports(1, "commissions");
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
    
                    DataSet dsMon = mon.GetMonthListForReports(2, "commissions");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }
                }

            }//end try
            catch (Exception)
            {
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function populates the Commissions detail
    public void PopulateCommissions(string MasterNum, string Month)
    {
        double RepTotalSum = 0;

        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";
                
        CommissionsBL Commission = new CommissionsBL();
        double FundedCount = 0;
        FundedCount = Commission.ReturnFundedCount(MasterNum, Month);

        //Display Confirmation Code
        double CarryOver = 0;
        int AffiliateID = Convert.ToInt16(Session["AffiliateID"]);
        AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
        DataSet dsCode = new DataSet();

        if (lstPeriod.SelectedValue.ToString().Trim() == "0")
        {
            //dsCode = Aff.GetConfirmationComm(lstMonth.SelectedItem.Text.ToString().Trim());
            dsCode = Aff.GetConfirmationCommByRepMonPeriod(lstMonth.SelectedItem.Text.ToString().Trim(), 1);
        }
        else
        {
            dsCode = Aff.GetConfirmationCommByRepMonPeriod(lstMonth.SelectedItem.Text.ToString().Trim(), Convert.ToInt32(lstPeriod.SelectedItem.Value));
        }
        
        if (dsCode.Tables[0].Rows.Count > 0)
        {
            if (dsCode.Tables[0].Rows[0]["CarryOverBalance"].ToString() != "")
                CarryOver = Convert.ToDouble(dsCode.Tables[0].Rows[0]["CarryOverBalance"]);
        }
        else
            pnlConfirmation.Visible = false;

        if ((dsCode.Tables[0].Rows.Count > 0)&&(dsCode.Tables.Count > 0))
        {
            for (int i = 0; i <= dsCode.Tables[0].Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dsCode.Tables.Count - 1; j++)
                {
                    if (dsCode.Tables[j].Rows[i]["ConfirmNum"].ToString() != "")
                    {
                        pnlConfirmation.Visible = true;
                        lblConfirmation.Text = "Commission paid on " + dsCode.Tables[j].Rows[i]["ConfirmDate"].ToString().Trim() + " with Confirmation Code: " + dsCode.Tables[j].Rows[i]["ConfirmNum"].ToString().Trim() + "\n";
                    }
                }
            }
        }
        else
            pnlConfirmation.Visible = false;

        CommissionsBL CommissionsDetail = new CommissionsBL();
        PartnerDS.CommissionsDataTable dt = new PartnerDS.CommissionsDataTable();
        //PartnerDS.CommissionsDataTable dt  = CommissionsDetail.GetCommissionsByRepMon(MasterNum, Month);
        if (lstPeriod.SelectedValue.ToString().Trim() == "0")
            dt = CommissionsDetail.GetCommissionsByRepMon(MasterNum, Month);
        else
            dt = CommissionsDetail.GetCommissionsByRepMonPeriod(MasterNum, Month, Convert.ToInt32(lstPeriod.SelectedItem.Value));

        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            int iCount = 0;
            string[] arrColumns = { "DBA", "Sales Rep", "Referred By", "Product", "Qty", "COG (Per Unit)", "Sell Price (Per Unit)", "Total", "Profit", "Comm", "Rep Total" };
            iCount = arrColumns.Length;
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < iCount; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i].ToString();
                td.Style["font-weight"] = "Bold";
                td.CssClass = "MenuHeader";
                tr.Cells.Add(td);
            }

            tblCommissions.Rows.Add(tr);
            string strRepTotalCurr = "";
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();

                //DBA
                lblValue = new Label();
                lblValue.Text = dt[i].DBA.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = dt[i].RepName.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Referred By
                lblValue = new Label();
                lblValue.Text = dt[i].ReferredBy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Product
                lblValue = new Label();
                lblValue.Text = dt[i].Product.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Units
                lblValue = new Label();
                lblValue.Text = Convert.ToInt32( dt[i].Units).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //COG
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble( dt[i].COG).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Sell Price
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble( dt[i].Price).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Total
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble( dt[i].Total).ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);                

                //Profit
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble(dt[i].Profit).ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Commission
                lblValue = new Label();
                lblValue.Text = Convert.ToInt32(dt[i].Commission).ToString() + "%";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepTotal
                lblValue = new Label();
                if (dt[i].RepTotal.ToString().Trim() != "")
                    strRepTotalCurr = "$" + Convert.ToString(Convert.ChangeType(dt[i].RepTotal.ToString().Trim(), typeof(float)));
                lblValue.Text = strRepTotalCurr;
                RepTotalSum = RepTotalSum + Convert.ToDouble(dt[i].RepTotal);
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
             
                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblCommissions.Rows.Add(tr);
            }//end for rows
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
            lblValue.Text = "$" + RepTotalSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Attributes.Add("ColSpan", "2");
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
           
            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblCommissions.Rows.Add(tr);

            tr = new TableRow();
            td = new TableCell();
            //Commission
            lblValue = new Label();
            lblValue.Text = " Total Funded Accounts for " + lstMonth.SelectedItem.Text + ": " + FundedCount;
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Attributes.Add("ColSpan", "11");
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblCommissions.Rows.Add(tr);

            tblCommissions.Rows.Add(new TableRow());

            CommissionsBL Bonus = new CommissionsBL();
            DataSet ds = Bonus.GetBonusInfo(Session["MasterNum"].ToString(), lstMonth.SelectedItem.Value.ToString().Trim(), 2);
            ValueLabel.ForeColor = System.Drawing.Color.Black;
            ValueLabel.Font.Size = FontUnit.Small;
            ValueLabel.Font.Name = "Arial";

            ValueLabelHeader.ForeColor = System.Drawing.Color.White;
            ValueLabelHeader.Font.Size = FontUnit.Small;
            ValueLabelHeader.Font.Bold = true;
            ValueLabelHeader.Font.Name = "Arial";


            tr = new TableRow();
            td = new TableCell();
            double BonusTotal = 0;
            string [] arrColumns2 = { "Bonuses for the Month", "Reason", "Month", "RepTotal" };
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < arrColumns2.Length; i++)
            {
                td = new TableCell();
                td.Text = arrColumns2[i].ToString();
                td.Style["font-weight"] = "Bold";
                td.CssClass = "MenuHeader";
                tr.Cells.Add(td);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                tblBonus.Rows.Add(tr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    tr = new TableRow();

                    //Bonus Desc
                    lblValue = new Label();
                    lblValue.Text = dr["BonusDesc"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Reason
                    lblValue = new Label();
                    lblValue.Text = dr["Reason"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Month
                    lblValue = new Label();
                    lblValue.Text = dr["Mon"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //RepTotal
                    lblValue = new Label();
                    lblValue.Text = "$" + dr["RepTotal"].ToString().Trim();
                    BonusTotal = BonusTotal + Convert.ToDouble(dr["RepTotal"]);
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblBonus.Rows.Add(tr);
                }//end for

                tr = new TableRow();
                td = new TableCell();
                td.Attributes.Add("ColSpan", "3");
                tr.Cells.Add(td);

                //Bonus Total Header
                lblValue = new Label();
                lblValue.Text = "Bonus Total: ";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Bonus Total
                lblValue = new Label();
                lblValue.Text = "$" + BonusTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblBonus.Rows.Add(tr);
            }//end if count not 0


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
            lblValue.Text = "$" + Convert.ToString(RepTotalSum+CarryOver);
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblBonus.Rows.Add(tr);

        }//end if count not 0
        else if (CarryOver != 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;

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
            lblValue.Text = "$" + Convert.ToString(RepTotalSum + CarryOver);
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblBonus.Rows.Add(tr);
        }
        else
        {
            DisplayMessage("No Records found for this month");
        }
 
    }//end populate commissions

    
    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            RepInfoBL repInfo = new RepInfoBL();

            string MasterNum = Session["MasterNum"].ToString();

            lblError.Visible = false;
            lblMonth.Text = "Commissions for the month of: " + lstMonth.SelectedItem.Text;
            if ((User.IsInRole("Admin") || User.IsInRole("Employee")) && (Session["MasterNum"].ToString() == ""))
            {
                PopulateCommissions("ALL", lstMonth.SelectedItem.Text);
            }
            else if (User.IsInRole("Office"))
            {
                DataSet dsOfficeAgentnum = repInfo.ReturnOfficeAgentMasterNum(MasterNum);
                if (dsOfficeAgentnum.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsOfficeAgentnum.Tables[0].Rows.Count; i++)
                    {
                        DataRow drOfficeAgentnum = dsOfficeAgentnum.Tables[0].Rows[i];
                        //ResidualsBL Residuals1 = new ResidualsBL(Month, Convert.ToString(drOfficeAgentnum["MasterNum"]));

                        //DataSet dsTotals;
                        if (!Convert.IsDBNull(drOfficeAgentnum["MasterNum"]))
                        {
                            PopulateCommissions(drOfficeAgentnum["MasterNum"].ToString(), lstMonth.SelectedItem.Text);
                        }
                    }
                }
            }
            else
                PopulateCommissions(Session["MasterNum"].ToString(), lstMonth.SelectedItem.Text);
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }
}
