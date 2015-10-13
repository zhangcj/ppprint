using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using CloudMachine.Model.Helper;
using CloudMachine.Model.Global;
using CloudMachine.Model.Report;

namespace CloudMachine.Service
{
    /// <summary>
    /// api service
    /// </summary>
    public class HttpAPIService
    {
        /// <summary>
        /// 获取订单信息
        /// </summary>
        public static OrderResult GetOrderFromAPI(string orderCode)
        {
            OrderResult order = null;
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["GetOrderAPI"];

                var parameter = new SortedDictionary<string, string>
                {
                    {"mid", orderCode.Trim()}
                };
                string jsonResult = HttpHelper.Get(apiUrl, HttpHelper.CreateParameter(parameter),
                    new NameValueCollection(), Encoding.UTF8);
                order = JsonLib.JSONToObject<OrderResult>(jsonResult);

                if (order != null && order.data.Count>0 && !string.IsNullOrWhiteSpace(order.data[0].file_url))
                {
                    PrintResultAPI(order.data[0].order_id,"1");
                }
            }
            catch (Exception ex)
            {
                PrintResultAPI(order.data[0].order_id, "0");
            }
            return order;
        }

        /// <summary>
        /// 状态报告
        /// </summary>
        public static void LiveReportAPI(string err,string live="1")
        {
            string apiUrl = ConfigurationManager.AppSettings["LiveReportAPI"];

            var parameter = new SortedDictionary<string, string>
            {
                {"mid",GlobalCodeBuilder.TempMachineCode},
                {"live",live},
                {"error",err.Trim()}
            };
            string jsonResult = HttpHelper.Get(apiUrl, HttpHelper.CreateParameter(parameter), new NameValueCollection(), Encoding.UTF8);
        }

        /// <summary>
        /// 打印成功状态报告
        /// </summary>
        /// <param name="orderId"></param>
        public static void PrintResultAPI(string orderId,string result)
        {
            string apiUrl = ConfigurationManager.AppSettings["PrintResultAPI"];

            var parameter = new SortedDictionary<string, string>
            {
                {"mid",GlobalCodeBuilder.TempMachineCode},
                {"order_id",orderId.Trim()},
                {"result",result.Trim()}
            };
            string jsonResult = HttpHelper.Get(apiUrl, HttpHelper.CreateParameter(parameter), new NameValueCollection(), Encoding.UTF8);
        }

        /// <summary>
        /// 取件成功状态报告
        /// </summary>
        /// <param name="snCode"></param>
        public static void PickSuccessAPI(string snCode)
        {
            string apiUrl = ConfigurationManager.AppSettings["PickSuccessAPI"];

            var parameter = new SortedDictionary<string, string>
            {
                {"sn",snCode}
            };
            string jsonResult = HttpHelper.Get(apiUrl, HttpHelper.CreateParameter(parameter), new NameValueCollection(), Encoding.UTF8);
        }

        /// <summary>
        /// 是否取件成功状态报告
        /// </summary>
        /// <param name="snCode"></param>
        public static void IsPickSuccessAPI(string orderId)
        {
            string apiUrl = ConfigurationManager.AppSettings["IsPickSuccessAPI"];

            var parameter = new SortedDictionary<string, string>
            {
                {"order_id",orderId}
            };
            string jsonResult = HttpHelper.Get(apiUrl, HttpHelper.CreateParameter(parameter), new NameValueCollection(), Encoding.UTF8);
        }
    }
}
