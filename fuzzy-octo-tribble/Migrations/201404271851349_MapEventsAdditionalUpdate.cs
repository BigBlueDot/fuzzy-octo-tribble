namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapEventsAdditionalUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventDataModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.uniq);
            
            AddColumn("dbo.MapEventModels", "eventData_uniq", c => c.Int());
            CreateIndex("dbo.MapEventModels", "eventData_uniq");
            AddForeignKey("dbo.MapEventModels", "eventData_uniq", "dbo.EventDataModels", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MapEventModels", "eventData_uniq", "dbo.EventDataModels");
            DropIndex("dbo.MapEventModels", new[] { "eventData_uniq" });
            DropColumn("dbo.MapEventModels", "eventData_uniq");
            DropTable("dbo.EventDataModels");
        }
    }
}
