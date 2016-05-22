namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceNameMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Device", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Device", "Name");
        }
    }
}
