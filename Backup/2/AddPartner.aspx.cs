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

public partial class AddPartner: System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            else if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "~/T1Agent.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "~/site.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "~/Employee.master";
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
        
        if ((Session.IsNewSession) || (Session == null))
            Response.Redirect("~/login.aspx");

        //This page is accessible only be Admins
        if (!User.IsInRole("Admin"))// && !User.IsInRole("T1Agent"))
            Response.Redirect("~/login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                //Populate Affiliate List
                AffiliatesBL Aff = new AffiliatesBL(Convert.ToInt16(Session["AffiliateID"]));
                string MasterNum = Session["MasterNum"].ToString();
                if (User.IsInRole("Admin"))
                    MasterNum = "ALL";

                //Populate Sales Rep
                DataSet dsAff = Aff.GetNonRepList(MasterNum);
                if (dsAff.Tables[0].Rows.Count > 0)
                {
                    lstRepName.DataSource = dsAff;
                    lstRepName.DataTextField = "Contact";
                    lstRepName.DataValueField = "AffiliateID";
                    lstRepName.DataBind();
                }
                ListItem lstItem = new ListItem();
                ListItem lstItemT1 = new ListItem();
                lstItem.Text = "";
                lstItem.Value = "";
                lstRepName.Items.Add(lstItem);
                lstRepName.SelectedValue = "";

                //Populate Tier 1 Sales Rep
                ListBL RepList = new ListBL();

                //Show and populate the Tier 1 List only for Admins
                lstT1RepName.Visible = false;
                if (User.IsInRole("Admin"))
                {
                    lstT1RepName.Visible = false;
                    DataSet dsTier1 = RepList.GetPartnerList();
                    if (dsTier1.Tables[0].Rows.Count > 0)
                    {
                        lstT1RepName.DataSource = dsTier1;
                        lstT1RepName.DataTextField = "RepName";
                        lstT1RepName.DataValueField = "MasterNum";
                        lstT1RepName.DataBind();
                    }
                }
                lstItemT1.Value = "0";
                lstItemT1.Text = "None";
                lstT1RepName.Items.Add(lstItemT1);
                lstT1RepName.SelectedValue = "0";                

                DataSet dsRepCat = RepList.GetRepCatList();
                if (dsRepCat.Tables[0].Rows.Count > 0)
                {
                    lstCategory.DataSource = dsRepCat;
                    lstCategory.DataTextField = "RepCat";
                    lstCategory.DataValueField = "RepCat";
                    lstCategory.DataBind();
                }

                if (User.IsInRole("T1Agent"))
                {
                    pnlRepNumbers.Visible = false;
                    pnlPayoutReqs.Visible = false;
                }
                    
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Populating Lists");
            }
        }//end if not post back
    }//end page load

    //This function populates RepInfoBL information
    public void Populate()
    {
        //Populate RepInfoBL Info
        RepInfoBL Rep = new RepInfoBL();

        int AffiliateID = 0;
        if (lstRepName.SelectedItem.Value != "")
            AffiliateID = Convert.ToInt32(lstRepName.SelectedItem.Value);
        AffiliatesBL Aff = new AffiliatesBL(AffiliateID);

        if (AffiliateID != 0)
        {
            string MasterNum = Aff.ReturnMasterNum();
            string T1MasterNum = Session["MasterNum"].ToString();
            DataSet ds = Rep.GetPartnerInfoCurrMon(MasterNum);

            //Affiliate has been added as a Rep 
            if (ds.Tables[0].Rows.Count > 0)
            //this If Statement should not execute because the Dropdown limits the seleciton
            //to Non-Reps
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCompanyName.Text = dr["CompanyName"].ToString().Trim();
                txtDBA.Text = dr["DBA"].ToString().Trim();
                txtMasterNum.Text = dr["MasterNum"].ToString().Trim();
                if (User.IsInRole("T1Agent"))
                {
                    txtCompanyName.Attributes.Add("readonly", "readonly");
                    txtDBA.Attributes.Add("readonly", "readonly");
                    txtMasterNum.Attributes.Add("readonly", "readonly");
                }
                txtSage.Text = dr["SageNum"].ToString().Trim(); 
                txtiPay3.Text = dr["IPay3Num"].ToString().Trim();
                txtIMSQB.Text = dr["IMS2Num"].ToString().Trim();
                txtChase.Text = dr["ChaseNum"].ToString().Trim();                                        
 
                lstCategory.SelectedValue = dr["RepCat"].ToString();
                lstResidual.Text = dr["RepSplit"].ToString().Trim();
                lstCommission.Text = dr["Comm"].ToString().Trim();

                PopulateDropdownSplits(lstT1RepName.SelectedValue.ToString());
            }//end if count not 0
            else
            {   //Affiliate has not been added as a Rep            

                ds = Aff.GetAffiliateAddPartner();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtCompanyName.Text = dr["CompanyName"].ToString().Trim();
                    txtDBA.Text = dr["DBA"].ToString().Trim();
                    txtMasterNum.Text = dr["MasterNum"].ToString().Trim();
                    if (User.IsInRole("T1Agent"))
                        txtMasterNum.Attributes.Add("readonly", "readonly");

                    PopulateDropdownSplits(lstT1RepName.SelectedValue.ToString());
                }//end if count not 0
            }//end else
        }//end if affiliate id not 0
    }//end function Populate

    //This function handles submit button click event
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                RepInfoBL Rep = new RepInfoBL();
                //Adds MasterNum as the T1RepNum, so that the rep is automatically Tier1. If Rep is not Tier1 go to Manage Partners and edit.
                Rep.AddPartnerInfo(lstRepName.SelectedItem.Text, txtCompanyName.Text.Trim(),
                   txtDBA.Text.Trim(), lstSageDeclined.SelectedValue.ToString().Trim(), txtSage.Text.Trim(), txtIPS.Text.Trim(), txtUnoUsername.Text.Trim(), txtUnoPassword.Text.Trim(),
                   txtiPay3.Text.Trim(), txtIMSQB.Text.Trim(), txtChase.Text.Trim(),
                   lstResidual.Text.Trim(), lstCategory.SelectedItem.Value, lstCommission.Text.Trim(),
                   lstRepName.SelectedItem.Value, txtFundMin.Text.Trim(), txtRefMin.Text.Trim(), txtResidualMin.Text.Trim(),
                   txtMasterNum.Text.Trim(), txtMasterNum.Text.Trim());
                DisplayMessage("Rep Added Successfully");
                PartnerLogBL LogData = new PartnerLogBL();
                LogData.InsertPartnerLog(Convert.ToInt32(Session["AffiliateID"]), "RepInfo Added for Partner - " + lstRepName.SelectedItem.Text.ToString().Trim() + ".");
            }//end if page is valid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Adding Agent");
        }
    }//end submit button click

    //This function handles repname changed event and populates rep info
    protected void lstRepName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtCompanyName.Text = "";
            txtDBA.Text = "";
            txtIMSQB.Text = "";
            txtiPay3.Text = "";
            txtChase.Text = "";
            txtSage.Text = "";
            txtMasterNum.Text = "";          

            Populate();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error retrieving Rep Info Information");
        }
    }//end repname changed event

    //This function handles repname changed event and populates rep info
    protected void lstT1RepName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstT1RepName.SelectedValue == "0")
                pnlPayoutReqs.Visible = true;  //enable Panel for Tier 1 information    
            //only for Sub-Reps
            /*else
            {
                pnlPayoutReqs.Visible = false;
                txtResidualMin.Text = "";
                txtRefMin.Text = "";
                txtFundMin.Text = "";
            }*/

            PopulateDropdownSplits(lstT1RepName.SelectedValue.ToString());         
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error retrieving Rep Info Information");
        }
    }//end repname changed event

    protected void lstSageDeclined_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstSageDeclined.SelectedValue == "Yes")
            {
                txtSage.Text = ""; 
                txtSage.Enabled = false;
                txtUnoUsername.Text = "";
                txtUnoUsername.Enabled = false;
                txtUnoPassword.Text = "";
                txtUnoPassword.Enabled = false;
            }
            else
            {                
                txtSage.Enabled = true;                
                txtUnoUsername.Enabled = true;                
                txtUnoPassword.Enabled = true;
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "lstSageDeclined_SelectedIndexChanged - " + err.Message);
            DisplayMessage("Error Adding Sage Rep Status.");
        }
    }//end sage declined list changed event


    /*
    //This function Validates Rep splits before applying for agents
    public string ValidateSplits(string MasterNum)
    {
        string strRetVal = string.Empty;

        RepInfoBL Rep = new RepInfoBL();

        double RepSplit = Rep.ReturnRepSplit(MasterNum);
        double CommSplit = Rep.ReturnCommPct(MasterNum);

        if ((lstResidual.Text != "") && (Convert.ToDouble(lstResidual.Text) > RepSplit))
            strRetVal = "Residual percent cannot exceed your Rep Split of " + RepSplit + "%";
        if ((lstCommission.Text != "") && (Convert.ToDouble(lstCommission.Text) > CommSplit))
            strRetVal = "Commission percent cannot exceed your Commission Split of" + CommSplit + "%" ;
        return strRetVal;
    }
    */

    public void PopulateDropdownSplits(string MasterNum)
    {
        //Populate Partner Info
        RepInfoBL PartnerInfo = new RepInfoBL();
        int MaxCommPct = 100;
        int MaxRepSplit = 100;

        lstResidual.Items.Clear();
        lstCommission.Items.Clear();
    
        //if the Rep to be added is a Tier 2 Rep
        if (pnlPayoutReqs.Visible == false)
        {
            //Get the Max Split to be set on Sub Rep
            DataSet ds = PartnerInfo.GetPartnerSplits(MasterNum);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MaxCommPct = Convert.ToInt16(dr["Comm"]);
                MaxRepSplit = Convert.ToInt16(dr["RepSplit"]);
            }
        }

        //Populate Commission and Residual Pct dropdown Pct
        //Populate from 0 to Max Comm Pct in increments of 5
        for (int i = 0; i <= MaxCommPct; i = i + 5)
        {
            ListItem lstItem = new ListItem();
            lstItem.Text = i.ToString();
            lstItem.Value = i.ToString();
            lstCommission.Items.Add(lstItem);
        }

        //Populate from 0 to Max Rep Split in increments of 5
        for (int j = 0; j <= MaxRepSplit; j = j + 5)
        {
            ListItem lstItem = new ListItem();
            lstItem.Text = j.ToString();
            lstItem.Value = j.ToString();
            lstResidual.Items.Add(lstItem);
        }

    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
