using AccountingApp.Data;
using AccountingApp.ExternalDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingApp.Migrations
{
    public class MigrationService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public MigrationService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    await context.Database.EnsureCreatedAsync();
                    await context.Database.MigrateAsync();

                    if (!context.Users.Any())
                    {
                        using (var userManager = serviceScope.ServiceProvider.GetService<UserManager<AccountingUser>>())
                        {
                            await CreateUserAsync(userManager, "AlphaDb");
                            await CreateUserAsync(userManager, "BetaDb");
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task CreateUserAsync(UserManager<AccountingUser> userManager, string connectionStringName)
        {
            var id = $"{connectionStringName.ToLower()}@mail.com";
            var connString = configuration.GetConnectionString(connectionStringName);

            var user = new AccountingUser
            {
                UserName = id,
                Email = id,
                ExternalDbConnectionString = connString
            };

            await userManager.CreateAsync(user, "P@ssw0rd");
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, token);

            CreateExternalSampleDatabase(connectionStringName, connString);
        }

        private void CreateExternalSampleDatabase(string connectionStringName, string connString)
        {
            using (var context = new ExternalDbContext(connString))
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();

                if (context.MyTable.Count() == 0)
                {
                    var myRecord = new MyTable
                    {
                        Data = connectionStringName
                    };

                    context.MyTable.Add(myRecord);

                    context.SaveChanges();
                }
            }
        }
    }
}
