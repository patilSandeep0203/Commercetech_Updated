<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="BuildTextLinks.aspx.cs" Inherits="BuildTextLinks" Title="E-Commerce Exchange - Partner Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 350px;" class="SilverBorder">
            <tr>
                <td align="center" colspan="3" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                    height: 25px">
                    <b><span class="MenuHeader">
                    Select Link Category</span></b></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 5px">
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 35%">
                    <asp:Label ID="lblLinks" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11px"
                        Text="Select Link Category"></asp:Label>&nbsp;
                </td>
                <td align="left" colspan="2">
                    &nbsp;<asp:DropDownList ID="lstLinks" runat="server" Height="18px" Font-Size="X-Small" Font-Names="Arial">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="center" colspan="3" style="height: 5px">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit"  Font-Name="Arial" ForeColor="#4e4e4e" Font-Size="X-Small"/>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3" style="height: 5px">
                </td>
            </tr>
        </table>
    <br />
    <asp:Table ID="tblTextLinks" runat="server">
    </asp:Table>
</asp:Content>

