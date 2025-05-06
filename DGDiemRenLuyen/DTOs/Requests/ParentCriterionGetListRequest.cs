using DGDiemRenLuyen.DTOs.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ParentCriterionGetListRequest : BaseListRequest
{
    [MaxLength(255)]
    public string? CriteriaName { get; set; }
    public int? IsActive { get; set; }
}
