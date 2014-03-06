namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartyUpdae : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerModels", "activeParty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlayerModels", "activeParty");
        }
    }
}
