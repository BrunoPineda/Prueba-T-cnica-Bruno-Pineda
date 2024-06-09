using Apuestas.Data;
using Apuestas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Apuestas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] UsuarioDto usuarioDto)
        {
            var usuarios = _context.Usuarios.AsEnumerable(); // Trae los datos al cliente
            var usuario = usuarios.FirstOrDefault(u => string.Equals(u.Nombre, usuarioDto.Nombre, StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Monto = usuarioDto.Monto
                };
                _context.Usuarios.Add(usuario);
            }
            else
            {
                usuario.Monto += usuarioDto.Monto;
            }

            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        [HttpGet("{nombre}")]
        public Task<IActionResult> GetUserMonto(string nombre)
        {
            var usuarios = _context.Usuarios.AsEnumerable(); // Trae los datos al cliente
            var usuario = usuarios.FirstOrDefault(u => string.Equals(u.Nombre, nombre, StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }

            return Task.FromResult<IActionResult>(Ok(usuario.Monto));
        }
    }
}

public class UsuarioDto
{
    public required string Nombre { get; set; }
    public decimal Monto { get; set; }
}
