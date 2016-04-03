namespace Service.PersonelService.Models
{
    public class PersonDto
    {
        public PersonDto()
        {
            IsActive = true;
        }

        public int PersonId { get; set; }
        public string Name { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public bool IsActive { get; set; }
    }
}