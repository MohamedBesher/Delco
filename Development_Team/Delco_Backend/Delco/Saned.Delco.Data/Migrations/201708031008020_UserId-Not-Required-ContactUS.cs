namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdNotRequiredContactUS : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ContactUs", new[] { "UserId" });
            AlterColumn("dbo.ContactUs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ContactUs", "UserId");
            AddForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ContactUs", new[] { "UserId" });
            AlterColumn("dbo.ContactUs", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ContactUs", "UserId");
            AddForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
