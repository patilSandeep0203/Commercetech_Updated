<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditInfo.aspx.cs" Inherits="EditInfo" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <asp:ValidationSummary ID="ValidateSummary" runat="server" BackColor="#FFC0C0" BorderColor="red"
        BorderWidth="1px" ForeColor="Black" HeaderText="Please check the fields marked in red."
        Width="250px" />
       
    <span class="LabelsRed"><b>*</b> - denotes a required field</span>
    <table border="0" cellspacing="0" cellpadding="0" class="DivLightGray">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>  
        <tr>
            <td align="center" style="height: 20px;width: 100%" class="DivBlue">
                <b><span class="MenuHeader">Update Account Information</span></b>
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <table border="0" style="width: 500px" cellpadding="2" cellspacing="0">
                    <tr>
                        <td align="right" style=" width: 40%;">
                            <span class="LabelsSmall">User Name</span>
                            </td>
                        <td style=" width: 60%;" align="left">
                            <asp:Label ID="lblUserName" runat="server" Font-Bold="True"></asp:Label>
                            &nbsp; <a
                                class="LinkXSmall" href="ChangePWD.aspx"><b>
                                    Click here to change password</b></a>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span class="LabelsSmall">Password Phrase Reminder</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtPasswordPhrase" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                runat="server" ControlToValidate="txtPasswordPhrase" ErrorMessage="Password Phrase Reminder"
                                EnableClientScript="False"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">First Name</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtFirstName" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*</span>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                runat="server" ControlToValidate="txtFirstName" ErrorMessage="First Name" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Last Name</span></td>
                        <td align="left" >
                            <asp:TextBox ID="txtLastName" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                runat="server" ControlToValidate="txtLastName" EnableClientScript="False" ErrorMessage="Last Name"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Make Check Payable To</span></td>
                        <td  align="left">
                            <asp:RadioButton ID="rdbLegalName" runat="server" GroupName="LegalDBA" Text="Legal Name OR " />
                            <br />
                            <asp:RadioButton ID="rdbDBA" runat="server" GroupName="LegalDBA" Text="DBA Name" />
                            <span class="LabelsRedLarge">*</span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Legal Name (Full Personal Name as Company Legal Name if Sole Proprietorship)</span></td>
                        <td  align="left">
                            <asp:Label ID="lblCompanyName" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">DBA Name</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtDBAName" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                runat="server" ControlToValidate="txtDBAName" ErrorMessage="DBA" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Legal Status</span></td>
                        <td align="left" >
                            <asp:DropDownList ID="lstLegalStatus" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Corporation</asp:ListItem>
                                <asp:ListItem>Government</asp:ListItem>
                                <asp:ListItem>Sole Proprietorship</asp:ListItem>
                                <asp:ListItem>Legal/Medical Corp.</asp:ListItem>
                                <asp:ListItem>Int'l Org.</asp:ListItem>
                                <asp:ListItem>LLC</asp:ListItem>
                                <asp:ListItem>Non-Profit</asp:ListItem>
                                <asp:ListItem>Other</asp:ListItem>
                                <asp:ListItem>Partnership</asp:ListItem>
                                <asp:ListItem>Sole Proprietorship</asp:ListItem>
                                <asp:ListItem>Tax Exempt</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right"  valign="top">
                            <asp:RadioButton ID="rdbTaxID" runat="server" GroupName="TaxSSN" Text="Federal Tax ID OR" /><br />
                            <asp:RadioButton ID="rdbSSN" runat="server" GroupName="TaxSSN" Text="Social Security Number" /></td>
                        <td  align="left">
                            <asp:TextBox ID="txtTaxSSN" runat="server" MaxLength="9" Width="140px" >                                                       
                            </asp:TextBox>
                              <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTaxSSN"
                                            FilterType="Custom, Numbers" />
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                runat="server" ControlToValidate="txtTaxSSN" ErrorMessage="Federal Tax ID or Social Security"
                                EnableClientScript="False"></asp:RequiredFieldValidator>
                                </span>
                                 <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" Visible="false"  OnClick="lnkEdit_Click"></asp:LinkButton>   
                                </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Email</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtEmail" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                                runat="server" ControlToValidate="txtEmail" ErrorMessage="Email" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" >
                            <span class="LabelsSmall"><b>Business Address</b></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Address</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtAddress" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator7"
                                runat="server" ControlToValidate="txtAddress" ErrorMessage="Address" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">City</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtCity" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator8"
                                runat="server" ControlToValidate="txtCity" ErrorMessage="City" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">State</span></td>
                        <td  align="left">
                            <asp:DropDownList ID="lstState" runat="server">
                            </asp:DropDownList>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator9"
                                runat="server" ControlToValidate="lstState" ErrorMessage="State" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Region</span></td>
                        <td align="left" >
                            <asp:TextBox ID="txtBusRegion" runat="server" Width="140px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Zip</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtZip" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator10"
                                runat="server" ControlToValidate="txtZip" ErrorMessage="Zip" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Country</span></td>
                        <td  align="left">
                            <asp:DropDownList ID="lstCountry" runat="server">
                            </asp:DropDownList>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator11"
                                runat="server" ControlToValidate="lstCountry" ErrorMessage="Country" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" >
                            <span class="LabelsSmall"><b>Mailing Address (Checks will be mailed to this address)</b></span>
                            <asp:CheckBox ID="chkMailingSame" runat="server" AutoPostBack="True" OnCheckedChanged="chkMailingSame_CheckedChanged"
                                Text="Same as Business Address" /></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Address</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtMailingAddress" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator13"
                                runat="server" ControlToValidate="txtMailingAddress" ErrorMessage="Address"
                                EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">City</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtMailingCity" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator14"
                                runat="server" ControlToValidate="txtMailingCity" ErrorMessage="City" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">State</span></td>
                        <td  align="left">
                            <asp:DropDownList ID="lstMailingState" runat="server" >
                            </asp:DropDownList>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator15"
                                runat="server" ControlToValidate="lstMailingState" ErrorMessage="State" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Region</span></td>
                        <td align="left" >
                            <asp:TextBox ID="txtMailingRegion" runat="server" Width="140px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Zip</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtMailingZip" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator16"
                                runat="server" ControlToValidate="txtMailingZip" ErrorMessage="Zip" EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Country</span></td>
                        <td  align="left">
                            <asp:DropDownList ID="lstMailingCountry" runat="server">
                            </asp:DropDownList>
                            <span class="LabelsRedLarge">*<asp:RequiredFieldValidator ID="RequiredFieldValidator17"
                                runat="server" ControlToValidate="lstMailingCountry" ErrorMessage="Country"
                                EnableClientScript="False"></asp:RequiredFieldValidator></span></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" >
                        </td>
                    </tr>

                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Business Phone</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtPhone" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                                runat="server" ControlToValidate="txtPhone" ErrorMessage="Business Phone" EnableClientScript="False"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Home Phone</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtHomePhone" runat="server" Width="140px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Mobile Phone</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtMobilePhone" runat="server" Width="140px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Fax</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtFax" runat="server" Width="140px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Website URL http://www.</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtURL" runat="server" Width="140px"></asp:TextBox>
                            <span class="LabelsRedLarge"></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Comments</span></td>
                        <td  align="left">
                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="140px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <span class="LabelsSmall">Notify me by Email when I make a Sale</span></td>
                        <td align="left" >
                            <asp:DropDownList ID="lstNotify" runat="server">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <span class="LabelsSmall">			<asp:HyperLink ID="lnkAddImage" runat="server" CssClass="One" NavigateUrl="~/UploadLogo.aspx" Target="_blank" Font-Bold="True" Font-Names="Arial" Font-Size="Small">
			<br />
			Click here to Upload a Logo<br />
			</asp:HyperLink></span></td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 198px; height: 25px">
                            <span class="LabelsSmall">Do you want to sign up for Direct Deposit?</span></td>
                        <td align="left" >
                            <asp:RadioButton ID="rdbDDYes" runat="server" GroupName="DD" OnCheckedChanged="rdbDDYes_CheckedChanged"
                                Text="Yes" AutoPostBack="True" />&nbsp;<asp:RadioButton ID="rdbDDNo" runat="server"
                                    GroupName="DD" OnCheckedChanged="rdbDDYes_CheckedChanged" Text="No" AutoPostBack="True" /></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Panel ID="pnlBanking" runat="server" Width="100%" Visible="False">
                                <table border="0" style="width: 100%" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td align="center" colspan="3"  class="DivBlue">
                                            <strong><span class="MenuHeader">Bank Account Information</span></strong></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Bank Name</span>
                                            </td>
                                        <td align="left" colspan="2" valign="middle">
                                            &nbsp;<asp:Label ID="lblBankName" Font-Bold="true" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">If Other, specify</span>
                                            </td>
                                        <td align="left" colspan="2" valign="middle">
                                            <strong>&nbsp;<asp:Label ID="lblOtherBank" runat="server" Font-Bold="True"></asp:Label></strong></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Bank Address</span>
                                            </td>
                                        <td align="left" colspan="2" valign="middle">
                                            &nbsp;<asp:Label ID="lblBankAddress" runat="server" Font-Bold="True"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Bank City</span>
                                            </td>
                                        <td colspan="2" align="left" valign="middle">
                                            &nbsp;<asp:Label ID="lblBankCity" Font-Bold="True" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Zip Code</span>
                                            </td>
                                        <td align="left" colspan="2" valign="middle">
                                            <strong>&nbsp;<asp:Label ID="lblZipCode" runat="server" Font-Bold="True"></asp:Label></strong></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">State</span>
                                            </td>
                                        <td align="left" colspan="2" valign="middle">
                                            &nbsp;<asp:Label ID="lblBankState" runat="server" Font-Bold="True"></asp:Label>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Bank Phone Number</span>
                                            </td>
                                        <td colspan="2" align="left" valign="middle">
                                            &nbsp;<asp:Label Font-Bold="True" ID="lblBankPhone" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Name Exactly As It Appears On Checking Account</span>
                                        </td>
                                        <td colspan="2" align="left" valign="middle">
                                            &nbsp;<asp:Label ID="lblNameOnChecking" Font-Bold="true" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Bank Routing Number</span>
                                            </td>
                                        <td colspan="2" align="left" valign="middle">
                                            &nbsp;<asp:Label ID="lblBankRoutingNumber" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%" valign="middle">
                                            <span class="LabelsSmall">Bank Account Number</span>
                                        </td>
                                        <td colspan="2" align="left" valign="middle">
                                            &nbsp;<asp:Label ID="lblAcctNumber" runat="server" Font-Bold="True"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <span style="font-size: 10pt; font-family:Arial">
                                                <b><!--<asp:HyperLink ID="lnkEditInfo" CssClass="One" runat="server" NavigateUrl="EditBanking.aspx">Edit Bank Account Information</asp:HyperLink>-->
                                                <asp:LinkButton ID="lnkEditBankInfo" runat="server" Text="Edit Bank Account Information" OnClick="lnkEditBankInfo_Click"></asp:LinkButton></b></span></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2"  valign="middle">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
