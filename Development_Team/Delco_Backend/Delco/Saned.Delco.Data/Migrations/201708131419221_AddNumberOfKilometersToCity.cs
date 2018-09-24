namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberOfKilometersToCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "NumberOfKilometers", c => c.Int(nullable: false));
            AlterStoredProcedure(
                "dbo.City_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 250),
                        Latitude = p.String(maxLength: 50),
                        Longitude = p.String(maxLength: 50),
                        NumberOfKilometers = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Cities]([Name], [Latitude], [Longitude], [NumberOfKilometers])
                      VALUES (@Name, @Latitude, @Longitude, @NumberOfKilometers)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[Cities]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Cities] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.City_Update",
                p => new
                    {
                        Id = p.Long(),
                        Name = p.String(maxLength: 250),
                        Latitude = p.String(maxLength: 50),
                        Longitude = p.String(maxLength: 50),
                        NumberOfKilometers = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Cities]
                      SET [Name] = @Name, [Latitude] = @Latitude, [Longitude] = @Longitude, [NumberOfKilometers] = @NumberOfKilometers
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cities", "NumberOfKilometers");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
