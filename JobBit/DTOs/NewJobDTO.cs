using JobBit_Business;

namespace JobBit.DTOs
{
    public class NewJobDTO
    {
        public int CompanyID { get; set; }
        public string Title { get; set; }
        public Job.enJopType JobType { get; set; }
        public Job.enJobExperience Experience { get; set; }
        public string? Description { get; set; }
        public List<int>? Skills { get; set; }
    }
}
