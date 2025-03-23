using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Apartamento
{
    public int ApartamentoId { get; set; }

    public string Numero { get; set; } = null!;

    public string UsuarioResponsable { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Torre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Piso { get; set; }

    public decimal AreaM2 { get; set; }
}
