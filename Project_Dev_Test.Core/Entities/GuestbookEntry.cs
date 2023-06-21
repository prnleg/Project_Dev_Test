using Project_Dev_Test.Core.SharedKernel;
using System;

namespace Project_Dev_Test.Core.Entities
{
    public class GuestbookEntry : BaseEntity
    {
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
    }
}
