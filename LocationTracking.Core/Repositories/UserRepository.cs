using LocationTracking.Data.Entities;
using LocationTracking.Core.Interfaces;
using MongoDB.Driver;

namespace LocationTracking.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users");
        }

        public User GetUserById(string id)
        {
            return _userCollection.Find(u => u.Id == id).FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userCollection.Find(_ => true).ToList();
        }

        public User AddUser(User user)
        {
            _userCollection.InsertOne(user);
            return user;
        }
    }

}
