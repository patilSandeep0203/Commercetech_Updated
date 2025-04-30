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
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AnetMerchantBoarding;
using AnetMerchantBoarding.ANetAPI;


namespace AnetMerchantBoarding
{
    public class InputParams
    {
        // These require a value.    
        /*public string apiUrl = "https://apitest.authorize.net/xml/v1/reseller.api";
        public string resellerLogin = "MBAPIdemo123";
        public string resellerApiKey = "3TT7G2N9et24seGP";*/

        //Reseller account info
        public string apiUrl = "https://api.authorize.net/xml/v1/reseller.api";
        public string resellerLogin = "comtech1";
        public string resellerApiKey = "5GmhZncNZ4743j2C";

        //public bool showXml = true;
        //public string filename = "";

        // This is the marketTypeId used in the getResellerProcessors API call
        //public int marketTypeId = 0;   // 0=ecommmerce, 1=moto, 2=retail

        // This is the serviceTypeId used in the getServiceBuyRatePrograms API call
        //public int serviceTypeId = ServiceID.GATEWAY;
    }

    public static class Utils
    {
        /// <summary>
        /// This is a support function for API calls.
        /// Create a reseller authentication object and populate with input params.
        /// </summary>
        /// <param name="inputParams"></param>
        /// <returns>
        /// New resellerAuthenticationType
        /// </returns>
        public static resellerAuthenticationType CreateResellerAuthentication(InputParams inputParams)
        {
            resellerAuthenticationType auth = new resellerAuthenticationType();
            auth.name = inputParams.resellerLogin;
            auth.apiKey = inputParams.resellerApiKey;
            return auth;
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Call the API server and return the response object;
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // ----------------------------------------------------------------------------------------
        public static object CallAPIServer(InputParams inputParams, object request)
        {
            object response = null;
            XmlDocument xmldocResponse;
            bool successful = Utils.PostRequest(inputParams, request, out xmldocResponse);
            if (successful)
            {
                // create a response object using the XML returned by the call to the API server
                successful = Utils.ProcessXmlResponse(xmldocResponse, out response);

                //if (successful) Utils.ProcessResponse(response);
            }
            return response;
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Send the request to the API server and load the response into an XML document.
        /// An XmlSerializer is used to form the XML used in the request to the API server. 
        /// The response from the server is also XML. An XmlReader is used to process the
        /// response stream from the API server so that it can be loaded into an XmlDocument.
        /// </summary>
        /// <param name="apiRequest"></param>
        /// <returns>
        /// True if successful, false if not. If true then the specified xmldoc will contain the
        /// response received from the API server.
        /// </returns>
        // ----------------------------------------------------------------------------------------
        public static bool PostRequest(InputParams inputParams, object apiRequest, out XmlDocument xmldocResponse)
        {
            bool bResult = false;
            XmlSerializer serializer = null;

            xmldocResponse = null;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(inputParams.apiUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "text/xml";
                webRequest.KeepAlive = true;

                // Serialize the request
                if (apiRequest.GetType() == typeof(String))
                {
                    String xmlRequest = (String)apiRequest;
                    Stream stream = webRequest.GetRequestStream();
                    byte[] byteArrayRequest = Encoding.ASCII.GetBytes(xmlRequest);
                    stream.Write(byteArrayRequest, 0, byteArrayRequest.Length);
                    stream.Close();
                }
                else
                {
                    serializer = new XmlSerializer(apiRequest.GetType());
                    XmlWriter writer = new XmlTextWriter(webRequest.GetRequestStream(), Encoding.UTF8);
                    serializer.Serialize(writer, apiRequest);
                    writer.Close();
                }

                // Get the response
                WebResponse webResponse = webRequest.GetResponse();

                // Load the response from the API server into an XmlDocument.
                xmldocResponse = new XmlDocument();
                xmldocResponse.Load(XmlReader.Create(webResponse.GetResponseStream()));

                bResult = true;
            }
            catch (Exception)
            {
                bResult = false;
            }

            return bResult;
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Deserialize the given XML document into the correct object type using the root
        /// node to determine the type of output object.
        /// 
        /// For any given API request the response can be one of two types:
        ///    ErorrResponse or [methodname]Response. 
        /// For example, the getResellerServicesRequest would normally result in a response of
        /// getResellerServicesResponse. This is also the name of the root node of the response.
        /// This name can be used to deserialize the response into local objects. 
        /// </summary>
        /// <param name="xmldoc">
        /// This is the XML document to process. It holds the response from the API server.
        /// </param>
        /// <param name="apiResponse">
        /// This will hold the deserialized object of the appropriate type.
        /// </param>
        /// <returns>
        /// True if successful, false if not.
        /// </returns>
        // ----------------------------------------------------------------------------------------
        public static bool ProcessXmlResponse(XmlDocument xmldoc, out object apiResponse)
        {
            bool bResult = true;
            XmlSerializer serializer;

            apiResponse = null;
            try
            {
                // Use the root node to determine the type of response object to create
                switch (xmldoc.DocumentElement.Name)
                {
                    case "getResellerServicesResponse":
                        serializer = new XmlSerializer(typeof(getResellerServicesResponse));
                        apiResponse = (getResellerServicesResponse)serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "getResellerProcessorsResponse":
                        serializer = new XmlSerializer(typeof(getResellerProcessorsResponse));
                        apiResponse = (getResellerProcessorsResponse)serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "getServiceBuyRateProgramsResponse":
                        serializer = new XmlSerializer(typeof(getServiceBuyRateProgramsResponse));
                        apiResponse = (getServiceBuyRateProgramsResponse)serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "resellerCreateMerchantResponse":
                        serializer = new XmlSerializer(typeof(resellerCreateMerchantResponse));
                        apiResponse = (resellerCreateMerchantResponse)serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    case "ErrorResponse":
                        serializer = new XmlSerializer(typeof(ANetApiResponse));
                        apiResponse = (ANetApiResponse)serializer.Deserialize(new StringReader(xmldoc.DocumentElement.OuterXml));
                        break;

                    default:
                        bResult = false;
                        break;
                }
            }
            catch (Exception)
            {
                bResult = false;
                apiResponse = null;
            }

            return bResult;
        }

        //
        
        // ----------------------------------------------------------------------------------------
        /// The response is generated directly on the XMLBL.cs
        /// <summary>
        /// Determine the type of the response object and process accordingly.
        /// Since this is just sample code the only processing being done here is to write a few
        /// bits of information to the console window.
        /// </summary>
        /// <param name="response"></param>
        // ----------------------------------------------------------------------------------------
        /*public static void ProcessResponse(object response)
        {
            // Every response is based on ANetApiResponse so you can always do this sort of type casting.
            ANetApiResponse baseResponse = (ANetApiResponse)response;

            // Write the results to the console window
            string result = "Result: " + baseResponse.messages.resultCode.ToString();
            //Console.Write("Result: ");
            //Console.WriteLine(baseResponse.messages.resultCode.ToString());

            // If the result code is "Ok" then the request was successfully processed.
            if (baseResponse.messages.resultCode != messageTypeEnum.Ok)
            {
                // Write error messages to console window
                for (int i = 0; i < baseResponse.messages.message.Length; i++)
                {
                    result = "[" + baseResponse.messages.message[i].code + "] " + baseResponse.messages.message[i].text;
                    //Console.WriteLine("[" + baseResponse.messages.message[i].code + "] " + baseResponse.messages.message[i].text);
                }
            }
            else
            {
                // resellerCreateMerchantResponse is the only API call in this example
                // that returns data other than messages.
                if (response.GetType() == typeof(resellerCreateMerchantResponse))
                {
                    result = "Merchant Id: " + ((resellerCreateMerchantResponse)response).merchantId.ToString();
                    //Console.WriteLine("Merchant Id: " + ((resellerCreateMerchantResponse)response).merchantId.ToString());
                }
            }
        }*/

    }
}
