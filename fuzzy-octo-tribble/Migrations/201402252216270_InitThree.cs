namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitThree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.uniq);
            
            AddColumn("dbo.PlayerModels", "rootMap", c => c.String(defaultValue: "Ensemble Village"));
            AddColumn("dbo.PlayerModels", "rootX", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.PlayerModels", "rootY", c => c.Int(nullable: false, defaultValue: 5));
            AddColumn("dbo.PartyModels", "location_uniq", c => c.Int());
            AddForeignKey("dbo.PartyModels", "location_uniq", "dbo.MapModels", "uniq");
            CreateIndex("dbo.PartyModels", "location_uniq");
            DropColumn("dbo.PartyModels", "location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartyModels", "location", c => c.String());
            DropIndex("dbo.PartyModels", new[] { "location_uniq" });
            DropForeignKey("dbo.PartyModels", "location_uniq", "dbo.MapModels");
            DropColumn("dbo.PartyModels", "location_uniq");
            DropColumn("dbo.PlayerModels", "rootY");
            DropColumn("dbo.PlayerModels", "rootX");
            DropColumn("dbo.PlayerModels", "rootMap");
            DropTable("dbo.MapModels");
        }
    }
}
