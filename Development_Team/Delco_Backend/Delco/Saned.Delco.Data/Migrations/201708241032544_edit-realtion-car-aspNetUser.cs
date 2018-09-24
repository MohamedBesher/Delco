namespace Saned.Delco.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrealtioncaraspNetUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cars", "Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.Cars", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "Id", "dbo.AspNetUsers");
            AddForeignKey("dbo.Cars", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
