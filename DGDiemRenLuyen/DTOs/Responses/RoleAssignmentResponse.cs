using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.responses;

public class RoleAssignmentResponse
{
    public string ObjectId { get; set; } = null!;

    public string? ObjectType { get; set; }

    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
