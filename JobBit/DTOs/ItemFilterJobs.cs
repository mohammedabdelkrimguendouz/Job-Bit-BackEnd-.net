namespace JobBit.DTOs
{
    public class ItemFilterJobs
    {
       public string title {  get; set; }
       public List<EnumDto> filters { get; set; }

        public ItemFilterJobs(string title, List<EnumDto> filters)
        {
            this.title = title;
            this.filters = filters;
        }
        public ItemFilterJobs()
        {
            
        }
    }
}
