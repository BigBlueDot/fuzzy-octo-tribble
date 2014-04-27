namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventCollectionFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MapEventModels", "MapEventCollectionModel_uniq", c => c.Int());
            CreateIndex("dbo.MapEventModels", "MapEventCollectionModel_uniq");
            AddForeignKey("dbo.MapEventModels", "MapEventCollectionModel_uniq", "dbo.MapEventCollectionModels", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MapEventModels", "MapEventCollectionModel_uniq", "dbo.MapEventCollectionModels");
            DropIndex("dbo.MapEventModels", new[] { "MapEventCollectionModel_uniq" });
            DropColumn("dbo.MapEventModels", "MapEventCollectionModel_uniq");
        }
    }
}
