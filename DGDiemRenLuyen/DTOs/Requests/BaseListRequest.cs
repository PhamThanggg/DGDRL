using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests
{
    public class BaseListRequest
    {
        [Required(ErrorMessage = "PageSize không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "PageSize phải lớn hơn 0.")]
        public int? PageSize { get; set; }

        [Required(ErrorMessage = "PageIndex không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex phải từ 1 trở lên.")]
        public int? PageIndex { get; set; }
    }
}
