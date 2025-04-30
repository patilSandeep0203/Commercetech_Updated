<%@ Page Language="C#" MasterPageFile="~/Reports/Admin.master" AutoEventWireup="true" CodeFile="ECXLegacy.aspx.cs" Inherits="Residuals_ECXLegacy" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <b><span class="MenuHeader">ECX Legacy
                    Residual Reports</span></b></td>
        </tr>
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
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <br />
    <asp:Table ID="tblResiduals" runat="server" CellPadding="2" CellSpacing="0" GridLines="Both">
    </asp:Table>
</asp:Content>

