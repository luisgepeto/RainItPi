namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceSettingsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        DeviceId = c.Int(nullable: false),
                        MinutesRefreshRate = c.Int(nullable: false),
                        MillisecondLatchDelay = c.Int(nullable: false),
                        MillisecondClockDelay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Settings", "DeviceId", "dbo.Device");
            DropIndex("dbo.Settings", new[] { "DeviceId" });
            DropTable("dbo.Settings");
        }
    }
}
