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

public partial class AdminHistory : System.Web.UI.Page
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
            Response.Redirect("login.aspx");

        if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
            Response.Redirect("~/login.aspx?Authentication=False");
                
        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                Populate();
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error populating application history");
            }
        }//end if not post back
    }//end page load

    //This function populates table
    public void Populate()
    {
        PartnerLogBL LogData = new PartnerLogBL();
        DataSet ds = LogData.GetLogForRates();
        if (ds.Tables[0].Rows.Count > 0)
        {
            grdHistory.DataSource = ds;
            grdHistory.DataBind();
        }//end if count not 0
        else
            DisplayMessage("No Records Found");
    }//end function Populate

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}