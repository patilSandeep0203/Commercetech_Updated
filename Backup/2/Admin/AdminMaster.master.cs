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

public partial class Admin_AdminMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MenuItemStyle styleAgent = new MenuItemStyle();
            styleAgent.BackColor = System.Drawing.Color.Blue;
            styleAgent.ForeColor = System.Drawing.Color.White;

            
        }

    }
}
