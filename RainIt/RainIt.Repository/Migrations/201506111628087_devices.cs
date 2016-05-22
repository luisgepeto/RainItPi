namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoutineId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.Routine", t => t.RoutineId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoutineId);
            
            CreateTable(
                "dbo.DeviceCredential",
                c => new
                    {
                        DeviceId = c.Int(nullable: false),
                        Salt = c.String(nullable: false),
                        Hash = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.DeviceInfo",
                c => new
                    {
                        DeviceId = c.Int(nullable: false),
                        Identifier = c.Guid(nullable: false),
                        Serial = c.String(nullable: false),
                        RegisteredUTCDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Device", "UserId", "dbo.User");
            DropForeignKey("dbo.Device", "RoutineId", "dbo.Routine");
            DropForeignKey("dbo.DeviceInfo", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.DeviceCredential", "DeviceId", "dbo.Device");
            DropIndex("dbo.DeviceInfo", new[] { "DeviceId" });
            DropIndex("dbo.DeviceCredential", new[] { "DeviceId" });
            DropIndex("dbo.Device", new[] { "RoutineId" });
            DropIndex("dbo.Device", new[] { "UserId" });
            DropTable("dbo.DeviceInfo");
            DropTable("dbo.DeviceCredential");
            DropTable("dbo.Device");
        }
    }
}
