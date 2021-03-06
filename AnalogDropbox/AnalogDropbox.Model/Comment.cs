﻿using System;

namespace AnalogDropbox.Model
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid FileId { get; set; }
        public User Author { get; set; }
        public string Text { get; set; }
        public DateTime PostTime { get; set; }
    }
}
