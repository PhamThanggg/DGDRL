namespace DGDiemRenLuyen.DTOs.Requests
{
    public class StatusUpdateListRequest
    {
        public List<Guid> Ids { get; set; } = new();
        public int Status { get; set; }
    }
}
