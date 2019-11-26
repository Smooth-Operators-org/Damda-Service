using Damda_Service.Data;
using Damda_Service.Services;
using Damda_Service.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;


namespace Damda_Service
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
                    services.AddCors();
                    services.AddDbContext<DataContext>(options => options.UseMySql(Configuration.GetConnectionString("Default")));
                    services.AddDbContext<CouponContext>(options => options.UseMySql(Configuration.GetConnectionString("Coupon")));
                    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                    services.AddHttpClient();
                    services.AddScoped<UserService, UserService>();
                    services.AddScoped<GroupService, GroupService>();
                    services.AddScoped<AuthService, AuthService>();
                    services.AddScoped<CouponService, CouponService>();
                    services.AddScoped<Utilities, Utilities>();
                    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
                }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                app.UseMvc();

        }
    }
    }
