using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UdemyNLayerProject.API.Extensions;
using UdemyNLayerProject.API.Filters;
using UdemyNLayerProject.Core.Repositories;
using UdemyNLayerProject.Core.Services;
using UdemyNLayerProject.Core.UnitOfWorks;
using UdemyNLayerProject.Data;
using UdemyNLayerProject.Data.Repositories;
using UdemyNLayerProject.Data.UnitOfWorks;
using UdemyNLayerProject.Service.Services;

namespace UdemyNLayerProject.API
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
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<NotFoundFilter>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"].ToString(), o => o.MigrationsAssembly("UdemyNLayerProject.Data"));
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new ValidationFilter());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UdemyNLayerProject.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UdemyNLayerProject.API v1"));
            }
            app.UseCustomException();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
