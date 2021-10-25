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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DapperContext _context;
        public AuthorRepository(DapperContext context)
        {
            _context = context;
        }

        /// <summary>
        /// DeleteAuthor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAuthor(int id)
        {
            var query = "DELETE FROM authors WHERE authorid = @id";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { id });
                return result > 0;
            }
        }

        /// <summary>
        /// UpdateAuthor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDto"></param>
        /// <returns></returns>
        public async Task<Author> UpdateAuthor(int id, AuthorDto authorDto)
        {
            var query = "UPDATE authors SET authorname = @Authorname, country = @Country WHERE authorid = @id";
            var parameters = new DynamicParameters();
            parameters.Add("Authorname", authorDto.Authorname, DbType.String);
            parameters.Add("Country", authorDto.Country, DbType.String);
            parameters.Add("id", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
                var result = await GetAuthors();
                return result.Where(x => x.Authorid == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// CreateBook
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<Author> CreateAuthor(Author author)
        {
            var query = "INSERT INTO authors (authorname, country) VALUES (@Authorname, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new DynamicParameters();
            parameters.Add("Authorname", author.Authorname, DbType.String);
            parameters.Add("Country", author.Country, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteAsync(query, parameters);
                var createdAuthor = new Author
                {
                    Authorname = author.Authorname,
                    Country = author.Country
                };
                return createdAuthor;
            }
        }

        /// <summary>
        /// GetBooks
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            var query = "SELECT * FROM authors";
            using (var connection = _context.CreateConnection())
            {
                var author = await connection.QueryAsync<Author>(query);
                return author.ToList();
            }
        }
    }
}
