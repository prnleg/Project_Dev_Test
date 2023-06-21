using Project_Dev_Test.Web.ApiModels;
using System.Collections.Generic;

namespace Project_Dev_Test.Web.ViewModels
{
    public class HomePageViewModel
    {
        public string GuestbookName { get; set; }
        public List<GuestbookEntryDTO> PreviousEntries { get; } = new List<GuestbookEntryDTO>();
        public GuestbookEntryDTO NewEntry { get; set; }
    }
}
