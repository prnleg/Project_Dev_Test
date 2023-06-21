using Ardalis.GuardClauses;
using Project_Dev_Test.Core.Events;
using Project_Dev_Test.Core.Interfaces;

namespace Project_Dev_Test.Core.Services
{
    public class ItemCompletedEmailNotificationHandler : IHandle<ToDoItemCompletedEvent>
    {
        public void Handle(ToDoItemCompletedEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            // Do Nothing
        }
    }
}
