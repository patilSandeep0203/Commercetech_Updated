<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResidualsPrint.aspx.cs" Inherits="Residuals_ResidualsPrint" Theme="Admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
</head>
<body>
<center>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
        <asp:Table ID="tblPrintResd" runat="server" Width="760px" CellPadding="3" CellSpacing="0" GridLines="Both">
        </asp:Table>
    </div>
    </form>
    </center>
</body>
</html>
