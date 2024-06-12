using Apuestas.Data;
using Apuestas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apuestas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly Random _random = new Random();
        private static readonly Dictionary<int, string> NumberColorMapping = new Dictionary<int, string>
        {
            { 1, "rojo" }, { 2, "negro" }, { 3, "rojo" }, { 4, "negro" }, { 5, "rojo" },
            { 6, "negro" }, { 7, "rojo" }, { 8, "negro" }, { 9, "rojo" }, { 10, "negro" },
            { 11, "negro" }, { 12, "rojo" }, { 13, "negro" }, { 14, "rojo" }, { 15, "negro" },
            { 16, "rojo" }, { 17, "negro" }, { 18, "rojo" }, { 19, "rojo" }, { 20, "negro" },
            { 21, "rojo" }, { 22, "negro" }, { 23, "rojo" }, { 24, "negro" }, { 25, "rojo" },
            { 26, "negro" }, { 27, "rojo" }, { 28, "negro" }, { 29, "negro" }, { 30, "rojo" },
            { 31, "negro" }, { 32, "rojo" }, { 33, "negro" }, { 34, "rojo" }, { 35, "negro" },
            { 36, "rojo" }, { 0, "verde" }
        };

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre.ToLower() == usuarioDto.Nombre.ToLower());

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

        [HttpPost("apostar")]
        public async Task<IActionResult> Apostar([FromBody] ApuestaDto apuestaDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre.ToLower() == apuestaDto.Nombre.ToLower());

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (usuario.Monto < apuestaDto.Monto)
            {
                return BadRequest("Saldo insuficiente");
            }

            // Restar el monto apostado del saldo del usuario
            usuario.Monto -= apuestaDto.Monto;

            // Generar un resultado aleatorio para la ruleta
            int numeroResultado = _random.Next(0, 37);
            string colorResultado = NumberColorMapping[numeroResultado];

            // Calcular el monto del premio
            decimal premio = 0;

            if (apuestaDto.TipoApuesta == "numero" && apuestaDto.Numero.HasValue && apuestaDto.Color != null)
            {
                if (apuestaDto.Numero == numeroResultado && apuestaDto.Color.ToLower() == colorResultado)
                {
                    premio = apuestaDto.Monto * 3; // Ganar el triple de la cantidad apostada
                }
            }
            else if (apuestaDto.TipoApuesta == "color" && apuestaDto.Color != null)
            {
                if (apuestaDto.Color.ToLower() == colorResultado)
                {
                    premio = apuestaDto.Monto / 2; // Ganar la mitad de la cantidad apostada
                }
            }
            else if (apuestaDto.TipoApuesta == "pares" && apuestaDto.Color != null)
            {
                if (numeroResultado % 2 == 0 && apuestaDto.Color.ToLower() == colorResultado)
                {
                    premio = apuestaDto.Monto; // Ganar la cantidad apostada
                }
            }
            else if (apuestaDto.TipoApuesta == "impares" && apuestaDto.Color != null)
            {
                if (numeroResultado % 2 != 0 && apuestaDto.Color.ToLower() == colorResultado)
                {
                    premio = apuestaDto.Monto; // Ganar la cantidad apostada
                }
            }

            // Añadir el premio al saldo del usuario
            usuario.Monto += premio;

            await _context.SaveChangesAsync();
            return Ok(new { numeroResultado, colorResultado, premio, saldo = usuario.Monto });
        }

        [HttpGet("{nombre}")]
        public async Task<IActionResult> GetUserMonto(string nombre)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre.ToLower() == nombre.ToLower());

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario.Monto);
        }
    }
}

public class UsuarioDto
{
    public required string Nombre { get; set; }
    public decimal Monto { get; set; }
}

public class ApuestaDto
{
    public required string Nombre { get; set; }
    public decimal Monto { get; set; }
    public int? Numero { get; set; }
    public required string Color { get; set; }
    public required string TipoApuesta { get; set; }
}
