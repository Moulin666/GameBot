using Microsoft.EntityFrameworkCore;


namespace GameBotProject.Models
{
	public class DataBaseContext : DbContext
	{
		public DataBaseContext(DbContextOptions<DataBaseContext> options) : base (options) { }
    }
}
