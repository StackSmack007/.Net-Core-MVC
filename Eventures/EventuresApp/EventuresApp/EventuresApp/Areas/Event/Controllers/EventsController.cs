namespace EventuresApp.Areas.Event.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using EventuresApp.Areas.Event.Models;
    using EventuresApp.Data;
    using EventuresApp.Models;
    using EventuresApp.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using X.PagedList;

    [Area("Event")]
    public class EventsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;
        private readonly ILogger<EventsController> logger;
        private readonly UserManager<AppUser> userManager;

        public EventsController(IMapper mapper, ApplicationDbContext db, ILogger<EventsController> logger, UserManager<AppUser> userManager)
        {
            this.mapper = mapper;
            this.db = db;
            this.logger = logger;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
            return View(dto);
        }

        [Authorize]
        public IActionResult All( int? pageNumber, string foo)
        {
            var eventDtos = db.Events.Where(x => x.TotalTickets > 0)
                .OrderByDescending(x => x.TotalTickets).ProjectTo<eventInfoDtoOutput>(mapper.ConfigurationProvider);
      
            var portionOfEvents = eventDtos.ToPagedList(pageNumber ?? 1, 1);
            return View(portionOfEvents);
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            string currentUserId = userManager.GetUserId(this.User);

            myEventDto[] myEvents = db.Events.Where(x => x.Orders.Any(o => o.AppUserId == currentUserId))
               .Select(x => new myEventDto
               {
                   Name = x.Name,
                   Start = x.Start,
                   End = x.End,
                   TotalTicketCount = x.Orders.Where(o => o.AppUserId == currentUserId).Sum(o => o.TicketCount)
               }).OrderByDescending(x=>x.TotalTicketCount).ToArray();
            return View(myEvents);
        }
    }
}