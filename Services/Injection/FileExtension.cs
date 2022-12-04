using Microsoft.AspNetCore.Http;
using Services.Injection.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection
{
    public static class FileExtension
    {
        public static async Task<string> CreateFile(this IFormFile file, string userDirectory)
        {
            try
            {
                if (file.Length > 0)
                {
                    var img = Guid.NewGuid();
                    var directoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\" + userDirectory;
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    using var stream = new FileStream($"{directoryPath}/{img}{Path.GetExtension(file.FileName)}", FileMode.Create);

                    await file.CopyToAsync(stream);

                    return $"{img}{Path.GetExtension(file.FileName)}";
                }
                return string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static void DeleteFolder(string filePath)
        {
            try
            {
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath);
                }
                else
                {
                    throw new ArgumentException("Directory not exists", nameof(filePath));
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, nameof(filePath));
            }

        }
        public static Result<bool> DeleteFile(string filePath)
        {
            try
            {
                filePath = filePath.Replace('/', '\\');
                filePath = $"{Directory.GetCurrentDirectory()}\\wwwroot{filePath}";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return new Result<bool>
                    {
                        Succeeded = true,
                        MessageType= ValidationMessageTypes.Success,
                    };
                }
                else
                {

                    //throw new ArgumentException("File doesn't exists", nameof(filePath));
                    return new Result<bool>
                    {
                        Succeeded = false,
                        MessageType = ValidationMessageTypes.Exception,
                        Errors = new List<string>() { String.Format("File doesn't exists", nameof(filePath)) }
                    };
                }
            }
            catch (Exception e)
            {
                return new Result<bool>
                {
                    Succeeded = false,
                    MessageType = ValidationMessageTypes.Exception,
                    Errors = new List<string>() { String.Format("File doesn't exists", nameof(filePath)) }
                };
            }

        }
    }
}
