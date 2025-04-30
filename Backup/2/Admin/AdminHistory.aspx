<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminHistory.aspx.cs"
    Inherits="AdminHistory" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <br />
    <asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="Vertical">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="Contact" HeaderText="Modified By" SortExpression="Contact">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Action" HeaderText="Action" SortExpression="Action" />
            <asp:BoundField DataField="RecordedDate" HeaderText="Date Recorded" SortExpression="DateRecorded">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
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
