<%@ Page Language="C#" MasterPageFile="~/OnlineAppMgmt/Admin.master" AutoEventWireup="true"
    CodeFile="default.aspx.cs" Inherits="OnlineAppAdmin" Title="E-Commerce Exchange - Partner Portal"
    Theme="Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout ="3600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanelError" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td align="center" style="width:30%" valign="top">
                <div align="center" style="width:90%" class="DivHelp">
                <span class="LabelsRedSmall">* - denotes QuickBooks Signup</span><br />
                <span class="LabelsRedSmall">** - denotes WorldPay Signup</span>
                </div>
            </td>
            <td align="center" style="width:40%">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 350px;" class="LightBlueBG; SilverBorder">
                    <tr>
                        <td colspan="3" align="center" style="background-image: url(../Images/homeback.gif)">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <cc1:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_CSS">
                                        <cc1:TabPanel ID="TabSearch" runat="server" HeaderText="Search">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                    <tr>
                                                        <td align="right" style="width: 50%">
                                                            <span class="LabelsSmall"><b>Select Rep Name</b></span>&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="lstRepName" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="width: 50%">
                                                            <span class="LabelsSmall"><b>Select Status</b></span>&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="lstStatus" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:CheckBox ID="chkDisplayUnsynched" runat="server" AutoPostBack="True" OnCheckedChanged="chkDisplayUnsynched_CheckedChanged"
                                                                Text="Display Only Unsynched Apps" Font-Names="Arial" Font-Size="8pt" ForeColor="#383838" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabAdvanced" runat="server" HeaderText="Advanced">
                                            <ContentTemplate>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate">
                                                </cc1:CalendarExtender>
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate">
                                                </cc1:CalendarExtender>
                                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                    <tr>
                                                        <td align="right" style="width: 30%">
                                                            <span class="LabelsSmall"><b>Filter Date From</b></span>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="50" Width="80px"></asp:TextBox></td>
                                                        <td align="left">
                                                            <span class="LabelsSmall"><b>To</b></span>
                                                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="50" Width="80px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="width: 30%">
                                                            <span class="LabelsSmall"><b>Sort By</b></span>&nbsp;</td>
                                                        <td align="left" colspan="2">
                                                            <asp:DropDownList ID="lstSortBy" runat="server">
                                                                <asp:ListItem Value="AppID">App ID</asp:ListItem>
                                                                <asp:ListItem Value="FirstName">First Name</asp:ListItem>
                                                                <asp:ListItem Value="LastName">Last Name</asp:ListItem>
                                                                <asp:ListItem>DBA</asp:ListItem>
                                                                <asp:ListItem Value="ReferralName">Referral Source</asp:ListItem>
                                                                <asp:ListItem Value="AcctTypeDesc">Account Type</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabLookup" runat="server" HeaderText="Lookup">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td align="right" style="width: 30%">
                                                            <span class="LabelsSmall"><b>Look up By</b></span> &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%">
                                                            <asp:DropDownList ID="lstLookup" runat="server">
                                                                <asp:ListItem>Email</asp:ListItem>
                                                                <asp:ListItem>FirstName</asp:ListItem>
                                                                <asp:ListItem>LastName</asp:ListItem>
                                                                <asp:ListItem>DBA</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" style="width: 45%">
                                                            <asp:TextBox ID="txtLookup" runat="server" MaxLength="50" Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="3">
                                                            <asp:Button ID="btnLookup" runat="server" OnClick="btnLookup_Click" Text="Lookup" /></td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabUpdate" runat="server" HeaderText="Update Status">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                    <tr>
                                                        <td colspan="3" align="center">
                                                            <span class="LabelsSmall"><b>Update Status to copy Status Information and
                                                                Sales Opps from ACT for all Online Apps</b></span><br />
                                                            <asp:Button ID="btnUpdateStatus" runat="server" Text="Update Status" OnClick="btnUpdateStatus_Click"
                                                                ToolTip="Updates Status information and Sales Opps information" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />                            
                        </td>
                    </tr>
                </table>
            </td>
            <td align="center" style="width:30%" valign="top">
                <asp:UpdatePanel ID="UpdatePanelDate" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlUpdateStatusDate" runat="server" Width="90%" CssClass="DivHelp" Visible="false">
                        <asp:Label ID="lblUpdateStatusDate" runat="server" Font-Bold="true" CssClass="Labels"></asp:Label>
                        </asp:Panel>                            
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <asp:Image ID="imgProgress" runat="server" ImageUrl="~/Images/indicator.gif" /><span class="LabelsRed"><b>Retrieving Data...Please Wait</b></span>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlUpdateStatus" runat="server" BackColor="Ivory" BorderColor="silver"
                BorderWidth="1px" Visible="False" Width="700px">                
                <span class="Labels">Updated Status Information for the list below except those indicated below:</span>
                <br />
                <div align="left">
                <span style="font-family:Arial; font-size:small; color:DarkGreen">+ Merchant Status or Gateway Status in Act is not beyond COMPLETED. Record will NOT be updated.
                </span>
                <br />
                <span style="font-family:Arial; font-size:small; color:Red">? Record does not exist in ACT.
                </span>
                <br />
                <!--<span style="font-family:Arial; font-size:small; color:DarkBlue"># Online App Status is locked. Updating to DECLINED status will unlock the application. Please correct status in ACT!.
                </span>
                <br />-->
                <span style="font-family:Arial; font-size:small; color:Orange">$ No changes made to ACT! record since Status was updated last updated.
                </span>
                </div>
                <br />
                <span class="Labels">Please click on the Submit button again to see the changed status</span>                
            </asp:Panel>
            <br />
            <asp:Label ID="lblNumberOfRecords" runat="server" Font-Bold="True" Font-Names="Arial"
                Font-Size="Smaller" ForeColor="Maroon"></asp:Label>
            <asp:Table ID="tblSummary" runat="server" BorderColor="Silver" BorderStyle="Solid"
                BorderWidth="1px" CellPadding="0" CellSpacing="0" GridLines="Both" Width="100%">
            </asp:Table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
