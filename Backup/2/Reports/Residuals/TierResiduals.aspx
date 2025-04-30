<%@ Page Language="C#" MasterPageFile="~/Reports/Agent.master" AutoEventWireup="true" CodeFile="TierResiduals.aspx.cs" Inherits="Reports_Residuals_TierResidual" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 300px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <b><span class="MenuHeader">E-Commerce Exchange Tier
                    Residual Reports</span></b></td>
        </tr>
        <tr>
            <td align="right" colspan="3" style="height:5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <span class="LabelsSmall"><b>Select Month</b></span>&nbsp;</td>
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
    <asp:Label ID="lblRepName" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"
                    Visible="False"></asp:Label>
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <br />
    <asp:Table ID="tblResiduals" runat="server" CellPadding="2" Width="600px" CellSpacing="0" GridLines="Both">
    </asp:Table>
</asp:Content>

