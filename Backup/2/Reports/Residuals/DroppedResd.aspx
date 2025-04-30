<%@ Page Language="C#" MasterPageFile="~/Reports/Admin.master" AutoEventWireup="true" CodeFile="DroppedResd.aspx.cs"
    Inherits="Residuals_DroppedResd" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table style="width: 260px;" cellspacing="0" cellpadding="0" border="0" class="SilverBorder">
        <tr>
            <td style="background-image: url(/PartnerPortal/Images/topMain.gif); height: 25px" align="center"
                colspan="3">
                <b><span class="MenuHeader">Dropped Residuals</span></b></td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td style="width: 50%" align="right">
                <asp:Label ID="lblLeadReport" runat="server" Font-Size="Smaller" Font-Names="Arial"
                    Text="Select Report" Font-Bold="True"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstReport" runat="server">
                    <asp:ListItem>iPayment</asp:ListItem>
                    <asp:ListItem>iPayment2</asp:ListItem>
                    <asp:ListItem>iPayment3</asp:ListItem>
                    <asp:ListItem Value="iPayFBBH">iPayment FBBH</asp:ListItem>
                    <asp:ListItem>IMS</asp:ListItem>
                    <asp:ListItem>IPS</asp:ListItem>
                    <asp:ListItem>IMS2</asp:ListItem>
                    <asp:ListItem>Sage</asp:ListItem>
                    <asp:ListItem>Innovative</asp:ListItem>
                    <asp:ListItem>CPS</asp:ListItem>
                    <asp:ListItem>Chase</asp:ListItem>
                    <asp:ListItem>Merrick</asp:ListItem>
                    <asp:ListItem Value="OptimalCA">Optimal CA</asp:ListItem>
                    <asp:ListItem>WorldPay</asp:ListItem>
                    <asp:ListItem Value="Authnet">Authorize.Net</asp:ListItem>
                    <asp:ListItem Value="iPayGate">iPayment Gateway</asp:ListItem>
                    <asp:ListItem Value="InnGate">Innovative Gateway</asp:ListItem>
                    <asp:ListItem>PlugNPay</asp:ListItem>
                    <asp:ListItem Value="CheckServices">Check Services</asp:ListItem>
                    <asp:ListItem Value="GiftCardServices">Gift Card Services</asp:ListItem>
                    <asp:ListItem>CTCart</asp:ListItem>
                    <asp:ListItem Value="MerchantCashAdvance">Merchant Cash Advance</asp:ListItem>
                    <asp:ListItem>Payroll</asp:ListItem>
                </asp:DropDownList>
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
                <strong><span style="color: #ff0000; font-family: Arial; font-size:8pt">NOTE: Dropped
                    Merchants may include ACH Rejects and Non-Payments</span></strong></td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <asp:Table ID="tblResiduals" runat="server" CellPadding="2" CellSpacing="0" GridLines="Both">
    </asp:Table>
</asp:Content>
