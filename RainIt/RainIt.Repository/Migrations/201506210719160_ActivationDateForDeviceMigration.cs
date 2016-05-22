namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivationDateForDeviceMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceInfo", "ActivatedUTCDate", c => c.DateTime());
            DropColumn("dbo.DeviceInfo", "RegisteredUTCDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceInfo", "RegisteredUTCDate", c => c.DateTime());
            DropColumn("dbo.DeviceInfo", "ActivatedUTCDate");
        }
    }
}
