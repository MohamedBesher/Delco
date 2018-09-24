namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPassengerCountColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "PassengerCount", c => c.Int(nullable: false));
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
                        PassengerCount = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Cars]([Id], [CompanyName], [Type], [Model], [Color], [PlateNumber], [PassengerCount])
                      VALUES (@Id, @CompanyName, @Type, @Model, @Color, @PlateNumber, @PassengerCount)"
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
                        PassengerCount = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Cars]
                      SET [CompanyName] = @CompanyName, [Type] = @Type, [Model] = @Model, [Color] = @Color, [PlateNumber] = @PlateNumber, [PassengerCount] = @PassengerCount
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "PassengerCount");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
