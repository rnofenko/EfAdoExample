using System.Data.Entity;
using AdoExample.Models;

namespace AdoExample.DataAccess
{
    public class EfContext : DbContext
    {
        public IDbSet<Client> Clients { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<RiskGroup> RiskGroups { get; set; }

        public EfContext()
            : base(getConnectionName())
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected static string getConnectionName()
        {
            return "Server=roman;Database=Ef1;Trusted_Connection=True;";
            //return "name=" + ConfigurationManager.AppSettings["ConnectionName"];
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
