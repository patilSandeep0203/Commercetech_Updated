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
                string Period = string.Empty;
                if (Request.Params.Get("Month") != null)
                    Month = Request.Params.Get("Month");
                if (Request.Params.Get("Period") != null)
                    Period = Request.Params.Get("Period");

                lblMonth.Text = "Commissions/Referral Payments for the month of " + Month + "-" + Period;

                int iPeriod = 0;
                if (Period == "Full Month")
                    iPeriod = 0;
                else if (Period == "First Half")
                    iPeriod = 1;
                else if (Period == "Second Half")
                    iPeriod = 2;

                PopulateSummary(Month, iPeriod);
            }//end try
            catch (Exception err)
            {
                //CreateLog Log = new CreateLog();
                //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage(err.Message);
            }
        }//end if not postback
    }//end page load

    double CommSubTotal_emp = 0;
    double RefSubTotal_emp = 0;
    double COSubTotal_emp = 0;
    double TotalSubTotal_emp = 0;
    double PaymentSubTotal_emp = 0;

    double CommSubTotal_PrevEmp = 0;
    double RefSubTotal_PrevEmp = 0;
    double COSubTotal_PrevEmp = 0;
    double TotalSubTotal_PrevEmp = 0;
    double PaymentSubTotal_PrevEmp = 0;

    double CommSubTotal_dd = 0;
    double RefSubTotal_dd = 0;
    double COSubTotal_dd = 0;
    double TotalSubTotal_dd = 0;
    double PaymentSubTotal_dd = 0;

    double CommSubTotal_bp = 0;
    double RefSubTotal_bp = 0;
    double COSubTotal_bp = 0;
    double TotalSubTotal_bp = 0;
    double PaymentSubTotal_bp = 0;

    //This function populates summary
    public void PopulateSummary(string Month, int Period)
    {
        Label lblValue;
        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        double outSubTotal = 0;
        double FinalCommTotal = 0;
        double FinalRefTotal = 0;
        double FinalCOTotal = 0;
        double FinalTotal = 0;
        double FinalPaymentTotal = 0;

        //Get Employees Summary
        CommissionsBL CommSummary = new CommissionsBL();
        //DataSet dsEmp = CommSummary.GetCommRefPaymentByDD(1, 0, Month);
        DataSet dsEmp = new DataSet();
        if (Period == 0)
            dsEmp = CommSummary.GetCommRefPaymentByDD(1, 0, Month);
        else
            dsEmp = CommSummary.GetCommRefPaymentByMonPeriod(1, 0, Month, Period);
        if (dsEmp.Tables[0].Rows.Count > 0)
        {
            pnlEmployees.Visible = true;
            pnlCurrEmployees.Visible = true;
            grdEmployeeSummary.DataSource = dsEmp;
            grdEmployeeSummary.DataBind();

            for (int i = 0; i < grdEmployeeSummary.Rows.Count; i++)
            {
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[6].Text.ToString(), out outSubTotal))
                    CommSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[6].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    RefSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    COSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    TotalSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[10].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[11].Text.Trim());
                grdEmployeeSummary.DataBind();
            }
        }
        else
        {
            CommSubTotal_emp = 0;
            RefSubTotal_emp = 0;
            COSubTotal_emp = 0;
            TotalSubTotal_emp = 0;
            PaymentSubTotal_emp = 0;
            outSubTotal = 0;
            grdEmployeeSummary.DataSource = null;
            pnlCurrEmployees.Visible = false;
        }

        FinalCommTotal += CommSubTotal_emp;
        FinalRefTotal += RefSubTotal_emp;
        FinalCOTotal += COSubTotal_emp;
        FinalTotal += TotalSubTotal_emp;
        FinalPaymentTotal += PaymentSubTotal_emp;

        //Get Previous Employees Summary  
        outSubTotal = 0;
        CommSummary = new CommissionsBL();
        //DataSet dsPrevEmp = CommSummary.GetCommRefPaymentByDD(2, 0, Month);

        DataSet dsPrevEmp = new DataSet();
        if (Period == 0)
            dsPrevEmp = CommSummary.GetCommRefPaymentByDD(2, 0, Month);
        else
            dsPrevEmp = CommSummary.GetCommRefPaymentByMonPeriod(2, 0, Month, Period);

        if (dsPrevEmp.Tables[0].Rows.Count > 0)
        {
            pnlEmployees.Visible = true;
            pnlPrevEmployees.Visible = true;
            grdPrevEmployeeSummary.DataSource = dsPrevEmp;
            grdPrevEmployeeSummary.DataBind();
            for (int i = 0; i < grdPrevEmployeeSummary.Rows.Count; i++)
            {
                if (Double.TryParse(grdPrevEmployeeSummary.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                    CommSubTotal_PrevEmp += Convert.ToDouble(grdPrevEmployeeSummary.Rows[i].Cells[7].Text.Trim());
                if (Double.TryParse(grdPrevEmployeeSummary.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    RefSubTotal_PrevEmp += Convert.ToDouble(grdPrevEmployeeSummary.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdPrevEmployeeSummary.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    COSubTotal_PrevEmp += Convert.ToDouble(grdPrevEmployeeSummary.Rows[i].Cells[10].Text.Trim());
                if (Double.TryParse(grdPrevEmployeeSummary.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                    TotalSubTotal_PrevEmp += Convert.ToDouble(grdPrevEmployeeSummary.Rows[i].Cells[11].Text.Trim());
                if (Double.TryParse(grdPrevEmployeeSummary.Rows[i].Cells[12].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_PrevEmp += Convert.ToDouble(grdPrevEmployeeSummary.Rows[i].Cells[12].Text.Trim());
                grdPrevEmployeeSummary.DataBind();
            }
        }
        else
        {
            CommSubTotal_PrevEmp = 0;
            RefSubTotal_PrevEmp = 0;
            COSubTotal_PrevEmp = 0;
            TotalSubTotal_PrevEmp = 0;
            PaymentSubTotal_PrevEmp = 0;
            outSubTotal = 0;
            grdPrevEmployeeSummary.DataSource = null;
            pnlPrevEmployees.Visible = false;
        }

        FinalCommTotal += CommSubTotal_PrevEmp;
        FinalRefTotal += RefSubTotal_PrevEmp;
        FinalCOTotal += COSubTotal_PrevEmp;
        FinalTotal += TotalSubTotal_PrevEmp;
        FinalPaymentTotal += PaymentSubTotal_PrevEmp;

        if ((dsEmp.Tables[0].Rows.Count <= 0) && (dsPrevEmp.Tables[0].Rows.Count <= 0))
            pnlEmployees.Visible = false;

        //Get Partners Summary
        outSubTotal = 0;
        CommSummary = new CommissionsBL();
        //DataSet ds = CommSummary.GetCommRefPaymentByDD(0, 1, Month);

        DataSet ds = new DataSet();
        if (Period == 0)
            ds = CommSummary.GetCommRefPaymentByDD(0, 1, Month);
        else
            ds = CommSummary.GetCommRefPaymentByMonPeriod(0, 1, Month, Period);

        if (ds.Tables[0].Rows.Count > 0)
        {
            pnlPartners.Visible = true;
            pnlDDPartner.Visible = true;
            grdSummaryDD.DataSource = ds;
            grdSummaryDD.DataBind();
            for (int i = 0; i < grdSummaryDD.Rows.Count; i++)
            {
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[6].Text.ToString(), out outSubTotal))
                    CommSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[6].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    RefSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    COSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    TotalSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[10].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[11].Text.Trim());
                grdSummaryDD.DataBind();
            }
        }
        else
        {
            CommSubTotal_dd = 0;
            RefSubTotal_dd = 0;
            COSubTotal_dd = 0;
            TotalSubTotal_dd = 0;
            PaymentSubTotal_dd = 0;
            outSubTotal = 0;
            grdSummaryDD.DataSource = null;
            pnlDDPartner.Visible = false;
        }

        FinalCommTotal += CommSubTotal_dd;
        FinalRefTotal += RefSubTotal_dd;
        FinalCOTotal += COSubTotal_dd;
        FinalTotal += TotalSubTotal_dd;
        FinalPaymentTotal += PaymentSubTotal_dd;

        //BillPay
        outSubTotal = 0;
        CommSummary = new CommissionsBL();
        //DataSet dsBP = CommSummary.GetCommRefPaymentByDD(0, 0, Month);

        DataSet dsBP = new DataSet();
        if (Period == 0)
            dsBP = CommSummary.GetCommRefPaymentByDD(0, 0, Month);
        else
            dsBP = CommSummary.GetCommRefPaymentByMonPeriod(0, 0, Month, Period);

        if (dsBP.Tables[0].Rows.Count > 0)
        {
            pnlPartners.Visible = true;
            pnlBPPartners.Visible = true;
            grdSummaryBP.DataSource = dsBP;
            grdSummaryBP.DataBind();

            for (int i = 0; i < grdSummaryBP.Rows.Count; i++)
            {
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[6].Text.ToString(), out outSubTotal))
                    CommSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[6].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    RefSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    COSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    TotalSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[10].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[11].Text.Trim());
                grdSummaryBP.DataBind();
            }
        }
        else
        {
            CommSubTotal_bp = 0;
            RefSubTotal_bp = 0;
            COSubTotal_bp = 0;
            TotalSubTotal_bp = 0;
            PaymentSubTotal_bp = 0;
            outSubTotal = 0;
            grdSummaryBP.DataSource = null;
            pnlBPPartners.Visible = false;
        }

        FinalCommTotal += CommSubTotal_bp;
        FinalRefTotal += RefSubTotal_bp;
        FinalCOTotal += COSubTotal_bp;
        FinalTotal += TotalSubTotal_bp;
        FinalPaymentTotal += PaymentSubTotal_bp;

        if ((ds.Tables[0].Rows.Count <= 0) && (dsBP.Tables[0].Rows.Count <= 0))
            pnlPartners.Visible = false;

        //style info
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Name = "Arial";
        ValueLabel.Font.Size = 8;

        tr = new TableRow();
        td = new TableCell();
        td.Attributes.Add("ColSpan", "12");
        //tr.Cells.Add(td);
        tblTotal.Rows.Add(tr);

        lblValue = new Label();
        lblValue.Text = "GRAND TOTALS: ";
        td = new TableCell();
        td.Width = 200;
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "10");
        td.Attributes.Add("align", "left");
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalCommTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Width = 80;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = " ";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Width = 50;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalRefTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Width = 70;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalCOTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Width = 65;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Width = 50;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalPaymentTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Bold = true;
        td.Width = 65;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
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
                /*if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[11].Text = "0";*/
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "$" + Convert.ToString(CommSubTotal_dd);
                e.Row.Cells[8].Text = "$" + Convert.ToString(RefSubTotal_dd);
                e.Row.Cells[9].Text = "$" + Convert.ToString(COSubTotal_dd);
                e.Row.Cells[10].Text = "$" + Convert.ToString(TotalSubTotal_dd);
                e.Row.Cells[11].Text = "$" + Convert.ToString(PaymentSubTotal_dd);
            }
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
                /*if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[11].Text = "0";*/
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "$" + Convert.ToString(CommSubTotal_bp);
                e.Row.Cells[8].Text = "$" + Convert.ToString(RefSubTotal_bp);
                e.Row.Cells[9].Text = "$" + Convert.ToString(COSubTotal_bp);
                e.Row.Cells[10].Text = "$" + Convert.ToString(TotalSubTotal_bp);
                e.Row.Cells[11].Text = "$" + Convert.ToString(PaymentSubTotal_bp);
            }
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
                /*if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[11].Text = "0";*/
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "$" + Convert.ToString(CommSubTotal_emp);
                e.Row.Cells[8].Text = "$" + Convert.ToString(RefSubTotal_emp);
                e.Row.Cells[9].Text = "$" + Convert.ToString(COSubTotal_emp);
                e.Row.Cells[10].Text = "$" + Convert.ToString(TotalSubTotal_emp);
                e.Row.Cells[11].Text = "$" + Convert.ToString(PaymentSubTotal_emp);
            }
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
    protected void grdPrevEmployeeSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double CommTotal = 0;
                double BonusTotal = 0;
                double ReferralTotal = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out CommTotal))
                    CommTotal = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out BonusTotal))
                    BonusTotal = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                if (Double.TryParse(e.Row.Cells[9].Text.Trim(), out ReferralTotal))
                    ReferralTotal = Convert.ToDouble(e.Row.Cells[9].Text.Trim());
                if (Double.TryParse(e.Row.Cells[10].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[10].Text.Trim());
                e.Row.Cells[11].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                /*if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[12].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[12].Text = "0";*/
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = "$" + Convert.ToString(CommSubTotal_PrevEmp);
                e.Row.Cells[9].Text = "$" + Convert.ToString(RefSubTotal_PrevEmp);
                e.Row.Cells[10].Text = "$" + Convert.ToString(COSubTotal_PrevEmp);
                e.Row.Cells[11].Text = "$" + Convert.ToString(TotalSubTotal_PrevEmp);
                e.Row.Cells[12].Text = "$" + Convert.ToString(PaymentSubTotal_PrevEmp);
            }
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
}
