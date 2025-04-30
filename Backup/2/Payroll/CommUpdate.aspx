<%@ Page Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true" CodeFile="CommUpdate.aspx.cs"
    Inherits="CommUpdate" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <asp:Label ID="lblMonthBonus" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" Visible="False"></asp:Label><br />
    <asp:Panel ID="pnlAddBonus" runat="server">
        <table cellpadding="1" cellspacing="1" border="0" style="border-right: silver 1px solid;
        border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
        background-color: #edf7ff; width:700px">
        <tr>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Bonuses/Discover Commissions For the Month</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Reason</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Rep Name</span></b>&nbsp;</td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">RepTotal</span></b>
            </td>            
        </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="lstBonus" runat="server">
                        <asp:ListItem>Additions/Deductions</asp:ListItem>
                        <asp:ListItem>Miscellaneous Bonus</asp:ListItem>
                    </asp:DropDownList></td>
                <td>
                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                <td>
                    <asp:DropDownList ID="lstRepListBonus" runat="server">
                    </asp:DropDownList></td>
                <td>
                    <asp:TextBox ID="txtRepTotal" runat="server" Width="40px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" colspan="5">
                    <br />
                <asp:Button ID="btnAddBonus" runat="server" Text="Add Bonus" OnClick="btnAddBonus_Click" /></td>                
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlUpdateComm" runat="server">
    <table cellpadding="1" cellspacing="1" border="0" style="width:700px" class="DivGreen">
        <tr>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">DBA</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Merchant ID</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Rep Name</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Rep Num</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Referral Partner DBA</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Product</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Qty</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Price</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">COG</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Comm</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Funded Value</span></b>
            </td>
             <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Referral Paid</span></b>
            </td>
            <td align="center" style="background-color: #5d7b9d">
            <b><span class="MenuHeader">Rep Total</span></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDBA" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblMerchantID" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblRepName" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblRepNum" runat="server"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="lstLegalName" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblProduct" runat="server"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" Width="20px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtPrice" runat="server" Width="20px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtCOG" runat="server" Width="20px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="lstComm" runat="server" Enabled="True">
                    <asp:ListItem>0</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>60</asp:ListItem>
                    <asp:ListItem>65</asp:ListItem>
                    <asp:ListItem>70</asp:ListItem>
                    <asp:ListItem>75</asp:ListItem>
                    <asp:ListItem>80</asp:ListItem>
                    <asp:ListItem>85</asp:ListItem>
                    <asp:ListItem>90</asp:ListItem>
                    <asp:ListItem>95</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="lstFundedValue" runat="server">
                    <asp:ListItem>0.00</asp:ListItem>
                    <asp:ListItem>0.25</asp:ListItem>
                    <asp:ListItem>0.50</asp:ListItem>
                    <asp:ListItem>1.00</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtRefPaid" runat="server" Enabled="true"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblRepTotal" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="12" align="center">
                <br />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" /></td>
        </tr>
    </table>
    </asp:Panel>    
</asp:Content>
