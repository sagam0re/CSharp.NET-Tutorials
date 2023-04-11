using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;
        public CharacterController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _context.Characters.Where(c => c.UserId == userId).Include(c => c.Weapon).Include(c => c.Skills).ToListAsync();

            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(createCharacterDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (User == null)
            {
                return NotFound("User not found");
            }

            var newCharacter = new Character
            {
                UserId = request.UserId,
                Name = request.Name,
                RpgClass = request.RpgClass,
                User = user,
            };

           _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return await Get(newCharacter.UserId);
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDto request)
        {
            var character = await _context.Characters.FindAsync(request.CharacterId);
            if (character == null)
            {
                return NotFound("Character not found");
            }

            var newWeapon = new Weapon
            {
               Name= request.Name,
               Damage= request.Damage,
               Character = character
            };

            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();

            return Ok(character);
        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto request)
        {
            var character = await _context.Characters.Where(c => c.Id == request.CharacterId).Include(c => c.Skills).FirstOrDefaultAsync();
            if (character == null)
            {
                return NotFound("Character not found");
            }

            var skill = await _context.Skills.FindAsync(request.SkillId);
            if (skill == null)
            {
                return NotFound("Skill not found");
            }

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return Ok(character);
        }
    }
}
