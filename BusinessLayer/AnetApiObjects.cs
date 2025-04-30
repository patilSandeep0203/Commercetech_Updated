/*
 * D I S C L A I M E R          
 * 
 * WARNING: ANY USE BY YOU OF THE SAMPLE CODE PROVIDED IS AT YOUR OWN RISK.
 * 
 * Authorize.Net provides this code "as is" without warranty of any kind, either
 * express or implied, including but not limited to the implied warranties of 
 * merchantability and/or fitness for a particular purpose.   
 * Authorize.Net owns and retains all right, title and interest in and to the 
 * Authorize.Net API intellectual property.
 */

using System;
using System.Collections.Generic;
using System.Text;
using AnetMerchantBoarding;
using AnetMerchantBoarding.ANetAPI;

namespace AnetMerchantBoarding
{
    class ServiceID
    {
        public const int GATEWAY = 8;
        public const int ECHECK = 4;
        public const int ARB = 7;
        public const int FDS = 1;
        public const int CIM = 17;
    }

    class FeeID
    {
        public const int GATEWAY_MONTHLY = 11;
        public const int GATEWAY_PER_TRANSACTION = 19;
    }

    public class ApiObjects
    {
        /// <summary>
        /// GetResellerServicesRequestObj
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// A populated getResellerServicesRequest object.
        /// </returns>
        public static getResellerServicesRequest GetResellerServicesRequestObj(InputParams inputParams)
        {
            getResellerServicesRequest request = new getResellerServicesRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);
            return request;
        }

        /// <summary>
        /// GetResellerProcessorsRequestObj
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// A populated getResellerProcessorsRequest object.
        /// </returns>
        public static getResellerProcessorsRequest GetResellerProcessorsRequestObj(InputParams inputParams)
        {
            getResellerProcessorsRequest request = new getResellerProcessorsRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);
            request.marketTypeId = inputParams.marketTypeId;

            return request;
        }

        /// <summary>
        /// GetServiceBuyRateProgramsRequestObj
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// A populated getServiceBuyRateProgramsRequest object.
        /// </returns>
        /*public static getServiceBuyRateProgramsRequest GetServiceBuyRateProgramsRequestObj(InputParams inputParams)
        {
            getServiceBuyRateProgramsRequest request = new getServiceBuyRateProgramsRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);
            request.serviceTypeId = inputParams.serviceTypeId;
            request.marketTypeId = inputParams.marketTypeId;
            return request;
        }*/

        public static getServiceBuyRateProgramsRequest GetServiceBuyRateProgramsRequestObj(InputParams inputParams)
        {
            getServiceBuyRateProgramsRequest request = new getServiceBuyRateProgramsRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);
            request.serviceTypeId = inputParams.serviceTypeId;
            request.marketTypeId = inputParams.marketTypeId;
            request.marketTypeIdSpecified = true;
            return request;
        }


        /// <summary>
        /// ResellerCreateMerchantRequestObj
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// A populated resellerCreateMerchantRequest object.
        /// </returns>
        public static resellerCreateMerchantRequest ResellerCreateMerchantRequestObj(InputParams inputParams)
        {
            List<serviceType> services = new List<serviceType>();
            foreach (int serviceId in new int[] { ServiceID.GATEWAY, ServiceID.ECHECK, ServiceID.ARB, ServiceID.FDS, ServiceID.CIM })
            {
                serviceType svc = new serviceType();
                svc.id = serviceId;
                List<serviceBuyRateProgramType> buyratePrograms = GetServiceBuyRatePrograms(inputParams, serviceId, false);
                if (buyratePrograms.Count > 0)
                {
                    serviceBuyRateProgramsType brps = new serviceBuyRateProgramsType();
                    brps.serviceBuyRateProgram = buyratePrograms.ToArray();
                    svc.Item = brps;
                }
                else
                {
                    svc.Item = true; // optOut = true.
                }
                services.Add(svc);
            }

            // For this example I'm going to create a merchant that will use the first
            // processor that the reseller is configured for.
            processorType processor = GetResellerProcessor(inputParams);
            if (processor == null) return null;         // indicate error

            foreach (fieldConfigType fc in processor.procConfig)
            {
                fc.fieldValue = "";
                if (fc.minLength > 0)
                {
                    fc.fieldValue = fc.fieldValue.PadRight(fc.minLength, '0');
                }
            }


            resellerCreateMerchantRequest request = new resellerCreateMerchantRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);

            merchantType merchant = new merchantType();

            merchant.name = "Test " + DateTime.Now.ToLongTimeString();
            merchant.email = "testmerchant@testsite.com";
            merchant.phone = "(425) 555-1212";
            merchant.fax = "(425) 555-1212";

            billingInfoType billingInfo = new billingInfoType();
            billingInfo.bankABACode = "125000024";
            billingInfo.bankAccountNumber = "1111222233334444";
            billingInfo.nameOnBankAccount = "Test Merchant";
            billingInfo.bankAccountOwnerType = bankAccountOwnerTypeEnum.Business;
            billingInfo.bankAccountType = resellerBankAccountTypeEnum.Checking;
            billingInfo.bankCity = "Bellevue";
            billingInfo.bankName = "Bank of America";
            billingInfo.bankState = "WA";
            billingInfo.bankZip = "98004";
            merchant.billingInfo = billingInfo;

            addressType businessAddress = new addressType();
            businessAddress.streetAddress = "1 Main Street";
            businessAddress.city = "Bellevue";
            businessAddress.state = "WA";
            businessAddress.zip = "98004";
            businessAddress.country = "US";
            merchant.businessAddress = businessAddress;

            ownerInfoType ownerInfo = new ownerInfoType();
            ownerInfo.name = "Company Owner";
            ownerInfo.phone = "(425) 555-1212";
            ownerInfo.ssn = "111223333";
            ownerInfo.title = "CEO";
            addressType ownerAddress = new addressType();
            ownerAddress.streetAddress = "200 Maple Grove";
            ownerAddress.city = "Bellevue";
            ownerAddress.state = "WA";
            ownerAddress.zip = "98004";
            ownerAddress.country = "US";
            ownerInfo.address = ownerAddress;
            merchant.ownerInfo = ownerInfo;

            businessInfoType businessInfo = new businessInfoType();
            businessInfo.ageOfBusiness = 3;
            businessInfo.businessType = businessTypeEnum.SoleProprietorShip;
            businessInfo.marketTypeId = inputParams.marketTypeId;
            businessInfo.productsSold = "Various widgets";
            businessInfo.sicCode = "2741";
            businessInfo.taxId = "111223333";
            merchant.businessInfo = businessInfo;

            merchant.services = services.ToArray();

            paymentGroupingType paygroup = new paymentGroupingType();
            paygroup.paymentTypes = new string[] { "V", "M", "A", "D", "C", "J" };
            processor.acquirerId = 44;
            processor.acquirerIdSpecified = true;
            paygroup.processor = processor;
            merchant.paymentGrouping = paygroup;

            salesRepType salesRep = new salesRepType();
            salesRep.salesRepCommission = 5.6M;
            salesRep.salesRepId = "121";
            salesRep.salesRepName = "Joe Smith";
            merchant.salesRep = salesRep;

            request.merchant = merchant;

            return request;
        }

        /// <summary>
        /// This is a support function for ResellerCreateMerchantRequestObj.
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        private static List<serviceBuyRateProgramType> GetServiceBuyRatePrograms(InputParams inputParams, int serviceTypeId, bool selfProvisioning)
        {
            List<serviceBuyRateProgramType> brps = new List<serviceBuyRateProgramType>();

            getServiceBuyRateProgramsRequest request = new getServiceBuyRateProgramsRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);
            request.serviceTypeId = serviceTypeId;

            request.marketTypeId = inputParams.marketTypeId;
            request.marketTypeIdSpecified = true;


            ANetApiResponse apiResponse = (ANetApiResponse)Utils.CallAPIServer(inputParams, request);
            if (apiResponse != null && apiResponse.messages.resultCode == messageTypeEnum.Ok)
            {
                getServiceBuyRateProgramsResponse response = (getServiceBuyRateProgramsResponse)apiResponse;

                // For this example we will just choose a buyrateProgram arbitrarily but we will use sorting 
                // to favor ones that are or aren't self provisioning (based on input parameter) and we will 
                // favor ones that are marked as the default.
                Array.Sort(response.serviceBuyRatePrograms,
                    delegate(serviceBuyRateProgramType brp1, serviceBuyRateProgramType brp2)
                    {
                        int score1 = ((brp1.isSelfProvisioning == selfProvisioning) ? 10 : 20)
                            + (brp1.isDefault ? 1 : 2);
                        int score2 = ((brp2.isSelfProvisioning == selfProvisioning) ? 10 : 20)
                            + (brp2.isDefault ? 1 : 2);
                        return score1 - score2;
                    }
                );

                // For echeck we need a standard and a preferred buyrateProgram.
                int desiredLength = (ServiceID.ECHECK == serviceTypeId) ? 2 : 1;

                foreach (serviceBuyRateProgramType brp in response.serviceBuyRatePrograms)
                {
                    if (brps.Count >= desiredLength) break;
                    if (ServiceID.GATEWAY == serviceTypeId)
                    {
                        foreach (feeType fee in brp.fees)
                        {
                            switch (fee.id)
                            {
                                case FeeID.GATEWAY_MONTHLY:
                                    fee.tiers[0].sellRate = 20.00M;
                                    break;
                                case FeeID.GATEWAY_PER_TRANSACTION:
                                    fee.tiers[0].sellRate = 0.10M;
                                    break;
                            }
                        }
                    }
                    brps.Add(brp);
                }
            }

            return brps;
        }

        /// <summary>
        /// This is a support function for ResellerCreateMerchantRequestObj.
        /// Get the first getResellerProcessor returned in the call to the API server.
        /// Dummy up the processor config data if values are not null.
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// A processorType with dummy data.
        /// </returns>
        private static processorType GetResellerProcessor(InputParams inputParams)
        {
            processorType processor = null;

            getResellerProcessorsRequest request = new getResellerProcessorsRequest();
            request.resellerAuthentication = CreateResellerAuthentication(inputParams);
            request.marketTypeId = inputParams.marketTypeId;

            ANetApiResponse apiResponse = (ANetApiResponse)Utils.CallAPIServer(inputParams, request);
            if (apiResponse != null && apiResponse.messages.resultCode == messageTypeEnum.Ok)
            {
                getResellerProcessorsResponse response = (getResellerProcessorsResponse)apiResponse;
                processor = response.processors[0];
                string lit = "123456789012345678901234567890";
                foreach (fieldConfigType config in processor.procConfig)
                {
                    if (config.fieldValue == null) config.fieldValue = lit.Substring(0, config.minLength);
                    // hide these in outbound request
                    config.minLengthSpecified = false;
                    config.maxLengthSpecified = false;
                    config.displayLabel = null;
                }
            }

            return processor;
        }

        /// <summary>
        /// This is a support function for API calls.
        /// Create a reseller authentication object and populate with input params.
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// New resellerAuthenticationType
        /// </returns>
        private static resellerAuthenticationType CreateResellerAuthentication(InputParams inputParams)
        {
            resellerAuthenticationType auth = new resellerAuthenticationType();
            auth.name = inputParams.resellerLogin;
            auth.apiKey = inputParams.resellerApiKey;
            return auth;
        }
    }
}

