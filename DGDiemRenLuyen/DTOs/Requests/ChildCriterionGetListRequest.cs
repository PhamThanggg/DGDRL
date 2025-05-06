using DGDiemRenLuyen.DTOs.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ChildCriterionGetListRequest : BaseListRequest
{
    public Guid? ParentCriteriaId { get; set; }
    public Guid Id { get; set; }
    public string? CriteriaName { get; set; }
    public int? CriteriaType {  get; set; }
    public int? IsActive { get; set; }
}
