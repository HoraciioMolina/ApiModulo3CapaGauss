using MiWebApiM3.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Entities
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(10, ErrorMessage = "El campo debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }

        [Range(18, 120)]
        public int Edad { get; set; }

        //[CreditCard]
        public string CreditCard { get; set; }

        [Url]
        public string Url { get; set; }

        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if(primeraLetra!= primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula");
                }
            }
        }
    }
}
