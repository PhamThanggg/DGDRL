using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("times")]
public partial class Time
{
    [Key]
    [Column("id")]
    [StringLength(255)]
    public string Id { get; set; } = null!;

    [Column("semester")]
    public int? Semester { get; set; }

    [Column("startYear", TypeName = "datetime")]
    public DateTime? StartYear { get; set; }

    [Column("endYear", TypeName = "datetime")]
    public DateTime? EndYear { get; set; }

    [Column("startDate", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("endDate", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("studentDeadline", TypeName = "datetime")]
    public DateTime? StudentDeadline { get; set; }

    [Column("teacherDeadline", TypeName = "datetime")]
    public DateTime? TeacherDeadline { get; set; }

    [Column("createdBy")]
    [StringLength(255)]
    public string? CreatedBy { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Time")]
    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
