using System;
using System.Collections.Generic;
using System.Text;

namespace SSH_USER_CORE
{
    public static class UserContainer
    {
        public static List<User> Users = new List<User>();

        public static User getUserById(string id) {
            User found = UserContainer.Users.Find(item => item.UserId == id);
            return found;
        }
    }
}
