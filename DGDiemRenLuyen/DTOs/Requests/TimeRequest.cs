namespace DGDiemRenLuyen.DTOs.requsets;

public partial class TimeRequest
{
    public Guid Id { get; set; }

    public int? Semester { get; set; }

    public int? StartYear { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
