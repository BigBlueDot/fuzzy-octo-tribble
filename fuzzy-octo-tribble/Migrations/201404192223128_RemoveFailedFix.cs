namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFailedFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CombatModificationsModels", "afflictedName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CombatModificationsModels", "afflictedName", c => c.String());
        }
    }
}
