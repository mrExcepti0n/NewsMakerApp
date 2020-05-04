using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsMaker.Web.Models
{
    public class NewsDto
    {
        public int Id { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }
    }
}
