namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedToUTCNameMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SampleRoutine", "UpdateUTCDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.SampleRoutine", "UpdateDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SampleRoutine", "UpdateDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.SampleRoutine", "UpdateUTCDateTime");
        }
    }
}
