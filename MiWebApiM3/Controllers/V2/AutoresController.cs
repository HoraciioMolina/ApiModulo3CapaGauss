using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiWebApiM3.Context;
using MiWebApiM3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Controllers.V2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    //[HttpHeaderIsPresent("x-version", "2")]
    public class AutoresController : ControllerBase
    {
        private ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet(Name = "ObtenerAutoresV2")]
        public async Task<ActionResult<IEnumerable<Autor>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            return autores;
        }
    }
}
