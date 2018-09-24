namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePassengerNumberTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PassengerNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Cars", "PassengerNumberId", c => c.Long(nullable: false));
            AddColumn("dbo.Requests", "PassengerNumberId", c => c.Long());
            CreateIndex("dbo.Cars", "PassengerNumberId");
            CreateIndex("dbo.Requests", "PassengerNumberId");
            AddForeignKey("dbo.Cars", "PassengerNumberId", "dbo.PassengerNumbers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "PassengerNumberId", "dbo.PassengerNumbers", "Id");
            DropColumn("dbo.Cars", "PassengerCount");
            DropColumn("dbo.Requests", "PassengerNumber");
            CreateStoredProcedure(
                "dbo.PassengerNumber_Insert",
                p => new
                    {
                        Value = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[PassengerNumbers]([Value])
                      VALUES (@Value)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[PassengerNumbers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PassengerNumbers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PassengerNumber_Update",
                p => new
                    {
                        Id = p.Long(),
                        Value = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[PassengerNumbers]
                      SET [Value] = @Value
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PassengerNumber_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[PassengerNumbers]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Car_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        CompanyName = p.String(maxLength: 250),
                        Type = p.String(maxLength: 250),
                        Model = p.String(maxLength: 250),
                        Color = p.String(maxLength: 250),
                        PlateNumber = p.String(maxLength: 250),
                        PassengerNumberId = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[Cars]([Id], [CompanyName], [Type], [Model], [Color], [PlateNumber], [PassengerNumberId])
                      VALUES (@Id, @CompanyName, @Type, @Model, @Color, @PlateNumber, @PassengerNumberId)"
            );
            
            AlterStoredProcedure(
                "dbo.Car_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        CompanyName = p.String(maxLength: 250),
                        Type = p.String(maxLength: 250),
                        Model = p.String(maxLength: 250),
                        Color = p.String(maxLength: 250),
                        PlateNumber = p.String(maxLength: 250),
                        PassengerNumberId = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[Cars]
                      SET [CompanyName] = @CompanyName, [Type] = @Type, [Model] = @Model, [Color] = @Color, [PlateNumber] = @PlateNumber, [PassengerNumberId] = @PassengerNumberId
                      WHERE ([Id] = @Id)"
            );
            
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
                    },
                body:
                    @"INSERT [dbo].[Requests]([Address], [FromLongtitude], [FromLatitude], [FromLocation], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [CityId], [UserId], [AgentId], [PassengerNumberId], [Description], [Status], [Type])
                      VALUES (@Address, @FromLongtitude, @FromLatitude, @FromLocation, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @CityId, @UserId, @AgentId, @PassengerNumberId, @Description, @Status, @Type)
                      
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
                    },
                body:
                    @"UPDATE [dbo].[Requests]
                      SET [Address] = @Address, [FromLongtitude] = @FromLongtitude, [FromLatitude] = @FromLatitude, [FromLocation] = @FromLocation, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [CityId] = @CityId, [UserId] = @UserId, [AgentId] = @AgentId, [PassengerNumberId] = @PassengerNumberId, [Description] = @Description, [Status] = @Status, [Type] = @Type
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.PassengerNumber_Delete");
            DropStoredProcedure("dbo.PassengerNumber_Update");
            DropStoredProcedure("dbo.PassengerNumber_Insert");
            AddColumn("dbo.Requests", "PassengerNumber", c => c.Int());
            AddColumn("dbo.Cars", "PassengerCount", c => c.Int(nullable: false));
            DropForeignKey("dbo.Requests", "PassengerNumberId", "dbo.PassengerNumbers");
            DropForeignKey("dbo.Cars", "PassengerNumberId", "dbo.PassengerNumbers");
            DropIndex("dbo.Requests", new[] { "PassengerNumberId" });
            DropIndex("dbo.Cars", new[] { "PassengerNumberId" });
            DropColumn("dbo.Requests", "PassengerNumberId");
            DropColumn("dbo.Cars", "PassengerNumberId");
            DropTable("dbo.PassengerNumbers");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
