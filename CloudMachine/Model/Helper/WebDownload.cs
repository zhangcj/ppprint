using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CloudMachine.Model.Helper
{
    public class WebDownload : WebClient
    {

        /// <summary>
        /// 接口超时时间（毫秒）
        /// </summary>
        private static int timeOutMs = 5* 1000;

        /// <summary>
        ///重写 GetWebRequest方法 添加超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest result = base.GetWebRequest(address);
            result.Timeout = timeOutMs;
            return result;
        }
    }
}
