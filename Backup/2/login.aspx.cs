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

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)//This determines if the page has not been posted back to itself
        {
            //IMPORTANT - ALL FUNCTIONS THAT DISPLAY DATA ON A PAGE HAVE TO BE CALLED FROM
            //WITHIN THIS IF (!ISPOSTBACK) STATEMENT ON EVERY PAGE. CHECKING FOR USER AUTHENTICATION
            //MUST ALSO BE DONE FROM WITHIN THIS IF STATEMENT
            if ((Request.Params.Get("Authorization") != null))
            {
                if (Request.Params.Get("Authorization") == "False")
                    Response.Redirect("?Authorization=False");
            }
            
            try
            {
                if (Request.QueryString["Token"] != null) //Get token from the URL
                {
                    //Verify token in the token table and get the dataset
                    //containing Affiliateid, username and appname
                    UsersBL User = new UsersBL();
                    //To go to the VerifyToken function (or any function defined), get the cursor on the                     
                    //function name and press F12 to go to the definition
                    DataSet ds = User.VerifyToken(Request.QueryString["Token"].ToString());
                    if (ds.Tables[0].Rows.Count > 0) //Check if dataset returned has atleast one row
                    {
                        //Each dataset has multiple rows. To access each row, create a DataRow object as shown below
                        DataRow dr = ds.Tables[0].Rows[0]; //Rows[0] means get the data from the first row in the dataset
                        //Validate user based on the iUserID. Set session variables
                        int iValidate = ValidateUser(Convert.ToInt16(dr["iUserID"]));
                        if (iValidate == 1)//Means user is validated
                        {
                            //CreateCookie and redirect based on appname
                            FormsAuthentication.SetAuthCookie(dr["sLoginID"].ToString(), false);
                            string AppName = dr["sAppName"].ToString();
                            string strRedirectURL = Request["ReturnURL"];
                            if (strRedirectURL == null)
                            {
                                if (AppName.Contains("Partner"))
                                    Response.Redirect("Home.aspx", false);
                                else if (AppName == "Payroll")
                                    Response.Redirect("Payroll/ResidualsAdmin.aspx?Payroll=True", false);
                                else if (AppName == "CTC")
                                    Response.Redirect("Admin/ManagePartners.aspx?Admin=True", false);
                                else if (AppName == "ACT!")
                                    Response.Redirect("ACT/ExportACT.aspx?ACT=True", false);
                                else if (AppName == "Reports")
                                    Response.Redirect("Reports/CTCReports.aspx?Reports=True", false);
                                else if (AppName == "Online App Management")
                                    Response.Redirect("OnlineAppMgmt/main.aspx", false);
                                else if (AppName == "Documents And Logins")
                                    Response.Redirect("DocsLoginsMain.aspx", false);
                                else
                                    Response.Redirect("Home.aspx", false);
                            }
                            else
                                Response.Redirect(strRedirectURL);
                        }//end if valid user
                        else
                        {
                            if (iValidate == 0)
                                SetErrorMessage("Login Name and/or Password incorrect. Please try again");
                        }
                    }//end if count not 0
                    else
                        Response.Redirect("index.aspx?Authentication=False", false);
                }//end if token not null
                else
                {
                    //UNCOMMENT THIS LINE OF CODE BEFORE YOU BUILD THE PROJECT
                    ////THIS LINE OF CODE SHOULD ONLY BE COMMENTED OUT FOR DEBUGGING
                    Response.Redirect("index.aspx?Authentication=False", false);

                }
            }//end try
            catch (Exception err)
            {
                //Create an error log. The error log is located in the PartnerPortal directory on the server
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                SetErrorMessage("Error processing request. Please contact Technical Support.");
                //If you want to see the actual error message on the page, then uncomment the following line
                //SetErrorMessage(err.Message);
            }
        }//end if not post back
    }

    //This function displays error message on a label
    protected void SetErrorMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    //This function validates user with database
    protected int ValidateUser(int iUserID)
    {
        string strAccess = string.Empty;
        string LoginPassword = string.Empty;
        int AffiliateID = 0;
        string AffiliateName = string.Empty;
        string MasterNum = string.Empty;
        string T1MasterNum = string.Empty;

        UsersBL User = new UsersBL();
        bool bAccess = User.CheckIDExists(iUserID); //Call stored procedure GetLoginInfo
        if (bAccess)
        {
            AffiliateID = iUserID;

            //Get MasterNum from Affiliate ID
            AffiliatesBL Aff = new AffiliatesBL(AffiliateID);
            MasterNum = Aff.ReturnMasterNum();

            //Get T1MasterNum from Affiliate ID
            T1MasterNum = Aff.ReturnT1MasterNum();

            //Get Contact Name from Affiliate ID
            AffiliateName = Aff.ReturnContactName();

            //Set Session variables
            Session["AffiliateID"] = AffiliateID;
            Session["AffiliateName"] = AffiliateName;
            Session["MasterNum"] = MasterNum;
            Session["T1MasterNum"] = T1MasterNum;
            return 1;
        }
        else
            return 0;
    }//end function validate user    

    
}