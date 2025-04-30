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
using System.Xml;
using System.Text;
using AjaxControlToolkit;

public partial class Home : Loader
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)//Check if session variables have been set, else redirect to logout
                Response.Redirect("../logout.aspx");
            //This code changes the master file based on the access value. The master file has a menu and each
            //access type has a different menu
            if (User.IsInRole("Agent"))
                Page.MasterPageFile = "~/AgentMaster.master";
            else if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "~/T1Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "~/MasterPage.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "~/site.master";
            if (User.IsInRole("Employee"))
                Page.MasterPageFile = "~/Employee.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
        if (!User.IsInRole("Admin"))
        {
            TabContainerGoals.Controls.Remove(TabDeleteGoals);
            TabContainerGoals.Controls.Remove(TabAddGoals);
            TabContainerGoals.Controls.Remove(TabAddNews);
            TabContainerGoals.Controls.Remove(TabDeleteNews);
            TabGoals.Width = Unit.Percentage(100);
        }
    }

    public static int CPPID = 0;
    public static int CNPPID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //This code does not allow the page to be cached
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (Session.IsNewSession)
            Response.Redirect("login.aspx");

        if (!Page.IsPostBack)
        {
            //Check if the user has been authenticated. User.Identity.IsAuthenticated is a built-in aspx boolean
            //variable which is set automatically on the login page when an authentication cookie is created.
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("login.aspx?Authentication=False");
            try
            {
                //This sets style for the Add News text area
                Style TextArea = new Style();
                TextArea.Width = new Unit(500);
                TextArea.Height = new Unit(50);
                TextArea.Font.Size = FontUnit.Point(8);
                TextArea.Font.Name = "Arial";
                txtNews.ApplyStyle(TextArea);

                lblTodayDate.Text = DateTime.Now.ToString();//Display date and time on the home page

                #region NEW APP REMINDER
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    OnlineAppSummaryBL Apps = new OnlineAppSummaryBL();
                    DataSet ds = Apps.GetNewAppIDs("ALL");
                    DataSet dsUnsync = Apps.GetUnsyncAppbyRep("ALL");
                    DataSet dsNewUpload = Apps.GetNewUploadbyRep("ALL");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        AnimationExtender1.Enabled = true;
                        lblNewapps.Visible = true;
                        lnkNewapps.Visible = true;
                        lnkNewapps.Text = "You have " + ds.Tables[0].Rows.Count + " New Online App(s).";
                        lnkNewapps.ForeColor = System.Drawing.Color.Red;
                        //lblNewapps.Text = "You have " + ds.Tables[0].Rows.Count + " New Online App(s).";
                        //lblNewapps.ForeColor = System.Drawing.Color.Red;
                    }
                    //else
                        //AnimationExtender1.Enabled = false;

                    if (dsUnsync.Tables[0].Rows.Count > 0)
                    {
                        AnimationExtender1.Enabled = true;
                        lblNewapps.Visible = true;
                        lnkUnsynched.Visible = true;
                        lnkUnsynched.Text = "You have " + dsUnsync.Tables[0].Rows.Count + " Unsynched Online App(s).";
                        lnkUnsynched.ForeColor = System.Drawing.Color.Red;
                    }
                    //else
                        //AnimationExtender1.Enabled = false;

                    if (dsNewUpload.Tables[0].Rows.Count > 0)
                    {
                        AnimationExtender1.Enabled = true;
                        lblNewapps.Visible = true;
                        lnkFileUploaded.Visible = true;
                        lnkFileUploaded.Text = dsNewUpload.Tables[0].Rows.Count + " of your merchant(s) recently uploaded documents.";
                        lnkFileUploaded.ForeColor = System.Drawing.Color.Red;
                    }
                    //else
                        //AnimationExtender1.Enabled = false;

                    if (!((dsNewUpload.Tables[0].Rows.Count > 0) || (dsUnsync.Tables[0].Rows.Count > 0) || (ds.Tables[0].Rows.Count > 0)))
                    {
                        AnimationExtender1.Enabled = false;
                    }
                }
                else if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                {
                    OnlineAppSummaryBL Apps = new OnlineAppSummaryBL();
                    DataSet ds = Apps.GetNewAppIDs(Session["MasterNum"].ToString().Trim());
                    DataSet dsUnsync = Apps.GetUnsyncAppbyRep(Session["MasterNum"].ToString().Trim());
                    DataSet dsNewUpload = Apps.GetNewUploadbyRep(Session["MasterNum"].ToString().Trim());
                    //If the count of New App IDs is greater than 0
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblNewapps.Visible = true;
                        lnkNewapps.Visible = true;
                        lnkNewapps.Text = "You have " + ds.Tables[0].Rows.Count + " New Online App(s).";
                        lnkNewapps.ForeColor = System.Drawing.Color.Red;
                        //lblNewapps.Text = "You have " + ds.Tables[0].Rows.Count + " New Online Apps.";
                        //lblNewapps.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                        AnimationExtender1.Enabled = false;

                    if (dsUnsync.Tables[0].Rows.Count > 0)
                    {
                        AnimationExtender1.Enabled = true;
                        lblNewapps.Visible = true;
                        lnkUnsynched.Visible = true;
                        lnkUnsynched.Text = "You have " + dsUnsync.Tables[0].Rows.Count + " Unsynched Online App(s).";
                        lnkUnsynched.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                        AnimationExtender1.Enabled = false;

                    if (dsNewUpload.Tables[0].Rows.Count > 0)
                    {
                        AnimationExtender1.Enabled = true;
                        lblNewapps.Visible = true;
                        lnkFileUploaded.Visible = true;
                        lnkFileUploaded.Text = dsNewUpload.Tables[0].Rows.Count + " of your merchant(s) recently uploaded documents.";
                        lnkFileUploaded.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                        AnimationExtender1.Enabled = false;
                }
                else
                    AnimationExtender1.Enabled = false;
                #endregion

                Populate();

                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                    PopulateDeleteNews();//This function populates the delete news grid when the page is loaded

            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error populating page.");
            }
        }//end if page post back        
    }//end page load

    #region POPULATE PAGE
    //This function populates page
    public void Populate()
    {
        DisplayNews();//This function displays the news
        if (User.IsInRole("Admin"))
        {
            DataSet dsMon = (DataSet)Cache["MonList"];
            if (dsMon == null)
            {
                MonthBL mon = new MonthBL();
                dsMon = mon.GetMonthList();
                Cache.Insert("MonList", dsMon, null);
            }
            if (dsMon.Tables[0].Rows.Count > 0)
            {
                lstMonth.DataSource = dsMon;
                lstMonth.DataTextField = "Mon";
                lstMonth.DataValueField = "Mon";
                lstMonth.DataBind();
            }

            //Populate rep list in delete goals tab
            MonthBL RepListMonth = new MonthBL();
            DataSet dsRep = RepListMonth.GetRepListFromRepMonFundings(lstMonth.SelectedItem.Text.Trim());
            if (dsRep.Tables[0].Rows.Count > 0)
            {
                lstGoalsRepList.DataSource = dsRep;
                lstGoalsRepList.DataTextField = "RepName";
                lstGoalsRepList.DataValueField = "MasterNum";
                lstGoalsRepList.DataBind();
            }

            //Populate rep list in add goals tab
            /*DataSet dsRepList = (DataSet)Cache["RepList"];
            if (dsRepList == null)
            {
                ListBL RepList = new ListBL();
                dsRepList = RepList.GetSalesRepList();
                Cache.Insert("RepList", dsRepList, null);
            }*/

            ListBL RepList = new ListBL();
            DataSet dsRepList = RepList.GetSalesRepList();
            if (dsRepList.Tables[0].Rows.Count > 0)
            {
                lstRepList.DataSource = dsRepList;
                lstRepList.DataTextField = "RepName";
                lstRepList.DataValueField = "MasterNum";
                lstRepList.DataBind();
            }
            ListItem item = new ListItem();
            item.Text = "Other";
            item.Value = "Other";
            lstRepList.Items.Add(item);

            /*item = new ListItem();
			item.Text = "CTC";
            item.Value = "CTC";
            lstRepList.Items.Add(item);*/

        }//end if user admin

        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
        {
            pnlGoalsView.Visible = true;
            pnlPartnerGoals.Visible = false;
        }
        else if (User.IsInRole("Agent") || User.IsInRole("Reseller") || User.IsInRole("T1Agent"))
        {
            pnlGoalsView.Visible = false;
            pnlPartnerGoals.Visible = true;
        }
        else
        {
            pnlGoalsView.Visible = false;
            pnlPartnerGoals.Visible = false;
        }

        if (!User.IsInRole("Agent") && !User.IsInRole("Employee") && !User.IsInRole("Admin") && !User.IsInRole("T1Agent"))
            lnkbtnChangePackage.Visible = false;

        //Display Create online app and affiliate website link        
        lnkOnlineApp.NavigateUrl = "https://www.firstaffiliates.com/onlineapplication/start.aspx?Referral=" + Session["AffiliateID"].ToString();
        lnkOnlineApp.Target = "_Blank";

        lnkAffiliateWebsite.NavigateUrl = "https://www.firstaffiliates.com/Affiliatewiz/aw.aspx?A=" + Session["AffiliateID"].ToString();
        //lnkAffiliateWebsite.Text = "https://www.firstaffiliates.com/Affiliatewiz/aw.aspx?A=" + Session["AffiliateID"].ToString();
        lnkAffiliateWebsite.Target = "_Blank";

        lnkWebsiteHome.NavigateUrl = "https://www.firstaffiliates.com/Affiliatewiz/aw.asp?A=" + Session["AffiliateID"].ToString();
        //lnkWebsiteHome.Text = "https://www.firstaffiliates.com/Affiliatewiz/aw.asp?A=" + Session["AffiliateID"].ToString();
        lnkWebsiteHome.Target = "_Blank";

        lnkResellerWebsite.NavigateUrl = "https://www.firstaffiliates.com/Affiliatewiz/reseller.aspx?A=" + Session["AffiliateID"].ToString();
        lnkResellerWebsite.Target = "_Blank";

        lnkAgentWebsite.NavigateUrl = "https://www.firstaffiliates.com/Affiliatewiz/agent.aspx?A=" + Session["AffiliateID"].ToString();
        lnkAgentWebsite.Target = "_Blank";

        if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
            lnkAgentWebsite.Visible = false;
        if (User.IsInRole("Affiliate"))
            lnkResellerWebsite.Visible = false;        
        
        string Month = clndrDates.TodaysDate.Month.ToString();
        string Year = clndrDates.TodaysDate.Year.ToString();

        lblUserName.Text = Session["AffiliateName"].ToString().Trim();//Display Welcome affiliate name

        //Get the residual and commission dates
        MonthBL CommDates = new MonthBL();
        DataSet ds = CommDates.GetResdCommDates(Month, Year);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            DateTime ResdDate = Convert.ToDateTime(dr["ResidualDate"]);
            DateTime CommDate = Convert.ToDateTime(dr["CommissionDate"]);
            //Set selected dates in the calendar
            clndrDates.SelectedDate = ResdDate;
            clndrDates.SelectedDate = CommDate;

            SelectedDatesCollection theDates = clndrDates.SelectedDates;
            theDates.Clear();
            theDates.Add(ResdDate);
            theDates.Add(CommDate);
        }//end if count not 0

        //Get holidays
        MonthBL Holidays = new MonthBL();
        DataSet dsHolidays = Holidays.GetHolidays(Month, Year);
        if (dsHolidays.Tables[0].Rows.Count > 0)
        {
            DataRow dr = null;
            for (int i = 0; i < dsHolidays.Tables[0].Rows.Count; i++)
            {
                dr = dsHolidays.Tables[0].Rows[i];
                DateTime HolDate = Convert.ToDateTime(dr["HolidayDate"]);
                clndrDates.SelectedDate = HolDate;
                SelectedDatesCollection theDates = clndrDates.SelectedDates;
                theDates.Clear();
                theDates.Add(HolDate);
            }
        }//end if count not 0      
    }//end function populate
    #endregion

    #region CALENDAR
    protected void clndrDates_DayRender(object sender, DayRenderEventArgs e)
    {
        try
        {
            string Month = e.Day.Date.Month.ToString();
            string Year = e.Day.Date.Year.ToString();

            MonthBL CommDates = new MonthBL();
            DataSet ds = CommDates.GetResdCommDates(Month, Year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                DateTime ResdDate = Convert.ToDateTime(dr["ResidualDate"]);
                DateTime CommDate = Convert.ToDateTime(dr["CommissionDate"]);
                if (Convert.ToInt32(e.Day.DayNumberText) == ResdDate.Day)
                {
                    e.Cell.BackColor = System.Drawing.Color.Blue;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    e.Cell.ToolTip = "Residual & Commissions Posting Date";
                }

                if (Convert.ToInt32(e.Day.DayNumberText) == CommDate.Day)
                {
                    e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    e.Cell.ToolTip = "Commissions Posting Date";
                }
            }//end if count not 0

            //Get holidays
            MonthBL Holidays = new MonthBL();
            DataSet dsHolidays = Holidays.GetHolidays(Month, Year);
            if (dsHolidays.Tables[0].Rows.Count > 0)
            {
                DataRow dr = null;
                for (int i = 0; i < dsHolidays.Tables[0].Rows.Count; i++)
                {
                    dr = dsHolidays.Tables[0].Rows[i];
                    DateTime HolDate = Convert.ToDateTime(dr["HolidayDate"]);
                    if (Convert.ToInt32(e.Day.DayNumberText) == HolDate.Day)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Green;
                        e.Cell.ForeColor = System.Drawing.Color.White;
                        e.Cell.ToolTip = dr["HolidayText"].ToString().Trim();
                    }
                }
            }//end if count not 0
            if ((e.Day.DayNumberText == "1") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                PopulateGoals();
            if ((e.Day.DayNumberText == "1") && (User.IsInRole("Agent") || User.IsInRole("T1Agent") || User.IsInRole("Reseller")))
                PopulatePartnerGoals();

        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating dates");
        }
    }//end function day render

    //This function handles month changed event
    protected void clndrDates_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            string Month = e.NewDate.Month.ToString();
            string Year = e.NewDate.Year.ToString();

            if (e.NewDate.Day == 1)
            {
                //Get the residual and commission dates
                MonthBL CommDates = new MonthBL();
                DataSet ds = CommDates.GetResdCommDates(Month, Year);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    DateTime ResdDate = Convert.ToDateTime(dr["ResidualDate"]);
                    DateTime CommDate = Convert.ToDateTime(dr["CommissionDate"]);
                    clndrDates.SelectedDate = ResdDate;
                    clndrDates.SelectedDate = CommDate;

                    SelectedDatesCollection theDates = clndrDates.SelectedDates;
                    theDates.Clear();
                    theDates.Add(ResdDate);
                    theDates.Add(CommDate);
                }//end if count not 0

                //Get holidays
                MonthBL Holidays = new MonthBL();
                DataSet dsHolidays = Holidays.GetHolidays(Month, Year);
                if (dsHolidays.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = null;
                    for (int i = 0; i < dsHolidays.Tables[0].Rows.Count; i++)
                    {
                        dr = dsHolidays.Tables[0].Rows[i];
                        DateTime HolDate = Convert.ToDateTime(dr["HolidayDate"]);
                        clndrDates.SelectedDate = HolDate;
                        SelectedDatesCollection theDates = clndrDates.SelectedDates;
                        theDates.Clear();
                        theDates.Add(HolDate);
                    }
                }//end if count not 0
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating page");
        }
    }//end month changed click
    #endregion

    #region DEFAULT PACKAGES
    protected void btnApply_Click(object sender, EventArgs e)
    {
        DisplayNews();
        pnlChangePackage.Visible = false;
        lnkbtnChangePackage.Visible = true;
        try
        {
            if (lstPackages.SelectedItem.Text != "")
            {
                //Change the Default Rates Package
                AffiliatesBL Aff = new AffiliatesBL(Convert.ToInt16(Session["AffiliateID"]));
                bool retVal = Aff.ChangeDefaultPackage( Convert.ToInt32(lstPackages.SelectedValue), Convert.ToInt32(lstCPPackages.SelectedValue));
                if (!retVal)
                    DisplayMessage("Error changing Default Packages");
                else
                {
                    DisplayMessage("Default Rates Packages have been set successfully.");
                    //Add log
                    PartnerLogBL LogData = new PartnerLogBL();
                    if (CPPID != Convert.ToInt32(lstCPPackages.SelectedItem.Value))
                    {
                        retVal = LogData.InsertLogRates(Convert.ToInt32(lstCPPackages.SelectedItem.Value),
                            Convert.ToInt32(Session["AffiliateID"]), "Default CP Package changed to " + lstCPPackages.SelectedItem.Text + ".");
                    }
                    if (CNPPID != Convert.ToInt32(lstPackages.SelectedItem.Value))
                    {
                        retVal = LogData.InsertLogRates(Convert.ToInt32(lstPackages.SelectedItem.Value),
                            Convert.ToInt32(Session["AffiliateID"]), "Default CNP Package changed to " + lstPackages.SelectedItem.Text + ".");
                    }
                }
            }//end if package select not blank
            else
                DisplayMessage("Please enter a Package from the list before applying.");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error applying package.");
        }
    }//end button apply click

    protected void lnkbtnChangePackage_Click(object sender, EventArgs e)
    {
        try
        {
            pnlChangePackage.Visible = true;
            lnkbtnChangePackage.Visible = false;
            PopulatePackages();
            DisplayNews();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating Packages");
        }
    }//end link button change package click

    //This function Populates default Package list
    public void PopulatePackages()
    {
        //Populate CNP Package list
        RepBL Rep = new RepBL(Session["MasterNum"].ToString());
        DataSet dsPack = Rep.GetRepPackages("CNP");
        if (dsPack.Tables[0].Rows.Count > 0)
        {
            lstPackages.DataSource = dsPack;
            lstPackages.DataTextField = "PackageName";
            lstPackages.DataValueField = "PackageID";
            lstPackages.DataBind();
        }
        ListItem lstItem = new ListItem();
        lstItem.Text = "None";
        lstItem.Value = "0";
        lstPackages.Items.Add(lstItem);

        //Populate CP Package list
        dsPack = Rep.GetRepPackages("CP");
        if (dsPack.Tables[0].Rows.Count > 0)
        {
            lstCPPackages.DataSource = dsPack;
            lstCPPackages.DataTextField = "PackageName";
            lstCPPackages.DataValueField = "PackageID";
            lstCPPackages.DataBind();
        }
        lstItem = new ListItem();
        lstItem.Text = "None";
        lstItem.Value = "0";
        lstCPPackages.Items.Add(lstItem);

        DataSet dsDef = Rep.GetRepDefaultPackage();
        if (dsDef.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsDef.Tables[0].Rows[0];

            if (dr["PackageID"].ToString().Trim() != "")
                lstPackages.SelectedValue = lstPackages.Items.FindByValue(dr["PackageID"].ToString()).Value;
            else
                lstPackages.SelectedValue = lstPackages.Items.FindByValue("0").Value;

            if (dr["CPPackageID"].ToString().Trim() != "")
                lstPackages.SelectedValue = lstPackages.Items.FindByValue(dr["CPPackageID"].ToString()).Value;
            else
                lstPackages.SelectedValue = lstPackages.Items.FindByValue("0").Value;

        }
 
      

        CPPID = Convert.ToInt32(lstCPPackages.SelectedItem.Value);
        CNPPID = Convert.ToInt32(lstPackages.SelectedItem.Value);
    }//end function PopulatePackages

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            pnlChangePackage.Visible = false;
            lnkbtnChangePackage.Visible = true;
            lblError.Visible = false;
            DisplayNews();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating page.");
        }
    }//end cancel button click
    #endregion

    #region NEWS
    //This function displays news
    public void DisplayNews()
    {
        try
        {
            tblNews.Controls.Clear();
            //Parse the XML doc and display news text
            XmlDocument doc = new XmlDocument();
            if (Cache["NewsXML"] == null)
            {
                doc.Load(Server.MapPath("../NewsFeed.xml"));
                Cache.Insert("NewsXML", doc, new System.Web.Caching.CacheDependency(Server.MapPath("../NewsFeed.xml")));
            }
            else
                doc = (XmlDocument)Cache["NewsXML"];
            XmlNodeList nodeList = doc.GetElementsByTagName("News");
            if (nodeList.Count != 0)
            {
                TableRow tr;
                TableCell td;
                foreach (XmlNode childNode in nodeList)
                {
                    string strClass = "Labels";
                    if (childNode.Attributes.GetNamedItem("Imp").Value == "True")
                        strClass = "DivHelpLarge";
                    if (childNode.Attributes.GetNamedItem("Display").Value == "Employees" && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        td.CssClass = strClass;
                        td.Text = "<b><span class=\"LabelsBlue\">" +
                            Server.HtmlEncode(childNode.Attributes.GetNamedItem("Date").Value + " - ") + "</span></b>" +
                            Server.HtmlEncode(childNode.InnerText.ToString());
                        td.Attributes.Add("Align", "Left");
                        tr.Controls.Add(td);
                        tblNews.Controls.Add(tr);
                    }
                    if (childNode.Attributes.GetNamedItem("Display").Value == "Agents" && (User.IsInRole("Admin") || User.IsInRole("Employee") || User.IsInRole("Agent") || User.IsInRole("T1Agent")))
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        td.CssClass = strClass;
                        td.Text = "<b><span class=\"LabelsBlue\">" +
                            Server.HtmlEncode(childNode.Attributes.GetNamedItem("Date").Value + " - ") + "</span></b>" +
                            Server.HtmlEncode(childNode.InnerText.ToString());
                        td.Attributes.Add("Align", "Left");
                        tr.Controls.Add(td);
                        tblNews.Controls.Add(tr);
                    }
                    if (childNode.Attributes.GetNamedItem("Display").Value == "ALL")
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        
                        td.CssClass = strClass;
                        td.Text = "<b><span class=\"LabelsBlue\">" +
                            Server.HtmlEncode(childNode.Attributes.GetNamedItem("Date").Value + " - ") + "</span></b>" +
                            Server.HtmlEncode(childNode.InnerText.ToString());
                        td.Attributes.Add("Align", "Left");
                        tr.Controls.Add(td);
                        tblNews.Controls.Add(tr);
                    }
                }//end foreach
            }//end if count not 0
            else
                pnlNews.Visible = false;
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }//end function display news

    //This function populates the delete news grid when the page is loaded
    public void PopulateDeleteNews()
    {
        DataSet ds = new DataSet();
        ds.ReadXml(Server.MapPath("../NewsFeed.xml"));
        if (ds.Tables.Count > 0)
        {
            grdNews.Visible = true;
            grdNews.DataSource = ds;
            grdNews.DataBind();
        }
        else
            grdNews.Visible = false;
    }//end function Display Grid

    protected void btnAddNews_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                //Create XML file or append to existing XML file
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath("../NewsFeed.xml"));
                if (doc != null)
                {
                    XmlElement childNode = doc.CreateElement("News");

                    XmlAttribute attNode = doc.CreateAttribute("Date");
                    attNode.Value = DateTime.Now.ToString();
                    childNode.Attributes.Append(attNode);

                    attNode = doc.CreateAttribute("Imp");
                    attNode.Value = Convert.ToString(chkImp.Checked);
                    childNode.Attributes.Append(attNode);

                    attNode = doc.CreateAttribute("Display");
                    attNode.Value = Convert.ToString(lstDisplayNews.SelectedItem.Text);
                    childNode.Attributes.Append(attNode);

                    childNode.InnerText = txtNews.Text.Trim();
                    XmlNode node = doc.DocumentElement;
                    node.PrependChild(childNode);
                    doc.Save(Server.MapPath("../NewsFeed.xml"));
                }
                else
                {
                    DisplayMessage("News Feed Not Found");
                }
                DisplayNews();
                PopulateDeleteNews();
            }//end if user is admin or employee
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage(err.Message);
        }
    }//end add new feed button click

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            //Delete news text
            int index = e.RowIndex;
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("../NewsFeed.xml"));
            XmlNode xmlnode = doc.DocumentElement.ChildNodes.Item(index);
            xmlnode.ParentNode.RemoveChild(xmlnode);
            doc.Save(Server.MapPath("../NewsFeed.xml"));
            PopulateDeleteNews();
            DisplayNews();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Populating News");
        }
    }
    #endregion

    #region GOALS
    //This function creates a table dynamically and populates goals.
    protected void PopulateGoals()
    {
        //Populate goals only if used logged in is an admin or an employee
        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
        {
            try
            {
                DisplayNews();
                tblGoals.Rows.Clear();

                double ctcTotalGoals = 0;
                double ctcTotalTmp = 0;
                //Get Goals
                MonthBL Goals = new MonthBL();
                DataSet dsGoals = Goals.GetFundedGoals(clndrDates.SelectedDate.Month.ToString(), clndrDates.SelectedDate.Year.ToString());
                if (dsGoals.Tables[0].Rows.Count > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    string[] arrColumns = { "Rep Name", "Goal", "Actual Funding" };
                    tr.CssClass = "DivBlue";
                    for (int i = 0; i < arrColumns.Length; i++)
                    {

                        td = new TableCell();
                        td.Text = arrColumns[i].ToString();
                        td.CssClass = "MenuHeader";
                        tr.Cells.Add(td);
                    }
                    tblGoals.Rows.Add(tr);



                    for (int i = 0; i < dsGoals.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dsGoals.Tables[0].Rows[i];
                        ctcTotalGoals += Convert.ToDouble(dr[1]);
                        if (dr[0].ToString() == "Total CTC")
                        {
                            ctcTotalTmp = Convert.ToDouble(dr[1]);
                        }
                    }

                    ctcTotalGoals = ctcTotalGoals - ctcTotalTmp;

                    for (int i = 0; i < dsGoals.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dsGoals.Tables[0].Rows[i];
                        tr = new TableRow();

                        //Rep Name                        
                        td = new TableCell();
                        if (dr[0].ToString() == "Total CTC")
                            tr.Font.Bold = true;
                        td.Text = "<span class=\"LabelsSmall\">" + dr[0].ToString().Trim() + "</span>";
                        tr.Cells.Add(td);

                        //Goals
                        td = new TableCell();
                        if (dr[0].ToString() == "Total CTC")
                        {
                            td.Text = "<span class=\"LabelsSmall\">" + ctcTotalGoals.ToString().Trim() + "</span>";
                        }
                        else
                        {
                            td.Text = "<span class=\"LabelsSmall\">" + dr[1].ToString().Trim() + "</span>";
                        }
                        tr.Cells.Add(td);

                        //Actual Fundings
                        td = new TableCell();
                        td.Text = "<span class=\"LabelsSmall\">" + dr[2].ToString().Trim() + "</span>";
                        tr.Cells.Add(td);

                        if (i % 2 == 0)
                            tr.BackColor = System.Drawing.Color.FromArgb(247, 246, 243);
                        tblGoals.Rows.Add(tr);
                    }//end for

                }//end if count not 0
                else
                    lblNoGoals.Visible = true;
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error populating page.");
            }
        }//end if is in role
    }//end functions PopulateGoals    

    protected void PopulatePartnerGoals()
    {
        if (User.IsInRole("Agent") || User.IsInRole("Reseller") || User.IsInRole("T1Agent"))
        {
            try
            {
                DisplayNews();
                tblPartnerGoals.Rows.Clear();

                //Get Goals
                MonthBL Goals = new MonthBL();
                DataSet dsGoals = Goals.GetFundedPartnerGoals(Session["MasterNum"].ToString(), clndrDates.SelectedDate.Month.ToString(), clndrDates.SelectedDate.Year.ToString());
                if (dsGoals.Tables[0].Rows.Count > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    string[] arrColumns = { "Rep Name", "Goal", "Actual Funding" };
                    tr.CssClass = "DivBlue";
                    for (int i = 0; i < arrColumns.Length; i++)
                    {
                        td = new TableCell();
                        td.Text = arrColumns[i].ToString();
                        td.CssClass = "MenuHeader";
                        tr.Cells.Add(td);
                    }
                    tblPartnerGoals.Rows.Add(tr);

                    DataRow dr = dsGoals.Tables[0].Rows[0];
                    tr = new TableRow();

                    //Rep Name                        
                    td = new TableCell();
                    td.Text = "<span class=\"LabelsSmall\">" + dr[0].ToString().Trim() + "</span>";
                    tr.Cells.Add(td);

                    //Goals
                    td = new TableCell();
                    td.Text = "<span class=\"LabelsSmall\">" + dr[1].ToString().Trim() + "</span>";
                    tr.Cells.Add(td);

                    //Actual Fundings
                    td = new TableCell();
                    td.Text = "<span class=\"LabelsSmall\">" + dr[2].ToString().Trim() + "</span>";
                    tr.Cells.Add(td);

                    tblPartnerGoals.Rows.Add(tr);

                }//end if count not 0
                else
                {
                    lblNoPartnerGoals.Visible = true;
                }
            }//end try
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error populating page.");
            }
        }//end if is in role
        else
            tblPartnerGoals.Visible = false;
    }//end functions PopulatePartnerGoals  

    protected void btnAddRep_Click(object sender, EventArgs e)
    {
        if (User.IsInRole("Admin"))
        {
            try
            {
                MonthBL Goals = new MonthBL();
                int iRetVal = Goals.InsertUpdateRepFundings(lstRepList.SelectedItem.Value.Trim(), txtFundedGoals.Text.Trim(), clndrDates.SelectedDate.Month.ToString(), clndrDates.SelectedDate.Year.ToString());
            }
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error loading package information.");
            }
        }
    }

    protected void btnDeleteRep_Click(object sender, EventArgs e)
    {
        if (User.IsInRole("Admin"))
        {
            try
            {
                MonthBL Goals = new MonthBL();
                int iRetVal = Goals.DeleteRepFundings(lstGoalsRepList.SelectedItem.Value.Trim(), clndrDates.SelectedDate.Month.ToString(), clndrDates.SelectedDate.Year.ToString());
            }
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error loading package information.");
            }
        }
    }

    protected void lstMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Populate rep list in add goals tab
            MonthBL RepListMonth = new MonthBL();
            DataSet dsRep = RepListMonth.GetRepListFromRepMonFundings(lstMonth.SelectedItem.Text.Trim());
            if (dsRep.Tables[0].Rows.Count > 0)
            {
                lstGoalsRepList.DataSource = dsRep;
                lstGoalsRepList.DataTextField = "RepName";
                lstGoalsRepList.DataValueField = "MasterNum";
                lstGoalsRepList.DataBind();
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error loading package information.");
        }
    }
    #endregion

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lblTodayDate.Text = DateTime.Now.ToString();
        DisplayNews();
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void grdNews_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdNews.EditIndex = e.NewEditIndex;
        PopulateDeleteNews();
    }

    protected void grdNews_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdNews.EditIndex = -1;
        PopulateDeleteNews();
    }

    protected void grdNews_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int index = e.RowIndex;
            
            string NewsText = ((TextBox)grdNews.Rows[index].Cells[3].FindControl("EditText")).Text;
            string Display = ((DropDownList)grdNews.Rows[index].Cells[2].FindControl("EditDisplay")).SelectedItem.Text;
            string Imp = ((DropDownList)grdNews.Rows[index].Cells[1].FindControl("EditImp")).SelectedItem.Text;
            string strDate = ((TextBox)grdNews.Rows[index].Cells[0].FindControl("EditDate")).Text;

            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("../NewsFeed.xml"));
            XmlNode xmlnode = doc.DocumentElement.ChildNodes.Item(index);
            xmlnode.InnerText = NewsText;
            xmlnode.Attributes["Imp"].Value = Imp;
            xmlnode.Attributes["Display"].Value = Display;
            xmlnode.Attributes["Date"].Value = strDate;
            doc.Save(Server.MapPath("../NewsFeed.xml"));
            grdNews.EditIndex = -1;
            PopulateDeleteNews();
        }//end try
        catch (Exception err)
        {
            DisplayMessage(err.StackTrace);
        }
    }
}
