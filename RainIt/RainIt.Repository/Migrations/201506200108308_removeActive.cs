namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeActive : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Routine", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routine", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
