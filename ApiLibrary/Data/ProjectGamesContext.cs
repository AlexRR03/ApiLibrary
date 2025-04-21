using Microsoft.EntityFrameworkCore;
using NugetLibraryModels;

namespace ApiLibrary.Data
{
    public class ProjectGamesContext: DbContext
    {
        public ProjectGamesContext(DbContextOptions<ProjectGamesContext> options) : base(options) { }

        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserVideoGame> UserVideoGames { get; set; }
        public DbSet<UserList> UserList { get; set; }
        public DbSet<UserListVideoGame> UserListVideoGame { get; set; }
    }
}

