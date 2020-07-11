namespace Proyecto_PAA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthorId : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Budgets", "AuthorId");
            AddForeignKey("dbo.Budgets", "AuthorId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Budgets", "AuthorId", "dbo.Users");
            DropIndex("dbo.Budgets", new[] { "AuthorId" });
        }
    }
}
