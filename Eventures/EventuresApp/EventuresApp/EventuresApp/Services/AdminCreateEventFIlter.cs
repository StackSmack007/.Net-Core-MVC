namespace EventuresApp.Services
{
    using EventuresApp.Areas.Event.Models;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System;
    public class AdminCreateEventFIlter : IActionFilter
    {
        private readonly ILogger<AdminCreateEventFIlter> logger;

        public AdminCreateEventFIlter(ILogger<AdminCreateEventFIlter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            eventCreateDto eventDTO = (eventCreateDto)context.ActionArguments["dto"];
            logger.LogCritical($"{DateTime.UtcNow.ToString("R")} Administrator {context.HttpContext.User.Identity.Name} create event {eventDTO.Name} ({eventDTO.Start.ToString("R")}/{eventDTO.End.ToString("R")})");
        }
    }
}