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

public partial class ReferralsPrint : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    public static string Month = "";
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
                if (Request.Params.Get("Month") != null)
                    Month = Request.Params.Get("Month").ToString();

                lblMonth.Text = "Referrals for the month of: " + Month;
                float fRepTotalSum = PopulateReferrals();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                Response.Write("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function populates the Referrals detail
    public float PopulateReferrals()
    {
        float fRepTotalSum = 0;

        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(7);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Point(7);
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        ReferralsBL ReferralsDetail = new ReferralsBL();
        DataSet ds = ReferralsDetail.GetReferralsDetail("ALL", Month);

        if (ds.Tables[0].Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;
            Label lblStatusSTR;
            int iCount = 0;
            string[] arrColumns = { "Referred By", "Referral ID", "DBA", "Merchant Num", "Rep Name", "Product", "Qty", "Total", "Referral Paid"};
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

            tblPrintReferrals.Rows.Add(tr);
            float fRefTotalSum = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                if ((float)Convert.ChangeType(dr["RefTotal"].ToString().Trim(), typeof(float)) > 0)
                {
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
                    lblValue.Text = dr["Units"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Total
                    lblValue = new Label();
                    lblValue.Text = dr["Total"].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //RefTotal
                    lblValue = new Label();
                    lblValue.Text = Convert.ToString(Convert.ChangeType(dr["RefTotal"].ToString().Trim(), typeof(float)));
                    fRefTotalSum = fRefTotalSum + (float)Convert.ChangeType(dr["RefTotal"].ToString().Trim(), typeof(float));
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblPrintReferrals.Rows.Add(tr);
                }//end if reftotal greater than 0
            }//end for rows

            #region TIER 1 REFERRALS
            //Get the tier 1 referrals            

            ReferralsDetail = new ReferralsBL();
            DataSet dsT1 = ReferralsDetail.GetT1Referrals("ALL", Month);
            if (dsT1.Tables[0].Rows.Count > 0)
            {
                tr = new TableRow();
                td = new TableCell();
                td.Attributes.Add("ColSpan", "9");
                td.Text = "<span class=\"Labels\"><b>Tier 1 Referrals</b></span>";
                td.Attributes.Add("align", "center");
                td.CssClass = "DivGreen";
                tr.Cells.Add(td);
                tblPrintReferrals.Rows.Add(tr);
                for (int i = 0; i < dsT1.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsT1.Tables[0].Rows[i];
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
                    fRefTotalSum = fRefTotalSum + (float)Convert.ChangeType(dr["RefTotal"].ToString().Trim(), typeof(float));
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                                       
                    if (i % 2 == 0)
                        tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                    tblPrintReferrals.Rows.Add(tr);
                }//end for rows
            }//end if T1 count not 0

            #endregion

            tr = new TableRow();
            td = new TableCell();
            td.Attributes.Add("ColSpan", "6");
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
            tblPrintReferrals.Rows.Add(tr);

        }//end if count not 0
        return fRepTotalSum;
    }//end populate Referrals
}
