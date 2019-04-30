namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "StreetAddress", c => c.String(nullable: false));
            AddColumn("dbo.Users", "UserGuid", c => c.Guid(nullable: false));
            AddColumn("dbo.Users", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DateCreated");
            DropColumn("dbo.Users", "UserGuid");
            DropColumn("dbo.Users", "StreetAddress");
        }
    }
}
