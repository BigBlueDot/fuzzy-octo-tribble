namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombatDataEventModelFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventDataModels", "encounter_uniq", "dbo.Encounters");
            DropIndex("dbo.EventDataModels", new[] { "encounter_uniq" });
            CreateIndex("dbo.EventDataModels", "encounter_uniq");
            AddForeignKey("dbo.EventDataModels", "encounter_uniq", "dbo.Encounters", "uniq");
            DropColumn("dbo.EventDataModels", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventDataModels", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.EventDataModels", "encounter_uniq", "dbo.Encounters");
            DropIndex("dbo.EventDataModels", new[] { "encounter_uniq" });
            CreateIndex("dbo.EventDataModels", "encounter_uniq");
            AddForeignKey("dbo.EventDataModels", "encounter_uniq", "dbo.Encounters", "uniq");
        }
    }
}
