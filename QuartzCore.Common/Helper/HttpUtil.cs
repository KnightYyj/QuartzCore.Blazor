using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Common.Helper
{
    public class HttpUtil
    {
        /// <summary>
        /// 静态的HttpClient
        /// </summary>
        private static readonly HttpClient httpClient = new HttpClient();

        #region HttpClient
        /// <summary>
        /// get（不要每次都new一个httpclient）
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <returns></returns>
        public static async Task<string> GetAsyncWithHttpClient(string remoteUrl)
        {
            var result = new StringBuilder();
            var response = await httpClient.GetStringAsync(remoteUrl);
            if (response == null)
            {
                return string.Empty;
            }
            else
            {
                return response;
            }
        }
        public static async Task<string> PostRequestWithHttpClient(string remoteUrl, string postData)
        {
            string responseData = "";

            using (var response = await httpClient.PostAsync(new Uri(remoteUrl), new StringContent(postData, Encoding.UTF8, "application / json")))
            {
                if (response != null)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }

            return responseData;
        }

        /// <summary>
        /// 发起POST async请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>        
        /// <returns></returns>
        public async Task<T> PostRequestWithHttpClient<T>(string url, string postData = null, string contentType = null, int timeOut = 30, Dictionary<string, string> headers = null)
        {
            postData = postData ?? "";
            using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
            {
                if (contentType != null)
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                if (headers != null)
                {
                    foreach (var header in headers)
                        httpContent.Headers.Add(header.Key, header.Value);
                }
                using (HttpResponseMessage response = await httpClient.PostAsync(url, httpContent))
                {
                    try
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return  JsonConvert.DeserializeObject<T>(result);
                    }
                    catch (Exception ex)
                    {
                        return default;
                    }
                }
            }
        }

        #endregion

        public static Task<string> HttpGetAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 30000;
                if (headers != null)
                {
                    foreach (var header in headers)
                        request.Headers[header.Key] = header.Value;
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    return streamReader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }

        public static Task<string> HttpPostAsync(string url, string postData = null, string contentType = "application/json", int timeOut = 30000, Dictionary<string, string> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST"; request.Timeout = timeOut;
            if (!string.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }
            if (headers != null)
            {
                foreach (var header in headers)
                    request.Headers[header.Key] = header.Value;
            }

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData ?? "");
                using (Stream sendStream = request.GetRequestStream())
                {
                    sendStream.Write(bytes, 0, bytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    return streamReader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }

        }

    }
}
