using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiWebApiM3.Models
{
    public class AutorCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }

        public int Edad { get; set; }

        public string CreditCard { get; set; }

        public string Url { get; set; }

    }
}
