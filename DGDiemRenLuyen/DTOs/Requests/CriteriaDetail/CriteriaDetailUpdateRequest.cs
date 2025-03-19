using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;

public class CriteriaDetailUploadRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "File PDF không được để trống.")]
    public IFormFile File { get; set; }
}
