namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devices1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceCredential", "DeviceId", "dbo.Device");
            DropIndex("dbo.DeviceCredential", new[] { "DeviceId" });
            DropPrimaryKey("dbo.DeviceCredential");
            AddColumn("dbo.DeviceCredential", "Device_DeviceId", c => c.Int());
            AlterColumn("dbo.DeviceCredential", "DeviceId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DeviceCredential", "DeviceId");
            CreateIndex("dbo.DeviceCredential", "Device_DeviceId");
            AddForeignKey("dbo.DeviceCredential", "Device_DeviceId", "dbo.Device", "DeviceId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceCredential", "Device_DeviceId", "dbo.Device");
            DropIndex("dbo.DeviceCredential", new[] { "Device_DeviceId" });
            DropPrimaryKey("dbo.DeviceCredential");
            AlterColumn("dbo.DeviceCredential", "DeviceId", c => c.Int(nullable: false));
            DropColumn("dbo.DeviceCredential", "Device_DeviceId");
            AddPrimaryKey("dbo.DeviceCredential", "DeviceId");
            CreateIndex("dbo.DeviceCredential", "DeviceId");
            AddForeignKey("dbo.DeviceCredential", "DeviceId", "dbo.Device", "DeviceId", cascadeDelete: true);
        }
    }
}
