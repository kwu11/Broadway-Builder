using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class BroadwayBuilderContext : DbContext
    {
        public BroadwayBuilderContext() : base("name=BroadwayBuilder")
        {
            // Todo: will want to remove this once we start persiting data as this will cause our data to be lost when changes to models occur
            //Database.SetInitializer<BroadwayBuilderContext>(new DropCreateDatabaseIfModelChanges<BroadwayBuilderContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BroadwayBuilderContext, DataAccessLayer.Migrations.Configuration>());
        }

        //Creating property on db context
        //Allows to access table in db
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TheaterJobPosting> TheaterJobPostings { get; set; }
        public DbSet<ProductionJobPosting> ProductionJobPostings { get; set; }
        public DbSet<ProductionDateTime> ProductionDateTimes { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeTheaterJob> ResumeTheaterJobs {get;set;}
        public DbSet<Session> Sessions { get; set; }

        //
        // Summary:
        //     This method is called when the model for a derived context has been initialized,
        //     but before the model has been locked down and used to initialize the context.
        //     The default implementation of this method does nothing, but it can be overridden
        //     in a derived class such that the model can be further configured before it is
        //     locked down.
        //
        // Parameters:
        //   modelBuilder:
        //     The builder that defines the model for the context being created.
        //
        // Remarks:
        //     Typically, this method is called only once when the first instance of a derived
        //     context is created. The model for that context is then cached and is for all
        //     further instances of the context in the app domain. This caching can be disabled
        //     by setting the ModelCaching property on the given ModelBuidler, but note that
        //     this can seriously degrade performance. More control over caching is provided
        //     through use of the DbModelBuilder and DbContextFactory classes directly.
        //protected override void OnModelCreating(DbModelBuilder modelBuilder) { }

    }
}
