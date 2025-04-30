<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyPackage.aspx.cs" Inherits="ModifyPackage"
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

    <script type="text/javascript" src="fillRates.js" language="javascript">    
    </script>

    <link href="../PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<a href="http://www.instantssl.com" id="comodoTL">SSL</a>
<script language="JavaScript" type="text/javascript">
COT("https://www.firstaffiliates.com/images/secure_site.gif", "SC2", "none");
</script>
<body>
    <form id="form1" runat="server">
        <center>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>     
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlNewsHeader"
                    Collapsed="false" CollapsedImage="~/images/expand_blue.jpg" CollapsedText="(Show)"
                    ExpandControlID="pnlNewsHeader" ExpandedImage="~/images/collapse_blue.jpg" ExpandedText="(Hide)"
                    ImageControlID="imgShowDetails" SuppressPostBack="true" TargetControlID="pnlNews"
                    TextLabelID="lblShowDetails">
                </cc1:CollapsiblePanelExtender>
                        <asp:Label ID="lblError" CssClass="LabelsError" runat="server" Visible="False"  EnableTheming="false"></asp:Label>
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 740px;" class="DivGreen">
                    <tr>
                        <td colspan="5" align="center" style="height: 25px; background-image: url(/PartnerPortal/Images/topMain.gif)">
                            <b><span class="MenuHeader">Modify Package ID:
                                <asp:Label ID="lblPID" runat="server"></asp:Label>
                                 <asp:Label ID="lblInactive" text="(Inactive)" runat="server"></asp:Label>
                                </span></b></td>
                    </tr>
                    <tr>
                        <td style="height: 15px" colspan="5" align="center">
                            <strong><asp:HyperLink ID="lnkRatesCharts" runat="server" Target="_blank" CssClass="One" NavigateUrl="~/../Comparison/Processor%20Buy%20Rate%20Comparison.xls">Rate Comparison Chart</asp:HyperLink><br />
                                <span style="font-size: 10pt; color: #ff0000; font-family: Arial">Note: Modifying a
                                    package will NOT affect the rates for previous apps</span></strong></td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 100px">
                            <asp:Label ID="lblSelectPackage" runat="server" Text="Select Package"></asp:Label></td>
                        <td align="left">
                            <asp:DropDownList ID="lstPackageNames" runat="server" TabIndex="1" OnSelectedIndexChanged="lstPackageNames_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList></td>
                        <td align="left" colspan=3>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" TabIndex="42" OnClick="btnDelete_Click" />
                        </td>
                    </tr>
                    <tr height=5px></tr>
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Panel ID="pnlDeletePackage" Visible="false" runat="server" BackColor="#FFC0C0" BorderColor="Salmon"
                                BorderStyle="Double" Width="45%">
                                <center>
                                <asp:Label ID="lblDelete" runat="server" Text="Are you sure you want to delete this package?"></asp:Label>
                                <br />
                                <asp:Button ID="btnDelYes" runat="server" OnClick="btnDelYes_Click" Text="Yes" />
                                <asp:Button ID="btnDelNo" runat="server" OnClick="btnDelNo_Click" Text="No" />
                                </center>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr height=5px></tr>
                    <tr>
                        <td align="center" colspan="5"> 
                            <asp:Panel ID="pnlDetail" Visible="true" runat="server" BackColor="#FFC0C0" BorderColor="Salmon"
                                BorderStyle="Double" Width="70%"  Height="40px">
                                 <asp:Label ID="lblAffiliateIDHdr" runat="server" Text="Assigned to Partner ID(s): "></asp:Label>
                                 <asp:Label ID="lblAffiliateIDs" runat="server"></asp:Label>
                                 <asp:Label ID="lblAffiliateIDsMore" runat="server" Text="more" Font-Underline="true" Visible = "false"></asp:Label>
                                <br />                    
                                <asp:Label ID="lblAppIDHdr" runat="server" Text="Assigned to App ID(s):"></asp:Label>
                                <asp:Label ID="lblAppIDs" runat="server"></asp:Label>
                                <asp:Label ID="lblAppIDsMore" runat="server" Text="more" Font-Underline="true" Visible="false"></asp:Label>
                                <br />     
                            </asp:Panel>                                       
                        </td>
                    </tr>
                    <tr height=10></tr>
                    <tr>
                        <td align="right" valign=bottom style="width: 100px">
                            <asp:Label ID="lblSelectProcessor" runat="server" Text="Select Processor"></asp:Label></td>
                        <td align="left"colspan=2>
                            <asp:DropDownList ID="lstProcessorNames" runat="server" Enabled="False" TabIndex="1">
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkApplyInterchange" runat="server" AutoPostBack="True" Text="Interchange Rates" TabIndex="2" OnCheckedChanged="chkApplyInterchange_CheckedChanged"/><asp:CheckBox ID="chkBillAssessment" Visible=false runat="server" Text="Billing Assessments"/></td>
                        <td align="right" valign=bottom>
                            <asp:Label ID="lblSalesRepHeader" runat="server" Text="Sales Rep:" Font-Bold="True"></asp:Label>&nbsp;</td>
                        <td align="left" valign=bottom>
                            <asp:Label ID="lblSalesRep" runat="server" Font-Bold="True" Width=100></asp:Label>
                        </td>
                    </tr>
                    <tr height=10></tr>
                    <tr>
                        <td></td>
                        <td align="left" colspan="4">
                            <asp:RadioButton ID="rdbCP" runat="server" Enabled="false" GroupName="CPCNP" Text="Card Present"
                                TabIndex="4" />
                            <asp:RadioButton ID="rdbCNP" runat="server" Enabled="false" GroupName="CPCNP" Text="Card Not Present" /></td>
                    </tr>
                    <tr height=10></tr>
                    <tr>
                        <td align="left" colspan="5">
                            <asp:Panel ID="pnlMerchantRates" runat="server" Width="100%" Visible="True">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                    <tr><td align="right" style="height: 30px; width: 210px;">
                                            <asp:Label ID="lblDRQP" runat="server" Text="Disc Rate Qual Pres"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 30px; width: 80px;">
                                            <asp:TextBox ID="txtDRQP" runat="server" onBlur="Javascript:fillDebit(); fillMidQual(); fillNonQual(); return false;"
                                                Width="40px" TabIndex="3" MaxLength="6"></asp:TextBox>
                                              <span class="LabelsSmall">%</span>
                                        </td>
                                        <td align="right" style="height: 24px; width: 250px;">
                                            <asp:Label ID="lblCustomerService" runat="server" Text="Customer Service"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px; width: 80px;">
                                           <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtCustomerService" runat="server" Width="40px" TabIndex="8" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px; width: 150px;">
                                            <asp:Label ID="lblTransFee" runat="server" Text="Transaction Fee"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px; width: 80px;">
                                             <span class="LabelsSmall">$</span>                
                                             <asp:TextBox ID="txtTransFee" runat="server" MaxLength="5" AutoPostBack=true OnTextChanged="txtTransFee_TextChanged" 
                                                TabIndex="13" Width="40px"></asp:TextBox></td>
                                        <td align="right" style="width: 180px; height: 24px;">
                                            <asp:Label ID="lblAnnualFee" runat="server" Text="Annual  Fee $"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px; width: 100px;">
                                            <asp:DropDownList ID="lstAnnualFee" runat="server" TabIndex="18" Width=60px>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr><td align="right" style="height: 24px;">
                                            <asp:Label ID="lblDRQNP" runat="server" Text="Disc Rate Qual NP"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px;">
                                            <asp:TextBox ID="txtDRQNP" onBlur="Javascript:fillMidQual(); fillNonQual(); fillDebit(); return false;"
                                                runat="server" Width="40px" TabIndex="4" MaxLength="6"></asp:TextBox>
                                              <span class="LabelsSmall">%</span>
                                        </td>
                                        <td align="right" style="height: 24px;">
                                            <asp:Label ID="lblMonMin" runat="server" Text="Monthly Minimum"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px;">
                                           <span class="LabelsSmall">$
                                           <asp:TextBox ID="txtMonMin" runat="server" MaxLength="5" 
                                                TabIndex="9" Width="40px"></asp:TextBox></span>
                                            </td>
                                        <td align="right" style="height: 24px;">
                                            <asp:Label ID="lblBatchHeader" runat="server" Text="Batch Header"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                           <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtBatchHeader" runat="server" Width="40px" TabIndex="14" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px;">
                                            <asp:Label ID="lblChargebackFee" runat="server" Text="Chargeback Fee $"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px;">
                                            <asp:TextBox ID="txtChargebackFee" runat="server" Width="40px" TabIndex="19" MaxLength="5"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblDRMQ" runat="server" Text="Disc Rate Mid Qual"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtDRMQ" runat="server" Width="40px" TabIndex="5" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span>
                                            </td><td align="right" style="height: 24px">
                                            <asp:Label ID="lblInternetStmt" runat="server" Text="Internet Statement"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                             <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtInternetStmt" runat="server" Width="40px" TabIndex="10" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblNBCTransFee" runat="server" Text="Non Bankcard Trans"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                              <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtNBCTransFee" runat="server" Width="40px" TabIndex="15" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblRetrievalFee" runat="server" Text="Retrieval Fee $"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtRetrievalFee" runat="server" Width="40px" TabIndex="20" MaxLength="5"></asp:TextBox></td>
                                    </tr>
                                    <tr><td align="right" style="height: 24px">
                                            <asp:Label ID="lblDRNQ" runat="server" Text="Disc Rate Non Qual"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtDRNQ" runat="server" Width="40px" TabIndex="6" MaxLength="5"></asp:TextBox>
                                            <span class="LabelsSmall">%</span>
                                        </td>
                                        <td align="right" style="height: 24px">
                                                <span class="LabelsSmall">Regulatory Fee&nbsp;</span></td>
                                            <td align="left" style="height: 24px; width: 80px" colspan="1">
                                            <span class="LabelsSmall">$</span>
                                                <asp:TextBox ID="txtComplianceFee" runat="server" Width="40px" TabIndex="24" MaxLength="6"></asp:TextBox></td>
                                       
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblAVS" runat="server" Text="AVS"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                              <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtAVS" runat="server" Width="40px" TabIndex="16" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblRollingReserve" runat="server" Text="Rolling Reserve"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtRollingReserve" runat="server" Width="40px" TabIndex="21" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span></td>
                                    </tr>
                                    <tr><td align="right" style="height: 24px">
                                            <asp:Label ID="lblDRQD" runat="server" Text="Disc Rate Qual Debit"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtDRQD" runat="server" Width="40px" TabIndex="7" MaxLength="6"></asp:TextBox>
                                            <span class="LabelsSmall">%</span>
                                            </td>
                                            
                                             <td align="right" style="height: 24px">
                                            <asp:Label ID="lblWirelessAccess" runat="server" Text="Wireless Access"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                           <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtWirelessAccess" runat="server" Width="40px" TabIndex="11" MaxLength="5"></asp:TextBox></td>
                                        
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblVoiceAuth" runat="server" Text="Voice Authorization"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                          <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtVoiceAuth" runat="server" Width="40px" TabIndex="17" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px">
                                            <asp:Label ID="lblSetupFee" runat="server" Text="Setup Fee $"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtSetupFee" runat="server" Width="40px" TabIndex="22" MaxLength="6"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan=4></td>
                                         <td align="right" style="height: 24px">
                                            <asp:Label ID="lblWirelessTransFee" runat="server" Text="Wireless Trans"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <span class="LabelsSmall">$</span>
                                            <asp:TextBox ID="txtWirelessTransFee" runat="server" Width="40px" TabIndex="12" MaxLength="5"></asp:TextBox></td>
                                        <td align="right" style="height: 24px">
                                       
                                            <asp:Label ID="lblApplicationFee" runat="server" Text="Application Fee $"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px">
                                            <asp:TextBox ID="txtApplicationFee" runat="server" Width="40px" TabIndex="23" MaxLength="6"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="10" style="height: 15px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                                                    <td align="center" colspan="5">
                                                    <asp:Panel ID="pnlOnlineDebit" runat="server" Width="100%">
                                                    <tr>
                                                        <td align="center" colspan="5">
                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Online Debit"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                                <asp:Label ID="lblDebitMonFee" runat="server" Text="Debit Mon Fee"></asp:Label></td>
                                                            <td align="left">
                                                                &nbsp;<asp:Label ID="Label14" runat="server" Text="$"></asp:Label>
                                                                <asp:TextBox ID="txtDebitMonFee" runat="server" MaxLength="5" TabIndex="28" Width="36px"></asp:TextBox></td>
                                                            <td width=50></td>
                                                            <td align="right">
                                                                <asp:Label ID="lblDebitTransFee" runat="server" Text="Debit Trans Fee"></asp:Label></td>
                                                            <td align="left">
                                                                &nbsp;<asp:Label ID="Label15" runat="server" Text="$"></asp:Label>
                                                                <asp:TextBox ID="txtDebitTransFee" runat="server" MaxLength="5" TabIndex="29" Width="36px"></asp:TextBox></td>
                                                   </tr>
                                                   </asp:Panel>
                                                   </td>
                     </tr>
                      <tr>
                                                    <td align="center" colspan="5">
                                                        <asp:Panel ID="pnlEBT" runat="server" Visible="false" Width="100%">
                                                            <tr>
                                                                <td align="center" colspan="5">
                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="EBT"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="Label19" runat="server" Text="Monthly Fee"></asp:Label></td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label ID="Label20" runat="server" Text="$"></asp:Label>
                                                                    <asp:TextBox ID="txtEBTMonFee" runat="server" MaxLength="5" TabIndex="36" Width="36px"></asp:TextBox></td>
                                                                <td width=50></td>
                                                                <td align="right">
                                                                    <asp:Label ID="Label21" runat="server" Text="Transaction Fee"></asp:Label></td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label ID="Label22" runat="server" Text="$"></asp:Label>
                                                                    <asp:TextBox ID="txtEBTTransFee" runat="server" MaxLength="5" TabIndex="37" Width="36px"></asp:TextBox></td>
                                                            </tr>
                                                        </asp:Panel>
                                                    </td>
                       </tr>
                     <tr>
                        <td style="height: 15px" align="left" colspan="5">
                        </td>
                    </tr>              
                    <tr>
                        <td align="left" colspan="5">
                            <asp:Panel ID="pnlGatewayRates" runat="server" Width="100%" Visible="True">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                    <tr>
                                        <td align="right" width=112px>
                                            <asp:Label ID="lblGateway" runat="server" Text="Gateway" Font-Bold="True"></asp:Label>&nbsp;</td>
                                        <td align="left" style="height: 24px;" width=100px>
                                            <asp:DropDownList ID="lstGatewayNames" runat="server" TabIndex="24" AutoPostBack="True"
                                                OnSelectedIndexChanged="lstGatewayNames_SelectedIndexChanged" Width=150px>
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
                                            <asp:Label ID="lblGatewaySetupFee" runat="server" Text="Setup Fee $"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:TextBox ID="txtGWSetupFee" runat="server" Width="40px" TabIndex="25" MaxLength="6"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblGatewayMonthlyFee" runat="server" Text="Monthly Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:Label ID="Label98" runat="server" Text="$"></asp:Label>
                                            <asp:TextBox ID="txtGWMonthlyFee" runat="server" Width="40px" TabIndex="26" MaxLength="5"></asp:TextBox></td>
                                        <td align="right">
                                            <asp:Label ID="lblGatewayTransFee" runat="server" Text="Trans Fee"></asp:Label>&nbsp;</td>
                                        <td align="left">
                                            <asp:Label ID="Label99" runat="server" Text="$"></asp:Label>
                                            <asp:TextBox ID="txtGWTransFee" runat="server" Width="40px" TabIndex="27" MaxLength="5"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="5" style="height: 15px">
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="center" colspan="5">
                            <asp:Panel ID="pnlNewsHeader" runat="server" Height="20px">
                                <div style="cursor: pointer; vertical-align: middle; width: 700px; height: 20px; background-image:url(/PartnerPortal/Images/topmain.gif)">
                                    <div style="float: left; text-align: center; margin-left: 230px;">
                                        <asp:Label ID="lblNewsUpdates" runat="server" Font-Bold="True" CssClass="MenuHeader" Text="Additional Services (Optional)" EnableTheming="false"></asp:Label>
                                    </div>
                                    <div style="float: left; margin-left: 20px;">
                                        <asp:Label ID="lblShowDetails" runat="server" Font-Bold="True" CssClass="MenuHeader" EnableTheming="false">(Show)</asp:Label>
                                    </div>
                                    <div style="float: right; vertical-align: middle;">
                                        <asp:Image ID="imgShowDetails" runat="server" ImageUrl="~/images/expand_blue.jpg" /></div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlNews" runat="server" Width="700px" Visible="True">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="DivGreen">
                                    <tr>
                                        <td align="center" colspan="8">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 60%;">
                                                
                                                <tr>
                                                    <td align="right" colspan="5" style="height: 15px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="5">
                                                        <asp:Panel ID="pnlCheckGuarantee" runat="server" Width="100%">
                                                        <tr>
                                                            <td align="center" colspan="5">
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Check Services"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="Label3" runat="server" Text="Monthly Fee"></asp:Label></td>
                                                            <td align="left">
                                                                &nbsp;<asp:Label ID="Label13" runat="server" Text="$"></asp:Label>
                                                                <asp:TextBox ID="txtCGMonFee" runat="server" MaxLength="5" TabIndex="30" Width="36px"></asp:TextBox></td>
                                                            <td width=50></td>
                                                            <td align="right">
                                                                <asp:Label ID="Label4" runat="server" Text="Transaction Fee"></asp:Label></td>
                                                            <td align="left">
                                                                &nbsp;<asp:Label ID="Label16" runat="server" Text="$"></asp:Label>
                                                                <asp:TextBox ID="txtCGTransFee" runat="server" MaxLength="5" TabIndex="31" Width="36px"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="Label5" runat="server" Text="Monthly Minimum"></asp:Label></td>
                                                            <td align="left">
                                                                &nbsp;<asp:Label ID="Label12" runat="server" Text="$"></asp:Label>
                                                                <asp:TextBox ID="txtCGMonMin" runat="server" MaxLength="5" TabIndex="32" Width="36px"></asp:TextBox></td>
                                                            <td width=50></td>
                                                            <td align="right">
                                                                <asp:Label ID="Label6" runat="server" Text="Discount Rate"></asp:Label></td>
                                                            <td align="left">
                                                                &nbsp;<asp:Label ID="Label17" runat="server" Text="$"></asp:Label>
                                                                <asp:TextBox ID="txtCGDiscRate" runat="server" MaxLength="5" TabIndex="33" Width="36px"></asp:TextBox></td>
                                                        </tr>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="5" style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="5">
                                                        <asp:Panel ID="pnlGiftCard" runat="server" Visible="false" Width="100%">
                                                            <tr>
                                                                <td align="center" colspan="5">
                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Gift Card"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="Label8" runat="server" Text="Monthly Fee"></asp:Label></td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label ID="Label11" runat="server" Text="$"></asp:Label>
                                                                    <asp:TextBox ID="txtGCMonFee" runat="server" MaxLength="5" TabIndex="34" Width="36px"></asp:TextBox></td>
                                                                <td width=50></td>
                                                                <td align="right">
                                                                    <asp:Label ID="Label9" runat="server" Text="Transaction Fee"></asp:Label></td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label ID="Label18" runat="server" Text="$"></asp:Label>
                                                                    <asp:TextBox ID="txtGCTransFee" runat="server" MaxLength="5" TabIndex="35" Width="36px"></asp:TextBox></td>
                                                            </tr>
                                                         </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="5" style="height: 8px">
                                                    </td>
                                                </tr>
                                               
                                             </table>
                                        </td>
                                    </tr>                         
                            </table>
                        </asp:Panel>
                        </td>
                    </tr>
                    </table>
                    <tr>
                        <td style="height: 5px" align="left" colspan="5">
                        </td>
                    </tr>                  
                    <tr>
                        <td align="right" colspan="1">
                            <asp:Label ID="lblPackageName" runat="server" Text="Specify Package Name: "></asp:Label></td>
                        <td align="left" colspan="4" style="width: 70%">
                            <asp:TextBox ID="txtPackagePrefix" Width="180px" runat="server" TabIndex="38"></asp:TextBox>
                            <asp:TextBox ID="txtPackageSuffix" Width="180px" runat="server" TabIndex="39"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px" align="left" colspan="5">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 30%;
                            height: 30px">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" TabIndex="40" OnClick="btnReset_Click" CausesValidation="False" />
                        </td>
                        <td style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 40%; height: 30px" align="center"
                            colspan="3">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                TabIndex="41" />
                        </td>
                        <td align="center" style="background-image: url(/PartnerPortal/Images/topMain.gif); width: 30%;
                            height: 30px">
                            <asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="41" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="Javascript:window.close();" />
                        </td>
                    </tr>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>                
            </div>
        </center>
    </form>
</body>
</html>
