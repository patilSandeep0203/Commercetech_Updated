using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using BusinessLayer;
using DLPartner;
using System.Text;
using System.IO;
using System.Xml;

namespace attachiPayDoc
{
    /// <summary>
    /// Summary description for WebForm1.
    /// </summary>
    public partial class attachfiles : System.Web.UI.Page
    {
        /*protected System.Web.UI.HtmlControls.HtmlInputFile filUpload;
        protected System.Web.UI.WebControls.Image imgPicture;
        protected System.Web.UI.WebControls.Label lblOutput;
        protected System.Web.UI.WebControls.Button btnUpload;*/

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Session.IsNewSession)
                Response.Redirect("~/login.aspx");

            //Resellers and Affiliates do not have access to this page. Redirect them to the login page.
            if (User.IsInRole("Reseller") || User.IsInRole("Affiliate") || User.IsInRole("Agent") || User.IsInRole("Employee") || User.IsInRole("T1Agent"))
                Response.Redirect("~/login.aspx?Authentication=False");

            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Response.Redirect("~/login.aspx?Authentication=False");

            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        private void btnUpload_Click(object sender, System.EventArgs e)
        {
            // Initialize variables
            //string sSavePath;
            //string sThumbExtension;
            //int intThumbWidth;
            //int intThumbHeight;

            // Set constant values
            //sSavePath = "../AffiliateWiz/Images/ProductImages/";
            //sSavePath = "https://prostores5.carrierzone.com/stores/ecenowcom/catalog/";
            //sSavePath = "Images/";
            //sThumbExtension = "_thumb";
            //intThumbWidth = 160;
            //intThumbHeight = 120;

            // If file field isn't empty
                // Initialize variables
                string sSavePath;
                string sFilename;
                string result = "";
                //string sThumbExtension;
                //int intThumbWidth;
                //int intThumbHeight;

                // Set constant values
                //sSavePath = "../AffiliateWiz/Images/ProductImages/";
                sSavePath = "";
                //sSavePath = "https://prostores5.carrierzone.com/stores/ecenowcom/catalog/";
                //sSavePath = "Images/";
                //sThumbExtension = "_thumb";
                //intThumbWidth = 160;
                //intThumbHeight = 120;

                string strMapPath = "";

                // If file field isn't empty
                if (filUpload.PostedFile != null)
                {
                    // Check file size (mustn't be 0)
                    HttpPostedFile myFile = filUpload.PostedFile;
                    int nFileLen = myFile.ContentLength;
                    if (nFileLen == 0)
                    {
                        lblOutput.Text = "There wasn't any file uploaded.";
                        return;
                    }

                    // Check file extension (must be JPG)
                    /*if (System.IO.Path.GetExtension(myFile.FileName).ToLower() != ".jpg")
                    {
                        lblOutput.Text = "The file must have an extension of JPG";
                        return;
                    }*/

                    // Read file into a data stream
                    byte[] myData = new Byte[nFileLen];
                    myFile.InputStream.Read(myData, 0, nFileLen);
                    // Make sure a duplicate file doesn't exist.  If it does, keep on appending an incremental numeric until it is unique
                    sFilename = System.IO.Path.GetFileName(myFile.FileName);
                    int file_append = 0;
                    while (System.IO.File.Exists(Server.MapPath(sSavePath + sFilename)))
                    {
                        file_append++;
                        sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + System.IO.Path.GetExtension(myFile.FileName);
                    }

                    // Save the stream to disk
                    System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFilename), System.IO.FileMode.Create);
                    newFile.Write(myData, 0, Convert.ToInt32(myData.Length));
                    newFile.Close();

                    strMapPath = Server.MapPath(sSavePath + sFilename);

                    
                    StringBuilder SubmitResults = new StringBuilder();
                    SubmitResults.Append("<ApplicationAttachmentRequest xmlns=\"http://ipaymentinc.com/MerchantApplications/20110105\">");
                    SubmitResults.Append("<ApplicationID></ApplicationID>");
                    SubmitResults.Append("<Description>Original App</Description>");
                    SubmitResults.Append("<Document>");
                    //byte[] bytes = System.IO.File.ReadAllBytes("C:\\MERCHANT_APPLICATION_PACKAGE.pdf");
                    byte[] bytes = System.IO.File.ReadAllBytes(strMapPath);
                    SubmitResults.Append(Convert.ToBase64String(bytes));
                    SubmitResults.Append("</Document>");
                    SubmitResults.Append("<DocumentType>pdf</DocumentType>");
                    SubmitResults.Append("</ApplicationAttachmentRequest>");

                    string URLString = null;
                    //string strAppID = "825820";
                    string strAppID = Convert.ToString(Session["iPayAppID"]).Trim();

                    URLString = "http://api.ipaymentinc.com/MerchantApplications/" + strAppID + "/attachments?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";
                    //string submitUrl = "https://api.ipaymentinc.com/merchantapplications/" + strAppID + "/SubmitApplication?apitoken=3DB65AAE-70F6-11E0-B698-4DE54724019B&usertoken=9028BB7E-F20F-11E1-A924-663B6288709B";
                    HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(URLString);
                    httpReq.Method = "POST";
                    httpReq.ContentType = "application/xml";
                    byte[] postData = System.Text.UTF8Encoding.UTF8.GetBytes(SubmitResults.ToString());
                    httpReq.ContentLength = postData.LongLength;


                    Stream writer = httpReq.GetRequestStream();
                    writer.Write(postData, 0, Convert.ToInt32(httpReq.ContentLength));
                    HttpWebResponse r = default(HttpWebResponse);
                    /*
                    try
                    {
                        r = (HttpWebResponse)httpReq.GetResponse();

                        StreamReader re = new StreamReader(r.GetResponseStream());

                    }
                    catch (Exception ex)
                    {
                        r.Close();

                    }*/

                    r = (HttpWebResponse)httpReq.GetResponse();

                    StreamReader re = new StreamReader(r.GetResponseStream());

                    result = re.ReadToEnd();

                    r.Close();

                    System.Xml.XmlDocument xmlResult = new System.Xml.XmlDocument();

                    xmlResult.LoadXml(result);

                    string strResultPath = Server.MapPath("iPayAttResponse1.xml");
                    xmlResult.Save(strResultPath);

                    XmlNodeList resultNodelist = xmlResult.GetElementsByTagName("ErrorCount");
                    XmlNode resultNode = resultNodelist.Item(0);

                    string strErrorCount = resultNode.InnerText;

                    if ((strAppID != "") && (strErrorCount == "0"))
                    {
                        DisplayMessage("Attachment for application " + strAppID + " is uploaded.");
                    }
                    else
                    {
                        DisplayMessage(result);
                    } 

                    System.IO.File.Delete(Server.MapPath(sSavePath + sFilename));

                    //DisplayMessage(strAppID);


                    // Check whether the file is really a JPEG by opening it

                    //System.Drawing.Image.GetThumbnailImageAbort myCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                    //Bitmap myBitmap;
                    //try
                    //{
                    //  myBitmap = new Bitmap(Server.MapPath(sSavePath + sFilename));

                    // If jpg file is a jpeg, create a thumbnail filename that is unique.
                    /*file_append = 0;
                    string sThumbFile = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + sThumbExtension + ".jpg";
                    while (System.IO.File.Exists(Server.MapPath(sSavePath + sThumbFile)))
                    {
                        file_append++;
                        sThumbFile = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + sThumbExtension + ".jpg";
                    }*/

                    // Save thumbnail and output it onto the webpage
                    /*System.Drawing.Image myThumbnail = myBitmap.GetThumbnailImage(intThumbWidth, intThumbHeight, myCallBack, IntPtr.Zero);
                    myThumbnail.Save(Server.MapPath(sSavePath + sThumbFile));*/
                    //imgPicture.ImageUrl = sSavePath + sFilename;

                    // Displaying success information
                    //lblOutput.Text = "File uploaded successfully!";

                    // Destroy objects
                    //myThumbnail.Dispose();
                    //myBitmap.Dispose();
                    //}
                    //catch (ArgumentException errArgument)
                    //{
                    // The file wasn't a valid jpg file
                    //  lblOutput.Text = "The file wasn't a valid image file. Please try uploading another format (ex: .jpg)";
                    //  System.IO.File.Delete(Server.MapPath(sSavePath + sFilename));
                    //}
                }
                else
                {
                    CreateLog Log = new CreateLog();
                    Log.ErrorLog(Server.MapPath("~/ErrorLog"), "iPayment attachment - did not find attachment.");
                    DisplayMessage("No file attached.");
                }


        }

        protected void DisplayMessage(string errText)
        {
            lblError.Visible = true;
            lblError.Text = errText;
        }//end function set error message
    }
}
