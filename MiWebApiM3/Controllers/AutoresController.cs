using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiWebApiM3.Context;
using MiWebApiM3.Entities;
using MiWebApiM3.Helpers;
using MiWebApiM3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ClaseB claseB;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, ClaseB claseB, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.claseB = claseB;
            this.logger = logger;
        }


        //[ResponseCache(Duration = 15)]
        [HttpGet("/listado")]
        [HttpGet("listado")]
        [HttpGet]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult<IEnumerable<Autor>> Get()
        {
            //throw new NotImplementedException();
            logger.LogInformation("Obteniendo los autores");
            claseB.HacerAlgo();
            return context.Autores.Include(li => li.Libros).ToList();
        }

        [HttpGet("{id}", Name = "ObtenerAutor")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                logger.LogWarning($"El autor de id {id} no ha sido encontrado");
                return NotFound();
            }

            return autor;

        }

        [HttpPost]
        public ActionResult Post([FromBody] Autor autor)
        {
            TryValidateModel(autor);
            context.Autores.Add(autor);
            context.SaveChanges();

            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autor);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            if (id != value.Id)
                return BadRequest();

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }


        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor = context.Autores.FirstOrDefault(x => x.Id == id);

            if (autor == null)
                return NotFound();

            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }
    }
}
