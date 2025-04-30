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
using System.Security.Cryptography;

public partial class AppList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;
     
        if ( Session.IsNewSession)
            Response.Redirect("login.aspx?SessionExpired=True");
        
         HttpCookie ck = Request.Cookies.Get("firstaffiliates");
         if (ck == null)
            Response.Redirect("login.aspx?Authentication=False");

        if (!IsPostBack)
        {
            Style errLabel = new Style();
            errLabel.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            errLabel.ForeColor = System.Drawing.Color.Black;
            errLabel.BorderColor = System.Drawing.Color.Red;
            errLabel.BorderStyle = BorderStyle.Solid;
            errLabel.BorderWidth = Unit.Pixel(1);
            errLabel.Font.Size = FontUnit.Small;
            lblError.ApplyStyle(errLabel);

            errLabel = new Style();
            errLabel.ForeColor = System.Drawing.Color.SeaGreen;
            errLabel.Font.Size = FontUnit.Point(12);
            lblWelcome.ApplyStyle(errLabel);

            try
            {
                lblError.Visible = false;
                int iValidate = GetAppList(Session["Login"].ToString());
                if (iValidate == 1)
                    pnlGrid.Visible = true;
                else
                {
                    if (iValidate == 2)
                        SetErrorMessage("Error Retrieving Application List");
                }
            }//end try
            catch (Exception err)
            {
                SetErrorMessage(err.Message);
            }
        }//end if not post back
    }

    //This function displays error message on a label
    protected void SetErrorMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    public void grdApps_ItemCommand(object source,
       System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        bool redirect = false;
        string token = String.Empty;
        LinkButton lb;

        try
        {
            lb = (LinkButton)e.Item.Cells[0].Controls[1];

            UsersBL User = new UsersBL();
            // Create a Token for this user/app
            token = User.CreateLoginToken(
              lb.Text,
              Session["Login"].ToString(),
              Convert.ToInt16(lb.Attributes["UserID"]),
              Convert.ToInt32(lb.Attributes["AppID"]));

            redirect = true;
        }
        catch (Exception ex)
        {
            redirect = false;
            SetErrorMessage(ex.Message);
        }

        if (redirect)
        {
            // Redirect to web application 
            // passing in the generated token
            Response.Redirect(e.CommandArgument.ToString() + "?Token=" + token, false);
        }
    }

   
    //This function validates user with database
    protected int GetAppList(string strUserName)
    {
        UsersBL User = new UsersBL();
        DataSet dsAccess = User.GetAppsByLoginID(strUserName);
        if (dsAccess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsAccess.Tables[0].Rows[0];
            grdApps.DataSource = dsAccess;
            grdApps.DataBind();
            return 1;
        }
        else
            return 2;
    }//end function validate user    

}