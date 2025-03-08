using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class TimeGetListRequest
{
    public int? Semester { get; set; }

    public int? StartYear { get; set; }

    [Required]
    public int? PageSize { get; set; }

    [Required]
    public int? PageIndex { get; set; }
}
