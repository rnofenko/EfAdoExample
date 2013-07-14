namespace AdoExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Changed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.RiskGroups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RiskGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Payments", new[] { "ClientId" });
            DropIndex("dbo.Clients", new[] { "GroupId" });
            DropIndex("dbo.Clients", new[] { "CategoryId" });
            DropForeignKey("dbo.Payments", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "GroupId", "dbo.RiskGroups");
            DropForeignKey("dbo.Clients", "CategoryId", "dbo.Categories");
            DropTable("dbo.Payments");
            DropTable("dbo.RiskGroups");
            DropTable("dbo.Categories");
            DropTable("dbo.Clients");
        }
    }
}
