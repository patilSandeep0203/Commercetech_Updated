<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommRefSummaryPrint.aspx.cs" Inherits="Payroll_CommRefSummaryPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
</head>
<body style="font-size: 12pt">
<form runat=server>
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <asp:Panel ID="pnlEmployees" runat="server" Visible="False">
        <table border="0" cellspacing="0" style="border-right: silver 1px solid; border-top: silver 1px solid;
            border-left: silver 1px solid; width: 900px; border-bottom: silver 1px solid;
            background-color: #fbfbfb">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial; ">Employees</span></b>
                </td>            
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlCurrEmployees" runat="server">
                        <table border="0" cellspacing="0" style="width: 900px">
                            <tr>
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size: small">Current Employees</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdEmployeeSummary" runat="server" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowDataBound="grdEmployeeSummary_RowDataBound"
                                        ShowFooter="True" Width ="900px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CommRefConfirmNum" HeaderText="Confirmation" FooterStyle-Font-Size="Small" FooterText ="Sub-Totals:"/>
                                            <asp:BoundField DataField="CommRefConfirmDate" HeaderText="Date Paid" />
                                             <asp:BoundField DataField="CommRefNote" HeaderText="Notes" />
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" />
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Total">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment">
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
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlPrevEmployees" runat="server">
                        <table border="0" cellspacing="0" style="width: 900px">
                            <tr>
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size: small">Previous Employees</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdPrevEmployeeSummary" runat="server" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                                        OnRowDataBound="grdPrevEmployeeSummary_RowDataBound" ShowFooter = "True" Width="900px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:ButtonField CommandName="Confirmation" Text="Edit" FooterStyle-Font-Size="Small" FooterText="SubTotals:">
                                                <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="CommRefConfirmNum" HeaderText="Confirmation" />
                                            <asp:BoundField DataField="CommRefConfirmDate" HeaderText="Date Paid" />
                                            <asp:BoundField DataField="CommRefNote" HeaderText="Note" />
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" />
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Total">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment">
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
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlPartners" runat="server" Visible="False">
        <table border="0" cellspacing="0" style="border-right: silver 1px solid; border-top: silver 1px solid;
            border-left: silver 1px solid; width: 900px; border-bottom: silver 1px solid;
            background-color: #fbfbfb">
            <tr>
                <td align="center" style="background-color:#5c995d">
                    <b><span style="color: white; font-family: Arial">Partners</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlDDPartner" runat="server">
                        <table border="0" cellspacing="0" style="width: 900px">
                            <tr>
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size:small">Direct Deposit</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdSummaryDD" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="grdSummaryDD_RowDataBound"
                                        ShowFooter="True" Width ="900px">
                                    <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CommRefConfirmNum" HeaderText="Confirmation" FooterStyle-Font-Size="Small" FooterText="SubTotals:"/>
                                            <asp:BoundField DataField="CommRefConfirmDate" HeaderText="Date Paid" />
                                            <asp:BoundField DataField="CommRefNote" HeaderText="Notes" />
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" />
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Total">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment">
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
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlBPPartners" runat="server">
                        <table border="0" cellspacing="0" style="width: 900px">
                            <tr>                      
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size:small">Bill Pay</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdSummaryBP" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="Vertical" OnRowDataBound="grdSummaryBP_RowDataBound"
                                        ShowFooter="True" Width ="900px">
                                    <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CommRefConfirmNum" HeaderText="Confirmation" FooterStyle-Font-Size="Small" FooterText="SubTotals:"/>
                                            <asp:BoundField DataField="CommRefConfirmDate" HeaderText="Date Paid" />
                                            <asp:BoundField DataField="CommRefNote" HeaderText="Notes" />
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" />
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Total" />
                                           <asp:BoundField DataField="Payment" HeaderText="Payment" />
                                        </Columns>
                                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                                        <EditRowStyle BackColor="#999999" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                                            ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>                               
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Table ID="tblTotal" runat="server" CellPadding="0" CellSpacing="0" BackColor="#edf7ff" ForeColor="Black" Width="900px">
    </asp:Table>
</form>
</body>
</html>
