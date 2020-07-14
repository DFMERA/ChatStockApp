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
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ChatStockApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, IHubContext<ChatHub> chatHub, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _chatHub = chatHub;
            _configuration = configuration;
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
            string urlString = _configuration.GetSection("AzureFunctionsUrl").Value;
            string respValue = await Services.UtilServices.PostBotStockValue(messageTxt, urlString);
            
            //TODO: Save the history of the messages per user

            return Ok(respValue);
        }

        public async Task<string> ListenTest(string messageTxt, string urlTest)
        {
            //DZM: The bot is created here
            string urlString = urlTest;
            string respValue = await Services.UtilServices.PostBotStockValue(messageTxt, urlString);

            //TODO: Save the history of the messages per user

            return respValue;
        }

        [HttpGet]
        public async Task<IActionResult> ListenBot(string messageTxt)
        {
            var message = new Message("Bot", messageTxt);
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", message);

            return Ok(messageTxt);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
