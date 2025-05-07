using JobBit_Business;

namespace JobBit.DTOs
{
    public class UpdateJobSeekerDTO
    {
        public int JobSeekerID { get; set; }
        public int? WilayaID { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? LinkProfileLinkden { get; set; }
        public string? LinkProfileGithub { get; set; }
    }
}
