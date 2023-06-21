﻿using System;
using System.Threading.Tasks;
using Project_Dev_Test.Core.Entities;
using Project_Dev_Test.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project_Dev_Test.Web.Filters
{
    public class VerifyGuestbookExistsAttribute : TypeFilterAttribute
    {
        public VerifyGuestbookExistsAttribute() : base(typeof(VerifyGuestbookExistsFilter))
        {

        }

        private class VerifyGuestbookExistsFilter : IAsyncActionFilter
        {
            private readonly IRepository _repository;

            public VerifyGuestbookExistsFilter(IRepository repository)
            {
                _repository = repository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    if (context.ActionArguments["id"] is int id)
                    {
                        if (_repository.GetById<Guestbook>(id) is null)
                        {
                            context.Result = new NotFoundObjectResult(id);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
