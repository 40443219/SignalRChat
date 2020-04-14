using System.Collections.Generic;
using System.Threading.Tasks;

using SignalRChat.Models;

namespace SignalRChat.Services
{
    public interface IUserService
    {
        Task<List<UserModel>> AddList(UserModel user);
        Task<List<UserModel>> RemoveList(string connectionId);
        Task<UserModel> FindUser(string connectionId);
    }
}