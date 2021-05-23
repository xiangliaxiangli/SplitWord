using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAPI.Entities
{
    public class Field_File
    {
        public string filename { get; set; }
        public int fileID { get; set; }
        public List<string> keywords;
    }
}