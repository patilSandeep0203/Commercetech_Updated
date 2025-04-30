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

public partial class Payroll_CommRefSummaryPrint : System.Web.UI.Page
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
            Response.Redirect("~/login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                string Month = string.Empty;
                if (Request.Params.Get("Month") != null)
                    Month = Request.Params.Get("Month");

                lblMonth.Text = "Commissions/Referral Payments for the month of " + Month;
                PopulateSummary(Month);
            }//end try
            catch (Exception err)
            {
                //CreateLog Log = new CreateLog();
                //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage(err.Message);
            }
        }//end if not postback
    }//end page load

    //This function populates summary
    public void PopulateSummary(string Month)
    {
        //Get Employees Summary
        CommissionsBL CommSummary = new CommissionsBL();
        DataSet dsEmp = CommSummary.GetCommRefPaymentByDD(1, 0, Month);
        if (dsEmp.Tables[0].Rows.Count > 0)
        {
            grdEmployeeSummary.DataSource = dsEmp;
            grdEmployeeSummary.DataBind();
        }

        double SubTotal = 0;
        double outSubTotal = 0;
        double FinalTotal = 0;
        for (int i = 0; i < grdEmployeeSummary.Rows.Count; i++)
        {
            if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                SubTotal = SubTotal + Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[11].Text.Trim());
        }
        lblEmployeeTotal.Text = "Sub Total: " + SubTotal.ToString();
        FinalTotal = FinalTotal + SubTotal;

        //Get Partners Summary
        CommSummary = new CommissionsBL();
        DataSet ds = CommSummary.GetCommRefPaymentByDD(0, 1, Month);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdSummaryDD.DataSource = ds;
            grdSummaryDD.DataBind();
        }

        CommSummary = new CommissionsBL();
        DataSet dsBP = CommSummary.GetCommRefPaymentByDD(0, 0, Month);
        if (dsBP.Tables[0].Rows.Count > 0)
        {
            grdSummaryBP.DataSource = dsBP;
            grdSummaryBP.DataBind();
        }

        SubTotal = 0;
        outSubTotal = 0;
        for (int i = 0; i < grdSummaryDD.Rows.Count; i++)
        {

            if (Double.TryParse(grdSummaryDD.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                SubTotal = SubTotal + Convert.ToDouble(grdSummaryDD.Rows[i].Cells[11].Text.Trim());
        }
        lblDDTotal.Text = "Sub Total: " + SubTotal.ToString();
        FinalTotal = FinalTotal + SubTotal;

        SubTotal = 0;
        outSubTotal = 0;
        for (int i = 0; i < grdSummaryBP.Rows.Count; i++)
        {

            if (Double.TryParse(grdSummaryBP.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                SubTotal = SubTotal + Convert.ToDouble(grdSummaryBP.Rows[i].Cells[11].Text.Trim());
        }
        lblBPTotal.Text = "Sub Total: " + SubTotal.ToString();
        FinalTotal = FinalTotal + SubTotal;
        lblFinalTotal.Text = "Total: " + FinalTotal.ToString();
    }
        
    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void grdSummaryDD_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double CommTotal = 0;
                double BonusTotal = 0;
                double ReferralTotal = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[6].Text.Trim(), out CommTotal))
                    CommTotal = Convert.ToDouble(e.Row.Cells[6].Text.Trim());
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out BonusTotal))
                    BonusTotal = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out ReferralTotal))
                    ReferralTotal = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                if (Double.TryParse(e.Row.Cells[9].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[9].Text.Trim());
                e.Row.Cells[10].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[11].Text = "0";
            }//end if DataRow
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void grdSummaryBP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double CommTotal = 0;
                double BonusTotal = 0;
                double ReferralTotal = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[6].Text.Trim(), out CommTotal))
                    CommTotal = Convert.ToDouble(e.Row.Cells[6].Text.Trim());
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out BonusTotal))
                    BonusTotal = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out ReferralTotal))
                    ReferralTotal = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                if (Double.TryParse(e.Row.Cells[9].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[9].Text.Trim());
                e.Row.Cells[10].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[11].Text = "0";
            }//end if DataRow
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
        
    protected void grdEmployeeSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double CommTotal = 0;
                double BonusTotal = 0;
                double ReferralTotal = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[6].Text.Trim(), out CommTotal))
                    CommTotal = Convert.ToDouble(e.Row.Cells[6].Text.Trim());
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out BonusTotal))
                    BonusTotal = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out ReferralTotal))
                    ReferralTotal = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                if (Double.TryParse(e.Row.Cells[9].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[9].Text.Trim());
                e.Row.Cells[10].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[11].Text = "0";
            }//end if DataRow
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
}
