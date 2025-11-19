using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.Models;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "datetime(6)")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime(6)")]
    public DateTime UpdatedAt { get; set; }

    [Column(TypeName = "datetime(6)")]
    public DateTime? DeletedAt { get; set; }
}

