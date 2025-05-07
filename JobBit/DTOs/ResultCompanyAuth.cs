using JobBit_Business;

namespace JobBit.DTOs
{
    public class ResultCompanyAuth
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public Company.AllCompanyInfo allCompanyInfo { get; set; }

        public ResultCompanyAuth(string token, Company.AllCompanyInfo allCompanyInfo)
        {
            Token = token;
            this.allCompanyInfo = allCompanyInfo;
            Role = "Company";
        }
    }
}
