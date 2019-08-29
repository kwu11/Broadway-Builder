using DataAccessLayer.Logging;
using DataAccessLayer.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UsageLogService
    {
        private MongoDbContext mongoDbContext;
        public UsageLogService()
        {
            mongoDbContext = new MongoDbContext();
        }

        public void CreateUsageLog(UsageLog usageLog)
        {
            mongoDbContext.UsageLogs.InsertOneAsync(usageLog);
        }

        public List<UsageLog> GetUsageLogs(DateTime minimumDate, DateTime maximumDate)
        {
            List<UsageLog> errorLogs = mongoDbContext.UsageLogs.Find(log => log.TimeStamp >= minimumDate & log.TimeStamp <= maximumDate).ToList();
            return errorLogs;
        }

        public void DeleteUsageLogs(DateTime minimumDate, DateTime maximumDate)
        {
            mongoDbContext.UsageLogs.DeleteManyAsync(log => log.TimeStamp >= minimumDate & log.TimeStamp <= maximumDate);
        }
    }
}
