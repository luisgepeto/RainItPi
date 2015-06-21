namespace RainIt.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultipleRoutinesPerDeviceMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Device", "RoutineId", "dbo.Routine");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropIndex("dbo.Device", new[] { "RoutineId" });
            CreateTable(
                "dbo.[dbo.DeviceRoutine]",
                c => new
                    {
                        DeviceId = c.Int(nullable: false),
                        RoutineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeviceId, t.RoutineId })
                .ForeignKey("dbo.Device", t => t.DeviceId)
                .ForeignKey("dbo.Routine", t => t.RoutineId)
                .Index(t => t.DeviceId)
                .Index(t => t.RoutineId);
            
            AddForeignKey("dbo.UserRole", "UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.UserRole", "RoleId", "dbo.Role", "RoleId");
            DropColumn("dbo.Device", "RoutineId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Device", "RoutineId", c => c.Int());
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.[dbo.DeviceRoutine]", "RoutineId", "dbo.Routine");
            DropForeignKey("dbo.[dbo.DeviceRoutine]", "DeviceId", "dbo.Device");
            DropIndex("dbo.[dbo.DeviceRoutine]", new[] { "RoutineId" });
            DropIndex("dbo.[dbo.DeviceRoutine]", new[] { "DeviceId" });
            DropTable("dbo.[dbo.DeviceRoutine]");
            CreateIndex("dbo.Device", "RoutineId");
            AddForeignKey("dbo.UserRole", "RoleId", "dbo.Role", "RoleId", cascadeDelete: true);
            AddForeignKey("dbo.UserRole", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Device", "RoutineId", "dbo.Routine", "RoutineId");
        }
    }
}
