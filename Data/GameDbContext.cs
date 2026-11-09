using GameVault.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Data
{
    public class GameDbContext: DbContext
    {
        public DbSet<Game> Games => Set<Game>();
        public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Title = "The Legend of Zelda: Breath of the Wild",
                    Platform = "Nintendo Switch",
                    Genre = "Action Adventure",
                    ReleaseYear = 2017,
                    Developer = "Nintendo",
                },
                new Game
                {
                    Id = 2,
                    Title = "Red Dead Redemption 2",
                    Platform = "PlayStation 4",
                    Genre = "Action Adventure",
                    ReleaseYear = 2018,
                    Developer = "Rockstar Games",
                },
                new Game
                {
                    Id = 3,
                    Title = "God of War",
                    Platform = "PlayStation 4",
                    Genre = "Action",
                    ReleaseYear = 2018,
                    Developer = "Santa Monica Studio",
                },
                new Game
                {
                    Id = 4,
                    Title = "Halo Infinite",
                    Platform = "Xbox Series X",
                    Genre = "First Person Shooter",
                    ReleaseYear = 2021,
                    Developer = "343 Industries",
                }
            );
        }
    }
}
