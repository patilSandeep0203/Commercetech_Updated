<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppList.aspx.cs" Inherits="AppList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<script language="javascript" type="text/javascript">
//<![CDATA[
var cot_loc0=(window.location.protocol == "https:")? "https://secure.comodo.net/trustlogo/javascript/cot.js" :
"http://www.trustlogo.com/trustlogo/javascript/cot.js";
document.writeln('<scr' + 'ipt language="JavaScript" src="'+cot_loc0+'" type="text\/javascript">' + '<\/scr' + 'ipt>');
//]]>
</script>
<head id="Head1" runat="server">
    <title>E-Commerce Exchange - Partner Portal</title>
    <link type="text/css" href="StyleAffWiz.css" rel="stylesheet" />
</head>
<a href="http://www.instantssl.com" id="comodoTL">SSL</a>
<script language="JavaScript" type="text/javascript">
COT("https://www.firstaffiliates.com/images/secure_site.gif", "SC2", "none");
</script>
<body>
    <center>
        <form id="form1" runat="server">
            <div>
                <br />
                <asp:Panel ID="pnlMainPage" runat="server">
                    <table border="0" width="500px" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="1" style="height: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1">
                                <table style="width: 90%;" border="0" cellpadding="0" cellspacing="0" class="DivGreen">
                                    <tr>
                                        <td align="center" valign="middle" style="height:30px">
                                            <strong><span style="font-size: Medium; color: #064787; font-family: Arial">Welcome
                                                to the E-Commerce Exchange Portal.</span></strong>                                                
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblError" runat="server" BackColor="Red" Font-Size="Medium" ForeColor="White"
                                    Visible="False"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="height: 20px" align="center">
                                <asp:Panel ID="pnlGrid" runat="server" Visible="True" Width="100%">
                                    <asp:Label ID="lblWelcome" runat="server" Font-Bold="True" Font-Names="Arial" 
                                    Text="Please click on the section name you want to access."></asp:Label><br />
                                    <br />
                                    <asp:DataGrid ID="grdApps" runat="server" CellPadding="4" AutoGenerateColumns="False"
                                        OnItemCommand="grdApps_ItemCommand" ForeColor="#333333" GridLines="Both">
                                        <SelectedItemStyle Font-Bold="True" ForeColor="#333333" BackColor="#D1DDF1"></SelectedItemStyle>
                                        <AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
                                        <ItemStyle CssClass="Labels DivGreen" Font-Bold="true"></ItemStyle>
                                        <HeaderStyle Font-Bold="True" Font-Names="Arial" ForeColor="White" BackColor="#507CD1">
                                        </HeaderStyle>
                                        <FooterStyle ForeColor="White" Font-Names="Arial" BackColor="#507CD1" Font-Bold="True">
                                        </FooterStyle>
                                        <Columns>
                                            <asp:TemplateColumn SortExpression="sAppName" HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="One" Font-Bold="true" ID="lnkApp" runat="server" UserID='<%# DataBinder.Eval(Container.DataItem, "iUserID") %>'
                                                        AppID='<%# DataBinder.Eval(Container.DataItem, "iAppID") %>' Text='<%# DataBinder.Eval(Container.DataItem, "sAppName") %>'
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "sURL") %>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="sDesc" HeaderText="Description"></asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#2461BF"></PagerStyle>
                                        <EditItemStyle BackColor="#2461BF" />
                                    </asp:DataGrid><br />
                                    <asp:HyperLink ID="lnkLogout" CssClass="One" runat="server" Font-Bold="True" Font-Names="Arial"
                                        NavigateUrl="logout.aspx">Logout</asp:HyperLink>
                                    <br />                                    
                                    <br />
                                    <asp:Panel ID="pnlLogoutMsg" runat="server" Width="70%" CssClass="DivHelp">                                        
                                            <asp:Image ID="imgNote" runat="server" ImageUrl="~/Images/exclamation.gif" />&nbsp;
                                            <strong><span class="Labels">To Log Out</span></strong><br />
                                        <span class="LabelsRed"><strong>To ensure
                                            Log Out, you MUST quit/close the browser.</strong></span>
                                            <br />                                        
                                    </asp:Panel>
                                </asp:Panel>
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </form>
    </center>
</body>
</html>
