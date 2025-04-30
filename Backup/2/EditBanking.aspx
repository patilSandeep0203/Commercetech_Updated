<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="EditBanking.aspx.cs"
    Inherits="EditBanking" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <asp:ValidationSummary ID="ValidateSummary" runat="server"
            BackColor="#FFC0C0" BorderColor="red" BorderWidth="1px" ForeColor="Black" HeaderText="Please check the fields marked in red."
            Width="250px" />
    <span class="LabelsRedLarge"><b>*</b> - denotes
        a required field</span>
    <br />
    <asp:Panel ID="pnlMainPage" runat="server" Width="500px">
    <table border="0" cellspacing="0" cellpadding="2" style="width:500px" class="DivLightGray">
        <tr>
            <td align="center" colspan="3" class="DivBlue">
                <strong><span class="MenuHeader">Bank Account Information</span></strong></td>
        </tr>
        <tr>
            <td style="height:10px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <span class="LabelsSmall">Bank Name</span></td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstBankName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstBankName_SelectedIndexChanged"
                    TabIndex="1">
                </asp:DropDownList><span class="LabelsRedLarge">*</span>
                <asp:RequiredFieldValidator ID="ValidateBank" runat="server" ControlToValidate="lstBankName"
                    EnableClientScript="False" ErrorMessage="Bank Name" Font-Bold="False"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <span class="LabelsSmall">If Other, spcify</span></td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtOtherBank" runat="server" Enabled="False" MaxLength="64" TabIndex="2"
                    Width="140px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ValidateOtherBank" runat="server" ControlToValidate="txtOtherBank"
                    EnableClientScript="False" Enabled="False" ErrorMessage="Other Bank Name" Font-Bold="False"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <span class="LabelsSmall">Bank Address</span></td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtBankAddress" runat="server" MaxLength="96" TabIndex="3" Width="140px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <span class="LabelsSmall">City</span></td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtBankCity" runat="server" MaxLength="64" TabIndex="4" Width="140px"></asp:TextBox>
                <span class="LabelsRedLarge">*</span>
                <asp:RequiredFieldValidator ID="ValidateCity" runat="server" ControlToValidate="txtBankCity"
                    EnableClientScript="False" ErrorMessage="Bank City" Font-Bold="False"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%" valign="middle">
                <span class="LabelsSmall">Zip Code</span></td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtZipCode" runat="server" MaxLength="30" TabIndex="5" Width="140px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%" valign="middle">
                <span class="LabelsSmall">State</span></td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstBankState" runat="server" TabIndex="6"></asp:DropDownList>
                <span class="LabelsRedLarge">*</span>
                <asp:RequiredFieldValidator ID="ValidateState" runat="server" ControlToValidate="lstBankState"
                    EnableClientScript="False" ErrorMessage="State" Font-Bold="False"></asp:RequiredFieldValidator>&nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <span class="LabelsSmall">Bank Phone Number</span></td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtPhone" runat="server" MaxLength="25" TabIndex="8" Width="140px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server"
                    ControlToValidate="txtPhone" EnableClientScript="False" ErrorMessage="Invalid Phone Number"
                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%">
                <span class="LabelsSmall">Name Exactly As It Appears On Checking Account</span>
            </td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtNameOnChecking" runat="server" MaxLength="50" TabIndex="9" Width="140px"></asp:TextBox>
                <span class="LabelsRedLarge">*</span>
                <asp:RequiredFieldValidator ID="ValidateCheckingAcct" runat="server" ControlToValidate="txtNameOnChecking"
                    EnableClientScript="False" ErrorMessage="Name on Checking Acct." Font-Bold="False"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%" valign="top">
                <span class="LabelsSmall">Bank Routing Number</span></td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtBankRoutingNumber" runat="server" MaxLength="9" TabIndex="10" Width="140px"></asp:TextBox>
                    <span class="LabelsRedLarge">*</span>
                    <span class="LabelsSmall">(9 digits long)</span>
                <asp:RequiredFieldValidator ID="ValidateRoutingNo" runat="server" ControlToValidate="txtBankRoutingNumber"
                    EnableClientScript="False" ErrorMessage="Routing Number"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RERoutingNum" runat="server" ControlToValidate="txtBankRoutingNumber"
                    EnableClientScript="False" ErrorMessage="Numbers Only" ValidationExpression="[0-9]*$"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right" style="width: 40%" valign="top">
                <span class="LabelsSmall">Bank Account Number</span>
            </td>
            <td align="left" colspan="2">
                <asp:TextBox ID="txtAcctNumber" runat="server" MaxLength="16" TabIndex="11" Width="140px"></asp:TextBox>
                <span class="LabelsRedLarge">*</span>
                <asp:RequiredFieldValidator ID="ValidateAcctNo" runat="server" ControlToValidate="txtAcctNumber"
                    EnableClientScript="False" ErrorMessage="Account Number"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="REAcctNum" runat="server" ControlToValidate="txtAcctNumber"
                    EnableClientScript="False" ErrorMessage="Numbers Only" ValidationExpression="[0-9]*$"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right" valign="top">
                <asp:HyperLink ID="lnkAuthAgree" runat="server" CssClass="One" Font-Names="Arial" Font-Size="Small"
                    NavigateUrl="~/Direct Deposit Authorization Form.pdf" TabIndex="37" Target="_blank">Authorization Agreement</asp:HyperLink></td>
            <td align="left" colspan="2">
                <span class="LabelsSmall">Download, complete and fax this form to (310) 321-5411.</span></td>
        </tr>
        <tr>
            <td align="center" colspan="3"  valign="middle">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />               
                &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></td>
        </tr>        
    </table>
    </asp:Panel>
</asp:Content>
