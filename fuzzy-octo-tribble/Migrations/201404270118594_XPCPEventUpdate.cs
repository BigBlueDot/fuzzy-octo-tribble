namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XPCPEventUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapEventModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        eventId = c.Int(nullable: false),
                        rewardType = c.Int(nullable: false),
                        rewardValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.uniq);
            
            AddColumn("dbo.MapModels", "activeEvent_uniq", c => c.Int());
            CreateIndex("dbo.MapModels", "activeEvent_uniq");
            AddForeignKey("dbo.MapModels", "activeEvent_uniq", "dbo.MapEventModels", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MapModels", "activeEvent_uniq", "dbo.MapEventModels");
            DropIndex("dbo.MapModels", new[] { "activeEvent_uniq" });
            DropColumn("dbo.MapModels", "activeEvent_uniq");
            DropTable("dbo.MapEventModels");
        }
    }
}
