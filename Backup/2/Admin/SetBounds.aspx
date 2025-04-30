<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetBounds.aspx.cs" Inherits="SetBounds" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <link href="../PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<a href="http://www.instantssl.com" id="comodoTL">SSL</a>
<script language="JavaScript" type="text/javascript">
COT("https://www.firstaffiliates.com/images/secure_site.gif", "SC2", "none");
</script>
  <body>   
  <center>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SMAdmin" runat="server">
    </asp:ScriptManager> 
   <cc1:TabContainer runat="server" ID="Tabs"> 
        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Processor Minuimums"> 
            <ContentTemplate>

                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
 
                <table align="center"  cellpadding="0" cellspacing="0" border="0" style="width: 800px;" class="DivGreen">
                    <tr>
                        <td colspan="8" align="center" style="height: 25px; background-image: url(../Images/topMain.gif)">
                            <b><span class="MenuHeader">Set Processor Minimums</span></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="8">
                            <asp:Label ID="lblSelectPackage" runat="server" Text="Select Processing Structure to Edit"></asp:Label>
                            <asp:DropDownList ID="lstProcessor" runat="server" TabIndex="1" AutoPostBack="True"
                                OnSelectedIndexChanged="lstProcessor_SelectedIndexChanged">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="8" style="height: 15px">
                            <span class="LabelsRed"><strong>Enter 0 if there
                                is no minimum. Leave blank if the fee/rate does not exist.<br />
                                Minimums apply to setting rates and packages. <br /> * On Set Rates, field can be left blank but minimum still applies if entered </strong></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label ID="lblProcessorHeader" runat="server" Text="Processor" Font-Bold="True"></asp:Label>
                            &nbsp;<asp:Label ID="lblProcessor" runat="server" Font-Bold="True"></asp:Label></td>
                        <td align="center" colspan="4">
                            <asp:Label ID="lblLastModifiedHeader" runat="server" Text="Last Modified" Font-Bold="True"></asp:Label>
                            &nbsp;<asp:Label ID="lblLastModified" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="8">
                            <asp:Panel ID="pnlMerchantRates" runat="server" Width="100%" Visible="True">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDRQP" runat="server" Text="DR Qual Pres"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDRQP" runat="server" onBlur="Javascript:fillDebit(); fillMidQual(); fillNonQual(); return false;"
                                                Width="40px" TabIndex="2" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span></td>
                                        <td align="right">
                                            <asp:Label ID="lblCustomerService" runat="server" Text="Customer Service"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtCustomerService" runat="server" Width="40px" TabIndex="7" MaxLength="5"></asp:TextBox></td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblTransFee" runat="server" Text="Trans Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtTransFee" runat="server" Width="40px" TabIndex="12" MaxLength="5"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblAnnualFee" runat="server" Text="Annual  Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:DropDownList ID="lstAnnualFee" runat="server" TabIndex="17">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDRQNP" runat="server" Text="DR Qual NP"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDRQNP" onBlur="Javascript:fillMidQual(); fillNonQual(); fillDebit(); return false;"
                                                runat="server" Width="40px" TabIndex="3" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span></td>
                                            
                                                                                    <td align="right">
                                            <asp:Label ID="lblMonMin" runat="server" Text="Monthly Minimum"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtMonMin" runat="server" Width="40px" TabIndex="9" MaxLength="5"></asp:TextBox></td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblBatchHeader" runat="server" Text="Batch Header"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtBatchHeader" runat="server" Width="40px" TabIndex="13" MaxLength="5"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblChargebackFee" runat="server" Text="Chargeback Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtChargebackFee" runat="server" Width="40px" TabIndex="18" MaxLength="5"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDRMQ" runat="server" Text="DR Mid Qual"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDRMQ" runat="server" Width="40px" TabIndex="4" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span></td>
                                            <td align="right">
                                                <asp:Label ID="lblInternetStmt" runat="server" Text="Online Reporting"></asp:Label>
                                                 <span class="LabelsSmall" style ="color: Red">*</span>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtInternetStmt" runat="server" MaxLength="5" TabIndex="8" Width="40px"></asp:TextBox></td>
                                            

                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblNBCTransFee" runat="server" Text="Non Bankcard Trans Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtNBCTransFee" runat="server" Width="40px" TabIndex="14" MaxLength="5"></asp:TextBox></td><td align="right">
                                            <asp:Label ID="lblRetrievalFee" runat="server" Text="Retrieval Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtRetrievalFee" runat="server" Width="40px" TabIndex="19" MaxLength="5"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDRNQ" runat="server" Text="DR Non Qual"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDRNQ" runat="server" Width="40px" TabIndex="5" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span></td>
                                        <td align="right">
                                            <asp:Label ID="lblWirelessAccess" runat="server" Text="Wireless Access"></asp:Label>
                                           <span class="LabelsSmall" style ="color: Red">*</span>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtWirelessAccess" runat="server" Width="40px" TabIndex="10" MaxLength="5"></asp:TextBox></td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblAVS" runat="server" Text="AVS"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtAVS" runat="server" Width="40px" TabIndex="16" MaxLength="5"></asp:TextBox></td>

                                        <td align="right">
                                            <asp:Label ID="lblApplicationFee" runat="server" Text="Application Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtApplicationFee" runat="server" Width="40px" TabIndex="20" MaxLength="4"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblDRQD" runat="server" Text="DR Qual Debit"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDRQD" runat="server" Width="40px" TabIndex="6" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span></td>
                                        <td align="right">
                                            <asp:Label ID="lblWirelessTransFee" runat="server" Text="Wireless Trans Fee"></asp:Label>
                                            <span class="LabelsSmall" style ="color: Red">*</span>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtWirelessTransFee" runat="server" Width="40px" TabIndex="11" MaxLength="5"></asp:TextBox>
                                         
                                         </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td align="left">
                                        </td>
                                                                                <td align="right">
                                            <asp:Label ID="lblVoiceAuth" runat="server" Text="Voice Auth"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtVoiceAuth" runat="server" Width="40px" TabIndex="15" MaxLength="5"></asp:TextBox></td>
                                        

                                        <td align="right">
                                            <asp:Label ID="lblSetupFee" runat="server" Text="Setup Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtSetupFee" runat="server" Width="40px" TabIndex="21" MaxLength="4"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="1" style="width: 102px; height: 15px">
                                        </td>
                                        <td align="right" colspan="1" style="width: 70px; height: 15px">
                                        </td>
                                        <td align="right" colspan="10" style="height: 15px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="1" scope="Height:15px" style="width: 102px">
                                        </td>
                                        <td align="center" colspan="1" scope="Height:15px" style="width: 70px">
                                        </td>
                                        <td align="center" colspan="10" scope="Height:15px">
                                            <asp:Panel ID="pnlAdditionalServices" runat="server" Visible="true" Width="100%">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <asp:Label ID="lblAddlServices" runat="server" Font-Bold="True" Text="Additional Services"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <asp:Panel ID="pnlOnlineDebit" runat="server" Visible="true" Width="100%">
                                                                <table border="0" cellpadding="0" cellspacing="2">
                                                                    <tr>
                                                                        <td align="center" colspan="4">
                                                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Online Debit"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblDebitMonFee" runat="server" Text="Debit Mon Fee"></asp:Label></td>
                                                                        <td align="left">
                                                                            <span class="LabelsSmall">$</span>
                                                                            <asp:TextBox ID="txtDebitMonFee" runat="server" MaxLength="5" TabIndex="22" Width="40px"></asp:TextBox></td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblDebitTransFee" runat="server" Text="Debit Trans Fee"></asp:Label></td>
                                                                        <td align="left">
                                                                            <span class="LabelsSmall">$</span>
                                                                            <asp:TextBox ID="txtDebitTransFee" runat="server" MaxLength="5" TabIndex="23" Width="40px"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="4" style="height: 15px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <asp:Panel ID="pnlEBT" runat="server" Visible="true" Width="100%">
                                                                <table border="0" cellpadding="0" cellspacing="2">
                                                                    <tr>
                                                                        <td align="center" colspan="4">
                                                                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="EBT"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="Label19" runat="server" Text="Monthly Fee"></asp:Label></td>
                                                                        <td align="left">
                                                                            <span class="LabelsSmall">$</span>
                                                                            <asp:TextBox ID="txtEBTMonFee" runat="server" MaxLength="5" TabIndex="24" Width="40px"></asp:TextBox></td>
                                                                        <td align="right">
                                                                            <asp:Label ID="Label21" runat="server" Text="Trans Fee"></asp:Label></td>
                                                                        <td align="left">
                                                                            <span class="LabelsSmall">$</span>
                                                                            <asp:TextBox ID="txtEBTTransFee" runat="server" MaxLength="5" TabIndex="25" Width="40px"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="1" style="width: 102px">
                                        </td>
                                        <td align="center" colspan="1" style="width: 70px">
                                        </td>
                                        <td align="center" colspan="10">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                        <td style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 40%;
                            height: 30px" align="center" colspan="6">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" TabIndex="26" OnClick="btnReset_Click"
                                CausesValidation="False" />
                            &nbsp;
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                                TabIndex="27" />
                        </td>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                    </tr>
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>        
      </cc1:TabPanel>
      <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Gateway Minuimums"> 
            <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="lblErrorGateway" CssClass="LabelsError"  runat="server" Visible="False" ForeColor="Black" Font-Size="Medium"
                    BackColor="LemonChiffon"></asp:Label>
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 800px;" class="DivGreen">
                    <tr>
                        <td colspan="8" align="center" style="height: 25px; background-image: url(../Images/topMain.gif)">
                            <b><span class="MenuHeader">Set Gateway Minimums</span></b></td>
                    </tr>
                    <tr>
                        <td align="left" colspan="8">
                            &nbsp;<asp:Panel ID="pnlGatewayRates" runat="server" Visible="True" Width="100%">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblGateway" runat="server" Font-Bold="True" Text="Gateway"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:DropDownList ID="lstGatewayNames" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstGatewayNames_SelectedIndexChanged"
                                                TabIndex="28">
                                            </asp:DropDownList></td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblGatewaySetupFee" runat="server" Text="Setup Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtGWSetupFee" runat="server" MaxLength="4" TabIndex="29" Width="40px"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblGatewayMonthlyFee" runat="server" Text="Monthly Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtGWMonthlyFee" runat="server" MaxLength="5" TabIndex="30" Width="40px"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblGatewayTransFee" runat="server" Text="Trans Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtGWTransFee" runat="server" MaxLength="5" TabIndex="31" Width="40px"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                        <td style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 40%;
                            height: 30px" align="center" colspan="6">
                            <asp:Button ID="btnResetGateway" runat="server" Text="Reset" TabIndex="32" OnClick="btnResetGateway_Click"
                                CausesValidation="False" />
                            &nbsp; &nbsp;<asp:Button ID="btnUpdateGateway" runat="server" Text="Update" OnClick="btnUpdateGateway_Click"
                                TabIndex="33" />
                        </td>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                    </tr>
                </table>                
                    </ContentTemplate>
                </asp:UpdatePanel>
        
        
            </ContentTemplate>
   </cc1:TabPanel>
   
   <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Check Service Minuimums"> 
            <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="lblErrorCheckService" CssClass="LabelsError"  runat="server" Visible="False" ForeColor="Black" Font-Size="Medium"
                    BackColor="LemonChiffon"></asp:Label>
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 800px;" class="DivGreen">
                    <tr>
                        <td colspan="8" align="center" style="height: 25px; background-image: url(../Images/topMain.gif)">
                            <b><span class="MenuHeader">Set Check Service Minimums</span></b></td>
                    </tr>
                    <tr>
                        <td align="left" colspan="8">
                            &nbsp;<asp:Panel ID="pnlCheckService" runat="server" Visible="True" Width="100%">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCheckService" runat="server" Font-Bold="True" Text="Check Service"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:DropDownList ID="lstCheckService" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstCheckService_SelectedIndexChanged"
                                                TabIndex="28">
                                            </asp:DropDownList></td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCheckServiceDiscRate" runat="server" Text="Discount Rate"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtCheckServiceDiscRate" runat="server" MaxLength="4" TabIndex="29" Width="40px"></asp:TextBox><span class="LabelsSmall">$</span></td>
                                        <td align="right">
                                            <asp:Label ID="lblCheckServiceMonFee" runat="server" Text="Monthly Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtCheckServiceMonFee" runat="server" MaxLength="5" TabIndex="30" Width="40px"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblCheckServiceMonMin" runat="server" Text="Monthly Minimum"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtCheckServiceMonMin" runat="server" MaxLength="5" TabIndex="31" Width="40px"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblCheckServiceTransFee" runat="server" Text="Transaction Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtCheckServiceTransFee" runat="server" MaxLength="5" TabIndex="31" Width="40px"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                        <td style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 40%;
                            height: 30px" align="center" colspan="6">
                            <asp:Button ID="Button1" runat="server" Text="Reset" TabIndex="32" OnClick="btnResetCheckService_Click"
                                CausesValidation="False" />
                            &nbsp; &nbsp;<asp:Button ID="Button2" runat="server" Text="Update" OnClick="btnUpdateCheckService_Click"
                                TabIndex="33" />
                        </td>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                    </tr>
                </table>                
                    </ContentTemplate>
                </asp:UpdatePanel>
        
        
            </ContentTemplate>
   </cc1:TabPanel>
   
   <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Gift Loyalty Minuimums"> 
            <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="lblErrorGiftCard" CssClass="LabelsError"  runat="server" Visible="False" ForeColor="Black" Font-Size="Medium"
                    BackColor="LemonChiffon"></asp:Label>
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 800px;" class="DivGreen">
                    <tr>
                        <td colspan="8" align="center" style="height: 25px; background-image: url(../Images/topMain.gif)">
                            <b><span class="MenuHeader">Set Gift Loyalty Minimums</span></b></td>
                    </tr>
                    <tr>
                        <td align="left" colspan="8">
                            &nbsp;<asp:Panel ID="pnlGiftCard" runat="server" Visible="True" Width="100%">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblGiftCard" runat="server" Font-Bold="True" Text="Gift Loyalty"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:DropDownList ID="lstGiftCard" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstGiftCard_SelectedIndexChanged"
                                                TabIndex="28">
                                            </asp:DropDownList></td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblGifrCardMonFee" runat="server" Text="Monthly Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtGifrCardMonFee" runat="server" MaxLength="5" TabIndex="29" Width="40px"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblGifrCardTransFee" runat="server" Text="Transaction Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtGifrCardTransFee" runat="server" MaxLength="5" TabIndex="30" Width="40px"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                        <td style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 40%;
                            height: 30px" align="center" colspan="6">
                            <asp:Button ID="Button3" runat="server" Text="Reset" TabIndex="32" OnClick="btnResetCheckService_Click"
                                CausesValidation="False" />
                            &nbsp; &nbsp;<asp:Button ID="Button4" runat="server" Text="Update" OnClick="btnUpdateGiftCard_Click"
                                TabIndex="33" />
                        </td>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                            width: 30%; height: 30px">
                            &nbsp;</td>
                    </tr>
                </table>                
                    </ContentTemplate>
                </asp:UpdatePanel>
        
        
            </ContentTemplate>
   </cc1:TabPanel>
    </cc1:TabContainer> 
    </form>
    </center>
   </body>
</html>
    



