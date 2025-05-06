using DGDiemRenLuyen.DTOs.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class TimeGetListRequest : BaseListRequest
{
    public int? Semester { get; set; }

    public int? StartYear { get; set; }
}
