using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Books.Models.Dto
{
    public partial class BookDto
    {
        public string? Title { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int? Authorid { get; set; }
    }
}
