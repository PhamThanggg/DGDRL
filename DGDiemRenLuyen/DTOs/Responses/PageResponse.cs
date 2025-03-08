namespace DGDiemRenLuyen.DTOs.Responses
{
    public class PageResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public T Data { get; set; } = default;
    }
}
