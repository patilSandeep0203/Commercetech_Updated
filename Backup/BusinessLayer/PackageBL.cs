using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DLPartner;

namespace BusinessLayer
{
    public class PackageBL
    {
       
        //This function Inserts Package Info. CALLED BY CreatePackage.aspx
        public bool InsertPackage(string PackagePrefix, string Processor, string PackageSuffix, string CardPresent,
            string RepNum, string CustServFee, string InternetStmt, string TransactionFee, string DiscRateQualPres, string DiscRateQualNP,
            string DiscRateMidQual, string DiscRateNonQual, string DiscRateQualDebit, string ChargebackFee,
            string RetrievalFee, string VoiceAuth, string BatchHeader, string AVS, string MonMin,
            string NBCTransFee, string AnnualFee, string WirelessAccessFee, string WirelessTransFee,
            string AppFee, string AppSetupFee, string Gateway, string GatewayTransFee, string GatewayMonFee,
            string GatewaySetupFee, string DebitMonFee, string DebitTransFee, 
            string CGDiscRate, string CGMonFee, string CGMonMin, string CGTransFee, 
            string GCMonFee, string GCTransFee, string EBTMonFee, 
            string EBTTransFee, string RollingReserve, bool bInterchange, bool bAssessments)
        {
            PackageDL InsertPack = new PackageDL();
            bool retVal = InsertPack.InsertPackageInfo(PackagePrefix, Processor, PackageSuffix, CardPresent,
            RepNum, CustServFee, InternetStmt, TransactionFee, DiscRateQualPres, DiscRateQualNP,
            DiscRateMidQual, DiscRateNonQual, DiscRateQualDebit, ChargebackFee,
            RetrievalFee, VoiceAuth, BatchHeader, AVS, MonMin,
            NBCTransFee, AnnualFee, WirelessAccessFee, WirelessTransFee,
            AppFee, AppSetupFee, Gateway, GatewayTransFee, GatewayMonFee,
            GatewaySetupFee, DebitMonFee, DebitTransFee,
            CGDiscRate, CGMonFee, CGMonMin, CGTransFee,
            GCMonFee, GCTransFee, EBTMonFee,
            EBTTransFee, RollingReserve, bInterchange, bAssessments);
            return retVal;
        }//end function InsertPackage

        //This function Updates Package Info
        public bool UpdatePackage(int PackageID, string PackagePrefix, string PackageSuffix,
            string CustServFee, string InternetStmt, string TransactionFee, string DiscRateQualPres, string DiscRateQualNP,
            string DiscRateMidQual, string DiscRateNonQual, string DiscRateQualDebit, string ChargebackFee,
            string RetrievalFee, string VoiceAuth, string BatchHeader, string AVS, string MonMin,
            string NBCTransFee, string AnnualFee, string WirelessAccessFee, string WirelessTransFee,
            string AppFee, string AppSetupFee, string Gateway, string GatewayTransFee, string GatewayMonFee,
            string GatewaySetupFee, string DebitMonFee, string DebitTransFee,
            string CGDiscRate, string CGMonFee, string CGMonMin, string CGTransFee,
            string GCMonFee, string GCTransFee, string EBTMonFee,
            string EBTTransFee)
        {
            PackageDL UpdatePack = new PackageDL();
            bool retVal = UpdatePack.UpdatePackageInfo(PackageID, PackagePrefix, PackageSuffix,
            CustServFee, InternetStmt, TransactionFee, DiscRateQualPres, DiscRateQualNP,
            DiscRateMidQual, DiscRateNonQual, DiscRateQualDebit, ChargebackFee,
            RetrievalFee, VoiceAuth, BatchHeader, AVS, MonMin,
            NBCTransFee, AnnualFee, WirelessAccessFee, WirelessTransFee,
            AppFee, AppSetupFee, Gateway, GatewayTransFee, GatewayMonFee,
            GatewaySetupFee, DebitMonFee, DebitTransFee,
            CGDiscRate, CGMonFee, CGMonMin, CGTransFee,
            GCMonFee, GCTransFee, EBTMonFee,
            EBTTransFee);
            return retVal;
        }//end function UpdatePackage

        public bool UpdateOtherProcessingPackage(int PackageID, bool bInterchange, bool bAssessments, string RollingReserve)
        {
            PackageDL UpdatePack = new PackageDL();
            bool bRetVal = UpdatePack.UpdateOtherProcessingPackage(PackageID, bInterchange, bAssessments, RollingReserve);
            return bRetVal;
        }//end function UpdateInterchange

        //This function deletes package
        public int DeletePackage(int PackageID)
        {
            PackageDL DelPackage = new PackageDL();
            return  DelPackage.DeletePackageInfo(PackageID);
        }//end function DeletePackage

        //This function gets all Packages
        public DataSet GetAllPackages()
        {
            PackageDL GetRatesInfo = new PackageDL();
            DataSet ds = GetRatesInfo.GetPackageList();
            return ds;
        }//end function GetPackages

        //This function returns Packages for specified Rep
        //CALLED BY SetRates.aspx, ModifyPackage.aspx
        public DataSet GetPackagesForRep(string MasterNum)
        {
            PackageDL GetRatesInfo = new PackageDL();
            DataSet ds = GetRatesInfo.GetPackageListForRep(MasterNum);
            return ds;
        }//end function GetPackageListForRepNum

        //This function returns CP or CNP package list
        public DataSet GetPackageList(string CardPresent)
        {
            PackageDL Pack = new PackageDL();
            DataSet ds = Pack.GetPackageList(CardPresent);
            return ds;
        }//end function GetPackageList

        //Gets the Affiliate IDs that have the Package set as a default
        public DataSet GetAffiliateIDs(int PID)
        {
            PackageDL Pack = new PackageDL();
            DataSet ds = Pack.GetAffiliateIDs(PID);
            return ds;
        }//end function 
        //Gets the Affiliate IDs that have the Package set as a default
        public DataSet GetAppIDs(int PID)
        {
            PackageDL Pack = new PackageDL();
            DataSet ds = Pack.GetAppIDs(PID);
            return ds;
        }//end function 



    }//end class PackageBL
}
