using Database.Common;
using Database.Repositories;
using FintachartRestApi.Common;
using FintachartRestApi.ThirdPartyHelpers.Finchart;
using FintachartRestApi.Services;

namespace FintachartRestApi
{
	public class Program
	{
		public static void Main (string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Host.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddConsole();
			});


			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//Configuration
			builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
			builder.Services.Configure<FinchartApiSettings>(builder.Configuration.GetSection("ThirdPartyApis").GetSection("FinchartApi"));

			//Database
			builder.Services.AddSingleton<AssetService>();
			builder.Services.AddSingleton<MongoRepository>();

			//Third party DI
			builder.Services.AddTransient<FinchartApiService>();
			builder.Services.AddTransient<FinchartAuthService>();
			builder.Services.AddTransient<FinchartWebsocketService>();

			var app = builder.Build();

			Task.WhenAll(app.Services.SeedMongoDatabase());

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