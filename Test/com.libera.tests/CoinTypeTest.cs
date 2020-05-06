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
    public class CoinTypeTest : BaseTest
    {
        private readonly IConfiguration configuration;
        private readonly IServiceCollection services;
        private readonly IServiceProvider provider;
        private readonly ILogger logger;

        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SQLiteConnection connection;

        public CoinTypeTest()
        {
            services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();

            logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            connection = new SQLiteConnection(configuration.GetConnectionString("defaultConnection"));
            BuildDatabaseAndSeed(connection);

            services.AddSingleton<IDbConnection>(db => connection)
                .AddLogging(l => l.AddSerilog(logger))
                .AddScoped<IRepository, Repository>()
                .AddScoped<ICoinTypeService, CoinTypeService>();

            provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task CheckCoinTypesTest()
        {
            //configure
            var ctServ = provider.GetRequiredService<ICoinTypeService>();

            //act
            var result = await ctServ.GetCoinTypesAsync(1, 1);

            //assert
            result.Response.Count().ShouldBe(4);
        }

        [Theory]
        [InlineData("penny", 0.01)]
        [InlineData("nickel", 0.05)]
        [InlineData("dime", 0.1)]
        [InlineData("quarter", 0.25)]
        public async Task CheckCoinTypeValuesTest(string type, decimal value)
        {
            //configure
            var ctServ = provider.GetRequiredService<ICoinTypeService>();

            //act
            var result = await ctServ.GetCoinTypeAsync(type, 1, 1);

            //assert
            result.Response.Value.ShouldBe(value);
        }
    }
}
