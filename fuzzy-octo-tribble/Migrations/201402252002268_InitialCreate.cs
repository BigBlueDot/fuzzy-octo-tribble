namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        gp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CharacterModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        stats_uniq = c.Int(),
                        equipment_uniq = c.Int(),
                        currentQuest_uniq = c.Int(),
                        PlayerModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.StatsModels", t => t.stats_uniq)
                .ForeignKey("dbo.EquipmentModels", t => t.equipment_uniq)
                .ForeignKey("dbo.CharacterQuestModels", t => t.currentQuest_uniq)
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_uniq)
                .Index(t => t.stats_uniq)
                .Index(t => t.equipment_uniq)
                .Index(t => t.currentQuest_uniq)
                .Index(t => t.PlayerModel_uniq);
            
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
                "dbo.PartyModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        maxSize = c.Int(nullable: false),
                        location = c.String(),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ConfigurationModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.PartyModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterClassModels", new[] { "CharacterModel_uniq" });
            DropIndex("dbo.CharacterQuestProgressModels", new[] { "CharacterQuestModel_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "PlayerModel_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "currentQuest_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "equipment_uniq" });
            DropIndex("dbo.CharacterModels", new[] { "stats_uniq" });
            DropIndex("dbo.UserProfile", new[] { "player_uniq" });
            DropForeignKey("dbo.ConfigurationModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.PartyModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterClassModels", "CharacterModel_uniq", "dbo.CharacterModels");
            DropForeignKey("dbo.CharacterQuestProgressModels", "CharacterQuestModel_uniq", "dbo.CharacterQuestModels");
            DropForeignKey("dbo.CharacterModels", "PlayerModel_uniq", "dbo.PlayerModels");
            DropForeignKey("dbo.CharacterModels", "currentQuest_uniq", "dbo.CharacterQuestModels");
            DropForeignKey("dbo.CharacterModels", "equipment_uniq", "dbo.EquipmentModels");
            DropForeignKey("dbo.CharacterModels", "stats_uniq", "dbo.StatsModels");
            DropForeignKey("dbo.UserProfile", "player_uniq", "dbo.PlayerModels");
            DropTable("dbo.ConfigurationModels");
            DropTable("dbo.PartyModels");
            DropTable("dbo.CharacterClassModels");
            DropTable("dbo.CharacterQuestProgressModels");
            DropTable("dbo.CharacterQuestModels");
            DropTable("dbo.EquipmentModels");
            DropTable("dbo.StatsModels");
            DropTable("dbo.CharacterModels");
            DropTable("dbo.PlayerModels");
            DropTable("dbo.UserProfile");
        }
    }
}
