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
	/// Interacts with the Rentals Table
	/// </summary>
	[Route("api/[controller]/")]
	[ApiController]
	public class RentalsController : ControllerBase
	{
		private readonly IBookRepository _bookRepository;
		private readonly IRentalRepository _rentalRepository;
		private readonly IMapper _mapper;

		public RentalsController(IBookRepository bookRepository,
			IRentalRepository rentalRepository,
			IMapper mapper)
		{
			_bookRepository = bookRepository;
			_rentalRepository = rentalRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get All Rentals
		/// </summary>
		/// <returns>A list of book rentals</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetRentals()
		{
			try
			{
				var rentals = await _rentalRepository.FindAll();
				return Ok(rentals);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Gets a Rental by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>A book rental record</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetRental(int id)
		{
			try
			{
				var rental = await _rentalRepository.FindById(id);
				if (rental == null)
				{
					return NotFound();
				}
				return Ok(rental);
			}

			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Create a Rental
		/// </summary>
		/// <param name="Rental"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = "Member")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] RentalCreateDto rentalDto)
		{
			try
			{
				if (rentalDto == null)
				{
					return BadRequest(ModelState);
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var rental = _mapper.Map<Rental>(rentalDto);
				var isSuccess = await _rentalRepository.Create(rental);
				if (!isSuccess)
				{
					return StatusCode(500);
				}
				return Created("Create", new { rental });
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Update an Rental
		/// </summary>
		/// <param name="id"></param>
		/// <param name="rental"></param>
		/// <returns></returns>
		//[HttpPut("Update/{id:int}")]
		[HttpPut("{id}")]
		[Authorize(Roles = "Librarian")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update(int id, [FromBody] RentalUpdateDto rentalDto)
		{
			try
			{
				if (id < 1 || rentalDto == null)
				{
					return BadRequest();
				}
				var isExists = await _rentalRepository.isExists(id);
				if (!isExists)
				{
					return NotFound();
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var rental = _mapper.Map<Rental>(rentalDto);
				rental.Id = id;
				var isSuccess = await _rentalRepository.Update(rental);
				if (!isSuccess)
				{
					return StatusCode(500, "Something went wrong. Please contact the Administrator.");
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
