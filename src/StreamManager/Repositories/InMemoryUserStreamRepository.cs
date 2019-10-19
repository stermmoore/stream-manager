using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamManager.Repositories
{
    public class InMemoryUserStreamRepository : IUserStreamRepository
    {
        private Dictionary<string, int> _userStreamCounts;
        public InMemoryUserStreamRepository()
        {
            _userStreamCounts = new Dictionary<string, int>();
        }

        public Task<int> GetUserStreamCount(string username)
        {
            if(_userStreamCounts.ContainsKey(username))
                return Task.FromResult(_userStreamCounts[username]);

            return Task.FromResult(0);
        }

        public Task IncrementUserStreamCount(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task DecrementUserStreamCount(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}