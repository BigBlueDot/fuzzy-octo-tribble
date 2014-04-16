namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombatUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TurnOrderCharacterModels", "TurnOrderModel_uniq", "dbo.TurnOrderModels");
            DropForeignKey("dbo.CombatModels", "turnOrder_uniq", "dbo.TurnOrderModels");
            DropIndex("dbo.TurnOrderCharacterModels", new[] { "TurnOrderModel_uniq" });
            DropIndex("dbo.CombatModels", new[] { "turnOrder_uniq" });
            AddColumn("dbo.CombatNPCModels", "nextAttackTime", c => c.Int(nullable: false));
            AddColumn("dbo.CombatNPCModels", "combatUniq", c => c.Int(nullable: false));
            AddColumn("dbo.CombatPCModels", "nextAttackTime", c => c.Int(nullable: false));
            AddColumn("dbo.CombatPCModels", "combatUniq", c => c.Int(nullable: false));
            DropColumn("dbo.CombatModels", "turnOrder_uniq");
            DropTable("dbo.TurnOrderModels");
            DropTable("dbo.TurnOrderCharacterModels");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.TurnOrderModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.uniq);
            
            AddColumn("dbo.CombatModels", "turnOrder_uniq", c => c.Int());
            DropColumn("dbo.CombatPCModels", "combatUniq");
            DropColumn("dbo.CombatPCModels", "nextAttackTime");
            DropColumn("dbo.CombatNPCModels", "combatUniq");
            DropColumn("dbo.CombatNPCModels", "nextAttackTime");
            CreateIndex("dbo.CombatModels", "turnOrder_uniq");
            CreateIndex("dbo.TurnOrderCharacterModels", "TurnOrderModel_uniq");
            AddForeignKey("dbo.CombatModels", "turnOrder_uniq", "dbo.TurnOrderModels", "uniq");
            AddForeignKey("dbo.TurnOrderCharacterModels", "TurnOrderModel_uniq", "dbo.TurnOrderModels", "uniq");
        }
    }
}
