namespace EventBooking.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lund : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.Items", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserActivityItems", "UserId", "dbo.Users");
            DropIndex("dbo.Items", new[] { "Activity_Id" });
            DropIndex("dbo.Items", new[] { "User_Id" });
            DropIndex("dbo.UserActivityItems", new[] { "UserId" });
            RenameColumn(table: "dbo.UserActivityItems", name: "UserId", newName: "User_Id");
            CreateTable(
                "dbo.MailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Subject = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ActivityItems", "Activity_Id", c => c.Int());
            AddForeignKey("dbo.ActivityItems", "Activity_Id", "dbo.Activities", "Id");
            AddForeignKey("dbo.UserActivityItems", "User_Id", "dbo.Users", "Id");
            CreateIndex("dbo.ActivityItems", "Activity_Id");
            CreateIndex("dbo.UserActivityItems", "User_Id");
            DropTable("dbo.Items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Activity_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.UserActivityItems", new[] { "User_Id" });
            DropIndex("dbo.ActivityItems", new[] { "Activity_Id" });
            DropForeignKey("dbo.UserActivityItems", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ActivityItems", "Activity_Id", "dbo.Activities");
            DropColumn("dbo.ActivityItems", "Activity_Id");
            DropTable("dbo.MailTemplates");
            RenameColumn(table: "dbo.UserActivityItems", name: "User_Id", newName: "UserId");
            CreateIndex("dbo.UserActivityItems", "UserId");
            CreateIndex("dbo.Items", "User_Id");
            CreateIndex("dbo.Items", "Activity_Id");
            AddForeignKey("dbo.UserActivityItems", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Items", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Items", "Activity_Id", "dbo.Activities", "Id");
        }
    }
}
