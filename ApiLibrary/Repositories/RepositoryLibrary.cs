using NugetLibraryModels;
using ApiLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using ApiLibrary.Helpers;
using System.Security.Claims;
using Newtonsoft.Json;

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
        public async Task<int> GetUsuariosMaxIdAsync()
        {
            int maxIdUsuario = await this.context.Users.MaxAsync(u => u.Id);
            return maxIdUsuario + 1;
        }

        public async Task InsertUsuarioAsync(string nombre, string email,string password)
        {
            User user = new User();
            user.Id = await GetUsuariosMaxIdAsync();
            user.Username = nombre;
            user.Email = email;
            user.Password = password;
            user.Salt = HelperCriptography.GenerateSalt();
            user.PasswordHash = HelperCriptography.EncryptPass(password, user.Salt);
            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();

        }
        public async Task<User> LoginUsuariosAsync(string email, string password)
        {
            User user = await this.context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                byte[] passwordHash = HelperCriptography.EncryptPass(password, user.Salt);
                if (HelperCriptography.ComparePass(user.PasswordHash, passwordHash))
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<List<UserVideoGameModel>> GetVideoGamesByUserAsync(int userId)
        {
            var result = await context.Database.SqlQueryRaw<UserVideoGameModel>(
         "SP_GetGamesByUser @UserId",
         new SqlParameter("@UserId", userId)
     ).ToListAsync();

            return result;


        }
        public async Task AddGameToLibraryAsync(int idUser,int idVideoGame, int playtimeHours, string status)
        {
            UserVideoGame userVideoGame = new UserVideoGame();
            userVideoGame.UserId = idUser;
            userVideoGame.VideoGameId = idVideoGame;
            userVideoGame.PlayTimeHours = playtimeHours;
            userVideoGame.Status = status;

            await this.context.UserVideoGames.AddAsync(userVideoGame);
            await this.context.SaveChangesAsync();
        }


    }
}
