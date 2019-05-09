namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_TheaterJobPosting_Model : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TheaterJobPostings", "Position", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.TheaterJobPostings", "Description", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.TheaterJobPostings", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.TheaterJobPostings", "Hours", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.TheaterJobPostings", "Requirements", c => c.String(nullable: false, maxLength: 1500));
            AlterColumn("dbo.TheaterJobPostings", "JobType", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TheaterJobPostings", "JobType", c => c.String(nullable: false));
            AlterColumn("dbo.TheaterJobPostings", "Requirements", c => c.String(nullable: false));
            AlterColumn("dbo.TheaterJobPostings", "Hours", c => c.String(nullable: false));
            AlterColumn("dbo.TheaterJobPostings", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.TheaterJobPostings", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.TheaterJobPostings", "Position", c => c.String(nullable: false));
        }
    }
}
