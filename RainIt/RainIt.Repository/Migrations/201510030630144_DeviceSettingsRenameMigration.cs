namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceSettingsRenameMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Settings", newName: "DeviceSettings");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DeviceSettings", newName: "Settings");
        }
    }
}
