using DGDiemRenLuyen.DTOs.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class TimeGetListRequest : BaseListRequest
{
    [RegularExpression("^(HK01|HK02|HK03)$", ErrorMessage = "Chỉ chấp nhận: HK01, HK02, HK03")]
    public string? TermID { get; set; }

    public int? StartYear { get; set; }
}
