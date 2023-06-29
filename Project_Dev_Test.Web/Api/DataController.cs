using Microsoft.AspNetCore.Mvc;
using Project_Dev_Test.Web.Repository;

namespace Project_Dev_Test.Web.Api
{
    public class DataController : Controller
    {
        private readonly DataRepository repository;

        public DataController(DataRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{userId}/get-data")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> GetData([FromRoute] int userId)
        {
            var data = repository.GetAllResultsFromUser(userId);

            var result = new
            {
                Data = data,
                Length = data.Count
            };

            return Ok(result);
        }
    }
}