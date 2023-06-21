using Project_Dev_Test.Core.Entities;
using Project_Dev_Test.Core.SharedKernel;

namespace Project_Dev_Test.Core.Events
{
    public class EntryAddedEvent : BaseDomainEvent
    {
        public int GuestbookId { get; }
        public GuestbookEntry Entry { get; }

        public EntryAddedEvent(int guestbookId, GuestbookEntry entry)
        {
            GuestbookId = guestbookId;
            Entry = entry;
        }
    }
}
