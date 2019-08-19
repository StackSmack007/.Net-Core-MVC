namespace Panda_Exam.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Panda_Exam.Data;
    using Panda_Exam.DTOS.Receipts;
    using Panda_Exam.Models;
    using System.Linq;

    public class ReceiptsController : Controller
    {
        private UserManager<User> userManager;
        private PandaDbContext DB;
        private IMapper mapper;
        public ReceiptsController(PandaDbContext db, UserManager<User> um, IMapper mapper)
        {
            DB = db;
            userManager = um;
            this.mapper = mapper;
        }

        public static void Create(Package package, PandaDbContext DB, IMapper mapper)
        {
            var receipt = mapper.Map<Receipt>(package);
            DB.Receipts.Add(receipt);
            DB.SaveChanges();
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentUserId = userManager.GetUserAsync(HttpContext.User).Result.Id;

            var receipts = DB.Receipts.Where(x => x.RecipientId == currentUserId)
                .ProjectTo<outputReceiptIndexDto>(mapper.ConfigurationProvider)
                .ToArray();
            return View(receipts);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var currentUserId = userManager.GetUserAsync(HttpContext.User).Result.Id;
            var receipt = mapper.Map<outputReceiptDetailsDto>(
                                                             DB.Receipts.Where(x => x.RecipientId == currentUserId && x.Id == id)
                                                             .Include(x=>x.Package).FirstOrDefault());

            if (receipt is null)
            {
                return View("Error", new ErrorViewModel() { RequestId = $"Receipt with Id {id} not found!" });
            }
            return View(receipt);
        }
    }
}