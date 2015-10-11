using System;
using System.Windows;
using CloudMachine.Model.Global;
using CloudMachine.Model.Helper;

namespace CloudMachine
{
    using CloudMachine.Model.Extend;
    using DC = DreamPlatform.CommonLibrary;
    using CloudMachine.Config;
    using System.Windows.Controls;
    using System.Threading;
    using System.Windows.Input;
    using CloudMachine.Model.Events;
    using System.Drawing.Printing;
    using System.Windows.Threading;
    using CloudMachine.Service;
    using System.Web;
    /// <summary>
    /// PayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PayWindow : Window
    {
        private DispatcherTimer ShowTimer;
        public PayWindow()
        {
            InitializeComponent();

            #region 窗体素材加载

            this.txtMachineCode.Text = GlobalCodeBuilder.MachineCodeBuilder;

            //打印预览frame
            PrintFrame();
            //Word装载预览
            this.Loaded += UC_PrintPreview_Loaded;

            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);//起个Timer一直获取当前时间
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            //底部流程
            ProcessInit();

            //测试阶段 隐藏掉支付步骤
            WhenClickPayWay();

            #endregion
        }

        #region UI
        //操作按钮组
        private void InitOperBtnFrame()
        {
            //窗体背景图
            this.imgBackground.Source = ButtonExtend.InitButtonWithNormalImg("Background");

            //二维码
            this.imgQrFram.Source = ButtonExtend.InitButtonWithNormalImg("imgQrFram");
            this.imgQr.Source = ButtonExtend.InitButtonWithNormalImg("QRcode");

            //左侧 执行打印按钮
            this.imgDoPrint.Source = ButtonExtend.InitButtonWithNormalImg("btnDoPrint");
            this.imgDoPrint.Visibility = Visibility.Collapsed;
            imgDoPrint.MouseDown += DoPrint;

            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome");
            //返回主页 两个button
            this.btnBack.Click += btnBackClickDown;
            this.btnHome.Click += btnBackHomeClickDown;

            this.imgPayWeichat.Source = ButtonExtend.InitButtonWithNormalImg("weiChat");
            imgPayWeichat.MouseDown += PayWayClick;

            this.imgPayAli.Source = ButtonExtend.InitButtonWithNormalImg("aliPay");
            imgPayAli.MouseDown += PayWayClick;

            this.imgPayCard.Source = ButtonExtend.InitButtonWithNormalImg("cardPay");
            imgPayCard.MouseDown += PayWayClick;

        }

        //打印预览frame
        private void PrintFrame()
        {
            //大矩形框子
            this.imgViewFrame.Source = ButtonExtend.InitButtonWithNormalImg("567Frame");

            this.imgViewPrting.Source = ButtonExtend.InitButtonWithNormalImg("doPrinting");
            imgViewPrting.Visibility = Visibility.Collapsed;

            this.imgViewPrintFinish.Source = ButtonExtend.InitButtonWithNormalImg("printSuccess");
            imgViewPrintFinish.Visibility = Visibility.Collapsed;

            //3d框子
            this.imgViewFrame_back.Source = ButtonExtend.InitButtonWithNormalImg("preViewFrame");
            imgViewFrame_back.Visibility = Visibility.Collapsed;

            //图片预览 #只有打印图片时才显示对应的图片 #打印Word文件这里预览文件
            this.imgViewFrame_WithPreView.Source = ButtonExtend.InitButtonWithNormalImg("preViewImg");
            imgViewFrame_WithPreView.Visibility = Visibility.Collapsed;

            //选择支付方式 title
            this.imgPayScanTitle.Source = ButtonExtend.InitButtonWithNormalImg("choicePayWay");

            //操作按钮组
            InitOperBtnFrame();
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
        #endregion

        #region 事件
        //文档装载预览
        void UC_PrintPreview_Loaded(object sender, RoutedEventArgs e)
        {
            dvcontent.Zoom = 75.0;
            try
            {
                if (!string.IsNullOrWhiteSpace(GlobalCodeBuilder.filePaht))
                {
                    string[] strArr = GlobalCodeBuilder.filePaht.Split('\\');
                    string xpsName = strArr[strArr.Length - 1].Substring(0, strArr[strArr.Length - 1].IndexOf('.'));
                    string xpsfilename = AppDomain.CurrentDomain.BaseDirectory + string.Format(@"Docs\{0}.xps", xpsName);
                    string err = "";
                    System.Windows.Xps.Packaging.XpsDocument doc = FileHelper.ConvertWordToXps(GlobalCodeBuilder.filePaht, xpsfilename, out err);
                    if (!string.IsNullOrWhiteSpace(err))
                    {
                        MessageBox.Show("doc is err:" +err);
                    }
                    if (doc != null)
                    {
                        dvcontent.Visibility = Visibility.Visible;
                        dvcontent.Document = doc.GetFixedDocumentSequence();
                        doc.Close();
                    }
                }
                else {
                    MessageBox.Show("未能正确获取文档地址");
                }
            }
            catch (Exception ex)
            {
                HttpAPIService.LiveReportAPI(HttpUtility.UrlEncode("文档预览错误"),"0");
            }
        }

        //返回主页 返回按下
        private void btnBackClickDown(object sender, EventArgs e)
        {
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome_press2");
            BackAStep();
        }

        //返回主页 主页按下
        private void btnBackHomeClickDown(object sender, EventArgs e)
        {
            this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome_press1");
            BackHoem();
        }

        //返回一步
        private void BackAStep()
        {
            //GlobalCodeBuilder.ProcessNum -= 1;
            //测试在没有 支付页面的情况下
            GlobalCodeBuilder.ProcessNum -= 2;

            if (GlobalCodeBuilder.ProcessNum <= 1) //返回首页
            {
                BackHoem();
            }

            if (GlobalCodeBuilder.ProcessNum <= 2) //返回首页
            {
                var newWindow = new ScanWindow(new DocumentPrintFlowElement() { BtnNumPick = 1 });
                newWindow.Show();
                this.Close();
            }

            //当前页面
            if (GlobalCodeBuilder.ProcessNum >= 3)
            {
                //支付
                if (GlobalCodeBuilder.ProcessNum == 3)
                {
                    imgViewFrame.Visibility = imgViewPrting.Visibility = Visibility.Collapsed;
                    //imgViewFrame_back.Visibility = imgViewFrame_WithPreView.Visibility = imgBtnChoice4.Visibility = imgDoPrint.Visibility = Visibility.Visible;
                    imgViewFrame_back.Visibility = imgBtnChoice4.Visibility = imgDoPrint.Visibility = Visibility.Visible;

                    this.imgBtnChoice4.Source = ButtonExtend.InitButtonWithNormalImg("backHome");

                    this.imgDoPrint.Source = ButtonExtend.InitButtonWithNormalImg("btnDoPrint");
                    //this.imgDoPrint.Visibility = imgViewFrame_back.Visibility = imgViewFrame_WithPreView.Visibility = Visibility.Collapsed;
                    this.imgDoPrint.Visibility = imgViewFrame_back.Visibility = Visibility.Collapsed;

                    this.imgPayWeichat.Source = ButtonExtend.InitButtonWithNormalImg("weiChat");
                    this.imgPayAli.Source = ButtonExtend.InitButtonWithNormalImg("aliPay");
                    this.imgPayCard.Source = ButtonExtend.InitButtonWithNormalImg("cardPay");
                    imgPayScanTitle.Visibility = imgViewFrame.Visibility = imgPayWeichat.Visibility = imgPayAli.Visibility = imgPayCard.Visibility = Visibility.Visible;
                    ProcessInit();
                }
            }
        }

        //返回首页
        private void BackHoem()
        {
            GlobalCodeBuilder.ProcessNum = 1;
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void ShowCurTimer(object sender, EventArgs e)
        {
            //获得年月日
            this.tbTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");   //yyyy年MM月dd日 HH:mm:ss
        }

        //支付方式选择
        private void PayWayClick(object sender, EventArgs e)
        {
            var payWay = sender as Image;

            if (payWay.Uid == "weichat")//微信
            {
                this.imgPayWeichat.Source = ButtonExtend.InitButtonWithNormalImg("weiChat_press");
            }
            else if (payWay.Uid == "alipay")//支付宝
            {
                this.imgPayAli.Source = ButtonExtend.InitButtonWithNormalImg("aliPay_press");
            }
            else if (payWay.Uid == "card")//刷卡
            {
                this.imgPayCard.Source = ButtonExtend.InitButtonWithNormalImg("cardPay_press");
            }
            WhenClickPayWay();
        }

        //选择支付方式之后
        private void WhenClickPayWay() {
            GlobalCodeBuilder.ProcessNum += 1;
            ProcessInit();
            //imgDoPrint.Visibility = imgViewFrame_WithPreView.Visibility = imgViewFrame_back.Visibility = Visibility.Visible;
            imgDoPrint.Visibility = imgViewFrame_back.Visibility = Visibility.Visible;
            imgPayWeichat.Visibility = imgPayAli.Visibility = imgPayCard.Visibility = imgViewFrame.Visibility = imgPayScanTitle.Visibility = Visibility.Collapsed;
        }

        //执行打印
        private void DoPrint(object sender, MouseButtonEventArgs e)
        {
            this.imgDoPrint.Source = ButtonExtend.InitButtonWithNormalImg("btnDoPrint_press");
            imgViewFrame.Visibility = imgViewPrting.Visibility = Visibility.Visible;
            //imgViewFrame_back.Visibility = imgViewFrame_WithPreView.Visibility = imgBtnChoice4.Visibility = imgDoPrint.Visibility = Visibility.Collapsed;
            imgViewFrame_back.Visibility = imgBtnChoice4.Visibility = dvcontent.Visibility = imgDoPrint.Visibility = Visibility.Collapsed;
            GlobalCodeBuilder.ProcessNum += 1;
            ProcessInit();

            //子线程打印
            Thread printThread = new Thread(FinishPrint);
            printThread.IsBackground = true;
            printThread.Start();

            ////初始化PrintDialog
            //var printDialog = new PrintDialog();
            ////选择一个打印机
            //var selectedPrinter = new LocalPrintServer().DefaultPrintQueue;
            ////设置打印机
            //PrintTicket pt = new PrintTicket();
            //pt.Duplexing = Duplexing.OneSided;//arg.Duplex;
            //printDialog.PrintQueue = selectedPrinter;
            //printDialog.PrintTicket = pt;
            ////printDialog.PrintDocument(dvcontent.Document.DocumentPaginator, "");

            //FinishPrint();
        }

        //完成打印
        private void FinishPrint()
        {
            //实例化打印对象
            PrintDocument printDocument1 = new PrintDocument();
            //设置打印用的纸张,当设置为Custom的时候，可以自定义纸张的大小
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 500, 500);
            //注册PrintPage事件，打印每一页时会触发该事件
            printDocument1.PrintPage += new PrintPageEventHandler(this.PrintDocument_PrintPage);
            printDocument1.PrintController = new StandardPrintController();
            //开始打印
            printDocument1.Print();

            Thread.Sleep(1000);
            if (!string.IsNullOrWhiteSpace(GlobalCodeBuilder.filePaht))
            {
                FileHelper.DeleteFile(GlobalCodeBuilder.filePaht);
                string xpsFile = GlobalCodeBuilder.filePaht.Substring(0, GlobalCodeBuilder.filePaht.LastIndexOf('.')) + ".xps";
                FileHelper.DeleteFile(xpsFile);
            }
            GlobalCodeBuilder.filePaht = "";
            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    imgViewPrintFinish.Visibility = Visibility.Visible;
                    imgViewPrting.Visibility = Visibility.Collapsed;
                    GlobalCodeBuilder.ProcessNum += 1;
                    ProcessInit();
                    GlobalCodeBuilder.ProcessNum = 1;

                    //子线程打印
                    Thread printThread = new Thread(GoBackMain);
                    printThread.IsBackground = true;
                    printThread.Start();
                }));
            }).Start();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintDocument_PrintPage(object sender,PrintPageEventArgs e)
        {
            try
            {
                //获取待打内容
                string content = FileHelper.GetFileContent(GlobalCodeBuilder.filePaht);
                //设置打印内容及其字体，颜色和位置  new System.Drawing.Font(new System.Drawing.FontFamily("黑体"), 24)
                e.Graphics.DrawString(content, new System.Drawing.Font(new System.Drawing.FontFamily("Tahoma"), 11), System.Drawing.Brushes.Red, 50, 50);
            }
            catch (Exception ex)
            {

            }
        }

        //回到首页
        private void GoBackMain()
        {
            Thread.Sleep(2000);//操作成功
            new Thread(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    OpenNewWin();
                }));
            }).Start();
        }

        //回到首页
        private void OpenNewWin()
        {
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        #endregion
    }
}
