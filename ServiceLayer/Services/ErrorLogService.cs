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
    public class ErrorLogService
    {
        private MongoDbContext mongoDbContext;
        public ErrorLogService()
        {
            mongoDbContext = new MongoDbContext();
        }

        public void CreateErrorLog(ErrorLog errorLog)
        {
            mongoDbContext.ErrorLogs.InsertOneAsync(errorLog);
        }

        public List<ErrorLog> GetErrorLogs(DateTime minimumDate, DateTime maximumDate)
        {
            List<ErrorLog> errorLogs = mongoDbContext.ErrorLogs.Find(log => log.TimeStamp>=minimumDate & log.TimeStamp<=maximumDate).ToList();
            return errorLogs;
        }

        public void DeleteErrorLogs(DateTime minimumDate, DateTime maximumDate)
        {
            mongoDbContext.ErrorLogs.DeleteManyAsync(log => log.TimeStamp >= minimumDate & log.TimeStamp <= maximumDate);
        }
    }
}
