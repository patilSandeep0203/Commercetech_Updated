<%@ Page Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true"
    CodeFile="CommAdmin.aspx.cs" Inherits="CommAdmin" Title="E-Commerce Exchange - Partner Portal"
    Theme="Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <cc1:DragPanelExtender ID="DragPanelExtender1" runat="server" TargetControlID="pnlFeatures"
        DragHandleID="pnlDrag">
    </cc1:DragPanelExtender>
    <cc1:AnimationExtender ID="AnimationExtender1" runat="server" TargetControlID="lnkClose">
        <Animations>
                <OnClick>
                    <Sequence>
                        <StyleAction AnimationTarget="info" Attribute="overflow" Value="hidden"/>
                        <StyleAction AnimationTarget="info" Attribute="display" Value="none"/>
                        <StyleAction AnimationTarget="info" Attribute="width" Value="700px"/>
                        <StyleAction AnimationTarget="info" Attribute="height" Value=""/>
                    </Sequence>
                </OnClick>                
        </Animations>
    </cc1:AnimationExtender>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
        Visible="False"></asp:Label><br />
        </ContentTemplate>
    </asp:UpdatePanel>    
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td colspan="3" align="center" style="background-image: url(../Images/homeback.gif)">
                <cc1:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_CSS">
                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Commissions">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblSelectRepName" Font-Size="8pt" Font-Names="Arial" runat="server"
                                            Font-Bold="True" Text="Select Rep Name"></asp:Label>&nbsp;
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:DropDownList ID="lstRepList" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                                            Font-Size="8pt" Text="Select Month"></asp:Label>&nbsp;
                                    </td>
                                    <td align="left" colspan="2" style="width: 174px">
                                        <asp:DropDownList ID="lstMonth" runat="server" Width=100>
                                            <asp:ListItem>Nov 2006</asp:ListItem>
                                            <asp:ListItem>Oct 2006</asp:ListItem>
                                            <asp:ListItem>Sep 2006</asp:ListItem>
                                            <asp:ListItem>Aug 2006</asp:ListItem>
                                            <asp:ListItem>July 2006</asp:ListItem>
                                            <asp:ListItem>June 2006</asp:ListItem>
                                            <asp:ListItem>May 2006</asp:ListItem>
                                            <asp:ListItem>Apr 2006</asp:ListItem>
                                            <asp:ListItem>Mar 2006</asp:ListItem>
                                            <asp:ListItem>Feb 2006</asp:ListItem>
                                            <asp:ListItem>Jan 2006</asp:ListItem>
                                            <asp:ListItem>Dec 2005</asp:ListItem>
                                            <asp:ListItem>Nov 2005</asp:ListItem>
                                            <asp:ListItem>Oct 2005</asp:ListItem>
                                            <asp:ListItem>May 2005</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" Font-Names="Arial"
                                            Font-Size="8pt" Text="Select Period"></asp:Label>&nbsp;
                                    </td>
                                    <td align="left" colspan="2" style="width: 174px">
                                        <asp:DropDownList ID="lstPeriod" runat="server" Width=100>
                                            <asp:ListItem Value=0 Selected="True">Full Month</asp:ListItem>
                                            <asp:ListItem Value=1>First Half</asp:ListItem>
                                            <asp:ListItem Value=2>Second Half</asp:ListItem>
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
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanelUpload" runat="server" HeaderText="Upload Commissions">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanelUpload" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:Label ID="lblUploadComm" runat="server" Font-Bold="True" Font-Names="Arial"
                                                    Font-Size="8pt" Text="Upload commissions for current month"></asp:Label>
                                                <br />
                                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"/>
                                                <asp:Panel ID="pnlUploadComm" runat="server" Width="90%" CssClass="DivHelp" Visible="false">
                                                    <b>Are you sure you want to upload Commissions for the current month?</b></div>
                                                    <asp:Button ID="btnUploadYes" runat="server" OnClick="btnUploadYes_Click" Text="Yes" />
                                                    <asp:Button ID="btnUploadNo" runat="server" OnClick="btnUploadNo_Click" Text="No" /></asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>                            
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10px"
                    ForeColor="Red" Text="+ denotes full-funded account"></asp:Label><br />
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10px"
                    ForeColor="Green" Text="# denotes half fundings"></asp:Label>
                    <asp:Label ID="quaterfunded" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10px"
                    ForeColor="Green" Text="- denotes quater fundings"></asp:Label>
            </td>
        </tr>
        <!--<tr>
            <td align="center" colspan="3">
                <asp:Panel ID="pnlResetComm" runat="server" Width="100%" Visible="false" BackColor="#edf7ff">
                    <asp:Label ID="lblResetComm" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"
                        Text="Reset Commission Pct to the Sales Rep default for the list below"></asp:Label>
                    <br />
                    <asp:Button ID="btnResetComm" Visible="false" runat="server" Text="Reset" OnClick="btnResetComm_Click" /><br />
                </asp:Panel>
            </td>
        </tr>-->
    </table>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <asp:Image ID="imgProgress" runat="server" ImageUrl="~/Images/indicator.gif" /><span class="LabelsRed"><b>Retrieving Data...Please Wait</b></span>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="info" style="z-index: 2; background-color: #FFFFFF; width: 750px;">
        <asp:Panel ID="pnlFeatures" Visible="false" runat="server" Width="750px" BackColor="white">
            <asp:Panel ID="pnlDrag" runat="server" BorderColor="black" BorderStyle="Solid" BorderWidth="1px"
                Height="15px">
                <div style="background-color: #4D99E6; width: 100%" id="btnCloseParent">
                    <asp:LinkButton ID="lnkClose" runat="server" OnClientClick="return false;" Font-Bold="True"
                        Font-Names="Arial" Font-Size="Smaller" ForeColor="White">Close</asp:LinkButton>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlGrid" runat="server" Style="border-right: silver thin solid; border-top: silver thin solid;
                z-index: 1; border-left: silver thin solid; border-bottom: silver thin solid;">
                <asp:GridView ID="grdComm" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" Visible="False">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Mon" HeaderText="Month" SortExpression="Mon">
                            <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA">
                            <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MerchantID" HeaderText="Merchant Number" SortExpression="MerchantNum">
                            <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RepName" HeaderText="Rep Name" SortExpression="RepName" />
                        <asp:BoundField DataField="ReferredBy" HeaderText="Referred By" SortExpression="ReferredBy">
                            <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product">
                            <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Units" HeaderText="Qty" SortExpression="Qty">
                            <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Price" HeaderText="Price (Per Unit)" SortExpression="Price" />
                        <asp:BoundField DataField="Total" HeaderText="Total" />
                        <asp:BoundField DataField="COG" HeaderText="COG" />
                        <asp:BoundField DataField="Commission" HeaderText="Comm" />
                        <asp:BoundField DataField="RepTotal" HeaderText="Rep Total" />
                        <asp:BoundField DataField="RefTotal" HeaderText="Referral Paid" />
                    </Columns>
                    <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                        ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
    </div>
    &nbsp;&nbsp;
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <br />
    <asp:Table ID="tblCommSummary" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
    &nbsp;&nbsp;<br />
    <asp:Table ID="tblCommissions" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
    <br />
    <asp:Table ID="tblBonus" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
</asp:Content>
