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


    }
}