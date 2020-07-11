using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatStockApp.Models;
using Microsoft.AspNetCore.Identity;
using ChatStockApp.Areas.Identity.Data;

namespace ChatStockApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var message = new Message();
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                message.User = currentUser.Email;
            }
            else
            {
                message.User = "Anonymous";
            }
            
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Message message)
        {
            //TO DO:
            //Create the bot hear
            //ChatStockApp.ChatHubs.ChatHub hub = new ChatHubs.ChatHub();
            //await hub.SendMessage("prueba", "prueba");
            return View(message);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
