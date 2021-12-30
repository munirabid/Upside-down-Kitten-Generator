using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealworldOneBackendTest.Helpers;
using RealworldOneBackendTest.Models;
using RealworldOneBackendTest.Services.Interfaces;

namespace RealworldOneBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService _userService, IOptions<AppSettings> appSettings)
        {
            this.userService = _userService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Registers user using in memory database.
        /// </summary>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]

        [HttpPost]
        public async Task<ActionResult> RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool userExists = await userService.UserExists(user.Username);

                    if (userExists)
                    {
                        return StatusCode(409, "User with given username already exists");
                    }

                    var savedUser = await userService.RegisterUser(user);

                    return CreatedAtAction("GetUser", new { id = user.ID }, savedUser);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Can not process request at the moment");
                }
            }

            return BadRequest();

        }

        #region Authentication

        /// <summary>
        /// Authenticates user and return JWT Token with user data.
        /// </summary>
        [ProducesResponseType(typeof(AuthenticateResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody]AuthenticateModel model)
        {
            try
            {
                var user = await userService.Authenticate(model.Username, model.Password);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                // authentication successful so generate jwt token
                var token = generateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Can not process request at the moment");
            }

        }

        #endregion

        // GET: api/Users
        /// <summary>
        /// Retrieves list of users. Uses JWT Authentication.
        /// </summary>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]

        [HttpGet]
        [Authorize(AuthenticationSchemes = SD.Bearer)]
        public async Task<ActionResult> GetUsers()
        {
            var users = await userService.GetUsers();
            return Ok(users);
        }

        // GET: api/Users/5
        /// <summary>
        /// Retrieves a specific user by unique id, uses JWT authntication.
        /// </summary>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]

        [HttpGet]
        [Authorize(AuthenticationSchemes = SD.Bearer)]
        [Route("GetUser/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Retrieves a specific user by unique id, uses Basic authentication.
        /// </summary>
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]

        [HttpGet]
        [Route("GetUserById/{id}")]
        [Authorize(AuthenticationSchemes = SD.Basic)]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }      

        #region Private Methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.AppSecretKey);
            var issuer = _appSettings.Issuer;
            var audience = _appSettings.Audience;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.ID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
