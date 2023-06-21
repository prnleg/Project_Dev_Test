using System.Collections.Generic;

namespace Project_Dev_Test.Web.ApiModels
{
    public class GuestbookDTO
    {
        public int Id { get; set; }
        public List<GuestbookEntryDTO> Entries { get; set; } = new List<GuestbookEntryDTO>();
    }
}
