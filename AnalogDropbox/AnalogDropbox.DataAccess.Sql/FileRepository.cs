using System;
using System.Data.SqlClient;
using AnalogDropbox.Model;
using AnalogDropbox.DataAccess;
using System.Collections.Generic;

namespace AnalogDropbox.DataAccess.Sql
{
    public class FilesRepository : IFilesRepository
    {
        private readonly string _connectionString;
        private readonly IUserRepository _usersRepository;

        public FilesRepository(string connectionString, IUserRepository usersRepository)
        {
            _connectionString = connectionString;
            _usersRepository = usersRepository;
        }

        public File Add(File file)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("up_Insert_file", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var fileId = Guid.NewGuid();
                    command.Parameters.AddWithValue("@id", fileId);
                    command.Parameters.AddWithValue("@name", file.Name);
                    command.Parameters.AddWithValue("@type", GetFileType(file.Name));
                    command.Parameters.AddWithValue("@owner", file.Owner.Id);
                    command.Parameters.AddWithValue("@creationTime", file.CreationTime);
                    command.Parameters.AddWithValue("@lastWriteTime", file.LastWriteTime);
                    command.ExecuteNonQuery();
                    file.Id = fileId;
                    return file;
                }
            }
        }

        private string GetFileType(string name)
        {
            if (name.Contains("."))
            {
                int dotIndex = name.LastIndexOf(".");
                return name.Substring(dotIndex + 1, name.Length - dotIndex - 1);
            }
            else
                return "";
        }

        public byte[] GetContent(Guid fileId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select Data from Files where Id = @id";
                    command.Parameters.AddWithValue("@id", fileId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            return reader.GetSqlBinary(reader.GetOrdinal("Data")).Value;
                        throw new ArgumentException($"file {fileId} not found");
                    }
                }
            }
        }

        public File GetInfo(Guid fileId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select Id, Name, Size, CreationTime, LastWriteTime, Owner from uf_Select_file_info (@id)";
                    command.Parameters.AddWithValue("@id", fileId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new File
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Owner = _usersRepository.Get(reader.GetGuid(reader.GetOrdinal("Owner"))),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Size = reader.GetInt64(reader.GetOrdinal("Size")),
                                CreationTime = reader.GetDateTime(reader.GetOrdinal("CreationTime")),
                                LastWriteTime = reader.GetDateTime(reader.GetOrdinal("LastWriteTime"))
                            };
                        }
                        throw new ArgumentException("file not found");
                    }
                }
            }
        }

        public void UpdateContent(Guid fileId, byte[] content)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update Files set Data = @content where Id = @id";
                    command.Parameters.AddWithValue("@content", content);
                    command.Parameters.AddWithValue("@id", fileId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<File> GetUserFiles(Guid userId)
        {
            var result = new List<File>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select FileId from Shared where UserId = @userid";
                    command.Parameters.AddWithValue("@userid", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(GetInfo(reader.GetGuid(reader.GetOrdinal("id"))));
                        }
                        return result;
                    }
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Files where Id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Comment> GetFileComments(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Comment AddCommentToFile(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void Shared(Guid fileId, Guid userId, bool readOnlyAccess)
        {
            throw new NotImplementedException();
        }

    }
}
