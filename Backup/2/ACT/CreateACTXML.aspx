<%@ Page Language="C#" MasterPageFile="~/ACT/Admin.master" AutoEventWireup="true" CodeFile="CreateACTXML.aspx.cs"
    Inherits="CreateACTXML" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="panSearch" runat="server" DefaultButton="btnSubmit" Width="100%" >
    <table cellpadding="0" cellspacing="0" border="0" style="width: 300px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(../Images/topMain.gif)">
                <b><span class="MenuHeader">Create XML from ACT!</span></b>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 23%" valign="top">
                <asp:Label ID="lblEmailHeader" Font-Bold="True" runat="server" Text="Look up by "></asp:Label>&nbsp;
            </td>
            <td align="left">
                &nbsp;<asp:DropDownList ID="lstLookup" runat="server">
                    <asp:ListItem>Email</asp:ListItem>
                    <asp:ListItem>Contact</asp:ListItem>
                    <asp:ListItem>CompanyName</asp:ListItem>
                    <asp:ListItem>DBA</asp:ListItem>
                </asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtLookup" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLookup"
                    EnableClientScript="False" ErrorMessage="Please enter the value to look up"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="2">
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <asp:Image ID="imgProgress" runat="server" ImageUrl="~/Images/indicator.gif" /><span
                class="LabelsRed"><b>Retrieving Data...Please Wait</b></span>
        </ProgressTemplate>
    </asp:UpdateProgress>    
    <br />
    <asp:UpdatePanel ID="UpdatePanelLeads" runat="server">
        <ContentTemplate>
        
            <asp:Panel ID="pnlAttachment" runat="server" BackColor="Ivory" BorderColor="Red" BorderWidth="1px"
        Width="300px" Visible="true">
        <strong><span style="font-family:Arial; font-size:small; color:#383838">Attach file</span>                 
                    
            <asp:Label ID="lblAttachment" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>:<br />   
            <span style="font-size: 8pt">
                <asp:LinkButton ID="btnAttachFile" runat="server" OnClick="btnAttachFile_Click" OnClientClick="aspnetForm.target ='_blank';" CssClass="One" Font-Names="Arial" Font-Size="10pt" Text="Attach document"></asp:LinkButton><br />
            </span></strong></asp:Panel>
    <asp:GridView ID="grdPDF" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdPDF_RowCommand">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="AppId" HeaderText="AppId" SortExpression="AppId">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="ContactID" HeaderText="ContactID" SortExpression="ContactID">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="RepName" HeaderText="Sales Rep">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Processor" HeaderText="Processor" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="MerchantStatus" HeaderText="Merchant Status" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:ButtonField Text="Create XML" CommandName="CreateXML">
            <ItemStyle Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" />
                <HeaderStyle Font-Names="Arial" Font-Size="X-Small" />
            </asp:ButtonField>
        </Columns>
        <RowStyle Font-Names="Arial" Font-Size="X-Small" BackColor="#EDF7FF" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
