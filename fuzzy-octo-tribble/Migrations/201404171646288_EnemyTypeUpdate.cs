namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnemyTypeUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CombatCharacterModels", "classType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CombatCharacterModels", "classType");
        }
    }
}
