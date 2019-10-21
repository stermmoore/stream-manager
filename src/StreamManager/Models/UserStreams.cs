using System;
using Amazon.DynamoDBv2.DataModel;

namespace StreamManager.Models
{
    [DynamoDBTable("user-streams")]
    public class UserStreams 
    {
        [DynamoDBHashKey("userName")]
        public string UserName {get; set;}
        [DynamoDBProperty("streamCount")]
        public int StreamCount {get; set;}
    }
}