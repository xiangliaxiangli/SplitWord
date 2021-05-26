using System;
using System.Text;
using System.Web;
using Word = Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Spire.Doc;

namespace TestWebAPI.Entities
{
    public class HandleFile
    {
        private string[] split_word = new string[50];   //分隔词表

        /// <summary>
        /// 构造函数 初始化分割词表
        /// </summary>
        public HandleFile()
        {
            Init_Stop_Words();  
        }

        /// <summary>
        /// 初始化停用词表
        /// </summary>
        private void Init_Stop_Words()
        {
            split_word = Init_words.split_word;  //通过全局静态类获取静态停用词
        }

        //public string handle_word(string path)
        //{
        //    MessageBox.Show("看看看看扩");
        //    //创建word 的com
        //    //慢得离谱
        //    Word.Application app = new Microsoft.Office.Interop.Word.Application();
        //    Type wordType = app.GetType();
        //    Word.Document doc = null;
        //    object unknow = Type.Missing;
        //    app.Visible = false;

        //    //慢得也离谱
        //    MessageBox.Show("?sss");
        //    object files = path;
        //    doc = app.Documents.Open(ref files,
        //    ref unknow, ref unknow, ref unknow, ref unknow,
        //    ref unknow, ref unknow, ref unknow, ref unknow,
        //    ref unknow, ref unknow, ref unknow, ref unknow,
        //    ref unknow, ref unknow, ref unknow);
        //    int count = doc.Paragraphs.Count;
        //    StringBuilder sb = new StringBuilder();
        //    MessageBox.Show("开始!");
        //    //循环读取文件内容
        //    for (int i = 1; i <= count; i++)
        //    {

        //        sb.Append(doc.Paragraphs[i].Range.Text.Trim());
        //    }
        //    MessageBox.Show("结束");
        //    //关闭word文件
        //    doc.Close(ref unknow, ref unknow, ref unknow);
        //    wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, app, null);
        //    doc = null;
        //    app = null;
        //    //垃圾回收
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //    //转化为字符串
        //    string temp = sb.ToString();
        //    //如果有必要 转化为txt文件输出
        //    return temp;
        //    //预处理
        //}


        /// <summary>
        /// 对word类型的文件进行处理 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string handle_word(string path)
        {
            //加载word文档
            Document doc = new Document();
            doc.LoadFromFile(path);
            //使用GetText方法获取文档的所有文本
            string s = doc.GetText();
            return s;
        }

        /// <summary>
        /// pdf类型文件的读取
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string handle_pdf(string path)
        {
            //pdf文件转换
            try
            {
                PdfReader pdfReader = new PdfReader(path);
                int numberofPages = pdfReader.NumberOfPages;
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= numberofPages; i++)
                {
                    text.Append(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, i));
                }
                pdfReader.Close();  //关闭文件读取流
                return text.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 对txt类型的文件进行处理
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string handle_txt(string path)
        {
            StreamReader sr = new StreamReader(path,Encoding.UTF8);
            StringBuilder sb = new StringBuilder();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将文件内容按照分隔符分成句子
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string splitFileToSenstence(HttpPostedFile file)
        {
            //MessageBox.Show("分词开始");
            string[] array = file.FileName.Split('.');
            string type = array[1];  //获得文件的类型
            string all_content = null;  //文件的内容
            string[] sentence = null; //文件内容分割后得到的句子
            string path = HttpContext.Current.Server.MapPath("~/Files/") + file.FileName;  //将文件存储在服务器
            file.SaveAs(path);
            if (type == "pdf")  //pdf类型的文件
            {
                all_content = handle_pdf(path);  //将文本内容全部读取出来
            }
            else if (type == "doc" || type == "docx")  //word类型的文件
            {
                all_content = handle_word(path);
                //取出空字符元素
            }else if (type == "txt") //txt类型的文件
            {
                all_content = handle_txt(path);
            }
            //if (all_content != null)
            //{
            //    sentence = all_content.Split(split_word, StringSplitOptions.RemoveEmptyEntries);
            //}
            //string path_save_sentence = HttpContext.Current.Server.MapPath("~/OutPut/") + array[0] + ".txt";
            ////创建文件输入流 (文件的路径,存在同名文件时候覆盖源文件,文件的格式)
            //StreamWriter sw = new StreamWriter(path_save_sentence,false,Encoding.UTF8);
            //for(int i = 0; i < sentence.Length; i++)
            //{
            //    sw.WriteLine(sentence[i]);
            //}
            //sw.Flush();
            //sw.Close();
            //MessageBox.Show("分词结束");
            return all_content;
        }
    }
}