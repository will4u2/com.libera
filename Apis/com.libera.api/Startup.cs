using System;
using System.Data;
using System.Data.SQLite;
using com.libera.core;
using com.libera.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace com.libera.api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();
            connection = new SQLiteConnection(configuration.GetConnectionString("defaultConnection"));
            BuildDatabaseAndSeed(connection);
        }

        public IConfiguration Configuration { get; }
        private readonly ILogger logger;
        private readonly SQLiteConnection connection;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IDbConnection>(db => connection)
                .AddLogging(l => l.AddSerilog(logger))
                .AddScoped<IRepository, Repository>()
                .AddScoped<ICoinTypeService, CoinTypeService>()
                .AddScoped<ITillService, TillService>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyOrigin();
                                      builder.AllowAnyMethod();
                                  });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Libera Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Libera Api");
            });

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        internal void BuildDatabaseAndSeed(SQLiteConnection connection)
        {
            string sqlString = @"CREATE TABLE CoinTypes (
		                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Type varchar(200) NOT NULL,
                                            Value decimal(18,2) NOT NULL,
	                                        Active bit NOT NULL,
	                                        InUser bigint NOT NULL,
	                                        InDate datetime NOT NULL,
	                                        InApplication bigint NULL,
	                                        ModificationUser bigint NULL,
	                                        ModificationDate datetime NULL,
	                                        ModificationApplication bigint NULL,
	                                        DeleteUser bigint NULL,
	                                        DeleteDate datetime NULL,
	                                        DeleteApplication bigint NULL                                            
                                        );
                                    CREATE TABLE Coins
                                        (
		                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Quantity int NOT NULL,
                                            CoinTypeId int NOT NULL,
	                                        Active bit NOT NULL,
	                                        InUser bigint NOT NULL,
	                                        InDate datetime NOT NULL,
	                                        InApplication bigint NULL,
	                                        ModificationUser bigint NULL,
	                                        ModificationDate datetime NULL,
	                                        ModificationApplication bigint NULL,
	                                        DeleteUser bigint NULL,
	                                        DeleteDate datetime NULL,
	                                        DeleteApplication bigint NULL                                            
                                        );
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('quarter', 0.25, 1, date('now'), 1, 1);
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('dime', 0.1, 1, date('now'), 1, 1);
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('nickel', 0.05, 1, date('now'), 1, 1);
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('penny', 0.01, 1, date('now'), 1, 1);
									INSERT INTO Coins (Quantity, CoinTypeId, Active, InDate, InUser, InApplication) VALUES (40, 1, 1, date('now'), 1, 1);
									INSERT INTO Coins (Quantity, CoinTypeId, Active, InDate, InUser, InApplication) VALUES (50, 2, 1, date('now'), 1, 1);
									INSERT INTO Coins (Quantity, CoinTypeId, Active, InDate, InUser, InApplication) VALUES (40, 3, 1, date('now'), 1, 1);
									INSERT INTO Coins (Quantity, CoinTypeId, Active, InDate, InUser, InApplication) VALUES (50, 4, 1, date('now'), 1, 1);
                                ";
            try
            {
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlString.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("  ", " ");
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.Connection.Open();
                        var results = cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        string message = exc.Message.ToString();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.Message.ToString();
            }
        }
    }
}
