<%@ Page Language="C#" MasterPageFile="~/ACT/Admin.master" AutoEventWireup="true" CodeFile="CreateACTPDF.aspx.cs"
    Inherits="CreateACTPDF" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="panSearch" runat="server" DefaultButton="btnSubmit" Width="100%" >
        <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
        <table cellpadding="0" cellspacing="0" border="0" style="width: 300px;" class="SilverBorder">
            <tr>
                <td align="center" colspan="3" style="height: 25px; background-image: url(../Images/topMain.gif)">
                    <b><span class="MenuHeader">Create PDF from ACT!</span></b>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 23%" valign="top">
                    <asp:Label ID="lblEmailHeader" Font-Bold="True" runat="server" Text="Look up by "></asp:Label>&nbsp;
                </td>
                <td align="left">
                    &nbsp;<asp:DropDownList ID="lstLookup" runat="server">
                        <asp:ListItem>Email</asp:ListItem>
                        <asp:ListItem>Contact</asp:ListItem>
                        <asp:ListItem>CompanyName</asp:ListItem>
                        <asp:ListItem>DBA</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                    <asp:TextBox ID="txtLookup" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLookup"
                        EnableClientScript="False" ErrorMessage="Please enter the value to look up"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="height: 5px" colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_Click" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" colspan="2">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblDownload" runat="server" Font-Bold="True" Font-Names="Arial"></asp:Label><br />
    <asp:Panel ID="pnlChasePDF" runat="server" BackColor="Ivory" BorderColor="Red" BorderWidth="1px"
        Width="300px" Visible="False">
        <strong><span style="font-family:Arial; font-size:small; color:#383838">Choose the Chase PDF for</span>
            <asp:Label ID="lblDBASel" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>:<br />
            <span style="font-size: 8pt">
                <!--<asp:LinkButton ID="btnChaseAbout" runat="server" OnClick="btnChaseAbout_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">About Merchant</asp:LinkButton><br />
                <asp:LinkButton ID="btnChaseFee" runat="server" OnClick="btnChaseFee_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Fee Schedule</asp:LinkButton>
                <br />
                <asp:LinkButton ID="btnChaseMP" runat="server" OnClick="btnChaseMP_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Merchant Processing</asp:LinkButton><br />
                <asp:LinkButton ID="btnCreditAdd" runat="server" OnClick="btnChaseCreditAdd_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Credit Addendum</asp:LinkButton><br />
                <asp:HyperLink ID="lnkOpGuide" Font-Size="10pt" Font-Names="Arial" CssClass="One" runat="server" NavigateUrl="~/PDF/Chase Operating Guide.pdf" Target="_blank">Chase Operating Guide</asp:HyperLink>-->
                <asp:LinkButton ID="btnChaseMPA" runat="server" CssClass="One" Font-Names="Arial"
                    Font-Size="10pt" OnClick="btnChaseMPA_Click" CausesValidation="False">Chase Merchant Application and Agreement</asp:LinkButton><br />    
                <asp:LinkButton ID="btnChaseFS3Tier" runat="server" CssClass="One" Font-Names="Arial"
                    Font-Size="10pt" OnClick="btnChaseFS3Tier_Click" Visible="false" CausesValidation="False">Chase Fee Schedule 3 tier</asp:LinkButton><br />    
                <asp:LinkButton ID="btnChaseFSInterchangePlus" runat="server" CssClass="One" Font-Names="Arial"
                    Font-Size="10pt" OnClick="btnChaseFSInterchangePlus_Click" Visible="false" CausesValidation="False">Chase Fee Schedule Interchange Plus</asp:LinkButton><br />    
            </span></strong></asp:Panel>
    <asp:Panel ID="pnlSagePDF" runat="server" BackColor="Ivory" BorderColor="Red" BorderWidth="1px"
        Width="300px" Visible="False">
        <strong><span style="font-family:Arial; font-size:small; color:#383838">Choose the Sage PDF for</span>
            <asp:Label ID="lblDBASel2" runat="server" Font-Names="Arial" Font-Size="11pt"></asp:Label>:<br />
            <span style="font-size: 8pt">
                <asp:LinkButton ID="btnSageApp" runat="server" OnClick="btnSageApp_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Sage Application</asp:LinkButton><br />
                <!--<asp:LinkButton ID="btnSageMOTO" runat="server" OnClick="btnSageMOTO_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Sage MOTO Application</asp:LinkButton><br />-->
                <asp:LinkButton ID="btnSageAgreement" runat="server" OnClick="btnSageAgreement_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Sage Agreement</asp:LinkButton>
                <!--<asp:HyperLink ID="lnkSageAgreement" Font-Size="10pt" Font-Names="Arial" CssClass="One" runat="server" NavigateUrl="~/PDF/Sage Merchant Agreement Ts & Cs_SPS_Rev 06.08.pdf" Target="_blank">Sage Agreement</asp:HyperLink>-->
            </span></strong></asp:Panel>
    <asp:Panel ID="pnlAddlServices" runat="server" BackColor="Ivory" BorderColor="Red" BorderWidth="1px"
        Width="250px" Visible=false>
        <strong><asp:Label ID="lblAddlServices" runat="server" Font-Names="Arial" Font-Size="11pt" Text="Choose the Additional Services PDF"></asp:Label>
            <span style="font-size: 8pt">
                <asp:LinkButton ID="btnRoamPayPDF" runat="server" OnClick="btnRoamPayPDF_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">ROAMpay PDF</asp:LinkButton>
                <asp:LinkButton ID="btnNorthernLeasePDF" runat="server" OnClick="btnNorthernLeasePDF_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Northern Lease PDF</asp:LinkButton><br />
                <asp:LinkButton ID="btnGETIGiftCardPDF" runat="server" OnClick="btnGETIGiftCardPDF_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Sage Payment Solutions EFT PDF</asp:LinkButton><br />
                <asp:LinkButton ID="btnAMIPDF" runat="server" OnClick="btnAMIPDF_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">AdvanceMe, Inc. PDF</asp:LinkButton>
                <asp:LinkButton ID="btnRapidAdvancePDF" runat="server" OnClick="btnRapidAdvancePDF_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">RapidAdvance PDF</asp:LinkButton>
                <asp:LinkButton ID="btnBFSPDF" runat="server" OnClick="btnBFSPDF_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Business Financial Services PDF</asp:LinkButton>
                <!--<asp:LinkButton ID="LinkButton3" runat="server" OnClick="btnSageAgreement_Click" CssClass="One" Font-Names="Arial" Font-Size="10pt">Sage Agreement</asp:LinkButton>-->
                <!--<asp:HyperLink ID="LinkButton4" Font-Size="10pt" Font-Names="Arial" CssClass="One" runat="server" NavigateUrl="~/PDF/Sage Merchant Agreement Ts & Cs_SPS_Rev 06.08.pdf" Target="_blank">Sage Agreement</asp:HyperLink>-->
                <asp:Label ID="lblNoAddlServices" runat="server" CssClass="LabelsRed" Text="No Additional Services PDFs found."></asp:Label>
            </span></strong></asp:Panel>
    <br />
    <asp:GridView ID="grdPDF" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdPDF_RowCommand">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="AppId" HeaderText="AppId" SortExpression="AppId">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="ContactID" HeaderText="ContactID" SortExpression="ContactID">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="RepName" HeaderText="Sales Rep">
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="Processor" HeaderText="Processor" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:ButtonField CommandName="CreatePDF" Text="Create PDF">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" />
                <HeaderStyle Font-Names="Arial" Font-Size="X-Small" />
            </asp:ButtonField>            
            <asp:BoundField DataField="LeaseStatus" HeaderText="Lease" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="GiftCardStatus" HeaderText="Gift Card" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="MCAStatus" HeaderText="Merchant Cash Advance" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:ButtonField Text="Create Addl Services PDF" CommandName="CreateASPDF">
            <ItemStyle Font-Names="Arial" Font-Size="X-Small" Font-Bold="True" />
                <HeaderStyle Font-Names="Arial" Font-Size="X-Small" />
            </asp:ButtonField>
        </Columns>
        <RowStyle Font-Names="Arial" Font-Size="X-Small" BackColor="#EDF7FF" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
                <!--<asp:BoundField DataField="Gateway" HeaderText="Gateway" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>
            <asp:BoundField DataField="GatewayStatus" HeaderText="Gateway Status" >
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:BoundField>-->
</asp:Content>
