using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;


namespace BusinessLayer
{
    public class RepBL
    {

        private string MasterNum = "";
        public RepBL(string MasterNum)
        {
            this.MasterNum = MasterNum;
        }

        //Returns the current Commission % for a given Rep
        public double ReturnMaxCommPct()
        {
            RepInfoDL Rep = new RepInfoDL();
            double CommPct = Rep.ReturnMaxCommPct(MasterNum);

            return CommPct;
        }

        //Returns the current Rep Split % for a given Rep
        public double ReturnMaxRepSplit()
        {
            RepInfoDL Rep = new RepInfoDL();
            double RepSplit = Rep.ReturnMaxRepSplit(MasterNum);

            return RepSplit;
        }

        public bool CheckTierAccess(string T1MasterNum)
        {
            RepInfoDL Access = new RepInfoDL();
            bool bAccess = Access.CheckTierAccess(MasterNum, T1MasterNum);

            return bAccess;
        }

        //This function gets all CP or CNP packages for Agents and Employees. CALLED BY Home.aspx
        public DataSet GetRepPackages(string CardPresent)
        {
            RepInfoDL Rep = new RepInfoDL();
            DataSet ds = Rep.GetRepPackages(MasterNum, CardPresent);
            return ds;
        }//end function GetPackageListRep

        //This function gets the CP and CNP default package for Agents and Employees. CALLED BY Home.aspx
        public DataSet GetRepDefaultPackage()
        {
            RepInfoDL Rep = new RepInfoDL();
            DataSet ds = Rep.GetRepDefaultPackage(MasterNum);

            return ds;
        }//end function GetDefaultPackageRep           
    }
}
