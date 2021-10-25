using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Books.Models
{
    public partial class AuthorBook
    {
        public int Id { get; set; }
        public long FkIsbn { get; set; }
        public int FkAuthorid { get; set; }
    }
}
