using System.Diagnostics;
using DiaWamba.Data;
using DiaWamba.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiaWamba.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] ContactMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.DateSent = DateTime.Now;

            dbContext.ContactMessages.Add(model);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Message sent successfully!" });
        }

        public IActionResult Messages()
        {
            var messages = dbContext.ContactMessages
                .OrderByDescending(x => x.DateSent)
                .ToList();

            return View(messages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
