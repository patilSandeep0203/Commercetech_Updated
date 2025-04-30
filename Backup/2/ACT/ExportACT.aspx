<%@ Page Language="C#" MasterPageFile="~/ACT/Admin.master" AutoEventWireup="true" CodeFile="ExportACT.aspx.cs"
    Inherits="ExportACT" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="panSearch" runat="server" DefaultButton="btnSubmit" Width="100%" >
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height:25px; background-image: url(../Images/topMain.gif)">
                <b><span class="MenuHeader">Export ACT! record to Online App</span></b>
            </td>
        </tr>
        <tr>
            <td align="right" style="width:40%" valign="top">
                <asp:Label ID="lblEmailHeader" Font-Bold="true" runat="server" Text="Email"></asp:Label>&nbsp;
            </td>
            <td align="left">
                <asp:TextBox ID="txtEmail" runat="server" Width="128px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail"
                    EnableClientScript="False" ErrorMessage="Please enter the Email address to look up"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" style="height:20px">
                <asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="2">
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlSameLoginID" Width = 50% runat="server" HorizontalAlign="Center" Visible=false BackColor="Ivory" BorderStyle=Solid BorderWidth=1 BorderColor="red"><br />
        <asp:Label ID="lblContactID" runat="server" Font-Bold="True" Visible="False"></asp:Label><br />
        <span class="LabelsRed">This Login Name (email address) is either invalid or it already exists.<br />Please enter a different Login Name below:<br />
            <asp:TextBox ID="txtLogin" runat="server" Width="128px"></asp:TextBox><br />
            <asp:Button ID="btnSubmitNewLogin" runat="server" Text="Submit" OnClick="btnSubmitNewLogin_Click" />
        </span><br /><br />
    </asp:Panel>
    <asp:Panel ID="pnlNote" runat="server" BackColor="Ivory" BorderColor="silver"
        BorderWidth="1px" Width="50%">
        <div class="LabelsRed">
            Please follow these steps after the record is exported
            <div align="left">
                <ol>
                    <li>Enter the new Online App ID in the ACT! record.</li>
                    <li>Set any Processing Percentages via the Card Percentages tab with in the Online Application.</li>
                    <li>Default username is the email address (if present) and default password is 'Succeed1'</li>
                
                </ol>
            </div>
        </div>
    </asp:Panel>
    <br />
    <asp:GridView ID="grdResidual" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdResidual_RowCommand">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="AppId" HeaderText="AppId" SortExpression="AppId" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="ContactID" HeaderText="ContactID" SortExpression="ContactID" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="AffiliateReferral" HeaderText="Referred By" SortExpression="AffiliateReferral" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="RepName" HeaderText="Sales Rep" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Processor" HeaderText="Processor" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="MerchantStatus" HeaderText="Merchant Status" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Gateway" HeaderText="Gateway" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="GatewayStatus" HeaderText="Gateway Status" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:ButtonField CommandName="Export" Text="EXPORT" >
                <ItemStyle Font-Names="Arial" Font-Size="Small" Font-Bold="True" />
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:ButtonField>
        </Columns>
        <RowStyle Font-Names="Arial" Font-Size="X-Small" BackColor="#EDF7FF" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <br />
</asp:Content>
