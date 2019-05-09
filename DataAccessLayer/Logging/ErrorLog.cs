using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Logging
{
    public class ErrorLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("User")]
        public int UserId { get; set; }

        [BsonElement("HttpMethod")]
        public string HttpMethod { get; set; }

        [BsonElement("Request")]
        public string Request { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("StackTrace")]
        public string StackTrace { get; set; }

        [BsonElement("TargetSite")]
        public string TargetSite { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp{get;set;}

        [BsonElement("AdditionalInformation")]
        public Dictionary<string, object> AdditionalInfo { get; set; }


        public ErrorLog(int userId, string httpMethod, string request, string message, string stack, string targetSite, DateTime timeStamp)
        {
            this.UserId = userId;
            this.HttpMethod = httpMethod;
            this.Request = request;
            this.Message = message;
            this.StackTrace = stack;
            this.TargetSite = targetSite;
            this.TimeStamp = timeStamp;
        }
    }
}
