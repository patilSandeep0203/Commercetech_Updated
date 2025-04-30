<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login"
    Title="E-Commerce Exchange - Partner Portal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
</head>
<body>
 <center>
    <form id="form1" runat="server">   
        <div >
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                ForeColor="Green" Text="Logging In....Please Wait...."></asp:Label>
                </div>
    </form>    
 </center>
</body>
</html>
