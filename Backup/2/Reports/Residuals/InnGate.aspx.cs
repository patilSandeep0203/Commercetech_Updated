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
using DLPartner.PartnerDSTableAdapters;

public partial class Residuals_InnGate : System.Web.UI.Page
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
                MonthBL mon = new MonthBL();
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
 
                    DataSet dsMon = mon.GetMonthListForReports(1, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    ListBL RepList = new ListBL();
                    DataSet dsRep = RepList.GetRepListForVendor("inngate");
                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = dsRep;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "MasterNum";
                        lstRepList.DataBind();
                    }
                    ListItem item = new ListItem();
                    item.Text = "ALL";
                    item.Value = "ALL";
                    lstRepList.Items.Add(item);
                    lstRepList.SelectedIndex = lstRepList.Items.IndexOf(item);

                    string MasterNum = "";
                    string Month = "";
                    if (Request.Params.Get("Month") != null)
                        Month = Request.Params.Get("Month");

                    if (Request.Params.Get("MasterNum") != null)
                        MasterNum = Request.Params.Get("MasterNum");

                    if ((Month != "") && (MasterNum != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                    {
                        try
                        {
                            lblError.Visible = false;
                            lstMonth.SelectedValue = lstMonth.Items.FindByText(Month).Value;
                            lblError.Visible = false;
                            lblMonth.Text = "Innovative Gateway Residuals for the month of: " + lstMonth.SelectedItem.Text;
                            if (lstRepList.Items.FindByValue(MasterNum) != null)
                                lstRepList.SelectedValue = MasterNum;
                            Populate(MasterNum, Month, false, "");
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
            catch (Exception)
            {
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback

        else
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Tabs.Tabs.Remove(TabPanelLookup);
        }
    }//end page load

    //This function populates ipayment residuals
    public void Populate(string MasterNum, string Month, bool bDBA, string DBA)
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
        PartnerDS.InnovativeGatewayDataTable dt = null; 
        ResidualsBL Resd = new ResidualsBL(Month, MasterNum);

        if (!bDBA)
        {            
            DataSet dsTotals = Resd.GetInnGateTotals();
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTotals.Tables[0].Rows[0];
                ECETotal = Convert.ToDouble(dr["InnGateecetotal"]);
                RepTotal = Convert.ToDouble(dr["InnGatereptotal"]);
            }//end if count not 0

            dt = Resd.GetInnGateResiduals();
        }
        else
        {
            ResidualsAdminBL InnGate = new ResidualsAdminBL(Month);
            dt = InnGate.GetInnovativeGatewayResidualsByDBA(DBA);
        }
        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblValue;

            int IDHead = 0;
            int IDNext = 0;
            for (int i = 0; i < dt.Rows.Count; )
            {
                tr = new TableRow();

                IDHead = Convert.ToInt32(dt[i].GatewayUserID);
                IDNext = IDHead;

                //tblHeaderInfo = new Table();
                //RepName
                lblValue = new Label();
                if ((MasterNum == "ALL") || (bDBA))
                    lblValue.Text = dt[i].RepName.ToString().Trim();
                else
                    lblValue.Text = "";
                
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Attributes.Add("height", "50px");
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                //Merchant ID
                lblValue = new Label();
                lblValue.Text = "Gateway User ID: " + dt[i].GatewayUserID.ToString().Trim();
                td = new TableCell();
                td.Attributes.Add("colspan", "8");
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                //DBA
                lblValue = new Label();
                lblValue.Text = "Business Name: " + dt[i].DBA.ToString().Trim();
                td = new TableCell();
                td.Attributes.Add("colspan", "8");
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);
                tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);

                #region Header Row

                string[] arrColumns = { "", "Product", "Solt At", "Qty Sold", "Buy Rate", "Uncollected", 
                "Referral", "Commission", "ECE Total", "Rep Split", "Rep Total"};

                tr = new TableRow();
                tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
                for (int k = 0; k < arrColumns.Length; k++)
                {
                    td = new TableCell();
                    td.Text = arrColumns[k].ToString();
                    td.Style["font-family"] = "Arial";
                    td.Style["font-size"] = "Small";
                    td.Style["font-weight"] = "Bold";
                    td.Style["Color"] = "White";
                    tr.Cells.Add(td);
                }
                tblResiduals.Rows.Add(tr);

                #endregion

                do
                {
                    tr = new TableRow();
                    lblValue = new Label();
                    if (bDBA)
                        lblValue.Text = dt[i].Mon.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].Product.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].SoldAt.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].QtySold.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].BuyRate.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].Uncollected.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].Referral.ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].Commission.ToString().Trim() + "%";
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = Convert.ToDouble(dt[i].EceTotal).ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = dt[i].RepSplit.ToString().Trim() + "%";
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = Convert.ToDouble(dt[i].reptotal).ToString().Trim();
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    tblResiduals.Rows.Add(tr);
                    
                    i = i + 1;
                    if (i == dt.Rows.Count)
                        break;
                    IDNext = Convert.ToInt32(dt[i].GatewayUserID);

                } while (IDHead == IDNext);//end while current merchant id and next merchant id are same

                if (!bDBA)
                {
                    tr = new TableRow();
                    td = new TableCell();
                    td.Attributes.Add("colspan", "7");
                    tr.Cells.Add(td);

                    lblValue = new Label();
                    lblValue.Text = "Sub Total";
                    td = new TableCell();
                    lblValue.ApplyStyle(ValueLabel);
                    lblValue.Font.Bold = true;
                    td.Controls.Add(lblValue);
                    tr.Cells.Add(td);

                    //Get Subtotal
                    DataSet dsSubTotals = Resd.GetInnGateSubTotals(IDHead);
                    if (dsSubTotals.Tables[0].Rows.Count > 0)
                    {
                        DataRow drTotals = dsSubTotals.Tables[0].Rows[0];

                        lblValue = new Label();
                        lblValue.Text = "$" + drTotals["inngateEcesubTotal"].ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        lblValue.Font.Bold = true;
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        tr.Cells.Add(td);

                        lblValue = new Label();
                        lblValue.Text = "$" + drTotals["inngaterepsubTotal"].ToString().Trim();
                        td = new TableCell();
                        lblValue.ApplyStyle(ValueLabel);
                        lblValue.Font.Bold = true;
                        td.Controls.Add(lblValue);
                        tr.Cells.Add(td);
                    }//end if subtotals count not 0

                    tblResiduals.Rows.Add(tr);

                    tr = new TableRow();
                    td = new TableCell();
                    td.Attributes.Add("height", "30px");
                    td.Attributes.Add("colspan", "11");
                    tr.Cells.Add(td);
                    tblResiduals.Rows.Add(tr);
                }
            }//end for
            if (!bDBA)
            {
                tr = new TableRow();
                td = new TableCell();
                td.Attributes.Add("colspan", "7");
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
            }
        }//end if count not 0
        else
            DisplayMessage("No Records found.");
    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            lblMonth.Visible = true;
            lblMonth.Text = "Innovative Gateway Residuals for the month of: " + lstMonth.SelectedItem.Text;
            string Rep = Session["MasterNum"].ToString();
            //if the Rep List is visible, set the Rep to be searched
            if (lstRepList.Visible == true)
                Rep = lstRepList.SelectedValue;

            Populate(Rep, lstMonth.SelectedItem.Value.ToString().Trim(), false, "");
            
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
            lblMonth.Visible = false;
            Populate("", "", true, txtLookup.Text.Trim());
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
}
