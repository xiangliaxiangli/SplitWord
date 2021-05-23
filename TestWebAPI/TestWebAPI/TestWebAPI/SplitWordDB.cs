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

        //�ļ���
        public string fileName { set; get; }

        //���±���
        public string title { set; get; }

        //�û���
        public string userName { set; get; }
        //����
        public string content { set; get; }

        //�ؼ��� ����ؼ�����|�ָ�
        public string keyWorld { set; get; }

        //���·���
        public string field { set; get; }

        //�ϴ�ʱ��  ������ַ������ͣ�ǰ�����н����ڻ���ʱ���ת��
        public string uploadTime { set; get; }

        //�ļ�����
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
