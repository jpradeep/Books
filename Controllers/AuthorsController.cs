using Books.Models;
using Books.Models.Dto;
using Books.Repository.Interface;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authRepo;
        public AuthorsController(IAuthorRepository authorRepo)
        {
            _authRepo = authorRepo;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var dbBook = await _authRepo.GetAuthors();
                if (dbBook.Where(x => x.Authorid == id).ToList().Count < 1)
                    return NotFound();
                var result = await _authRepo.DeleteAuthor(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorDto authorDto)
        {
            try
            {
                var dbBook = await _authRepo.GetAuthors();
                if (dbBook.Where(x => x.Authorid == id).ToList().Count < 1)
                    return NotFound();

                var updatedBook = await _authRepo.UpdateAuthor(id, authorDto);
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                var companies = await _authRepo.GetAuthors();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Author> CreateAuthor([FromBody]Author author)
        {
            try
            {
                return await _authRepo.CreateAuthor(author);
            }
            catch (Exception ex)
            {
                //log error
                throw ex;
            }
        }
    }
}
