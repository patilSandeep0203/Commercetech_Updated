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

public partial class Payroll_ResdSummaryPrint : System.Web.UI.Page
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

                lblMonth.Text = "Residual Payments for the month of " + Month;
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

    double PaymentSubTotal_emp = 0;
    double TotalSubTotal_emp = 0;
    double ResidualSubTotal_emp = 0;
    double CarryOverSubTotal_emp = 0;

    double PaymentSubTotal_dd = 0;
    double TotalSubTotal_dd = 0;
    double ResidualSubTotal_dd = 0;
    double CarryOverSubTotal_dd = 0;

    double PaymentSubTotal_bp = 0;
    double TotalSubTotal_bp = 0;
    double ResidualSubTotal_bp = 0;
    double CarryOverSubTotal_bp = 0;

    //This function populates summary
    public void PopulateSummary(string Month)
    {
        double outSubTotal = 0;
        double ResidualTotal = 0;
        double COTotal = 0;
        double FinalTotal = 0;
        double PaymentTotal = 0;
        //Get Employees Summary
        ResidualsAdminBL ResdSummary = new ResidualsAdminBL(Month);
        DataSet dsEmp = ResdSummary.GetResidualPaymentSummaryByDD(1, 0);
        if (dsEmp.Tables[0].Rows.Count > 0)
        {
            grdEmployeeSummary.DataSource = dsEmp;
            grdEmployeeSummary.DataBind();
        }
        for (int i = 0; i < grdEmployeeSummary.Rows.Count; i++)
        {
            if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                PaymentSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[9].Text.Trim());
            if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[6].Text.ToString(), out outSubTotal))
                ResidualSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[6].Text.Trim());
            if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                CarryOverSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[7].Text.Trim());
            if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                TotalSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[8].Text.Trim());
        }

        //lblEmployeeTotal.Text = "Sub Total: " + PaymentSubTotal_emp.ToString();
        ResidualTotal += ResidualSubTotal_emp;
        COTotal += CarryOverSubTotal_emp;
        FinalTotal += TotalSubTotal_emp;
        PaymentTotal += PaymentSubTotal_emp;

        grdEmployeeSummary.DataBind();

        //Get Partners Summary
        DataSet ds = ResdSummary.GetResidualPaymentSummaryByDD(0, 1);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdSummaryDD.DataSource = ds;
            grdSummaryDD.DataBind();
        }

        DataSet dsBP = ResdSummary.GetResidualPaymentSummaryByDD(0, 0);
        if (dsBP.Tables[0].Rows.Count > 0)
        {
            grdSummaryBP.DataSource = dsBP;
            grdSummaryBP.DataBind();
        }

        outSubTotal = 0;
        for (int i = 0; i < grdSummaryDD.Rows.Count; i++)
        {
            if (Double.TryParse(grdSummaryDD.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                PaymentSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[9].Text.Trim());
            if (Double.TryParse(grdSummaryDD.Rows[i].Cells[6].Text.ToString(), out outSubTotal))
                ResidualSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[6].Text.Trim());
            if (Double.TryParse(grdSummaryDD.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                CarryOverSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[7].Text.Trim());
            if (Double.TryParse(grdSummaryDD.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                TotalSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[8].Text.Trim());
        }
        //lblDDTotal.Text = "Sub Total: " + PaymentSubTotal_dd.ToString();
        ResidualTotal += ResidualSubTotal_dd;
        COTotal += CarryOverSubTotal_dd;
        FinalTotal += TotalSubTotal_dd;
        PaymentTotal += PaymentSubTotal_dd;
        grdSummaryDD.DataBind();

        outSubTotal = 0;
        for (int i = 0; i < grdSummaryBP.Rows.Count; i++)
        {
            if (Double.TryParse(grdSummaryBP.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                PaymentSubTotal_bp = PaymentSubTotal_bp + Convert.ToDouble(grdSummaryBP.Rows[i].Cells[9].Text.Trim());
            if (Double.TryParse(grdSummaryBP.Rows[i].Cells[6].Text.ToString(), out outSubTotal))
                ResidualSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[6].Text.Trim());
            if (Double.TryParse(grdSummaryBP.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                CarryOverSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[7].Text.Trim());
            if (Double.TryParse(grdSummaryBP.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                TotalSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[8].Text.Trim());
        }
        //lblBPTotal.Text = "Sub Total: " + PaymentSubTotal_bp.ToString();
        ResidualTotal += ResidualSubTotal_bp;
        COTotal += CarryOverSubTotal_bp;
        FinalTotal += TotalSubTotal_bp;
        PaymentTotal += PaymentSubTotal_bp;
        grdSummaryBP.DataBind();

        #region FinalTotals
        //Style info
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";

        Label lblValue;

        TableRow tr = new TableRow();
        TableCell td = new TableCell();
        td.Attributes.Add("ColSpan", "10");
        //tr.Cells.Add(td);
        tblTotal.Rows.Add(tr);
        lblValue = new Label();
        lblValue.Text = "GRAND TOTALS:";
        td.Width = 100;
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "10");
        td.Attributes.Add("align", "left");
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + ResidualTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Width = 62;
        td.Attributes.Add("align", "center");
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + COTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Width = 68;
        td.Attributes.Add("align", "center");
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "center");
        td.Width = 50;
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + PaymentTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        td.Attributes.Add("align", "center");
        td.Width = 60;
        tr.Cells.Add(td);
        #endregion
        //lblFinalTotal.Text = "Total: " + FinalTotal.ToString();
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
                double ResdPay = 0;
                double CarryOver = 0;
                
                if (Double.TryParse(e.Row.Cells[6].Text.Trim(), out ResdPay))
                    ResdPay = Convert.ToDouble(e.Row.Cells[6].Text.Trim()) + Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                e.Row.Cells[8].Text = Convert.ToString(ResdPay);

            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "$" + Convert.ToString(ResidualSubTotal_dd);
                e.Row.Cells[7].Text = "$" + Convert.ToString(CarryOverSubTotal_dd);
                e.Row.Cells[8].Text = "$" + Convert.ToString(TotalSubTotal_dd);
                e.Row.Cells[9].Text = "$" + Convert.ToString(PaymentSubTotal_dd);
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void grdSummaryBP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double ResdPay = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[6].Text.Trim(), out ResdPay))
                    ResdPay = Convert.ToDouble(e.Row.Cells[6].Text.Trim()) + Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                e.Row.Cells[8].Text = Convert.ToString(ResdPay);
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[9].Text = "$" + Convert.ToString(PaymentSubTotal_bp);
                e.Row.Cells[7].Text = "$" + Convert.ToString(CarryOverSubTotal_bp);
                e.Row.Cells[8].Text = "$" + Convert.ToString(TotalSubTotal_bp);
                e.Row.Cells[6].Text = "$" + Convert.ToString(ResidualSubTotal_bp);
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
                double ResdPay = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[6].Text.Trim(), out ResdPay))
                    ResdPay = Convert.ToDouble(e.Row.Cells[6].Text.Trim()) + Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                e.Row.Cells[8].Text = Convert.ToString(ResdPay);

            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "$" + Convert.ToString(ResidualSubTotal_emp);
                e.Row.Cells[7].Text = "$" + Convert.ToString(CarryOverSubTotal_emp);
                e.Row.Cells[8].Text = "$" + Convert.ToString(TotalSubTotal_emp);
                e.Row.Cells[9].Text = "$" + Convert.ToString(PaymentSubTotal_emp);
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
