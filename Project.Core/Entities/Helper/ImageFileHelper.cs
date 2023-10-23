using Microsoft.AspNetCore.Http;
using Project.Core.Config;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities.Helper
{

    public class ImageFile
    {
        public string FileId { get; set; }
        public byte[] Content { get; set; }
        public bool ItIsFile { get; set; }

        public string ImageString
        {
            set { this.Content = !string.IsNullOrEmpty(value) ? Convert.FromBase64String(value.Replace("data:image/jpg;base64,", "")) : null; }
            get { return Content == null ? null : String.Concat("data:image/jpg;base64,", Convert.ToBase64String(this.Content)); }
        }
    }

    public static class DocumentHelper
    {
        public static string CopyTo(this string base64str)
        {
            string _folderPath = AppSettings.Current.FilePath;
            string FileId = string.Empty;
            if (!string.IsNullOrEmpty(base64str))
            {
                try
                {
                    FileId = Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();

                    // Delete existing document
                    if (!string.IsNullOrEmpty(FileId) && File.Exists(Path.Combine(_folderPath, FileId)))
                    {
                        File.Delete(Path.Combine(_folderPath, FileId));
                    }

                    File.WriteAllBytes(Path.Combine(_folderPath, FileId), Convert.FromBase64String(base64str.Replace("data:image/jpg;base64,", "")));
                }
                catch (Exception e)
                {
                    FileId = string.Empty;
                    Log.Error(e, "Image file not saved!");
                }
            }

            return FileId;
        }

        public static ImageFile CopyTo(this string base64str, string FileId = null)
        {
            string _folderPath = AppSettings.Current.FilePath;
            ImageFile file = new ImageFile();
            if (!string.IsNullOrEmpty(base64str))
            {
                try
                {
                    FileId = string.IsNullOrEmpty(FileId) ? Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() : FileId;

                    // Delete existing document
                    if (!string.IsNullOrEmpty(FileId) && File.Exists(Path.Combine(_folderPath, FileId)))
                    {
                        File.Delete(Path.Combine(_folderPath, FileId));
                    }

                    File.WriteAllBytes(Path.Combine(_folderPath, FileId), Convert.FromBase64String(base64str.Replace("data:image/jpg;base64,", "")));
                    file.FileId = FileId;
                    file.ImageString = base64str;
                }
                catch (Exception e)
                {
                    FileId = string.Empty;
                    Log.Error(e, "Image file not saved!");
                }
            }

            return file;
        }

        public static string CopyToSecond(this string base64str, string FileId = null)
        {
            string _folderPath = AppSettings.Current.FilePath;
            string file = "";
            if (!string.IsNullOrEmpty(base64str))
            {
                try
                {
                    FileId = string.IsNullOrEmpty(FileId) ? Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() : FileId;

                    // Delete existing document
                    if (!string.IsNullOrEmpty(FileId) && File.Exists(Path.Combine(_folderPath, FileId)))
                    {
                        File.Delete(Path.Combine(_folderPath, FileId));
                    }

                    File.WriteAllBytes(Path.Combine(_folderPath, FileId), Convert.FromBase64String(base64str.Replace("data:image/jpg;base64,", "")));
                    file = FileId;


                }
                catch (Exception e)
                {
                    FileId = string.Empty;
                    Log.Error(e, "Image file not saved!");
                }
            }
            return file;
        }

        public static bool IsItImage(string FileName)
        {
            var extension = Path.GetExtension(FileName).ToLower();
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            return validExtensions.Contains(extension);
        }

        public static async Task<String> CopyToAsync(this string base64str, string FileId = null)
        {
            string _folderPath = AppSettings.Current.FilePath;
            if (!IsValidPdf(base64str) && !string.IsNullOrEmpty(base64str))
            {
                try
                {
                    FileId = string.IsNullOrEmpty(FileId) ? Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() : FileId;
                    // Delete existing document
                    if (!string.IsNullOrEmpty(FileId) && File.Exists(Path.Combine(_folderPath, FileId)))
                    {
                        File.Delete(Path.Combine(_folderPath, FileId));
                    }

                    await File.WriteAllBytesAsync(Path.Combine(_folderPath, FileId), Convert.FromBase64String(base64str.Replace("data:image/jpg;base64,", "")));
                }
                catch (Exception e)
                {
                    FileId = string.Empty;
                    Log.Error(e, "Image file not saved!");
                }
            }

            await Task.CompletedTask;

            return FileId;
        }

        public static async Task<String> CopyToFileAsync(this IFormFile file, string FileId = null)
        {
            try
            {
                string _folderPath = AppSettings.Current.FilePath;
                FileId = string.IsNullOrEmpty(FileId) ? Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() : FileId;
                FileId += Path.GetExtension(file.FileName);
                // Delete existing document
                if (!string.IsNullOrEmpty(FileId) && File.Exists(Path.Combine(_folderPath, FileId)))
                {
                    File.Delete(Path.Combine(_folderPath, FileId));
                }
                using (var stream = new FileStream(Path.Combine(_folderPath, FileId), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception e)
            {
                FileId = string.Empty;
                Log.Error(e, "File not saved!");
            }

            await Task.CompletedTask;

            return FileId;
        }

        private static bool IsValidPdf(string base64Pdf)
        {
            byte[] pdfBytes = Convert.FromBase64String(base64Pdf);
            if (pdfBytes.Length < 8)
            {
                return false;
            }

            //Check the first 4 bytes
            if (pdfBytes[0] != 0x25 || pdfBytes[1] != 0x50 || pdfBytes[2] != 0x44 || pdfBytes[3] != 0x46)
            {
                return false;
            }
            //Check the last 4 bytes
            if (pdfBytes[pdfBytes.Length - 4] != 0x25 || pdfBytes[pdfBytes.Length - 3] != 0x25 || pdfBytes[pdfBytes.Length - 2] != 0x45 || pdfBytes[pdfBytes.Length - 1] != 0x4f)
            {
                return false;
            }
            return true;
        }

        public static async Task<String> CopyToAsync(this byte[] Content, string FileId = null)
        {
            string _folderPath = AppSettings.Current.FilePath;
            if (Content != null && Content.Length != 0)
            {

                try
                {
                    if (string.IsNullOrEmpty(FileId))
                    {
                        FileId = Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();
                    }
                    else
                    {
                        /*string[] files = Directory.GetFiles(_folderPath, FileId + "_*");
                        if (files.Any())
                        {
                            FileId = string.Concat(FileId, "_", files.Length + 1);
                        }*/

                        // Delete existing document
                        if (!string.IsNullOrEmpty(FileId) && File.Exists(Path.Combine(_folderPath, FileId)))
                        {
                            File.Delete(Path.Combine(_folderPath, FileId));
                        }
                    }

                    await File.WriteAllBytesAsync(Path.Combine(_folderPath, FileId), Content);
                }
                catch (Exception e)
                {
                    FileId = string.Empty;
                    Log.Error(e, "Image file not saved!");
                }
            }

            await Task.CompletedTask;

            return FileId;
        }

        public static ImageFile GetFile(this string id)
        {
            string folderPath = AppSettings.Current.FilePath;
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            }

            if (!string.IsNullOrEmpty(id) && File.Exists(Path.Combine(folderPath, id)))
            {
                var ext = Path.GetExtension(id).ToLower();
                if (IsItImage(id) || ext == "") return new ImageFile { Content = File.ReadAllBytes(Path.Combine(folderPath, id)) };
                else
                {
                    ImageFile imageFile = new ImageFile();
                    imageFile.ItIsFile = true;
                    imageFile.FileId = id;
                    return imageFile;
                }
            }

            return new ImageFile();
        }

        public static void Delete(this string fileName)
        {
            string folderPath = AppSettings.Current.FilePath;
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
        }
    }
}
