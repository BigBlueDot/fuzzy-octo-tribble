namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoubleAbility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CombatDataModels", "doubleSelectionState", c => c.Int(nullable: false));
            AddColumn("dbo.CombatDataModels", "doubleSelectionDelay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CombatDataModels", "doubleSelectionDelay");
            DropColumn("dbo.CombatDataModels", "doubleSelectionState");
        }
    }
}
