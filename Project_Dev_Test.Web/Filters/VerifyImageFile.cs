using System;
using System.Threading.Tasks;
using Project_Dev_Test.Core.Entities;
using Project_Dev_Test.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Security.Cryptography;

namespace Project_Dev_Test.Web.Filters
{
    public class VerifyImageFile : TypeFilterAttribute
    {
        public VerifyImageFile() : base(typeof(VerifyImageFileFilter))
        {

        }

        private class VerifyImageFileFilter : IAsyncActionFilter
        {
            private readonly IRepository _repository;

            public VerifyImageFileFilter(IRepository repository)
            {
                _repository = repository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                //if (context.ActionArguments.FirstOrDefault(x => x.Value)) 
                //{
                //    return;
                //}
            }
        }
    }
}
