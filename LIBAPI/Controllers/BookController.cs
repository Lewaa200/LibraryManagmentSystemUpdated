using LIBAPI.Services;
using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LIBAPI.DTOs;
using Data.Utilities;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<Book>>> GetBooks(int pageIndex = 1, int pageSize = 10)
        {
            var books = await _bookService.GetBooksAsync(pageIndex, pageSize);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return BadRequest();
            }
            var bookDTO = _mapper.Map<BookDTO>(book);
            return Ok(bookDTO);
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                throw new Exception("Test");
                var book = _mapper.Map<Book>(bookDTO);
                await _bookService.AddBookAsync(book);
                var createdBookDTO = _mapper.Map<BookDTO>(book);
                return CreatedAtAction(nameof(GetBook), new { id = createdBookDTO.ID }, createdBookDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookDTO bookDTO)
        {
            if (id != bookDTO.ID)
            {
                return BadRequest();
            }

            var book = _mapper.Map<Book>(bookDTO);
            await _bookService.UpdateBookAsync(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }


    }
}
