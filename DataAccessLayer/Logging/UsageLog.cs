using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Logging
{
    public class UsageLog
    {
        public UsageLog()
        {
            TimeStamp = DateTime.UtcNow;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; private set; }
        // WHERE
        [BsonElement("HttpMethod")]
        public string HttpMethod { get; set; }

        [BsonElement("Request")]
        public string Request { get; set; }

        [BsonElement("Product")]
        public string Product { get; set; }

        //[BsonElement("Location")]
        //public string Location { get; set; }

        //[BsonElement("HostName")]
        //public string Hostname { get; set; }

        [BsonElement("IPAddress")]
        public string IPAddress { get; set; }

        // WHO
        [BsonElement("User")]
        public int UserId { get; set; }

        [BsonElement("AdditionalInformation")]
        public Dictionary<string, object> AdditionalInfo { get; set; }  // everything else

    }
}
