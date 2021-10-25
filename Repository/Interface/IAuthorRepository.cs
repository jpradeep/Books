using Books.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Books.Models.Dto;

namespace Books.Repository.Interface
{
    public interface IAuthorRepository
    {
        public Task<IEnumerable<Author>> GetAuthors();
        public Task<Author> CreateAuthor(Author author);
        public Task<Author> UpdateAuthor(int id, AuthorDto authorDto);
        public Task<bool> DeleteAuthor(int id);
    }
}
