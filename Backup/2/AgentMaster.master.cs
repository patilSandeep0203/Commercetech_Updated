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

public partial class AgentMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strURL = Request.Url.ToString();
        if ((strURL.Contains("'")) || (strURL.Contains("--")) || (strURL.Contains("#")) || (strURL.Contains(";")) || (strURL.Contains("exec")) || (strURL.Contains("EXEC")) || (strURL.Contains("<")) || (strURL.Contains(">")) || (strURL.ToLower().Contains("script")))
        {
            Response.Redirect("DefaultError.htm");
        }
    }
}
