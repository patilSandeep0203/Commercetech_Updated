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

public partial class CommUpdate : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    public static int CommissionID = 0;
    public static string Month = string.Empty;
    public static string Period = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin"))
            Response.Redirect("~/logout.aspx");

        if (Request.Params.Get("CommID") != null)
            CommissionID = Convert.ToInt32(Request.Params.Get("CommID"));

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                Style TextArea = new Style();
                TextArea.Width = new Unit(140);
                TextArea.Height = new Unit(50);
                TextArea.Font.Size = FontUnit.Point(8);
                TextArea.Font.Name = "Arial";
                txtReason.ApplyStyle(TextArea);

                int AffiliateID = Convert.ToInt16(Session["AffiliateID"]);
                AffiliatesBL AffList = new AffiliatesBL(AffiliateID);
                DataSet ds = AffList.GetAffiliateList();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lstLegalName.DataSource = ds;
                    lstLegalName.DataTextField = "DBA";
                    lstLegalName.DataValueField = "AffiliateID";
                    lstLegalName.DataBind();
                }
                ListItem lstOther = new ListItem();
                lstOther.Text = "";
                lstOther.Value = "";
                lstLegalName.Items.Add(lstOther);

                if (Request.Params.Get("Task") != null)
                {
                    if (Request.Params.Get("Task") == "Update")
                    {
                        pnlUpdateComm.Visible = true;
                        pnlAddBonus.Visible = false;
                        Populate();
                    }
                    else if (Request.Params.Get("Task") == "AddBonus")
                    {
                        pnlUpdateComm.Visible = false;
                        pnlAddBonus.Visible = true;
                        lblMonthBonus.Visible = true;                        
                        if (Request.Params.Get("Month") != null)
                            Month = Request.Params.Get("Month");
                        string MasterNum = string.Empty;
                        if (Request.Params.Get("MasterNum") != null)
                        {
                            MasterNum = Request.Params.Get("MasterNum");
                            Period = Request.Params.Get("Period").ToString();
                        }

                        CommissionsBL Rep = new CommissionsBL();
                        DataSet dsRep = Rep.GetRepList();
                        if (dsRep.Tables[0].Rows.Count > 0)
                        {
                            lstRepListBonus.DataSource = dsRep;
                            lstRepListBonus.DataTextField = "RepName";
                            lstRepListBonus.DataValueField = "RepNum";
                            lstRepListBonus.DataBind();
                        }

                        lblMonthBonus.Text = "Bonus for the Month of " + Month + "-" + Period;
                        if  (MasterNum != "ALL")
                            lstRepListBonus.SelectedValue = lstRepListBonus.Items.FindByValue(MasterNum).Value;
                                                
                    }
                }//end if request not null                
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage(err.Message);
            }
        }//end if not postback
    }//end page load

    //This function Populates Update Commissions Info
    public void Populate()
    {
        CommissionsBL CommInfo = new CommissionsBL();
        PartnerDS.CommissionsDataTable dt = CommInfo.GetCommDetailFromID(Convert.ToInt32(CommissionID));
        if (dt.Rows.Count > 0)
        {
            lblDBA.Text = dt[0].DBA.ToString().Trim();
            lblMerchantID.Text = dt[0].MerchantID.ToString().Trim();
            lblRepName.Text = dt[0].RepName.ToString().Trim();
            lblRepNum.Text = dt[0].RepNum.ToString().Trim();
            lstLegalName.SelectedValue = lstLegalName.Items.FindByValue(dt[0].ReferralID.ToString()).Value;
            lblProduct.Text = dt[0].Product.ToString().Trim();
            txtQty.Text = dt[0].Units.ToString().Trim();
            txtPrice.Text = dt[0].Price.ToString().Trim();
            txtCOG.Text = dt[0].COG.ToString().Trim();
            lstComm.SelectedValue = lstComm.Items.FindByValue(Convert.ToInt32( dt[0].Commission).ToString()).Value;
            lstFundedValue.SelectedValue = lstFundedValue.Items.FindByValue(dt[0].FundedValue.ToString()).Value;
            lblRepTotal.Text = dt[0].RepTotal.ToString().Trim();
            txtRefPaid.Text = dt[0].RefTotal.ToString().Trim();
        }//end if count not 0
    }//end function Populate

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            CommissionsBL Update = new CommissionsBL();
            bool retVal = Update.UpdateCommissionInfo(lblProduct.Text, lstLegalName.SelectedItem.Value, txtPrice.Text,
                txtCOG.Text, txtQty.Text, lstComm.SelectedItem.Value, lstFundedValue.SelectedItem.Value, txtRefPaid.Text, Convert.ToInt32(CommissionID));
            if ( retVal )
                DisplayMessage("Information Updated Successfully");
            else
                DisplayMessage("Error Updating Commissions");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating Commissions");
        }
    }
        
    protected void btnAddBonus_Click(object sender, EventArgs e)
    {
        try
        {
            int iPeriod = 0; //Full Month
            if (Period == "First Half")
                iPeriod = 1;
            else if (Period == "Second Half")
                iPeriod = 2;

            CommissionsBL Bonus = new CommissionsBL();
            Bonus.InsertBonus(lstRepListBonus.SelectedItem.Value, lstBonus.SelectedItem.Text, txtReason.Text.Trim(), txtRepTotal.Text.Trim(), Month, iPeriod);
            DisplayMessage("Added Bonus for Rep " + lstRepListBonus.SelectedItem.Text + " for the Month of " + Month + "-" + Period);
        }
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("CommAdmin.aspx");
    }
}
