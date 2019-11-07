using Force.Common.AES;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Force.Common.Http
{

    public static class HttpHelper
    {
        private static readonly byte[] _secret_key = Convert.FromBase64String("kyD0GK7X2KEUimQ2BPVS3iqFREJUdG5mykwh5A4nT3A=");
        private static readonly byte[] _auth_key = Convert.FromBase64String("nr8hxFqfLkQek1mmp07L7/8s/AAsfIX5Wg0DnH/v2D0=");
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse HttpGet(string url, int timeout = 0, string userAgent = "", CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout > 0)
            {
                request.Timeout = timeout;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse HttpPost(string url, IDictionary<string, string> parameters = null, int timeout = 0, string userAgent = "", Encoding requestEncoding = null, CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }
            if (timeout > 0)
            {
                request.Timeout = timeout;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static HttpWebResponse HttpPost(string url, object parameter = null, int timeout = 0, string userAgent = "", Encoding requestEncoding = null, CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "text/plain";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }
            if (timeout > 0)
            {
                request.Timeout = timeout;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            //如果需要POST数据  
            if (parameter != null)
            {
                var json = JsonConvert.SerializeObject(parameter);
                var encrypt = EncryptStr(json);
                var base64 = Convert.ToBase64String(encrypt);
                var bytes = requestEncoding.GetBytes(base64);
                request.ContentLength = Encoding.UTF8.GetByteCount(base64);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// Http Post请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr, bool isEncrypt = false)
        {
            string result = string.Empty;
            HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(Url);
            wbRequest.Method = "POST";
            wbRequest.ContentType = "text/plain";
            byte[] bytes = null;
            if (isEncrypt)
            {
                var encrypt = EncryptStr(postDataStr);
                var base64 = Convert.ToBase64String(encrypt);
                bytes = Encoding.UTF8.GetBytes(base64);
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(base64);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(postDataStr);
                wbRequest.Headers.Add("X-QQ:1");
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            }
            using (Stream requestStream = wbRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
            using (Stream responseStream = wbResponse.GetResponseStream())
            {
                using (StreamReader sread = new StreamReader(responseStream))
                {
                    result = sread.ReadToEnd();
                }
            }

            return result;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        public static long ToLong(this IPAddress ip)
        {
            return BitConverter.ToInt32(ip.GetAddressBytes(), 0);
        }

        #region 加解密
        private static byte[] EncryptStr(string message)
        {
            var msg_byte = Encoding.UTF8.GetBytes(message);

            return AESThenHMAC.SimpleEncrypt(msg_byte, _secret_key, _auth_key);
        }
        #endregion
    }
}
