namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTermsOfConditionsType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Settings", "TermsOfConditions", c => c.String(nullable: false));
            AlterStoredProcedure(
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
                        TermsOfConditions = p.String(),
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
            
            AlterStoredProcedure(
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
                        TermsOfConditions = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Settings]
                      SET [UnSupportedPathMessage] = @UnSupportedPathMessage, [SuspendAgentMessage] = @SuspendAgentMessage, [MinimumPrice] = @MinimumPrice, [StartPrice] = @StartPrice, [KiloMeterPrice] = @KiloMeterPrice, [MinutePrice] = @MinutePrice, [ManagementPercentage] = @ManagementPercentage, [AbuseEmail] = @AbuseEmail, [ContactUsEmail] = @ContactUsEmail, [TermsOfConditions] = @TermsOfConditions
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Settings", "TermsOfConditions", c => c.String(nullable: false, maxLength: 1000));
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
