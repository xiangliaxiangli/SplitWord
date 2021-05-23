namespace TestWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.File",
                c => new
                    {
                        fileId = c.Int(nullable: false, identity: true),
                        fileName = c.String(),
                        title = c.String(),
                        userName = c.String(),
                        content = c.String(),
                        keyWorld = c.String(),
                        field = c.String(),
                        uploadTime = c.String(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.fileId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        userName = c.String(),
                        password = c.String(),
                        userSex = c.String(),
                        userType = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
            DropTable("dbo.File");
        }
    }
}
