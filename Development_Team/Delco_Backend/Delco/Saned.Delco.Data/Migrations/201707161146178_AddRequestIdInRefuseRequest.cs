namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestIdInRefuseRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.RefuseRequests", "RequestId", c => c.Long(nullable: false));
            CreateIndex("dbo.RefuseRequests", "RequestId");
            AddForeignKey("dbo.RefuseRequests", "RequestId", "dbo.Requests", "Id", cascadeDelete: true);
            AlterStoredProcedure(
                "dbo.Request_Insert",
                p => new
                    {
                        Address = p.String(),
                        FromLongtitude = p.String(),
                        FromLatitude = p.String(),
                        FromLocation = p.String(),
                        ToLongtitude = p.String(),
                        ToLatitude = p.String(),
                        ToLocation = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        CityId = p.Long(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        PassengerNumberId = p.Long(),
                        Description = p.String(),
                        Status = p.Int(),
                        Type = p.Int(),
                        CreatedDate = p.DateTime(storeType: "datetime2"),
                    },
                body:
                    @"INSERT [dbo].[Requests]([Address], [FromLongtitude], [FromLatitude], [FromLocation], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [CityId], [UserId], [AgentId], [PassengerNumberId], [Description], [Status], [Type], [CreatedDate])
                      VALUES (@Address, @FromLongtitude, @FromLatitude, @FromLocation, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @CityId, @UserId, @AgentId, @PassengerNumberId, @Description, @Status, @Type, @CreatedDate)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Requests]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Requests] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Request_Update",
                p => new
                    {
                        Id = p.Long(),
                        Address = p.String(),
                        FromLongtitude = p.String(),
                        FromLatitude = p.String(),
                        FromLocation = p.String(),
                        ToLongtitude = p.String(),
                        ToLatitude = p.String(),
                        ToLocation = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        CityId = p.Long(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        PassengerNumberId = p.Long(),
                        Description = p.String(),
                        Status = p.Int(),
                        Type = p.Int(),
                        CreatedDate = p.DateTime(storeType: "datetime2"),
                    },
                body:
                    @"UPDATE [dbo].[Requests]
                      SET [Address] = @Address, [FromLongtitude] = @FromLongtitude, [FromLatitude] = @FromLatitude, [FromLocation] = @FromLocation, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [CityId] = @CityId, [UserId] = @UserId, [AgentId] = @AgentId, [PassengerNumberId] = @PassengerNumberId, [Description] = @Description, [Status] = @Status, [Type] = @Type, [CreatedDate] = @CreatedDate
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RefuseRequest_Insert",
                p => new
                    {
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        RefuseReasonId = p.Long(),
                        RequestId = p.Long(),
                        Cause = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RefuseRequests]([UserId], [AgentId], [RefuseReasonId], [RequestId], [Cause])
                      VALUES (@UserId, @AgentId, @RefuseReasonId, @RequestId, @Cause)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[RefuseRequests]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RefuseRequests] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RefuseRequest_Update",
                p => new
                    {
                        Id = p.Long(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        RefuseReasonId = p.Long(),
                        RequestId = p.Long(),
                        Cause = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RefuseRequests]
                      SET [UserId] = @UserId, [AgentId] = @AgentId, [RefuseReasonId] = @RefuseReasonId, [RequestId] = @RequestId, [Cause] = @Cause
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefuseRequests", "RequestId", "dbo.Requests");
            DropIndex("dbo.RefuseRequests", new[] { "RequestId" });
            DropColumn("dbo.RefuseRequests", "RequestId");
            DropColumn("dbo.Requests", "CreatedDate");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
