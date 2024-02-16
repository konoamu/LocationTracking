using LocationTracking.Data.Dto;
using LocationTracking.Data.Entities;

namespace LocationTracking.Core.Interfaces
{
    public interface IUserService
    {
        UserDto GetUserById(string id);
        IEnumerable<User> GetAllUsers();
        User AddUser(UserDto user);
    }
}
