namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreFixCombatMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartyCharacterModels", "hp", c => c.Int(nullable: false));
            AddColumn("dbo.PartyCharacterModels", "mp", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartyCharacterModels", "mp");
            DropColumn("dbo.PartyCharacterModels", "hp");
        }
    }
}
