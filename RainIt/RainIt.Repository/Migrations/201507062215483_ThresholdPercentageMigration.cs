namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThresholdPercentageMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConversionParameter", "ThresholdPercentage", c => c.Double(nullable: false));
            DropColumn("dbo.ConversionParameter", "ThresholdValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConversionParameter", "ThresholdValue", c => c.Double(nullable: false));
            DropColumn("dbo.ConversionParameter", "ThresholdPercentage");
        }
    }
}
