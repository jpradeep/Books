using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Books.DataContext;
using Books.Models;
using Books.Models.Dto;
using Books.Repository.Interface;
using Dapper;

namespace Books.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly DapperContext _context;
        public BookRepository(DapperContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetBookDetails
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BookDetails>> GetBookDetails()
        {
            using (var connection = _context.CreateConnection())
            {
                return connection.Query<BookDetails>("select * from BookDetails").ToList();
            }
        }

        /// <summary>
        /// UpdateBook
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<Book> UpdateBook(long isbn, BookDto book)
        {
            var query = "UPDATE Books SET Title = @Title, PublicationDate = @PublicationDate WHERE ISBN = @ISBN";
            var parameters = new DynamicParameters();
            parameters.Add("ISBN", isbn, DbType.Int64);
            parameters.Add("Title", book.Title, DbType.String);
            parameters.Add("PublicationDate", book.PublicationDate, DbType.Date);

            if (book.Authorid > 0)
            {
                var author = await GetAuthor((int)book.Authorid);
                if (author != null)
                {
                    var authorBook = await GetAuthorByIsbn(isbn);
                    var bookReq = new Book
                    {
                        Isbn = isbn,
                        Authorid = author.Authorid,
                        PublicationDate = (DateTime)book.PublicationDate,
                        Title = book.Title
                    };
                    if (authorBook != null)
                    {
                        var authorBookUpdated = await UpdateAuthorBook(authorBook.Id, bookReq);
                    }
                    else
                    {
                        await CreateAuthorBook(bookReq);
                    }
                }
            }
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                var result = await GetBooks();
                return result.Where(x => x.Isbn == isbn).FirstOrDefault();
            }
        }


        /// <summary>
        /// CreateBook
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<Book> CreateBook(Book book)
        {
            var query = "INSERT INTO Books (Isbn, Title, PublicationDate) VALUES (@Isbn, @Title, @PublicationDate)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new DynamicParameters();
            parameters.Add("Isbn", book.Isbn, DbType.Int64);
            parameters.Add("Title", book.Title, DbType.String);
            parameters.Add("PublicationDate", book.PublicationDate, DbType.Date);

            var author = await GetAuthor(book.Authorid);
            var queryJoin = string.Empty;
            var parametersJoin = new DynamicParameters();

            if (author != null)
            {
                var authorBook = await CreateAuthorBook(book);
            }
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteAsync(query, parameters);
                var createdbook = new Book
                {
                    Isbn = book.Isbn,
                    Title = book.Title,
                    PublicationDate = book.PublicationDate,
                    Authorid = book.Authorid
                };
                return createdbook;
            }
        }

        /// <summary>
        /// GetBooks
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> GetBooks()
        {
            var query = "SELECT * FROM books ";
            using (var connection = _context.CreateConnection())
            {
                var books = await connection.QueryAsync<Book>(query);
                return books.ToList();
            }
        }

        /// <summary>
        /// DeleteBook
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBook(long isbn)
        {
            var query = "DELETE FROM Books WHERE ISBN = @isbn";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { isbn });
                var res = await DeleteAuthorBookByIsbn(isbn);
                return result > 0;
            }
        }

        /// <summary>
        /// GetAuthor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Author> GetAuthor(int id)
        {
            var query = "SELECT * FROM authors WHERE authorid = @Id";
            using (var connection = _context.CreateConnection())
            {
                var author = await connection.QuerySingleOrDefaultAsync<Author>(query, new { id });
                return author;
            }
        }

        /// <summary>
        /// DeleteAuthorBookByIsbn
        /// </summary>
        /// <param name="Isbn"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAuthorBookByIsbn(long Isbn)
        {
            var authorBook = await GetAuthorByIsbn(Isbn);
            if (authorBook != null)
            {
                var query = "DELETE FROM AuthorBook WHERE Fk_ISBN = @isbn";
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, new { Isbn });
                    return result > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// GetAuthorByIsbn
        /// </summary>
        /// <param name="Isbn"></param>
        /// <returns></returns>
        public async Task<AuthorBook> GetAuthorByIsbn(long Isbn)
        {
            var query = "SELECT * FROM AuthorBook WHERE Fk_ISBN = @Isbn";
            using (var connection = _context.CreateConnection())
            {
                var author = await connection.QuerySingleOrDefaultAsync<AuthorBook>(query, new { Isbn });
                return author;
            }
        }

        /// <summary>
        /// UpdateAuthorBook
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<AuthorBook> UpdateAuthorBook(int id, Book book)
        {
            var query = "UPDATE AuthorBook SET Fk_ISBN = @Fk_ISBN, Fk_authorid = @Fk_authorid WHERE ID = @id";
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);
            parameters.Add("Fk_ISBN", book.Isbn, DbType.Int64);
            parameters.Add("Fk_authorid", book.Authorid, DbType.Int32);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                return new AuthorBook
                {
                    FkIsbn = book.Isbn,
                    FkAuthorid = book.Authorid
                };
            }
        }

        /// <summary>
        /// CreateAuthorBook
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<AuthorBook> CreateAuthorBook(Book book)
        {
            var author = await GetAuthor(book.Authorid);
            var parameters = new DynamicParameters();

            if (author != null)
            {
                var query = "INSERT INTO AuthorBook (Fk_ISBN, Fk_authorid) VALUES (@FkIsbn, @FkAuthorid)" +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                parameters.Add("FkIsbn", book.Isbn, DbType.Int64);
                parameters.Add("FkAuthorid", book.Authorid, DbType.Int32);

                using (var connection = _context.CreateConnection())
                {
                    if (!string.IsNullOrEmpty(query))
                    {
                        var authorId = await connection.ExecuteAsync(query, parameters);
                    }
                    var createdAuthorBook = new AuthorBook
                    {
                        FkIsbn = book.Isbn,
                        FkAuthorid = book.Authorid
                    };
                    return createdAuthorBook;
                }
            }
            else
            {
                return new AuthorBook
                {
                    FkIsbn = book.Isbn,
                    FkAuthorid = book.Authorid
                };
            }
        }

    }
}
