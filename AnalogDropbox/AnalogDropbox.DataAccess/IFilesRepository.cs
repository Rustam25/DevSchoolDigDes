using System;
using System.Collections.Generic;
using AnalogDropbox.Model;

namespace AnalogDropbox.DataAccess
{
    public interface IFilesRepository
    {
        /// <summary>
        /// Добавление файла
        /// </summary>
        /// <param name="file"><see cref="File"></param>
        /// <returns><see cref="File"></returns>
        File Add(File file);

        /// <summary>
        /// Получение содержимого файла
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <returns>Byte array</returns>
        byte[] GetContent(Guid id);

        /// <summary>
        /// Получение информации о файле
        /// </summary>
        /// <param name="Id">Идентификатор файла</param>
        /// <returns><see cref="File"></returns>
        File GetInfo(Guid Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId">Идентификатор файла</param>
        /// <param name="content">Обновление содержимого файла</param>
        void UpdateContent(Guid fileId, byte[] content);

        /// <summary>
        /// Получение коллекции файлов пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>IEnumerable<<see cref="File">></returns>
        IEnumerable<File> GetUserFiles(Guid userId);

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        void Delete(Guid id);
    }
}
