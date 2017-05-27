using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AnalogDropbox.DataAccess;
using AnalogDropbox.DataAccess.Sql;
using AnalogDropbox.Model;
using System.Threading.Tasks;

namespace AnalogDropbox.WebApi.Controllers
{
    public class FilesController : ApiController
    {
        private const string ConnectionString = "Data Source=SQLEXPRESS2016;Initial Catalog=DropboxAnalog;Integrated Security=False;User ID=dropboxuser; Password=dropboxuser";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;

        public FilesController()
        {
            _filesRepository = new FilesRepository(ConnectionString, _usersRepository);
        }

        [HttpPost]
        public File CreateFile(File file)
        {
            return _filesRepository.Add(file);
        }

        /// <summary>
        /// Спросить почему асинхронный
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/files/{id}/content")]
        public async Task UpdateFileContent(Guid id)
        {
            var bytes = await Request.Content.ReadAsByteArrayAsync();
            _filesRepository.UpdateContent(id, bytes);
        }

        /// <summary>
        /// Уточнить. Норм?
        /// </summary>
        [HttpGet]
        [Route("api/files/{id}/content")]
        public byte[] GetFileContetnt(Guid id)
        {
            return _filesRepository.GetContent(id);
        }

        [HttpGet]
        [Route("api/files/{id}")]
        public File GetFileInfo(Guid id)
        {
            return _filesRepository.GetInfo(id);
        }

        [HttpDelete]
        [Route("api/files/{id}")]
        public void DeleteFile(Guid id)
        {
            _filesRepository.Delete(id);
        }

        [HttpGet]
        [Route("api/files/{id}/comments")]
        public IEnumerable<Comment> GetFileComments(Guid id)
        {
            return _filesRepository.GetFileComments(id);
        }

        /// <summary>
        /// Норм?
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/files/comments")]
        public Comment AddCommentToFile([FromBody] Comment comment)
        {
            return _filesRepository.AddCommentToFile(comment);
        }
    }
}