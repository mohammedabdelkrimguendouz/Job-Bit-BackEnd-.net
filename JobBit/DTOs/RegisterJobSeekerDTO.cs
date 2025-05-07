using JobBit_Business;

namespace JobBit.DTOs
{
    public class RegisterJobSeekerDTO
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IFormFile? CV { get; set; }

        public List<int>? Skils { get; set; }

    }
}
