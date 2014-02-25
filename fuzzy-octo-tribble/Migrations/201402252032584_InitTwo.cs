namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTwo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerItemModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        count = c.Int(nullable: false),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlayerItemModels", new[] { "PlayerModel_uniq" });
            DropForeignKey("dbo.PlayerItemModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropTable("dbo.PlayerItemModels");
        }
    }
}
