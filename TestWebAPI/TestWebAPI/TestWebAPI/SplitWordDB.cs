using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
namespace TestWebAPI
{
    public partial class SplitWordDB : DbContext
    {
        public SplitWordDB()
            : base("name=SplitWordDB")
        {
        }

        public DbSet<File> File { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }

    [Table("File")]
    public class File
    {
        [Key]
        public int fileId { set; get; }

        //文件名
        public string fileName { set; get; }

        //文章标题
        public string title { set; get; }

        //用户名
        public string userName { set; get; }
        //内容
        public string content { set; get; }

        //关键字 多个关键字以|分隔
        public string keyWorld { set; get; }

        //文章分类
        public string field { set; get; }

        //上传时间  这里存字符串类型，前端自行将日期或者时间戳转换
        public string uploadTime { set; get; }

        //文件类型
        public string type { set; get; }
    }

    [Table("User")]
    public class User
    {
        [Key]
        public int UserID { set; get; }

        public string userName { set; get; }

        public string password { set; get; }

        public string userSex { set; get; }

        public string userType { set; get; }
    }
}
