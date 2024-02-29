using Microsoft.AspNetCore.Mvc;
using Profunion.Models;
using Profunion.Dto.AnnouncementsDto;
using Profunion.Interfaces;
using AutoMapper;
using Profunion.Services;

namespace Profunion.Controllers.AnnouncementsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _announcementsRepository;
        private readonly IMapper _mapper;

        public AnnouncementsController(IAnnouncementsRepository announcementsRepository, IMapper mapper)
        {
            _announcementsRepository = announcementsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Announcements>))]
        public async Task<IActionResult> GetAnnouncements()
        {
            var announcements = _mapper.Map<List<AnnouncementsDto>>(await _announcementsRepository.GetAnnouncements());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(announcements);
        }

        [HttpGet("Titles")]
        [ProducesResponseType(200, Type = typeof(Announcements))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAnnouncemetnsByID(string titles)
        {
            if (!await _announcementsRepository.AnnouncementsExists(titles))
                return NotFound();

            var user = _mapper.Map<AnnouncementsDto>(await _announcementsRepository.GetAnnouncementsByTitles(titles));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost("Create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAnnouncements([FromBody] AnnouncementsDto announcementsCreate)
        {
            var announcements = await _announcementsRepository.GetAnnouncements();

            var existingAnnouncements = announcements
                .FirstOrDefault(a => a.AnnouncementsID.Trim().ToUpper() == announcementsCreate.AnnouncementsID.ToUpper());

            if (existingAnnouncements != null)
            {
                ModelState.AddModelError(" ", "Такое обьявление уже создано");
            }

            if (announcementsCreate == null)
                return BadRequest(ModelState);

            var announcement = announcements
                .Where(a => a.Titles.Trim().ToUpper() == announcementsCreate.Titles.ToUpper()
                && a.Descriptions.Trim().ToUpper() == announcementsCreate.Descriptions.ToUpper()
                && a.PhotoPath.Trim().ToUpper() == announcementsCreate.PhotoPath.ToUpper()
                ).FirstOrDefault();

           

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcementsMap = _mapper.Map<Announcements>(announcementsCreate);

            if (await _announcementsRepository.CreateAnnouncements(announcementsMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так при сохранении");
                return StatusCode(500, ModelState);
            }
            return Ok("Обьявление успешно создан");
        }



    }
}
