using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using SignalRChat.Models;

namespace SignalRChat.Services
{
    public class UserService : IUserService
    {
        private readonly List<UserModel> _userList;

        public UserService()
        {
            _userList = new List<UserModel>();
        }

        public async Task<List<UserModel>> AddList(UserModel user)
        {
            _userList.Add(user);
            return await Task.Run(() => _userList.Where(x => x.groupName == user.groupName).ToList());
        }

        public async Task<List<UserModel>> RemoveList(string connectionId)
        {
            var user = _userList.FirstOrDefault(x => x.connectionId == connectionId);
            if(user != null) {
                _userList.Remove(user);
            }

            return await Task.Run(() => _userList.Where(x => x.groupName == user.groupName).ToList());
        }

        public async Task<UserModel> FindUser(string connectionId)
        {
            return await Task.Run(() => _userList.FirstOrDefault(x => x.connectionId == connectionId));
        }
    }
}