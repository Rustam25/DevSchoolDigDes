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
        private readonly IUsersRepository _usersRepository;

        public FilesRepository(string connectionString, IUsersRepository usersRepository)
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

        public void Shared(Guid fileId, Guid userId, bool readOnlyAccess)
        {
            using (SqlConnection connect = new SqlConnection(_connectionString))
            {
                connect.Open();
                using (SqlCommand command = new SqlCommand("up_Shared_file", connect))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@fileId", fileId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@readOnlyAccess", readOnlyAccess);
                    command.ExecuteNonQuery();
                }
            }
        }


        public Comment GetComment(Guid commentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select CommentId, FileId, Author, Text, PostTime from uf_Select_comment (@id)";
                    command.Parameters.AddWithValue("@id", commentId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new Comment
                            {
                                CommentId = reader.GetGuid(reader.GetOrdinal("CommentId")),
                                FileId = reader.GetGuid(reader.GetOrdinal("FileId")),
                                Author = _usersRepository.Get(reader.GetGuid(reader.GetOrdinal("Author"))),
                                Text = reader.GetString(reader.GetOrdinal("Text")),
                                PostTime = reader.GetDateTime(reader.GetOrdinal("PostTime"))
                            };
                        }
                        throw new ArgumentException("comment not found");
                    }
                }
            }
        }

        public IEnumerable<Comment> GetFileComments(Guid fileId)
        {
            var result = new List<Comment>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select Comments.Id as Id from Shared join Comments on Shared.Id = Comments.SharedId where Shared.FileId = @fileId";
                    command.Parameters.AddWithValue("@fileId", fileId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(GetComment(reader.GetGuid(reader.GetOrdinal("Id"))));
                        }
                        return result;
                    }
                }
            }
        }

        public Comment AddCommentToFile(Comment comment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("up_Insert_comment", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var commentId = Guid.NewGuid();
                    command.Parameters.AddWithValue("@id", commentId);
                    command.Parameters.AddWithValue("@fileId", comment.FileId);
                    command.Parameters.AddWithValue("@userId", comment.Author.Id);
                    command.Parameters.AddWithValue("@text", comment.Text);
                    command.Parameters.AddWithValue("@postTime", comment.PostTime);
                    command.ExecuteNonQuery();
                    comment.CommentId = commentId;
                    return comment;
                }
            }
        }

    }
}
