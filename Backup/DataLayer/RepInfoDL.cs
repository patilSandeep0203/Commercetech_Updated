using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DLPartner
{
    public class RepInfoDL
    {
        private static string ConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private static string ConnStringPartner = ConfigurationManager.AppSettings["ConnectionStringPartner"].ToString();

        //CALLED BY RepInfoBL.GetSalesRepList
        public DataSet GetSalesRepList()
        {
          SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepList = new SqlCommand("sp_GetRepList", Conn);
                cmdRepList.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRepList;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                Conn.Close();
                Conn.Dispose();
                return ds;       
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetSalesRepList

        //CALLED BY RepInfoBL.GetT1RepList
        public DataSet GetT1RepList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetT1RepList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                Conn.Close();
                Conn.Dispose();
                return ds;
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetT1RepList

        
        //This function gets all the rep info. Called By RepInfoBL.AddNewMonth
        public DataSet GetRepInfoAll()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdRepInfoAll = new SqlCommand("sp_GetRepInfoAll", Conn);
                cmdRepInfoAll.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRepInfoAll;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");

                Conn.Close();
                Conn.Dispose();
                return ds;

            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetRepInfoAll

        public DataSet GetRepInfo(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetRepInfo", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                pNum.Value = MasterNum;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetRepInfo

        public DataSet GetRepInfoByAffiliateID(string AffiliateID)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdResd = new SqlCommand("sp_GetRepInfoByAffiliateID", Conn);
                cmdResd.CommandType = CommandType.StoredProcedure;
                SqlParameter pNum = cmdResd.Parameters.Add("@AffiliateID", SqlDbType.VarChar);
                pNum.Value = AffiliateID;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdResd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetRepInfo


        //This function returns partners for manage partners. CALLED BY RepInfoBL.GetPartners
        public DataSet GetPartners(string Month, string RepCat, string T1MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdList = new SqlCommand("sp_GetPartners", Conn);
                cmdList.CommandType = CommandType.StoredProcedure;

                SqlParameter pMon = cmdList.Parameters.Add("@mon", SqlDbType.VarChar);
                SqlParameter pRepCat = cmdList.Parameters.Add("@RepCat", SqlDbType.VarChar);
                SqlParameter pT1MasterNum = cmdList.Parameters.Add("@T1MasterNum", SqlDbType.VarChar);

                pMon.Value = Month;
                pRepCat.Value = RepCat;
                pT1MasterNum.Value = T1MasterNum;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdList;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPartnerList

        //This function returns partners for manage partners. CALLED BY RepInfoBL.GetPartners
        public DataSet GetPartnersByOffice(string Month, string RepCat, string OfficeMasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdList = new SqlCommand("sp_GetPartners", Conn);
                cmdList.CommandType = CommandType.StoredProcedure;

                SqlParameter pMon = cmdList.Parameters.Add("@mon", SqlDbType.VarChar);
                SqlParameter pRepCat = cmdList.Parameters.Add("@RepCat", SqlDbType.VarChar);
                SqlParameter pT1MasterNum = cmdList.Parameters.Add("@T1MasterNum", SqlDbType.VarChar);

                pMon.Value = Month;
                pRepCat.Value = RepCat;
                pT1MasterNum.Value = OfficeMasterNum;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdList;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPartnerList

        //This function gets rep category list - CALLED BY RepInfoBL.GetRepCatList
        public DataSet GetRepCatList()
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmds = new SqlCommand("sp_GetRepCatList", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepCategory");
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetRepCatList

        //CALLED BY RepInfoBL.GetRepPackages
        public DataSet GetRepPackages(string MasterNum, string CardPresent)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {

                SqlCommand cmds = new SqlCommand("sp_GetRepPackages", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection.Open();
                cmds.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmds.Parameters.Add(new SqlParameter("@CardPresent", CardPresent));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetCNPPackageList


        //CALLED BY RepInfoBL.GetPackageListRep
        public DataSet GetRepDefaultPackage(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmds = new SqlCommand("sp_GetRepDefaultPackage", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Connection.Open();
                cmds.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetCNPPackageList

      
        

        //Returns True if T1 has Access to MasterNum
        public bool CheckTierAccess(string MasterNum, string T1MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);

            int RetVal = 0;
            try
            {
                SqlCommand cmdRepAccess = new SqlCommand("sp_CheckTierAccess", Conn);
                cmdRepAccess.CommandType = CommandType.StoredProcedure;
                cmdRepAccess.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmdRepAccess.Parameters.Add(new SqlParameter("@T1MasterNum", T1MasterNum));
                
                SqlParameter pRetVal = cmdRepAccess.Parameters.Add("@RetVal", SqlDbType.SmallInt);
                pRetVal.Direction = ParameterDirection.ReturnValue;

                cmdRepAccess.Connection.Open();
                cmdRepAccess.ExecuteNonQuery();
                RetVal = Convert.ToInt32(pRetVal.Value);

                return Convert.ToBoolean(RetVal);
            }
            catch (SqlException sqlerr)
            {
                throw sqlerr;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function CheckTierAccess

        //This function Inserts/Updates rep table
        public bool UpdateRepInfo(string RepName, string Company, string DBA, string SageNum, string IPay3Num, string iPaySalesID,
            string IMS2Num, string ChaseNum, string IPSNum, string RepSplit, string RepCat, string Comm, string MasterNum,
            string T1MasterNum, string FundMin, string RefMin, string ResidualMin, string PrevMasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("sp_UpdateRepInfo", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepName = cmdRepInfo.Parameters.Add("@RepName", SqlDbType.VarChar);
                SqlParameter pCompany = cmdRepInfo.Parameters.Add("@CompanyName", SqlDbType.VarChar);
                SqlParameter pDBA = cmdRepInfo.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pSageNum = cmdRepInfo.Parameters.Add("@SageNum", SqlDbType.VarChar);
                SqlParameter pIPay3Num = cmdRepInfo.Parameters.Add("@IPay3Num", SqlDbType.VarChar);
                SqlParameter piPaySalesID = cmdRepInfo.Parameters.Add("@iPaySalesID", SqlDbType.VarChar);
                SqlParameter pIMS2Num = cmdRepInfo.Parameters.Add("@IMS2Num", SqlDbType.VarChar);
                SqlParameter pChaseNum = cmdRepInfo.Parameters.Add("@ChaseNum", SqlDbType.VarChar);
                SqlParameter pIPSNum = cmdRepInfo.Parameters.Add("@IPSNum", SqlDbType.VarChar);
                SqlParameter pRepSplit = cmdRepInfo.Parameters.Add("@RepSplit", SqlDbType.Decimal);
                SqlParameter pRepCat = cmdRepInfo.Parameters.Add("@RepCat", SqlDbType.VarChar);
                SqlParameter pComm = cmdRepInfo.Parameters.Add("@Comm", SqlDbType.Decimal);
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pFundMin = cmdRepInfo.Parameters.Add("@FundMin", SqlDbType.VarChar);
                SqlParameter pRefMin = cmdRepInfo.Parameters.Add("@RefMin", SqlDbType.VarChar);
                SqlParameter pResidualMin = cmdRepInfo.Parameters.Add("@ResidualMin", SqlDbType.VarChar);
                SqlParameter pPrevMasterNum = cmdRepInfo.Parameters.Add("@PrevMasterNum", SqlDbType.VarChar);
                SqlParameter pT1MasterNum = cmdRepInfo.Parameters.Add("@T1MasterNum", SqlDbType.VarChar);

                pRepName.Value = RepName;
                pCompany.Value = Company;
                pDBA.Value = DBA;
                pSageNum.Value = SageNum;
                pIPay3Num.Value = IPay3Num;
                piPaySalesID.Value = iPaySalesID;
                pIMS2Num.Value = IMS2Num;
                pChaseNum.Value = ChaseNum;
                pIPSNum.Value = IPSNum;
                pRepSplit.Value = RepSplit;
                pRepCat.Value = RepCat;
                pComm.Value = Comm;
                pMasterNum.Value = MasterNum;
                pFundMin.Value = FundMin;
                pRefMin.Value = RefMin;
                pResidualMin.Value = ResidualMin;
                pPrevMasterNum.Value = PrevMasterNum;
                pT1MasterNum.Value = T1MasterNum;
                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }//end function UpdateRepInfo

        //This function Inserts/Updates repinfomonthly table - CALLED BY RepInfoBL.AddPartnerInfo, RepInfoBL.UpdatePartnerInfoCurrMon, RepInfoBL.AddNewMonth
        public bool InsertUpdateRepInfoMonthly(string RepName, string CurrMon, string RepSplit, string RepCat,
            string Comm, string FundMin, string RefMin, string ResidualMin, string MasterNum, string T1MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("sp_InsertUpdateRepInfoMonthly", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepName = cmdRepInfo.Parameters.Add("@RepName", SqlDbType.VarChar);
                SqlParameter pCurrMon = cmdRepInfo.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pRepSplit = cmdRepInfo.Parameters.Add("@RepSplit", SqlDbType.Decimal);
                SqlParameter pRepCat = cmdRepInfo.Parameters.Add("@RepCat", SqlDbType.VarChar);
                SqlParameter pComm = cmdRepInfo.Parameters.Add("@Comm", SqlDbType.Decimal);
                SqlParameter pFundMin = cmdRepInfo.Parameters.Add("@FundMin", SqlDbType.VarChar);
                SqlParameter pRefMin = cmdRepInfo.Parameters.Add("@RefMin", SqlDbType.VarChar);
                SqlParameter pResidualMin = cmdRepInfo.Parameters.Add("@ResidualMin", SqlDbType.VarChar);
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pT1MasterNum = cmdRepInfo.Parameters.Add("@T1MasterNum", SqlDbType.VarChar);

                pRepName.Value = RepName;
                pCurrMon.Value = CurrMon;
                pRepSplit.Value = RepSplit;
                pRepCat.Value = RepCat;
                pComm.Value = Comm;
                pFundMin.Value = FundMin;
                pRefMin.Value = RefMin;
                pResidualMin.Value = ResidualMin;
                pMasterNum.Value = MasterNum;
                pT1MasterNum.Value = T1MasterNum;

                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function InsertUpdateRepInfoMonthly

        //This function Updates RepInfoSage Table with UNO login info
        public bool UpdateRepInfoUnoLogin(string MasterNum, string UnoUsername, string UnoPassword)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("sp_UpdateUnoLogin", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);                
                SqlParameter pUnoUsername = cmdRepInfo.Parameters.Add("@UnoUsername", SqlDbType.VarChar);
                SqlParameter pUnoPassword = cmdRepInfo.Parameters.Add("@UnoPassword", SqlDbType.VarChar);

                pMasterNum.Value = MasterNum;
                pUnoUsername.Value = UnoUsername;
                pUnoPassword.Value = UnoPassword;
                
                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }//end function UpdateRepInfoUnoLogin

        //This function Updates RepInfoSage Table with Sage Declined Status
        public bool UpdateSageDeclinedStatus(string MasterNum, bool SageDeclinedStatus)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("sp_UpdateSageDeclinedStatus", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pSageDeclinedStatus = cmdRepInfo.Parameters.Add("@SageDeclinedStatus", SqlDbType.Bit);

                pMasterNum.Value = MasterNum;
                pSageDeclinedStatus.Value = SageDeclinedStatus;

                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }//end function UpdateSageDeclinedStatus

        //This function gets the Rep List
        public DataSet GetRepInfoUnoLogin(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("sp_GetUnoLogin", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                cmdRepInfo.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRepInfo;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPartnerList

        //This function gets the Rep List
        public DataSet GetPartnerList()
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select RepName, MasterNum, AffiliateID from RepInfo Order By RepName ASC";
                SqlCommand cmds = new SqlCommand(strQuery, Conn);
                cmds.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnPartnerList

        //CALLED BY RepInfoBL.GetPartnerInfoCurrMon
        public DataSet GetPartnerInfoMon(string MasterNum, string Mon)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmds = new SqlCommand("SP_GetPartnerInfoMon", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                cmds.Parameters.Add(new SqlParameter("@Mon", Mon));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;                
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetPartnerInfoMon

        //CALLED BY RepInfoBL.GetPartnerSplits
        public DataSet GetPartnerSplits(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmds = new SqlCommand("SP_GetPartnerSplits", Conn);
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmds;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }//end try
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function GetPartnerSplits

        //This function Inserts rep table - CALLED BY RepInfoBL.AddPartnerInfo
        public bool InsertRepInfo(string RepName, string Company, string DBA, string SageNum, string IPSNum, string IPay3Num,
            string IMS2Num, string ChaseNum, string RepSplit, string RepCat, string Comm, string AffiliateID,
            string FundMin, string RefMin, string ResidualMin, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("SP_InsertRepInfo", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepName = cmdRepInfo.Parameters.Add("@RepName", SqlDbType.VarChar);
                SqlParameter pCompany = cmdRepInfo.Parameters.Add("@CompanyName", SqlDbType.VarChar);
                SqlParameter pDBA = cmdRepInfo.Parameters.Add("@DBA", SqlDbType.VarChar);
                SqlParameter pSageNum = cmdRepInfo.Parameters.Add("@SageNum", SqlDbType.VarChar);
                SqlParameter pIPSNum = cmdRepInfo.Parameters.Add("@IPSNum", SqlDbType.VarChar);
                SqlParameter pIPay3Num = cmdRepInfo.Parameters.Add("@IPay3Num", SqlDbType.VarChar);
                SqlParameter pIMS2Num = cmdRepInfo.Parameters.Add("@IMS2Num", SqlDbType.VarChar);
                SqlParameter pChaseNum = cmdRepInfo.Parameters.Add("@ChaseNum", SqlDbType.VarChar);
                SqlParameter pRepSplit = cmdRepInfo.Parameters.Add("@RepSplit", SqlDbType.Decimal);
                SqlParameter pRepCat = cmdRepInfo.Parameters.Add("@RepCat", SqlDbType.VarChar);
                SqlParameter pComm = cmdRepInfo.Parameters.Add("@Comm", SqlDbType.Decimal);
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);
                SqlParameter pFundMin = cmdRepInfo.Parameters.Add("@FundMin", SqlDbType.VarChar);
                SqlParameter pRefMin = cmdRepInfo.Parameters.Add("@RefMin", SqlDbType.VarChar);
                SqlParameter pResidualMin = cmdRepInfo.Parameters.Add("@ResidualMin", SqlDbType.VarChar);
                SqlParameter pAffiliateID = cmdRepInfo.Parameters.Add("@AffiliateID", SqlDbType.VarChar);

                pRepName.Value = RepName;
                pCompany.Value = Company;
                pDBA.Value = DBA;
                pSageNum.Value = SageNum;
                pIPSNum.Value = IPSNum;
                pIPay3Num.Value = IPay3Num;
                pIMS2Num.Value = IMS2Num;
                pChaseNum.Value = ChaseNum;
                pRepSplit.Value = RepSplit;
                pRepCat.Value = RepCat;
                pComm.Value = Comm;
                pMasterNum.Value = MasterNum;
                pFundMin.Value = FundMin;
                pRefMin.Value = RefMin;
                pResidualMin.Value = ResidualMin;
                pAffiliateID.Value = AffiliateID;

                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function InsertRepInfo
       
        //This function Inserts repinfomonthly table
        public bool InsertRepInfoMonthly(string RepName, string CurrMon, string RepSplit, string RepCat,
            string Comm, string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                SqlCommand cmdRepInfo = new SqlCommand("SP_InsertRepInfoMonthly", Conn);
                cmdRepInfo.CommandType = CommandType.StoredProcedure;
                SqlParameter pRepName = cmdRepInfo.Parameters.Add("@RepName", SqlDbType.VarChar);
                SqlParameter pCurrMon = cmdRepInfo.Parameters.Add("@Mon", SqlDbType.VarChar);
                SqlParameter pRepSplit = cmdRepInfo.Parameters.Add("@RepSplit", SqlDbType.VarChar);
                SqlParameter pRepCat = cmdRepInfo.Parameters.Add("@RepCat", SqlDbType.VarChar);
                SqlParameter pComm = cmdRepInfo.Parameters.Add("@Comm", SqlDbType.VarChar);
                SqlParameter pMasterNum = cmdRepInfo.Parameters.Add("@MasterNum", SqlDbType.VarChar);

                pRepName.Value = RepName;
                pCurrMon.Value = CurrMon;
                pRepSplit.Value = RepSplit;
                pRepCat.Value = RepCat;
                pComm.Value = Comm;
                pMasterNum.Value = MasterNum;

                cmdRepInfo.Connection.Open();
                cmdRepInfo.ExecuteNonQuery();
                Conn.Close();
                Conn.Dispose();
                return true;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function InsertRepInfoMonthly

        //This function returns the Master Rep Number based on repname. CALLED BY ExportACTBL.ExportData
        public string ReturnOfficeMasterNum(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select OfficeMasterNum From RepInfo WHERE MasterNum=@MasterNum";

                SqlCommand cmdRep = new SqlCommand(strQuery, Conn);
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");
                string returnStr = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    returnStr = dr["OfficeMasterNum"].ToString().Trim();
                }
                return returnStr;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMasterNum

        public DataSet ReturnOfficeAgentMasterNum(string OfficeMasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select MasterNum, RepName From RepInfo WHERE OfficeMasterNum=@OfficeMasterNum";

                SqlCommand cmdRep = new SqlCommand(strQuery, Conn);
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@OfficeMasterNum", OfficeMasterNum));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");
                //string returnStr = "";
                /*if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    returnStr = dr["OfficeMasterNum"].ToString().Trim();
                }*/
                return ds;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMasterNum

        //This function returns the Master Rep Number based on repname. CALLED BY ExportACTBL.ExportData
        public string ReturnMasterNum(string RepName)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select MasterNum From RepInfo WHERE RepName=@RepName";
           
                SqlCommand cmdRep = new SqlCommand(strQuery, Conn);
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@RepName", RepName));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfo");
                string returnStr = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    returnStr = dr["MasterNum"].ToString().Trim();
                }
                return returnStr;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMasterNum

        //This function returns the iPayment Sales ID based on repname. CALLED BY ExportACTBL.ExportData
        public string ReturniPaySalesID(string iPay3Num)
        {
            SqlConnection Conn = new SqlConnection(ConnString);
            try
            {
                string strQuery = "Select SalesID From RepInfoIPay3 WHERE iPay3Num = @iPay3Num";

                SqlCommand cmdRep = new SqlCommand(strQuery, Conn);
                cmdRep.Connection.Open();
                cmdRep.Parameters.Add(new SqlParameter("@iPay3Num", iPay3Num));
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmdRep;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "RepInfoIPay3");
                string returnStr = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    returnStr = dr["SalesID"].ToString().Trim();
                }
                return returnStr;
            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnMasterNum

        //This function returns the Max Commission Pct that can be set on a  Rep LED BY RepInfoBL.ReturnCommPct
        public int ReturnMaxCommPct(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdComm = new SqlCommand("SP_ReturnMaxCommPct", Conn);
                cmdComm.CommandType = CommandType.StoredProcedure;
                cmdComm.Parameters.Add(new SqlParameter("@MasterNum", MasterNum));

                SqlParameter pMaxCommPct = new SqlParameter("@MaxCommPct", SqlDbType.SmallInt);
                cmdComm.Parameters.Add(pMaxCommPct);
                pMaxCommPct.Direction = ParameterDirection.ReturnValue;

                cmdComm.Connection.Open();

                cmdComm.ExecuteNonQuery();

                int MaxCommPct = Convert.ToInt32(pMaxCommPct.Value);
                
                Conn.Close();
                Conn.Dispose();
                return MaxCommPct;

            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnCommPct

        //This function returns the Max Commission Pct that can be set on a Rep LED BY RepInfoBL.ReturnRepSplit
        public int ReturnMaxRepSplit(string MasterNum)
        {
            SqlConnection Conn = new SqlConnection(ConnStringPartner);
            try
            {
                SqlCommand cmdRepSplit = new SqlCommand("SP_ReturnMaxRepSplit", Conn);
                cmdRepSplit.CommandType = CommandType.StoredProcedure;
                SqlParameter pMasterNum = new SqlParameter("@MasterNum", SqlDbType.VarChar, 16);
                cmdRepSplit.Parameters.Add(pMasterNum);
                pMasterNum.Value = MasterNum;
                SqlParameter pMaxRepSplit = new SqlParameter("@MaxRepSplit", SqlDbType.SmallInt);
                cmdRepSplit.Parameters.Add(pMaxRepSplit);
                pMaxRepSplit.Direction = ParameterDirection.ReturnValue;

                cmdRepSplit.Connection.Open();

                cmdRepSplit.ExecuteNonQuery();

                int MaxRepSplit = Convert.ToInt32(pMaxRepSplit.Value);
                Conn.Close();
                Conn.Dispose();
                return MaxRepSplit;

            }//end try
            catch (SqlException err)
            {
                throw err;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }//end function ReturnRepSplit


        

  }//end class RepInfoDL
}
