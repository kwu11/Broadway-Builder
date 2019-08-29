namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        PermissionID = c.Int(nullable: false),
                        PermissionName = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PermissionID);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        PermissionID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        isEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.PermissionID, t.RoleID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.PermissionID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        RoleName = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        isEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        Permission_PermissionID = c.Int(),
                        Theater_TheaterID = c.Int(),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_PermissionID)
                .ForeignKey("dbo.Theaters", t => t.Theater_TheaterID)
                .Index(t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.Permission_PermissionID)
                .Index(t => t.Theater_TheaterID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 450),
                        FirstName = c.String(),
                        LastName = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        StateProvince = c.String(),
                        Country = c.String(),
                        IsEnabled = c.Boolean(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                        UserGuid = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.Resumes",
                c => new
                    {
                        ResumeID = c.Int(nullable: false, identity: true),
                        ResumeGuid = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResumeID)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ResumeGuid, unique: true)
                .Index(t => t.UserId, unique: true);
            
            CreateTable(
                "dbo.ProductionsDateTimes",
                c => new
                    {
                        ProductionDateTimeId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Time = c.Time(nullable: false, precision: 7),
                        ProductionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductionDateTimeId)
                .ForeignKey("dbo.Productions", t => t.ProductionID, cascadeDelete: true)
                .Index(t => t.ProductionID);
            
            CreateTable(
                "dbo.Productions",
                c => new
                    {
                        ProductionID = c.Int(nullable: false, identity: true),
                        ProductionName = c.String(nullable: false),
                        DirectorFirstName = c.String(nullable: false),
                        DirectorLastName = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        City = c.String(nullable: false),
                        StateProvince = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Zipcode = c.String(nullable: false),
                        TheaterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductionID)
                .ForeignKey("dbo.Theaters", t => t.TheaterID, cascadeDelete: true)
                .Index(t => t.TheaterID);
            
            CreateTable(
                "dbo.ProductionJobPostings",
                c => new
                    {
                        HelpWantedID = c.Int(nullable: false, identity: true),
                        ProductionID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Position = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Hours = c.String(nullable: false),
                        Requirements = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.HelpWantedID)
                .ForeignKey("dbo.Productions", t => t.ProductionID, cascadeDelete: true)
                .Index(t => t.ProductionID);
            
            CreateTable(
                "dbo.Theaters",
                c => new
                    {
                        TheaterID = c.Int(nullable: false, identity: true),
                        TheaterName = c.String(nullable: false),
                        CompanyName = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TheaterID);
            
            CreateTable(
                "dbo.TheaterJobPostings",
                c => new
                    {
                        HelpWantedID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Position = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 3000),
                        Title = c.String(nullable: false, maxLength: 70),
                        Hours = c.String(nullable: false, maxLength: 20),
                        Requirements = c.String(nullable: false, maxLength: 1500),
                        JobType = c.String(nullable: false, maxLength: 20),
                        TheaterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HelpWantedID)
                .ForeignKey("dbo.Theaters", t => t.TheaterID, cascadeDelete: true)
                .Index(t => t.TheaterID);
            
            CreateTable(
                "dbo.ResumeTheaterJobs",
                c => new
                    {
                        ResumeTheaterJobID = c.Int(nullable: false, identity: true),
                        HelpWantedID = c.Int(nullable: false),
                        ResumeID = c.Int(nullable: false),
                        DateUploaded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResumeTheaterJobID)
                .ForeignKey("dbo.TheaterJobPostings", t => t.HelpWantedID, cascadeDelete: true)
                .ForeignKey("dbo.Resumes", t => t.ResumeID, cascadeDelete: true)
                .Index(t => t.HelpWantedID)
                .Index(t => t.ResumeID);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Token = c.String(nullable: false),
                        ExpiresAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Signature = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "UserId", "dbo.Users");
            DropForeignKey("dbo.ResumeTheaterJobs", "ResumeID", "dbo.Resumes");
            DropForeignKey("dbo.ResumeTheaterJobs", "HelpWantedID", "dbo.TheaterJobPostings");
            DropForeignKey("dbo.UserRoles", "Theater_TheaterID", "dbo.Theaters");
            DropForeignKey("dbo.TheaterJobPostings", "TheaterID", "dbo.Theaters");
            DropForeignKey("dbo.Productions", "TheaterID", "dbo.Theaters");
            DropForeignKey("dbo.ProductionJobPostings", "ProductionID", "dbo.Productions");
            DropForeignKey("dbo.ProductionsDateTimes", "ProductionID", "dbo.Productions");
            DropForeignKey("dbo.UserRoles", "Permission_PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Resumes", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.Sessions", new[] { "UserId" });
            DropIndex("dbo.ResumeTheaterJobs", new[] { "ResumeID" });
            DropIndex("dbo.ResumeTheaterJobs", new[] { "HelpWantedID" });
            DropIndex("dbo.TheaterJobPostings", new[] { "TheaterID" });
            DropIndex("dbo.ProductionJobPostings", new[] { "ProductionID" });
            DropIndex("dbo.Productions", new[] { "TheaterID" });
            DropIndex("dbo.ProductionsDateTimes", new[] { "ProductionID" });
            DropIndex("dbo.Resumes", new[] { "UserId" });
            DropIndex("dbo.Resumes", new[] { "ResumeGuid" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.UserRoles", new[] { "Theater_TheaterID" });
            DropIndex("dbo.UserRoles", new[] { "Permission_PermissionID" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleID" });
            DropIndex("dbo.RolePermissions", new[] { "PermissionID" });
            DropTable("dbo.Sessions");
            DropTable("dbo.ResumeTheaterJobs");
            DropTable("dbo.TheaterJobPostings");
            DropTable("dbo.Theaters");
            DropTable("dbo.ProductionJobPostings");
            DropTable("dbo.Productions");
            DropTable("dbo.ProductionsDateTimes");
            DropTable("dbo.Resumes");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Permissions");
        }
    }
}
