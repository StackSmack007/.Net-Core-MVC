namespace EventuresApp.Areas.Event.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using EventuresApp.Areas.Event.Models;
    using EventuresApp.Data;
    using EventuresApp.Models;
    using EventuresApp.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    [Area("Event")]
    public class EventsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;
        private readonly ILogger<EventsController> logger;

        public EventsController(IMapper mapper, ApplicationDbContext db, ILogger<EventsController> logger)
        {
            this.mapper = mapper;
            this.db = db;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
       // [TypeFilter(typeof(ThrottleFilter), Arguments = new object[] { 10 })]
        [TypeFilter(typeof(AdminCreateEventFIlter))]
        public IActionResult Create(eventCreateDto dto)
        {
            logger.LogCritical($"{User.Identity.Name} is creating an event with name {dto.Name}");
            if (ModelState.IsValid)
            {
                Event newEvent = mapper.Map<Event>(dto);
                db.Events.Add(newEvent);
                db.SaveChanges();
                return RedirectToAction(nameof(All));
            }
            string[] errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return View();
        }

        [Authorize]
        public IActionResult All()
        {
            eventInfoDtoOutput[] eventDtos = db.Events.ProjectTo<eventInfoDtoOutput>(mapper.ConfigurationProvider).ToArray();
            return View(eventDtos);
        }
    }
}