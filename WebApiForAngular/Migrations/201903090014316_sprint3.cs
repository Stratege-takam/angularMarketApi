namespace WebApiForAngular.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sprint3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Custumer_Id", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "Custumer_Id" });
            RenameColumn(table: "dbo.Orders", name: "Custumer_Id", newName: "CustomerId");
            AlterColumn("dbo.Orders", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "CustemerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CustemerId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            AlterColumn("dbo.Orders", "CustomerId", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "Custumer_Id");
            CreateIndex("dbo.Orders", "Custumer_Id");
            AddForeignKey("dbo.Orders", "Custumer_Id", "dbo.Customers", "Id");
        }
    }
}
