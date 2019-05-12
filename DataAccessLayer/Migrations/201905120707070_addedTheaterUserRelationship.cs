namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTheaterUserRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TheaterUsers",
                c => new
                    {
                        TheaterID = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TheaterID, t.UserId })
                .ForeignKey("dbo.Theaters", t => t.TheaterID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TheaterID)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TheaterUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.TheaterUsers", "TheaterID", "dbo.Theaters");
            DropIndex("dbo.TheaterUsers", new[] { "UserId" });
            DropIndex("dbo.TheaterUsers", new[] { "TheaterID" });
            DropTable("dbo.TheaterUsers");
        }
    }
}
