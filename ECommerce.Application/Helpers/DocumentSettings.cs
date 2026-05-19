using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(folderPath, fileName);

            using var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);

            return $"/images/{folderName}/{fileName}";
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string pureFileName = Path.GetFileName(fileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName, pureFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
