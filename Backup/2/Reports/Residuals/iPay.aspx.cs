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
using DLPartner.PartnerDSTableAdapters;

public partial class Residuals_iPay : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!Session.IsNewSession)
        {
            if (User.IsInRole("Agent") || User.IsInRole("T1Agent"))
                Page.MasterPageFile = "../Agent.master";
            else if (User.IsInRole("Reseller") || User.IsInRole("Affiliate"))
                Page.MasterPageFile = "../Agent.master";
            else if (User.IsInRole("Employee"))
                Page.MasterPageFile = "../Employee.master";
            else if (User.IsInRole("Admin"))
                Page.MasterPageFile = "../Admin.master";
        }
    }

    void Page_Init(object sender, EventArgs e)
    {
        ViewStateUserKey = Session.SessionID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession)
            Response.Redirect("~/login.aspx");
                
        if (!Page.IsPostBack)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/login.aspx?Authentication=False");
            try
            {
                MonthBL mon = new MonthBL();
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {

                    DataSet dsMon = mon.GetMonthListForReports(1, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    ListBL RepList = new ListBL();
                    DataSet dsRep = RepList.GetRepListForVendor("ipay");
                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        lstRepList.DataSource = dsRep;
                        lstRepList.DataTextField = "RepName";
                        lstRepList.DataValueField = "MasterNum";
                        lstRepList.DataBind();
                    }
                    ListItem item = new ListItem();
                    item.Text = "ALL";
                    item.Value = "ALL";
                    lstRepList.Items.Add(item);
                    lstRepList.SelectedIndex = lstRepList.Items.IndexOf(item);

                    string MasterNum = "";
                    string Month = "";
                    if (Request.Params.Get("Month") != null)
                        Month = Request.Params.Get("Month");

                    if (Request.Params.Get("MasterNum") != null)
                        MasterNum = Request.Params.Get("MasterNum");

                    if ((Month != "") && (MasterNum != "") && (User.IsInRole("Admin") || User.IsInRole("Employee")))
                    {
                        try
                        {
                            lblError.Visible = false;                            
                            lstMonth.SelectedValue = lstMonth.Items.FindByText(Month).Value;
                            lblError.Visible = false;
                            lblMonth.Text = "iPayment(1503) Residuals for the month of: " + lstMonth.SelectedItem.Text;
                            if (lstRepList.Items.FindByValue(MasterNum) != null)
                                lstRepList.SelectedValue = MasterNum;
                   
                            Populate(MasterNum, Month, false, "");
                        }
                        catch (Exception err)
                        {
                            CreateLog Log = new CreateLog();
                            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
                            DisplayMessage("Error Processing Request. Please contact technical support");
                        }
                    }//end if month not null

                }//end if
                else
                {
                    Tabs.Tabs.Remove(TabPanelLookup);

                    DataSet dsMon = mon.GetMonthListForReports(2, "residuals");
                    if (dsMon.Tables[0].Rows.Count > 0)
                    {
                        lstMonth.DataSource = dsMon;
                        lstMonth.DataTextField = "Mon";
                        lstMonth.DataValueField = "Mon";
                        lstMonth.DataBind();
                    }

                    lstRepList.Visible = false;
                    lblSelectRepName.Visible = false;
                }
            }//end try
            catch (Exception)
            {
                DisplayMessage("Error Processing Request. Please contact technical support");
            }
        }//end if not postback

        else
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Employee"))
                Tabs.Tabs.Remove(TabPanelLookup);
        }
    }//end page load

    //This function populates ipayment residuals
    public void Populate(string MasterNum, string Month, bool bDBA, string DBA)
    {
        Style ValueLabel = new Style();
        ValueLabel.ForeColor = System.Drawing.Color.Black;
        ValueLabel.Font.Size = FontUnit.Point(8);
        ValueLabel.Font.Name = "Arial";

        Style ValueLabelHeader = new Style();
        ValueLabelHeader.ForeColor = System.Drawing.Color.White;
        ValueLabelHeader.Font.Size = FontUnit.Small;
        ValueLabelHeader.Font.Bold = true;
        ValueLabelHeader.Font.Name = "Arial";

        Style sHyperLink = new Style();
        sHyperLink.Font.Bold = true;
        sHyperLink.Font.Size = FontUnit.Point(8);
        sHyperLink.Font.Name = "Arial";

        double ECETotal = 0;
        double RepTotal = 0;
        double T1RepTotal = 0;
        PartnerDS.iPaymentDataTable dt = null;
        if (!bDBA)
        {
            ResidualsBL Resd = new ResidualsBL(Month, MasterNum);
            DataSet dsTotals = Resd.GetiPayTotals();            
            if (dsTotals.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTotals.Tables[0].Rows[0];
                ECETotal = Convert.ToDouble(dr["IPayecetotal"]);
                RepTotal = Convert.ToDouble(dr["IPayreptotal"]);
            }//end if count not 0
            dt = Resd.GetIPayResiduals();
        }
        else
        {
            ResidualsAdminBL IPay = new ResidualsAdminBL(Month);
            dt = IPay.GetiPayResidualsByDBA(DBA);
        }
        if (dt.Rows.Count > 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            string[] arrColumns = { "", "Merchant Name", "", "Qual Fee", "Mid-Q Fee", "Non-Q Fee", "Qual Trns", 
                "Mid-Q Trns", "Non-Q Trns", "Chg Bks", "Ret Reqs", "Batch Hdr", "AVS", "VAuth", "FDR Asst", 
                "Stmt Res", "Mon Min", "Other Card Type", "Debit Trans", "Debit Gate", "Annual Fee", 
                "Amex", "Tele check", "Convert", "ACH Reject", "Adjusted", "iAccess Residual", "ECE Total", "Rep Split", 
                "Rep Total", "Tier Split", 
                "Tier Total"};

            int NumColsDisplay = arrColumns.Length;

            #region First Header Row
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "ECE % Split with Bank";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            td.Attributes.Add("colspan", Convert.ToString(NumColsDisplay - 9));
            td.Attributes.Add("align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "Partner Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            td.Attributes.Add("colspan", "9");
            tr.Cells.Add(td);

            tblResiduals.Rows.Add(tr);

            #endregion

            #region Second Header Row

            tr = new TableRow();
            td = new TableCell();
            td.Attributes.Add("colspan", "3");
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "80% Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "50% Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            td.Attributes.Add("colspan", "2");
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "80% Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "50% Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            td.Attributes.Add("colspan", "2");
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "80% Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            td.Attributes.Add("colspan", "11");
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "50% Split";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);

            td = new TableCell();
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Text = "100%";
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            tr.Cells.Add(td);

            td = new TableCell();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            td.Style["font-family"] = "Arial";
            td.Style["font-size"] = "Small";
            td.Style["font-weight"] = "Bold";
            td.Style["Color"] = "White";
            td.Attributes.Add("colspan", "9");
            tr.Cells.Add(td);

            tblResiduals.Rows.Add(tr);
            #endregion

            Label lblValue;

            #region Third Header Row
            
            tr = new TableRow();
            tr.BackColor = System.Drawing.Color.FromArgb(93, 123, 157);
            for (int i = 0; i < NumColsDisplay; i++)
            {
                td = new TableCell();
                td.Text = arrColumns[i].ToString();
                td.Style["font-family"] = "Arial";
                td.Style["font-size"] = "Small";
                td.Style["font-weight"] = "Bold";
                td.Style["Color"] = "White";
                tr.Cells.Add(td);
            }

            tblResiduals.Rows.Add(tr);

            #endregion

            #region iPayment Rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();

                //RepName
                lblValue = new Label();
                if (MasterNum == "ALL")
                    lblValue.Text = dt[i].RepName.ToString().Trim();
                else
                    lblValue.Text = "";
                if (bDBA)
                    lblValue.Text = dt[i].RepName.ToString().Trim() + " (" + dt[i].Mon.ToString().Trim() + ")";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("align", "left");
                td.Attributes.Add("rowspan", "4");
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                
                //Merchant Name
                lblValue = new Label();
                lblValue.Text = dt[i].Merchant_Name.ToString().Trim() + "<br/>" + dt[i].Merchant_Number.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                td.Attributes.Add("rowspan", "4");
                tr.Cells.Add(td);

                //Residual Header
                lblValue = new Label();
                lblValue.Text = "Residual";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Q Res
                lblValue = new Label();
                lblValue.Text = dt[i].Q_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MRes
                lblValue = new Label();
                lblValue.Text = dt[i].M_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NRes
                lblValue = new Label();
                lblValue.Text = dt[i].N_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //QTRes
                lblValue = new Label();
                lblValue.Text = dt[i].Q_T_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MTRes
                lblValue = new Label();
                lblValue.Text = dt[i].M_T_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NTRes
                lblValue = new Label();
                lblValue.Text = dt[i].N_T_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //CBRes
                lblValue = new Label();
                lblValue.Text = dt[i].CB_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RetRes
                lblValue = new Label();
                lblValue.Text = dt[i].Ret_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //BatchRes
                lblValue = new Label();
                lblValue.Text = dt[i].Batch_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //AVSRes
                lblValue = new Label();
                lblValue.Text = dt[i].AVS_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VAuthRes
                lblValue = new Label();
                lblValue.Text = dt[i].VAuth_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VstRes
                lblValue = new Label();
                lblValue.Text = dt[i].Vst_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //StmtRes
                lblValue = new Label();
                lblValue.Text = dt[i].Stmt_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MonMinRes
                lblValue = new Label();
                lblValue.Text = dt[i].Mon_Min_Res.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //OtherCardType
                lblValue = new Label();
                lblValue.Text = dt[i].Other_Card_Type.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //DebitTrans
                lblValue = new Label();
                lblValue.Text = dt[i].Debit_Trans.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //DebitGateway
                lblValue = new Label();
                lblValue.Text = dt[i].Debit_Gateway.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //AnnualFee
                lblValue = new Label();
                lblValue.Text = dt[i].Annual_Fee.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //AmEx
                lblValue = new Label();
                lblValue.Text = dt[i].AmEx.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Telecheck
                lblValue = new Label();
                lblValue.Text = dt[i].Telecheck.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Conversion
                lblValue = new Label();
                lblValue.Text = dt[i].Conversion.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //ACHReject
                lblValue = new Label();
                lblValue.Text = dt[i].ACH_Reject.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //ACHAdjusted
                lblValue = new Label();
                lblValue.Text = dt[i].Adjusted.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //iAccess
                lblValue = new Label();
                lblValue.Text = dt[i].iAccessResidual.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //EceTotal
                lblValue = new Label();
                lblValue.Text = dt[i].ECETotal;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepSplit
                lblValue = new Label();
                lblValue.Text = dt[i].RepSplit.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RepTotal
                lblValue = new Label();
                lblValue.Text = dt[i].RepTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Tier Split
                lblValue = new Label();
                lblValue.Text = dt[i].TierSplit.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Tier Total
                lblValue = new Label();
                lblValue.Text = dt[i].TierTotal.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);               


                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                //tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);

                tr = new TableRow();
                //td = new TableCell();
                //tr.Cells.Add(td);

                //Volume Header
                lblValue = new Label();
                lblValue.Text = "Volume";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //QVol
                lblValue = new Label();
                lblValue.Text = dt[i].Q_Vol.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MVol
                lblValue = new Label();
                lblValue.Text = dt[i].M_Vol.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NVol
                lblValue = new Label();
                lblValue.Text = dt[i].N_Vol.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //QTrans
                lblValue = new Label();
                lblValue.Text = dt[i].Q_Trans.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MTrans
                lblValue = new Label();
                lblValue.Text = dt[i].M_Trans.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NTrans
                lblValue = new Label();
                lblValue.Text = dt[i].N_Trans.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Chargeback
                lblValue = new Label();
                lblValue.Text = dt[i].Chargeback.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Retrieval
                lblValue = new Label();
                lblValue.Text = dt[i].Retrieval.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //BatchHdr
                lblValue = new Label();
                lblValue.Text = dt[i].Batch_Hdr.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //AVSCnt
                lblValue = new Label();
                lblValue.Text = dt[i].AVS_Cnt.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VoiceAuthCnt
                lblValue = new Label();
                lblValue.Text = dt[i].Voice_Auth_Cnt.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VoiceAsstCnt
                lblValue = new Label();
                lblValue.Text = dt[i].Voice_Asst_Cnt.ToString().Trim(); ;
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                tr.Cells.Add(td);

                td = new TableCell();
                tr.Cells.Add(td);

                //Other Card Type
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Debit Trans
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes.Add("colspan", "13");
                tr.Cells.Add(td);

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);

                tr = new TableRow();

                //td = new TableCell();
                //tr.Cells.Add(td);

                //Actual Header
                lblValue = new Label();
                lblValue.Text = "Actual";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //QRate
                lblValue = new Label();
                lblValue.Text = dt[i].Q_Rate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MRate
                lblValue = new Label();
                lblValue.Text = dt[i].M_Rate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NRate
                lblValue = new Label();
                lblValue.Text = dt[i].N_Rate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //QTran
                lblValue = new Label();
                lblValue.Text = dt[i].Q_Tran.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MTran
                lblValue = new Label();
                lblValue.Text = dt[i].M_Tran.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NTran
                lblValue = new Label();
                lblValue.Text = dt[i].N_Tran.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //CBFee
                lblValue = new Label();
                lblValue.Text = dt[i].CB_Fee.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RetrievalFee
                lblValue = new Label();
                lblValue.Text = dt[i].Retrieval_Fee.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //BatchHedr
                lblValue = new Label();
                lblValue.Text = dt[i].Batch_Hedr.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //AVSFee
                lblValue = new Label();
                lblValue.Text = dt[i].AVS_Fee.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VoiceAuth
                lblValue = new Label();
                lblValue.Text = dt[i].Voice_Auth.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VoiceAsst
                lblValue = new Label();
                lblValue.Text = dt[i].Voice_Asst.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //StmtFee
                lblValue = new Label();
                lblValue.Text = dt[i].Stmt_Fee.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MonMin
                lblValue = new Label();
                lblValue.Text = dt[i].Mon_Min.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Other Card Type
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Debit Trans
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Debit Gateway
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes.Add("colspan", "12");
                tr.Cells.Add(td);

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);

                tr = new TableRow();

                //td = new TableCell();
                //tr.Cells.Add(td);

                //Buy Header
                lblValue = new Label();
                lblValue.Text = "Buy";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                lblValue.Font.Bold = true;
                tr.Cells.Add(td);

                //QBUYRATE
                lblValue = new Label();
                lblValue.Text = dt[i].Q_Buy_Rate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MBUYRATE
                lblValue = new Label();
                lblValue.Text = dt[i].M_Buy_Rate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NBUYRATE
                lblValue = new Label();
                lblValue.Text = dt[i].N_Buy_Rate.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //QBTran
                lblValue = new Label();
                lblValue.Text = dt[i].Q_B_Tran.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MBTran
                lblValue = new Label();
                lblValue.Text = dt[i].M_B_Tran.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //NBTran
                lblValue = new Label();
                lblValue.Text = dt[i].N_B_Tran.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //CBBuy
                lblValue = new Label();
                lblValue.Text = dt[i].CB_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //RetBuy
                lblValue = new Label();
                lblValue.Text = dt[i].Ret_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //BatchBuy
                lblValue = new Label();
                lblValue.Text = dt[i].Batch_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //AVSBuy
                lblValue = new Label();
                lblValue.Text = dt[i].AVS_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VAuthBuy
                lblValue = new Label();
                lblValue.Text = dt[i].V_Auth_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //VstBuy
                lblValue = new Label();
                lblValue.Text = dt[i].Vst_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //StmtBuy
                lblValue = new Label();
                lblValue.Text = dt[i].Stmt_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //MMBuy
                lblValue = new Label();
                lblValue.Text = dt[i].Mon_Min_Buy.ToString().Trim();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Other Card Type
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                //Debit Trans
                lblValue = new Label();
                lblValue.Text = "";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes.Add("colspan", "13");
                tr.Cells.Add(td);

                if (i % 2 == 0)
                    tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }//end for rows

            #endregion  

            #region Totals

            if (!bDBA)
            {
                tr = new TableRow();

                td = new TableCell();
                td.Attributes.Add("ColSpan", Convert.ToString(NumColsDisplay - 6));
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "Total: ";
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + ECETotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + RepTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                td = new TableCell();
                tr.Cells.Add(td);

                lblValue = new Label();
                lblValue.Text = "$" + T1RepTotal.ToString();
                td = new TableCell();
                lblValue.ApplyStyle(ValueLabel);
                lblValue.Font.Size = FontUnit.Point(10);
                lblValue.Font.Bold = true;
                td.Controls.Add(lblValue);
                tr.Cells.Add(td);

                tr.BackColor = System.Drawing.Color.WhiteSmoke;
                tblResiduals.Rows.Add(tr);
            }
            #endregion

        }//end if count not 0
        else
            DisplayMessage("No Records found.");
    }//end function Populate

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            lblMonth.Visible = true;
            lblMonth.Text = "iPayment Residuals for the month of: " + lstMonth.SelectedItem.Text;
            string Rep = Session["MasterNum"].ToString();
            //if the Rep List is visible, set the Rep to be searched
            if (lstRepList.Visible == true)
                Rep = lstRepList.SelectedValue;
            Populate(Rep, lstMonth.SelectedItem.Value.ToString().Trim(), false, "");
        }//end try
        catch (Exception err)
        {
            CreateLog Log = new CreateLog();
            Log.ErrorLog(Server.MapPath("~/ErrorLog"), err.Message);
            DisplayMessage("Error Processing Request. Please contact technical support");
        }
    }

    //This function displays error message on a label
    protected void DisplayMessage(string errText)
    {
        lblError.Visible = true;
        lblError.Text = errText;
    }//end function set error message

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Visible = false;
            lblMonth.Visible = false;
            Populate("", "", true, txtLookup.Text.Trim());
        }
        catch (Exception err)
        {
            DisplayMessage(err.Message);
        }
    }
}
