namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestsUpdateTwo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapEventCollectionModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.uniq);
            
            AddColumn("dbo.MapModels", "eventCollection_uniq", c => c.Int());
            CreateIndex("dbo.MapModels", "eventCollection_uniq");
            AddForeignKey("dbo.MapModels", "eventCollection_uniq", "dbo.MapEventCollectionModels", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MapModels", "eventCollection_uniq", "dbo.MapEventCollectionModels");
            DropIndex("dbo.MapModels", new[] { "eventCollection_uniq" });
            DropColumn("dbo.MapModels", "eventCollection_uniq");
            DropTable("dbo.MapEventCollectionModels");
        }
    }
}
