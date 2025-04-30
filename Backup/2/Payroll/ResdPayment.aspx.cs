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

public partial class Payroll_ResdSummary : System.Web.UI.Page
{
    private static ResidualsAdminBL Resd = new ResidualsAdminBL();
    private static Boolean archived = false;
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
                MonthBL Mon = new MonthBL();
               DataSet dsMon = Mon.GetMonthListForReports(0, "resdpayment");

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
                //CreateLog Log = new CreateLog();
                //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage(err.Message);
            }
        }//end if not postback
    }//end page load

    double PaymentSubTotal_emp = 0;
    double ResidualSubTotal_emp = 0;
    double CarryOverSubTotal_emp = 0;
    double TotalSubTotal_emp = 0;
    double PaymentSubTotal_dd = 0;
    double ResidualSubTotal_dd = 0;
    double CarryOverSubTotal_dd = 0;
    double TotalSubTotal_dd = 0;
    double PaymentSubTotal_bp = 0;
    double ResidualSubTotal_bp = 0;
    double CarryOverSubTotal_bp = 0;
    double TotalSubTotal_bp = 0;

    //This function populates summary
    public void PopulateSummary(string Month)
    {
        //Style info
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Name = "Arial";
        ValueLabel.Font.Size = 14;
        Label lblValue;
        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        lnkPrint.Visible = true;
        lnkPrint.NavigateUrl = "ResdPaymentPrint.aspx?Month=" + Month;

        double outSubTotal = 0;
        double FinalPaymentTotal = 0;
        double FinalTotal = 0;
        double FinalResTotal = 0;
        double FinalCOTotal = 0;
        //Get Employees Summary        
        Resd = new ResidualsAdminBL(Month);
        MonthBL Mon = new MonthBL(Month);

        //Define two Datasets
        DataSet dsEmp = null; //Employee
        DataSet dsBP = null; //Partner with Bill Pay
        DataSet dsDD = null; //Partner with Direct Deposit 

        //if month selected is the current residual month
        if (Month.Trim() == Resd.ReturnCurrMonth().Trim())
        {
            archived = false;
            //calculate the payment amount based on database totals 
            lblMonth.Text = "Residual Payment Calculation for " + lstMonth.SelectedItem.Text + "</br>";
            lblMonth.Text += "Note: This Summary will be finalized and cannot be changed after the Residual Posting Date";
  
            dsBP = Resd.GetResidualPaymentCalcSummaryByDD(0, 0);  
            dsEmp = Resd.GetResidualPaymentCalcSummaryByDD(1, 0);
            dsDD = Resd.GetResidualPaymentCalcSummaryByDD(0, 1);
        }
        else //past month selected
        {
            archived = true;
            lblMonth.Text = "Residual Payment Information for the month of " + lstMonth.SelectedItem.Text;
            //get the payment amount from archived information  
            dsEmp = Resd.GetResidualPaymentSummaryByDD(1, 0);
            dsBP = Resd.GetResidualPaymentSummaryByDD(0, 0);  
            dsDD = Resd.GetResidualPaymentSummaryByDD(0, 1);            
        }

        if (dsEmp.Tables[0].Rows.Count > 0)
        {
            pnlEmployees.Visible = true;
            grdEmployeeSummary.DataSource = dsEmp;
            grdEmployeeSummary.DataBind();
            //sum up all the payment totals in the grid
            for (int i = 0; i < grdEmployeeSummary.Rows.Count; i++)
            {
                //if a past month was selected, disable the Edit button
                /*if (archived) 
                    grdEmployeeSummary.Rows[i].Cells[0].Text = "";*/
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                    ResidualSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[7].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    CarryOverSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    TotalSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdEmployeeSummary.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_emp += Convert.ToDouble(grdEmployeeSummary.Rows[i].Cells[10].Text.Trim());
                grdEmployeeSummary.DataBind();
            }
        }
        else
        {
            ResidualSubTotal_emp = 0;
            CarryOverSubTotal_emp = 0;
            PaymentSubTotal_emp = 0;
            TotalSubTotal_emp = 0;
            outSubTotal = 0;
            grdEmployeeSummary.DataSource = null;
            pnlEmployees.Visible = false;
        }

        #region EmpOldTotals
        /*
        tr = new TableRow();
        td = new TableCell();
        td.Attributes.Add("ColSpan", "11");
        //tr.Cells.Add(td);
        tblEmployeeTotal.Rows.Add(tr);

        lblValue = new Label();
        lblValue.Text = "Employees Sub Totals: ";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "9");
        td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Residual: $" + ResidualSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Carryover: $" + CarryOverSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Total: $" + TotalSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Payment: $" + PaymentSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        */
        #endregion

        //lblEmployeeTotal.Text = "Payment Sub Total: " + SubTotal.ToString();
        FinalPaymentTotal += PaymentSubTotal_emp;
        FinalResTotal += ResidualSubTotal_emp;
        FinalCOTotal += CarryOverSubTotal_emp;
        FinalTotal += TotalSubTotal_emp;

        //Get Partners Summary        
        outSubTotal = 0;

        if (dsDD.Tables[0].Rows.Count > 0)
        {
            pnlPartners.Visible = true;
            pnlDDPartner.Visible = true;
            grdSummaryDD.DataSource = dsDD;
            grdSummaryDD.DataBind();
            for (int i = 0; i < grdSummaryDD.Rows.Count; i++)
            {
                //if a past month was selected, disable the Edit button
                /*if (archived)
                    grdSummaryDD.Rows[i].Cells[0].Text = "";*/
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                    ResidualSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[7].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    CarryOverSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    TotalSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdSummaryDD.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_dd += Convert.ToDouble(grdSummaryDD.Rows[i].Cells[10].Text.Trim());
                grdSummaryDD.DataBind();
            }
        }
        else
        {
            ResidualSubTotal_dd = 0;
            CarryOverSubTotal_dd = 0;
            PaymentSubTotal_dd = 0;
            TotalSubTotal_dd = 0;
            outSubTotal = 0;
            grdSummaryDD.DataSource = null;
            pnlDDPartner.Visible = false;
        }
        #region DDOldTotals
        /*
        tr = new TableRow();
        td = new TableCell();
        td.Attributes.Add("ColSpan", "11");
        //tr.Cells.Add(td);
        tblDDTotal.Rows.Add(tr);

        lblValue = new Label();
        lblValue.Text = "Direct Deposit Sub Totals: ";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "9");
        td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Residual: $" + ResidualSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Carryover: $" + CarryOverSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Total: $" + TotalSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Payment: $" + PaymentSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        */
        #endregion

        FinalPaymentTotal += PaymentSubTotal_dd;
        FinalResTotal += ResidualSubTotal_dd;
        FinalCOTotal += CarryOverSubTotal_dd;
        FinalTotal += TotalSubTotal_dd;

        /*lblDDTotal.Text = "Payment Sub Total: " + SubTotal.ToString();
        FinalTotal = FinalTotal + SubTotal;

        ResidualSubTotal = 0; 
        SubTotal = 0;
        outSubTotal = 0;*/

        outSubTotal = 0;

        if (dsBP.Tables[0].Rows.Count > 0)
        {
            pnlPartners.Visible = true;
            pnlBPPartners.Visible = true;
            grdSummaryBP.DataSource = dsBP;
            grdSummaryBP.DataBind();
            for (int i = 0; i < grdSummaryBP.Rows.Count; i++)
            {
                //if a past month was selected, disable the Edit button
                /*if (archived)
                    grdSummaryBP.Rows[i].Cells[0].Text = "";*/
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                    ResidualSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[7].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    CarryOverSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    TotalSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdSummaryBP.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_bp += Convert.ToDouble(grdSummaryBP.Rows[i].Cells[10].Text.Trim());
                grdSummaryBP.DataBind();
            }
        }
        else
        {
            ResidualSubTotal_bp = 0;
            CarryOverSubTotal_bp = 0;
            PaymentSubTotal_bp = 0;
            outSubTotal = 0;
            TotalSubTotal_bp = 0;
            grdSummaryBP.DataSource = null;
            pnlBPPartners.Visible = false;
        }

        #region BPOldTotals
        /*tr = new TableRow();
        td = new TableCell();
        td.Attributes.Add("ColSpan", "11");
        //tr.Cells.Add(td);
        tblBPTotal.Rows.Add(tr);

        lblValue = new Label();
        lblValue.Text = "Bill Pay Sub Totals: ";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "9");
        td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Residual: $" + ResidualSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Carryover: $" + CarryOverSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "Total: $" + TotalSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        
        lblValue = new Label();
        lblValue.Text = "Payment: $" + PaymentSubTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        //td.Attributes.Add("align", "left");
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        */
        #endregion

        FinalPaymentTotal += PaymentSubTotal_bp;
        FinalResTotal += ResidualSubTotal_bp;
        FinalCOTotal += CarryOverSubTotal_bp;
        FinalTotal +=TotalSubTotal_bp;

        tr = new TableRow();
        td = new TableCell();
        td.Attributes.Add("ColSpan", "10");
        //tr.Cells.Add(td);
        tblBPTotal.Rows.Add(tr);

        lblValue = new Label();
        lblValue.Text = "GRAND TOTALS: ";
        td = new TableCell();
        td.Width = 100;
        lblValue.ApplyStyle(ValueLabel);
        td.Attributes.Add("ColSpan", "10");
        td.Attributes.Add("align", "left");
        lblValue.Font.Size = 12;
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalResTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = 12;
        lblValue.Font.Bold = true;
        td.Width = 62;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalCOTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = 12;
        lblValue.Font.Bold = true;
        td.Width = 63;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = 12;
        lblValue.Font.Bold = true;
        td.Width = 50;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        lblValue = new Label();
        lblValue.Text = "$" + FinalPaymentTotal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = 12;
        lblValue.Font.Bold = true;
        td.Width = 60;
        td.Attributes.Add("align", "center");
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);
        /*lblBPTotal.Text = "Payment Sub Total: " + SubTotal.ToString();

        if ((dsDD.Tables[0].Rows.Count <= 0) && (dsBP.Tables[0].Rows.Count <= 0))
            pnlPartners.Visible = false;
        

        FinalTotal = FinalTotal + SubTotal;
        lblFinalTotal.Text = "Payment Total: " + FinalTotal.ToString();*/
    }

    //This function populates info to be updated
    public void PopulateUpdate(int AffiliateID)
    {
        if (User.IsInRole("Admin"))
        {
            AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
            DataSet ds = Aff.GetResidualPaymentByAffiliateID(lstMonth.SelectedItem.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblAffiliateID.Text = AffiliateID.ToString();
                lblCompanyName.Text = dr["CompanyName"].ToString().Trim();
                txtConfirmationCode.Text = dr["ResdConfirmNum"].ToString().Trim();
                txtNote.Text = dr["ResdNote"].ToString().Trim();
                lblResidual.Text = dr["Residual"].ToString().Trim();
                txtCarryOver.Text = dr["CarryOverResd"].ToString().Trim();

                if (dr["ResdConfirmNum"].ToString().Trim() != "")
                {
                    txtDatePaid.Text = dr["ResdConfirmDate"].ToString().Trim();
                    txtPayment.Text = dr["Payment"].ToString().Trim();
                }
                else
                {
                    txtDatePaid.Text = DateTime.Now.Date.ToString();
                    txtNote.Text = "Residual for " + lstMonth.SelectedValue;
                    txtCarryOver.Text = "0.00";
                    txtPayment.Text = Convert.ToString(Convert.ToDecimal(dr["Residual"]) + Convert.ToDecimal(dr["CarryOverResd"]));
                }
            }//end if count not 0
            else
                DisplayMessage("Rep information not found");
        }//end if user is in role
    }//end function PopulateUpdate

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                //Update product information
                if (txtConfirmationCode.Text.Trim() != "")
                {
                    int iRetVal = Resd.InsertUpdateResdConfirm(lblAffiliateID.Text.Trim(), txtConfirmationCode.Text.Trim(), txtNote.Text.Trim(),
                                  Convert.ToDecimal(txtCarryOver.Text.Trim()), Convert.ToDecimal(txtPayment.Text.Trim()), txtDatePaid.Text.Trim());
                }
                else
                {
                    int iRetVal = Resd.InsertUpdateResdConfirm(lblAffiliateID.Text.Trim(), txtConfirmationCode.Text.Trim(), txtNote.Text.Trim(),
                                  Convert.ToDecimal(txtCarryOver.Text.Trim()), Convert.ToDecimal(null), txtDatePaid.Text.Trim());
                }
                pnlConfirmation.Visible = false;
                PopulateSummary(lstMonth.SelectedItem.Text);
            }//end if access
            else
                DisplayMessage("Cannot Update");
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblError.Visible = false;
        pnlConfirmation.Visible = false;
    }

    protected void grdSummaryDD_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double ResdPay = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out ResdPay))
                    ResdPay = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                e.Row.Cells[9].Text = Convert.ToString(ResdPay + CarryOver);
                /*
                if  ((ResdPay + CarryOver)  > 0)
                    e.Row.Cells[10].Text = Convert.ToString(ResdPay + CarryOver);
                else
                    e.Row.Cells[10].Text = "0";
                */
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = "$" + Convert.ToString(ResidualSubTotal_dd);
                e.Row.Cells[8].Text = "$" + Convert.ToString(CarryOverSubTotal_dd);
                e.Row.Cells[9].Text = "$" + Convert.ToString(TotalSubTotal_dd);
                e.Row.Cells[10].Text = "$" + Convert.ToString(PaymentSubTotal_dd);
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
                double ResdPay = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out ResdPay))
                    ResdPay = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                e.Row.Cells[9].Text = Convert.ToString(ResdPay + CarryOver);
                /*
                if ((ResdPay + CarryOver) > 0)
                    e.Row.Cells[10].Text = Convert.ToString(ResdPay + CarryOver);
                else
                    e.Row.Cells[10].Text = "0";
                */
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = "$" + Convert.ToString(ResidualSubTotal_bp);
                e.Row.Cells[8].Text = "$" + Convert.ToString(CarryOverSubTotal_bp);
                e.Row.Cells[9].Text = "$" + Convert.ToString(TotalSubTotal_bp);
                e.Row.Cells[10].Text = "$" + Convert.ToString(PaymentSubTotal_bp);
            }
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateSummary(lstMonth.SelectedItem.Text.Trim());
        }//end try
        catch (Exception err)
        {
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
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out ResdPay))
                    ResdPay = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                e.Row.Cells[9].Text = Convert.ToString(ResdPay + CarryOver);

                /*
                if ((ResdPay + CarryOver) > 0)
                    e.Row.Cells[10].Text = Convert.ToString(ResdPay + CarryOver);
                else
                    e.Row.Cells[10].Text = "0";*/

            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = "$" + Convert.ToString(ResidualSubTotal_emp);
                e.Row.Cells[8].Text = "$" + Convert.ToString(CarryOverSubTotal_emp);
                e.Row.Cells[9].Text = "$" + Convert.ToString(TotalSubTotal_emp);
                e.Row.Cells[10].Text = "$" + Convert.ToString(PaymentSubTotal_emp);
            }
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void grdEmployeeSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString() == "Confirmation")
            {
                pnlConfirmation.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdEmployeeSummary.Rows[index];
                int AffiliateID = Convert.ToInt16 (Server.HtmlDecode(grdRow.Cells[4].Text) );
                pnlConfirmation.Visible = true;
                PopulateUpdate(AffiliateID);
            }
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void grdSummaryDD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString() == "Confirmation")
            {
                pnlConfirmation.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdSummaryDD.Rows[index];
                int AffiliateID = Convert.ToInt16(Server.HtmlDecode(grdRow.Cells[4].Text));
                pnlConfirmation.Visible = true;
                PopulateUpdate(AffiliateID);
            }
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void grdSummaryBP_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString() == "Confirmation")
            {
                pnlConfirmation.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdSummaryBP.Rows[index];
                int AffiliateID = Convert.ToInt16(Server.HtmlDecode(grdRow.Cells[4].Text) );
                pnlConfirmation.Visible = true;
                PopulateUpdate(AffiliateID);
            }
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
}
