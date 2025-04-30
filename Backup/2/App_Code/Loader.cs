using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Loader
/// </summary>
public class Loader : System.Web.UI.Page
{
    public void initNotify(string StrSplash)
    {
        // Only do this on the first call to the page
        if ((!IsCallback) && (!IsPostBack))
        {
            //Register loadingNotifier.js for showing the Progress Bar
            Response.Write(
                string.Format(
                    "<script type=\"text/javascript\" src=\"/PartnerPortal/loadingNotifier.js\"></script><script language=\"javascript\" type=\"text/javascript\">initLoader('{0}');</script>", StrSplash));
            // Send it to the client
            Response.Flush();

        }

    }

    public void Notify(string strPercent, string strMessage)
    {
        // Only do this on the first call to the page
        if ((!IsCallback) && (!IsPostBack))
        {
            //Update the Progress bar

            Response.Write(string.Format("<script language='javascript' type='text/javascript'>setProgress({0},'{1}'); </script>", strPercent, strMessage));
            Response.Flush();

        }

    }
}
