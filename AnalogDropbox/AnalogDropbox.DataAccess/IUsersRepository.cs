using System;
using AnalogDropbox.Model;

namespace AnalogDropbox.DataAccess
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Добавление нового пользоователя
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="secondName">Фамилия</param>
        /// <param name="email">Адрес электронной почты</param>
        /// <returns><see cref="User"</returns>
        User Add(string firstName, string secondName, string email);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        void Delete(Guid id);

        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns><see cref="User"></returns>
        User Get(Guid id);

        /// <summary>
        /// Изменение личных данных пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="firstName">Имя</param>
        /// <param name="secondName">Фамилия</param>
        /// <returns><see cref="User"></returns>
        User Update(Guid id, string firstName, string secondName);
    }
}
