using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.SharedModel.SocialMedia;

namespace Resume.API.Controllers.Managment {
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class UserSocialMediaController : ControllerBase {
        private readonly ISocialMediaService _socialMediaService;

        public UserSocialMediaController(ISocialMediaService socialMediaService) {
            _socialMediaService = socialMediaService;
        }

        [HttpGet("socials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<SocialMediaDto>>> GetAllSocial() {
            return await _socialMediaService.GetAllAsync();
        }

        [HttpGet("get_user_socials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<SocialMediaDto>>> GetAllUserSocial([FromQuery] Guid userID) {
            return await _socialMediaService.GetAllUserSocialAsync(userID);
        }

        [HttpGet("{id:guid}", Name = "get_social")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<SocialMediaDto>> GetSocial([FromRoute] Guid id) {
            return await _socialMediaService.GetAsync(id);
        }

        [HttpPost("create_social")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Create([FromForm] CreateSocialMedia p) {
            return await _socialMediaService.CreateAsync(p);
        }

        [HttpPut("Edit_social")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Edit([FromForm] UpdateSocialMedia p) {
            return await _socialMediaService.EditAsync(p);
        }

        [HttpDelete("delete_social")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Delete([FromQuery] RemoveSocialMediaParam p) {
            return await _socialMediaService.DeleteAsync(p.Id, p.UserId);
        }
    }
}
