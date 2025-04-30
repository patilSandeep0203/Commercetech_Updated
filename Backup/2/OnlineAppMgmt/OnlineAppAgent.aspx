<%@ Page Language="C#" MasterPageFile="~/OnlineAppMgmt/Agent.master" AutoEventWireup="true"
    CodeFile="OnlineAppAgent.aspx.cs" Inherits="OnlineAppAgent" Title="E-Commerce Exchange - Partner Portal"
    Theme="Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 300px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <span class="MenuHeader"><b>Manage Online Applications</b></span>
                <asp:ImageButton ID="imgHelp" Style="cursor:pointer" runat="server" CausesValidation="false" 
                ImageUrl="/PartnerPortal/Images/help.gif" ToolTip="Help" OnClientClick="javascript:popupHelp('/PartnerPortal/HelpAgent.aspx', '01');return false;" />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <cc1:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_CSS">
                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Search">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblStatus" Font-Size="11px" Font-Names="Arial" runat="server" Font-Bold="True"
                                            Text="Select Status" ForeColor="#383838"></asp:Label>&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="lstStatus" runat="server">
                                            <asp:ListItem>ALL</asp:ListItem>
                                            <asp:ListItem>INCOMPLETE</asp:ListItem>
                                            <asp:ListItem>COMPLETED</asp:ListItem>
                                            <asp:ListItem>DECLINED</asp:ListItem>
                                            <asp:ListItem>PENDING</asp:ListItem>
                                            <asp:ListItem>FUNDED JOB DONE</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Advanced Search" ToolTip="Advanced search options">
                        <ContentTemplate>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            <table cellpadding="0" cellspacing="2" border="0" style="width: 100%;">
                                <tr>
                                    <td align="right" style="width: 30%">
                                        <span class="LabelsSmall"><b>Filter Date From</b></span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="50" Width="80px"></asp:TextBox></td>
                                    <td align="left">
                                        <span class="LabelsSmall"><b>To</b></span>
                                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="50" Width="80px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 30%">
                                        <asp:Label ID="lblSortBy" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11px"
                                            ForeColor="#383838" Text="Sort By"></asp:Label>&nbsp;</td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstSortBy" runat="server">
                                            <asp:ListItem>AppId</asp:ListItem>
                                            <asp:ListItem Value="FirstName">First Name</asp:ListItem>
                                            <asp:ListItem Value="LastName">Last Name</asp:ListItem>
                                            <asp:ListItem>DBA</asp:ListItem>
                                            <asp:ListItem Value="AcctTypeDesc">Account Type</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Create Online Applications">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:Label ID="lblCreateOnlineApplications" runat="server" Font-Bold="True" Text="You can create online applications by clicking on the following link:"
                                            Font-Names="Arial" Font-Size="11px" ForeColor="#383838"></asp:Label><br />
                                        <asp:HyperLink CssClass="One" ID="lnkCreateOnlineApp" runat="server" Font-Bold="True"
                                            Font-Names="Arial" Font-Size="Small" Target="_blank">Create Online Applications</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /><br />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                    ForeColor="Red" Text="* - denotes QuickBooks Signup"></asp:Label><br />
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblNumberOfRecords" runat="server" ForeColor="SteelBlue" Font-Size="Smaller"
        Font-Names="Arial" Font-Bold="True"></asp:Label>
    <asp:Table ID="tblSummary" runat="server" BorderColor="Silver" BorderStyle="Solid"
        GridLines="Vertical" BorderWidth="1px" CellPadding="0" CellSpacing="0" Width="80%">
    </asp:Table>
</asp:Content>
