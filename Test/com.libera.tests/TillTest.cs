using com.libera.core;
using com.libera.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shouldly;
using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace com.libera.tests
{
    public class TillTest : BaseTest
    {
        private readonly IConfiguration configuration;
        private readonly IServiceCollection services;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SQLiteConnection connection;

        public TillTest()
        {
            services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();

            logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            connection = new SQLiteConnection(configuration.GetConnectionString("defaultConnection"));

            services.AddSingleton<IDbConnection>(db => connection)
                .AddLogging(l => l.AddSerilog(logger))
                .AddScoped<IRepository, Repository>()
                .AddScoped<ICoinTypeService, CoinTypeService>()
                .AddScoped<ITillService, TillService>();

            services.AddMvcCore().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            BuildDatabaseAndSeed(connection);

            provider = services.BuildServiceProvider();
        }

        [Theory]
        [InlineData(0, 10, 0, 34, .99, 0, 9, 0, 9)]
        [InlineData(0, 10, 3, 9, .99, 0, 9, 1, 4)]
        [InlineData(0, 10, 0, 0, .99, 0, 0, 0, 0)]
        public async Task MakeChangeTest(int quarters, int dimes, int nickels, int pennies, decimal amountToChange, int resultquarters, int resultdimes, int resultnickels, int resultpennies)
        {
            //configure
            var tServ = provider.GetRequiredService<ITillService>();
            await tServ.ClearTillAsync(1, 1);
            await tServ.AddCoinToTillAsync("quarter", quarters, 1, 1);
            await tServ.AddCoinToTillAsync("dime", dimes, 1, 1);
            await tServ.AddCoinToTillAsync("nickel", nickels, 1, 1);
            await tServ.AddCoinToTillAsync("penny", pennies, 1, 1);

            //act
            var result = await tServ.GetCorrectChangeAsync(amountToChange, 1, 1);

            //assert
            if (result.Success)
            {
                result.Response.FirstOrDefault(c => c.Type.Type.ToLower().Equals("quater"))?.Quantity.ShouldBe(resultquarters);
                result.Response.FirstOrDefault(c => c.Type.Type.ToLower().Equals("dime"))?.Quantity.ShouldBe(resultdimes);
                result.Response.FirstOrDefault(c => c.Type.Type.ToLower().Equals("nickel"))?.Quantity.ShouldBe(resultnickels);
                result.Response.FirstOrDefault(c => c.Type.Type.ToLower().Equals("penny"))?.Quantity.ShouldBe(resultpennies);
            }
            else
            {
                result.Messages.FirstOrDefault().ShouldBe("Not able to make change.");
            }
        }
    }
}
