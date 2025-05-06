using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests
{
    public class RoleAssignmentRequest
    {

        [Required]
        [MaxLength(20)]
        public string ObjectId { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [RegularExpression("^(GV|SV)$")]
        public string? ObjectType { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("^(CBL|TK|HDT|ADMIN)$", ErrorMessage = "Vai trò không hợp lệ. Chỉ chấp nhận: CBL, TK, HDT, ADMIN.")]
        public string? Role { get; set; }
    }
}
