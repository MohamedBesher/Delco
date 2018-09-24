namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialtablescreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abuses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 1000),
                        Title = c.String(nullable: false, maxLength: 150),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ConfirmedToken = c.String(),
                        ResetPasswordlToken = c.String(),
                        IsDeleted = c.Boolean(),
                        FullName = c.String(),
                        PhotoUrl = c.String(),
                        Address = c.String(),
                        IsNotified = c.Boolean(nullable: false),
                        SecondPhoneNumber = c.String(),
                        Type = c.Int(nullable: false),
                        CityId = c.Long(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TotalDegree = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AgentId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Degree = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AgentId);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(nullable: false, maxLength: 250),
                        Type = c.String(nullable: false, maxLength: 250),
                        Model = c.String(nullable: false, maxLength: 250),
                        Color = c.String(nullable: false, maxLength: 250),
                        PlateNumber = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Latitude = c.String(nullable: false, maxLength: 50),
                        Longitude = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Address = c.String(),
                        ToLongtitude = c.String(nullable: false),
                        ToLatitude = c.String(nullable: false),
                        ToLocation = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CityId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AgentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CityId)
                .Index(t => t.UserId)
                .Index(t => t.AgentId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CityId)
                .Index(t => t.UserId)
                .Index(t => t.AgentId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactUs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 1000),
                        Title = c.String(nullable: false, maxLength: 250),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DeviceSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DeviceId = c.String(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Host = c.String(maxLength: 150),
                        FromEmail = c.String(maxLength: 150),
                        Password = c.String(maxLength: 150),
                        SubjectAr = c.String(maxLength: 150),
                        SubjectEn = c.String(maxLength: 150),
                        MessageBodyAr = c.String(),
                        MessageBodyEn = c.String(),
                        EmailSettingType = c.String(maxLength: 10),
                        Port = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationDevices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NotificationMessageId = c.Long(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.NotificationMessages", t => t.NotificationMessageId, cascadeDelete: true)
                .Index(t => t.NotificationMessageId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.NotificationMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 500),
                        EnglishMessage = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TripId = c.Long(),
                        RequestId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.RequestId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .Index(t => t.TripId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.Paths",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        FromCityId = c.Long(nullable: false),
                        ToCityId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.FromCityId, cascadeDelete: true)
                .ForeignKey("dbo.Cities", t => t.ToCityId)
                .Index(t => t.FromCityId)
                .Index(t => t.ToCityId);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 100),
                        IssuedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ExpiresUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnSupportedPathMessage = c.String(nullable: false, maxLength: 1000),
                        SuspendAgentMessage = c.String(nullable: false, maxLength: 1000),
                        MinimumPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KiloMeterPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinutePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ManagementPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AbuseEmail = c.String(nullable: false),
                        ContactUsEmail = c.String(nullable: false),
                        TermsOfConditions = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateStoredProcedure(
                "dbo.Abuse_Insert",
                p => new
                    {
                        Message = p.String(maxLength: 1000),
                        Title = p.String(maxLength: 150),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Abuses]([Message], [Title], [UserId])
                      VALUES (@Message, @Title, @UserId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Abuses]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Abuses] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Abuse_Update",
                p => new
                    {
                        Id = p.Long(),
                        Message = p.String(maxLength: 1000),
                        Title = p.String(maxLength: 150),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Abuses]
                      SET [Message] = @Message, [Title] = @Title, [UserId] = @UserId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Abuse_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Abuses]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ApplicationUser_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        ConfirmedToken = p.String(),
                        ResetPasswordlToken = p.String(),
                        IsDeleted = p.Boolean(),
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
                    @"INSERT [dbo].[AspNetUsers]([Id], [ConfirmedToken], [ResetPasswordlToken], [IsDeleted], [FullName], [PhotoUrl], [Address], [IsNotified], [SecondPhoneNumber], [Type], [CityId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                      VALUES (@Id, @ConfirmedToken, @ResetPasswordlToken, @IsDeleted, @FullName, @PhotoUrl, @Address, @IsNotified, @SecondPhoneNumber, @Type, @CityId, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)"
            );
            
            CreateStoredProcedure(
                "dbo.ApplicationUser_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        ConfirmedToken = p.String(),
                        ResetPasswordlToken = p.String(),
                        IsDeleted = p.Boolean(),
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
                      SET [ConfirmedToken] = @ConfirmedToken, [ResetPasswordlToken] = @ResetPasswordlToken, [IsDeleted] = @IsDeleted, [FullName] = @FullName, [PhotoUrl] = @PhotoUrl, [Address] = @Address, [IsNotified] = @IsNotified, [SecondPhoneNumber] = @SecondPhoneNumber, [Type] = @Type, [CityId] = @CityId, [Email] = @Email, [EmailConfirmed] = @EmailConfirmed, [PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [PhoneNumber] = @PhoneNumber, [PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEndDateUtc] = @LockoutEndDateUtc, [LockoutEnabled] = @LockoutEnabled, [AccessFailedCount] = @AccessFailedCount, [UserName] = @UserName
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ApplicationUser_Delete",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[AspNetUsers]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Rating_Insert",
                p => new
                    {
                        TotalDegree = p.Int(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        Date = p.DateTime(storeType: "datetime2"),
                        Degree = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Ratings]([TotalDegree], [UserId], [AgentId], [Date], [Degree])
                      VALUES (@TotalDegree, @UserId, @AgentId, @Date, @Degree)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Ratings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Ratings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Rating_Update",
                p => new
                    {
                        Id = p.Long(),
                        TotalDegree = p.Int(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        Date = p.DateTime(storeType: "datetime2"),
                        Degree = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Ratings]
                      SET [TotalDegree] = @TotalDegree, [UserId] = @UserId, [AgentId] = @AgentId, [Date] = @Date, [Degree] = @Degree
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Rating_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Ratings]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Car_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        CompanyName = p.String(maxLength: 250),
                        Type = p.String(maxLength: 250),
                        Model = p.String(maxLength: 250),
                        Color = p.String(maxLength: 250),
                        PlateNumber = p.String(maxLength: 250),
                    },
                body:
                    @"INSERT [dbo].[Cars]([Id], [CompanyName], [Type], [Model], [Color], [PlateNumber])
                      VALUES (@Id, @CompanyName, @Type, @Model, @Color, @PlateNumber)"
            );
            
            CreateStoredProcedure(
                "dbo.Car_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        CompanyName = p.String(maxLength: 250),
                        Type = p.String(maxLength: 250),
                        Model = p.String(maxLength: 250),
                        Color = p.String(maxLength: 250),
                        PlateNumber = p.String(maxLength: 250),
                    },
                body:
                    @"UPDATE [dbo].[Cars]
                      SET [CompanyName] = @CompanyName, [Type] = @Type, [Model] = @Model, [Color] = @Color, [PlateNumber] = @PlateNumber
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Car_Delete",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[Cars]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.City_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 250),
                        Latitude = p.String(maxLength: 50),
                        Longitude = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[Cities]([Name], [Latitude], [Longitude])
                      VALUES (@Name, @Latitude, @Longitude)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Cities]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Cities] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.City_Update",
                p => new
                    {
                        Id = p.Long(),
                        Name = p.String(maxLength: 250),
                        Latitude = p.String(maxLength: 50),
                        Longitude = p.String(maxLength: 50),
                    },
                body:
                    @"UPDATE [dbo].[Cities]
                      SET [Name] = @Name, [Latitude] = @Latitude, [Longitude] = @Longitude
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.City_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Cities]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserClaim_Insert",
                p => new
                    {
                        UserId = p.String(maxLength: 128),
                        ClaimType = p.String(),
                        ClaimValue = p.String(),
                    },
                body:
                    @"INSERT [dbo].[AspNetUserClaims]([UserId], [ClaimType], [ClaimValue])
                      VALUES (@UserId, @ClaimType, @ClaimValue)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[AspNetUserClaims]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[AspNetUserClaims] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserClaim_Update",
                p => new
                    {
                        Id = p.Int(),
                        UserId = p.String(maxLength: 128),
                        ClaimType = p.String(),
                        ClaimValue = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[AspNetUserClaims]
                      SET [UserId] = @UserId, [ClaimType] = @ClaimType, [ClaimValue] = @ClaimValue
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserClaim_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AspNetUserClaims]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserLogin_Insert",
                p => new
                    {
                        LoginProvider = p.String(maxLength: 128),
                        ProviderKey = p.String(maxLength: 128),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[AspNetUserLogins]([LoginProvider], [ProviderKey], [UserId])
                      VALUES (@LoginProvider, @ProviderKey, @UserId)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserLogin_Update",
                p => new
                    {
                        LoginProvider = p.String(maxLength: 128),
                        ProviderKey = p.String(maxLength: 128),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"RETURN"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserLogin_Delete",
                p => new
                    {
                        LoginProvider = p.String(maxLength: 128),
                        ProviderKey = p.String(maxLength: 128),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[AspNetUserLogins]
                      WHERE ((([LoginProvider] = @LoginProvider) AND ([ProviderKey] = @ProviderKey)) AND ([UserId] = @UserId))"
            );
            
            CreateStoredProcedure(
                "dbo.Request_Insert",
                p => new
                    {
                        Address = p.String(),
                        ToLongtitude = p.String(),
                        ToLatitude = p.String(),
                        ToLocation = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        CityId = p.Long(),
                        Status = p.Int(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Requests]([Address], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [CityId], [Status], [UserId], [AgentId])
                      VALUES (@Address, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @CityId, @Status, @UserId, @AgentId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Requests]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Requests] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Request_Update",
                p => new
                    {
                        Id = p.Long(),
                        Address = p.String(),
                        ToLongtitude = p.String(),
                        ToLatitude = p.String(),
                        ToLocation = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        CityId = p.Long(),
                        Status = p.Int(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Requests]
                      SET [Address] = @Address, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [CityId] = @CityId, [Status] = @Status, [UserId] = @UserId, [AgentId] = @AgentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Request_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Requests]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserRole_Insert",
                p => new
                    {
                        UserId = p.String(maxLength: 128),
                        RoleId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[AspNetUserRoles]([UserId], [RoleId])
                      VALUES (@UserId, @RoleId)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserRole_Update",
                p => new
                    {
                        UserId = p.String(maxLength: 128),
                        RoleId = p.String(maxLength: 128),
                    },
                body:
                    @"RETURN"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityUserRole_Delete",
                p => new
                    {
                        UserId = p.String(maxLength: 128),
                        RoleId = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[AspNetUserRoles]
                      WHERE (([UserId] = @UserId) AND ([RoleId] = @RoleId))"
            );
            
            CreateStoredProcedure(
                "dbo.Trip_Insert",
                p => new
                    {
                        FromLongtitude = p.String(),
                        FromLatitude = p.String(),
                        FromLocation = p.String(),
                        ToLongtitude = p.String(),
                        ToLatitude = p.String(),
                        ToLocation = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        PassengerNumber = p.Int(),
                        CityId = p.Long(),
                        Address = p.String(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        Status = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Trips]([FromLongtitude], [FromLatitude], [FromLocation], [ToLongtitude], [ToLatitude], [ToLocation], [Price], [PassengerNumber], [CityId], [Address], [UserId], [AgentId], [Status])
                      VALUES (@FromLongtitude, @FromLatitude, @FromLocation, @ToLongtitude, @ToLatitude, @ToLocation, @Price, @PassengerNumber, @CityId, @Address, @UserId, @AgentId, @Status)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Trips]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Trips] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Trip_Update",
                p => new
                    {
                        Id = p.Long(),
                        FromLongtitude = p.String(),
                        FromLatitude = p.String(),
                        FromLocation = p.String(),
                        ToLongtitude = p.String(),
                        ToLatitude = p.String(),
                        ToLocation = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        PassengerNumber = p.Int(),
                        CityId = p.Long(),
                        Address = p.String(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        Status = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Trips]
                      SET [FromLongtitude] = @FromLongtitude, [FromLatitude] = @FromLatitude, [FromLocation] = @FromLocation, [ToLongtitude] = @ToLongtitude, [ToLatitude] = @ToLatitude, [ToLocation] = @ToLocation, [Price] = @Price, [PassengerNumber] = @PassengerNumber, [CityId] = @CityId, [Address] = @Address, [UserId] = @UserId, [AgentId] = @AgentId, [Status] = @Status
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Trip_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Trips]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Client_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        Secret = p.String(),
                        Name = p.String(maxLength: 100),
                        ApplicationType = p.Int(),
                        Active = p.Boolean(),
                        RefreshTokenLifeTime = p.Int(),
                        AllowedOrigin = p.String(maxLength: 100),
                    },
                body:
                    @"INSERT [dbo].[Clients]([Id], [Secret], [Name], [ApplicationType], [Active], [RefreshTokenLifeTime], [AllowedOrigin])
                      VALUES (@Id, @Secret, @Name, @ApplicationType, @Active, @RefreshTokenLifeTime, @AllowedOrigin)"
            );
            
            CreateStoredProcedure(
                "dbo.Client_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        Secret = p.String(),
                        Name = p.String(maxLength: 100),
                        ApplicationType = p.Int(),
                        Active = p.Boolean(),
                        RefreshTokenLifeTime = p.Int(),
                        AllowedOrigin = p.String(maxLength: 100),
                    },
                body:
                    @"UPDATE [dbo].[Clients]
                      SET [Secret] = @Secret, [Name] = @Name, [ApplicationType] = @ApplicationType, [Active] = @Active, [RefreshTokenLifeTime] = @RefreshTokenLifeTime, [AllowedOrigin] = @AllowedOrigin
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Client_Delete",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[Clients]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactUs_Insert",
                p => new
                    {
                        Message = p.String(maxLength: 1000),
                        Title = p.String(maxLength: 250),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[ContactUs]([Message], [Title], [UserId])
                      VALUES (@Message, @Title, @UserId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[ContactUs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ContactUs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ContactUs_Update",
                p => new
                    {
                        Id = p.Long(),
                        Message = p.String(maxLength: 1000),
                        Title = p.String(maxLength: 250),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[ContactUs]
                      SET [Message] = @Message, [Title] = @Title, [UserId] = @UserId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ContactUs_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[ContactUs]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeviceSetting_Insert",
                p => new
                    {
                        DeviceId = p.String(),
                        ApplicationUserId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[DeviceSettings]([DeviceId], [ApplicationUserId])
                      VALUES (@DeviceId, @ApplicationUserId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[DeviceSettings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[DeviceSettings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.DeviceSetting_Update",
                p => new
                    {
                        Id = p.Long(),
                        DeviceId = p.String(),
                        ApplicationUserId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[DeviceSettings]
                      SET [DeviceId] = @DeviceId, [ApplicationUserId] = @ApplicationUserId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.DeviceSetting_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[DeviceSettings]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EmailSetting_Insert",
                p => new
                    {
                        Host = p.String(maxLength: 150),
                        FromEmail = p.String(maxLength: 150),
                        Password = p.String(maxLength: 150),
                        SubjectAr = p.String(maxLength: 150),
                        SubjectEn = p.String(maxLength: 150),
                        MessageBodyAr = p.String(),
                        MessageBodyEn = p.String(),
                        EmailSettingType = p.String(maxLength: 10),
                        Port = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[EmailSettings]([Host], [FromEmail], [Password], [SubjectAr], [SubjectEn], [MessageBodyAr], [MessageBodyEn], [EmailSettingType], [Port])
                      VALUES (@Host, @FromEmail, @Password, @SubjectAr, @SubjectEn, @MessageBodyAr, @MessageBodyEn, @EmailSettingType, @Port)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[EmailSettings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[EmailSettings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.EmailSetting_Update",
                p => new
                    {
                        Id = p.Int(),
                        Host = p.String(maxLength: 150),
                        FromEmail = p.String(maxLength: 150),
                        Password = p.String(maxLength: 150),
                        SubjectAr = p.String(maxLength: 150),
                        SubjectEn = p.String(maxLength: 150),
                        MessageBodyAr = p.String(),
                        MessageBodyEn = p.String(),
                        EmailSettingType = p.String(maxLength: 10),
                        Port = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[EmailSettings]
                      SET [Host] = @Host, [FromEmail] = @FromEmail, [Password] = @Password, [SubjectAr] = @SubjectAr, [SubjectEn] = @SubjectEn, [MessageBodyAr] = @MessageBodyAr, [MessageBodyEn] = @MessageBodyEn, [EmailSettingType] = @EmailSettingType, [Port] = @Port
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.EmailSetting_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[EmailSettings]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationDevice_Insert",
                p => new
                    {
                        NotificationMessageId = p.Long(),
                        ApplicationUserId = p.String(maxLength: 128),
                        Type = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[NotificationDevices]([NotificationMessageId], [ApplicationUserId], [Type])
                      VALUES (@NotificationMessageId, @ApplicationUserId, @Type)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[NotificationDevices]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[NotificationDevices] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationDevice_Update",
                p => new
                    {
                        Id = p.Long(),
                        NotificationMessageId = p.Long(),
                        ApplicationUserId = p.String(maxLength: 128),
                        Type = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[NotificationDevices]
                      SET [NotificationMessageId] = @NotificationMessageId, [ApplicationUserId] = @ApplicationUserId, [Type] = @Type
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationDevice_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[NotificationDevices]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationMessage_Insert",
                p => new
                    {
                        Message = p.String(maxLength: 500),
                        EnglishMessage = p.String(),
                        CreationDate = p.DateTime(storeType: "datetime2"),
                        TripId = p.Long(),
                        RequestId = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[NotificationMessages]([Message], [EnglishMessage], [CreationDate], [TripId], [RequestId])
                      VALUES (@Message, @EnglishMessage, @CreationDate, @TripId, @RequestId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[NotificationMessages]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[NotificationMessages] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationMessage_Update",
                p => new
                    {
                        Id = p.Long(),
                        Message = p.String(maxLength: 500),
                        EnglishMessage = p.String(),
                        CreationDate = p.DateTime(storeType: "datetime2"),
                        TripId = p.Long(),
                        RequestId = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[NotificationMessages]
                      SET [Message] = @Message, [EnglishMessage] = @EnglishMessage, [CreationDate] = @CreationDate, [TripId] = @TripId, [RequestId] = @RequestId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.NotificationMessage_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[NotificationMessages]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Path_Insert",
                p => new
                    {
                        Price = p.Decimal(precision: 18, scale: 2),
                        Type = p.Int(),
                        FromCityId = p.Long(),
                        ToCityId = p.Long(),
                    },
                body:
                    @"INSERT [dbo].[Paths]([Price], [Type], [FromCityId], [ToCityId])
                      VALUES (@Price, @Type, @FromCityId, @ToCityId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Paths]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Paths] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Path_Update",
                p => new
                    {
                        Id = p.Long(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        Type = p.Int(),
                        FromCityId = p.Long(),
                        ToCityId = p.Long(),
                    },
                body:
                    @"UPDATE [dbo].[Paths]
                      SET [Price] = @Price, [Type] = @Type, [FromCityId] = @FromCityId, [ToCityId] = @ToCityId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Path_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[Paths]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RefreshToken_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        Subject = p.String(maxLength: 50),
                        ClientId = p.String(maxLength: 100),
                        IssuedUtc = p.DateTime(storeType: "datetime2"),
                        ExpiresUtc = p.DateTime(storeType: "datetime2"),
                        ProtectedTicket = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RefreshTokens]([Id], [Subject], [ClientId], [IssuedUtc], [ExpiresUtc], [ProtectedTicket])
                      VALUES (@Id, @Subject, @ClientId, @IssuedUtc, @ExpiresUtc, @ProtectedTicket)"
            );
            
            CreateStoredProcedure(
                "dbo.RefreshToken_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        Subject = p.String(maxLength: 50),
                        ClientId = p.String(maxLength: 100),
                        IssuedUtc = p.DateTime(storeType: "datetime2"),
                        ExpiresUtc = p.DateTime(storeType: "datetime2"),
                        ProtectedTicket = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RefreshTokens]
                      SET [Subject] = @Subject, [ClientId] = @ClientId, [IssuedUtc] = @IssuedUtc, [ExpiresUtc] = @ExpiresUtc, [ProtectedTicket] = @ProtectedTicket
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RefreshToken_Delete",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[RefreshTokens]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityRole_Insert",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        Name = p.String(maxLength: 256),
                    },
                body:
                    @"INSERT [dbo].[AspNetRoles]([Id], [Name])
                      VALUES (@Id, @Name)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityRole_Update",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                        Name = p.String(maxLength: 256),
                    },
                body:
                    @"UPDATE [dbo].[AspNetRoles]
                      SET [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.IdentityRole_Delete",
                p => new
                    {
                        Id = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[AspNetRoles]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Setting_Insert",
                p => new
                    {
                        UnSupportedPathMessage = p.String(maxLength: 1000),
                        SuspendAgentMessage = p.String(maxLength: 1000),
                        MinimumPrice = p.Decimal(precision: 18, scale: 2),
                        StartPrice = p.Decimal(precision: 18, scale: 2),
                        KiloMeterPrice = p.Decimal(precision: 18, scale: 2),
                        MinutePrice = p.Decimal(precision: 18, scale: 2),
                        ManagementPercentage = p.Decimal(precision: 18, scale: 2),
                        AbuseEmail = p.String(),
                        ContactUsEmail = p.String(),
                        TermsOfConditions = p.String(maxLength: 1000),
                    },
                body:
                    @"INSERT [dbo].[Settings]([UnSupportedPathMessage], [SuspendAgentMessage], [MinimumPrice], [StartPrice], [KiloMeterPrice], [MinutePrice], [ManagementPercentage], [AbuseEmail], [ContactUsEmail], [TermsOfConditions])
                      VALUES (@UnSupportedPathMessage, @SuspendAgentMessage, @MinimumPrice, @StartPrice, @KiloMeterPrice, @MinutePrice, @ManagementPercentage, @AbuseEmail, @ContactUsEmail, @TermsOfConditions)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Settings]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Settings] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Setting_Update",
                p => new
                    {
                        Id = p.Int(),
                        UnSupportedPathMessage = p.String(maxLength: 1000),
                        SuspendAgentMessage = p.String(maxLength: 1000),
                        MinimumPrice = p.Decimal(precision: 18, scale: 2),
                        StartPrice = p.Decimal(precision: 18, scale: 2),
                        KiloMeterPrice = p.Decimal(precision: 18, scale: 2),
                        MinutePrice = p.Decimal(precision: 18, scale: 2),
                        ManagementPercentage = p.Decimal(precision: 18, scale: 2),
                        AbuseEmail = p.String(),
                        ContactUsEmail = p.String(),
                        TermsOfConditions = p.String(maxLength: 1000),
                    },
                body:
                    @"UPDATE [dbo].[Settings]
                      SET [UnSupportedPathMessage] = @UnSupportedPathMessage, [SuspendAgentMessage] = @SuspendAgentMessage, [MinimumPrice] = @MinimumPrice, [StartPrice] = @StartPrice, [KiloMeterPrice] = @KiloMeterPrice, [MinutePrice] = @MinutePrice, [ManagementPercentage] = @ManagementPercentage, [AbuseEmail] = @AbuseEmail, [ContactUsEmail] = @ContactUsEmail, [TermsOfConditions] = @TermsOfConditions
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Setting_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Settings]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Setting_Delete");
            DropStoredProcedure("dbo.Setting_Update");
            DropStoredProcedure("dbo.Setting_Insert");
            DropStoredProcedure("dbo.IdentityRole_Delete");
            DropStoredProcedure("dbo.IdentityRole_Update");
            DropStoredProcedure("dbo.IdentityRole_Insert");
            DropStoredProcedure("dbo.RefreshToken_Delete");
            DropStoredProcedure("dbo.RefreshToken_Update");
            DropStoredProcedure("dbo.RefreshToken_Insert");
            DropStoredProcedure("dbo.Path_Delete");
            DropStoredProcedure("dbo.Path_Update");
            DropStoredProcedure("dbo.Path_Insert");
            DropStoredProcedure("dbo.NotificationMessage_Delete");
            DropStoredProcedure("dbo.NotificationMessage_Update");
            DropStoredProcedure("dbo.NotificationMessage_Insert");
            DropStoredProcedure("dbo.NotificationDevice_Delete");
            DropStoredProcedure("dbo.NotificationDevice_Update");
            DropStoredProcedure("dbo.NotificationDevice_Insert");
            DropStoredProcedure("dbo.EmailSetting_Delete");
            DropStoredProcedure("dbo.EmailSetting_Update");
            DropStoredProcedure("dbo.EmailSetting_Insert");
            DropStoredProcedure("dbo.DeviceSetting_Delete");
            DropStoredProcedure("dbo.DeviceSetting_Update");
            DropStoredProcedure("dbo.DeviceSetting_Insert");
            DropStoredProcedure("dbo.ContactUs_Delete");
            DropStoredProcedure("dbo.ContactUs_Update");
            DropStoredProcedure("dbo.ContactUs_Insert");
            DropStoredProcedure("dbo.Client_Delete");
            DropStoredProcedure("dbo.Client_Update");
            DropStoredProcedure("dbo.Client_Insert");
            DropStoredProcedure("dbo.Trip_Delete");
            DropStoredProcedure("dbo.Trip_Update");
            DropStoredProcedure("dbo.Trip_Insert");
            DropStoredProcedure("dbo.IdentityUserRole_Delete");
            DropStoredProcedure("dbo.IdentityUserRole_Update");
            DropStoredProcedure("dbo.IdentityUserRole_Insert");
            DropStoredProcedure("dbo.Request_Delete");
            DropStoredProcedure("dbo.Request_Update");
            DropStoredProcedure("dbo.Request_Insert");
            DropStoredProcedure("dbo.IdentityUserLogin_Delete");
            DropStoredProcedure("dbo.IdentityUserLogin_Update");
            DropStoredProcedure("dbo.IdentityUserLogin_Insert");
            DropStoredProcedure("dbo.IdentityUserClaim_Delete");
            DropStoredProcedure("dbo.IdentityUserClaim_Update");
            DropStoredProcedure("dbo.IdentityUserClaim_Insert");
            DropStoredProcedure("dbo.City_Delete");
            DropStoredProcedure("dbo.City_Update");
            DropStoredProcedure("dbo.City_Insert");
            DropStoredProcedure("dbo.Car_Delete");
            DropStoredProcedure("dbo.Car_Update");
            DropStoredProcedure("dbo.Car_Insert");
            DropStoredProcedure("dbo.Rating_Delete");
            DropStoredProcedure("dbo.Rating_Update");
            DropStoredProcedure("dbo.Rating_Insert");
            DropStoredProcedure("dbo.ApplicationUser_Delete");
            DropStoredProcedure("dbo.ApplicationUser_Update");
            DropStoredProcedure("dbo.ApplicationUser_Insert");
            DropStoredProcedure("dbo.Abuse_Delete");
            DropStoredProcedure("dbo.Abuse_Update");
            DropStoredProcedure("dbo.Abuse_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Paths", "ToCityId", "dbo.Cities");
            DropForeignKey("dbo.Paths", "FromCityId", "dbo.Cities");
            DropForeignKey("dbo.NotificationDevices", "NotificationMessageId", "dbo.NotificationMessages");
            DropForeignKey("dbo.NotificationMessages", "TripId", "dbo.Trips");
            DropForeignKey("dbo.NotificationMessages", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.NotificationDevices", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeviceSettings", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactUs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Abuses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trips", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trips", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Trips", "AgentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Requests", "AgentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cars", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "AgentId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Paths", new[] { "ToCityId" });
            DropIndex("dbo.Paths", new[] { "FromCityId" });
            DropIndex("dbo.NotificationMessages", new[] { "RequestId" });
            DropIndex("dbo.NotificationMessages", new[] { "TripId" });
            DropIndex("dbo.NotificationDevices", new[] { "ApplicationUserId" });
            DropIndex("dbo.NotificationDevices", new[] { "NotificationMessageId" });
            DropIndex("dbo.DeviceSettings", new[] { "ApplicationUserId" });
            DropIndex("dbo.ContactUs", new[] { "UserId" });
            DropIndex("dbo.Trips", new[] { "AgentId" });
            DropIndex("dbo.Trips", new[] { "UserId" });
            DropIndex("dbo.Trips", new[] { "CityId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Requests", new[] { "AgentId" });
            DropIndex("dbo.Requests", new[] { "UserId" });
            DropIndex("dbo.Requests", new[] { "CityId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Cars", new[] { "Id" });
            DropIndex("dbo.Ratings", new[] { "AgentId" });
            DropIndex("dbo.Ratings", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            DropIndex("dbo.Abuses", new[] { "UserId" });
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.Paths");
            DropTable("dbo.NotificationMessages");
            DropTable("dbo.NotificationDevices");
            DropTable("dbo.EmailSettings");
            DropTable("dbo.DeviceSettings");
            DropTable("dbo.ContactUs");
            DropTable("dbo.Clients");
            DropTable("dbo.Trips");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Requests");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Cities");
            DropTable("dbo.Cars");
            DropTable("dbo.Ratings");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Abuses");
        }
    }
}
