<%@ Page Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true"
    CodeFile="ActResiduals.aspx.cs" Inherits="ResidualsAdmin" Title="E-Commerce Exchange - Partner Portal"
    Theme="Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
                Visible="False"></asp:Label><br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(../Images/homeback.gif)">
                <cc1:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_CSS">
                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="ACT Residual Status">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                                            Font-Size="8pt" Text="Select Month"></asp:Label>&nbsp;</td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstMonth" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial"
                                            Font-Size="8pt" Text="Services"></asp:Label>&nbsp;</td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstService" runat="server">
                                            <asp:ListItem Value="Merchant Account">Merchant Account</asp:ListItem>
                                            <asp:ListItem>Gateway</asp:ListItem>
                                            <asp:ListItem Value="Check Service">Check Service</asp:ListItem>
                                            <asp:ListItem Value="Gift Card">Gift Card</asp:ListItem>
                                            <asp:ListItem Value="Cash Advance">Cash Advance</asp:ListItem>
                                            <asp:ListItem>Payroll</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial"
                                            Font-Size="8pt" Text="Status"></asp:Label>&nbsp;</td>
                                        <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstStatus" runat="server">
                                            <asp:ListItem>CLOSED RESIDUAL</asp:ListItem>
                                            <asp:ListItem>ACTIVE</asp:ListItem>
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
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelUpload" runat="server" HeaderText="Update ACT Residuals">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanelUpload" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td align="center" colspan="3" height="10px">
                                                <asp:Label ID="lblUploadResd" runat="server" Font-Bold="True" Font-Names="Arial"
                                                    Font-Size="8pt" Text="Update ACT Rates"></asp:Label>
                                            </td>
                                            
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpdateACTRates_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabActivatedReport" runat="server" HeaderText="Update Dropped Residuals">
                        <ContentTemplate>
                            
                                    <table cellspacing="0" cellpadding="0" border="0" style="width: 100%;">
        <tr>
            <td style="background-image: url(/PartnerPortal/Images/topMain.gif); height: 25px" align="center"
                colspan="3">
                <b><span class="MenuHeader">Dropped Reports</span></b></td>
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
                    <asp:ListItem>IPS</asp:ListItem>
                    <asp:ListItem>IMS2</asp:ListItem>
                    <asp:ListItem>Sage</asp:ListItem>
                    <asp:ListItem>CPS</asp:ListItem>
                    <asp:ListItem>Chase</asp:ListItem>
                    <asp:ListItem>Merrick</asp:ListItem>
                    <asp:ListItem Value="OptimalCA">Optimal CA</asp:ListItem>
                    <asp:ListItem Value="Authnet">Authorize.Net</asp:ListItem>
                    <asp:ListItem Value="iPayGate">iPayment Gateway</asp:ListItem>
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
                <asp:DropDownList ID="lstMonthDropped" runat="server">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="height: 5px" align="center" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Button ID="btnGetDropped" OnClick="btnGetDropped_Click" runat="server" Text="Submit"></asp:Button>
            </td>
        </tr>
        
        <tr>
            <td style="height: 5px" align="center" colspan="3">
            </td>
        </tr>
    </table>
                                
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
               

            </td>
        </tr>
    </table>
    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <asp:Image ID="imgProgress" runat="server" ImageUrl="~/Images/indicator.gif" /><span class="LabelsRed"><b>Retrieving Data...Please Wait</b></span>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    &nbsp; &nbsp;&nbsp;<br />
    <asp:Table ID="tblResiduals" runat="server" BorderColor="Silver" BorderStyle="Solid"
        BorderWidth="1px" CellPadding="2" CellSpacing="0" GridLines="Both">
    </asp:Table>
    <br/>
    <asp:Panel ID="pnlUpdateStatus" runat="server" Visible="false">
        <asp:Button ID="btnUpdateStatus" runat="server" Text="Update Status" OnClick="btnUpdateStatus_Click" />
    </asp:Panel><br/>
    <asp:Panel ID="pnlUpdateDroppedReport" runat="server" Visible="false">
        <asp:Button ID="Button1" runat="server" Text="Update Dropped Reports" OnClick="btnUpdateDropped_Click" />
    </asp:Panel><br/>
    <asp:Table ID="tblTotals" runat="server" BorderColor="Silver" BorderStyle="Solid"
        BorderWidth="1px" CellPadding="2" CellSpacing="0" GridLines="Both">
    </asp:Table>
</asp:Content>
