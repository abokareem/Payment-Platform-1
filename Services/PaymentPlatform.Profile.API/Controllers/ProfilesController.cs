using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Framework.Constants.Logger;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Profile.API.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfilesController : Controller
    {
        private readonly IProfileService _profileService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="profileService">Profile сервис.</param>
        public ProfilesController(IProfileService profileService) => _profileService = profileService ?? throw new ArgumentException(nameof(profileService));

        // GET: api/profiles
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<ProfileViewModel>> GetProfiles(int? take, int? skip)
        {
            var profiles = await _profileService.GetAllProfilesAsync(take, skip);
            var count = profiles.ToList().Count;

            Log.Information($"{count} {ProfileLoggerConstants.GET_PROFILES}");

            return profiles;
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
                Log.Warning($"{profile.Id} {ProfileLoggerConstants.GET_PROFILE_NOT_FOUND}");

                return NotFound();
            }

            Log.Information($"{profile.Id} {ProfileLoggerConstants.GET_PROFILE_FOUND}");

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
                Log.Warning($"{profile.Id} {ProfileLoggerConstants.ADD_PROFILE_CONFLICT}");

                return Conflict(result);
            }

            profile.Id = new Guid(result);

            Log.Information($"{profile.Id} {ProfileLoggerConstants.ADD_PROFILE_OK}");

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
                Log.Warning($"{profile.Id} {ProfileLoggerConstants.GET_PROFILE_NOT_FOUND}");

                return NotFound();
            }

            var updatedResult = await _profileService.UpdateProfileAsync(profile);

            if (!updatedResult)
            {
                Log.Warning($"{profile.Id} {ProfileLoggerConstants.UPDATE_PROFILE_CONFLICT}");

                return Conflict();
            }

            Log.Information($"{profile.Id} {ProfileLoggerConstants.UPDATE_PROFILE_OK}");

            return Ok(profile);
        }
    }
}