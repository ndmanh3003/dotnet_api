using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.Models;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("created_at", TypeName = "datetime(6)")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime(6)")]
    public DateTime UpdatedAt { get; set; }

    [Column("deleted_at", TypeName = "datetime(6)")]
    public DateTime? DeletedAt { get; set; }
}

