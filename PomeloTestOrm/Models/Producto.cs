using System.ComponentModel.DataAnnotations;

namespace PomeloTestOrm.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
    [Required]
    public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
    }
}
