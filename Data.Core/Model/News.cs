using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Core.Model
{
    public class News
    {
        public int Id { get; set; }
        public string Header { get; set; }

        public string Content { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
