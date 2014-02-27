namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CharacterName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CharacterModels", "name", c => c.String(defaultValue:"Scott Pilgrim"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CharacterModels", "name");
        }
    }
}
