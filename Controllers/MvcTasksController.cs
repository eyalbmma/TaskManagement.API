using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    public class MvcTasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly MvcTaskService _taskService;

        public MvcTasksController(AppDbContext context, MvcTaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        [HttpGet]
       
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _context.Users.ToListAsync();
            ViewBag.AllTasks = await _context.Tasks.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string TaskType, int AssignedUserId)
        {
            var success = await _taskService.CreateTaskAsync(TaskType, AssignedUserId);
            if (success)
                return RedirectToAction("Index");

            ViewBag.Error = "שגיאה ביצירת משימה";
            ViewBag.Users = await _context.Users.ToListAsync(); // חשוב גם כאן אם יש שגיאה

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditData(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.ProcurementData)
                .Include(t => t.DevelopmentData)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            ViewBag.Users = await _context.Users.ToListAsync();
            ViewBag.AllTasks = await _context.Tasks.ToListAsync();

            return View(task);
        }

        [HttpPost("MvcTasks/EditData/Procurement/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProcurement(
    int id,
    [FromForm] UpdateProcurementDataDto dto,
    [FromServices] ProcurementDataUpdater updater,
    [FromServices] AppDbContext context)
        {
            var success = await updater.UpdateAsync(id, dto);

            if (success)
            {
                // שולף את המשתמש שהוקצתה לו המשימה
                var task = await context.Tasks.FindAsync(id);
                var userId = task?.AssignedUserId ?? 1; // ברירת מחדל ל־1 אם משום מה לא קיים

                return RedirectToAction("Index", new { userId });
            }

            ViewBag.Error = "שגיאה בשמירת נתוני רכש";
            return RedirectToAction("EditData", new { id });
        }


        [HttpPost("MvcTasks/EditData/Development/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDevelopment(
     int id,
     [FromForm] UpdateDevelopmentDataDto dto,
     [FromServices] DevelopmentDataUpdater updater,
     [FromServices] AppDbContext context)
        {
            var success = await updater.UpdateAsync(id, dto);

            if (success)
            {
                // שולף את המשתמש שהוקצה לו
                var task = await context.Tasks.FindAsync(id);
                var userId = task?.AssignedUserId ?? 1;

                return RedirectToAction("Index", new { userId });
            }

            ViewBag.Error = "שגיאה בשמירת נתוני פיתוח";
            return RedirectToAction("EditData", new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int taskId, int nextStatus, int newUserId)
        {
            var success = await _taskService.ChangeTaskStatusAsync(taskId, nextStatus, newUserId);

            // שליפה של userId כדי שנחזור לאותו מסך משימות
            var task = await _context.Tasks.FindAsync(taskId);
            var userId = task?.AssignedUserId ?? 1;

            if (success)
                return RedirectToAction("Index", new { userId });

            TempData["Error"] = "שגיאה בשינוי סטטוס";
            return RedirectToAction("Index", new { userId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int taskId)
        {
            var success = await _taskService.CloseTaskAsync(taskId);

            var task = await _context.Tasks.FindAsync(taskId);
            var userId = task?.AssignedUserId ?? 1;

            if (success)
                return RedirectToAction("Index", new { userId });

            TempData["Error"] = "שגיאה בסגירת משימה";
            return RedirectToAction("Index", new { userId });
        }


        public async Task<IActionResult> Index(int userId = 1)
        {
            var tasks = await _context.Tasks
                .Include(t => t.ProcurementData)
                .Include(t => t.DevelopmentData)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();

            ViewBag.Users = await _context.Users.ToListAsync();
            ViewBag.AllTasks = await _context.Tasks.ToListAsync();
            ViewBag.UserId = userId;

            return View(tasks);
        }

    }
}
