using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiWebApiM3.Context;
using MiWebApiM3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiWebApiM3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsuariosController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost("AsignarUsuarioRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (usuario == null)
                return NotFound();

            //Autenticacion clasica con Identity
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            
            //Autenticacion con Jwt
            await userManager.AddToRoleAsync(usuario, editarRolDTO.RoleName);

            return Ok();
        }



        [HttpPost("RemoverUsuarioRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (usuario == null)
                return NotFound();

            //Autenticacion clasica con Identity
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));

            //Autenticacion con Jwt
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RoleName);

            return Ok();
        }

    }
}
