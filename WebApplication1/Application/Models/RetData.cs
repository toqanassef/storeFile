using System.Collections.Generic;

namespace Application.Models
{
    public class RetData
    {
        public int TransactionNum { get; set; }
        public int AmountTotal { get; set; }
        public int AmountRemain { get; set; }
        public List<GoodDataDto> GoodData { get; set; }
    }
}
