using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;


namespace BusinessLayer
{
    public class Reminder
    {
       private int AppId  = 0;

       private OnlineAppReminderTableAdapter _OnlineAppRemindersAdapter = null;
       protected OnlineAppReminderTableAdapter Adapter
       {
           get
           {
               if (_OnlineAppRemindersAdapter == null)
                   _OnlineAppRemindersAdapter = new OnlineAppReminderTableAdapter();

               return _OnlineAppRemindersAdapter;
           }
       }

       public Reminder (int AppId)
        {
            this.AppId = AppId;
        }

        //This function gets summary info for sending reminder. CALLED BY SendReminder.aspx
        public PartnerDS.OnlineAppReminderDataTable GetReminderSummary()
        {
            return Adapter.GetData(Convert.ToInt16(AppId) );
        }//end function GetReminderSummary

        //This function Inserts a Note into the Online App and ACT. CALLED BY SendReminder.aspx
        public bool InsertNoteReminder(string partnerID, string Note)
        {
            ACTDataDL ACT = new ACTDataDL();

            string partnerContactID = ACT.ReturnContactID(partnerID);
            OnlineAppStatusDL NoteInsert = new OnlineAppStatusDL();
            //Insert Note in Online App
            DataSet ds = NoteInsert.InsertNote(partnerContactID, AppId, Note, DateTime.Now.ToString());

            DataRow dr = ds.Tables[0].Rows[0];
            //Get the NoteID generated
            string NoteID = dr["NoteID"].ToString();

            //Get the ContactID associated with this AppID
            
            string ContactID = ACT.ReturnContactID(AppId);
            //Insert Note into ACT if a valid ContactID is found

            if (ContactID != "")
                //ACT.InsertNotes(NoteID, ContactID, ActUserID, Note, DateTime.Now.ToString());
                ACT.InsertHistory(NoteID, ContactID, partnerContactID, DateTime.Now.ToString(), Note);
            else
                //return false if this record does not exist in ACT
                return false;

            return true;
        }
        public bool InsertACTHistory(string ActUserID, string strTo, string strCc, string strSubject, string strBody)
        {
            //Get the ContactID associated with this AppID
            ACTDataDL ACT = new ACTDataDL();
            string ContactID = ACT.ReturnContactID(AppId);
            if (ContactID != "")
            {
                // Creates a new Outlook Application Instance
                Outlook.Application objOutlook = new Outlook.Application();
                // Creating a new Outlook Message from the Outlook Application Instance
                Outlook.MailItem mic = (Outlook.MailItem)(objOutlook.CreateItem(Outlook.OlItemType.olMailItem));
                // Assigns the "TO", "CC" and "BCC" Fields
                mic.To = strTo.ToString();
                mic.CC = strCc.ToString();
                mic.BCC = "";
                // Assigns the Subject Field
                mic.Subject = strSubject.ToString();
                //Assigns Importance to Normal
                mic.Importance = Outlook.OlImportance.olImportanceNormal;
                // Define the Mail Message Body. In this example, you can add in HTML content to the mail message body
                mic.HTMLBody = strBody.ToString();

                string FileName = "RE" + ContactID.ToString() + ".msg";
                string DisplayName = FileName;

                mic.SaveAs(@"C:\Shared\ACT\LAContacts-database files\Attachments\" + FileName, Outlook.OlSaveAsType.olMSG);

                ACT.InsertHistory(ContactID, ActUserID, DateTime.Now.ToString(), FileName, DisplayName);
            }
            else
                //return false if this record does not exist in ACT
                return false;

            return true;
        }
    }//end class Reminder
}
