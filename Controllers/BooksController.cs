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
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        public BooksController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        [HttpGet("GetBookDetails")]
        public async Task<IActionResult> GetBookDetails()
        {
            try
            {
                var bookDetails = await _bookRepo.GetBookDetails();
                return Ok(bookDetails);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteBook(long isbn)
        {
            try
            {
                var dbBook = await _bookRepo.GetBooks();
                if (dbBook.Where(x => x.Isbn == isbn).ToList().Count < 1)
                    return NotFound();
                var result = await _bookRepo.DeleteBook(isbn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{isbn}")]
        public async Task<IActionResult> UpdateBook(long isbn, BookDto book)
        {
            try
            {
                var dbBook = await _bookRepo.GetBooks();
                if (dbBook.Where(x=>x.Isbn == isbn).ToList().Count < 1)
                    return NotFound();

                var updatedBook = await _bookRepo.UpdateBook(isbn, book);
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var companies = await _bookRepo.GetBooks();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Book> CreateBook([FromBody]Book book)
        {
            try
            {
                return await _bookRepo.CreateBook(book);
            }
            catch (Exception ex)
            {
                //log error
                throw ex;
            }
        }
    }
}
