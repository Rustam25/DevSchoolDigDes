using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalogDropbox.Model
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
        public DateTime PostTime { get; set; }
    }
}
