using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMachine.Model.Report
{
    //打印状态
    public class PrintResult
    {
        public int mid { get; set; }         //机器唯一编码
        public int order_id { get; set; }    //订单id
    }

    public class PrintReturnVal
    {
        public bool returnV { get; set; }    //打印返回值
    }
}
