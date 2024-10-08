using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        [StringLength(12, MinimumLength = 9, ErrorMessage = "El RUT debe tener entre 9 y 12 caracteres.")]
        public string Rut { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression(@"Masculino|Femenino|Otro|Prefiero no decirlo", ErrorMessage = "El género no es válido.")]
        public string Gender { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
    }
}