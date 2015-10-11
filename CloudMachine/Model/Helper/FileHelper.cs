using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CloudMachine.Model.Helper
{
    using WORD = Microsoft.Office.Interop.Word;
    //文件
    public class FileHelper
    {
        //远程文件下载
        public static string FileDownload(string orderId, string fileAddress)
        {
            try
            {
                var webClient = new WebClient();
                var savePath = AppDomain.CurrentDomain.BaseDirectory + "\\Docs\\";
                string fileName = savePath + orderId + "_" + System.IO.Path.GetFileName(fileAddress);
                webClient.DownloadFile(fileAddress, fileName);
                return fileName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 获取待打文件内容
        /// </summary>
        /// <param name="filePaht"></param>
        /// <returns></returns>
        public static string GetFileContent(string filePath)
        {
            string content = "";
            WORD.Application app = new WORD.Application();//打开word程序
            WORD.Document doc = null;//实例化一个新的word文档
            
            Type wordType = app.GetType();
            object fileName = filePath;
            object unknow = Type.Missing;
            app.Visible = false;
            doc = app.Documents.Open(ref fileName,
                                ref unknow, ref unknow, ref unknow, ref unknow, ref unknow,
                                ref unknow, ref unknow, ref unknow, ref unknow, ref unknow,
                                ref unknow, ref unknow, ref unknow, ref unknow, ref unknow);

            content = doc.Content.Text.Trim();

            doc.Close(ref unknow, ref unknow, ref unknow);
            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, app, null);
            doc = null;
            app = null;
            //垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            return content;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary> 
        ///  Convert the word document to xps document 
        /// </summary> 
        /// <param name="wordFilename">Word document Path</param> 
        /// <param name="xpsFilename">Xps document Path</param> 
        /// <returns></returns> 
        public static System.Windows.Xps.Packaging.XpsDocument ConvertWordToXPS(string wordDocName, string xpsDocName, out string err)
        {
            err = "";
            System.Windows.Xps.Packaging.XpsDocument result = null;

            //创建一个word文档，并将要转换的文档添加到新创建的对象
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApplication.Documents.Add(wordDocName);
                Microsoft.Office.Interop.Word.Document doc = wordApplication.ActiveDocument;
                doc.ExportAsFixedFormat(xpsDocName, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatXPS, false, Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 0, 0, Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent, true, true, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, Type.Missing);
                result = new System.Windows.Xps.Packaging.XpsDocument(xpsDocName, System.IO.FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                err = "wordDocName:" + wordDocName + ",xpsDocName:" + xpsDocName+",err:"+ex.Message;
                wordApplication.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            }

            wordApplication.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);

            return result;
        }

        /// <summary>
        /// 方法2
        /// </summary>
        /// <param name="wordFilename"></param>
        /// <param name="xpsFilename"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static System.Windows.Xps.Packaging.XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename, out string err)
        {
            err = "";
            // Create a WordApplication and host word document 
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);
                // To Invisible the word document 
                wordApp.Application.Visible = false;
                // Minimize the opened word document 
                wordApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMinimize;
                Microsoft.Office.Interop.Word.Document doc = wordApp.ActiveDocument;
                doc.SaveAs(xpsFilename, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXPS);
                System.Windows.Xps.Packaging.XpsDocument xpsDocument = new System.Windows.Xps.Packaging.XpsDocument(xpsFilename, FileAccess.Read);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                err = "wordDocName:" + wordFilename + ",xpsDocName:" + xpsFilename + ",err:" + ex.Message;
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((Microsoft.Office.Interop.Word._Application)wordApp).Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            }
        } 
    }
}
