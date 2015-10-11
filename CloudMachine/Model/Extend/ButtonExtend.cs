using CloudMachine.Config;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DC = DreamPlatform.CommonLibrary;

namespace CloudMachine.Model.Extend
{
    /// <summary>
    /// 按钮扩展类
    /// </summary>
    public class ButtonExtend
    {
        //初始化图片按钮
        public static BitmapImage InitButtonWithNormalImg(string normalImg)
        {
            return DC.BitmapHelper.GetBitmapImage(string.Format("Images/{0}.png",normalImg));
        }

        /// <summary>
        /// 按钮 normal 和 press 图片
        /// </summary>
        /// <param name="normalImg"></param>
        /// <param name="code"></param>
        /// <param name="pressImg"></param>
        /// <returns></returns>
        public static BitmapImage InitButtonWithNormalAndPressImg(string normalImg, int code, string pressImg)
        {
            switch (code)
            {
                case 0:
                    return DC.BitmapHelper.GetBitmapImage(string.Format("Images/{0}.png", normalImg));
                    break;
                case 1:
                    return DC.BitmapHelper.GetBitmapImage(string.Format("Images/{0}.png", pressImg));
                    break;
                default:
                    return DC.BitmapHelper.GetBitmapImage(string.Format("Images/{0}.png", normalImg));
                    break;
            }
        }

        /// <summary>
        /// 流程
        /// </summary>
        /// <param name="finish"></param>
        /// <param name="ch"></param>
        /// <param name="watting"></param>
        /// <returns></returns>
        public static BitmapImage InitButtonWithProcessImg(string finish, bool ch, string watting)
        {
            if(ch)
                return DC.BitmapHelper.GetBitmapImage(string.Format("Images/{0}.png", watting));
            return DC.BitmapHelper.GetBitmapImage(string.Format("Images/{0}.png", finish));
        }
    }
}
