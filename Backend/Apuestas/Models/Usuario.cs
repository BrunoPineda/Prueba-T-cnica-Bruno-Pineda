namespace Apuestas.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public decimal Monto { get; set; }
    }
}
