using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ChildCriterionGetListRequest
{
    public Guid? ParentCriteriaId { get; set; }
    [MaxLength(255)]
    public Guid Id { get; set; }
    public string? CriteriaName { get; set; }
    public int? CriteriaType {  get; set; }
    public int? IsActive { get; set; }

    [Required(ErrorMessage = "PageSize không được để trống.")]
    [Range(1, int.MaxValue, ErrorMessage = "PageSize phải lớn hơn 0.")]
    public int? PageSize { get; set; }

    [Required(ErrorMessage = "PageIndex không được để trống.")]
    [Range(1, int.MaxValue, ErrorMessage = "PageIndex phải từ 1 trở lên.")]
    public int? PageIndex { get; set; }
}
