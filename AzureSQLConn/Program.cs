using AzureSQLConn.Database;
using AzureSQLConn.Repositories;
using MessageBus;
using Microsoft.EntityFrameworkCore;

namespace AzureSQLConn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("cors", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.Audience = "api://b5652012-1efb-4c6f-8cef-4e35d6ca33c6";
                options.Authority = "https://login.microsoftonline.com/e504ade5-ffda-4c8d-ae63-7fdd2d273a8c";
            });
            builder.Services.AddScoped<IMessageBus, MessageBus.MessageBus>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseCors("cors");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}