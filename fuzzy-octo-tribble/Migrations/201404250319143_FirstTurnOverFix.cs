namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstTurnOverFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TurnOverModels",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        CombatDataModel_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.CombatDataModels", t => t.CombatDataModel_uniq)
                .Index(t => t.CombatDataModel_uniq);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TurnOverModels", "CombatDataModel_uniq", "dbo.CombatDataModels");
            DropIndex("dbo.TurnOverModels", new[] { "CombatDataModel_uniq" });
            DropTable("dbo.TurnOverModels");
        }
    }
}
