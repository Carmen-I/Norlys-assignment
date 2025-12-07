using API.BusinessLogic;
using API.Conversion;
using API.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private IPersonLogic _personLogic;
        private ILogger<PersonsController> _logger;
        public PersonsController(IPersonLogic personLogic,ILogger<PersonsController> logger)
        {
            _personLogic = personLogic;
            _logger = logger;
        }

        // GET: api/<PersonsCongtroller>
        [HttpGet]
        public async Task<ActionResult<List<PersonReadDto>>> Get()
        {

            List<Person> people = await _personLogic.GetPeople();
            
            List<PersonReadDto> dtoList = PersonConversion.MapToReadDtoList(people);

            return Ok(dtoList);

        }

        // GET api/<PersonsCongtroller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonReadDto>> Get(int id)
        {
            Person? person = null;
            
            person= (await _personLogic.GetPeople(id)).FirstOrDefault();
            
            PersonReadDto? dto = PersonConversion.MapToReadDto(person);
            
           
            return Ok(dto);
        }

        // POST api/<PersonsCongtroller>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PersonToSaveDto dto)
        {  
            Person p=PersonConversion.MapToPerson(dto);

            int id= await _personLogic.CreatePerson(p);

            _logger.LogInformation($"The person with number {id} was created succesfully.");

            return CreatedAtAction(nameof(Get), new { id = id }, id);
        }

        // PUT api/<PersonsCongtroller>/5
          [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PersonToSaveDto dto)
        {
           Person person = PersonConversion.MapToPerson(dto);
           
            person.Id = id;

           await _personLogic.UpdatePerson(person);

            _logger.LogInformation($"The person with number {id} was updated succesfully.");

            return NoContent();
        }


        // DELETE api/<PersonsCongtroller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _personLogic.DeletePerson(id);

            return  NoContent ();
        }
    }
}
