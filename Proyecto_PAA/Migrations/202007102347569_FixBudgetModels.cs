namespace Proyecto_PAA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixBudgetModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Budgets", "ProductId", "dbo.Products");
            DropIndex("dbo.Budgets", new[] { "ProductId" });
            CreateTable(
                "dbo.BudgetProducts",
                c => new
                    {
                        BudgetProductId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetProductId)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BudgetId);
            
            AddColumn("dbo.Budgets", "AuthorId", c => c.Int(nullable: false));
            AddColumn("dbo.Budgets", "BudgetState", c => c.Int(nullable: false));
            AddColumn("dbo.Budgets", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Budgets", "DeletedAt", c => c.DateTime());
            DropColumn("dbo.Budgets", "ProductId");
            DropColumn("dbo.Budgets", "UpdateAt");
            DropColumn("dbo.Budgets", "Price");
            DropColumn("dbo.Budgets", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Budgets", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Budgets", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.Budgets", "UpdateAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Budgets", "ProductId", c => c.Int(nullable: false));
            DropForeignKey("dbo.BudgetProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BudgetProducts", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.BudgetProducts", new[] { "BudgetId" });
            DropIndex("dbo.BudgetProducts", new[] { "ProductId" });
            DropColumn("dbo.Budgets", "DeletedAt");
            DropColumn("dbo.Budgets", "UpdatedAt");
            DropColumn("dbo.Budgets", "BudgetState");
            DropColumn("dbo.Budgets", "AuthorId");
            DropTable("dbo.BudgetProducts");
            CreateIndex("dbo.Budgets", "ProductId");
            AddForeignKey("dbo.Budgets", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
    }
}
