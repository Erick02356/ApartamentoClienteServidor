using System.ComponentModel.DataAnnotations;

namespace ClienteWeb.Models.ViewModels
{
    public class ApartamentoViewModel
    {
        [Required]
        public int ApartamentoId { get; set; }

        [Required(ErrorMessage = "El número del apartamento es obligatorio.")]
        [StringLength(10, ErrorMessage = "El número no puede tener más de 10 caracteres.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Debe asignar un usuario responsable.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string UsuarioResponsable { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "La torre es obligatoria.")]
        [StringLength(10, ErrorMessage = "La torre no puede tener más de 10 caracteres.")]
        public string Torre { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El piso es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El piso debe estar entre 1 y 100.")]
        public int Piso { get; set; }

        [Required(ErrorMessage = "El área es obligatoria.")]
        [Range(10, 1000, ErrorMessage = "El área debe estar entre 10 y 1000 m².")]
        public double AreaM2 { get; set; }

        [Display(Name = "Coste de Administración")]
        [DataType(DataType.Currency)]
        public double CosteAdministracion => AreaM2 * 3000;
    }
}
