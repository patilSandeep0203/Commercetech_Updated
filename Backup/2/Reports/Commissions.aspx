<%@ Page Language="C#" MasterPageFile="~/Reports/Employee.master" AutoEventWireup="true" CodeFile="Commissions.aspx.cs" Inherits="Commissions" Title="E-Commerce Exchange - Partner Portal" Theme="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table cellpadding="0" cellspacing="0" border="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                <b><span class="MenuHeader">Commission Reports</span></b></td>
        </tr>
        <tr>
            <td style="height: 5px" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="8pt" Text="Select Month"></asp:Label>&nbsp;</td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstMonth" runat="server">
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
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="center" style="height: 5px" colspan="3">
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
            <td colspan="3" align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="height:5px">             </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <br />
    <asp:Panel ID="pnlConfirmation" runat="server" BackColor="Ivory" BorderColor="Orange"
        BorderStyle="Solid" BorderWidth="1px" Height="50px" Visible="False" Width="500px">
        <br />
        <asp:Label ID="lblConfirmation" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="Smaller"></asp:Label></asp:Panel>
    <br />
    <asp:Table ID="tblCommissions" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
    <br />
    <asp:Table ID="tblBonus" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
    <asp:Table ID="tblAdjustments" runat="server" CellPadding="2" CellSpacing="0" GridLines="Vertical">
    </asp:Table>
    <br />
    <table border="0" cellpadding="2" cellspacing="2" style="width: 700px;" class="DivGreen">
        <tr>
            <td align="left">
                <strong><span class="Labels">Criteria to have account Funded</span></strong></td>
        </tr>
        <tr>
            <td align="left">
                <span class="Labels">1. Full payment received
                    including lease funding received prior to cutoff date for all POS sales, application
                    fees and other setup costs.</span></td>
        </tr>
        <tr>
            <td align="left">
                <span class="Labels">2. All merchant numbers
                    including non bankcard that the customer wants have been issued and programmed in
                    to equipment or software.</span>
                <br />
                <span class="Labels"><b>Note</b>: All customers receive
                    Discover but you have to charge the $25 application fee to receive a commission.
                </span>
            </td>
        </tr>
        <tr>
            <td align="left">
                <span class="Labels">3. Startup letter
                    and any labels for equipment we did not have to ship has been e-mailed, mailed or
                    faxed.</span></td>
        </tr>
        <tr>
            <td align="left">
                <span class="Labels">4. The customer has
                    received their equipment and any additional services that are part of the sale have
                    been activated and tested upon it.</span></td>
        </tr>
        <tr>
            <td align="left">
                <span class="Labels">5. If a payment gateway
                    is sold, the merchant has completed the login of the gateway and all requested card
                    types are active.</span>
            </td>
        </tr>
        <tr>
            <td align="left">
                <span class="Labels">6. Lastly, the merchant
                    is happy and expects nothing else to be completed.</span></td>
        </tr>
    </table>
</asp:Content>

