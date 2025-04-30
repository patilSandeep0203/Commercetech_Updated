<%@ Page Language="C#" MasterPageFile="~/OnlineAppMgmt/User.master" AutoEventWireup="true" CodeFile="OnlineAppReferrals.aspx.cs" Inherits="OnlineAppReferrals" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 400px; border-right: silver 1px solid;
        border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
        background-color: #edf7ff">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <span style="font-family: Arial; font-size: Small; color: White"><b>Online Applications</b></span>
            </td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <asp:Label ID="lblStatus" Font-Size="8pt" Font-Names="Arial" runat="server" Font-Bold="True"
                    Text="Select Status" ForeColor="#383838"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstStatus" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <asp:Label ID="lblFilterDate" Font-Size="8pt" Font-Names="Arial" runat="server"
                    Font-Bold="True" Text="Filter Date From" ForeColor="#383838"></asp:Label>&nbsp;</td>
            <td align="left">
                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="50" Width="80px"></asp:TextBox></td>
            <td align="left">
                <asp:Label ID="lblToDate" Font-Size="8pt" Font-Names="Arial" runat="server" Font-Bold="True"
                    Text=" To"></asp:Label>
                <asp:TextBox ID="txtToDate" runat="server" MaxLength="50" Width="80px"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /><br />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Label ID="lblCreateOnlineApplications" runat="server" Font-Bold="True" Text="You can create online applications by clicking on the following link:" Font-Names="Arial" Font-Size="11px" ForeColor="#383838"></asp:Label><br />
                <asp:HyperLink CssClass="One" ID="lnkCreateOnlineApp" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" Target="_blank">Create Online Applications</asp:HyperLink></td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <b><span style="color: #ff0000; font-family:Arial; font-size:small">* - denotes QuickBooks Signup</span></b>
            </td>
        </tr>
    </table>    
    <br />
    <asp:Label id="lblNumberOfRecords" runat="server" ForeColor="SteelBlue" Font-Size="Smaller" Font-Names="Arial" Font-Bold="True"></asp:Label>
    <asp:Table ID="tblSummary" runat="server" BorderColor="Silver" BorderStyle="Solid" GridLines="Vertical"
        BorderWidth="1px" CellPadding="0" CellSpacing="0" Width="90%">
    </asp:Table>
    <br />
</asp:Content>

