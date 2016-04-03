namespace Service.PersonelService.Models
{
    public class PersonQueryContext
    {
        public int? PersonId { get; set; }
        public int? DeptId { get; set; }
        public string StartWithName { get; set; }
    }
}