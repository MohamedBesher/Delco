namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnNameToPassengerNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PassengerNumbers", "Name", c => c.String());
            AlterStoredProcedure(
                "dbo.PassengerNumber_Insert",
                p => new
                    {
                        Value = p.Long(),
                        Name = p.String(),
                    },
                body:
                    @"INSERT [dbo].[PassengerNumbers]([Value], [Name])
                      VALUES (@Value, @Name)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[PassengerNumbers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PassengerNumbers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.PassengerNumber_Update",
                p => new
                    {
                        Id = p.Long(),
                        Value = p.Long(),
                        Name = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[PassengerNumbers]
                      SET [Value] = @Value, [Name] = @Name
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.PassengerNumbers", "Name");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
