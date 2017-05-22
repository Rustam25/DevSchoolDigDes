using AnalogDropbox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalogDropbox.DataAccess
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Добавление коментария
        /// </summary>
        /// <param name="Text">Текст коментария</param>
        /// <returns><see cref="Comment"></returns>
        Comment Add(string Text);

        /// <summary>
        /// Удаление коментария
        /// </summary>
        /// <param name="commentId">Идентификатор коментария</param>
        void Delete(Guid commentId);
    }
}
