using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace TestWebAPI.Entities
{
    public class Segment
    {
        private HashSet<string> StopWordsDictionary;
        private DomainDictionary DomainDictionary;
        private char[] Separator = { '，', '？', '。', '"', ';', '!', '：', '、', '-' }; // 中文分隔符
        private JiebaSegmenter segmenter = new JiebaSegmenter();
        public string field { get; set; } // 领域
        public List<string> articles { get; set; } // 待分词的文章

        List<string> res = new List<string>();//领域分词筛选过后的结果

        private static object Locker = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict">字典</param>
        /// <param name="field">领域</param>
        public Segment(DomainDictionary DomainDictionary, HashSet<string> StopWordsDictionary)
        {
            this.DomainDictionary = DomainDictionary;
            this.StopWordsDictionary = StopWordsDictionary;
        }

        /// <summary>
        /// 把一行文本进行分词,去掉停用词，提取领域关键词
        /// </summary>
        /// <param name="text">待分词的文本</param>
        /// <returns>以列表形式返回提取领域关键字结果</returns>
        private List<string> Cut(string text)
        {
            List<string> res = new List<string>();
            var segments = segmenter.CutForSearch(text);
            foreach (string s in segments)
            {
                if (!StopWordsDictionary.Contains(s) && DomainDictionary.FindWordInDict(s, field))
                {
                    res.Add(s);
                }
            }
            return res;
        }

        class CutThreadParameters
        {
            public int begin;
            public int end;

            public CutThreadParameters(int begin, int end)
            {
                this.begin = begin;
                this.end = end;
            }
        }

        /// <summary>
        /// 用于在多线程时对部分段落进行分词
        /// </summary>
        /// <param name="CutThreadParameters"></param>
        private void CutArticleBetween(object CutThreadParameters)
        {
            CutThreadParameters parameters = CutThreadParameters as CutThreadParameters;
            List<string> DomainKeyWords = new List<string>();
            for (int i = parameters.begin; i < parameters.end; i++)
            {
                DomainKeyWords.AddRange(Cut(articles[i]));
            }
            lock (Locker)
            {
                res.AddRange(DomainKeyWords);
            }
        }

        private void SplitBySeparator(string text)
        {
            string[] temp = text.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            articles = new List<string>(temp);
        }
        private void SplitByDomainDictionary()
        {
            res.Clear();
            int ThreadNumber = 10; //默认线程数10
            int size = articles.Count / ThreadNumber;
            List<Thread> threads = new List<Thread>();
            int begin = 0, end = size;
            if (articles.Count > ThreadNumber)
            {
                while (end < articles.Count)
                {
                    CutThreadParameters parameters = new CutThreadParameters(begin, end);
                    Thread thread = new Thread(new ParameterizedThreadStart(CutArticleBetween));
                    thread.IsBackground = true;
                    thread.Start(parameters);
                    threads.Add(thread);
                    begin += size;
                    end = end + size < articles.Count ? end + size : articles.Count;
                }
                for (int i = 0; i < threads.Count; i++)
                {
                    threads[i].Join();
                }
            }
            else
            {
                CutArticleBetween(new CutThreadParameters(0, articles.Count));
            }
        }


        public List<string> CutArticle(string text, string field)
        {
            this.field = field;
            SplitBySeparator(text); //使用分隔符，把文章切分成句子
            SplitByDomainDictionary(); // 领域词典进行分词
            return res;
        }
    }
}