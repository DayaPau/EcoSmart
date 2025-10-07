using System.ComponentModel.DataAnnotations;

namespace EkoTrack.Models
{
    public class Material
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Nombre { get; set; } = "";

        [Required, StringLength(10)]
        public string Unidad { get; set; } = "kg";
    }
}
