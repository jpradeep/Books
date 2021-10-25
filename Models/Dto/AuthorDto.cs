using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Books.Models.Dto
{
    public partial class AuthorDto
    {
        public string Authorname { get; set; }
        public string Country { get; set; }
    }
}
