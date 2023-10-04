using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotNetRpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get() {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetById(int id) {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddNewCharacter(AddCharacterDto newCharacter) {
            return Ok(await _characterService.AddNewCharacter(newCharacter));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter) {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if (response.Data is null) return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id) {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Data is null) return NotFound(response);

            return Ok(response);
        }
    }
}
