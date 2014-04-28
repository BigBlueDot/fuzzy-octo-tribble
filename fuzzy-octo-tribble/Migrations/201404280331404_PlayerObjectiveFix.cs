namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerObjectiveFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerObjectiveModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerObjectiveModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropIndex("dbo.PlayerObjectiveModels", new[] { "PlayerModel_uniq" });
            DropTable("dbo.PlayerObjectiveModels");
        }
    }
}
