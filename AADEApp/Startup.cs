
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aade.Integrations;
using Aade.Models.Aade;
using Aade.Models.Messages;
using Aade.Models.User;
using Aade.Services;

namespace Aade
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("user");
            services.AddDbContext<DeliDocContext>(c => c.UseSqlServer(connectionString));

            string aadeConnectionString = Configuration.GetConnectionString("aade");

            string messagesConnectionString = Configuration.GetConnectionString("messages");

            services.AddDbContext<DeliDocContext>(c => c.UseSqlServer(connectionString));
            services.AddDbContext<AadeContext>(c => c.UseSqlServer(aadeConnectionString));
            services.AddDbContext<MessagesContext>(c => c.UseSqlServer(messagesConnectionString));

            // https://www.freecodespot.com/blog/asp-net-core-identity/
            services.AddIdentity<Models.Aade.AspNetUsers, IdentityRole>().AddEntityFrameworkStores<AadeContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                
            });

            services.AddScoped<IUserDbService, UserDbService<DeliDocContext>>();
            services.AddScoped<IUserDbIntegration, UserDbIntegration>();
            services.AddScoped<IAadeDbService, AadeDbService<AadeContext>>();
            services.AddScoped<IAadeDbIntegration, AadeDbIntegration>();
            services.AddScoped<IMessagesDbService, MessagesDbService<MessagesContext>>();
            services.AddScoped<IMessageDbIntegration, MessageDbIntegration>();


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
