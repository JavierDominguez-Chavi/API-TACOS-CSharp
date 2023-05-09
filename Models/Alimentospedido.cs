using System;
using System.Collections.Generic;

namespace TACOS.Models;

public partial class Alimentospedido
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int IdAlimento { get; set; }

    public int IdPedido { get; set; }

    public virtual Alimento IdAlimentoNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
