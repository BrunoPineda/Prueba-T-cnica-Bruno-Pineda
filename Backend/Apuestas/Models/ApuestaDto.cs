namespace Apuestas.Models
{
    public class ApuestaDto
    {
        public required string Nombre { get; set; }
        public decimal Monto { get; set; }
        public required string TipoApuesta { get; set; }
        public int Numero { get; set; }
        public required string Color { get; set; }
    }
}
