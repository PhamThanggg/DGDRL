using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests
{
    public class RoleAssignmentGetListRequest : BaseListRequest
    {
        public string ObjectId { get; set; } = null!;
        public string? ObjectType { get; set; }
        public string? Role { get; set; }
    }
}
