<%@ Page Language="C#" MasterPageFile="~/Reports/Admin.master" AutoEventWireup="true" CodeFile="iPayFBBH.aspx.cs" Inherits="Residuals_iPayFBBH" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
<asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <b><span class="MenuHeader">iPay FBBH
                    Residual Reports</span></b></td>
        </tr>
        <tr>
            <td>
                <cc1:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_CSS">
                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Search">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblSelectRepName" Font-Size="8pt" Font-Names="Arial" runat="server"
                                            Font-Bold="True" Text="Select Rep Name"></asp:Label>&nbsp;
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstRepList" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                                            Font-Size="8pt" Text="Select Month"></asp:Label>&nbsp;</td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstMonth" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="center" style="height: 5px" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3" style="height:5px">             
                                        </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelLookup" runat="server" HeaderText="Lookup by DBA">
                        <ContentTemplate>
                        <asp:Panel ID="panSearch" runat="server" DefaultButton="btnLookup" Width="100%" >
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="right" style="width:50%">
                                        <asp:Label ID="lblLookUpBy" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                            ForeColor="#383838" Text="Lookup By DBA"></asp:Label>&nbsp;</td>
                                    <td align="left" style="width:50%">
                                        <asp:TextBox ID="txtLookup" runat="server" MaxLength="150" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnLookup" runat="server" OnClick="btnLookup_Click" Text="Lookup" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label>
    <asp:Table ID="tblResiduals" runat="server" CellPadding="2" CellSpacing="0" GridLines="Both">
    </asp:Table>
</asp:Content>

