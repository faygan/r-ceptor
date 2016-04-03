using System;

namespace Service.PersonelService.Models
{
    public class PaymentQueryContext
    {
        public decimal? PayTotal { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}