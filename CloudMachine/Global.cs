using System;
namespace CloudMachine
{
    using DC = DreamPlatform.CommonLibrary;
    public class Global
    {

        #region 获取服务对象

        /// <summary>
        /// 二合一照片
        /// </summary>
        public static System.Drawing.Image PrintImage { get; set; }
        public static int PrintCount { get; set; }
        public static bool IsPrint { get; set; }
        public static bool IsSend { get; set; }
        #endregion

        #region Go To ErrorPage

        public static event EventHandler OnErrorEvent;
        public static void OnError()
        {
            if (OnErrorEvent != null)
                OnErrorEvent(null, null); ;
        }

        #endregion

        #region 自动返回主页

        public delegate void backMainWindowsHandle(int backtime);

        private static backMainWindowsHandle _backMainWindows = null;
        public static backMainWindowsHandle backMainWindows
        {
            get
            {
                return _backMainWindows;
            }
            set
            {
                _backMainWindows = value;
            }
        }




        #endregion


        #region 调用接口服务

        #endregion

        #region 机器配置

        static DC.INIFileHelper iniFileHelper = new DC.INIFileHelper(AppDomain.CurrentDomain.BaseDirectory + "Sys_Set.INF");
        public static string MachineCode
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "MachineCode");
            }
        }

        public static string UserName
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "UserName");
            }
        }

        public static string UserPWD
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "PWD");
            }
        }

        public static int GetBackTime
        {
            get
            {
                int backtime = 0;
                int.TryParse(iniFileHelper.IniReadValue("System", "BackTime"), out backtime);
                return backtime;
            }
        }

        public static string CameraName
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "CameraName");
            }
        }


        public static string A6PrinterName
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "A6PrinterName");
            }
        }

        public static string A4PrinterName
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "A4PrinterName");
            }
        }

        public static string WebServiceUrl
        {
            get
            {
                return iniFileHelper.IniReadValue("System", "WebServiceUrl");
            }
        }
        #endregion
    }
}
