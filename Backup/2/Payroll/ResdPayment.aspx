<%@ Page Language="C#" MasterPageFile="~/Payroll/Admin.master" AutoEventWireup="true" CodeFile="ResdPayment.aspx.cs" Inherits="Payroll_ResdSummary" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManagerComm" runat="server"  EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDatePaid">
    </cc1:CalendarExtender>
<asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="background-image: url(../Images/topMain.gif);
                height: 25px">
                <b><span class="MenuHeader">Residual Payment Summary</span></b></td>
        </tr>
        <tr>
            <td colspan="3" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblSelectMonth" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Select Month"></asp:Label>&nbsp;</td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstMonth" runat="server">

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
    </table>
    <br />
    <asp:HyperLink ID="lnkPrint" runat="server" CssClass="One" Font-Bold="True" Font-Names="Arial"
        Visible="false" Font-Size="Small" NavigateUrl="CommRefPaymentPrint.aspx" Target="_blank">Print Payments</asp:HyperLink><br />
    <br />
    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label><br />
    <asp:Panel ID="pnlConfirmation" runat="server" Visible="False" Width="750px">
        <asp:Label ID="lblUpdateHeader" runat="server" Font-Bold="True" Text="Update Information"></asp:Label>
        <table style="border-right: silver 1px solid; border-top: silver 1px solid; border-left: silver 1px solid;
            width: 745px; border-bottom: silver 1px solid">
            <tr>
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Confirmation</span></b></td>
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <strong><span style="color: #ffffff; font-family: Arial; font-size:small">Date Paid</span></strong></td>                    
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Note</span></b></td> 
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Partner ID</span></b>
                </td>
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Company</span></b></td>
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Residual</span></b></td>   
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Carryover</span></b></td>    
                <td style="background-image: url(/PartnerPortal/Images/topMain.gif)">
                    <b><span class="MenuHeader">Payment</span></b></td>                                                           
            </tr>
            <tr>
                <td style="background-color: #fbfbfb">
                    <asp:TextBox ID="txtConfirmationCode" runat="server" Width="80px" MaxLength="32"></asp:TextBox>
                </td>
                <td style="background-color: #fbfbfb">
                <asp:TextBox ID="txtDatePaid" runat="server" Width="120px" MaxLength="32"></asp:TextBox>
                </td>
                <td style="background-color: #fbfbfb">
                    <asp:TextBox ID="txtNote" runat="server" Width="160px" MaxLength="32"></asp:TextBox>
                </td>
                <td style="background-color: #fbfbfb">
                    <asp:Label ID="lblAffiliateID" runat="server" Font-Bold="True"></asp:Label></td>
                <td style="background-color: #fbfbfb">
                    <asp:Label ID="lblCompanyName" runat="server" Font-Bold="True"></asp:Label></td>
                <td style="background-color: #fbfbfb">                        
                    <asp:Label ID="lblResidual" runat="server" Width="40px" Font-Bold="True"></asp:Label>                     
                </td>
                 <td style="background-color: #fbfbfb">                        
                    <asp:TextBox ID="txtCarryOver" runat="server" Width="40px" MaxLength="8" ></asp:TextBox>                                     
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCarryOver"
                                            FilterType="Custom, Numbers" ValidChars=". -" /> 
                <td style="background-color: #fbfbfb">                        
                    <asp:TextBox ID="txtPayment" runat="server" Width="40px" MaxLength="8" ></asp:TextBox>                                     
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPayment"
                                            FilterType="Custom, Numbers" ValidChars=". -" /> 
            </tr>
            <tr>
                <td colspan="3" style="background-color: #fbfbfb">
                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click"
                        Text="Cancel" /></td>  
                <td colspan="5" align=right valign=bottom><span style="color: red; font-family: Arial; font-size:x-small">NOTE: </span>
                <span style="color:black; font-family: Arial; font-size:x-small">Payment will be updated once Confirmation Code is entered.</span></td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlEmployees" runat="server" Visible="False" Width="750px">
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 750px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Employees</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="grdEmployeeSummary" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdEmployeeSummary_RowCommand"
                        OnRowDataBound="grdEmployeeSummary_RowDataBound" ShowFooter="True" Width ="745px">
                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                        <Columns>
                            <asp:ButtonField CommandName="Confirmation" Text="Edit" FooterStyle-Font-Size="Small" FooterText="SubTotals:">
                                <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="ResdConfirmNum" HeaderText="Confirmation" />
                            <asp:BoundField DataField="ResdConfirmDate" HeaderText="Date Paid" />
                            <asp:BoundField DataField="ResdNote" HeaderText="Note" />
                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company">
                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Residual" HeaderText="Residual">
                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CarryoverResd" HeaderText="Carryover">
                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Total">
                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Payment" HeaderText="Payment">
                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                            ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
            <tr></tr>
            <tr>
                <td align="right">
                    <!--<asp:Table ID="tblEmployeeTotal" runat="server" CellPadding="2" CellSpacing="0" GridLines="Both" BackColor="#edf7ff" Width="700px">
                    </asp:Table></td>-->
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlPartners" runat="server" Visible="False" Width="750px">
        <table border="0" cellspacing="0" style="background-color: #fbfbfb; border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            width: 750px">
            <tr>
                <td align="center" style="background-color:#5c995d;">
                    <b><span style="color: white; font-family: Arial;">Partners</span></b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlDDPartner" runat="server" Width="747px">
                        <table border="0" cellspacing="0" style="width: 747px">
                            <tr>
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size: small">Direct Deposit</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdSummaryDD" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdSummaryDD_RowCommand"
                                        OnRowDataBound="grdSummaryDD_RowDataBound" ShowFooter="True" Width ="745px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:ButtonField CommandName="Confirmation" Text="Edit" FooterStyle-Font-Size="Small" FooterText="SubTotals:">
                                                <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="ResdConfirmNum" HeaderText="Confirmation" />
                                            <asp:BoundField DataField="ResdConfirmDate" HeaderText="Date Paid" />
                                            <asp:BoundField DataField="ResdNote" HeaderText="Note" />
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Residual" HeaderText="Residual">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarryoverResd" HeaderText="Carryover">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Total">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Payment" HeaderText="Payment">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                        </Columns>
                                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                                        <EditRowStyle BackColor="#999999" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                                            ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <!--<td align="right">
                                    <asp:Table ID="tblDDTotal" runat="server" CellPadding="2" CellSpacing="0" GridLines="Both" BackColor="#edf7ff" Width="700px">
                                </asp:Table></td>-->
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlBPPartners" runat="server" Width="747px">
                        <table border="0" cellspacing="0" style="width: 747px">
                            <tr>
                                <td align="center">
                                    <b><span style="color: #383838; font-family: Arial; font-size: small">Bill Pay</span></b>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="grdSummaryBP" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdSummaryBP_RowCommand"
                                        OnRowDataBound="grdSummaryBP_RowDataBound" ShowFooter="True" Width ="745px">
                                        <FooterStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" ForeColor="#284775" />
                                        <Columns>
                                            <asp:ButtonField CommandName="Confirmation" Text="Edit" FooterStyle-Font-Size="Small" FooterText="SubTotals:">
                                                <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="ResdConfirmNum" HeaderText="Confirmation" />
                                            <asp:BoundField DataField="ResdConfirmDate" HeaderText="Date Paid" />
                                            <asp:BoundField DataField="ResdNote" HeaderText="Note" />
                                            <asp:BoundField DataField="AffiliateID" HeaderText="Partner ID" />
                                            <asp:BoundField DataField="Contact" HeaderText="Contact" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Residual" HeaderText="Residual">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarryoverResd" HeaderText="Carryover">
                                                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Total" />
                                            <asp:BoundField DataField="Payment" HeaderText="Payment" />
                                        </Columns>
                                        <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                                        <EditRowStyle BackColor="#999999" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                                            ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Table ID="tblBPTotal" runat="server" Font-Size="Medium" CellPadding="2" CellSpacing="0" GridLines="Both" BackColor="#edf7ff" Width="750px" ForeColor="White"></asp:Table></td>
                            </tr>
                            </table>
                    </asp:Panel>
                    </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <br />
</asp:Content>

