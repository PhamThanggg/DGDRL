using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ParentCriterionGetListRequest
{
    public string? CriteriaName { get; set; }
    public int? IsActive { get; set; }

    [Required]
    public int? PageSize { get; set; }

    [Required]
    public int? PageIndex { get; set; }
}
