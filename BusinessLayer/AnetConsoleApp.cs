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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AnetMerchantBoarding.ANetAPI;

namespace AnetMerchantBoarding
{
    class ConsoleApp
    {
        // constants used to set the application return code
        const int RC_OK = 0;
        const int RC_SHOW_HELP = 1;
        const int RC_EXEC_FAILED = 2;

        private static string methodName = "";
        private static bool confirmBeforeExecute = false;
        private static InputParams inputParams = new InputParams();

        /// <summary>
        /// ShowHelp
        /// Show the help screen
        /// </summary>
        static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName + " /L:login /K:ApiKey /M:MethodName [/F:Filename] [/MT:MarketType] [/ST:ServiceType] [/URL:apiServerUrl] [/XML] [/Pause]");
            Console.WriteLine();
            Console.WriteLine("  /M:         Execute the specified API method.");
            Console.WriteLine("  MethodName    getResellerServices");
            Console.WriteLine("                getResellerProcessors");
            Console.WriteLine("                getServiceBuyRatePrograms");
            Console.WriteLine("                resellerCreateMerchant");
            Console.WriteLine("                ALL - Execute each of the above API calls.");
            Console.WriteLine("  /F:         Use the specified XML file to initialize the object instead");
            Console.WriteLine("                of hard-coded internal data. If filename is specified then");
            Console.WriteLine("                MethodName will be ignored.");
            Console.WriteLine("  /L:         Reseller login name");
            Console.WriteLine("  /K:         Reseller API Key");
            Console.WriteLine("  /MT:        Market type used by getResellerServices");
            Console.WriteLine("  MarketType    0=Ecommerce (default if /MT not specified)");
            Console.WriteLine("                1=MOTO");
            Console.WriteLine("                2=Retail");
            Console.WriteLine("  /ST:        Service type used by getServiceBuyRatePrograms");
            Console.WriteLine("  ServiceType   1=Fraud Detection Suite");
            Console.WriteLine("                4=eCheck.Net Transaction Processing");
            Console.WriteLine("                6=Virtual point of sale (VPOS)");
            Console.WriteLine("                7=Automated Recurring Billing (ARB)");
            Console.WriteLine("                17=Customer Information Manager (CIM)");
            Console.WriteLine("                8=Payment Gateway Account (default if /ST not specified)");
            Console.WriteLine("  /Pause      Pause for confirmation before executing the API method.");
            Console.WriteLine("  /XML        Show request and response XML.");
            Console.WriteLine("  /URL:       Set the URL of the API server.");
            Console.WriteLine();
        }

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args"></param>
        static int Main(string[] args)
        {
            int returnCode = RC_OK;     // 0 is successful

            if (args != null && args.Length > 0)
            {
                for (int a = 0; a < args.Length; a++)
                {
                    // extract the param name from possible data
                    string cmd;
                    string data;
                    int x = args[a].IndexOf(':');
                    if (x != -1)
                    {
                        cmd = args[a].Substring(0, x).ToUpper();
                        data = args[a].Substring(x + 1);
                    }
                    else
                    {
                        cmd = args[a].ToUpper();
                        data = "";
                    }

                    // process the command
                    switch (cmd)
                    {
                        case "/M":
                            methodName = data.ToLower();
                            if (methodName == "getresellerservices"
                            || methodName == "getresellerprocessors"
                            || methodName == "getservicebuyrateprograms"
                            || methodName == "resellercreatemerchant"
                            || methodName == "all")
                            {
                                methodName = data;  // maintain original input for later display
                            }
                            else
                            {
                                Console.WriteLine("The MethodName specified for /M is missing or invalid.");
                                returnCode = RC_SHOW_HELP;
                            }
                            break;

                        case "/L":
                            inputParams.resellerLogin = data;
                            break;

                        case "/MT":
                            inputParams.marketTypeId = Convert.ToInt32(data);
                            break;

                        case "/ST":
                            inputParams.serviceTypeId = Convert.ToInt32(data);
                            break;

                        case "/K":
                            inputParams.resellerApiKey = data;
                            break;

                        case "/PAUSE":
                            confirmBeforeExecute = true;
                            break;

                        case "/URL":
                            inputParams.apiUrl = data;
                            break;

                        case "/XML":
                            inputParams.showXml = true;
                            break;

                        case "/F":
                            inputParams.filename = data;
                            break;

                        default:
                            Console.WriteLine("Parameter \"" + cmd + "\" is not valid");
                            returnCode = RC_SHOW_HELP;
                            break;

                    }

                    if (returnCode == RC_SHOW_HELP)
                    {
                        Console.WriteLine();
                        Console.WriteLine(String.Join(" ", args));
                        Console.WriteLine();
                        break;
                    }
                }
            }

            if (returnCode == RC_OK && !ValidateParams())
            {
                returnCode = RC_EXEC_FAILED;
            }
            else
            {
                ShowExecParams();
                if (confirmBeforeExecute)
                {
                    Console.WriteLine();
                    Console.Write("Do you want to continue (Y/N)? ");
                    string ans = Console.In.ReadLine();
                    Console.WriteLine();
                    if (ans.ToUpper().Trim() != "Y")
                    {
                        Console.WriteLine("Terminated.");
                        returnCode = RC_EXEC_FAILED;
                    }
                }

                if (returnCode == RC_OK)
                {
                    int result = 0;
                    if (methodName.ToLower() == "all")
                    {
                        string mn = methodName;
                        methodName = "getResellerServices";
                        result |= ExecuteMethod();
                        methodName = "getResellerProcessors";
                        result |= ExecuteMethod();
                        methodName = "getServiceBuyRatePrograms";
                        result |= ExecuteMethod();
                        methodName = "resellerCreateMerchant";
                        result |= ExecuteMethod();
                        methodName = mn;
                    }
                    else
                    {
                        result = ExecuteMethod();
                    }
                    if (result != 0) returnCode = RC_EXEC_FAILED;
                }
            }

            if (returnCode == RC_SHOW_HELP) ShowHelp();

            Console.WriteLine();
            Console.ReadLine();
            return returnCode;
        }

        /// <summary>
        /// dump params to console.
        /// </summary>
        private static void ShowExecParams()
        {
            Console.WriteLine();
            Console.WriteLine("URL...................." + inputParams.apiUrl);
            Console.WriteLine("API Method............." + methodName);
            Console.WriteLine("Reseller Login........." + inputParams.resellerLogin);
            Console.WriteLine("Reseller API Key......." + inputParams.resellerApiKey);
            Console.WriteLine("MarketTypeId..........." + inputParams.marketTypeId.ToString());
            Console.WriteLine("ServiceTypeId.........." + inputParams.serviceTypeId.ToString());
            Console.WriteLine("Show XML..............." + (inputParams.showXml ? "Yes" : "No"));
            Console.WriteLine("Filename..............." + inputParams.filename);
        }

        /// <summary>
        /// Execute the API method
        /// </summary>
        /// <returns></returns>
        private static int ExecuteMethod()
        {
            int resultCode = 0;
            ANetApiResponse apiResponse = null;

            Console.WriteLine("\r\nExecuting.");

            object request = null;

            switch (methodName.ToLower())
            {
                case "getresellerservices":
                    Console.WriteLine("Calling getResellerServices");
                    request = ApiObjects.GetResellerServicesRequestObj(inputParams);
                    break;

                case "getresellerprocessors":
                    Console.WriteLine("Calling getResellerProcessors");
                    request = ApiObjects.GetResellerProcessorsRequestObj(inputParams);
                    break;

                case "getservicebuyrateprograms":
                    Console.WriteLine("Calling getServiceBuyRatePrograms");
                    request = ApiObjects.GetServiceBuyRateProgramsRequestObj(inputParams);
                    break;

                case "resellercreatemerchant":
                    Console.WriteLine("Calling resellerCreateMerchant");
                    if (inputParams.filename == null || inputParams.filename.Length == 0)
                    {
                        request = ApiObjects.ResellerCreateMerchantRequestObj(inputParams);
                    }
                    else
                    {
                        try
                        {
                            string xml = File.ReadAllText(inputParams.filename);
                            XmlSerializer serializer = new XmlSerializer(typeof(resellerCreateMerchantRequest));
                            request = serializer.Deserialize(new StringReader(xml));
                            resellerAuthenticationType auth = ((resellerCreateMerchantRequest)request).resellerAuthentication;
                            if (auth.name.Length == 0) auth.name = inputParams.resellerLogin;
                            if (auth.apiKey.Length == 0) auth.apiKey = inputParams.resellerApiKey;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                    }
                    break;

                default:
                    resultCode = RC_SHOW_HELP;
                    break;
            }

            if (request == null)
            {
                resultCode = RC_EXEC_FAILED;
            }
            else
            {
                // All response types are based on ANetApiResponse so this is safe.
                apiResponse = (ANetApiResponse)Utils.CallAPIServer(inputParams, request);

                if (apiResponse == null || apiResponse.messages.resultCode != messageTypeEnum.Ok)
                {
                    resultCode = RC_EXEC_FAILED;
                }
            }
            return resultCode;
        }

        /// <summary>
        /// Validate input params
        /// </summary>
        /// <returns></returns>
        private static bool ValidateParams()
        {
            bool bValid = true;
            bool bNoFile = (inputParams.filename == null || inputParams.filename.Length == 0);

            if (methodName == "" && bNoFile)
            {
                Console.WriteLine("You must specify either an API Method (use /M) or a Filename (use /F).");
                bValid = false;
            }
            if (inputParams.apiUrl == "")
            {
                Console.WriteLine("You must specify URL of the API server (use /URL)");
                bValid = false;
            }
            if (inputParams.resellerLogin == "" && bNoFile)
            {
                Console.WriteLine("You must specify a reseller login (use /L)");
                bValid = false;
            }
            if (inputParams.resellerApiKey == "" && bNoFile)
            {
                Console.WriteLine("You must specify a reseller apiKey (use /P)");
                bValid = false;
            }

            return bValid;
        }

    }
}
