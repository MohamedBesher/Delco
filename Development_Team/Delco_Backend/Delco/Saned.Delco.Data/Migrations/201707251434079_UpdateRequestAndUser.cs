namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestAndUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "UserId" });
            AddColumn("dbo.AspNetUsers", "IsOnline", c => c.Boolean(nullable: false));
            AddColumn("dbo.Requests", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Requests", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "UserId");
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
            AddForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AlterStoredProcedure(
                "dbo.ApplicationUser_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        ConfirmedToken = p.String(),
                        ResetPasswordlToken = p.String(),
                        IsDeleted = p.Boolean(),
                        IsSuspend = p.Boolean(),
                        IsOnline = p.Boolean(),
                        FullName = p.String(),
                        PhotoUrl = p.String(),
                        Address = p.String(),
                        IsNotified = p.Boolean(),
                        SecondPhoneNumber = p.String(),
                        Type = p.Int(),
                        CityId = p.Long(),
                        Email = p.String(maxLength: 256),
                        EmailConfirmed = p.Boolean(),
                        PasswordHash = p.String(),
                        SecurityStamp = p.String(),
                        PhoneNumber = p.String(),
                        PhoneNumberConfirmed = p.Boolean(),
                        TwoFactorEnabled = p.Boolean(),
                        LockoutEndDateUtc = p.DateTime(storeType: "datetime2"),
                        LockoutEnabled = p.Boolean(),
                        AccessFailedCount = p.Int(),
                        UserName = p.String(maxLength: 256),
                    },
                body:
                    @"INSERT [dbo].[AspNetUsers]([Id], [ConfirmedToken], [ResetPasswordlToken], [IsDeleted], [IsSuspend], [IsOnline], [FullName], [PhotoUrl], [Address], [IsNotified], [SecondPhoneNumber], [Type], [CityId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                      VALUES (@Id, @ConfirmedToken, @ResetPasswordlToken, @IsDeleted, @IsSuspend, @IsOnline, @FullName, @PhotoUrl, @Address, @IsNotified, @SecondPhoneNumber, @Type, @CityId, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)"
            );
            
            AlterStoredProcedure(
                "dbo.ApplicationUser_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        ConfirmedToken = p.String(),
                        ResetPasswordlToken = p.String(),
                        IsDeleted = p.Boolean(),
                        IsSuspend = p.Boolean(),
                        IsOnline = p.Boolean(),
                        FullName = p.String(),
                        PhotoUrl = p.String(),
                        Address = p.String(),
                        IsNotified = p.Boolean(),
                        SecondPhoneNumber = p.String(),
                        Type = p.Int(),
                        CityId = p.Long(),
                        Email = p.String(maxLength: 256),
                        EmailConfirmed = p.Boolean(),
                        PasswordHash = p.String(),
                        SecurityStamp = p.String(),
                        PhoneNumber = p.String(),
                        PhoneNumberConfirmed = p.Boolean(),
                        TwoFactorEnabled = p.Boolean(),
                        LockoutEndDateUtc = p.DateTime(storeType: "datetime2"),
                        LockoutEnabled = p.Boolean(),
                        AccessFailedCount = p.Int(),
                        UserName = p.String(maxLength: 256),
                    },
                body:
                    @"UPDATE [dbo].[AspNetUsers]
                      SET [ConfirmedToken] = @ConfirmedToken, [ResetPasswordlToken] = @ResetPasswordlToken, [IsDeleted] = @IsDeleted, [IsSuspend] = @IsSuspend, [IsOnline] = @IsOnline, [FullName] = @FullName, [PhotoUrl] = @PhotoUrl, [Address] = @Address, [IsNotified] = @IsNotified, [SecondPhoneNumber] = @SecondPhoneNumber, [Type] = @Type, [CityId] = @CityId, [Email] = @Email, [EmailConfirmed] = @EmailConfirmed, [PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [PhoneNumber] = @PhoneNumber, [PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEndDateUtc] = @LockoutEndDateUtc, [LockoutEnabled] = @LockoutEnabled, [AccessFailedCount] = @AccessFailedCount, [UserName] = @UserName
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
                        CreatedDate = p.DateTime(storeType: "datetime2"),
                        ApplicationUser_Id = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Requests]([Address], [FromLongtitude], [FromLatitude], [FromLocation], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [CityId], [UserId], [AgentId], [PassengerNumberId], [Description], [Status], [Type], [CreatedDate], [ApplicationUser_Id])
                      VALUES (@Address, @FromLongtitude, @FromLatitude, @FromLocation, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @CityId, @UserId, @AgentId, @PassengerNumberId, @Description, @Status, @Type, @CreatedDate, @ApplicationUser_Id)
                      
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
                        ApplicationUser_Id = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Requests]
                      SET [Address] = @Address, [FromLongtitude] = @FromLongtitude, [FromLatitude] = @FromLatitude, [FromLocation] = @FromLocation, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [CityId] = @CityId, [UserId] = @UserId, [AgentId] = @AgentId, [PassengerNumberId] = @PassengerNumberId, [Description] = @Description, [Status] = @Status, [Type] = @Type, [CreatedDate] = @CreatedDate, [ApplicationUser_Id] = @ApplicationUser_Id
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Request_Delete",
                p => new
                    {
                        Id = p.Long(),
                        ApplicationUser_Id = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[Requests]
                      WHERE (([Id] = @Id) AND (([ApplicationUser_Id] = @ApplicationUser_Id) OR ([ApplicationUser_Id] IS NULL AND @ApplicationUser_Id IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Requests", new[] { "UserId" });
            AlterColumn("dbo.Requests", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Requests", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "IsOnline");
            CreateIndex("dbo.Requests", "UserId");
            AddForeignKey("dbo.Requests", "UserId", "dbo.AspNetUsers", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
