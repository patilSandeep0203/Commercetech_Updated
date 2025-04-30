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

public partial class Residuals_IMS2 : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Page.MasterPageFile = "../Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "../Agent.master";
            else if (User.IsInRole("Employee"))
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
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {                
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    ResidualsBL mon = new ResidualsBL();
                    DataSet dsMon = mon.GetMonthListForReports(1, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    ResidualsBL Rep = new ResidualsBL();
                    DataSet dsRep = Rep.GetRepListForVendor("ims2");
                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = dsRep;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "RepNum";
                        lstRepList.DataBind();
                    }
                    ListItem item = new ListItem();
                    item.Text = "ALL";
                    item.Value = "ALL";
                    lstRepList.Items.Add(item);
                    lstRepList.SelectedIndex = lstRepList.Items.IndexOf(item);

                    string IMS2Num = "";
                    string Month = "";
                    if (Request.Params.Get("Month") != null)
                        Month = Request.Params.Get("Month");

                    if (Request.Params.Get("IMS2Num") != null)
                        IMS2Num = Request.Params.Get("IMS2Num");

                    if ((Month != "") && (IMS2Num != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                    {
                        try
                        {
                            lblError.Visible = false;
                            lstMonth.SelectedIndex = lstMonth.Items.IndexOf(lstMonth.Items.FindByText(Month));
                            lblError.Visible = false;
                            lblMonth.Text = "IMS2 Residuals for the month of: " + lstMonth.SelectedItem.Text;
                            //CallPopulateForRep(IMS2Num);
                            lstRepList.SelectedIndex = lstRepList.Items.IndexOf(lstRepList.Items.FindByValue(IMS2Num));
                            Populate(IMS2Num, lstMonth.SelectedItem.Value.ToString().Trim(), false, "");
                        }
                        catch (Exception err)
                        {
                            CreateLog Log = new CreateLog();
                            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                            DisplayMessage("Error Processing Request. Please contact technical support");
                        }
                    }//end if month not null

                }//end if
                else
                {
                    Tabs.Tabs.Remove(TabPanelLookup);
                    ResidualsBL mon = new ResidualsBL();
                    DataSet dsMon = mon.GetMonthListForReports(2, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    lstRepList.Visible = false;
                    lblSelectRepName.Visible = false;
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

    //This function populates ipayment residuals
    public void Populate(string IMS2Num, string Month, bool bDBA, string DBA)
    {
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
        sHyperLink.Font.Size = FontUnit.Point(8);
        sHyperLink.Font.Name = "Arial";

        double ECETotal = 0;
        double RepTotal = 0;
        double RepTotalT1 = 0;     
        if (!bDBA)
        {
            ResidualsBL IMS2Totals = new ResidualsBL();
            DataSet dsTotals = IMS2Totals.GetIMS2Totals(IMS2Num, Month);
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTotals.Tables[0].Rows[0];
                ECETotal = Convert.ToDouble(dr["IMS2ecetotal"]);
                RepTotal = Convert.ToDouble(dr["IMS2reptotal"]);
            }//end if count not 0

            DataSet dsTotalsT1 = IMS2Totals.GetIMS2TotalsT1(IMS2Num, Month);
            if (dsTotalsT1.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTotalsT1.Tables[0].Rows[0];
                RepTotalT1 = Convert.ToDouble(dr["IMS2TTotal"]);
            }//end if count not 0


        }

        ResidualsBL IMS2Resd;
        DataSet ds;
        if (!bDBA)
        {
            IMS2Resd = new ResidualsBL();
            ds = IMS2Resd.GetIMS2Residuals(IMS2Num, Month);
        }
        else
        {
            IMS2Resd = new ResidualsBL();
            ds = IMS2Resd.GetIMS2ResidualsByDBA(DBA);
        }

        //number of columns to actually display from the view
        int numColDisplay;
        numColDisplay = ds.Tables[0].Columns.Count - 4;
        if (ds.Tables[0].Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();

            Label lblValue;

            #region Header Row

            tr = new TableRow();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td = new TableCell();
            td.Text = "";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "9px";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);

            for (int i = 0; i < numColDisplay; i++)
            {
                DataColumn dc = ds.Tables[0].Columns[i];
                td = new TableCell();
                td.Text = dc.ColumnName;
                
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "9px";
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

                //RepName
                lblValue = new Label();
                if (IMS2Num == "ALL")
                    lblValue.Text = dr["RepName"].ToString().Trim();
                else
                    lblValue.Text = "";
                if ( bDBA )
                    lblValue.Text = dr["RepName"].ToString().Trim() + " (" + dr["Mon"].ToString().Trim() + ")";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                for (int j = 0; j < numColDisplay; j++)
                {
                    lblValue = new Label();
                    lblValue.Text = dr[j].ToString().Trim();                    
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);
                }
                if (bDBA)
                {
                    ECETotal = ECETotal + Convert.ToDouble(dr[40]);
                    RepTotal = RepTotal + Convert.ToDouble(dr[42]);
                }

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }//end for

            //if (!bDBA)
            //{
                tr = new TableRow();
                td = new TableCell();
                td.Attributes.Add("colspan", Convert.ToString(numColDisplay - 3) );
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "Total";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + ECETotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + RepTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            //}
        }//end if count not 0
        else
            DisplayMessage("No Records found");
    }//end function Populate

    public void CallPopulateForRep(string MasterNum)
    {
        ResidualsBL Residuals = new ResidualsBL();
        string IMS2Num = Residuals.ReturnVendorNumForRep(MasterNum, "IMS2");
        if (IMS2Num == "")
            DisplayMessage("Rep does not have an IMS2 Number");
	    else
	    {
            lstRepList.SelectedIndex = lstRepList.Items.IndexOf(lstRepList.Items.FindByValue(MasterNum));
            Populate(IMS2Num, lstMonth.SelectedItem.Value.ToString().Trim(), false, "");
	    }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            lblMonth.Text = "IMS2 Residuals for the month of: " + lstMonth.SelectedItem.Text;
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                Populate(lstRepList.SelectedItem.Value, lstMonth.SelectedItem.Value.ToString().Trim(), false, "");
            else
                CallPopulateForRep(Session["MasterNum"].ToString());
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

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            Populate("", "", true, txtLookup.Text.Trim());
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
}
