using System;

namespace Application.Models
{
    public class GoodDataDto
    {
        public int GoodId { get; set; }
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Amount { get; set; }
        public string Direction { get; set; }
        public string Comment { get; set; }
    }
}
