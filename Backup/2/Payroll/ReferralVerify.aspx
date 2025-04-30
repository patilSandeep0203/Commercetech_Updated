<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReferralVerify.aspx.cs" Inherits="ReferralVerify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
</head>
<body>
    <center>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
        <asp:Label ID="lblHistory" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
        <br />
    <asp:GridView id="grdReferrals" runat="server" ForeColor="#333333" Visible="False" CellPadding="4" AutoGenerateColumns="False">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
        <Columns>
        <asp:BoundField DataField="Mon" HeaderText="Month" SortExpression="Mon">
                <HeaderStyle Font-Names="Arial" Font-Size="Small"  />
            </asp:BoundField>
            <asp:BoundField DataField="ReferredBy" HeaderText="Referred By" SortExpression="ReferredBy">
                <HeaderStyle Font-Names="Arial" Font-Size="Small"  />
            </asp:BoundField>
            <asp:BoundField DataField="ReferralID" HeaderText="Referral ID" SortExpression="ReferralID">
                <HeaderStyle Font-Names="Arial" Font-Size="Small"  />
            </asp:BoundField>
            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" ></asp:BoundField>
            <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA">
                <HeaderStyle Font-Names="Arial" Font-Size="Small"  />
            </asp:BoundField>
            <asp:BoundField DataField="MerchantID" HeaderText="Merchant ID" SortExpression="MerchantID">
                <HeaderStyle Font-Names="Arial" Font-Size="Small"  />
            </asp:BoundField>
            <asp:BoundField DataField="RepName" HeaderText="RepName" SortExpression="RepName">
                <HeaderStyle Font-Names="Arial" Font-Size="Small"  />
            </asp:BoundField>
            <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" ></asp:BoundField>
            <asp:BoundField DataField="Units" HeaderText="Units" />
            <asp:BoundField DataField="Total" HeaderText="Total" />
            <asp:BoundField DataField="RefTotal" HeaderText="Referral Paid" />
        </Columns>
        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333"  />
        <EditRowStyle BackColor="#999999"  />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"  />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"  />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            ForeColor="White"  />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
    </asp:GridView>
    </div>
    </form>
    </center>
</body>
</html>
