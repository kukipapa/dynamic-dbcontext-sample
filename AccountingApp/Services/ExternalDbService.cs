using AccountingApp.ExternalDb;
using AccountingApp.Interfaces;
using System.Linq;

namespace AccountingApp.Services
{
    public class ExternalDbService : IExternalDbService
    {
        private readonly ExternalDbContext context;

        public ExternalDbService(ExternalDbContext context)
        {
            this.context = context;
        }

        public string GetItem()
        {
            return (from m in context.MyTable
                    select m.Data).FirstOrDefault();
        }
    }
}
