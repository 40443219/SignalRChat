using System;

namespace SignalRChat.Models
{
    public class UserModel
    {
        public string connectionId { get; set; }
        public string username { get; set; }
        public string groupName { get; set;}
        public DateTime onlineTime { get; set; }
    }
}