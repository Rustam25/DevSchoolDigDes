using AnalogDropbox.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalogDropbox.DataAccess.Sql.Tests
{
    [TestClass]
    public class UserRepositoryTest
    {
        private const string ConnectionString = "Data Source=SQLEXPRESS2016;Initial Catalog=DropboxAnalog;Integrated Security=False;User ID=dropboxuser; Password=dropboxuser";
        private readonly IUsersRepository _usersRepository;

        private User testUser = null;

        public UserRepositoryTest()
        {
            _usersRepository = new UsersRepository(ConnectionString);
        }

        [TestInitialize]
        public void Init()
        {

        }

        [TestCleanup]
        public void Clean()
        {
            if (testUser != null)
                _usersRepository.Delete(testUser.Id);
        }

        [TestMethod]
        public void ShouldCreateUpdateAndGetUser()
        {
            var newUser = _usersRepository.Add("testFirstName", "testSecondName", "test@test.com");
            testUser = _usersRepository.Get(newUser.Id);
            Assert.IsTrue(newUser.Id == testUser.Id && newUser.FirstName == testUser.FirstName && newUser.SecondName == testUser.SecondName && newUser.Email == testUser.Email);

            newUser = _usersRepository.Update(testUser.Id, "updatedFirstName", "updatedSecondName");
            testUser = _usersRepository.Get(testUser.Id);
            Assert.IsTrue(newUser.Id == testUser.Id && newUser.FirstName == testUser.FirstName && newUser.SecondName == testUser.SecondName && newUser.Email == testUser.Email);
        }

    }
}
