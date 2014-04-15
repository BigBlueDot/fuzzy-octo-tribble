namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombatUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "currentCombat_uniq", "dbo.CombatModels");
            DropIndex("dbo.UserProfile", new[] { "currentCombat_uniq" });
            AddColumn("dbo.PlayerModels", "currentCombat_uniq", c => c.Int());
            CreateIndex("dbo.PlayerModels", "currentCombat_uniq");
            AddForeignKey("dbo.PlayerModels", "currentCombat_uniq", "dbo.CombatModels", "uniq");
            DropColumn("dbo.UserProfile", "currentCombat_uniq");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "currentCombat_uniq", c => c.Int());
            DropForeignKey("dbo.PlayerModels", "currentCombat_uniq", "dbo.CombatModels");
            DropIndex("dbo.PlayerModels", new[] { "currentCombat_uniq" });
            DropColumn("dbo.PlayerModels", "currentCombat_uniq");
            CreateIndex("dbo.UserProfile", "currentCombat_uniq");
            AddForeignKey("dbo.UserProfile", "currentCombat_uniq", "dbo.CombatModels", "uniq");
        }
    }
}
