using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkoTrack.Models
{
    public class RecyclingRecord
    {
        public int Id { get; set; }

       
        [Required]
        public int UserId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required, Range(0.001, 999999, ErrorMessage = "Ingresa una cantidad válida")]
        public decimal Cantidad { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [StringLength(200)]
        public string? Notas { get; set; }

        
        public Material? Material { get; set; }
    }
}

