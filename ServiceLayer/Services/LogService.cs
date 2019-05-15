using DataAccessLayer.Logging;
using DataAccessLayer.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceLayer.Services
{
    public class LogService
    {
        public LogService()
        {
        }

        //public void LogError(HttpContext httpContext,Exception exception)
        //{
        //    string request = httpContext.Request.Url.ToString();
        //    string httpMethod = httpContext.Request.HttpMethod;
        //    DateTime time = httpContext.Timestamp;
        //    string message = exception.Message;
        //    string stackTrace = exception.StackTrace;
        //    string targetSite = exception.TargetSite.ToString();

        //    ErrorLog log = new ErrorLog(1, httpMethod, request, message, stackTrace, targetSite, time);
        //    ErrorLogService errorLogService = new ErrorLogService();
        //    errorLogService.CreateErrorLog(log);
        //}
        public IEnumerable GetErrorLogs(DateTime minimumDate, DateTime maximumDate)
        {
            ErrorLogService errorLogService = new ErrorLogService();
            return errorLogService.GetErrorLogs(minimumDate, maximumDate);
        }
        public void TestLogger()
        {
            throw new Exception("Testing Logger");
        }
    }
}
