﻿using System;

namespace Project_Dev_Test.Web.ApiModels
{
    public class GuestbookEntryDTO
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
    }
}
