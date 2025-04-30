using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class UserRolesBL
    {

        //CALLED BY login.aspx

        //This function gets user info. CALLED BY ManageUsers.aspx
        public DataSet GetUsersByAccess(string Access, string iAppId)
        {
            UserRolesDL Users = new UserRolesDL();
            DataSet ds = Users.GetUsersByAccess(Access, iAppId);
            return ds;
        }//end function GetUsers

        //CALLED BY ManageUsers.aspx
        public DataSet GetAffiliate(int AffiliateID)
        {
            UsersDL Users = new UsersDL();
            DataSet ds = Users.GetUserInfo(AffiliateID);
            return ds;
        }


        //This function gets user roles from AffiliateID (iUserID) - CALLED BY ManageUsers.aspx.cs
        public DataSet GetRolesByAffiliateID(int iUserID)
        {
            UserRolesDL Roles = new UserRolesDL();
            DataSet ds = Roles.GetRolesByAffiliateID(iUserID);
            return ds;
        }//end function GetRolesByAffiliateID

        //This function gets user roles description. CALLED BY ManageUsers.aspx.cs
        public DataSet GetRolesDescription()
        {
            UserRolesDL Roles = new UserRolesDL();
            DataSet ds = Roles.GetRolesDescription();
            return ds;
        }//end function GetRolesDescription

        //This function updates User Roles - CALLED BY ManageUser.aspx.cs
        public int UpdateUserRoles(string iUserID, string iAppId, string Access)
        {
            UserRolesDL Roles = new UserRolesDL();
            int iRetVal = Roles.UpdateUserRoles(iUserID, iAppId, Access);
            return iRetVal;
        }
    }//end class ManageUsersInfo
}
