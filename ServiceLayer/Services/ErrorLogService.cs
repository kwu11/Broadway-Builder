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
            mongoDbContext.ErrorLogs.InsertOne(errorLog);
        }

        //public void DeleteErrorLog(string id)
        //{
        //    mongoDbContext.ErrorLogs.FindOneAndDelete({ _id:id});
        //}
    }
}
