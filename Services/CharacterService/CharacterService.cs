using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotNetRpg.Data;

namespace dotNetRpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character{ Id = 1, Name = "Emilia" }
        };

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            // response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            
            var dbResponse = await _context.Characters.ToListAsync();
            response.Data = dbResponse.Select(data => _mapper.Map<GetCharacterDto>(data)).ToList();

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            // var character = characters.FirstOrDefault(c => c.Id == id);
            var dbResponse = await _context.Characters.FirstOrDefaultAsync(data => data.Id == id);
            response.Data = _mapper.Map<GetCharacterDto>(dbResponse);
            
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddNewCharacter(AddCharacterDto newCharacter)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            // character.Id = characters.Max(c => c.Id) + 1;
            // characters.Add(character);
            // response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            var dbResponse = await _context.Characters.ToListAsync();
            response.Data = dbResponse.Select(data => _mapper.Map<GetCharacterDto>(data)).ToList();

            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = new ServiceResponse<GetCharacterDto>();

            try {
                // var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id) ?? throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
                var character = await _context.Characters.FirstOrDefaultAsync(data => data.Id == updatedCharacter.Id) ?? throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
                
                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                _context.Characters.Update(character);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(character);
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();

            try {
                // var character = characters.FirstOrDefault(c => c.Id == id) ?? throw new Exception($"Character with Id '{id}' not found.");
                var character = await _context.Characters.FirstOrDefaultAsync(data => data.Id == id) ?? throw new Exception($"Character with Id '{id}' not found.");
                
                // characters.Remove(character);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }
    }
}