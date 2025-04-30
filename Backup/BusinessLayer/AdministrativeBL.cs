using System;
using System.Data;
using DLPartner;
using DLPartner.PartnerDSTableAdapters;

namespace BusinessLayer
{
    public class AdministrativeBL
    {
        private AdministrativeInfoTableAdapter _AdministrativeInfoAdapter = null;
        protected AdministrativeInfoTableAdapter AdministrativeInfoAdapter
        {
            get
            {
                if (_AdministrativeInfoAdapter == null)
                    _AdministrativeInfoAdapter = new AdministrativeInfoTableAdapter();

                return _AdministrativeInfoAdapter;
            }
        }

        //This function returns Update Status date and name - CALLED BY OnlineAppMgmt/default.aspx
        public PartnerDS.AdministrativeInfoDataTable GetAdministrativeInfo()
        {
            return AdministrativeInfoAdapter.GetData();            
        }//end function GetSummary

        //This function returns Update Status date - CALLED BY OnlineAppMgmt/default.aspx
        public int UpdateAdministrativeInfo(string UserName)
        {
            AdministrativeDL Admin = new AdministrativeDL();
            int iRetVal = Admin.UpdateAdministrativeInfo(UserName);
            return iRetVal;
        }//end function GetSummary
    }//end class AdministrativeBL
}
