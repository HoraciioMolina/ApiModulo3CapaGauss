using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using MiWebApiM3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class ValuesController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly HashService _hashService;

        public ValuesController(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            _protector = protectionProvider.CreateProtector("valor_unico_y_quizas_secreto");
            _hashService = hashService;
        }

        // GET api/values
        [HttpGet]
        [EnableCors("PermitirApiRequest")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("hash")]
        public ActionResult GetHash()
        {
            string textoPlano = "Felipe Gavilán";
            var hashResult1 = _hashService.Hash(textoPlano).Hash;
            var hashResult2 = _hashService.Hash(textoPlano).Hash;
            return Ok(new { textoPlano, hashResult1, hashResult2 });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            string textoPlano = "Felipe Gavilán";
            string textoCifrado = _protector.Protect(textoPlano);
            string textoDesencriptado = _protector.Unprotect(textoCifrado);
            return Ok(new { textoPlano, textoCifrado, textoDesencriptado });
        }

        private void EjemploDeEncriptacionLimitadaPorTiempo()
        {
            var protectorLimitadoPorTiempo = _protector.ToTimeLimitedDataProtector();
            string textoPlano = "Felipe Gavilán";
            string textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, TimeSpan.FromSeconds(5));
            string textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            return Ok();
        }
    }
}
