using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.DTOs;
using BookStore_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
	/// <summary>
	/// Interacts with the Books Table
	/// </summary>
	[Route("api/[controller]/")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;

		public BooksController(IBookRepository bookRepository,
			IMapper mapper)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get All Books
		/// </summary>
		/// <returns>A list of books</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetBooks()
		{
			try
			{
				var books = await _bookRepository.FindAll();
				return Ok(books);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Gets a Book by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>A book record</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetBook(int id)
		{
			try
			{
				var book = await _bookRepository.FindById(id);
				if (book == null)
				{
					return NotFound();
				}
				return Ok(book);
			}

			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
		
		/// <summary>
		/// Create a Book
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = "Librarian")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] BookDto bookDto)
		{
			try
			{
				if (bookDto == null)
				{
					return BadRequest(ModelState);
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var book = _mapper.Map<Book>(bookDto);
				var isSuccess = await _bookRepository.Create(book);
				if (!isSuccess)
				{
					return StatusCode(500);
				}
				return Created("Create", new { book });
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Update an Book
		/// </summary>
		/// <param name="id"></param>
		/// <param name="book"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update(int id, [FromBody] BookDto bookDto)
		{
			try
			{
				if (id < 1 || bookDto == null)
				{
					return BadRequest();
				}
				var isExists = await _bookRepository.isExists(id);
				if (!isExists)
				{
					return NotFound();
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var book = _mapper.Map<Book>(bookDto);
				book.Id = id;
				var isSuccess = await _bookRepository.Update(book);
				if (!isSuccess)
				{
					return StatusCode(500);
				}
				return NoContent();
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Delete a Book
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[Authorize(Roles = "Librarian")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				if (id < 1)
				{
					return BadRequest();
				}
				var isExists = await _bookRepository.isExists(id);
				if (!isExists)
				{
					return NotFound();
				}
				var book = await _bookRepository.FindById(id);
				var isSuccess = await _bookRepository.Delete(book);
				if (!isSuccess)
				{
					return StatusCode(500);
				}
				return NoContent();
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
