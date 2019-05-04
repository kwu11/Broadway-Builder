using DataAccessLayer.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.MongoDb
{
    public class MongoDbContext
    {
        public IMongoDatabase Database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb+srv://F5MongodbUser:F5BroadwayBuilder@broadwaybuilder-e1ziq.mongodb.net/test?retryWrites=true");
            Database = client.GetDatabase("BroadwayBuilder");
        }

        public IMongoCollection<ErrorLog> ErrorLogs => Database.GetCollection<ErrorLog>("errorLogs");
    }
}
