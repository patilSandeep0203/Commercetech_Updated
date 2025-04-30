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

public partial class CommVerify : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");

        if (!User.IsInRole("Admin") && (User.IsInRole("Employee")))
            Response.Redirect("~/login.aspx");

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {                
                string DBA = "";
                string Month = "";
                if (Request.Params.Get("Month") != null)
                    Month = Request.Params.Get("Month");

                if (Request.Params.Get("DBA") != null)
                    DBA = Request.Params.Get("DBA");

                if ((Month != "") && (DBA != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                {
                    lblHistory.Text = "Commissions history for " + DBA + " excluding the month of " + Month;
                    CommissionsBL Comm = new CommissionsBL();
                    PartnerDS.CommissionsDataTable dt  = Comm.GetCommissionsByDBA(DBA, Month);
                    if (dt.Rows.Count > 0)
                    {
                        grdComm.Visible = true;
                        grdComm.DataSource = dt;
                        grdComm.DataBind();
                    }
                    else
                        DisplayMessage("No Records found.");
                }//end if
                else
                    DisplayMessage("No Records found.");
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if page postback
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

}
