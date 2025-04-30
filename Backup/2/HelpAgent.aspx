<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HelpAgent.aspx.cs" Inherits="HelpAgent" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Help</title>
    <link href="partnercss.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <center>
    <form id="form1" runat="server">    
    <asp:Panel ID="pnlHelp" runat="server" Width="700px" CssClass="SilverBorder">
        <a name="#00"></a>
            <div style="width: 90%">
                <asp:Panel ID="pnlAdminQs" runat="server" Width="100%" Visible="false">
                    <a name="#07"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>Admins</b></span></div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#07-1" target="_self">How can I update Agent Rep Numbers in the Portal and ACT!?</a>
                    </div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#07-2" target="_self">How can I modify User privileges or assign rights or block users from the Partner Portal?</a>
                    </div>
                    <div class="DivGreen" align="center"><span class="LabelsRed"><b>Goals</b></span></div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#07-3" target="_self">How can I Add Goals?</a>
                    </div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#07-4" target="_self">How can I Update Goals?</a>
                    </div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#07-5" target="_self">How can I Delete Goals?</a>
                    </div>
                    <div class="DivGreen" align="center"><span class="LabelsRed"><b>Online Applications</b></span></div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#07-6" target="_self">How can I update the Login Name for an Online Application?</a>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlEmployeeQs" runat="server" Width="100%" Visible="false">
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#08-1" target="_self">How do I upload a Gateway?</a>
                    </div>
                    <a name="#08"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>ACT! Features</b></span></div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#08-2" target="_self">How can I create a PDF from ACT!?</a>
                    </div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#08-3" target="_self">How can I export an existing record to the Partner Portal from ACT!?</a>
                    </div>
                    <a name="#02"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>Home</b></span></div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#02-1" target="_self">How can I Add News/Events?</a>
                    </div>
                    <div align="left">
                        &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                        &nbsp;<a class="One" href="#02-2" target="_self">How can I Delete News/Events?</a>
                    </div>
                </asp:Panel>
                <a name="#01"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>Online Applications</b></span></div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-1" target="_self">How can I view/modify online application information?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-2" target="_self">What website address (URL) do I send the customer to complete the application?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-3" target="_self">How can I send reminders to customers?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-4" target="_self">How can I set rates for an online application?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-5" target="_self">How can I add/delete Sales Opportunities for an application?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-6" target="_self">How can I create PDF for an online application?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-7" target="_self">How can I add notes to an online application?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-8" target="_self">Where can I view the status for the online application and additional services?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-9" target="_self">How can I create Online Applications?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">                
                    &nbsp;<a class="One" href="#01-10" target="_self">How can I modify a Rates Package?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-11" target="_self">How can I create a Rates Package?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#01-12" target="_self">How can I manage my leads?</a>
                </div>
                <a name="#05"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>Reports</b></span></div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-1" target="_self">How can I view my Commissions Report?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-2" target="_self">When are commissions uploaded to the Portal?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-3" target="_self">How do I know whether a payment for my Commissions was done?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-4" target="_self">How can I view my Residuals Report?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-5" target="_self">When are Residuals uploaded to the Portal?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-6" target="_self">How do I know whether a payment for my Residuals was done?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-7" target="_self">How long does it take for an ACH deposit to show in my account?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-8" target="_self">How long does it take for a check to be mailed?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-9" target="_self">Who should I contact if I have questions about payments, commissions or residuals?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-10" target="_self">Why can't I view previous month's residuals?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-11" target="_self">How do I sign up for Direct Deposit?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#05-12" target="_self">How long does it take after commissions/residuals posting date for direct deposit to show up in the bank account?</a>
                </div>
                <a name="#09"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>Edit Profile</b></span></div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#09-1" target="_self">How do I change my password to the Partner Portal?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#09-2" target="_self">How do I modify my banking information?</a>
                </div>
                <a name="#03"></a><div class="DivHelp" align="center"><span class="LabelsRed"><b>Miscellaneous</b></span></div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#03-1" target="_self">How can I see the cost of products and services offered?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#03-2" target="_self">Where can I get Application forms, Rate charts, Information brochures and other forms and documents?</a>
                </div>
                <div align="left">
                    &nbsp;<img src="images/obullet.gif" height="8" width="8" border="0">
                    &nbsp;<a class="One" href="#03-3" target="_self">What are the Account Funding Criteria?</a>
                </div>
            </div>
            <br />
            
            <table cellpadding="0" cellspacing="0" border="0" style="width: 90%">
            <tr>
                <td align="left">
                    <hr noshade size="1" width="90%">
                    <!--*************MANAGE ONLINE APPLICATIONS*************-->
                    <a name="01-1"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I view/modify online application information?</b></span><br />                    
                    <span class="LabelsSmall">Click on "Online App" in the menu. Then click on the App ID on the </span>
                    <span class="LabelsRedSmall"><b>LEFT</b></span> 
                    <span class="LabelsSmall">
                    to automatically log in to the Online Application. You can go to the online application 
                    directly to modify or view detailed application information.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-2"></a>
                    <span class="LabelsDarkBlueSmall"><b>What website address (URL) do I send the customer to complete the application?</b></span><br />                        
                    <span class="LabelsSmall">Click on "Online App" in the menu. The App ID on the </span>
                    <span class="LabelsRedSmall"><b>RIGHT</b></span>
                    <span class="LabelsSmall">represents
                        the customer's login URL. The customer can login using the email address and password
                        he/she signed up with.</span>
                    <br />                        
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-3"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I send reminders to customers?</b></span><br />                        
                    <span class="LabelsSmall">Go to the Online Applications Section and click on the Contact
                        Name in the list of online applications to open the Send reminder window. You can customize the reminder message
                        to your liking. </span>
                    <br />                        
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-4"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I set rates for an online application?</b></span><br />                        
                    <span class="LabelsSmall">Go to "Online Apps" from the menu. Click on the "Edit" link 
                    in the online applications list to view additional options for the application. 
                    Click on the "Rates" tab to view the current rates for the application on the Edit page. Click on the 
                    "Click here to Modify Rates" link to set/modify rates.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-5"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I add/delete Sales Opportunities for an application?</b></span><br />
                    <span class="LabelsSmall">Go to "Online Apps" from the menu. Click on the "Edit" link to view additional options for the application. 
                    Click on the "Sales Opps" tab to view the current Sales Opps for the application. Click on 
                    Add Sales Opps to Add a new Sales Opportunity to the application. You can also delete a sales opportunity 
                    by clicking on the Delete link in the opportunities list.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-6"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I create PDF for an online application?</b></span><br />                        
                    <span class="LabelsSmall">Go to "Online Apps" from the menu. Click on the "Edit" link to view additional options for the application. 
                    Click on the "Create PDF" button to create the PDF.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-7"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I add notes to an online application?</b></span><br />                                                
                    <span class="LabelsSmall">Go to "Online Apps" from the menu. Click on the "Edit" link to view additional options for the application. 
                    Click on the "Notes" tab to view notes for the application. To add a note, type the note in the 
                    text box at the bottom and click on "Add Note". If this is an urgent note, you can check the 
                    "Notify Administrator" box.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-8"></a>
                    <span class="LabelsDarkBlueSmall"><b>Where can I view the status for the online application and additional services?</b></span><br />                        
                    <span class="LabelsSmall">Go to "Online Apps" from the Partner Portal menu. You can view the status of the application and additional services in the 
                    Manage Online Applications section. Application Status and Additional Services status is updated multiple 
                    times daily. </span>
                    <br />                        
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>                        
                    <hr noshade size="1" width="90%">
                    
                    <a name="01-9"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I create Online Applications?</b></span><br />                        
                    <span class="LabelsSmall">Go to "Online Apps" from the Partner Portal menu. </span>
                    <ul class="LabelsSmall">
                    <li>
                        To create online applications, click on the Create Online Applications Tab and then click on the link. 
                        If you have the customer information, you can start the online application by following the instructions. If you want to send out this unfinished 
                        application to the customer, you can go to "Online Apps" from the main menu in the Partner Portal, then right 
                        click on the AppId on the RIGHT in the Online Applications list for the application you just created and click on 
                        "Copy Shortcut" from the menu and send this URL to the customer. 
                    </li>
                    <li>
                        If you do not have any customer information and you want the customer to start the online application, 
                        you can right click on the "Create Online Applications" link, click on "Copy Shortcut" and paste this short cut 
                        in the email you want to send out to the customer.
                    </li>
                    </ul>
                    <br />                        
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                                        
                    <!--*************MODIFY RATES PACKAGE*************-->                    
                    <a name="01-10"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I modify Rates Package?</b></span><br />
                    <span class="LabelsSmall">Go to "Online Apps" from the Partner Portal menu. You can modify rates for all of the rates packages you have 
                    set up in the Partner Portal by clicking on Modify Rates Package in the menu.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                
                    <!--*************CREATE RATES PACKAGE*************-->
                    
                    <a name="01-11"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I create Rates Package?</b></span><br />
                    <span class="LabelsSmall">Go to "Online Apps" from the Partner Portal menu. 
                    You can create new rates packages by clicking on the Create Rates Package in the menu.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                
                    <!--*************LEADS*************-->             
                    <a name="01-12"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I manage my leads?</b></span><br />
                    <span class="LabelsSmall">You can view the list of all leads you have received from 
                    www.firstaffiliates.com clicking on Online Application and then View Leads. There are two options 
                    in the "Select Report Type" list on the leads page. The "Leads" option will display all the Free Report, 
                    Free Consult and Free Apply leads from www.firstaffiliates.com. The "Affiliate Signups" option displays 
                    all the affiliate/agent/reseller signups from you affiliate website.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <!--*************Commissions*************-->
                    <a name="05-1"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I view my Commissions Report?</b></span><br />
                    <span class="LabelsSmall">Click on "Reports" in the menu. Then click on "Commissions". 
                    Select the month for which you want to view the Commissions report and click on Submit.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-2"></a>
                    <span class="LabelsDarkBlueSmall"><b>When are Commissions uploaded to the Portal?</b></span><br />
                    <span class="LabelsSmall">You can check the Business Calendar on the Home page for 
                    Commissions and Residuals Upload date. You can change the month in the calendar by clicking 
                    on ">" and "<" buttons and view important dates for the selected month.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-3"></a>
                    <span class="LabelsDarkBlueSmall"><b>How do I know whether a payment for my Commissions was done?</b></span><br />
                    <span class="LabelsSmall">Click on "Reports" in the menu. Then click on "Commissions". 
                    Select the month for which you want to view the Commissions report and click on Submit.
                    If a Payment was done, you will see a Confirmation Number similar to the example below: </span>
                    <asp:Panel ID="pnlConfirmation" runat="server" BackColor="Ivory" BorderColor="Orange"
                        BorderStyle="Solid" BorderWidth="1px" Height="25px" Width="300px">
                        <asp:Label ID="lblConfirmationExample" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="7pt">Commission paid on Mar 1 2007 with Confirmation Code: 111111</asp:Label></asp:Panel>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <!--*************Residuals*************-->
                    <a name="05-4"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I view my Residuals Report?</b></span><br />
                    <span class="LabelsSmall">Click on "Reports" in the menu. Then click on "Residuals". 
                    Select the month for which you want to view the Residuals report and click on Submit.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-5"></a>
                    <span class="LabelsDarkBlueSmall"><b>When are Residuals uploaded to the Portal?</b></span><br />
                    <span class="LabelsSmall">You can check the Business Calendar on the Home page for 
                    Commissions and Residuals Upload date. You can change the month in the calendar by clicking 
                    on ">" and "<" buttons and view important dates for the selected month.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-6"></a>
                    <span class="LabelsDarkBlueSmall"><b>How do I know whether a payment for my Residuals was done?</b></span><br />
                    <span class="LabelsSmall">Click on "Reports" in the menu. Then click on "Residuals". 
                    Select the month for which you want to view the Residuals report and click on Submit.
                    If a Payment was done, you will see a Confirmation Number similar to the example below: </span>
                    <asp:Panel ID="pnlResdExample" runat="server" BackColor="Ivory" BorderColor="Orange"
                        BorderStyle="Solid" BorderWidth="1px" Height="25px" Width="300px">
                        <asp:Label ID="lblResdExample" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="7pt">Residual paid on Mar 23 2007 with Confirmation Code: 111111</asp:Label></asp:Panel>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-7"></a>
                    <span class="LabelsDarkBlueSmall"><b>How long does it take for an ACH deposit to show in my account?</b></span><br />
                    <span class="LabelsSmall">It takes approximately 3 business days from the calendar posting date for an ACH deposit to show. 
                    To verify if a commission or residual was paid, click <a class="One" href="#05-3">here</a></span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-8"></a>
                    <span class="LabelsDarkBlueSmall"><b>How long does it take for a check to be mailed?</b></span><br />
                    <span class="LabelsSmall">It takes approximately 5 business days from the calendar posting date until
                    receipt of the check. To verify if a commission or residual was paid, click <a class="One" href="#05-3">here</a></span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-9"></a>
                    <span class="LabelsDarkBlueSmall"><b>Who should I contact if I have questions about payments, commissions or residuals?</b></span><br />
                    <span class="LabelsSmall">You can contact the Accounting/HR Manager whose contact information can be found in the Contact Us section regarding 
                    any questions on payments, residuals or commissions. You can also refer to the Contact Us page for more information.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-10"></a>
                    <span class="LabelsDarkBlueSmall"><b>Why can't I view previous month's residuals?</b></span><br />
                    <span class="LabelsSmall">We have reports that come from over 20 different vendors, some of which we 
                    don't receive for over a month after the end of the billing month. Thus, we report all residuals 3 business 
                    days after the 15th or next previous business day of the 2nd month following the billing month. You can see 
                    the schedule on the Business Calendar on the Home Page when you login.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-11"></a>
                    <span class="LabelsDarkBlueSmall"><b>How do I sign up for Direct Deposit?</b></span><br />
                    <span class="LabelsSmall">On the Home Page, click on "Edit Profile" located towards the top of the page next to 
                    your name. In the Edit Profile page, click on "Yes" for "Do you want to sign up for direct deposit?". 
                    Then click on the "Edit Banking Information" link to enter your banking information. Please download, complete  
                    and fax the <a class="LinkXSmall" href="http://firstaffiliates.com/PartnerPortal/Direct%20Deposit%20Authorization%20Form.pdf">
                    Authorization Agreement</a> to (310) 321-5410.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="05-12"></a>
                    <span class="LabelsDarkBlueSmall"><b>How long does it take after commissions/residuals posting date for direct deposit to show up in the bank account?</b></span><br />
                    <span class="LabelsSmall">It generally takes 3 business days after the posting date to the portal. For example, 
                    if pay day (posting date) is June 5th then payment will show up in the bank account by June 8th.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <!--*************EDIT PROFILE*************-->
                    <a name="09-1"></a>
                    <span class="LabelsDarkBlueSmall"><b>How do I change my password to the Partner Portal?</b></span><br />
                    <span class="LabelsSmall">On the Home Page, click on "Edit Profile" located towards the top of the page next to 
                    your name. In the Edit Profile page, click on "Click here to change password". Enter your new password and 
                    click on the submit button to save the changes.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="09-2"></a>
                    <span class="LabelsDarkBlueSmall"><b>How do I modify my banking information?</b></span><br />
                    <span class="LabelsSmall">On the Home Page, click on "Edit Profile" located towards the top of 
                    the page next to your name. In the Edit Profile page, click on "Edit Bank Account Information". 
                    Enter your banking information and click on submit to save changes.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <!--*************MISCELLANEOUS*************-->
                    <a name="03-1"></a>
                    <span class="LabelsDarkBlueSmall"><b>How can I see the cost of products and services offered?</b></span><br />
                    <span class="LabelsSmall">On the Home Page, click on "Miscellaneous" from the menu on the top and then click on 
                    "Item List" from the menu below the main menu. You can view the products and services offered and update 
                    the certain information for each product.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="03-2"></a>
                    <span class="LabelsDarkBlueSmall"><b>Where can I get Application forms, Rate charts, Information brochures and other forms and documents?</b></span><br />
                    <span class="LabelsSmall">On the Home Page, click on "Miscellaneous" from the menu on the top and then click on 
                    "Documents And Logins" from the menu below the main menu. All forms and documents are categorized and you can 
                    download them by clicking on the form name.</span>
                    <br />
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <a name="03-3"></a>
                    <span class="LabelsDarkBlueSmall"><b>What are the Account Funding Criteria?</b></span><br />
                    <span class="LabelsSmall">Account Funding Criteria:</span>
                    <ul class="LabelsSmall">
                    <li>Account must be approved by underwriting.</li>
                    <li>All payments due from merchant for equipment, set up costs, lease funding, etc. must be received.</li>
                    <li>If there is a gateway or any other service which requires activation, merchant must log in and activate gateway and/or any other service.</li>
                    <li>Referral/affiliate source must be provided if different from agent.</li>
                    <li>Merchant must have received all equipment associated with the particular sale and all services must be tested and active on that equipment.</li>
                    <li>Merchant must be happy and not need or expect anything else.</li>	
                    </ul>
                    <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                    <hr noshade size="1" width="90%">
                    
                    <asp:Panel ID="pnlAdminAns" runat="server" Width="100%" Visible="false">
                        <!--*************ADMINS*************-->             
                        <a name="07-1"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I update Agent Rep Numbers in the Portal and ACT!?</b></span><br />
                        <span class="LabelsSmall">Go to Admin -> Manage Partners and then click on the Agent name 
                        you want to update. Enter the information and click on update.</span>
                        <span class="LabelsRedSmall">Note: You have to restart ACT! for the changes to take effect.</span>
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="07-2"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I modify User privileges or assign rights or block users from the Partner Portal?</b></span><br />
                        <span class="LabelsSmall">Go to Admin -> Manage Users and then click on "Edit" for the record you want to 
                        to modify. The list on the left shows the different applications within the Partner Portal. You can select 
                        a role from the list of available roles for each user and then click on "Update Roles" to save the changes.</span>                        
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <!--*********GOALS**********-->
                        <a name="07-3"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I Add Goals?</b></span><br />
                        <span class="LabelsSmall">
                            On the Home page, click on Add/Update/Delete goals. Before you add goals, make sure the month for which you 
                            want to add goals is selected in the Calendar. Then you can select the Rep from the list and enter the 
                            expected goals for the Rep and click "Add/Update".
                        </span>                        
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="07-4"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I Update Goals?</b></span><br />
                        <span class="LabelsSmall">
                            On the Home page, click on Add/Update/Delete goals. Before you update goals, make sure the month for which you 
                            want to update goals is selected in the Calendar. Then you can select the Rep from the list and update 
                            the expected goals in the textbox and click "Add/Update".
                        </span>                        
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="07-5"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I Delete Goals?</b></span><br />
                        <span class="LabelsSmall">
                            On the Home page, click on Add/Update/Delete goals. Click on the "Delete Goals" tab. Then 
                            select the month for which you want to delete goals. The Rep List below the month list will be 
                            populated automatically with the Reps which have goals for the selected month. Select the Rep you 
                            want to delete from the list and click on "Delete".
                        </span>                        
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="07-6"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I update the Login Name for an Online Application?</b></span><br />
                        <span class="LabelsSmall">
                            On the Edit page, click on Modify Information. If you want to modify the login name, click on "Edit" next to 
                            the login name and type in the new login name. If you do not want to update the login name, please click on 
                            "Cancel" next to the login name before you click on submit.
                        </span>                        
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                    </asp:Panel>
                    <asp:Panel ID="pnlEmployeeAns" runat="server" Width="100%" Visible="false">
                    <a name="08-1"></a>
                        <span class="LabelsDarkBlueSmall"><b>How do I upload a Gateway?</b></span><br />
                        <span class="LabelsSmall">Go to Admin -> Upload Gateway. Look up the record in ACT! and click on 
                        "Upload Gateway". If any information is missing or incorrect, a message will be displayed on the top of the page. 
                        Please make any changes in ACT! if necessary and then click on the "OK" button to general a text file. 
                        The file will be automatically saved in the customer folder. You can then log in to the Authorize.net Reseller 
                        login and upload the gateway.</span>
                        <br />
                        <span class="LabelsRedSmall">Note: Make sure that the record is saved in ACT! whenever any changes are made 
                        before you generate the Gateway file.</span>
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="08-2"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I create a PDF from ACT!?</b></span><br />
                        <span class="LabelsSmall">On the Home page, go to ACT! Features -> Create PDF. Look up the record 
                        in ACT! and click on "Create PDF" for the record. A PDF will be automatically generated based on the 
                        processor selected in ACT! and the PDF file will be saved in the customer folder automatically.</span>
                        <br />
                        <span class="LabelsRedSmall">Note: Make sure that the record is saved in ACT! whenever any changes are made 
                        before you generate the PDF file.</span>
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="08-3"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I export an existing record to the Partner Portal from ACT!?</b></span><br />
                        <span class="LabelsSmall">On the Home page, click on ACT! Features -> Create Online App. Look up the record 
                        in ACT! and click on "EXPORT" for the record. Follow the instructions in red after the record is exported.
                        Go to the Edit page for the record after it is exported and then click on Modify Information and type in a 
                        login name and click on submit.</span>
                        <br />
                        <span class="LabelsRedSmall">Note: Make sure that the record is saved in ACT! whenever any changes are made 
                        before you export it.</span>
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <!--*********HOME PAGE**********-->
                        
                        <a name="02-1"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I Add News/Events?</b></span><br />
                        <span class="LabelsSmall">On the Home page, click on "Add News/Events". In the text box that appears, type in the 
                        news text you want to display and click on "Add".</span>
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                        
                        <a name="02-2"></a>
                        <span class="LabelsDarkBlueSmall"><b>How can I Delete News/Events?</b></span><br />
                        <span class="LabelsSmall">On the Home page, click on "Delete News/Events". In the list that appears, 
                        click on "Delete" next to the news you want to delete.</span>
                        <br />
                        <span class="LabelsRedSmall">Warning: Once a news/event is deleted, it cannot be recovered.</span>
                        <br />
                        <a class="LinkXSmall" href="#00" target="_self">Back to the top</a>
                        <hr noshade size="1" width="90%">
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>    
    </form>
    </center>
</body>
</html>
