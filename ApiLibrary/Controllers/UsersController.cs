using ApiLibrary.Helpers;
using ApiLibrary.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private RepositoryLibrary repo;
        private HelperActionServiceOAuth helper;
        public UsersController(RepositoryLibrary repo, HelperActionServiceOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        public async Task<ActionResult> InsertUsuario(string nombre, string email, string pass)
        {
            try
            {
                await this.repo.InsertUsuarioAsync(nombre, email, pass);
                return Ok("Usuario insertado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al insertar el usuario: " + ex.Message);
            }
        }
    }
}
