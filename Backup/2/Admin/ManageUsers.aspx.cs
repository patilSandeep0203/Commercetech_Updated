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
using DLPartner;
using BusinessLayer;

public partial class ManageUsers : System.Web.UI.Page
{
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

        if (!User.IsInRole("Admin"))
        {
            //Response.Redirect("login.aspx?Authentication=False");
            DisplayMessage("You are not authorized to view this resource.");
            btnSubmit.Enabled = false;
        }

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

        }//end if not post back
    }//end page load

    public void Populate(string strAccess, string AppName)
    {
        try
        {
            lblError.Visible = false;
            grdManageUsers.Visible = true;
            //Get users information and bind it to the grid
            UserRolesBL Users = new UserRolesBL();
            DataSet ds = Users.GetUsersByAccess(strAccess, AppName);
            if (ds.Tables[0].Rows.Count > 0)
            {      
                grdManageUsers.DataSource = ds;
                grdManageUsers.DataBind();
            }//end if count not 0 
            else
            {
                grdManageUsers.Visible = false;
                DisplayMessage("No Records Found.");
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating user information");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlUpdate.Visible = false;
    }

    protected void grdManageUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditInfo")//If Edit button in grid is clicked
            {
                pnlUpdate.Visible = true;
                int index = Convert.ToInt32(e.CommandArgument);//Get the index of the row where the button was clicked
                GridViewRow grdRow = grdManageUsers.Rows[index];//Get the row
                int AffiliateID = Convert.ToInt32(grdRow.Cells[0].Text);//Get the value in the first cell
                PopulateUser(AffiliateID);
            }//end if command name
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error loading user information");
        }
    }//end rowcommand

    //This function populates info for user info to be updated
    public void PopulateUser(int AffiliateID)
    {
        //Populate User Details
        AffiliatesBL Aff = new AffiliatesBL(AffiliateID);

        lblAffiliateID.Text = AffiliateID.ToString();
        lblContact.Text = Aff.ReturnContactName();

        //Login Name
        UsersBL loginInfo = new UsersBL();
        DataSet dsUser = loginInfo.GetLoginInfo(AffiliateID);
        DataRow drUser = dsUser.Tables[0].Rows[0];
        lblLoginName.Text = drUser["sLoginID"].ToString().Trim();

        //Password Phrase hint
        PartnerDS.AffiliatesDataTable affDT = Aff.GetAffiliate();
        lblPasswordHint.Text = affDT[0].Phrase.ToString().Trim();
  
        //Populate User Roles
        UserRolesBL Roles = new UserRolesBL();
        DataSet dsRoles = Roles.GetRolesByAffiliateID(AffiliateID);
        if (dsRoles.Tables[0].Rows.Count > 0)
        {
            //Get user roles and check radio buttons based on user role and app
            DataRow dr = dsRoles.Tables[0].Rows[0];
            #region iAppID 1 - Partner
            if (dr["iAppId"].ToString().Trim() == "1")
            {
                //Uncheck all previously checked radio buttons
                rdbPartnerAdmin.Checked = false;
                rdbPartnerEmployee.Checked = false;
                rdbPartnerT1Agent.Checked = false;
                rdbPartnerAgent.Checked = false;
                rdbPartnerAffiliate.Checked = false;
                rdbPartnerReseller.Checked = false;
                rdbPartnerNoAccess.Checked = false;
                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbPartnerAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbPartnerEmployee.Checked = true;
                        break;
                    case "T1Agent":
                        rdbPartnerT1Agent.Checked = true;
                        break;
                    case "Agent":
                        rdbPartnerAgent.Checked = true;
                        break;
                    case "Affiliate":
                        rdbPartnerAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbPartnerReseller.Checked = true;
                        break;
                    default:
                        rdbPartnerNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 1
            #endregion

            dr = dsRoles.Tables[0].Rows[2];
            #region iAppID 3 - ACT
            if (dr["iAppId"].ToString().Trim() == "3")
            {
                //lblACT.ToolTip = dr["Description"].ToString().Trim();
                rdbACTAdmin.Checked = false;
                rdbACTEmployee.Checked = false;
                rdbACTAgent.Checked = false;
                rdbACTAffiliate.Checked = false;
                rdbACTReseller.Checked = false;
                rdbACTNoAccess.Checked = false;
                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbACTAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbACTEmployee.Checked = true;
                        break;
                    case "Agent":
                        rdbACTAgent.Checked = true;
                        break;
                    case "Affiliate":
                        rdbACTAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbACTReseller.Checked = true;
                        break;
                    default:
                        rdbACTNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 3
            #endregion

            dr = dsRoles.Tables[0].Rows[3];
            #region iAppID 4 - Reports
            if (dr["iAppId"].ToString().Trim() == "4")
            {
                //lblReports.ToolTip = dr["Description"].ToString().Trim();
                rdbReportsAdmin.Checked = false;
                rdbReportsEmployee.Checked = false;
                rdbReportsT1Agent.Checked = false;
                rdbReportsAgent.Checked = false;
                rdbReportsAffiliate.Checked = false;
                rdbReportsReseller.Checked = false;
                rdbReportsNoAccess.Checked = false;
                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbReportsAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbReportsEmployee.Checked = true;
                        break;
                    case "T1Agent":
                        rdbReportsT1Agent.Checked = true;
                        break;
                    case "Agent":
                        rdbReportsAgent.Checked = true;
                        break;
                    case "Affiliate":
                        rdbReportsAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbReportsReseller.Checked = true;     
                        break;
                    default:
                        rdbReportsNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 4
            #endregion

            dr = dsRoles.Tables[0].Rows[4];
            #region iAppID 5 - Online App Mgmt
            if (dr["iAppId"].ToString().Trim() == "5")
            {
                //lblOnlineAppMgmt.ToolTip = dr["Description"].ToString().Trim();
                rdbMgmtAdmin.Checked = false;
                rdbMgmtEmployee.Checked = false;
                rdbMgmtAgent.Checked = false;
                rdbMgmtAffiliate.Checked = false;
                rdbMgmtReseller.Checked = false;
                rdbMgmtNoAccess.Checked = false;

                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbMgmtAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbMgmtEmployee.Checked = true;
                        break;
                    case "Agent":
                        rdbMgmtAgent.Checked = true;
                        break;
                    case "Affiliate":
                        rdbMgmtAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbMgmtReseller.Checked = true;
                        break;
                    default:
                        rdbMgmtNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 5
            #endregion

            dr = dsRoles.Tables[0].Rows[5];
            #region iAppID 6 - CTC
            if (dr["iAppId"].ToString().Trim() == "6")
            {
                //lblCTC.ToolTip = dr["Description"].ToString().Trim();

                rdbCTCAdmin.Checked = false;
                rdbCTCEmployee.Checked = false;
                rdbCTCAgent.Checked = false;
                rdbCTCAffiliate.Checked = false;
                rdbCTCReseller.Checked = false;
                rdbCTCNoAccess.Checked = false;
                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbCTCAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbCTCEmployee.Checked = true;
                        break;
                    case "Agent":
                        rdbCTCAgent.Checked = true;
                        break;
                    case "Affiliate":         
                        rdbCTCAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbCTCReseller.Checked = true;
                        break;
                    default:
                        rdbCTCNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 6
            #endregion

            dr = dsRoles.Tables[0].Rows[6];
            #region iAppID 7 - Payroll
            if (dr["iAppId"].ToString().Trim() == "7")
            {
                //lblPayroll.ToolTip = dr["Description"].ToString().Trim();
                rdbPayrollAdmin.Checked = false;
                rdbPayrollEmployee.Checked = false;
                rdbPayrollAgent.Checked = false;
                rdbPayrollAffiliate.Checked = false;
                rdbPayrollReseller.Checked = false;
                rdbPayrollNoAccess.Checked = false;
                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbPayrollAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbPayrollEmployee.Checked = true;
                        break;
                    case "Agent":
                        rdbPayrollAgent.Checked = true;

                        break;
                    case "Affiliate":
                        rdbPayrollAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbPayrollReseller.Checked = true;
                        break;
                    default:

                        rdbPayrollNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 7
            #endregion

            dr = dsRoles.Tables[0].Rows[7];
            #region iAppID 8 - Documents And Logins
            if (dr["iAppId"].ToString().Trim() == "8")
            {
                //lblPayroll.ToolTip = dr["Description"].ToString().Trim();
                rdbDocAdmin.Checked = false;
                rdbDocEmployee.Checked = false;
                rdbDocAgent.Checked = false;
                rdbDocAffiliate.Checked = false;
                rdbDocReseller.Checked = false;
                rdbDocNoAccess.Checked = false;
                switch (dr["Access"].ToString().Trim())
                {
                    case "Admin":
                        rdbDocAdmin.Checked = true;
                        break;
                    case "Employee":
                        rdbDocEmployee.Checked = true;
                        break;
                    case "Agent":
                        rdbDocAgent.Checked = true;

                        break;
                    case "Affiliate":
                        rdbDocAffiliate.Checked = true;
                        break;
                    case "Reseller":
                        rdbDocReseller.Checked = true;
                        break;
                    default:

                        rdbDocNoAccess.Checked = true;
                        break;
                }//end switch
            }//end if iAppId = 8
            #endregion

            //Get description for each role. If desc is blank, then hide radio button, which means there is no
            //role refined for that application.
            DataSet dsDesc = Roles.GetRolesDescription();
            if (dsDesc.Tables[0].Rows.Count > 0)
            {
                DataRow drDesc = dsDesc.Tables[0].Rows[0];
                #region iAppId 1
                if (drDesc["Admin"].ToString() != "")
                    rdbPartnerAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbPartnerAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbPartnerEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbPartnerEmployee.Visible = false;

                if (drDesc["T1Agent"].ToString() != "")
                    rdbPartnerT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbPartnerT1Agent.Visible = false;

                if (drDesc["Agent"].ToString() != "")
                    rdbPartnerAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbPartnerAgent.Visible = false;

                if (drDesc["Affiliate"].ToString() != "")
                    rdbPartnerAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbPartnerAffiliate.Visible = false;

                if (drDesc["Reseller"].ToString() != "")
                    rdbPartnerReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbPartnerReseller.Visible = false;

                if (drDesc["None"].ToString() != "")
                    rdbPartnerNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbPartnerNoAccess.Visible = false;
                #endregion

                drDesc = dsDesc.Tables[0].Rows[2];
                #region iAppId 3
                if (drDesc["Admin"].ToString() != "")
                    rdbACTAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbACTAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbACTEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbACTEmployee.Visible = false;


                if (drDesc["Agent"].ToString() != "")
                    rdbACTAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbACTAgent.Visible = false;

		      if (drDesc["T1Agent"].ToString() != "")
                    rdbACTT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbACTT1Agent.Visible = false;
                    
                if (drDesc["Affiliate"].ToString() != "")
                    rdbACTAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbACTAffiliate.Visible = false;

                if (drDesc["Reseller"].ToString() != "")
                    rdbACTReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbACTReseller.Visible = false;

                if (drDesc["None"].ToString() != "")
                    rdbACTNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbACTNoAccess.Visible = false;
                #endregion

                drDesc = dsDesc.Tables[0].Rows[3];
                #region iAppId 4
                if (drDesc["Admin"].ToString() != "")
                    rdbReportsAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbReportsAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbReportsEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbReportsEmployee.Visible = false;

                if (drDesc["T1Agent"].ToString() != "")
                    rdbReportsT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbReportsT1Agent.Visible = false;


                if (drDesc["Agent"].ToString() != "")
                    rdbReportsAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbReportsAgent.Visible = false;

                if (drDesc["Affiliate"].ToString() != "")
                    rdbReportsAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbReportsAffiliate.Visible = false;

                if (drDesc["Reseller"].ToString() != "")
                    rdbReportsReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbReportsReseller.Visible = false;

                if (drDesc["None"].ToString() != "")
                    rdbReportsNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbReportsNoAccess.Visible = false;
                #endregion

                drDesc = dsDesc.Tables[0].Rows[4];
                #region iAppId 5
                if (drDesc["Admin"].ToString() != "")
                    rdbMgmtAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbMgmtAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbMgmtEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbMgmtEmployee.Visible = false;

                if (drDesc["T1Agent"].ToString() != "")
                    rdbMgmtT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbMgmtT1Agent.Visible = false;

                if (drDesc["Agent"].ToString() != "")
                    rdbMgmtAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbMgmtAgent.Visible = false;

                if (drDesc["Affiliate"].ToString() != "")
                    rdbMgmtAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbMgmtAffiliate.Visible = false;

                if (drDesc["Reseller"].ToString() != "")
                    rdbMgmtReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbMgmtReseller.Visible = false;

                if (drDesc["None"].ToString() != "")
                    rdbMgmtNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbMgmtNoAccess.Visible = false;
                #endregion

                drDesc = dsDesc.Tables[0].Rows[5];
                #region iAppId 6
                if (drDesc["Admin"].ToString() != "")
                    rdbCTCAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbCTCAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbCTCEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbCTCEmployee.Visible = false;

                if (drDesc["T1Agent"].ToString() != "")
                    rdbCTCT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbCTCT1Agent.Visible = false;

                if (drDesc["Agent"].ToString() != "")
                    rdbCTCAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbCTCAgent.Visible = false;

                if (drDesc["Affiliate"].ToString() != "")
                    rdbCTCAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbCTCAffiliate.Visible = false;

                if (drDesc["Reseller"].ToString() != "")
                    rdbCTCReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbCTCReseller.Visible = false;

                if (drDesc["None"].ToString() != "")
                    rdbCTCNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbCTCNoAccess.Visible = false;
                #endregion

                drDesc = dsDesc.Tables[0].Rows[6];
                #region iAppId 7
                if (drDesc["Admin"].ToString() != "")
                    rdbPayrollAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbPayrollAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbPayrollEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbPayrollEmployee.Visible = false;

                if (drDesc["T1Agent"].ToString() != "")
                    rdbPayrollT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbPayrollT1Agent.Visible = false;

                if (drDesc["Agent"].ToString() != "")
                    rdbPayrollAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbPayrollAgent.Visible = false;

                if (drDesc["Affiliate"].ToString() != "")
                    rdbPayrollAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbPayrollAffiliate.Visible = false;

                if (drDesc["Reseller"].ToString() != "")
                    rdbPayrollReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbPayrollReseller.Visible = false;

                if (drDesc["None"].ToString() != "")
                    rdbPayrollNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbPayrollNoAccess.Visible = false;
                #endregion

                drDesc = dsDesc.Tables[0].Rows[7];
                #region iAppId 8
                if (drDesc["Admin"].ToString() != "")
                    rdbDocAdmin.ToolTip = drDesc["Admin"].ToString();
                else
                    rdbDocAdmin.Visible = false;

                if (drDesc["Employee"].ToString() != "")
                    rdbDocEmployee.ToolTip = drDesc["Employee"].ToString();
                else
                    rdbDocEmployee.Visible = false;

                if (drDesc["T1Agent"].ToString() != "")
                    rdbDocT1Agent.ToolTip = drDesc["T1Agent"].ToString();
                else
                    rdbDocT1Agent.Visible = false;

                if (drDesc["Agent"].ToString() != "")
                    rdbDocAgent.ToolTip = drDesc["Agent"].ToString();
                else
                    rdbDocAgent.Visible = false;

                if (drDesc["Affiliate"].ToString() != "")
                    rdbDocAffiliate.ToolTip = drDesc["Affiliate"].ToString();
                else
                    rdbDocAffiliate.Visible = true;

                if (drDesc["Reseller"].ToString() != "")
                    rdbDocReseller.ToolTip = drDesc["Reseller"].ToString();
                else
                    rdbDocReseller.Visible = true;

                if (drDesc["None"].ToString() != "")
                    rdbDocNoAccess.ToolTip = drDesc["None"].ToString();
                else
                    rdbDocNoAccess.Visible = false;
                #endregion
            }//end if count not 0
        }//end if count not 0
    }//end PopulateUser

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
            Populate(lstSelAccess.SelectedItem.Value, lstAppList.SelectedItem.Value);
            lblNewPassword.Visible = false;
            pnlUpdate.Visible = false;
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        pnlConfirm.Visible = true;
    }

    protected void btnCreateRecordNo_Click(object sender, EventArgs e)
    {
        pnlConfirm.Visible = false;
    }

    protected void btnCreateRecordYes_Click(object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            Random random = new Random();
            char ch;
            //Generate a random password string
            for (int i = 0; i < 5; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            random = new Random();
            builder.Append(random.Next(1000, 9999));
            string Password = builder.ToString().ToLower();
            UsersBL User = new UsersBL();
            User.ResetAffiliatePassword(Convert.ToInt32(lblAffiliateID.Text.Trim()), Password);
            lblNewPassword.Text = "The password has been reset to : <br/>" + Password + "<br/> You can copy and email this password to the user. The user can change it by going to the Edit Information page.";
            lblNewPassword.Visible = true;
            pnlConfirm.Visible = false;
            pnlUpdate.Visible = false;

        }//end try
        catch (Exception err)
        {
            //CreateLog Log = new CreateLog();
            //Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }

    protected void btnUpdateRoles_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                string Access = string.Empty;
                UserRolesBL Roles = new UserRolesBL();
                #region iAppID 1 - Partner Roles
                if (rdbPartnerAdmin.Checked)
                    Access = "Admin";
                else if (rdbPartnerEmployee.Checked)
                    Access = "Employee";
                else if (rdbPartnerAgent.Checked)
                    Access = "Agent";
                else if (rdbPartnerT1Agent.Checked)
                    Access = "T1Agent";
                else if (rdbPartnerAffiliate.Checked)
                    Access = "Affiliate";
                else if (rdbPartnerReseller.Checked)
                    Access = "Reseller";
                else if (rdbPartnerNoAccess.Checked)
                    Access = "None";
                int iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "1", Access);
                #endregion

                #region iAppID 3 - ACT Roles
                if (rdbACTAdmin.Checked)
                    Access = "Admin";
                else if (rdbACTEmployee.Checked)
                    Access = "Employee";
                else if (rdbACTAgent.Checked)
                    Access = "Agent";
                else if (rdbACTAffiliate.Checked)
                    Access = "Affiliate";
                else if (rdbACTReseller.Checked)
                    Access = "Reseller";
                else if (rdbACTNoAccess.Checked)
                    Access = "None";
                iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "3", Access);
                #endregion

                #region iAppID 4 - Reports Roles
                if (rdbReportsAdmin.Checked)
                    Access = "Admin";
                else if (rdbReportsEmployee.Checked)
                    Access = "Employee";
                else if (rdbReportsAgent.Checked)
                    Access = "Agent";
                else if (rdbPartnerT1Agent.Checked)
                    Access = "T1Agent";
                else if (rdbReportsAffiliate.Checked)
                    Access = "Affiliate";
                else if (rdbReportsReseller.Checked)
                    Access = "Reseller";
                else if (rdbReportsNoAccess.Checked)
                    Access = "None";
                iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "4", Access);
                #endregion

                #region iAppID 5 - Online App Mgmt Roles
                if (rdbMgmtAdmin.Checked)
                    Access = "Admin";
                else if (rdbMgmtEmployee.Checked)
                    Access = "Employee";
                else if (rdbMgmtAgent.Checked)
                    Access = "Agent";
                else if (rdbMgmtAffiliate.Checked)
                    Access = "Affiliate";
                else if (rdbMgmtReseller.Checked)
                    Access = "Reseller";
                else if (rdbMgmtNoAccess.Checked)
                    Access = "None";
                iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "5", Access);
                #endregion

                #region iAppID 6 - CTC Roles
                if (rdbCTCAdmin.Checked)
                    Access = "Admin";
                else if (rdbCTCEmployee.Checked)
                    Access = "Employee";
                else if (rdbCTCAgent.Checked)
                    Access = "Agent";
                else if (rdbCTCAffiliate.Checked)
                    Access = "Affiliate";
                else if (rdbCTCReseller.Checked)
                    Access = "Reseller";
                else if (rdbCTCNoAccess.Checked)
                    Access = "None";
                iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "6", Access);
                #endregion

                #region iAppID 7 - Payroll Roles
                if (rdbPayrollAdmin.Checked)
                    Access = "Admin";
                else if (rdbPayrollEmployee.Checked)
                    Access = "Employee";
                else if (rdbPayrollAgent.Checked)
                    Access = "Agent";
                else if (rdbPayrollAffiliate.Checked)
                    Access = "Affiliate";
                else if (rdbPayrollReseller.Checked)
                    Access = "Reseller";
                else if (rdbPayrollNoAccess.Checked)
                    Access = "None";

                iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "7", Access);
                #endregion

                #region iAppID 8 - Payroll Roles
                if (rdbDocAdmin.Checked)
                    Access = "Admin";
                else if (rdbDocEmployee.Checked)
                    Access = "Employee";
                else if (rdbDocAgent.Checked)
                    Access = "Agent";
                else if (rdbDocAffiliate.Checked)
                    Access = "None";
                else if (rdbDocReseller.Checked)
                    Access = "None";
                else if (rdbDocNoAccess.Checked)
                    Access = "None";
                iRetVal = Roles.UpdateUserRoles(lblAffiliateID.Text.Trim(), "8", Access);
                #endregion
                pnlUpdate.Visible = false;
                DisplayMessage("Roles Updated");
                PartnerLogBL LogData = new PartnerLogBL();
                LogData.InsertPartnerLog(Convert.ToInt32(Session["AffiliateID"]), "User Roles Updated for Partner - " + lblContact.Text.ToString().Trim() + ".");
            }
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
}
