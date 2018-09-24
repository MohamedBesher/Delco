namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergTripAndRequestTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Trips", "AgentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trips", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Trips", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationMessages", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Requests", "CityId", "dbo.Cities");
            DropIndex("dbo.Requests", new[] { "CityId" });
            DropIndex("dbo.Trips", new[] { "CityId" });
            DropIndex("dbo.Trips", new[] { "UserId" });
            DropIndex("dbo.Trips", new[] { "AgentId" });
            DropIndex("dbo.NotificationMessages", new[] { "TripId" });
            AddColumn("dbo.Requests", "FromLongtitude", c => c.String());
            AddColumn("dbo.Requests", "FromLatitude", c => c.String());
            AddColumn("dbo.Requests", "FromLocation", c => c.String());
            AddColumn("dbo.Requests", "PassengerNumber", c => c.Int());
            AddColumn("dbo.Requests", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "CityId", c => c.Long());
            CreateIndex("dbo.Requests", "CityId");
            AddForeignKey("dbo.Requests", "CityId", "dbo.Cities", "Id");
            DropTable("dbo.Trips");
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
                        PassengerNumber = p.Int(),
                        Status = p.Int(),
                        Type = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Requests]([Address], [FromLongtitude], [FromLatitude], [FromLocation], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [CityId], [UserId], [AgentId], [PassengerNumber], [Status], [Type])
                      VALUES (@Address, @FromLongtitude, @FromLatitude, @FromLocation, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @CityId, @UserId, @AgentId, @PassengerNumber, @Status, @Type)
                      
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
                        PassengerNumber = p.Int(),
                        Status = p.Int(),
                        Type = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Requests]
                      SET [Address] = @Address, [FromLongtitude] = @FromLongtitude, [FromLatitude] = @FromLatitude, [FromLocation] = @FromLocation, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [CityId] = @CityId, [UserId] = @UserId, [AgentId] = @AgentId, [PassengerNumber] = @PassengerNumber, [Status] = @Status, [Type] = @Type
                      WHERE ([Id] = @Id)"
            );
            
            DropStoredProcedure("dbo.Trip_Insert");
            DropStoredProcedure("dbo.Trip_Update");
            DropStoredProcedure("dbo.Trip_Delete");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FromLongtitude = c.String(),
                        FromLatitude = c.String(),
                        FromLocation = c.String(),
                        ToLongtitude = c.String(nullable: false),
                        ToLatitude = c.String(nullable: false),
                        ToLocation = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PassengerNumber = c.Int(nullable: false),
                        CityId = c.Long(nullable: false),
                        Address = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AgentId = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Requests", "CityId", "dbo.Cities");
            DropIndex("dbo.Requests", new[] { "CityId" });
            AlterColumn("dbo.Requests", "CityId", c => c.Long(nullable: false));
            DropColumn("dbo.Requests", "Type");
            DropColumn("dbo.Requests", "PassengerNumber");
            DropColumn("dbo.Requests", "FromLocation");
            DropColumn("dbo.Requests", "FromLatitude");
            DropColumn("dbo.Requests", "FromLongtitude");
            CreateIndex("dbo.NotificationMessages", "TripId");
            CreateIndex("dbo.Trips", "AgentId");
            CreateIndex("dbo.Trips", "UserId");
            CreateIndex("dbo.Trips", "CityId");
            CreateIndex("dbo.Requests", "CityId");
            AddForeignKey("dbo.Requests", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotificationMessages", "TripId", "dbo.Trips", "Id");
            AddForeignKey("dbo.Trips", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Trips", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Trips", "AgentId", "dbo.AspNetUsers", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
