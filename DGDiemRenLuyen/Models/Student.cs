using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("students")]
public partial class Student
{
    [Key]
    [Column("studentId")]
    [StringLength(255)]
    public string StudentId { get; set; } = null!;

    [Column("classId")]
    [StringLength(255)]
    public string? ClassId { get; set; }

    [Column("role")]
    public int? Role { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("Students")]
    public virtual Class? Class { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
