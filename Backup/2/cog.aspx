<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="cog.aspx.cs"
    Inherits="cog" Title="E-Commerce Exchange - Partner Portal" Theme="AppTheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
	<asp:ScriptManager ID="ScriptManagerCOG" runat="server">
    </asp:ScriptManager>
    <asp:Label ID="lblError" CssClass="LabelsError" EnableTheming="false" runat="server" Visible="False"></asp:Label><br />
        <table border="0" cellpadding="1" cellspacing="0" style="width: 300px;" class="SilverBorder">
        <tr>
            <td align="center" colspan="2" style="background-image: url(Images/topMain.gif);
                height: 25px">
                <b><span class="MenuHeader">Item List</span></b></td>
        </tr>
        <tr>
            <td colspan="2" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="right" width="37%">
                <asp:Label ID="lblType" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Select Type"></asp:Label>&nbsp;</td>
            <td align="left">
                <asp:DropDownList ID="lstSelectType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstSelectType_SelectedIndexChanged">
                    <asp:ListItem Value="Inventory" Text="Inventory"></asp:ListItem>
                    <asp:ListItem Value="Other Charge" Text="Other Charge"></asp:ListItem>
                    <asp:ListItem Value="Service" Text="Service"></asp:ListItem>
                    <asp:ListItem Selected="True" Value="ALL" Text="ALL"></asp:ListItem>                    
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblCategory" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Select Category"></asp:Label>&nbsp;</td>
            <td align="left">
                <asp:DropDownList ID="lstSelectCategory" runat="server" Enabled="false">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" >
                <asp:Label ID="lblSortBy" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Smaller" Text="Sort By"></asp:Label>&nbsp;</td>
            <td align="left">
                <asp:DropDownList ID="lstSortBy" runat="server">
                    <asp:ListItem Selected="True" Value="ProductCode" Text="Product Code"></asp:ListItem>
                    <asp:ListItem Value="ProductName" Text="Product Name"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="center" colspan="2">        
                <asp:CheckBox ID="chkInactive" runat="server" AutoPostBack="True" Font-Bold="True"
                Checked="true" Font-Names="Arial" Font-Size="Smaller" OnCheckedChanged="chkInactive_CheckedChanged"
                Text="Hide Inactive Products" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 5px">
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanelCOG" runat="server">
        <ContentTemplate>
			<asp:LinkButton ID="lnkbtnAddProduct" runat="server" CssClass="One" OnClick="lnkbtnAddProduct_Click" Font-Bold="True" Font-Names="Arial" Font-Size="Small">
			<br />
			Click here to Add a new Product<br />
			</asp:LinkButton>
			<asp:HyperLink ID="lnkAddImage" runat="server" CssClass="One" NavigateUrl="~/UploadImages.aspx" Target="_blank" Font-Bold="True" Font-Names="Arial" Font-Size="Small">
			<br />
			Click here to Upload a new Image<br />
			</asp:HyperLink>
			<asp:Panel ID="pnlAddProduct" runat="server" Visible="False">
				<span class="LabelsRedSmall"><strong>If applicable, also add product to Prostores.</strong></span>
				<table style="width: 100%" class="DivGreen">
					<tr>
						<td style="background-image: url(Images/topMain.gif);">
						<b><span class="MenuHeader">Product Name</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Description (Brief)</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">COG</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Sell Price</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Type</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Category</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Website Display</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Prostores Code</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Image Name</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Sell Description</span></strong></td>
					</tr>
					<tr>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtAddProduct" runat="server" Width="150px">
						</asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddProduct" ErrorMessage="Product Name">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtAddDescription" runat="server" Width="160px">
						</asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddDescription" ErrorMessage="Description">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtAddCOG" runat="server" Width="40px">
						</asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddCOG" ErrorMessage="Cost of Goods">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtAddSellPrice" runat="server" Width="40px">
						</asp:TextBox>
						<br />
						<span class="LabelsSmall">(This price will be displayed on firstaffiliates website)</span></td>
						<td valign="top" style="height: 28px">
						<asp:DropDownList ID="lstType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstType_SelectedIndexChanged">
							<asp:ListItem Selected="True" Value="Inventory" Text="Inventory">
							</asp:ListItem>
							<asp:ListItem Value="Other Charge" Text="Other Charge">
							</asp:ListItem>
							<asp:ListItem Value="Service" Text="Service">
							</asp:ListItem>
						</asp:DropDownList>
						</td>
						<td valign="top" style="height: 28px">
						<asp:DropDownList ID="lstCategory" Width="165px" runat="server">
						</asp:DropDownList>
						</td>
						<td valign="top" style="height: 28px">
						<asp:CheckBox ID="chkWebsiteDisplay" runat="server" />
						</td>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtProstoresCode" runat="server" MaxLength="3" Width="37px" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtProstoresCode" ErrorMessage="Prostores Code">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtAddImageName" runat="server" Width="150px" />
						</td>
						<td valign="top" style="height: 28px">
						<asp:TextBox ID="txtSellingDesc" runat="server" Width="160px" TextMode="MultiLine" MaxLength="1024">
						</asp:TextBox>
						</td>
					</tr>
					<tr>
						<td colspan="10" align="center">
						<asp:Button ID="btnAddProduct" runat="server" OnClick="btnAddProduct_Click" Text="Add" />
						<asp:Button ID="btnCancelAdd" runat="server" OnClick="btnCancelAdd_Click" Text="Cancel" CausesValidation="False" />
						</td>
					</tr>
				</table>
			</asp:Panel>
			<asp:Panel ID="pnlUpdate" runat="server" Visible="False">
				<span class="LabelsRedSmall"><strong>Sell Price and Sell Description will be updated
                    in the Firstaffiliates website. If applicable, also update in Prostores.</strong></span>
				<table style="width: 100%" class="DivGreen">
					<tr>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Code</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Product Name</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Description (Brief)</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">COG</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Sell Price</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Active</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Type</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Category</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Website Display</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Prostores Code</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong><span class="MenuHeader">Image Name</span></strong></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong>
						<span style="color: #ffffff; font-family: Arial; font-size: small">Sell Description</span></strong></td>
					</tr>
					<tr>
						<td valign="top">
						<asp:Label ID="lblCode" runat="server" Font-Bold="True">
						</asp:Label>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtProductName" runat="server" Width="100px">
						</asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtProductName" ErrorMessage="Product Name" Font-Bold="False">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtDescription" runat="server" Width="150px">
						</asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtCOG" runat="server" Width="37px">
						</asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCOG" ErrorMessage="Cost of Goods">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtSellPrice" runat="server" Width="37px">
						</asp:TextBox>
						<br />
						<span class="LabelsSmall">(This price will be displayed on firstaffiliates website)</span></td>
						<td valign="top">
						<asp:CheckBox ID="chkActive" runat="server" />

						</td>
						<td valign="top">
						<asp:DropDownList ID="lstUpdateType" runat="server" Width="91px" AutoPostBack="true" OnSelectedIndexChanged="lstUpdateType_SelectedIndexChanged">
							<asp:ListItem Selected="True" Value="Inventory" Text="Inventory">
							</asp:ListItem>
							<asp:ListItem Value="Other Charge" Text="Other Charge">
							</asp:ListItem>
							<asp:ListItem Value="Service" Text="Service">
							</asp:ListItem>
						</asp:DropDownList>
						</td>
						<td valign="top">
						<asp:DropDownList ID="lstUpdateCategoryID" runat="server" Width="155px">
						</asp:DropDownList>
						</td>
						<td valign="top">
						<asp:CheckBox ID="chkUpdateWebsiteDisplay" runat="server" />
						</td>
						<td valign="top">
						<asp:TextBox ID="txtUpdateProstoresCode" runat="server" MaxLength="3" Width="37px" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtUpdateProstoresCode" ErrorMessage="Prostores Code">
						</asp:RequiredFieldValidator>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtUpdateImageName" runat="server" Width="150px" />
						</td>
						<td valign="top">
						<asp:TextBox ID="txtUpdateSellDesc" runat="server" Width="150px" TextMode="MultiLine" MaxLength="1024">
						</asp:TextBox>
						</td>
					</tr>
					<tr>
						<td colspan="9" align="center">
						<asp:CheckBox ID="chkCascade" runat="server" Text="Do you want to cascade the sell price to the sell price set by Reps?" />
						<asp:Button ID="btnUpdateCOG" runat="server" OnClick="btnUpdateCOG_Click" Text="Update" />&nbsp;
						<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CausesValidation="False" />
						</td>
					</tr>
				</table>
			</asp:Panel>
			<asp:Panel ID="pnlDeleteProduct" runat="server" Visible="False" Width="750px">
				<table style="width: 610px" class="DivGreen">
					<tr>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Code</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Product Name</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Description</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Cost Of Goods</span></b></td>
					</tr>
					<tr>
						<td>
						<asp:Label ID="lblDelCode" runat="server" Font-Bold="True">
						</asp:Label>
						</td>
						<td>
						<asp:Label ID="lblDelProduct" runat="server" Font-Bold="True">
						</asp:Label>
						</td>
						<td>
						<asp:Label ID="lblDelDescription" runat="server" Font-Bold="True" Width="160px">
						</asp:Label>
						&nbsp;</td>
						<td>
						<asp:Label ID="lblDelCOG" runat="server" Font-Bold="True">
						</asp:Label>
						</td>
					</tr>
					<tr>
						<td colspan="4" align="center">
						<asp:Button ID="btnDeleteProd" runat="server" OnClick="btnDeleteProd_Click" Text="Delete" />&nbsp;
						<asp:Button ID="btnCancelDel" runat="server" OnClick="btnCancelDel_Click" Text="Cancel" />
						</td>
					</tr>
				</table>
			</asp:Panel>
			<asp:Panel ID="pnlUpdateAgent" runat="server" Visible="False">
				<span class="LabelsRedSmall"><strong>Sell Price and Sell Description will be updated in
                    your Affiliate Website</strong></span>
				<table style="width: 850px" class="DivGreen">
					<tr>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Code</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Product Name</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Description</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<b><span class="MenuHeader">Sell Price</span></b></td>
						<td style="background-image: url(Images/topMain.gif)">
						<strong>
						<span style="color: #ffffff; font-family: Arial; font-size: small">Sell Description</span></strong></td>
					</tr>
					<tr>
						<td valign="top">
						<asp:Label ID="lblCodeAgent" runat="server" Font-Bold="True">
						</asp:Label>
						</td>
						<td valign="top">
						<asp:Label ID="lblProductNameAgent" runat="server" Width="160px">
						</asp:Label>
						</td>
						<td valign="top">
						<asp:Label ID="lblDescriptionAgent" runat="server" Width="160px">
						</asp:Label>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtSellPriceAgent" runat="server" Width="160px">
						</asp:TextBox>
						</td>
						<td valign="top">
						<asp:TextBox ID="txtUpdateSellDescAgent" runat="server" Width="160px" TextMode="MultiLine" MaxLength="1024">
						</asp:TextBox>
						</td>
					</tr>
					<tr>
						<td colspan="5" align="center">
						<asp:Button ID="btnUpdateCOGAgent" runat="server" OnClick="btnUpdateCOGAgent_Click" Text="Update" />&nbsp;
						<asp:Button ID="btnCancelAgent" runat="server" OnClick="btnCancelAgent_Click" Text="Cancel" CausesValidation="False" />
						</td>
					</tr>
				</table>
			</asp:Panel>
			<br />

			<asp:GridView ID="grdCOG" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical" OnRowCommand="grdCOG_RowCommand" OnRowDeleting="grdCOG_RowDeleting" OnRowDataBound="grdCOG_RowDataBound" Width="100%">
				<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
				<Columns>
					<asp:boundfield DataField="ProductCode" HeaderText="Code" SortExpression="Code">
					</asp:boundfield>
					<asp:boundfield DataField="ProductName" HeaderText="Product">
					</asp:boundfield>
					<asp:imagefield DataImageUrlField="ImageURL" DataImageUrlFormatString="https://www.firstaffiliates.com/Affiliatewiz/Images/ProductImages/{0}" HeaderText="Image" ControlStyle-Width="50px">
					</asp:imagefield>
					<asp:boundfield DataField="Description" HeaderText="Description" SortExpression="Description">
					</asp:boundfield>
					<asp:hyperlinkfield DataNavigateUrlFields="TargetURL" Target="_blank" Text="More..." HeaderText="More Info" />
					<asp:boundfield DataField="COG" HeaderText="Cost Of Goods" SortExpression="COG">
					</asp:boundfield>
					<asp:boundfield DataField="SellPrice" HeaderText="Sell Price">
					</asp:boundfield>
					<asp:boundfield DataField="Active" HeaderText="Active" SortExpression="Active">
					</asp:boundfield>
					<asp:commandfield ShowDeleteButton="True" />
					<asp:buttonfield CommandName="UpdateCOG" Text="Update">
						<ItemStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
					</asp:buttonfield>
					<asp:hyperlinkfield DataNavigateUrlFields="ProstoresCode" DataNavigateUrlFormatString="http://store.ecenow.com/servlet/Cart?smode=add&amp;product_no={0}&amp;qty=1" HeaderText="Buy" NavigateUrl="http://www.ecenow.com/Cart.bok" Target="_blank" Text="Buy">
						<ItemStyle Font-Bold="True" Font-Names="Arial" />
					</asp:hyperlinkfield>
				</Columns>
				<RowStyle BackColor="#EDF7FF" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" />
				<EditRowStyle BackColor="#999999" />
				<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
				<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
				<HeaderStyle BackColor="#5D7B9D" CssClass="MenuHeader" />
				<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
			</asp:GridView>
			<br />
		</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
