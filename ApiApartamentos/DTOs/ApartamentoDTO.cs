namespace ApiApartamentos.DTOs
{
    public class ApartamentoDTO
    {
        public int ApartamentoId { get; set; }
        public string Numero { get; set; }
        public string UsuarioResponsable { get; set; }
        public string Estado { get; set; }
        public string Torre { get; set; }
        public int Piso { get; set; }
        public decimal AreaM2 { get; set; }
    }
}
