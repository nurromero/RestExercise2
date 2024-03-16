using Microsoft.AspNetCore.Mvc;
using ActorRepositoryLib;
using System.Reflection.Metadata.Ecma335;



namespace RestExercise1.Controllers
{
    //fx localhost:5018/api/Actors/
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ActorsRepository _actorsRepository;

        // Constructor injection of the ActorsRepository
        public ActorsController(ActorsRepository actorsRepository)
        {
            _actorsRepository = actorsRepository;
            GenerateActors();
        }

        private void GenerateActors()
        {
            List<Actor> actors = new List<Actor>
            {
                new Actor { Name = "Tom Hanks", BirthYear = 1956 },
                new Actor { Name = "Meryl Streep", BirthYear = 1949 },
                new Actor { Name = "Leonardo DiCaprio", BirthYear = 1974 },
                new Actor { Name = "Emma Stone", BirthYear = 1988 },
                new Actor { Name = "Denzel Washington", BirthYear = 1954 },
                new Actor { Name = "Scarlett Johansson", BirthYear = 1984 },
                new Actor { Name = "Brad Pitt", BirthYear = 1963 },
                new Actor { Name = "Jennifer Lawrence", BirthYear = 1990 },
                new Actor { Name = "Johnny Depp", BirthYear = 1963 },
                new Actor { Name = "Angelina Jolie", BirthYear = 1975 },
                new Actor { Name = "Robert Downey Jr.", BirthYear = 1965 },
                new Actor { Name = "Charlize Theron", BirthYear = 1975 },
                new Actor { Name = "Will Smith", BirthYear = 1968 },
                new Actor { Name = "Natalie Portman", BirthYear = 1981 },
                new Actor { Name = "Matt Damon", BirthYear = 1970 }
            };

            foreach (var actor in actors)
            {
                _actorsRepository?.Add(actor);
            }
        }

        // GET: api/Actors
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult Get()
        {
            if (_actorsRepository.Get().Any())
            {
                return Ok(_actorsRepository.Get());
            }

            return NotFound("No actors found, sorry.");
        }

        // GET api/Actors/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (!(id < 0) && _actorsRepository.Get().Any(a => a.Id == id))
            {
                return Ok(_actorsRepository.GetId(id));
            }

            return NotFound("No actor with that ID found, sorry.");
        }

        // POST api/Actors
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPost]
        public ActionResult Post([FromBody] Actor actor)
        {
            try
            {
                if (actor.Validate())
                {
                    return Ok(_actorsRepository.Add(actor));
                }

            }
            catch(ArgumentNullException)
            {

                return BadRequest("Name cannot be empty.");

            }
            catch (ArgumentException)
            {
                return BadRequest("Name must be 3 characters or longer, and birthyear must be 1820 or later.");
            }

            return BadRequest();
        }

        // PUT api/Actors/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Actor actor)
        {
            if (_actorsRepository.Get().Any(a => a.Id == id) && actor.Validate())
            {
                return Ok(_actorsRepository?.Update(id, actor));
            }

            return BadRequest("Dont change the ID, name must be longer than 3 & birthyear must be 1820 or after.");
        }

        // DELETE api/Actors/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!(id < 0) && _actorsRepository.Get().Count() > id)
            {
                return Ok(_actorsRepository.Delete(id));
            }

            return BadRequest("ID not found."); 
        }
    }
}
