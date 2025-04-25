using NugetLibraryModels;
using ApiLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<VideoGame>> VideoGameSearch(string? name, string? genre, int? year, string? developer)
        {
            IQueryable<VideoGame> query = this.context.VideoGames;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(v => EF.Functions.Like(v.Name, $"%{name}%"));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(v => EF.Functions.Like(v.Genre, $"%{genre}%"));
            }

            if (year.HasValue)
            {
                query = query.Where(v => v.ReleaseYear == year);
            }

            if (!string.IsNullOrEmpty(developer))
            {
                query = query.Where(v => EF.Functions.Like(v.Developer, $"%{developer}%"));
            }
            return await query.ToListAsync();
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
