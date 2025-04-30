using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class UsersBL
    {

        //CALLED BY login.aspx
        //Returns true if username and password match       
        public bool CheckLogin(string LoginID, string Password)
        {
            bool bLogin = false;
            UsersDL User = new UsersDL();
            //Dataset contains the login ID if the password matches
            DataSet ds = User.CheckLogin(LoginID, Password);
            if (ds.Tables[0].Rows.Count > 0)
                bLogin = true;
            return bLogin;
        }

        //CALLED BY login.aspx
        //Returns if the User ID (AffiliateID) exists
        public bool CheckIDExists(int iUserID)
        {
            bool bLogin = false;
            UsersDL Login = new UsersDL();
            bLogin = Login.CheckIDExists(iUserID);
            return bLogin;
        }

        //CALLED BY Login.aspx
        public DataSet VerifyToken(string strToken)
        {
            UsersDL User = new UsersDL();
            DataSet ds = User.VerifyLoginToken(strToken);
            return ds;
        }

        public string CreateLoginToken(string appName, string loginID, int userID, int appID)
        {
            UsersDL User = new UsersDL();
            string strToken = User.CreateLoginToken(appName, loginID, userID, appID);
            return strToken;
        }

        //This function resets password - CALLED BY ManageUsers.aspx and ChangePWD.aspx
        public int ResetAffiliatePassword(int AffiliateID, string Password)
        {
            UsersDL Login = new UsersDL();
            int iRetVal = Login.UpdatePassword(AffiliateID, Password);
            return iRetVal;
        }

        public DataSet CheckUserAccess(string LoginName, string Password)
        {
            UsersDL Login = new UsersDL();
            DataSet dsAccess = Login.CheckLogin(LoginName, Password);
            return dsAccess;
        }

        //This function gets the user roles for a user in a Data Set
        public DataSet GetRoles(string UserName)
        {
            UsersDL Roles = new UsersDL();
            DataSet ds = Roles.GetRoles(UserName);
            return ds;
        }//end function GetRoles

        //Gets LoginInfo BY AffiliateID
        public DataSet GetLoginInfo(int AffiliateID)
        {
            UsersDL Roles = new UsersDL();
            DataSet ds = Roles.GetLoginInfo(AffiliateID);
            return ds;
        }

        public int GetAffiliateIDbyLoginID(string PortalUserID)
        {
            UsersDL Roles = new UsersDL();
            int AffiliateID = Roles.GetAffiliateIDbyLoginID(PortalUserID);
            return AffiliateID;
        }

        //Gets LoginInfo BY LoginID
        public DataSet GetLoginInfoByLoginID(string LoginID)
        {
            UsersDL Roles = new UsersDL();
            DataSet ds = Roles.GetLoginInfoByLoginID(LoginID);
            return ds;
        }

        //
        public DataSet GetAppsByLoginID(string UserName)
        {
            UsersDL User = new UsersDL();
            DataSet ds = User.GetAppsByLoginID(UserName);
            return ds;
        }//end function GetRoles

        
        //This function returns user roles
        public DataSet GetRoleByApp(string UserName, int iAppID)
        {
            UsersDL User = new UsersDL();
            DataSet ds = User.GetRoleByApp(UserName, iAppID);
            return ds;
        }//end function GetRolesByApp
        
 
    }//end class ManageUsersInfo
}
