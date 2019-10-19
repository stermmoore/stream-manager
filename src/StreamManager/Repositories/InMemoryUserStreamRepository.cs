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
            if(_userStreamCounts.ContainsKey(username))
                _userStreamCounts[username]++;
            else
                _userStreamCounts[username] = 1;

            return Task.CompletedTask;
        }

        public Task DecrementUserStreamCount(string username)
        {
            if(_userStreamCounts.ContainsKey(username) && _userStreamCounts[username] > 0)
                _userStreamCounts[username]--;

            return Task.CompletedTask;
        }
    }
}