using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionPrueba.Models;

public partial class Producto
{
    [Key]
    [Display(Name = "Código")]
    public int IdProducto { get; set; }

    [Display(Name = "Nombre*", Prompt = "Ingresa el nombre")]
    [Required(ErrorMessage = "Nombre obligatorio")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Nombre no se encuentra en el rango valido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s.;,:''""]*$", ErrorMessage = "Nombre incorrecto")]

    public string Nombre { get; set; } = null!;

    [Display(Name = "Precio*", Prompt = "Ingresa el precio")]
    [Required(ErrorMessage = "Precio obligatorio")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "money")]

    public decimal Precio { get; set; }

    [Display(Name = "Descripcion*", Prompt = "Ingresa el descripcion")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Las descripcion no se encuentra en el rango valido")]
   
    public string Ingredientes { get; set; } = null!;

    [Display(Name = "Categoria*")]
    [Required(ErrorMessage = "Categoria obligatorio")]
    public string Categoria { get; set; } = null!;

    [Display(Name = "Aditivos*")]

    public string Aditivos { get; set; }

    [Display(Name = "Estatus")]
    [Required(ErrorMessage = "Estatus obligatorio")]
    public bool Estatus { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
