using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace TestWebAPI.Entities
{
    public class DomainDictionary
    {
        public static Dictionary<string, HashSet<string>> dict { get; set; } = null;
        public static object Locker = new object();
        public static int DomainDictNumber { get; set; } = 1;

        public DomainDictionary()
        {
            if (dict != null) return;
            Init();
        }

        private void Init()
        {
            dict = new Dictionary<string, HashSet<string>>();
            Thread[] threads = new Thread[DomainDictNumber];
            for (int i = 0; i < DomainDictNumber; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(InitDictionary));
                threads[i].IsBackground = true;
                threads[i].Start(i);
            }
            for (int i = 0; i < DomainDictNumber; i++)
            {
                threads[i].Join();
            }
        }

        private void InitDictionary(object index)
        {
            string root = AppDomain.CurrentDomain.BaseDirectory; // 项目根目录
            string[] path = new string[DomainDictNumber];
            path[0] = root + @"Dict\0";
            //path[1] = root + @"Dict\1";
            //path[2] = root + @"Dict\2";
            //path[3] = root + @"Dict\3";
            //path[4] = root + @"Dict\4";
            //path[5] = root + @"Dict\5";
            //path[6] = root + @"Dict\6";
            HashSet<string> DomainLexicon = new HashSet<string>();
            DirectoryInfo folder = new DirectoryInfo(path[(int)index]);
            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            DomainLexicon.Add(line);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            lock (Locker)
            {
                dict[index.ToString()] = DomainLexicon;
                //Console.WriteLine("字典" + index + "加载完毕");
            }
        }

        public bool FindWordInDict(string word, string index)
        {
            return dict[index].Contains(word);
        }
    }
}