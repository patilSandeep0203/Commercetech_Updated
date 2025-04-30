<%@ Page Language="C#" MasterPageFile="~/OnlineAppMgmt/User.master" AutoEventWireup="true" CodeFile="ManageLeadsPartner.aspx.cs" Inherits="ManageLeadsPartner" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;"  class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/Images/topMain.gif)">
                <b><span class="MenuHeader">firstaffiliates.com
                    Leads</span></b>
            </td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblLeadReport" Font-Size="8pt" Font-Names="Arial" runat="server"
                    Font-Bold="True" Text="Select Report Type"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstLeadReport" runat="server">
                    <asp:ListItem>Leads</asp:ListItem>
                    <asp:ListItem>Partner Signups</asp:ListItem>
                </asp:DropDownList>
            </td>
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
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView GridLines="Vertical" ID="grdAllLeads" runat="server" AutoGenerateColumns="False"
        CellPadding="4" ForeColor="#333333"
        Visible="False">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" SortExpression="CreateDate">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
        </Columns>
        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle Font-Names="Arial" Font-Size="Small" BackColor="#5D7B9D" Font-Bold="True"
            ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    &nbsp;&nbsp;
    <asp:GridView ID="grdAffiliateSignups" runat="server" AutoGenerateColumns="False"
        CellPadding="1" ForeColor="#333333" GridLines="Vertical" Visible="False">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="AffiliateId" HeaderText="Partner ID">
                <HeaderStyle Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="FirstName" HeaderText="First Name">
                <HeaderStyle Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="LastName" HeaderText="Last Name"/>
            <asp:BoundField DataField="Email" HeaderText="Email">
                <HeaderStyle Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="CompanyName" HeaderText="Company"/>
            <asp:BoundField DataField="DBA" HeaderText="DBA"/>
            <asp:BoundField DataField="ReferralTier" HeaderText="Referral Tier"/>
        </Columns>
        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Names="Arial" Font-Size="Small" Font-Bold="True"
            ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
</asp:Content>

