using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Web.Http;
using axians.contacts.services.Data.Repositories.Abstraction;
using axians.contacts.services.Data.Repositories.Implementation;
using Microsoft.IdentityModel.Tokens;
using axians.contacts.services.Models;
using api.Utils;
using NLog;

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

            var passwordHash = HashHelper.ComputeSHA256Hash( model.Password); 

            var userId = _authRepository.RegisterUser(model.Username, passwordHash, model.FullName);

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

            var isValid =  _authRepository.ValidateUser(model.Username, HashHelper.ComputeSHA256Hash(model.Password));

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

            var tokenString = TokenHelper.GenerateToken(identity);

            return Ok(new { Token = tokenString });
        }

    }
}