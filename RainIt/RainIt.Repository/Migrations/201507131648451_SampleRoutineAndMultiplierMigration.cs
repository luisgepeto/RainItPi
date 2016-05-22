namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleRoutineAndMultiplierMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pattern", "SamplePattern_SamplePatternId", "dbo.SamplePattern");
            DropIndex("dbo.Pattern", new[] { "SamplePattern_SamplePatternId" });
            CreateTable(
                "dbo.SampleRoutine",
                c => new
                    {
                        SampleRoutineId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SampleRoutineId)
                .ForeignKey("dbo.Device", t => t.SampleRoutineId, cascadeDelete: true)
                .Index(t => t.SampleRoutineId);
            
            AddColumn("dbo.RoutinePattern", "Repetitions", c => c.Int(nullable: false));
            AddColumn("dbo.RoutinePattern", "SampleRoutineId", c => c.Int());
            AddColumn("dbo.Pattern", "SampleRoutineId", c => c.Int());
            CreateIndex("dbo.RoutinePattern", "SampleRoutineId");
            CreateIndex("dbo.Pattern", "SampleRoutineId");
            AddForeignKey("dbo.Pattern", "SampleRoutineId", "dbo.SampleRoutine", "SampleRoutineId");
            AddForeignKey("dbo.RoutinePattern", "SampleRoutineId", "dbo.SampleRoutine", "SampleRoutineId");
            DropColumn("dbo.Pattern", "SamplePattern_SamplePatternId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pattern", "SamplePattern_SamplePatternId", c => c.Int());
            DropForeignKey("dbo.SampleRoutine", "SampleRoutineId", "dbo.Device");
            DropForeignKey("dbo.RoutinePattern", "SampleRoutineId", "dbo.SampleRoutine");
            DropForeignKey("dbo.Pattern", "SampleRoutineId", "dbo.SampleRoutine");
            DropIndex("dbo.SampleRoutine", new[] { "SampleRoutineId" });
            DropIndex("dbo.Pattern", new[] { "SampleRoutineId" });
            DropIndex("dbo.RoutinePattern", new[] { "SampleRoutineId" });
            DropColumn("dbo.Pattern", "SampleRoutineId");
            DropColumn("dbo.RoutinePattern", "SampleRoutineId");
            DropColumn("dbo.RoutinePattern", "Repetitions");
            DropTable("dbo.SampleRoutine");
            CreateIndex("dbo.Pattern", "SamplePattern_SamplePatternId");
            AddForeignKey("dbo.Pattern", "SamplePattern_SamplePatternId", "dbo.SamplePattern", "SamplePatternId");
        }
    }
}
