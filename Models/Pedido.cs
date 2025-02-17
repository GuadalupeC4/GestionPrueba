using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionPrueba.Models;

public partial class Pedido
{
    [Key]
    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    public int Usuario { get; set; }

    [Display(Name = "Cantidad*", Prompt = "Ingresa la cantidad")]
    [Required(ErrorMessage = "Cantidad obligatoria")]
    public int Cantidad { get; set; }


    [Display(Name = "Número de mesa*", Prompt = "Ingresa el número de mesa")]
    [Required(ErrorMessage = "número de mesa obligatorio")]
    public int NumeroMesa { get; set; }


    [Display(Name = "Nota*", Prompt = "Ingresa alguna nota sobre el pedido")]

    public string Nota { get; set; } = null!;

    [Display(Name = "Estatus*")]
    [Required(ErrorMessage = "Estatus obligatorio")]
    public string Estatus { get; set; } = null!;
    [Display(Name = "IdProducto*")]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
    [Display(Name = "Mesero*")]
    public virtual Usuario UsuarioNavigation { get; set; } = null!;

}
