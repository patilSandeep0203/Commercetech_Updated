using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class RepInfoBL
    {
        //This function returns repnum based on repname
        public string ReturnMasterNum(string RepName)
        {
            RepInfoDL Rep = new RepInfoDL();
            string RepNum = Rep.ReturnMasterNum(RepName);
            return RepNum;
        }

        public string ReturnOfficeMasterNum(string MasterNum)
        {
            RepInfoDL Rep = new RepInfoDL();
            string RepNum = Rep.ReturnOfficeMasterNum(MasterNum);
            return RepNum;
        }

        public DataSet ReturnOfficeAgentMasterNum(string MasterNum)
        {
            RepInfoDL Rep = new RepInfoDL();
            DataSet dsRepNum = Rep.ReturnOfficeAgentMasterNum(MasterNum);
            return dsRepNum;
        }

        //This function adds agent Info to the RepInfo and RepInfoMonthly for the current month
        //CALLED BY AddPartner.aspx for admins
        public bool AddPartnerInfo(string RepName, string Company, string DBA,  
            string SageDeclinedStatus, string SageNum, string IPSNum, string UnoUsername, string UnoPassword, string IPay3Num,
            string IMS2Num, string ChaseNum, string RepSplit, string RepCat, string Comm, string AffiliateID,
            string FundMin, string RefMin, string ResidualMin, string MasterNum, string T1MasterNum)
        {
            MonthDL Mon = new MonthDL();
            String CurrMon = Mon.ReturnCurrMon();
            
            bool retVal = false;    
            RepInfoDL AddInfo = new RepInfoDL();
            retVal = AddInfo.InsertRepInfo(RepName, Company, DBA, SageNum, IPSNum, IPay3Num, IMS2Num, ChaseNum,
                    RepSplit, RepCat, Comm, AffiliateID, FundMin, RefMin, ResidualMin, MasterNum);

            if (SageDeclinedStatus == "Yes")
            {
                retVal = AddInfo.UpdateSageDeclinedStatus(MasterNum, true);
                //Update default rate packages to iPayment
                AffiliatesDL Aff = new AffiliatesDL();
                retVal = Aff.UpdateDefaultPackage(Convert.ToInt32(AffiliateID), 191, 193);
            }
            
            retVal = AddInfo.UpdateRepInfoUnoLogin(MasterNum, UnoUsername, UnoPassword);
            
            RepInfoDL RepInfo = new RepInfoDL();
            retVal = RepInfo.InsertUpdateRepInfoMonthly(RepName, CurrMon, RepSplit, RepCat, Comm,
                FundMin, RefMin, ResidualMin, MasterNum, T1MasterNum);

            //Add rep info to ACT dropdown
            ACTDataDL Act = new ACTDataDL();
            retVal = Act.InsertRepInfoInACT(RepName, SageNum, IPSNum, IPay3Num, IMS2Num, ChaseNum, MasterNum);
            
            return retVal;
        }//end function AddPartnerInfo
       
        //This function returns partner info for the Current Month of the year - CALLED BY AddPartner.aspx, UpdatePartner.aspx
        public DataSet GetPartnerInfoCurrMon(string MasterNum)
        {           
            MonthDL Mon = new MonthDL();
            String CurrMon = Mon.ReturnCurrMon();
       
            RepInfoDL Partner = new RepInfoDL();
            DataSet ds = Partner.GetPartnerInfoMon(MasterNum, CurrMon);
           return ds;
        }//end function GetPartnerInfo

         //This function returns partner info for the Current Month of the year - CALLED BY AddPartner.aspx, UpdatePartner.aspx
        public DataSet GetPartnerSplits(string MasterNum)
         {
             RepInfoDL Partner = new RepInfoDL();
             DataSet ds = Partner.GetPartnerSplits(MasterNum);
             return ds;
         }//end function GetPartnerSplits

        //This function Updates Partner Info. CALLED BY UpdatePartner.aspx by Admin
        public bool UpdatePartnerInfoCurrMon(string RepName, string Company, string DBA, string SageDeclinedStatus, string SageNum, string UnoUsername,
            string UnoPassword, string IPay3Num, string iPaySalesID, string IMS2Num, string ChaseNum, string IPSNum, string RepSplit, string RepCat, string Comm,
            string MasterNum, string T1MasterNum, int PID, int CPPID, int AID, string FundMin, string RefMin, string ResidualMin, 
            string PrevMasterNum)
        {
            MonthDL Mon = new MonthDL();
            String CurrMon = Mon.ReturnCurrMon();
            bool retVal = false;


            RepInfoDL Rep = new RepInfoDL();
            AffiliatesDL Aff = new AffiliatesDL();
            retVal = Rep.UpdateRepInfo(RepName, Company, DBA, SageNum, IPay3Num, iPaySalesID, IMS2Num, ChaseNum, IPSNum, 
                RepSplit, RepCat, Comm, MasterNum, T1MasterNum, FundMin, RefMin, ResidualMin, PrevMasterNum);

            retVal = Rep.InsertUpdateRepInfoMonthly(RepName, CurrMon, RepSplit, RepCat, Comm, FundMin, 
             RefMin, ResidualMin, MasterNum, T1MasterNum);

            retVal = Aff.UpdateDefaultPackage(AID, PID, CPPID);

            if (SageDeclinedStatus == "Yes")
                retVal = Rep.UpdateSageDeclinedStatus(MasterNum, true);

            retVal = Rep.UpdateRepInfoUnoLogin(MasterNum, UnoUsername, UnoPassword);

            //Add Rep Numbers to ACT dropdown (if they don't already exist)
            ACTDataDL Act = new ACTDataDL();
            retVal = Act.InsertRepInfoInACT(RepName, SageNum, IPSNum, IPay3Num, IMS2Num, ChaseNum, MasterNum);
            return retVal;
        }//end function UpdatePartnerInfo
       

        //This function checks if new month has been added and add partner info for new month
        //CALLED BY ManagePartners.aspx
        public bool AddNewMonth(string Mon)
        {
          MonthDL Month = new MonthDL();
          bool retVal = Month.CheckRepMonExists(Mon);

            //if the query in RepInfoMonthly does not find the month specified, 
            //add to each to Rep
            if (!retVal)
            {
                try
                {
                    //Get Rep Info
                    RepInfoDL AllReps = new RepInfoDL();
                    DataSet dsRepAll = AllReps.GetRepInfoAll();
                    if (dsRepAll.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr;
                        for (int i = 0; i < dsRepAll.Tables[0].Rows.Count; i++)
                        {
                            //Insert rep info for each rep
                            dr = dsRepAll.Tables[0].Rows[i];
                            RepInfoDL Rep = new RepInfoDL();
                            retVal = Rep.InsertUpdateRepInfoMonthly(dr["RepName"].ToString().Trim(), Mon, dr["RepSplit"].ToString().Trim(),
                                dr["RepCat"].ToString().Trim(), dr["Comm"].ToString().Trim(),
                                dr["FundMin"].ToString().Trim(), dr["RefMin"].ToString().Trim(),
                                dr["ResidualMin"].ToString().Trim(), dr["MasterNum"].ToString().Trim(), dr["T1MasterNum"].ToString().Trim());
                        }//end for
                    }//end if count not 0
                }//end try
                catch (Exception err)
                {
                    throw err;
                }
            }//end if count not 0
            else
                retVal = true;
            return retVal;
        }//end function AddNewMonth

        //This function gets partners based on the Rep Cat - CALLED BY ManagePartners.aspx
        public DataSet GetPartners(string Mon, string RepCat, string T1MasterNum)
        {
            RepInfoDL Partners = new RepInfoDL();
            DataSet ds = Partners.GetPartners(Mon, RepCat, T1MasterNum);
            return ds;
        }//end function GetPartners

        //This function gets the Uno Login Credentials. CALLED BY UpdatePartner.aspx and XMLBL.cs
        public DataSet GetRepInfoUnoLogin(string MasterNum)
        {
            RepInfoDL Rep = new RepInfoDL();
            DataSet ds = Rep.GetRepInfoUnoLogin(MasterNum);

            return ds;
        }//end function GetRepInfoUnoLogin   

    
    }//end class RepInfoBL


}
