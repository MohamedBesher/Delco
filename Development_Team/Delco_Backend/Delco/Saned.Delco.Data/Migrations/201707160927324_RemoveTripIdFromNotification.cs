namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTripIdFromNotification : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NotificationMessages", "TripId");
            AlterStoredProcedure(
                "dbo.NotificationMessage_Insert",
                p => new
                    {
                        Message = p.String(maxLength: 500),
                        EnglishMessage = p.String(),
                        CreationDate = p.DateTime(storeType: "datetime2"),
                        RequestId = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[NotificationMessages]([Message], [EnglishMessage], [CreationDate], [RequestId])
                      VALUES (@Message, @EnglishMessage, @CreationDate, @RequestId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[NotificationMessages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[NotificationMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.NotificationMessage_Update",
                p => new
                    {
                        Id = p.Long(),
                        Message = p.String(maxLength: 500),
                        EnglishMessage = p.String(),
                        CreationDate = p.DateTime(storeType: "datetime2"),
                        RequestId = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[NotificationMessages]
                      SET [Message] = @Message, [EnglishMessage] = @EnglishMessage, [CreationDate] = @CreationDate, [RequestId] = @RequestId
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.NotificationMessages", "TripId", c => c.Long());
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
