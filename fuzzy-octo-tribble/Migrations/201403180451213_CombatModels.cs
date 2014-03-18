namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombatModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CombatModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        currentTime = c.Int(nullable: false),
                        turnOrder_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.TurnOrderModels", t => t.turnOrder_uniq)
                .Index(t => t.turnOrder_uniq);
            
            CreateTable(
                "dbo.CombatNPCModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        enemyName = c.String(),
                        stats_uniq = c.Int(),
                        CombatModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.TemporaryCombatStatsModels", t => t.stats_uniq)
                .ForeignKey("dbo.CombatModels", t => t.CombatModel_uniq)
                .Index(t => t.stats_uniq)
                .Index(t => t.CombatModel_uniq);
            
            CreateTable(
                "dbo.CombatModificationsModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        CombatNPCModel_uniq = c.Int(),
                        CombatPCModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CombatNPCModels", t => t.CombatNPCModel_uniq)
                .ForeignKey("dbo.CombatPCModels", t => t.CombatPCModel_uniq)
                .Index(t => t.CombatNPCModel_uniq)
                .Index(t => t.CombatPCModel_uniq);
            
            CreateTable(
                "dbo.CombatConditionModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        state = c.String(),
                        CombatModificationsModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CombatModificationsModels", t => t.CombatModificationsModel_uniq)
                .Index(t => t.CombatModificationsModel_uniq);
            
            CreateTable(
                "dbo.TemporaryCombatStatsModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        hp = c.Int(nullable: false),
                        mp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CombatPCModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        characterUniq = c.Int(nullable: false),
                        stats_uniq = c.Int(),
                        CombatModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.TemporaryCombatStatsModels", t => t.stats_uniq)
                .ForeignKey("dbo.CombatModels", t => t.CombatModel_uniq)
                .Index(t => t.stats_uniq)
                .Index(t => t.CombatModel_uniq);
            
            CreateTable(
                "dbo.TurnOrderModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.TurnOrderCharacterModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        isPC = c.Boolean(nullable: false),
                        charUniq = c.Int(nullable: false),
                        time = c.Int(nullable: false),
                        TurnOrderModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.TurnOrderModels", t => t.TurnOrderModel_uniq)
                .Index(t => t.TurnOrderModel_uniq);
            
            AddColumn("dbo.UserProfile", "currentCombat_uniq", c => c.Int());
            CreateIndex("dbo.UserProfile", "currentCombat_uniq");
            AddForeignKey("dbo.UserProfile", "currentCombat_uniq", "dbo.CombatModels", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfile", "currentCombat_uniq", "dbo.CombatModels");
            DropForeignKey("dbo.CombatModels", "turnOrder_uniq", "dbo.TurnOrderModels");
            DropForeignKey("dbo.TurnOrderCharacterModels", "TurnOrderModel_uniq", "dbo.TurnOrderModels");
            DropForeignKey("dbo.CombatPCModels", "CombatModel_uniq", "dbo.CombatModels");
            DropForeignKey("dbo.CombatPCModels", "stats_uniq", "dbo.TemporaryCombatStatsModels");
            DropForeignKey("dbo.CombatModificationsModels", "CombatPCModel_uniq", "dbo.CombatPCModels");
            DropForeignKey("dbo.CombatNPCModels", "CombatModel_uniq", "dbo.CombatModels");
            DropForeignKey("dbo.CombatNPCModels", "stats_uniq", "dbo.TemporaryCombatStatsModels");
            DropForeignKey("dbo.CombatModificationsModels", "CombatNPCModel_uniq", "dbo.CombatNPCModels");
            DropForeignKey("dbo.CombatConditionModels", "CombatModificationsModel_uniq", "dbo.CombatModificationsModels");
            DropIndex("dbo.UserProfile", new[] { "currentCombat_uniq" });
            DropIndex("dbo.CombatModels", new[] { "turnOrder_uniq" });
            DropIndex("dbo.TurnOrderCharacterModels", new[] { "TurnOrderModel_uniq" });
            DropIndex("dbo.CombatPCModels", new[] { "CombatModel_uniq" });
            DropIndex("dbo.CombatPCModels", new[] { "stats_uniq" });
            DropIndex("dbo.CombatModificationsModels", new[] { "CombatPCModel_uniq" });
            DropIndex("dbo.CombatNPCModels", new[] { "CombatModel_uniq" });
            DropIndex("dbo.CombatNPCModels", new[] { "stats_uniq" });
            DropIndex("dbo.CombatModificationsModels", new[] { "CombatNPCModel_uniq" });
            DropIndex("dbo.CombatConditionModels", new[] { "CombatModificationsModel_uniq" });
            DropColumn("dbo.UserProfile", "currentCombat_uniq");
            DropTable("dbo.TurnOrderCharacterModels");
            DropTable("dbo.TurnOrderModels");
            DropTable("dbo.CombatPCModels");
            DropTable("dbo.TemporaryCombatStatsModels");
            DropTable("dbo.CombatConditionModels");
            DropTable("dbo.CombatModificationsModels");
            DropTable("dbo.CombatNPCModels");
            DropTable("dbo.CombatModels");
        }
    }
}
