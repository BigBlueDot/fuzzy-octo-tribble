namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        player_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.PlayerModels", t => t.player_uniq)
                .Index(t => t.player_uniq);
            
            CreateTable(
                "dbo.PlayerModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        rootMap = c.String(),
                        rootX = c.Int(nullable: false),
                        rootY = c.Int(nullable: false),
                        gp = c.Int(nullable: false),
                        activeParty = c.Int(nullable: false),
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
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
            CreateTable(
                "dbo.CharacterModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        currentClass = c.String(),
                        lvl = c.Int(nullable: false),
                        xp = c.Int(nullable: false),
                        currentQuest_uniq = c.Int(),
                        equipment_uniq = c.Int(),
                        stats_uniq = c.Int(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CharacterQuestModels", t => t.currentQuest_uniq)
                .ForeignKey("dbo.EquipmentModels", t => t.equipment_uniq)
                .ForeignKey("dbo.StatsModels", t => t.stats_uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.currentQuest_uniq)
                .Index(t => t.equipment_uniq)
                .Index(t => t.stats_uniq)
                .Index(t => t.PlayerModel_uniq);
            
            CreateTable(
                "dbo.CharacterAbilityModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        CharacterModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CharacterModels", t => t.CharacterModel_uniq)
                .Index(t => t.CharacterModel_uniq);
            
            CreateTable(
                "dbo.CharacterClassModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        className = c.String(),
                        cp = c.Int(nullable: false),
                        lvl = c.Int(nullable: false),
                        CharacterModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CharacterModels", t => t.CharacterModel_uniq)
                .Index(t => t.CharacterModel_uniq);
            
            CreateTable(
                "dbo.CharacterQuestModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        questName = c.String(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CharacterQuestProgressModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        questName = c.String(),
                        questProgressId = c.Int(nullable: false),
                        currentProgress = c.Int(nullable: false),
                        goal = c.Int(nullable: false),
                        CharacterQuestModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CharacterQuestModels", t => t.CharacterQuestModel_uniq)
                .Index(t => t.CharacterQuestModel_uniq);
            
            CreateTable(
                "dbo.EquipmentModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        weapon = c.String(),
                        armor = c.String(),
                        accessory = c.String(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.StatsModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        maxHP = c.Int(nullable: false),
                        maxMP = c.Int(nullable: false),
                        strength = c.Int(nullable: false),
                        vitality = c.Int(nullable: false),
                        intellect = c.Int(nullable: false),
                        wisdom = c.Int(nullable: false),
                        agility = c.Int(nullable: false),
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
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
            CreateTable(
                "dbo.CharacterCompletedQuestModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
            CreateTable(
                "dbo.ConfigurationModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        configName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
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
            
            CreateTable(
                "dbo.PartyModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        maxSize = c.Int(nullable: false),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        location_uniq = c.Int(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.MapModels", t => t.location_uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.location_uniq)
                .Index(t => t.PlayerModel_uniq);
            
            CreateTable(
                "dbo.PartyCharacterModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        characterUniq = c.Int(nullable: false),
                        PartyModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PartyModels", t => t.PartyModel_uniq)
                .Index(t => t.PartyModel_uniq);
            
            CreateTable(
                "dbo.MapModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        startX = c.Int(nullable: false),
                        startY = c.Int(nullable: false),
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
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
            CreateTable(
                "dbo.CharacterUnlockedClassModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.PlayerModel_uniq);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfile", "player_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterUnlockedClassModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterPurchaseableModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.PartyModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.PartyModels", "location_uniq", "dbo.MapModels");
            DropForeignKey("dbo.PartyCharacterModels", "PartyModel_uniq", "dbo.PartyModels");
            DropForeignKey("dbo.PlayerItemModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.ConfigurationModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterCompletedQuestModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterCommandModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterModels", "stats_uniq", "dbo.StatsModels");
            DropForeignKey("dbo.CharacterModels", "equipment_uniq", "dbo.EquipmentModels");
            DropForeignKey("dbo.CharacterModels", "currentQuest_uniq", "dbo.CharacterQuestModels");
            DropForeignKey("dbo.CharacterQuestProgressModels", "CharacterQuestModel_uniq", "dbo.CharacterQuestModels");
            DropForeignKey("dbo.CharacterClassModels", "CharacterModel_uniq", "dbo.CharacterModels");
            DropForeignKey("dbo.CharacterAbilityModels", "CharacterModel_uniq", "dbo.CharacterModels");
            DropForeignKey("dbo.CharacterBattleCommandModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropIndex("dbo.UserProfile", new[] { "player_uniq" });
            DropIndex("dbo.CharacterUnlockedClassModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterPurchaseableModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.PartyModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.PartyModels", new[] { "location_uniq" });
            DropIndex("dbo.PartyCharacterModels", new[] { "PartyModel_uniq" });
            DropIndex("dbo.PlayerItemModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.ConfigurationModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterCompletedQuestModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterCommandModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "stats_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "equipment_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "currentQuest_uniq" });
            DropIndex("dbo.CharacterQuestProgressModels", new[] { "CharacterQuestModel_uniq" });
            DropIndex("dbo.CharacterClassModels", new[] { "CharacterModel_uniq" });
            DropIndex("dbo.CharacterAbilityModels", new[] { "CharacterModel_uniq" });
            DropIndex("dbo.CharacterBattleCommandModels", new[] { "PlayerModel_uniq" });
            DropTable("dbo.CharacterUnlockedClassModels");
            DropTable("dbo.CharacterPurchaseableModels");
            DropTable("dbo.MapModels");
            DropTable("dbo.PartyCharacterModels");
            DropTable("dbo.PartyModels");
            DropTable("dbo.PlayerItemModels");
            DropTable("dbo.ConfigurationModels");
            DropTable("dbo.CharacterCompletedQuestModels");
            DropTable("dbo.CharacterCommandModels");
            DropTable("dbo.StatsModels");
            DropTable("dbo.EquipmentModels");
            DropTable("dbo.CharacterQuestProgressModels");
            DropTable("dbo.CharacterQuestModels");
            DropTable("dbo.CharacterClassModels");
            DropTable("dbo.CharacterAbilityModels");
            DropTable("dbo.CharacterModels");
            DropTable("dbo.CharacterBattleCommandModels");
            DropTable("dbo.PlayerModels");
            DropTable("dbo.UserProfile");
        }
    }
}
