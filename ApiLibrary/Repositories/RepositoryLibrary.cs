using NugetLibraryModels;
using ApiLibrary.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
