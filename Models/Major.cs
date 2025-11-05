using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.Models;

[Table("majors")]
[Index(nameof(Code), IsUnique = true)]
public class Major : BaseEntity
{
    [Required]
    [MaxLength(100)]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("code", TypeName = "varchar(20)")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}

