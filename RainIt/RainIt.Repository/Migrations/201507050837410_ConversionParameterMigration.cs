namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConversionParameterMigration : DbMigration
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
                        ThresholdValue = c.Double(nullable: false),
                        IsInverted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PatternId)
                .ForeignKey("dbo.Pattern", t => t.PatternId, cascadeDelete: true)
                .Index(t => t.PatternId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConversionParameter", "PatternId", "dbo.Pattern");
            DropIndex("dbo.ConversionParameter", new[] { "PatternId" });
            DropTable("dbo.ConversionParameter");
        }
    }
}
