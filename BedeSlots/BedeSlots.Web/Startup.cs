using BedeSlots.Common.Providers;
using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Games;
using BedeSlots.Games.Contracts;
using BedeSlots.Services;
using BedeSlots.Services.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.External;
using BedeSlots.Services.External.Contracts;
using BedeSlots.Web.Infrastructure.Extensions;
using BedeSlots.Web.Infrastructure.Providers;
using BedeSlots.Web.Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BedeSlots
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.RegisterData(services);
            this.RegisterAuthentication(services);
            this.RegisterServices(services);
            this.RegisterInfrastructure(services);

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                     new CacheProfile()
                     {
                         Location = ResponseCacheLocation.Any,
                         Duration = 30 * 60
                     });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseDatabaseMigration();

            if (this.Environment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterData(IServiceCollection services)
        {
            services.AddDbContext<BedeSlotsDbContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IUserBalanceService, UserBalanceService>();
            services.AddTransient<ICurrencyConverterService, CurrencyConverterService>();
            services.AddTransient<IExchangeRateApiService, ExchangeRateApiService>();
            services.AddTransient<IExchangeRatesApiCaller, ExchangeRatesApiCaller>();
            services.AddSingleton<ISlotMachine, SlotMachine>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient(typeof(IPaginationProvider<>), typeof(PaginationProvider<>));
            services.AddTransient<IRandomProvider, RandomProvider>();
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BedeSlotsDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            });

            if (this.Environment.IsDevelopment())
            {
                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequiredLength = 3;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
                    options.Lockout.MaxFailedAccessAttempts = 999;
                });
            }
        }

        private void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMemoryCache();

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
    }
}
