<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ManagePartners.aspx.cs" Inherits="ManagePartners" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 300px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(Images/topMain.gif)">
                <span style="font-family: Arial; font-size: Small; color: White"><b>Manage Partners</b></span>
            </td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Panel ID="pnlRepList" runat="server" Width="100%">
                <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="right" style="width: 50%">
                            <asp:Label ID="Label1" Font-Size="Smaller" Font-Names="Arial" runat="server"
                                Font-Bold="True" Text="Select Partner Name"></asp:Label>&nbsp;
                        </td>
                        <td align="left" colspan="2">
                            <asp:DropDownList ID="lstRepList" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </td>            
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblPartnerCategory" Font-Size="Smaller" Font-Names="Arial" runat="server"
                    Font-Bold="True" Text="Select Partner Category"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstPartnerCategory" runat="server">
                    <asp:ListItem>ALL</asp:ListItem>
                    <asp:ListItem Value="I">Inactive</asp:ListItem>
                    <asp:ListItem Value="A">Agent</asp:ListItem>
                    <asp:ListItem Value="PE">Past Employee</asp:ListItem>
                    <asp:ListItem Value="E">Employee</asp:ListItem>
                    <asp:ListItem Value="R">Reseller</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblMonth" Font-Size="Smaller" Font-Names="Arial" runat="server" Font-Bold="True"
                    Text="Select a Month to View"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstMonth" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="height:30px" valign="middle" colspan="3" align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />                
            </td>
        </tr>
    </table>
    <br />
    <strong><span style="color: #ff0000; font-family:Arial; font-size:smaller">* - denotes a month-based field</span></strong>
    <asp:Table ID="tblSummary" runat="server" BorderColor="Silver" BorderStyle="Solid"
        BorderWidth="1px" CellPadding="2" CellSpacing="1" Width="750px">
    </asp:Table>    
</asp:Content>

