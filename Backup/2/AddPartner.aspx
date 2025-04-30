<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true"
    CodeFile="AddPartner.aspx.cs" Inherits="AddPartner" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
        Visible="False"></asp:Label><br />
    <strong><span class="LabelsRed">Please Enter All data before Submitting. Leave Blank
        if the Rep Number does not apply.</span></strong>
    <table cellpadding="0" cellspacing="2" border="0" style="width: 650px;" class="DivGreen">
        <tr>
            <td align="center" colspan="2" style="background-image: url(../Images/topMain.gif);
                height: 25px">
                <span class="MenuHeader"><b>Add Partner Information</b></span>
            </td>
        </tr>    

       <tr>
           <td align="right" style="width: 30%">
              <!--<span class="LabelsSmall">TIER 1 Rep</span>--></td>
               <td align="left">
              <asp:DropDownList ID="lstT1RepName" runat="server" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="lstT1RepName_SelectedIndexChanged">
              </asp:DropDownList>
             <!--<span class="LabelsSmall">(e.g. Jay Scott)</span>--></td>
        </tr>    
                             
        <tr>
            <td align="right" style="width: 30%">
                <span class="LabelsSmall">Rep Name</span></td>
            <td align="left">
                <asp:DropDownList ID="lstRepName" runat="server" AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="lstRepName_SelectedIndexChanged">
                </asp:DropDownList>
                <span class="LabelsSmall">(e.g. Jay Scott)</span></td>
        </tr>
        <tr>
            <td align="right">
                <span class="LabelsSmall">Company Name</span></td>
            <td align="left">
                <asp:TextBox ID="txtCompanyName" runat="server" TabIndex="3" ReadOnly="true"></asp:TextBox>
                <span class="LabelsSmall">(e.g. Commerce Technologies Corp.)</span></td>
        </tr>
        <tr>
            <td align="right">
                <span class="LabelsSmall">DBA Name</span></td>
            <td align="left">
                <asp:TextBox ID="txtDBA" runat="server" TabIndex="4" ReadOnly="true"></asp:TextBox>
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
                                <asp:DropDownList ID="lstSageDeclined" runat="server" AutoPostBack="true" TabIndex="6" OnSelectedIndexChanged="lstSageDeclined_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr> 
                        <tr>
                            <td align="right" style="width: 30%">
                                <span class="LabelsSmall">Sage</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtSage" runat="server" Width="50px" TabIndex="7" MaxLength="6"></asp:TextBox>
                                <span class="LabelsSmall">Sage Rep #</span></td>
                        </tr>  
                                                <tr>
                            <td align="right" style="width: 30%">
                                <span class="LabelsSmall">IPS</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtIPS" runat="server" Width="50px" TabIndex="7" MaxLength="6"></asp:TextBox>
                                <span class="LabelsSmall">IPS Rep #</span></td>
                        </tr>  
                        <tr>
                            <td align="right" style="width:30%">
                                <span class="LabelsSmall">Uno Username</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtUnoUsername" runat="server" Width="150px" TabIndex="8" MaxLength="10"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="right" style="width:30%">
                                <span class="LabelsSmall">Uno Password</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtUnoPassword" TextMode="Password" TabIndex="9" runat="server" Width="150px" MaxLength="15"></asp:TextBox></td>
                        </tr>
                                          
                        <tr>
                            <td align="right" style="width: 30%">
                                <span class="LabelsSmall">iPayment (02733)</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtiPay3" runat="server" Width="50px" TabIndex="10" MaxLength="9"></asp:TextBox>
                                <span class="LabelsSmall">(iPayment 02733 Rep #)</span></td>
                        </tr>
  
                        <tr>
                            <td align="right">
                                <span class="LabelsSmall">IMS2 (QB CTC 487)</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtIMSQB" runat="server" Width="50px" TabIndex="11" MaxLength="5"></asp:TextBox>
                                <span class="LabelsSmall">(IMS Rep #)</span></td>
                        </tr>
  
                        <tr>
                            <td align="right">
                                <span class="LabelsSmall">Chase</span></td>
                            <td align="left">
                                <asp:TextBox ID="txtChase" Width="50px" runat="server" TabIndex="12" MaxLength="5"></asp:TextBox>
                                <span class="LabelsSmall">(Chase Rep #)</span></td>
                        </tr> 
                        <tr height="5px"></tr>     
                        <tr>
                            <td align="right" valign="top">
                                <span class="LabelsSmall">Rep Category</span></td>
                            <td align="left">
                                <asp:DropDownList ID="lstCategory" runat="server" TabIndex="13">
                                </asp:DropDownList>
                                <span class="LabelsSmall">(A - Agent, E - Employee(Current), I-Inactive,
                                    PE - Past Employee, R - Reseller)</span></td>
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
                       <td align="right" style="width: 30%">
                         <span class="LabelsSmall">Funding Minimum</span>
                         </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFundMin" Width="30px" TabIndex="14" runat="server"></asp:TextBox><span class="LabelsRedLarge">*</span>
                                    <span class="LabelsSmall">(# of Fundings to meet Payout Requirement)</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFundMin"
                                        ErrorMessage="Residual Min"></asp:RequiredFieldValidator>      
                                          </td>
                            </tr>
                            <tr>
                                  <td align="right" style="width: 30%">
                                    <span class="LabelsSmall">Referral Minimum</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtRefMin" Width="30px" TabIndex="15" runat="server"></asp:TextBox><span class="LabelsRedLarge">*</span>
                                    <span class="LabelsSmall">(# of Referrals to meet Payout Requirement)</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRefMin"
                                        ErrorMessage="Residual Min"></asp:RequiredFieldValidator>  
                                        </td>
                            </tr>
                            <tr>
                                  <td align="right" style="width: 30%">
                                    <span class="LabelsSmall">Residual Minimum</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtResidualMin"  Width="50px" TabIndex="16" runat="server"></asp:TextBox><span class="LabelsRedLarge">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtResidualMin"
                                        ErrorMessage="Residual Min">     <span class="LabelsSmall">(Residual Total Minimum
                                        meet Payout Requirement)</span></asp:RequiredFieldValidator></td>               
                          </tr>
                                 
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <span class="LabelsSmall">Master Rep #</span></td>
            <td align="left">
                <asp:TextBox ID="txtMasterNum" Width="50px" runat="server" TabIndex="17"></asp:TextBox>
                <span class="LabelsRedLarge">*</span> <span class="LabelsSmall">(Should default to the first
                    Rep # assigned in any of the programs)</span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMasterNum"
                    ErrorMessage="Master Num"></asp:RequiredFieldValidator></td>
        </tr>
    
        <tr>
            <td align="right" valign="top">
                <span class="LabelsSmall">Residual %</span></td>
            <td align="left">
                <asp:DropDownList ID="lstResidual" Width="50px" runat="server" TabIndex="18" ToolTip="Subtract from your Residual Pct to get the Tier 1 Residual Pct"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" EnableClientScript="false" ControlToValidate="lstResidual" runat="server" ErrorMessage="Residual %"></asp:RequiredFieldValidator>
                <span class="LabelsSmall">(Do not enter the % sign, e.g. 35)</span></td>
        </tr>
        <tr>
            <td align="right">
                <span class="LabelsSmall">Commission %</span></td>
            <td align="left">
                <asp:DropDownList ID="lstCommission" Width="50px" runat="server" TabIndex="19" ToolTip="Subtract from your Commission Pct to get the Tier 1 Commission Pct"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" EnableClientScript="false" ControlToValidate="lstCommission" runat="server" ErrorMessage="Commission %"></asp:RequiredFieldValidator>
                <span class="LabelsSmall">(Do not Enter the % sign)</span></td>
        </tr>
        <tr>
            <td align="right" colspan="2" style="height: 15px">
            </td>
        </tr>
        <tr>
            <td style="background-image: url(/PartnerPortal/Images/topMain.gif); height: 25px"
                align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    TabIndex="20" /></td>
        </tr>
    </table>
</asp:Content>
