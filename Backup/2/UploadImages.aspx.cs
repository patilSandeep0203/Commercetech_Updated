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

namespace netimageupload
{
    /// <summary>
    /// Summary description for WebForm1.
    /// </summary>
    public partial class uploadfiles : System.Web.UI.Page
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
            string sSavePath;
            //string sThumbExtension;
            //int intThumbWidth;
            //int intThumbHeight;

            // Set constant values
            sSavePath = "../AffiliateWiz/Images/ProductImages/";
            //sSavePath = "https://prostores5.carrierzone.com/stores/ecenowcom/catalog/";
            //sSavePath = "Images/";
            //sThumbExtension = "_thumb";
            //intThumbWidth = 160;
            //intThumbHeight = 120;

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
                string sFilename = System.IO.Path.GetFileName(myFile.FileName);
                int file_append = 0;
                while (System.IO.File.Exists(Server.MapPath(sSavePath + sFilename)))
                {
                    file_append++;
                    sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + ".jpg";
                    //System.IO.File.Delete(Server.MapPath(sSavePath + sFilename));
                }

                // Save the stream to disk
                System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFilename), System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                // Check whether the file is really a JPEG by opening it
                System.Drawing.Image.GetThumbnailImageAbort myCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                Bitmap myBitmap;
                try
                {
                    myBitmap = new Bitmap(Server.MapPath(sSavePath + sFilename));

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
                    imgPicture.ImageUrl = sSavePath + sFilename;

                    // Displaying success information
                    lblOutput.Text = "File uploaded successfully!";

                    // Destroy objects
                    //myThumbnail.Dispose();
                    myBitmap.Dispose();
                }
                catch (ArgumentException errArgument)
                {
                    // The file wasn't a valid jpg file
                    lblOutput.Text = "The file wasn't a valid image file. Please try uploading another format (ex: .jpg)";
                    System.IO.File.Delete(Server.MapPath(sSavePath + sFilename));
                }
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
    }
}