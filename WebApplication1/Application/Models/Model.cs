using Microsoft.AspNetCore.Http;
using System;

namespace Application.Models
{
    public class Model
    {
        public IFormFile File { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int GoodId { get; set; }
    }
}
