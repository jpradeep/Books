using Books.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Books.Models.Dto;

namespace Books.Repository.Interface
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book> CreateBook(Book book);
        public Task<Author> GetAuthor(int id);
        public Task<Book> UpdateBook(long isbn, BookDto bookDto);
        public Task<bool> DeleteBook(long isbn);
        public Task<IEnumerable<BookDetails>> GetBookDetails();
        public Task<AuthorBook> GetAuthorByIsbn(long Isbn);
        public Task<AuthorBook> CreateAuthorBook(Book book);
        public Task<AuthorBook> UpdateAuthorBook(int id, Book book);
        public Task<bool> DeleteAuthorBookByIsbn(long Isbn);
    }
}
