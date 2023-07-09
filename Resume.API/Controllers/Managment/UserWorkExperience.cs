using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Domain;
using Resume.SharedModel.SocialMedia;
using Resume.SharedModel.WorkExperience;

namespace Resume.API.Controllers.Managment {
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorkExperienceController : ControllerBase {
        private readonly IWorkExperienceService _workExperienceService;

        public UserWorkExperienceController(IWorkExperienceService workExperienceService) {
            _workExperienceService = workExperienceService;
        }

        [HttpGet("workExperiences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<WorkExperienceDto>>> GetAllWorkExperienc() {
            return await _workExperienceService.GetAllAsync();
        }

        [HttpGet("get_user_workExperience")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<WorkExperienceDto>>> GetAllUserSocial([FromQuery] Guid userID) {
            return await _workExperienceService.GetAllUserWorkExperienceAsync(userID);
        }

        [HttpGet("{id:guid}", Name = "get_workExperience")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<WorkExperienceDto>> GetSocial([FromRoute] Guid id) {
            return await _workExperienceService.GetAsync(id);
        }

        [HttpPost("create_workExperience")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Create([FromForm] CreateWorkExperience p) {
            return await _workExperienceService.CreateAsync(p);
        }

        [HttpPut("Edit_workExperience")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Edit([FromForm] UpdateWorkExperience p) {
            return await _workExperienceService.EditAsync(p);
        }

        [HttpDelete("delete_workExperience")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Delete([FromQuery] RemoveworkExperience p) {
            return await _workExperienceService.DeleteAsync(p.Id, p.UserId);
        }
    }
}
