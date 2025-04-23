using Meishi.Server.Models.Response;
using Meishi.Server.Services.GithubApi;
using Meishi.Server.Services.GithubAuth;
using Meishi.Server.Services.GithubAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Meishi.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IGithubAuthService _githubAuthService;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(ILogger<UserController> logger, IGithubAuthService githubAuthService, IHttpClientFactory httpClientFactory)
        {
            this._logger = logger;
            this._githubAuthService = githubAuthService;
            this._httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Token([FromQuery] string code)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(code, nameof(code));

                var githubApiService =
                    new GithubApiService(
                       this._httpClientFactory,
                       this._githubAuthService,
                       code
                    );

                await githubApiService.GetUserInfoAsync();

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                this._logger.LogError(ex, "ArgumentNullException: {ParamName}", ex.ParamName);

                return BadRequest(new ErrorResponse
                {
                    Message = $"\"{ex.ParamName}\" not informed"
                });
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
