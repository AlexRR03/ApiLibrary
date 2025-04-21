using NugetLibraryModels;
using ApiLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.Data.SqlClient;

namespace ApiLibrary.Repositories
{

    public class RepositoryLibrary
    {
        private ProjectGamesContext context;
        public RepositoryLibrary(ProjectGamesContext context)
        {
            this.context = context;
        }

        public Task<List<VideoGame>> GetVideogamesAsync()
        {
            return this.context.VideoGames.ToListAsync();
        }
        public async Task<VideoGame> FindVideoGameAsync(int id)
        {
            return await this.context.VideoGames.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<string>> GetPlatformsAsync() {
            return await this.context.Database
        .SqlQueryRaw<string>($"SELECT Name FROM Platform")
        .ToListAsync();
        }
        public async Task<List<String>> GetPlatformsVideoGamesAsync(string name)        
        {
            string sql = "EXEC SP_GetPlatformsByGame @GameName";

        var platforms = await this.context.Database
            .SqlQueryRaw<string>(sql, new SqlParameter("@GameName", name))
            .ToListAsync();

            return platforms;
        }
    
}
}
