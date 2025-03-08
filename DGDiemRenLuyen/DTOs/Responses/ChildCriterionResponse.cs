using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.DTOs.responses;

public class ChildCriterionResponse
{
    public Guid Id { get; set; }

    public Guid ParentCriteriaId { get; set; }

    public string? CriteriaName { get; set; }

    public int? MaxScore { get; set; }

    public int? OrderIndex { get; set; }

    public int? IsActive { get; set; }

    public int? CriteriaType { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
