<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdatePartner.aspx.cs" Inherits="UpdatePartner"
    Theme="AppTheme" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E-Commerce Exchange - Update Partner</title>
    <link href="PartnerCSS.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
                Visible="False"></asp:Label><br />
            <strong><span class="LabelsRed">Leave blank for any fields that do not apply.</span></strong>
            <table cellpadding="0" cellspacing="2" border="0" style="width: 800px;" class="DivGreen">
                <tr>
                    <td colspan="2" style="background-image: url(/PartnerPortal/Images/topMain.gif);
                        height: 25px">
                        <span class="MenuHeader"><b>Update Partner Information</b></span>
                    </td>
                </tr>
                <tr>
                <td align="right" style="width: 30%">
                    <span class="LabelsSmall">TIER 1 Rep</span></td>
                  <td align="left">
                    <asp:DropDownList ID="lstT1RepName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstT1RepName_SelectedIndexChanged">
                  </asp:DropDownList>
                   <span class="LabelsSmall">(e.g. Jay Scott)</span></td>
                </tr>   
                <tr>
                    <td align="right" style="width: 30%">
                        <span class="LabelsSmall">Rep Name</span></td>
                    <td align="left">
                        <asp:TextBox ID="txtRepName" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                        <span class="LabelsSmall">(Ex. Jay Scott)</span></td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="LabelsSmall">Company Name</span></td>
                    <td align="left">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="150px" ReadOnly="true"></asp:TextBox><span class="LabelsRedLarge">*</span>
                        <span class="LabelsSmall">(e.g. Commerce Technologies Corp.)</span></td>
                        <asp:RequiredFieldValidator ID="RequiredFieldLegalName" runat="server" ControlToValidate="txtCompanyName"
                                            ErrorMessage="Company name"></asp:RequiredFieldValidator>
                </tr>
                <tr>
                    <td align="right">
                        <span class="LabelsSmall">DBA Name</span></td>
                    <td align="left">
                        <asp:TextBox ID="txtDBA" runat="server" Width="150px" ReadOnly="false"></asp:TextBox>
                        <span class="LabelsSmall">(e.g. E-Commerce Exchange)</span></td>
                </tr>
                <tr height="5px"></tr>
                <tr>
                     <td colspan="2" align="left">
                        <asp:Panel ID="pnlRepNumbers" runat="server" Width="100%">
                            <table style="width:100%" cellpadding="0" cellspacing="2" border="0"> 
                                <tr>
                                    <td align="right" style="width: 30%">
                                        <span class="LabelsSmall">Sage Rep # Declined</span></td>
                                    <td align="left">
                                        <asp:DropDownList ID="lstSageDeclined" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstSageDeclined_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width:30%">
                                        <span class="LabelsSmall">Sage</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSage" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                                        <span class="LabelsSmall">(Sage Rep #)</span></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width:30%">
                                        <span class="LabelsSmall">Uno Username</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtUnoUsername" runat="server" Width="150px" MaxLength="20"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width:30%">
                                        <span class="LabelsSmall">Uno Password</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtUnoPassword" TextMode="Password" runat="server" Width="150px" MaxLength="15"></asp:TextBox></td>
                                </tr>
                                <tr height="5px"></tr>
                                <tr>
                                    <td align="right" style="width:30%">
                                        <span class="LabelsSmall">iPayment (ECX 1503)</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtiPay1" runat="server" ReadOnly="True" Width="50px" MaxLength="5"></asp:TextBox>
                                        <span class="LabelsSmall">(iPayment1 Rep #)</span></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span class="LabelsSmall">iPayment (ECX 40558)</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtiPay2" runat="server" ReadOnly="True" Width="50px" MaxLength="5"></asp:TextBox>
                                        <span class="LabelsSmall">(iPayment2 Rep #)</span></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span class="LabelsSmall">iPayment (02733)</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtiPay3" runat="server" Width="50px" MaxLength="9"></asp:TextBox>
                                        <span class="LabelsSmall">(iPayment3 Rep #)</span></td>
                                </tr>
                                <tr>
                                    <td align="right" style="width:30%">
                                        <span class="LabelsSmall">iPayment Sales ID</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtiPaySalesID" runat="server" Width="50px" MaxLength="8"></asp:TextBox>
                                        <span class="LabelsSmall">(iPayment Sales ID)</span></td>
                                </tr>
                                <tr height="5px"></tr>
                                <tr>
                                    <td align="right">
                                        <span class="LabelsSmall">IMS (CTC 248)</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtIMS" runat="server" ReadOnly="True" Width="50px" MaxLength="5"></asp:TextBox>
                                        <span class="LabelsSmall">(IMS Rep #)</span></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span class="LabelsSmall">IMS2 (QB CTC 487)</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtIMSQB" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                                        <span class="LabelsSmall">(IMS2 Rep #)</span></td>
                                </tr>
                                <tr height="5px"></tr>
                                <tr>
                                    <td align="right">
                                        <span class="LabelsSmall">Chase</span></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtChase" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                                        <span class="LabelsSmall">(Chase Rep #)</span></td>
                                </tr>   
                                <tr height="5px"></tr>
                                <tr>
                                    <td align="right"><span class="LabelsSmall">IPS</span></td>
                                    <td align="left">
                                    <asp:TextBox ID="txtIPS" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                                    <span class="LabelsSmall">(IPS Rep#)</span></td>
                                </tr>                                
                                <tr>
                                   <td align="right" >
                                        <span class="LabelsSmall">Category</span></td>
                                    <td align="left">
                                        <asp:DropDownList ID="lstCategory" runat="server" Width="55px" AutoPostBack="true" OnSelectedIndexChanged="lstCategory_SelectedIndexChanged">
                                        </asp:DropDownList><span class="LabelsRedLarge">*</span>
                                        <span class="LabelsSmall">(A - Agent, E - Employee(Current), I-Inactive, PE - Past Employee, R - Reseller)</span></td>
                                </tr>    
                            </table>
                       </asp:Panel>
                       </td>
                   </tr>
                   <tr>
                     <td colspan="2" align="left">
                        <asp:Panel ID="pnlPayoutReqs" runat="server" Width="100%">
                            <table style="width:100%" cellpadding="0" cellspacing="2" border="0">      
                                <tr>
                                   <td align="right" style="width:30%">
                                        <span class="LabelsSmall">Funding Minimum</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFundMin" runat="server" Width="30px"></asp:TextBox><span class="LabelsRedLarge">*</span>
                                        <span class="LabelsSmall">(# of Fundings to meet Payout Requirement)</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFundMin"
                                            ErrorMessage="Funding Min"></asp:RequiredFieldValidator>
                                           </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 24px">
                                        <span class="LabelsSmall">Referral Minimum</span>
                                    </td>
                                    <td align="left" style="height: 24px">
                                        <asp:TextBox ID="txtRefMin" runat="server" Width="30px"></asp:TextBox><span class="LabelsRedLarge">*</span>
                                        <span class="LabelsSmall">(# of Referrals to meet Payout Requirement)</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRefMin"
                                            ErrorMessage="Referral Min"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 24px">
                                        <span class="LabelsSmall">Residual Minimum</span>
                                    </td>
                                    <td align="left" style="height: 24px">
                                        <asp:TextBox ID="txtResidualMin" runat="server" Width="30px"></asp:TextBox><span class="LabelsRedLarge">*</span>
                                        <span class="LabelsSmall">(Residual Total Minimum to meet Payout Requirement)</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtResidualMin"
                                            ErrorMessage="Residual Min"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="LabelsSmall">Master Rep #</span></td>
                    <td align="left">
                        <asp:TextBox ID="txtMasterNum" runat="server" Width="50px" MaxLength="10" ReadOnly="true"></asp:TextBox>
                        <span class="LabelsSmall">(Should default to the first Rep # assigned in any of the programs)</span></td>
                </tr>      
                <tr>
                    <td align="right" style="height: 22px">
                        <span class="LabelsSmall">Residual %</span></td>
                    <td align="left" style="height: 22px">
                        <asp:DropDownList ID="lstResidual" runat="server" ToolTip="Subtract from your Residual Pct to get the Tier 1 Residual Pct"></asp:DropDownList><span class="LabelsRedLarge">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" EnableClientScript="false" ControlToValidate="lstResidual" runat="server" ErrorMessage="Residual %"></asp:RequiredFieldValidator>
                        <span class="LabelsSmall">(Rep's Pct on the Residual amount)</span></td>
                </tr>
                <tr>
                    <td align="right" style="height: 24px">
                        <span class="LabelsSmall">Commission %</span></td>
                    <td align="left" style="height: 24px">
                        <asp:DropDownList ID="lstCommission" runat="server" ToolTip="Subtract from your Commission Pct to get the Tier 1 Commission Pct"></asp:DropDownList><span class="LabelsRedLarge">*</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" EnableClientScript="false" ControlToValidate="lstCommission" runat="server" ErrorMessage="Commission %"></asp:RequiredFieldValidator>
                        <span class="LabelsSmall">(Rep's Pct on the Commission) </span></td>
                </tr>   
                <tr>
                    <td align="right">
                        <span class="LabelsSmall">CNP Default Rate Package</span></td>
                    <td align="left">
                        <asp:DropDownList ID="lstCNPPackageList" runat="server">
                        </asp:DropDownList>
                        <span class="LabelsSmall">(This will be the default rates for Card Not Present App Signups)</span></td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="LabelsSmall">CP Default Rate Package</span></td>
                    <td align="left">
                        <asp:DropDownList ID="lstCPPackageList" runat="server">
                        </asp:DropDownList>
                        <span class="LabelsSmall">(This will be the default rates for Card Present App Signups)</span></td>
                </tr>
                <tr>
                    <td align="left" colspan="2" style="width: 100%">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <strong><span class="LabelsRed">* - Updating these fields will change the values for
                            only the CURRENT month and any months going forward.<br />
                            Any updates on these fields in past months must be changed manually on the backend.
                        </span></strong>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2" style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td style="background-image: url(/PartnerPortal/Images/topMain.gif); height: 25px"
                        align="center" colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            TabIndex="41" />&nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="42" OnClick="btnClose_Click"
                            CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>
