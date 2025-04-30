<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="BuildBanners.aspx.cs" Inherits="BuildBanners" Title="E-Commerce Exchange - Partner Portal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 350px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                height: 25px">
                <span class="MenuHeader"><b>Select a Banner Group</b></span></td>
        </tr>
        <tr>
            <td colspan="3" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 35%">
                <asp:Label ID="lblBanner" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11px"
                    Text="Select Banner Group"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                &nbsp;<asp:DropDownList ID="lstBannerGroups" runat="server" Height="18px" Font-Size="X-Small" Font-Names="Arial">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" Font-Names="Arial" ForeColor="#4e4e4e" Font-Size="X-Small"/>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="height: 5px">
            </td>
        </tr>
    </table>
    <br />
    <asp:Table ID="tblBanners" runat="server">
    </asp:Table>

</asp:Content>

