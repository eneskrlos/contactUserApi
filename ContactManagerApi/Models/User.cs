using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactManagerApi.Models;

public partial class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Firstname { get; set; } = null!;
    [Required]
    public string Lastname { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Contact> Contacts { get; } = new List<Contact>();
    [JsonIgnore]
    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
