namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false, maxLength: 50),
                        State = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        AddressLine1 = c.String(nullable: false, maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 20),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Password",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Salt = c.String(nullable: false),
                        Hash = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Pattern",
                c => new
                    {
                        PatternId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        FileType = c.String(nullable: false, maxLength: 50),
                        BytesFileSize = c.Long(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Path = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatternId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RoutinePattern",
                c => new
                    {
                        RoutinePatternId = c.Int(nullable: false, identity: true),
                        RoutineId = c.Int(),
                        PatternId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoutinePatternId)
                .ForeignKey("dbo.Pattern", t => t.PatternId)
                .ForeignKey("dbo.Routine", t => t.RoutineId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoutineId)
                .Index(t => t.PatternId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Routine",
                c => new
                    {
                        RoutineId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoutineId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfo", "UserId", "dbo.User");
            DropForeignKey("dbo.Routine", "UserId", "dbo.User");
            DropForeignKey("dbo.RoutinePattern", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.Pattern", "UserId", "dbo.User");
            DropForeignKey("dbo.RoutinePattern", "RoutineId", "dbo.Routine");
            DropForeignKey("dbo.RoutinePattern", "PatternId", "dbo.Pattern");
            DropForeignKey("dbo.Password", "UserId", "dbo.User");
            DropForeignKey("dbo.Address", "UserId", "dbo.User");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserInfo", new[] { "UserId" });
            DropIndex("dbo.Routine", new[] { "UserId" });
            DropIndex("dbo.RoutinePattern", new[] { "UserId" });
            DropIndex("dbo.RoutinePattern", new[] { "PatternId" });
            DropIndex("dbo.RoutinePattern", new[] { "RoutineId" });
            DropIndex("dbo.Pattern", new[] { "UserId" });
            DropIndex("dbo.Password", new[] { "UserId" });
            DropIndex("dbo.Address", new[] { "UserId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.UserInfo");
            DropTable("dbo.Role");
            DropTable("dbo.Routine");
            DropTable("dbo.RoutinePattern");
            DropTable("dbo.Pattern");
            DropTable("dbo.Password");
            DropTable("dbo.User");
            DropTable("dbo.Address");
        }
    }
}
