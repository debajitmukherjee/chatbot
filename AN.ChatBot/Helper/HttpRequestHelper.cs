using AN.SC.Commons.Constant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace AN.ChatBot.Helper
{

    public class HttpRequestHelper
    {
        public WebRequest request;
        public Stream streamdataStream;

        /// <summary>
        /// The request status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="url"></param>
        public HttpRequestHelper(string url)
        {
            // Create a request using a URL that can receive a post.
            request = WebRequest.Create(url);
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        public HttpRequestHelper(string url, string method)
            : this(url)
        {

            if (method.Equals(Constants.GET) || method.Equals(Constants.POST) || method.Equals(Constants.PUT) || method.Equals(Constants.DEL))
            {
                // Set the Method property of the request to POST.
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        public HttpRequestHelper(string url, string method, string data, IDictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded", bool compress = false)
            : this(url, method)
        {

            // Set the ContentType property of the WebRequest.
            request.ContentType = contentType;
            ((HttpWebRequest)request).Proxy = null;

            if (compress)
            {
                ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            if (headers != null)
            {
                foreach (var item in headers)
                    request.Headers.Add(item.Key, item.Value);
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                // Create POST data and convert it to a byte array.
                var postData = data;
                var byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;

                var stream = request.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Flush();
                stream.Close();

            }

        }

        /// <summary>
        /// Get response
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {
            var responseFromServer = string.Empty;

            //Enable TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
           

            // Get the original response.
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                Status = response.StatusDescription;
                using (BufferedStream buffer = new BufferedStream(response.GetResponseStream()))
                {
                    using (StreamReader reader = new StreamReader(buffer))
                    {
                        responseFromServer = reader.ReadToEnd();
                    }
                }
                response.Close();
            }

            return responseFromServer;
        }

        //public HttpRequestHelper(string url, IDictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        //        : this(url, "GET")
        //{

        //    // Set the ContentType property of the WebRequest.
        //    request.ContentType = contentType;
        //    if (headers != null)
        //    {
        //        foreach (var item in headers)
        //            request.Headers.Add(item.Key, item.Value);
        //    }
        //}




    }
}