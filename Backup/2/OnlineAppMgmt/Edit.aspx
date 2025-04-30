<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Edit"
    Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<script language="javascript" type="text/javascript">
//<![CDATA[
var cot_loc0=(window.location.protocol == "https:")? "https://secure.comodo.net/trustlogo/javascript/cot.js" :
"http://www.trustlogo.com/trustlogo/javascript/cot.js";
document.writeln('<scr' + 'ipt language="JavaScript" src="'+cot_loc0+'" type="text\/javascript">' + '<\/scr' + 'ipt>');
//]]>
</script>

<head id="Head1" runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
    <link href="~/PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>

<a href="http://www.instantssl.com" id="comodoTL">SSL</a>
<script language="JavaScript" type="text/javascript">
COT("https://www.firstaffiliates.com/images/secure_site.gif", "SC2", "none");
</script>
<body>
    <center>        
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div ID="pnlDiv" align="left"></div>
            <table style="width: 940px" class="SilverBorder" align="center">
                <tr>
                    <td align="center" style="background-image: url(../Images/topMain.gif); height: 25px">
                        <span class="MenuHeader"><b>Edit Information</b></span>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="2" style="width: 100%;" border="0">
                            <tr>
                                <td align="left" colspan="1" valign="top" style="width: 18%" class="DivGray">
                                    <asp:Panel ID="pnlACT" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanelACT" runat="server">
                                            <ContentTemplate>
                                                <cc1:PopupControlExtender ID="PopupControlExtender2" runat="server" PopupControlID="pnlActUpdateNote"
                                                    TargetControlID="imgActExc" Position="Bottom" />
                                                <div style="width: 100%;" align="center" >
                                                    <div align="center">
                                                        <span class="LabelsSmall"><b>ACT! Features</b></span>
                                                        <asp:Image ID="imgActExc" runat="server" ImageUrl="~/Images/help.gif"
                                                            Style="cursor: pointer" ToolTip="Help" />
                                                    </div>
                                                    <asp:ImageButton ID="imgAddToACT" runat="server" ImageUrl="~/Images/AddToACT.gif"
                                                        OnClick="imgAddToACT_Click" />                                                                                            

                                                    <br />
                                                    <asp:ImageButton ID="imgUpdateInACT" runat="server" ImageUrl="~/Images/UpdateInACT.gif"
                                                        OnClick="imgUpdateInACT_Click" />
                                                </div>
                                                <asp:Panel ID="pnlActUpdateNote" runat="server" Style="display: none; z-index: 1;"
                                                    Width="250px" CssClass="DivHelp">
                                                    <ul class="LabelsSmall">
                                                        <li>"ADD TO ACT" adds everything to ACT including rates and sales opps. </li>
                                                        <li>"UPDATE" updates everything EXCEPT  Sales Opps. To add Sales Opps, go to the Sales Opps section
                                                            and click on "Add To ACT" next to the Sales Opp. </li>
                                                        <!--<li>"UPDATE RATES" updates Rates only. Click on "UPDATE RATES" whenever rates are changed
                                                            in the Partner Portal. </li>To Update Rates, click
                                                            on the "Update Rates" button below. -->
                                                        <li>
                                                            <span class="LabelsRedSmall">NOTE: Before you update record in ACT!, please check the
                                                            History for the record to see the changes made to the app.</span></li>
                                                    </ul>                                                    
                                                </asp:Panel>
                                                
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <br />
                                    </asp:Panel>
                                    
                                    
                                    <asp:Panel ID="pndAddlServicesPDF" runat="server" >
                                        <div style="width: 100%;" align="center">
                                      
                                            <div align="center">
                                                <b><span class="LabelsSmall">
                                                    <asp:Label ID="lblMCAPDF" runat="server" Font-Bold="True" Text=""></asp:Label>  <asp:Image ID="HelpMCAPDF" runat="server" ImageUrl="~/Images/help.gif"
                                                    Style="cursor: pointer" /><br/>
                                                    <asp:ImageButton CssClass="MenuLink" ID="ImageMCAPDF" runat="server" CausesValidation="false" ImageUrl="~/Images/CreateIMSPDF.gif"
                                                OnClientClick="form1.target ='_blank';" OnClick="imgMCAPDF_Click" />
                                                    

                                                    </span></b>
                                                <cc1:PopupControlExtender ID="PopupControlExtenderMCA" runat="server" PopupControlID="pnlPDFNote"
                                                    TargetControlID="HelpMCAPDF" Position="Bottom" />
                                                
                                            </div>
                                            
                                            <div align="center">
                                            <b><span class="LabelsSmall">
                                            <asp:Label ID="lblLeasePDF" runat="server" Font-Bold="True" Text=""></asp:Label><asp:Image ID="HelpLeasePDF" runat="server" ImageUrl="~/Images/help.gif"
                                                    Style="cursor: pointer" /><br/>
                                                    <asp:ImageButton CssClass="MenuLink" ID="ImageLeasePDF" runat="server" CausesValidation="false" ImageUrl="~/Images/CreateIMSPDF.gif"
                                                OnClientClick="form1.target ='_blank';" OnClick="imgLeasePDF_Click" />
                                                </span></b>
                                           <cc1:PopupControlExtender ID="PopupControlExtenderLease" runat="server" PopupControlID="pnlPDFNote"
                                                    TargetControlID="HelpLeasePDF" Position="Bottom" />
                                            </div>
                                            
                                            <div align="center">
                                            <b><span class="LabelsSmall">
                                            <asp:Label ID="lblGiftcardPDF" runat="server" Font-Bold="True" Text=""></asp:Label><asp:Image ID="HelpGiftPDF" runat="server" ImageUrl="~/Images/help.gif"
                                                    Style="cursor: pointer" /><br/>
                                                    <asp:ImageButton CssClass="MenuLink" ID="ImageGiftCardPDF" runat="server" CausesValidation="false" ImageUrl="~/Images/CreateIMSPDF.gif"
                                                OnClientClick="form1.target ='_blank';" OnClick="imgGiftCardPDF_Click" />
                                                </span></b>
                                            <cc1:PopupControlExtender ID="PopupControlExtenderGift" runat="server" PopupControlID="pnlPDFNote"
                                                    TargetControlID="HelpGiftPDF" Position="Bottom" />
                                            </div>
                                            
                                        </div>
                                    </asp:Panel>
                                    
                                    <br />
                                    <asp:Panel ID="pnlDeleteApp" Width="100%" runat="server">
                                        <center>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Images/Delete.gif" OnClick="imgDelete_Click" /><br/>
                                                    <asp:Panel ID="pnlDeleteConfirm" runat="server" BackColor="#FFC0C0" BorderColor="Salmon"
                                                        BorderStyle="Double" Visible="False">
                                                        <asp:Image ID="imgExclamation" runat="server" ImageUrl="~/Images/exclamation.gif" />
                                                        <asp:Label ID="lblDeleteMsg" runat="server" Font-Bold="True" Font-Size="Medium" Text="">Confirm Delete?</asp:Label><br />
                                                        <asp:Button ID="btnDeleteYes" runat="server" OnClick="btnDeleteYes_Click" Text="Yes" />
                                                        <asp:Button ID="btnDeleteNo" runat="server" OnClick="btnDeleteNo_Click" Text="No" /></asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="imgDelete" EventName="Click" />
                                                    <asp:PostBackTrigger ControlID="btnDeleteYes" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </center>
                                    </asp:Panel>
                                    <br/>

                                    <br/>
                                </td>
                                <td align="center" colspan="2" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanelDisplayInfo" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlDisplayGeneralInfo" runat="server" Width="100%">
                                                <table cellpadding="0" cellspacing="5" style="width: 60%;" border="0">
                                                    <tr>
                                                        <td align="right" style="width: 40%">
                                                            <span class="LabelsSmall">AppId</span></td>
                                                        <td align="left">
                                                            <asp:Label ID="lblAppId" runat="server" Font-Bold="True"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="LabelsSmall">First Name</span></td>
                                                        <td align="left">
                                                            <b><asp:Label ID="lblFirstNameValue" runat="server" Font-Bold="True"></asp:Label></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="LabelsSmall">Last Name</span></td>
                                                        <td align="left">
                                                            <b><asp:Label ID="lblLastNameValue" runat="server" Font-Bold="True"></asp:Label></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="LabelsSmall">Title</span></td>
                                                        <td align="left">
                                                            <b><asp:Label ID="lblTitleValue" runat="server" Font-Bold="True"></asp:Label></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="LabelsSmall">Business Phone</span></td>
                                                        <td align="left">
                                                            <b><asp:Label ID="lblPhoneValue" runat="server" Font-Bold="True"></asp:Label></b><asp:Label ID="lblExt" runat="server" Text=" Ext. "></asp:Label><b><asp:Label ID="lblPhoneExtValue" runat="server" Font-Bold="True"></asp:Label></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="LabelsSmall">Home Phone</span></td>
                                                        <td align="left">
                                                            <b><asp:Label ID="lblHomePhoneValue" runat="server" Font-Bold="True"></asp:Label></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="LabelsSmall">Mobile Phone</span></td>
                                                        <td align="left">
                                                            <b><asp:Label ID="lblMobilePhoneValue" runat="server" Font-Bold="True"></asp:Label></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2" style="height: 30px">
                                                            <asp:LinkButton ID="lnkbtnModify" Font-Bold="true" Font-Names="Arial" Font-Size="9"
                                                                runat="server" Text="Modify Profile" OnClick="lnkbtnModify_Click" CssClass="One" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlModifyGeneralInfo" runat="server" Width="100%" Visible="false">
                                                <asp:Panel ID="pnlMainPage" runat="server">
                                                    <asp:ValidationSummary ID="ValidateSummary" runat="server" BackColor="#FFC0C0" BorderColor="red"
                                                        BorderWidth="1px" ForeColor="Black" HeaderText="Please check the fields marked in red."
                                                        Width="250px" />
                                                    <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                                        <tr>
                                                            <td></td>
                                                            <td align="left" colspan="2">
                                                                <b><span class="LabelsRedLarge">*</span><span class="LabelsRed"> - denotes a required field</span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="width: 32%">
                                                                <span class="LabelsSmall">Login Name</span></td>
                                                            <td align="left" colspan="2">
                                                                <asp:UpdatePanel ID="UpdatePanelLoginName" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtLoginName" runat="server" MaxLength="32" Width="140px" Enabled="False" TabIndex="1"></asp:TextBox>
                                                                        <asp:LinkButton ID="lnkbtnUpdateLoginName" runat="server" Font-Names="Arial" Font-Size="8pt"
                                                                            OnClick="lnkbtnUpdateLoginName_Click">Edit</asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="width: 32%">
                                                                <span class="LabelsSmall">First Name</span>
                                                            </td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtFirstName" runat="server" Width="140px" MaxLength="32" TabIndex="2"></asp:TextBox>
                                                                <span class="LabelsRedLarge">*</span>
                                                                <asp:RequiredFieldValidator ID="ValidateFirstName" runat="server" Display="Static"
                                                                    ControlToValidate="txtFirstName" ErrorMessage="First Name" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Last Name</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtLastName" runat="server" Width="140px" MaxLength="32" TabIndex="3"></asp:TextBox>
                                                                <span class="LabelsRedLarge">*</span>
                                                                <asp:RequiredFieldValidator ID="ValidateLastName" runat="server" ControlToValidate="txtLastName"
                                                                    ErrorMessage="Last Name" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Email</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="32" Width="140px" TabIndex="15"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Title</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" Width="140px" TabIndex="5"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Business Phone</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtPhone" runat="server" Width="140px" MaxLength="16" TabIndex="6"></asp:TextBox>
                                                                <span class="LabelsSmall">Ext.</span>
                                                                <asp:TextBox ID="txtPhoneExt" runat="server" Width="40px" MaxLength="4" TabIndex="7"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="REExt" runat="server" ControlToValidate="txtPhoneExt"
                                                                    ErrorMessage="Numbers Only" ValidationExpression="[0-9]*$" EnableClientScript="False"></asp:RegularExpressionValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Home Phone</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtHomePhone" runat="server" MaxLength="50" Width="140px" TabIndex="8"></asp:TextBox>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Mobile Phone</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="txtMobilePhone" runat="server" Width="140px" MaxLength="16" TabIndex="9"></asp:TextBox>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Sales Rep</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:DropDownList ID="lstSalesRep" runat="server" TabIndex="10">
                                                                </asp:DropDownList>
                                                                <span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                                    runat="server" ControlToValidate="lstSalesRep" ErrorMessage="Sales Rep"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="middle">
                                                                <span class="LabelsSmall">Referred By<br />(DBA - Company Name - PartnerID)</span></td>
                                                            <td colspan="2" align="left">
                                                                <asp:DropDownList ID="lstReferredBy" runat="server" TabIndex="11">
                                                                </asp:DropDownList>
                                                                <span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                                    runat="server" ControlToValidate="lstReferredBy" ErrorMessage="Referred By"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Other Referral</span></td>
                                                            <td align="left" colspan="2"><asp:DropDownList ID="lstOtherReferral" runat="server" TabIndex="12">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td colspan="3" align="center">
                                                                <asp:Button ID="btnContinue" runat="server" Text="Submit" OnClick="btnContinue_Click"
                                                                    TabIndex="16" />
                                                                &nbsp;&nbsp;
                                                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                                                    TabIndex="17" CausesValidation="False" UseSubmitBehavior="False" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel ID="pnlLoginAttempts" runat="server" Width="100%" CssClass="DivHelp">
                                        NOTE: This application has been locked because of too many login attempts. To unlock,
                                        click
                                        <asp:LinkButton ID="lnkbtnUnlock" runat="server" Font-Bold="true" Font-Names="Arial"
                                            Font-Size="Small" OnClick="lnkbtnUnlock_Click">here</asp:LinkButton></asp:Panel>
                                                             
                                            <asp:Panel ID="pnlChasePDF" runat="server" Visible="False" Width="300px" CssClass="DivHelp">
                                                <strong><span class="Labels">Choose the Chase PDF you want to create:</span></strong><br />
                                                <!--<asp:LinkButton ID="btnChaseAbout" runat="server" CssClass="One" OnClick="btnChaseAbout_Click" CausesValidation="false">About Merchant</asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="btnChaseFee" runat="server" CssClass="One" OnClick="btnChaseFee_Click" CausesValidation="false">Fee Schedule</asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="btnChaseMP" runat="server" CssClass="One" OnClick="btnChaseMP_Click" CausesValidation="false">Merchant Processing</asp:LinkButton>
                                                <br />
                                                <asp:LinkButton ID="btnCreditAdd" runat="server" CssClass="One" OnClick="btnChaseCreditAdd_Click" CausesValidation="false">Credit Addendum</asp:LinkButton>
                                                <br />
                                                <asp:HyperLink CssClass="One" ID="lnkOpGuide" runat="server" NavigateUrl="~/PDF/Chase Operating Guide.pdf"
                                                    Target="_blank">Chase Operating Guide</asp:HyperLink>-->
                                                <asp:LinkButton ID="btnChaseMPA" runat="server" CssClass="One" Font-Names="Arial"
                                                    Font-Size="10pt" OnClick="btnChaseMPA_Click" CausesValidation="False">Chase Merchant Application and Agreement</asp:LinkButton><br />    
                                                <asp:LinkButton ID="btnChaseFS3Tier" runat="server" CssClass="One" Font-Names="Arial"
                                                    Font-Size="10pt" OnClick="btnChaseFS3Tier_Click" Visible="false" CausesValidation="False">Chase Fee Schedule 3 tier</asp:LinkButton><br />    
                                                <asp:LinkButton ID="btnChaseFSInterchangePlus" runat="server" CssClass="One" Font-Names="Arial"
                                                    Font-Size="10pt" OnClick="btnChaseFSInterchangePlus_Click" Visible="false" CausesValidation="False">Chase Fee Schedule Interchange Plus</asp:LinkButton><br />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlSagePDF" runat="server" Visible="False" Width="300px" CssClass="DivHelp">
                                                <strong><span class="Labels">Choose the Sage PDF you want to create:</span></strong><br />
                                                        <asp:LinkButton ID="btnSageApp" runat="server" CssClass="One" OnClick="btnSageApp_Click" CausesValidation="false">Sage Application</asp:LinkButton><br />
                                                        <!--<asp:LinkButton ID="btnSageMOTO" runat="server" OnClick="btnSageMOTO_Click" CssClass="One">Sage MOTO Application</asp:LinkButton><br />-->
                                                        <asp:HyperLink ID="lnkSageAgreement" CssClass="One" runat="server" NavigateUrl="~/PDF/Sage Merchant Agreement.pdf" Target="_blank">Sage Agreement</asp:HyperLink>
                                            </asp:Panel>
                             
                                    
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
                                                Visible="False"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlConfirm" runat="server" BackColor="#FFC0C0" BorderColor="Salmon"
                                    BorderStyle="Double" Visible="False" Width="200px">
                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label><br />
                                    <asp:Label ID="lblMessage" runat="server" Text="Do you want to create a new record?"></asp:Label><br />
                                    <asp:Button ID="btnCreateRecordYes" runat="server" OnClick="btnCreateRecordYes_Click"
                                        Text="Yes" />
                                    <asp:Button ID="btnCreateRecordNo" runat="server" OnClick="btnCreateRecordNo_Click"
                                        Text="No" /></asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgAddToACT" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:Panel ID="pnlPDFNote" runat="server" Style="display: none; z-index: 1;" Width="250px"
                            CssClass="DivHelp">
                            NOTE: PDF Creation works only with the Microsoft Internet Explorer (Version 7.0 
                            or above), Mozilla Firefox (Version 2.0 or above) and Safari (Version 4.0 or above). For viewing PDFs, please
                            install Abode Reader Version 8.0 or higher. Download the latest version here: 
                            <asp:HyperLink ID="lnkAdobe" CssClass="One" runat="server" NavigateUrl="http://www.adobe.com"
                                Target="_blank">www.adobe.com</asp:HyperLink></asp:Panel>
                        <cc1:TabContainer runat="server" ID="Tabs">
                            <cc1:TabPanel ID="TabNotes" runat="server" HeaderText="Notes">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlNotes" runat="server" Width="100%" BackColor="#ffffff" Visible="True">
                                                <div style="width: 100%;" align="center">
                                                    <asp:GridView ID="grdNotes" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                        ForeColor="#333333" GridLines="Vertical" OnRowDeleting="grdNotes_RowDeleting"
                                                        OnRowDataBound="grdNotes_RowDataBound">
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <Columns>
                                                            <asp:BoundField DataField="username" HeaderText="Modified By"></asp:BoundField>
                                                            <asp:BoundField DataField="DateRecorded" HeaderText="Date Recorded" />
                                                            <asp:BoundField DataField="NoteText" HeaderText="Note Text"></asp:BoundField>
                                                            <asp:BoundField DataField="NoteID" HeaderText="Note ID"></asp:BoundField>
                                                            <asp:CommandField CausesValidation="False" ShowDeleteButton="True" />
                                                        </Columns>
                                                        <RowStyle BackColor="#bce2d3" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                                                        <EditRowStyle BackColor="#999999" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="MenuHeader" BackColor="#5D7B9D" />
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    </asp:GridView>
                                                    <br />
                                                    <asp:Label ID="lblNotesHeader" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Text="Add Notes"></asp:Label><br />
                                                    <asp:TextBox ID="txtNotes" runat="server" TabIndex="35" TextMode="MultiLine"></asp:TextBox>
                                                    <div align="center">
                                                        <asp:CheckBox ID="chkNotify" runat="server" Font-Bold="True" TabIndex="36" Text="Notify New Account/Sales Support" />&nbsp;<br/>
                                                        <asp:Button ID="btnAddNote" runat="server" OnClick="btnAddNote_Click" TabIndex="37"
                                                            Text="Add Note" />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabHistory" runat="server" HeaderText="History">
                                <ContentTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanelHistory" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="Vertical">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="Contact" HeaderText="Modified By" SortExpression="Contact"/>
                                                    <asp:BoundField DataField="Action" HeaderText="Action" SortExpression="Action" />
                                                    <asp:BoundField DataField="RecordedDate" HeaderText="Date Recorded" SortExpression="DateRecorded">
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle BackColor="Honeydew" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                                                <EditRowStyle BackColor="#999999" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="MenuHeader" BackColor="#5D7B9D" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>                                
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabSalesOpps" runat="server" HeaderText="Opportunities">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lnkAddSalesOpps" runat="server" Font-Bold="True" Font-Names="Arial"
                                                Font-Size="9" OnClick="lnkAddSalesOpps_Click" Font-Underline="false">Click here to Add a Sales Opportunity</asp:LinkButton>
                                            <asp:Panel ID="pnlAddOpp" runat="server" Width="100%" Visible="False">
                                                <asp:Label ID="lblAddSalesOpp" runat="server" Font-Bold="True" Text="Add Sales Opportunity"
                                                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                                                <table class="BlueBorder" border="0" style="width:100%">
                                                    <tr>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Product Name</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Sell Price</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">COG</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Qty</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Sub Total</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Sales Rep</span></b></td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Payment Method</span></b></td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Reprogram</span></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:DropDownList ID="lstProductName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstProductName_SelectedIndexChanged">
                                                            </asp:DropDownList><span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                                                    runat="server" ControlToValidate="lstProductName" ErrorMessage="Productname"></asp:RequiredFieldValidator></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtAddSellPrice" runat="server" MaxLength="7" Width="50"></asp:TextBox></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtAddCOG" runat="server" Width="50" Enabled="false"></asp:TextBox></td>
                                                        <td valign="top">
                                                            <asp:DropDownList ID="lstAddQuantity" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstAddQuantity_SelectedIndexChanged">
                                                                <asp:ListItem>1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtAddSubtotal" runat="server" MaxLength=6 Width=50 Enabled=false></asp:TextBox></td>
                                                        <td valign="top">
                                                            <asp:DropDownList ID="lstRepNameAdd" runat="server">
                                                            </asp:DropDownList></td>
                                                        <td valign="top">
                                                            <asp:DropDownList ID="lstPayment" runat="server">
                                                                <asp:ListItem Selected=True>Invoice Merchant</asp:ListItem>
                                                                <asp:ListItem>ACH Merchant</asp:ListItem>    
                                                                <asp:ListItem>Lease Merchant</asp:ListItem>                                                              
                                                                </asp:DropDownList></td>
                                                        <td valign="top">
                                                            <asp:DropDownList ID="lstAddReprogram" runat="server" AutoPostBack="True"/></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" align="center">
                                                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" />
                                                            &nbsp;
                                                            <asp:Button ID="btnCancelAdd" runat="server" CausesValidation="False" OnClick="btnCancelAdd_Click"
                                                                Text="Cancel" UseSubmitBehavior="False" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlEditSalesOpp" runat="server" Width="100%" Visible="False">
                                                <asp:Label ID="lblEditSalesOpp" runat="server" Font-Bold="True" Text="Edit Sales Opportunity"
                                                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                                                <table class="BlueBorder" border="0" style="width:100%">
                                                    <tr>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">ID</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Product Name</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Sell Price</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">COG</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Qty</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Sub Total</span></b>
                                                        </td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Sales Rep</span></b></td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Payment Method</span></b></td>
                                                        <td class="DivBlue">
                                                            <b><span class="MenuHeader">Reprogram</span></b></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" /></td>
                                                        <td>
                                                            <asp:DropDownList ID="listEditProductName" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" OnSelectedIndexChanged="lst_ProductNameChanged"/></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSellPrice" runat="server" MaxLength="7" Width="50"/></td>
                                                        <td>
                                                            <asp:Label ID="lblCOG" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" /></td>
                                                        <td>
                                                            <asp:DropDownList ID="lstQuantity" runat="server" AutoPostBack="True">
                                                                <asp:ListItem>1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                        <td>
                                                            <asp:Label ID="lblSubtotal" runat="server" Width="50" Font-Bold="True" Font-Names="Arial" Font-Size="Small" /></td>
                                                        <td>
                                                            <asp:DropDownList ID="lstRepName" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" /></td>
                                                        <td>
                                                            <asp:DropDownList ID="lstEditPaymentMethod" runat="server">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem>Invoice Merchant</asp:ListItem>
                                                                <asp:ListItem>ACH Merchant</asp:ListItem> 
                                                                <asp:ListItem>Lease Merchant</asp:ListItem>                                                                 
                                                            </asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="lstReprogram" runat="server" AutoPostBack="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="9" align="center">
                                                            <asp:Button ID="btnEditSubmit" runat="server" OnClick="btnEditSubmit_Click" Text="Submit" />
                                                            &nbsp;
                                                            <asp:Button ID="btnEditCancel" runat="server" CausesValidation="False" OnClick="btnEditCancel_Click"
                                                                Text="Cancel" UseSubmitBehavior="False" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                            <asp:GridView ID="grdSalesOpps" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdSalesOpps_RowCommand"
                                                OnRowDataBound="grdSalesOpps_RowDataBound" OnRowDeleting="grdSalesOpps_RowDeleting">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="Product" HeaderText="Product Name"></asp:BoundField>
                                                    <asp:BoundField DataField="Price" HeaderText="Price"></asp:BoundField>
                                                    <asp:BoundField DataField="CostOfGoods" HeaderText="Cost Of Goods" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity"></asp:BoundField>
                                                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal"></asp:BoundField>
                                                    <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                                                    <asp:BoundField DataField="Stage" HeaderText="Stage" />
                                                    <asp:BoundField DataField="RepName" HeaderText="RepName" />
                                                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" />
                                                    <asp:BoundField DataField="Reprogram" HeaderText="Reprogram" />
                                                    <asp:BoundField DataField="Linked" HeaderText="Linked" />
                                                    <asp:BoundField DataField="IsAddedAct" HeaderText="Added/ Updated to Act" />
                                                    <asp:CommandField CausesValidation="False" ShowDeleteButton="True" />
                                                    <asp:ButtonField CommandName="RowEdit" Text="Edit">
                                                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="X-Small" />
                                                    </asp:ButtonField> 
                                                    <asp:ButtonField CommandName="AddToACT" Text="Add/Update to ACT!">
                                                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="X-Small" />
                                                    </asp:ButtonField>   
                                                </Columns>
                                                <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                                                <EditRowStyle BackColor="#999999" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="MenuHeader" BackColor="#5D7B9D" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>                                
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabRates" runat="server" HeaderText="Services">
                                <ContentTemplate>
                                    <div align="center">
                                        <asp:HyperLink ID="lnkModifyRates" CssClass="One" Font-Bold="true" Font-Names="Arial"
                                            Font-Size="9" runat="server">Click here to Modify</asp:HyperLink>
                                    </div>
                                    <table width="60%" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Panel ID="pnlMerchant" runat="server" Width="100%">
                                                    <table border="0" cellpadding="0" cellspacing="5" style="width: 100%;" class="DivGreen">
                                                        <tr>
                                                            <td style="width: 35%; height: 15px" align="left">
                                                                <asp:Label Font-Bold="true" Font-Size="9" ID="lblMerchantAcc" runat="server" Text="Merchant Account"></asp:Label>
                                                            </td>
                                                            <td style="width: 35%; height: 15px" align="left">
                                    
                                                            </td>
                                                            <td align="right" valgin="middle" style="height: 15px">
                                                            <asp:Panel ID="pnlPDF" runat="server" >
                                    <ContentTemplate>
                                        <div style="width: 100%; align:right; vertical-align:middle;">
                                            <div style="float:left; ">
                                                <b><span class="LabelsSmall">
                                                    <asp:Label ID="lblProcessorPDF" runat="server" Font-Bold="True" Text=""></asp:Label></span></b>
                                                
                                            </div>
                                            <div style="float:right;">
                                            <asp:ImageButton CssClass="MenuLink" ID="imgCreatePDF" runat="server" CausesValidation="false" ImageUrl="~/Images/CreateIMSPDF.gif"
                                                OnClientClick="form1.target ='_blank';" OnClick="imgCreatePDF_Click" /><cc1:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="pnlPDFNote"
                                                    TargetControlID="imgPDFHelp" Position="Bottom" />
                                                <asp:Image ID="imgPDFHelp" runat="server" ImageUrl="~/Images/help.gif"
                                                    Style="cursor: pointer" /><br/>
                                                 <asp:HyperLink ID="lnkSageAgreement1" Visible="False" Font-Names="Arial" CssClass="Link" runat="server" NavigateUrl="~/PDF/Sage Merchant Agreement.pdf" Target="_blank">Sage Agreement</asp:HyperLink>
                                            <br/>



                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    </asp:Panel>
                                    
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 35%; height: 15px" align="left"></asp:Label>
                                                            </td>
                                                            <td style="width: 35%; height: 15px" align="left">
                                   <asp:Panel ID="pnlEnvStatus" Width="100%" runat="server">
                                    <center>
                                    <asp:Label Font-Bold="true" Font-Size="9" ID="lblEnvStatus" runat="server" ></asp:Label>
                                        </center>
                                      
                                    </asp:Panel>
                                                            </td>
                                                            <td align="right" valgin="middle" style="height: 15px">
                                                                <b><span class="LabelsSmall">
                                                                <asp:Panel ID="pnlDelDocuSignEnv" Width="100%" runat="server">
                                           <center>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lnkDelDocuSignEnv" Font-Bold="true" Font-Names="Arial" Font-Size="9"
                                                                runat="server" Text="Delete Envelope" OnClick="lnkDelDocuSignEnv_Click" CssClass="One" /><br/>
                                                    <asp:Panel ID="pnlDelDocuEnvConfirm" runat="server" BackColor="#FFC0C0" BorderColor="Salmon"
                                                        BorderStyle="Double" Visible="False">
                                                        <asp:Image ID="imgExclamationDelDocEnv" runat="server" ImageUrl="~/Images/exclamation.gif" />
                                                        <asp:Label ID="lblDeleteMsgDocEnv" runat="server" Font-Bold="True" Font-Size="Medium" Text="">Confirm Delete?</asp:Label><br />
                                                        <asp:Button ID="btnDeleteDocEnvYes" runat="server" OnClick="btnDelDocuSignEnvYes_Click" Text="Yes" />
                                                        <asp:Button ID="btnDeleteDocEnvNo" runat="server" OnClick="btnDelDocuSignEnvNo_Click" Text="No" /></asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lnkDelDocuSignEnv" EventName="Click" />
                                                    <asp:PostBackTrigger ControlID="btnDeleteDocEnvYes" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </center>
                                      
                                    </asp:Panel></span></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblAppFee" runat="server" Text="Application Fee"></asp:Label></td>
                                                            <td align="left">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblApplicationFee" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblAppSetupFee" runat="server" Text="Setup Fee"></asp:Label></td>
                                                            <td align="left">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblApplicationSetupFee" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblDiscRate" runat="server" Text="Visa/MC Discount Rate"></asp:Label></td>
                                                            <td align="left">
                                                                <b>
                                                                    <asp:Label ID="lblDiscountRate" runat="server"></asp:Label><span class="LabelsSmall"> %</span></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="labelDebitRate" Text="Visa/MC Debit Rate" runat="server"></asp:Label></td>
                                                            <td align="left">
                                                                <b>
                                                                    <asp:Label ID="lblDebitRate" runat="server"></asp:Label><span class="LabelsSmall"> %</span>
                                                                    </b></td>
                                                        </tr>  
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblPerAuthorization" runat="server" Text="Per Authorization - All Card Types"></asp:Label></td>
                                                            <td align="left">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblPerAuth" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblMonthlyMin" runat="server" Text="Monthly Minimum"></asp:Label></td>
                                                            <td align="left">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblMonMin" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblTollFree" runat="server" Text="Customer Service Fee"></asp:Label></td>
                                                            <td align="left">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblTollFreeService" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlMerchantInfo" runat="server" Width="100%">
                                                    <table cellpadding="0" cellspacing="5" border="0" style="width: 100%;" class="DivGreen">
                                                        <tr>
                                                            <td align="left" style="width: 25%">
                                                                <span class="LabelsSmall">Processor</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblProcessorText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" style="width: 25%">
                                                                <span class="LabelsSmall">Discover</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblDiscoverText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 25%">
                                                                <span class="LabelsSmall">Visa/Mastercard</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblVisaMasterNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" style="width: 25%">
                                                                <span class="LabelsSmall">Amex</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblAmexText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Merchant Status</span></td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="lstMerchantStatus" runat="server" TabIndex="3" Width="170">
                                                                </asp:DropDownList></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">JCB</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblJCBText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>

                                                <!--<asp:Panel ID="pnlReprogram" runat="server" Width="100%" Visible="false">
                                                    <table cellpadding="0" cellspacing="5" border="0" style="width: 100%;" class="DivGreen">
                                                        <tr>
                                                            <td align="left" style="width: 25%">
                                                                <span class="LabelsSmall">Platform</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblRPlatformACT" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" style="width: 25%">
                                                                <span class="LabelsSmall">Login ID</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblRLoginIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Merchant ID</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRMerchantIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Terminal ID</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRTerminalIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Bank ID Number (BIN)</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRBINNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Agent Chain Number</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRAgentChainNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Agent Bank Number</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRAgentBankNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Store Number</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRStoreNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">MCC Category Code</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRMCCCodeText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>-->
                                                <asp:Panel ID="pnlPlatform" runat="server"  Visible="False" Width="100%">
                                                    <table cellpadding="0" cellspacing="5" border="0" style="width: 100%;" class="DivGreen">
                                                        <tr>
                                                            <td align="left" style="width: 25%">
                                                                <span class="LabelsSmall">Platform</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:DropDownList ID="lstPlatform" runat="server" TabIndex="4" Width="170px"></asp:DropDownList>
                                                                </td>
                                                            <td align="right" style="width: 25%">
                                                                <span class="LabelsSmall">Login ID</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblLoginIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Merchant ID</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblMerchantIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Terminal ID</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblTerminalIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Bank ID Number (BIN)</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblBINNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Agent Chain Number</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblAgentChainNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Agent Bank Number</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblAgentBankNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Store Number</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblStoreNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">MCC Category Code</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblMCCCodeText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlDBEBT" runat="server" Width="100%">
                                                    <asp:Table ID="tblDBEBT" runat="server" Width="100%" style="width: 100%;" class="DivGreen">
                                                    </asp:Table>
                                                </asp:Panel>
                                            </td>
                                        </tr>                                      
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Panel ID="pnlGateway" runat="server" Height="100%" Width="100%">
                                                    <table border="0" style="width: 100%;" class="DivGreen">
                                                        <tr>
                                                            <td style="height: 18px; width: 70%;" align="left">
                                                                <asp:Label Font-Bold="true" ID="lblGatewayHeader" Font-Size="9" runat="server" Text="Payment Gateway"></asp:Label></td>
                                                            <td align="left" style="height: 18px">
                                                                <b>
                                                                    <asp:Label ID="lblGateway" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblSetupFee" runat="server" Text="Setup Fee"></asp:Label></td>
                                                            <td align="left" style="height: 15px;">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblGatewaySetupFee" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblGatewayAccess" runat="server" Text="Monthly Gateway Access"></asp:Label></td>
                                                            <td align="left" style="height: 15px;">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblMonthlyGatewayAccess" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 15px; width: 70%;" align="left">
                                                                <asp:Label ID="lblTransFee" runat="server" Text="Gateway Transaction Fee"></asp:Label></td>
                                                            <td align="left">
                                                                <b><span class="LabelsSmall">$ </span><asp:Label ID="lblGatewayTransFee" runat="server"></asp:Label></b></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlGatewayInfo" runat="server" Visible="False" Width="100%">
                                                    <table cellpadding="0" cellspacing="5" border="0" style="width: 100%;" class="DivGreen">
                                                        <!--<tr>
                                                            <td align="right" style="width: 25%">
                                                                <span class="LabelsSmall">Gateway</span></td>
                                                            <td align="left" style="width: 25%">
                                                                <asp:Label ID="lblGatewayText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" style="width: 25%">
                                                            </td>
                                                            <td align="left" style="width: 25%">
                                                            </td>
                                                        </tr>-->
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Login/User ID</span></td>
                                                            <td align="left">
                                                                <asp:Label ID="lblLoginUserIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right">
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="LabelsSmall">Gateway Status</span></td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="lstGatewayStatus" runat="server" TabIndex="10" Width="170">
                                                                </asp:DropDownList></td>
                                                            <td align="left">
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Panel ID="pnlAdditionalServices" runat="server" Width="100%">
                                                    <asp:Table ID="tblAddlServices" runat="server" Width="100%" style="width: 100%;" class="DivGreen">
                                                    </asp:Table>
                                                </asp:Panel>

                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                        <td align="center">
                                        <div style="width: 100%;" align="center">
                                                <!--<asp:Label ID="lblContactHeader" Font-Bold="true" runat="server" Text="Contact:"></asp:Label>
                                                &nbsp;<asp:Label ID="lblContact" Font-Bold="true" runat="server"></asp:Label><br />-->
                                                
                                                <asp:Panel ID="pnlReprogramQuestion" runat="server" Width="100%" Visible="false">
                                                    <asp:Label ID="lblReprogramHeader" runat="server" Font-Size="9" Text=" Re-Program Existing Merchant Account"></asp:Label>&nbsp;
                                                    <asp:RadioButton ID="rdbYes" runat="server" GroupName="rdbReprogram" Text="Yes" OnCheckedChanged="rdbYes_CheckedChanged"
                                                        AutoPostBack="True" TabIndex="21" Enabled="false" />
                                                    <asp:RadioButton ID="rdbNo" runat="server" GroupName="rdbReprogram" Text="No" AutoPostBack="True"
                                                        OnCheckedChanged="rdbYes_CheckedChanged" Enabled="false" /><br />
                                                    <asp:Label ID="lblWarning" runat="server" Text='("Yes" if the customer already has a merchant account. "No" if no previous merchant account)'></asp:Label>
                                                    <br />
                                                </asp:Panel>
                                                
                                                <br />
                                                <br />
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                                    TabIndex="32" />
                                            </div>
                                                      </td>
                                                      </tr>
                                         <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSubmit" />
                                        </Triggers>
                                    </table>
                                </ContentTemplate>                                
                            </cc1:TabPanel>
                            
                            
                        </cc1:TabContainer>
                        <div align="center" style="height: 30px; background-image: url(../Images/topMain.gif)">
                            <asp:Button ID="btnCreateIMSApp" runat="server" Text="Create IMS App"
                                Visible="False" />
                            <input type="button" value="Close" style="height: 25px; width: 50px;font-size:8pt; font-family:Arial" onclick="javascript:window.close();">
                        </div>
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>