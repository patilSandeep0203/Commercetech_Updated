using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLayer;
using DLPartner;
using System.Data.SqlClient;

public partial class cog : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (Session.Keys.Count == 0)
                Response.Redirect("../logout.aspx");
            if (User.IsInRole("Agent"))
                Page.MasterPageFile = "~/AgentMisc.master";
            else if (User.IsInRole("T1Agent"))
                Page.MasterPageFile = "~/T1Agent.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "~/site.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "~/Employee.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Pragma", "no-cache");
        Response.Expires = -1;

        if (!Session.IsNewSession)
        {
            //This page is accessible only by admins and employees
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee") && !User.IsInRole("Agent") && !User.IsInRole("T1Agent"))
                Response.Redirect("~/login.aspx?Authentication=False");
        }
        else
            Response.Redirect("~/login.aspx");

        if (!IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");

            Style TextArea = new Style();
            TextArea.Width = new Unit(180);
            TextArea.Height = new Unit(80);
            TextArea.Font.Size = FontUnit.Point(8);
            TextArea.Font.Name = "Arial";
            txtSellingDesc.ApplyStyle(TextArea);
            txtUpdateSellDesc.ApplyStyle(TextArea);
            txtUpdateSellDescAgent.ApplyStyle(TextArea);


            if (!User.IsInRole("Admin"))
            {
                lnkbtnAddProduct.Visible = false;
                lnkAddImage.Visible = false;
            }

            ProductsBL COGInfo = new ProductsBL();
            DataSet dsCat = COGInfo.GetProductCategories();
            if (dsCat.Tables[0].Rows.Count > 0)
            {
                //Sort Category List
                lstSelectCategory.DataSource = dsCat;
                lstSelectCategory.DataTextField = "CategoryName";
                lstSelectCategory.DataValueField = "CategoryID";
                lstSelectCategory.DataBind();
                ListItem category = new ListItem();
                category.Text = "ALL";
                category.Value = "ALL";
                lstSelectCategory.Items.Add(category);
                lstSelectCategory.SelectedValue = lstSelectCategory.Items.FindByText("ALL").Value;
                
                //Add Product Category List
                lstCategory.DataSource = dsCat;
                lstCategory.DataTextField = "CategoryName";
                lstCategory.DataValueField = "CategoryID";
                lstCategory.DataBind();
            }

            /*try
            {
                string SortBy = "ProductCode";
                if (Request.Params.Get("SortBy") != null)
                    SortBy = Request.Params.Get("SortBy").ToString().Trim();

                Populate();
            }
            catch (Exception err)
            {
                CreateLog Log = new CreateLog();
                Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                DisplayMessage("Error populating product list.");
            }*/
        }//end post back       
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlUpdate.Visible = false;
    }

    //This function handles add product button click event
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    //Add product information
                    ProductsBL AddCOG = new ProductsBL();

                    if (txtProstoresCode.Text == "")
                        txtProstoresCode.Text = "0";

                    int iRetVal = AddCOG.InsertProductInfo(txtAddProduct.Text.Trim(), txtAddDescription.Text.Trim(),
                        Convert.ToDecimal(txtAddCOG.Text.Trim()), Convert.ToDecimal(txtAddSellPrice.Text.Trim()), lstType.SelectedItem.Text, 
                        lstCategory.SelectedItem.Value, Convert.ToInt32(chkWebsiteDisplay.Checked), 
                        Convert.ToInt16(txtProstoresCode.Text), txtAddImageName.Text.Trim(), txtSellingDesc.Text.Trim());
                    DisplayMessage("Product information Added");

                    pnlAddProduct.Visible = false;
                    lnkbtnAddProduct.Visible = true;
                    lnkAddImage.Visible = true;
                    txtAddProduct.Text = "";
                    txtAddDescription.Text = "";
                    txtCOG.Text = "";
                    txtSellingDesc.Text = "";
                    Populate();
                }//end if access
                else
                    DisplayMessage("You do not have access to add new products");
            }//end if page is valid            
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error adding product information");
        }
    }//end add button click

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        pnlAddProduct.Visible = false;
        lnkbtnAddProduct.Visible = true;
        lnkAddImage.Visible = true;
    }

    protected void lnkbtnAddProduct_Click(object sender, EventArgs e)
    {
        pnlAddProduct.Visible = true;
        pnlDeleteProduct.Visible = false;
        pnlUpdate.Visible = false;
        pnlUpdateAgent.Visible = false;
        lnkbtnAddProduct.Visible = false;
        lnkAddImage.Visible = true;
    }

    //This function handles update product button click event
    protected void btnUpdateCOG_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                //Update product information
                ProductsBL Product = new ProductsBL();
                if (txtUpdateProstoresCode.Text == "")
                    txtUpdateProstoresCode.Text = "0";

                int iRetVal = Product.UpdateProductInfo(Convert.ToInt16(lblCode.Text), txtProductName.Text.Trim(),
                    txtDescription.Text.Trim(), Convert.ToDecimal(txtCOG.Text.Trim()), Convert.ToDecimal(txtSellPrice.Text.Trim()),
                    Convert.ToInt32(chkActive.Checked), lstUpdateType.SelectedItem.Text.Trim(),
                    lstUpdateCategoryID.SelectedItem.Value, Convert.ToInt32(chkUpdateWebsiteDisplay.Checked), 
                    Convert.ToInt16(txtUpdateProstoresCode.Text), txtUpdateImageName.Text.Trim(), txtUpdateSellDesc.Text.Trim(), chkCascade.Checked);
                DisplayMessage("Product information Updated");

                pnlUpdate.Visible = false;

                Populate();
            }//end if access
            else
                DisplayMessage("You cannot update product information");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Update Product - " + err.Message);
            DisplayMessage("Error updating product information");
        }
    }//end update button click

    //This function handles delete product button click event
    protected void btnDeleteProd_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                if (lblDelCode.Text != "")
                {
                    //Delete product from table
                    ProductsBL delProd = new ProductsBL();
                    bool retVal = delProd.DeleteProduct(Convert.ToInt16(lblDelCode.Text));
                    if (retVal)
                        DisplayMessage("Product " + lblDelCode.Text + " deleted");
                    else
                        DisplayMessage("Error Deleting Product");
                    pnlDeleteProduct.Visible = false;

                    Populate();
                }//end if delete code not blank
                else
                    DisplayMessage("Record not found");
            }//end if access
            else
                DisplayMessage("You cannot delete product information");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Delete Product - " + err.Message);
            DisplayMessage("Error Deleting Product");
        }
    }//end delete product click

    protected void btnCancelDel_Click(object sender, EventArgs e)
    {
        pnlDeleteProduct.Visible = false;
    }

    //This function populates info to be updated
    public void PopulateUpdate(int iCode)
    {
        if (User.IsInRole("Admin"))
        {
            //Populate Category List
            ProductsBL Products = new ProductsBL();
            DataSet dsCat = Products.GetProductCategories();
            if (dsCat.Tables[0].Rows.Count > 0)
            {
                lstUpdateCategoryID.DataSource = dsCat;
                lstUpdateCategoryID.DataTextField = "CategoryName";
                lstUpdateCategoryID.DataValueField = "CategoryID";
                lstUpdateCategoryID.DataBind();
            }

            PartnerDS.ProductInfoDataTable dt = Products.GetProductInfo(iCode);
            if (dt.Rows.Count > 0)
            {
                pnlUpdate.Visible = true;
                lnkbtnAddProduct.Visible = true;
                lnkAddImage.Visible = true;
                pnlAddProduct.Visible = false;
                pnlDeleteProduct.Visible = false;
                pnlUpdateAgent.Visible = false;
                lblCode.Text = dt[0].ProductCode.ToString().Trim();
                txtProductName.Text = dt[0].ProductName.ToString().Trim();
                txtDescription.Text = dt[0].Description.ToString().Trim();
                txtCOG.Text = dt[0].COG.ToString().Trim();
                txtSellPrice.Text = dt[0].SellPrice.ToString().Trim();
                txtUpdateProstoresCode.Text = dt[0].ProstoresCode.ToString().Trim();
                txtUpdateImageName.Text = dt[0].ImageURL.ToString().Trim();
                //lstActive.SelectedValue = lstActive.Items.FindByText(dt[0].Active.ToString()).Value;
                if (dt[0].Active.ToString() == "True")
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;

                if (dt[0].WebsiteDisplay.ToString() == "True")
                    chkUpdateWebsiteDisplay.Checked = true;
                else
                    chkUpdateWebsiteDisplay.Checked = false;

                lstUpdateType.SelectedValue = lstUpdateType.Items.FindByText(dt[0].Type.ToString()).Value;
                lstUpdateCategoryID.SelectedValue = lstUpdateCategoryID.Items.FindByValue(dt[0].CategoryID.ToString()).Value;                
                txtUpdateSellDesc.Text = dt[0].SellDescription.ToString().Trim();

                if ((lstUpdateType.SelectedItem.Text == "Other Charge") || (lstUpdateType.SelectedValue.ToString().Trim() == "Service"))
                {
                    lstUpdateCategoryID.Enabled = false;
                    lstUpdateCategoryID.SelectedValue = lstUpdateCategoryID.Items.FindByText("None").Value;
                }
                else
                    lstUpdateCategoryID.Enabled = true;
            }//end if count not 0
            else
                DisplayMessage("Product information not found");
        }//end if user is in role
    }//end function PopulateUpdate

    //This function populates info to be updated
    public void PopulateUpdateAgent(int iCode)
    {
        if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
        {
            ProductsBL Products = new ProductsBL();
            PartnerDS.ProductInfoDataTable dt = Products.GetProductInfo(iCode);
            if (dt.Rows.Count > 0)
            {
                pnlUpdateAgent.Visible = true;
                lnkbtnAddProduct.Visible = true;
                lnkAddImage.Visible = true;
                pnlAddProduct.Visible = false;
                pnlDeleteProduct.Visible = false;
                pnlUpdate.Visible = false;
                lblCodeAgent.Text = dt[0].ProductCode.ToString().Trim();
                txtSellPriceAgent.Text = dt[0].SellPrice.ToString().Trim();
                lblDescriptionAgent.Text = dt[0].Description.ToString().Trim();
                lblProductNameAgent.Text = dt[0].ProductName.ToString().Trim();
                txtUpdateSellDescAgent.Text = dt[0].SellDescription.ToString().Trim();
                if (!Convert.ToBoolean(dt[0].WebsiteDisplay))
                {
                    txtSellPriceAgent.Enabled = false;
                    txtUpdateSellDescAgent.Enabled = false;
                    btnUpdateCOGAgent.Enabled = false;
                }
                else
                {
                    txtSellPriceAgent.Enabled = true;
                    txtUpdateSellDescAgent.Enabled = true;
                    btnUpdateCOGAgent.Enabled = true;
                }
            }//end if count not 0
            else
            {
                DisplayMessage("Product information not found");
                pnlUpdateAgent.Visible = false;
            }
        }//end if user is in role
    }//end function PopulateUpdateAgent

    //This function populates info to be deleted
    public void PopulateDelRecord(string Code)
    {
        if (User.IsInRole("Admin"))
        {
            pnlDeleteProduct.Visible = true;
            lnkbtnAddProduct.Visible = true;
            lnkAddImage.Visible = true;
            pnlAddProduct.Visible = false;
            pnlUpdate.Visible = false;
            pnlUpdateAgent.Visible = false;
            ProductsBL Prod = new ProductsBL();
            PartnerDS.ProductInfoDataTable dt = Prod.GetProductInfo(Convert.ToInt32(Code));
            if (dt.Rows.Count > 0)
            {
                lblDelCode.Text = dt[0].ProductCode.ToString().Trim();
                lblDelCOG.Text = dt[0].COG.ToString().Trim();
                lblDelDescription.Text = dt[0].Description.ToString().Trim();
                lblDelProduct.Text = dt[0].ProductName.ToString().Trim();
            }//end if count not 0
            else
                DisplayMessage("Product information not found");
        }//end if user is in role
    }//end function PopulateDelRecord

    //This Populates COG table
    public void Populate()
    {
        //Get cog info
        ProductsBL Prod = new ProductsBL();
        DataSet ds = null;
        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            ds = Prod.GetProductsByCategory(lstSortBy.SelectedItem.Value.ToString(), lstSelectType.SelectedItem.Value.ToString(), lstSelectCategory.SelectedItem.Value.ToString(), chkInactive.Checked);
        else
            ds = Prod.GetProductsRep(lstSortBy.SelectedItem.Value.ToString(), lstSelectType.SelectedItem.Value.ToString(), lstSelectCategory.SelectedItem.Value.ToString(), chkInactive.Checked, Session["MasterNum"].ToString().Trim());

        if (ds.Tables[0].Rows.Count > 0)
        {            
            lblError.Visible = false;
            grdCOG.DataSource = ds;
            grdCOG.DataBind();
            grdCOG.Visible = true;
        }//end if count not 0
        else
        {
            DisplayMessage("No Products Found.");
            grdCOG.Visible = false;
        }
    }//end function populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Populate();
        }
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Submit Error - " + err.Message);
            DisplayMessage("Error populating product information.");
        }
    }

    protected void grdCOG_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (User.IsInRole("Admin"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdCOG.Rows[index];
                string strCode = Server.HtmlDecode(grdRow.Cells[0].Text);
                PopulateUpdate(Convert.ToInt32(strCode));
            }//end if user is admin or employee
            else if (User.IsInRole("Agent") || (User.IsInRole("T1Agent")))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow grdRow = grdCOG.Rows[index];
                string strCode = Server.HtmlDecode(grdRow.Cells[0].Text);
                PopulateUpdateAgent(Convert.ToInt32(strCode));
            }
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating product information.");
        }
    }

    protected void grdCOG_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            pnlDeleteProduct.Visible = true;
            pnlUpdate.Visible = false;
            int index = e.RowIndex;
            GridViewRow grdRow = grdCOG.Rows[index];
            PopulateDelRecord(Server.HtmlDecode(grdRow.Cells[0].Text));
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error populating product information.");
        }
    }

    protected void grdCOG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drView = (DataRowView)e.Row.DataItem;
                //Gets the prostores code from the table COG
                string strProstoresCode = drView[1].ToString();
                if ((strProstoresCode == "") || (strProstoresCode == "0"))
                    e.Row.Cells[10].Text = "";
                
                //gets the Target URL
                string strTargetURL = drView[9].ToString();
                if (strTargetURL.Trim() == "")
                    e.Row.Cells[4].Text = "";

                e.Row.Cells[5].Text = "$" + Convert.ToString(Convert.ToDouble(e.Row.Cells[5].Text.Trim()));
                if (!Convert.ToBoolean(e.Row.Cells[7].Text))
                {
                    e.Row.ToolTip = "Inactive";
                    e.Row.BackColor = System.Drawing.Color.Wheat;
                    /*if (chkInactive.Checked)
                        e.Row.Visible = false;
                    else
                        e.Row.Visible = true;*/
                }
            }//end if DataRow

            if (User.IsInRole("Employee") || User.IsInRole("Agent") || User.IsInRole("T1Agent"))
            {
                grdCOG.Columns[8].Visible = false;//Delete
                grdCOG.Columns[9].Visible = false;//Update
            }            
                
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "COG Row Databound - " + err.Message);
        }
    }//end row databound

    protected void chkInactive_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            /*string SortBy = "ProductCode";
            if (Request.Params.Get("SortBy") != null)
                SortBy = Request.Params.Get("SortBy").ToString().Trim();*/
            Populate();
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Show/Hide Inactive Products - " + err.Message);
        }
    }

    protected void lstSelectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdCOG.Visible = false;
            if ((lstSelectType.SelectedValue.ToString().Trim() == "Other Charge") || (lstSelectType.SelectedValue.ToString().Trim() == "Service")
                || (lstSelectType.SelectedValue.ToString().Trim() == "ALL"))
            {
                lstSelectCategory.Enabled = false;
                lstSelectCategory.SelectedValue = lstSelectCategory.Items.FindByText("ALL").Value;
            }
            else
                lstSelectCategory.Enabled = true;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "lstSelectType_SelectedIndexChanged - " + err.Message);
        }
    }

    protected void lstType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if ((lstType.SelectedValue.ToString().Trim() == "Other Charge") || (lstType.SelectedValue.ToString().Trim() == "Service"))
            {
                lstCategory.Enabled = false;
                lstCategory.SelectedValue = lstCategory.Items.FindByText("None").Value;
            }
            else
                lstCategory.Enabled = true;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "lstType_SelectedIndexChanged - " + err.Message);
        }
    }

    protected void lstUpdateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if ((lstUpdateType.SelectedValue.ToString().Trim() == "Other Charge") || (lstUpdateType.SelectedValue.ToString().Trim() == "Service"))
            {
                lstUpdateCategoryID.Enabled = false;
                lstUpdateCategoryID.SelectedValue = lstUpdateCategoryID.Items.FindByText("None").Value;
            }
            else
                lstUpdateCategoryID.Enabled = true;
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "lstUpdateType_SelectedIndexChanged - " + err.Message);
        }
    }

    protected void btnUpdateCOGAgent_Click(object sender, EventArgs e)
    {
        try
        {
            if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
            {
                //Update product information
                ProductsBL Product = new ProductsBL();
                int iRetVal = Product.InsertUpdateProductInfoRep(Session["MasterNum"].ToString().Trim(),
                    lblCodeAgent.Text.Trim(), txtSellPriceAgent.Text.Trim(), txtUpdateSellDescAgent.Text.Trim());
                DisplayMessage("Product information Updated");
                pnlUpdateAgent.Visible = false;

                Populate();
            }//end if access
            else
                DisplayMessage("You cannot update product information");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), "Update Product - " + err.Message);
            DisplayMessage("Error updating product information");
        }
    }

    protected void btnCancelAgent_Click(object sender, EventArgs e)
    {
        pnlUpdateAgent.Visible = false;
    }
}
