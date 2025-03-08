using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ChildCriterionGetListRequest
{
    public Guid? ParentCriteriaId { get; set; }
    public string? CriteriaName { get; set; }
    public int? CriteriaType {  get; set; }
    public int? IsActive { get; set; }

    [Required]
    public int? PageSize { get; set; }

    [Required]
    public int? PageIndex { get; set; }
}
