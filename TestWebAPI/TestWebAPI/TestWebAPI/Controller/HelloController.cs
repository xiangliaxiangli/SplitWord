using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Windows.Forms;
using TestWebAPI.Entities;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace TestWebAPI.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HelloController : ApiController
    {
        
        /// <summary>
        /// 从前端获取用户传输的文件和参数
        /// 参数目前包括的是用户选择的领域
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public List<WordFrequency> Post()
        {
            var files = HttpContext.Current.Request.Files;  //获取所有的文件
            var field = HttpContext.Current.Request.Params["fields"];
            MessageBox.Show(field);  //打印出用户选择的领域号
            //var temp_ = JsonConvert.DeserializeObject(fields);
            //接收前端传来的用户选择的领域  可以让后面分词的同学选择相应的领域词库
            //JArray jb = (JArray)temp_;  
            //MessageBox.Show(jb[0].ToString());
            HandleFile hf = new HandleFile(); //文件处理程序
            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];
                if (string.IsNullOrEmpty(file.FileName) == false)
                {
                    string text = hf.splitFileToSenstence(file);  //获取文章内容
                    DomainDictionary domain = new DomainDictionary();
                    Segment segment = new Segment(domain, InitStopWordsDictionary());
                    List<WordFrequency> wordFrequencies = Statistics.SortWordsByFrequency(segment.CutArticle(text, field));
                    return wordFrequencies;
                }
            }
            return null;
        }

        /// <summary>
        /// 前端发送文章ID 后端返回File类型的文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult getFileByID(int ID)
        {
            MyFile mf = new MyFile();
            string filename = "《时间简史》读后感.docx";
            string path = HttpContext.Current.Server.MapPath("~/Files/") + filename;
            HandleFile hf = new HandleFile();
            mf.FileName = filename;
            mf.Content = hf.handle_word(path);
            return Json<MyFile>(mf);
        }



        //根据ID下载文件
        //FileID  文章的ID
        [HttpGet]
        public HttpResponseMessage DownloadFileByID(int ID)
        {
            //查询数据库
            string filename = "《时间简史》读后感.docx";
            string path = HttpContext.Current.Server.MapPath("~/Files/") + filename;

            //这部分代码尽量不要改动
            FileStream stream = new FileStream(path, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = HttpUtility.UrlEncode(filename)
            };
            response.Headers.Add("Access-Control-Expose-Headers", "FileName");
            response.Headers.Add("FileName", HttpUtility.UrlEncode(filename));
            return response;
        }

        #region--根据领域名称获取全部的文章
        [HttpPost]
        public IHttpActionResult getAllFilesByField(string Field)
        {
            List<Field_File> list = new List<Field_File>();
            Field_File temp = new Field_File();
            temp.fileID = 1;
            temp.filename = "你真的认识你自己吗？";
            temp.keywords = new List<string>();
            temp.keywords.Add("人工智能");
            temp.keywords.Add("图像识别");
            temp.keywords.Add("机器学习");
            temp.keywords.Add("计算机");
            temp.keywords.Add("科学");
            list.Add(temp);

            return Json<List<Field_File>>(list);
        }
        #endregion

        public HashSet<string> InitStopWordsDictionary()
        {
            HashSet<string> dict = new HashSet<string>();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Words";
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            dict.Add(line);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            return dict;
        }

        //[HttpPost]
        //public List<WordFrequency> SplitText()
        //{
        //    string text = "梦幻西游无限仙玉版到底有没有？今天我就带大家来了解一下，首先梦幻西游是网易游戏的一个拳头产品，这个问题已经不必多说了，梦幻西游是很多80，90后童年美好的回忆，不过当时大家都是小孩子，经济上没有独立，所以很多人玩梦幻就是耗时间，没办法氪金，所以在当时就有很多人想，如果梦幻西游仙玉可以无限使用该多好啊。";
        //    string field = "0";
        //    DomainDictionary domain = new DomainDictionary();
        //    Segment segment = new Segment(domain, InitStopWordsDictionary());
        //    List<WordFrequency> wordFrequencies = Statistics.SortWordsByFrequency(segment.CutArticle(text, field));
        //    return wordFrequencies;
        //}


        [HttpPost]
        //存储领域词汇
        public string UploadKeyWords()
        {
            var keywords = HttpContext.Current.Request.Params["keywords"];
            var temp = JsonConvert.DeserializeObject(keywords);
            //用户选择的关键字 将其存入数据库
            JArray jb = (JArray)temp;
         
            //MessageBox.Show(jb[0].ToString());
            //MessageBox.Show(jb[1].ToString());
            //MessageBox.Show(jb[2].ToString());
            return "Success";
        }

        //前端用户选择领域，输入查询语句，后端处理返回所有相关文件
        //Field:用户选取的领域
        //content:用户输入的查询语句
        //return:List<Field_File> 只返回文件名称和文件在数据库中的ID
        public IHttpActionResult Search(string Field,string content)
        {
            MessageBox.Show(Field + " " + content);
            List<Field_File> list = new List<Field_File>();
            Field_File temp = new Field_File();
            temp.fileID = 1;
            temp.filename = "你真的认识你自己吗？";
            list.Add(temp);
            Field_File temp2 = new Field_File();
            temp2.fileID = 2;
            temp2.filename = "非常不错";
            list.Add(temp2);
            return Json<List<Field_File>>(list);

        }

    }
}
