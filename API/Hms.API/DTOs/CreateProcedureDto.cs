using System.ComponentModel.DataAnnotations;

namespace Hms.API.DTOs
{
    public class CreateProcedureDto
    {
        [Required]
        public string Name { get; set; }

        [Range(1, 100000)]
        public float Cost { get; set; }
    }
}
