using System.ComponentModel.DataAnnotations;

namespace Hms.API.DTOs
{
    public class CreateProcedureDto
    {     
        public string Name { get; set; }

        public float Cost { get; set; }
    }
}
