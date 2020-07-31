using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiWebApiM3.Context;
using MiWebApiM3.Entities;
using MiWebApiM3.Helpers;
using MiWebApiM3.Models;
using MiWebApiM3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors("PermitirApiRequest")]
    //[Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ClaseB claseB;
        private readonly ILogger<AutoresController> logger;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, ClaseB claseB, ILogger<AutoresController> logger, IMapper mapper)
        {
            this.context = context;
            this.claseB = claseB;
            this.logger = logger;
            this.mapper = mapper;
        }


        //[ResponseCache(Duration = 15)]
        [HttpGet("/listado")]
        [HttpGet("listado")]
        [HttpGet]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get(int numeroDePagina = 1, int cantidadDeRegistros = 10)
        {
            var query = context.Autores.Include(li => li.Libros).AsQueryable();

            var totalDeRegistros = query.Count();

            logger.LogInformation("Obteniendo los autores");
            claseB.HacerAlgo();
            var autores = await query
                .Skip(cantidadDeRegistros * (numeroDePagina - 1))
                .Take(cantidadDeRegistros)
                .ToListAsync();

            Response.Headers["X-Total-Registros"] = totalDeRegistros.ToString();
            Response.Headers["X-Cantidad-Paginas"] =
                ((int)Math.Ceiling((double)totalDeRegistros / cantidadDeRegistros)).ToString();

            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);

            return autoresDTO;
        }

        [HttpGet("{id}", Name = "ObtenerAutor")] 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                logger.LogWarning($"El autor de id {id} no ha sido encontrado");
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return autorDTO;

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {
            var autor = mapper.Map<Autor>(autorCreacion);

            TryValidateModel(autor);
            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        } 

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            var autorDeLaDB = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autorDeLaDB == null)
                return NotFound();

            var autorDTO = mapper.Map<AutorCreacionDTO>(autorDeLaDB);

            patchDocument.ApplyTo(autorDTO, ModelState);

            mapper.Map(autorDTO, autorDeLaDB);

            var isValid = TryValidateModel(autorDeLaDB);

            if (!isValid)
                return BadRequest(ModelState);

            await context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Borra un elemento en específico
        /// </summary>
        /// <param name="id">Id del elemento a borrar</param>  
        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            var autorId = await context.Autores.Select(i => i.Id).FirstOrDefaultAsync(x => x == id);

            if (autorId == default(int))
                return NotFound();

            context.Remove(new Autor { Id = autorId });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
