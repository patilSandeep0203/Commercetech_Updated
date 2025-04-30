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
using System.Data.SqlClient;
using BusinessLayer;

public partial class ExportACT : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Employee"))
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

        if (!Session.IsNewSession)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Response.Redirect("~/logout.aspx");
        }

        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        
        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            
            txtEmail.Focus();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            if (Page.IsValid)
            {
                ExportActBL ActRecords = new ExportActBL();
                DataSet ds = ActRecords.GetSummaryDataFromAct( txtEmail.Text.Trim());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grdResidual.DataSource = ds;
                    grdResidual.DataBind();
                }
                else
                {
                    DisplayMessage("No Records found.");
                }
            }//end if page isvalid
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }//end submit button click
    
    //This function Exports the record from ACT
    public void ExportACTRecord(string p_ContactID, string Email)
    {
        try
        {
            //check if email already exists as a user name
            ExportActBL App = new ExportActBL();
            bool retValue = App.CheckLoginNameExists(Email);
            if ((retValue) || (Email == ""))
            {
                pnlSameLoginID.Visible = true;
            }
            else
            {
                pnlSameLoginID.Visible = false;
                //Export the ACT record based on the contact ID
                ExportActBL ExportNewRecord = new ExportActBL();
                //send 0 as login so email can be used for login ID
                string strMessage = ExportNewRecord.ExportData(p_ContactID, "0");
                DisplayMessage(strMessage);
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }//end function ExportActRecord

    protected void btnSubmitNewLogin_Click(object sender, EventArgs e)
    {
        try
        {
            //Export the ACT record based on the contact ID
            ExportActBL ExportNewRecord = new ExportActBL();
            string strMessage = ExportNewRecord.ExportData(lblContactID.Text, txtLogin.Text);
            DisplayMessage(strMessage);
            pnlSameLoginID.Visible = false;
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Export to Online App Error - " + err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void grdResidual_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Export")//If the export button is click in the grid
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdResidual.Rows[index];
                string ContactID = Server.HtmlDecode(grdRow.Cells[1].Text);
                string Email = Server.HtmlDecode(grdRow.Cells[5].Text);
                if (ContactID.Trim() != "")
                    ExportACTRecord(ContactID, Email);
                lblContactID.Text = ContactID.ToString();
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error exporting data");
        }
    }
}
