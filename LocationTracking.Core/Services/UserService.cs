using LocationTracking.Data.Dto;
using LocationTracking.Data.Entities;
using LocationTracking.Core.Interfaces;

namespace LocationTracking.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto GetUserById(string id)
        {
            var user = _userRepository.GetUserById(id);
            return new UserDto { Name = user.Name };
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User AddUser(UserDto user)
        {
            var entity = new User { Id= Guid.NewGuid().ToString(), Name = user.Name };
            return _userRepository.AddUser(entity);
        }
    }
}
