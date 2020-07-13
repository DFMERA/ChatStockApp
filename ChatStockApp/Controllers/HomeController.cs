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
using Microsoft.AspNetCore.SignalR;
using ChatStockApp.ChatHubs;

namespace ChatStockApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _chatHub;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, IHubContext<ChatHub> chatHub)
        {
            _logger = logger;
            _userManager = userManager;
            _chatHub = chatHub;
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

        [HttpGet]
        public async Task<IActionResult> Listen(string user, string messageTxt)
        {
            //DZM: The bot is created here
            string respValue;
            
            respValue = await Services.UtilServices.PostBotStockValue(messageTxt);
            //TODO: Replace the azure function for a MQ
            var message = new Message("Bot", respValue);
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", message);

            return Ok(respValue);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
