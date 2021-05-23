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
        /// 
        /// </summary>
        /// <returns></returns>
        //public string Get()
        //{
        //    return "Hello World";
        //}
        /// <summary>
        /// 接收前端传来的文件
        /// </summary>
        /// <returns></returns>
        //public IHttpActionResult getFiles()
        //{
        //    //获取文件
        //    HttpFileCollection files = HttpContext.Current.Request.Files;
        //    string filename = "NotFile";
        //    if (files.Count != 0)
        //    {
        //        filename = "Fileget";
        //    }
        //    foreach (string key in files.AllKeys)
        //    {
        //        filename = key;
        //        HttpPostedFile file1 = files[key];
        //        if (string.IsNullOrEmpty(file1.FileName) == false)
        //        {
        //            //filename = file1.FileName;
        //        }
        //        //file1.SaveAs(HttpContext.Current.Server.MapPath("~/Files/") + file1.FileName);
        //    }
        //    return Ok(filename);
        //}

        /// <summary>
        /// 从前端获取用户传输的文件和参数
        /// 参数目前包括的是用户选择的领域
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public string[] Post()
        {
            var files = HttpContext.Current.Request.Files;  //获取所有的文件
            var fields = HttpContext.Current.Request.Params["fields"];
            var temp_ = JsonConvert.DeserializeObject(fields);
            JArray jb = (JArray)temp_;  //接收前端传来的用户选择的领域  可以让后面分词的同学选择相应的领域词库
            HandleFile hf = new HandleFile(); //文件处理程序
            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];
                if (string.IsNullOrEmpty(file.FileName) == false)
                {
                    return hf.splitFileToSenstence(file);
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


    }
}
