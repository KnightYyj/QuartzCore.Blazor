using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QuartzCore.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.MongoDB.LogEntity
{
    [MongoDBTable("QzRunLogMo")]
    [BsonIgnoreExtraElements]
    public class QzRunLogMoEntity: IBaseMEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("taskqz_id")]
        public int TasksQzId { get; set; }

        [BsonElement("app_id")]
        public string AppId { get; set; }

        /// <summary>
        ///  Normal = 0,Warn = 1
        /// </summary>
        [BsonElement("log_type")]
        public int LogType { get; set; }
        /// <summary>
        /// Job执行开始时间
        /// </summary>
        [BsonElement("log_time")]
        public DateTime? LogTime { get; set; }

        [BsonElement("log_text")]
        public string LogText { get; set; }

        /// <summary>
        /// 耗时毫秒
        /// </summary>
        [BsonElement("milliseconds")]
        public string Milliseconds { get; set; }
    }
}
