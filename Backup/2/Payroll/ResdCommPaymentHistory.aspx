<%@ Page Culture="auto" UICulture="auto" Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true" 
CodeFile="ResdCommPaymentHistory.aspx.cs" Inherits="Payroll_PartnerPaymentHistory"
    Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManagerComm" runat="server"  EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 425px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="2" style="background-image: url(../Images/topMain.gif);
                height: 25px">
                <b><span class="MenuHeader">Residual/Commission Payment History</span></b></td>
        </tr>
        <tr>
            <td colspan="2" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblPartnerName" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Partner Name"></asp:Label>&nbsp;</td>
            <td align="left">
                <asp:DropDownList ID="lstPartnerName" runat="server">
                </asp:DropDownList></td>
        </tr>        
        <tr>
            <td align="center" colspan="2" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlResdCommHistory" runat="server" Visible="False">
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 900px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Residual and Commission History</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="grdResdCommHistory" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                        OnRowDataBound="grdResdCommHistory_RowDataBound" ShowFooter = "True" Width="900px">
                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="ResidualMon" HeaderText="Residual Month" FooterText="SubTotal">
                            </asp:BoundField>
                            <asp:BoundField DataField="Mon" HeaderText="Commission Month"/>
                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID"/>
                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name"/>
                            <asp:BoundField DataField="ConfirmNum" HeaderText="Confirmation"/>
                            <asp:BoundField DataField="ConfirmDate" HeaderText="Date Paid"/>
                            <asp:BoundField DataField="Note" HeaderText="Note"/>                            
                            <asp:BoundField DataField="CommTotal" HeaderText="Commission"/>
                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus"/>
                            <asp:BoundField DataField="RefTotal" HeaderText="Referral"/>
                            <asp:BoundField DataField="Residual" HeaderText="Residual"/>
                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover"/>
                            <asp:BoundField HeaderText="Total"/>
                            <asp:BoundField DataField="Payment" HeaderText="Payment"/>                                            
                        </Columns>
                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" CssClass="MenuHeader" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlResdHistory" runat="server" Visible="False">
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 900px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Residual History</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="grdResdHistory" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical"
                        OnRowDataBound="grdResdHistory_RowDataBound" ShowFooter="True" Width ="900px">
                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                        <Columns>                            
                            <asp:BoundField DataField="ResidualMon" HeaderText="Residual Month" FooterText="SubTotal">
                            </asp:BoundField>
                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID"/>
                            <asp:BoundField DataField="Contact" HeaderText="Contact"/>
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name"/>
                            <asp:BoundField DataField="ResdConfirmNum" HeaderText="Confirmation"/>
                            <asp:BoundField DataField="ResdConfirmDate" HeaderText="Date Paid"/>
                            <asp:BoundField DataField="ResdNote" HeaderText="Note"/>                            
                            <asp:BoundField DataField="Residual" HeaderText="Residual"/>
                            <asp:BoundField DataField="CarryoverResd" HeaderText="Carryover"/>
                            <asp:BoundField HeaderText="Total"/>
                            <asp:BoundField DataField="Payment" HeaderText="Payment"/>  
                        </Columns>
                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" CssClass="MenuHeader" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlCommHistory" runat="server" Visible="False">
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 900px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Commission History</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="grdCommHistory" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Vertical"
                        OnRowDataBound="grdCommHistory_RowDataBound" ShowFooter="True" Width ="900px">
                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Mon" HeaderText="Month" FooterText="SubTotal">
                            </asp:BoundField>
                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID"/>
                            <asp:BoundField DataField="Contact" HeaderText="Contact"/>
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name"/>
                            <asp:BoundField DataField="CommRefConfirmNum" HeaderText="Confirmation"/>
                            <asp:BoundField DataField="CommRefConfirmDate" HeaderText="Date Paid"/>
                            <asp:BoundField DataField="CommRefNote" HeaderText="Note"/>                            
                            <asp:BoundField DataField="CommTotal" HeaderText="Commission"/>
                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus"/>
                            <asp:BoundField DataField="RefTotal" HeaderText="Referral"/>
                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover"/>
                            <asp:BoundField HeaderText="Total"/>
                            <asp:BoundField DataField="Payment" HeaderText="Payment"/>  
                        </Columns>
                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" CssClass="MenuHeader" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>    
    <asp:Table ID="tblTotal" runat="server" CellPadding="0" CellSpacing="0" BackColor="#edf7ff" Width="900px" ForeColor="Black"></asp:Table>
    <br />
</asp:Content>
