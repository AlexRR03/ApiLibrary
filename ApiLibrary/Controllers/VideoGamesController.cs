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
        [HttpGet("search")]
        public async Task<ActionResult<List<VideoGame>>> Search(
                [FromQuery] string? name = null,
                [FromQuery] string? genre = null,
                [FromQuery] int? year = null,
                [FromQuery] string? developer = null)
        {
            try
            {
                var results = await this.repo.VideoGameSearch(name, genre, year, developer);



                return Ok(results);
            }
            catch (Exception ex)
            {
                // Considera usar un logger aquí
                return StatusCode(500, "Error interno al buscar videojuegos: " + ex.Message);
            }
        }



            [HttpGet("platforms")]
        public async Task<ActionResult<List<string>>> GetPlatforms()
        {
            var result = await this.repo.GetPlatformsAsync();
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("platforms/{name}")]
        public async Task<ActionResult<List<string>>> GetPlatformsVideoGame(string name)
        {
            return await this.repo.GetPlatformsVideoGamesAsync(name);
        }
    }
}
