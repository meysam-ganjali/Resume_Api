using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.SharedModel.Skills.SkillTypes;

namespace Resume.API.Controllers.Managment {
    [Authorize(Roles = SD.AdminManagment)]
    [Route("api/[controller]")]
    [ApiController]
    public class SkillTypesController : ControllerBase {
        private readonly ISkillTypeService _skillTypeService;

        public SkillTypesController(ISkillTypeService skillTypeService)
        {
            _skillTypeService = skillTypeService; 
        }

        [HttpGet("get_all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<BaseResult<List<SkillTypeDto>>> GetAll()
        {
            return await _skillTypeService.GetAllAsync();
        }
        [HttpGet("get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<BaseResult<SkillTypeDto>> Get([FromQuery] Guid id) {
            return await _skillTypeService.GetAsync(id);
        }
        [HttpPost("create_skilltype")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Create([FromBody] CreateSkillTypeParam p) {
            return await _skillTypeService.CreateAsync(p);
        }
        [HttpPut("Edit_skilltype")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Edit([FromBody] UpdateSkillTypeParam p) {
            return await _skillTypeService.EditAsync(p);
        }
        [HttpDelete("remove_skilltype")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<BaseResult> Edit([FromQuery] Guid id) {
            return await _skillTypeService.DeleteAsync(id);
        }
    }
}
