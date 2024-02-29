using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Profunion.Interfaces;
using Profunion.Models;
using Profunion.Dto.UserDto;

namespace Profunion.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(await _userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{userid}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUser(string userid)
        {
            if (!await _userRepository.UserExists(userid))
                return NotFound();

            var user = _mapper.Map<UserDto>(await _userRepository.GetUserByID(userid));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPatch()]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string Username, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (Username != updatedUser.Username)
                return BadRequest(ModelState);

            if (!await _userRepository.UserExists(Username))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(updatedUser);

            if (await _userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Произошла ошибка обновления пользователя");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string userid)
        {

            if (!await _userRepository.UserExists(userid))
                return NotFound();

            var userToDelete = await _userRepository.GetUserByID(userid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError(" ", "Ошибка удаления пользователя");
            }

            return NoContent();
        }
    }
}
