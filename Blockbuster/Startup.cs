using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Blockbuster.Models;
using Microsoft.AspNetCore.Identity;

namespace Blockbuster
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json"); //this line replaces .AddEnvironmentVariables();
    Configuration = builder.Build();
}

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
    services.AddMvc();

    services.AddEntityFrameworkMySql()
        .AddDbContext<BlockbusterContext>(options => options
        .UseMySql(Configuration["ConnectionStrings:DefaultConnection"]));
    services.AddIdentity<ApplicationUser, IdentityRole> ()
        .AddEntityFrameworkStores<BlockbusterContext>()
        .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
    });

          
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseStaticFiles();

      app.UseDeveloperExceptionPage();

      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("Something went wrong!");
      });
      
    }
  }
}