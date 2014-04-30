namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnnecessaryModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CharacterBattleCommandModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterCommandModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterCompletedQuestModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterPurchaseableModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterUnlockedClassModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropIndex("dbo.CharacterBattleCommandModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterCommandModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterCompletedQuestModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterPurchaseableModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterUnlockedClassModels", new[] { "PlayerModel_uniq" });
            DropTable("dbo.CharacterBattleCommandModels");
            DropTable("dbo.CharacterCommandModels");
            DropTable("dbo.CharacterCompletedQuestModels");
            DropTable("dbo.CharacterPurchaseableModels");
            DropTable("dbo.CharacterUnlockedClassModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CharacterUnlockedClassModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CharacterPurchaseableModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CharacterCompletedQuestModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CharacterCommandModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CharacterBattleCommandModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateIndex("dbo.CharacterUnlockedClassModels", "PlayerModel_uniq");
            CreateIndex("dbo.CharacterPurchaseableModels", "PlayerModel_uniq");
            CreateIndex("dbo.CharacterCompletedQuestModels", "PlayerModel_uniq");
            CreateIndex("dbo.CharacterCommandModels", "PlayerModel_uniq");
            CreateIndex("dbo.CharacterBattleCommandModels", "PlayerModel_uniq");
            AddForeignKey("dbo.CharacterUnlockedClassModels", "PlayerModel_uniq", "dbo.PlayerModels", "uniq");
            AddForeignKey("dbo.CharacterPurchaseableModels", "PlayerModel_uniq", "dbo.PlayerModels", "uniq");
            AddForeignKey("dbo.CharacterCompletedQuestModels", "PlayerModel_uniq", "dbo.PlayerModels", "uniq");
            AddForeignKey("dbo.CharacterCommandModels", "PlayerModel_uniq", "dbo.PlayerModels", "uniq");
            AddForeignKey("dbo.CharacterBattleCommandModels", "PlayerModel_uniq", "dbo.PlayerModels", "uniq");
        }
    }
}
