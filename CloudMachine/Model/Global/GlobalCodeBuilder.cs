using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMachine.Model.Global
{
    /// <summary>
    /// 系统编码生成器
    /// </summary>
    public class GlobalCodeBuilder
    {
        static string _tempMachineCode = CloudMachine.Global.MachineCode;

        /// <summary>
        /// 机器码生成
        /// </summary>
        /// <returns></returns>
        public static string MachineCodeBuilder {
            get {return string.Format("{0}{1}", "云机编号：", _tempMachineCode); }
        }

        /// <summary>
        /// 机器码
        /// </summary>
        public static string TempMachineCode{
            get { return _tempMachineCode; }
        }

        //操作步
        public static int ProcessNum = 1;
        public static string filePaht = "";

        //状态报告时间间隔 秒
        public static int StateReportTimeSpan = int.Parse(System.Configuration.ConfigurationManager.AppSettings["StateReportTimeSpan"]);
    }
}
