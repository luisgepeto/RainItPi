namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleRoutineMigration : DbMigration
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
            
            AddColumn("dbo.Pattern", "SampleRoutineId", c => c.Int());
            CreateIndex("dbo.Pattern", "SampleRoutineId");
            AddForeignKey("dbo.Pattern", "SampleRoutineId", "dbo.SampleRoutine", "SampleRoutineId");
            DropColumn("dbo.Pattern", "SamplePattern_SamplePatternId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pattern", "SamplePattern_SamplePatternId", c => c.Int());
            DropForeignKey("dbo.SampleRoutine", "SampleRoutineId", "dbo.Device");
            DropForeignKey("dbo.Pattern", "SampleRoutineId", "dbo.SampleRoutine");
            DropIndex("dbo.SampleRoutine", new[] { "SampleRoutineId" });
            DropIndex("dbo.Pattern", new[] { "SampleRoutineId" });
            DropColumn("dbo.Pattern", "SampleRoutineId");
            DropTable("dbo.SampleRoutine");
            CreateIndex("dbo.Pattern", "SamplePattern_SamplePatternId");
            AddForeignKey("dbo.Pattern", "SamplePattern_SamplePatternId", "dbo.SamplePattern", "SamplePatternId");
        }
    }
}
