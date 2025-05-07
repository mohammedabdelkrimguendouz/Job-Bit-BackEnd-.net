
using EASendMail;
using JobBit.DTOs;
using JobBit_Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobBit.Global
{
    public class Util
    {

        public static string GenerateOTP()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                int otp = Math.Abs(BitConverter.ToInt32(randomNumber, 0)) % 1000000;
                return otp.ToString("D6");
            }
        }

        static public List<EnumDto> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => new EnumDto { Id = Convert.ToInt32(e), Name = e.ToString() })
                       .ToList();
        }

        static private string GetImageFolderPath()
        {
            // Set up configuration builder
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Reading a connection string
            string ImageFolderPath = configuration.GetSection("ImageFolderPath").Value;
            return ImageFolderPath;
        }
        static public string GenerateGuid()
        {
            Guid NewGuid = Guid.NewGuid();
            return NewGuid.ToString();
        }

        static public bool CreateFolderIsNotExist(string FolderPath)
        {
            if (!Directory.Exists(FolderPath)) 
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch(Exception Ex)
                {
                    clsEventLog.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                    return false;
                }
                
            }
            return true;
        }

        
        static public string ReplaceFileNameWithGuid(string FileName)
        {
            FileInfo fi = new FileInfo(FileName) ;
            string Ext=fi.Extension;
            return GenerateGuid() + Ext;
        }


        static public async Task<string?> CopyImageToProjectImagesFolder(IFormFile ImageFile)
        {
            string DestinationFolder = GetImageFolderPath();
            if (!CreateFolderIsNotExist(DestinationFolder))
                return null;

            
            string DestinationFile = DestinationFolder + ReplaceFileNameWithGuid(ImageFile.FileName);

       

            try
            {
                using (var stream = new FileStream(DestinationFile, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                
            }
            catch(Exception Ex)
            {
                clsEventLog.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                return null;
            }
           
            return DestinationFile;
        }

        static public bool DeleteFile(string SourceFile)
        {
            if (SourceFile == "")
                return false;
            try
            {
                File.Delete(SourceFile);
            }
            catch (Exception Ex)
            {
                clsEventLog.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                return false;
            }
            return true;
        }

        public static string GetMimeType(string FileName)
        {
            string UploadDirectory = @"C:\MyImages";
            string FilePath = Path.Combine(UploadDirectory, FileName);

            var Extension = Path.GetExtension(FilePath).ToLowerInvariant();

            return Extension switch
            {
                ".jpg" => "image/jpg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }


    }
}
