namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDeleteRequestsNotificationMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NotificationMessages", "RequestId", "dbo.Requests");
            DropIndex("dbo.NotificationMessages", new[] { "RequestId" });
            AlterColumn("dbo.NotificationMessages", "RequestId", c => c.Long(nullable: false));
            CreateIndex("dbo.NotificationMessages", "RequestId");
            AddForeignKey("dbo.NotificationMessages", "RequestId", "dbo.Requests", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationMessages", "RequestId", "dbo.Requests");
            DropIndex("dbo.NotificationMessages", new[] { "RequestId" });
            AlterColumn("dbo.NotificationMessages", "RequestId", c => c.Long());
            CreateIndex("dbo.NotificationMessages", "RequestId");
            AddForeignKey("dbo.NotificationMessages", "RequestId", "dbo.Requests", "Id");
        }
    }
}
