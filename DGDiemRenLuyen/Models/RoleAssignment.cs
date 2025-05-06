using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.Models;

[Table("roleAssignments")]
public partial class RoleAssignment : BaseEntity
{
    [Key]
    [Column("objectId")]
    [StringLength(20)]
    public string ObjectId { get; set; } = null!;

    [Column("objectType")]
    [StringLength(20)]
    public string? ObjectType { get; set; }

    [Column("role")]
    [StringLength(20)]
    public string? Role { get; set; }
}
