namespace EventHub.Application.SeedWork.DTOs.Role;

public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
