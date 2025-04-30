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
using DataLayer;
using BusinessLayer;

public partial class CommPrint : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }
    public static string RepNum = "";
    public static string Month = "";
    public static string Period = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin"))
            Response.Redirect("~/login.aspx?Authentication=False");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                if (Request.Params.Get("RepNum") != null)
                    RepNum = Request.Params.Get("RepNum").ToString();

                if (Request.Params.Get("Month") != null)
                {
                    Month = Request.Params.Get("Month").ToString();
                    Period = Request.Params.Get("Period").ToString();
                }

                lblMonth.Text = "Commissions for the month of: " + Month + "-" + Period;
                double fRepTotalSum = PopulateCommissions();
                PopulateBonus(fRepTotalSum);
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                Response.Write("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function populates the Commissions detail
    public double PopulateCommissions()
    {
        double fRepTotalSum = 0;
        double fRefTotalSum = 0;

        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(7);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Point(7);
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";
        
        CommissionsBL Comm = new CommissionsBL();
        //DataSet dsTotals = Comm.GetCommissionsTotals(RepNum, Month);
        /*
        if (dsTotals.Tables[0].Rows.Count > 0)
        {
            DataRow drTotals = dsTotals.Tables[0].Rows[0];
            if ( drTotals["CommTotal"].ToString() != "" )
                fRepTotalSum = Convert.ToDouble(drTotals["CommTotal"]);
            if (drTotals["RefTotal"].ToString() != "")
                fRefTotalSum = Convert.ToDouble(drTotals["RefTotal"]);
        }
         */
        DLPartner.PartnerDS.CommissionsDataTable dt = new DLPartner.PartnerDS.CommissionsDataTable();
        int iPeriod = 0; //Full Month
        if (Period == "First Half")
            iPeriod = 1;
        else if (Period == "Second Half")
            iPeriod = 2;

        if (Period == "Full Month")
            dt = Comm.GetCommissionsByRepMon(RepNum, Month);
        else
            dt = Comm.GetCommissionsByRepMonPeriod(RepNum, Month, iPeriod);

        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            Label lblStatusSTR;
            int iCount = 0;
            string[] arrColumns = { "DBA", "Merchant Number", "Rep Name", "Referred By", "Product", "Qty", "Price (Per Unit)", "Total", "COG (Per Unit)", "Comm", "Rep Total", "Referral Total" };
            if (User.IsInRole("Admin"))
                iCount = arrColumns.Length;
            else
                iCount = arrColumns.Length - 1;

            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < iCount; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i].ToString();
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "10px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }

            tblPrintComm.Rows.Add(tr);
            string strRepTotalCurr = "";
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();
                lblStatusSTR = new Label();

                if (dt[i].FundedValue.ToString().Trim() == "1.00")
                {
                    Style StatusLabel = new Style();
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                    StatusLabel.Font.Size = FontUnit.Small;
                    StatusLabel.Font.Bold = true;
                    StatusLabel.Font.Name = "Arial";
                    lblStatusSTR.Text = "+";
                    lblStatusSTR.ApplyStyle(StatusLabel);
                }
                else if (dt[i].FundedValue.ToString().Trim() == "0.50")
                {
                    Style StatusLabel = new Style();
                    StatusLabel.ForeColor = System.Drawing.Color.Green;
                    StatusLabel.Font.Size = FontUnit.Small;
                    StatusLabel.Font.Bold = true;
                    StatusLabel.Font.Name = "Arial";
                    lblStatusSTR.Text = "#";
                    lblStatusSTR.ApplyStyle(StatusLabel);
                }
                else lblStatusSTR.Text = "";

                //DBA
                lblValue = new Label();
                lblValue.Text = dt[i].DBA.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblStatusSTR);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                tr.Cells.Add(td);

                //Merchant Number
                lblValue = new Label();
                lblValue.Text = dt[i].MerchantID.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepName
                lblValue = new Label();
                lblValue.Text = dt[i].RepName.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
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

                //Price                
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

                //COG
                lblValue = new Label();
                lblValue.Text = Convert.ToDouble( dt[i].COG).ToString().Trim();
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
                    strRepTotalCurr = Convert.ToDouble(dt[i].RepTotal).ToString();
                lblValue.Text = strRepTotalCurr;                
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);
                fRepTotalSum = fRepTotalSum + Convert.ToDouble(dt[i].RepTotal);

                //RefTotal
                lblValue = new Label();
                lblValue.Text = "$" + Convert.ToDouble(dt[i].RefTotal).ToString().Trim();   
                fRefTotalSum = fRefTotalSum + Convert.ToDouble(dt[i].RefTotal);             
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblPrintComm.Rows.Add(tr);
            }//end for rows
            tr = new TableRow();

            td = new TableCell();
            td.Attributes.Add("ColSpan", "6");
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "Sub Total: ";
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            td.Attributes.Add("ColSpan", "4");
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            lblValue = new Label();
            lblValue.Text = "$" + fRepTotalSum.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
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
            tblPrintComm.Rows.Add(tr);

        }//end if count not 0
        return fRepTotalSum + fRefTotalSum;
    }//end populate commissions

    //This function populates Bonus detail
    public void PopulateBonus(double fRepTotalSum)
    {
        CommissionsBL Bonus = new CommissionsBL();
        int iPeriod = 0; //Full Month
        if (Period == "First Half")
            iPeriod = 1;
        else if (Period == "Second Half")
            iPeriod = 2;
        DataSet ds = Bonus.GetBonusInfo(RepNum, Month, iPeriod);
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
        double fBonusTotal = 0;
        string[] arrColumns = { "Bonuses/Discover Commissions For the Month", "Reason", "Month", "RepNum", "RepTotal"};
        tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
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

                //RepNum
                lblValue = new Label();
                lblValue.Text = dr["RepNum"].ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepTotal
                lblValue = new Label();
                lblValue.Text = "$" + Convert.ToDouble(dr["RepTotal"]).ToString().Trim();
                fBonusTotal = fBonusTotal + Convert.ToDouble(dr["RepTotal"]);
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
            lblValue.Text = "$" + fBonusTotal.ToString();
            td = new TableCell();
            lblValue.ApplyStyle(ValueLabel);
            lblValue.Font.Size = FontUnit.Point(10);
            lblValue.Font.Bold = true;
            td.Controls.Add(lblValue);
            tr.Cells.Add(td);

            tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
            tblBonus.Rows.Add(tr);
        }//end if count not 0

        double fRepFinal = fRepTotalSum + fBonusTotal;        

        //Display Monthly Total
        tr = new TableRow();
        td = new TableCell();
        td.Attributes.Add("ColSpan", "3");
        tr.Cells.Add(td);

        //Monthly Total With Bonus Header
        lblValue = new Label();
        lblValue.Text = "Monthly Total With Bonus: ";
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        //Bonus Total
        lblValue = new Label();
        lblValue.Text = "$" + fRepFinal.ToString();
        td = new TableCell();
        lblValue.ApplyStyle(ValueLabel);
        lblValue.Font.Size = FontUnit.Point(10);
        lblValue.Font.Bold = true;
        td.Controls.Add(lblValue);
        tr.Cells.Add(td);

        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
        tblBonus.Rows.Add(tr);
    }//end function PopulateBonus
}
