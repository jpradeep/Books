using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Books.Models
{
    public partial class Author
    {
        public int Authorid { get; set; }
        public string Authorname { get; set; }
        public string Country { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
