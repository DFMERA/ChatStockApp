using System;
using ChatStockApp.Areas.Identity.Data;
using ChatStockApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ChatStockApp.Areas.Identity.IdentityHostingStartup))]
namespace ChatStockApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IndentityDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("IndentityDbContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<IndentityDbContext>();
            });
        }
    }
}