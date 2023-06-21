using Project_Dev_Test.Core.Events;
using Project_Dev_Test.Core.SharedKernel;
using System.Collections.ObjectModel;

namespace Project_Dev_Test.Core.Entities
{
    public class Guestbook : BaseEntity
    {
        private readonly List<GuestbookEntry> _entries = new List<GuestbookEntry>();

        public IEnumerable<GuestbookEntry> Entries
        {
            get { return new ReadOnlyCollection<GuestbookEntry>(_entries); }
        }

        public string Name { get; set; }
        
        public void AddEntry(GuestbookEntry entry)
        {
            _entries.Add(entry);
            Events.Add(new EntryAddedEvent(this.Id, entry));
        }
    }
}
