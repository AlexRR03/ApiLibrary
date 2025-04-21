using ApiLibrary.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NugetLibraryModels;

namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGamesController : ControllerBase
    {
        private RepositoryLibrary repo;
        public VideoGamesController(RepositoryLibrary repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetVideoGames() {
            return await this.repo.GetVideogamesAsync();
            
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGame>> FindVideoGames(int id)
        {
            var result = await this.repo.FindVideoGameAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
