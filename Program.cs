using GameVault.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("GameDbContext") ?? throw new InvalidOperationException("Connection string 'GameDbContext' not found.");

            builder.Services.AddDbContext<GameDbContext>(options => options.UseSqlite("Data Source=games.db"));

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
                db.Database.EnsureCreated();
            }
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
