namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequestIsSeenApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RequestIsSeen", c => c.Boolean(nullable: false));
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
                        RequestIsSeen = p.Boolean(),
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
                    @"INSERT [dbo].[AspNetUsers]([Id], [ConfirmedToken], [ResetPasswordlToken], [IsDeleted], [IsSuspend], [IsOnline], [FullName], [PhotoUrl], [Address], [IsNotified], [RequestIsSeen], [SecondPhoneNumber], [Type], [CityId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                      VALUES (@Id, @ConfirmedToken, @ResetPasswordlToken, @IsDeleted, @IsSuspend, @IsOnline, @FullName, @PhotoUrl, @Address, @IsNotified, @RequestIsSeen, @SecondPhoneNumber, @Type, @CityId, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)"
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
                        RequestIsSeen = p.Boolean(),
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
                      SET [ConfirmedToken] = @ConfirmedToken, [ResetPasswordlToken] = @ResetPasswordlToken, [IsDeleted] = @IsDeleted, [IsSuspend] = @IsSuspend, [IsOnline] = @IsOnline, [FullName] = @FullName, [PhotoUrl] = @PhotoUrl, [Address] = @Address, [IsNotified] = @IsNotified, [RequestIsSeen] = @RequestIsSeen, [SecondPhoneNumber] = @SecondPhoneNumber, [Type] = @Type, [CityId] = @CityId, [Email] = @Email, [EmailConfirmed] = @EmailConfirmed, [PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [PhoneNumber] = @PhoneNumber, [PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEndDateUtc] = @LockoutEndDateUtc, [LockoutEnabled] = @LockoutEnabled, [AccessFailedCount] = @AccessFailedCount, [UserName] = @UserName
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RequestIsSeen");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
