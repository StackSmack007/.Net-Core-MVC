using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventuresApp.Areas.Event.Models;
using X.PagedList;

namespace EventuresApp.Services.Contracts
{
    public interface IEventsService
    {
        Task AddNew(eventCreateDto dto, string userName);
        IPagedList<eventInfoDtoOutput> GetPortionOfEvents(int? pageNumber, int eventsPerPage);
        ICollection<myEventDto> GetMyEvents(string currentUserId);
    }
}
