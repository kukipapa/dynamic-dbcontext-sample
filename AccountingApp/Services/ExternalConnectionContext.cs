using AccountingApp.Data;
using AccountingApp.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AccountingApp.Services
{
    public class ExternalConnectionContext : IExternalConnectionContext
    {
        private readonly HttpContext httpContext;
        private readonly ApplicationDbContext context;

        public ExternalConnectionContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            httpContext = httpContextAccessor.HttpContext;
            this.context = context;
        }

        public string GetConnectionString()
        {
            var connString = (from u in context.Users
                              where u.UserName == httpContext.User.Identity.Name
                              select u.ExternalDbConnectionString).SingleOrDefault();

            return connString;
        }
    }
}
