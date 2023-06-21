using Project_Dev_Test.Core.Entities;
using Project_Dev_Test.Core.Events;
using Project_Dev_Test.Core.Interfaces;
using Project_Dev_Test.Core.Specifications;
using System.Linq;

namespace Project_Dev_Test.Core.Handlers
{
    public class GuestbookNotificationHandler : IHandle<EntryAddedEvent>
    {
        private IRepository _repository;
        private IMessageSender _messageSender;

        public GuestbookNotificationHandler(IRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
            _messageSender = messageSender;
        }

        public void Handle(EntryAddedEvent entryAddedEvent)
        {
            var notificationPolicy = new GuestbookNotificationPolicy(entryAddedEvent.Entry.Id);

            //Send updates to previous entries made in the last day
            var emailsToNotify = _repository.List(notificationPolicy).Select(e => e.EmailAddress);

            foreach (var emailAddress in emailsToNotify)
            {
                string messageBody = $"{entryAddedEvent.Entry.EmailAddress} left a message {entryAddedEvent.Entry.Message}";
                _messageSender.SendGuestbookNotificationEmail(emailAddress, messageBody);
            }
        }
    }
}
