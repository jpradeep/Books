using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Books.Models
{
    public partial class Book
    {
        public long Isbn { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public int Authorid { get; set; }
    }
}
