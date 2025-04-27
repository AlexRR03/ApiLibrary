using ApiLibrary.Helpers;
using ApiLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using NugetLibraryModels;
using System.Security.Claims;

namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVideoGamesController : ControllerBase
    {
        private RepositoryLibrary repo;
        private HelperActionServiceOAuth helper;
        public UserVideoGamesController(RepositoryLibrary repo, HelperActionServiceOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<UserVideoGameModel>> VideoGamesUser()
        {
           Claim claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "datosUsuario");
            string jsonUsuario = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonUsuario);
            if (user == null)
            {
                return null;
            }
            else
            {
                return await this.repo.GetVideoGamesByUserAsync(user.Id);
            }
           


        }
        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task AddVideoGameUserLibrary(int idVideoGame, int playtimeHours, string status)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "datosUsuario");
            string jsonUsuario = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonUsuario);

            await this.repo.AddGameToLibraryAsync(user.Id, idVideoGame, playtimeHours, status);
        }
    }
}
