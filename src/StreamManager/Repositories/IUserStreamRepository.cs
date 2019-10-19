using System.Threading.Tasks;

namespace StreamManager.Repositories
{
    public interface IUserStreamRepository
    {
        Task IncrementUserStreamCount(string username);

        Task DecrementUserStreamCount(string username);

        Task<int> GetUserStreamCount(string username);
    }
}