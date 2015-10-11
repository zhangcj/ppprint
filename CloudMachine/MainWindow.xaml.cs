using System;
using System.Windows;
using System.Windows.Input;
using CloudMachine.Config;
using CloudMachine.Model.Global;

namespace CloudMachine
{
    using CloudMachine.Model.Extend;
    using System.Runtime.InteropServices;
    using System.Windows.Threading;
    using DC = DreamPlatform.CommonLibrary;
    using CloudMachine.Service;
    using CloudMachine.Model.Events;

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer ShowTimer;
        private DispatcherTimer StateReportTimer;//状态报告
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        public MainWindow()
        {
            InitializeComponent();
            //加载初始页
            InitFrame();

            ShowTimer = new DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);//起个Timer一直获取当前时间
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            //状态报告
            StateReportTimer = new DispatcherTimer();
            StateReportTimer.Tick += new EventHandler(StateClickTimer);
            StateReportTimer.Interval = new TimeSpan(0, 0, 0, 0, GlobalCodeBuilder.StateReportTimeSpan);
            StateReportTimer.Start();
            StateReportTimer.IsEnabled = true;

            this.txtMachineCode.Text = GlobalCodeBuilder.MachineCodeBuilder;

            #region 事件声明
            //打印
            this.imgPrint.MouseDown += imgPrint_MouseDown;
            //扫描
            this.imgScan.MouseDown += imgScan_MouseDown;
            #endregion
        }

        //状态报告
        private void StateClickTimer(object sender, EventArgs e)
        {
            //状态报告，err 为空
            HttpAPIService.LiveReportAPI("");
        }

        //加载初始页
        private void InitFrame()
        {
            #region 窗体素材加载
            //窗体背景图
            this.imgBackground.Source = ButtonExtend.InitButtonWithNormalImg("Background");

            //imgAd 广告图
            this.AdImg.Source = ButtonExtend.InitButtonWithNormalImg("AdImg");

            //MainBottomImage 底部图片组背景
            this.MainBottomImage.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomBtn");
            //打印
            this.imgPrint.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomLeft");
            //冲洗
            this.imgScan.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomRight");
            this.MainBottomTopImage.Source = ButtonExtend.InitButtonWithNormalImg("hp-logo");

            //二维码
            this.imgQrFram.Source = ButtonExtend.InitButtonWithNormalImg("imgQrFram");
            this.imgQr.Source = ButtonExtend.InitButtonWithNormalImg("QRcode");

            #endregion
        }

        public void ShowCurTimer(object sender, EventArgs e)
        {
            //获得年月日
            this.tbTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");   //yyyy年MM月dd日 HH:mm:ss
        }

        private void imgPrint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //打印
            this.imgPrint.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomLeft-press");
            //冲洗
            this.imgScan.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomRight");

            ShowNewWind();
        }

        /// <summary>
        /// 打开拍照界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgScan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //冲洗
            this.imgScan.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomRight-press");
            //打印
            this.imgPrint.Source = ButtonExtend.InitButtonWithNormalImg("MainBottomLeft");

            ShowNewWind();
        }

        //打开新窗体
        private void ShowNewWind()
        {
            GlobalCodeBuilder.ProcessNum += 1;
            var neWindow = new ScanWindow(new DocumentPrintFlowElement() { BtnNumPick = 1 });
            neWindow.Show();
            this.Close();
        }
    }
}
