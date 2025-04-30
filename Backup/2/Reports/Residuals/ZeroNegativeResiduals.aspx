<%@ Page Language="C#" MasterPageFile="~/Reports/Admin.master" AutoEventWireup="true" CodeFile="ZeroNegativeResiduals.aspx.cs" Inherits="Reports_Residuals_ZeroNegativeResiduals" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table style="width: 260px;" cellspacing="0" cellpadding="0" border="0" class="SilverBorder">
        <tr>
            <td style="background-image: url(../../Images/topMain.gif); height: 25px" align="center"
                colspan="3">
                <b><span class="MenuHeader">Zero & Negative Residuals</span></b></td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblMonthHeader" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Select Month"></asp:Label>&nbsp;</td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstMonth" runat="server">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="height: 5px" align="center" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit"></asp:Button>
            </td>
        </tr>
        <tr>
            <td style="height: 5px" align="center" colspan="3">
            </td>
        </tr>
    </table>
    <br />    
    <asp:Table ID="tblResiduals" runat="server" CellPadding="2" CellSpacing="0" GridLines="Both">
    </asp:Table>
</asp:Content>

