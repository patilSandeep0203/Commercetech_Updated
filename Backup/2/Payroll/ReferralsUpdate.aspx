<%@ Page Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true" CodeFile="ReferralsUpdate.aspx.cs" Inherits="ReferralsUpdate" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <br />    
    <asp:Panel ID="pnlUpdateReferral" runat="server">
    <table cellpadding="1" cellspacing="1" border="0" style="border-right: silver 1px solid;
        border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
        background-color: #edf7ff; width:700px">
        <tr>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Referred By</span></b></td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Referral ID</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Merchant DBA</span></b></td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Month</span></b></td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Merchant ID</span></b>&nbsp;</td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Rep Name</span></b></td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Product</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Units</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Amount</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Ref Paid</span></b></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblReferredBy" runat="server"></asp:Label>&nbsp;</td>
            <td>
                <asp:Label ID="lblReferralID" runat="server"></asp:Label></td>
            <td>
                &nbsp;<asp:Label ID="lblDBA" runat="server"></asp:Label></td>
            <td>
                <asp:Label ID="lblMonth" runat="server"></asp:Label></td>
            <td>
                &nbsp;<asp:Label ID="lblMerchantID" runat="server"></asp:Label></td>
            <td>
                &nbsp;<asp:Label ID="lblRepName" runat="server"></asp:Label></td>
            <td>
                <asp:Label ID="lblProduct" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;<asp:Label ID="lblUnits" runat="server"></asp:Label></td>
            <td>
                <asp:Label ID="lblAmount" runat="server"></asp:Label>&nbsp;</td>
            <td>
                <asp:TextBox ID="txtRefPaid" runat="server" Width="40px"></asp:TextBox>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="10" align="center">
                <br />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
            </td>
        </tr>
    </table>
    </asp:Panel>    
</asp:Content>

