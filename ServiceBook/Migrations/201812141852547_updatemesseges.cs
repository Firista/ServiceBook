namespace ServiceBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemesseges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messeges", "userId", c => c.Int());
            CreateIndex("dbo.Messeges", "userId");
            AddForeignKey("dbo.Messeges", "userId", "dbo.Users", "userId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messeges", "userId", "dbo.Users");
            DropIndex("dbo.Messeges", new[] { "userId" });
            DropColumn("dbo.Messeges", "userId");
        }
    }
}
