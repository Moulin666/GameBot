using GameBotProject.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;


namespace GameBotProject.Models
{
	public class DataBaseContext : DbContext
	{
		public DataBaseContext(DbContextOptions<DataBaseContext> options) : base (options) { }

		public virtual DbSet<DbTest> DbTest { get; set; }

		public virtual DbSet<Account> Accounts { get; set; }
		public virtual DbSet<Character> Characters { get; set; }
    }
}
