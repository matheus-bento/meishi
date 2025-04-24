using Meishi.Server.Models.Response;
using Meishi.Server.Services.GithubApi;
using Meishi.Server.Services.GithubApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Meishi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IGithubApiService _githubApiService;

        public UserController(
            ILogger<UserController> logger,
            IGithubApiService githubApiService)
        {
            this._logger = logger;
            this._githubApiService = githubApiService;
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetUser(string user)
        {
            try
            {
                GithubUserResponse githubUser = await this._githubApiService.GetUserInfoAsync(user);

                return Ok(githubUser);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception: {Message}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Message = "An error occurred while processing your request"
                });
            }
        }

        [HttpGet("{user}/repos")]
        public async Task<IActionResult> GetRepos(string user)
        {
            try
            {
                IEnumerable<GithubRepoResponse> repos = await this._githubApiService.GetUserRepositoriesAsync(user);

                return Ok(repos);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception: {Message}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Message = "An error occurred while processing your request"
                });
            }
        }
    }
}
