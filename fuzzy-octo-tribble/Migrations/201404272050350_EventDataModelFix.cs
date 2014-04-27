namespace fuzzy_octo_tribble.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventDataModelFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Encounters",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        message = c.String(),
                    })
                .PrimaryKey(t => t.uniq);
            
            CreateTable(
                "dbo.Enemies",
                c => new
                    {
                        uniq = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        level = c.Int(nullable: false),
                        type = c.String(),
                        maxHP = c.Int(nullable: false),
                        maxMP = c.Int(nullable: false),
                        strength = c.Int(nullable: false),
                        vitality = c.Int(nullable: false),
                        intellect = c.Int(nullable: false),
                        wisdom = c.Int(nullable: false),
                        agility = c.Int(nullable: false),
                        xp = c.Int(nullable: false),
                        cp = c.Int(nullable: false),
                        Encounter_uniq = c.Int(),
                    })
                .PrimaryKey(t => t.uniq)
                .ForeignKey("dbo.Encounters", t => t.Encounter_uniq)
                .Index(t => t.Encounter_uniq);
            
            AddColumn("dbo.EventDataModels", "hasMessage", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventDataModels", "message", c => c.String());
            AddColumn("dbo.EventDataModels", "eventId", c => c.Int(nullable: false));
            AddColumn("dbo.EventDataModels", "type", c => c.Int(nullable: false));
            AddColumn("dbo.EventDataModels", "nextEvent_uniq", c => c.Int());
            AddColumn("dbo.EventDataModels", "encounter_uniq", c => c.Int());
            CreateIndex("dbo.EventDataModels", "nextEvent_uniq");
            CreateIndex("dbo.EventDataModels", "encounter_uniq");
            AddForeignKey("dbo.EventDataModels", "nextEvent_uniq", "dbo.EventDataModels", "uniq");
            AddForeignKey("dbo.EventDataModels", "encounter_uniq", "dbo.Encounters", "uniq");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventDataModels", "encounter_uniq", "dbo.Encounters");
            DropForeignKey("dbo.Enemies", "Encounter_uniq", "dbo.Encounters");
            DropForeignKey("dbo.EventDataModels", "nextEvent_uniq", "dbo.EventDataModels");
            DropIndex("dbo.EventDataModels", new[] { "encounter_uniq" });
            DropIndex("dbo.Enemies", new[] { "Encounter_uniq" });
            DropIndex("dbo.EventDataModels", new[] { "nextEvent_uniq" });
            DropColumn("dbo.EventDataModels", "encounter_uniq");
            DropColumn("dbo.EventDataModels", "nextEvent_uniq");
            DropColumn("dbo.EventDataModels", "type");
            DropColumn("dbo.EventDataModels", "eventId");
            DropColumn("dbo.EventDataModels", "message");
            DropColumn("dbo.EventDataModels", "hasMessage");
            DropTable("dbo.Enemies");
            DropTable("dbo.Encounters");
        }
    }
}
