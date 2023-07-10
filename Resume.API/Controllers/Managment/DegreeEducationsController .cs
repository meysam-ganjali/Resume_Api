using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.SharedModel.DegreeEducations;

namespace Resume.API.Controllers.Managment {
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class DegreeEducationsController : ControllerBase {
        private readonly IDegreeEducationService _degreeEducationService;

        public DegreeEducationsController(IDegreeEducationService degreeEducationService) {
            _degreeEducationService = degreeEducationService;
        }

        [HttpGet("degree_educations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<DegreeEducationDto>>> GetAllDegreeEdu() {
            return await _degreeEducationService.GetAllAsync();
        }

        [HttpGet("get_user_degree_educations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<DegreeEducationDto>>> GetAllUserDegreeEdu([FromQuery] Guid userID) {
            return await _degreeEducationService.GetAllUserDegreeEducationAsync(userID);
        }

        [HttpGet("{id:guid}", Name = "get_degree_education")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<DegreeEducationDto>> GetDegreeEdu([FromRoute] Guid id) {
            return await _degreeEducationService.GetAsync(id);
        }

        [HttpPost("create_degree_education")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Create([FromForm] CreateDegreeEducation p) {
            return await _degreeEducationService.CreateAsync(p);
        }

        [HttpPut("Edit_degree_education")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Edit([FromForm] UpdateDegreeEducation p) {
            return await _degreeEducationService.UpdateAsync(p);
        }

        [HttpDelete("delete_degree_education")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Delete([FromQuery] RemoveDegreeEducationParam p) {
            return await _degreeEducationService.DeleteAsync(p.Id, p.UserId);
        }
    }
}
