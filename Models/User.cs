using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using dotnet.Enums;

namespace dotnet.Models;

[Table("users")]
[Index(nameof(GoogleId), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User : BaseEntity
{
    [MaxLength(255)]
    [Column("google_id", TypeName = "varchar(255)")]
    public string GoogleId { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("email", TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("full_name", TypeName = "varchar(255)")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("role", TypeName = "smallint")]
    public Role Role { get; set; }

    [MaxLength(500)]
    [Column("picture", TypeName = "varchar(500)")]
    public string Picture { get; set; } = string.Empty;

    public Student? Student { get; set; }
}
