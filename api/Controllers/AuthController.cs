using System.Security.Claims;
using System.Web.Http;
using axians.contacts.services.Data.Repositories.Abstraction;
using axians.contacts.services.Models;
using NLog;
using axians.contacts.services.Services.Implementation;

namespace api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private IAuthRepository _authRepository;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public IHttpActionResult Register(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_authRepository.UserExists(model.Username))
            {
                return BadRequest("Username already exists");
            }

            var passwordHash = HashService.ComputeSHA256Hash( model.Password); 

            var userId = _authRepository.RegisterUser(model.Username, passwordHash, model.FullName);

            logger.Info($"{model.Username} registered");

            return Ok(new { UserId = userId });
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IHttpActionResult Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isValid =  _authRepository.ValidateUser(model.Username, HashService.ComputeSHA256Hash(model.Password));

            if (!isValid)
            {
                return Unauthorized();
            }

            logger.Info($"{model.Username} logged in");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.NameIdentifier, _authRepository.GetIdByUsername(model.Username).ToString()),
            });

            var tokenString = TokenService.GenerateToken(identity);

            return Ok(new { Token = tokenString });
        }

    }
}