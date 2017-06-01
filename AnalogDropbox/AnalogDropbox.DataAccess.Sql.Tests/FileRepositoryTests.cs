using System;
using System.Linq;
using System.Text;
using AnalogDropbox.Model;
using AnalogDropbox.DataAccess;
using AnalogDropbox.DataAccess.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dropbox.DataAccess.Sql.Tests
{
    [TestClass]
    public class FilesRepositoryTests
    {
        private const string ConnectionString = "Data Source=SQLEXPRESS2016;Initial Catalog=DropboxAnalog;Integrated Security=False;User ID=dropboxuser; Password=dropboxuser";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;

        private User TestUserOwner { get; set; }
        private User TestUser { get; set; }

        public FilesRepositoryTests()
        {
            _filesRepository = new FilesRepository(ConnectionString, _usersRepository);
        }

        [TestInitialize]
        public void Init()
        {
            TestUserOwner = _usersRepository.Add("testFirstName", "testSecondName", "test@test.com");
            TestUser = _usersRepository.Add("testUser2", "testUser2", "test2@test.com");
        }

        [TestCleanup]
        public void Clean()
        {
            if (TestUserOwner != null)
            {
                foreach (var file in _filesRepository.GetUserFiles(TestUserOwner.Id))
                    _filesRepository.Delete(file.Id);
                _usersRepository.Delete(TestUserOwner.Id);
            }
            if (TestUser != null)
            {
                foreach (var file in _filesRepository.GetUserFiles(TestUserOwner.Id))
                    _filesRepository.Delete(file.Id);
                _usersRepository.Delete(TestUserOwner.Id);
            }
        }

        [TestMethod]
        public void ShouldCreateAndGetFile()
        {
            //arrange
            var file = new File
            {
                Name = "testFile",
                Owner = TestUserOwner,
                CreationTime = DateTime.Now,
                LastWriteTime = DateTime.Now
            };
            //act
            var newFile = _filesRepository.Add(file);
            var result = _filesRepository.GetInfo(newFile.Id);
            //asserts
            Assert.AreEqual(file.Owner.Id, result.Owner.Id);
            Assert.AreEqual(file.Name, result.Name);
        }

        [TestMethod]
        public void ShouldUpdateFileContent()
        {
            //arrange
            var file = new File
            {
                Name = "testFile with content",
                Owner = TestUserOwner,
                CreationTime = DateTime.Now,
                LastWriteTime = DateTime.Now
            };
            var content = Encoding.UTF8.GetBytes("Hello");
            var newFile = _filesRepository.Add(file);
            //act
            _filesRepository.UpdateContent(newFile.Id, content);
            var resultContent = _filesRepository.GetContent(newFile.Id);
            //asserts
            Assert.IsTrue(content.SequenceEqual(resultContent));
        }

        [TestMethod]
        public void ShouldGetUserFiles()
        {
            //arrange
            var file = new File
            {
                Name = "testFile",
                Owner = TestUserOwner,
                CreationTime = DateTime.Now,
                LastWriteTime = DateTime.Now
            };
            var newFile = _filesRepository.Add(file);
            //act
            var userFiles = _filesRepository.GetUserFiles(TestUserOwner.Id);
            //asserts
            Assert.IsTrue(userFiles != null && userFiles.Count() > 0);
        }

        [TestMethod]
        public void ShouldSharedFiles()
        {
            var file = new File
            {
                Name = "testFile",
                Owner = TestUserOwner,
                CreationTime = DateTime.Now,
                LastWriteTime = DateTime.Now
            };
            var newFile = _filesRepository.Add(file);
            Share share = new Share
            {
                FileId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                PartOwnerId = Guid.NewGuid(),
                ReadOnlyAccess = true
            };
            _filesRepository.Shared(share);
            //act
            var userFiles = _filesRepository.GetUserFiles(TestUser.Id);
            //asserts
            Assert.IsTrue(userFiles != null && userFiles.Count() > 0);
        }
    }
}
