namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedUnusedRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRoles", "Permission_PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.UserRoles", "Theater_TheaterID", "dbo.Theaters");
            DropIndex("dbo.UserRoles", new[] { "Permission_PermissionID" });
            DropIndex("dbo.UserRoles", new[] { "Theater_TheaterID" });
            DropColumn("dbo.UserRoles", "Permission_PermissionID");
            DropColumn("dbo.UserRoles", "Theater_TheaterID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRoles", "Theater_TheaterID", c => c.Int());
            AddColumn("dbo.UserRoles", "Permission_PermissionID", c => c.Int());
            CreateIndex("dbo.UserRoles", "Theater_TheaterID");
            CreateIndex("dbo.UserRoles", "Permission_PermissionID");
            AddForeignKey("dbo.UserRoles", "Theater_TheaterID", "dbo.Theaters", "TheaterID");
            AddForeignKey("dbo.UserRoles", "Permission_PermissionID", "dbo.Permissions", "PermissionID");
        }
    }
}
