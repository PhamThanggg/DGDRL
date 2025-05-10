using Azure.Core;

namespace DGDiemRenLuyen.Services.UploadFileSrevice
{
    public class FileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadFolder;
        private readonly int _maxFileSize = 5;

        public FileService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _uploadFolder = configuration["FileUpload:UploadFolder"] ?? "uploads";
        }

        public async Task<string> UploadPdfAsync(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Vui lòng chọn file để upload.");

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception("Chỉ được upload file PDF hoặc hình ảnh (jpg, jpeg, png).");

            long maxFileSize = _maxFileSize * 1024 * 1024;
            if (file.Length > maxFileSize)
                throw new Exception($"Kích thước file không được vượt quá {maxFileSize}MB.");

            var webRootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            if (string.IsNullOrEmpty(webRootPath))
                throw new Exception("Không thể xác định thư mục lưu file.");

            var uploadsFolder = Path.Combine(webRootPath, "uploads", userId);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{userId}/{fileName}";
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            string rootPath = _environment.WebRootPath ?? _environment.ContentRootPath;

            string absolutePath = Path.Combine(rootPath, filePath.TrimStart('/'));

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
                return true;
            }

            return false;
        }
    }

}
