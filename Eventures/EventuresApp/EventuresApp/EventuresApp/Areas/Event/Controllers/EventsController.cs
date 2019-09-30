namespace EventuresApp.Areas.Event.Controllers
{
    using EventuresApp.Areas.Event.Models;
    using EventuresApp.Models;
    using EventuresApp.Services;
    using EventuresApp.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Area("Event")]
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;
        private readonly UserManager<AppUser> userManager;

        public EventsController(IEventsService eventsService, UserManager<AppUser> userManager)
        {
            this.eventsService = eventsService;
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
        public async Task<IActionResult> Create(eventCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await eventsService.AddNew(dto, User.Identity.Name);
                return RedirectToAction(nameof(All));
            }
            return View(dto);
        }

        [Authorize]
        public IActionResult All(int? pageNumber)
        {
            int eventsPerPage = 3;
            var portionOfEvents = eventsService.GetPortionOfEvents(pageNumber, eventsPerPage);
            return View(portionOfEvents);
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            string currentUserId = userManager.GetUserId(this.User);
            ICollection<myEventDto> myEvents = eventsService.GetMyEvents(currentUserId);
            return View(myEvents);
        }
    }
}