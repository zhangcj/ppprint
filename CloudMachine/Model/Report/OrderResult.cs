using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMachine.Model.Report
{
    //打印订单返回
    public class OrderResult{
        public List<OrderInfo> data { get; set; }    //文档订单返回值
    }

    //打印订单信息
    public class OrderInfo{
        public string id { get; set; }          //订单id
        public string order_id { get; set; }    //订单码
        public int uid { get; set; }            //用户id
        public int type { get; set; }           //1，图片打印，2文档打印
        public int photo_type { get; set; }     //照片尺寸 5=5寸，6=6寸
        public int print_num { get; set; }      //打印份数
        public int print_back { get; set; }
        public string img { get; set; }         //图片路径
        public string order_price { get; set; } //订单价格
        public int paid { get; set; }           //是否支付，1支付，0未支付
        public string pay_time { get; set; }    //支付时间
        public string file_url { get; set; }    //文档地址

    }
}
