using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("class")]
public partial class Class
{
    [Key]
    [Column("classId")]
    [StringLength(255)]
    public string ClassId { get; set; } = null!;

    [Column("teacherId")]
    [StringLength(255)]
    public string? TeacherId { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Class")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [ForeignKey("TeacherId")]
    [InverseProperty("Classes")]
    public virtual Teacher? Teacher { get; set; }
}
