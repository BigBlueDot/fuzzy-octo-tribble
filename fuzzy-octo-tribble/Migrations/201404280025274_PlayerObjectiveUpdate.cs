namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerObjectiveUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventDataModels", "objective", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventDataModels", "objective");
        }
    }
}
