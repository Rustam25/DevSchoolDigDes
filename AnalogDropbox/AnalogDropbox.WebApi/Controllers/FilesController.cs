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
    public class FilesController : ApiController
    {
        private readonly string ConnectionString;
        private readonly IUsersRepository _usersRepository;

        public FilesController(string connectionString, IUsersRepository usersRepository)
        {
            ConnectionString = connectionString;
            _usersRepository = usersRepository;
        }

    }
}