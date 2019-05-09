namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated_Resume_Model : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Resumes", new[] { "UserId" });
            CreateIndex("dbo.Resumes", "UserId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Resumes", new[] { "UserId" });
            CreateIndex("dbo.Resumes", "UserId");
        }
    }
}
