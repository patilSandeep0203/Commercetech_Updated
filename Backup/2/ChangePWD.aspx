<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ChangePWD.aspx.cs" Inherits="ChangePWD" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <br />
    <asp:ScriptManager ID="ScriptManagerPass" runat="server">
    </asp:ScriptManager>
    <cc1:PasswordStrength ID="PasswordStrength1" runat="server" TargetControlID="txtPassword"
        DisplayPosition="RightSide" StrengthIndicatorType="Text" PreferredPasswordLength="6"
        PrefixText="Strength:" TextCssClass="TextIndicator_TextBox1" TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
        MinimumNumericCharacters="0" MinimumSymbolCharacters="0" RequiresUpperAndLowerCaseCharacters="false" />
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server"
        Visible="False"></asp:Label><br />
    <table width="400px" border="0" cellspacing="0" cellpadding="2" class="DivLightGray">
        <tr>
            <td style="height: 20px;" align="center" colspan="2" class="DivBlue">
                <b><span class="MenuHeader">Password Change</span></b>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 10px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 40%" valign="top">
                <span class="LabelsSmall">New Password</span></td>
            <td align="left">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                <span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                    runat="server" ControlToValidate="txtPassword" EnableClientScript="False" ErrorMessage="Password"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPasswordConfirm"
                    ControlToValidate="txtPassword" EnableClientScript="False" ErrorMessage="Passwords do not match"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <span class="LabelsSmall">Confirm Password</span></td>
            <td align="left">
                <asp:TextBox ID="txtPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox>
                <span class="LabelsRedLarge">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                    runat="server" ControlToValidate="txtPasswordConfirm" EnableClientScript="False"
                    ErrorMessage="Confirm Password"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></td>
        </tr>
        <tr>
            <td colspan="2" style="height: 10px">
            </td>
        </tr>
    </table>
</asp:Content>
