using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Profile.API.Services.Interfaces;
using PaymentPlatform.Profile.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.Controllers
{
    /// <summary>
    /// Основной контроллер для Profile.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfilesController : Controller
    {
        private readonly IProfileService _profileService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="profileService">profile сервис.</param>
        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/profiles
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<ProfileViewModel>> GetProfiles(int? take, int? skip)
        {
            return await _profileService.GetAllProfilesAsync(take, skip);
        }

        // GET: api/profiles/{id}
        [Authorize(Roles = "User, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = await _profileService.GetProfileByIdAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        // POST: api/profiles
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewProfile([FromBody] ProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, success) = await _profileService.AddNewProfileAsync(profile);

            if (!success)
            {
                return Conflict(result);
            }
			
            profile.Id = new Guid(result);

            return CreatedAtAction(nameof(AddNewProfile), profile);
        }

        // PUT: api/profiles/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileViewModel = await _profileService.GetProfileByIdAsync(profile.Id);
            var profileExist = profileViewModel != null;

            if (!profileExist)
            {
                return NotFound();
            }

            var updatedResult = await _profileService.UpdateProfileAsync(profile);

            if (!updatedResult)
            {
                return Conflict();
            }

            return Ok(profile);
        }
    }
}
