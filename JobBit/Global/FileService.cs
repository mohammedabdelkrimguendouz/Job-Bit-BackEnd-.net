using JobBit_Business;

namespace JobBit.Global
{
    public class FileService
    {
        public enum enFileType { Image, CV };
        private static readonly long _maxImageSize = 2 * 1024 * 1024; 
        private static readonly long _maxCVSize = 5 * 1024 * 1024; 


        private static readonly List<string> _allowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private static readonly List<string> _allowedCVExtensions = new List<string> { ".pdf" };

        public FileService()
        {
           
        }

        static public bool ValidateFile(IFormFile file, enFileType fileType, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file == null || file.Length == 0)
            {
                errorMessage = "File is required.";
                return false;
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            long fileSize = file.Length;

            if (fileType == enFileType.Image)
            {
                if (!_allowedImageExtensions.Contains(extension))
                {
                    errorMessage = "Invalid image format. Allowed formats: .jpg, .jpeg, .png.";
                    return false;
                }
                if (fileSize > _maxImageSize)
                {
                    errorMessage = "Image size exceeds 2MB.";
                    return false;
                }
            }
            else if (fileType == enFileType.CV)
            {
                if (!_allowedCVExtensions.Contains(extension))
                {
                    errorMessage = "Invalid CV format. Allowed formats: .pdf, .doc, .docx.";
                    return false;
                }
                if (fileSize > _maxCVSize)
                {
                    errorMessage = "CV size exceeds 5MB.";
                    return false;
                }
            }

            return true;
        }

        static public string SaveFile(IFormFile file,string targetPath)
        {
            if (file == null || file.Length == 0) return null;

           

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(targetPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filePath;
        }
    }
}

