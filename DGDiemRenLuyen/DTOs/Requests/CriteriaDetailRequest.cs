using DGDiemRenLuyen.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.DTOs.requsets;

public class CriteriaDetailRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ChildCriteriaId { get; set; }

    [Required]
    public Guid ScoreId { get; set; }

    public string? Proof { get; set; }

    public string? Note { get; set; }

    public int? StudentScore { get; set; }

    public int? MoniterScore { get; set; }

    public int? TeacherScore { get; set; }
}
