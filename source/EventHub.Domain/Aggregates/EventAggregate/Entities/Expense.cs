using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate.Entities;

[Table("Expenses")]
public class Expense : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] public Guid EventId { get; set; } = Guid.Empty;

    [Required]
    [Column(TypeName = "nvarchar(1000)")]
    public string Title { get; set; } = string.Empty;

    [Required] public long Total { get; set; } = 0;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
    
    public virtual ICollection<SubExpense> SubExpenses { get; set; } = new List<SubExpense>();
}
