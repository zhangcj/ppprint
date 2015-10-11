namespace CloudMachine.Config
{
    //文档打印元素
    public class DocumentPrintFlowElement
    {
        //按钮 验证码取件
        public int BtnNumPick = 0;
        //按钮 二维码取件
        public int QrCodePick = 0;
        //一卡通取件
        public int CardPick = 0;
        //返回首页
        public int BtnBackHome = 0;

        //取件方式
        public PickType Pick = PickType.NumPick;
    }

    /// <summary>
    /// 取件方式
    /// </summary>
    public enum PickType
    {
        NumPick=0,
        QrCodePick=1,
        CardPick=2
    }
}
