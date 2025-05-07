using JobBit_Business;

namespace JobBit.DTOs
{
    public class ResultJobSeekerAuth
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public JobSeeker.AllJobSeekerInfo allJobSeekerInfo {get;set;}

        public ResultJobSeekerAuth(string token, JobSeeker.AllJobSeekerInfo allJobSeekerInfo)
        {
            Token = token;
            this.allJobSeekerInfo = allJobSeekerInfo;
            Role = "JobSeeker";
        }
    }
}
