<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReferralsPrint.aspx.cs" Inherits="ReferralsPrint" Theme="Admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
    <link href="../PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
     <center>
    <div>    
        <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
        <asp:Table ID="tblPrintReferrals" runat="server" Width="700px" CellPadding="3" CellSpacing="0" GridLines="Vertical">
        </asp:Table>
    </div>
    </center>
    </form>
</body>
</html>
