namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConversionParameterAndSamplePatternMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConversionParameter",
                c => new
                    {
                        PatternId = c.Int(nullable: false),
                        RWeight = c.Double(nullable: false),
                        GWeight = c.Double(nullable: false),
                        BWeight = c.Double(nullable: false),
                        ThresholdPercentage = c.Double(nullable: false),
                        IsInverted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PatternId)
                .ForeignKey("dbo.Pattern", t => t.PatternId, cascadeDelete: true)
                .Index(t => t.PatternId);
            
            CreateTable(
                "dbo.SamplePattern",
                c => new
                    {
                        SamplePatternId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        Base64Image = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SamplePatternId)
                .ForeignKey("dbo.Device", t => t.SamplePatternId, cascadeDelete: true)
                .Index(t => t.SamplePatternId);
            
            AddColumn("dbo.Pattern", "SamplePattern_SamplePatternId", c => c.Int());
            CreateIndex("dbo.Pattern", "SamplePattern_SamplePatternId");
            AddForeignKey("dbo.Pattern", "SamplePattern_SamplePatternId", "dbo.SamplePattern", "SamplePatternId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SamplePattern", "SamplePatternId", "dbo.Device");
            DropForeignKey("dbo.Pattern", "SamplePattern_SamplePatternId", "dbo.SamplePattern");
            DropForeignKey("dbo.ConversionParameter", "PatternId", "dbo.Pattern");
            DropIndex("dbo.SamplePattern", new[] { "SamplePatternId" });
            DropIndex("dbo.ConversionParameter", new[] { "PatternId" });
            DropIndex("dbo.Pattern", new[] { "SamplePattern_SamplePatternId" });
            DropColumn("dbo.Pattern", "SamplePattern_SamplePatternId");
            DropTable("dbo.SamplePattern");
            DropTable("dbo.ConversionParameter");
        }
    }
}
