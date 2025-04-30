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

public partial class BuildTextLinks : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Agent"))
                Page.MasterPageFile = "~/AgentMisc.master";
            else if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "~/T1Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "~/UserMisc.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "~/site.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "~/Employee.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("login.aspx?Authentication=False");

            try
            {
                //Populate list
                BannersBL Links = new BannersBL();
                DataSet ds = Links.GetGroups();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lstLinks.DataSource = ds;
                    lstLinks.DataTextField = "GroupName";
                    lstLinks.DataValueField = "GroupID";
                    lstLinks.DataBind();
                }//end if count not 0
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }
    }//end page load

    //This function Populates the table
    public void Populate()
    {
        Style TextArea = new Style();
        TextArea.Width = new Unit(400);
        TextArea.Height = new Unit(80);
        TextArea.Font.Size = FontUnit.Point(8);
        TextArea.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        Style sHyperLink = new Style();
        sHyperLink.Font.Bold = true;
        sHyperLink.Font.Size = FontUnit.Smaller;
        sHyperLink.Font.Name = "Arial";
        sHyperLink.CssClass = "One";

        //Get Banners by Group ID
        BannersBL Links = new BannersBL();
        DataSet ds = Links.GetGroupsByGroupID(Convert.ToInt32(lstLinks.SelectedItem.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr;
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            Label lblHeader = new Label();

            lblHeader.Text = "<b>Static Text Links: </b> Use this code to display static text links with your Referral ID.";
            lblHeader.ApplyStyle(ValueLabelHeader);
            td.Controls.Add(lblHeader);
            td.BackColor = System.Drawing.Color.Maroon;
            tr.Cells.Add(td);
            tblTextLinks.Rows.Add(tr);
            HyperLink lnkExample;
            TextBox txtBannerLinks;
            tblTextLinks.Attributes.Add("style", "border-right: silver 1px solid;border-top: silver 1px solid; font-weight: bold; border-left: silver 1px solid;width: 350px; border-bottom: silver 1px solid; font-family: Arial;");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = ds.Tables[0].Rows[i];
                tr = new TableRow();
                td = new TableCell();
                lnkExample = new HyperLink();
                lnkExample.NavigateUrl = dr["TargetURL"].ToString().Trim() + "?" +
                    dr["ParamReferral"].ToString().Trim() + "=" +
                    Session["AffiliateID"].ToString().Trim();
                lnkExample.Text = dr["TextDisplay"].ToString().Trim();
                lnkExample.ApplyStyle(sHyperLink);
                lnkExample.Target = "_blank";
                
                txtBannerLinks = new TextBox();
                txtBannerLinks.ReadOnly = true;
                txtBannerLinks.Text = "<!-- Begin Affiliate Code -->" + System.Environment.NewLine +
                    "<a href=\"" + dr["TargetURL"].ToString().Trim() + "?" +
                    dr["ParamReferral"].ToString().Trim() + "=" +
                    Session["AffiliateID"].ToString().Trim() + "\">" + 
                    dr["TextDisplay"].ToString().Trim() + "</a>" +
                    System.Environment.NewLine + "<!-- End Affiliate Code -->";
                txtBannerLinks.ApplyStyle(TextArea);
                txtBannerLinks.Wrap = true;
                txtBannerLinks.TextMode = TextBoxMode.MultiLine;

                td.Attributes.Add("align", "center");
                td.Controls.Add(lnkExample);
                Label lblBlank = new Label();
                lblBlank.Text = "<br/><br/>";
                td.Controls.Add(lblBlank);
                td.Controls.Add(txtBannerLinks);
                tr.Cells.Add(td);

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.FromArgb(230, 237, 245);
                tblTextLinks.Rows.Add(tr);
                
            }//end for
        }//end if count not 0
    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Populate();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message
}
