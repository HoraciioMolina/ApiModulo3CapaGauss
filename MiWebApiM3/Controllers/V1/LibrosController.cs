using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiWebApiM3.Context;
using MiWebApiM3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Libro>> Get()
        {
            return context.Libros.Include(au => au.Autor).ToList();
        }

        [HttpGet("{id}", Name="ObtenerLibro")]
        public ActionResult<Libro> Get(int id)
        {
            var libro = context.Libros.Include(au => au.Autor).FirstOrDefault(x => x.Id == id);

            if (libro == null)
                return NotFound();

            return libro;

        }

        [HttpPost]
        public ActionResult Post([FromBody] Libro libro)
        {
            context.Libros.Add(libro);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.Id }, libro);
        }
    }
}
