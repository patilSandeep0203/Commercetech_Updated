<%@ Page Language="C#"  MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true"
    CodeFile="OnlineAppLeads.aspx.cs" Inherits="OnlineAppLeads" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script language="Javascript" type="text/javascript"> 
 function Cover(bottom, top, ignoreSize) {
        var location = Sys.UI.DomElement.getLocation(bottom);
        top.style.position = 'absolute';
        top.style.top = location.y + 'px';
        top.style.left = location.x + 'px';
        if (!ignoreSize) {
            top.style.height = bottom.offsetHeight + 'px';
            top.style.width = bottom.offsetWidth + 'px';
        }
    }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
    </ContentTemplate>
    </asp:UpdatePanel>    
    <table border="0" cellpadding="0" cellspacing="0" style="width: 300px;"  class="SilverBorder">
        <tr>
            <td align="center" colspan="3" style="background-image: url(../Images/topMain.gif);
                height: 25px">
                <b><span class="MenuHeader">firstaffiliates.com
                    Leads</span></b>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 50%">
                <asp:Label ID="lblLeadReport" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Select Lead Report"></asp:Label>&nbsp;
            </td>
            <td align="left" colspan="2">
                <asp:DropDownList ID="lstLeadReport" runat="server">
                    <asp:ListItem>Free Report</asp:ListItem>
                    <asp:ListItem>Free Consult</asp:ListItem>
                    <asp:ListItem>Free Apply</asp:ListItem>
                    <asp:ListItem>Partner Signups</asp:ListItem>
                </asp:DropDownList>
            </td>
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <asp:Image ID="imgProgress" runat="server" ImageUrl="/PartnerPortal/Images/indicator.gif" /><span class="LabelsRed"><b>Retrieving Data...Please Wait</b></span>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanelLeads" runat="server"><ContentTemplate>
<cc1:AnimationExtender id="AnimationExtender1" runat="server" TargetControlID="lnkbtnLookup">
                <Animations>
                    <OnClick>                        
                        <Sequence>                               
                            <ScriptAction Script="Cover($get('ctl00_ctl00_RootContent_MainContent_lnkbtnLookup'), $get('flyout'));" />
                            <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>                            
                            <ScriptAction Script="Cover($get('flyout'), $get('info'), true);" />
                            <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>                            
                            <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                            <StyleAction AnimationTarget="info" Attribute="height" value="auto" />
                            <Parallel Duration="0">
                                <Color AnimationTarget="info" StartValue="#383838" EndValue="#383838" Property="style" PropertyKey="color" />
                                <Color AnimationTarget="info" StartValue="#febd0d" EndValue="#383838" Property="style" PropertyKey="borderColor" />
                            </Parallel>
                            <Parallel Duration="0">
                                <Color AnimationTarget="info" StartValue="#383838" EndValue="#383838" Property="style" PropertyKey="color" />
                                <Color AnimationTarget="info" StartValue="#febd0d" EndValue="#383838" Property="style" PropertyKey="borderColor" />
                                <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />                            
                            </Parallel>                                
                        </Sequence>
                    </OnClick>
                </Animations>
            </cc1:AnimationExtender> <cc1:AnimationExtender id="AnimationExtender2" runat="server" TargetControlID="btnClose">
                <Animations>
                <OnClick>
                    <Sequence>
                        <StyleAction AnimationTarget="info" Attribute="overflow" Value="hidden"/>
                        <StyleAction AnimationTarget="info" Attribute="display" Value="none"/>
                        <StyleAction AnimationTarget="info" Attribute="width" Value="250px"/>
                        <StyleAction AnimationTarget="info" Attribute="height" Value=""/>
                        <StyleAction AnimationTarget="info" Attribute="fontSize" Value="12px"/>
                        <StyleAction AnimationTarget="btnCloseParent" Attribute="opacity" value="0" />
                        <StyleAction AnimationTarget="btnCloseParent" Attribute="filter" value="alpha(opacity=0)" />                        
                    </Sequence>
                </OnClick>
                <OnMouseOver>
                    <Color Duration="0" StartValue="#FFFFFF" EndValue="#FF0000" Property="style" PropertyKey="color" />                            
                </OnMouseOver>
                <OnMouseOut>
                    <Color Duration="0" EndValue="#FFFFFF" StartValue="#FF0000" Property="style" PropertyKey="color" />                            
                </OnMouseOut>
                </Animations>
            </cc1:AnimationExtender> <DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 2; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; BORDER-BOTTOM: #d0d0d0 1px solid; BACKGROUND-COLOR: #ffffff" id="flyout"></DIV><DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 10pt; Z-INDEX: 2; PADDING-BOTTOM: 5px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 250px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; FONT-FAMILY: Arial; BACKGROUND-COLOR: #f9f9d9; TEXT-ALIGN: center" id="info"><DIV style="FILTER: alpha(opacity=0); FLOAT: right; opacity: 0" id="btnCloseParent"><asp:LinkButton style="BORDER-RIGHT: white thin outset; PADDING-RIGHT: 5px; BORDER-TOP: white thin outset; PADDING-LEFT: 5px; FONT-WEIGHT: bold; PADDING-BOTTOM: 5px; BORDER-LEFT: white thin outset; COLOR: white; PADDING-TOP: 5px; BORDER-BOTTOM: white thin outset; BACKGROUND-COLOR: #666666; TEXT-ALIGN: center; TEXT-DECORATION: none" id="btnClose" runat="server" Text="" OnClientClick="return false;" ToolTip="Close">X</asp:LinkButton> <TABLE style="WIDTH: 100%; BACKGROUND-COLOR: ivory" cellSpacing=0 cellPadding=0 border=0><TBODY><TR><TD style="HEIGHT: 15px" align=right colSpan=3></TD></TR><TR><TD style="WIDTH: 30%" align=right><asp:Label id="lblLookUpBy" runat="server" Text="Look up By" Font-Size="11px" Font-Names="Arial" Font-Bold="True" ForeColor="#383838"></asp:Label> &nbsp; </TD><TD style="WIDTH: 35%" align=left><asp:DropDownList id="lstLookup" runat="server">
                                    <asp:ListItem>Email</asp:ListItem>
                                    <asp:ListItem>FirstName</asp:ListItem>
                                    <asp:ListItem>LastName</asp:ListItem>
                                    <asp:ListItem>DBA</asp:ListItem>
                                </asp:DropDownList> </TD><TD style="WIDTH: 35%" align=left><asp:TextBox id="txtLookup" runat="server" MaxLength="50" Width="80px"></asp:TextBox> </TD></TR><TR><TD style="HEIGHT: 10px" align=center colSpan=3></TD></TR><TR><TD align=center colSpan=3><asp:Button id="btnLookup" onclick="btnLookup_Click" runat="server" Text="Lookup"></asp:Button></TD></TR><TR><TD style="HEIGHT: 15px" align=center colSpan=3></TD></TR></TBODY></TABLE></DIV></DIV><asp:Panel id="pnlConfirm" runat="server" Visible="False" Width="200px" BackColor="#FFC0C0" BorderColor="Salmon" Height="50px" BorderStyle="Double">
                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label><br />
                <asp:Label ID="lblMessage" runat="server" Text="Do you want to create a new record?"></asp:Label><br />
                <asp:Button ID="btnCreateRecordYes" runat="server" OnClick="btnCreateRecordYes_Click"
                    Text="Yes" />
                <asp:Button ID="btnCreateRecordNo" runat="server" OnClick="btnCreateRecordNo_Click"
                    Text="No" /></asp:Panel> 
                 <asp:Panel id="pnlDeleteConfirm" runat="server" Visible="False" Width="100px" BackColor="#FFC0C0" BorderColor="Salmon" BorderStyle="Double">
                <asp:Label ID="lblDeleteMsg" runat="server" Font-Bold="true" Font-Size="Medium" Visible="True">Are you sure you want to delete this application?</asp:Label>
                <asp:Label ID="lblDeleteLeadType" runat="server" Visible="False"></asp:Label><br />
                <asp:Label ID="lblstrLeadID" runat="server" Visible="False"></asp:Label><br />
                <asp:Button ID="btnDeleteYes" runat="server" OnClick="btnDeleteYes_Click" Text="Yes" />
                <asp:Button ID="btnDeleteNo" runat="server" OnClick="btnDeleteNo_Click" Text="No" />
            </asp:Panel>
            <asp:Panel id="pnlMerge" runat="server" Visible="false" Width="160px" BackColor="#FFC0C0" BorderColor="Salmon" Height="80px" BorderStyle="Double">
                <asp:Label id="lblMergeApp" runat="server" Text="Specify App ID to Merge:"></asp:Label><BR />
                <asp:TextBox id="txtMerge" runat="server" Width="51px" Height="22px"></asp:TextBox><BR />
                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExt1"  runat="server"  TargetControlID="txtMerge" FilterType="numbers" ></cc1:FilteredTextBoxExtender>
                <asp:Button id="btnMerge" onclick="btnMerge_Click" runat="server" Text="Merge"></asp:Button> 
                <asp:Button id="btnCancelMerge" runat="server" Text="Cancel" OnClick="btnCancelMerge_Click"></asp:Button>
            </asp:Panel><BR />
            <asp:Panel id="pnlSortBy" runat="server" Visible="False" Width="600px">
            <asp:Label id="lblSortByHeader" runat="server" CssClass="Labels" Text="Sort By: " Font-Bold="true">
             </asp:Label>
              <asp:LinkButton id="lnkbtnSortByAffiliateID" onclick="lnkbtnSortByAffiliateID_Click" runat="server" CssClass="One">Affiliate ID</asp:LinkButton>&nbsp; &nbsp;<asp:LinkButton id="lnkBtnSortByFirst" onclick="lnkBtnSortByFirst_Click" runat="server" CssClass="One">FirstName</asp:LinkButton>&nbsp; <asp:LinkButton id="lnkBtnSortByLast" onclick="lnkBtnSortByLast_Click" runat="server" CssClass="One">LastName</asp:LinkButton> &nbsp; <asp:LinkButton id="lnkBtnSortByCompany" onclick="lnkBtnSortByCompany_Click" runat="server" CssClass="One">Company Name</asp:LinkButton>&nbsp; &nbsp; <asp:LinkButton id="lnkbtnSortByDBA" onclick="lnkbtnSortByDBA_Click" runat="server" CssClass="One">DBA</asp:LinkButton> </asp:Panel> &nbsp; <asp:LinkButton id="lnkbtnLookup" runat="server" Visible="False" CssClass="One" Font-Bold="True" OnClientClick="return false;" CausesValidation="False">Look up specific Affiliate record</asp:LinkButton> <BR /><asp:GridView id="grdFreeReport" runat="server" Visible="False" ForeColor="#333333" AutoGenerateColumns="False" CellPadding="4" GridLines="Vertical" OnRowCommand="grdFreeReport_RowCommand" OnRowDeleting="grdFreeReport_RowDeleting" OnRowDataBound="grdFreeReport_RowDataBound" >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="LeadId" HeaderText="Lead ID">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReferralSource" HeaderText="Referral Source">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreateDate" HeaderText="Create Date">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                 
                    <asp:ButtonField CommandName="CreateApp" Text="Create App">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
           
                    <asp:CommandField ShowDeleteButton="True" />
                    <asp:ButtonField CommandName="AddToACT" Text="Add to ACT!">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="ActAddDate" HeaderText="Date Added" />
                </Columns>
                <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                    ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView> <asp:GridView id="grdFreeConsult" runat="server" Visible="False" ForeColor="#333333" AutoGenerateColumns="False" CellPadding="4" GridLines="Vertical" OnRowCommand="grdFreeConsult_RowCommand" OnRowDeleting="grdFreeConsult_RowDeleting" OnRowDataBound="grdFreeConsult_RowDataBound">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="LeadId" HeaderText="Lead ID">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReferralSource" HeaderText="Referral Source">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreateDate" HeaderText="Create Date">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CCHomePhone" HeaderText="Home Phone" />
                    <asp:BoundField DataField="CCPhone" HeaderText="Business Phone" />
       
                    <asp:ButtonField CommandName="CreateApp" Text="Create App">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
                    <asp:ButtonField CommandName="MergeApp" Text="Merge App">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>   
                    <asp:CommandField ShowDeleteButton="True" />
                    <asp:ButtonField CommandName="AddToACT" Text="Add to ACT!">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="ActAddDate" HeaderText="Date Added" />
                </Columns>
                <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                    ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView> <asp:GridView id="grdFreeApply" runat="server" Visible="False" ForeColor="#333333" AutoGenerateColumns="False" CellPadding="4" GridLines="Vertical" OnRowCommand="grdFreeApply_RowCommand" OnRowDeleting="grdFreeApply_RowDeleting" OnRowDataBound="grdFreeApply_RowDataBound">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="LeadId" HeaderText="Lead ID">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReferralSource" HeaderText="Referral Source">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreateDate" HeaderText="Create Date">
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CCHomePhone" HeaderText="Home Phone" />
                    <asp:BoundField DataField="CCPhone" HeaderText="Business Phone" />
                    <asp:BoundField DataField="CCMobilePhone" HeaderText="Mobile Phone" />
                    <asp:ButtonField CommandName="CreateApp" Text="Create App">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
                    <asp:ButtonField CommandName="MergeApp" Text="Merge App">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>                    
                    <asp:CommandField ShowDeleteButton="True" />
                    <asp:ButtonField CommandName="AddToACT" Text="Add To ACT!">
                        <ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                    </asp:ButtonField>
                     <asp:BoundField DataField="ActAddDate" HeaderText="Date Added" />
                    
                </Columns>
                <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                    ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView> <asp:GridView id="grdAffiliateSignups" runat="server" Visible="False" ForeColor="#333333" AutoGenerateColumns="False" CellPadding="1" GridLines="Vertical" OnRowCommand="grdAffiliateSignups_RowCommand" OnRowDeleting="grdAffiliateSignups_RowDeleting" OnRowDataBound="grdAffiliateSignups_RowDataBound">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="AffiliateId" HeaderText="Partner ID" SortExpression="AffiliateId">
                        <HeaderStyle Font-Size="Small" />
                    </asp:BoundField>
                    <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="AffiliateId" DataNavigateUrlFormatString="UpdatePartnerInfo.aspx?PartnerID={0}"
                        Target="_blank" ControlStyle-ForeColor="#284775">
                        <ItemStyle Font-Bold="True" Font-Size="Small"/>
                        <HeaderStyle Font-Size="Small" /></asp:HyperLinkField>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName">
                        <HeaderStyle Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                        <HeaderStyle Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" />
                    <asp:BoundField DataField="DBA" HeaderText="DBA" SortExpression="DBA" />
                    <asp:BoundField DataField="Referral" HeaderText="Referral Source" SortExpression="AffiliateReferral">
                        <HeaderStyle Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Category" HeaderText="Signup Category" SortExpression="Category" />
                    <asp:BoundField DataField="LastModified" HeaderText="Last Modified">
                        <HeaderStyle Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastSync" HeaderText="Last Sync">
                        <HeaderStyle Font-Size="Small" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DirectDeposit" HeaderText="Direct Deposit" />
                    <asp:CommandField ShowDeleteButton="True" />
                    <asp:ButtonField CommandName="AddToACT" Text="Add To ACT!">
                        <ItemStyle Font-Bold="True" Font-Size="Small" />
                        <HeaderStyle Font-Size="Small" />
                    </asp:ButtonField>
                    <asp:ButtonField CommandName="UpdateInACT" Text="Update In ACT!">
                        <ItemStyle Font-Bold="True" Font-Size="Small" />
                        <HeaderStyle Font-Size="Small" />
                    </asp:ButtonField>
                </Columns>
                <RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                    ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click"></asp:AsyncPostBackTrigger>
</Triggers>
</asp:UpdatePanel>
</asp:Content> 
