namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSessionModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsComplete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sessions", "Signature", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "FirstName", c => c.String());
            AlterColumn("dbo.Users", "LastName", c => c.String());
            AlterColumn("dbo.Users", "StreetAddress", c => c.String());
            AlterColumn("dbo.Users", "City", c => c.String());
            AlterColumn("dbo.Users", "StateProvince", c => c.String());
            AlterColumn("dbo.Users", "Country", c => c.String());
            AlterColumn("dbo.Sessions", "ExpiresAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Age");
            DropColumn("dbo.Users", "DateOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Age", c => c.Int(nullable: false));
            AlterColumn("dbo.Sessions", "CreatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Sessions", "UpdatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Sessions", "ExpiresAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "StateProvince", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "StreetAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false));
            DropColumn("dbo.Sessions", "Signature");
            DropColumn("dbo.Users", "IsComplete");
        }
    }
}
