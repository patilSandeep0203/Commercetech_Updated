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

public partial class Payroll_PartnerPaymentHistory : System.Web.UI.Page
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
                int PartnerID = Convert.ToInt32(Session["AffiliateID"].ToString());
                AffiliatesBL Partner = new AffiliatesBL(PartnerID);
                DataSet ds = Partner.GetAffiliateList();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                        item.Text = dr["DBA"].ToString().Trim() + " - " + dr["Contact"].ToString().Trim();
                        item.Value = dr["AffiliateID"].ToString().Trim();
                        lstPartnerName.Items.Add(item);
                    }
                    //lstPartnerName.DataSource = ds;
                    //lstPartnerName.DataTextField = "Contact";
                    //lstPartnerName.DataValueField = "AffiliateID";
                    //lstPartnerName.
                    //lstPartnerName.DataBind();
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            PopulateSummary(Convert.ToInt32(lstPartnerName.SelectedItem.Value));
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    double ResdSubTotal_R = 0;
    double COSubTotal_R = 0;
    double TotalSubTotal_R = 0;
    double PaymentSubTotal_R = 0;

    double CommSubTotal_C = 0;
    double RefSubTotal_C = 0;
    double COSubTotal_C = 0;
    double TotalSubTotal_C = 0;
    double PaymentSubTotal_C = 0;

    double CommSubTotal_RC = 0;
    double RefSubTotal_RC = 0;
    double ResdSubTotal_RC = 0;
    double COSubTotal_RC = 0;
    double TotalSubTotal_RC = 0;
    double PaymentSubTotal_RC = 0;

    //This function populates summary
    public void PopulateSummary(int PartnerID)
    {
        double outSubTotal = 0;
        double FinalCommTotal = 0;
        double FinalRefTotal = 0;
        double FinalResdTotal = 0;
        double FinalCOTotal = 0;
        double FinalTotal = 0;
        double FinalPaymentTotal = 0;

        //Get Residual and Commission Summary        
        CommissionsBL ResdCommPay = new CommissionsBL();
        DataSet dsResdComm = new DataSet();
        dsResdComm = ResdCommPay.GetResdCommPayHistory(PartnerID);

        if (dsResdComm.Tables[0].Rows.Count > 0)
        {
            pnlResdCommHistory.Visible = true;
            grdResdCommHistory.DataSource = dsResdComm;
            grdResdCommHistory.DataBind();
            for (int i = 0; i < grdResdCommHistory.Rows.Count; i++)
            {
                if (Double.TryParse(grdResdCommHistory.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    CommSubTotal_RC += Convert.ToDouble(grdResdCommHistory.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdResdCommHistory.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    RefSubTotal_RC += Convert.ToDouble(grdResdCommHistory.Rows[i].Cells[10].Text.Trim());
                if (Double.TryParse(grdResdCommHistory.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                    ResdSubTotal_RC += Convert.ToDouble(grdResdCommHistory.Rows[i].Cells[11].Text.Trim());
                if (Double.TryParse(grdResdCommHistory.Rows[i].Cells[12].Text.ToString(), out outSubTotal))
                    COSubTotal_RC += Convert.ToDouble(grdResdCommHistory.Rows[i].Cells[12].Text.Trim());
                if (Double.TryParse(grdResdCommHistory.Rows[i].Cells[13].Text.ToString(), out outSubTotal))
                    TotalSubTotal_RC += Convert.ToDouble(grdResdCommHistory.Rows[i].Cells[13].Text.Trim());
                if (Double.TryParse(grdResdCommHistory.Rows[i].Cells[14].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_RC += Convert.ToDouble(grdResdCommHistory.Rows[i].Cells[14].Text.Trim());
                grdResdCommHistory.DataBind();
            }
        }
        else
        {
            CommSubTotal_RC = 0;
            RefSubTotal_RC = 0;
            ResdSubTotal_RC = 0;
            COSubTotal_RC = 0;
            TotalSubTotal_RC = 0;
            PaymentSubTotal_RC = 0;
            outSubTotal = 0;
            grdResdCommHistory.DataSource = null;
            pnlResdCommHistory.Visible = false;
        }

        FinalCommTotal += CommSubTotal_RC;
        FinalRefTotal += RefSubTotal_RC;
        FinalResdTotal += ResdSubTotal_RC;
        FinalCOTotal += COSubTotal_RC;
        FinalTotal += TotalSubTotal_RC;
        FinalPaymentTotal += PaymentSubTotal_RC;

        outSubTotal = 0;

        //Get Residual Summary        
        ResidualsBL ResdPay = new ResidualsBL("", "");
        DataSet dsResd = new DataSet();
        dsResd = ResdPay.GetResdPayHistory(PartnerID);

        if (dsResd.Tables[0].Rows.Count > 0)
        {
            pnlResdHistory.Visible = true;
            grdResdHistory.DataSource = dsResd;
            grdResdHistory.DataBind();
            for (int i = 0; i < grdResdHistory.Rows.Count; i++)
            {
                if (Double.TryParse(grdResdHistory.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                    ResdSubTotal_R += Convert.ToDouble(grdResdHistory.Rows[i].Cells[7].Text.Trim());
                if (Double.TryParse(grdResdHistory.Rows[i].Cells[8].Text.ToString(), out outSubTotal))
                    COSubTotal_R += Convert.ToDouble(grdResdHistory.Rows[i].Cells[8].Text.Trim());
                if (Double.TryParse(grdResdHistory.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    TotalSubTotal_R += Convert.ToDouble(grdResdHistory.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdResdHistory.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_R += Convert.ToDouble(grdResdHistory.Rows[i].Cells[10].Text.Trim());
                grdResdHistory.DataBind();
            }
        }
        else
        {
            ResdSubTotal_R = 0;
            COSubTotal_R = 0;
            TotalSubTotal_R = 0;
            PaymentSubTotal_R = 0;
            outSubTotal = 0;
            grdResdHistory.DataSource = null;
            pnlResdHistory.Visible = false;
        }

        FinalResdTotal += ResdSubTotal_R;
        FinalCOTotal += COSubTotal_R;
        FinalTotal += TotalSubTotal_R;
        FinalPaymentTotal += PaymentSubTotal_R;

        //Get Previous Employees Summary   
        outSubTotal = 0;

        //Get Commission Summary        
        CommissionsBL CommPay = new CommissionsBL();
        DataSet dsComm = new DataSet();
        dsComm = CommPay.GetCommPayHistory(PartnerID);

        if (dsComm.Tables[0].Rows.Count > 0)
        {
            pnlCommHistory.Visible = true;
            grdCommHistory.DataSource = dsComm;
            grdCommHistory.DataBind();
            for (int i = 0; i < grdCommHistory.Rows.Count; i++)
            {
                if (Double.TryParse(grdCommHistory.Rows[i].Cells[7].Text.ToString(), out outSubTotal))
                    CommSubTotal_C += Convert.ToDouble(grdCommHistory.Rows[i].Cells[7].Text.Trim());
                if (Double.TryParse(grdCommHistory.Rows[i].Cells[9].Text.ToString(), out outSubTotal))
                    RefSubTotal_C += Convert.ToDouble(grdCommHistory.Rows[i].Cells[9].Text.Trim());
                if (Double.TryParse(grdCommHistory.Rows[i].Cells[10].Text.ToString(), out outSubTotal))
                    COSubTotal_C += Convert.ToDouble(grdCommHistory.Rows[i].Cells[10].Text.Trim());
                if (Double.TryParse(grdCommHistory.Rows[i].Cells[11].Text.ToString(), out outSubTotal))
                    TotalSubTotal_C += Convert.ToDouble(grdCommHistory.Rows[i].Cells[11].Text.Trim());
                if (Double.TryParse(grdCommHistory.Rows[i].Cells[12].Text.ToString(), out outSubTotal))
                    PaymentSubTotal_C += Convert.ToDouble(grdCommHistory.Rows[i].Cells[12].Text.Trim());
                grdCommHistory.DataBind();
            }
        }
        else
        {
            CommSubTotal_C = 0;
            RefSubTotal_C = 0;
            COSubTotal_C = 0;
            TotalSubTotal_C = 0;
            PaymentSubTotal_C = 0;
            outSubTotal = 0;
            grdCommHistory.DataSource = null;
            pnlCommHistory.Visible = false;
        }

        FinalCommTotal += CommSubTotal_C;
        FinalRefTotal += RefSubTotal_C;
        FinalCOTotal += COSubTotal_C;
        FinalTotal += TotalSubTotal_C;
        FinalPaymentTotal += PaymentSubTotal_C;

        if ((FinalCommTotal == 0.00) && (FinalRefTotal == 0.00) && (FinalResdTotal == 0.00) && (FinalCOTotal == 0.00) && (FinalTotal == 0.00))
        {
            tblTotal.Visible = false;
            DisplayMessage("No Payment details found.");
        }
        else
        {
            Label lblValue;
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            tr = new TableRow();
            td = new TableCell();
            td.Attributes.Add("ColSpan", "12");
            //tr.Cells.Add(td);
            tblTotal.Rows.Add(tr);

            lblValue = new Label();
            lblValue.Text = "GRAND TOTAL: ";
            td = new TableCell();
            td.Width = 100;
            //lblValue.ApplyStyle(ValueLabel);
            td.Attributes.Add("ColSpan", "10");
            td.Attributes.Add("align", "left");
            lblValue.Font.Size = FontUnit.Point(12);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            /*lblValue = new Label();
            lblValue.Text = "$" + FinalCommTotal.ToString();
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 80;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = " ";
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 50;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + FinalRefTotal.ToString();
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 65;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + FinalResdTotal.ToString();
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 65;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + FinalCOTotal.ToString();
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 65;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + FinalTotal.ToString();
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 50;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);*/

            lblValue = new Label();
            lblValue.Text = "$" + FinalPaymentTotal.ToString();
            td = new TableCell();
            //lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = 12;
            lblValue.Font.Bold = true;
            td.Width = 65;
            td.Attributes.Add("align", "center");
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void grdResdCommHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double CommTotal = 0;
                double BonusTotal = 0;
                double ReferralTotal = 0;
                double ResidualTotal = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out CommTotal))
                    CommTotal = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                if (Double.TryParse(e.Row.Cells[9].Text.Trim(), out BonusTotal))
                    BonusTotal = Convert.ToDouble(e.Row.Cells[9].Text.Trim());
                if (Double.TryParse(e.Row.Cells[10].Text.Trim(), out ReferralTotal))
                    ReferralTotal = Convert.ToDouble(e.Row.Cells[10].Text.Trim());
                if (Double.TryParse(e.Row.Cells[11].Text.Trim(), out ResidualTotal))
                    ResidualTotal = Convert.ToDouble(e.Row.Cells[11].Text.Trim());
                if (Double.TryParse(e.Row.Cells[12].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[12].Text.Trim());
                e.Row.Cells[13].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + ResidualTotal + CarryOver);
                /*if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[12].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[12].Text = "0";*/
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[8].Text = "$" + Convert.ToString(CommSubTotal_RC);
                e.Row.Cells[10].Text = "$" + Convert.ToString(RefSubTotal_RC);
                e.Row.Cells[11].Text = "$" + Convert.ToString(ResdSubTotal_RC);
                e.Row.Cells[12].Text = "$" + Convert.ToString(COSubTotal_RC);
                e.Row.Cells[13].Text = "$" + Convert.ToString(TotalSubTotal_RC);
                e.Row.Cells[14].Text = "$" + Convert.ToString(PaymentSubTotal_RC);
            }
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void grdResdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double ResidualTotal = 0;
                double CarryOver = 0;
                if (Double.TryParse(e.Row.Cells[7].Text.Trim(), out ResidualTotal))
                    ResidualTotal = Convert.ToDouble(e.Row.Cells[7].Text.Trim());
                if (Double.TryParse(e.Row.Cells[8].Text.Trim(), out CarryOver))
                    CarryOver = Convert.ToDouble(e.Row.Cells[8].Text.Trim());
                
                e.Row.Cells[9].Text = Convert.ToString(ResidualTotal + CarryOver);
                /*if ((CommTotal + BonusTotal + ReferralTotal + CarryOver) > 0)
                    e.Row.Cells[12].Text = Convert.ToString(CommTotal + BonusTotal + ReferralTotal + CarryOver);
                else
                    e.Row.Cells[12].Text = "0";*/
            }//end if DataRow
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = "$" + Convert.ToString(ResdSubTotal_R);
                e.Row.Cells[8].Text = "$" + Convert.ToString(COSubTotal_R);
                e.Row.Cells[9].Text = "$" + Convert.ToString(TotalSubTotal_R);
                e.Row.Cells[10].Text = "$" + Convert.ToString(PaymentSubTotal_R);
            }
        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }
    protected void grdCommHistory_RowDataBound(object sender, GridViewRowEventArgs e)
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
                e.Row.Cells[7].Text = "$" + Convert.ToString(CommSubTotal_C);
                e.Row.Cells[9].Text = "$" + Convert.ToString(RefSubTotal_C);
                e.Row.Cells[10].Text = "$" + Convert.ToString(COSubTotal_C);
                e.Row.Cells[11].Text = "$" + Convert.ToString(TotalSubTotal_C);
                e.Row.Cells[12].Text = "$" + Convert.ToString(PaymentSubTotal_C);
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
