using KodeFlow.Data.Context;
using KodeFlow.Filters;
using KodeFlow.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KodeFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TutoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(LogExecucaoFilter))]
        [ServiceFilter(typeof(TempoExecucaoFilter))]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetTutoresAsync()
        {
            try
            {
                var tutores = await _context.Tutores
                        .Include(t => t.Contato)
                        .Include(t => t.Endereco)
                        .AsNoTracking()
                        .ToListAsync();

                return Ok(tutores);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }


        [HttpGet("{id:int}", Name = "GetTutorById")]
        public async Task<ActionResult<Tutor>> GetTutorAsync(int id)
        {
            //var tutor = await _context.Tutores
            //    .Include(t => t.Contato)
            //    .Include(t => t.Endereco)
            //    .FirstOrDefaultAsync(t => t.TutorId == id);

            //if (tutor is null)
            //    return NotFound($"O tutor com id:{id} não existe.");

            //return Ok(tutor);

            throw new ArgumentException("Ocorreu um erro no tratamento do request.");
        }


        [HttpPost]
        public ActionResult<Tutor> Post(Tutor tutor)
        {
            _context.Tutores.Add(tutor);
            _context.SaveChanges();

            return CreatedAtRoute("GetTutorById", new { id = tutor.TutorId }, tutor);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Tutor> Put(int id, Tutor tutor)
        {
            if (id != tutor.TutorId)
                return BadRequest("Dados Inválidos.");

            var tutorExistente = _context.Tutores
                .Include(t => t.Contato)
                .Include(t => t.Endereco)
                .FirstOrDefault(t => t.TutorId == id);

            if (tutorExistente is null)
                return NotFound($"Esse tutor com id: {id} informado não existe...");

            tutorExistente.Nome = tutor.Nome;
            tutorExistente.CPF = tutor.CPF;

            _context.SaveChanges();

            return Ok(tutor);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var tutor = _context.Tutores.Find(id);

            if (tutor is null)
                return NotFound($"Esse tutor com o id:{id} não existe");

            _context.Tutores.Remove(tutor);
            _context.SaveChanges();

            return Ok(tutor);
        }
    }
}
