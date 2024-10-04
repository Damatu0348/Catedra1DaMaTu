using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(12, MinimumLength = 9, ErrorMessage = "El RUT debe tener entre 9 y 12 caracteres.")]
        public string Rut { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        public string Email { get; set; }
        [RegularExpression(@"Masculino|Femenino|Otro|Prefiero no decirlo")]
        public string Gender { get; set; }
        public string BirthDate { get; set; }

    }
}