namespace EventBooking.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Cellphone = c.String(),
                        StreetAddress = c.String(),
                        Zipcode = c.String(),
                        City = c.String(),
                        Created = c.DateTime(nullable: false),
                        Birthdate = c.DateTime(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Summary = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Coordinator_Id = c.Int(),
                        OrganizingTeam_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Coordinator_Id)
                .ForeignKey("dbo.Teams", t => t.OrganizingTeam_Id)
                .Index(t => t.Coordinator_Id)
                .Index(t => t.OrganizingTeam_Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromTime = c.Time(nullable: false),
                        ToTime = c.Time(nullable: false),
                        VolunteersNeeded = c.Int(nullable: false),
                        Activity_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.Activity_Id)
                .Index(t => t.Activity_Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Activity_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.Activity_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Activity_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserActivityItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.ActivityItems", t => t.Item_Id)
                .Index(t => t.UserId)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.ActivityItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InterviewQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.TrainingQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.ActivityItemTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SessionUsers",
                c => new
                    {
                        Session_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Session_Id, t.User_Id })
                .ForeignKey("dbo.Sessions", t => t.Session_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Session_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SessionUsers", new[] { "User_Id" });
            DropIndex("dbo.SessionUsers", new[] { "Session_Id" });
            DropIndex("dbo.TrainingQuestions", new[] { "TeamId" });
            DropIndex("dbo.InterviewQuestions", new[] { "TeamId" });
            DropIndex("dbo.UserActivityItems", new[] { "Item_Id" });
            DropIndex("dbo.UserActivityItems", new[] { "UserId" });
            DropIndex("dbo.Items", new[] { "User_Id" });
            DropIndex("dbo.Items", new[] { "Activity_Id" });
            DropIndex("dbo.Sessions", new[] { "Activity_Id" });
            DropIndex("dbo.Activities", new[] { "OrganizingTeam_Id" });
            DropIndex("dbo.Activities", new[] { "Coordinator_Id" });
            DropIndex("dbo.Users", new[] { "Team_Id" });
            DropForeignKey("dbo.SessionUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SessionUsers", "Session_Id", "dbo.Sessions");
            DropForeignKey("dbo.TrainingQuestions", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.InterviewQuestions", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.UserActivityItems", "Item_Id", "dbo.ActivityItems");
            DropForeignKey("dbo.UserActivityItems", "UserId", "dbo.Users");
            DropForeignKey("dbo.Items", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Items", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.Sessions", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.Activities", "OrganizingTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.Activities", "Coordinator_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Team_Id", "dbo.Teams");
            DropTable("dbo.SessionUsers");
            DropTable("dbo.ActivityItemTemplates");
            DropTable("dbo.TrainingQuestions");
            DropTable("dbo.InterviewQuestions");
            DropTable("dbo.ActivityItems");
            DropTable("dbo.UserActivityItems");
            DropTable("dbo.Items");
            DropTable("dbo.Sessions");
            DropTable("dbo.Activities");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
        }
    }
}
