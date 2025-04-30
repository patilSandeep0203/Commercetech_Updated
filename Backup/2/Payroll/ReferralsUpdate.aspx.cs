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

public partial class ReferralsUpdate : System.Web.UI.Page
{
    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    public static string CommissionID = "";
    public static string Month = "";
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
            CommissionID = Convert.ToString(Request.Params.Get("CommID"));

        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            try
            {
                if (Request.Params.Get("Month") != null)
                    Month = Request.Params.Get("Month").ToString();

                Populate();
                
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback
    }//end page load

    //This function Populates Update Commissions Info
    public void Populate()
    {
        ReferralsBL ReferralsInfo = new ReferralsBL();
        DataSet ds = ReferralsInfo.GetReferralDetailFromID(CommissionID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblDBA.Text = dr["DBA"].ToString().Trim();
            lblMerchantID.Text = dr["MerchantID"].ToString().Trim();
            lblRepName.Text = dr["RepName"].ToString().Trim();                        
            lblProduct.Text = dr["Product"].ToString().Trim();
            lblUnits.Text = Convert.ToInt32(dr["Units"]).ToString().Trim();            
            txtRefPaid.Text = dr["RefTotal"].ToString().Trim();
            lblReferredBy.Text = dr["ReferredBy"].ToString().Trim();
            lblReferralID.Text = dr["ReferralID"].ToString().Trim();
            lblAmount.Text = dr["Total"].ToString().Trim();
            lblMonth.Text = Month;
        }//end if count not 0
    }//end function Populate

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            ReferralsBL Update = new ReferralsBL();
            bool retVal = Update.UpdateReferralsInfo(txtRefPaid.Text, CommissionID);
            if (retVal)
                DisplayMessage("Information Updated Successfully");
            else
                DisplayMessage("Error Updating Referral");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Updating Referral");
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
