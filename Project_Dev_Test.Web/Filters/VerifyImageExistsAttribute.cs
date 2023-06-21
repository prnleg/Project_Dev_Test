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
    public class VerifyImageExistsAttribute : TypeFilterAttribute
    {
        public VerifyImageExistsAttribute() : base(typeof(VerifyImageExistsFilter))
        {

        }

        private class VerifyImageExistsFilter : IAsyncActionFilter
        {
            private readonly IRepository _repository;

            public VerifyImageExistsFilter(IRepository repository)
            {
                _repository = repository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                


                await next();
            }
        }
    }
}
