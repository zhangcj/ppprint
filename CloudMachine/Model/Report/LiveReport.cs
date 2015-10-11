using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMachine.Model.Report
{
    //心跳上传
    public class LiveReport
    {
        public string mid { get; set; }    //机器唯一编码
        public int live { get; set; }            //是否异常 1正常，0异常
        public string error { get; set; }        //异常信息，可空
    }

    //返回信息
    public class ReturnVal
    {
        public bool returnV { get; set; }    //返回状态 true/false
        public bool success { get; set; }    //请求状态 true/false
    }
}
