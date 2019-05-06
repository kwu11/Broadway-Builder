namespace DataAccessLayer.Migrations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.BroadwayBuilderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DataAccessLayer.BroadwayBuilderContext";
        }

        string GenerateEmailAddress(int numberOfCharacters)
        {
            var characters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

            var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            var sb = new StringBuilder();
            for (int i = 0; i < numberOfCharacters; i++)
            {
                var randChar = characters[random.Next(characters.Length)];
                sb.Append(randChar);
            }
            sb.Append($"@{GenerateName(random.Next(10,30))}.com");

            var emailAddress = sb.ToString();
            return emailAddress;
        }

        string GenerateName(int numberOfCharacters)
        {
            var characters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

            var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            var sb = new StringBuilder();
            for (int i = 0; i < numberOfCharacters; i++)
            {
                var randChar = characters[random.Next(characters.Length)];
                sb.Append(randChar);
            }

            return sb.ToString();
        }

        protected override void Seed(DataAccessLayer.BroadwayBuilderContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            var userIds = Enumerable.Range(1, 100);
            foreach (var userId in userIds)
            {
                var random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                context.Users.AddOrUpdate(x => x.UserId, new User()
                {
                    UserId = userId,
                    Age = random.Next(18,99),
                    StreetAddress = GenerateName(20),
                    City = GenerateName(12),
                    Country = GenerateName(3),
                    FirstName = GenerateName(random.Next(6, 12)),
                    LastName = GenerateName(random.Next(5,15)),
                    DateCreated = DateTime.Now,
                    DateOfBirth = DateTime.Now - TimeSpan.FromDays(365 * 19),
                    isEnabled = true,
                    Username = GenerateEmailAddress(random.Next(5, 20)),
                    UserGuid = Guid.NewGuid(),
                    StateProvince = GenerateName(2),
                });
            }

            context.Theaters.AddOrUpdate(x => x.TheaterID,
                new Theater { TheaterID = 1, TheaterName = "Dramatic", CompanyName = "Company1", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 2, TheaterName = "Broadway", CompanyName = "Company2", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 3, TheaterName = "Alamo", CompanyName = "Company3", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "333-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 4, TheaterName = "Dreamworks", CompanyName = "Company4", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "444-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 5, TheaterName = "Pixar", CompanyName = "Company5", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "555-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 6, TheaterName = "Lucas", CompanyName = "Company6", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "666-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 7, TheaterName = "Smash Bros", CompanyName = "Company7", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "777-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 8, TheaterName = "Raven", CompanyName = "Company8", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "888-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 9, TheaterName = "Cinemark", CompanyName = "Company9", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "999-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 10, TheaterName = "MyTheater", CompanyName = "Company10", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-111-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 11, TheaterName = "Dramatic", CompanyName = "Company1", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 12, TheaterName = "Broadway", CompanyName = "Company2", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 13, TheaterName = "Alamo", CompanyName = "Company3", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "333-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 14, TheaterName = "Dreamworks", CompanyName = "Company4", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "444-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 15, TheaterName = "Pixar", CompanyName = "Company5", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "555-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 16, TheaterName = "Lucas", CompanyName = "Company6", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "666-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 17, TheaterName = "Smash Bros", CompanyName = "Company7", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "777-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 18, TheaterName = "Raven", CompanyName = "Company8", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "888-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 19, TheaterName = "Cinemark", CompanyName = "Company9", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "999-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 20, TheaterName = "MyTheater", CompanyName = "Company10", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-111-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 21, TheaterName = "Dramatic", CompanyName = "Company1", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 22, TheaterName = "Broadway", CompanyName = "Company2", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 23, TheaterName = "Alamo", CompanyName = "Company3", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "333-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 24, TheaterName = "Dreamworks", CompanyName = "Company4", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "444-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 25, TheaterName = "Pixar", CompanyName = "Company5", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "555-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 26, TheaterName = "Lucas", CompanyName = "Company6", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "666-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 27, TheaterName = "Smash Bros", CompanyName = "Company7", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "777-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 28, TheaterName = "Raven", CompanyName = "Company8", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "888-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 29, TheaterName = "Cinemark", CompanyName = "Company9", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "999-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 30, TheaterName = "MyTheater", CompanyName = "Company10", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-111-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 31, TheaterName = "Dramatic", CompanyName = "Company1", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 32, TheaterName = "Broadway", CompanyName = "Company2", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 33, TheaterName = "Alamo", CompanyName = "Company3", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "333-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 34, TheaterName = "Dreamworks", CompanyName = "Company4", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "444-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 35, TheaterName = "Pixar", CompanyName = "Company5", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "555-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 36, TheaterName = "Lucas", CompanyName = "Company6", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "666-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 37, TheaterName = "Smash Bros", CompanyName = "Company7", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "777-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 38, TheaterName = "Raven", CompanyName = "Company8", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "888-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 39, TheaterName = "Cinemark", CompanyName = "Company9", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "999-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 40, TheaterName = "MyTheater", CompanyName = "Company10", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-111-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 41, TheaterName = "Dramatic", CompanyName = "Company1", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 42, TheaterName = "Broadway", CompanyName = "Company2", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 43, TheaterName = "Alamo", CompanyName = "Company3", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "333-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 44, TheaterName = "Dreamworks", CompanyName = "Company4", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "444-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 45, TheaterName = "Pixar", CompanyName = "Company5", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "555-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 46, TheaterName = "Lucas", CompanyName = "Company6", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "666-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 47, TheaterName = "Smash Bros", CompanyName = "Company7", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "777-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 48, TheaterName = "Raven", CompanyName = "Company8", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "888-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 49, TheaterName = "Cinemark", CompanyName = "Company9", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "999-222-2222", DateCreated = DateTime.Now },
                new Theater { TheaterID = 50, TheaterName = "MyTheater", CompanyName = "Company10", StreetAddress = "street", City = "LA", State = "CA", Country = "USA", PhoneNumber = "222-111-2222", DateCreated = DateTime.Now }
                );

            context.TheaterJobPostings.AddOrUpdate(x => x.HelpWantedID,
                new TheaterJobPosting { HelpWantedID = 1, TheaterID = 1, DateCreated = new DateTime(2019, 01, 27), Position = "Actor", Description = "lengthy description", Title = "SOS", Hours = "20", Requirements = "some reqirements", JobType = "type of job" },
                new TheaterJobPosting { HelpWantedID = 2, TheaterID = 1, DateCreated = new DateTime(2019, 02, 11), Position = "Backstage", Description = "lengthy description", Title = "Help Please", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 3, TheaterID = 1, DateCreated = new DateTime(2019, 03, 24), Position = "Usher", Description = "lengthy description", Title = "Low Wages", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 4, TheaterID = 1, DateCreated = new DateTime(2018, 12, 19), Position = "Director", Description = "lengthy description", Title = "Just For Show", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 5, TheaterID = 1, DateCreated = new DateTime(2018, 11, 30), Position = "StageHands", Description = "lengthy description", Title = "Hiring", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 6, TheaterID = 1, DateCreated = new DateTime(2019, 01, 07), Position = "Stage Manager", Description = "lengthy description", Title = "Sign Up", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 7, TheaterID = 1, DateCreated = new DateTime(2019, 04, 22), Position = "Wardrobe Supevisor", Description = "lengthy description", Title = "Encore", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 8, TheaterID = 1, DateCreated = new DateTime(2018, 12, 25), Position = "Producer", Description = "lengthy description", Title = "Blah", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 9, TheaterID = 1, DateCreated = new DateTime(2019, 04, 01), Position = "Scenic Artist", Description = "lengthy description", Title = "Sleep", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 10, TheaterID = 1, DateCreated = new DateTime(2019, 03, 10), Position = "Dresser", Description = "lengthy description", Title = "Critic", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 11, TheaterID = 2, DateCreated = new DateTime(2019, 01, 27), Position = "Actor", Description = "lengthy description", Title = "sometitle", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 12, TheaterID = 2, DateCreated = new DateTime(2019, 03, 27), Position = "Backstage", Description = "lengthy description", Title = "Bulma", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 13, TheaterID = 2, DateCreated = new DateTime(2019, 02, 27), Position = "Actor", Description = "lengthy description", Title = "Vue", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 14, TheaterID = 2, DateCreated = new DateTime(2018, 12, 25), Position = "Stagehands", Description = "lengthy description", Title = "Developer", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 15, TheaterID = 2, DateCreated = new DateTime(2018, 11, 20), Position = "Stage Manager", Description = "lengthy description", Title = "Willy Wonka", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 16, TheaterID = 2, DateCreated = new DateTime(2018, 10, 10), Position = "Wardrobe Supervisor", Description = "lengthy description", Title = "Variables", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 17, TheaterID = 2, DateCreated = new DateTime(2018, 09, 29), Position = "Scenic Artist", Description = "lengthy description", Title = "Audition", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 18, TheaterID = 2, DateCreated = new DateTime(2018, 10, 31), Position = "Director", Description = "lengthy description", Title = "Discord", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 19, TheaterID = 2, DateCreated = new DateTime(2018, 12, 15), Position = "Producer", Description = "lengthy description", Title = "Github", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 20, TheaterID = 2, DateCreated = new DateTime(2019, 04, 01), Position = "Usher", Description = "lengthy description", Title = "Dumby Data", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 21, TheaterID = 3, DateCreated = new DateTime(2018, 11, 11), Position = "Dresser", Description = "lengthy description", Title = "Actual Data", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 22, TheaterID = 3, DateCreated = new DateTime(2019, 12, 12), Position = "Actor", Description = "lengthy description", Title = "Ping", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 23, TheaterID = 3, DateCreated = new DateTime(2019, 09, 20), Position = "Stagehands", Description = "lengthy description", Title = "Google", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 24, TheaterID = 3, DateCreated = new DateTime(2019, 04, 10), Position = "Backstage", Description = "lengthy description", Title = "Magicians", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 25, TheaterID = 3, DateCreated = new DateTime(2019, 01, 01), Position = "Stage Manager", Description = "lengthy description", Title = "Breaking Bad", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 26, TheaterID = 3, DateCreated = new DateTime(2019, 04, 12), Position = "Wardrobe Supervisor", Description = "lengthy description", Title = "Smash Bros", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 27, TheaterID = 3, DateCreated = new DateTime(2019, 02, 01), Position = "Scenic Artist", Description = "lengthy description", Title = "Switch", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 28, TheaterID = 3, DateCreated = new DateTime(2019, 03, 01), Position = "Director", Description = "lengthy description", Title = "Samsung", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 29, TheaterID = 3, DateCreated = new DateTime(2019, 02, 14), Position = "Director", Description = "lengthy description", Title = "Apple", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 30, TheaterID = 3, DateCreated = DateTime.Now, Position = "Producer", Description = "lengthy description", Title = "Title 2.0", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 31, TheaterID = 4, DateCreated = new DateTime(2019, 01, 27), Position = "Usher", Description = "lengthy description", Title = "Failing Theater", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 32, TheaterID = 4, DateCreated = new DateTime(2019, 02, 27), Position = "Dresser", Description = "lengthy description", Title = "Database", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 33, TheaterID = 4, DateCreated = new DateTime(2019, 03, 27), Position = "Actor", Description = "lengthy description", Title = "Audience", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 34, TheaterID = 4, DateCreated = new DateTime(2019, 01, 01), Position = "Stagehands", Description = "lengthy description", Title = "Well Well Well", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 35, TheaterID = 4, DateCreated = new DateTime(2018, 12, 15), Position = "Backstage", Description = "lengthy description", Title = "God Bless America", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 36, TheaterID = 4, DateCreated = new DateTime(2019, 04, 01), Position = "Stage Manager", Description = "lengthy description", Title = "The What", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 37, TheaterID = 4, DateCreated = new DateTime(2019, 09, 11), Position = "Wardrobe Supervisor", Description = "lengthy description", Title = "Built in", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 38, TheaterID = 4, DateCreated = new DateTime(2019, 10, 30), Position = "Scenic Artist", Description = "lengthy description", Title = "Vueify", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 39, TheaterID = 4, DateCreated = new DateTime(2018, 11, 30), Position = "Director", Description = "lengthy description", Title = "Bulma", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 40, TheaterID = 4, DateCreated = DateTime.Now, Position = "Producer", Description = "lengthy description", Title = "Yea", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 41, TheaterID = 5, DateCreated = DateTime.Now, Position = "Usher", Description = "lengthy description", Title = "I AM HERE", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 42, TheaterID = 5, DateCreated = new DateTime(2019, 04, 01), Position = "Dresser", Description = "lengthy description", Title = "Deku", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 43, TheaterID = 5, DateCreated = new DateTime(2019, 01, 27), Position = "Actor", Description = "lengthy description", Title = "All Might", Hours = "20", Requirements = "some reqirements", JobType = "Full Time" },
                new TheaterJobPosting { HelpWantedID = 44, TheaterID = 5, DateCreated = new DateTime(2019, 02, 14), Position = "Stagehands", Description = "lengthy description", Title = "Goku", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 45, TheaterID = 5, DateCreated = new DateTime(2019, 01, 01), Position = "Producer", Description = "lengthy description", Title = "Vegeta", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 46, TheaterID = 5, DateCreated = new DateTime(2019, 04, 19), Position = "Director", Description = "lengthy description", Title = "Gohan", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 47, TheaterID = 5, DateCreated = new DateTime(2018, 12, 25), Position = "Usher", Description = "lengthy description", Title = "Picolo", Hours = "20", Requirements = "some reqirements", JobType = "Part Time" },
                new TheaterJobPosting { HelpWantedID = 48, TheaterID = 5, DateCreated = new DateTime(2018, 09, 21), Position = "Backstage", Description = "lengthy description", Title = "Beerus", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 49, TheaterID = 5, DateCreated = new DateTime(2018, 10, 10), Position = "Usher", Description = "lengthy description", Title = "Whis", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" },
                new TheaterJobPosting { HelpWantedID = 50, TheaterID = 5, DateCreated = DateTime.Now, Position = "Actor", Description = "lengthy description", Title = "Broly", Hours = "20", Requirements = "some reqirements", JobType = "Seasonal" }
                );

            context.Productions.AddOrUpdate(x => x.ProductionID,
                new Production { ProductionID = 1, TheaterID = 1, ProductionName = "Production Name1", DirectorFirstName = "DiectorF1", DirectorLastName = "DirectorL1", StateProvince = "state1", Street = "street1", City = "city1", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 2, TheaterID = 1, ProductionName = "Production Name2", DirectorFirstName = "DiectorF2", DirectorLastName = "DirectorL2", StateProvince = "state2", Street = "street2", City = "city2", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 3, TheaterID = 1, ProductionName = "Production Name3", DirectorFirstName = "DiectorF3", DirectorLastName = "DirectorL3", StateProvince = "state3", Street = "street3", City = "city3", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 4, TheaterID = 1, ProductionName = "Production Name4", DirectorFirstName = "DiectorF4", DirectorLastName = "DirectorL4", StateProvince = "state4", Street = "street4", City = "city4", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 5, TheaterID = 1, ProductionName = "Production Name5", DirectorFirstName = "DiectorF5", DirectorLastName = "DirectorL5", StateProvince = "state5", Street = "street5", City = "city5", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 6, TheaterID = 1, ProductionName = "Production Name6", DirectorFirstName = "DiectorF6", DirectorLastName = "DirectorL6", StateProvince = "state6", Street = "street6", City = "city6", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 7, TheaterID = 1, ProductionName = "Production Name7", DirectorFirstName = "DiectorF7", DirectorLastName = "DirectorL7", StateProvince = "state7", Street = "street7", City = "city7", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 8, TheaterID = 1, ProductionName = "Production Name8", DirectorFirstName = "DiectorF8", DirectorLastName = "DirectorL8", StateProvince = "state8", Street = "street8", City = "city8", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 9, TheaterID = 1, ProductionName = "Production Name9", DirectorFirstName = "DiectorF9", DirectorLastName = "DirectorL9", StateProvince = "state9", Street = "street9", City = "city9", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 10, TheaterID = 1, ProductionName = "Production Name10", DirectorFirstName = "DiectorF10", DirectorLastName = "DirectorL10", StateProvince = "state10", Street = "street10", City = "city10", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 11, TheaterID = 2, ProductionName = "Production Name1", DirectorFirstName = "DiectorF1", DirectorLastName = "DirectorL1", StateProvince = "state1", Street = "street1", City = "city1", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 12, TheaterID = 2, ProductionName = "Production Name2", DirectorFirstName = "DiectorF2", DirectorLastName = "DirectorL2", StateProvince = "state2", Street = "street2", City = "city2", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 13, TheaterID = 2, ProductionName = "Production Name3", DirectorFirstName = "DiectorF3", DirectorLastName = "DirectorL3", StateProvince = "state3", Street = "street3", City = "city3", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 14, TheaterID = 2, ProductionName = "Production Name4", DirectorFirstName = "DiectorF4", DirectorLastName = "DirectorL4", StateProvince = "state4", Street = "street4", City = "city4", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 15, TheaterID = 2, ProductionName = "Production Name5", DirectorFirstName = "DiectorF5", DirectorLastName = "DirectorL5", StateProvince = "state5", Street = "street5", City = "city5", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 16, TheaterID = 2, ProductionName = "Production Name6", DirectorFirstName = "DiectorF6", DirectorLastName = "DirectorL6", StateProvince = "state6", Street = "street6", City = "city6", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 17, TheaterID = 2, ProductionName = "Production Name7", DirectorFirstName = "DiectorF7", DirectorLastName = "DirectorL7", StateProvince = "state7", Street = "street7", City = "city7", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 18, TheaterID = 2, ProductionName = "Production Name8", DirectorFirstName = "DiectorF8", DirectorLastName = "DirectorL8", StateProvince = "state8", Street = "street8", City = "city8", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 19, TheaterID = 2, ProductionName = "Production Name9", DirectorFirstName = "DiectorF9", DirectorLastName = "DirectorL9", StateProvince = "state9", Street = "street9", City = "city9", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 20, TheaterID = 2, ProductionName = "Production Name10", DirectorFirstName = "DiectorF10", DirectorLastName = "DirectorL10", StateProvince = "state10", Street = "street10", City = "city10", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 21, TheaterID = 3, ProductionName = "Production Name1", DirectorFirstName = "DiectorF1", DirectorLastName = "DirectorL1", StateProvince = "state1", Street = "street1", City = "city1", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 22, TheaterID = 3, ProductionName = "Production Name2", DirectorFirstName = "DiectorF2", DirectorLastName = "DirectorL2", StateProvince = "state2", Street = "street2", City = "city2", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 23, TheaterID = 3, ProductionName = "Production Name3", DirectorFirstName = "DiectorF3", DirectorLastName = "DirectorL3", StateProvince = "state3", Street = "street3", City = "city3", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 24, TheaterID = 3, ProductionName = "Production Name4", DirectorFirstName = "DiectorF4", DirectorLastName = "DirectorL4", StateProvince = "state4", Street = "street4", City = "city4", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 25, TheaterID = 3, ProductionName = "Production Name5", DirectorFirstName = "DiectorF5", DirectorLastName = "DirectorL5", StateProvince = "state5", Street = "street5", City = "city5", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 26, TheaterID = 3, ProductionName = "Production Name6", DirectorFirstName = "DiectorF6", DirectorLastName = "DirectorL6", StateProvince = "state6", Street = "street6", City = "city6", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 27, TheaterID = 3, ProductionName = "Production Name7", DirectorFirstName = "DiectorF7", DirectorLastName = "DirectorL7", StateProvince = "state7", Street = "street7", City = "city7", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 28, TheaterID = 3, ProductionName = "Production Name8", DirectorFirstName = "DiectorF8", DirectorLastName = "DirectorL8", StateProvince = "state8", Street = "street8", City = "city8", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 29, TheaterID = 3, ProductionName = "Production Name9", DirectorFirstName = "DiectorF9", DirectorLastName = "DirectorL9", StateProvince = "state9", Street = "street9", City = "city9", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 30, TheaterID = 3, ProductionName = "Production Name10", DirectorFirstName = "DiectorF10", DirectorLastName = "DirectorL10", StateProvince = "state10", Street = "street10", City = "city10", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 31, TheaterID = 4, ProductionName = "Production Name1", DirectorFirstName = "DiectorF1", DirectorLastName = "DirectorL1", StateProvince = "state1", Street = "street1", City = "city1", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 32, TheaterID = 4, ProductionName = "Production Name2", DirectorFirstName = "DiectorF2", DirectorLastName = "DirectorL2", StateProvince = "state2", Street = "street2", City = "city2", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 33, TheaterID = 4, ProductionName = "Production Name3", DirectorFirstName = "DiectorF3", DirectorLastName = "DirectorL3", StateProvince = "state3", Street = "street3", City = "city3", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 34, TheaterID = 4, ProductionName = "Production Name4", DirectorFirstName = "DiectorF4", DirectorLastName = "DirectorL4", StateProvince = "state4", Street = "street4", City = "city4", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 35, TheaterID = 4, ProductionName = "Production Name5", DirectorFirstName = "DiectorF5", DirectorLastName = "DirectorL5", StateProvince = "state5", Street = "street5", City = "city5", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 36, TheaterID = 4, ProductionName = "Production Name6", DirectorFirstName = "DiectorF6", DirectorLastName = "DirectorL6", StateProvince = "state6", Street = "street6", City = "city6", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 37, TheaterID = 4, ProductionName = "Production Name7", DirectorFirstName = "DiectorF7", DirectorLastName = "DirectorL7", StateProvince = "state7", Street = "street7", City = "city7", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 38, TheaterID = 4, ProductionName = "Production Name8", DirectorFirstName = "DiectorF8", DirectorLastName = "DirectorL8", StateProvince = "state8", Street = "street8", City = "city8", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 39, TheaterID = 4, ProductionName = "Production Name9", DirectorFirstName = "DiectorF9", DirectorLastName = "DirectorL9", StateProvince = "state9", Street = "street9", City = "city9", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 40, TheaterID = 4, ProductionName = "Production Name10", DirectorFirstName = "DiectorF10", DirectorLastName = "DirectorL10", StateProvince = "state10", Street = "street10", City = "city10", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 41, TheaterID = 5, ProductionName = "Production Name1", DirectorFirstName = "DiectorF1", DirectorLastName = "DirectorL1", StateProvince = "state1", Street = "street1", City = "city1", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 42, TheaterID = 5, ProductionName = "Production Name2", DirectorFirstName = "DiectorF2", DirectorLastName = "DirectorL2", StateProvince = "state2", Street = "street2", City = "city2", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 43, TheaterID = 5, ProductionName = "Production Name3", DirectorFirstName = "DiectorF3", DirectorLastName = "DirectorL3", StateProvince = "state3", Street = "street3", City = "city3", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 44, TheaterID = 5, ProductionName = "Production Name4", DirectorFirstName = "DiectorF4", DirectorLastName = "DirectorL4", StateProvince = "state4", Street = "street4", City = "city4", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 45, TheaterID = 5, ProductionName = "Production Name5", DirectorFirstName = "DiectorF5", DirectorLastName = "DirectorL5", StateProvince = "state5", Street = "street5", City = "city5", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 46, TheaterID = 5, ProductionName = "Production Name6", DirectorFirstName = "DiectorF6", DirectorLastName = "DirectorL6", StateProvince = "state6", Street = "street6", City = "city6", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 47, TheaterID = 5, ProductionName = "Production Name7", DirectorFirstName = "DiectorF7", DirectorLastName = "DirectorL7", StateProvince = "state7", Street = "street7", City = "city7", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 48, TheaterID = 5, ProductionName = "Production Name8", DirectorFirstName = "DiectorF8", DirectorLastName = "DirectorL8", StateProvince = "state8", Street = "street8", City = "city8", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 49, TheaterID = 5, ProductionName = "Production Name9", DirectorFirstName = "DiectorF9", DirectorLastName = "DirectorL9", StateProvince = "state9", Street = "street9", City = "city9", Country = "USA", Zipcode = "90044" },
                new Production { ProductionID = 50, TheaterID = 5, ProductionName = "Production Name10", DirectorFirstName = "DiectorF10", DirectorLastName = "DirectorL10", StateProvince = "state10", Street = "street10", City = "city10", Country = "USA", Zipcode = "90044" }
                );

            context.ProductionDateTimes.AddOrUpdate(x => x.ProductionDateTimeId,
                new ProductionDateTime { ProductionDateTimeId = 1, ProductionID = 1, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 2, ProductionID = 1, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 3, ProductionID = 1, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 4, ProductionID = 2, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 5, ProductionID = 2, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 6, ProductionID = 2, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 7, ProductionID = 3, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 8, ProductionID = 3, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 9, ProductionID = 3, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 10, ProductionID = 4, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 11, ProductionID = 4, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 12, ProductionID = 4, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 13, ProductionID = 5, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 14, ProductionID = 5, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 15, ProductionID = 5, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 16, ProductionID = 6, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 17, ProductionID = 6, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 18, ProductionID = 6, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 19, ProductionID = 7, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 20, ProductionID = 7, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 21, ProductionID = 7, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 22, ProductionID = 8, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 23, ProductionID = 8, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 24, ProductionID = 8, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 25, ProductionID = 9, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 26, ProductionID = 9, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 27, ProductionID = 9, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 28, ProductionID = 10, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 29, ProductionID = 10, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 30, ProductionID = 10, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 31, ProductionID = 11, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 32, ProductionID = 11, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 33, ProductionID = 11, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 34, ProductionID = 12, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 35, ProductionID = 12, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 36, ProductionID = 12, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 37, ProductionID = 13, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 38, ProductionID = 13, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 39, ProductionID = 13, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 40, ProductionID = 14, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 41, ProductionID = 14, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 42, ProductionID = 14, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 43, ProductionID = 15, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 44, ProductionID = 15, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 45, ProductionID = 15, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 46, ProductionID = 16, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 47, ProductionID = 16, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 48, ProductionID = 16, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 49, ProductionID = 16, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 50, ProductionID = 16, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 51, ProductionID = 17, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 52, ProductionID = 18, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 53, ProductionID = 19, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 54, ProductionID = 20, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 55, ProductionID = 21, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 56, ProductionID = 22, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 57, ProductionID = 23, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 58, ProductionID = 24, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 59, ProductionID = 25, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 60, ProductionID = 26, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 61, ProductionID = 27, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 62, ProductionID = 28, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 63, ProductionID = 29, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 64, ProductionID = 30, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 65, ProductionID = 31, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 66, ProductionID = 32, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 67, ProductionID = 33, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 68, ProductionID = 34, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 69, ProductionID = 35, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 70, ProductionID = 36, Date = new DateTime(2019, 01, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 71, ProductionID = 37, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 72, ProductionID = 38, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 73, ProductionID = 39, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 74, ProductionID = 40, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 75, ProductionID = 41, Date = new DateTime(2019, 03, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 76, ProductionID = 42, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 77, ProductionID = 43, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 78, ProductionID = 44, Date = new DateTime(2019, 04, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 79, ProductionID = 45, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(5, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 80, ProductionID = 46, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(6, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 81, ProductionID = 47, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(2, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 82, ProductionID = 48, Date = new DateTime(2019, 02, 27), Time = new TimeSpan(3, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 83, ProductionID = 49, Date = new DateTime(2019, 05, 27), Time = new TimeSpan(4, 30, 00) },
                new ProductionDateTime { ProductionDateTimeId = 84, ProductionID = 50, Date = new DateTime(2019, 05, 27), Time = new TimeSpan(5, 30, 00) }
               );
            base.Seed(context);
            
            

            
        }
    }
}
