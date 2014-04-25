namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombatDataStore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CombatDataModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        currentFleeCount = c.Int(nullable: false),
                        combatInitalized = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.CooldownModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        character = c.String(),
                        name = c.String(),
                        time = c.Int(nullable: false),
                        CombatDataModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CombatDataModels", t => t.CombatDataModel_uniq)
                .Index(t => t.CombatDataModel_uniq);
            
            AddColumn("dbo.CombatModels", "combatData_uniq", c => c.Int());
            CreateIndex("dbo.CombatModels", "combatData_uniq");
            AddForeignKey("dbo.CombatModels", "combatData_uniq", "dbo.CombatDataModels", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CombatModels", "combatData_uniq", "dbo.CombatDataModels");
            DropForeignKey("dbo.CooldownModels", "CombatDataModel_uniq", "dbo.CombatDataModels");
            DropIndex("dbo.CombatModels", new[] { "combatData_uniq" });
            DropIndex("dbo.CooldownModels", new[] { "CombatDataModel_uniq" });
            DropColumn("dbo.CombatModels", "combatData_uniq");
            DropTable("dbo.CooldownModels");
            DropTable("dbo.CombatDataModels");
        }
    }
}
