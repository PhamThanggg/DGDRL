using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("teachers")]
public partial class Teacher
{
    [Key]
    [Column("teacherId")]
    [StringLength(255)]
    public string TeacherId { get; set; } = null!;

    [Column("role")]
    public int? Role { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
