namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteAspNetUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Abuses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RefuseRequests", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.Abuses", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RefuseRequests", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefuseRequests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Abuses", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.RefuseRequests", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Abuses", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
