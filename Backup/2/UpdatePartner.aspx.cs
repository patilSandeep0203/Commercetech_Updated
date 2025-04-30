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

public partial class UpdatePartner : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    private static string MasterNum = "";
    private static int AffID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if ((Session.IsNewSession) || (Session == null))
            Response.Redirect("~/login.aspx");

        if (User.IsInRole("T1Agent"))
        {
            //Check if the partner logged in has access to the Tier 2 Rep. If not redirect to login.aspx
            bool bAccess = false;

            RepBL Tier1Rep = new RepBL(Session["MasterNum"].ToString());
            bAccess = Tier1Rep.CheckTierAccess(Request.Params.Get("MasterNum").ToString());

            if (!bAccess)
                Response.Redirect("~/login.aspx?Authentication=False");
        }
        else if (!User.IsInRole("Admin") )
        {
            Response.Redirect("~/login.aspx");
        }

        if ((Request.Params.Get("MasterNum") != null))
            MasterNum = Convert.ToString(Request.Params.Get("MasterNum"));

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            if (MasterNum == "")
                Response.Redirect("~/login.aspx");

            try
            {
                if (User.IsInRole("T1Agent"))
                {
                    pnlRepNumbers.Visible = false;
                    pnlPayoutReqs.Visible = false;
                }

                Populate();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error retrieving Partner Infomation");
            }
        }//end if not post back
    }

    //This function populates partner information
    public void Populate()
    {
        //Populate Tier 1 Sales Rep
        ListBL RepList = new ListBL();
        DataSet dsTier1 = RepList.GetPartnerList();
        if (dsTier1.Tables[0].Rows.Count > 0)
        {
            lstT1RepName.DataSource = dsTier1;
            lstT1RepName.DataTextField = "RepName";
            lstT1RepName.DataValueField = "MasterNum";
            lstT1RepName.DataBind();
        }
        ListItem lstItem = new ListItem();
        lstItem.Value = "0";
        lstItem.Text = "None";
        lstT1RepName.Items.Add(lstItem);
        
        DataSet dsRepCat = RepList.GetRepCatList();
        if (dsRepCat.Tables[0].Rows.Count > 0)
        {
            lstCategory.DataSource = dsRepCat;
            lstCategory.DataTextField = "RepCat";
            lstCategory.DataValueField = "RepCat";
            lstCategory.DataBind();
        }

        //Populate CNP Package list
        PackageBL Package = new PackageBL();
        DataSet dsPack = Package.GetPackageList("CNP");
        if (dsPack.Tables[0].Rows.Count > 0)
        {
            lstCNPPackageList.DataSource = dsPack;
            lstCNPPackageList.DataTextField = "PackageName";
            lstCNPPackageList.DataValueField = "PackageID";
            lstCNPPackageList.DataBind();
        }

        //Populate CP Package list
        dsPack = Package.GetPackageList("CP");
        if (dsPack.Tables[0].Rows.Count > 0)
        {
            lstCPPackageList.DataSource = dsPack;
            lstCPPackageList.DataTextField = "PackageName";
            lstCPPackageList.DataValueField = "PackageID";
            lstCPPackageList.DataBind();
        }

        PopulateDropdownSplits(MasterNum);

        RepInfoBL PartnerInfo = new RepInfoBL();
        DataSet ds = PartnerInfo.GetPartnerInfoCurrMon(MasterNum);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtRepName.Text = dr["RepName"].ToString().Trim();
            if (!Convert.IsDBNull(dr["CompanyName"]))
            {
                if (Convert.ToString(dr["CompanyName"]) != "")
                {
                    txtCompanyName.Text = dr["CompanyName"].ToString().Trim();
                    txtCompanyName.Enabled = false;
                }
            }
            txtDBA.Text = dr["DBA"].ToString().Trim();
            lstT1RepName.SelectedValue = lstT1RepName.Items.FindByValue(dr["T1MasterNum"].ToString()).Value;

            pnlPayoutReqs.Visible = true;  //enable Panel for Tier 1 information    
            txtFundMin.Text = dr["FundMin"].ToString();
            txtRefMin.Text = dr["RefMin"].ToString();
            txtResidualMin.Text = dr["ResidualMin"].ToString();
            //if Rep is the same as the Tier 1 Rep
            /*if (lstT1RepName.SelectedValue == MasterNum)
            {
                pnlPayoutReqs.Visible = true;  //enable Panel for Tier 1 information    
                txtFundMin.Text = dr["FundMin"].ToString();
                txtRefMin.Text = dr["RefMin"].ToString();
                txtResidualMin.Text = dr["ResidualMin"].ToString();
            }
            //only for Sub-Reps
            else
            {
                pnlPayoutReqs.Visible = false;
                txtResidualMin.Text = "";
                txtRefMin.Text = "";
                txtFundMin.Text = "";
            }*/
            txtSage.Text = dr["SageNum"].ToString().Trim();
            txtiPay1.Text = dr["IPayNum"].ToString().Trim();
            txtiPay2.Text = dr["IPay2Num"].ToString().Trim();
            txtiPay3.Text = dr["IPay3Num"].ToString().Trim();
            txtiPaySalesID.Text = dr["iPaySalesID"].ToString().Trim();
            txtIMS.Text = dr["ImsNum"].ToString().Trim();
            txtIMSQB.Text = dr["IMS2Num"].ToString().Trim();
            txtChase.Text = dr["ChaseNum"].ToString().Trim();
            txtIPS.Text = dr["IPSNum"].ToString().Trim();
                        
            DataSet dsSage = PartnerInfo.GetRepInfoUnoLogin(MasterNum);

            if (dsSage.Tables[0].Rows.Count > 0)
            {
                DataRow drSage = dsSage.Tables[0].Rows[0];
                if (Convert.ToBoolean(drSage["Declined"]))
                {
                    lstSageDeclined.SelectedValue = "Yes";
                    txtSage.Enabled = false;
                    txtUnoUsername.Enabled = false;
                    txtUnoPassword.Enabled = false;
                }
                else
                lstSageDeclined.SelectedValue = "No";
                txtUnoUsername.Text = drSage["UnoUsername"].ToString().Trim();
                txtUnoPassword.Attributes.Add("value", drSage["UnoPassword"].ToString().Trim());
                //txtUnoPassword.Text = drSage["UnoPassword"].ToString().Trim();
            }
               
            txtFundMin.Text = dr["FundMin"].ToString();
            txtRefMin.Text = dr["RefMin"].ToString();
            txtResidualMin.Text = dr["ResidualMin"].ToString();

            txtMasterNum.Text = MasterNum;
            lstCategory.SelectedValue = dr["RepCat"].ToString();
            lstResidual.SelectedValue = dr["RepSplit"].ToString();
            lstCommission.SelectedValue = dr["Comm"].ToString();

            AffID = Convert.ToInt32(dr["AffiliateID"]);

            if (dr["PackageID"].ToString() != "")
                lstCNPPackageList.SelectedValue = dr["PackageID"].ToString();

            if (dr["CPPackageID"].ToString() != "")
                lstCPPackageList.SelectedValue = dr["CPPackageID"].ToString();

            if ((User.IsInRole("T1Agent")) || (User.IsInRole("Employee")))
            {
                txtRepName.Attributes.Add("readonly", "readonly");
                txtCompanyName.Attributes.Add("readonly", "readonly");
                txtDBA.Attributes.Add("readonly", "readonly");
                txtMasterNum.Attributes.Add("readonly", "readonly");
                txtFundMin.Attributes.Add("readonly", "readonly");
                txtRefMin.Attributes.Add("readonly", "readonly");
                txtResidualMin.Attributes.Add("readonly", "readonly");
                lstCategory.Attributes.Add("readonly", "readonly");
                lstResidual.Attributes.Add("readonly", "readonly");
                lstCommission.Attributes.Add("readonly", "readonly");
                txtMasterNum.Attributes.Add("readonly", "readonly");
                txtUnoUsername.Attributes.Add("readonly", "readonly");
                txtUnoPassword.Attributes.Add("readonly", "readonly");
                
                if (txtSage.Text != "")
                    txtSage.Attributes.Add("readonly", "readonly");
                if (txtiPay1.Text != "")
                    txtiPay1.Attributes.Add("readonly", "readonly");
                if (txtiPay2.Text != "")
                    txtiPay2.Attributes.Add("readonly", "readonly");
                if (txtiPay3.Text != "")
                    txtiPay3.Attributes.Add("readonly", "readonly");
                if (txtIMS.Text != "")
                    txtIMS.Attributes.Add("readonly", "readonly");
                if (txtIMSQB.Text != "")
                    txtIMSQB.Attributes.Add("readonly", "readonly");
                if (txtChase.Text != "")
                    txtChase.Attributes.Add("readonly", "readonly");
                if (txtIPS.Text != "")
                    txtIPS.Attributes.Add("readonly", "readonly");
            }

        }//end if count not 0

    }//end function Populate

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'> { self.close() }</script>");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                RepInfoBL Rep = new RepInfoBL();
                string T1MasterNum = lstT1RepName.SelectedValue.ToString();
                //If Master Rep Number has changed for a Tier 1 Rep
                if ((txtMasterNum.Text.Trim() != MasterNum) && (T1MasterNum == MasterNum))
                    T1MasterNum = txtMasterNum.Text.Trim(); //set Tier 1 Master Number to be new Master Rep Number
                bool retVal = Rep.UpdatePartnerInfoCurrMon(txtRepName.Text.Trim(), txtCompanyName.Text.Trim(),
                    txtDBA.Text.Trim(), lstSageDeclined.SelectedValue.ToString().Trim(), txtSage.Text.Trim(), txtUnoUsername.Text.Trim(), txtUnoPassword.Text.Trim(),
                    txtiPay3.Text.Trim(), txtiPaySalesID.Text.Trim(), txtIMSQB.Text.Trim(), txtChase.Text.Trim(), txtIPS.Text.Trim(),
                    lstResidual.Text.Trim(), lstCategory.SelectedItem.Value, lstCommission.Text.Trim(),
                    txtMasterNum.Text.Trim(), T1MasterNum, Convert.ToInt32(lstCNPPackageList.SelectedItem.Value), 
                    Convert.ToInt32(lstCPPackageList.SelectedItem.Value), AffID, txtFundMin.Text.Trim(), 
                    txtRefMin.Text.Trim(), txtResidualMin.Text.Trim(), MasterNum);
                if (retVal)
                {
                    DisplayMessage("Rep Updated Successfully");
                    PartnerLogBL LogData = new PartnerLogBL();
                    retVal = LogData.InsertPartnerLog(Convert.ToInt32(Session["AffiliateID"]), "RepInfo Updated for Partner - " + txtRepName.Text.ToString().Trim() + ".");
                }
            }//end if page is valid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating Partner Information");
        }
    }//end submit button click

    //This function handles Tier1 repname changed event and populates rep info
    protected void lstT1RepName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PopulateDropdownSplits(lstT1RepName.SelectedValue.ToString());
            //if Rep is the same as the Tier 1 Rep
            if (lstT1RepName.SelectedValue == MasterNum)
            {
                pnlPayoutReqs.Visible = true;  //enable Panel for Tier 1 information    
            }
            
            /*else //Rep is being updated to a Tier 2 
            {
                //only for Sub-Reps
                pnlPayoutReqs.Visible = false;
                txtFundMin.Text = "";
                txtRefMin.Text = "";
                txtResidualMin.Text = ""; 

                RepInfoBL PartnerInfo = new RepInfoBL();
                DataSet ds = PartnerInfo.GetPartnerInfoCurrMon(MasterNum);
                DataRow dr = ds.Tables[0].Rows[0];
                lstResidual.SelectedValue = dr["RepSplit"].ToString();
                lstCommission.SelectedValue = dr["Comm"].ToString();
            }*/

            RepInfoBL PartnerInfo = new RepInfoBL();
            DataSet ds = PartnerInfo.GetPartnerInfoCurrMon(MasterNum);
            DataRow dr = ds.Tables[0].Rows[0];
            lstResidual.SelectedValue = dr["RepSplit"].ToString();
            lstCommission.SelectedValue = dr["Comm"].ToString();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error retrieving Rep Info Information");
        }
    }//end repname changed event

    protected void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (lstCategory.SelectedItem.Text == "PE")
            {
                lstResidual.SelectedIndex = lstResidual.Items.IndexOf(lstResidual.Items.FindByText("0"));
                //lstCommission.SelectedIndex = lstCommission.Items.IndexOf(lstCommission.Items.FindByText("0"));
            }
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Category Change Error - " + err.Message);
            DisplayMessage("Category Change Error");
        }
    }//end 

    //This function Validates Rep splits before applying for agents
    public string ValidateSplits()
    {
        string strRetVal = string.Empty;
        double RepSplit = 9;
        double CommSplit = 0;
        RepInfoBL Rep = new RepInfoBL();
        DataSet ds = Rep.GetPartnerSplits(Session["MasterNum"].ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            RepSplit = Convert.ToDouble(dr["RepSplit"]);
            CommSplit = Convert.ToDouble(dr["Comm"]);
        }//end if count not 0
        if ((lstResidual.Text != "") && (Convert.ToDouble(lstResidual.Text) > RepSplit))
            strRetVal = "Residual percent cannot exceed your RepSplit which is " + RepSplit + "%. Please set this less than " + RepSplit + "%";
        if ((lstCommission.Text != "") && (Convert.ToDouble(lstCommission.Text) > CommSplit))
            strRetVal = "Commission percent cannot exceed your Commission Split which is " + CommSplit + "%. Please set this less than " + CommSplit + "%"; ;
        return strRetVal;
    }

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
                //txtUnoPassword.Text = "";
                txtUnoPassword.Attributes.Add("value", "");
                txtUnoPassword.Enabled = false;

                //Set rate packages to iPayment
                lstCNPPackageList.SelectedValue = "191";
                lstCPPackageList.SelectedValue = "193";
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
            DisplayMessage("Error Changing Sage Rep Status.");
        }
    }//end sage declined list changed event

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
            //Get the Splits to be set on  Sub Rep)
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
