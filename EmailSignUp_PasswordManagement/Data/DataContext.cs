using Microsoft.EntityFrameworkCore;

namespace EmailSignUp_PasswordManagement.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //Connection string can also be set in the appsetting.json file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.
                UseSqlServer("Server=YUANJIE-WU\\SQLEXPRESS;Database=LocalDb;Trusted_Connection=True;");
        }

        public DbSet<User> Users => Set<User>();
    }
}
