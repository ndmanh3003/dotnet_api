using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using dotnet.Enums;

namespace dotnet.Models;

[Table("students")]
[Index(nameof(Code), IsUnique = true)]
public class Student : BaseEntity
{
    [MaxLength(50)]
    [Column("student_id", TypeName = "varchar(50)")]
    public string? Code { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    [Column("year")]
    public int? Year { get; set; }

    [MaxLength(20)]
    [Column("major_code", TypeName = "varchar(20)")]
    public string? MajorCode { get; set; }

    public Major? Major { get; set; }

    [Column("gender", TypeName = "smallint")]
    public Gender? Gender { get; set; }
}