using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AnalogDropbox.DataAccess;
using AnalogDropbox.DataAccess.Sql;
using AnalogDropbox.Model;

namespace AnalogDropbox.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private const string ConnectionString = "Data Source=SQLEXPRESS2016;Initial Catalog=DropboxAnalog;Integrated Security=False;User ID=dropboxuser; Password=dropboxuser";
        private IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private IFilesRepository _filesRepository;

        public UsersController()
        {
            _filesRepository = new FilesRepository(ConnectionString, _usersRepository);
        }

        /// <summary>
		/// Create
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost]
        [Route("api/users")]
        public User CreateUser([FromBody]User user)
        {
            return _usersRepository.Add(user.FirstName, user.SecondName, user.Email);
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public User GetUser(Guid id)
        {
            return _usersRepository.Get(id);
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        public void DeleteUser(Guid id)
        {
            //Log.Logger.ServiceLog.Info("Delete user with id: {0}", id);
            _usersRepository.Delete(id);
        }

        [Route("api/users/{id}/files")]
        [HttpGet]
        public IEnumerable<File> GetUserFiles(Guid id)
        {
            return _filesRepository.GetUserFiles(id);
        }

        [HttpPut]
        [Route("api/users/{id}")]
        public User UpdateUser(Guid id, [FromBody] User user)
        {
            return _usersRepository.Update(id, user.FirstName, user.SecondName);
        }

        /// <summary>
        /// Уточнить
        /// </summary>
        [HttpPost]
        [Route("api/users/{id}/share/{fileId}/{access}")]
        public void SharedFile(Guid id, Guid fileId, bool access, [FromBody] User user)
        {
            _filesRepository.Shared(id, fileId, user.Id, access);
        }
    }
}