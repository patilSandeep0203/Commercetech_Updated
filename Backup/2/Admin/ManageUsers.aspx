<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true"
    CodeFile="ManageUsers.aspx.cs" Inherits="ManageUsers" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManagerUsers" runat="server">
    </asp:ScriptManager>
    <br />    
    <table border="0" cellpadding="0" cellspacing="0" style="width: 300px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="background-image: url(../Images/topMain.gif);
                height: 25px">
                <span class="MenuHeader"><b>Manage Users</b></span>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblAppName" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"
                    Text="Select App Name"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                &nbsp;<asp:DropDownList ID="lstAppList" runat="server">
                    <asp:ListItem Value="1">Partner</asp:ListItem>
                    <asp:ListItem Value="3">ACT</asp:ListItem>
                    <asp:ListItem Value="4">Reports</asp:ListItem>
                    <asp:ListItem Value="5">Online App Mgmt</asp:ListItem>
                    <asp:ListItem Value="6">CTC</asp:ListItem>
                    <asp:ListItem Value="7">Payroll</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblAccess" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"
                    Text="Select Access"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                &nbsp;<asp:DropDownList ID="lstSelAccess" runat="server">
                    <asp:ListItem>ALL</asp:ListItem>
                    <asp:ListItem>Employee</asp:ListItem>
                    <asp:ListItem>Affiliate</asp:ListItem>
                    <asp:ListItem>Admin</asp:ListItem>
                    <asp:ListItem>Reseller</asp:ListItem>                    
                    <asp:ListItem>Agent</asp:ListItem>
                    <asp:ListItem>T1Agent</asp:ListItem>
                    <asp:ListItem>None</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3" style="height: 5px">
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="500">
        <ProgressTemplate>
            <div class="DivHelp" style="width: 50%">
                <span class="LabelsRed"><b>Please Wait...</b></span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanelUsers" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
            <asp:Panel ID="pnlUpdate" runat="server" Visible="False" Width="750px">
                <asp:Panel ID="pnlConfirm" runat="server" BackColor="#FFC0C0" BorderColor="Salmon"
                    BorderStyle="Double" Height="50px" Visible="False" Width="200px">
                    <asp:Label ID="lblMessage" runat="server" Text="Are you sure you want to reset the password?"></asp:Label><br />
                    <asp:Button ID="btnCreateRecordYes" runat="server" OnClick="btnCreateRecordYes_Click"
                        Text="Yes" />
                    <asp:Button ID="btnCreateRecordNo" runat="server" OnClick="btnCreateRecordNo_Click"
                        Text="No" /></asp:Panel>
                <table style="border-right: silver 1px solid; border-top: silver 1px solid; border-left: silver 1px solid;
                    width: 700px; border-bottom: silver 1px solid; background-color: #fbfbfb">
                    <tr>
                        <td colspan="8" align="center" style="background-color: #5D7B9D">
                            <b><span class="MenuHeader">User Roles</span></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="1" class="DivGreen">
                            <span class="LabelsSmall"><b>Partner ID: </b></span>
                            <asp:Label ID="lblAffiliateID" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                        <td colspan="3" align="center" class="DivGreen">
                            <span class="LabelsSmall"><b>Contact: </b></span>
                            <asp:Label ID="lblContact" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="2" align="center" class="DivGreen">
                            <span class="LabelsSmall"><b>Login Name: </b></span>
                            <asp:Label ID="lblLoginName" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="2" align="center" class="DivGreen">
                            <span class="LabelsSmall"><b>Password Hint: </b></span>
                            <asp:Label ID="lblPasswordHint" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblPartner" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                Text="Partner"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="Employee" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="Tier 1 Agent" /></td>                                
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPartnerNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PartnerGroup" Text="No Access" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblACT" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                Text="ACT"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbACTAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbACTEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="Employee" /></td>
                                
                       <td align="center">
                            <asp:RadioButton ID="rdbACTT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="Tier 1 Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbACTAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbACTReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbACTAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbACTNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ACTGroup" Text="No Access" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblReports" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                Text="Reports"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="Employee" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="Tier 1 Agent" /></td>                                                    
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbReportsNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="ReportsGroup" Text="No Access" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblOnlineAppMgmt" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" Text="Online App Management"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="Employee" /></td>                                
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="Tier 1 Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbMgmtNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="MgmtGroup" Text="No Access" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblCTC" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                Text="CTC"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbCTCAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbCTCEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="Employee" /></td>
                         <td align="center">
                            <asp:RadioButton ID="rdbCTCT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="Tier 1 Agent" /></td>                                
                        <td align="center">
                            <asp:RadioButton ID="rdbCTCAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbCTCReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbCTCAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbCTCNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="CTCGroup" Text="No Access" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblPayroll" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                Text="Payroll"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPayrollAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPayrollEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="Employee" /></td>
                         <td align="center">
                            <asp:RadioButton ID="rdbPayrollT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="Tier 1 Agent" /></td>                                
                        <td align="center">
                            <asp:RadioButton ID="rdbPayrollAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPayrollReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPayrollAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbPayrollNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="PayrollGroup" Text="No Access" /></td>
                    </tr>
                                        <tr>
                        <td align="center">
                            <asp:Label ID="lblDocs" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                Text="Documents And Logins"></asp:Label></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbDocAdmin" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="Admin" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbDocEmployee" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="Employee" /></td>
                         <td align="center">
                            <asp:RadioButton ID="rdbDocT1Agent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="Tier 1 Agent" /></td>                                
                        <td align="center">
                            <asp:RadioButton ID="rdbDocAgent" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="Agent" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbDocReseller" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="Reseller" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbDocAffiliate" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="Affiliate" /></td>
                        <td align="center">
                            <asp:RadioButton ID="rdbDocNoAccess" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="Small" GroupName="DocsGroup" Text="No Access" /></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click"
                                Text="Cancel" />
                        </td>
                        <td align="center" colspan="3">
                            <asp:Button ID="btnUpdateRoles" runat="server" Text="Update Roles" OnClick="btnUpdateRoles_Click" /></td>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblNewPassword" runat="server" Font-Bold="True" Visible="False" BackColor="Ivory"
                BorderColor="Silver" BorderWidth="1px"></asp:Label><br />
            <asp:GridView ID="grdManageUsers" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdManageUsers_RowCommand">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" SortExpression="AffiliateID">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LoginName" HeaderText="Login Name" />
                    <asp:BoundField DataField="Access" HeaderText="Access Level" SortExpression="Access">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastLogin" HeaderText="Last Login" />
                    <asp:ButtonField CommandName="EditInfo" Text="Edit">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
                </Columns>
                <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
