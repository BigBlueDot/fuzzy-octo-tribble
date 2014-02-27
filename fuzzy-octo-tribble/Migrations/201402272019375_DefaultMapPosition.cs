namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultMapPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MapModels", "startX", c => c.Int(nullable: false));
            AddColumn("dbo.MapModels", "startY", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MapModels", "startY");
            DropColumn("dbo.MapModels", "startX");
        }
    }
}
