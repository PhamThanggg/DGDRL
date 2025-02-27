using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.DTOs.responses;

public class ParentCriterionResponse
{
    public string Id { get; set; }

    public string? CriteriaName { get; set; }

    public int? MaxScore { get; set; }

    public int? OrderIndex { get; set; }

    public int? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
