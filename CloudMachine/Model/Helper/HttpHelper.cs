using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CloudMachine.Model.Helper
{
    public class HttpHelper
    {
        /// <summary>
        /// 模拟Post
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="content">请求的内容</param>
        /// <param name="headers">header里加入的内容</param>
        /// <param name="encode">编码，如果传null，默认是的是UTF8编码</param>
        /// <returns>请求返回的信息</returns>
        public static string Post(string url, NameValueCollection content, NameValueCollection headers, Encoding encode)
        {
            string remoteInfo;
            Encoding encoding = encode ?? Encoding.UTF8;
            var webClientObj = new WebDownload() { Encoding = encoding };
            if (headers != null)
                webClientObj.Headers.Add(headers);
            try
            {
                byte[] byRemoteInfo = webClientObj.UploadValues(url, "POST", content ?? new NameValueCollection());
                remoteInfo = encoding.GetString(byRemoteInfo);
            }
            catch (Exception ex)
            {
                remoteInfo = ex.ToString();
            }
            return remoteInfo;
        }

        /// <summary>
        /// 模拟Get
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="content">请求的内容</param>
        /// <param name="headers">header里加入的内容</param>
        /// <param name="encode">编码，如果传null，默认是的是UTF8编码</param>
        /// <returns>请求返回的信息</returns>
        public static string Get(string url, NameValueCollection content, NameValueCollection headers, Encoding encode)
        {
            string remoteInfo = string.Empty;
            Encoding encoding = encode ?? Encoding.UTF8;
            var webClientObj = new WebDownload() { Encoding = encoding };
            webClientObj.Headers.Add(headers);

            try
            {
                webClientObj.QueryString.Add(content);
                return webClientObj.DownloadString(url);
            }
            catch (Exception ex)
            {
                remoteInfo = ex.ToString();
            }
            return remoteInfo;
        }

        /// <summary>
        /// 构造请求参数
        /// </summary>
        /// <param name="dictionaryParameter"></param>
        /// <returns></returns>
        public static NameValueCollection CreateParameter(SortedDictionary<string, string> dictionaryParameter)
        {
            var parameter = new NameValueCollection { };
            if (dictionaryParameter != null)
            {
                foreach (KeyValuePair<string, string> item in dictionaryParameter)
                {
                    if (!string.IsNullOrEmpty(item.Key) && item.Value != null)
                    {
                        parameter.Add("" + item.Key + "", item.Value);
                    }
                }
            }
            return parameter;
        }

    }
}
