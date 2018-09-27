using System.IO;
using System.Reflection;
using GameBotProject.Models;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace GameBotProject
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

	    public Startup(IConfiguration configuration)
	    {
		    Configuration = configuration;

		    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
		    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
	    }

		public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<DataBaseContext>(options =>
			{
				options.UseMySQL(Configuration["ServerSettings:ConnectionString"]);
			});

			services.AddMvc();
            services.AddSingleton(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
