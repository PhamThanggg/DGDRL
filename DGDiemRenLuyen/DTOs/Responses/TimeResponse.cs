namespace DGDiemRenLuyen.DTOs.responses;

public partial class TimeResponse
{
    public Guid Id { get; set; }

    public string? TermID { get; set; }

    public int? StartYear { get; set; }

    public int? EndYear { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
