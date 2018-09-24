namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRefuseReasonsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefuseReasons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RefuseRequests", "RefuseReasonId", c => c.Long());
            CreateIndex("dbo.RefuseRequests", "RefuseReasonId");
            AddForeignKey("dbo.RefuseRequests", "RefuseReasonId", "dbo.RefuseReasons", "Id");
            CreateStoredProcedure(
                "dbo.RefuseReason_Insert",
                p => new
                    {
                        Value = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RefuseReasons]([Value])
                      VALUES (@Value)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[RefuseReasons]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RefuseReasons] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.RefuseReason_Update",
                p => new
                    {
                        Id = p.Long(),
                        Value = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RefuseReasons]
                      SET [Value] = @Value
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.RefuseReason_Delete",
                p => new
                    {
                        Id = p.Long(),
                    },
                body:
                    @"DELETE [dbo].[RefuseReasons]
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.RefuseRequest_Insert",
                p => new
                    {
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        RefuseReasonId = p.Long(),
                        Cause = p.String(),
                    },
                body:
                    @"INSERT [dbo].[RefuseRequests]([UserId], [AgentId], [RefuseReasonId], [Cause])
                      VALUES (@UserId, @AgentId, @RefuseReasonId, @Cause)
                      
                      DECLARE @Id bigint
                      SELECT @Id = [Id]
                      FROM [dbo].[RefuseRequests]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[RefuseRequests] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.RefuseRequest_Update",
                p => new
                    {
                        Id = p.Long(),
                        UserId = p.String(maxLength: 128),
                        AgentId = p.String(maxLength: 128),
                        RefuseReasonId = p.Long(),
                        Cause = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[RefuseRequests]
                      SET [UserId] = @UserId, [AgentId] = @AgentId, [RefuseReasonId] = @RefuseReasonId, [Cause] = @Cause
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.RefuseReason_Delete");
            DropStoredProcedure("dbo.RefuseReason_Update");
            DropStoredProcedure("dbo.RefuseReason_Insert");
            DropForeignKey("dbo.RefuseRequests", "RefuseReasonId", "dbo.RefuseReasons");
            DropIndex("dbo.RefuseRequests", new[] { "RefuseReasonId" });
            DropColumn("dbo.RefuseRequests", "RefuseReasonId");
            DropTable("dbo.RefuseReasons");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
