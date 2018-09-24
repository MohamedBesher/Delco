namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRefuseRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefuseRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Cause = c.String(),
                        UserId = c.String(maxLength: 128),
                        AgentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AgentId);
            
            CreateStoredProcedure(
                "dbo.RefuseRequest_Insert",
                p => new
                    {
                        Cause = p.String(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[RefuseRequests]([Cause], [UserId], [AgentId])
                      VALUES (@Cause, @UserId, @AgentId)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[RefuseRequests]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RefuseRequests] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.RefuseRequest_Update",
                p => new
                    {
                        Id = p.Long(),
                        Cause = p.String(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[RefuseRequests]
                      SET [Cause] = @Cause, [UserId] = @UserId, [AgentId] = @AgentId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RefuseRequest_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[RefuseRequests]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.RefuseRequest_Delete");
            DropStoredProcedure("dbo.RefuseRequest_Update");
            DropStoredProcedure("dbo.RefuseRequest_Insert");
            DropForeignKey("dbo.RefuseRequests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RefuseRequests", "AgentId", "dbo.AspNetUsers");
            DropIndex("dbo.RefuseRequests", new[] { "AgentId" });
            DropIndex("dbo.RefuseRequests", new[] { "UserId" });
            DropTable("dbo.RefuseRequests");
        }
    }
}
