<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetRates.aspx.cs" Theme="AppTheme"
    Inherits="SetRates" %>

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

    <script type="text/javascript" src="SetRates.js" language="javascript">
    </script>

    <link href="../PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<a href="http://www.instantssl.com" id="comodoTL">SSL</a>

<script language="JavaScript" type="text/javascript">
COT("https://www.firstaffiliates.com/images/secure_site.gif", "SC2", "none");
</script>

<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlAddlServices"
                        Collapsed="false" CollapsedImage="~/images/expand_blue.jpg" CollapsedText="(Show)"
                        ExpandControlID="pnlAddlServices" ExpandedImage="~/images/collapse_blue.jpg"
                        ExpandedText="(Hide)" ImageControlID="imgShowDetails" SuppressPostBack="true"
                        TargetControlID="pnlNews" TextLabelID="lblShowDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
                        Visible="False"></asp:Label><br />
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 700px" class="DivGreen">
                        <tr>
                            <td colspan="3" align="center" style="height: 25px; background-image: url(../Images/topMain.gif)">
                                <b><span class="MenuHeader">Set Rates for App ID: &nbsp;
                                    <asp:Label ID="lblAppId" runat="server" EnableTheming="true" CssClass="MenuHeader"></asp:Label>&nbsp;
                                    <asp:Label ID="lblContact" runat="server" CssClass="MenuHeader"></asp:Label>
                                    <!--  <asp:HyperLink ID="lnkBack" runat="server" ForeColor="Red">Back to Edit</asp:HyperLink> 
                                 <asp:LinkButton ID="lnkBackEdit" Font-Bold=true runat="server" ForeColor="Red" Text="Back to Edit" OnClick="btnBack_Click"
                                CausesValidation="False" />-->
                                </span></b>
                            </td>
                        </tr>
                        <tr height="10px">
                        </tr>
                        <tr>
                            <td align="left" height="20" valign="bottom" colspan="3">
                                <span class="LabelsSmall"><b>Last Modified Date: </b></span>
                                <asp:Label ID="lblLastModifiedDate" runat="server" Font-Bold="true"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right" width="30%">
                                <asp:Label ID="lblSelectPackage" runat="server" Text="Select Package"></asp:Label>&nbsp;</td>
                            <td align="left" width="33%">
                                <asp:DropDownList ID="lstPackageNames" runat="server">
                                </asp:DropDownList></td>
                            <td align="center">
                                <asp:LinkButton ID="btnApplyPackage" Font-Names="Arial" Font-Size="Small" Font-Bold="True"
                                    runat="server" Text="Apply Package" TabIndex="52" OnClick="btnApplyPackage_Click" /><br />
                                <span class="LabelsRedSmall">Note: This will overwrite all fees below</span></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Panel ID="pnlApplyPackage" Visible="false" runat="server" BackColor="#FFC0C0"
                                    BorderColor="Salmon" BorderStyle="Double" Width="45%">
                                    <center>
                                        <span class="LabelsSmall">Are you sure you want to overwrite the fees with this Package?</span><br />
                                        <asp:Button ID="btnApplyYes" runat="server" OnClick="btnApplyPkgYes_Click" Text="Yes" />
                                        <asp:Button ID="btnApplyNo" runat="server" OnClick="btnApplyPkgNo_Click" Text="No" />
                                    </center>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr height="10">
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                    <tr>
                                        <td colspan="8" align="center" style="vertical-align: middle; width: 100%; height: 20px;
                                            background-image: url(../Images/topMain.gif)">
                                            <b><span class="MenuHeader">Merchant Account</span></b>
                                        </td>
                                    </tr>
                                    <tr height="10px">
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            &nbsp; &nbsp; &nbsp;
                                            <asp:CheckBox ID="chkMA" runat="server" AutoPostBack="True" Font-Bold="True" OnCheckedChanged="chkMA_CheckedChanged"
                                                Text="Merchant Account" /></td>
                                    </tr>
                                    <asp:Panel ID="pnlMerchantRates" runat="server" Width="100%" Visible="False">
                                        <tr height="10px">
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblSelectProcessor" runat="server" Text="Select Processor"></asp:Label>&nbsp;</td>
                                            <td align="left" colspan="2">
                                                <asp:DropDownList ID="lstProcessorNames" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstProcessorNames_SelectedIndexChanged"
                                                    TabIndex="1">
                                                </asp:DropDownList></td>
                                            <td align="left" colspan="4">
                                                <asp:CheckBox ID="chkApplyInterchange" runat="server" AutoPostBack="True" OnCheckedChanged="chkApplyInterchange_CheckedChanged"
                                                    Text="Apply Interchange Rates" />
                                                <asp:CheckBox ID="chkBillAssessment" Visible="false" runat="server" Text="Billing Assessments" />
                                            </td>
                                        </tr>
                                        <tr height="10px">
                                        </tr>
                                        <tr height="15px">
                                            <td>
                                            </td>
                                            <td align="left" colspan="4">
                                                <asp:RadioButton ID="rdbCP" runat="server" AutoPostBack="True" GroupName="CPCNP"
                                                    Text="Card Present" TabIndex="2" OnCheckedChanged="rdbCP_CheckedChanged" />
                                                &nbsp;<asp:RadioButton ID="rdbCNP" runat="server" AutoPostBack="True" GroupName="CPCNP"
                                                    Text="Card Not Present" OnCheckedChanged="rdbCP_CheckedChanged" TabIndex="3" /></td>
                                        </tr>
                                        <tr height="10px">
                                        </tr>
                                        <tr style="height: 24px;">
                                            <td align="right">
                                                <span class="LabelsSmall">DR Qual Debit</span>&nbsp;</td>
                                            <td align="left">
                                                <asp:TextBox ID="txtDRQD" runat="server" Width="40px" TabIndex="8" MaxLength="6"></asp:TextBox>
                                                <span class="LabelsSmall">%</span>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtDRQD"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>

                                            <td align="right">
                                                <span class="LabelsSmall">Customer Service $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCustomerService" runat="server" Width="40px" TabIndex="9" MaxLength="5"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers"
                                                    ValidChars="." TargetControlID="txtCustomerService" />
                                            </td>
                                            <td align="right">
                                                <span class="LabelsSmall">Transaction fee $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTransFee" runat="server" MaxLength="5" AutoPostBack="true" OnTextChanged="txtTransFee_TextChanged"
                                                    TabIndex="14" Width="40px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtTransFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            <td align="right">
                                                <span class="LabelsSmall">Annual Fee $&nbsp;</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"></span>
                                                <asp:DropDownList ID="lstAnnualFee" runat="server" TabIndex="19" Width="60px">
                                                </asp:DropDownList></td>
                                        </tr>
                                       
                                        <tr style="height: 24px;">
                                        <td align="right">
                                                <span class="LabelsSmall">DR Qual Pres</span>&nbsp;</td>
                                          <td align="left">
                                                <asp:TextBox ID="txtDRQP" runat="server" AutoPostBack="true" OnTextChanged="txtDRQP_TextChanged"
                                                    Width="40px" TabIndex="4" MaxLength="6"></asp:TextBox>
                                                <span class="LabelsSmall">%</span><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                                    runat="server" TargetControlID="txtDRQP" FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                           
                                            <td align="right">
                                                <span class="LabelsSmall">Monthly Minimum $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtMonMin" runat="server" Width="40px" TabIndex="10" MaxLength="5"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMonMin"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            <td align="right">
                                                <span class="LabelsSmall">Batch Header $&nbsp;</span></td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txtAVS"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtBatchHeader" runat="server" Width="40px" TabIndex="15" MaxLength="5"></asp:TextBox></td>
                                            <td align="right">
                                                <span class="LabelsSmall">Chargeback Fee $&nbsp;</span></td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtRetrievalFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtChargebackFee" runat="server" Width="40px" TabIndex="20" MaxLength="5"
                                                    AutoPostBack="true" OnTextChanged="txtChargebackFee_TextChanged"></asp:TextBox></td>
                                        </tr>
                                        <tr style="height: 24px;">
                                        <td align="right">
                                                <span class="LabelsSmall">DR Qual NP</span>&nbsp;</td>
                                                 <td align="left">
                                                <asp:TextBox ID="txtDRQNP" AutoPostBack="true" OnTextChanged="txtDRQP_TextChanged"
                                                    runat="server" Width="40px" TabIndex="5" MaxLength="6"></asp:TextBox>
                                                <span class="LabelsSmall">%</span>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtDRQNP"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                                
                                            <td align="right">
                                                <span class="LabelsSmall">Online Reporting $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtInternetStmt" runat="server" MaxLength="5" AutoPostBack="true"
                                                    OnTextChanged="txtTransFee_TextChanged" TabIndex="11" Width="40px"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txtTransFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            <td align="right">
                                                <span class="LabelsSmall">Non Bankcard Trans $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtNBCTransFee" runat="server" Width="40px" TabIndex="16" MaxLength="5"></asp:TextBox></td>
                                            <td align="right">
                                                <span class="LabelsSmall">Retrieval Fee $&nbsp;</span></td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtVoiceAuth"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtRetrievalFee" runat="server" Width="40px" TabIndex="21" MaxLength="5"></asp:TextBox></td>
                                        </tr>
                                        <tr style="height: 24px;">
                                        <td align="right">
                                                <span class="LabelsSmall">DR Mid Qual</span>&nbsp;</td>
                                             <td align="left">
                                                <asp:TextBox ID="txtDRMQ" runat="server" Width="40px" TabIndex="6" MaxLength="6"></asp:TextBox>
                                                <span class="LabelsSmall">%</span>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtDRMQ"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                        <td align="right">
                                                <span class="LabelsSmall">Regulatory Fee&nbsp;</span></td>
                                            <td align="left" style="height: 24px; width: 80px" colspan="1">
                                                <asp:TextBox ID="txtComplianceFee" runat="server" Width="40px" TabIndex="24" MaxLength="6"></asp:TextBox></td>
                                            
                                            <td align="right">
                                                <span class="LabelsSmall">AVS $&nbsp;</span></td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtNBCTransFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtAVS" runat="server" Width="40px" TabIndex="17" MaxLength="5"></asp:TextBox></td>
                                            <td align="right">
                                                <span class="LabelsSmall">Rolling Reserve</span>&nbsp;</td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtChargebackFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtRolling" runat="server" MaxLength="5" TabIndex="22" Width="40px"></asp:TextBox>
                                                <span class="LabelsSmall">%</span>
                                            </td>
                                        </tr>
                                        <tr style="height: 24px;">
                                             <td align="right">
                                                <span class="LabelsSmall">DR Non Qual</span>&nbsp;</td>
                                                                                            <td align="left">
                                                <asp:TextBox ID="txtDRNQ" runat="server" Width="40px" TabIndex="7" MaxLength="6"></asp:TextBox>
                                                <span class="LabelsSmall">%</span>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtDRNQ"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            
                                             <td align="right">
                                                <span class="LabelsSmall">Wireless Access $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtWirelessAccess" runat="server" Width="40px" TabIndex="12" MaxLength="5"
                                                    AutoPostBack="true" OnTextChanged="txtWirelessFee_TextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtWirelessAccess"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>

                                            
                                            <td align="right">
                                                <span class="LabelsSmall">Voice Authorization $&nbsp;</span></td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtBatchHeader"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtVoiceAuth" runat="server" Width="40px" TabIndex="18" MaxLength="5"></asp:TextBox></td>
                                            <td align="right">
                                                <span class="LabelsSmall">Application Fee $&nbsp;</span></td>
                                            <td align="left">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtSetupFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                                <asp:TextBox ID="txtApplicationFee" runat="server" Width="40px" TabIndex="23" MaxLength="6"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtApplicationFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                        </tr>
                                        <tr style="height: 24px;">
                                            <td align="right">
                                                <span class="LabelsSmall">Discount Paid</span>&nbsp;</td>
                                            <td align="left">
                                                <asp:DropDownList ID="lstDiscountPaid" runat="server" TabIndex="8">
                                                <asp:ListItem Text="" Selected="True" />
                                                <asp:ListItem Text="Daily" />
                                                <asp:ListItem Text="Monthly" />
                                                </asp:DropDownList>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txtDRQD"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                           <td align="left" colspan="2">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtAVS"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>  
                                            
                                            <td align="right">
                                                <span class="LabelsSmall">Wireless Trans Fee $&nbsp;</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtWirelessTransFee" runat="server" Width="40px" TabIndex="13" MaxLength="5"
                                                    AutoPostBack="true" OnTextChanged="txtWirelessFee_TextChanged"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtWirelessTransFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            
                                            
                                                    <td align="right">
                                                <span class="LabelsSmall">Setup Fee $&nbsp;</span></td>
                                            <td align="left" style="height: 24px; width: 80px" colspan="1">
                                                <asp:TextBox ID="txtSetupFee" runat="server" Width="40px" TabIndex="24" MaxLength="6"></asp:TextBox></td>
                                            
                                            
                                        </tr>
                                        
                                        <asp:Panel ID="pnlOnlineDebit" runat="server" Width="100%">

                                                        <tr>
                                                            <td align="left" colspan="2">
                                                            &nbsp; &nbsp; &nbsp;
                                                                <asp:CheckBox ID="chkOnlineDebit" runat="server" AutoPostBack="True" Font-Bold="True"
                                                                    OnCheckedChanged="chkOnlineDebit_CheckedChanged" Text="Online Debit" /></td>
                                                            <td align="right">
                                                                <span class="LabelsSmall">Monthly Fee $</span>&nbsp;</td>
                                                            <td align="left" >
                                                                <asp:TextBox ID="txtDebitMonFee" runat="server" MaxLength="5" Width="40px" TabIndex="28"></asp:TextBox></td>
                                                            <td align="left" >
                                                                <span class="LabelsSmall">Transaction Fee $</span>&nbsp;</td>
                                                            <td align="left" colspan="2" >
                                                                <asp:TextBox ID="txtDebitTransFee" runat="server" MaxLength="5" Width="40px" TabIndex="29"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="8" style="height: 20px">
                                                            </td>
                                                        </tr>

                                                </asp:Panel>
                                                <asp:Panel ID="pnlEBT" runat="server" Width="100%">

                                                        <tr>
                                                            <td align="left"  colspan="2">
                                                            &nbsp; &nbsp; &nbsp;
                                                                <asp:CheckBox ID="chkEBT" runat="server" AutoPostBack="True" Font-Bold="True" Text="EBT"
                                                                    OnCheckedChanged="chkEBT_CheckedChanged" /></td>
                                                            <td align="right" >
                                                                <span class="LabelsSmall">Monthly Fee $</span>&nbsp;</td>
                                                            <td align="left" >
                                                                <asp:TextBox ID="txtEBTMonFee" runat="server" Width="40px" TabIndex="30" MaxLength="5"></asp:TextBox></td>
                                                            <td align="left" >
                                                                <span class="LabelsSmall" colspan="2">Transaction Fee $</span>&nbsp;</td>
                                                            <td align="left" colspan="2">
                                                                <asp:TextBox ID="txtEBTTransFee" runat="server" MaxLength="5" TabIndex="31" Width="40px"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="8" style="height: 20px">
                                                            </td>
                                                        </tr>

                                                </asp:Panel>
                                    </asp:Panel>
                                    <tr style="height: 24px;">
                                           
                                           <td align="center" colspan="8"> 

<asp:Button ID="btnProcessorSubmit" runat="server" Text="Submit" 
                                    TabIndex="50" OnClick="btnProcessorSubmit_Click"/>
                                            </td>
                                           
                                        </tr>

                                    <!--
                                     <tr>
                                                            <td align="right" colspan="1">
                                                                <span class="LabelsSmall">Platform &nbsp</span></td>
                                                            <td align="left" colspan="4">
                                                                <asp:DropDownList ID="lstPlatform" runat="server" TabIndex="4" Width="170px"></asp:DropDownList>
                                                                </td>
                                                            <td align="right" colspan="2">
                                                                <span class="LabelsSmall">Login ID</span></td>
                                                            <td align="left" colspan="1">
                                                                <asp:Label ID="lblLoginIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="1">
                                                                <span class="LabelsSmall">Merchant ID &nbsp</span></td>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblMerchantIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" colspan="2">
                                                                <span class="LabelsSmall">Terminal ID</span></td>
                                                            <td align="left" colspan="1">
                                                                <asp:Label ID="lblTerminalIDText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="1">
                                                                <span class="LabelsSmall">&nbsp Bank ID Number (BIN) &nbsp </span></td>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblBINNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" colspan="2">
                                                                <span class="LabelsSmall">Agent Chain Number</span></td>
                                                            <td align="left" colspan="1">
                                                                <asp:Label ID="lblAgentChainNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="1">
                                                                <span class="LabelsSmall">&nbsp Agent Bank Number &nbsp </span></td>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblAgentBankNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" colspan="2">
                                                                <span class="LabelsSmall">Store Number</span></td>
                                                            <td align="left" colspan="1">
                                                                <asp:Label ID="lblStoreNumberText" runat="server" Font-Bold="true"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="1">
                                                                <span class="LabelsSmall">&nbsp MCC Category Code &nbsp </span></td>
                                                            <td align="left" colspan="4">
                                                                <asp:Label ID="lblMCCCodeText" runat="server" Font-Bold="true"></asp:Label></td>
                                                            <td align="right" colspan="2">
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                        </tr>-->

                                </table>
                            </td>
                        </tr>
                        <tr height="15px">
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                    <tr>
                                        <td colspan="6" align="center" style="vertical-align: middle; width: 100%; height: 20px;
                                            background-image: url(../Images/topMain.gif)">
                                            <b><span class="MenuHeader">Payment Gateway</span></b>
                                        </td>
                                    </tr>
                                    <tr height="10px">
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            &nbsp; &nbsp; &nbsp;
                                            <asp:CheckBox ID="chkGateway" runat="server" AutoPostBack="True" Font-Bold="True"
                                                OnCheckedChanged="chkGateway_CheckedChanged" Text="Payment Gateway" /></td>
                                    </tr>
                                    <asp:Panel ID="pnlGatewayRates" runat="server" Width="100%" Visible="False">
                                        <tr height="10px">
                                        </tr>
                                        <tr>
                                            <td align="right" valign="middle" height="24px">
                                                <asp:Label ID="lblGateway" runat="server" Text="Gateway" Font-Bold="True"></asp:Label>&nbsp;</td>
                                            <td align="left" valign="middle" colspan="3">
                                                <asp:DropDownList ID="lstGatewayNames" runat="server" TabIndex="24" AutoPostBack="True"
                                                    OnSelectedIndexChanged="lstGatewayNames_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkECheck" runat="server" AutoPostBack="True" Text="Include E-Check"
                                                    OnCheckedChanged="chkECheck_CheckedChanged" Visible="false" /></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="middle">
                                                <span class="LabelsSmall">Setup Fee $</span>&nbsp;</td>
                                            <td align="left" valign="middle">
                                                <asp:TextBox ID="txtGWSetupFee" runat="server" Width="40px" TabIndex="25" MaxLength="6"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtGWSetupFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            <td align="right" valign="middle">
                                                <span class="LabelsSmall">Monthly Fee</span>&nbsp;</td>
                                            <td align="left" valign="middle">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txtGWMonthlyFee" runat="server" Width="40px" TabIndex="25" MaxLength="5"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtGWMonthlyFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                            <td align="right">
                                                <span class="LabelsSmall">Trans Fee</span>&nbsp;</td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txtGWTransFee" runat="server" Width="40px" TabIndex="27" MaxLength="5"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txtGWTransFee"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                        </tr>
                                       
                                    </asp:Panel>
                                    
                                </table>
                            </td>
                        </tr>
                        <tr height="15px">
                        </tr>
                        
                        <td align="center" colspan="6">
                                <asp:Button ID="btnGatewaySubmit" runat="server" Text="Submit" 
                                    TabIndex="50" OnClick="btnGatewaySubmit_Click"/>
                            </td>
                        

                        

                            </tr>
                        <tr height="15px">
                        </tr>
                        <tr>
                            <td colspan="3" align="center" valign="middle">
                                <asp:Panel ID="pnlAddlServices" runat="server" Height="20px">
                                    <div style="cursor: pointer; vertical-align: middle; width: 100%; height: 20px; background-image: url(../Images/topMain.gif)">
                                        <div style="float: left; text-align: center; margin-left: 230px;">
                                            <asp:Label ID="lblAddlServices" runat="server" Font-Bold="True" CssClass="MenuHeader"
                                                EnableTheming="False" Text="Additional Services (Optional)"></asp:Label>
                                        </div>
                                        <div style="float: left; margin-left: 20px;">
                                            <asp:Label ID="lblShowDetails" runat="server" CssClass="MenuHeader" EnableTheming="false">(Show)</asp:Label>
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                            <asp:Image ID="imgShowDetails" runat="server" ImageUrl="~/images/expand_blue.jpg" /></div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlNews" runat="server" Width="100%" Visible="True">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr height="10px">
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                
                                                <asp:Panel ID="pnlCheckGuarantee" runat="server" Width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 650px;">
                                                        <tr>
                                                            <td align="left" width="150px" valign="top">
                                                                <asp:CheckBox ID="chkCheckGuarantee" runat="server" AutoPostBack="True" Font-Bold="True"
                                                                    Text="Check Guarantee, Conversion, Verification or ACH" OnCheckedChanged="chkCheckGuarantee_CheckedChanged" /></td>
                                                            <td width="500px">
                                                                <table cellpadding="0" cellspacing="0" border="0" style="width: 500px;">
                                                                    <tr>
                                                                        <td align="right" width="100px">
                                                                            <span class="LabelsSmall">Service</span>&nbsp;</td>
                                                                        <td align="left" colspan="3">
                                                                            <asp:DropDownList ID="lstCheckService" runat="server" TabIndex="32" AutoPostBack="True" OnSelectedIndexChanged="lstCheckService_SelectedIndexChanged">

                                                                                <asp:ListItem Text="" Selected="True" />
                                                                                <asp:ListItem Text="CrossCheck" />
                                                                                <asp:ListItem Text="Direct Debit" />
                                                                                <asp:ListItem Text="eCheck.Net" />
                                                                                <asp:ListItem Text="Sage EFT Check" />
                                                                                <asp:ListItem Text="Telecheck" />
                                                                            </asp:DropDownList>
                                                                            </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" width="100px">
                                                                            <span class="LabelsSmall">Discount Rate</span>&nbsp;</td>
                                                                        <td align="left" width="150px">
                                                                        	<asp:TextBox ID="txtCGDiscRate" runat="server" MaxLength="5" Width="40px" TabIndex="33"></asp:TextBox></td>
                                                                        	<span class="LabelsSmall"></span>
                                                                        <td align="right" width="100px">
                                                                            <span class="LabelsSmall">Monthly Fee $</span>&nbsp;</td>
                                                                        <td align="left" width="150px">
                                                                            <asp:TextBox ID="txtCGMonFee" runat="server" MaxLength="5" Width="40px" TabIndex="34"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" width="100px">
                                                                            <span class="LabelsSmall">Monthly Minimum $</span>&nbsp;</td>
                                                                        <td align="left" width="150px">
                                                                            <asp:TextBox ID="txtCGMonMin" runat="server" MaxLength="5" Width="40px" TabIndex="35"></asp:TextBox></td>
                                                                        <td align="right" width="100px">
                                                                            <span class="LabelsSmall">Transaction Fee $</span>&nbsp;</td>
                                                                        <td align="left" width="150px">
                                                                            <asp:TextBox ID="txtCGTransFee" runat="server" MaxLength="5" Width="40px" TabIndex="36"></asp:TextBox></td>
                                                                    		
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="2" style="height: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlGiftCard" runat="server" Width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 650px;">
                                                        <tr>
                                                            <td align="left" width="150px">
                                                                <asp:CheckBox ID="chkGiftCard" runat="server" AutoPostBack="True" Font-Bold="True"
                                                                    Text="Gift/Loyalty Card" OnCheckedChanged="chkGiftCard_CheckedChanged" /></td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Company</span>&nbsp;</td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="lstGCType" runat="server" AutoPostBack="True" TabIndex="36" OnSelectedIndexChanged="lstGiftCard_SelectedIndexChanged">
                                                                    <asp:ListItem Text="" Selected="True" />
                                                                    <asp:ListItem Text="Sage EFT Gift & Loyalty" />
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Monthly Fee $</span>&nbsp;</td>
                                                            <td align="left" width="150px">
                                                                <asp:TextBox ID="txtGCMonFee" MaxLength="5" runat="server" TabIndex="37" Width="40px"></asp:TextBox></td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Transaction Fee $</span>&nbsp;</td>
                                                            <td align="left" width="150px">
                                                                <asp:TextBox ID="txtGCTransFee" MaxLength="5" runat="server" TabIndex="38" Width="40px"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="5" style="height: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlMerchantFunding" runat="server" Width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 650px;">
                                                        <tr>
                                                            <td align="left" width="150" >
                                                                <asp:CheckBox ID="chkMerchantFunding" runat="server" AutoPostBack="True" Font-Bold="True"
                                                                    Text="Merchant Cash Advance" OnCheckedChanged="chkMerchantFunding_CheckedChanged" /></td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Company</span>&nbsp;</td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="lstMCAType" runat="server" Width="185px" TabIndex="39">
                                                                    <asp:ListItem Text="" Selected="True" />
                                                                    <asp:ListItem Text="AdvanceMe, Inc." />
                                                                    <asp:ListItem Text="Business Financial Services" />
                                                                    <asp:ListItem Text="RapidAdvance" />
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="150">
                                                                </td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Amount</span>&nbsp;</td>
                                                            <td align="left" colspan="3"><asp:TextBox ID="txtCashDesired" runat="server"  />
                                                               </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="5" style="height: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlPayroll" runat="server" Width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 650px;">
                                                        <tr>
                                                            <td align="left" width="150">
                                                                <asp:CheckBox ID="chkPayroll" runat="server" AutoPostBack="True" Font-Bold="True"
                                                                    Text="Payroll" OnCheckedChanged="chkPayroll_CheckedChanged" /></td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Service</span>&nbsp;</td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="lstPayrollType" runat="server" Width="185px" TabIndex="39">
                                                                    <asp:ListItem Text="" Selected="True" />
                                                                    <asp:ListItem Text="Intuit QuickBooks Payroll Assisted" />
                                                                    <asp:ListItem Text="Sage 50 Managed Payroll" />
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="5" style="height: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlLease" runat="server" Width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 650px;">
                                                        <tr>
                                                            <td align="left" width="150">
                                                                <asp:CheckBox ID="chkLease" runat="server" AutoPostBack="True" Font-Bold="True" Text="Lease"
                                                                    OnCheckedChanged="chkLease_CheckedChanged" /></td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Company</span>&nbsp;</td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="lstLeaseCompany" runat="server" Width="185px" TabIndex="40">
                                                                    <asp:ListItem Text="" />
                                                                    <asp:ListItem Text="Northern Leasing Systems, Inc." Selected="True"/>
                                                                    
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Lease Payment $</span>&nbsp;</td>
                                                            <td align="left" width="150px">
                                                                <asp:TextBox ID="txtLeasePayment" runat="server" Width="40px" TabIndex="41" MaxLength="7"></asp:TextBox></td>
                                                            <td align="right" width="100px">
                                                                <span class="LabelsSmall">Lease Term</span>&nbsp;</td>
                                                            <td align="left" width="150px">
                                                                <asp:DropDownList ID="lstLeaseTerm" runat="server" TabIndex="42" Width="50px">
                                                                    <asp:ListItem Text="" Selected="True" />
                                                                    <asp:ListItem Text="12" />
                                                                    <asp:ListItem Text="24" />
                                                                    <asp:ListItem Text="36" />
                                                                    <asp:ListItem Text="48" />
                                                                </asp:DropDownList>
                                                                <span class="LabelsSmall">&nbsp;Months</span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="5" style="height: 20px">
                                                            </td>
                                                        </tr>
                                                        
                            <td align="center" colspan="6">
                                <asp:Button ID="btnAddlServSubmit" runat="server" Text="Submit" 
                                    TabIndex="50" OnClick="btnAddlServSubmit_Click"/>
                            </td>
                             <tr height="15px">
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
                            <td align="left" colspan="3">
                                <asp:Panel ID="pnlECheckRates" runat="server" Width="100%" Visible="False">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td colspan="4" align="center" style="height: 20px; background-image: url(../Images/topMain.gif)">
                                                <b><span class="MenuHeader">eCheck Rates</span></b>
                                            </td>
                                        </tr>
                                        <tr height="10px">
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <span class="LabelsSmall"><b>eChecks Standard Industry Rates</b></span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">Buy Rates</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Sell Rates</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Setup Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.0</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.0</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">Chargeback Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$25.00</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$25.00</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Returned Item Fee Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$3.00</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$3.00</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">Batch Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.20</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.30</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Monthly Minimum Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$5.00</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txteSIRMonMin" runat="server" Width="40px" TabIndex="38"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Per-Transaction Fee</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(Cumulative Tier)</b></span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(MUS 30%)</b></span></td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$0-$4,999.99</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.15</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txteSIRTransFee1" runat="server" Width="40px" TabIndex="39"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">$5,000.00-$49,999.99</span></td>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">$0.15</span></td>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txteSIRTransFee2" runat="server" Width="40px" TabIndex="40"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$50,000.00-$199,999.99</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.15</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txteSIRTransFee3" runat="server" Width="40px" TabIndex="41"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$200,000.00 or more</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.15</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txteSIRTransFee4" runat="server" Width="40px" TabIndex="33"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Discount Rate</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(Cumulative Tier)</b></span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(MUS 30%)</b></span></td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$0-$4,999.99</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">0.75%</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txteSIRDiscountRate1" runat="server" Width="40px" TabIndex="42"></asp:TextBox>
                                                <span class="LabelsSmall">%</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$5,000.00-$49,999.99</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">0.65%</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txteSIRDiscountRate2" runat="server" Width="40px" TabIndex="43"></asp:TextBox>
                                                <span class="LabelsSmall">%</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$50,000.00-$199,999.99</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">0.45%</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txteSIRDiscountRate3" runat="server" Width="40px" TabIndex="44"></asp:TextBox>
                                                <span class="LabelsSmall">%</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">$200,000.00 or more</span></td>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left" style="height: 24px">
                                                <span class="LabelsSmall">0.25%</span></td>
                                            <td align="left" style="height: 24px">
                                                <asp:TextBox ID="txteSIRDiscountRate4" runat="server" Width="40px" TabIndex="45"></asp:TextBox>
                                                <span class="LabelsSmall">%</span></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 15px" align="left" colspan="4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <span class="LabelsSmall"><b>eCheck Preferred Industry Rates</b></span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Setup Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.0</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.0</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">Chargeback Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$25.00</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$25.00</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Returned Item Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$3.00</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$3.00</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">Batch Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.20</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.30</span></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Monthly Minimum Fee</span></td>
                                            <td align="left">
                                            </td>
                                            <td align="left">
                                                <span class="LabelsSmall">$5.00</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txtePIRMonMin" runat="server" Width="40px" TabIndex="46"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck per Transaction Fee</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(Cumulative Tier)</b></span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(MUS 30%)</b></span></td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$0 or more</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$0.25</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txtePIRTransFee1" runat="server" Width="40px" TabIndex="47"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">eCheck Discount Rate</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(Cumulative Tier)</b></span></td>
                                            <td align="left">
                                                <span class="LabelsSmall"><b>(MUS 30%)</b></span></td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="LabelsSmall">$0 or more</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">Charge Transaction Dollar Volume</span></td>
                                            <td align="left">
                                                <span class="LabelsSmall">0.00%</span></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtePIRDiscountRate1" runat="server" Width="40px" TabIndex="48"></asp:TextBox>
                                                <span class="LabelsSmall">%</span></td>
                                        </tr>
                                        <tr height="10px">
                                        </tr>
                                        

                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="background-image: url(../Images/topMain.gif); height: 25px; width:380px">
                                <asp:Button ID="btnBack" runat="server" Text="Back" TabIndex="49" OnClick="btnBack_Click"
                                    CausesValidation="False" />
                            </td>
                            <td style="background-image: url(../Images/topMain.gif);" align="left">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                    TabIndex="50" />
                            </td>
                            <td align="right" style="background-image: url(../Images/topMain.gif);">
                                <asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="51" OnClientClick="javascript:window.close();"
                                    CausesValidation="False" />
                            </td>
                        </tr>

                    </table>
                    <asp:HiddenField ID="CustServLow" runat="server" />
                    <asp:HiddenField ID="InternetStmtLow" runat="server" />
                    <asp:HiddenField ID="MonMinLow" runat="server" />
                    <asp:HiddenField ID="TransFeeLow" runat="server" />
                    <asp:HiddenField ID="NBCTransFeeLow" runat="server" />
                    <asp:HiddenField ID="WirelessAccessFeeLow" runat="server" />
                    <asp:HiddenField ID="WirelessTransFeeLow" runat="server" />
                    <asp:HiddenField ID="DiscRateQualPresLow" runat="server" />
                    <asp:HiddenField ID="DiscRateQualNPLow" runat="server" />
                    <asp:HiddenField ID="DiscRateMidQualLow" runat="server" />
                    <asp:HiddenField ID="DiscRateMidQualStep" runat="server" />
                    <asp:HiddenField ID="DiscRateNonQualLow" runat="server" />
                    <asp:HiddenField ID="DiscRateNonQualStep" runat="server" />
                    <asp:HiddenField ID="DiscRateQualDebitLow" runat="server" />
                    <asp:HiddenField ID="ChargebackFeeLow" runat="server" />
                    <asp:HiddenField ID="RetrievalFeeLow" runat="server" />
                    <asp:HiddenField ID="VoiceAuthLow" runat="server" />
                    <asp:HiddenField ID="BatchHeaderLow" runat="server" />
                    <asp:HiddenField ID="AVSLow" runat="server" />
                    <asp:HiddenField ID="AnnualFeeLow" runat="server" />
                    <asp:HiddenField ID="GatewayMonFeeLow" runat="server" />
                    <asp:HiddenField ID="GatewayTransFeeLow" runat="server" />
                    <asp:HiddenField ID="GatewaySetupFeeLow" runat="server" />
                    <asp:HiddenField ID="DebitMonFeeLow" runat="server" />
                    <asp:HiddenField ID="DebitTransFeeLow" runat="server" />
                    <asp:HiddenField ID="CGMonFeeLow" runat="server" />
                    <asp:HiddenField ID="CGTransFeeLow" runat="server" />
                    <asp:HiddenField ID="CGMonMinLow" runat="server" />
                    <asp:HiddenField ID="CGDiscRateLow" runat="server" />
                    <asp:HiddenField ID="GCMonFeeLow" runat="server" />
                    <asp:HiddenField ID="GCTransFeeLow" runat="server" />
                    <asp:HiddenField ID="EBTMonFeeLow" runat="server" />
                    <asp:HiddenField ID="EBTTransFeeLow" runat="server" />
                    <asp:HiddenField ID="eSIRMonMinLow" runat="server" Value="5.00" />
                    <asp:HiddenField ID="eSIRTransFee1Low" runat="server" Value="0.15" />
                    <asp:HiddenField ID="eSIRTransFee2Low" runat="server" Value="0.15" />
                    <asp:HiddenField ID="eSIRTransFee3Low" runat="server" Value="0.15" />
                    <asp:HiddenField ID="eSIRTransFee4Low" runat="server" Value="0.15" />
                    <asp:HiddenField ID="eSIRDiscountRate1Low" runat="server" Value="0.75" />
                    <asp:HiddenField ID="eSIRDiscountRate2Low" runat="server" Value="0.65" />
                    <asp:HiddenField ID="eSIRDiscountRate3Low" runat="server" Value="0.45" />
                    <asp:HiddenField ID="eSIRDiscountRate4Low" runat="server" Value="0.25" />
                    <asp:HiddenField ID="ePIRMonMinLow" runat="server" Value="5.00" />
                    <asp:HiddenField ID="ePIRTransFee1Low" runat="server" Value="0.25" />
                    <asp:HiddenField ID="ePIRDiscountRate1Low" runat="server" Value="0.00" />
                    <asp:Label ID="CustServLowMessage" runat="server" />
                    <asp:Label ID="TestReturnProcessorName" runat="server" />
                    <asp:Label ID="TestReturnCardPresent" runat="server" />
                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </center>
</body>
</html>
