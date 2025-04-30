<%@ Page Language="C#" MasterPageFile="~/Reports/Employee.master" AutoEventWireup="true" CodeFile="CTCSalesOpps.aspx.cs" Inherits="CTCSalesOpps" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="2" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <b><span class="MenuHeader"">Pending Sales Opportunities</span></b></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <asp:Label ID="lblSelectRepName" Font-Size="8pt" Font-Names="Arial" runat="server"
                    Font-Bold="True" Text="Select Rep Name"></asp:Label>
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstRepList" runat="server">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                    Text="Select Status"></asp:Label></td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstStatus" runat="server" OnSelectedIndexChanged="lstStatus_SelectedIndexChanged">
                <asp:ListItem Selected="True">Open</asp:ListItem>
                <asp:ListItem >Closed</asp:ListItem>               
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="8pt" Text="Select Month"></asp:Label></td>
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
    <asp:Panel ID="pnlNote" runat="server" BackColor="Ivory" BorderColor="silver"
        BorderWidth="1px" Width="650px">
    <asp:Label ID="lblNote" runat="server" Font-Bold="True" ForeColor="red" Font-Names="Arial" Font-Size="Smaller">Closed-Won Opportunities are not officially funded until they are added to the Commission Report.</asp:Label>
    <br />
    <asp:Label ID="lblEmail" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller">If you find any discrepancies in this list, please contact </asp:Label>
    <asp:HyperLink ID="lnkEmail" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
        NavigateUrl="mailto:accounting@ecenow.com">accounting@ecenow.com</asp:HyperLink></asp:Panel>
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <asp:Label ID="lblTotal" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label>&nbsp;
    <br />
    <asp:GridView ID="grdSalesOpps" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="Vertical">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="DBA" HeaderText="DBA" />
            <asp:BoundField DataField="MerchantNum" HeaderText="Merchant Number" />
            <asp:BoundField DataField="Product" HeaderText="Product">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Price" HeaderText="Sell Price">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="COG" HeaderText="Cost Of Goods" />
            <asp:BoundField DataField="Quantity" HeaderText="Qty">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Status" HeaderText="Opportunity Status">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Stage" HeaderText="Stage" />
            <asp:BoundField DataField="ProductStatus" HeaderText="Product Status" />
            <asp:BoundField DataField="RepName" HeaderText="Rep Name" />
            <asp:BoundField DataField="ReferredBy" HeaderText="Referred By" />
            <asp:BoundField DataField="OpenDate" HeaderText="Open Date" />
            <asp:BoundField DataField="ActualCloseDate" HeaderText="Close Date" />
        </Columns>
        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
</asp:Content>

