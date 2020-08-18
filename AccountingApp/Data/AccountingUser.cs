using Microsoft.AspNetCore.Identity;

namespace AccountingApp.Data
{
    public class AccountingUser : IdentityUser
    {
        public string ExternalDbConnectionString { get; set; }
    }
}
