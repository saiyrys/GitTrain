using Konscious.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Profunion.Interfaces;
using Profunion.Models;
using Profunion.Dto.UserDto;
using Profunion.Services.UserServices;

namespace Profunion.Controllers.UserControllers
{
    [Route("api/auth")]
    [ApiController]
    public class Authentifications : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly HashingPassword _hashingPassword;
        private readonly GenerateMultipleJWT _generateMJWT;

        public Authentifications(IConfiguration configuration,
            IUserRepository userRepository,
            IMapper mapper,
            HashingPassword hashingPassword,
            GenerateMultipleJWT generateMJWT)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _mapper = mapper;
            _hashingPassword = hashingPassword;
            _generateMJWT = generateMJWT;
        }


        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userCreate)
        {
            var users = await _userRepository.GetUsers();
            var existingUser = users
                .FirstOrDefault(u => u.Username.Trim().ToUpper() == userCreate.Username.ToUpper());

            if (existingUser != null)
            {
                ModelState.AddModelError(" ", "Пользователь с таким псевдонимом уже существует.");
                return StatusCode(422, ModelState);
            }

            if (userCreate == null)
                return BadRequest(ModelState);

            var user = users
                .Where(u => u.Username.Trim().ToUpper() == userCreate.Username.ToUpper()
                && u.FirstName.Trim().ToUpper() == userCreate.FirstName.ToUpper()
                && u.LastName.Trim().ToUpper() == userCreate.LastName.ToUpper()
            )
            .FirstOrDefault();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userCreate.PasswordHash = _hashingPassword.HashPassword(userCreate.PasswordHash);

            var userMap = _mapper.Map<User>(userCreate);

            if (userMap.userRole == 0)
                userMap.userRole = UserRole.User;
            else
            {
                userMap.userRole = UserRole.Admin;
            }

            if (await _userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так при сохранении");
                return StatusCode(500, ModelState);
            }
            return Ok("Пользолватель успешно создан");
        }

        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUser)
        {
            var users = await _userRepository.GetUsers();
            var user = users
                .Where(u => u.Username.Trim().ToUpper() == loginUser.Username.ToUpper()
                )
                .FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError(" ", "Такого пользователя не существует");
                return StatusCode(422, ModelState);
            }

            if (string.IsNullOrEmpty(loginUser.PasswordHash))
            {
                ModelState.AddModelError(" ", "Пароль не может быть пустым");
                return StatusCode(422, ModelState);
            }


            var accessToken = _generateMJWT.GenerateAccessToken(user);
            var refreshToken = _generateMJWT.GenerateRefreshToken();

            var userMap = _mapper.Map<User>(loginUser);

            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            if (user.IsAdmin)
            {
                return Ok($"Вход выполнен в роли администратора");
            }
            return Ok(response);
        }
    }
}
