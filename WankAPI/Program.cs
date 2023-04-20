using H1_ERP.DomainModel;
using H1_ERP.Interfaces_s;
using W.A.N.K_API.Repostoriy;
using WankAPI.Controllers;

namespace WankAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped <IRepos<Customer>, CustomerRepos>();
            builder.Services.AddScoped<IRepos<Product>, ProductrRpository>(); 
            builder.Services.AddScoped<IRepos<SalesOrderHeader>, SalesRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}