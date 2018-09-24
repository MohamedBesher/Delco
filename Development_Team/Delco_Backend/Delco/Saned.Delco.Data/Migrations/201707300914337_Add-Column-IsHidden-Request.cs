namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIsHiddenRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "IsHidden", c => c.Boolean(nullable: false));
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
                        IsHidden = p.Boolean(),
                        Type = p.Int(),
                        CreatedDate = p.DateTime(storeType: "datetime2"),
                        ApplicationUser_Id = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Requests]([Address], [FromLongtitude], [FromLatitude], [FromLocation], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [CityId], [UserId], [AgentId], [PassengerNumberId], [Description], [Status], [IsHidden], [Type], [CreatedDate], [ApplicationUser_Id])
                      VALUES (@Address, @FromLongtitude, @FromLatitude, @FromLocation, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @CityId, @UserId, @AgentId, @PassengerNumberId, @Description, @Status, @IsHidden, @Type, @CreatedDate, @ApplicationUser_Id)
                      
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
                        IsHidden = p.Boolean(),
                        Type = p.Int(),
                        CreatedDate = p.DateTime(storeType: "datetime2"),
                        ApplicationUser_Id = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Requests]
                      SET [Address] = @Address, [FromLongtitude] = @FromLongtitude, [FromLatitude] = @FromLatitude, [FromLocation] = @FromLocation, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [CityId] = @CityId, [UserId] = @UserId, [AgentId] = @AgentId, [PassengerNumberId] = @PassengerNumberId, [Description] = @Description, [Status] = @Status, [IsHidden] = @IsHidden, [Type] = @Type, [CreatedDate] = @CreatedDate, [ApplicationUser_Id] = @ApplicationUser_Id
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "IsHidden");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
