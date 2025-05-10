using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("times")]
public partial class Time : BaseEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("termID")]
    public string? TermID { get; set; }

    [Column("startYear")]
    public int? StartYear { get; set; }

    [Column("endYear")]
    public int? EndYear { get; set; }

    [Column("startDate", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("endDate", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("createdBy")]
    [StringLength(255)]
    public string? CreatedBy { get; set; }

    [InverseProperty("Time")]
    public virtual ICollection<ScoreStatus> ScoreStatus { get; set; } = new List<ScoreStatus>();
}
