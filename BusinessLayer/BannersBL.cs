using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class BannersBL
    {
        //Gets banner groups. Called by Buildbanners.aspx.cs
        public DataSet GetGroups()
        {
            BannersDL Banners = new BannersDL();
            DataSet ds = Banners.ReturnGroups();
            return ds;
        }//end function GetGroups

        //Gets banners. Called by Buildbanners.aspx.cs
        public DataSet GetBannersByGroupID(int GroupID)
        {
            BannersDL Banners = new BannersDL();
            DataSet ds = Banners.ReturnBannersByGroupID(GroupID);
            return ds;
        }//end function GetBannersByGroupID

        //Gets Groups based on GroupID. Called by BuildTextLinks.aspx.cs
        public DataSet GetGroupsByGroupID(int GroupID)
        {
            BannersDL Banners = new BannersDL();
            DataSet ds = Banners.ReturnTextLinksByGroupID(GroupID);
            return ds;
        }//end function GetGourpsByGroupID
    }//end class BannersBL
}
