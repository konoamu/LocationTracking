using LocationTracking.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationTracking.Core.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(string id);
        IEnumerable<User> GetAllUsers();
        User AddUser(User user);
    }
}
