using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate.ValueObjects;

[Table("SubExpenses")]
public class SubExpense : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] public Guid ExpenseId { get; set; } = Guid.Empty;

    [Required]
    [Column(TypeName = "nvarchar(1000)")]
    public string Name { get; set; } = string.Empty;

    [Required] public long Price { get; set; } = 0;

    [ForeignKey("ExpenseId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Expense Expense { get; set; } = null!;
}
