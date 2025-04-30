<%@ Page Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true" CodeFile="ReferralsAdmin.aspx.cs" Inherits="ReferralsAdmin" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(../Images/topMain.gif)">
                <b><span class="MenuHeader">Referral Reports</span></b></td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="8pt" Text="Select Month"></asp:Label>&nbsp;</td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstMonth" runat="server">
                    <asp:ListItem>Nov 2006</asp:ListItem>
                    <asp:ListItem>Oct 2006</asp:ListItem>
                    <asp:ListItem>Sep 2006</asp:ListItem>
                    <asp:ListItem>Aug 2006</asp:ListItem>
                    <asp:ListItem>July 2006</asp:ListItem>
                    <asp:ListItem>June 2006</asp:ListItem>
                    <asp:ListItem>May 2006</asp:ListItem>
                    <asp:ListItem>Apr 2006</asp:ListItem>
                    <asp:ListItem>Mar 2006</asp:ListItem>
                    <asp:ListItem>Feb 2006</asp:ListItem>
                    <asp:ListItem>Jan 2006</asp:ListItem>
                    <asp:ListItem>Dec 2005</asp:ListItem>
                    <asp:ListItem>Nov 2005</asp:ListItem>
                    <asp:ListItem>Oct 2005</asp:ListItem>
                    <asp:ListItem>May 2005</asp:ListItem>
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
    &nbsp;
    &nbsp;&nbsp;<br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    &nbsp;<br />
    <asp:Table ID="tblReferrals" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
</asp:Content>

