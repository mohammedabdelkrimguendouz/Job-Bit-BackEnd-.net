namespace JobBit.DTOs
{
    public class UpdateCompanyDTO
    {
        public int CompanyID { get; set; }
        public int? WilayaID { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Logo { get; set; }

        public string? Link { get; set; }

    }

}
