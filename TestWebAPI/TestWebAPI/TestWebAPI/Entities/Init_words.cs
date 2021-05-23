using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TestWebAPI.Entities
{
    public static class Init_words
    {
        public static string[] split_word = new string[50];   //停用词表
        private static HashSet<string> StopWords = new HashSet<string>();  //停用词表

        //静态构造函数 确保停用词只加载一次
        static Init_words()
        {
            string[] path = new string[4];  //存储四个停用词表的路径
            StreamReader sr;
            string line;
            //获得所有停用词词表所在路径
            path[0] = HttpContext.Current.Server.MapPath("~/Words/") + "baidu_stopwords.txt";
            path[1] = HttpContext.Current.Server.MapPath("~/Words/") + "cn_stopwords.txt";
            path[2] = HttpContext.Current.Server.MapPath("~/Words/") + "hit_stopwords.txt";
            path[3] = HttpContext.Current.Server.MapPath("~/Words/") + "scu_stopwords.txt";
            //利用hashSet整合所有的停用词 去重
            for (int i = 0; i < 4; i++)
            {
                sr = new StreamReader(path[i], Encoding.UTF8);
                while ((line = sr.ReadLine()) != null)
                {
                    StopWords.Add(line);
                }
            }
            StopWords.Add("\n");
            List<string> list = new List<string>(StopWords);
            split_word = list.ToArray();
        }
    }
}