using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Models;

[Table("students")]
[Index(nameof(StudentId), IsUnique = true)]
public class Student : BaseEntity
{
    [Required]
    [MaxLength(50)]
    [Column("student_id", TypeName = "varchar(50)")]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
