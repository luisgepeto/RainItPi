namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalDeviceForUserMigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Device", new[] { "UserId" });
            AlterColumn("dbo.Device", "UserId", c => c.Int());
            CreateIndex("dbo.Device", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Device", new[] { "UserId" });
            AlterColumn("dbo.Device", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Device", "UserId");
        }
    }
}
