using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPrueba.Models;

public partial class Usuario
{
    [Key]
    [Display(Name = "Usuario")]
    public int Usuario1 { get; set; }

    [Display(Name = "Nombre(s)*", Prompt = "Ingresa el nombre")]
    [Required(ErrorMessage = "Nombre obligatorio")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Nombre no se encuentra en el rango valido")]

    [RegularExpression(@"^[A-Z]+[a-z]+$", ErrorMessage = "Nombre incorrecto")]
    public string Nombre { get; set; } = null!;


    [Display(Name = "Apellido paterno*", Prompt = "Ingresa el apellido paterno")]
    [Required(ErrorMessage = "Apellido paterno obligatorio")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El apellido paterno no se encuentra en el rango valido")]
    [RegularExpression(@"^[A-Z]+[a-z]+$", ErrorMessage = "Apellido paterno incorrecto")]

    public string ApellidoP { get; set; } = null!;

    [Display(Name = "Apellido materno", Prompt = "Ingresa el apellido materno")]

    public string ApellidoM { get; set; }

    [Display(Name = "Cargo*")]
    [Required(ErrorMessage = "cargo obligatorio")]
    public string Cargo { get; set; } = null!;

    [Display(Name = "Sexo*")]
    [Required(ErrorMessage = "Sexo obligatorio")]

    public string Sexo { get; set; } = null!;

    [Display(Name = "Edad*")]
    [Required(ErrorMessage = "Edad obligatorio")]
    [RegularExpression(@"^([0-9]{2})$", ErrorMessage = "NC Incorrecto")]
    public int Edad { get; set; }

    [Key]
    [Display(Name = "Correo*", Prompt = "Ingresa un correo activo")]
    [Required(ErrorMessage = "Correo obligatorio")]


    public string Correo { get; set; } = null!;

    [Display(Name = "Contraseña*", Prompt = "Ingresa al menos 8 caracteres")]
    [Required(ErrorMessage = "Contraseña obligatorio")]
    [StringLength(15, MinimumLength = 8, ErrorMessage = "Contaseña fuera del limite")]

    public string Contraseña { get; set; } = null!;

    [Display(Name = "Estatus*")]
    [Required(ErrorMessage = "Estatus obligatorio")]
    public bool Estatus { get; set; }
    [NotMapped]
    public bool MantenerActivo { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
