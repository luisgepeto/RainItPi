namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSettingsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        MaxPatternByteCount = c.Int(nullable: false),
                        MaxPatternPixelHeight = c.Int(nullable: false),
                        MaxPatternPixelWidth = c.Int(nullable: false),
                        MaxPatternCountPerRoutine = c.Int(nullable: false),
                        MaxNumberOfRepetitionsPerPattern = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSettings", "UserId", "dbo.User");
            DropIndex("dbo.UserSettings", new[] { "UserId" });
            DropTable("dbo.UserSettings");
        }
    }
}
