<%@ Page Language="c#" AutoEventWireup="false" CodeFile="UploadLogo.aspx.cs" Inherits="netimageupload.uploadfiles"
    Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<script language="javascript" type="text/javascript">
//<![CDATA[
var cot_loc0=(window.location.protocol == "https:")? "https://secure.comodo.net/trustlogo/javascript/cot.js" :
"http://www.trustlogo.com/trustlogo/javascript/cot.js";
document.writeln('<scr' + 'ipt language="JavaScript" src="'+cot_loc0+'" type="text\/javascript">' + '<\/scr' + 'ipt>');
//]]>
</script>
<head>
    <title>E-Commerce Exchange - Partner Portal</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<a href="http://www.instantssl.com" id="comodoTL">SSL</a>

<script language="JavaScript" type="text/javascript">
COT("https://www.firstaffiliates.com/images/secure_site.gif", "SC2", "none");
</script>

<body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
        <table style="width: 600px" class="SilverBorder" align="center">
            <tr>
                <td align="center" style="background-image: url(Images/topMain.gif); height: 25px" colspan="2">
                    <span class="MenuHeader"><b>Upload Images</b></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">                    
                    <asp:Label CssClass="LabelsError" ID="lblOutput" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblFileName" Text="File Name and Location" runat="server"></asp:Label>
                </td>
                <td>
                    <input id="filUpload" type="file" name="filUpload" runat="server"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" Width="90px" Height="25px"></asp:Button>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Image ID="imgPicture" runat="server" Height="100px" ImageUrl="Images/ProductImages/NA.gif"></asp:Image>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>