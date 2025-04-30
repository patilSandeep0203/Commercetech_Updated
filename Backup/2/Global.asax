<%@ Application Language="C#" %>
<%@ Import Namespace="BusinessLayer" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        try
        {
            if (Request.IsAuthenticated)
            {
                string strAppID = ConfigurationManager.AppSettings["AppID"].ToString();
                UsersBL Roles = new UsersBL();
                ArrayList lstRoles = new ArrayList();
                System.Data.DataSet ds = Roles.GetRoleByApp(User.Identity.Name, Convert.ToInt32(strAppID) );
                if (ds.Tables[0].Rows.Count > 0)
                {
                    System.Data.DataRow dr = null;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dr = ds.Tables[0].Rows[i];
                        lstRoles.Add(dr["sRole"].ToString());
                    }//end for
                    string[] strRoleListArray = (string[])lstRoles.ToArray(typeof(string));
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(User.Identity, strRoleListArray);
                }//end if count not 0
                else
                    Response.Redirect("/Partner/NoAccess.htm", false);                
            }//end if request authenticated
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
