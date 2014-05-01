namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventFleeDisableUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Encounters", "canFlee", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Encounters", "canFlee");
        }
    }
}
