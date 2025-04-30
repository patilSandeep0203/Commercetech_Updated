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

public partial class Residuals_ActivatedResd : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Employee"))
                Page.MasterPageFile = "../Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "../Admin.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!Session.IsNewSession)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Response.Redirect("~/login.aspx?Authentication=False");
        }

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            MonthBL mon = new MonthBL();
            DataSet dsMon = mon.GetMonthListForReports(1, "residuals");
            if (dsMon.Tables[0].Rows.Count > 0)
            {
                lstMonth.DataSource = dsMon;
                lstMonth.DataTextField = "Mon";
                lstMonth.DataValueField = "Mon";
                lstMonth.DataBind();
            }
        }//end if not post back
    }//end page load

    //This function populates Activated residuals
    public void Populate(string Report, string Month)
    {
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(12);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        ResidualsAdminBL ActivatedResd = new ResidualsAdminBL(Month);
        DataSet ds = ActivatedResd.GetActivatedResiduals(Report);
        if (ds.Tables[0].Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            Label lblValue;

            #region Header Row

            tr = new TableRow();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                DataColumn dc = ds.Tables[0].Columns[i];
                td = new TableCell();
                td.Text = dc.ColumnName;
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "12px";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }

            tblResiduals.Rows.Add(tr);

            #endregion

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                tr = new TableRow();

                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    lblValue = new Label();
                    lblValue.Text = dr[j].ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }//end for
        }//end if count not 0
        else
            DisplayMessage("No Records found for this month");
    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            Populate(lstReport.SelectedItem.Value, lstMonth.SelectedItem.Value);
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
}
