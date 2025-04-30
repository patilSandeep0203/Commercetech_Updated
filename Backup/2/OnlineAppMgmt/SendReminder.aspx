<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendReminder.aspx.cs" Inherits="SendReminder"
    Theme="AppTheme" ValidateRequest="false" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
    <link href="../PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<body>
<center>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <br />
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
            <table width="700px" border="0" cellspacing="0" class="DivGreen">
                <tr>
                    <td align="center" style="height: 20px; background-image: url('/PartnerPortal/Images/topMain.gif');" colspan="2">
                        <b><span class="MenuHeader">Send Reminder</span></b></td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lblTo" Font-Bold="true" runat="server" Text="To"></asp:Label>&nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txtTo" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lblCC" Font-Bold="true" runat="server" Text="CC"></asp:Label>&nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txtCC" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lblFrom" Font-Bold="true" runat="server" Text="From"></asp:Label>&nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txtFrom" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right" style="width: 30%">
                        <asp:Label ID="lblSubject" Font-Bold="true" runat="server" Text="Subject"></asp:Label>&nbsp;</td>
                    <td align="left" style="width: 70%">
                        <asp:TextBox ID="txtSubject" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">                        
                        <asp:TextBox ID="txtBody" runat="server" Columns="80" Rows="18" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:15px">
                    </td>
                </tr>
                <tr>
                    <td style="height: 20px; background-image: url('/PartnerPortal/Images/topMain.gif')" align="center"
                        colspan="2">
                        <asp:Button ID="btnSendEmail" runat="server" OnClick="btnSendEmail_Click" Text="Send Reminder" />
                        <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" CausesValidation="False" UseSubmitBehavior="False" /></td>
                </tr>
            </table>
        </div>
    </form>
    </center>
</body>
</html>
