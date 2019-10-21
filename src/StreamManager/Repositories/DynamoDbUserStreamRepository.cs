using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using StreamManager.Models;

namespace StreamManager.Repositories
{
    public class DynamoDbUserStreamReporitory : IUserStreamRepository
    {
        private readonly ILogger<DynamoDbUserStreamReporitory> _logger;
        private readonly IAmazonDynamoDB _amazonDynamoDB;
        public DynamoDbUserStreamReporitory(ILogger<DynamoDbUserStreamReporitory> logger, IAmazonDynamoDB amazonDynamoDB)
        {
            _logger = logger;
            _amazonDynamoDB = amazonDynamoDB;
        }

        public async Task DecrementUserStreamCount(string username)
        {
            var user = await GetUser(username);
            if (user.StreamCount>0)
            {
                user.StreamCount--;

                await SaveUser(user);
            }
        }

        public async Task<int> GetUserStreamCount(string username)
        {
            var user = await GetUser(username);

            return user== null? 0 : user.StreamCount; 
        }

        public async Task IncrementUserStreamCount(string username)
        {
            var user = await GetUser(username);

            user.StreamCount++;

            await SaveUser(user);
        }

        private async Task<UserStreams> GetUser(string username)
        {
            _logger.LogInformation("Getting UserStreams object for {0}", username);

            var context = GetContext();

            var user = await context.LoadAsync<UserStreams>(username);               

            if(user == null)
                user = new UserStreams{ UserName=username, StreamCount = 0};

            return user;
        }

        private async Task SaveUser(UserStreams user)
        {
            _logger.LogInformation("Saving UserStreams object for {0}", user.UserName);

            using(var context = GetContext())
            {
                await context.SaveAsync<UserStreams>(user);  
            }
        }

        private DynamoDBContext GetContext()
        {
            return new DynamoDBContext(_amazonDynamoDB, 
                new DynamoDBOperationConfig { ConsistentRead = true});
        }
    }
}