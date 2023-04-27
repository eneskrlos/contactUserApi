using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContactManagerApi.Models;

public partial class Contact
{
    [Required]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "El campo Firstname es requerido"),
        MaxLength(128, ErrorMessage = "El campo Firstname no debe tener mas de 128 caracteres.")]
    public string Firstname { get; set; } = null!;

    [MaxLength(128)]
    public string? Lastname { get; set; }

    [Required(ErrorMessage = "El campo Email es requerido"), EmailAddress(ErrorMessage = "Direccion de Correo Invalido"),
        MaxLength(128, ErrorMessage = "El campo Firstname no debe tener mas de 128 caracteres.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El campo DateOfBirth es requerido")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "El campo Phone es requerido")]
    public string Phone { get; set; } = null!;

    public Guid? Owner { get; set; }

    [JsonIgnore]
    public virtual User? OwnerNavigation { get; set; }
}
