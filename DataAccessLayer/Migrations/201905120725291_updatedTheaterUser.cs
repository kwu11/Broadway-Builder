namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedTheaterUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TheaterUsers", "IsDisabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TheaterUsers", "IsDisabled");
        }
    }
}
