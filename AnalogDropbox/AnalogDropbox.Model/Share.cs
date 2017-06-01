using System;


namespace AnalogDropbox.Model
{
    public class Share
    {
        public Guid FileId { get; set; } 
        public Guid OwnerId { get; set; }
        public Guid PartOwnerId { get; set; }
        public bool ReadOnlyAccess { get; set; }
    }
}
