using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace web_blog.Services;

public class ImageService
{
	public const String avatarUploadPath = "/app/uploads";
	
    public async Task<string> SaveImageAsync(string base64String, string username)
    {
        if (string.IsNullOrEmpty(base64String))
        {
            throw new ArgumentException("Base64 string is null or empty.");
        }

        var parts = base64String.Split(",");
        var format = parts[0].Split(':')[1].Split(';')[0].Split('/')[1];
        var data = parts[1];
        
        var bytes = Convert.FromBase64String(data);
        
        var fileName = username + "." + format;

        var filePath = Path.Combine(avatarUploadPath, fileName);
        await File.WriteAllBytesAsync(filePath, bytes);

        return fileName;
    }

	public async Task<FileContentResult> GetImageAsync(string filename) {
		// Kiểm tra file có tồn tại trên ổ đĩa hay không
		var filePath = Path.Combine(avatarUploadPath, filename);
		if (!System.IO.File.Exists(filePath)) {
			return null;
		}

		// Đọc dữ liệu từ file và trả về dưới dạng file response
		var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
		return new FileContentResult(fileBytes, "image/jpeg"); // Thay đổi MIME type tương ứng với định dạng của ảnh
	}
}