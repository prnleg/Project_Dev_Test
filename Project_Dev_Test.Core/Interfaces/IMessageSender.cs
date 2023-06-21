namespace Project_Dev_Test.Core.Interfaces
{
    public interface IMessageSender
    {
        void SendGuestbookNotificationEmail(string toAddress, string messageBody);
    }
}
