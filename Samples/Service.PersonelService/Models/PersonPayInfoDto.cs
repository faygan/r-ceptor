using System;

namespace Service.PersonelService.Models
{
    public class PersonPayInfoDto
    {
        public PersonDto Person { get; set; }
        public decimal PayTotal { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}