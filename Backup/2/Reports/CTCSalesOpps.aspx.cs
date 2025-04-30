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

public partial class CTCSalesOpps : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Affiliate"))
                Page.MasterPageFile = "Affiliates.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Page.MasterPageFile = "Agent.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "Admin.master";
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
                
        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                MonthBL mon = new MonthBL();
                DataSet dsMon = mon.GetMonthList();
                if (dsMon.Tables[0].Rows.Count > 0)
                {
                    lstMonth.DataSource = dsMon;
                    lstMonth.DataTextField = "Mon";
                    lstMonth.DataValueField = "MonthID";
                    lstMonth.DataBind();
                }
                ListItem lstItemMonth = new ListItem();
                lstItemMonth.Text = "ALL";
                lstItemMonth.Value = "ALL";
                lstMonth.Items.Add(lstItemMonth);
                lstMonth.SelectedValue = lstMonth.Items.FindByValue("ALL").Value;

                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    lstStatus.AutoPostBack = false;

                    //Get Sales Rep list for Add Product Panel
                    ListBL SalesRepList = new ListBL();
                    DataSet dsRep = SalesRepList.GetSalesRepList();
                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = dsRep;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "MasterNum";
                        lstRepList.DataBind();
                    }
                    ListItem lstItem = new ListItem();
                    lstItem.Text = "ALL";
                    lstItem.Value = "ALL";
                    lstRepList.Items.Add(lstItem);
                    lstRepList.SelectedValue = lstRepList.Items.FindByValue("ALL").Value;
                }//end if session admin
                else
                {
                    lstStatus.AutoPostBack = true;

                    lblSelectMonth.Visible = true;
                    lstMonth.Visible = true;
                    lblSelectRepName.Visible = false;                    
                    lstRepList.Visible = false;
                }
            }//end try
            catch (Exception err)
            {
                DisplayMessage(err.Message);
            }
        }//end if not postback
    }

    public void Populate(string RepNum, string Month, string Year, string Status)
    {
        grdSalesOpps.Visible = true;
        //Get Sales Opp information from ACT!
        ACTDataBL ACT = new ACTDataBL();
        DataSet ds = ACT.GetACTSalesOpps(RepNum, Month, Year, Status);
        lblMonth.Text = "Pending Sales Opps for the month of " + lstMonth.SelectedItem.Text;
        lblTotal.Text = "Number of records: " + ds.Tables[0].Rows.Count;
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdSalesOpps.DataSource = ds;
            grdSalesOpps.DataBind();
        }//end if count not 0
        else
        {
            grdSalesOpps.Visible = false;
            DisplayMessage("No Records Found");
        }
    }//end function Populate

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            string Month = "ALL";
            string Year = "ALL";
            if (lstMonth.SelectedItem.Value != "ALL")
            {
                MonthBL Months = new MonthBL();
                DataSet dsMon = Months.GetMonthYear(Convert.ToInt16(lstMonth.SelectedItem.Value));
                if (dsMon.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsMon.Tables[0].Rows[0];
                    Month = dr["Month"].ToString().Trim();
                    Year = dr["Year"].ToString().Trim();
                }//end if count not 0
            }            
            string MasterNum = "";
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                MasterNum = Session["MasterNum"].ToString().Trim();
            else
                MasterNum = lstRepList.SelectedItem.Value.Trim();
            
            Populate(MasterNum, Month, Year, lstStatus.SelectedItem.Value);
            
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void lstStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
        {
            if (lstStatus.SelectedItem.Value == "Open")
            {
                lblSelectMonth.Visible = true;
                lstMonth.Visible = true;
            }
            else
            {
                lblSelectMonth.Visible = false;
                lstMonth.Visible = false;
                lstMonth.SelectedIndex = 0;
            }
        }
    }
}
