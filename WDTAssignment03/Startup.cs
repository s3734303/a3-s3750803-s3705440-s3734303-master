using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NWBAWebAPI.Models.DataManager;
using System;
using Admin.Data;


namespace WDTAssignment03
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<NWBAContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("NWBAContext")));

            services.AddTransient<APIAccountManager>();
            services.AddTransient<APIBillPayManager>();
            services.AddTransient<APICustomerManager>();
            services.AddTransient<APILoginManager>();
            services.AddTransient<APIPayeeManager>();
            services.AddTransient<APITransactionManager>();

            //services.AddControllers();


            services.AddDbContext<NWBAContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(NWBAContext)));

                // Enable lazy loading.
                options.UseLazyLoadingProxies();
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Make the session cookie essential.
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
