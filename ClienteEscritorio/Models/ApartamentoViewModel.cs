using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteEscritorio.Models
{
    public class ApartamentoViewModel
    {
        public int ApartamentoId { get; set; }

        [Required]
        [StringLength(10)]
        public string Numero { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuarioResponsable { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [Required]
        [StringLength(10)]
        public string Torre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        [Range(1, 100)]
        public int Piso { get; set; }

        [Required]
        [Range(10, 1000)]
        public double AreaM2 { get; set; }

        [DataType(DataType.Currency)]
        public double CosteAdministracion => AreaM2 * 3000;
    }
}
