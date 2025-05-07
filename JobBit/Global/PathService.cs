namespace JobBit.Global
{
    public class PathService
    {
        static IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public static string ProfileImagesFolder
        {
            get => configuration["UploadSettings:ProfileImages"];
        }
        public static string CVsFolder
        {
            get => configuration["UploadSettings:CVs"];
        }
        public static string CompaniesLogoFolder
        {
            get => configuration["UploadSettings:CompaniesLogo"];
        }

       
    }
}
