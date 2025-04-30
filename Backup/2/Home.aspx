<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Home.aspx.cs"
    Inherits="Home" Title="E-Commerce Exchange -  Partner Portal" Theme="Admin" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script language="Javascript" type="text/javascript">    //<![CDATA[    
    function Cover(bottom, top, ignoreSize) {
        var location = Sys.UI.DomElement.getLocation(bottom);
        top.style.position = 'absolute';
        top.style.top = location.y + 'px';
        top.style.left = location.x + 40 + 'px';
        if (!ignoreSize) {
            top.style.height = bottom.offsetHeight + 'px';
            top.style.width = bottom.offsetWidth + 'px';
        }
    }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick">
    </asp:Timer>
    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlNews"
        ExpandControlID="pnlNewsHeader" CollapseControlID="pnlNewsHeader" Collapsed="false"
        TextLabelID="lblShowDetails" ExpandedText="(Hide)" CollapsedText="(Show)" ImageControlID="imgShowDetails"
        ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
        SuppressPostBack="true" />
    <cc1:AnimationExtender ID="AnimationExtender1" runat="server" TargetControlID="lblNewApps" >
        <Animations>
                <OnLoad>                        
                    <Sequence>                               
                        <ScriptAction Script="Cover($get('ctl00_ctl00_RootContent_MainContent_lnkEditProfile'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>                            
                        <Parallel AnimationTarget="flyout" Duration=".1" Fps="25">                            
                            <Resize Width="250" Height="30" />
                            <Color AnimationTarget="flyout" StartValue="#AAAAAA" EndValue="#f9f9d9" Property="style" PropertyKey="backgroundColor" />                                
                        </Parallel>                            
                        <ScriptAction Script="Cover($get('flyout'), $get('info'), true);" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="info" Duration=".2"/>                            
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        <StyleAction AnimationTarget="info" Attribute="height" value="auto" />
                        <Parallel Duration=".2">
                            <Color AnimationTarget="info" StartValue="#383838" EndValue="#383838" Property="style" PropertyKey="color" />
                            <Color AnimationTarget="info" StartValue="#febd0d" EndValue="#383838" Property="style" PropertyKey="borderColor" />
                        </Parallel>
                        <Parallel Duration=".2">
                            <Color AnimationTarget="info" StartValue="#383838" EndValue="#383838" Property="style" PropertyKey="color" />
                            <Color AnimationTarget="info" StartValue="#febd0d" EndValue="#383838" Property="style" PropertyKey="borderColor" />
                            <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />                            
                        </Parallel>                                
                    </Sequence>
                </OnLoad>
        </Animations>
    </cc1:AnimationExtender>
    <cc1:AnimationExtender ID="AnimationExtender2" runat="server" TargetControlID="btnClose">
        <Animations>
            <OnClick>
                <Sequence>
                    <StyleAction AnimationTarget="info" Attribute="overflow" Value="hidden"/>
                    <Parallel AnimationTarget="info" Duration=".1" Fps="15">
                        <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                        <FadeOut />
                    </Parallel>
                    <StyleAction AnimationTarget="info" Attribute="display" Value="none"/>
                    <StyleAction AnimationTarget="info" Attribute="width" Value="300px"/>
                    <StyleAction AnimationTarget="info" Attribute="height" Value=""/>
                    <StyleAction AnimationTarget="info" Attribute="fontSize" Value="12px"/>
                    <StyleAction AnimationTarget="btnCloseParent" Attribute="opacity" value="0" />
                    <StyleAction AnimationTarget="btnCloseParent" Attribute="filter" value="alpha(opacity=0)" />                        
                </Sequence>
            </OnClick>
            <OnMouseOver>
                <Color Duration=".2" StartValue="#FFFFFF" EndValue="#FF0000" Property="style" PropertyKey="color" />                            
            </OnMouseOver>
            <OnMouseOut>
                <Color Duration=".2" EndValue="#FFFFFF" StartValue="#FF0000" Property="style" PropertyKey="color" />                            
            </OnMouseOut>
        </Animations>
    </cc1:AnimationExtender>    
    <asp:Label ID="lblError" CssClass="LabelsError" runat="server" Visible="False"></asp:Label><br />
    <asp:Panel ID="pnlDiv" runat="server" align="Left">
    </asp:Panel>
    <table border="0" cellpadding="2" cellspacing="2" style="width: 800px;">
        <tr>
            <td align="center" colspan="2" class="DivGreen" valign="top">
                <div style="width: 700px;">
                    <div align="left" style="height: 30px; width: 50%; float: left">
                        <strong><span style="font-size: 10pt; color: #064787; font-family: Arial">Welcome
                            <asp:Label ID="lblUserName" runat="server" Font-Bold="True"></asp:Label>
                            <asp:HyperLink CssClass="One" ID="lnkEditProfile" runat="server" Font-Size="8pt"
                                NavigateUrl="EditInfo.aspx">(Edit Profile)</asp:HyperLink></span></strong></div>
                    <div align="right" style="height: 30px; float: right">
                        <asp:UpdatePanel ID="UpdatePanelDate" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblTodayDate" runat="server" CssClass="LabelsDarkBlue" Font-Bold="True"
                                    Font-Size="X-Small"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" rowspan="1" style="width: 35%" valign="top">
                <div style="width: 100%;" class="SilverBorder" align="center">
                    <div align="center" style="height: 20px; width: 100%; background-image: url('Images/topMain.gif');"
                        class="BorderBlack">
                        <b><span class="MenuHeader">Business Calendar</span></b>
                    </div>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Calendar ID="clndrDates" runat="server" BackColor="White" BorderColor="#999999"
                                CellPadding="4" DayNameFormat="Shortest" Font-Names="Arial" Font-Size="8pt" ForeColor="Black"
                                Height="160px" OnDayRender="clndrDates_DayRender" OnVisibleMonthChanged="clndrDates_VisibleMonthChanged"
                                SelectionMode="None" Width="180px">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <OtherMonthDayStyle ForeColor="Gray" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                            </asp:Calendar>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblResdDate" runat="server" BackColor="Blue" Height="15px" Width="15px">&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                    <asp:Label ID="lblResdMsg" runat="server" ForeColor="#064787" Font-Size="8pt" Font-Names="Arial"
                        Text="Residual and Commissions Posting Date"></asp:Label><br />
                    <asp:Label ID="lblCommDate" runat="server" BackColor="Red" Height="15px" Width="15px">&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                    <asp:Label ID="lblCommDateMsg" runat="server" ForeColor="#064787" Font-Names="Arial"
                        Font-Size="8pt" Text="Commissions Posting Date"></asp:Label>                    
                    <asp:Label ID="lblHolidayDate" runat="server" BackColor="Green" Height="15px" Width="15px">&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                    <asp:Label ID="lblHolidayMsg" runat="server" ForeColor="#064787" Font-Size="8pt"
                        Font-Names="Arial" Text="Holidays"></asp:Label>
                </div>                
            </td>
            <td align="right" style="width: 65%" valign="top">
                <asp:Panel ID="pnlOnlineApp" runat="server" Width="95%">
                    <div style="width: 100%;" class="SilverBorder" align="center">
                        <div align="center" style="height: 20px; width: 100%; background-image: url('Images/topMain.gif');"
                            class="BorderBlack">
                            <b><span class="MenuHeader">Online Applications</span></b>
                        </div>
                        <div style="width: 100%" align="center">
                            <span class="LabelsDarkBlueSmall">You can create Online Applications by clicking on
                                the following link:<br />
                                <asp:HyperLink CssClass="One" ID="lnkOnlineApp" Font-Size="Small" runat="server"
                                    Font-Bold="True" Font-Names="Arial">Create Online Applications</asp:HyperLink>
                                <br />
                                You can change your default rates packages by clicking below:<br />
                            </span>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lnkbtnChangePackage" runat="server" CssClass="One" OnClick="lnkbtnChangePackage_Click">(Click here to change Default Rate Packages)</asp:LinkButton><br />
                                    <asp:Panel ID="pnlChangePackage" runat="server" Visible="False" Width="90%">
                                        <table border="0" cellspacing="0" cellpadding="0" style="width: 100%; border-right: #b8dfff 2px solid;
                                            border-top: #b8dfff 2px solid; border-left: #b8dfff 2px solid; border-bottom: #b8dfff 2px solid;">
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <span class="LabelsSmall"><b>Select Default Rates Packages</b></span>
                                                    <hr noshade width="90%" size="1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width = "60%" align="right" valign="top">
                                                    <span class="LabelsSmall"><b>Make this my default CNP Package</b></span>&nbsp;<br />
                                                </td>
                                                <td align="left">
                                                    &nbsp;<asp:DropDownList ID="lstPackages" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan=2><span class="LabelsRedSmall">(Note: The default CNP rates package
                                                        will be applied to Online Applications)</span>
                                                </td>    
                                            </tr>
                                            <tr>
                                                <td style="height: 10px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <span class="LabelsSmall"><b>Make this my default CP Package</b></span>&nbsp;</td>
                                                <td align="left">
                                                    &nbsp;<asp:DropDownList ID="lstCPPackages" runat="server">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan=2>
                                                    <asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" />&nbsp;&nbsp;
                                                    &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlAffiliateLink" runat="server" Width="95%">
                    <div style="width: 100%;" class="SilverBorder" align="center">
                        <div align="center" style="height: 20px; width: 100%; background-image: url('Images/topMain.gif');"
                            class="BorderBlack">
                            <b><span class="MenuHeader">Website Links</span></b>
                        </div>
                        <div style="height: 42px; width: 100%" align="center">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:HyperLink CssClass="One" ID="lnkWebsiteHome" runat="server" Font-Names="Arial"
                                            Font-Size="9pt">Website Link</asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:HyperLink CssClass="One" ID="lnkAgentWebsite" runat="server" Font-Names="Arial"
                                            Font-Size="9pt">Agent Website Link</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HyperLink CssClass="One" ID="lnkAffiliateWebsite" runat="server" Font-Names="Arial"
                                            Font-Size="9pt">Affiliate Website Link</asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:HyperLink CssClass="One" ID="lnkResellerWebsite" runat="server" Font-Names="Arial"
                                            Font-Size="9pt">Reseller Website Link</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlFAQ" runat="server" Width="95%">
                    <div style="width: 100%;" class="SilverBorder" align="center">
                        <div align="center" style="height: 20px; width: 100%; background-image: url('Images/topMain.gif');"
                            class="BorderBlack">
                            <b><span class="MenuHeader">Frequently Asked Questions</span></b>
                        </div>
                        <div style="height: 41px; width: 100%" align="center">
                            <asp:LinkButton ID="lnkbtnFAQ" runat="server" Font-Names="Arial" Font-Size="Small"
                                CssClass="One" OnClientClick="javascript:popupHelp('HelpAgent.aspx', '00');return false;"
                                Visible="True">Click here to view answers to questions about the Partner Portal, Reports and Online Applications</asp:LinkButton>
                            <asp:ImageButton ID="imgHelp" Style="cursor: pointer" runat="server" CausesValidation="false"
                                ImageUrl="~/Images/help.gif" ToolTip="Help" OnClientClick="javascript:popupHelp('HelpAgent.aspx', '00');return false;" />
                        </div>
                    </div>
                </asp:Panel>
                <div id="flyout" style="z-index: 2; display: none; overflow: hidden;">
                </div>
                <div id="info" style="z-index: 2; display: none; text-align: center; width: 250px; padding: 5px;" class="DivHelp">
                    <div style="float: right; opacity: 0; filter: alpha(opacity=0);" id="btnCloseParent">
                        <asp:LinkButton ID="btnClose" CssClass="CloseButton" runat="server" OnClientClick="return false;"
                            Text="" ToolTip="Close">X</asp:LinkButton>
                    </div>
                    <asp:Label ID="lblNewapps" runat="server" Visible="false" Font-Names="Arial"
                        Font-Size="8pt" Font-Bold="true">
                        <asp:HyperLink ID="lnkNewapps" runat="server" Visible="false" Font-Names="Arial"
                        Font-Size="8pt" NavigateUrl="~/OnlineAppMgmt/default.aspx" Font-Bold="true"></asp:HyperLink><br/>
                        <asp:HyperLink ID="lnkUnsynched" runat="server" Visible="false" Font-Names="Arial"
                        Font-Size="8pt" NavigateUrl="~/OnlineAppMgmt/default.aspx" Font-Bold="true"></asp:HyperLink><br/>
                        <asp:HyperLink ID="lnkFileUploaded" runat="server" Visible="false" Font-Names="Arial"
                        Font-Size="8pt" NavigateUrl="~/OnlineAppMgmt/default.aspx" Font-Bold="true"></asp:HyperLink>
                    </asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2" rowspan="1" valign="top">
                <asp:Panel ID="pnlGoalsView" runat="server" Width="100%">
                    <cc1:TabContainer runat="server" ID="TabContainerGoals">
                        <cc1:TabPanel ID="TabGoals" runat="server" HeaderText="Goals">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanelViewGoals" runat="server">
                                    <ContentTemplate>
                                        <div align="center">
                                            <asp:Label ID="lblNoGoals" runat="server" ForeColor="#064787" Font-Size="Small" Font-Names="Arial"
                                                Visible="False" Font-Bold="True" Text="No Goals set for this month."></asp:Label>
                                        </div>
                                        <asp:Table ID="tblGoals" runat="server" Width="100%">
                                        </asp:Table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="clndrDates" EventName="Load"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabAddGoals" runat="server" HeaderText="Add/Update Goals">
                            <ContentTemplate>
                                <div style="width: 100%;" align="center">
                                    <asp:DropDownList ID="lstRepList" runat="server">
                                    </asp:DropDownList>&nbsp;
                                    <asp:TextBox ID="txtFundedGoals" Width="50px" runat="server"></asp:TextBox>
                                    <br />
                                    <asp:Button ID="btnAddRep" OnClick="btnAddRep_Click" runat="server" Text="Add/Update" />
                                </div>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabDeleteGoals" runat="server" HeaderText="Delete Goals">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanelDeleteGoals" runat="server">
                                    <ContentTemplate>
                                        <div style="width: 100%" align="center">
                                            <asp:DropDownList ID="lstMonth" AutoPostBack="true" OnSelectedIndexChanged="lstMonth_SelectedIndexChanged"
                                                runat="server">
                                            </asp:DropDownList>&nbsp;
                                            <asp:DropDownList ID="lstGoalsRepList" runat="server">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:Button ID="btnDeleteRep" runat="server" OnClick="btnDeleteRep_Click" Text="Delete" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnDeleteRep" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabAddNews" runat="server" HeaderText="Add News">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanelAddNews" runat="server">
                                    <ContentTemplate>
                                        <div style="width: 100%;" align="center">
                                            <span class="LabelsDarkBlue"><b>Type News Text</b></span><br />
                                            <asp:TextBox ID="txtNews" runat="server" Height="60px" TextMode="MultiLine" Width="290px"
                                                Wrap="true"></asp:TextBox><br />
                                            <span class="Labels">Display </span>
                                            <asp:DropDownList ID="lstDisplayNews" runat="server">
                                                <asp:ListItem>ALL</asp:ListItem>
                                                <asp:ListItem>Employees</asp:ListItem>
                                                <asp:ListItem>Agents</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="chkImp" runat="server" CssClass="Labels" Text="Highlight" />
                                            <asp:Button ID="btnAddNews" runat="server" Text="Add" OnClick="btnAddNews_Click" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddNews" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="TabDeleteNews" runat="server" HeaderText="Update/Delete News">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanelDeleteNews" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView id="grdNews" runat="server" ForeColor="#333333" AutoGenerateColumns="false"
                                        OnRowDeleting="GridView1_RowDeleting" GridLines="Vertical" 
                                        CellPadding="4" OnRowEditing="grdNews_RowEditing" Width="100%"
                                        OnRowCancelingEdit="grdNews_RowCancelingEdit" OnRowUpdating="grdNews_RowUpdating">
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True"></FooterStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date"
                                                    ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                      <asp:Panel ID="pnlEvalDate" Runat="Server" Height="30px" ScrollBars="Vertical">
                                                        <asp:Label ID="lblEvalDate" Text='<%# Eval("Date") %>' Runat="Server"/>
                                                      </asp:Panel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                      <asp:TextBox id="EditDate" Text='<%# Bind("Date") %>' Runat="Server" CssClass="LabelsSmall" EnableTheming="false"/>
                                                    </EditItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Important"
                                                    ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                      <asp:Panel ID="pnlEvalImp" Runat="Server" Height="30px" ScrollBars="Vertical">
                                                        <asp:Label ID="lblEvalImp" Text='<%# Eval("Imp") %>' Runat="Server"/>
                                                      </asp:Panel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="EditImp" runat="server">
                                                            <asp:ListItem>True</asp:ListItem>
                                                            <asp:ListItem>False</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Display"
                                                    ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                      <asp:Panel ID="pnlEvalDisplay" Runat="Server" Height="30px" ScrollBars="Vertical">
                                                        <asp:Label ID="lblEvalDisplay" Text='<%# Eval("Display") %>' Runat="Server"/>
                                                      </asp:Panel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="EditDisplay" runat="server">
                                                            <asp:ListItem>ALL</asp:ListItem>
                                                            <asp:ListItem>Employees</asp:ListItem>
                                                            <asp:ListItem>Agents</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                  </asp:TemplateField>
                                                <asp:TemplateField HeaderText="News_Text"
                                                    ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                      <asp:Panel ID="pnlEvalText" Runat="Server" Width="200px" Height="60px" ScrollBars="Vertical">
                                                        <asp:Label ID="lblEvalText" Text='<%# Eval("News_Text") %>' Runat="Server"/>
                                                      </asp:Panel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                      <asp:TextBox id="EditText" Text='<%# Bind("News_Text") %>' Runat="Server"
                                                        TextMode="MultiLine" Rows="6" Width="200px" CssClass="LabelsSmall" EnableTheming="false"/>
                                                    </EditItemTemplate>
                                                  </asp:TemplateField>
                                                <asp:CommandField ShowDeleteButton="True">
                                                    <ItemStyle Font-Size="8pt" Font-Names="Arial"></ItemStyle>
                                                </asp:CommandField>
                                                <asp:CommandField ShowEditButton="True"></asp:CommandField>
                                            </Columns>
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="8pt" Font-Names="Arial">
                                            </RowStyle>
                                            <EditRowStyle BackColor="#999999"></EditRowStyle>
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                            <HeaderStyle BackColor="#5D7B9D" ForeColor="White" Font-Size="8pt" Font-Names="Arial"
                                                Font-Bold="True"></HeaderStyle>
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2" rowspan="1" valign="top">
                <asp:Panel ID="pnlPartnerGoals" runat="server" Width="100%">
                    <cc1:TabContainer runat="server" ID="TabContainerGoalsAgent">
                        <cc1:TabPanel ID="PartnerGoals" runat="server" HeaderText="Goals">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanelPartnerGoals" runat="server">
                                    <ContentTemplate>
                                        <div align="center">
                                            <asp:Label ID="lblNoPartnerGoals" runat="server" ForeColor="#064787" Font-Size="Small" Font-Names="Arial"
                                                Visible="False" Font-Bold="True" Text="No Goals set for this month."></asp:Label>
                                        </div>
                                        <asp:Table ID="tblPartnerGoals" runat="server" Width="100%">
                                        </asp:Table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="clndrDates" EventName="Load"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" rowspan="1" valign="top">
                <asp:Panel ID="pnlNewsHeader" runat="server" Height="20px">
                    <div style="cursor: pointer; vertical-align: middle; width: 100%; height: 20px; background-image: url(Images/topmain.gif);
                        background-repeat: repeat-x" class="SilverBorder">
                        <div style="float: left; text-align: center; margin-left: 70%;">
                            <span class="MenuHeader"><b>Latest News & Alerts </b></span>
                        </div>
                        <div style="float: left; margin-left: 5%">
                            <asp:Label ID="lblShowDetails" runat="server" Font-Bold="True" CssClass="MenuHeader">(Show)</asp:Label>
                        </div>
                        <div style="float: right; vertical-align: middle;">
                            <asp:Image ID="imgShowDetails" runat="server" ImageUrl="~/images/expand_blue.jpg" /></div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlNews" runat="server" Width="99.80%" BackColor="#ffffff" Visible="True"
                    CssClass="SilverBorder">
                    <asp:UpdatePanel ID="UpdatePanelNews" runat="server">
                        <ContentTemplate>
                            <asp:Table ID="tblNews" Width="95%" runat="server">
                            </asp:Table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>                
            </td>
        </tr>
    </table>
</asp:Content>
