using System;
using System.Windows;
using System.Windows.Threading;
using CloudMachine.Config;
using CloudMachine.Model.Extend;
using CloudMachine.Model.Global;
using System.Runtime.InteropServices;

namespace CloudMachine
{
    using DC = DreamPlatform.CommonLibrary;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using CloudMachine.Model.Events;
    using System.Windows.Interop;
    using System.Text;
    using CloudMachine.Service;
    using CloudMachine.Model.Helper;
    using System.Web;
    /// <summary>
    /// Interaction logic for ScanWindow.xaml
    /// </summary>
    public partial class ScanWindow : Window
    {
        private DispatcherTimer ShowTimer;
        private DispatcherTimer qrCodeTimer;
        public DocumentPrintFlowElement _flowElement;

        public ScanWindow(DocumentPrintFlowElement flowElement)
        {
            InitializeComponent();
            // 在此点下面插入创建对象所需的代码.
            this._flowElement = flowElement;

            #region 窗体素材加载

            //窗体背景图
            this.imgBackground.Source = ButtonExtend.InitButtonWithNormalImg("Background");
            //通过消息得到解码信息
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            Dll_Camera.GetAppHandle(hwnd);

            //二维码扫描设备获取异常
            if (Dll_Camera.GetDevice() != 1)
            {
                HttpAPIService.LiveReportAPI(HttpUtility.UrlEncode("二维码扫描设备获取异常"),"0");
            }

            //初始化界面元素
            InitSysImg();
            #endregion
        }

        #region UI
        //初始化系统界面
        private void InitSysImg()
        {
            if (_flowElement != null)
            {
                //====================================================
                //操作按钮组
                InitOperBtnFrame();

                #region 事件处理
                this.txtNumShow.GotFocus += InputFocus;
                this.txtNumShow.LostFocus += LostFocus;
                #endregion

                //====================================================

                this.txtMachineCode.Text = GlobalCodeBuilder.MachineCodeBuilder;
                //二维码
                this.imgQrFram.Source = ButtonExtend.InitButtonWithNormalImg("imgQrFram");
                this.imgQr.Source = ButtonExtend.InitButtonWithNormalImg("QRcode");

                ShowTimer = new DispatcherTimer();
                ShowTimer.Tick += new EventHandler(ShowCurTimer); //起个Timer一直获取当前时间
                ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                ShowTimer.Start();


                qrCodeTimer = new DispatcherTimer();
                qrCodeTimer.Tick += new EventHandler(QrcodeTimer); //起个Timer一直验证是否扫码
                qrCodeTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                qrCodeTimer.Start();
                qrCodeTimer.IsEnabled = false;
                //====================================================
                ProcessInit();
            }
        }

        //底部流程
        private void ProcessInit()
        {
            //====================================================

            this.imgProcess.Source = ButtonExtend.InitButtonWithNormalImg("processFrame"); //流程框图

            this.imgProcess1.Source = ButtonExtend.InitButtonWithProcessImg("flow1-2", 1 <= GlobalCodeBuilder.ProcessNum, "flow1"); //关注公众号
            this.imgGoto1.Source = ButtonExtend.InitButtonWithProcessImg("singleFlag", 2 <= GlobalCodeBuilder.ProcessNum, "singleFlag-2"); //完成箭头

            this.imgProcess2.Source = ButtonExtend.InitButtonWithProcessImg("flow2-2", 2 <= GlobalCodeBuilder.ProcessNum, "flow2"); //选择取件方式
            this.imgGoto2.Source = ButtonExtend.InitButtonWithProcessImg("singleFlag", 3 <= GlobalCodeBuilder.ProcessNum, "singleFlag-2"); //完成箭头

            this.imgProcess3.Source = ButtonExtend.InitButtonWithProcessImg("flow3-2", 3 <= GlobalCodeBuilder.ProcessNum, "flow3"); //支付方式
            this.imgGoto3.Source = ButtonExtend.InitButtonWithProcessImg("singleFlag", 4 <= GlobalCodeBuilder.ProcessNum, "singleFlag-2"); //完成箭头

            this.imgProcess4.Source = ButtonExtend.InitButtonWithProcessImg("flow4-2", 4 <= GlobalCodeBuilder.ProcessNum, "flow4"); //预览
            this.imgGoto4.Source = ButtonExtend.InitButtonWithProcessImg("singleFlag", 5 <= GlobalCodeBuilder.ProcessNum, "singleFlag-2"); //完成箭头

            this.imgProcess5.Source = ButtonExtend.InitButtonWithProcessImg("flow5-2", 5 <= GlobalCodeBuilder.ProcessNum, "flow5"); //执行打印
            this.imgGoto5.Source = ButtonExtend.InitButtonWithProcessImg("singleFlag", 6 <= GlobalCodeBuilder.ProcessNum, "singleFlag-2"); //完成箭头

            this.imgProcess6.Source = ButtonExtend.InitButtonWithProcessImg("flow6-2", 6 <= GlobalCodeBuilder.ProcessNum, "flow6"); //打印完成
        }

        //操作按钮组
        private void InitOperBtnFrame()
        {
            this.txtNumShow.Text = SysStringConfig.NumInputPlaceholder;

            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalAndPressImg("numPick", _flowElement.BtnNumPick, "numPick_heilight");//验证码取件
            imgBtnChoice1.MouseDown += numPickClick;
            imgBtnChoice1.MouseUp += numPickClickUp;

            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalAndPressImg("qrcodePick", _flowElement.QrCodePick, "qrcodePick_heilight");//二维码取件
            imgBtnChoice2.MouseDown += qrCodePickClick;
            imgBtnChoice2.MouseUp += qrCodePickClickUp;

            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalAndPressImg("cardPick", _flowElement.CardPick, "cardPick_heilight");//一卡通
            imgBtnChoice3.MouseDown += cardPickClick;
            imgBtnChoice3.MouseUp += cardPickClickUp;

            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalAndPressImg("backHome", _flowElement.BtnBackHome, "backHome");//返回首页
            //返回主页 两个button
            this.btnBack.Click += btnBackClickDown;
            this.btnHome.Click += btnBackHomeClickDown;

            this.imgNumFrame.Source = ButtonExtend.InitButtonWithProcessImg("567Frame", _flowElement.Pick == PickType.QrCodePick, "23Frame");//23内框
            this.imgNumTxt.Source = ButtonExtend.InitButtonWithNormalImg("numInput");//验证码输入

            this.imgNum1.Source = ButtonExtend.InitButtonWithNormalImg("1");
            this.imgNum1.MouseUp += numClick;

            this.imgNum2.Source = ButtonExtend.InitButtonWithNormalImg("2");
            this.imgNum2.MouseUp += numClick;

            this.imgNum3.Source = ButtonExtend.InitButtonWithNormalImg("3");
            this.imgNum3.MouseUp += numClick;

            this.imgNum4.Source = ButtonExtend.InitButtonWithNormalImg("4");
            this.imgNum4.MouseUp += numClick;

            this.imgNum5.Source = ButtonExtend.InitButtonWithNormalImg("5");
            this.imgNum5.MouseUp += numClick;

            this.imgNum6.Source = ButtonExtend.InitButtonWithNormalImg("6");
            this.imgNum6.MouseUp += numClick;

            this.imgNum7.Source = ButtonExtend.InitButtonWithNormalImg("7");
            this.imgNum7.MouseUp += numClick;

            this.imgNum8.Source = ButtonExtend.InitButtonWithNormalImg("8");
            this.imgNum8.MouseUp += numClick;

            this.imgNum9.Source = ButtonExtend.InitButtonWithNormalImg("9");
            this.imgNum9.MouseUp += numClick;

            this.imgNum10.Source = ButtonExtend.InitButtonWithNormalImg("numClear");//清除文字
            this.imgNum10.MouseUp += numClick;

            this.imgNum11.Source = ButtonExtend.InitButtonWithNormalImg("0");
            this.imgNum11.MouseUp += numClick;

            this.imgNum12.Source = ButtonExtend.InitButtonWithNormalImg("numSure");//确认文字
            this.imgNum12.MouseUp += numClick;


            this.imgPhoneScan.Source = ButtonExtend.InitButtonWithNormalImg("qrCodeInstructions");//扫描打印
            if (_flowElement.QrCodePick != (int)PickType.QrCodePick)
            {
                imgPhoneScan.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region 事件

        #region 左侧按钮组

        #region 序列码
        //输入码取件
        private void numPickClick(object sender, EventArgs e)
        {
            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalImg("numPick_press");
            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalImg("qrcodePick");
            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalImg("cardPick");
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome");
            inputValue = "";//清空输入
            qrCodeTimer.IsEnabled = false;
            Dll_Camera.ReleaseDevice();
        }

        //输入码取件-弹起
        private void numPickClickUp(object sender, EventArgs e)
        {
            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalImg("numPick_heilight");

            this.numberFrame.Visibility = Visibility.Visible;
            this.imgPhoneScan.Visibility = Visibility.Collapsed;
            txtNumShow.Text = SysStringConfig.NumInputPlaceholder;
        }

        #endregion

        #region 二维码 扫码取件
        int flag = 0;

        //扫描取件
        private void qrCodePickClick(object sender, EventArgs e)
        {
            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalImg("numPick");
            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalImg("qrcodePick_press");
            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalImg("cardPick");
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome");
            inputValue = "";//清空输入
            this.numberFrame.Visibility = Visibility.Collapsed;
            this.imgPhoneScan.Visibility = Visibility.Visible;
        }

        //扫描取件 - 弹起
        private void qrCodePickClickUp(object sender, EventArgs e)
        {
            #region 设备启动
            flag = Dll_Camera.StartDevice();
            //二维码扫描设备启动异常
            if (flag != 1)
            {
                HttpAPIService.LiveReportAPI(HttpUtility.UrlEncode("二维码扫描设备启动异常"), "0");
            }
            else
            {
                //设置一维
                Dll_Camera.setBarcode(barEnable);
                //设置qr
                Dll_Camera.setQRable(qrEnable);
                //设置dm
                Dll_Camera.setDMable(dmEnable);
                //设置扫码成功蜂鸣
                Dll_Camera.SetBeepTime(100);

                qrCodeTimer.IsEnabled = true;
            }
            #endregion
            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalImg("qrcodePick_heilight");
        }

        //扫码检测
        private void QrcodeTimer(object sender, EventArgs e)
        {
            int Length = 1024;
            StringBuilder tempStr = new StringBuilder(Length);
            Dll_Camera.GetDecodeString(tempStr, out Length);
            byte[] bts = System.Text.Encoding.Default.GetBytes(tempStr.ToString());
            string newStr = System.Text.Encoding.GetEncoding("UTF-8").GetString(bts, 0, bts.Length);

            if (!string.IsNullOrWhiteSpace(newStr))
            {
                qrCodeTimer.IsEnabled = false;

                //扫描订单Code
                GetServerOrder(newStr);
                Dll_Camera.ReleaseDevice();
                ShowNewWin();
            }
        }

        //设备状态
        public Boolean deviceState = false;
        public Boolean qrEnable = true;
        public Boolean dmEnable = true;
        public Boolean hxEnable = true;
        public Boolean barEnable = true;
        #endregion

        #region 一卡通
        //一卡通取件
        private void cardPickClick(object sender, EventArgs e)
        {
            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalImg("numPick");
            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalImg("qrcodePick");
            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalImg("cardPick_press");
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome");
            inputValue = "";//清空输入
            qrCodeTimer.IsEnabled = false;
            Dll_Camera.ReleaseDevice();
        }

        //一卡通取件 - 弹起
        private void cardPickClickUp(object sender, EventArgs e)
        {
            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalImg("cardPick_heilight");

            this.numberFrame.Visibility = Visibility.Collapsed;
            this.imgPhoneScan.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region 返回主页
        //返回 返回按下
        private void btnBackClickDown(object sender, EventArgs e)
        {
            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalImg("numPick");
            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalImg("qrcodePick");
            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalImg("cardPick");
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome_press2");
            inputValue = "";//清空输入
            Dll_Camera.ReleaseDevice();
            BackAStep();
        }

        //主页 主页按下
        private void btnBackHomeClickDown(object sender, EventArgs e)
        {
            this.imgBtnChoice1.Source = ButtonExtend.InitButtonWithNormalImg("numPick");
            this.imgBtnChoice2.Source = ButtonExtend.InitButtonWithNormalImg("qrcodePick");
            this.imgBtnChoice3.Source = ButtonExtend.InitButtonWithNormalImg("cardPick");
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome_press1");
            inputValue = "";//清空输入
            Dll_Camera.ReleaseDevice();
            BackHoem();
        }
        #endregion

        #endregion

        #region 数字按钮组

        string inputValue = SysStringConfig.NumInputPlaceholder;
        //数字组点击
        private void numClick(object sender, EventArgs e)
        {
            var choiceBtn = sender as Image;
            string info = choiceBtn.Uid;

            if (info == "clear")//清空
            {
                inputValue = SysStringConfig.NumInputPlaceholder;
            }
            else if (info == "ok" && !string.IsNullOrWhiteSpace(inputValue) && inputValue != SysStringConfig.NumInputPlaceholder) //确认值
            {
                //满足验证码长度的才允许下一步
                if (inputValue.Length == int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxInputLength"]))
                {
                    //输入码 获取订单信息
                    GetServerOrder(inputValue);
                    ShowNewWin();
                }
            }
            else if (info != "clear" && info != "ok")
            {
                if (inputValue == SysStringConfig.NumInputPlaceholder) inputValue = "";
                if (inputValue.Length < int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxInputLength"]))
                {
                    inputValue += info;
                }
            }
            txtNumShow.Text = inputValue;
        }
        #endregion

        #region 数字按钮
        private void ShowCurTimer(object sender, EventArgs e)
        {
            //获得年月日
            tbTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");   //yyyy年MM月dd日 HH:mm:ss
        }

        //鼠标点击
        private void InputFocus(object sender, EventArgs e)
        {
            if (txtNumShow.Text == SysStringConfig.NumInputPlaceholder)
                txtNumShow.Text = "";
        }

        //鼠标移出
        private void LostFocus(object sender, EventArgs e)
        {
            if (txtNumShow.Text == "")
                txtNumShow.Text = SysStringConfig.NumInputPlaceholder;
        }

        //获取服务端订单
        private void GetServerOrder(string orderCode)
        {
            //获取订单
            var order = HttpAPIService.GetOrderFromAPI(orderCode);
            if (order != null)
            {
                GlobalCodeBuilder.filePaht = FileHelper.FileDownload(order.data[0].order_id, order.data[0].file_url);
            }
        }

        //打开新窗体
        private void ShowNewWin()
        {
            GlobalCodeBuilder.ProcessNum += 1;
            var newWindow = new PayWindow();
            newWindow.Show();
            this.Close();
        }

        //返回首页
        private void BackHoem()
        {
            GlobalCodeBuilder.ProcessNum = 1;
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        //返回一步
        private void BackAStep()
        {
            GlobalCodeBuilder.ProcessNum -= 1;

            if (GlobalCodeBuilder.ProcessNum <= 1) //返回首页
            {
                BackHoem();
            }

            //当前页面
            if (GlobalCodeBuilder.ProcessNum > 1)
            {
                ProcessInit();
            }
        }

        #endregion
        #endregion
    }
}
