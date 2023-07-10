using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.SharedModel.Skills;

namespace Resume.API.Controllers.Managment {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserSkillsController : ControllerBase {
        private readonly IUserSkillService _userSkillService;

        public UserSkillsController(IUserSkillService userSkillService) {
            _userSkillService = userSkillService;
        }
        [HttpGet("skills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<SkillDto>>> GetAll() {
            return await _userSkillService.GetAllAsync();
        }


        [HttpGet("get_user_skills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<SkillDto>>> GetAllUserSkill([FromQuery] Guid userID) {
            return await _userSkillService.GetAllUserSkillAsync(userID);
        }


        [HttpGet("{id:guid}",Name = "get_skill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<SkillDto>> Getkill([FromRoute]Guid id) {
            return await _userSkillService.GetAsync(id);
        }


        [HttpPost("create_skill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult> Create([FromForm] CreateSkillParam p) {
            return await _userSkillService.CreateAsync(p);
        }


        [HttpPut("edit_skill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult> Edit([FromForm] UpdateSkillParam p) {
            return await _userSkillService.EditAsync(p);
        }


        [HttpDelete("remove_skill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult> Delete([FromQuery]RemoveSkillParam p) {
            return await _userSkillService.DeleteAsync(p.SkillId,p.UserId);
        }

    }
}
