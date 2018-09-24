namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolIsreadNotificationDevices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotificationDevices", "IsRead", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.NotificationDevice_Insert",
                p => new
                    {
                        NotificationMessageId = p.Long(),
                        ApplicationUserId = p.String(maxLength: 128),
                        IsRead = p.Boolean(),
                        Type = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[NotificationDevices]([NotificationMessageId], [ApplicationUserId], [IsRead], [Type])
                      VALUES (@NotificationMessageId, @ApplicationUserId, @IsRead, @Type)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[NotificationDevices]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[NotificationDevices] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.NotificationDevice_Update",
                p => new
                    {
                        Id = p.Long(),
                        NotificationMessageId = p.Long(),
                        ApplicationUserId = p.String(maxLength: 128),
                        IsRead = p.Boolean(),
                        Type = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[NotificationDevices]
                      SET [NotificationMessageId] = @NotificationMessageId, [ApplicationUserId] = @ApplicationUserId, [IsRead] = @IsRead, [Type] = @Type
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotificationDevices", "IsRead");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
