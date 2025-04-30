<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResdCommPaymentPrint.aspx.cs" Inherits="Payroll_ResdCommPaymentPrint" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
</head>
<body style="font-size: 12pt">
<form id="Form1" runat=server>


    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <br />
    <asp:Panel ID="pnlEmployees" runat="server" Visible="False">
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 900px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Employees</span></b>
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
                                        CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                                        OnRowDataBound="grdEmployeeSummary_RowDataBound" ShowFooter = "True" Width="900px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="ConfirmNum" HeaderText="Confirmation" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="ConfirmDate" HeaderText="Date Paid" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Note" HeaderText="Note" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ItemStyle-Width=90/>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission" ItemStyle-Width=80/>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="Residual" HeaderText="Residual" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover" ItemStyle-Width=60/>
                                            <asp:BoundField HeaderText="Total" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment" ItemStyle-Width=60/>                                            
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
                                            <asp:BoundField DataField="ConfirmNum" HeaderText="Confirmation" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="ConfirmDate" HeaderText="Date Paid" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Note" HeaderText="Note" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ItemStyle-Width=90/>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission" ItemStyle-Width=80/>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="Residual" HeaderText="Residual" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover" ItemStyle-Width=60/>
                                            <asp:BoundField HeaderText="Total" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment" ItemStyle-Width=60/>  
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
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 900px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Partners</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlDDPartner" runat="server">
                        <table border="0" cellspacing="0" style="width: 900px">
                            <tr>
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size: small">Direct Deposit</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdSummaryDD" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="Vertical" 
                                        OnRowDataBound="grdSummaryDD_RowDataBound" ShowFooter="True" Width ="900px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="ConfirmNum" HeaderText="Confirmation" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="ConfirmDate" HeaderText="Date Paid" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Note" HeaderText="Note" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ItemStyle-Width=90/>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission" ItemStyle-Width=80/>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="Residual" HeaderText="Residual" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover" ItemStyle-Width=60/>
                                            <asp:BoundField HeaderText="Total" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment" ItemStyle-Width=60/>  
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
                                    <b><span style="color: #383838; font-family: Arial; font-size: small">Bill Pay</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdSummaryBP" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="Vertical" 
                                        OnRowDataBound="grdSummaryBP_RowDataBound" ShowFooter="True" Width ="900px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size=X-Small Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="ConfirmNum" HeaderText="Confirmation" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="ConfirmDate" HeaderText="Date Paid" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Note" HeaderText="Note" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Width=70/>
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ItemStyle-Width=90/>
                                            <asp:BoundField DataField="CommTotal" HeaderText="Commission" ItemStyle-Width=80/>
                                            <asp:BoundField DataField="BonusTotal" HeaderText="Bonus" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="RefTotal" HeaderText="Referral" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="Residual" HeaderText="Residual" ItemStyle-Width=60/>
                                            <asp:BoundField DataField="CarryoverBalance" HeaderText="Carryover" ItemStyle-Width=60/>
                                            <asp:BoundField HeaderText="Total" ItemStyle-Width=40/>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment" ItemStyle-Width=60/>  
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
            </tr>
        </table>
    </asp:Panel>
    <asp:Table ID="tblBPTotal" runat="server" CellPadding="0" CellSpacing="0" BackColor="#edf7ff" Width="900px" ForeColor="Black"></asp:Table>
    <br />
</form>
</body>
</html>