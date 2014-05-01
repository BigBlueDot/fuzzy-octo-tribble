namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalCanFleeUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CombatDataModels", "canFlee", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CombatDataModels", "canFlee");
        }
    }
}
