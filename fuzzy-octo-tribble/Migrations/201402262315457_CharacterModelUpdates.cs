namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharacterModelUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CharacterModels", "currentClass", c => c.String());
            AddColumn("dbo.CharacterModels", "lvl", c => c.Int(nullable: false));
            AddColumn("dbo.CharacterModels", "xp", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CharacterModels", "xp");
            DropColumn("dbo.CharacterModels", "lvl");
            DropColumn("dbo.CharacterModels", "currentClass");
        }
    }
}
