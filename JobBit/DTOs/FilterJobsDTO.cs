namespace JobBit.DTOs
{
    public class FilterJobsDTO
    {
       public List<int>? WilayaIDs { get; set; }
       public List<int>? SkillIDs { get; set; }
       public List<int>? JobTypeIDs { get; set; }
       public List<int>? JobExperienceIDs { get; set; }
    }
}
