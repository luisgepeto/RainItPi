namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameToUTCSamplePatternMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SamplePattern", "UpdateUTCDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.SamplePattern", "UpdateDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SamplePattern", "UpdateDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.SamplePattern", "UpdateUTCDateTime");
        }
    }
}
