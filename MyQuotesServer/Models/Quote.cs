using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyQuotesServer.Models
{
    public class Quote
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Data { get; set; }
        public string? Author { get; set; }
        public string? OriginType { get; set; }
        public string? OriginName { get; set; }
        public int? Page { get; set; }
        public List<string> Labels { get; set; }
    }
}
