using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet.Enums;

namespace dotnet.Models;

[Table("todos")]
public class Todo : BaseEntity
{
    [Required]
    [MaxLength(500)]
    [Column("title", TypeName = "varchar(500)")]
    public string Title { get; set; } = string.Empty;

    [Column("due_date", TypeName = "datetime(6)")]
    public DateTime? DueDate { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("status", TypeName = "varchar(50)")]
    public TodoStatus Status { get; set; } = TodoStatus.InProgress;

    [Required]
    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

