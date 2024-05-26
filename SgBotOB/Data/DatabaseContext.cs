using Microsoft.EntityFrameworkCore;
using SgBotOB.Model;
using SgBotOB.Model.SgGame;
namespace SgBotOB.Data
{
    internal class DataBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<CollectedData> CollectedDatas { get; set; }
        public DbSet<Player> Players { get; set; }

        public string DbPath { get; }

        public DataBaseContext()
        {
            DbPath = Path.Combine(StaticData.ExePath!, "Data/data.db");
            // DbPath = "D:\\vspjt\\SgBot.Open\\SgBot.Open\\bin\\Debug\\net6.0\\Data\\data.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
