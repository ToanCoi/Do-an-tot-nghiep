using Microsoft.AspNetCore.Builder;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using PG.Core.Service;
using PG.Infrastructure.Repository;
using PG.API.Middlewares;
using Microsoft.OpenApi.Models;
using PG.Library.Service;
using PG.Storage;

namespace PG.API
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCROSPolicy",
                    builder =>
                    {
                        builder.WithOrigins("*")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            //Config DI

            //1.Base
            services.AddScoped<ITypeService, TypeService>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            //Storage
            services.AddSingleton<IFileStorageService, FileStorageService>();

            //Employee
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            //Table
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<ITableService, TableService>();

            //Dish
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IDishService, DishService>();

            //Order
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            //OrderDish
            services.AddScoped<IOrderDishRepository, OrderDishRepository>();

            //Department
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            //Tax
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<ITaxService, TaxService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PG.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PG.API v1"));
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
