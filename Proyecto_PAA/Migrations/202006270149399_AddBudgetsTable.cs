namespace Proyecto_PAA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBudgetsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdateAt = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Budgets", "ProductId", "dbo.Products");
            DropIndex("dbo.Budgets", new[] { "ProductId" });
            DropTable("dbo.Budgets");
        }
    }
}
