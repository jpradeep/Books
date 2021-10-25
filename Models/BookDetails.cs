using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class BookDetails
    {
        public long Isbn { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Authorid { get; set; }
        public string Authorname { get; set; }
        public string Country { get; set; }
    }
}
