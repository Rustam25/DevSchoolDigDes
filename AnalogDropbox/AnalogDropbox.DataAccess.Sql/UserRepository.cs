using System;
using System.Data.SqlClient;
using AnalogDropbox.Model;
using AnalogDropbox.DataAccess;
using AnalogDropbox.Log;
using System.Reflection;

namespace AnalogDropbox.DataAccess.Sql
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User Add(string firstName, string secondName, string email)
        {
            using (LogWrapper logger = new LogWrapper())
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("[up_Insert_user]", connection))
                        {
                            var userId = Guid.NewGuid();
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@id", userId);
                            command.Parameters.AddWithValue("@firstName", firstName);
                            command.Parameters.AddWithValue("@secondName", secondName);
                            command.Parameters.AddWithValue("email", email);
                            command.ExecuteNonQuery();
                            return new User
                            {
                                Id = userId,
                                Email = email,
                                FirstName = firstName,
                                SecondName = secondName
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    var method = MethodBase.GetCurrentMethod();
                    logger.Error($"Error in {method.ReflectedType.Name}.{method.Name}(firstName = {firstName}, secondName = {secondName}, email = {email})|Error message: {ex.Message}");
                    return new User();
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("[up_Delete_user]", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User Get(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select Id, FirstName, SecondName, Email from Users where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new User
                            {
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                SecondName = reader.GetString(reader.GetOrdinal("SecondName"))
                            };
                        }
                        throw new ArgumentException("user not found");
                    }
                }
            }
        }

        public User Update(Guid id, string firstName, string secondName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("[up_Update_user]", connection))
                {
                    var userId = Guid.NewGuid();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", userId);
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@secondName", secondName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new User
                            {
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Id = id,
                                FirstName = firstName,
                                SecondName = secondName
                            };
                        }
                        throw new ArgumentException("user not found");
                    }
                }
            }
        }

    }
}
