using dotnet.Enums.Course;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.Models;

[Index(nameof(Code), IsUnique = true)]
[Table("courses")]
public class Course : BaseEntity
{
    [Required]
    [MaxLength(50)]
    [Column("code", TypeName = "varchar(50)")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "smallint")]
    public int Credits { get; set; }

    [Required]
    [Column(TypeName = "smallint")]
    public CourseType Type { get; set; } = CourseType.Compulsory;

    [Required]
    [Column(TypeName = "smallint")]
    public int TheoryHours { get; set; }

    [Required]
    [Column(TypeName = "smallint")]
    public int PracticeHours { get; set; }

    [Required]
    [Column(TypeName = "smallint")]
    public int ExerciseHours { get; set; }
}
